using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Model;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Security;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["login"] == null || Convert.ToInt16(Session["strUserLevel"].ToString()) > 2)
            {
                Response.Redirect("login.aspx");
            }
        }
    }

    protected void setPWD_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
    {
        Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('setPWD_WindowCallback！');</script>");
    }
    //protected string newPWD(string pwd, string conpwd)
    //{

    //    return pwd + "_" + conpwd;
    //}
    protected void btOK_Click(object sender, EventArgs e)
    {
        if (tbPWD.Text.ToString() != "" && tbPWD.Text.ToString() == tbPassword.Text.ToString())
        {
            //修改密码
            string pwd = FormsAuthentication.HashPasswordForStoringInConfigFile(tbPWD.Text.Trim(), "MD5");// 把密码转为MD5码的形式
            BasicInfo bi = new BasicInfo(Session["lngopUserId"].ToString(), pwd);
            bool b = new BasicInfoManager().Update_UserPWD(bi);
            System.Web.UI.ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "updateScript", "alert('密码修改成功,请牢记新密码！');", true);
        }
    }

}