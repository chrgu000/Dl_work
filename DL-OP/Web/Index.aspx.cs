using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Model;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Security;
using System.Collections;

public partial class Index : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        ASPxLabel1.Text = Session.SessionID.ToString();
        //Response.CacheControl = "no-cache";
        if (!IsPostBack)
        {

        }
        //if (Session["login"] == null || Session["strUserLevel"].ToString() != "3")
        if (Session["login"] == null)
        {
            //Response.Redirect("login.aspx");
            Response.Write("<script>top.window.location='login.aspx'</script>");   //跳转到登陆页
        }
    }
    protected void setPWD_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
    {
        Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('setPWD_WindowCallback！');</script>");
    }
    //protected string newPWD(string pwd, string conpwd)
    //{

    //    return pwd + "_" + conpwd;
    //}
    protected void btOK_Click(object sender, EventArgs e)
    {
        if (tbPWD.Text.ToString() != "" && tbPWD.Text.ToString() == tbPassword.Text.ToString())
        {
            //修改密码
            string pwd = FormsAuthentication.HashPasswordForStoringInConfigFile(tbPWD.Text.Trim(), "MD5");// 把密码转为MD5码的形式
            if (Session["lngopUserExId"].ToString() != "0")
            {               
                BasicInfo bi = new BasicInfo(Session["lngopUserId"].ToString(), pwd);
                bool b = new BasicInfoManager().Update_UserPWD(bi);
            }
            else
            {
                bool b = new BasicInfoManager().Update_SubUserPWD(Session["lngopUserExId"].ToString(), pwd);
            }           
            System.Web.UI.ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "updateScript", "alert('密码修改成功,请牢记新密码！');", true);
        }
    }


    protected void SessionTimer_Init(object sender, EventArgs e)
    {
        //Hashtable hOnline = (Hashtable)Application["Online"];
        ////Session.Contents.Remove("login");
        //if (hOnline.Contains(Session["ConstcCusCode"].ToString()))       //判断哈希表是否包含特定键,其返回值为true或false
        //{
        //    string s = (string)hOnline[Session["ConstcCusCode"].ToString()];
        //    string sId = Session.SessionID.ToString();  //获取当前session的id
        //    if (s != sId)
        //    {
        //        Session["login"] = null;
        //        Session.Contents.Remove("login");
        //        Warm.Text = "该用户已在其他地点登录!";
        //        SessionTimer.Enabled = false;
        //        //System.Web.UI.ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "updateScript", "alert('该用户已在其他地方登录，如非您本人操作，请及时联系管理员！');", true);                
        //    }
        //}
        SessionTimer.Interval = 10000;
        SessionTimer.Enabled = true;
    }
    protected void SessionTimer_Tick(object sender, EventArgs e)
    {
        Hashtable hOnline = (Hashtable)Application["Online"];
        //Session.Contents.Remove("login");
        if (hOnline.Contains(Session["strAllAcount"].ToString()))       //判断哈希表是否包含特定键,其返回值为true或false
        {
            //5秒更新在线状况
            string s = (string)hOnline[Session["strAllAcount"].ToString()];
            string sId = Session.SessionID.ToString();  //获取当前session的id
            if (s != sId)
            {
                Session["login"] = null;
                Session.Contents.Remove("login");
                //Warm.Text = "该用户已在其他地点登录!";
                SessionTimer.Enabled = false;
                Response.Redirect("logout.aspx?id=1");
                //System.Web.UI.ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "updateScript", "alert('该用户已在其他地方登录，如非您本人操作，请及时联系管理员！');", true);                
            }
        }

        //通知显示
        if (Session["timer_News"] == null)
        {
            Session["timer_News"] = 0;
        }
        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();
        if (Session["timer_News"].ToString() == "0")
        {
            dt = new BasicInfoManager().DLproc_NewsBySel();
            Session["News"] = dt;
            dt1 = new BasicInfoManager().DLproc_News1BySel();
            Session["News1"] = dt1;


            //Session["timer_News"] = 0;
        }
        #region 更新普通通知
        if (Session["News"] != null)
        {
            dt = (DataTable)Session["News"];
            if (dt.Rows.Count > 0)
            {
                if (Session["NewsId"] != null)
                {

                }
                else
                {
                    Session["NewsId"] = 0;
                }
                int s = Convert.ToInt32(Session["NewsId"].ToString());
                if (s < dt.Rows.Count)
                {
                    MainNewNo2.Text = dt.Rows[s]["srtNewsContent"].ToString();
                    Session["NewsId"] = s + 1;
                }
                else
                {
                    MainNewNo2.Text = dt.Rows[0]["srtNewsContent"].ToString();
                    Session["NewsId"] = 0;
                }
            }
            else
            {
                MainNewNo2.Text = "No Content ! ";
            }
        }
        #endregion

        #region 更新重要通知
        dt1 = (DataTable)Session["News1"];
        MainNewNo1.Text = dt1.Rows[0]["srtNewsContent"].ToString();
        #endregion

        #region 计时器+5，超过60重置
        if (Convert.ToInt32(Session["timer_News"].ToString()) + 5 >= 60)
        {
            Session["timer_News"] = 0;
        }
        else
        {
            Session["timer_News"] = Convert.ToInt32(Session["timer_News"].ToString()) + 5;
        }
        #endregion




    }



    #region 获取用户名,手机号码和当前时间,拼接为字符串,并加密
    public string Get_Token()
    {
        ;
        string ccuscode = Session["cCusCode"].ToString();
        string phone = Session["cCusPhone"].ToString();
        string pwd = Session["pwd"].ToString();
        string time = DateTime.Now.ToString();
        string token = ccuscode + "|" + pwd + "|" + phone + "|" + time;
        //HttpContext.Current.Session["token"] = EncryptDES(token);
        return new Check().EncryptDES(token);
    }
    #endregion

}