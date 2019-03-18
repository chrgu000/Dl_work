using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Text;
using System.Security.Cryptography;
 

namespace DingDan_WebForm.test
{
    public partial class test : System.Web.UI.Page
    {
        SignalR.MyHub MyHub = new MyHub();
        DAL.SQLHelper1 sh = new DAL.SQLHelper1();

 
        protected void Page_Load(object sender, EventArgs e)
        {
     
              
        }

        [System.Web.Services.WebMethod]
        public static string asdf()
        {
            string reportStatus = HttpContext.Current.Request.Form["reportStatus"];
            string mobile = HttpContext.Current.Request.Form["mobile"];
            string submitDate = HttpContext.Current.Request.Form["submitDate"];
            //string receiveDate = HttpContext.Current.Request.Form["receiveDate"];
            string receiveDate = DateTime.Now.ToString();
            string errorCode = HttpContext.Current.Request.Form["errorCode"];
            string msgGroup = HttpContext.Current.Request.Form["msgGroup"];
            bool b = SMSReport(mobile, submitDate, receiveDate, errorCode, msgGroup, reportStatus);
            JObject jo = new JObject();
            if (b)
            {
                jo["flag"] = 1;
            }
            else
            {
                jo["flag"] = 0;
            }
            return JsonConvert.SerializeObject(jo);
        }

        public static bool SMSReport(string phone, string submitDate, string receiveDate, string errorCode, string msgGroup, string reportStatus)
        {
            bool b = false;
            string cmdText = "insert into Dl_opSMSReport (phone,submitDate,receiveDate,errorCode,msgGroup,reportStatus) values(@phone,@submitDate,@receiveDate,@errorCode,@msgGroup,@reportStatus)";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@phone",phone),
            new SqlParameter("@submitDate",submitDate),
            new SqlParameter("@receiveDate",receiveDate),
            new SqlParameter("@errorCode",errorCode),
            new SqlParameter("@msgGroup",msgGroup),
            new SqlParameter("@reportStatus",reportStatus)
       
            };
            int res = new DAL.SQLHelper().ExecuteNonQuery(cmdText, paras, CommandType.Text);
            if (res > 0)
            {
                b = true;
            }
            return b;
        }

        public string signalR_Login()
        {
            JObject info = new JObject();
            info["signalRId"] = HttpContext.Current.Request.QueryString["signalRId"];
            info["sessionId"] = HttpContext.Current.Session.SessionID.ToString();
            info["loginPhone"] = HttpContext.Current.Session["cCusPhone"].ToString();
            info["strLoginName"] = HttpContext.Current.Session["strLoginName"].ToString();
            info["strAllAcount"] = HttpContext.Current.Session["strAllAcount"].ToString();
            info["lngopUserId"] = HttpContext.Current.Session["lngopUserId"].ToString();
            info["time"] = DateTime.Now.ToString();

            MyHub.signalR_Login(info);

            return JsonConvert.SerializeObject(info);
        }

        [System.Web.Services.WebMethod]
        public static string tt()
        {

            JObject info = new JObject();
            info["signalRId"] = HttpContext.Current.Request.QueryString["signalRId"];
            info["sessionId"] = HttpContext.Current.Session.SessionID.ToString();
            info["loginPhone"] = HttpContext.Current.Session["cCusPhone"].ToString();
            info["strLoginName"] = HttpContext.Current.Session["strLoginName"].ToString();
            info["strAllAcount"] = HttpContext.Current.Session["strAllAcount"].ToString();
            info["lngopUserId"] = HttpContext.Current.Session["lngopUserId"].ToString();
            info["time"] = DateTime.Now.ToString();

            //   MyHub.signalR_Login(info);

            return JsonConvert.SerializeObject(info);
            //  return "this is tt";

            // return info;
            //  MyHub.send1();

        }

        protected void btn_Click(object sender, EventArgs e)
        {

            string a = new BLL.Check().DecryptDES(text1.Text);
            Response.Write(a);
        }
        protected void btn1_Click(object sender, EventArgs e)
        {
            JObject jo = new JObject();
            jo["orderType"] = 1;
            jo["orderId"] = @"新华社北京11月30日电11月29日，联合国举行“声援巴勒斯坦人民国际日”纪念大会，国家主席习近平向大会致贺电。
习近平在贺电中表示，巴勒斯坦问题是中东问题的根源性问题，攸关巴勒斯坦等中东各国长治久安和繁荣发展。早日实现巴勒斯坦问题全面公正解决，符合巴勒斯坦等地区各国人民利益，有利于促进世界和平稳定。
习近平指出，中国是巴以和平的坚定支持者，今年7月提出中方推动解决巴勒斯坦问题的四点主张，旨在推动巴以双方重建互信，争取早日重启和谈。中国作为联合国安理会常任理事国和负责任的大国，愿继续同国际社会一道，为早日实现中东全面、公正、持久和平作出不懈努力。";
            string a = new BLL.Check().EncryptDES(JsonConvert.SerializeObject(jo));
            Response.Write(a);
        }

        [System.Web.Services.WebMethod]
        public static string dd()
        {
            string xmlPath = "test/test.xml";
            string systemPath = System.AppDomain.CurrentDomain.BaseDirectory;
            string path = systemPath + xmlPath;
            DataSet ds = new DataSet();
            ds.ReadXml(path);

            JObject jo = new JObject();
            jo["flag"] = 1;
            jo["DataSet"] = JToken.Parse(JsonConvert.SerializeObject(ds));
            //    return jo;


            //  ds.WriteXml(systemPath + "test/1.xml");
            return JsonConvert.SerializeObject(jo);
        }




        [System.Web.Services.WebMethod]
        public static string ee()
        {
            string dataset = HttpContext.Current.Request.Form["dataset"];
            string systemPath = System.AppDomain.CurrentDomain.BaseDirectory;

            DataSet ds = new DataSet();
            // ds= JsonConvert.DeserializeObject(dataset);
            //ds = JToken.Parse(JsonConvert.DeserializeObject(dataset));

            JObject jo = new JObject();
            jo["flag"] = 1;
            jo["DataSet"] = JToken.Parse(JsonConvert.SerializeObject(ds));
            //    return jo;


            ds.WriteXml(systemPath + "test/1.xml");
            return JsonConvert.SerializeObject(jo);
        }

        [System.Web.Services.WebMethod]
        public static string ff(List<string> arr)
        {

            string a = "";
            foreach (var item in arr)
            {
                a += item;
            }
            arr.ToArray();
            JObject jo = new JObject();
            jo["name"] = arr[0];
            jo["sex"] = arr[1];
            return JsonConvert.SerializeObject(jo);
        }


        [System.Web.Services.WebMethod]
        public static string readJson(string name)
        {

            string systemPath = System.AppDomain.CurrentDomain.BaseDirectory;

            StreamReader myreader = File.OpenText(systemPath + "test/test.json");//读取记事本文件
            string s = "";
            s = myreader.ReadToEnd();//从当前位置读取到文件末尾
            JObject jo = new JObject();
            jo = JObject.Parse(s);
            myreader.Close();
            myreader.Dispose();
            jo["name"] = name;
            return JsonConvert.SerializeObject(jo);
        }

        [System.Web.Services.WebMethod]
        public static string writeJson(List<string> arr)
        {

            string a = "";
            foreach (var item in arr)
            {
                a += item;
            }
            arr.ToArray();
            JObject jo = new JObject();
            jo["name"] = arr[0];
            jo["sex"] = arr[1];
            return JsonConvert.SerializeObject(jo);
        }


        [System.Web.Services.WebMethod]
        public static string SendSms()
        {
            string url = "http://112.35.1.155:1992/sms/norsubmit";
            string ec_name = "四川多联实业有限公司";
            string ec_userid = "duo";
            string ec_password = "duolian";
            string sign = "eb2Lji2Uz";
            string addSerial = "";
            string mobiles = "13438904933";
            string content = "test";
            string TemplateId = "a6dbf258b63e4904adf964a6accf08a8";
            string result = string.Empty;
            JToken jo = new JObject();

            //生成mac
            // string MacString = ec_name + ec_userid + ec_password+TemplateId + mobiles +  sign + addSerial;
            string MacString = ec_name + ec_userid + ec_password + mobiles + content + sign + addSerial;

            string mac = GetMd5Hash(MacString);

            jo["MacString"] = MacString;
            jo["mac"] = mac;
            //return mac;
            // ecName，apId，secretKey，templateId，mobiles，params，sign，addSerial按照顺序拼接，然后通过md5(32位小写)计算后得出的值
            //生成json数据
            string body = "ecName=" + ec_name + "&apId=" + ec_userid + "&secretKey=" + ec_password + "&mobiles=" + mobiles + "&content=" + content + "&sign=" + sign + "&addSerial=" + addSerial + "&mac=" + mac;
            //     body = "{\"addSerial\":\"" + addSerial + "\",\"apId\":\"" + ec_userid + "\",\"content\":\"" + content + "\",\"ecName\":\"" + ec_name + "\",\"mac\":\"" + mac + "\",\"mobiles\":\"" + mobiles + "\",\"secretKey\":\"" + ec_password + "\",\"sign\":\"" + sign + "\",\"TemplateId\":\"" + TemplateId + "\"}";
            //将json数据转换成base64
            body = "{\"addSerial\":\"" + addSerial + "\",\"apId\":\"" + ec_userid + "\",\"content\":\"" + content + "\",\"ecName\":\"" + ec_name + "\",\"mac\":\"" + mac + "\",\"mobiles\":\"" + mobiles + "\",\"secretKey\":\"" + ec_password + "\",\"sign\":\"" + sign + "\"}";

            body = ToBase64(body);
            jo["body"] = body;

            //  result = "【base64】" + body + "<br />";

            //通过post方法提交短信，提交内容为转换好的base64
            // result= HttpPost(url, body)  ;
            result = HttpPost(url, body);
            jo["result"] = result;
            return JsonConvert.SerializeObject(jo);
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
            //  var uri = new Uri(Url, true);
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(Url);

            httpWebRequest.ContentType = "application/x-www-form-urlencoded"; ;
            httpWebRequest.Method = "POST";
            httpWebRequest.Timeout = 20000; //setInstanceFollowRedirects
            httpWebRequest.MediaType = "json";

            byte[] btBodys = Encoding.UTF8.GetBytes(Body);
            // httpWebRequest.ContentLength = btBodys.Length;
            httpWebRequest.GetRequestStream().Write(btBodys, 0, btBodys.Length);
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
            try
            {



                ResponseContent = streamReader.ReadToEnd();


            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                httpWebResponse.Close();
                streamReader.Close();
                httpWebRequest.Abort();
                httpWebResponse.Close();
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

        [System.Web.Services.WebMethod]
        public string Upload()
        {
            return "afd";
        }
    }


}