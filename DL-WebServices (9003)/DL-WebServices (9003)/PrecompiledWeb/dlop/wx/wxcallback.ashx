<%@ WebHandler Language="C#" Class="wxcallback" %>

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using BLL;

public class wxcallback : IHttpHandler
{

    //public void ProcessRequest(HttpContext context)
    //{
    //    context.Response.ContentType = "text/plain";
    //    context.Response.Write("Hello World");
    //}

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    public void ProcessRequest(HttpContext context)
    {
        string postString = string.Empty;
        if (HttpContext.Current.Request.HttpMethod.ToUpper() == "GET")
        {
            Auth();
        }
    }

    /// <summary>
    /// 成为开发者的第一步，验证并相应服务器的数据
    /// </summary>
    private void Auth()
    {
        string token = ConfigurationManager.AppSettings["CorpToken"];//从配置文件获取Token

        string encodingAESKey = ConfigurationManager.AppSettings["EncodingAESKey"];//从配置文件获取EncodingAESKey

        string corpId = ConfigurationManager.AppSettings["CorpId"];//从配置文件获取corpId

        string echoString = HttpContext.Current.Request.QueryString["echoStr"];
        string signature = HttpContext.Current.Request.QueryString["msg_signature"];//企业号的 msg_signature
        string timestamp = HttpContext.Current.Request.QueryString["timestamp"];
        string nonce = HttpContext.Current.Request.QueryString["nonce"];

        string decryptEchoString = "";
        if (CheckSignature(token, signature, timestamp, nonce, corpId, encodingAESKey, echoString, ref decryptEchoString))
        {
            if (!string.IsNullOrEmpty(decryptEchoString))
            {
                HttpContext.Current.Response.Write(decryptEchoString);
                HttpContext.Current.Response.End();
            }
        }
    }

    /// <summary>
    /// 验证企业号签名
    /// </summary>
    /// <param name="token">企业号配置的Token</param>
    /// <param name="signature">签名内容</param>
    /// <param name="timestamp">时间戳</param>
    /// <param name="nonce">nonce参数</param>
    /// <param name="corpId">企业号ID标识</param>
    /// <param name="encodingAESKey">加密键</param>
    /// <param name="echostr">内容字符串</param>
    /// <param name="retEchostr">返回的字符串</param>
    /// <returns></returns>
    public bool CheckSignature(string token, string signature, string timestamp, string nonce, string corpId, string encodingAESKey, string echostr, ref string retEchostr)
    {
        WXBizMsgCrypt wxcpt = new WXBizMsgCrypt(token, encodingAESKey, corpId);
        int result = wxcpt.VerifyURL(signature, timestamp, nonce, echostr, ref retEchostr);
        if (result != 0)
        {
            //LogTextHelper.Error("ERR: VerifyURL fail, ret: " + result);
            return false;
        }

        return true;

        //ret==0表示验证成功，retEchostr参数表示明文，用户需要将retEchostr作为get请求的返回参数，返回给企业号。
        // HttpUtils.SetResponse(retEchostr);
    }


}




 
