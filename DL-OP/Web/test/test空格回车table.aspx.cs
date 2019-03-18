using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class test_test空格回车table : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void ASPxButton1_Click(object sender, EventArgs e)
    {
        string s1 = ASPxTextBox1.Text;
        string s2 = ASPxMemo1.Text;
        string s3 = s1.Replace("\r", ",").Replace("\n", ",").Replace("\t", ",").Replace(" ", ",");
        string s4 = s2.Replace("\r", ",").Replace("\n", ",").Replace("\t", ",").Replace(" ", ",").Replace(",,,,", ",").Replace(",,,", ",").Replace(",,", ",");
        ASPxTextBox2.Text = s3;
        ASPxTextBox3.Text = s4;
    }
}