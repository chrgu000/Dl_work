using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.IO;
using System.Text;
using BLL;
using System.Configuration;
using DevExpress.XtraPrinting;
using DevExpress.Export;

public partial class dluser_SOASetting : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DataTable dt = new BasicInfoManager().DL_SOAAutoSendBySel();
        SOAGrid.DataSource = dt;
        SOAGrid.DataBind();
    }

    protected void SOAGrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
    {

    }

    protected void SOAGrid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
    {
        string ccdefine1 = e.NewValues["NewSendDate"].ToString(); //获取新值
        //if (NewSendDate=="0")
        //{
        //    NewSendDate = "null";
        //}
        string cCusCode = e.NewValues["cCusCode"].ToString();
        bool c = new BasicInfoManager().DL_SOAAutoSendByUpd(cCusCode, ccdefine1);

        e.Cancel = true;
        //重新绑定Grid
        DataTable dt = new BasicInfoManager().DL_SOAAutoSendBySel();
        SOAGrid.DataSource = dt;
        SOAGrid.DataBind();
    }

    protected void SOAGrid_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
    {

    }

    protected void SOAGrid_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
    {
        if (e.DataColumn.FieldName.ToString() == "NewSendDate" && e.VisibleIndex >= 0)
        {
            e.Cell.Text = SOAGrid.GetRowValues(e.VisibleIndex, "SOASendTime").ToString();
        }

    }

    protected void BtnSendSOA_Click(object sender, EventArgs e)
    {
        //SOAGrid.Selection.SelectRowByKey("010101");
        //SOAGrid.Selection.SetSelectionByKey("01010101", true);
        //SOAGrid.Selection.SetSelectionByKey("0101010101", true);
        //SOAGrid.Selection.SetSelectionByKey("010101010102", true);
        int sendok = 0;
        int sendfalse = 0;
        for (int i = 0; i < SOAGrid.VisibleRowCount; i++)
        {
            if (SOAGrid.Selection.IsRowSelected(i) && Convert.ToDouble(SOAGrid.GetRowValues(i, "SOASendTime").ToString()) > 0)
            {
                //查询账单信息
                string cus = SOAGrid.GetRowValues(i, "cCusCode").ToString();
                string ddate = SOAGrid.GetRowValues(i, "SOASendTime").ToString();
                DataTable dt = new BasicInfoManager().DLproc_U8SOAForDateBySel(cus, ddate);
                if (dt.Rows.Count > 0)
                {
                    string RMB = new EcanRMB().CmycurD(Convert.ToDouble(dt.Rows[0]["QK"].ToString()).ToString("0.00"));
                    string Lbmoneyup = RMB;
                    string HFccuscode = cus;
                    string HFccusname = dt.Rows[0]["MX1"].ToString();
                    string HFdblAmount = Convert.ToDouble(dt.Rows[0]["QK"].ToString()).ToString("0.00");
                    string HFstrUper = RMB;
                    if (Convert.ToDouble(dt.Rows[0]["QK"].ToString()) < 0)
                    {
                        HFstrUper = "负" + HFstrUper;
                    }
                    string HFstrEndDate = dt.Rows[0]["ddate"].ToString();
                    HFstrEndDate = HFstrEndDate + " 23:59:59";
                    //生成账单信息,发送给顾客
                    string ccuscode = HFccuscode;
                    string ccusname = HFccusname;
                    string strEndDate = HFstrEndDate;
                    double dblAmount = Convert.ToDouble(HFdblAmount);
                    string strUper = HFstrUper;
                    string strOper = Session["lngopUserId"].ToString();
                    string strOperName = Session["strUserName"].ToString();
                    int intPeriod = Convert.ToInt16(dt.Rows[0]["mm"].ToString());
                    bool c = new OrderManager().DL_NewSOAByIns(ccuscode, ccusname, strEndDate, dblAmount, strUper, strOper, strOperName, intPeriod);
                    if (c)
                    {
                        sendok = sendok + 1;
                    }
                    else
                    {
                        sendfalse = sendfalse + 1;
                    }
                }
            }
        }
        Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('账单发送执行完毕!总计发送成功" + sendok + "条,失败:" + sendfalse + "条！');</script>");
    }
    protected void BtnSoaExp_Click(object sender, EventArgs e)
    {
        DataTable dt = new SearchManager().DL_U8SOASearchOfOperBySel(ComboPeriodYear_exp.Text, ComboPeriodMon_exp.Text);
        SOAGrid_exp.DataSource = dt;
        SOAGrid_exp.DataBind();
        //gridExport.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
        gridExport.WriteXlsxToResponse(new XlsxExportOptionsEx { ExportType = ExportType.WYSIWYG });
    }
}