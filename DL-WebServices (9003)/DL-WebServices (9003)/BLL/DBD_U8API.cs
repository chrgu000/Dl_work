using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using System.Runtime.InteropServices;
using UFIDA.U8.MomServiceCommon;
using UFIDA.U8.U8MOMAPIFramework;
using UFIDA.U8.U8APIFramework;
using UFIDA.U8.U8APIFramework.Meta;
using UFIDA.U8.U8APIFramework.Parameter;
using MSXML2;
using U8API;
using System.Web.UI;
using System.Configuration;
using System.Collections;
using System.IO;
using System.Net;

namespace BLL
{
    public class DBD_U8API
    {
        public String AddXSDDAPI(string strBillNo)
        {
            #region U8API调用-生成销售订单

            //第一步：构造u8login对象并登陆(引用U8API类库中的Interop.U8Login.dll)
            //如果当前环境中有login对象则可以省去第一步
            U8Login.clsLogin u8Login = new U8Login.clsLogin();
            //String sSubId = "AS";
            //String sAccID = "(default)@006";
            //String sYear = "2017";
            //String sUserID = "1226";
            //String sPassword = "123";
            //String sDate = "2017-04-12";
            //String sServer = "192.168.0.250";
            String sSerial = "";

            String sSubId = System.Web.Configuration.WebConfigurationManager.AppSettings["sSubId"];
            //String sYear = DateTime.Now.Year.ToString();
            String sYear = "2015";
            String sDate = DateTime.Now.ToString("yyyy-MM-dd");
            //String sDate = "2017-01-01";
            String sAccID = System.Web.Configuration.WebConfigurationManager.AppSettings["sAccID"].Replace("test", "测试服务").ToString();
            String sUserID = System.Web.Configuration.WebConfigurationManager.AppSettings["sUserID"];
            String sPassword = System.Web.Configuration.WebConfigurationManager.AppSettings["sPassword"];
            String sServer = System.Web.Configuration.WebConfigurationManager.AppSettings["sServer"];

            if (!u8Login.Login(ref sSubId, ref sAccID, ref sYear, ref sUserID, ref sPassword, ref sDate, ref sServer, ref sSerial))
            {
                Console.WriteLine("登陆失败，原因：" + u8Login.ShareString);
                Marshal.FinalReleaseComObject(u8Login);
                return "登陆失败";
            }

            //第二步：构造环境上下文对象，传入login，并按需设置其它上下文参数
            U8EnvContext envContext = new U8EnvContext();
            envContext.U8Login = u8Login;

            //第三步：设置API地址标识(Url)
            //当前API：添加新单据的地址标识为：U8API/TransVouch/Add
            U8ApiAddress myApiAddress = new U8ApiAddress("U8API/TransVouch/Add");

            //第四步：构造APIBroker
            U8ApiBroker broker = new U8ApiBroker(myApiAddress, envContext);

            //第五步：API参数赋值

            //给普通参数sVouchType赋值。此参数的数据类型为System.String，此参数按值传递，表示单据类型：12
            broker.AssignNormalValue("sVouchType", "12");



            //方法一是直接传入MSXML2.DOMDocumentClass对象
            //-------------------------------------------------------------------------------------------------------
            ADODB.Connection conn = new ADODB.ConnectionClass();
            ADODB.Recordset rs = new ADODB.RecordsetClass();
            MSXML2.DOMDocument domhead = new MSXML2.DOMDocumentClass();
            string strConn = envContext.U8Login.UfDbName;
            conn.Open(strConn, "sa", envContext.U8Login.SysPassword, 0);
            string sql = "select cast(null as nvarchar(2)) as editprop,a.*,b.* from TransM a left join dbo.TransVouch_extradefine b ON b.ID = a.id WHERE 1=2";
            rs.Open(sql, conn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockOptimistic, -1);
            rs.Save(domhead, ADODB.PersistFormatEnum.adPersistXML);
            U8APIHelper.FormatDom(ref domhead, "A");
            broker.AssignNormalValue("DomHead", domhead);
            //string strBillNo = TextBox1.Text.ToString();
            DataTable dtHead = new BLL.OrderManager().DLproc_NewOrderU8BySel(strBillNo);
            if (dtHead.Rows.Count < 1)
            {
                return "没有数据！";
            }
            if (dtHead.Rows.Count > 0)
            {
                //方法一是直接传入MSXML2.DOMDocumentClass对象，表头
                ENUtil.SetToDomH(domhead, "id", "0");
                ENUtil.SetToDomH(domhead, "editprop", "A");
                ENUtil.SetToDomH(domhead, "dtvdate", dtHead.Rows[0]["dtvdate"].ToString());
                ENUtil.SetToDomH(domhead, "cowhcode", dtHead.Rows[0]["cowhcode"].ToString());
                ENUtil.SetToDomH(domhead, "ciwhcode", dtHead.Rows[0]["ciwhcode"].ToString());
                ENUtil.SetToDomH(domhead, "cirdcode", dtHead.Rows[0]["cirdcode"].ToString());
                ENUtil.SetToDomH(domhead, "cordcode", dtHead.Rows[0]["cordcode"].ToString());
                ENUtil.SetToDomH(domhead, "cdefine2", dtHead.Rows[0]["cdefine2"].ToString());
                ENUtil.SetToDomH(domhead, "cmaker", dtHead.Rows[0]["cmaker"].ToString());
                ENUtil.SetToDomH(domhead, "vt_id", dtHead.Rows[0]["vt_id"].ToString());
                ENUtil.SetToDomH(domhead, "cpspcode", dtHead.Rows[0]["cpspcode"].ToString());
                ENUtil.SetToDomH(domhead, "cmpocode", dtHead.Rows[0]["cmpocode"].ToString());
                ENUtil.SetToDomH(domhead, "iquantity", dtHead.Rows[0]["iquantity"].ToString());
                ENUtil.SetToDomH(domhead, "cordertype", dtHead.Rows[0]["cordertype"].ToString());
                ENUtil.SetToDomH(domhead, "csource", dtHead.Rows[0]["csource"].ToString());
                ENUtil.SetToDomH(domhead, "itransflag", dtHead.Rows[0]["itransflag"].ToString());
                ENUtil.SetToDomH(domhead, "dnmaketime", dtHead.Rows[0]["dnmaketime"].ToString());
                ENUtil.SetToDomH(domhead, "iswfcontrolled", dtHead.Rows[0]["iswfcontrolled"].ToString());
                ENUtil.SetToDomH(domhead, "iprintcount", dtHead.Rows[0]["iprintcount"].ToString());
            }

            //方法一是直接传入MSXML2.DOMDocumentClass对象，表体
            MSXML2.DOMDocument domBody = new MSXML2.DOMDocumentClass();
            ADODB.Recordset rs1 = new ADODB.RecordsetClass();
            sql = "select cast(null as nvarchar(2)) as editprop,a.*,b.* from TransD a left join dbo.TransVouchs_extradefine b ON b.autoID = a.autoid WHERE 1=2";
            rs1.Open(sql, conn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockOptimistic, -1);

            rs1.Save(domBody, ADODB.PersistFormatEnum.adPersistXML);
            //U8APIHelper.FormatDom(ref domBody, "A");
            broker.AssignNormalValue("domBody", domBody);

            if (dtHead.Rows.Count > 0)
            {
                int rownum = 0;
                for (int i = 0; i < dtHead.Rows.Count; i++)
                {
                    rownum = i;
                    ENUtil.SetToDomB(domBody, rownum, "id", "0");
                    ENUtil.SetToDomB(domBody, rownum, "editprop", "A");
                    ENUtil.SetToDomB(domBody, rownum, "ctvcode", dtHead.Rows[0]["ctvcode"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "cinvcode", dtHead.Rows[0]["cinvcode"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "itvquantity", dtHead.Rows[0]["itvquantity"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "fsalecost", dtHead.Rows[0]["fsalecost"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "fsaleprice", dtHead.Rows[0]["fsaleprice"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "issotype", dtHead.Rows[0]["issotype"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "idsotype", dtHead.Rows[0]["idsotype"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "bcosting", dtHead.Rows[0]["bcosting"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "cmocode", dtHead.Rows[0]["cmocode"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "invcode", dtHead.Rows[0]["invcode"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "imoseq", dtHead.Rows[0]["imoseq"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "imoids", dtHead.Rows[0]["imoids"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "corufts", dtHead.Rows[0]["corufts"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "iexpiratdatecalcu", dtHead.Rows[0]["iexpiratdatecalcu"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "irowno", i++);
                }
            }

            //给普通参数domPosition赋值。此参数的数据类型为System.Object，此参数按引用传递，表示货位：传空
            broker.AssignNormalValue("domPosition", null);

            //该参数errMsg为OUT型参数，由于其数据类型为System.String，为一般值类型，因此不必传入一个参数变量。在API调用返回时，可以通过GetResult("errMsg")获取其值

            //给普通参数cnnFrom赋值。此参数的数据类型为ADODB.Connection，此参数按引用传递，表示连接对象,如果由调用方控制事务，则需要设置此连接对象，否则传空
            broker.AssignNormalValue("cnnFrom", null);

            //该参数VouchId为INOUT型普通参数。此参数的数据类型为System.String，此参数按值传递。在API调用返回时，可以通过GetResult("VouchId")获取其值
            broker.AssignNormalValue("VouchId", "");

            //该参数domMsg为OUT型参数，由于其数据类型为MSXML2.IXMLDOMDocument2，非一般值类型，因此必须传入一个参数变量。在API调用返回时，可以直接使用该参数
            MSXML2.IXMLDOMDocument2 domMsg = new MSXML2.DOMDocumentClass();
            broker.AssignNormalValue("domMsg", domMsg);

            //给普通参数bCheck赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否控制可用量。
            broker.AssignNormalValue("bCheck", false);

            //给普通参数bBeforCheckStock赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示检查可用量
            broker.AssignNormalValue("bBeforCheckStock", false);

            //给普通参数bIsRedVouch赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否红字单据
            broker.AssignNormalValue("bIsRedVouch", false);

            //给普通参数sAddedState赋值。此参数的数据类型为System.String，此参数按值传递，表示传空字符串
            broker.AssignNormalValue("sAddedState", "");

            //给普通参数bReMote赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否远程：转入false
            broker.AssignNormalValue("bReMote", false);


            //第六步：调用API
            if (!broker.Invoke())
            {
                //错误处理
                Exception apiEx = broker.GetException();
                if (apiEx != null)
                {
                    if (apiEx is MomSysException)
                    {
                        MomSysException sysEx = apiEx as MomSysException;
                        Console.WriteLine("系统异常：" + sysEx.Message);
                        //todo:异常处理
                    }
                    else if (apiEx is MomBizException)
                    {
                        MomBizException bizEx = apiEx as MomBizException;
                        Console.WriteLine("API异常：" + bizEx.Message);
                        //todo:异常处理
                    }
                    //异常原因
                    String exReason = broker.GetExceptionString();
                    if (exReason.Length != 0)
                    {
                        Console.WriteLine("异常原因：" + exReason);
                    }
                }
                //结束本次调用，释放API资源
                broker.Release();
                return "error";
            }

            //第七步：获取返回结果

            //获取返回值
            //获取普通返回值。此返回值数据类型为System.Boolean，此参数按值传递，表示返回值:true:成功,false:失败
            //System.Boolean result = Convert.ToBoolean(broker.GetReturnValue());
            System.String result = Convert.ToString(broker.GetReturnValue());

            //获取out/inout参数值

            //获取普通OUT参数errMsg。此返回值数据类型为System.String，在使用该参数之前，请判断是否为空
            System.String errMsgRet = broker.GetResult("errMsg") as System.String;

            //获取普通INOUT参数VouchId。此返回值数据类型为System.String，在使用该参数之前，请判断是否为空
            System.String VouchIdRet = broker.GetResult("VouchId") as System.String;

            //获取普通OUT参数domMsg。此返回值数据类型为MSXML2.IXMLDOMDocument2，在使用该参数之前，请判断是否为空
            //MSXML2.IXMLDOMDocument2 domMsgRet = Convert.ToObject(broker.GetResult("domMsg"));



            //结束本次调用，释放API资源
            broker.Release();
            return result;

            #endregion
        }
    }
}
