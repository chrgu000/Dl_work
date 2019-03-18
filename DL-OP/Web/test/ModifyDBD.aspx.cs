using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using U8API;
using UFIDA.U8.U8APIFramework;
using System.Data;
using UFIDA.U8.U8MOMAPIFramework;
using System.Runtime.InteropServices;
using MSXML2;
using BLL;

public partial class test_ModifyDBD : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DBD dbd = new DBD();
        string aa = dbd.MDBD("170700005");
        Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('" + aa + "');</script>");
        return;
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        //string aa = MDBD("170700005");
    }

}