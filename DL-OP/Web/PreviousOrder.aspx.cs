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

public partial class PreviousOrder : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        //string begin = "2015-01-01";
        //string end = "2099-01-01";
        //if (DateEditbegin.Value != null)
        //{
        //    begin = DateEditbegin.Date.Date.ToShortDateString();
        //}

        //if (DateEditend.Value != null)
        //{
        //    end = DateEditend.Date.Date.ToShortDateString();
        //}
        //if (Convert.ToDateTime(begin)>Convert.ToDateTime(end))
        //{
        //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('开始日期不能大于截止日期！');</script>");
        //    return;
        //}

        //DataTable dtgrid = new DataTable();
        //dtgrid = new OrderManager().DL_PreviousOrderBySel(Session["ConstcCusCode"].ToString() + '%', begin, end);
        //GridOrder.DataSource = dtgrid;
        //GridOrder.DataBind();


    }
    protected void BtnRefresh_Click(object sender, EventArgs e)
    {
        DateTime aaa = DateTime.Now;
        DateTime bbb = aaa.AddDays(-90);
        string begin = bbb.ToString("yyyy-MM-dd");
        string end = DateTime.Now.ToString("yyyy-MM-dd");
        //begin = "2016/07/01";
        //end = "2016/07/01";
        //begin = begin.ToString("yyyy-MM-dd");
        //end = end.ToString("yyyy-MM-dd");
        if (DateEditbegin.Value != null)
        {
            begin = DateEditbegin.Date.Date.ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            aaa = Convert.ToDateTime(begin);
        }

        if (DateEditend.Value != null)
        {
            end = DateEditend.Date.Date.ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            bbb = Convert.ToDateTime(end);
        }
        if (Convert.ToDateTime(begin) > Convert.ToDateTime(end))
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('开始日期不能大于截止日期！');</script>");
            return;
        }
        TimeSpan ts = bbb.Subtract(aaa);
        if (Convert.ToDouble(ts.TotalDays.ToString()) > 90)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('查询范围不能超过90天！');</script>");
            return;
        }

        DataTable dt = new DataTable();
        if (ComBoType.Value.ToString() == "0")
        {
            //查询已审核订单
            dt = new OrderManager().DL_PreviousOrderBySel(Session["ConstcCusCode"].ToString() + '%', string.Format("{0:G}", begin), string.Format("{0:G}", end));
        }
        else
        {
            //查询作废订单
            dt = new OrderManager().DL_PreviousInvalidOrderBySel(Session["ConstcCusCode"].ToString() + '%', string.Format("{0:G}", begin), string.Format("{0:G}", end));
        }

        GridOrder.DataSource = dt;
        GridOrder.DataBind();
    }
}