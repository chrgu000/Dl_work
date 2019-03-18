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

public partial class OrderConfirm : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        BtnModifyOrder.Visible = false;
        if (!IsPostBack)
        {
            #region 确认生成U8订单
            if (Request.QueryString["billno"] != null)
            {
                string strBillNo = Request.QueryString["billno"].ToString();
                if (strBillNo.Substring(0, 1).ToString() == "D")       //DL和U8的订单查询,DL订单
                {
                    //插入表头数据    --10.25更新方法,在插入表头数据之后,继续插入表体数据.最后更新dl_oporder表中的U8订单号,cSOCode,并且更新dl_oporder订单状态
                    //string strBillNo = Request.QueryString["billno"].ToString();
                    //bool c = new OrderManager().DLproc_NewOrderU8ByIns(strBillNo);
                    //if (c == false)
                    //{
                    //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + "确认失败,请联系系统管理员！');</script>");
                    //    return;
                    //}
                }
                else if (strBillNo.Substring(0, 3).ToString() == "FHD")
                {
                    bool c = new OrderManager().DL_U8FHDOrderBillConfirmByUpd(strBillNo.Replace("FHD", ""));
                    if (c == false)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + "确认失败,请联系系统管理员！');</script>");
                        return;
                    }
                }
                else     //DL和U8的订单查询,U8订单
                {
                    bool c = new OrderManager().DL_U8OrderBillConfirmByUpd(strBillNo);
                    if (c == false)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + "确认失败,请联系系统管理员！');</script>");
                        return;
                    }
                }

                //插入表体数据    --插入表头数据时,已经写入!

                //更新dlorder表中该单据状态值为已审核,并且更新DL订单的表头上的正式订单号     --插入表头数据时,已经更新!

                //重新绑定
                DataTable dt = new DataTable();
                int bytStatus = 2;
                dt = new OrderManager().DL_UnauditedOrderBySel(bytStatus, Session["ConstcCusCode"].ToString() + '%');
                GridOrder.DataSource = dt;
                GridOrder.DataBind();

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('网单:" + strBillNo + ",已经成功生成正式订单！');</script>");
            }
            #endregion

            #region 查看订单状态
            if (Request.QueryString["vbillno"] != null)
            {
                //查看订单状态
                string strBillNo = Request.QueryString["vbillno"].ToString();
                DataTable dt = new DataTable();
                if (strBillNo.Substring(0, 1).ToString() == "D")       //DL和U8的订单查询,DL订单
                {
                    dt = new OrderManager().DL_OrderBillBySel(strBillNo);
                    TxtBiller.Text = dt.Rows[0]["ccusname"].ToString();
                    //BtnModifyOrder.Visible = true;
                    BtnModifyOrder.Visible = false;
                }
                else if (strBillNo.Substring(0, 3).ToString() == "FHD")
                {
                    dt = new OrderManager().DL_U8FHDOrderBillBySel(strBillNo.Replace("FHD",""));
                    TxtBiller.Text = dt.Rows[0]["ccusname"].ToString();
                    BtnModifyOrder.Visible = false;
                }
                else     //DL和U8的订单查询,U8订单
                {
                    dt = new OrderManager().DL_U8OrderBillBySel(strBillNo);
                    TxtBiller.Text = dt.Rows[0]["Biller"].ToString();
                    BtnModifyOrder.Visible = false;
                }
                //绑定表头字段,text
                TxtBillDate.Text = dt.Rows[0]["datCreateTime"].ToString();
                //TxtBiller.Text = dt.Rows[0]["ccusname"].ToString();
                TxtCustomer.Text = dt.Rows[0]["ccusname"].ToString();
                TxtOrderBillNo.Text = strBillNo;
                TxtOrderMark.Text = dt.Rows[0]["strRemarks"].ToString();
                TxtQQ.Text = dt.Rows[0]["strQQName"].ToString();
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
                //绑定表头grid
                DataTable dtgrid = new DataTable();
                int bytStatus = 2;
                dt = new OrderManager().DL_UnauditedOrderBySel(bytStatus, Session["ConstcCusCode"].ToString() + '%');
                GridOrder.DataSource = dt;
                GridOrder.DataBind();

            }
            #endregion
        }
    }

    protected void BtnRefresh_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        int bytStatus = 2;
        dt = new OrderManager().DL_UnauditedOrderBySel(bytStatus, Session["ConstcCusCode"].ToString() + '%');
        GridOrder.DataSource = dt;
        GridOrder.DataBind();
    }

    protected void GridOrder_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
    {
        //DataTable dt = new DataTable();
        //int bytStatus = 1;
        //dt = new OrderManager().DL_UnauditedOrderBySel(bytStatus, Session["ConstcCusCode"].ToString() + '%');
        //GridOrder.DataSource = dt;
        //GridOrder.DataBind();
        //= grid.GetRowValues(e.VisibleIndex, fieldName)
        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('OK');</script>");
        //System.Web.UI.ScriptManager.RegisterStartupScript(this.UpdatePanel2, this.GetType(), "updateScript", "alert('请输入登录名！');", true);
        //throw new Exception("Here I am!"); 
        //string strBillNo = GridOrder.GetRowValues(e.VisibleIndex, "strBillNo").ToString();
        //HFstrBillNo.Value = strBillNo;
        //Response.Redirect("OrderConfirm.aspx");
    }

    //    function EndCallBack(s, e) { 
    //if (s.cpErrorMsg!="") { 
    //alert(s.cpErrorMsg); 
    //} 
    //} 



    protected void BtnModifyOrder_Click(object sender, EventArgs e)
    {
        string strBillNo = Request.QueryString["vbillno"].ToString();
        bool c = new OrderManager().DL_ModifyOrderByUpd(strBillNo);
        if (c)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script>alert('订单" + strBillNo + " 状态修改成功！请到被驳回订单中继续编辑此订单！');{window.location='OrderConfirm.aspx'}</script>");
        }
        else
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script>alert('订单变更失败,请联系管理员！')");
        }
    }
}