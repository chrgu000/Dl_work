using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Collections;
using mas.ecloud.sdkclient;
using mas.ecloud.Model;
using System.Web.Services;
using Newtonsoft.Json;
using BLL;
using System.Net;
using System.IO;
using System.Data;
using Model;
using System.Configuration;
using Newtonsoft.Json.Linq;
using ThoughtWorks.QRCode.Codec;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;


/// <summary>
/// SendSMS2Customer 的摘要说明
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
// [System.Web.Script.Services.ScriptService]
public class SendSMS2Customer : System.Web.Services.WebService
{


    public SendSMS2Customer() { }

    public static Client client = Client.instance;
    bool loginResult = login();
    BLL.SearchManager searchManager = new SearchManager();


    [WebMethod(Description = "SendSMS_Old")]
    public bool SendSMS_Old(string num, string content)
    {
        string[] nums = num.Split(',');

        bool b = false;

        int result = 0;

        string msgGroup = Guid.NewGuid().ToString();

        result = send(nums, content, msgGroup);

      //  Insert_SMSRecord(num, content, result, msgGroup); //短信发送记录写入数据库

     //   Insert_Report(); //写入状态报告



        if (result == 1)
        {
            b = true;
        }

        return b;
    }


    #region 2017-09-25添加，移动短信发送新通道
    [WebMethod(Description = "SendSMS_New")]
    public bool SendSMS(string mobiles, string content)
    {
        string url = "http://112.35.1.155:1992/sms/norsubmit";
        string ec_name = "四川多联实业有限公司";
        string ec_userid = "duo";
        string ec_password = "duolian";
        string sign = "eb2Lji2Uz";
        string addSerial = "";
       // string TemplateId = "a6dbf258b63e4904adf964a6accf08a8";
        string result = string.Empty;
        JToken jo = new JObject();

        //生成mac
        string MacString = ec_name + ec_userid + ec_password + mobiles + content + sign + addSerial;

        string mac = GetMd5Hash(MacString);

        // ecName，apId，secretKey，templateId，mobiles，params，sign，addSerial按照顺序拼接，然后通过md5(32位小写)计算后得出的值
        //生成json数据
        string body = "ecName=" + ec_name + "&apId=" + ec_userid + "&secretKey=" + ec_password + "&mobiles=" + mobiles + "&content=" + content + "&sign=" + sign + "&addSerial=" + addSerial + "&mac=" + mac;
        //将json数据转换成base64
        body = "{\"addSerial\":\"" + addSerial + "\",\"apId\":\"" + ec_userid + "\",\"content\":\"" + content + "\",\"ecName\":\"" + ec_name + "\",\"mac\":\"" + mac + "\",\"mobiles\":\"" + mobiles + "\",\"secretKey\":\"" + ec_password + "\",\"sign\":\"" + sign + "\"}";

        body = ToBase64(body);


        //通过post方法提交短信，提交内容为转换好的base64
        result = HttpPost(url, body);
        jo["result"] = result;
        JToken j=JToken.Parse(result);
       //jo["msgGroup"]= j["msgGroup"];
       //jo["rspcod"] = j["rspcod"];
       int res = j["success"].ToString() == "True" ? 1 :0;
       Insert_SMSRecord(mobiles, content, result, j["msgGroup"].ToString(),j["rspcod"].ToString(),j["success"].ToString(),res);
        //return JsonConvert.SerializeObject(jo);
       if (res==1)
       {
           return true;
       }
       else
       {
           return false;
       }

    }
    #endregion

    [WebMethod]
    public void ReceiveSMSReport(string report) {
      bool  b = searchManager.SMSReport(report,"","","","","");
    }

    [WebMethod]
    public static bool login()
    {

        //  bool loginResult = client.login("http://mas.ecloud.10086.cn/app/sdk/login", "admin", "mTcpz*83", "四川多联实业有限公司");
        bool loginResult = client.login("http://mas.ecloud.10086.cn/app/sdk/login", "test", "test", "四川多联实业有限公司");
        return loginResult;
    }

    private int send(string[] nums, string content, string msgGroup)
    {
        // 发送短信 
        //int sendResult = client.sendDSMS(new string[] { "13438904933", "15308078836", "18280496975", "13518148295" }, "短信内容测试", "", 5, "7SBIGYp9","");
        int sendResult = client.sendDSMS(nums, content, "", 5, "a8rRwnMB", msgGroup);


        return sendResult;
    }

    #region 获取状态报告
    [WebMethod]
    public List<StatusReportModel> getreport()
    {

        List<StatusReportModel> list = client.getReport();
        // string a = JsonConvert.SerializeObject(list);
        return list;
    }
    #endregion



    #region 获取提交报告
    [WebMethod]
    public List<SubmitReportModel> getSubmitReport()
    {
        List<SubmitReportModel> list = client.getSubmitReport();
        string a = JsonConvert.SerializeObject(list);
        return list;
    }

    #endregion


    #region 状态报告写入数据库
    [WebMethod]
    public bool Insert_Report()
    {
        List<StatusReportModel> list = getreport();
        bool b = false;
        if (list.Count > 0)
        {
            foreach (var item in list)
            {
                b = searchManager.SMSReport(item.mobile, item.submitDate, item.receiveDate, item.errorCode, item.msgGroup, item.reportStatus);
            }
        }
        return b;
    }
    #endregion

    /// <summary>
    /// 短信发送记录写入数据库
    /// </summary>
    /// <param name="phone">发送电话</param>
    /// <param name="content">发送内容</param>
    /// <param name="result">返回结果</param>
    /// <param name="msgGroup">唯一码</param>
    /// <returns></returns>
    #region 短信发送记录写入数据库
    public bool Insert_SMSRecord(string phone, string content, string resultStr, string msgGroup, string rspcod, string success,int res)
    {
        bool b = searchManager.SMSRecord(phone, content, resultStr, msgGroup, rspcod, success,res);
        return b;
    }
    #endregion


    #region 短信发送记录写入数据库
    public bool Insert_SMSRecord(string phone, string content, int result, string msgGroup)
    {
        bool b = searchManager.SMSRecord(phone, content, result, msgGroup);
        return b;
    }
    #endregion

    /// <summary>
    /// SendFHSMS2Customer(发送未发货短信)
    /// </summary>
    /// <param name="phoneno"></param>
    /// <param name="orderno"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    [WebMethod(Description = "SendFHSMS2Customer")]
    public string SendFHSMS2Customer(String phoneno, String orderno, String content)
    {
        #region MyRegion
        ////GB2312转换成UTF8
        ////声明字符集   
        //System.Text.Encoding utf8, gb2312;
        ////gb2312   
        //gb2312 = System.Text.Encoding.GetEncoding("gb2312");
        ////utf8   
        //utf8 = System.Text.Encoding.GetEncoding("utf-8");
        //byte[] gb;
        //gb = gb2312.GetBytes(smstext);
        //gb = System.Text.Encoding.Convert(gb2312, utf8, gb);
        ////返回转换后的字符   
        //string txt = utf8.GetString(gb);

        ////UTF8转换成GB2312
        ////声明字符集   
        //System.Text.Encoding utf8, gb2312;
        ////utf8   
        //utf8 = System.Text.Encoding.GetEncoding("utf-8");
        ////gb2312   
        //gb2312 = System.Text.Encoding.GetEncoding("gb2312");
        //byte[] utf;
        //utf = utf8.GetBytes(smstext);
        //utf = System.Text.Encoding.Convert(utf8, gb2312, utf);
        ////返回转换后的字符   
        //string txt = gb2312.GetString(utf);
        #endregion

        SendSMS sms = new SendSMS();
        string s = phoneno;
        string sendsms = "";
        string newcontent = "";
        if (s != "")
        {
            #region 循环发送
            //string[] sArray = s.Split(';');
            //for (int i = 0; i < sArray.Length; i++)
            //{
            //    ////创建一行
            //    //DataRow row = phone.NewRow();
            //    ////将此行添加到table中
            //    //phone.Rows.Add(i + 1, sArray[i].ToString());
            //    ////if (i==9) //限制电话号码个数
            //    ////{
            //    ////    i = 999;
            //    ////}
            //    ////20160727,直接发送消息
            //    bool c = sms.SingleSend(sArray[i].ToString(), smstext);
            //}      
            #endregion

            //将发送内容拆分换行，接收的格式为
            //1:联多环保芯层发泡降噪管110x3.2,数量:72.00米;2:联多环保排水管160x2.8,数量:280.00米;3:阻燃普通冷弯管（C型）16/3.8米,数量:8550.00米;5:联多排水管件90°弯头160,数量:60.00个;6:联多环保芯层发泡降噪管110x3.2,数量:72.00米;7:联多环保排水管160x2.8,数量:280.00米;8:阻燃普通冷弯管（C型）16/3.8米,数量:8550.00米;9:联多排水管件90°弯头160,数量:60.00个
            //将 ; 替换为换行符

            String str = content;
            String strWxMessage = content;
            String[] sArray = str.Split(';');//split就是以传进去的字符进行分割 
            //for (int i = 0; i < sArray.Length; i++)
            //{
            //    if ((newcontent + sArray[i].ToString()).Length < 130)
            //    {
            //        newcontent = newcontent + sArray[i].ToString() + "\r\n";
            //    }
            //    else
            //    {
            //        #region 批量发送，电话号码格式为xxx，xxx，xxx
            //        sendsms = "【温馨提示】尊敬的客户：您好！您的订单（" + orderno + "）有如下产品未装完：\r\n" + newcontent + "以上信息请您知悉，如有正好是您急需的产品，请您在十分钟内与公司客服专员联系，联系电话：4008786333转3，如果未按约定时间回复，公司将会默认您同意此装车方式。";
            //      //  bool c = sms.SingleSend(s, sendsms);
            //        bool c = SendSMS(s, sendsms);
            //        #endregion
            //        newcontent = sArray[i].ToString() + "\r\n";
            //    }
            //}
            for (int i = 0; i < sArray.Length; i++)
            {
                newcontent = newcontent + sArray[i].ToString() + "\r\n";
            }

            sendsms = "【温馨提示】尊敬的客户：您好！您的订单（" + orderno + "）有如下产品未装完：\r\n" + newcontent + "以上信息请您知悉，如有正好是您急需的产品，请您在十分钟内与公司客服专员联系，联系电话：4008786333转3，如果未按约定时间回复，公司将会默认您同意此装车方式。";
            bool v = SendSMS(s, sendsms);
            strWxMessage = "订单（" + orderno + "）有如下产品未装完：" + strWxMessage;
            //  bool v = sms.SingleSend(s, sendsms);



            #region SendWeiXinMessage
            //对标签id为10的"未发货短信"进行群发消息
            //string touser = "15308078836";
            string totag = "10";
            string agentid = "22";
            string responeJsonStr = "{";
            //responeJsonStr += "\"touser\": \"" + touser + "\",";
            responeJsonStr += "\"totag\": \"" + totag + "\",";
            responeJsonStr += "\"msgtype\": \"text\",";
            responeJsonStr += "\"agentid\": \"" + agentid + "\",";
            responeJsonStr += "\"text\": {";
            responeJsonStr += "  \"content\": \"" + strWxMessage + "\"";
            responeJsonStr += "},";
            responeJsonStr += "\"safe\":\"0\"";
            responeJsonStr += "}";
            string WXRel = SendQYMessage("1", "2", responeJsonStr, Encoding.UTF8);
            #endregion
        }
        return phoneno + "!" + sendsms;
    }



    /// <summary>
    /// 推送信息（企业号，标签）
    /// </summary>
    /// <param name="corpid">企业号ID</param>
    /// <param name="corpsecret">管理组密钥</param>
    /// <param name="paramData">提交的数据json</param>
    /// <param name="dataEncode">编码方式</param>
    /// <returns></returns>
    public string SendQYMessage(string corpid, string corpsecret, string paramData, Encoding dataEncode)
    {
        // DataTable dt = new SearchManager().DL_GetWXCropIdBySel();
        // string accessToken = dt.Rows[0]["access_token"].ToString();
        if (Application["access_token"] == null)
        {
            Get_access_token();
        }

        string url = "https://qyapi.weixin.qq.com/cgi-bin/message/send?access_token={0}";
        string postUrl = string.Format("https://qyapi.weixin.qq.com/cgi-bin/message/send?access_token={0}", Application["access_token"]);
        string res = PostWebRequest(postUrl, paramData, dataEncode);
        JObject jo = (JObject)JsonConvert.DeserializeObject(res);
        if (jo["errcode"] != null && jo["errcode"].ToString() == "40014")
        {
            Get_access_token();
            res = PostWebRequest(string.Format(url, Application["access_token"]), paramData, dataEncode);
        }
        return res;
    }

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
    /// Get数据接口
    /// </summary>
    /// <param name="getUrl"></param>
    /// <returns></returns>
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

    [WebMethod]
    #region 获取企业号access_token并写入Application
    public string Get_access_token()
    {
        string url = " https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid=wx85ee38394e42f0b7&corpsecret=kdFAL0VNTGNxiCJJV-J202ZLLNz2VgQvYoh0GxvEtCVXUHmSCGFdNxBlgx-xOXAO";
        string res = GetWebRequest(url);
        JObject jo = (JObject)JsonConvert.DeserializeObject(res);
        Application["access_token"] = jo["access_token"].ToString();
        return Application["access_token"].ToString();
    }
    #endregion



    /// <summary>
    /// 推送信息（企业号，标签）text
    /// </summary>
    /// <param name="corpid">企业号ID</param>
    /// <param name="corpsecret">管理组密钥</param>
    /// <param name="paramData">提交的数据json</param>
    /// <param name="dataEncode">编码方式</param>
    /// <returns></returns>
    [WebMethod(Description = "SendQY_Message_Text")]
    public bool SendQY_Message_Text(string touser, string toparty, string totag, string agentid, string strWxMessage)
    {
        #region SendQY_Message
        //对标签id为10的"未发货短信"进行群发消息
        //string touser = "15308078836";
        //string totag = "10";
        //string agentid = "22";
        //        参数	必须	说明
        //touser	否	成员ID列表（消息接收者，多个接收者用‘|’分隔，最多支持1000个）。特殊情况：指定为@all，则向关注该企业应用的全部成员发送
        //toparty	否	部门ID列表，多个接收者用‘|’分隔，最多支持100个。当touser为@all时忽略本参数
        //totag	否	标签ID列表，多个接收者用‘|’分隔，最多支持100个。当touser为@all时忽略本参数
        //msgtype	是	消息类型，此时固定为：text （支持消息型应用跟主页型应用）
        //agentid	是	企业应用的id，整型。可在应用的设置页面查看
        //content	是	消息内容，最长不超过2048个字节，注意：主页型应用推送的文本消息在微信端最多只显示20个字（包含中英文）
        //safe	否	表示是否是保密消息，0表示否，1表示是，默认0
        string responeJsonStr = "{";
        responeJsonStr += "\"touser\": \"" + touser + "\",";
        responeJsonStr += "\"toparty\": \"" + toparty + "\",";
        responeJsonStr += "\"totag\": \"" + totag + "\",";
        responeJsonStr += "\"msgtype\": \"text\",";
        responeJsonStr += "\"agentid\": \"" + agentid + "\",";
        responeJsonStr += "\"text\": {";
        responeJsonStr += "  \"content\": \"" + strWxMessage + "\"";
        responeJsonStr += "},";
        responeJsonStr += "\"safe\":\"0\"";
        responeJsonStr += "}";
        string WXRel = SendQYMessage("1", "2", responeJsonStr, Encoding.UTF8);
        searchManager.WXMsgRecord(touser, responeJsonStr, WXRel, agentid, 1);
        bool b = false;
        WX_MSG msg = JsonConvert.DeserializeObject<WX_MSG>(WXRel);
        if (msg.Errmsg == "ok")
        {
            b = true;
        }
        return b;
        #endregion

    }


    /// <summary>
    /// 推送信息（企业号，标签）text
    /// </summary>
    /// <param name="corpid">企业号ID</param>
    /// <param name="corpsecret">管理组密钥</param>
    /// <param name="paramData">提交的数据json</param>
    /// <param name="dataEncode">编码方式</param>
    /// <returns></returns>
    [WebMethod(Description = "SendQY_Message_Image")]
    public bool SendQY_Message_Image(string touser, string toparty, string totag, string agentid, string title, string description, string url, string picurl)
    {
        #region SendQY_Message_Image

        string responeJsonStr = "{";
        responeJsonStr += "\"touser\": \"" + touser + "\",";
        responeJsonStr += "\"toparty\": \"" + toparty + "\",";
        responeJsonStr += "\"totag\": \"" + totag + "\",";
        responeJsonStr += "\"msgtype\": \"news\",";
        responeJsonStr += "\"agentid\": \"" + agentid + "\",";
        responeJsonStr += "\"news\": {";
        responeJsonStr += "  \"articles\": [{\"title\":\"" + title + "\",";
        responeJsonStr += "  \"description\":\"" + description + "\",";
        responeJsonStr += "  \"url\":\"" + url + "\",";
        responeJsonStr += "  \"picurl\":\"" + picurl + "\",";
        responeJsonStr += "  \"btntxt\":\"查看详情\"";
        responeJsonStr += "}]}}";

        string WXRel = SendQYMessage("1", "2", responeJsonStr, Encoding.UTF8);
        searchManager.WXMsgRecord(touser, responeJsonStr, WXRel, agentid, 1);
        bool b = false;
        WX_MSG msg = JsonConvert.DeserializeObject<WX_MSG>(WXRel);
        if (msg.Errmsg == "ok")
        {
            b = true;
        }
        return b;
        #endregion
    }

    /// <summary>
    /// 生成二维码，并保存在当前目录下，用于预约查询（专用）
    /// </summary>
    /// <param name="content">格式应该为 "http://dl.duolian.com:1234/MAAQROrderInfo.aspx?MAACode=''"</param>
    /// <param name="MAACode">预约单号</param>
    /// <returns></returns>
    [WebMethod(Description = "MakeQR")]
    public string MakeQR(string content, string MAACode)
    {
        #region MakeQR
        if (content == "" || MAACode == "")
        {
            return "0";
        }
        QRCodeEncoder encoder = new QRCodeEncoder();
        encoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;  //编码方式(注意：BYTE能支持中文，ALPHA_NUMERIC扫描出来的都是数字)
        encoder.QRCodeScale = 25;    //大小(值越大生成的二维码图片像素越高)
        encoder.QRCodeVersion = 0;  //版本(注意：设置为0主要是防止编码的字符串太长时发生错误)
        encoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;  //错误效验、错误更正(有4个等级)
        //string localFilePath = System.Environment.CurrentDirectory;
        string localFilePath = HttpRuntime.AppDomainAppPath.ToString();
        //string[] strTxt = textBox1.Text.Split("\r\n".ToCharArray());
        String qrdata = "http://dl.duolian.com:8001/mobile/html/maa_info.html?id=" + content;
        System.Drawing.Bitmap bp = encoder.Encode(qrdata.ToString(), Encoding.GetEncoding("GB2312"));
        System.Drawing.Bitmap objNewPic = new System.Drawing.Bitmap(bp, 240, 240);//图片保存的大小尺寸  
        //objNewPic.Save(localFilePath + "\\QRImages\\" + qrdata.Replace("|", "_") + ".jpg", ImageFormat.Jpeg);
        //objNewPic.Save(localFilePath + "\\" + qrdata.Replace("|", "_") + ".jpg");
        objNewPic.Save(localFilePath + "QRImages\\" + content.Replace("|", "_") + ".jpg");
        System.Drawing.Image imgBack = System.Drawing.Image.FromFile(localFilePath + "QRImages\\" + "back.jpg");     //相框图片 
        System.Drawing.Image img = System.Drawing.Image.FromFile(localFilePath + "QRImages\\" + content.Replace("|", "_") + ".jpg");        //照片图片
        //从指定的System.Drawing.Image创建新的System.Drawing.Graphics       
        Graphics g = Graphics.FromImage(imgBack);
        //g.DrawImage(imgBack, 0, 0, 148, 124);      // g.DrawImage(imgBack, 0, 0, 相框宽, 相框高);
        g.FillRectangle(System.Drawing.Brushes.White, 0, 0, (int)600, ((int)400));//相片四周刷一层黑色边框，这里没有，需要调尺寸
        //g.DrawImage(img, 照片与相框的左边距, 照片与相框的上边距, 照片宽, 照片高);
        g.DrawImage(img, 120, 80, 240, 240);
        GC.Collect();
        string saveImagePath = localFilePath + "QRImages\\" + content.Replace("|", "_") + ".png";
        //save new image to file system.
        imgBack.Save(saveImagePath, ImageFormat.Png);
        return saveImagePath;
        #endregion

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

    // /// <summary>
    ///// 返回变量
    ///// </summary>
    ///// <returns></returns>
    //[WebMethod(Description = "BL")]
    //public string ReturnBL()
    //{
    //    string localFilePath = System.Environment.CurrentDirectory;
    //    return HttpRuntime.AppDomainAppPath.ToString();
    //}



}


