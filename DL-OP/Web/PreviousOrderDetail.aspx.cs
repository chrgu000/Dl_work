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

public partial class PreviousOrderDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        #region 查看订单状态
        if (Request.QueryString["ubillno"] != null)
        {
            //查看订单状态
            string strBillNo = Request.QueryString["ubillno"].ToString();
            DataTable dt=new DataTable();
            if  ( strBillNo.Substring(0,4).ToString() == "CZTS")
            {
                dt = new OrderManager().DL_OrderCZTSBillBySel(strBillNo);
            }
            else
            {
                dt = new OrderManager().DL_OrderU8BillBySel(strBillNo);
            }
            
            //绑定表头字段,text,cMaker	cPersonName	cSCCode	cMemo,cdefine11
            TxtBiller.Text = dt.Rows[0]["cMaker"].ToString();
            TxtOrderMark.Text = dt.Rows[0]["cMemo"].ToString();
            TxtcSCCode.Text = dt.Rows[0]["cSCCode"].ToString();
            TxtOrderShippingMethod.Text = dt.Rows[0]["cdefine11"].ToString();
            TxtBillNo.Text = strBillNo;
            TxtQQ.Text = dt.Rows[0]["strQQName"].ToString();
            TxtBillTime.Text = dt.Rows[0]["datBillTime"].ToString();
            TxtAuthTime.Text = dt.Rows[0]["datAuditordTime"].ToString();
            //绑定表体字段,grid
            ViewOrderGrid.DataSource = dt;
            ViewOrderGrid.DataBind();
            ////绑定表头grid
            //DataTable dtgrid = new DataTable();
            //dtgrid = new OrderManager().DL_PreviousOrderBySel(Session["ConstcCusCode"].ToString() + '%');
            //GridOrder.DataSource = dtgrid;
            //GridOrder.DataBind();
        }
        #endregion
    }
}