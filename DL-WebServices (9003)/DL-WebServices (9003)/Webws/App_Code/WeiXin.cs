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
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Configuration;

/// <summary>
/// WeiXin 的摘要说明
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
// [System.Web.Script.Services.ScriptService]
public class WeiXin : System.Web.Services.WebService
{

    public WeiXin()
    {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }

    BLL.HttpHandler HttpHandler = new HttpHandler();

    #region 获取企业号access_token并写入Application
    [WebMethod(Description = "获取企业号access_token并写入Application")]
    public string Get_access_token()
    {
        string url = " https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid=wx85ee38394e42f0b7&corpsecret=kdFAL0VNTGNxiCJJV-J202ZLLNz2VgQvYoh0GxvEtCVXUHmSCGFdNxBlgx-xOXAO";
        string res = HttpHandler.GetWebRequest(url);
        JObject jo = (JObject)JsonConvert.DeserializeObject(res);
        Application["access_token"] = jo["access_token"].ToString();
        return Application["access_token"].ToString();
    }
    #endregion

    #region 根据企业应用ID来获取access_token并写入Application
    /// <summary>
    ///  获取企业号access_token并写入Application
    /// </summary>
    /// <returns></returns>
    [WebMethod(Description = "根据企业应用ID来获取access_token并写入Application")]

    public string Get_access_token_byAPPID(string AgentId)
    {
        //if (AgentId.Length > 2)
        //{
        //    string url = " https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid=wx85ee38394e42f0b7&corpsecret=" + ConfigurationManager.AppSettings[AgentId];
        //    string res = HttpHandler.GetWebRequest(url);
        //    JObject jo = (JObject)JsonConvert.DeserializeObject(res);
        //    string ApplicationId = "access_token_" + AgentId;
        //    Application[ApplicationId] = jo["access_token"].ToString();
        //    return Application[ApplicationId].ToString();
        //}
        //else
        //{
        //    string url = " https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid=wx85ee38394e42f0b7&corpsecret=kdFAL0VNTGNxiCJJV-J202ZLLNz2VgQvYoh0GxvEtCVXUHmSCGFdNxBlgx-xOXAO";
        //    string res = HttpHandler.GetWebRequest(url);
        //    JObject jo = (JObject)JsonConvert.DeserializeObject(res);
        //    Application["access_token"] = jo["access_token"].ToString();
        //    return Application["access_token"].ToString();
        //}

        string url = " https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid=wx85ee38394e42f0b7&corpsecret=" + ConfigurationManager.AppSettings[AgentId];
        string res = HttpHandler.GetWebRequest(url);
        JObject jo = (JObject)JsonConvert.DeserializeObject(res);
        string ApplicationId = "access_token_" + AgentId;
        Application[ApplicationId] = jo["access_token"].ToString();
        return Application[ApplicationId].ToString();


    }
    #endregion

    #region 根据code获取微信登录用户的userID
    [WebMethod(Description = "根据code获取微信登录用户的userID")]
    public string Get_UserIdByAuthCode(string code)
    {
        string url = "https://qyapi.weixin.qq.com/cgi-bin/user/getuserinfo?access_token={0}&code={1}";
        if (Application["access_token"] == null)
        {
            Get_access_token();
        }

        string res = GetUrl(url,"20");
        return res;

    }
    #endregion

    #region 根据code和企业应用ID获取微信登录用户的userID
    [WebMethod(Description = "根据code和企业应用ID获取微信登录用户的userID")]
    public string Get_UserIdByAuthCode_ByAPPID(string code, string agentId)
    {
        string url = "https://qyapi.weixin.qq.com/cgi-bin/user/getuserinfo?access_token={0}&code="+code;
        string Secret = ConfigurationManager.AppSettings[agentId];
        string ApplicationId = "access_token_" + agentId;
        if (Application[ApplicationId] == null)
        {
            Get_access_token_byAPPID(agentId);
        }
       // string a = Application[ApplicationId].ToString();

        string get_url = string.Format(url, Application[ApplicationId], code);
        string res = GetUrl(url, agentId);
        //string res = HttpHandler.GetWebRequest(get_url);
        //JObject jo = jo = JObject.Parse(res); ;
        //if (string.IsNullOrEmpty(jo["UserId"].ToString()))
        //{

        //    if (jo["errcode"].ToString() == "40014" || jo["errcode"].ToString() == "420001")
        //    {
        //        get_url = string.Format(url, Get_access_token_byAPPID(agentId), code);
        //        res = HttpHandler.GetWebRequest(get_url);
        //    }
        //}
        return res;

    }
    #endregion

    #region 发送企业号消息：文本类型
    [WebMethod(Description = "发送企业号消息：文本类型")]
    public string SendMsg_Text(string touser, string toparty, string totag, string agentid, string strWxMessage)
    {
        JObject jo = new JObject();
        JObject j = new JObject();
        jo["touser"] = touser;
        jo["toparty"] = toparty;
        jo["totag"] = totag;
        jo["msgtype"] = "text";
        jo["agentid"] = agentid;
        j["content"] = strWxMessage;
        j["safe"] = 0;
        jo["text"] = j;

        string WXRel = SendQYMessage("1", "2", JsonConvert.SerializeObject(jo), Encoding.UTF8, agentid);
        new SearchManager().WXMsgRecord(touser, JsonConvert.SerializeObject(jo), WXRel, agentid, 1);
        return WXRel;

    }
    #endregion

     

 

    #region 发送企业号消息：文本类型
    [WebMethod(Description = "发送企业号消息：文本卡片类型")]
    public string SendMsg_TextCard(string touser, string toparty, string totag, string agentid, string title,string description,string url)
    {
        JObject jo = new JObject();
        JObject j = new JObject();
        jo["touser"] = touser;
        jo["toparty"] = toparty;
        jo["totag"] = totag;
        jo["msgtype"] = "textcard";
        jo["agentid"] = agentid;
        j["description"] =description;
        j["title"] = title;
        j["url"] = url;
        j["btntxt"] = "查看详情";
        jo["textcard"] = j;
         
        string WXRel = SendQYMessage("1", "2", JsonConvert.SerializeObject(jo), Encoding.UTF8, agentid);
        new SearchManager().WXMsgRecord(touser, JsonConvert.SerializeObject(jo), WXRel, agentid, 1);
        return WXRel;

    }
    #endregion

    #region 发送企业号消息：文本类型
    [WebMethod(Description = "发送企业号消息：文本卡片类型")]
    public string SendMsg_TextCard_Sr(string touser, string toparty, string totag, string agentid, string title, string description, string url)
    {
        JObject jo = new JObject();
        JObject j = new JObject();
        jo["touser"] = touser;
        jo["toparty"] = toparty;
        jo["totag"] = totag;
        jo["msgtype"] = "textcard";
        jo["agentid"] = agentid;
        j["description"] = description;
        j["title"] = title;
        j["url"] = url;
        j["btntxt"] = "查看详情";
        jo["textcard"] = j;

        string WXRel = SendQYMessage("1", "2", JsonConvert.SerializeObject(jo), Encoding.UTF8, agentid);
        return WXRel;

    }
    #endregion

    #region 发送企业号消息：文本类型,加密URL，主要用于发送微信订单审核信息，URL跳转至详情页面
    [WebMethod(Description = "发送企业号消息：文本卡片类型")]
    public string SendMsg_TextCard_EncryUrl(string touser, string toparty, string totag, string agentid, string title, string description, string orderId,string orderType)
    {
        JObject jo = new JObject();
        BLL.Cryptography cry = new Cryptography();
        jo["orderId"] =orderId;
        jo["orderType"] = orderType;
        string orderCode = cry.EncryptDES(JsonConvert.SerializeObject(jo));
        string url="http://dl.duolian.com:1234/mobile/html/order_info.html?orderCode="+orderCode;

        string WXRel = SendMsg_TextCard(touser, toparty, totag, agentid, title, description, url);
        return WXRel;

    }
    #endregion

    #region 判断微信返回值
     [WebMethod(Description = "判断微信返回值")]
    public bool Check_WXRel(string WXRel)
    {
        JObject jo = new JObject();
        jo = JObject.Parse(WXRel);
        bool b = false;
        if (jo["errcode"].ToString() == "0" && jo["errmsg"].ToString() == "ok")
        {
            b = true;
        }
        return b;
    }

    #endregion

    #region 发头微信消息
    /// <summary>
    /// 推送信息（企业号，标签）
    /// </summary>
    /// <param name="corpid">企业号ID</param>
    /// <param name="corpsecret">管理组密钥</param>
    /// <param name="paramData">提交的数据json</param>
    /// <param name="dataEncode">编码方式</param>
    /// <returns></returns>
    private string SendQYMessage(string corpid, string corpsecret, string paramData, Encoding dataEncode, string agentid)
    {
       // string access_token = Get_access_token_byAPPID(agentid);
        string url = "https://qyapi.weixin.qq.com/cgi-bin/message/send?access_token={0}";
        string res = PostUrl(url, paramData, dataEncode, agentid);
        return res;
    }
    #endregion

    #region 获取jsapi_ticket
    [WebMethod(Description = "获取jsapi_ticket")]
    public string Get_jsapi_ticket()
    {
        string url = "https://qyapi.weixin.qq.com/cgi-bin/get_jsapi_ticket?access_token={0}";
        return GetUrl(url, "20");

    }
    #endregion



    /// <summary>
    /// Post数据接口
    /// </summary>
    /// <param name="postUrl">接口地址</param>
    /// <param name="paramData">提交json数据</param>
    /// <param name="dataEncode">编码方式</param>
    /// <returns></returns>
    private string PostUrl(string Url, string paramData, Encoding dataEncode, string agentid)
    {
        string postUrl = string.Empty;
        //if (agentid.Length > 2)
        //{
        //    if ( Application["access_token_" + agentid]==null)
        //    {
        //        postUrl = string.Format(Url, Get_access_token_byAPPID(agentid));

        //    }
        //    else
        //    {
        //        postUrl = string.Format(Url, Application["access_token_" + agentid]);

        //    }

        //}
        //else
        //{
        //    if ( Application["access_token"]==null)
        //    {
        //        postUrl = string.Format(Url,Get_access_token_byAPPID(agentid));

        //    }
        //    else
        //    {
        //        postUrl = string.Format(Url, Application["access_token"]);
                
        //    }

        //}
        if (Application["access_token_" + agentid] == null)
        {
            postUrl = string.Format(Url, Get_access_token_byAPPID(agentid));

        }
        else
        {
            postUrl = string.Format(Url, Application["access_token_" + agentid]);

        }
        string res = HttpHandler.PostWebRequest(postUrl, paramData, dataEncode);
        JObject jo = new JObject();
        jo = JObject.Parse(res);
        if (jo["errcode"] != null && (jo["errcode"].ToString() == "40014" || jo["errcode"].ToString() == "42001"))
        {
            postUrl = string.Format(Url, Get_access_token_byAPPID(agentid));
            res = HttpHandler.PostWebRequest(postUrl, paramData, dataEncode);
        }
        return res;
    }

    /// <summary>
    /// Get数据接口
    /// </summary>
    /// <param name="Url"></param>
    /// <returns></returns>
    private string GetUrl(string Url, string agentid)
    {
        string getUrl = string.Empty;
        //if (agentid.Length > 2)
        //{
        //    if (Application["access_token_" + agentid] == null)
        //    {
        //        getUrl = string.Format(Url, Get_access_token_byAPPID(agentid));

        //    }
        //    else
        //    {
        //        getUrl = string.Format(Url, Application["access_token_" + agentid]);

        //    }

        //}
        //else
        //{
        //    if (Application["access_token"] == null)
        //    {
        //        getUrl = string.Format(Url, Get_access_token_byAPPID(agentid));

        //    }
        //    else
        //    {
        //        getUrl = string.Format(Url, Application["access_token"]);

        //    }

        //}
        if (Application["access_token_" + agentid] == null)
        {
            getUrl = string.Format(Url, Get_access_token_byAPPID(agentid));

        }
        else
        {
            getUrl = string.Format(Url, Application["access_token_" + agentid]);

        }
        string res = HttpHandler.GetWebRequest(getUrl);
        JObject jo = new JObject();
        jo = JObject.Parse(res);
        if (jo["errcode"] != null && (jo["errcode"].ToString() == "40014"||jo["errcode"].ToString() == "42001"))
        {
            getUrl = string.Format(Url, Get_access_token_byAPPID(agentid));
            res = HttpHandler.GetWebRequest(getUrl);
        }
        return res;
    }

        [WebMethod(Description = "测试方法")]

    public void test() {
       // string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx85ee38394e42f0b7&redirect_uri=http://dl.duolian.com:8001/mobile/html/RoleCheck.html&response_type=code&scope=snsapi_base&state=dl&connect_redirect=1#wechat_redirect";
      //  string re = HttpHandler.GetWebRequest(url);
        
    }


}
