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

public partial class dluser_Atasks : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //查询数据
        /*20151129不显示样品资料订单,直接关联给接单员*/
            DataTable dt = new DataTable();
            int bytStatus = 1;
            dt = new OrderManager().DLproc_UnauditedOrderBySel(bytStatus);
            GridOrder.DataSource = dt;
            GridOrder.DataBind();

        string strManagers = Session["lngopUserId"].ToString();
        #region 接单
        if (Request.QueryString["gbillno"] != null) //接单
        {
            string strBillNo = Request.QueryString["gbillno"].ToString();
            //检测是否已经有专属操作员
            DataTable d = new OrderManager().DL_ManagersOrderBillBySel(strBillNo);
            if (d.Rows.Count < 1)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + "已经被其他人员接收！');</script>");
                dt = new OrderManager().DLproc_UnauditedOrderBySel(bytStatus);
                GridOrder.DataSource = dt;
                GridOrder.DataBind();
                return;
            }
            ////检测是否被顾客取回
            //DataTable qhdt = new OrderManager().DL_OrderBillBySel(strBillNo);
            //if (qhdt.Rows[0]["bytStatus"].ToString() != "1")
            //{
            //    //重新绑定显示
            //    dt = new OrderManager().DLproc_UnauditedOrderBySel(bytStatus);
            //    GridOrder.DataSource = dt;
            //    GridOrder.DataBind();
            //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + "已经被顾客取回，无法接收该订单，请选择其他订单进行处理！');</script>");
            //    return;
            //}
            //绑定专属操作员
            bool c = new OrderManager().DL_ManagersOrderBillByUpd(strBillNo, strManagers);
            //更新关联样品资料订单的操作员

            //重新绑定显示
            dt = new OrderManager().DLproc_UnauditedOrderBySel(bytStatus);
            GridOrder.DataSource = dt;
            GridOrder.DataBind();

            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + "已经接收,请到待办工作中及时处理！');</script>");
        }
        #endregion
       
    }

    protected void BtnRefresh_Click(object sender, EventArgs e) //刷新,操作员为空的订单(可接订单)
    {
        //Response.Redirect("Atasks.aspx");

        //查询数据
        /*20151129不显示样品资料订单,直接关联给接单员*/
        DataTable dt = new DataTable();
        int bytStatus = 1;
        dt = new OrderManager().DLproc_UnauditedOrderBySel(bytStatus);
        GridOrder.DataSource = dt;
        GridOrder.DataBind();
    }
}