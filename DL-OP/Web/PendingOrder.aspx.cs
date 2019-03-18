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

public partial class PendingOrder : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        #region 清除session//切换会本页时清空OrderModify.aspx中的Session["ModifyOrderGrid"]的内容;
        //1,清除普通订单
        //切换会本页时清空OrderModify.aspx中的Session["ModifyOrderGrid"]的内容;
        if (Session["ModifyOrderGrid"] != null)
        {
            Session.Contents.Remove("ModifyOrderGrid");
        }
        //清除OrderModify.aspx中的送货地址信息
        if (Session["ModifyTxtOrderShippingMethod"] != null)
        {
            Session.Contents.Remove("ModifyTxtOrderShippingMethod");
        }
        //清除OrderModify.aspx中的订单表体信息
        if (Session["Modifyordergrid"] != null)
        {
            Session.Contents.Remove("Modifyordergrid");
        }
        //清除OrderModify.aspx中的开票单位名称信息
        if (Session["ModifyTxtCustomer"] != null)
        {
            Session.Contents.Remove("ModifyTxtCustomer");
        }
        //清除OrderModify.aspx中的开票单位代码信息
        if (Session["ModifyKPDWcCusCode"] != null)
        {
            Session.Contents.Remove("ModifyKPDWcCusCode");
        }
        //清除OrderModify.aspx中的业务员信息
        if (Session["ModifycCusPPerson"] != null)
        {
            Session.Contents.Remove("ModifycCusPPerson");
        }
        //清除OrderModify.aspx中的开票单位编码信息
        if (Session["ModifycCusPPerson"] != null)
        {
            Session.Contents.Remove("ModifyKPDWcCusCode");
        }
        //清除OrderModify.aspx中的发运方式编码
        if (Session["ModifyTxtcSCCode"] != null)
        {
            Session.Contents.Remove("ModifyTxtcSCCode");
        }
        //清除OrderModify.aspx中的车型信息
        if (Session["Modifycdefine3"] != null)
        {
            Session.Contents.Remove("Modifycdefine3");
        }
        //清除OrderModify.aspx中的销售类型
        Session["ModifycSTCode"] = "00";
        //获取客户端的Cookie对象,备注
        HttpCookie ModifyTxtOrderMark = Request.Cookies["ModifyTxtOrderMark"];
        if (ModifyTxtOrderMark != null)
        {
            TimeSpan ts = new TimeSpan(-1, 0, 0, 0);
            ModifyTxtOrderMark.Expires = DateTime.Now.Add(ts);//删除整个Cookie，只要把过期时间设置为现在
            Response.AppendCookie(ModifyTxtOrderMark);
        }
        //获取客户端的Cookie对象,装车方式
        HttpCookie ModifyTxtLoadingWays = Request.Cookies["ModifyTxtLoadingWays"];
        if (ModifyTxtLoadingWays != null)
        {
            TimeSpan ts = new TimeSpan(-1, 0, 0, 0);
            ModifyTxtLoadingWays.Expires = DateTime.Now.Add(ts);//删除整个Cookie，只要把过期时间设置为现在
            Response.AppendCookie(ModifyTxtLoadingWays);
        }
        //获取客户端的Cookie对象,交货日期
        HttpCookie ModifyDeliveryDate = Request.Cookies["ModifyDeliveryDate"];
        if (ModifyDeliveryDate != null)
        {
            TimeSpan ts = new TimeSpan(-1, 0, 0, 0);
            ModifyDeliveryDate.Expires = DateTime.Now.Add(ts);//删除整个Cookie，只要把过期时间设置为现在
            Response.AppendCookie(ModifyDeliveryDate);
        }

        //2,样品资料session
        //清除session
        Session.Remove("SampleOrderModify_StrBillNo");  //单据编号
        Session.Remove("SampleOrderModify_ordergrid");  //表体明细数据
        Session.Remove("SampleOrderModify_gridselect"); //选择的商品明细数据
        Session.Remove("SampleOrderModify_KPDWcode");   //开票单位编码
        Session.Remove("SampleOrderModify_treelistgrid");   //选择树的大类编码

        //3,清除酬宾订单session
        Session.Remove("OrderYModify_srtBillNo");  //单据编号
        Session.Remove("OrderYModify_HT");  //酬宾订单表头信息
        Session.Remove("OrderYModify_KPDWcCusCode");  //酬宾订单表头开票单位编码
        Session.Remove("OrderYModify_TreeListGrid");  //酬宾订单树节点选择信息
        Session.Remove("OrderYModify_OrderGrid");  //酬宾订单表体明细数据
        Session.Remove("OrderYModify_GridSelect");  //酬宾订单商品选择信息
        Session.Remove("OrderYModify_lngopUseraddressId");  //酬宾订单发货方式id



        #endregion


        if (!IsPostBack)
        {
            if (Request.QueryString["vbillno"] != null) //查看订单
            {
                EditOrder.Visible = true;
                //查看订单状态
                string strBillNo = Request.QueryString["vbillno"].ToString();
                DataTable dt = new OrderManager().DL_OrderBillBySel(strBillNo);
                //绑定表头字段,text
                TxtBillDate.Text = dt.Rows[0]["datCreateTime"].ToString();
                TxtBiller.Text = dt.Rows[0]["ccusname"].ToString();
                TxtCustomer.Text = dt.Rows[0]["ccusname"].ToString();
                TxtOrderBillNo.Text = strBillNo;
                TxtOrderMark.Text = dt.Rows[0]["strRemarks"].ToString();
                Session["ModifyKPDWcCusCode"] = dt.Rows[0]["ccuscode1"].ToString();
                HFlngBillType.Value = dt.Rows[0]["lngBillType"].ToString();
                TxtQQ.Text = dt.Rows[0]["strQQName"].ToString();
                //销售类型,用于生成物料树
                Session["ModifycSTCode"] = dt.Rows[0]["cSTCode"].ToString();

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
                int bytStatus = 3;
                //dtgrid = new OrderManager().DL_UnauditedOrderBySel(bytStatus, Session["ConstcCusCode"].ToString() + '%');
                dtgrid = new OrderManager().DL_UnauditedOrder_SubBySel(bytStatus, Session["lngopUserId"].ToString(), Session["lngopUserExId"].ToString());
                GridOrder.DataSource = dtgrid;
                GridOrder.DataBind();
                //绑定
                //EditOrder.NavigateUrl = "~/OrderModify.aspx?billno=" + strBillNo+"&ourl="+Request.RawUrl;
                //传递 url 参数
                switch (HFlngBillType.Value)
                {
                    case "0":
                        if ("00" == Session["ModifycSTCode"].ToString())   //普通订单
                        {
                            EditOrder.NavigateUrl = "OrderFrameModify.aspx";
                            Session["OrderFrameModifyURL"] = strBillNo;
                        }
                        else    //样品资料
                        {
                            EditOrder.NavigateUrl = "SampleOrderModify.aspx";
                            Session["SampleOrderModify_StrBillNo"] = strBillNo;
                        }
                        break;
                    case "1":   //参照酬宾订单
                        EditOrder.NavigateUrl = "OrderYModify.aspx";
                        Session["OrderYModify_srtBillNo"] = strBillNo;
                        break;
                    case "2":   //参照特殊订单
                        EditOrder.NavigateUrl = "OrderXModify.aspx";
                        Session["OrderXModify_srtBillNo"] = strBillNo;
                        break;
                    //default:

                    // break;
                }

            }
            else
            {
                //绑定grid
                DataTable dtgrid = new DataTable();
                int bytStatus = 3;
                //dtgrid = new OrderManager().DL_UnauditedOrderBySel(bytStatus, Session["ConstcCusCode"].ToString() + '%');
                dtgrid = new OrderManager().DL_UnauditedOrder_SubBySel(bytStatus, Session["lngopUserId"].ToString(), Session["lngopUserExId"].ToString());
                GridOrder.DataSource = dtgrid;
                GridOrder.DataBind();
            }
        }
        else
        {

        }
    }


    protected void BtnRefresh_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        int bytStatus = 3;
        //dt = new OrderManager().DL_UnauditedOrderBySel(bytStatus, Session["ConstcCusCode"].ToString() + '%');
        dt = new OrderManager().DL_UnauditedOrder_SubBySel(bytStatus, Session["lngopUserId"].ToString(), Session["lngopUserExId"].ToString());
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