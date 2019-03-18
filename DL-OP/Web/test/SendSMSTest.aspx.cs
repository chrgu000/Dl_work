using BLL;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model;

public partial class test_SendSMSTest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void BtnSend_Click(object sender, EventArgs e)
    {
        //DataTable dt = new GetPhoneNo().GetCustomerPhoneNo(cuscode.Text);
        //if (dt.Rows.Count>0)
        //{
        //    //短信内容
        //    //string SmsTxt = "【多联网上下单提示】尊敬的客户,您的网单号：" + strBillNo + "已经通过审核，我们将尽快为您安排发货！谢谢您的支持。 ";
        //    string SmsTxt = "【多联网上下单提示】尊敬的客户,您的网单号 已经通过审核，我们将尽快为您安排发货！谢谢您的支持。 ";
        //    //string SmsTxt = "超大文本发送测试，我在.net程序里操作数据时将一些字段数据加密了，这些数据是很多系统共用的，其中一delphi程序也需要用到，并且需要将数据解密，由于我在.net里加密的方式比较特殊，在delphi程序里解密比较繁琐且要消耗很多时间，所以不得不让sqlserver调用程序集的方式来解决问题。这个存储过程只能执行Dos控制台程序,其他的Exe程序不能在Sqlserver进程空间执行.但外部程式必须是一个自生灭的程式(即没有消息循环、不需要与用户交互)，否则将会进入死循环中。 ";
        //    SendSMS sms = new SendSMS();
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        sms.SingleSend(dt.Rows[i]["PhoneNo"].ToString(), SmsTxt.ToString());
        //    }
        //}
        //else
        //{
        //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单没有该用户电话号码信息！');</script>");
        //}
        SendSMS2Customer9003.SendSMS2CustomerSoapClient sms = new SendSMS2Customer9003.SendSMS2CustomerSoapClient();
        sms.SendSMS("15308078836","123");

    }
}