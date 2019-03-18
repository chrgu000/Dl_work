using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Collections;
using System.Web.Security;
using System.Data;
using System.Web.Services;
using Model;


namespace DingDan_WebForm
{
    public partial class Login_V2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {





            if (!IsPostBack)//第一次打开页面
            {
                if (!string.IsNullOrEmpty(Request.QueryString["token"]))
                {
                    string str_token = Request.QueryString["token"];
                    ReInfo ri = new ReInfo();
                    string token = str_token.Replace(" ", "+");
                    token = new Check().dis_Token();
                    string[] token_arr = token.Split('|');
                    DateTime now = DateTime.Now;
                    DateTime token_time = Convert.ToDateTime(token_arr[3]);
                    TimeSpan ts = now.Subtract(token_time);
                    if (ts.TotalSeconds>30)
                    {
                        Response.Write("<script>alert('跳转失败,请重新登录!')</script>");
                        
                        return;
                    }
                    string ccuscode = token_arr[0];
                    string pwd = token_arr[1];
                    string phone = token_arr[2];
                    ri = new Check().Direct_Login(ccuscode, phone,pwd);
                    if (ri.flag == "1")
                    {
                        Response.Redirect("index_v2.aspx");
                    }
                    else
                    {
                        Response.Redirect("login_v2.aspx");
                    }
 
                    Response.Write("<br />");
                
                    Response.End();
                }
            }

        }


        protected void CheckLogin(object sender, EventArgs e)
        {
            string UserName = Request.Form["name"].Trim();
            string Password = Request.Form["pass"];
            string Phone = Request.Form["phone"];
            string Phone_Check = Request.Form["phone_code"].Trim();
            //四位编号的员工不需要输入验证码,直接登录
            if (UserName.Length != 4)
            {
                //验证短信码,测试跳转
                if ("9" == UserName.Substring(0, 1).ToString())
                {
                    goto A1;
                }
                //验证短信有效性（时间）
                if (Session["Smsdt"].ToString() != "")//如果已经获取验证码
                {
                    string dtNow = DateTime.Now.ToString("G");
                    double SmsTime = (DateTime.Parse(dtNow) - DateTime.Parse(Session["Smsdt"].ToString())).TotalMinutes;
                    if (SmsTime > 10) //验证码超过10分钟有效时间，提示重新获取
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('验证码过期，请重新获取！');</script>");
                        return;
                    }
                }
                else //未获取验证码，提示
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('请先获取验证码！');</script>");
                    return;
                }

                //验证短信码
                if (Session["SmsCode"].ToString() != Phone_Check)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('验证码错误！');</script>");
                    return;
                }
            }
        A1:
            //登录验证

            string pwd = FormsAuthentication.HashPasswordForStoringInConfigFile(Password, "MD5");// 把密码转为MD5码的形式
            //DataTable dt = new LoginManager().Login(username, pwd);
            DataTable dt = new DataTable();
            if (UserName.Length == 4)
            {
                dt = new LoginManager().Login(UserName, pwd);
            }
            else
            {
                dt = new LoginManager().SubLogin(UserName, Phone, pwd);
            }

            bool b = false;
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["strStatus"].ToString() == "0")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('该帐号已被禁用！');</script>");
                    return;
                }
                b = true;
                #region session赋值
                Session["strUserLevel"] = dt.Rows[0]["strUserLevel"].ToString();       //权限Session
                Session["lngopUserId"] = dt.Rows[0]["lngopUserId"].ToString();         //登录用户id
                Session["strUserName"] = dt.Rows[0]["strUserName"].ToString();         //用户名称
                Session["KPDWcCusCode"] = dt.Rows[0]["cCusCode"].ToString();           //开票单位编码
                Session["cCusCode"] = dt.Rows[0]["cCusCode"].ToString();               //用户编码
                Session["login"] = "1";                                     //
                Session["ConstcCusCode"] = dt.Rows[0]["cCusCode"].ToString();          //常量,登录用户编码,不变更
                Session["cSTCode"] = "0";                                              //销售类型:默认,普通销售 
                Session["cCusPPerson"] = dt.Rows[0]["cCusPPerson"].ToString();  //业务员
                Session["cCusPhone"] = dt.Rows[0]["cCusPhone"].ToString();  //顾客电话号码*
                Session["strLoginName"] = dt.Rows[0]["strLoginName"].ToString();  //登录编码,2016-03-30增加
                Session["lngopUserExId"] = dt.Rows[0]["lngopUserExId"].ToString();  //子账户id,2016-08-26增加
                Session["strAllAcount"] = dt.Rows[0]["strAllAcount"].ToString();  //子账户编码,2016-08-26增加，如无子账户则为主账户编码,不变更的常量          	
                //创建一个HASHTABLE
                Hashtable ht = new Hashtable();//创建一个Hashtable实例
                ht.Add("IsExercisePrice", dt.Rows[0]["IsExercisePrice"].ToString());//添加key/键值对
                Session["SysSetting"] = ht;
                //Session["Treeccuscode"] = dt.Rows[0]["cCusCode"].ToString();            //物料树(限销)
                //dt = new SearchManager().T_Tdatewk();
                //Session["titledate"] = dt.Rows[0][0];   //获取日期Session
                //Session["titleweek"] = dt.Rows[0][1];   //获取星期Session
                //Session["username"] = username;     //用户Session
                //Session["pwd"] = pwd;   //密码Session
                //dt.Dispose();
                #endregion
            }

            if (b)
            {
                //登录成功,3,客户登录,跳转
                if ("3" == Session["strUserLevel"].ToString())
                {
                    //写入Application中
                    if (Application["Online"] != null)
                    {
                        Hashtable hOnline = (Hashtable)Application["Online"];
                        //删除当前顾客的记录
                        if (hOnline.Contains(Session["strAllAcount"].ToString()))  //判断哈希表是否包含特定键,其返回值为true或false
                        {
                            hOnline.Remove(Session["strAllAcount"].ToString());//移除一个key/value键值对
                        }
                        string sId = Session.SessionID.ToString();  //获取当前session的id
                        hOnline.Add(Session["strAllAcount"].ToString(), sId);//添加key/value键值对
                        Application["Online"] = hOnline;
                    }
                    else
                    {
                        Hashtable hOnline = new Hashtable();
                        string sId = Session.SessionID.ToString();  //获取当前session的id
                        hOnline.Add(Session["strAllAcount"].ToString(), sId);//添加key/value键值对
                        Application["Online"] = hOnline;
                    }

                    Response.Redirect("~/Index.aspx");//客户界面

                }
                if ("2" == Session["strUserLevel"].ToString())
                {
                    Response.Redirect("~/Default.aspx");//操作员界面
                }
                if ("4" == Session["strUserLevel"].ToString())
                {
                    Response.Redirect("~/Default4.aspx");//测试账户-查询界面
                }
                if ("1" == Session["strUserLevel"].ToString())
                {
                    Response.Redirect("~/Default.aspx");// 
                }
            }
            else
            {
                //登录失败
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('登录失败，用户名或密码错误！');</script>");
                return;
                //TxtUserName.Text = "请输入用户名";
                //TxtAuth.Text = "请输入密码";
            }
        }



        public void Get_Code(object sender, EventArgs e)
        {
            string Username = Request.Form["name"].Trim();
            string Phone = Request.Form["phone"].Trim();
            ChinaSms sms = new ChinaSms("duolian", "duolian123");
            //生成验证码
            System.Random Random = new System.Random();
            string SmsCode = Random.Next(1000, 9999).ToString();
            Session["SmsCode"] = SmsCode;
            string SmsTxt = "尊敬的客户,您于" + DateTime.Now.ToString("G") + "登录网上下单系统！ " + SmsCode + "，十分钟内有效。如非本人操作，请忽略本短信。";

            bool re = sms.SingleSend(Phone, SmsTxt);
            SendSMS(Username, Phone);


            #region 微信企业号发送信息，应用id20，
         //   string wxrel = new SMSSend9003.SendSMS2CustomerSoapClient().SendQY_Message_Text("", "", "11", "20", "尊敬的客户,您于" + DateTime.Now.ToString("G") + "登录网上下单系统！ " + SmsCode + "，十分钟内有效。如非本人操作，请忽略本信息。");
            #endregion

        }

        #region 通过js调用cs，发送短信验证码-按钮
        /// <summary>
        /// 通过js调用cs，发送短信验证码-按钮
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public string SendSMS(string telName, string telNo)
        {
            //判断是否输入用户名
            if ("" == telName)
            {
                Response.Write("<script>alert('请输入登录名!')</script>");
                return "";
            }
            if ("" == telNo)
            {
                Response.Write("<script>alert('请输入手机号码!')</script>");
                return "";
            }
            else
            {
                // 查询数据库中是否存在该用户，如果有则发送短信，如果没有则提示
                //return telNo + "no";
                //DataTable dt = new LoginManager().LoginSms(telName);
                //if (dt.Rows.Count != 1)
                //{
                //    System.Web.UI.ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "updateScript", "alert('未记录该信息,请联系管理员！');", true);
                //    return telName;
                //}
                //string telNo = dt.Rows[0]["cCusPhone"].ToString();
                //DataTable dttel = new GetPhoneNo().GetSubCustomerPhoneNo(telName, telNo);
                DataTable dttel = new LoginManager().GetSubPhoneNo(telName, telNo);
                //发送短信
                #region 商讯·中国 发送短信
                //生成验证码
                System.Random Random = new System.Random();
                string SmsCode = Random.Next(1000, 9999).ToString();
                Session["SmsCode"] = SmsCode;
                Session["Smsdt"] = DateTime.Now.ToString("G");
                string SmsTxt = "尊敬的客户,您于" + DateTime.Now.ToString("G") + "登录网上下单系统！ " + SmsCode + "，十分钟内有效。如非本人操作，请忽略本短信。";

                //发送
                //ChinaSms sms = new ChinaSms("duolian", "duolian123");
                //bool re = sms.SingleSend(telNo, SmsTxt);
                if (dttel.Rows.Count < 1)
                {
                    Response.Write("<script>alert('账号信息或者手机信息错误!')</script>");
                    return "";
                }
                //SendSMS sms = new SendSMS();
                //int ResultOk = 0; //  记录结果
                //int ResultNo = 0; //  记录结果
                //string Result = "";
                //for (int i = 0; i < dttel.Rows.Count; i++)
                //{
                //    bool re = sms.SingleSend(dttel.Rows[i]["PhoneNo"].ToString(), SmsTxt.ToString());
                //    if (re == false)
                //    {
                //        ResultNo = ResultNo + 1;
                //        Result = Result + dttel.Rows[i]["PhoneNo"].ToString() + ";";
                //    }
                //    else
                //    {
                //        ResultOk = ResultOk + 1;
                //    }
                //}

                Response.Write("<script>alert('短信发送成功!')</script>");
                return "";
                //if (true == re)
                //{
                //    System.Web.UI.ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "updateScript", "alert('短信发送成功，请注意查收。如长时间未收到请联系管理员！');", true);
                //    return telName;
                //}
                //else
                //{
                //    System.Web.UI.ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "updateScript", "alert('短信发送失败，请联系管理员！');", true);
                //    return telName;
                //}
                #endregion
            }
            //return "短信已发送，请注意查收！";
        }
        #endregion

        #region 简单封装中国短信接口规范v1.2
        /*
   类名：ChinaSms
   说明：简单封装中国短信接口规范v1.2
   更新历史：
   */
        public class ChinaSms
        {
            private string comName;
            private string comPwd;

            public ChinaSms()
            {
            }

            public ChinaSms(String name, String pwd)
            {
                this.comName = name;
                this.comPwd = pwd;
            }

            /*
            说明:封装单发接口
            参数:
              dst:目标手机号码
              msg:发送短信内容
            返回值:
              true:发送成功;
              false:发送失败
            */
            public bool SingleSend(String dst, String msg)
            {
                string sUrl = null;  //接口规范中的地址
                string sMsg = null;  //调用结果
                msg = System.Web.HttpUtility.UrlEncode(msg, System.Text.Encoding.GetEncoding("gb2312"));
                comName = System.Web.HttpUtility.UrlEncode(comName, System.Text.Encoding.GetEncoding("gb2312"));
                //备用IP地址为203.81.21.13
                sUrl = "http:" + "//203.81.21.34//send/gsend.asp?name=" + comName + "&pwd=" + comPwd + "&dst=" + dst + "&msg=" + msg;
                sMsg = GetUrl(sUrl);
                //Console.WriteLine(sMsg);

                if (sMsg.Substring(0, 5) != "num=0")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            /*通用调用接口*/
            public String GetUrl(String urlString)
            {
                string sMsg = "";		//引用的返回字符串
                try
                {
                    System.Net.HttpWebResponse rs = (System.Net.HttpWebResponse)System.Net.HttpWebRequest.Create(urlString).GetResponse();
                    System.IO.StreamReader sr = new System.IO.StreamReader(rs.GetResponseStream(), System.Text.Encoding.Default);
                    sMsg = sr.ReadToEnd();
                }
                catch
                {
                    return sMsg;
                }
                return sMsg;
            }
        }
        #endregion
    }
}