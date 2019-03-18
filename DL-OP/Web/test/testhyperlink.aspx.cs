using BLL;
using DevExpress.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model;
using System.Text.RegularExpressions;
using DevExpress.Web.ASPxTreeList;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
public partial class test_testhyperlink : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DataTable dt1 = new SearchManager().DLproc_TreeListDetailsAllBySel("010201002", "999999",1);
        TreeDetail.DataSource = dt1;
        TreeDetail.DataBind();
    }

}