using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Timers;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using System.Text.RegularExpressions;
using System.Text;
using System.Configuration;

namespace DingDan_WebForm.SuperAdmin
{
    public partial class ll : System.Web.UI.Page
    {
        static List<string> list = new List<string>();
        public SMSSend9001.SendSMS2CustomerSoapClient SMSSend9001 = new SMSSend9001.SendSMS2CustomerSoapClient();

        protected void Page_Load(object sender, EventArgs e)
        {
            //  SMSSend9001.SendSMS("13438904933", "asdf");
            // GetWebRequest();
            //reg();
            Send(null,null);
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Enabled = true;
            timer.Interval = 1800000; //执行间隔时间,单位为毫秒; 
            //timer.Interval = 60000; //执行间隔时间,单位为毫秒; 

            timer.Start();
            
            timer.Elapsed += new System.Timers.ElapsedEventHandler(Send);




        }

        public string getWX()
        {
            string url = "http://weixin.sogou.com/weixin?type=1&s_from=input&query=%E4%B8%AD%E5%9B%BD%E9%A3%9F%E4%BA%8B%E8%8D%AF%E9%97%BB&ie=utf8&_sug_=n&_sug_type_=";
            string s = GetWebRequest(url);
            int start = s.IndexOf("account_name_0");
            int end = s.IndexOf("<!--red_beg-->");
            s = s.Substring(start + 22, end - start - 28);

            return s;
        }

        public void Send(object source, ElapsedEventArgs e)
        {
            try
            {
                WriteLog("==========================================开始=========================================================");
                string url = getWX();
                url = url.Replace("amp;", "");
                WriteLog(url);
                // Response.Write("url1:" + url);
                string responseContent = GetWebRequest(url);
                int start = responseContent.IndexOf("var msgList = ");
                int end = responseContent.IndexOf("}}]};");
                string s = responseContent.Substring(start + 14, (end - start - 10));
                JToken jt = JToken.Parse(s);
                JObject jo = JObject.Parse(s);
                JArray ja = JArray.Parse(jo["list"].ToString());
                if (list.Count == 0)
                {
                    foreach (var item in ja)
                    {
                        list.Add(item["app_msg_ext_info"]["title"].ToString());
                    }
                }
                else
                {
                    List<string> newlist = new List<string>();
                    List<string> sendlist = new List<string>();
                    foreach (var item in ja)
                    {
                        newlist.Add(item["app_msg_ext_info"]["title"].ToString());
                        if (Array.IndexOf(list.ToArray(), item["app_msg_ext_info"]["title"].ToString()) == -1)
                        {
                            sendlist.Add(item["app_msg_ext_info"]["title"].ToString());
                        }
                    }
                    list = newlist;
                    if (sendlist.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("中国食事药闻发布了" + sendlist.Count + "篇新文章：\r\n");
                        int i = 1;
                        foreach (var item in sendlist)
                        {
                            sb.Append((i++) + "：" + item + "\r\n");
                        }
                        //SMSSend9001.SendSMS("13438904933",sb.ToString());
                        WriteLog("sendlist:" + sb.ToString());

                        new SMSSend9003.SendSMS2CustomerSoapClient().SendSMS_Old("13438904933,13880724616", sb.ToString());
                        //new SMSSend9003.SendSMS2CustomerSoapClient().SendSMS_Old("13438904933", sb.ToString());

                    }
                    StringBuilder ss = new StringBuilder();
                    foreach (var item in newlist)
                    {
                        ss.Append(item + "\r\n");
                    }
                    WriteLog("newlist:" + ss.ToString());
                   

                }
            }
            catch (Exception err)
            {
                WriteLog(err.ToString());
            }
        }
        public string GetWebRequest(string url)
        {

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);


            httpWebRequest.Method = "GET";
            httpWebRequest.Timeout = 20000;
            httpWebRequest.Headers["X-Forwarded-For"] = getRandomIp();
            httpWebRequest.UserAgent = rand_UserAgent();


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


        public string getRandomIp()
        {
            int[][] range = {
                        new int[]{975044608,977272831},////58.30.0.0-58.63.255.255
                        new int[]{607649792,608174079},//36.56.0.0-36.63.255.255
                        new int[]{999751680,999784447},//59.151.0.0-59.151.127.255
                        new int[]{1019346944,1019478015}, //60.194.0.0-60.195.255.255
                        new int[]{1038614528,1039007743},//61.232.0.0-61.237.255.255
                        new int[]{1783627776,1784676351},//106.80.0.0-106.95.255.255
                        new int[]{1947009024,1947074559},//116.13.0.0-116.13.255.255
                        new int[]{1987051520,1988034559},//118.112.0.0-118.126.255.255
                        new int[]{2035023872,2035154943},//121.76.0.0-121.77.255.255
                        new int[]{2078801920,2079064063},//123.232.0.0-123.235.255.255
                        new int[]{-1950089216,-1948778497},//139.196.0.0-139.215.255.255
                        new int[]{-1425539072,-1425014785},//171.8.0.0-171.15.255.255
                        new int[]{-1236271104,-1235419137},//182.80.0.0-182.92.255.255
                        new int[]{-770113536,-768606209},//210.25.0.0-210.47.255.255
                        new int[]{-569376768,-564133889},//222.16.0.0-222.95.255.255
                  
                        };

            Random rdint = new Random();
            int index = rdint.Next(10);
            string ip = num2ip(range[index][0] + new Random().Next(range[index][1] - range[index][0]));
            return ip;
        }

        /*
         * 将十进制转换成ip地址
        */
        public string num2ip(int ip)
        {
            int[] b = new int[4];
            string x = "";
            //位移然后与255 做高低位转换
            b[0] = (int)((ip >> 24) & 0xff);
            b[1] = (int)((ip >> 16) & 0xff);
            b[2] = (int)((ip >> 8) & 0xff);
            b[3] = (int)(ip & 0xff);
            x = (b[0]).ToString() + "." + (b[1]).ToString() + "." + (b[2]).ToString() + "." + (b[3]).ToString();

            return x;
        }

        public string rand_UserAgent()
        {
            string[] arr = {
"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_8; en-us) AppleWebKit/534.50 (KHTML, like Gecko) Version/5.1 Safari/534.50",
"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-us) AppleWebKit/534.50 (KHTML, like Gecko) Version/5.1 Safari/534.50",
"Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0",
"Mozilla/5.0 (Macintosh; Intel Mac OS X 10.6; rv:2.0.1) Gecko/20100101 Firefox/4.0.1",
"Mozilla/5.0 (Windows NT 6.1; rv:2.0.1) Gecko/20100101 Firefox/4.0.1",
"Opera/9.80 (Macintosh; Intel Mac OS X 10.6.8; U; en) Presto/2.8.131 Version/11.11",
"Opera/9.80 (Windows NT 6.1; U; en) Presto/2.8.131 Version/11.11",
"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_7_0) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.56 Safari/535.11",
"Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E)",
"Opera/9.80 (Windows NT 5.1; U; zh-cn) Presto/2.9.168 Version/11.50",
"Mozilla/5.0 (Windows NT 5.1; rv:5.0) Gecko/20100101 Firefox/5.0",
"Mozilla/5.0 (Windows NT 5.2) AppleWebKit/534.30 (KHTML, like Gecko) Chrome/12.0.742.122 Safari/534.30",
"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/536.11 (KHTML, like Gecko) Chrome/20.0.1132.11 TaoBrowser/2.0 Safari/536.11",
"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.1 (KHTML, like Gecko) Chrome/21.0.1180.71 Safari/537.1 LBBROWSER",
"Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E; LBBROWSER)",
"Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; Trident/4.0; SV1; QQDownload 732; .NET4.0C; .NET4.0E; 360SE)",
"Mozilla/5.0 (Windows NT 5.1) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.84 Safari/535.11 SE 2.X MetaSr 1.0",
"Mozilla/5.0 (Windows NT 5.1) AppleWebKit/537.1 (KHTML, like Gecko) Chrome/21.0.1180.89 Safari/537.1",
"Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; Trident/4.0; SV1; QQDownload 732; .NET4.0C; .NET4.0E; SE 2.X MetaSr 1.0)",
"Opera/9.27 (Windows NT 5.2; U; zh-cn)",
"Opera/8.0 (Macintosh; PPC Mac OS X; U; en)",
"Mozilla/5.0 (Macintosh; PPC Mac OS X; U; en) Opera 8.0",
"Mozilla/5.0 (Windows; U; Windows NT 5.2) Gecko/2008070208 Firefox/3.0.1",
"Mozilla/5.0 (Windows; U; Windows NT 5.1) Gecko/20070309 Firefox/2.0.0.3",
"Mozilla/5.0 (Windows; U; Windows NT 5.1) Gecko/20070803 Firefox/1.5.0.12",
"Mozilla/4.0 (compatible; MSIE 12.0",
"Mozilla/5.0 (Windows NT 5.1; rv:44.0) Gecko/20100101 Firefox/44.0",
"MQQBrowser/26 Mozilla/5.0 (Linux; U; Android 2.3.7; zh-cn; MB200 Build/GRJ22; CyanogenMod-7) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1",
"Opera/9.80 (Android 2.3.4; Linux; Opera Mobi/build-1107180945; U; en-GB) Presto/2.8.149 Version/11.10",
"Mozilla/5.0 (Windows NT 6.1) AppleWebKit/536.11 (KHTML, like Gecko) Chrome/20.0.1132.57 Safari/536.11",  
"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-us) AppleWebKit/534.50 (KHTML, like Gecko) Version/5.1 Safari/534.50",  
"Mozilla/5.0 (Windows NT 10.0; WOW64; rv:38.0) Gecko/20100101 Firefox/38.0",  
"Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; .NET4.0C; .NET4.0E; .NET CLR 2.0.50727; .NET CLR 3.0.30729; .NET CLR 3.5.30729; InfoPath.3; rv:11.0) like Gecko",  
"Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0",  
"Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.0; Trident/4.0)",  
"Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0)",  
"Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)",  
"Mozilla/5.0 (Macintosh; Intel Mac OS X 10.6; rv:2.0.1) Gecko/20100101 Firefox/4.0.1",  
"Mozilla/5.0 (Windows NT 6.1; rv:2.0.1) Gecko/20100101 Firefox/4.0.1",  
"Opera/9.80 (Macintosh; Intel Mac OS X 10.6.8; U; en) Presto/2.8.131 Version/11.11",  
"Opera/9.80 (Windows NT 6.1; U; en) Presto/2.8.131 Version/11.11",  
"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_7_0) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.56 Safari/535.11",  
"Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; Maxthon 2.0)",  
"Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; TencentTraveler 4.0)",  
"Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)",  
 "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; The World)",  
"Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; 360SE)",  
"Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; Trident/4.0; SE 2.X MetaSr 1.0; SE 2.X MetaSr 1.0; .NET CLR 2.0.50727; SE 2.X MetaSr 1.0)",  
"Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; Avant Browser)",  
"Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)",   
"Mozilla/5.0 (iPhone; U; CPU iPhone OS 4_3_3 like Mac OS X; en-us) AppleWebKit/533.17.9 (KHTML, like Gecko) Version/5.0.2 Mobile/8J2 Safari/6533.18.5",  
"Mozilla/5.0 (iPod; U; CPU iPhone OS 4_3_3 like Mac OS X; en-us) AppleWebKit/533.17.9 (KHTML, like Gecko) Version/5.0.2 Mobile/8J2 Safari/6533.18.5",  
"Mozilla/5.0 (iPad; U; CPU OS 4_3_3 like Mac OS X; en-us) AppleWebKit/533.17.9 (KHTML, like Gecko) Version/5.0.2 Mobile/8J2 Safari/6533.18.5",  
"Mozilla/5.0 (Linux; U; Android 2.3.7; en-us; Nexus One Build/FRF91) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1",  
"MQQBrowser/26 Mozilla/5.0 (Linux; U; Android 2.3.7; zh-cn; MB200 Build/GRJ22; CyanogenMod-7) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1",  
"Opera/9.80 (Android 2.3.4; Linux; Opera Mobi/build-1107180945; U; en-GB) Presto/2.8.149 Version/11.10",  
"Mozilla/5.0 (Linux; U; Android 3.0; en-us; Xoom Build/HRI39) AppleWebKit/534.13 (KHTML, like Gecko) Version/4.0 Safari/534.13",  
"Mozilla/5.0 (BlackBerry; U; BlackBerry 9800; en) AppleWebKit/534.1+ (KHTML, like Gecko) Version/6.0.0.337 Mobile Safari/534.1+",  
"Mozilla/5.0 (hp-tablet; Linux; hpwOS/3.0.0; U; en-US) AppleWebKit/534.6 (KHTML, like Gecko) wOSBrowser/233.70 Safari/534.6 TouchPad/1.0",  
"NOKIA5700/ UCWEB7.0.2.37/28/999",  
"Openwave/ UCWEB7.0.2.37/28/999",  
"Mozilla/4.0 (compatible; MSIE 6.0; ) Opera/UCWEB7.0.2.37/28/999",  
"Mozilla/5.0 (Linux; Android 6.0; 1503-M02 Build/MRA58K) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/37.0.0.0 Mobile MQQBrowser/6.2 TBS/036558 Safari/537.36 MicroMessenger/6.3.25.861 NetType/WIFI Language/zh_CN"
                           };
            int index = new Random().Next(0, arr.Length); //生成随机下标
            return arr[index];  //取值

        }

        public void reg()
        {
            string str = "";


            int start = str.IndexOf("var msgList = ");
            int end = str.IndexOf("}}]}");
            string s = str.Substring(start, end - start);
            Response.Write(s);
            //Response.Write(end);

        }


        #region 错误日志记录
        public void WriteLog(string msg)
        {

            string sFilePath = System.AppDomain.CurrentDomain.BaseDirectory + "superadmin/log/";

            string sFileName = DateTime.Now.ToString("yyyy-MM-dd") + ".log";
            sFileName = sFilePath + "\\" + sFileName; //文件的绝对路径
            if (!Directory.Exists(sFilePath))//验证路径是否存在
            {
                Directory.CreateDirectory(sFilePath);
                //不存在则创建
            }
            FileStream fs;
            StreamWriter sw;
            if (File.Exists(sFileName))
            //验证文件是否存在，有则追加，无则创建
            {
                fs = new FileStream(sFileName, FileMode.Append, FileAccess.Write);
            }
            else
            {
                fs = new FileStream(sFileName, FileMode.Create, FileAccess.Write);
            }
            sw = new StreamWriter(fs);
            sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  :  " + msg + "\r\n");
            //foreach (var item in ri.list_msg)
            //{
            //    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "   ---   " + item);
            //}

            sw.Close();
            fs.Close();

        }
        #endregion



    }
}