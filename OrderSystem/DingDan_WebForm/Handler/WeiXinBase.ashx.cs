using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;
using Newtonsoft.Json;
using System.Web.SessionState;
using System.Configuration;
using Newtonsoft.Json.Linq;

namespace DingDan_WebForm.Handler
{
    /// <summary>
    /// MobileBase 的摘要说明
    /// </summary>
    public class WeiXinBase : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            JObject jo = new JObject();
            //if (HttpContext.Current.Session["ConstcCusCode"] == null)
            //{
            //    jo["flag"] = "-1";
            //    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            //    return;
            //}

            string is_wxlogin = ConfigurationManager.AppSettings["is_wxlogin"];
            if (is_wxlogin == "1")
            {
                if (HttpContext.Current.Session["WXUserId"] == null)
                {
                   
                    jo["flag"] = "0";
                    jo["message"] = "没有权限进入";
                    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                    return;
                }
            }

         
            //  HttpContext.Current.Session["lngopUserId"] = 999;
            AjaxProcess(context);
        }

        public virtual void AjaxProcess(HttpContext context)
        {

        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}