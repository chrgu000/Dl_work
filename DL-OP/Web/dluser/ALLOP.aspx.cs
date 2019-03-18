using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;

public partial class dluser_ALLOP : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void BtnSHL_Click(object sender, EventArgs e)
    {
        bool c = new OrderManager().DLproc_SHLByUpd(TxtBillNo.Text);
        if (c)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('重算成功');</script>");
            return;
        }
        else
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('重算失败');</script>");
            return;
        }
    }
}