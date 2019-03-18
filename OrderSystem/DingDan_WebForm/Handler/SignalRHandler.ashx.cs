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
using System.Configuration;


namespace DingDan_WebForm.Handler
{
    /// <summary>
    /// SignalRHandler 的摘要说明
    /// </summary>
    public class SignalRHandler : SignalRBase
    {

        SignalR.MyHub MyHub = new SignalR.MyHub();
        Check check = new Check();
        public ReInfo errMsg = new ReInfo()
        {
            list_msg = new List<string>(),
            flag = "0",
            message = "程序出现错误,请重试或联系管理员!!"
        };
        public override void AjaxProcess(HttpContext context)
        {
            ReInfo ri = new ReInfo();
            string Action = HttpContext.Current.Request.Form["Action"];
            if (string.IsNullOrEmpty(Action))
            {
                ri.flag = "0";
                ri.message = "错误的方法!";
                context.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }
            var method = this.GetType().GetMethod(Action);
            if (method == null)
            {
                ri.flag = "0";
                ri.message = "错误的方法!";
                context.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }
            try
            {
                method.Invoke(this, new object[] { });
            }
            catch (Exception err)
            {

                errMsg.list_msg.Add("Action:" + HttpContext.Current.Request.Form["Action"]);
                errMsg.list_msg.Add("ConstcCusCode:" + HttpContext.Current.Session["ConstcCusCode"].ToString());
                errMsg.list_msg.Add(err.ToString());
                check.WriteLog(errMsg);
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(errMsg));
                return;
            }


        }
 

        public void client_Login()
        {
            JObject info = new JObject();
            info["signalRId"] = HttpContext.Current.Request.Form["signalRId"];
            info["sessionId"] = HttpContext.Current.Session.SessionID.ToString();
            info["strLoginPhone"] = HttpContext.Current.Session["strLoginPhone"].ToString();
            info["strLoginName"] = HttpContext.Current.Session["strLoginName"].ToString();
            info["strAllAcount"] = HttpContext.Current.Session["strAllAcount"].ToString();
            info["lngopUserId"] = HttpContext.Current.Session["lngopUserId"].ToString();
            info["loginTime"] = DateTime.Now.ToString();
            info["checkTimes"] = 0;
            MyHub.signalR_Login(info);
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(info));
        }

        public void getAllClient(){
            JObject lists = MyHub.getAllClient();
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(lists));
        }

        public void sendMsg() { 
            JObject jo=new JObject();
            string signalRId = HttpContext.Current.Request.Form["signalRId"];
            jo["title"]=HttpContext.Current.Request.Form["title"];
            jo["content"]=HttpContext.Current.Request.Form["content"];
            if (string.IsNullOrEmpty(signalRId))
            {
                MyHub.sendMsg(jo);
            }
            else
	{
                  MyHub.sendMsg(signalRId, jo);
	}
          
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
        }

 

    }
}