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
using System.Data.SqlClient;


namespace BLL
{
    public class XSDD_U8API
    {
        /// <summary>
        /// 生成销售订单
        /// </summary>
        /// <param name="strBillNo">网上订单单据编号</param>
        /// <returns></returns>
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

            //销售所有接口均支持内部独立事务和外部事务，默认内部事务
            //如果是外部事务，则需要传递ADO.Connection对象，并将IsIndependenceTransaction属性设置为false
            SqlConnection cnn = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString);
            envContext.BizDbConnection = cnn;
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
            ADODB.Connection conn = new ADODB.ConnectionClass();
            ADODB.Recordset rs = new ADODB.RecordsetClass();
            MSXML2.DOMDocument domhead = new MSXML2.DOMDocumentClass();
            string strConn = envContext.U8Login.UfDbName;
            conn.Open(strConn, "sa", envContext.U8Login.SysPassword, 0);
            string sql = "select cast(null as nvarchar(2)) as editprop,a.*,b.* from SaleOrderQ a left join SO_SOMain_extradefine b ON b.ID = a.id WHERE 1=2";
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
        /// 打开后关闭指定的U8销售订单
        /// </summary>
        /// <param name="U8csocode">U8销售订单号</param>
        /// <returns></returns>
        public String OpenAndCloseU8Order(string U8csocode, bool bclose)
        {
            //第一步：构造u8login对象并登陆(引用U8API类库中的Interop.U8Login.dll)
            //如果当前环境中有login对象则可以省去第一步
            U8Login.clsLogin u8Login = new U8Login.clsLogin();
            String sSerial = "";
            String sSubId = System.Web.Configuration.WebConfigurationManager.AppSettings["sSubId"];
            String sYear = "2015";
            String sDate = DateTime.Now.ToString("yyyy-MM-dd");
            String sAccID = System.Web.Configuration.WebConfigurationManager.AppSettings["sAccID"].Replace("test", "测试服务").ToString();
            String sUserID = System.Web.Configuration.WebConfigurationManager.AppSettings["sUserID"];
            String sPassword = System.Web.Configuration.WebConfigurationManager.AppSettings["sPassword"];
            String sServer = System.Web.Configuration.WebConfigurationManager.AppSettings["sServer"];
            if (!u8Login.Login(ref sSubId, ref sAccID, ref sYear, ref sUserID, ref sPassword, ref sDate, ref sServer, ref sSerial))
            {
                Console.WriteLine("登陆失败，原因：" + u8Login.ShareString);
                Marshal.FinalReleaseComObject(u8Login);
                return "登陆失败，原因：" + u8Login.ShareString;
            }

            //第二步：构造环境上下文对象，传入login，并按需设置其它上下文参数
            U8EnvContext envContext = new U8EnvContext();
            envContext.U8Login = u8Login;

            //销售所有接口均支持内部独立事务和外部事务，默认内部事务
            //如果是外部事务，则需要传递ADO.Connection对象，并将IsIndependenceTransaction属性设置为false
            //envContext.BizDbConnection = new ADO.Connection();
            //envContext.IsIndependenceTransaction = false;

            //设置上下文参数
            envContext.SetApiContext("VoucherType", 12); //上下文数据类型：int，含义：单据类型：12

            //第三步：设置API地址标识(Url)
            //当前API：装载单据的地址标识为：U8API/SaleOrder/Load
            U8ApiAddress myApiAddress = new U8ApiAddress("U8API/SaleOrder/Load");

            //第四步：构造APIBroker
            U8ApiBroker broker = new U8ApiBroker(myApiAddress, envContext);

            //第五步：API参数赋值
            //获取VouchID
            DataTable u8dt = new OrderManager().DL_U8OrderDataBySel(U8csocode);
            if (u8dt.Rows.Count < 1)
            {
                return "未查询到对应订单信息";
            }
            //给普通参数VouchID赋值。此参数的数据类型为string，此参数按值传递，表示单据号
            broker.AssignNormalValue("VouchID", u8dt.Rows[0]["ID"].ToString());

            //给普通参数blnAuth赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否控制权限：true
            broker.AssignNormalValue("blnAuth", true);

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
                return "调用API装载订单失败";
            }

            //第七步：获取返回结果

            //获取返回值
            //获取普通返回值。此返回值数据类型为System.String，此参数按值传递，表示成功为空串
            System.String result = broker.GetReturnValue() as System.String;
            if (!string.IsNullOrEmpty(result))
            {
                return result;
            }
            //获取out/inout参数值

            #region 打开订单,或者关闭订单传参数"bclose"
            //out参数domHead为BO对象(表头)，此BO对象的业务类型为销售订单。BO参数均按引用传递，具体请参考服务接口定义
            //如果要取原始的XMLDOM对象结果，请使用GetResult("domHead") as MSXML2.DOMDocument
            BusinessObject domHeadRet = broker.GetBoParam("domHead");
            Console.WriteLine("BO对象(表头)行数为：" + domHeadRet.RowCount); //获取BO对象(表头)的行数
            //获取BO对象(表头)各字段的值。字段定义详见API服务接口定义

            /****************************** 以下是必输字段 ****************************/
            int id = Convert.ToInt32(domHeadRet[0]["id"]); //主关键字段，int类型
            string csocode = Convert.ToString(domHeadRet[0]["csocode"]); //订 单 号，string类型
            DateTime ddate = Convert.ToDateTime(domHeadRet[0]["ddate"]); //订单日期，DateTime类型
            string cbustype = Convert.ToString(domHeadRet[0]["cbustype"]); //业务类型，int类型
            string cstname = Convert.ToString(domHeadRet[0]["cstname"]); //销售类型，string类型
            string ccusabbname = Convert.ToString(domHeadRet[0]["ccusabbname"]); //客户简称，string类型
            string cdepname = Convert.ToString(domHeadRet[0]["cdepname"]); //销售部门，string类型
            double itaxrate = Convert.ToDouble(domHeadRet[0]["itaxrate"]); //税率，double类型
            string cexch_name = Convert.ToString(domHeadRet[0]["cexch_name"]); //币种，string类型
            string cmaker = Convert.ToString(domHeadRet[0]["cmaker"]); //制单人，string类型
            string breturnflag = Convert.ToString(domHeadRet[0]["breturnflag"]); //退货标志，string类型
            string ufts = Convert.ToString(domHeadRet[0]["ufts"]); //时间戳，string类型
            string cstcode = Convert.ToString(domHeadRet[0]["cstcode"]); //销售类型编号，string类型
            string cdepcode = Convert.ToString(domHeadRet[0]["cdepcode"]); //部门编码，string类型
            string ccuscode = Convert.ToString(domHeadRet[0]["ccuscode"]); //客户编码，string类型
            string ccushand = Convert.ToString(domHeadRet[0]["ccushand"]); //客户联系人手机，string类型
            string cpsnophone = Convert.ToString(domHeadRet[0]["cpsnophone"]); //业务员办公电话，string类型
            string cpsnmobilephone = Convert.ToString(domHeadRet[0]["cpsnmobilephone"]); //业务员手机，string类型
            string cattachment = Convert.ToString(domHeadRet[0]["cattachment"]); //附件，string类型
            string csscode = Convert.ToString(domHeadRet[0]["csscode"]); //结算方式编码，string类型
            string cssname = Convert.ToString(domHeadRet[0]["cssname"]); //结算方式，string类型
            string cinvoicecompany = Convert.ToString(domHeadRet[0]["cinvoicecompany"]); //开票单位编码，string类型
            string cinvoicecompanyabbname = Convert.ToString(domHeadRet[0]["cinvoicecompanyabbname"]); //开票单位简称，string类型
            string ccuspersoncode = Convert.ToString(domHeadRet[0]["ccuspersoncode"]); //联系人编码，string类型
            string dclosedate = Convert.ToString(domHeadRet[0]["dclosedate"]); //关闭日期，string类型
            string dclosesystime = Convert.ToString(domHeadRet[0]["dclosesystime"]); //关闭时间，string类型
            string bmustbook = Convert.ToString(domHeadRet[0]["bmustbook"]); //必有定金，string类型
            string fbookratio = Convert.ToString(domHeadRet[0]["fbookratio"]); //定金比例，string类型
            string cgathingcode = Convert.ToString(domHeadRet[0]["cgathingcode"]); //收款单号，string类型
            string fbooksum = Convert.ToString(domHeadRet[0]["fbooksum"]); //定金原币金额，string类型
            string fbooknatsum = Convert.ToString(domHeadRet[0]["fbooknatsum"]); //定金本币金额，string类型
            string fgbooknatsum = Convert.ToString(domHeadRet[0]["fgbooknatsum"]); //定金累计实收本币金额，string类型
            string fgbooksum = Convert.ToString(domHeadRet[0]["fgbooksum"]); //定金累计实收原币金额，string类型
            string ccrmpersonname = Convert.ToString(domHeadRet[0]["ccrmpersonname"]); //相关员工，string类型
            string csysbarcode = Convert.ToString(domHeadRet[0]["csysbarcode"]); //单据条码，string类型
            string ioppid = Convert.ToString(domHeadRet[0]["ioppid"]); //销售机会ID，string类型
            string contract_status = Convert.ToString(domHeadRet[0]["contract_status"]); //contract_status，string类型
            string csvouchtype = Convert.ToString(domHeadRet[0]["csvouchtype"]); //来源电商，string类型
            string bcashsale = Convert.ToString(domHeadRet[0]["bcashsale"]); //现款结算，string类型
            string iflowid = Convert.ToString(domHeadRet[0]["iflowid"]); //流程id，string类型
            string cflowname = Convert.ToString(domHeadRet[0]["cflowname"]); //流程分支描述，string类型
            string cchangeverifier = Convert.ToString(domHeadRet[0]["cchangeverifier"]); //变更审批人，string类型
            string dchangeverifydate = Convert.ToString(domHeadRet[0]["dchangeverifydate"]); //变更审批日期，string类型
            string dchangeverifytime = Convert.ToString(domHeadRet[0]["dchangeverifytime"]); //变更审批时间，string类型

            //结束本次调用，释放API资源
            //broker.Release();


            //第二步：构造环境上下文对象，传入login，并按需设置其它上下文参数
            //U8EnvContext envContext = new U8EnvContext();
            //envContext.U8Login = u8Login;

            //销售所有接口均支持内部独立事务和外部事务，默认内部事务
            //如果是外部事务，则需要传递ADO.Connection对象，并将IsIndependenceTransaction属性设置为false
            //envContext.BizDbConnection = new ADO.Connection();
            //envContext.IsIndependenceTransaction = false;

            //设置上下文参数
            envContext.SetApiContext("VoucherType", 12); //上下文数据类型：int，含义：单据类型:12

            //第三步：设置API地址标识(Url)
            //当前API：关闭或打开的地址标识为：U8API/SaleOrder/Close
            myApiAddress = new U8ApiAddress("U8API/SaleOrder/Close");

            //第四步：构造APIBroker
            broker = new U8ApiBroker(myApiAddress, envContext);

            //第五步：API参数赋值

            //给BO表头参数domHead赋值，此BO参数的业务类型为销售订单，属表头参数。BO参数均按引用传递
            //提示：给BO表头参数domHead赋值有两种方法

            //方法一是直接传入MSXML2.DOMDocumentClass对象
            ADODB.Connection conn = new ADODB.ConnectionClass();
            ADODB.Recordset rs = new ADODB.RecordsetClass();
            MSXML2.DOMDocument domhead = new MSXML2.DOMDocumentClass();
            string strConn = envContext.U8Login.UfDbName;
            conn.Open(strConn, "sa", envContext.U8Login.SysPassword, 0);
            string sql = "select cast(null as nvarchar(2)) as editprop,a.*,b.* from SaleOrderQ a left join SO_SOMain_extradefine b ON b.ID = a.id WHERE 1=2";
            rs.Open(sql, conn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockOptimistic, -1);
            rs.Save(domhead, ADODB.PersistFormatEnum.adPersistXML);

            //DataTable dtHead = new BLL.OrderManager().DL_U8OrderDataBySel(TextBox1.Text);

            U8APIHelper.FormatDom(ref domhead, "M");
            broker.AssignNormalValue("DomHead", domhead);
            //ENUtil.SetToDomH(domhead, "id", dtHead.Rows[0]["id"].ToString());
            //ENUtil.SetToDomH(domhead, "csocode", dtHead.Rows[0]["csocode"].ToString());
            //ENUtil.SetToDomH(domhead, "ddate", dtHead.Rows[0]["ddate"].ToString());
            //ENUtil.SetToDomH(domhead, "cbustype", dtHead.Rows[0]["cbustype"].ToString());
            //ENUtil.SetToDomH(domhead, "cstname", dtHead.Rows[0]["cstname"].ToString());
            //ENUtil.SetToDomH(domhead, "ccusabbname", dtHead.Rows[0]["ccusabbname"].ToString());
            //ENUtil.SetToDomH(domhead, "cdepname", dtHead.Rows[0]["cdepname"].ToString());
            //ENUtil.SetToDomH(domhead, "itaxrate", dtHead.Rows[0]["itaxrate"].ToString());
            //ENUtil.SetToDomH(domhead, "cexch_name", dtHead.Rows[0]["cexch_name"].ToString());
            //ENUtil.SetToDomH(domhead, "cmaker", dtHead.Rows[0]["cmaker"].ToString());
            //ENUtil.SetToDomH(domhead, "breturnflag", dtHead.Rows[0]["breturnflag"].ToString());
            //ENUtil.SetToDomH(domhead, "ufts", dtHead.Rows[0]["ufts"].ToString());
            //ENUtil.SetToDomH(domhead, "cstcode", dtHead.Rows[0]["cstcode"].ToString());
            //ENUtil.SetToDomH(domhead, "cdepcode", dtHead.Rows[0]["cdepcode"].ToString());
            //ENUtil.SetToDomH(domhead, "ccuscode", dtHead.Rows[0]["ccuscode"].ToString());
            //ENUtil.SetToDomH(domhead, "ccushand", dtHead.Rows[0]["ccushand"].ToString());
            //ENUtil.SetToDomH(domhead, "cpsnophone", dtHead.Rows[0]["cpsnophone"].ToString());
            //ENUtil.SetToDomH(domhead, "cpsnmobilephone", dtHead.Rows[0]["cpsnmobilephone"].ToString());
            //ENUtil.SetToDomH(domhead, "cattachment", dtHead.Rows[0]["cattachment"].ToString());
            //ENUtil.SetToDomH(domhead, "csscode", dtHead.Rows[0]["csscode"].ToString());
            //ENUtil.SetToDomH(domhead, "cssname", dtHead.Rows[0]["cssname"].ToString());
            //ENUtil.SetToDomH(domhead, "cinvoicecompany", dtHead.Rows[0]["cinvoicecompany"].ToString());
            //ENUtil.SetToDomH(domhead, "cinvoicecompanyabbname", dtHead.Rows[0]["cinvoicecompanyabbname"].ToString());
            //ENUtil.SetToDomH(domhead, "ccuspersoncode", dtHead.Rows[0]["ccuspersoncode"].ToString());
            //ENUtil.SetToDomH(domhead, "dclosedate", dtHead.Rows[0]["dclosedate"].ToString());
            //ENUtil.SetToDomH(domhead, "dclosesystime", dtHead.Rows[0]["dclosesystime"].ToString());
            //ENUtil.SetToDomH(domhead, "bmustbook", dtHead.Rows[0]["bmustbook"].ToString());
            //ENUtil.SetToDomH(domhead, "fbookratio", dtHead.Rows[0]["fbookratio"].ToString());
            //ENUtil.SetToDomH(domhead, "cgathingcode", dtHead.Rows[0]["cgathingcode"].ToString());
            //ENUtil.SetToDomH(domhead, "fbooksum", dtHead.Rows[0]["fbooksum"].ToString());
            //ENUtil.SetToDomH(domhead, "fbooknatsum", dtHead.Rows[0]["fbooknatsum"].ToString());
            //ENUtil.SetToDomH(domhead, "fgbooknatsum", dtHead.Rows[0]["fgbooknatsum"].ToString());
            //ENUtil.SetToDomH(domhead, "fgbooksum", dtHead.Rows[0]["fgbooksum"].ToString());
            //ENUtil.SetToDomH(domhead, "ccrmpersonname", dtHead.Rows[0]["ccrmpersonname"].ToString());
            //ENUtil.SetToDomH(domhead, "csysbarcode", dtHead.Rows[0]["csysbarcode"].ToString());
            //ENUtil.SetToDomH(domhead, "ioppid", dtHead.Rows[0]["ioppid"].ToString());
            //ENUtil.SetToDomH(domhead, "contract_status", dtHead.Rows[0]["contract_status"].ToString());
            //ENUtil.SetToDomH(domhead, "csvouchtype", dtHead.Rows[0]["csvouchtype"].ToString());
            //ENUtil.SetToDomH(domhead, "bcashsale", dtHead.Rows[0]["bcashsale"].ToString());
            //ENUtil.SetToDomH(domhead, "iflowid", dtHead.Rows[0]["iflowid"].ToString());
            //ENUtil.SetToDomH(domhead, "cflowname", dtHead.Rows[0]["cflowname"].ToString());
            //ENUtil.SetToDomH(domhead, "cchangeverifier", dtHead.Rows[0]["cchangeverifier"].ToString());
            //ENUtil.SetToDomH(domhead, "dchangeverifydate", dtHead.Rows[0]["dchangeverifydate"].ToString());
            //ENUtil.SetToDomH(domhead, "dchangeverifytime", dtHead.Rows[0]["dchangeverifytime"].ToString());
            ENUtil.SetToDomH(domhead, "id", id);
            ENUtil.SetToDomH(domhead, "csocode", csocode);
            ENUtil.SetToDomH(domhead, "ddate", ddate);
            ENUtil.SetToDomH(domhead, "cbustype", cbustype);
            ENUtil.SetToDomH(domhead, "cstname", cstname);
            ENUtil.SetToDomH(domhead, "ccusabbname", ccusabbname);
            ENUtil.SetToDomH(domhead, "cdepname", cdepname);
            ENUtil.SetToDomH(domhead, "itaxrate", itaxrate);
            ENUtil.SetToDomH(domhead, "cexch_name", cexch_name);
            ENUtil.SetToDomH(domhead, "cmaker", cmaker);
            ENUtil.SetToDomH(domhead, "breturnflag", breturnflag);
            ENUtil.SetToDomH(domhead, "ufts", ufts);
            ENUtil.SetToDomH(domhead, "cstcode", cstcode);
            ENUtil.SetToDomH(domhead, "cdepcode", cdepcode);
            ENUtil.SetToDomH(domhead, "ccuscode", ccuscode);
            ENUtil.SetToDomH(domhead, "ccushand", ccushand);
            ENUtil.SetToDomH(domhead, "cpsnophone", cpsnophone);
            ENUtil.SetToDomH(domhead, "cpsnmobilephone", cpsnmobilephone);
            ENUtil.SetToDomH(domhead, "cattachment", cattachment);
            ENUtil.SetToDomH(domhead, "csscode", csscode);
            ENUtil.SetToDomH(domhead, "cssname", cssname);
            ENUtil.SetToDomH(domhead, "cinvoicecompany", cinvoicecompany);
            ENUtil.SetToDomH(domhead, "cinvoicecompanyabbname", cinvoicecompanyabbname);
            ENUtil.SetToDomH(domhead, "ccuspersoncode", ccuspersoncode);
            ENUtil.SetToDomH(domhead, "dclosedate", dclosedate);
            ENUtil.SetToDomH(domhead, "dclosesystime", dclosesystime);
            ENUtil.SetToDomH(domhead, "bmustbook", bmustbook);
            ENUtil.SetToDomH(domhead, "fbookratio", fbookratio);
            ENUtil.SetToDomH(domhead, "cgathingcode", cgathingcode);
            ENUtil.SetToDomH(domhead, "fbooksum", fbooksum);
            ENUtil.SetToDomH(domhead, "fbooknatsum", fbooknatsum);
            ENUtil.SetToDomH(domhead, "fgbooknatsum", fgbooknatsum);
            ENUtil.SetToDomH(domhead, "fgbooksum", fgbooksum);
            ENUtil.SetToDomH(domhead, "ccrmpersonname", ccrmpersonname);
            ENUtil.SetToDomH(domhead, "csysbarcode", csysbarcode);
            ENUtil.SetToDomH(domhead, "ioppid", ioppid);
            ENUtil.SetToDomH(domhead, "contract_status", contract_status);
            ENUtil.SetToDomH(domhead, "csvouchtype", csvouchtype);
            ENUtil.SetToDomH(domhead, "bcashsale", bcashsale);
            ENUtil.SetToDomH(domhead, "iflowid", iflowid);
            ENUtil.SetToDomH(domhead, "cflowname", cflowname);
            ENUtil.SetToDomH(domhead, "cchangeverifier", cchangeverifier);
            ENUtil.SetToDomH(domhead, "dchangeverifydate", dchangeverifydate);
            ENUtil.SetToDomH(domhead, "dchangeverifytime", dchangeverifytime);

            //给普通参数bclose赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示true关闭false打开
            broker.AssignNormalValue("bclose", bclose);

            //给普通参数idlsid赋值。此参数的数据类型为System.Int32，此参数按值传递，表示没用
            //broker.AssignNormalValue("idlsid", new System.Int32());

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
                return "调用打开API失败";
            }

            //第七步：获取返回结果

            //获取返回值
            //获取普通返回值。此返回值数据类型为System.String，此参数按值传递，表示成功为空串
            result = broker.GetReturnValue() as System.String;
            //if (!string.IsNullOrEmpty(result))
            //{
            //    return result;
            //}
            #endregion

            #region 关闭订单
            ////获取out/inout参数值

            ////inout参数domHead为BO对象(表头)，此BO对象的业务类型为销售订单。BO参数均按引用传递，具体请参考服务接口定义
            ////如果要取原始的XMLDOM对象结果，请使用GetResult("domHead") as MSXML2.DOMDocument
            ////BusinessObject domHeadRet = broker.GetBoParam("domHead");
            ////Console.WriteLine("BO对象(表头)行数为：" + domHeadRet.RowCount); //获取BO对象(表头)的行数
            ////获取BO对象(表头)各字段的值。字段定义详见API服务接口定义

            //#region 赋值
            ///****************************** 以下是必输字段 ****************************/
            ////int id = Convert.ToInt32(domHeadRet[0]["id"]); //主关键字段，int类型
            ////string csocode = Convert.ToString(domHeadRet[0]["csocode"]); //订 单 号，string类型
            ////DateTime ddate = Convert.ToDateTime(domHeadRet[0]["ddate"]); //订单日期，DateTime类型
            ////int cbustype = Convert.ToInt32(domHeadRet[0]["cbustype"]); //业务类型，int类型
            //////string cstname = Convert.ToString(domHeadRet[0]["cstname"]); //销售类型，string类型
            //////string ccusabbname = Convert.ToString(domHeadRet[0]["ccusabbname"]); //客户简称，string类型
            ////string cdepname = Convert.ToString(domHeadRet[0]["cdepname"]); //销售部门，string类型
            ////double itaxrate = Convert.ToDouble(domHeadRet[0]["itaxrate"]); //税率，double类型
            ////string cexch_name = Convert.ToString(domHeadRet[0]["cexch_name"]); //币种，string类型
            ////string cmaker = Convert.ToString(domHeadRet[0]["cmaker"]); //制单人，string类型
            ////string breturnflag = Convert.ToString(domHeadRet[0]["breturnflag"]); //退货标志，string类型
            ////string ufts = Convert.ToString(domHeadRet[0]["ufts"]); //时间戳，string类型
            ////string cstcode = Convert.ToString(domHeadRet[0]["cstcode"]); //销售类型编号，string类型
            ////string cdepcode = Convert.ToString(domHeadRet[0]["cdepcode"]); //部门编码，string类型
            ////string ccuscode = Convert.ToString(domHeadRet[0]["ccuscode"]); //客户编码，string类型
            ////string ccushand = Convert.ToString(domHeadRet[0]["ccushand"]); //客户联系人手机，string类型
            ////string cpsnophone = Convert.ToString(domHeadRet[0]["cpsnophone"]); //业务员办公电话，string类型
            ////string cpsnmobilephone = Convert.ToString(domHeadRet[0]["cpsnmobilephone"]); //业务员手机，string类型
            ////string cattachment = Convert.ToString(domHeadRet[0]["cattachment"]); //附件，string类型
            ////string csscode = Convert.ToString(domHeadRet[0]["csscode"]); //结算方式编码，string类型
            ////string cssname = Convert.ToString(domHeadRet[0]["cssname"]); //结算方式，string类型
            ////string cinvoicecompany = Convert.ToString(domHeadRet[0]["cinvoicecompany"]); //开票单位编码，string类型
            ////string cinvoicecompanyabbname = Convert.ToString(domHeadRet[0]["cinvoicecompanyabbname"]); //开票单位简称，string类型
            ////string ccuspersoncode = Convert.ToString(domHeadRet[0]["ccuspersoncode"]); //联系人编码，string类型
            ////string dclosedate = Convert.ToString(domHeadRet[0]["dclosedate"]); //关闭日期，string类型
            ////string dclosesystime = Convert.ToString(domHeadRet[0]["dclosesystime"]); //关闭时间，string类型
            ////string bmustbook = Convert.ToString(domHeadRet[0]["bmustbook"]); //必有定金，string类型
            ////string fbookratio = Convert.ToString(domHeadRet[0]["fbookratio"]); //定金比例，string类型
            ////string cgathingcode = Convert.ToString(domHeadRet[0]["cgathingcode"]); //收款单号，string类型
            ////string fbooksum = Convert.ToString(domHeadRet[0]["fbooksum"]); //定金原币金额，string类型
            ////string fbooknatsum = Convert.ToString(domHeadRet[0]["fbooknatsum"]); //定金本币金额，string类型
            ////string fgbooknatsum = Convert.ToString(domHeadRet[0]["fgbooknatsum"]); //定金累计实收本币金额，string类型
            ////string fgbooksum = Convert.ToString(domHeadRet[0]["fgbooksum"]); //定金累计实收原币金额，string类型
            ////string ccrmpersonname = Convert.ToString(domHeadRet[0]["ccrmpersonname"]); //相关员工，string类型
            ////string csysbarcode = Convert.ToString(domHeadRet[0]["csysbarcode"]); //单据条码，string类型
            ////string ioppid = Convert.ToString(domHeadRet[0]["ioppid"]); //销售机会ID，string类型
            ////string contract_status = Convert.ToString(domHeadRet[0]["contract_status"]); //contract_status，string类型
            ////string csvouchtype = Convert.ToString(domHeadRet[0]["csvouchtype"]); //来源电商，string类型
            ////string bcashsale = Convert.ToString(domHeadRet[0]["bcashsale"]); //现款结算，string类型
            ////string iflowid = Convert.ToString(domHeadRet[0]["iflowid"]); //流程id，string类型
            ////string cflowname = Convert.ToString(domHeadRet[0]["cflowname"]); //流程分支描述，string类型
            ////string cchangeverifier = Convert.ToString(domHeadRet[0]["cchangeverifier"]); //变更审批人，string类型
            ////string dchangeverifydate = Convert.ToString(domHeadRet[0]["dchangeverifydate"]); //变更审批日期，string类型
            ////string dchangeverifytime = Convert.ToString(domHeadRet[0]["dchangeverifytime"]); //变更审批时间，string类型
            //#endregion

            ////给普通参数bclose赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示true关闭false打开
            //broker.AssignNormalValue("bclose", true);

            ////给普通参数idlsid赋值。此参数的数据类型为System.Int32，此参数按值传递，表示没用
            ////broker.AssignNormalValue("idlsid", new System.Int32());

            ////第六步：调用API
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
            //        //异常原因
            //        String exReason = broker.GetExceptionString();
            //        if (exReason.Length != 0)
            //        {
            //            Console.WriteLine("异常原因：" + exReason);
            //        }
            //    }
            //    //结束本次调用，释放API资源
            //    broker.Release();
            //    return "调用关闭API失败";
            //}

            //result = broker.GetReturnValue() as System.String;
            #endregion

            //结束本次调用，释放API资源
            broker.Release();
            return result;
            //if (!string.IsNullOrEmpty(result))
            //{
            //    return result;
            //}
            //else
            //{
            //    return true;
            //}
        }

    }
}
