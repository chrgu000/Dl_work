using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Model;
using System.Data;
using System.Data.SqlClient;
using DevExpress.Export;
using DevExpress.XtraPrinting;
using DevExpress.Data;
using DevExpress.Web;

public partial class OrderExecute : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        //DataTable DtComboCustomer = new DataTable();
        //DtComboCustomer = new SearchManager().DL_ComboCustomerAllBySel(Session["ConstcCusCode"].ToString() + "%");
        //ComboBoxccuscode.Items.Add("全部", Session["ConstcCusCode"].ToString()+"%");
        //ComboBoxccuscode.SelectedIndex = 0;
        //for (int i = 0; i < DtComboCustomer.Rows.Count; i++)
        //{
        //    ComboBoxccuscode.Items.Add(DtComboCustomer.Rows[i]["cCusName"].ToString(), DtComboCustomer.Rows[i]["cCusCode"].ToString());
        //}

        if (ASPxSpinEdit1.Value != null)
        {
            int aa = Convert.ToInt32(ASPxSpinEdit1.Value.ToString());
            OrderExcute.Settings.VerticalScrollableHeight = aa;
        }



    }
    protected void BtnS_Click(object sender, EventArgs e)
    {
        OrderExcute.DataSource = null;
        OrderExcute.DataBind();
        string begin = "2016-01-01 00:00:00";
        string end = "2099-12-31 23:59:00";
        string strshowtype = showtype.Value.ToString();
        string strFHStatus = FHStatus.Value.ToString();
        string strBillNo = TxtstrBillNo.Text;
        //string ccuscode = Session["ConstcCusCode"].ToString();
        string ccuscode = ComboBoxccuscode.Value.ToString();
        string user = ComboUSer.Value.ToString();
        if (DateBegin.Value != null)
        {
            //begin = DateBegin.Date.Date.ToShortDateString();
            //begin = DateBegin.Value.ToString();
            begin = DateBegin.Date.Date.ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);

        }

        if (DateEnd.Value != null)
        {
            //end = DateEnd.Date.Date.ToShortDateString();
            //end = DateEnd.Value.ToString();
            end = DateEnd.Date.Date.ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }
        if (Convert.ToDateTime(begin) > Convert.ToDateTime(end))
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('开始日期不能大于截止日期！');</script>");
            return;
        }
        if (ComboTimeType.Value.ToString() == "XDtime")   //下单时间查询
        {
            //绑定数据
            DataTable dt = new SearchManager().DLproc_OrderExecuteBySel(strBillNo, ccuscode, begin, end, strshowtype, strFHStatus, user);
            OrderExcute.DataSource = dt;
            OrderExcute.DataBind();
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('查询完成！');</script>");
            return;
        }
        else            //审核时间查询
        {
            //绑定数据
            DataTable dt = new SearchManager().DLproc_OrderExecuteFordatAuditordTimeBySel(strBillNo, ccuscode, begin, end, strshowtype, strFHStatus, user);
            OrderExcute.DataSource = dt;
            OrderExcute.DataBind();
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('查询完成！');</script>");
            return;
        }

    }
    protected void BtnSXLS_Click(object sender, EventArgs e)
    {
        string begin = "2016-01-01 00:00:00";
        string end = "2099-12-31 23:59:00";
        string strshowtype = showtype.Value.ToString();
        string strFHStatus = FHStatus.Value.ToString();
        string strBillNo = TxtstrBillNo.Text;
        //string ccuscode = Session["ConstcCusCode"].ToString();
        string ccuscode = ComboBoxccuscode.Value.ToString();
        string user = ComboUSer.Value.ToString();
        if (DateBegin.Value != null)
        {
            //begin = DateBegin.Date.Date.ToShortDateString();
            //begin = DateBegin.Value.ToString();
            begin = DateBegin.Date.Date.ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }

        if (DateEnd.Value != null)
        {
            //end = DateEnd.Date.Date.ToShortDateString();
            //end = DateEnd.Value.ToString();
            end = DateEnd.Date.Date.ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }
        if (Convert.ToDateTime(begin) > Convert.ToDateTime(end))
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('开始日期不能大于截止日期！');</script>");
            return;
        }
        if (ComboTimeType.Value.ToString() == "XDtime")   //下单时间查询
        {
            //绑定数据
            DataTable dt = new SearchManager().DLproc_OrderExecuteBySel(strBillNo, ccuscode, begin, end, strshowtype, strFHStatus,user);
            OrderExcute.DataSource = dt;
            OrderExcute.DataBind();
            //导出数据
            //gridExport.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
            gridExport.WriteXlsxToResponse(new XlsxExportOptionsEx { ExportType = ExportType.WYSIWYG });
        }
        else            //审核时间查询
        {
            //绑定数据
            DataTable dt = new SearchManager().DLproc_OrderExecuteFordatAuditordTimeBySel(strBillNo, ccuscode, begin, end, strshowtype, strFHStatus, user);
            OrderExcute.DataSource = dt;
            OrderExcute.DataBind();
            //导出数据
            //gridExport.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
            gridExport.WriteXlsxToResponse(new XlsxExportOptionsEx { ExportType = ExportType.WYSIWYG });
        }

    }
    protected void OrderExcute_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
    {
        //if (e.RowType != DevExpress.Web.ASPxGridView.) return;
        //if (e.GetValue("vtype") != null && e.GetValue("vtype").ToString() == "发货单")
        //{
        //    //e.Row.Cells[11].Style.Add("color", "Red");
        //    //e.Row.Style.Add("CellStyle-BackColor", "Red");
        //    e.Row.BackColor = System.Drawing.Color.YellowGreen;
        //}
        //if (e.GetValue("vtype") != null && e.GetValue("vtype").ToString() != "发货单")
        //{
        //    e.Row.BackColor = System.Drawing.Color.Moccasin;
        //}
        //存储过程中的数据如下:显示颜色.
        // BillStatusName	BillStatus
        //'未发货':10 #CC33FF 紫色 MediumPurple
        //'已发完,无退货' :20 #33FF66 绿色 MediumSpringGreen
        //'已发完,有退货' :30 #FFCC33 黄色 #FF6633 橘黄 Orange
        //'未发完,无退货' :40 #008AB8 浅蓝 RoyalBlue
        //'未发完,有退货' :50 #B88A00 土黄 DarkKhaki
        //'无提货清单退货':60 #FF3366 红色 Crimson
        //'未发货':10
        if (e.GetValue("BillStatusName") != null && Convert.ToDouble(e.GetValue("BillStatusName")) == 10)
        {
            e.Row.BackColor = System.Drawing.Color.MediumPurple;
        }
        //'已发完,无退货' :20
        //if (e.GetValue("BillStatusName") != null && Convert.ToDouble(e.GetValue("BillStatusName")) == 20)
        //{
        //    e.Row.BackColor = System.Drawing.Color.MediumSpringGreen;
        //}
        //'已发完,有退货' :30
        if (e.GetValue("BillStatusName") != null && Convert.ToDouble(e.GetValue("BillStatusName")) == 30)
        {
            e.Row.BackColor = System.Drawing.Color.Orange;
        }
        //'未发完,无退货' :40
        if (e.GetValue("BillStatusName") != null && Convert.ToDouble(e.GetValue("BillStatusName")) == 40)
        {
            e.Row.BackColor = System.Drawing.Color.RoyalBlue;
        }
        //'未发完,有退货' :50
        if (e.GetValue("BillStatusName") != null && Convert.ToDouble(e.GetValue("BillStatusName")) == 50)
        {
            e.Row.BackColor = System.Drawing.Color.DarkKhaki;
        }
        //无提货清单退货,60
        if (e.GetValue("BillStatusName") != null && Convert.ToDouble(e.GetValue("BillStatusName")) == 60)
        {
            e.Row.BackColor = System.Drawing.Color.Crimson;
        }
        //单笔订单合计
        if (e.GetValue("cinvname") != null && e.GetValue("cinvname").ToString() == "合计")
        {
            e.Row.BackColor = System.Drawing.Color.Thistle;
        }
    }
    protected void ComboBoxccuscode_Init(object sender, EventArgs e)
    {
        //绑定客户
        DataTable DtComboCustomer = new DataTable();
        DtComboCustomer = new SearchManager().DL_ComboCustomerAllBySel(Session["ConstcCusCode"].ToString() + "%");
        ComboBoxccuscode.Items.Add("全部", Session["ConstcCusCode"].ToString() + "%");
        ComboBoxccuscode.SelectedIndex = 0;
        for (int i = 0; i < DtComboCustomer.Rows.Count; i++)
        {
            ComboBoxccuscode.Items.Add(DtComboCustomer.Rows[i]["cCusName"].ToString(), DtComboCustomer.Rows[i]["cCusCode"].ToString());
        }
    }
    protected void ASPxButton1_Click(object sender, EventArgs e)
    {
        if (ASPxSpinEdit1.Value != null)
        {
            int aa = Convert.ToInt32(ASPxSpinEdit1.Value.ToString());
            OrderExcute.Settings.VerticalScrollableHeight = aa;
        }
    }

    protected void ComboUSer_Init(object sender, EventArgs e)
    {
        //绑定账户
        DataTable DtComboUser = new DataTable();
        DtComboUser = new SearchManager().DL_AllCountBySel(Session["ConstcCusCode"].ToString());
        ComboUSer.Items.Add("全部", "%");
        ComboUSer.SelectedIndex = 0;
        for (int i = 0; i < DtComboUser.Rows.Count; i++)
        {
            ComboUSer.Items.Add(DtComboUser.Rows[i]["strAllAccount"].ToString(), DtComboUser.Rows[i]["cCusCode"].ToString());
        }
    }
}