using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using BLL;

public partial class test_weixin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["code"] != null)
        {
            TextBox1.Text = Request.QueryString["code"].ToString();
        }
        if (Request.QueryString["state"] != null)
        {
            TextBox2.Text = Request.QueryString["state"].ToString();
        }


        //获取userid
        string getAccessTokenUrl = "https://qyapi.weixin.qq.com/cgi-bin/user/getuserinfo?access_token={0}&code={1}";
        string UserId = "";
        string respText = "";
        //获取josn数据
        //获取accesstoken
        string access_token = "";
        if (Application["WXAccessToken"] != null)
        {
            string[] asx = Application["WXAccessToken"].ToString().Split(',');
            access_token = asx[0].ToString();
        }
        //string access_token = "KKg64swan96oQsPD9dAXLUDPDJ6hWwpqTtTSpSb2NjXTtPBkfrLMeIfRojm-kqom";
        string code = Request.QueryString["code"].ToString();
        string url = string.Format(getAccessTokenUrl, access_token, code);
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        using (Stream resStream = response.GetResponseStream())
        {
            StreamReader reader = new StreamReader(resStream, Encoding.Default);
            respText = reader.ReadToEnd();
            resStream.Close();
        }
        JavaScriptSerializer Jss = new JavaScriptSerializer();
        Dictionary<string, object> respDic = (Dictionary<string, object>)Jss.DeserializeObject(respText);
        //通过键UserId获取值
        UserId = respDic["UserId"].ToString();
        TextBox3.Text = UserId;

        if (Application["WXAccessToken"] != null)
        {
            string[] asx = Application["WXAccessToken"].ToString().Split(',');
            TextBox4.Text = asx[0].ToString();
        }

    }

}