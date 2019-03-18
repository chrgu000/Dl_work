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
using System.Collections;

public partial class CheckPendingOrder : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //初始化,设置金额合计启用报价合计还是执行价合计?
        Hashtable ht = (Hashtable)Session["SysSetting"];
        if (ht["IsExercisePrice"].ToString() == "0")   //报价金额
        {
            ViewOrderGrid.TotalSummary[1].FieldName = "cComUnitAmount";
        }
        else//执行价金额
        {
            ViewOrderGrid.TotalSummary[1].FieldName = "xx";
        }
        if (!IsPostBack)
        {
            if (Request.QueryString["vbillno"] != null) //查看
            {
                //查看订单状态
                string strBillNo = Request.QueryString["vbillno"].ToString();
                DataTable dt = new OrderManager().DL_OrderBillBySel(strBillNo);
                //绑定表头字段,text
                TxtBillDate.Text = dt.Rows[0]["datCreateTime"].ToString();
                TxtBiller.Text = dt.Rows[0]["ccusname"].ToString();
                TxtCustomer.Text = dt.Rows[0]["ccusname"].ToString();
                TxtOrderBillNo.Text = strBillNo;
                TxtOrderMark.Text = dt.Rows[0]["strRemarks"].ToString();
                switch (dt.Rows[0]["cSCCode"].ToString())
                {
                    case "00":
                        TxtcSCCode.Text = "自提";
                        break;
                    case "01":
                        TxtcSCCode.Text = "配送";
                        break;
                }
                TxtOrderShippingMethod.Text = dt.Rows[0]["cDefine11"].ToString();
                TxtSalesman.Text = dt.Rows[0]["cpersoncode"].ToString();
                //绑定表体字段,grid
                ViewOrderGrid.DataSource = dt;
                ViewOrderGrid.DataBind();
                //绑定grid
                DataTable dtgrid = new DataTable();
                int bytStatus = 1;
                dt = new OrderManager().DL_UnauditedOrder_SubBySel(bytStatus, Session["lngopUserId"].ToString(), Session["lngopUserExId"].ToString());
                GridOrder.DataSource = dt;
                GridOrder.DataBind();
            }
            if (Request.QueryString["cbillno"] != null) //催办
            {
                //绑定grid
                DataTable dt = new DataTable();
                int bytStatus = 1;
                dt = new OrderManager().DL_UnauditedOrder_SubBySel(bytStatus, Session["lngopUserId"].ToString(), Session["lngopUserExId"].ToString());
                GridOrder.DataSource = dt;
                GridOrder.DataBind();
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('催办成功!');</script>");
            }
            if (Request.QueryString["qhid"] != null) //取回
            {
                string strBillNo = Request.QueryString["qhid"].ToString();
                string strManagers=" ";
                //取回,将状态改为被驳回状态,status=3
                bool c = new OrderManager().DL_RejectOrderBillSelfByUpd(strBillNo, strManagers);
                //绑定grid
                DataTable dt = new DataTable();
                int bytStatus = 1;
                dt = new OrderManager().DL_UnauditedOrder_SubBySel(bytStatus, Session["lngopUserId"].ToString(), Session["lngopUserExId"].ToString());
                GridOrder.DataSource = dt;
                GridOrder.DataBind();
                if (c)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('取回成功,请到被驳回订单中处理该订单!');</script>");
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('取回失败,该订单已经被处理，无法取回!');</script>");
                }
                
            }
        }
    }


    protected void BtnRefresh_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        int bytStatus = 1;
        dt = new OrderManager().DL_UnauditedOrder_SubBySel(bytStatus, Session["lngopUserId"].ToString(), Session["lngopUserExId"].ToString());
        //dt = new OrderManager().DL_UnauditedOrder_DSH_BySel(bytStatus, Session["ConstcCusCode"].ToString() + '%');
        GridOrder.DataSource = dt;
        GridOrder.DataBind();
    }

    protected void GridOrder_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
    {
        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('button！');</script>");
        /*审核事件*/
        //插入表头数据

        //插入表体数据

        //更新DLorder表中该单据状态值为已审核,并且重新绑定grid


    }

}