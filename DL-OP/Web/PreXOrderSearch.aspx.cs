using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Data.Sql;
using System.Data;

public partial class PreXOrderSearch : System.Web.UI.Page
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
            //BeginDate = DatBeginDate.Value.ToString();
            BeginDate = DatBeginDate.Date.ToShortDateString();
        }

        if (DatEndDate.Value != null)                 //截至日期    
        {
            //EndDate = DatEndDate.Value.ToString();
            EndDate = DatEndDate.Date.ToShortDateString();
        }
        int OrderStatus = Convert.ToInt32(ComboOrderStatus.Value.ToString()); //订单状态
        string lngopUserId = Session["ConstcCusCode"].ToString();     //登录的userid
        //string strManagers = "91";
        if (BeginDate != "" && EndDate != "" && Convert.ToDateTime(BeginDate) > Convert.ToDateTime(EndDate))
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('截止日期大于开始日期！');</script>");
            return;
        }
        //绑定GRID
        DataTable dt = new DataTable();
        dt = new OrderManager().DLproc_MyWorkPreYOrderForCustomerBySel(BillNo, BeginDate, EndDate, OrderStatus, lngopUserId, "2");
        Grid.DataSource = dt;
        Grid.DataBind();
    }
}