using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace DingDan_WebForm
{
    public partial class Index : System.Web.UI.Page
    {
        public string str = "";
        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
            {

            }

            if (Session["login"] == null)
            {

                Response.Write("<script>top.window.location='login_V2.html'</script>");   //跳转到登陆页
            }
            else
            {
                //   Login_Name.Text = Session["strUserName"].ToString();
                //  Login_Name_Mobile.Text = Session["strUserName"].ToString();
                //Lock_Name.Text = Session["strUserName"].ToString();

                if (Session["strAllAcount"].ToString().Length == 6)
                {
                    Lock_Name.Text = Session["strUserName"].ToString() + "|" + "主账号";
                    Login_Name.Text = Session["strUserName"].ToString() + "|" + "主账号";
                    Login_Name_Mobile.Text = Session["strUserName"].ToString() + "|" + "主账号";
                }
                else
                {
                    Lock_Name.Text = Session["strAllAcount"].ToString() + "|" + "子账号";
                    Login_Name.Text = Session["strAllAcount"].ToString() + "|" + "子账号";
                    Login_Name_Mobile.Text = Session["strAllAcount"].ToString() + "|" + "子账号";
                }
                str = "strAllAcount=" + Session["strAllAcount"].ToString() + "&sId=" + Session.SessionID.ToString();


                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["isShowConn"])&&ConfigurationManager.AppSettings["isShowConn"]=="1")
                {
                    string connStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                    string[] arr = connStr.Split(';');
                    string ip = arr[0].Split('=')[1];
                    string bases = arr[1].Split('=')[1];

                    if (arr[0].Split('=')[1] == "192.168.0.252")
                    {
                        type.Text = "正式账号";
                        type.ForeColor = System.Drawing.Color.Red;
                        type.Font.Size = 20;
                    }
                    else
                    {
                        type.Text = "测试账号";

                        server.Text = ip;
                        database.Text = bases;
                    }
                }
         
            }


        }
    }
}