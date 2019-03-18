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


public partial class dluser_ActivityContent : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //重新绑定Grid
        DataTable dt = new OrderManager().DL_opSysSalesPolicy_LimitedSupplyBySel();
        GV_170511.DataSource = dt;
        GV_170511.DataBind();
    }

    protected void BtnHDNR_Click(object sender, EventArgs e)
    {
        GV_170511.DataSource = new OrderManager().DL_opSysSalesPolicy_LimitedSupplyBySel();
        GV_170511.DataBind();
    }
    protected void GV_170511_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
    {
        string NewiQuantity = e.NewValues["NewiQuantity"].ToString(); //获取新值
        //if (NewSendDate=="0")
        //{
        //    NewSendDate = "null";
        //}
        string cInvCode = e.NewValues["cInvCode"].ToString();
        bool c = new BasicInfoManager().DL_HDNRByUpd(cInvCode, NewiQuantity);

        e.Cancel = true;
        //重新绑定Grid
        DataTable dt = new OrderManager().DL_opSysSalesPolicy_LimitedSupplyBySel();
        GV_170511.DataSource = dt;
        GV_170511.DataBind();
    }

    protected void BtnSoaExp_Click(object sender, EventArgs e)
    {
        DataTable dt = new OrderManager().DL_opSysSalesPolicy_LimitedSupplyBySel();
        GV_170511.DataSource = dt;
        GV_170511.DataBind();
        //gridExport.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
        gridExport.WriteXlsxToResponse(new XlsxExportOptionsEx { ExportType = ExportType.WYSIWYG });
    }
}