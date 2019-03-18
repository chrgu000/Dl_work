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

public partial class dluser_UseSOA : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //this.DivSOA.Style.Add("display", "block");
        HFccusname.Value = Txtccuscode.Text;   //输入的顾客编码赋值给隐藏字段
        //查询
        string strComboPeriod = ComboPeriod.Value.ToString();   //账单期间
        string strTxtccuscode = " ";
        if (Txtccuscode.Text != null)
        {
            strTxtccuscode = Txtccuscode.Text;   //账单顾客编号,如为空,则查询全部         
        }
        string strComboCheck = "";
        if (Radio1.Checked)
        {
            strComboCheck = "-1";   //全部
        }
        if (Radio2.Checked)
        {
            strComboCheck = "0";    //已发账单
        }
        if (Radio3.Checked)
        {
            strComboCheck = "1";    //未发账单
        }
        DataTable dt = new SearchManager().DLproc_U8SOASearchOfOperBySel(strTxtccuscode, strComboPeriod, strComboCheck);
        SOAGrid.DataSource = dt;
        SOAGrid.DataBind();
        //SOAGrid.SettingsBehavior.AllowFocusedRow = true;
        //SOAGrid.FocusedRowIndex = -1;
        //SOAGrid.Selection.SelectRow(SOAGrid.FindVisibleIndexByKeyValue("011002")); ;
        //bool xx = SOAGrid.MakeRowVisible("011002");

        #region 快速定位到某一页
        //object oValue = null;
        //string sKeyFieldName = "keyid";
        //string sKeyFieldValue = "020104017";
        //for (int i = 0; i < SOAGrid.VisibleRowCount; i++)
        //{
        //    oValue = SOAGrid.GetRowValues(i, sKeyFieldName);
        //    if (oValue != null && oValue.ToString() == sKeyFieldValue)
        //    {
        //        SOAGrid.SettingsBehavior.AllowFocusedRow = false;
        //        SOAGrid.SettingsBehavior.AllowFocusedRow = true;
        //        SOAGrid.FocusedRowIndex = -1;
        //        SOAGrid.Selection.SelectRow(i);
        //        bool bBackToPage = SOAGrid.MakeRowVisible(sKeyFieldValue);// 可以定位回到某一页去  
        //        SOAGrid.FocusedRowIndex = i;
        //        break;
        //    }
        //}
        #endregion
        




    }
    protected void BtnSOA_Click(object sender, EventArgs e)     //生成账单
    {
        #region 检测输入内容
        if (Txtccuscode1.Text != null)
        {

        }
        else
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('请输入顾客编号！');</script>");
            return;
        }
        if (DateEdit1.Value != null)
        {

        }
        else
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('请输入账单截至日期！');</script>");
            return;
        }
        #endregion

        #region 生成账单
        string cus = Txtccuscode1.Text;
        string ddate = DateEdit1.Date.Date.ToShortDateString();
        DataTable dt = new BasicInfoManager().DLproc_U8SOABySel(cus, ddate);
        if (dt.Rows.Count > 0)
        {
            Lbdate.Text = DateEdit1.Date.Year.ToString() + "年" + DateEdit1.Date.Month.ToString() + "月" + DateEdit1.Date.Day.ToString() + "日";
            Lbmoney.Text = Convert.ToDouble(dt.Rows[0]["QK"].ToString()).ToString("0.00");
            string RMB = new EcanRMB().CmycurD(Convert.ToDouble(dt.Rows[0]["QK"].ToString()).ToString("0.00"));
            Lbmoneyup.Text = RMB;
            HFccuscode.Value = Txtccuscode1.Text;
            HFccusname.Value = dt.Rows[0]["MX1"].ToString();
            HFdblAmount.Value = Convert.ToDouble(dt.Rows[0]["QK"].ToString()).ToString("0.00");
            HFstrUper.Value = RMB;
            HFstrEndDate.Value = DateEdit1.Date.Date.ToShortDateString();
            return;
        }
        else
        {
            HFccuscode.Value = "";
            HFccuscode.Value = "";
            Lbdate.Text = DateEdit1.Date.Date.ToShortDateString();
            Lbmoney.Text = "无";
            Lbmoneyup.Text = "无";
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('暂无此客户的账款信息！');</script>");
            return;
        }
        //调用EcanRMB类
        //EcanRMB rmb = new EcanRMB();
        //MessageBox.Show(rmb.CmycurD("4568589647.34"));

        #endregion

    }
    protected void BtnSend_Click(object sender, EventArgs e)        //发送账单
    {
        //发送对账单
        string ccuscode = HFccuscode.Value.ToString();
        string ccusname = HFccusname.Value.ToString();
        string strEndDate = HFstrEndDate.Value.ToString();
        double dblAmount = Convert.ToDouble(HFdblAmount.Value.ToString());
        string strUper = HFstrUper.Value.ToString();
        string strOper = Session["lngopUserId"].ToString();
        string strOperName = Session["strUserName"].ToString();
        int intPeriod = Convert.ToInt16(DateEdit1.Date.Month.ToString());
        bool c = new OrderManager().DL_NewSOAByIns(ccuscode, ccusname, strEndDate, dblAmount, strUper, strOper, strOperName, intPeriod);
        if (c)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('对账单发送成功！');</script>");
        }
        else
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('发送失败，请联系系统管理员！');</script>");
        }

    }
    protected void BtnOk_Click(object sender, EventArgs e)      //操作员查询账单情况
    {
        //this.DivSOA.Style.Add("display", "block");
        HFccusname.Value = Txtccuscode.Text;   //输入的顾客编码赋值给隐藏字段
        //查询
        string strComboPeriod = ComboPeriod.Value.ToString();   //账单期间
        string strTxtccuscode = " ";
        if (Txtccuscode.Text != null)
        {
            strTxtccuscode = Txtccuscode.Text;   //账单顾客编号,如为空,则查询全部         
        }
        string strComboCheck = "";
        if (Radio1.Checked)
        {
            strComboCheck = "-1";   //全部
        }
        if (Radio2.Checked)
        {
            strComboCheck = "0";    //已发账单
        }
        if (Radio3.Checked)
        {
            strComboCheck = "1";    //未发账单
        }
        DataTable dt = new SearchManager().DLproc_U8SOASearchOfOperBySel(strTxtccuscode, strComboPeriod, strComboCheck);
        SOAGrid.DataSource = dt;
        SOAGrid.DataBind();
        //lbtest.Text = HFComboccusname.Value.ToString();
    }

    protected void BtnSoaExp_Click(object sender, EventArgs e)
    {
        DataTable dt = new SearchManager().DLproc_U8SOASearchOfOperBySel(" ", "0", "-1");
        SOAGrid.DataSource = dt;
        SOAGrid.DataBind();
        //gridExport.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
        gridExport.WriteXlsxToResponse(new XlsxExportOptionsEx { ExportType = ExportType.WYSIWYG });
    }

}