using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data;
using BLL;
using System.Web.Security;
using System.Collections;
using System.ServiceModel.Diagnostics.Application;
using System.Web.ApplicationServices;
using Newtonsoft.Json;
using Model;
using System.Configuration;
using Newtonsoft.Json.Linq;

namespace DingDan_WebForm.Handler
{
    /// <summary>
    /// LoginHandler 的摘要说明
    /// </summary>
    public class LoginHandler : IHttpHandler, IRequiresSessionState
    {
      public  WeiXin9003.WeiXinSoapClient weixin9003 = new WeiXin9003.WeiXinSoapClient();

        public void ProcessRequest(HttpContext context)
        {
            //  context.Response.ContentType = "text/plain";

            switch (context.Request.Form["Action"])
            {
                case "Logout":
                    context.Session.Abandon();
                    context.Session.Clear();
                    context.Response.Write("ok");
                    break;
                case "Get_Code":
                    context.Response.Write(Get_Code());
                    break;
                case "Login":

                    context.Response.Write(Login());
                    break;
                case "Unlock":
                    if (Unlock())
                    {
                        context.Response.Write("ok");
                    }
                    else
                    {
                        context.Response.Write("error");
                    }
                    break;
                case "ChangePwd":
                    context.Response.Write(JsonConvert.SerializeObject(ChangePwd()));
                    break;

                case "Get_Token":
                    context.Response.Write(JsonConvert.SerializeObject(Get_Token()));
                    break;

                case "dis_Token":
                    context.Response.Write(JsonConvert.SerializeObject(dis_Token()));
                    break;
                case "send_wx":
                    context.Response.Write(send_wx());
                    break;
                case "href_old":
                    context.Response.Write(href_old());
                    break;
                case "GetConnInfo":
                    context.Response.Write(JsonConvert.SerializeObject(GetConnInfo()));
                    break;
            }
        }

        #region 获取验证码
        public string Get_Code()
        {
            string name = HttpContext.Current.Request.Form["name"];
            string phone = HttpContext.Current.Request.Form["phone"];

            if (string.IsNullOrEmpty(name))
            {
                return "请输入用户名";
            }
            else if (string.IsNullOrEmpty(phone))
            {
                return "请输入正确的电话号码";
            }
            else
            {
                DataTable dt = new LoginManager().GetSubPhoneNo(name, phone);
                if (dt.Rows.Count < 1)
                {
                    return "账号信息或者手机信息错误！";
                }
                else
                {
                    System.Random Random = new System.Random();
                    string SmsCode = Random.Next(1000, 9999).ToString();
                    HttpContext.Current.Session["SmsCode"] = SmsCode;
                    HttpContext.Current.Session["Smsdt"] = DateTime.Now.ToString("G");
                    string SmsTxt = "尊敬的客户,您于" + DateTime.Now.ToString("G") + "登录网上下单系统！验证码为: " + SmsCode + "，十分钟内有效。如非本人操作，请忽略本短信。";
                    //    bool b = new SendSMS2Customer9001.SendSMS2CustomerSoapClient().SendSMS(phone, SmsTxt);
                    bool b = false;
                    if (HttpContext.Current.Request.Form["sms"] == "1")
                    {
                        b = new SMSSend9003.SendSMS2CustomerSoapClient().SendSMS(phone, SmsTxt);
                    }
                    else if (HttpContext.Current.Request.Form["sms"] == "2")
                    {
                        b = new SMSSend9001.SendSMS2CustomerSoapClient().SendSMS(phone, SmsTxt);
                    }
                    if (HttpContext.Current.Request.Form["wx"] == "1")
                    {
                     //   bool wxrel = new SMSSend9003.SendSMS2CustomerSoapClient().SendQY_Message_Text(phone, "", "", "20", "尊敬的客户,您于" + DateTime.Now.ToString("G") + "登录网上下单系统！验证码为: " + SmsCode + "，十分钟内有效。如非本人操作，请忽略本信息。");
                        string appid = ConfigurationManager.AppSettings["WeiXinAPPID"];
                        string res = weixin9003.SendMsg_Text(phone, "", "", appid, "尊敬的客户,您于" + DateTime.Now.ToString("G") + "登录网上下单系统！验证码为: " + SmsCode + "，十分钟内有效。如非本人操作，请忽略本信息。");
                      
                    }
                    if (b)
                    {
                        return "ok";
                    }
                    else
                    {
                        return "短信发送失败";
                    }

                }
            }
        }
        #endregion

        #region 判断登录
        public string  Login()
        {
            string UserName = HttpContext.Current.Request.Form["name"].Trim();
            string Password = HttpContext.Current.Request.Form["pass"];
            string Phone = HttpContext.Current.Request.Form["phone"];
            string Phone_Check = HttpContext.Current.Request.Form["code"].Trim();
            //四位编号的员工不需要输入验证码,直接登录
            string isCode = ConfigurationManager.AppSettings.Get("isCode");
            if (isCode == "0")
            {
                goto A1;
            }
            if (UserName.Length != 4)
            {
                //验证短信码,测试跳转
                //if ("9" == UserName.Substring(0, 1).ToString() || "9" != UserName.Substring(0, 1).ToString())
                if ("9" == UserName.Substring(0, 1).ToString())
                {
                    goto A1;
                }
                //验证短信有效性（时间）

                if (HttpContext.Current.Session["smsdt"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["smsdt"].ToString()))//如果已经获取验证码
                {
                    string dtNow = DateTime.Now.ToString("G");
                    double SmsTime = (DateTime.Parse(dtNow) - DateTime.Parse(HttpContext.Current.Session["Smsdt"].ToString())).TotalMinutes;
                    if (SmsTime > 10) //验证码超过10分钟有效时间，提示重新获取
                    {
                        return "验证码过期，请重新获取！";
                    }
                }
                else //未获取验证码，提示
                {
                    return "请先获取验证码!";
                }

                //验证短信码
                if (HttpContext.Current.Session["SmsCode"].ToString() != Phone_Check)
                {
                    return "验证码错误";
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
                    return "该帐号已被禁用!";
                }
                b = true;
                HttpContext.Current.Session["login"] = "1";   
                if (dt.Rows[0]["strUserLevel"].ToString()=="3")
                {
                    #region 客户Session赋值
                    HttpContext.Current.Session["pwd"] = pwd;
                    HttpContext.Current.Session["strLoginPhone"] = Phone;
                    HttpContext.Current.Session["strUserLevel"] = dt.Rows[0]["strUserLevel"].ToString();       //权限HttpContext.Current.Session
                    HttpContext.Current.Session["lngopUserId"] = dt.Rows[0]["lngopUserId"].ToString();         //登录用户id
                    HttpContext.Current.Session["strUserName"] = dt.Rows[0]["strUserName"].ToString();         //用户名称
                    HttpContext.Current.Session["KPDWcCusCode"] = dt.Rows[0]["cCusCode"].ToString();           //开票单位编码
                    HttpContext.Current.Session["cCusCode"] = dt.Rows[0]["cCusCode"].ToString();               //用户编码
                    HttpContext.Current.Session["ConstcCusCode"] = dt.Rows[0]["cCusCode"].ToString();          //常量,登录用户编码,不变更
                    HttpContext.Current.Session["cSTCode"] = "0";                                              //销售类型:默认,普通销售 
                    HttpContext.Current.Session["cCusPPerson"] = dt.Rows[0]["cCusPPerson"].ToString();  //业务员
                    HttpContext.Current.Session["cCusPhone"] = dt.Rows[0]["cCusPhone"].ToString();  //顾客电话号码*
                    HttpContext.Current.Session["strLoginName"] = dt.Rows[0]["strLoginName"].ToString();  //登录编码,2016-03-30增加
                    HttpContext.Current.Session["lngopUserExId"] = dt.Rows[0]["lngopUserExId"].ToString();  //子账户id,2016-08-26增加
                    HttpContext.Current.Session["strAllAcount"] = dt.Rows[0]["strAllAcount"].ToString();  //子账户编码,2016-08-26增加，如无子账户则为主账户编码,不变更的常量          	
                   
                    return HttpContext.Current.Session["strUserLevel"].ToString();
                    #endregion
                }
                else
                {
                    #region 后台用户Session赋值
                    HttpContext.Current.Session["AdminstrLoginPhone"] = Phone;
                    HttpContext.Current.Session["AdminstrUserLevel"] = dt.Rows[0]["strUserLevel"].ToString();       //权限HttpContext.Current.Session
                    HttpContext.Current.Session["AdminlngopUserId"] = dt.Rows[0]["lngopUserId"].ToString();         //登录用户id
                    HttpContext.Current.Session["AdminstrUserName"] = dt.Rows[0]["strUserName"].ToString();         //用户名称
                    HttpContext.Current.Session["AdmincCusCode"] = dt.Rows[0]["cCusCode"].ToString();               //用户编码
                    HttpContext.Current.Session["AdminstrLoginName"] = dt.Rows[0]["strLoginName"].ToString();  //登录编码,2016-03-30增加
                    HttpContext.Current.Session["AdminstrAllAcount"] = dt.Rows[0]["strAllAcount"].ToString();  //子账户编码,2016-08-26增加，如无子账户则为主账户编码,不变更的常量          	
                    return HttpContext.Current.Session["AdminstrUserLevel"].ToString();
                    
                    #endregion
                }
          
            }

            if (b)
            {
                //if (HttpContext.Current.Application["StartCheckOnline"] == null)
                //{
                //    HttpContext.Current.Application["StartCheckOnline"] = true;
                //    new SignalR.SignalRTask().StartCheck();

                //}

                //登录成功,3,客户登录,跳转
                //if ("3" == HttpContext.Current.Session["strUserLevel"].ToString())
                //{

                //    //写入HttpContext.Current.Application中
                //    if (HttpContext.Current.Application["Online"] != null)
                //    {
                //        Hashtable hOnline = (Hashtable)HttpContext.Current.Application["Online"];
                //        //删除当前顾客的记录
                //        if (hOnline.Contains(HttpContext.Current.Session["strAllAcount"].ToString()))  //判断哈希表是否包含特定键,其返回值为true或false
                //        {
                //            hOnline.Remove(HttpContext.Current.Session["strAllAcount"].ToString());//移除一个key/value键值对
                //        }
                //        string sId = HttpContext.Current.Session.SessionID.ToString();  //获取当前HttpContext.Current.Session的id
                //        hOnline.Add(HttpContext.Current.Session["strAllAcount"].ToString(), sId);//添加key/value键值对
                //        HttpContext.Current.Application["Online"] = hOnline;
                //    }
                //    else
                //    {
                //        Hashtable hOnline = new Hashtable();
                //        string sId = HttpContext.Current.Session.SessionID.ToString();  //获取当前HttpContext.Current.Session的id
                //        hOnline.Add(HttpContext.Current.Session["strAllAcount"].ToString(), sId);//添加key/value键值对
                //        HttpContext.Current.Application["Online"] = hOnline;
                //    }




                //    //  HttpContext.Current.Response.Redirect("/Index_V2.aspx");
                //    // HttpContext.Current.Response.Write("location.href='Index_V2.aspx'");
                //    //  HttpContext.Current.Response.Redirect("~/Index.aspx");//客户界面

                //}
                //if ("2" == HttpContext.Current.Session["strUserLevel"].ToString())
                //{
                //    HttpContext.Current.Response.Redirect("/Admin_V2.aspx");//操作员界面
                //}
                //if ("4" == HttpContext.Current.Session["strUserLevel"].ToString())
                //{
                //    HttpContext.Current.Response.Redirect("~/Default4.aspx");//测试账户-查询界面
                //}
                //if ("1" == HttpContext.Current.Session["strUserLevel"].ToString())
                //{
                //    HttpContext.Current.Response.Redirect("~/Default.aspx");// 
                //}
            }
            else
            {
                //登录失败
                return "登录失败，请核对用户名、密码或电话号码是否正确！";
                //TxtUserName.Text = "请输入用户名";
                //TxtAuth.Text = "请输入密码";
            }
            return "";
        }
        #endregion

        #region 锁屏解锁
        public bool Unlock()
        {
            string name = HttpContext.Current.Session["ConstcCusCode"].ToString();
            string Pass = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(HttpContext.Current.Request.Form["pass"], "MD5");
            DataTable dt = new LoginManager().Login(name, Pass);
            if (dt.Rows.Count < 1)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        #endregion

        #region 修改密码
        public ReInfo ChangePwd()
        {
            ReInfo ri = new ReInfo();
            string pwd_ini = HttpContext.Current.Request.Form["pwd_ini"];  //初始密码
            string pwd_fir = HttpContext.Current.Request.Form["pwd_fir"];  //第一次密码
            string pwd_sen = HttpContext.Current.Request.Form["pwd_sen"];  //第二次密码

            if (string.IsNullOrEmpty(pwd_ini) || string.IsNullOrEmpty(pwd_fir) || string.IsNullOrEmpty(pwd_sen))
            {
                ri.flag = "0";
                ri.message = "信息输入不完整,请重新输入!";
                return ri;
            }

            if (pwd_fir != pwd_sen)
            {
                ri.flag = "0";
                ri.message = "两次密码输入不一致,请重新输入!";
                return ri;
            }

            //string name = HttpContext.Current.Session["ConstcCusCode"].ToString();
            //DataTable dt = new LoginManager().Login(name, FormsAuthentication.HashPasswordForStoringInConfigFile(pwd_ini, "MD5"));
            //if (dt.Rows.Count < 1)
            //{
            //    ri.flag = "0";
            //    ri.message = "初始密码不正确,请核实后重新输入!";
            //    return ri;
            //}

            //修改密码

            string pwd = FormsAuthentication.HashPasswordForStoringInConfigFile(pwd_fir, "MD5");// 把密码转为MD5码的形式
            bool b;

           
            if (HttpContext.Current.Session["AdminlngopUserId"] != null)
            {
                b = new product().Check_pwd(HttpContext.Current.Session["AdminlngopUserId"].ToString(), FormsAuthentication.HashPasswordForStoringInConfigFile(pwd_ini, "MD5"));
                if (!b)
                {
                    ri.flag = "0";
                    ri.message = "初始密码不正确,请核实后重新输入!";
                    return ri;
                }
                b = new product().Update_UserPWD(HttpContext.Current.Session["AdminlngopUserId"].ToString(), pwd);
            }
            else {
             
                if (HttpContext.Current.Session["lngopUserExId"].ToString() != "0")
                {
                    b = new product().Check_pwd_sub(HttpContext.Current.Session["lngopUserExId"].ToString(), FormsAuthentication.HashPasswordForStoringInConfigFile(pwd_ini, "MD5"));
                    if (!b)
                    {
                        ri.flag = "0";
                        ri.message = "初始密码不正确,请核实后重新输入!";
                        return ri;
                    }
                    b = new product().Update_SubUserPWD(HttpContext.Current.Session["lngopUserExId"].ToString(), pwd);
                }
                else
                {
                    b = new product().Check_pwd(HttpContext.Current.Session["lngopUserId"].ToString(), FormsAuthentication.HashPasswordForStoringInConfigFile(pwd_ini, "MD5"));
                    if (!b)
                    {
                        ri.flag = "0";
                        ri.message = "初始密码不正确,请核实后重新输入!";
                        return ri;
                    }
                    b = new product().Update_UserPWD(HttpContext.Current.Session["lngopUserId"].ToString(), pwd);
                }

                if (!b)
                {
                    ri.flag = "0";
                    ri.message = "密码失败,请重试或联系管理员!";
                    return ri;
                }
            }
     

            ri.flag = "1";
            ri.message = "密码修改成功,请牢记新密码!";
            return ri;

        }
        #endregion

        #region 获取Token
        public ReInfo Get_Token()
        {
            string token = new Check().Get_Token();
            ReInfo ri = new ReInfo();
            ri.message = token;
            //  HttpContext.Current.Response.Write(JsonConvert.SerializeObject(token));
            return ri;
        }
        #endregion

        #region 跳转旧版
        public string href_old()
        {
            string str_token = HttpContext.Current.Request.QueryString["token"];
            ReInfo ri = new ReInfo();
            string token = str_token.Replace(" ", "+");
            token = new Check().dis_Token();
            string[] token_arr = token.Split('|');
            DateTime now = DateTime.Now;
            DateTime token_time = Convert.ToDateTime(token_arr[3]);
            TimeSpan ts = now.Subtract(token_time);
            if (ts.TotalSeconds > 30)
            {
                HttpContext.Current.Response.Write("<script>alert('跳转失败,请重新登录!')</script>");
                ri.flag = "0";
                ri.message = "跳转失败,请重新登录!";
                return JsonConvert.SerializeObject(ri);
            }
            string ccuscode = token_arr[0];
            string pwd = token_arr[1];
            string phone = token_arr[2];
            ri = new Check().Direct_Login(ccuscode, phone, pwd);
            if (ri.flag == "1")
            {
                // HttpContext.Current.Response.Redirect("index_v2.aspx");
                return JsonConvert.SerializeObject(ri);
            }
            else
            {
                // HttpContext.Current.Response.Redirect("login_v2.aspx");
                ri.flag = "0";
                ri.message = "跳转失败,请重新登录!";
                return JsonConvert.SerializeObject(ri);
            }


        }
        #endregion

        public string dis_Token()
        {
            string token = HttpContext.Current.Request.Form["token"];
            //  token = HttpContext.Current.Session["token"].ToString();
            token = new Check().DecryptDES(token);
            return token;
        }

        #region 发送微信测试信息
        public bool send_wx()
        {
            SMSSend9003.SendSMS2CustomerSoapClient s = new SMSSend9003.SendSMS2CustomerSoapClient();
            string phone=string.Empty;
            phone = HttpContext.Current.Request.Form["phone"];
            if (phone == "25434523645645")
            {
                phone = HttpContext.Current.Session["strLoginPhone"].ToString();
            }
         //   bool b = s.SendQY_Message_Text(phone, "", "", "20", "能接收到此条信息，表明您已正确关注多联公司微信企业号。(发送时间：" + DateTime.Now.ToString() + ")");
            string res = weixin9003.SendMsg_Text(phone, "", "", "20", "能接收到此条信息，表明您已正确关注多联公司微信企业号。(发送时间：" + DateTime.Now.ToString() + ")");
            JObject jo = new JObject();
            jo = JObject.Parse(res);
            bool b = false;
            if (jo["errcode"].ToString()=="0"&&jo["errmsg"].ToString()=="ok")
            {
                b = true;
            }
            return b;
        }
        #endregion

        #region 获取连接信息
        public JObject GetConnInfo() {
            string isShowConn = ConfigurationManager.AppSettings["isShowConn"];
            JObject jo = new JObject();
            if (!string.IsNullOrEmpty(isShowConn) && isShowConn == "1")
            {
                jo["flag"] = 1;
                string connStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                string[] arr = connStr.Split(';');
                string ip = arr[0].Split('=')[1];
                string database = arr[1].Split('=')[1];
                jo["ip"] = ip;
                if (ip == "192.168.0.252" && database == "UFDATA_005_2015")
                {
                    jo["message"] = "正式账号";
                }
                else {
                    jo["message"] = "测试账号";
                }
                jo["database"] = database;
            }
            else {
                jo["flag"] = 0;
            }
            return jo;
        }
	#endregion
       
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}