using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class other_SingleLoginCheck : System.Web.UI.Page
{
    public string strAllAcount;
    public string sId;
    protected void Page_Load(object sender, EventArgs e)
    {
        strAllAcount = Request.QueryString["strAllAcount"].ToString();
        sId = Request.QueryString["sId"].ToString();
        if (!IsPostBack)
        {
            //string sId = Session.SessionID.ToString();  //获取当前session的id
            //写入Application中
                if (Application["Online"] != null)
                {
                    Hashtable hOnline = (Hashtable)Application["Online"];
                    //删除当前顾客的记录
                    if (hOnline.Contains(strAllAcount))  //判断哈希表是否包含特定键,其返回值为true或false
                    {
                        hOnline.Remove(strAllAcount);//移除一个key/value键值对
                    }
                    //string sId = Session.SessionID.ToString();  //获取当前session的id
                    hOnline.Add(strAllAcount, sId);//添加key/value键值对
                    Application["Online"] = hOnline;
                }
                else
                {
                    Hashtable hOnline = new Hashtable();
                    //string sId = Session.SessionID.ToString();  //获取当前session的id
                    hOnline.Add(strAllAcount, sId);//添加key/value键值对
                    Application["Online"] = hOnline;
                }
        }
    }

    protected void SessionTimer_Init(object sender, EventArgs e)
    {
        SessionTimer.Interval = 10000;
        SessionTimer.Enabled = true;
    }

    protected void SessionTimer_Tick(object sender, EventArgs e)
    {
        Hashtable hOnline = (Hashtable)Application["Online"];
        //Session.Contents.Remove("login");
        //if (hOnline.Contains(Session["strAllAcount"].ToString()))       //判断哈希表是否包含特定键,其返回值为true或false
        if (hOnline.Contains(strAllAcount))       //判断哈希表是否包含特定键,其返回值为true或false
        {
            //5秒更新在线状况
            string s = (string)hOnline[strAllAcount];
            //string sId = Session.SessionID.ToString();  //获取当前session的id
            if (s != sId)
            {
                //Session["login"] = null;
                //Session.Contents.Remove("login");
                //Warm.Text = "该用户已在其他地点登录!";
                SessionTimer.Enabled = false;
                //Response.Redirect("http://192.168.0.249:8001/login_v2.aspx");
                Response.Write("<script>top.window.location='http://192.168.0.249:8001/login_v2.aspx'</script>");   //跳转到登陆页
                //System.Web.UI.ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "updateScript", "alert('该用户已在其他地方登录，如非您本人操作，请及时联系管理员！');", true);                
            }
        }
    }
}
