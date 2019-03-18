using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Text;

/// <summary>
/// SendSMS2Customer 的摘要说明
/// </summary>
[WebService(Namespace = "http://dl.duolian.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
// [System.Web.Script.Services.ScriptService]
public class SendSMS2Customer : System.Web.Services.WebService
{

    public SendSMS2Customer()
    {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }

    //[WebMethod]
    //public string HelloWorld()
    //{
    //    return "Hello World";
    //}



    [WebMethod(Description = "SendFHSMS2Customer")]
    public string SendFHSMS2Customer(String phoneno, String orderno, String content)
    {
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
            //1:联多环保芯层发泡降噪管110x3.2,数量:72.00米;2:联多环保排水管160x2.8,数量:280.00米;3:阻燃普通冷弯管（C型）16/3.8米,数量:8550.00米;5:联多排水管件90°弯头160,数量:60.00个
            //将 ; 替换为换行符
            String str = content;
            String[] sArray = str.Split(';');//split就是以传进去的字符进行分割 
            for (int i = 0; i < sArray.Length; i++)
            {
                if ((newcontent + sArray[i].ToString()).Length < 130)
                {
                    newcontent = newcontent + sArray[i].ToString() + "\r\n";
                }
                else
                {
                    #region 批量发送，电话号码格式为xxx，xxx，xxx
                    sendsms = "【温馨提示】尊敬的客户：您好！您的订单（" + orderno + "）有如下产品未装完：\r\n" + newcontent + "以上信息请您知悉，如有正好是您急需的产品，请您在十分钟内与公司客服专员联系，联系电话：4008786333转3，如果未按约定时间回复，公司将会默认您同意此装车方式。";
                    bool c = sms.SingleSend(s, sendsms);
                    #endregion
                    newcontent = sArray[i].ToString() + "\r\n";
                }
            }
            #region 批量发送，电话号码格式为xxx，xxx，xxx
            sendsms = "【温馨提示】尊敬的客户：您好！您的订单（" + orderno + "）有如下产品未装完：\r\n" + newcontent + "以上信息请您知悉，如有正好是您急需的产品，请您在十分钟内与公司客服专员联系，联系电话：4008786333转3，如果未按约定时间回复，公司将会默认您同意此装车方式。";
            bool v = sms.SingleSend(s, sendsms);
            #endregion
        }

        return phoneno + "!" + sendsms;
    }

    [WebMethod(Description = "SendTest")]
    public string SendTest(String phoneno, String orderno, String content)
    {

        //字符串添加换行

        string smstext = "换行测试\r\n 【温馨提示】尊敬的客户：您好！您的订单（1234567）有如下产品未装完：\r\n 产品1产品2产品3产品1产品2产品3产品1产品2产品3产品1产品2产品3产品1产品2产品3产品1产品2产品3产品1产品2产品3产品1产品2产品3产品1产品2产品3产品1产品2产品3。\r\n 以上信息请您知悉，如有正好是您急需的产品，请您在十分钟内与公司客服专员联系，联系电话：4008786333转3，如果未按约定时间回复，公司将会默认您同意此装车方式。";

        SendSMS sms = new SendSMS();
        #region 批量发送，电话号码格式为xxx，xxx，xxx
        bool c = sms.SingleSend(phoneno, smstext);
        #endregion
        return phoneno + "!" + smstext;
    }
}
