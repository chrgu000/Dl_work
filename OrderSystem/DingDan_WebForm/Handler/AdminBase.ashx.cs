using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Model;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Xml;
using System.Data;

namespace DingDan_WebForm.Handler
{
    /// <summary>
    /// Base 的摘要说明
    /// </summary>
    public class AdminBase : IHttpHandler, IRequiresSessionState
    {
       

        public void ProcessRequest(HttpContext context)
        {
           // context.Response.AddHeader("Access-Control-Allow-Origin", "*");
            //HttpRequest req = context.Request;
            //string code = req["Action"];
            //string a = HttpContext.Current.Request.Form[0];
            //JObject jo1 = new JObject();
            //jo1["Now"] = DateTime.Now.ToString();
            //context.Response.Write(JsonConvert.SerializeObject(jo1));
            //    return;
            JObject jo = Check_Role();
            if (jo["flag"].ToString() != "1")
            {
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                return;
            }
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

        public JObject Check_Role()
        {
            JObject jo = new JObject();

            if (HttpContext.Current.Session["AdminstrLoginName"] == null)
            {
                jo["flag"] = "-3";
                jo["message"] = "未登录！";
                return jo;
            }
            string xmlPath = "RoleControle.xml";
            string systemPath = System.AppDomain.CurrentDomain.BaseDirectory;
            string path = systemPath + xmlPath;
            DataSet ds = new DataSet();
            ds.ReadXml(path);
            DataView user_dv = ds.Tables["user"].DefaultView;
            user_dv.RowFilter = "loginname=" + HttpContext.Current.Session["AdminstrLoginName"].ToString();

            if (user_dv.Count == 0)
            {
                jo["flag"] = "-1";
                jo["message"] = "你没有权限进入后台！";
                return jo;
            }

            string groupid = user_dv.ToTable().Rows[0]["groupid"].ToString();
            if (groupid == "99")
            {
                jo["flag"] = "1";
                return jo;
            }
            DataView group_dv = ds.Tables["group"].DefaultView;
            group_dv.RowFilter = "groupid=" + groupid;

            if (group_dv.Count == 0)
            {
                jo["flag"] = "-1";
                jo["message"] = "你没有权限进入后台！";
                return jo;
            }

            DataView method_dv = ds.Tables["method"].DefaultView;
            method_dv.RowFilter = "methodname like '" + HttpContext.Current.Request.Form["Action"] + "'";
            if (method_dv.Count == 0)
            {
                jo["flag"] = "-2";
                jo["message"] = "错误的方法！";
                return jo;
            }
            string methodid = method_dv.ToTable().Rows[0]["methodid"].ToString();
            Array arr = group_dv.ToTable().Rows[0]["methodids"].ToString().Split('|');
            if (Array.IndexOf(arr, methodid) == -1)
            {
                jo["flag"] = "-1";
                jo["message"] = "没有权限！";
                return jo;
            }
            else
            {
                jo["flag"] = "1";
                return jo;

            }


        }

        //public JObject Check_Role()
        //{
        //    JObject jo = new JObject();
        //    string configPath = "RoleControle.xml";
        //    string systemPath = System.AppDomain.CurrentDomain.BaseDirectory;
        //    string path = systemPath + configPath;
        //    XmlDocument doc = new XmlDocument();
        //    doc.Load(path);

        //    string loginname = HttpContext.Current.Session["strLoginName"].ToString();
        //    XmlNodeList user_nodes = doc.SelectNodes("//user[@loginname=" + loginname + "]");
        //    if (user_nodes.Count == 0)
        //    {
        //        jo["flag"] = "-1";
        //        jo["message"] = "没有权限！";
        //        return jo;
        //    }
        //    string groupid = user_nodes.Item(0).Attributes["groupid"].Value;
        //    if (groupid == "99")
        //    {
        //        jo["flag"] = "1";
        //        return jo;
        //    }
        //    string action = HttpContext.Current.Request.Form["Action"];

        //    XmlNodeList method_nodes = doc.SelectNodes("//method");
        //    string methodid = string.Empty;
        //    foreach (XmlNode item in method_nodes)
        //    {
        //        if (item.InnerText == action)
        //        {
        //            methodid = item.Attributes["methodid"].Value;
        //        }
        //    }

        //    if (string.IsNullOrEmpty(methodid))
        //    {
        //        jo["flag"] = "-2";
        //        jo["message"] = "错误的方法！";
        //        return jo;

        //    }

        //    string allowed_method = doc.SelectSingleNode("//group[@groupid=" + groupid + "]").Attributes["methodids"].Value;

        //    Array arr = allowed_method.Split('|');

        //    if (Array.IndexOf(arr, methodid) == -1)
        //    {
        //        jo["flag"] = "0";
        //        jo["message"] = "没有权限！";
        //        return jo;
        //    }
        //    else
        //    {
        //        jo["flag"] = "1";
        //        return jo;
        //    }
        //}
    }
}