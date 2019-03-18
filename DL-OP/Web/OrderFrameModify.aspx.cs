using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Data;
using System.Data.SqlClient;

public partial class OrderFrameModify : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
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
        if (Session["ModifyTxtOrderBillNo"]!= null)
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
       

    }

    protected void ASPxMenu1_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
    {

    }
}