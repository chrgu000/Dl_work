using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Text;
using System.Security.Cryptography;
using System.Web.SessionState;
using Model;
using System.Configuration;

namespace DingDan_WebForm.Html
{
    public partial class ReceiveSMSReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            JObject jo = new JObject();
            string postContent = string.Empty;
            Stream postData = HttpContext.Current.Request.InputStream;
            StreamReader sRead = new StreamReader(postData, System.Text.Encoding.UTF8);
            postContent += sRead.ReadToEnd();
            sRead.Close();
            string a = postData.ToString();
            jo = JObject.Parse(postContent);
            bool b = SMSReport(jo["mobile"].ToString(), jo["submitDate"].ToString(), jo["receiveDate"].ToString(), jo["errorCode"].ToString(), jo["msgGroup"].ToString(), jo["reportStatus"].ToString());



        }


        public bool SMSReport(string phone, string submitDate, string receiveDate, string errorCode, string msgGroup, string reportStatus)
        {
            bool b = false;
            string cmdText = "insert into Dl_opSMSReport (phone,submitDate,receiveDate,errorCode,msgGroup,reportStatus) values(@phone,@submitDate,@receiveDate,@errorCode,@msgGroup,@reportStatus)";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@phone",phone),
            new SqlParameter("@submitDate",submitDate),
            new SqlParameter("@receiveDate",receiveDate),
            new SqlParameter("@errorCode",errorCode),
            new SqlParameter("@msgGroup",msgGroup),
            new SqlParameter("@reportStatus",reportStatus)
       
            };
            int res = new DAL.SQLHelper().ExecuteNonQuery(cmdText, paras, CommandType.Text);
            if (res > 0)
            {
                b = true;
            }
            return b;
        }
        public virtual void AjaxProcess(HttpContext context)
        {

        }
    }
}