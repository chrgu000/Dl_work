using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class wxapp_scan : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["state"]))
        {
            string getAccessTokenUrl = "https://qyapi.weixin.qq.com/cgi-bin/user/getuserinfo?access_token={0}&code={1}";
            string UserId = "";
            string respText = "";
            //获取josn数据
            //获取accesstoken
            string access_token = Request.QueryString["state"].ToString();
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
            Label2.Text = "你当前的企业应用账户为：" + UserId;
        }
    }
}