using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;
using UFIDA.U8.MomServiceCommon;
using UFIDA.U8.U8MOMAPIFramework;
using UFIDA.U8.U8APIFramework;
using UFIDA.U8.U8APIFramework.Meta;
using UFIDA.U8.U8APIFramework.Parameter;
using MSXML2;
using System.Web.UI;
using System.Configuration;
using System.Collections;
using System.IO;
using System.Net;
using System.Data.SqlClient;
//using ADODB;
using U8API;


namespace BLL
{
    public class DBD
    {
        public string MDBD(string strBillNo)
        {
            #region U8API调用-生成调拨单

            //第一步：构造u8login对象并登陆(引用U8API类库中的Interop.U8Login.dll)
            //如果当前环境中有login对象则可以省去第一步
            U8Login.clsLogin u8Login = new U8Login.clsLogin();
            String sSubId = "AS";
            String sAccID = "测试服务@777";
            String sServer = "192.168.0.242";
            String sSerial = "";
            String sYear = "2015";
            String sDate = "2018-12-10";
            String sUserID = "1226";
            String sPassword = "321";


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
            //当前API：修改单据的地址标识为：U8API/TransVouch/Update
            U8ApiAddress myApiAddress = new U8ApiAddress("U8API/TransVouch/Update");

            //第四步：构造APIBroker
            U8ApiBroker broker = new U8ApiBroker(myApiAddress, envContext);

            //第五步：API参数赋值

            //给普通参数sVouchType赋值。此参数的数据类型为System.String，此参数按值传递，表示单据类型:12
            broker.AssignNormalValue("sVouchType", "12");




            //方法一是直接传入MSXML2.DOMDocumentClass对象
            //-------------------------------------------------------------------------------------------------------
            ADODB.Connection conn = new ADODB.ConnectionClass();
            ADODB.Recordset rs = new ADODB.RecordsetClass();
            MSXML2.DOMDocument domhead = new MSXML2.DOMDocumentClass();
            string strConn = envContext.U8Login.UfDbName;
            conn.Open(strConn, "sa", envContext.U8Login.SysPassword, 0);
            //string sql = "select cast(null as nvarchar(2)) as editprop,a.*,b.* from TransM a left join dbo.TransVouch_extradefine b ON b.ID = a.id WHERE 1=2";
            string sql = "select cast(null as nvarchar(2)) as editprop,a.*,b.* from TransM a left join dbo.TransVouch_extradefine b ON b.ID = a.id WHERE a.ctvcode='170700005'";
            rs.Open(sql, conn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockOptimistic, -1);
            rs.Save(domhead, ADODB.PersistFormatEnum.adPersistXML);
            U8APIHelper.FormatDom(ref domhead, "M");
            broker.AssignNormalValue("DomHead", domhead);
            //SQLHelper sqlhelper = new SQLHelper();
            //string cmdText = "Dl_U8GetOrderMetai_Details_zs_WinForm_DBD_U8API";
            //DataTable dtHead = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);



            //方法一是直接传入MSXML2.DOMDocumentClass对象，表体
            MSXML2.DOMDocument domBody = new MSXML2.DOMDocumentClass();
            ADODB.Recordset rs1 = new ADODB.RecordsetClass();
            //sql = "select cast(null as nvarchar(2)) as editprop,a.*,b.* from TransD a left join dbo.TransVouchs_extradefine b ON b.autoID = a.autoid WHERE 1=2";
            sql = "select cast(null as nvarchar(2)) as editprop,a.*,b.* from TransD a left join dbo.TransVouchs_extradefine b ON b.autoID = a.autoid WHERE a.ctvcode='170700005'";
            rs1.Open(sql, conn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockOptimistic, -1);

            rs1.Save(domBody, ADODB.PersistFormatEnum.adPersistXML);
            U8APIHelper.FormatDom(ref domBody, "M");
            broker.AssignNormalValue("domBody", domBody);

            IXMLDOMNodeList ndbodylist = domBody.selectNodes("//z:row");

            foreach (IXMLDOMElement ele in ndbodylist)
            {
                ele.setAttribute("cinposcode", "");
                ele.setAttribute("cinposname", "");
                ele.setAttribute("cposition", "");
            }



            //给普通参数domPosition赋值。此参数的数据类型为System.Object，此参数按引用传递，表示货位：传空
            broker.AssignNormalValue("domPosition", null);

            //该参数errMsg为OUT型参数，由于其数据类型为System.String，为一般值类型，因此不必传入一个参数变量。在API调用返回时，可以通过GetResult("errMsg")获取其值

            //给普通参数cnnFrom赋值。此参数的数据类型为ADODB.Connection，此参数按引用传递，表示连接对象,如果由调用方控制事务，则需要设置此连接对象，否则传空
            broker.AssignNormalValue("cnnFrom", null);

            ////该参数VouchId为INOUT型普通参数。此参数的数据类型为System.String，此参数按值传递。在API调用返回时，可以通过GetResult("VouchId")获取其值
            //broker.AssignNormalValue("VouchId", "");

            //该参数domMsg为OUT型参数，由于其数据类型为MSXML2.IXMLDOMDocument2，非一般值类型，因此必须传入一个参数变量。在API调用返回时，可以直接使用该参数
            MSXML2.IXMLDOMDocument domMsg = new MSXML2.DOMDocumentClass();
            broker.AssignNormalValue("domMsg", domMsg);

            //给普通参数bCheck赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否控制可用量
            broker.AssignNormalValue("bCheck", false);

            //给普通参数bBeforCheckStock赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示检查可用量
            broker.AssignNormalValue("bBeforCheckStock", false);

            //给普通参数bIsRedVouch赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否红字单据
            broker.AssignNormalValue("bIsRedVouch", false);

            //给普通参数sAddedState赋值。此参数的数据类型为System.String，此参数按值传递，表示新增状态：传入空串
            broker.AssignNormalValue("sAddedState", "");

            //给普通参数bUpdateNeedEas赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示传true
            broker.AssignNormalValue("bUpdateNeedEas", true);


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
            //获取普通返回值。此返回值数据类型为System.Boolean，此参数按值传递，表示返回值:true:成功false:失败
            System.Boolean result = Convert.ToBoolean(broker.GetReturnValue());

            //获取out/inout参数值

            //获取普通OUT参数errMsg。此返回值数据类型为System.String，在使用该参数之前，请判断是否为空
            System.String errMsgRet = broker.GetResult("errMsg") as System.String;

            //获取普通OUT参数domMsg。此返回值数据类型为MSXML2.IXMLDOMDocument2，在使用该参数之前，请判断是否为空
            //MSXML2.IXMLDOMDocument2 domMsgRet = broker.GetResult("domMsg");


            if (!string.IsNullOrEmpty(errMsgRet))
            {
                broker.Release();
                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('" + errMsgRet + "');</script>");
                return errMsgRet;
            }

            //结束本次调用，释放API资源
            broker.Release();
            //return result;
            return "";

            #endregion
        }
    }
}
