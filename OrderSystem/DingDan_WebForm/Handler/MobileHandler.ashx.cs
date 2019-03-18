using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Data;
using BLL;
using System.Web.SessionState;
using System.Text;
using Model;
using System.Web.Script.Serialization;
using System.ServiceModel.Web;
using System.Runtime.Serialization.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Data.SqlClient;
using System.Configuration;
using System.Net;
using System.Threading;
using System.Text.RegularExpressions;



namespace DingDan_WebForm.Handler
{
    /// <summary>
    /// MobileHandler 的摘要说明
    /// </summary>
    public class MobileHandler : MobileBase
    {

        product pro = new product();
        AdminManager am = new AdminManager();
        OrderManager order = new OrderManager();
        Check check = new Check();
        WeiXin weixin = new WeiXin();
        IsoDateTimeConverter timejson = new IsoDateTimeConverter
        {
            DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss"
        };
        WeiXin9003.WeiXinSoapClient WeiXin9003 = new WeiXin9003.WeiXinSoapClient();
        public ReInfo errMsg = new ReInfo()
        {
            list_msg = new List<string>(),
            flag = "0",
            message = "程序出现错误,请重试或联系管理员!!"
        };


        public override void AjaxProcess(HttpContext context)
        {
            ReInfo ri = new ReInfo();
            string Action = HttpContext.Current.Request.Form["Action"];
            if (string.IsNullOrEmpty(Action))
            {
                ri.flag = "0";
                ri.message = "错误的方法!";
                context.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }
            var method = this.GetType().GetMethod(Action);
            if (method == null)
            {
                ri.flag = "0";
                ri.message = "错误的方法!";
                context.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }
            try
            {
                method.Invoke(this, new object[] { });
            }
            catch (Exception err)
            {

                errMsg.list_msg.Add("Action:" + HttpContext.Current.Request.Form["Action"]);
                errMsg.list_msg.Add(err.ToString());
                check.WriteLog(errMsg);
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(errMsg));
                return;
            }


        }


        //#region 销售经理更新违约金意见
        //public void Update_WYJ()
        //{
        //    string code = HttpContext.Current.Request.Form["code"];
        //    string wyjvalue = HttpContext.Current.Request.Form["WYJValue"];
        //    string id = HttpContext.Current.Session["lngopUserId"].ToString();
        //    ReInfo ri = am.Update_WYJ(id, wyjvalue, code);
        //    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
        //    return;

        //}
        //#endregion


        //#region 销售经理更新停止发货意见
        //public void Update_Stop()
        //{
        //    string code = HttpContext.Current.Request.Form["code"];
        //    string stopvalue = HttpContext.Current.Request.Form["StopValue"];
        //    string id = HttpContext.Current.Session["lngopUserId"].ToString();
        //    string cMemo = HttpContext.Current.Request.Form["cMemo"].ToString();
        //    ReInfo ri = am.Update_Stop(id, stopvalue, cMemo, code);
        //    if (ri.flag == "1")
        //    {
        //        //获取该通知单主账号电话
        //        DataTable dt = am.Get_PhoneByArrearCode(code);
        //        string msg = string.Empty;
        //        if (dt.Rows[0]["bytStatus"].ToString() == "22")
        //        {
        //            msg = "您有一张延期未结清账款通知单，请于两日内登录网上订单系统进行确认。如未按期确认，将影响您网上下单业务以及年终考评，感谢您的支持！";
        //        }
        //        else if (dt.Rows[0]["bytStatus"].ToString() == "23")
        //        {
        //            msg = "您的货款已连续三月未结清，系统已自动控制下单。如有疑问，请及时咨询销售部负责人！";
        //        }

        //        string nums = dt.Rows[0]["cCusPhone"].ToString();
        //        string[] num_arr = nums.Split(';');
        //        nums = string.Join(",", num_arr);
        //        SMSSend9003.SendSMS(nums, msg);
        //        foreach (var num in num_arr)
        //        {
        //            SMSSend9003.SendQY_Message_Text(num, "", "", "20", msg);
        //        }
        //    }
        //    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
        //    return;

        //}
        //#endregion


        //#region 销售经理确认通知单
        //public void Arrear_confirm()
        //{
        //    ReInfo ri=new ReInfo();
        //    string code = HttpContext.Current.Request.Form["code"];
        //    string id = HttpContext.Current.Session["lngopUserId"].ToString();
        //    string bytstatus = HttpContext.Current.Request.Form["bytStatus"];
        //    if (bytstatus!="31"&&bytstatus!="32")
        //    {
        //        ri.flag = "0";
        //        ri.message = "该通知单状态不正确！";
        //        HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
        //        return;
        //    }
        //    int byt=0;
        //    if (bytstatus == "31")
        //    {

        //        byt = 41;
        //        ri = am.Arrear_confirm(id, code, byt);
        //    }
        //    else if (bytstatus == "32")
        //    {
        //        if (HttpContext.Current.Request.Form["stopValue"] == "0")
        //        {
        //            byt = 42;
        //        }
        //        else
        //        {
        //            byt = 43;
        //        }
        //        ri = am.Arrear_confirm(id, code, byt, HttpContext.Current.Request.Form["stopValue"], HttpContext.Current.Request.Form["cMemo"]);
        //        if (ri.flag=="1")
        //        {

        //                 //获取该通知单主账号电话
        //        DataTable dt = am.Get_PhoneByArrearCode(code);
        //        string msg = string.Empty;
        //        if (byt==42)
        //        {
        //            msg = "您的网上下单业务已经恢复，感谢您的支持！";
        //        }
        //        else if (byt==43)
        //        {
        //            msg = "您的货款已连续三月未结清，系统已自动控制下单。如有疑问，请及时咨询销售部负责人！";
        //        }

        //        string nums = dt.Rows[0]["cCusPhone"].ToString();
        //        string[] num_arr = nums.Split(';');
        //        nums = string.Join(",", num_arr);
        //        SMSSend9003.SendSMS(nums, msg);
        //        foreach (var num in num_arr)
        //        {
        //            SMSSend9003.SendQY_Message_Text(num, "", "", "20", msg);
        //        }
        //        }
        //    }


        //    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
        //    return;

        //}
        //#endregion

        //#region 获取延期通知单列表
        //public void Get_ArrearList()
        //{
        //    ReInfo ri = new ReInfo();
        //    string start_date = HttpContext.Current.Request.Form["start_date"];
        //    string end_date = HttpContext.Current.Request.Form["end_date"];
        //    string bytstatus = HttpContext.Current.Request.Form["bytstatus"];
        //    ri.dt = am.Get_ArrearList(start_date, end_date, bytstatus);
        //    ri.flag = "1";
        //    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
        //}
        //#endregion

        #region 微信登录
        public void Wx_Login()
        {
            ReInfo ri = new ReInfo();
            string code = HttpContext.Current.Request.Form["code"];
            //string re = new WeiXin8222.WeiXinSoapClient().Get_UserIdByAuthCode(code);
            string appid = ConfigurationManager.AppSettings["WeiXinAPPID"];
            string re = WeiXin9003.Get_UserIdByAuthCode_ByAPPID(code, appid);
            JObject jo = (JObject)JsonConvert.DeserializeObject(re);

            if (!string.IsNullOrEmpty((string)jo["UserId"]))
            {
                string manager = ConfigurationManager.AppSettings["Manager"];
                string[] manager_arr = manager.Split('|');
                int d = System.Array.IndexOf(manager_arr, (string)jo["UserId"]);
                if (d == -1)
                {
                    ri.flag = "0";
                    ri.message = "没有权限！";
                    ri.msg = new string[]{
                                    code,
                                    (string)jo["UserId"]
                                  };

                    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                    return;
                }
                else
                {

                    DataTable dt = check.GetIdByPhone((string)jo["UserId"]);
                    if (dt.Rows.Count == 0)
                    {
                        ri.flag = "0";
                        ri.message = "用户表没有数据！";
                    }
                    else
                    {
                        ri.flag = "1";
                        HttpContext.Current.Session["WXUserId"] = (string)jo["UserId"];
                        HttpContext.Current.Session["AdminlngopUserId"] = dt.Rows[0]["lngopUserId"].ToString();
                        HttpContext.Current.Session["AdminstrLoginName"] = dt.Rows[0]["strLoginName"].ToString();

                    }
                    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                    return;
                }

            }
            else
            {
                ri.flag = "0";
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }

        }
        #endregion

        #region 微信登录
        public void Wx_Login_New()
        {
            string code = HttpContext.Current.Request.Form["code"];

            string appid = ConfigurationManager.AppSettings["WeiXinAPPID"];
            string re = WeiXin9003.Get_UserIdByAuthCode_ByAPPID(code, appid);
            JObject jo = (JObject)JsonConvert.DeserializeObject(re);
            if (!string.IsNullOrEmpty((string)jo["UserId"]))
            {
                string manager = ConfigurationManager.AppSettings["Manager"];
                string[] manager_arr = manager.Split('|');
                int d = System.Array.IndexOf(manager_arr, (string)jo["UserId"]);
                if (d == -1)
                {
                    jo["flag"] = 0;


                    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                    return;
                }
                else
                {

                    DataTable dt = check.GetIdByPhone((string)jo["UserId"]);
                    if (dt.Rows.Count == 0)
                    {
                        jo["flag"] = 0;
                        jo["message"] = "用户表没有数据！";
                    }
                    else
                    {
                        jo["flag"] = 1;
                        HttpContext.Current.Session["WXUserId"] = (string)jo["UserId"];
                        HttpContext.Current.Session["AdminlngopUserId"] = dt.Rows[0]["lngopUserId"].ToString();
                        HttpContext.Current.Session["AdminstrLoginName"] = dt.Rows[0]["strLoginName"].ToString();

                    }
                    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                    return;
                }

            }
            else
            {
                jo["flag"] = 0;

                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                return;
            }

        }
        #endregion

        #region 微信登录,不通过网上订单dl_opuser表
        public void Wx_Login_NoLogin()
        {
            string code = HttpContext.Current.Request.Form["code"];

            string appid = ConfigurationManager.AppSettings["MobileAPPID"];
            string re = WeiXin9003.Get_UserIdByAuthCode_ByAPPID(code, appid);
            JObject jo = (JObject)JsonConvert.DeserializeObject(re);
            if (!string.IsNullOrEmpty((string)jo["UserId"]))
            {

                jo["flag"] = 1;
                HttpContext.Current.Session["WXUserId"] = (string)jo["UserId"];
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                return;
            }
            else
            {
                jo["flag"] = 0;
                jo["message"] = "微信登录失败，请重试或联系管理员！";
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                return;
            }

        }
        #endregion

        #region 获取安全库存信息
        public void GetSafeStock()
        {
            JObject jo = new JObject();
            string Now = HttpContext.Current.Request.Form["date"];
            if (Now == null || Now == "")
            {
                Now = DateTime.Now.ToString("yyyyMMdd");
            }
            jo["flag"] = 1;
            jo["data"] = JToken.Parse(JsonConvert.SerializeObject(new WeiXin().GetSafeStock(Now)));
            jo["message"] = Now;
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 获取采购申请单未到货入库的数据
        public void DLproc_PUAppWarnBySel()
        {
            JObject jo = new JObject();
            jo["flag"] = 1;
            jo["data"] = JToken.Parse(JsonConvert.SerializeObject(new WeiXin().DLproc_PUAppWarnBySel("20180201"), timejson));
            jo["message"] = HttpContext.Current.Session["WXUserId"].ToString();
            // jo["message"] = "13730658316";
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 按cCode查询已入库采购单详情
        public void DLproc_PUAppDetail()
        {
            string cCode = HttpContext.Current.Request.Form["code"];

            JObject jo = new JObject();
            if (HttpContext.Current.Session["WXUserId"] == null)
            {
                jo["flag"] = 0;
                jo["message"] = "请从微信登录！";
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                return;
            }
            jo["flag"] = 1;
            jo["data"] = JToken.Parse(JsonConvert.SerializeObject(weixin.DLproc_PUAppDetail(cCode), timejson));
            jo["message"] = HttpContext.Current.Session["WXUserId"].ToString();
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        public void t()
        {
            string code = HttpContext.Current.Request.Form["code"];
            //string re = new WeiXin8222.WeiXinSoapClient().Get_UserIdByAuthCode(code);
            //   string re = new WeiXin9003.WeiXinSoapClient().Get_UserIdByAuthCode(code);
            JObject jo = new JObject();
            jo["data"] = code;
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }

        public void Upload()
        {
            JObject jo = new JObject();
            jo["flag"] = 0;
            jo["message"] = "用户表没有数据！";
            var a = HttpContext.Current.Request.Files[0];
            jo["code"] = 0;
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            string filePath = System.AppDomain.CurrentDomain.BaseDirectory + "test/img/";
            string fileName = DateTime.Now.ToString("yyyyMMddhhmmss") + "_" + a.FileName;
            a.SaveAs(filePath + fileName);
            return;
        }

        private string GetTimeStamp(DateTime dt)
        {
            DateTime dateStart = new DateTime(1970, 1, 1, 8, 0, 0);
            int timeStamp = Convert.ToInt32((dt - dateStart).TotalSeconds);
            return timeStamp.ToString();
        }



        //投票--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public void vote()
        {
            string url = "http://www.paihang360.com/mzt/slgd2017/index.jsp?record_id=147614";
            string postData = "op=op_submit";
            JObject jo = new JObject();
            jo["startTime"] = DateTime.Now.ToString();
            //Random ran = new Random();
            //int RandKey = ran.Next(5000, 10000);
            //Thread.Sleep(RandKey);
            string res = HttpPost(url, postData);
            if (res.Contains("投票成功"))
            {
                jo["message"] = "投票成功";

                //int b = res.IndexOf("vote-num");
                //jo["now_num"] = res.Substring(b + 41, 5);
                //Regex regex = new Regex(@"\d{5}", RegexOptions.IgnoreCase);
                //Match m = regex.Match(res.Substring(b, 100));
                //jo["i"] = m.Value;
                //jo["b"] = b;

            }
            else
            {
                jo["message"] = "投票失败";

            }
            jo["html"] = res;
            jo["flag"] = 1;
            //jo["sleep"] = RandKey;
            jo["endTime"] = DateTime.Now.ToString();
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }

        //HttpPost方法
        //body是要传递的参数,格式"roleId=1&uid=2"
        //post的cotentType填写:
        //"application/x-www-form-urlencoded"
        //soap填写:"text/xml; charset=utf-8"
        //http登录post
        private string HttpPost(string Url, string Body)
        {
            string ResponseContent = "";
            //  var uri = new Uri(Url, true);
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
            httpWebRequest.Headers["X-Forwarded-For"] = getRandomIp();
            httpWebRequest.UserAgent = rand_UserAgent();
            httpWebRequest.ContentType = "application/x-www-form-urlencoded"; ;
            httpWebRequest.Method = "POST";
            httpWebRequest.Timeout = 20000; //setInstanceFollowRedirects
            // httpWebRequest.MediaType = "json";

            byte[] btBodys = Encoding.UTF8.GetBytes(Body);
            // httpWebRequest.ContentLength = btBodys.Length;
            httpWebRequest.GetRequestStream().Write(btBodys, 0, btBodys.Length);
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("GB2312"));
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




    }
}