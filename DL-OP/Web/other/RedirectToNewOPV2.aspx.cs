using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;

public partial class other_RedirectToNewOPV2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Redirect("http://dl.duolian.com:1234/login_v2.aspx?token=" + Get_Token());
    }

    #region 获取用户名,手机号码和当前时间,拼接为字符串,并加密
    public string Get_Token()
    {
        ;
        string ccuscode = Session["cCusCode"].ToString();
        string phone = Session["strLoginPhone"].ToString();
        string pwd = Session["pwd"].ToString();
        string time = DateTime.Now.ToString();
        string token = ccuscode + "|" + pwd + "|" + phone + "|" + time;
        //HttpContext.Current.Session["token"] = EncryptDES(token);
        return new Check().EncryptDES(token);
    }
    #endregion

}