﻿using System;
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
    public class XSDD
    {
        /// <summary>
        /// U8API生成销售订单,传入订单编号
        /// </summary>
        /// <param name="strBillNo">单据编号</param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public String AddXSDDAPI(string strBillNo, ADODB.Connection conn)
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
            String sDate = "2018-12-10";
            //String sDate = "2017-01-01";
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
            //ADODB.ConnectionClass conn = new ConnectionClass();
            //ADODB.Connection conn = new ADODB.ConnectionClass();
            //ADODB.Connection conn = new ADODB.Connection();
            envContext.BizDbConnection = conn;
            //同时需要设置一个外部事务标记：
            envContext.IsIndependenceTransaction = false;
            //envContext.IsIndependenceTransaction = true;


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
            //SQLHelper sqlhelp = new SQLHelper();
            //DataTable dtHead = new DataTable();
            //string cmdText = "DLproc_NewOrderU8BySel";
            //SqlParameter[] paras = new SqlParameter[] { 
            // new SqlParameter("@strBillNo",strBillNo)     
            // };
            //dtHead = sqlhelp.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            DataTable dtHead = new OrderManager().DLproc_NewOrderU8BySel(strBillNo);
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
                ENUtil.SetToDomH(domhead, "cdefine1", dtHead.Rows[0]["cdefine1"].ToString());
                ENUtil.SetToDomH(domhead, "cdefine2", dtHead.Rows[0]["cdefine2"].ToString());
                ENUtil.SetToDomH(domhead, "cdefine3", dtHead.Rows[0]["cdefine3"].ToString());
                ENUtil.SetToDomH(domhead, "cdefine9", dtHead.Rows[0]["cdefine9"].ToString());
                ENUtil.SetToDomH(domhead, "cdefine10", dtHead.Rows[0]["cdefine10"].ToString());
                ENUtil.SetToDomH(domhead, "cdefine11", dtHead.Rows[0]["cdefine11"].ToString());
                ENUtil.SetToDomH(domhead, "cdefine12", dtHead.Rows[0]["cdefine12"].ToString());
                ENUtil.SetToDomH(domhead, "cdefine13", dtHead.Rows[0]["cdefine13"].ToString());
                ENUtil.SetToDomH(domhead, "dpredatebt", dtHead.Rows[0]["dpredatebt"].ToString());
                ENUtil.SetToDomH(domhead, "dpremodatebt", dtHead.Rows[0]["dpremodatebt"].ToString());
                ENUtil.SetToDomH(domhead, "ccusname", dtHead.Rows[0]["ccusname"].ToString());
                ENUtil.SetToDomH(domhead, "cinvoicecompany", dtHead.Rows[0]["cinvoicecompany"].ToString());
                ENUtil.SetToDomH(domhead, "cmemo", dtHead.Rows[0]["cmemo"].ToString());
                ENUtil.SetToDomH(domhead, "cpersoncode", dtHead.Rows[0]["cpersoncode"].ToString());
                ENUtil.SetToDomH(domhead, "cSCCode", dtHead.Rows[0]["cSCCode"].ToString());
                ENUtil.SetToDomH(domhead, "cDefine4", dtHead.Rows[0]["cDefine4"].ToString());
                ENUtil.SetToDomH(domhead, "cDefine14", dtHead.Rows[0]["cDefine14"].ToString());
                ENUtil.SetToDomH(domhead, "cSTCode", dtHead.Rows[0]["cSTCode"].ToString());
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
                ENUtil.SetToDomH(domhead, "chdefine1", dtHead.Rows[0]["chdefine1"].ToString());
                ENUtil.SetToDomH(domhead, "chdefine2", dtHead.Rows[0]["chdefine2"].ToString());
                ENUtil.SetToDomH(domhead, "chdefine13", dtHead.Rows[0]["chdefine13"].ToString());
                ENUtil.SetToDomH(domhead, "chdefine19", dtHead.Rows[0]["chdefine19"].ToString());
                ENUtil.SetToDomH(domhead, "chdefine23", dtHead.Rows[0]["chdefine23"].ToString());
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
                    ENUtil.SetToDomB(domBody, rownum, "iaoids", dtHead.Rows[i]["iaoids"].ToString());
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

        /// <summary>
        /// U8API生成销售订单,传入数据表datatable
        /// </summary>
        /// <param name="dtHead">表头,表体,扩展表的数据集</param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public String AddXSDDAPI(DataTable dtHead, ADODB.Connection conn)
        {
            #region U8API调用-生成销售订单

            //第一步：构造u8login对象并登陆(引用U8API类库中的Interop.U8Login.dll)
            //如果当前环境中有login对象则可以省去第一步
            U8Login.clsLogin u8Login = new U8Login.clsLogin();
            String sSerial = "";
            String sSubId = System.Web.Configuration.WebConfigurationManager.AppSettings["sSubId"];
            String sYear = "2015";
            String sDate = "2018-12-10";
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
            //ADODB.ConnectionClass conn = new ConnectionClass();
            //ADODB.Connection conn = new ADODB.ConnectionClass();
            //ADODB.Connection conn = new ADODB.Connection();
            envContext.BizDbConnection = conn;
            //同时需要设置一个外部事务标记：
            envContext.IsIndependenceTransaction = false;
            //envContext.IsIndependenceTransaction = true;


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
            //SQLHelper sqlhelp = new SQLHelper();
            //DataTable dtHead = new DataTable();
            //string cmdText = "DLproc_NewOrderU8BySel";
            //SqlParameter[] paras = new SqlParameter[] { 
            // new SqlParameter("@strBillNo",strBillNo)     
            // };
            //dtHead = sqlhelp.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);


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
                ENUtil.SetToDomH(domhead, "cdefine1", dtHead.Rows[0]["cdefine1"].ToString());
                ENUtil.SetToDomH(domhead, "cdefine2", dtHead.Rows[0]["cdefine2"].ToString());
                ENUtil.SetToDomH(domhead, "cdefine3", dtHead.Rows[0]["cdefine3"].ToString());
                ENUtil.SetToDomH(domhead, "cdefine9", dtHead.Rows[0]["cdefine9"].ToString());
                ENUtil.SetToDomH(domhead, "cdefine10", dtHead.Rows[0]["cdefine10"].ToString());
                ENUtil.SetToDomH(domhead, "cdefine11", dtHead.Rows[0]["cdefine11"].ToString());
                ENUtil.SetToDomH(domhead, "cdefine12", dtHead.Rows[0]["cdefine12"].ToString());
                ENUtil.SetToDomH(domhead, "cdefine13", dtHead.Rows[0]["cdefine13"].ToString());
                ENUtil.SetToDomH(domhead, "dpredatebt", dtHead.Rows[0]["dpredatebt"].ToString());
                ENUtil.SetToDomH(domhead, "dpremodatebt", dtHead.Rows[0]["dpremodatebt"].ToString());
                ENUtil.SetToDomH(domhead, "ccusname", dtHead.Rows[0]["ccusname"].ToString());
                ENUtil.SetToDomH(domhead, "cinvoicecompany", dtHead.Rows[0]["cinvoicecompany"].ToString());
                ENUtil.SetToDomH(domhead, "cmemo", dtHead.Rows[0]["cmemo"].ToString());
                ENUtil.SetToDomH(domhead, "cpersoncode", dtHead.Rows[0]["cpersoncode"].ToString());
                if ( dtHead.Rows[0]["iAddressType"].ToString()!="3" ) //厂车配送
                {
                    ENUtil.SetToDomH(domhead, "csccode", dtHead.Rows[0]["csccode"].ToString());
                }
                else
                {
                    ENUtil.SetToDomH(domhead, "csccode", "01");
                }
                ENUtil.SetToDomH(domhead, "cDefine4", dtHead.Rows[0]["cDefine4"].ToString());
                ENUtil.SetToDomH(domhead, "cDefine14", dtHead.Rows[0]["cDefine14"].ToString());
                ENUtil.SetToDomH(domhead, "cSTCode", dtHead.Rows[0]["cSTCode"].ToString());
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
                ENUtil.SetToDomH(domhead, "chdefine1", dtHead.Rows[0]["chdefine1"].ToString());
                ENUtil.SetToDomH(domhead, "chdefine2", dtHead.Rows[0]["chdefine2"].ToString());
                ENUtil.SetToDomH(domhead, "chdefine13", dtHead.Rows[0]["chdefine13"].ToString());
                ENUtil.SetToDomH(domhead, "chdefine19", dtHead.Rows[0]["chdefine19"].ToString());
                ENUtil.SetToDomH(domhead, "chdefine23", dtHead.Rows[0]["chdefine23"].ToString());
                ENUtil.SetToDomH(domhead, "chdefine26", dtHead.Rows[0]["chdefine26"].ToString());
                ENUtil.SetToDomH(domhead, "chdefine31", dtHead.Rows[0]["chdefine31"].ToString());
                ENUtil.SetToDomH(domhead, "chdefine49", dtHead.Rows[0]["chdefine49"].ToString());
                ENUtil.SetToDomH(domhead, "chdefine21", dtHead.Rows[0]["chdefine21"].ToString());
                ENUtil.SetToDomH(domhead, "chdefine15", dtHead.Rows[0]["chdefine15"].ToString());

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
                    ENUtil.SetToDomB(domBody, rownum, "itaxrate", dtHead.Rows[0]["itaxrate"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "cdefine22", dtHead.Rows[i]["cdefine22"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "iinvexchrate", dtHead.Rows[i]["iinvexchrate"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "cunitid", dtHead.Rows[i]["cunitid"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "cgroupcode", dtHead.Rows[i]["cgroupcode"].ToString());//计量单位组，string类型
                    ENUtil.SetToDomB(domBody, rownum, "iGroupType", dtHead.Rows[i]["iGroupType"].ToString());//计量单位组，string类型              
                    ENUtil.SetToDomB(domBody, rownum, "irowno", dtHead.Rows[i]["irowno"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "cinvname", dtHead.Rows[i]["cinvname"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "cSOCode", dtHead.Rows[i]["cSOCode"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "cpreordercode", dtHead.Rows[i]["cpreordercode"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "iaoids", dtHead.Rows[i]["iaoids"].ToString());
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
                    ENUtil.SetToDomB(domBody, rownum, "cbdefine16", dtHead.Rows[i]["cbdefine16"].ToString());

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

        /// <summary>
        /// U8API生成销售订单,传入数据表datatable(带预约号)
        /// </summary>
        /// <param name="dtHead">表头,表体,扩展表的数据集</param>
        /// <param name="conn"></param>
        /// <param name="yuyueno">预约号</param>
        /// <returns></returns>
        public String AddXSDDAPI(DataTable dtHead, ADODB.Connection conn, string yuyueno)
        {
            #region U8API调用-生成销售订单

            //第一步：构造u8login对象并登陆(引用U8API类库中的Interop.U8Login.dll)
            //如果当前环境中有login对象则可以省去第一步
            U8Login.clsLogin u8Login = new U8Login.clsLogin();
            String sSerial = "";
            String sSubId = System.Web.Configuration.WebConfigurationManager.AppSettings["sSubId"];
            String sYear = "2015";
            String sDate = "2018-12-10";
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
            //ADODB.ConnectionClass conn = new ConnectionClass();
            //ADODB.Connection conn = new ADODB.ConnectionClass();
            //ADODB.Connection conn = new ADODB.Connection();
            envContext.BizDbConnection = conn;
            //同时需要设置一个外部事务标记：
            envContext.IsIndependenceTransaction = false;
            //envContext.IsIndependenceTransaction = true;


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
            //SQLHelper sqlhelp = new SQLHelper();
            //DataTable dtHead = new DataTable();
            //string cmdText = "DLproc_NewOrderU8BySel";
            //SqlParameter[] paras = new SqlParameter[] { 
            // new SqlParameter("@strBillNo",strBillNo)     
            // };
            //dtHead = sqlhelp.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);


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
                ENUtil.SetToDomH(domhead, "cdefine1", dtHead.Rows[0]["cdefine1"].ToString());
                ENUtil.SetToDomH(domhead, "cdefine2", dtHead.Rows[0]["cdefine2"].ToString());
                ENUtil.SetToDomH(domhead, "cdefine3", dtHead.Rows[0]["cdefine3"].ToString());
                ENUtil.SetToDomH(domhead, "cdefine9", dtHead.Rows[0]["cdefine9"].ToString());
                ENUtil.SetToDomH(domhead, "cdefine10", dtHead.Rows[0]["cdefine10"].ToString());
                ENUtil.SetToDomH(domhead, "cdefine11", dtHead.Rows[0]["cdefine11"].ToString());
                ENUtil.SetToDomH(domhead, "cdefine12", dtHead.Rows[0]["cdefine12"].ToString());
                ENUtil.SetToDomH(domhead, "cdefine13", dtHead.Rows[0]["cdefine13"].ToString());
                ENUtil.SetToDomH(domhead, "dpredatebt", dtHead.Rows[0]["dpredatebt"].ToString());
                ENUtil.SetToDomH(domhead, "dpremodatebt", dtHead.Rows[0]["dpremodatebt"].ToString());
                ENUtil.SetToDomH(domhead, "ccusname", dtHead.Rows[0]["ccusname"].ToString());
                ENUtil.SetToDomH(domhead, "cinvoicecompany", dtHead.Rows[0]["cinvoicecompany"].ToString());
                ENUtil.SetToDomH(domhead, "cmemo", dtHead.Rows[0]["cmemo"].ToString());
                ENUtil.SetToDomH(domhead, "cpersoncode", dtHead.Rows[0]["cpersoncode"].ToString());
                if (dtHead.Rows[0]["iAddressType"].ToString() != "3")
                {
                    ENUtil.SetToDomH(domhead, "csccode", dtHead.Rows[0]["csccode"].ToString());
                }
                else
                {
                    ENUtil.SetToDomH(domhead, "csccode", "01");
                }
                ENUtil.SetToDomH(domhead, "cDefine4", dtHead.Rows[0]["cDefine4"].ToString());
                ENUtil.SetToDomH(domhead, "cDefine14", dtHead.Rows[0]["cDefine14"].ToString());
                ENUtil.SetToDomH(domhead, "cSTCode", dtHead.Rows[0]["cSTCode"].ToString());
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
                ENUtil.SetToDomH(domhead, "chdefine1", dtHead.Rows[0]["chdefine1"].ToString());
                ENUtil.SetToDomH(domhead, "chdefine2", dtHead.Rows[0]["chdefine2"].ToString());
                ENUtil.SetToDomH(domhead, "chdefine13", dtHead.Rows[0]["chdefine13"].ToString());
                ENUtil.SetToDomH(domhead, "chdefine19", dtHead.Rows[0]["chdefine19"].ToString());
                ENUtil.SetToDomH(domhead, "chdefine23", dtHead.Rows[0]["chdefine23"].ToString());
                ENUtil.SetToDomH(domhead, "chdefine26", dtHead.Rows[0]["chdefine26"].ToString());
                ENUtil.SetToDomH(domhead, "chdefine31", dtHead.Rows[0]["chdefine31"].ToString());
                ENUtil.SetToDomH(domhead, "chdefine49", dtHead.Rows[0]["chdefine49"].ToString());
                ENUtil.SetToDomH(domhead, "chdefine21", dtHead.Rows[0]["chdefine21"].ToString());
                ENUtil.SetToDomH(domhead, "chdefine15", dtHead.Rows[0]["chdefine15"].ToString());
                ENUtil.SetToDomH(domhead, "chdefine50", yuyueno);

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
                    ENUtil.SetToDomB(domBody, rownum, "iaoids", dtHead.Rows[i]["iaoids"].ToString());
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
                    ENUtil.SetToDomB(domBody, rownum, "cbdefine16", dtHead.Rows[i]["cbdefine16"].ToString());

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