using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Data;
using BLL;
using System.Web.SessionState;
using System.Text;
using Model;
using System.Web.Script.Serialization;
using System.ServiceModel.Web;
using System.Runtime.Serialization.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Data.SqlClient;
using System.Configuration;
using System.Net;

namespace DingDan_WebForm.Handler
{
    /// <summary>
    /// MobileHandler 的摘要说明
    /// </summary>
    public class WeiXinHandler : WeiXinBase
    {

        WeiXin weixin = new WeiXin();
        Check check = new Check();
        SMSSend9003.SendSMS2CustomerSoapClient SMSSend9003 = new SMSSend9003.SendSMS2CustomerSoapClient();
        WeiXin9003.WeiXinSoapClient WeiXin9003 = new WeiXin9003.WeiXinSoapClient();
        WeiXin8222.WeiXinSoapClient WeiXin8222 = new WeiXin8222.WeiXinSoapClient();
        public ReInfo errMsg = new ReInfo()
        {
            list_msg = new List<string>(),
            flag = "0",
            message = "程序出现错误,请重试或联系管理员!!"
        };


        public override void AjaxProcess(HttpContext context)
        {
            JObject jo = new JObject();
            string Action = HttpContext.Current.Request.Form["Action"];
            if (string.IsNullOrEmpty(Action))
            {
                jo["flag"] = 0;
                jo["message"] = "错误的方法!";
                context.Response.Write(JsonConvert.SerializeObject(jo));
                return;
            }
            var method = this.GetType().GetMethod(Action);
            if (method == null)
            {
                jo["flag"] = 0;
                jo["message"] = "错误的方法!";
                context.Response.Write(JsonConvert.SerializeObject(jo));
                return;
            }
            try
            {
                method.Invoke(this, new object[] { });
            }
            catch (Exception err)
            {

                errMsg.list_msg.Add("Action:" + HttpContext.Current.Request.Form["Action"]);
                if (HttpContext.Current.Session["ConstcCusCode"] == null)
                {
                    errMsg.list_msg.Add("ConstcCusCode:ConstcCusCode为空");
                }
                else {
                    errMsg.list_msg.Add("ConstcCusCode:" + HttpContext.Current.Session["ConstcCusCode"].ToString());
                }
              //  errMsg.list_msg.Add("ConstcCusCode:" +  HttpContext.Current.Session["ConstcCusCode"]==null?"ConstcCusCode为空":HttpContext.Current.Session["ConstcCusCode"].ToString());
                errMsg.list_msg.Add(err.ToString());
                check.WriteLog(errMsg);
                jo["flag"] = 0;
                jo["message"] = "程序出现错误,请重试或联系管理员!!!";
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                return;
            }


        }


        #region 测试专用
        public void test()
        {
            string id = HttpContext.Current.Request.Form["id"];
            JObject jo = new JObject();
            jo["Now"] = DateTime.Now.ToString();
            jo["id"] = id;
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
        }
        #endregion

        #region Http
        public void GetUrl() {
            string url = HttpContext.Current.Request.Form["url"];
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);    //创建一个请求示例
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();　　//获取响应，即发送请求
            Stream responseStream = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
            string res = streamReader.ReadToEnd();
            JObject jo=new JObject();
            jo["res"] = res;
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
        }
        
        #endregion

        #region 微信登录
        public void Wx_Login()
        {
            string code = HttpContext.Current.Request.Form["code"];

            string appid = ConfigurationManager.AppSettings["WeiXinAPPID"];
          string re = WeiXin9003.Get_UserIdByAuthCode_ByAPPID(code, appid);
          //  string re = WeiXin8222.Get_UserIdByAuthCode_ByAPPID(code, appid);
            JObject jo = (JObject)JsonConvert.DeserializeObject(re);
      
            if (!string.IsNullOrEmpty((string)jo["UserId"]))
            {

                    DataTable dt = new WeiXin().GetIdByPhone((string)jo["UserId"]);
                    if (dt.Rows.Count == 0)
                    {
                        jo["flag"] = 0;
                        jo["message"] = "用户表没有数据！";
                    }
                    else if (dt.Rows.Count > 1) {
                        jo["flag"] = 2;
                        jo["message"] = "";
                        jo["users"] = JToken.Parse(JsonConvert.SerializeObject(dt));
                    }
                    else if (dt.Rows.Count == 1)
                    {
                        jo["flag"] = 1;
                        HttpContext.Current.Session["WXUserId"] = (string)jo["UserId"];
                        //HttpContext.Current.Session["AdminlngopUserId"] = dt.Rows[0]["lngopUserId"].ToString();
                        //HttpContext.Current.Session["AdminstrLoginName"] = dt.Rows[0]["strLoginName"].ToString();
                        HttpContext.Current.Session["strUserLevel"] = dt.Rows[0]["strUserLevel"].ToString();       //权限HttpContext.Current.Session
                        HttpContext.Current.Session["lngopUserId"] = dt.Rows[0]["lngopUserId"].ToString();         //登录用户id
                        HttpContext.Current.Session["strUserName"] = dt.Rows[0]["strUserName"].ToString();         //用户名称
                        HttpContext.Current.Session["ConstcCusCode"] = dt.Rows[0]["cCusCode"].ToString();          //常量,登录用户编码,不变更
                        HttpContext.Current.Session["cCusPhone"] = dt.Rows[0]["cCusPhone"].ToString();  //顾客电话号码*
                        HttpContext.Current.Session["strLoginName"] = dt.Rows[0]["strLoginName"].ToString();  //登录编码,2016-03-30增加
                        HttpContext.Current.Session["lngopUserExId"] = dt.Rows[0]["lngopUserExId"].ToString();  //子账户id,2016-08-26增加
                        HttpContext.Current.Session["strAllAcount"] = dt.Rows[0]["strAllAcount"].ToString();  //子账户编码,2016-08-26增加，如无子账户则为主账户编码,不变更的常量        

                    }
                    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                    return;
                

            }
            else
            {
                jo["flag"] = 0;

                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                return;
            }

        }
        #endregion


        #region 当有一个手机号绑定多个账号时，用户选择一个用户名登录
        public void SelectUserToLogin() {
            string phone = HttpContext.Current.Request.Form["phone"];
            string ccuscode = HttpContext.Current.Request.Form["ccuscode"];
            JObject jo = new JObject();
            DataTable dt = weixin.SelectUserToLogin(phone, ccuscode);
            if (dt.Rows.Count==0)
            {
                jo["flag"] = 0;
                jo["message"] = "未获取数据，请重试或联系管理员！";
            }
            else 
            {
                if (dt.Rows.Count > 1)
                {
                    jo["flag"] = 0;
                    jo["message"] = "仍然有多条数据，请联系管理员处理！";
                }
                else {
                    jo["flag"] = 1;
                    HttpContext.Current.Session["WXUserId"] = (string)jo["UserId"];
                    HttpContext.Current.Session["strUserLevel"] = dt.Rows[0]["strUserLevel"].ToString();       //权限HttpContext.Current.Session
                    HttpContext.Current.Session["lngopUserId"] = dt.Rows[0]["lngopUserId"].ToString();         //登录用户id
                    HttpContext.Current.Session["strUserName"] = dt.Rows[0]["strUserName"].ToString();         //用户名称
                    HttpContext.Current.Session["ConstcCusCode"] = dt.Rows[0]["cCusCode"].ToString();          //常量,登录用户编码,不变更
                    HttpContext.Current.Session["cCusPhone"] = dt.Rows[0]["cCusPhone"].ToString();  //顾客电话号码*
                    HttpContext.Current.Session["strLoginName"] = dt.Rows[0]["strLoginName"].ToString();  //登录编码,2016-03-30增加
                    HttpContext.Current.Session["lngopUserExId"] = dt.Rows[0]["lngopUserExId"].ToString();  //子账户id,2016-08-26增加
                    HttpContext.Current.Session["strAllAcount"] = dt.Rows[0]["strAllAcount"].ToString();  //子账户编码,2016-08-26增加，如无子账户则为主账户编码,不变更的常量        

                }
            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;

        }
        #endregion

        #region 查询历史订单列表
        public void GetHistoryOrderList() {
            JObject jo = new JObject();
           //  string lngopUserID = HttpContext.Current.Session["lngopUserId"].ToString();
            string days = HttpContext.Current.Request.Form["days"];
            string start_day = DateTime.Now.AddDays(double.Parse("-" + days)).ToShortDateString();
            string end_day = DateTime.Now.AddDays(1).ToShortDateString();
            jo["list"] = JToken.Parse(JsonConvert.SerializeObject(weixin.GetHistoryOrderList("92",start_day,end_day)));
            jo["flag"] = 1;
            jo["message"] = "用户表没有数据！";
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return  ;
        }
        #endregion

        #region 查询历史订单详情
        public void GetOrderInfo()
        {
            JObject jo = new JObject();
            string orderId = HttpContext.Current.Request.Form["orderId"];
            string orderType = HttpContext.Current.Request.Form["orderType"];
            jo["info"] = JToken.Parse(JsonConvert.SerializeObject(weixin.GetOrderInfo(orderId)));
            jo["flag"] = 1;
            jo["orderType"] = orderType;
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 获取客户所有产品分类
        public void GetProClass() {
            JObject jo = new JObject();
            jo["flag"] = 1;
            jo["ProClass"] = JToken.Parse(JsonConvert.SerializeObject(new product().GetCodeConfigProductClass("999999")));
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
        }
        #endregion


        #region 根据产品分类获取对应的产品
        public void GetProducts()
        {
           
            string code = HttpContext.Current.Request.Form["code"];
            JObject jo = new JObject();
            jo["flag"] = 1;
            jo["Products"] = JToken.Parse(JsonConvert.SerializeObject(new BLL.product().GetCodeConfigProducts("999999", "999999", code)));
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;

        }
        #endregion

        public void Upload()
        {
            JObject jo = new JObject();
            jo["flag"] = 0;
            jo["message"] = "用户表没有数据！";
            var a = HttpContext.Current.Request.Files[0];
            jo["code"] = 0;
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            string filePath = System.AppDomain.CurrentDomain.BaseDirectory + "test/img/";
            string fileName = DateTime.Now.ToString("yyyyMMddhhmmss") + "_" + a.FileName;
            a.SaveAs(filePath + fileName);
            return;
        }

        private string GetTimeStamp(DateTime dt)
        {
            DateTime dateStart = new DateTime(1970, 1, 1, 8, 0, 0);
            int timeStamp = Convert.ToInt32((dt - dateStart).TotalSeconds);
            return timeStamp.ToString();
        }

 
    }
}