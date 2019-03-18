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

public partial class SOA : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //绑定顾客信息
            //DataTable DtComboCustomer = new SearchManager().DL_ComboCustomerAllBySel(Session["ConstcCusCode"].ToString() + "%");
            DataTable DtComboCustomer = new SearchManager().DL_ComboCustomerAllBySel(Session["ConstcCusCode"].ToString() + "%");
            //Comboccusname.DataSource = DtComboCustomer;
            //Comboccusname.DataBind();
            if (DtComboCustomer.Rows.Count > 0)
            {
                Comboccusname.Items.Add("全部", "0");
                for (int i = 0; i < DtComboCustomer.Rows.Count; i++)
                {
                    Comboccusname.Items.Add(DtComboCustomer.Rows[i]["cCusName"].ToString(), DtComboCustomer.Rows[i]["cCusCode"].ToString());
                }
            }
            Comboccusname.SelectedIndex = 0;

        }

    }
    protected void BtnOk_Click(object sender, EventArgs e)
    {
        //this.DivSOA.Style.Add("display", "block");
        HFComboccusname.Value = Comboccusname.Value.ToString();
        //绑定顾客信息
        DataTable DtComboCustomer = new SearchManager().DL_ComboCustomerAllBySel(Session["ConstcCusCode"].ToString() + "%");
        if (DtComboCustomer.Rows.Count > 0)
        {
            Comboccusname.Items.Add("全部", "0");
            for (int i = 0; i < DtComboCustomer.Rows.Count; i++)
            {
                Comboccusname.Items.Add(DtComboCustomer.Rows[i]["cCusName"].ToString(), DtComboCustomer.Rows[i]["cCusCode"].ToString());
            }
        }
        //查询
        string strComboPeriod = ComboPeriod.Value.ToString();
        string strComboccusname = Comboccusname.Value.ToString();
        string strComboCheck = ComboCheck.Value.ToString();
        string strconccuscode = Session["ConstcCusCode"].ToString();
        string strComboPeriodYear = ComboPeriodYear.Value.ToString();
        DataTable dt = new SearchManager().DLproc_U8SOASearchBySel(strComboPeriodYear,strComboccusname, strComboPeriod, strComboCheck, strconccuscode);
        SOAGrid.DataSource = dt;
        SOAGrid.DataBind();

    }

}