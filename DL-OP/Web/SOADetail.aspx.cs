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


public partial class SOADetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DataTable DtComboCustomer = new DataTable();
        DtComboCustomer = new SearchManager().DL_ComboCustomerAllBySel(Session["ConstcCusCode"].ToString() + "%");
        //ComboBoxccuscode.DataSource = DtComboCustomer;
        //ComboBoxccuscode.DataBind();
        for (int i = 0; i < DtComboCustomer.Rows.Count; i++)
        {
            ComboBoxccuscode.Items.Add(DtComboCustomer.Rows[i]["cCusName"].ToString(), DtComboCustomer.Rows[i]["cCusCode"].ToString());
        }
    }

    protected void BtnOk_Click(object sender, EventArgs e)
    {
        string begin = "2015-12-01";
        string end = "2099-01-01";
        if (DateEditbegin.Value != null)
        {
            begin = DateEditbegin.Date.Date.ToShortDateString();
        }

        if (DateEditend.Value != null)
        {
            end = DateEditend.Date.Date.ToShortDateString();
        }
        if (Convert.ToDateTime(begin) > Convert.ToDateTime(end))
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('开始日期不能大于截止日期！');</script>");
            return;
        }
        if (ComboBoxccuscode.SelectedItem == null)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('请选择单位！');</script>");
            return;
        }
        DataTable dt = new SearchManager().DLproc_SOADetailforCustomerBySel(begin, end, ComboBoxccuscode.SelectedItem.Value.ToString());
        SOAGrid.DataSource = dt;
        SOAGrid.DataBind();
    }

    protected void BtnToXlsx_Click(object sender, EventArgs e)
    {
        string begin = "2015-12-01";
        string end = "2099-01-01";
        if (DateEditbegin.Value != null)
        {
            begin = DateEditbegin.Date.Date.ToShortDateString();
        }

        if (DateEditend.Value != null)
        {
            end = DateEditend.Date.Date.ToShortDateString();
        }
        if (Convert.ToDateTime(begin) > Convert.ToDateTime(end))
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('开始日期不能大于截止日期！');</script>");
            return;
        }
        if (ComboBoxccuscode.SelectedItem == null)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('请选择单位！');</script>");
            return;
        }
        DataTable dt = new SearchManager().DLproc_SOADetailforCustomerBySel(begin, end, ComboBoxccuscode.SelectedItem.Value.ToString());
        SOAGrid.DataSource = dt;
        SOAGrid.DataBind();

        //gridExport.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
        gridExport.WriteXlsxToResponse(new XlsxExportOptionsEx { ExportType = ExportType.WYSIWYG });
    }

    protected void SOAGrid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
    {
        //if (e.RowType != DevExpress.Web.ASPxGridView.) return;
        if (e.GetValue("vtype") != null && e.GetValue("vtype").ToString() == "销售普通发票")
        {
            //e.Row.Cells[11].Style.Add("color", "Red");
            //e.Row.Style.Add("CellStyle-BackColor", "Red");
            e.Row.BackColor = System.Drawing.Color.YellowGreen;
        }
        if (e.GetValue("vtype") != null && e.GetValue("vtype").ToString() != "销售普通发票")
        {
            e.Row.BackColor = System.Drawing.Color.Moccasin;
        }
        if (e.GetValue("cOrderNo") != null && e.GetValue("cOrderNo").ToString() == "期初余额")
        {
            e.Row.BackColor = System.Drawing.Color.Red;
        }
    }

    protected void SAOGrid_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
    {
        //if (e.DataColumn.ToString() == "应付余额" && e.VisibleIndex == 0)
        //{
        //    Session["ye"] = e.CellValue;
        //    e.Cell.Text = e.CellValue.ToString();
        //}
        //if (e.DataColumn.ToString() == "应付余额" && e.VisibleIndex != 0)
        //{
        //    //e.CellValue = "1";
        //    e.Cell.Text = (Convert.ToDouble(Session["ye"].ToString()) + Convert.ToDouble(e.CellValue.ToString())).ToString();
        //    Session["ye"] = e.Cell.Text;
        //}
    }



    protected void SOAGrid_CustomSummaryCalculate(object sender, CustomSummaryEventArgs e)
    {


        switch (e.SummaryProcess)
        {
            case CustomSummaryProcess.Start:
                break;

            case CustomSummaryProcess.Calculate:
                break;

            case CustomSummaryProcess.Finalize:

                if (e.IsTotalSummary)
                {
                    //if (groupByItemWeightFields.Contains(fieldName))
                    //{
                    //    e.TotalValue = _itemSums.Sum(x => (decimal)x.Value[fieldName]);
                    //}
                    //else
                    //{
                    //    e.TotalValue = _itemSums.Sum(x => (int)x.Value[fieldName]);
                    //}
                    //SOAGrid.VisibleRowCount
                    if (Convert.ToInt16(SOAGrid.VisibleRowCount.ToString()) > 0)
                    {
                        string sf = SOAGrid.GetRowValues(Convert.ToInt16(SOAGrid.VisibleRowCount.ToString())-1, "ye").ToString();
                        e.TotalValue = sf;
                    }
                    else
                    {
                        e.TotalValue = "";
                    }

                }
                else if (e.IsGroupSummary)
                {
                    //if (isGroupByItemOnly)
                    //{
                    //    object id = view.GetRowCellValue(e.RowHandle, colItemID);
                    //    if (id == null) return;
                    //    itemId = (int)id;
                    //    if (!_itemSums.ContainsKey(itemId)) return;

                    //    e.TotalValue = _itemSums[itemId][fieldName];
                    //}
                }

                break;

            default:
                break;
        }
    }
}