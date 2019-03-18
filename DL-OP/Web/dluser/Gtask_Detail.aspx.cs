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


public partial class dluser_Gtask_Detail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //查看订单状态
        string strBillNo = Request.QueryString["vbillno"].ToString();
        int lngBillType = 0;
        DataTable dt = new OrderManager().DL_OrderBillBySel(strBillNo, lngBillType);
        if (dt.Rows.Count > 0)
        {
            //绑定表头字段,text
            TxtBillDate.Text = dt.Rows[0]["datCreateTime"].ToString();
            TxtBiller.Text = dt.Rows[0]["ccusname"].ToString();
            TxtCustomer.Text = dt.Rows[0]["ccusname"].ToString();
            TxtOrderBillNo.Text = strBillNo;
            TxtOrderMark.Text = dt.Rows[0]["strRemarks"].ToString();
            string fyfs = "";
            switch (dt.Rows[0]["cSCCode"].ToString()) //发运方式
            {
                case "00":
                    TxtcSCCode.Text = "自提";
                    fyfs = "自提";
                    break;
                case "01":
                    TxtcSCCode.Text = "配送";
                    fyfs = "配送";
                    break;
            }
            switch (dt.Rows[0]["cSTCode"].ToString())
            {
                case "00":
                    TxtcSTCode.Text = "普通销售";
                    break;
                case "01":
                    TxtcSTCode.Text = "样品资料";
                    break;
            }
            Txtcdefine3.Text = dt.Rows[0]["cdefine3"].ToString();   //车型
            TxtLoadingWays.Text = dt.Rows[0]["strLoadingWays"].ToString();  //装车方式
            TxtDeliveryDate.Text = dt.Rows[0]["datDeliveryDate"].ToString();//交货日期
            TxtOrderShippingMethod.Text = dt.Rows[0]["cDefine11"].ToString();//送货方式
            TxtSalesman.Text = dt.Rows[0]["cpersoncode"].ToString();//业务员
            TxtBillTime.Text = dt.Rows[0]["datBillTime"].ToString();//下单时间

            //绑定表头mome
            ASPxMemo1.Text = "网单号：" + strBillNo.ToString()
                + "；   制单人：" + dt.Rows[0]["ccusname"].ToString() + "；   订单日期：" + dt.Rows[0]["datCreateTime"].ToString()
                            + "；   开票单位：" + dt.Rows[0]["ccusname"].ToString()
                            + "；   业务员：" + dt.Rows[0]["cpersoncode"].ToString()
                            + "；   \r\n发货方式：" + fyfs
                            + "；   发运方式：" + dt.Rows[0]["cDefine11"].ToString()
                            + "；   \r\n备注：" + dt.Rows[0]["strRemarks"].ToString()
                            + "；   车型：" + dt.Rows[0]["cdefine3"].ToString()
                            + "；   装车方式：" + dt.Rows[0]["strLoadingWays"].ToString()
                            + "；   下单时间：" + dt.Rows[0]["datBillTime"].ToString()
                            + "；   提货时间：" + dt.Rows[0]["datDeliveryDate"].ToString()
            + "；   行政区：" + dt.Rows[0]["cdefine8"].ToString() + "；   \r\n托运信息：" + dt.Rows[0]["chdefine21"].ToString();

            //绑定表体字段,grid
            ViewOrderGrid.DataSource = dt;
            ViewOrderGrid.DataBind();
        }


    }
}