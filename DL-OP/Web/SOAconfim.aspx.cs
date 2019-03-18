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

public partial class SOAconfim : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["id"] != null)
        {
            DataTable dt = new SearchManager().DL_SOAforIdBySel(Request.QueryString["id"].ToString());
            Lbcccusname.Text= dt.Rows[0]["ccusname"].ToString();
            //Lbdate.Text = dt.Rows[0]["strEndDate"].ToString();    //yyyy-MM-dd hh:mm:ss
            Lbdate.Text = dt.Rows[0]["ddate"].ToString();   //yyyy-MM-dd
            Lbmoney.Text = dt.Rows[0]["dblAmount"].ToString();
            Lbmoneyup.Text = dt.Rows[0]["strUper"].ToString();
            if (dt.Rows[0]["bytCheck"].ToString()=="True")
            {
                BtnComf.Visible = false;
                CheckBoxOk.Visible = true;
            }
            else
            {
                BtnComf.Visible = true;
                CheckBoxOk.Visible = false;
            }
        }
        else
        {
            this.SOADiv.Style.Add("display", "none");   //隐藏Div,显示this.DivSOA.Style.Add("display", "block");
        }

    }
    protected void BtnComf_Click(object sender, EventArgs e)
    {
        //确认账单
        string id = Request.QueryString["id"].ToString();
        bool c = new OrderManager().DL_ConfimSOAByUpd(id);
        if (c)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('账单确认成功，感谢使用！');</script>");
            //this.DivSOA.Style.Add("display", "none");   //隐藏Div,显示this.DivSOA.Style.Add("display", "block");
            BtnComf.Visible = false;
            CheckBoxOk.Visible = true;
        }
        else
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('账单确认失败，请联系系统管理员！');</script>");
        }
    }
}