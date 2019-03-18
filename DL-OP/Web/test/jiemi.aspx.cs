using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
public partial class test_jiemi : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Response.Write(dis_Token("+KUAqds8N9UauuOUjGMHVn7hT4tKDtZGX9ZWTKKS9UaDlbu3QD7Vh+FZ+nLJL3NLlQCpWGKOResOV0yfvVZi02hrxmhfzO8Z"));

        Response.Write(ExecDateDiff(Convert.ToDateTime("2017-4-26 15:53:16"),DateTime.Now));
    }

    public string dis_Token(string token)
    {
        //string token = HttpContext.Current.Request.QueryString["token"];
        //     token = HttpContext.Current.Session["token"].ToString();
        token = token.Replace(" ", "+");
        token = new Check().DecryptDES(token);
        return token;
    }

    public string ExecDateDiff(DateTime dateBegin, DateTime dateEnd)
    {
        TimeSpan ts1 = new TimeSpan(dateBegin.Ticks);
        TimeSpan ts2 = new TimeSpan(dateEnd.Ticks);
        TimeSpan ts3 = ts1.Subtract(ts2).Duration();
        //你想转的格式
        return ts3.TotalSeconds.ToString();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("http://192.168.0.249:8001/login_v2.aspx?token=" + Get_Token());
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