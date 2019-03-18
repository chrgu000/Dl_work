using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Configuration;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using BLL;
using System.Data;
using System.Security.Cryptography;


namespace DingDan_WebForm.test
{
    public partial class wx : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            JObject jo = new JObject();
            WeiXin8222.WeiXinSoapClient wx8222 = new WeiXin8222.WeiXinSoapClient();
            WeiXin9002.WeiXinSoapClient wx9002 = new WeiXin9002.WeiXinSoapClient();
            SMS9002.SendSMS2CustomerSoapClient sms9002 = new SMS9002.SendSMS2CustomerSoapClient();
            string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx85ee38394e42f0b7&redirect_uri=http://dl.duolian.com:8001/WeiXin/html/RoleCheck.html&response_type=code&scope=snsapi_userinfo&state=order_info.html?orderCode=/uLGzE4OuCyeeQEMSoFXyU1bxGCddObpyr0uKmusnxi1EdwL/7bamg==&connect_redirect=1#wechat_redirect";
            wx9002.SendMsg_TextCard("13438904933", "", "", "1000003", "订单审核", "你的普通订单23423446890已审核",url);
            //   sms9002.SendSMS("13438904933", "sdfa");
          //wx9002.SendMsg_TextCard_EncryUrl("13438904933|15308078836", "", "", "1000003", "订单审核", "你的特殊订单23423446890已审核", "2217", "2");
          //wx9002.SendMsg_TextCard_EncryUrl("13438904933|15308078836", "", "", "1000003", "订单审核", "你的普通订单23423446890已审核", "114", "1");
          // string timestamp= getTimestamp();
          // string a = getNoncestr();
          //jo =JObject.Parse( wx8222.Get_jsapi_ticket());
          //string ticket = jo["ticket"].ToString();
          // Response.Write(ticket);
          // Response.Write(timestamp);
            //   WeiXin8222.WeiXinSoapClient wx8222 = new WeiXin8222.WeiXinSoapClient();
            //   JObject jo = new JObject();
            //   jo["orderId"]="2217";
            //   jo["orderType"] = "2";
            //   string orderCode = new Check().EncryptDES(JsonConvert.SerializeObject(jo));
            //string a=   wx8222.SendMsg_TextCard("13438904933", "", "", "1000003", "订单审核", "你的订单123123123已审核", "http://dl.duolian.com:8001/mobile/html/order_info.html?orderCode=" + orderCode);
            //Response.Write(a);
            //   Response.Write("http://dl.duolian.com:8001/mobile/html/order_info.html?orderCode=" + orderCode);
            //string a = "admin";
            //string b = AESEncrypt(a, "duoliana");
            //Response.Write(b);
            // string a=   new BLL.Check().EncryptDES("abc");
            // string b = new BLL.Check().DecryptDES("U2FsdGVkX1+1OFBWl88kw4pzGEmJ9B3e");
            //   Response.Write(a);

 
  
        }

        public string getNoncestr()
        {
            Random random = new Random();
            string charset = "GBK";
            string encypStr = random.Next(1000).ToString();
            string retStr;
            MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();

            //创建md5对象
            byte[] inputBye;
            byte[] outputBye;

            //使用GB2312编码方式把字符串转化为字节数组．
            try
            {
                inputBye = Encoding.GetEncoding(charset).GetBytes(encypStr);
            }
            catch (Exception ex)
            {
                inputBye = Encoding.GetEncoding("GB2312").GetBytes(encypStr);
            }
            outputBye = m5.ComputeHash(inputBye);

            retStr = System.BitConverter.ToString(outputBye);
            retStr = retStr.Replace("-", "").ToUpper();
            return retStr;
        }


        public string getTimestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        public string GetWebRequest(string getUrl)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(getUrl);

            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            httpWebRequest.Timeout = 20000;

            //byte[] btBodys = Encoding.UTF8.GetBytes(body);
            //httpWebRequest.ContentLength = btBodys.Length;
            //httpWebRequest.GetRequestStream().Write(btBodys, 0, btBodys.Length);

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
            string responseContent = streamReader.ReadToEnd();

            httpWebResponse.Close();
            streamReader.Close();
            return responseContent;
        }

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
        /// AES加密(无向量)  
        /// </summary>  
        /// <param name="plainBytes">被加密的明文</param>  
        /// <param name="key">密钥</param>  
        /// <returns>密文</returns>  
        public static string AESEncrypt(String Data, String Key)
        {
            MemoryStream mStream = new MemoryStream();
            RijndaelManaged aes = new RijndaelManaged();

            byte[] plainBytes = Encoding.UTF8.GetBytes(Data);
            Byte[] bKey = new Byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(Key.PadRight(bKey.Length)), bKey, bKey.Length);

            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            aes.KeySize = 128;
            //aes.Key = _key;  
            aes.Key = bKey;
            //aes.IV = _iV;  
            CryptoStream cryptoStream = new CryptoStream(mStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
            try
            {
                cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                cryptoStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            finally
            {
                cryptoStream.Close();
                mStream.Close();
                aes.Clear();
            }
        }
    }
}