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

/// <summary>
/// GetWeiXinAccessToken 的摘要说明
/// </summary>
[WebService(Namespace = "http://dl.duolian.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
// [System.Web.Script.Services.ScriptService]
public class GetWeiXinAccessToken : System.Web.Services.WebService
{
     

    public GetWeiXinAccessToken()
    {
        

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }

    //[WebMethod]
    //public string HelloWorld()
    //{
    //    return "Hello World";
    //}
    //[WebMethod(Description = "返回字符串")]
    //public string GetString(string str)
    //{
    //    return str;
    //}
    [WebMethod(Description = "返回两数之和")]
    public int GetSum(int a, int b)
    {
        return a + b;
    }
    /// <summary>
    /// 企业号应用
    /// </summary>
    [WebMethod(Description = "GetWeiXin_AccessToken")]
    public void GetAndSetApplicationWXAccessToken()
    {
        string getAccessTokenUrl = "https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid={0}&corpsecret={1}";
        string accessToken = "";
        string respText = "";

        //获取id,secret

        //获取josn数据
        //string url = string.Format(getAccessTokenUrl, corpid, corpsecret);
        string url = string.Format(getAccessTokenUrl, "wx85ee38394e42f0b7", "kdFAL0VNTGNxiCJJV-J202ZLLNz2VgQvYoh0GxvEtCVXUHmSCGFdNxBlgx-xOXAO");
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

        //return accessToken;

        //DataTable dt=
        string asx = accessToken + "," + System.DateTime.Now.ToString();
        Application["WXAccessToken"] = asx;
    }

    [WebMethod(Description = "GetWeiXin_AccessToken's Application")]
    public string GetApplicationWXAccessToken()
    {
        string WXAccessToken = "";
        if (Application["WXAccessToken"] != null)
        {
            WXAccessToken = Application["WXAccessToken"].ToString();
        }
        return WXAccessToken;
    }

    [WebMethod(Description = "动态获取token并且赋值application,WXAccessToken")]
    public string WXAccessToken()
    {
        //if (Application["WXAccessToken"] != null)
        //{
        //    string[] asx = Application["WXAccessToken"].ToString().Split(',');
        //    DateTime startTime = Convert.ToDateTime(asx[1].ToString());
        //    DateTime endTime = Convert.ToDateTime(System.DateTime.Now.ToString());
        //    TimeSpan ts = endTime - startTime;
        //    //if (ts.Minutes < 56)
        //    //{
        //    //    return;
        //    //}
        //}
        //获取access key
        string getAccessTokenUrl = "https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid={0}&corpsecret={1}";
        string accessToken = "";
        string respText = "";
        //获取josn数据
        //string url = string.Format(getAccessTokenUrl, corpid, corpsecret);
        DataTable dt = new SearchManager().DL_GetWXCropIdBySel();
        string CorpId = dt.Rows[0]["corpid"].ToString();
        string CorpSecret = dt.Rows[0]["corpsecret"].ToString();
        string url = string.Format(getAccessTokenUrl, CorpId, CorpSecret);
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
            string asxw = accessToken + "," + System.DateTime.Now.ToString();
            return asxw;
            //Application["WXAccessToken"] = asxw;
        }
        catch (Exception ex)
        {
            return "none";
        }
    }

    [WebMethod(Description = "动态获取token并且写入05数据库的表Dl_opSystemConfiguration,WXAccessTokenToSqlTable")]
    public string WXAccessTokenToSqlTable()
    {
        //获取access key
        string getAccessTokenUrl = "https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid={0}&corpsecret={1}";
        string accessToken = "";
        string respText = "";
        //获取josn数据
        //string url = string.Format(getAccessTokenUrl, corpid, corpsecret);
        DataTable dt = new SearchManager().DL_GetWXCropIdBySel();
        string CorpId = dt.Rows[0]["corpid"].ToString();
        string CorpSecret = dt.Rows[0]["corpsecret"].ToString();
        string url = string.Format(getAccessTokenUrl, CorpId, CorpSecret);
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
            //写入数据库
            bool c = new SearchManager().DL_WXaccesstokenByUpd(accessToken);
            string asxw = accessToken + "," + System.DateTime.Now.ToString();
            return asxw;
            //Application["WXAccessToken"] = asxw;
        }
        catch (Exception ex)
        {
            return "none";
        }
    }

    /// <summary>
    /// 服务号应用
    /// </summary>
    [WebMethod(Description = "服务号，动态获取token并且写入05数据库的表Dl_opSystemConfiguration,WXAccessTokenToSqlTable")]
    public string WXFWHAccessTokenToSqlTable()
    {
        //获取access key

        string getAccessTokenUrl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";
        string FWH_access_token = "";
        string respText = "";
        //获取josn数据
        //string url = string.Format(getAccessTokenUrl, corpid, corpsecret);
        DataTable dt = new SearchManager().DL_GetWXAppIdBySel();
        string appid = dt.Rows[0]["FWH_AppID"].ToString();
        string secret = dt.Rows[0]["FWH_AppSecret"].ToString();
        string url = string.Format(getAccessTokenUrl, appid, secret);
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        using (Stream resStream = response.GetResponseStream())
        {
            StreamReader reader = new StreamReader(resStream, Encoding.Default);
            respText = reader.ReadToEnd();
            resStream.Close();
        }
        string asxw = "";
        bool c = false;
        try
        {
            JavaScriptSerializer Jss = new JavaScriptSerializer();
            Dictionary<string, object> respDic = (Dictionary<string, object>)Jss.DeserializeObject(respText);

            //通过键access_token获取值
            FWH_access_token = respDic["access_token"].ToString();
            //写入数据库
            c = new SearchManager().DL_WXFWHaccesstokenByUpd(FWH_access_token);
            asxw = FWH_access_token + "," + System.DateTime.Now.ToString();
            return FWH_access_token;
            //Application["WXAccessToken"] = asxw;
        }
        catch (Exception ex)
        {
            return FWH_access_token + ex.Message.ToString();
        }
    }

    [WebMethod(Description = "获取服务号的信息")]
    public string GetFWHInfo()
    {
        DataTable dt = new SearchManager().DL_GetWXAppIdBySel();
        if (dt.Rows.Count > 0)
        {
            string FWH_AppID = dt.Rows[0]["FWH_AppID"].ToString();
            string FWH_AppSecret = dt.Rows[0]["FWH_AppSecret"].ToString();
            string FWH_access_token = dt.Rows[0]["FWH_access_token"].ToString();
            return FWH_AppID + "," + FWH_AppSecret + "," + FWH_access_token;
        }
        else
        {
            return "none";
        }
    }

    [WebMethod]
    public string test() { 
   
        return Application["access_token"]==null?"null":Application["access_token"].ToString();
    }

}
