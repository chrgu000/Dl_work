using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Data;


public partial class dluser_Log : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        DataTable dt = new OrderManager().DL_LogBySel();
        DVGLog.DataSource = dt;
        DVGLog.DataBind();
    }
    protected void Button2_Click(object sender, EventArgs e)
    {

    }
}