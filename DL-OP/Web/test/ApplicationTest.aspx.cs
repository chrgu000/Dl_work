using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Security;
using System.Collections;

public partial class test_ApplicationTest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Label1.Text = DateEditbegin.Date.Date.ToShortDateString();
        Label1.Text = System.DateTime.Now.ToString("d");
    }
    protected void ASPxButton1_Click(object sender, EventArgs e)
    {

        Hashtable hOnline = (Hashtable)Application["Online"];
        //Session.Contents.Remove("login");
        if (hOnline != null)       //判断哈希表是否包含特定键,其返回值为true或false
        {
            ASPxGridView1.DataSource = hOnline;
            ASPxGridView1.DataBind();
            //System.Web.UI.ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "updateScript", "alert('该用户已在其他地方登录，如非您本人操作，请及时联系管理员！');", true);                
        }
    }

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        int s=1;
        s = s + Convert.ToInt32(Session["timer"].ToString());
        Session["timer"] = s;
        xx.Text = s.ToString();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Session["timer"] = 1;
        Timer1.Enabled = true;
    }
}