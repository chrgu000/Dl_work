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
    public class TSXSDD
    {
        public String AddTSXSDDAPI(DataTable dtHead, ADODB.Connection conn, string minid)
        {
            #region U8API调用-生成特殊销售订单

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
            //String sDate = "2018-12-10";
            String sDate = DateTime.Now.ToShortDateString();
            //String sAccID = System.Web.Configuration.WebConfigurationManager.AppSettings["sAccID"].ToString();
            String sAccID = System.Web.Configuration.WebConfigurationManager.AppSettings["sAccID"].ToString();
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

            //销售所有接口均支持内部独立事务和外部事务，默认内部事务
            //如果是外部事务，则需要传递ADO.Connection对象，并将IsIndependenceTransaction属性设置为false
            envContext.BizDbConnection = conn;
            envContext.IsIndependenceTransaction = false;

            //设置上下文参数
            envContext.SetApiContext("VoucherType", 12); //上下文数据类型：int，含义：单据类型：12

            //第三步：设置API地址标识(Url)
            //当前API：新增或修改的地址标识为：U8API/SaleOrder/Save
            U8ApiAddress myApiAddress = new U8ApiAddress("U8API/SaleOrder/Save");

            //第四步：构造APIBroker
            U8ApiBroker broker = new U8ApiBroker(myApiAddress, envContext);


            //第五步：API参数赋值
            //方法一是直接传入MSXML2.DOMDocumentClass对象
            //broker.AssignNormalValue("domHead", new MSXML2.DOMDocumentClass())


            //方法一是直接传入MSXML2.DOMDocumentClass对象
            //-------------------------------------------------------------------------------------------------------
            //ADODB.Connection conn = new ADODB.ConnectionClass();
            ADODB.Recordset rs = new ADODB.RecordsetClass();
            MSXML2.DOMDocument domhead = new MSXML2.DOMDocumentClass();
            string strConn = envContext.U8Login.UfDbName;
            //conn.Open(strConn, "sa", envContext.U8Login.SysPassword, 0);
            string sql = "select cast(null as nvarchar(2)) as editprop,a.*,b.* from SaleOrderQ a left join SO_SOMain_extradefine b ON b.ID = a.id WHERE 1=2";
            rs.Open(sql, conn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockOptimistic, -1);
            rs.Save(domhead, ADODB.PersistFormatEnum.adPersistXML);
            U8APIHelper.FormatDom(ref domhead, "A");
            broker.AssignNormalValue("DomHead", domhead);
            //string strBillNo = TextBox1.Text.ToString();
            //DataTable dtHead = new BLL.OrderManager().DLproc_NewYOrderU8_TSBySel(strBillNo);
            if (dtHead.Rows.Count < 1)
            {
                return "没有数据！";
            }
            if (dtHead.Rows.Count > 0)
            {
                //方法一是直接传入MSXML2.DOMDocumentClass对象，表头
                ENUtil.SetToDomH(domhead, "id", "0");
                ENUtil.SetToDomH(domhead, "editprop", "A");
                ENUtil.SetToDomH(domhead, "ddate", dtHead.Rows[0]["ddate"].ToString());
                ENUtil.SetToDomH(domhead, "csocode", dtHead.Rows[0]["csocode"].ToString());
                ENUtil.SetToDomH(domhead, "ccuscode", dtHead.Rows[0]["ccuscode"].ToString());
                ENUtil.SetToDomH(domhead, "dpredatebt", dtHead.Rows[0]["dpredatebt"].ToString());
                ENUtil.SetToDomH(domhead, "dpremodatebt", dtHead.Rows[0]["dpremodatebt"].ToString());
                ENUtil.SetToDomH(domhead, "ccusname", dtHead.Rows[0]["ccusname"].ToString());
                ENUtil.SetToDomH(domhead, "cinvoicecompany", dtHead.Rows[0]["cinvoicecompany"].ToString());
                ENUtil.SetToDomH(domhead, "cmemo", dtHead.Rows[0]["cmemo"].ToString());
                ENUtil.SetToDomH(domhead, "cpersoncode", dtHead.Rows[0]["cpersoncode"].ToString());
                ENUtil.SetToDomH(domhead, "cSTCode", dtHead.Rows[0]["cSTCode"].ToString());
                ENUtil.SetToDomH(domhead, "cSCCode", dtHead.Rows[0]["cSCCode"].ToString());
                ENUtil.SetToDomH(domhead, "cDefine6", dtHead.Rows[0]["cDefine6"].ToString());
                ENUtil.SetToDomH(domhead, "cmaker", dtHead.Rows[0]["cmaker"].ToString());
                ENUtil.SetToDomH(domhead, "cdepcode", dtHead.Rows[0]["cdepcode"].ToString());
                ENUtil.SetToDomH(domhead, "cexch_name", dtHead.Rows[0]["cexch_name"].ToString());
                ENUtil.SetToDomH(domhead, "iexchrate", dtHead.Rows[0]["iexchrate"].ToString());
                ENUtil.SetToDomH(domhead, "itaxrate", dtHead.Rows[0]["itaxrate"].ToString());
                ENUtil.SetToDomH(domhead, "cbustype", dtHead.Rows[0]["cbustype"].ToString());
                ENUtil.SetToDomH(domhead, "bdisflag", dtHead.Rows[0]["bdisflag"].ToString());
                ENUtil.SetToDomH(domhead, "breturnflag", dtHead.Rows[0]["breturnflag"].ToString());
                ENUtil.SetToDomH(domhead, "iverifystate", dtHead.Rows[0]["iverifystate"].ToString());
                ENUtil.SetToDomH(domhead, "iswfcontrolled", dtHead.Rows[0]["iswfcontrolled"].ToString());
                ENUtil.SetToDomH(domhead, "bcashsale", dtHead.Rows[0]["bcashsale"].ToString());
                ENUtil.SetToDomH(domhead, "bmustbook", dtHead.Rows[0]["bmustbook"].ToString());
                ENUtil.SetToDomH(domhead, "ivtid", dtHead.Rows[0]["ivtid"].ToString());
                ENUtil.SetToDomH(domhead, "chdefine13", dtHead.Rows[0]["chdefine13"].ToString());
                ENUtil.SetToDomH(domhead, "chdefine19", dtHead.Rows[0]["chdefine19"].ToString());
                ENUtil.SetToDomH(domhead, "chdefine26", dtHead.Rows[0]["chdefine26"].ToString());
                ENUtil.SetToDomH(domhead, "chdefine31", dtHead.Rows[0]["chdefine31"].ToString());
            }

            //方法一是直接传入MSXML2.DOMDocumentClass对象，表体
            MSXML2.DOMDocument domBody = new MSXML2.DOMDocumentClass();
            ADODB.Recordset rs1 = new ADODB.RecordsetClass();
            sql = "select cast(null as nvarchar(2)) as editprop,a.*,b.* from SaleOrderSQ a left join SO_SODetails_extradefine b ON b.iSOsID = a.isosid where 1=0";
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
                    ENUtil.SetToDomB(domBody, rownum, "cinvcode", dtHead.Rows[i]["cinvcode"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "iquantity", dtHead.Rows[i]["iquantity"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "inum", dtHead.Rows[i]["inum"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "iquotedprice", dtHead.Rows[i]["iquotedprice"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "iunitprice", dtHead.Rows[i]["iunitprice"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "itaxunitprice", dtHead.Rows[i]["itaxunitprice"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "imoney", dtHead.Rows[i]["imoney"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "itax", dtHead.Rows[i]["itax"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "isum", dtHead.Rows[i]["isum"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "inatunitprice", dtHead.Rows[i]["inatunitprice"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "inatmoney", dtHead.Rows[i]["inatmoney"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "inattax", dtHead.Rows[i]["inattax"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "inatsum", dtHead.Rows[i]["inatsum"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "kl", dtHead.Rows[i]["kl"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "itaxrate", dtHead.Rows[i]["itaxrate"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "cdefine22", dtHead.Rows[i]["cdefine22"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "iinvexchrate", dtHead.Rows[i]["iinvexchrate"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "cunitid", dtHead.Rows[i]["cunitid"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "cgroupcode", dtHead.Rows[i]["cgroupcode"].ToString());//计量单位组，string类型
                    ENUtil.SetToDomB(domBody, rownum, "iGroupType", dtHead.Rows[i]["iGroupType"].ToString());//计量单位组，string类型              
                    ENUtil.SetToDomB(domBody, rownum, "irowno", dtHead.Rows[i]["irowno"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "cinvname", dtHead.Rows[i]["cinvname"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "cSOCode", dtHead.Rows[i]["cSOCode"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "cpreordercode", dtHead.Rows[i]["cpreordercode"].ToString());
                    //ENUtil.SetToDomB(domBody, rownum, "iaoids", dtHead.Rows[i]["iaoids"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "iaoids", Convert.ToString(Convert.ToInt32(minid) + i));
                    ENUtil.SetToDomB(domBody, rownum, "cDefine35", dtHead.Rows[i]["cDefine35"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "cDefine24", dtHead.Rows[i]["cDefine24"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "idemandtype", dtHead.Rows[i]["idemandtype"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "dpredate", dtHead.Rows[i]["dpredate"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "idiscount", dtHead.Rows[i]["idiscount"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "inatdiscount", dtHead.Rows[i]["inatdiscount"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "kl2", dtHead.Rows[i]["kl2"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "fsalecost", dtHead.Rows[i]["fsalecost"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "fsaleprice", dtHead.Rows[i]["fsaleprice"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "dpremodate", dtHead.Rows[i]["dpremodate"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "fcusminprice", dtHead.Rows[i]["fcusminprice"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "ballpurchase", dtHead.Rows[i]["ballpurchase"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "borderbom", dtHead.Rows[i]["borderbom"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "borderbomover", dtHead.Rows[i]["borderbomover"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "busecusbom", dtHead.Rows[i]["busecusbom"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "bsaleprice", dtHead.Rows[i]["bsaleprice"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "bgift", dtHead.Rows[i]["bgift"].ToString());
                }
            }

            //给普通参数VoucherState赋值。此参数的数据类型为int，此参数按值传递，表示状态:0增加;1修改
            broker.AssignNormalValue("VoucherState", 0);

            //该参数vNewID为INOUT型普通参数。此参数的数据类型为string，此参数按值传递。在API调用返回时，可以通过GetResult("vNewID")获取其值
            broker.AssignNormalValue("vNewID", "");

            //给普通参数DomConfig赋值。此参数的数据类型为MSXML2.IXMLDOMDocument2，此参数按引用传递，表示ATO,PTO选配
            MSXML2.IXMLDOMDocument2 domMsg = new MSXML2.DOMDocumentClass();
            broker.AssignNormalValue("DomConfig", domMsg);

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
            //获取普通返回值。此返回值数据类型为System.String，此参数按值传递，表示成功为空串
            System.String result = broker.GetReturnValue() as System.String;

            //获取out/inout参数值

            //获取普通INOUT参数vNewID。此返回值数据类型为string，在使用该参数之前，请判断是否为空
            string vNewIDRet = broker.GetResult("vNewID") as string;
            //结束本次调用，释放API资源
            broker.Release();
            return result;

            #endregion
        }

        public String AddTSXSDDAPI(ADODB.Connection conn, string strBillNo)
        {
            #region U8API调用-生成特殊销售订单

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
            String sDate = "2018-12-10";
            //String sDate = "2017-01-01";
            //String sAccID = System.Web.Configuration.WebConfigurationManager.AppSettings["sAccID"].ToString();
            String sAccID = System.Web.Configuration.WebConfigurationManager.AppSettings["sAccID"].Replace("test", "test").ToString();
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

            //销售所有接口均支持内部独立事务和外部事务，默认内部事务
            //如果是外部事务，则需要传递ADO.Connection对象，并将IsIndependenceTransaction属性设置为false
            envContext.BizDbConnection = conn;
            envContext.IsIndependenceTransaction = false;

            //设置上下文参数
            envContext.SetApiContext("VoucherType", 12); //上下文数据类型：int，含义：单据类型：12

            //第三步：设置API地址标识(Url)
            //当前API：新增或修改的地址标识为：U8API/SaleOrder/Save
            U8ApiAddress myApiAddress = new U8ApiAddress("U8API/SaleOrder/Save");

            //第四步：构造APIBroker
            U8ApiBroker broker = new U8ApiBroker(myApiAddress, envContext);


            //第五步：API参数赋值
            //方法一是直接传入MSXML2.DOMDocumentClass对象
            //broker.AssignNormalValue("domHead", new MSXML2.DOMDocumentClass())


            //方法一是直接传入MSXML2.DOMDocumentClass对象
            //-------------------------------------------------------------------------------------------------------
            //ADODB.Connection conn = new ADODB.ConnectionClass();
            ADODB.Recordset rs = new ADODB.RecordsetClass();
            MSXML2.DOMDocument domhead = new MSXML2.DOMDocumentClass();
            string strConn = envContext.U8Login.UfDbName;

            ADODB.Recordset rs9 = new ADODB.RecordsetClass();
            object ob = new object();
            rs9 = conn.Execute("DLproc_NewYOrderU8_TSBySel '" + strBillNo + "'", out ob);


            //conn.Open(strConn, "sa", envContext.U8Login.SysPassword, 0);
            string sql = "select cast(null as nvarchar(2)) as editprop,a.*,b.* from SaleOrderQ a left join SO_SOMain_extradefine b ON b.ID = a.id WHERE 1=2";
            rs.Open(sql, conn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockOptimistic, -1);
            rs.Save(domhead, ADODB.PersistFormatEnum.adPersistXML);
            U8APIHelper.FormatDom(ref domhead, "A");
            broker.AssignNormalValue("DomHead", domhead);
            //string strBillNo = TextBox1.Text.ToString();
            //DataTable dtHead = new BLL.OrderManager().DLproc_NewYOrderU8_TSBySel(strBillNo);
            if (rs9.RecordCount < 1)
            {
                return "没有数据！";
            }
            if (rs9.RecordCount > 0)
            {
                rs9.MoveFirst();
                //方法一是直接传入MSXML2.DOMDocumentClass对象，表头
                ENUtil.SetToDomH(domhead, "id", "0");
                ENUtil.SetToDomH(domhead, "editprop", "A");
                ENUtil.SetToDomH(domhead, "ddate", rs9.Fields["ddate"].ToString());
                ENUtil.SetToDomH(domhead, "csocode", rs9.Fields["csocode"].ToString());
                ENUtil.SetToDomH(domhead, "ccuscode", rs9.Fields["ccuscode"].ToString());
                ENUtil.SetToDomH(domhead, "dpredatebt", rs9.Fields["dpredatebt"].ToString());
                ENUtil.SetToDomH(domhead, "dpremodatebt", rs9.Fields["dpremodatebt"].ToString());
                ENUtil.SetToDomH(domhead, "ccusname", rs9.Fields["ccusname"].ToString());
                ENUtil.SetToDomH(domhead, "cinvoicecompany", rs9.Fields["cinvoicecompany"].ToString());
                ENUtil.SetToDomH(domhead, "cmemo", rs9.Fields["cmemo"].ToString());
                ENUtil.SetToDomH(domhead, "cpersoncode", rs9.Fields["cpersoncode"].ToString());
                ENUtil.SetToDomH(domhead, "cSTCode", rs9.Fields["cSTCode"].ToString());
                ENUtil.SetToDomH(domhead, "cSCCode", rs9.Fields["cSCCode"].ToString());
                ENUtil.SetToDomH(domhead, "cDefine6", rs9.Fields["cDefine6"].ToString());
                ENUtil.SetToDomH(domhead, "cmaker", rs9.Fields["cmaker"].ToString());
                ENUtil.SetToDomH(domhead, "cdepcode", rs9.Fields["cdepcode"].ToString());
                ENUtil.SetToDomH(domhead, "cexch_name", rs9.Fields["cexch_name"].ToString());
                ENUtil.SetToDomH(domhead, "iexchrate", rs9.Fields["iexchrate"].ToString());
                ENUtil.SetToDomH(domhead, "itaxrate", rs9.Fields["itaxrate"].ToString());
                ENUtil.SetToDomH(domhead, "cbustype", rs9.Fields["cbustype"].ToString());
                ENUtil.SetToDomH(domhead, "bdisflag", rs9.Fields["bdisflag"].ToString());
                ENUtil.SetToDomH(domhead, "breturnflag", rs9.Fields["breturnflag"].ToString());
                ENUtil.SetToDomH(domhead, "iverifystate", rs9.Fields["iverifystate"].ToString());
                ENUtil.SetToDomH(domhead, "iswfcontrolled", rs9.Fields["iswfcontrolled"].ToString());
                ENUtil.SetToDomH(domhead, "bcashsale", rs9.Fields["bcashsale"].ToString());
                ENUtil.SetToDomH(domhead, "bmustbook", rs9.Fields["bmustbook"].ToString());
                ENUtil.SetToDomH(domhead, "ivtid", rs9.Fields["ivtid"].ToString());
                ENUtil.SetToDomH(domhead, "chdefine13", rs9.Fields["chdefine13"].ToString());
                ENUtil.SetToDomH(domhead, "chdefine19", rs9.Fields["chdefine19"].ToString());
                ENUtil.SetToDomH(domhead, "chdefine26", rs9.Fields["chdefine26"].ToString());
                ENUtil.SetToDomH(domhead, "chdefine31", rs9.Fields["chdefine31"].ToString());
            }

            //方法一是直接传入MSXML2.DOMDocumentClass对象，表体
            MSXML2.DOMDocument domBody = new MSXML2.DOMDocumentClass();
            ADODB.Recordset rs1 = new ADODB.RecordsetClass();
            sql = "select cast(null as nvarchar(2)) as editprop,a.*,b.* from SaleOrderSQ a left join SO_SODetails_extradefine b ON b.iSOsID = a.isosid where 1=0";
            rs1.Open(sql, conn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockOptimistic, -1);

            rs1.Save(domBody, ADODB.PersistFormatEnum.adPersistXML);
            //U8APIHelper.FormatDom(ref domBody, "A");
            broker.AssignNormalValue("domBody", domBody);

            if (rs9.RecordCount > 0)
            {
                int rownum = 0;
                for (int i = 0; i < rs9.RecordCount; i++)
                {
                    rownum = i;
                    ENUtil.SetToDomB(domBody, rownum, "id", "0");
                    ENUtil.SetToDomB(domBody, rownum, "editprop", "A");
                    ENUtil.SetToDomB(domBody, rownum, "cinvcode", rs9.Fields["cinvcode"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "iquantity", rs9.Fields["iquantity"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "inum", rs9.Fields["inum"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "iquotedprice", rs9.Fields["iquotedprice"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "iunitprice", rs9.Fields["iunitprice"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "itaxunitprice", rs9.Fields["itaxunitprice"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "imoney", rs9.Fields["imoney"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "itax", rs9.Fields["itax"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "isum", rs9.Fields["isum"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "inatunitprice", rs9.Fields["inatunitprice"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "inatmoney", rs9.Fields["inatmoney"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "inattax", rs9.Fields["inattax"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "inatsum", rs9.Fields["inatsum"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "kl", rs9.Fields["kl"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "itaxrate", rs9.Fields["itaxrate"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "cdefine22", rs9.Fields["cdefine22"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "iinvexchrate", rs9.Fields["iinvexchrate"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "cunitid", rs9.Fields["cunitid"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "cgroupcode", rs9.Fields["cgroupcode"].ToString());//计量单位组，string类型
                    ENUtil.SetToDomB(domBody, rownum, "iGroupType", rs9.Fields["iGroupType"].ToString());//计量单位组，string类型              
                    ENUtil.SetToDomB(domBody, rownum, "irowno", rs9.Fields["irowno"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "cinvname", rs9.Fields["cinvname"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "cSOCode", rs9.Fields["cSOCode"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "cpreordercode", rs9.Fields["cpreordercode"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "iaoids", rs9.Fields["iaoids"].ToString());
                    //ENUtil.SetToDomB(domBody, rownum, "iaoids", Convert.ToString(Convert.ToInt32(minid) + i));
                    ENUtil.SetToDomB(domBody, rownum, "cDefine35", rs9.Fields["cDefine35"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "cDefine24", rs9.Fields["cDefine24"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "idemandtype", rs9.Fields["idemandtype"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "dpredate", rs9.Fields["dpredate"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "idiscount", rs9.Fields["idiscount"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "inatdiscount", rs9.Fields["inatdiscount"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "kl2", rs9.Fields["kl2"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "fsalecost", rs9.Fields["fsalecost"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "fsaleprice", rs9.Fields["fsaleprice"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "dpremodate", rs9.Fields["dpremodate"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "fcusminprice", rs9.Fields["fcusminprice"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "ballpurchase", rs9.Fields["ballpurchase"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "borderbom", rs9.Fields["borderbom"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "borderbomover", rs9.Fields["borderbomover"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "busecusbom", rs9.Fields["busecusbom"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "bsaleprice", rs9.Fields["bsaleprice"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "bgift", rs9.Fields["bgift"].ToString());
                    if (i != rs9.RecordCount - 1)
                    {
                        rs9.MoveNext();
                    }
                }
            }

            //给普通参数VoucherState赋值。此参数的数据类型为int，此参数按值传递，表示状态:0增加;1修改
            broker.AssignNormalValue("VoucherState", 0);

            //该参数vNewID为INOUT型普通参数。此参数的数据类型为string，此参数按值传递。在API调用返回时，可以通过GetResult("vNewID")获取其值
            broker.AssignNormalValue("vNewID", "");

            //给普通参数DomConfig赋值。此参数的数据类型为MSXML2.IXMLDOMDocument2，此参数按引用传递，表示ATO,PTO选配
            MSXML2.IXMLDOMDocument2 domMsg = new MSXML2.DOMDocumentClass();
            broker.AssignNormalValue("DomConfig", domMsg);

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
            //获取普通返回值。此返回值数据类型为System.String，此参数按值传递，表示成功为空串
            System.String result = broker.GetReturnValue() as System.String;

            //获取out/inout参数值

            //获取普通INOUT参数vNewID。此返回值数据类型为string，在使用该参数之前，请判断是否为空
            string vNewIDRet = broker.GetResult("vNewID") as string;
            //结束本次调用，释放API资源
            broker.Release();
            return result;

            #endregion
        }


    }
}
