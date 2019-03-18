using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Logout : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["id"]!=null)
        {
            Session.Abandon();  //清除Session 会话 
            Session.Clear();    //清除当前浏览器进程所有session 
            Session.Remove("login");
            Response.Write("<script>top.window.location='login.aspx?id=dl'</script>");   //跳转到登陆页
        }
        else
        {
            Session.Abandon();  //清除Session 会话 
            Session.Clear();    //清除当前浏览器进程所有session 
            Session.Remove("login");
            Response.Write("<script>top.window.location='login.aspx'</script>");   //跳转到登陆页
            //Response.Redirect("login.aspx");
        }
        if (Session["login"] == null)
        {
            Response.Write("<script>top.window.location='login.aspx'</script>");   //跳转到登陆页
        }
    }
}