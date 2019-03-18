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
using System.Collections;

public partial class PreXOrderDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        #region 查看订单状态
        if (Request.QueryString["id"] != null)
        {
            //查看订单状态
            string strBillNo = Request.QueryString["id"].ToString();
            DataTable dt = new OrderManager().DL_XOrderBillDetailBySel(strBillNo);
            //绑定表头字段,
            TxtBillNo.Text = strBillNo;
            TxtKPDW.Text = dt.Rows[0]["ccusname"].ToString();
            TxtDDate.Text = dt.Rows[0]["datBillTime"].ToString();
            //绑定表体字段,grid
            XOrderGrid.DataSource = dt;
            XOrderGrid.DataBind();
        }
        #endregion
    }

    protected void XOrderGrid_Init(object sender, EventArgs e)   //设置订单明细表初始化,显示报价金额.执行金额
    {
        //初始化,设置金额合计启用报价合计还是执行价合计?
        Hashtable ht = (Hashtable)Session["SysSetting"];
        //    if (ht.Contains("IsExercisePrice"))
        //{

        //}
        if (ht["IsExercisePrice"].ToString() == "0")   //报价金额
        {
            XOrderGrid.TotalSummary[1].FieldName = "cComUnitAmount";
        }
        else//执行价金额
        {
            XOrderGrid.TotalSummary[1].FieldName = "xx";
        }
    }

}