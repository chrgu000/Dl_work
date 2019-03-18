using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DingDan_WebForm.Handler
{
    /// <summary>
    /// Base 的摘要说明
    /// </summary>
    public class Base : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            //   string PageType = HttpContext.Current.Request.Form["PageType"];
            //if (HttpContext.Current.Request.Form["Action"] == "DL_PreviousOrderBySel")
            //{
            //    HttpContext.Current.Response.Write("");
            //    return;
            //}


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