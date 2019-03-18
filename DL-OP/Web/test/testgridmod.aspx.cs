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

public partial class test_testgridmod : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        object oValue = null;
        string sKeyFieldName = "strUserName";
        string sKeyFieldValue = "川南二十六片区";
        DataTable dt = new BasicInfoManager().DL_AllCustomerInfoBySel();
        ASPxGridView1.DataSource = dt;
        ASPxGridView1.DataBind();
        //ASPxGridView1.PageIndex = ASPxGridView1.PageCount-1;
        for (int i = 0; i < ASPxGridView1.VisibleRowCount; i++)
        {
            oValue = ASPxGridView1.GetRowValues(i, sKeyFieldName);
            if (oValue != null && oValue.ToString() == sKeyFieldValue)
            {
                ASPxGridView1.SettingsBehavior.AllowFocusedRow = false;
                ASPxGridView1.SettingsBehavior.AllowFocusedRow = true;
                ASPxGridView1.FocusedRowIndex = -1;
                ASPxGridView1.Selection.SelectRow(i);
                bool bBackToPage = ASPxGridView1.MakeRowVisible(sKeyFieldValue);// 可以定位回到某一页去  
                break;
            }
        }


    }
    protected void ASPxButton1_Click(object sender, EventArgs e)
    {

    }
}