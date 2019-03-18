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

public partial class test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DeliveryDate.Value = "2015-11-11"; 
    }

    protected void setPWD_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
    {
        Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('setPWD_WindowCallback！');</script>");
    }

    protected string newPWD(string pwd)
    {
        //if (pwd!="'+tbPWD+'")
        //{
        //    //修改密码
        //    BasicInfo bi = new BasicInfo("2", "dasdasdasd");
        //    bool b = new BasicInfoManager().Update_UserPWD(bi);
        //}
        return pwd;
    }

    protected void ASPxButton1_Click(object sender, EventArgs e)
    {

    }
    protected void btOK_Click(object sender, EventArgs e)
    {
        if (tbPWD.Text.ToString()!="" && tbPWD.Text.ToString()==tbPassword.Text.ToString())
        {
            //修改密码
            BasicInfo bi = new BasicInfo("2", tbPWD.Text.ToString());
            bool b = new BasicInfoManager().Update_UserPWD(bi);  
        }
           
    }
}