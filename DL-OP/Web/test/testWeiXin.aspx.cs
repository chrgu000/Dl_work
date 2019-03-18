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
using System.Data;
using System.Data.SqlClient;
using BLL;

public partial class test_testWeiXin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// 获取企业号的accessToken
    /// </summary>
    /// <param name="corpid">企业号ID</param>
    /// <param name="corpsecret">管理组密钥</param>
    /// <returns></returns>
    public string GetQYAccessToken(string corpid, string corpsecret)
    {
        string getAccessTokenUrl = "https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid={0}&corpsecret={1}";
        string accessToken = "";

        string respText = "";

        //获取josn数据
        string url = string.Format(getAccessTokenUrl, corpid, corpsecret);

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        using (Stream resStream = response.GetResponseStream())
        {
            StreamReader reader = new StreamReader(resStream, Encoding.Default);
            respText = reader.ReadToEnd();
            resStream.Close();
        }

        try
        {
            JavaScriptSerializer Jss = new JavaScriptSerializer();
            Dictionary<string, object> respDic = (Dictionary<string, object>)Jss.DeserializeObject(respText);

            //通过键access_token获取值
            accessToken = respDic["access_token"].ToString();
        }
        catch (Exception ex) { }

        return accessToken;
    }

    /// <summary>
    /// Post数据接口
    /// </summary>
    /// <param name="postUrl">接口地址</param>
    /// <param name="paramData">提交json数据</param>
    /// <param name="dataEncode">编码方式</param>
    /// <returns></returns>
    public string PostWebRequest(string postUrl, string paramData, Encoding dataEncode)
    {
        string ret = string.Empty;
        try
        {
            byte[] byteArray = dataEncode.GetBytes(paramData); //转化
            HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
            webReq.Method = "POST";
            webReq.ContentType = "application/x-www-form-urlencoded";

            webReq.ContentLength = byteArray.Length;
            Stream newStream = webReq.GetRequestStream();
            newStream.Write(byteArray, 0, byteArray.Length);//写入参数
            newStream.Close();
            HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
            ret = sr.ReadToEnd();
            sr.Close();
            response.Close();
            newStream.Close();
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        return ret;
    }

    /// <summary>
    /// 推送信息
    /// </summary>
    /// <param name="corpid">企业号ID</param>
    /// <param name="corpsecret">管理组密钥</param>
    /// <param name="paramData">提交的数据json</param>
    /// <param name="dataEncode">编码方式</param>
    /// <returns></returns>
    public string SendQYMessage(string corpid, string corpsecret, string paramData, Encoding dataEncode)
    {
        //string accessToken = GetQYAccessToken(corpid, corpsecret);
        //string accessToken = "EU0GQZabqQ3Iybt4j94vndzDI_kN5lkSqNKC58hQF8yelta7c12MMwPetC5q-Hu8";
        //string[] asx = Application["WXAccessToken"].ToString().Split(',');
        //string accessToken = asx[0].ToString();
        //accessToken = "dBdzVzf8nZxGk_P28q5hvJTcDT7QV0LeY3hoNatMlA3VeqO_DLQUhZFzcJIBUgqG";
        DataTable dt = new SearchManager().DL_GetWXCropIdBySel();
        string accessToken = dt.Rows[0]["access_token"].ToString();
        string postUrl = string.Format("https://qyapi.weixin.qq.com/cgi-bin/message/send?access_token={0}", accessToken);

        return PostWebRequest(postUrl, paramData, dataEncode);
    }




    protected void ASPxButton1_Click(object sender, EventArgs e)
    {
        string responeJsonStr = "{";
        //responeJsonStr += "\"touser\": \"" + ASPxTextBox7.Text.Trim().ToString() + "\",";
        responeJsonStr += "\"totag\": \"" + "10" + "\",";     
        responeJsonStr += "\"msgtype\": \"text\",";
        responeJsonStr += "\"agentid\": \"" + ASPxTextBox12.Text.Trim().ToString() + "\",";
        responeJsonStr += "\"text\": {";
        responeJsonStr += "  \"content\": \"" + ASPxTextBox8.Text.Trim().ToString() + "\"";
        responeJsonStr += "},";
        responeJsonStr += "\"safe\":\"0\"";
        responeJsonStr += "}";

        ASPxTextBox1.Text = SendQYMessage("1", "2", responeJsonStr, Encoding.UTF8);
    }
    protected void ASPxButton2_Click(object sender, EventArgs e)
    {
        ////WX9001.GetWeiXinAccessToken aa = new WX9001.GetWeiXinAccessToken();
        //WX9001.GetWeiXinAccessTokenSoap aa = new WX9001.GetWeiXinAccessTokenSoapClient();
        //ASPxTextBox4.Text = aa.GetSum(Convert.ToInt32(ASPxTextBox2.Text), Convert.ToInt32(ASPxTextBox3.Text)).ToString();

    }
    protected void ASPxButton3_Click(object sender, EventArgs e)
    {
        ////WebServicesForWXToken.GetWeiXinAccessTokenSoapClient aa = new WebServicesForWXToken.GetWeiXinAccessTokenSoapClient();
        ////ASPxTextBox5.Text = aa.GetAndSetApplicationWXAccessToken().ToString();
        ////WX9001.GetWeiXinAccessTokenSoap aa = new WX9001.GetWeiXinAccessTokenSoapClient();
        ////string app = aa.WXAccessToken();
        ////Application["WXAccessToken"] = app;
        //WX9001.GetWeiXinAccessTokenSoap wx = new WX9001.GetWeiXinAccessTokenSoapClient();
        //wx.WXAccessTokenToSqlTable();
        //DataTable dt = new SearchManager().DL_GetWXCropIdBySel();
        //ASPxTextBox5.Text = dt.Rows[0]["access_token"].ToString();

    }
    protected void ASPxButton4_Click(object sender, EventArgs e)
    {
        if (Application["WXAccessToken"] != null)
        {
            string[] asx = Application["WXAccessToken"].ToString().Split(',');
            ASPxTextBox6.Text = Application["WXAccessToken"].ToString();
            ASPxTextBox9.Text = asx[0].ToString();
        }
        else
        {
            ASPxTextBox6.Text = "null!";
            ASPxTextBox9.Text = "";
        }

    }
    protected void ASPxButton5_Click(object sender, EventArgs e)
    {
        if (Application["WXAccessToken"] != null)
        {
            string[] asx = Application["WXAccessToken"].ToString().Split(',');
            ASPxTextBox10.Text = Application["WXAccessToken"].ToString();
            ASPxTextBox11.Text = asx[0].ToString();
        }
        else
        {
            ASPxTextBox10.Text = "null!";
            ASPxTextBox11.Text = "";
        }
    }
    protected void ASPxButton6_Click(object sender, EventArgs e)
    {
        //WX9001.GetWeiXinAccessTokenSoap aa = new WX9001.GetWeiXinAccessTokenSoapClient();
        //string app = aa.WXAccessToken();
        //Application["WXAccessToken"] = app;
    }
    protected void ASPxButton7_Click(object sender, EventArgs e)
    {
        Application.Remove("WXAccessToken");
    }
}