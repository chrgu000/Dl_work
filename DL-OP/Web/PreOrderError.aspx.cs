using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using BLL;

public partial class PreOrderError : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DataTable dt = new SearchManager().DL_PreOrderSettingTimeBySel();
        ASPxLabel1.Text = "活动期间:"+dt.Rows[0]["datStartTime"].ToString() + "~" + dt.Rows[0]["datEndTime"].ToString();

    }
}