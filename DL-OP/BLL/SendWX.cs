using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BLL
{
    public class SendWX
    {
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
        public string SendQYMessage(string paramData, Encoding dataEncode,string AccessToken)
        {
            //string accessToken = GetQYAccessToken(corpid, corpsecret);
            //string accessToken = "EU0GQZabqQ3Iybt4j94vndzDI_kN5lkSqNKC58hQF8yelta7c12MMwPetC5q-Hu8";
            //string[] asx = Application["WXAccessToken"].ToString().Split(',');
            //string accessToken = asx[0].ToString();
            string postUrl = string.Format("https://qyapi.weixin.qq.com/cgi-bin/message/send?access_token={0}", AccessToken);
            return PostWebRequest(postUrl, paramData, dataEncode);
        }


        protected void SendMsg(string touser, string content, string AccessToken, string corpapplicationid)
        {
            string responeJsonStr = "{";
            responeJsonStr += "\"touser\": \"" + touser.Trim().ToString() + "\",";
            responeJsonStr += "\"msgtype\": \"text\",";
            responeJsonStr += "\"agentid\": \"" + corpapplicationid.Trim().ToString() + "\",";
            responeJsonStr += "\"text\": {";
            responeJsonStr += "  \"content\": \"" + content.Trim().ToString() + "\"";
            responeJsonStr += "},";
            responeJsonStr += "\"safe\":\"0\"";
            responeJsonStr += "}";
            string result = SendQYMessage(responeJsonStr, Encoding.UTF8, AccessToken);
            //ASPxTextBox1.Text = SendQYMessage(responeJsonStr, Encoding.UTF8);
        }


    }
}
