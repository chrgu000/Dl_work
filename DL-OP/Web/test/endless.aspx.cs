using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading;
using DevExpress.Web;
using DevExpress.Web.ASPxTreeList;
using BLL;
using Model;

public partial class test_endless : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DataTable dt = new SearchManager().DL_InventoryAllBySel();
        ASPxGridView1.DataSource = dt;
        ASPxGridView1.DataBind();
    }
    protected void ASPxButton1_Click(object sender, EventArgs e)
    {
        DataTable dt = new SearchManager().DL_InventoryAllBySel();
        ASPxGridView1.DataSource = dt;
        ASPxGridView1.DataBind();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        System.Threading.Thread.Sleep(2000);
        Label1.Text = DateTime.Now.ToString();
    }  
}