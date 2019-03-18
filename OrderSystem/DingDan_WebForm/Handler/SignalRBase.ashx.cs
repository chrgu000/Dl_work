using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Model;
using Newtonsoft.Json;
using System.Web.Services;

namespace DingDan_WebForm.Handler
{
    /// <summary>
    /// SignalRBase 的摘要说明
    /// </summary>
    public class SignalRBase : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
         
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