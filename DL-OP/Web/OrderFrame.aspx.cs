using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Data;
using System.Data.SqlClient;

public partial class OrderFrame : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["cSTCode"] = "00";
        }
    }

    protected void ASPxMenu1_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
    {
        Session.Remove("Ytreelistgrid");
        Session.Remove("Yordergrid");
        Session.Remove("Order_Y_gridselect");
        Session.Remove("TxtRelateU8NO");
        Session.Remove("Sampleordergrid");
        //TxtOrderShippingMethod.Text = "";
        //TxtcSCCode.Text = "";
        //Txtcdefine3.Text = "";

        //清除order.aspx中的送货地址信息
        if (Session["TxtOrderShippingMethod"] != null)
        {
            Session.Contents.Remove("TxtOrderShippingMethod");
        }
        //清除order.aspx中的订单表体信息
        if (Session["ordergrid"] != null)
        {
            Session.Contents.Remove("ordergrid");
        }
        //清除order.aspx中的物料选择
        if (Session["gridselect"] != null)
        {
            Session.Contents.Remove("ordergrid");
        }
        //清除order.aspx中的开票单位信息
        if (Session["TxtCustomer"] != null)
        {
            Session.Contents.Remove("TxtCustomer");
        }

        //清除order.aspx中的发运方式编码
        if (Session["TxtcSCCode"] != null)
        {
            Session.Contents.Remove("TxtcSCCode");
        }
        //清除order.aspx中的车型信息
        if (Session["cdefine3"] != null)
        {
            Session.Contents.Remove("cdefine3");
        }
        //清除order.aspx中的销售类型
        Session["cSTCode"] = 0;

        //获取客户端的Cookie对象,备注
        HttpCookie TxtOrderMark = Request.Cookies["TxtOrderMark"];
        if (TxtOrderMark != null)
        {
            TimeSpan ts = new TimeSpan(-1, 0, 0, 0);
            TxtOrderMark.Expires = DateTime.Now.Add(ts);//删除整个Cookie，只要把过期时间设置为现在
            Response.AppendCookie(TxtOrderMark);
        }

        //获取客户端的Cookie对象,装车方式
        HttpCookie TxtLoadingWays = Request.Cookies["TxtLoadingWays"];
        if (TxtLoadingWays != null)
        {
            TimeSpan ts = new TimeSpan(-1, 0, 0, 0);
            TxtLoadingWays.Expires = DateTime.Now.Add(ts);//删除整个Cookie，只要把过期时间设置为现在
            Response.AppendCookie(TxtLoadingWays);
        }

        //获取客户端的Cookie对象,交货日期
        HttpCookie DeliveryDate = Request.Cookies["DeliveryDate"];
        if (DeliveryDate != null)
        {
            TimeSpan ts = new TimeSpan(-1, 0, 0, 0);
            DeliveryDate.Expires = DateTime.Now.Add(ts);//删除整个Cookie，只要把过期时间设置为现在
            Response.AppendCookie(DeliveryDate);
        }

        #region 新增普通订单
        if (e.Item.Text == "新增普通订单")
        {
            Response.Redirect("OrderFrame.aspx");
        }
        #endregion

        #region 新增样品资料订单
        if (e.Item.Text == "新增样品资料订单")
        {
            Response.Write("<script>window.open('SampleOrder.aspx','_self')</script>");
        }
        #endregion

        #region 参照酬宾订单
        if (e.Item.Text == "参照酬宾订单")
        {
            Response.Write("<script>window.open('Order_Y.aspx','_self')</script>");
        }
        #endregion

        #region 参照特殊订单
        if (e.Item.Text == "参照特殊订单")
        {
            Response.Write("<script>window.open('Order_X.aspx','_self')</script>");
        }
        #endregion

    }
}