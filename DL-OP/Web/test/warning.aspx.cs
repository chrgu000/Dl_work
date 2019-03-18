using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net;
using System.IO;
using System.Text;
using BLL;

public partial class test_warning : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //DataTable dt = new BasicInfoManager().DLproc_NewsBySel();
        //Session["News"] = dt;
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        //if (Timer1.Enabled)
        //{
        //    Timer1.Enabled = false;
        //}
        //else
        //{
        //    Timer1.Enabled = true;
        //}
        //Timer1.Enabled = true;
        Session["timer"] = 1;
    }
    protected void Timer1_Tick(object sender, EventArgs e)
    {
        //if (Session["timer"]!=null)
        //{
        //    int s = 1;
        //    s = s + Convert.ToInt32(Session["timer"].ToString());
        //    xx.Text = s.ToString();
        //    Session["timer"] = s;          
        //}
        if (Session["timer"] == null)
        {
            Session["timer"] = 0;
        }
        if (Session["timer"].ToString() == "60")
        {
            DataTable dt = new BasicInfoManager().DLproc_NewsBySel();
            Session["News"] = dt;
            Session["timer"] = 0;
        }
        if (Session["News"] != null)
        {
            DataTable dt = (DataTable)Session["News"];
            if (dt.Rows.Count > 0)
            {
                if (Session["NewsId"] != null)
                {

                }
                else
                {
                    Session["NewsId"] = 1;
                }
                int s = Convert.ToInt32(Session["NewsId"].ToString());
                if (s < dt.Rows.Count)
                {
                    xx.Text = dt.Rows[s]["strNews"].ToString();
                    Session["NewsId"] = s + 1;
                }
                else
                {
                    xx.Text = dt.Rows[0]["strNews"].ToString();
                    Session["NewsId"] = 0;
                }
            }
            else
            {
                xx.Text = "No News ! ";
            }
        }
        Session["timer"] = Convert.ToInt32(Session["timer"].ToString()) + 5;
    }

}