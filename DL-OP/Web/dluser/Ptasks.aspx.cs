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

public partial class dluser_Ptasks : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void BtnOk_Click(object sender, EventArgs e)
    {
        string BillNo = TxtBillNo.Text.Trim().ToString();   //订单编号
        string BeginDate = "";
        string EndDate = "";
        if (DatBeginDate.Value != null)                     //开始日期        
        {
            BeginDate = DatBeginDate.Value.ToString();
        }

        if (DatEndDate.Value != null)                 //截至日期    
        {
            EndDate = DatEndDate.Value.ToString();
        }
        //string cSTCode = CombocSTCode.Value.ToString();     //销售类型
        int OrderStatus = Convert.ToInt32(ComboOrderStatus.Value.ToString()); //订单状态
        string strManagers = Session["lngopUserId"].ToString();     //订单专员
        //string strManagers = "91";
        if (BeginDate != "" && EndDate != "" && Convert.ToDateTime(BeginDate) > Convert.ToDateTime(EndDate))
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('截止日期大于开始日期！');</script>");
            return;
        }
        //绑定GRID
        DataTable dt = new DataTable();
        dt = new OrderManager().DLproc_MyWorkPreOrderBySel(BillNo, BeginDate, EndDate, OrderStatus, strManagers);
        Grid.DataSource = dt;
        Grid.DataBind();

    }
}