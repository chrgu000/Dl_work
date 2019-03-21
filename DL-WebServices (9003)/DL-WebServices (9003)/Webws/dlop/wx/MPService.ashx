<%@ WebHandler Language="C#" Class="MPService" %>

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using BLL;

public class MPService : IHttpHandler {

    public void ProcessRequest(HttpContext context)
    {
        string postString = string.Empty;
        if (HttpContext.Current.Request.HttpMethod.ToUpper() == "GET")
        {
            Auth();
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    /// <summary>
    /// 处理微信服务器验证消息
    /// </summary>
    private void Auth()
    {
        string token = ConfigurationManager.AppSettings["WeixinToken"].ToString();
        string signature = HttpContext.Current.Request.QueryString["signature"];
        string timestamp = HttpContext.Current.Request.QueryString["timestamp"];
        string nonce = HttpContext.Current.Request.QueryString["nonce"];
        string echostr = HttpContext.Current.Request.QueryString["echostr"];

        if (HttpContext.Current.Request.HttpMethod.ToUpper() == "GET")
        {
            //get method - 仅在微信后台填写URL验证时触发
            if (CheckSignature(signature, timestamp, nonce, token))
            {
                WriteContent(echostr); //返回随机字符串则表示验证通过
            }
            else
            {
                WriteContent("failed:" + signature + "," + GetSignature(timestamp, nonce, token) + "。" +
                            "如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url，请注意保持Token一致。");
            }
            HttpContext.Current.Response.End();
        }
    }



    private void WriteContent(string str)
    {
        HttpContext.Current.Response.Output.Write(str);
    }


    /// <summary>
    /// 检查签名是否正确
    /// </summary>
    /// <param name="signature"></param>
    /// <param name="timestamp"></param>
    /// <param name="nonce"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public static bool CheckSignature(string signature, string timestamp, string nonce, string token)
    {
        return signature == GetSignature(timestamp, nonce, token);
    }

    /// <summary>
    /// 返回正确的签名
    /// </summary>
    /// <param name="timestamp"></param>
    /// <param name="nonce"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public static string GetSignature(string timestamp, string nonce, string token)
    {
        string[] arr = new[] { token, timestamp, nonce }.OrderBy(z => z).ToArray();
        string arrString = string.Join("", arr);
        System.Security.Cryptography.SHA1 sha1 = System.Security.Cryptography.SHA1.Create();
        byte[] sha1Arr = sha1.ComputeHash(Encoding.UTF8.GetBytes(arrString));
        StringBuilder enText = new StringBuilder();
        foreach (var b in sha1Arr)
        {
            enText.AppendFormat("{0:x2}", b);
        }
        return enText.ToString();
    }

}