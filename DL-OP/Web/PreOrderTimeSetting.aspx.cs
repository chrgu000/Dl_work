using BLL;
using DevExpress.Web;
using System;
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

public partial class PreOrderTimeSetting : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //读取设置时间
        DataTable dt = new SearchManager().DL_PreOrderSettingTimeBySel();
        ASPxLabel1.Text = dt.Rows[0]["datStartTime"].ToString() + "~" + dt.Rows[0]["datEndTime"].ToString();
    }
    protected void ASPxButton1_Click(object sender, EventArgs e)
    {
       string startdate= ASPxDateEdit1.Value.ToString();
       string enddate = ASPxDateEdit2.Value.ToString();
       bool c = new SearchManager().DL_PreOrderSettingTimeByUpd(startdate, enddate);
    }
}