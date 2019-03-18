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

public partial class dluser_Atasks_Y : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //查询数据
        /*20151210,刷新可接订单*/
        DataTable dt = new DataTable();
        int bytStatus = 1;
        int lngBillType = 1;
        dt = new OrderManager().DLproc_UnauditedpreOrderBySel(bytStatus, lngBillType);
        GridOrder.DataSource = dt;
        GridOrder.DataBind();

        string strManagers = Session["lngopUserId"].ToString();

        if (Request.QueryString["gbillno"] != null) //接单
        {
            string strBillNo = Request.QueryString["gbillno"].ToString();
            //检测是否已经有专属操作员
            DataTable d = new OrderManager().DL_ManagersPreOrderBillBySel(strBillNo);
            if (d.Rows.Count < 1)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + "已经被其他人员接收！');</script>");
                dt = new OrderManager().DLproc_UnauditedOrderBySel(bytStatus);
                GridOrder.DataSource = dt;
                GridOrder.DataBind();
                return;
            }
            //绑定专属操作员
            bool c = new OrderManager().DL_ManagersPreOrderBillByUpd(strBillNo, strManagers);

            //重新绑定显示
            dt = new OrderManager().DLproc_UnauditedpreOrderBySel(bytStatus, lngBillType);
            GridOrder.DataSource = dt;
            GridOrder.DataBind();

            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + "已经接收,请到<预订单处理-待办酬宾订单>中及时处理！');</script>");
        }
    }

    protected void BtnRefresh_Click(object sender, EventArgs e) //刷新,操作员为空的订单(可接订单)
    {
        Response.Redirect("Atasks_Y.aspx");
    }
}