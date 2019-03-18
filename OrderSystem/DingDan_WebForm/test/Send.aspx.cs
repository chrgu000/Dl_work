using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using System.Threading;
using System.Configuration;
 

namespace DingDan_WebForm.test
{
    public partial class Send : System.Web.UI.Page
    {
        public string ec_name = "";
        public string ec_userid = "";
        public string ec_password = "";
        public string sign = "";
        public string addSerial = "";
        public string mobiles = "";
        public string content = "";
        public string mac = "";
        public string url = "";

        protected void Page_Load(object sender, EventArgs e)
        {

        }



        protected void btn_send_Click(object sender, EventArgs e)
        {
            url = this.txt_url.Text;
            ec_name = this.txt_ecname.Text;
            ec_userid = this.txt_username.Text;
            ec_password = this.txt_password.Text;
            sign = this.txt_sgin.Text;
            addSerial = this.txt_addSerial.Text;
            mobiles = this.txt_mobiles.Text;
            content = this.txt_content.Text;

            //生成mac
            string MacString = ec_name + ec_userid + ec_password + mobiles + content + sign + addSerial;
            this.lbl_result.Text += "【macstring】" + MacString + "<br />";
            mac = GetMd5Hash(MacString);
            this.lbl_result.Text += "【mac】" + mac + "<br />";

            //生成json数据
            string body = "ecName=" + ec_name + "&apId=" + ec_userid + "&secretKey=" + ec_password + "&mobiles=" + mobiles + "&content=" + content + "&sign=" + sign + "&addSerial=" + addSerial + "&mac=" + mac;
            body = "{\"addSerial\":\"" + addSerial + "\",\"apId\":\"" + ec_userid + "\",\"content\":\"" + content + "\",\"ecName\":\"" + ec_name + "\",\"mac\":\"" + mac + "\",\"mobiles\":\"" + mobiles + "\",\"secretKey\":\"" + ec_password + "\",\"sign\":\"" + sign + "\"}";
            this.lbl_result.Text += "【json】" + body + "<br />";

            //将json数据转换成base64
            body = ToBase64(body);
            this.lbl_result.Text += "【base64】" + body + "<br />";

            //通过post方法提交短信，提交内容为转换好的base64
            this.lbl_result.Text += "【提交结果】" + HttpPost(url, body) + "<br />";
            //this.lbl_result.Text += Base64toString(body);

        }

        //ToBase64方法
        //将utf-8字符串转换成base64形式
        private static string ToBase64(string body)
        {
            byte[] b = System.Text.Encoding.UTF8.GetBytes(body);
            //转成 Base64 形式的 System.String  
            body = Convert.ToBase64String(b);
            return body;
        }

        //Base64toString方法
        //将base64形式转换成utf-8字符串
        private static string Base64toString(string base64_string)
        {
            byte[] c = Convert.FromBase64String(base64_string);
            base64_string = System.Text.Encoding.UTF8.GetString(c);
            return base64_string;
        }


        //HttpPost方法
        //body是要传递的参数,格式"roleId=1&uid=2"
        //post的cotentType填写:
        //"application/x-www-form-urlencoded"
        //soap填写:"text/xml; charset=utf-8"
        //http登录post
        private static string HttpPost(string Url, string Body)
        {
            string ResponseContent = "";
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(Url);

            httpWebRequest.ContentType = "application/x-www-form-urlencoded"; ;
            httpWebRequest.Method = "POST";
            httpWebRequest.Timeout = 20000; //setInstanceFollowRedirects
            httpWebRequest.MediaType = "json";

            byte[] btBodys = Encoding.UTF8.GetBytes(Body);
            httpWebRequest.ContentLength = btBodys.Length;
            httpWebRequest.GetRequestStream().Write(btBodys, 0, btBodys.Length);

            try
            {


                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
                ResponseContent = streamReader.ReadToEnd();

                httpWebResponse.Close();
                streamReader.Close();
                httpWebRequest.Abort();
                httpWebResponse.Close();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return ResponseContent;
        }

        //MD5计算mac
        public static string GetMd5Hash(String input)
        {
            if (input == null)
            {
                return null;
            }

            MD5 md5Hash = MD5.Create();

            // 将输入字符串转换为字节数组并计算哈希数据  
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // 创建一个 Stringbuilder 来收集字节并创建字符串  
            StringBuilder sBuilder = new StringBuilder();

            // 循环遍历哈希数据的每一个字节并格式化为十六进制字符串  
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // 返回十六进制字符串  
            return sBuilder.ToString();
        }
    }
}