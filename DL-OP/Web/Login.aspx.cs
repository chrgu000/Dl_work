using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net;
using System.IO;
using System.Text;
using BLL;
using System.Collections;
using ThoughtWorks.QRCode.Codec;
using System.Drawing;
using System.Drawing.Imaging;


public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Request.QueryString["token"] != null)
        {
            //解密
            string aa = dis_Token(Request.QueryString["token"].ToString());
            string[] sArray = aa.Split('|');
            //System.DateTime currentTime = new System.DateTime();
            if (Convert.ToDouble(new Check().ExecDateDiff(Convert.ToDateTime(sArray[3].ToString()), DateTime.Now)) > 30)       //判断时间是否在30秒内有效
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('登录超时！');</script>");
                return;
            }
            //跳转登录
            DataTable dt = new LoginManager().SubLogin(sArray[0].ToString(), sArray[2].ToString(), sArray[1].ToString());
            if (dt.Rows.Count < 1)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('跳转登录失败，请重新登录！');</script>");
                return;
            }
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
            Session["pwd"] = sArray[1].ToString();   //密码Session
            Session["strLoginPhone"] = TxtUserSubPhone.Text.ToString().Trim();  //20170504,登录的电话号码     
            //dt.Dispose();
            #endregion
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
        }



        //tz.Text = ConfigurationManager.AppSettings["tz"].ToString();
        if (Request.QueryString["id"] != null)
        {
            //Warm.Text = "该用户已在其它地点登录!如非您授权操作,请尽快联系统系管理员!";
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('该用户已在其它地点登录!如非您授权操作,请尽快联系统系管理员！');</script>");
        }
        else
        {
            //Warm.Text = "";
        }

        if (!IsPostBack)//第一次打开页面
        {
            Session["Smsdt"] = "";//验证码赋值
            //Timer1.Enabled = false;
            //Timer1.Interval = 1000;
            string ssId = Session.SessionID.ToString();  //获取当前session的id
            HidFSessionID.Value = ssId;
            bool wxlogin = new LoginManager().QrForSessionID(ssId);
        }
        Timer2.Interval = 3000;
        Timer2.Enabled = true;



    }

    #region 系统登录验证
    /// <summary>
    /// 系统登录验证
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void IbtnLogin_Click(object sender, ImageClickEventArgs e)
    {
        //四位编号的员工不需要输入验证码,直接登录
        if (TxtUserName.Text.Length != 4)
        {
            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('" + TxtUserName.Text + "');</script>");//获取文本内容
            //return;
            //验证短信码,测试跳转
            if ("9" == TxtUserName.Text.Trim().Substring(0, 1).ToString() || "9" == TxtUserName.Text.Trim().Substring(0, 1).ToString() || "nocheck" == ConfigurationManager.AppSettings["yzm"].ToString())
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
            //验证短信码,测试跳转
            //if ("*" == TxtAuth.Text.Trim().ToString())
            //{
            //    goto A1;
            //}

            //验证短信码
            if (Session["SmsCode"].ToString() != TxtAuth.Text.Trim().ToString())
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('验证码错误！');</script>");
                return;
            }
        }
    //else
    //{
    //    if ("*" == TxtAuth.Text.Trim().ToString())
    //    {
    //        goto A1;
    //    }
    //    else
    //    {
    //        Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('验证码错误！');</script>");
    //        return;
    //    }
    //}
    A1:
        //登录验证
        string username = TxtUserName.Text.Trim();
        string phoneno = TxtUserSubPhone.Text.Trim();
        string pwd = FormsAuthentication.HashPasswordForStoringInConfigFile(TxtPWD.Text.Trim(), "MD5");// 把密码转为MD5码的形式
        //DataTable dt = new LoginManager().Login(username, pwd);
        DataTable dt = new DataTable();
        if (TxtUserName.Text.Length == 4)
        {
            dt = new LoginManager().Login(username, pwd);
        }
        else
        {
            dt = new LoginManager().SubLogin(username, phoneno, pwd);
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
            //Session["cCusPhoneSingle"] = phoneno;  //顾客电话号码*
            Session["cCusPhone"] = dt.Rows[0]["cCusPhone"].ToString();  //顾客电话号码*
            Session["strLoginName"] = dt.Rows[0]["strLoginName"].ToString();  //登录编码,2016-03-30增加
            Session["lngopUserExId"] = dt.Rows[0]["lngopUserExId"].ToString();  //子账户id,2016-08-26增加
            Session["strAllAcount"] = dt.Rows[0]["strAllAcount"].ToString();  //子账户编码,2016-08-26增加，如无子账户则为主账户编码,不变更的常量       
            Session["strLoginPhone"] = TxtUserSubPhone.Text.ToString().Trim();  //20170504,登录的电话号码     
            //创建一个HASHTABLE
            Hashtable ht = new Hashtable();//创建一个Hashtable实例
            ht.Add("IsExercisePrice", dt.Rows[0]["IsExercisePrice"].ToString());//添加key/键值对
            Session["SysSetting"] = ht;
            //Session["Treeccuscode"] = dt.Rows[0]["cCusCode"].ToString();            //物料树(限销)
            //dt = new SearchManager().T_Tdatewk();
            //Session["titledate"] = dt.Rows[0][0];   //获取日期Session
            //Session["titleweek"] = dt.Rows[0][1];   //获取星期Session
            //Session["username"] = username;     //用户Session
            Session["pwd"] = pwd;   //密码Session
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

                //Response.Write("<script language=\"javascript\">window.open('news.aspx','新窗口,\"toolbar=yes,location=no,directories=yes,status=yes,menubar=yes,resizable=yes,scrollbars=yes\");</script>");
                //Response.Write("<script>window.open('http://www.wenjuan.com/s/qyquUj/','_blank')</script>");
                //Response.Write("<script language='javascript'>window.open('http://www.wenjuan.com/s/qyquUj/');</script>");
                //Response.Write("<script language='javascript'>window.open('http://www.wenjuan.com/s/qyquUj/','','resizable=1,scrollbars=0,status=1,menubar=no,toolbar=no,location=no, menu=no');</script>");
                Response.Redirect("~/Index.aspx");//客户界面
                //Response.Write("<script language='javascript'>window.open('http://www.wenjuan.com/s/qyquUj/');</script>");

            }
            if ("2" == Session["strUserLevel"].ToString())
            {
                Response.Redirect("~/Default.aspx");//操作员界面
            }
            if ("4" == Session["strUserLevel"].ToString())
            {
                Response.Redirect("~/Default4.aspx");//测试账户-查询界面
            }
            if ("1" == Session["strUserLevel"].ToString()) //普通用户界面
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
    #endregion

    public string dis_Token(string token)
    {
        //string token = HttpContext.Current.Request.QueryString["token"];
        //     token = HttpContext.Current.Session["token"].ToString();
        token = token.Replace(" ", "+");
        token = new Check().DecryptDES(token);
        return token;
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
            System.Web.UI.ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "updateScript", "alert('请输入登录名！');", true);
            return telName;
        }
        if ("" == telNo)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "updateScript", "alert('请输入手机号码！');", true);
            return telNo;
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
            //#region 微信企业号发送信息，应用id20，
            //string wxrel = new SendSMS2Customer9001.SendSMS2CustomerSoapClient().SendQY_Message_Text("", "", "11", "20", "【会议演示】尊敬的客户,您于" + DateTime.Now.ToString("G") + "登录网上下单系统！ " + SmsCode + "，十分钟内有效。如非本人操作，请忽略本信息。【多联公司】");
            //#endregion

            //发送
            //ChinaSms sms = new ChinaSms("duolian", "duolian123");
            //bool re = sms.SingleSend(telNo, SmsTxt);
            if (dttel.Rows.Count < 1)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "updateScript", "alert('账号信息或者手机信息错误！');", true);
                return telName;
            }
            SendSMS sms1 = new SendSMS();  //20170427注释，启用9003新短信平台发送短信
            SendSMS2Customer9003.SendSMS2CustomerSoapClient sms = new SendSMS2Customer9003.SendSMS2CustomerSoapClient();
            int ResultOk = 0; //  记录结果
            int ResultNo = 0; //  记录结果
            int WXResultOk = 0; //  记录结果
            int WXResultNo = 0; //  记录结果
            string Result = "";
            bool re;
            bool wx = false; ;
            for (int i = 0; i < dttel.Rows.Count; i++)
            {
                if (RadioButton1.Checked == true)   //短信通道1,新平台
                {
                    re = sms.SendSMS(dttel.Rows[i]["PhoneNo"].ToString(), SmsTxt.ToString());
                    
                }
                else   ////短信通道2,老平台
                {
                    re = sms1.SingleSend(dttel.Rows[i]["PhoneNo"].ToString(), SmsTxt.ToString());     //20170427注释，启用9003新短信平台发送短信
                }
                if (re == false)
                {
                    ResultNo = ResultNo + 1;
                    Result = Result + dttel.Rows[i]["PhoneNo"].ToString() + ";";
                }
                else
                {
                    ResultOk = ResultOk + 1;
                }
                if (CheckBox1.Checked==true)
                {
                    wx = sms.SendQY_Message_Text(dttel.Rows[i]["PhoneNo"].ToString(), "", "", "20", SmsTxt.ToString());                
                }
               
                if (wx == false)
                {
                    WXResultNo = WXResultNo + 1;
                    //Result = Result + dttel.Rows[i]["PhoneNo"].ToString() + ";";
                }
                else
                {
                    WXResultOk = WXResultOk + 1;
                }
            }
            //System.Web.UI.ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "updateScript", "alert('短信发送成功,请注意查收.如长时间未收到请联系管理员!成功发送" + ResultOk + "条短信,发送失败" + ResultNo + "条短信!);", true);
            System.Web.UI.ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "updateScript", "alert('短信发送成功,请注意查收.如长时间未收到请联系管理员!成功发送" + ResultOk + "条短信," + WXResultOk + "条微信.发送失败" + ResultNo + "条短信," + WXResultNo + "条微信!');", true);
            return telName;
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

            if (sMsg.Substring(0, 5) != "num=0" && sMsg.Length > 1)
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

    protected void btn_Click(object sender, EventArgs e)
    {
        SendSMS(TxtUserName.Text.ToString().Trim(), TxtUserSubPhone.Text.ToString().Trim());
        Timer1.Interval = 1000;
        Label1.Text = "90";
        btn.Text = "重新发送";
        Timer1.Enabled = true;
        btn.Enabled = false;

    }
    protected void Timer1_Tick(object sender, EventArgs e)
    {
        Label1.Text = (int.Parse(Label1.Text) - 1).ToString();
        if (Label1.Text == "0")
        {
            btn.Enabled = true;
            Timer1.Enabled = false;
            Label1.Text = "";
        }
    }
    protected void Timer2_Tick(object sender, EventArgs e)
    {
        DataTable dt = new BasicInfoManager().DL_QrCodeLoginBySel(HidFSessionID.Value.ToString());
        if (dt.Rows.Count > 0)
        {

            //登录成功后
            bool c = new BasicInfoManager().Update_QrCodeLogin(HidFSessionID.Value.ToString(), "2", dt.Rows[0]["userid"].ToString());
            //登录取数据
            dt = new LoginManager().Login_wx(dt.Rows[0]["userid"].ToString());
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
            //Session["cCusPhoneSingle"] = phoneno;  //顾客电话号码*
            Session["cCusPhone"] = dt.Rows[0]["cCusPhone"].ToString();  //顾客电话号码*
            Session["strLoginName"] = dt.Rows[0]["strLoginName"].ToString();  //登录编码,2016-03-30增加
            Session["lngopUserExId"] = dt.Rows[0]["lngopUserExId"].ToString();  //子账户id,2016-08-26增加
            Session["strAllAcount"] = dt.Rows[0]["strAllAcount"].ToString();  //子账户编码,2016-08-26增加，如无子账户则为主账户编码,不变更的常量       
            Session["strLoginPhone"] = TxtUserSubPhone.Text.ToString().Trim();  //20170504,登录的电话号码     
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
            //Response.Redirect("/other/RedirectWXScanLogin.aspx?sessionid="+HidFSessionID.Value.ToString());
            Response.Redirect("~/Default.aspx");//操作员界面
        }

    }
    protected void QRRefresh_Click(object sender, EventArgs e)
    {

    }

}