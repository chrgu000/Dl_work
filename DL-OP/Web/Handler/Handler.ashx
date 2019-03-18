<%@ WebHandler Language="C#" Class="Handler" %>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Configuration;
using System.Data.SqlClient;

public class Handler : IHttpHandler, IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string answer = "";
        answer += "'" + context.Session["cCusCode"].ToString() + "'," + "'" + context.Session["strUserName"].ToString() + "',";
        for (int i = 1; i < 34; i++)
        {
            answer += "'" + context.Request["question" + i] + "',";
        }
        //  answer = answer.Substring(0, answer.Length - 1);
        answer += "'" + context.Request["advise"] + "',";
        answer += "'" + context.Session["cCusCode"].ToString() + "',";
        answer += "'" + DateTime.Now + "'";
        string sql = "insert into DiaoCha values(" + answer + ")";
        string ConnStr = ConfigurationManager.ConnectionStrings["db"].ConnectionString;
        using (SqlConnection connection = new SqlConnection(ConnStr))
        {
            connection.Open();
            using (SqlCommand sqlcmd = connection.CreateCommand())
            {

                sqlcmd.CommandText = sql;
                if (sqlcmd.ExecuteNonQuery() > 0)
                {
                    context.Response.Write("感谢您的参与！");
                }
                else
                {
                    context.Response.Write("提交失败");
                }
            }
        }

    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}