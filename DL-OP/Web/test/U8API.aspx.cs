using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Runtime.InteropServices;
using UFIDA.U8.U8APIFramework;
using UFIDA.U8.U8MOMAPIFramework;
using System.Xml;
using MSXML2;



public partial class test_U8API : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {

        //string aa = new FHD_U8API().AddFHD("CZTS20170502127");
        //Response.Write(aa);
        ////构造u8login对象并登陆

        //U8Login.clsLogin u8Login = new U8Login.clsLogin();
        //String sSubId = "AS";
        //String sAccID = "006";
        //String sYear = "2017";
        //String sUserID = "1226";
        //String sPassword = "";
        //String sDate = "2017-03-31";
        //String sServer = "192.168.0.250";
        //String sSerial = "";
        //if (!u8Login.Login(ref sSubId, ref sAccID, ref sYear, ref sUserID, ref sPassword, ref sDate, ref sServer, ref sSerial))
        //{
        //    Console.WriteLine("登陆失败，原因：" + u8Login.ShareString);
        //    Marshal.FinalReleaseComObject(u8Login);
        //    return;
        //}
     
        ////第二步：构造环境上下文对象，传入login，并按需设置其它上下文参数
        //U8EnvContext envContext = new U8EnvContext();
        //envContext.U8Login = u8Login as U8Login.clsLogin;

        ////第三步：设置API地址标识(Url)
        ////当前API：添加新单据的地址标识为：U8API/ProductIn/Add
        //U8ApiAddress myApiAddress = new U8ApiAddress("U8API/ProductIn/Add");

        ////第四步：构造APIBroker
        //U8ApiBroker broker = new U8ApiBroker(myApiAddress, envContext);

        ////第五步：API参数赋值
        ////给普通参数sVouchType赋值。此参数的数据类型为System.String，此参数按值传递，表示单据类型：10
        //broker.AssignNormalValue("sVouchType", "10");

        ////给BO表头参数DomHead赋值，此BO参数的业务类型为不合格品处理单，属表头参数。BO参数均按引用传递
        ////提示：给BO表头参数DomHead赋值有两种方法
        ////方法一是直接传入MSXML2.DOMDocumentClass对象
        ////-------------------------------------------------------------------------------------------------------
        //ADODB.Connection conn = new ADODB.ConnectionClass();
        //ADODB.Recordset rs = new ADODB.RecordsetClass();
        //MSXML2.DOMDocument domhead = new MSXML2.DOMDocumentClass();
        //string strConn = envContext.U8Login.UfDbName;
        //conn.Open(strConn, "sa", envContext.U8Login.SysPassword, 0);
        //string sql = "select top 1 * from RecordInQ where id=18";
        //rs.Open(sql, conn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockOptimistic, -1);

        ////rs.Save(domhead, ADODB.PersistFormatEnum.adPersistXML);
        ////U8APIHelper.FormatDom(ref domhead, "A");
        ////broker.AssignNormalValue("DomHead", domhead);

        ////6．	调用API 
        ////示例调用如下：
        //if (!broker.Invoke())
        //{
        //    //错误处理
        //    Exception apiEx = broker.GetException();
        //    if (apiEx != null)
        //    {
        //        if (apiEx is MomSysException)
        //        {
        //            MomSysException sysEx = apiEx as MomSysException;
        //            Console.WriteLine("系统异常：" + sysEx.Message);
        //            //todo:异常处理
        //        }
        //        else if (apiEx is MomBizException)
        //        {
        //            MomBizException bizEx = apiEx as MomBizException;
        //            Console.WriteLine("API异常：" + bizEx.Message);
        //            //todo:异常处理
        //        }
        //    }
        //    //结束本次调用，释放API资源
        //    broker.Release();
        //    return;
        //}









    }
}