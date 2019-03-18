using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using BLL;

public partial class WXRegister : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        dw.Text = "单位：" + Session["strUserName"].ToString() + "（目前最多登记5个微信号码）";
        DataTable dt = new BasicInfoManager().DL_GetCustomerWXInfoBySel(Session["ConstcCusCode"].ToString());
        WXGrid.DataSource = dt;
        WXGrid.DataBind();
        if (dt.Rows.Count >= 5)
        {
            this.wxtable.Style.Add("display", "none");
            BtnOk.Enabled = false;
        }
    }
    protected void BtnOk_Click(object sender, EventArgs e)
    {
        string WXName = "";
        string WXPhoneNum = "";
        string WXAcount = "";
        if (TxtWXName != null)
        {
            WXName = TxtWXName.Text;
        }
        if (TxtWXPhoneNum != null)
        {
            WXPhoneNum = TxtWXPhoneNum.Text;
        }
        if (TxtWXAcount != null)
        {
            WXAcount = TxtWXAcount.Text;
        }
        bool c = new BasicInfoManager().DL_CustomerWXInfoByIns(Session["strUserName"].ToString(), Session["ConstcCusCode"].ToString(), WXName, WXPhoneNum, WXAcount);
        TxtWXName.Text = "";
        TxtWXPhoneNum.Text = "";
        TxtWXAcount.Text = "";
    }
}