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

public partial class dluser_OrderFrameDlUser : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        #region 清除session
        //清除OrderModify.aspx中的送货地址信息
        if (Session["ModifyTxtOrderShippingMethod"] != null)
        {
            Session.Contents.Remove("ModifyTxtOrderShippingMethod");
        }
        //清除OrderModify.aspx中的订单表体信息
        if (Session["ModifyOrderGrid"] != null)
        {
            Session.Contents.Remove("ModifyOrderGrid");
        }
        //清除OrderModify.aspx中的开票单位信息
        if (Session["ModifyTxtCustomer"] != null)
        {
            Session.Contents.Remove("ModifyTxtCustomer");
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
        if (Session["ModifycCusPPerson"] != null)
        {
            Session.Contents.Remove("ModifyTxtcSCCode");
        }
        //清除OrderModify.aspx中的备注 
        if (Session["ModifyTxtOrderMark"] != null)
        {
            Session.Contents.Remove("ModifyTxtOrderMark");
        }
        //清除OrderModify.aspx中的编号 
        if (Session["ModifyTxtOrderBillNo"] != null)
        {
            Session.Contents.Remove("ModifyTxtOrderBillNo");
        }
        //清除OrderModify.aspx中的车型 
        if (Session["ModifyTxtcdefine3"] != null)
        {
            Session.Contents.Remove("ModifyTxtcdefine3");
        }
        //清除OrderModify.aspx中的备注 
        if (Session["ModifyTxtOrderMark"] != null)
        {
            Session.Contents.Remove("ModifyTxtOrderMark");
        }
        //清除OrderModify.aspx中的装车方式 
        if (Session["ModifyTxtLoadingWays"] != null)
        {
            Session.Contents.Remove("ModifyTxtLoadingWays");
        }
        //清除OrderModify.aspx中的提货时间 
        if (Session["ModifyDeliveryDate"] != null)
        {
            Session.Contents.Remove("ModifyDeliveryDate");
        }
        //清除OrderModify.aspx中的下单时间 
        if (Session["ModifyTxtBillTime"] != null)
        {
            Session.Contents.Remove("ModifyTxtBillTime");
        }
        #endregion

        Session["OrderFrameDlUser"] = Request.QueryString["billno"].ToString();//billno,DL网单号
        DataTable dt = new OrderManager().DL_OrderBillBySel(Request.QueryString["billno"].ToString());
        Session["ModifyKPDWcCusCode"] = dt.Rows[0]["ccuscode1"].ToString();//开票单位编码
        Session["ModifylngopUserId"] = dt.Rows[0]["lngopUserId"].ToString();//制单人dl代码,userid
        Session["ModifycCusCode"] = dt.Rows[0]["cCusCode"].ToString();//制单人u8代码,顾客登录编码
        Session["ModifyTxtOrderShippingMethod"] = dt.Rows[0]["cdefine11"].ToString();//送货地址方式
        Session["ModifyTxtCustomer"] = dt.Rows[0]["ccusname"].ToString();//开票单位名称
        Session["ModifycCusPPerson"] = dt.Rows[0]["cpersoncode"].ToString();//业务员
        Session["ModifystrUserName"] = dt.Rows[0]["strUserName"].ToString();//制单人名称
        Session["ModifydatBillTime"] = dt.Rows[0]["datBillTime"].ToString();//下单时间

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

        //获取对应订单的类型
        DataTable dturl = new DataTable();
        dturl = new SearchManager().DL_BillTypeBySel(Request.QueryString["billno"].ToString());
        Session["SampleOrderModify_StrBillNo"] = Request.QueryString["billno"].ToString();
        Session["OrderYModify_StrBillNo"] = Request.QueryString["billno"].ToString();
        switch (dturl.Rows[0]["BillType"].ToString())
        {
            case "普通订单":

                break;
            case "样品订单":

                Response.Redirect("SampleOrderModify.aspx");
                break;
            case "酬宾订单":

                Response.Redirect("OrderYModify.aspx");
                break;
            case "特殊订单":
                Session["OrderXModify_StrBillNo"] = Request.QueryString["billno"].ToString();
                Response.Redirect("OrderXModify.aspx");
                break;
        }

    }
    protected void ASPxMenu1_ItemClick(object source, MenuItemEventArgs e)
    {

    }
}