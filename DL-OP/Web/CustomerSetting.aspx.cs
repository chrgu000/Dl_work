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

public partial class CustomerSetting : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //获取手机号码
        string username = Session["ConstcCusCode"].ToString();
        DataTable dt = new GetPhoneNo().GetCustomerPhoneNo(username);
        if (dt.Rows.Count < 1)
        {
            PhoneGrid.SettingsText.EmptyDataRow = "还没有手机号码,赶紧添加一个!";
        }
        PhoneGrid.DataSource = dt;
        PhoneGrid.DataBind();
        //获取账单日期
        DataTable dtsoa = new BasicInfoManager().DL_SOAAutoSendBySel(username+"%");
        SOADateGrid.DataSource = dtsoa;
        SOADateGrid.DataBind();
    }
    protected void PhoneGrid_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e) //生成Grid的序号
    {
        if (e.Column.Caption == "序号" && e.IsGetData)
        {
            e.Value = (e.ListSourceRowIndex + 1).ToString();
        }
    }
}