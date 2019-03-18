using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;
using Newtonsoft.Json;
using System.Web.SessionState;
using System.Configuration;

namespace DingDan_WebForm.Handler
{
    /// <summary>
    /// MobileBase 的摘要说明
    /// </summary>
    public class MobileBase : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            string is_wxlogin = ConfigurationManager.AppSettings["is_wxlogin"];
            if (is_wxlogin=="1")
            {
                if (HttpContext.Current.Session["WXUserId"] == null)
                {
                    ReInfo ri = new ReInfo();
                    ri.flag = "0";
                    ri.message = "没有权限进入";
                    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
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