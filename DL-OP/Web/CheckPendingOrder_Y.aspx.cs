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

public partial class CheckPendingOrder_Y : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["vbillno"] != null)
            {
                //查看订单状态
                string strBillNo = Request.QueryString["vbillno"].ToString();
                DataTable dt = new OrderManager().DL_PreOrderBillBySel(strBillNo);
                //绑定表头字段,text
                TxtBillDate.Text = dt.Rows[0]["ddate"].ToString();
                TxtCustomer.Text = dt.Rows[0]["ccusname"].ToString();
                TxtOrderBillNo.Text = strBillNo;
                //绑定表体字段,grid
                ViewOrderGrid.DataSource = dt;
                ViewOrderGrid.DataBind();
                //绑定grid
                DataTable dtgrid = new DataTable();
                int bytStatus = 1;
                dt = new OrderManager().DL_UnauditedOrderBySel(bytStatus, Session["ConstcCusCode"].ToString() + '%');
                GridOrder.DataSource = dt;
                GridOrder.DataBind();
            }
            if (Request.QueryString["cbillno"] != null)
            {
                //绑定grid
                DataTable dt = new DataTable();
                int bytStatus = 1;
                dt = new OrderManager().DL_UnauditedPreOrderBySel(bytStatus, Session["ConstcCusCode"].ToString() + '%');
                GridOrder.DataSource = dt;
                GridOrder.DataBind();
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('催办成功!');</script>");
            }
        }
    }


    protected void BtnRefresh_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        int bytStatus = 1;
        dt = new OrderManager().DL_UnauditedPreOrderBySel(bytStatus, Session["ConstcCusCode"].ToString() + '%');
        GridOrder.DataSource = dt;
        GridOrder.DataBind();
    }

    protected void GridOrder_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
    {
    }
}