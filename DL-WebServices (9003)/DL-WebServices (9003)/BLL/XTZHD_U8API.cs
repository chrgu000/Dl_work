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
    public class XTZHD_U8API
    {
        /// <summary>
        /// U8API调用-不合格品入库单自动生成对应的形态转换单
        /// </summary>
        /// <param name="strBillNo">不合格品入库单编号</param>
        /// <returns></returns>
        public String AddXTZHDAPI(string strBillNo)
        {
            #region U8API调用-不合格品入库单自动生成对应的形态转换单

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

            #region 方法1，构造xml   
            //第二步：构造环境上下文对象，传入login，并按需设置其它上下文参数
            U8EnvContext envContext = new U8EnvContext();
            envContext.U8Login = u8Login;

            //第三步：设置API地址标识(Url)
            //当前API：添加新单据的地址标识为：U8API/ShapeChangVouch/Add
            U8ApiAddress myApiAddress = new U8ApiAddress("U8API/ShapeChangVouch/Add");

            //第四步：构造APIBroker
            U8ApiBroker broker = new U8ApiBroker(myApiAddress, envContext);

            //第五步：API参数赋值
            //给普通参数sVouchType赋值。此参数的数据类型为System.String，此参数按值传递，表示单据类型：15
            broker.AssignNormalValue("sVouchType", "15");


            //方法一是直接传入MSXML2.DOMDocumentClass对象
            //broker.AssignNormalValue("domHead", new MSXML2.DOMDocumentClass())


            //方法一是直接传入MSXML2.DOMDocumentClass对象
            //-------------------------------------------------------------------------------------------------------
            ADODB.Connection conn = new ADODB.ConnectionClass();
            ADODB.Recordset rs = new ADODB.RecordsetClass();
            MSXML2.DOMDocument domhead = new MSXML2.DOMDocumentClass();
            string strConn = envContext.U8Login.UfDbName;
            conn.Open(strConn, "sa", envContext.U8Login.SysPassword, 0);
            string sql = "select cast(null as nvarchar(2)) as editprop,* from AssemM where 1=0";
            rs.Open(sql, conn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockOptimistic, -1);
            rs.Save(domhead, ADODB.PersistFormatEnum.adPersistXML);
            U8APIHelper.FormatDom(ref domhead, "A");
            broker.AssignNormalValue("DomHead", domhead);

            //string strBillNo = TextBox1.Text.ToString();
            DataTable dtBody = new DataTable();
            dtBody = new BLL.OrderManager().DLproc_rdrecord10_to_AssemVouchBySel(strBillNo);

            if (dtBody.Rows.Count < 1)
            {
                return "没有数据！";
            }
            if (dtBody.Rows.Count > 0)
            {
                //方法一是直接传入MSXML2.DOMDocumentClass对象，表头
                ENUtil.SetToDomH(domhead, "id", "1");
                ENUtil.SetToDomH(domhead, "editprop", "A");
                ENUtil.SetToDomH(domhead, "cavcode", "999999");
                ENUtil.SetToDomH(domhead, "cvouchtype", "15");
                //ENUtil.SetToDomH(domhead, "davdate", DateTime.Now.ToString("yyyy-MM-dd"));
                ENUtil.SetToDomH(domhead, "davdate", "2017-06-20");
                ENUtil.SetToDomH(domhead, "cirdcode", "0121");
                ENUtil.SetToDomH(domhead, "cordcode", "0226");
                ENUtil.SetToDomH(domhead, "cavmemo", strBillNo);
                ENUtil.SetToDomH(domhead, "cmaker", "王俊杰");
                ENUtil.SetToDomH(domhead, "btransflag", "0");
                //ENUtil.SetToDomH(domhead, "vt_id", "91");
                //ENUtil.SetToDomH(domhead, "dnmaketime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                ENUtil.SetToDomH(domhead, "dnmaketime", "2017-05-12");
                //ENUtil.SetToDomH(domhead, "iprintcount", "0");
                ENUtil.SetToDomH(domhead, "ctransflag", "1");
                ENUtil.SetToDomH(domhead, "iswfcontrolled", "1");
            }

            //方法一是直接传入MSXML2.DOMDocumentClass对象，表体
            MSXML2.DOMDocument domBody = new MSXML2.DOMDocumentClass();
            ADODB.Recordset rs1 = new ADODB.RecordsetClass();
            sql = "select cast(null as nvarchar(2)) as editprop, *  from AssemD  where 1=0";
            rs1.Open(sql, conn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockOptimistic, -1);

            rs1.Save(domBody, ADODB.PersistFormatEnum.adPersistXML);
            U8APIHelper.FormatDom(ref domBody, "A");
            broker.AssignNormalValue("domBody", domBody);

            if (dtBody.Rows.Count > 0)
            {
                int rownum = 0;
                for (int i = 0; i < dtBody.Rows.Count; i++)
                {
                    rownum = i;
                    ENUtil.SetToDomB(domBody, rownum, "id", "1");
                    ENUtil.SetToDomB(domBody, rownum, "editprop", "A");
                    ENUtil.SetToDomB(domBody, rownum, "bavtype", dtBody.Rows[i]["bavtype"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "cwhcode", "2001");
                    ENUtil.SetToDomB(domBody, rownum, "cinvcode", dtBody.Rows[i]["cinvcode"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "iavnum", dtBody.Rows[i]["iavnum"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "iavquantity", dtBody.Rows[i]["iavquantity"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "cassunit", dtBody.Rows[i]["cassunit"].ToString());
                    ENUtil.SetToDomB(domBody, rownum, "bcosting", "1");
                    ENUtil.SetToDomB(domBody, rownum, "iexpiratdatecalcu", "0");
                    ENUtil.SetToDomB(domBody, rownum, "irowno", i+1);
                    ENUtil.SetToDomB(domBody, rownum, "igroupno", "1");
                }
            }

            //给普通参数domPosition赋值。此参数的数据类型为System.Object，此参数按引用传递，表示货位：传空
            broker.AssignNormalValue("domPosition", null);

            //该参数errMsg为OUT型参数，由于其数据类型为System.String，为一般值类型，因此不必传入一个参数变量。在API调用返回时，可以通过GetResult("errMsg")获取其值

            ////给普通参数cnnFrom赋值。此参数的数据类型为ADODB.Connection，此参数按引用传递，表示连接对象,如果由调用方控制事务，则需要设置此连接对象，否则传空
            broker.AssignNormalValue("cnnFrom", null);

            //该参数VouchId为INOUT型普通参数。此参数的数据类型为System.String，此参数按值传递。在API调用返回时，可以通过GetResult("VouchId")获取其值
            broker.AssignNormalValue("VouchId", "");

            //该参数domMsg为OUT型参数，由于其数据类型为MSXML2.IXMLDOMDocument2，非一般值类型，因此必须传入一个参数变量。在API调用返回时，可以直接使用该参数
            //MSXML2.IXMLDOMDocument2 domMsg = new MSXML2.IXMLDOMDocument2;
            //MSXML2.IXMLDOMDocument2 domMsg = new MSXML2.DOMDocument();
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




            ////该参数vNewID为INOUT型普通参数。此参数的数据类型为string，此参数按值传递。在API调用返回时，可以通过GetResult("vNewID")获取其值
            //broker.AssignNormalValue("vNewID", "");

            ////给普通参数DomConfig赋值。此参数的数据类型为MSXML2.IXMLDOMDocument2，此参数按引用传递，表示ATO,PTO选配
            //MSXML2.IXMLDOMDocument2 domMsg = new MSXML2.DOMDocumentClass();
            //broker.AssignNormalValue("DomConfig", domMsg);
            #endregion

            #region 方法2，系统obj
            ////第二步：构造环境上下文对象，传入login，并按需设置其它上下文参数
            //U8EnvContext envContext = new U8EnvContext();
            //envContext.U8Login = u8Login;

            ////第三步：设置API地址标识(Url)
            ////当前API：添加新单据的地址标识为：U8API/ShapeChangVouch/Add
            //U8ApiAddress myApiAddress = new U8ApiAddress("U8API/ShapeChangVouch/Add");

            ////第四步：构造APIBroker
            //U8ApiBroker broker = new U8ApiBroker(myApiAddress, envContext);

            ////第五步：API参数赋值

            ////给普通参数sVouchType赋值。此参数的数据类型为System.String，此参数按值传递，表示单据类型：15
            //broker.AssignNormalValue("sVouchType", "15");

            ////给BO表头参数DomHead赋值，此BO参数的业务类型为形态转换单，属表头参数。BO参数均按引用传递
            ////提示：给BO表头参数DomHead赋值有两种方法

            ////方法一是直接传入MSXML2.DOMDocumentClass对象
            ////broker.AssignNormalValue("DomHead", new MSXML2.DOMDocumentClass())

            ////方法二是构造BusinessObject对象，具体方法如下：
            //BusinessObject DomHead = broker.GetBoParam("DomHead");
            //DomHead.RowCount = 1; //设置BO对象(表头)行数，只能为一行
            ////给BO对象(表头)的字段赋值，值可以是真实类型，也可以是无类型字符串
            ////以下代码示例只设置第一行值。各字段定义详见API服务接口定义

            ///****************************** 以下是必输字段 ****************************/
            //DomHead[0]["id"] = "0"; //主关键字段，int类型
            //DomHead[0]["davdate"] = "2017-05-09 00:00:00"; //日期，DateTime类型
            //DomHead[0]["cavcode"] = "20170509"; //单据号，string类型
            //DomHead[0]["ctransflag"] = "0"; //转换方式，string类型
            //DomHead[0]["ireturncount"] = "1"; //打回次数，string类型
            //DomHead[0]["iswfcontrolled"] = "1"; //是否工作流控制，string类型
            //DomHead[0]["iverifystate"] = "0"; //状态，string类型
            //DomHead[0]["csysbarcode"] = ""; //单据条码，string类型

            ///***************************** 以下是非必输字段 ****************************/
            ////DomHead[0]["cmodifyperson"] = ""; //修改人，string类型
            ////DomHead[0]["dmodifydate"] = ""; //修改日期，DateTime类型
            //DomHead[0]["dnmaketime"] = "2017-05-09"; //制单时间，DateTime类型
            ////DomHead[0]["dnmodifytime"] = ""; //修改时间，DateTime类型
            ////DomHead[0]["dnverifytime"] = ""; //审核时间，DateTime类型
            ////DomHead[0]["cdepname"] = ""; //部门，string类型
            //DomHead[0]["crdname"] = "0121"; //入库类别，string类型
            //DomHead[0]["crdname_1"] = "0226"; //出库类别，string类型
            ////DomHead[0]["cpersonname"] = ""; //经手人，string类型
            ////DomHead[0]["dverifydate"] = ""; //审核日期，DateTime类型
            //DomHead[0]["cavmemo"] = strBillNo; //备注，string类型
            ////DomHead[0]["iamount"] = ""; //现存量，string类型
            //DomHead[0]["cmaker"] = "王俊杰"; //制单人，string类型
            ////DomHead[0]["cverifyperson"] = ""; //审核人，string类型
            ////DomHead[0]["ufts"] = ""; //时间戳，string类型
            ////DomHead[0]["iavexpense"] = ""; //费用，double类型
            ////DomHead[0]["cdepcode"] = ""; //部门编码，string类型
            ////DomHead[0]["cvouchname"] = ""; //单据类型名，string类型
            ////DomHead[0]["cirdcode"] = ""; //入库类别编码，string类型
            ////DomHead[0]["cordcode"] = ""; //出库类别编码，string类型
            ////DomHead[0]["cpersoncode"] = ""; //经手人编码，string类型
            ////DomHead[0]["cvouchtype"] = ""; //单据类型，string类型
            ////DomHead[0]["vt_id"] = ""; //单据模版号，int类型
            ////DomHead[0]["cdefine16"] = ""; //表头自定义项16，double类型
            ////DomHead[0]["btransflag"] = ""; //是否传递过，string类型
            ////DomHead[0]["iavaquantity"] = ""; //可用量，string类型
            ////DomHead[0]["iavanum"] = ""; //可用件数，string类型
            ////DomHead[0]["ipresentnum"] = ""; //现存件数，string类型
            ////DomHead[0]["isafesum"] = ""; //安全库存量，string类型
            ////DomHead[0]["itopsum"] = ""; //最高库存量，string类型
            ////DomHead[0]["ilowsum"] = ""; //最低库存量，string类型
            ////DomHead[0]["caccounter"] = ""; //记账人，string类型
            ////DomHead[0]["cdefine1"] = ""; //表头自定义项1，string类型
            ////DomHead[0]["cdefine2"] = ""; //表头自定义项2，string类型
            ////DomHead[0]["cdefine3"] = ""; //表头自定义项3，string类型
            ////DomHead[0]["cdefine4"] = ""; //表头自定义项4，DateTime类型
            ////DomHead[0]["cdefine5"] = ""; //表头自定义项5，int类型
            ////DomHead[0]["cdefine6"] = ""; //表头自定义项6，DateTime类型
            ////DomHead[0]["cdefine7"] = ""; //表头自定义项7，double类型
            ////DomHead[0]["cdefine8"] = ""; //表头自定义项8，string类型
            ////DomHead[0]["cdefine9"] = ""; //表头自定义项9，string类型
            ////DomHead[0]["cdefine10"] = ""; //表头自定义项10，string类型
            ////DomHead[0]["cdefine11"] = ""; //表头自定义项11，string类型
            ////DomHead[0]["cdefine12"] = ""; //表头自定义项12，string类型
            ////DomHead[0]["cdefine13"] = ""; //表头自定义项13，string类型
            ////DomHead[0]["cdefine14"] = ""; //表头自定义项14，string类型
            ////DomHead[0]["cdefine15"] = ""; //表头自定义项15，int类型

            ////给BO表体参数domBody赋值，此BO参数的业务类型为形态转换单，属表体参数。BO参数均按引用传递
            ////提示：给BO表体参数domBody赋值有两种方法

            ////方法一是直接传入MSXML2.DOMDocumentClass对象
            ////broker.AssignNormalValue("domBody", new MSXML2.DOMDocumentClass())

            ////方法二是构造BusinessObject对象，具体方法如下：
            //BusinessObject domBody = broker.GetBoParam("domBody");
            //domBody.RowCount = 2; //设置BO对象行数
            ////可以自由设置BO对象行数为大于零的整数，也可以不设置而自动增加行数
            ////给BO对象的字段赋值，值可以是真实类型，也可以是无类型字符串
            ////以下代码示例只设置第一行值。各字段定义详见API服务接口定义

            ///****************************** 以下是必输字段 ****************************/
            //domBody[0]["autoid"] = ""; //主关键字段，int类型
            //domBody[0]["cinvcode"] = "01010100410"; //存货编码，string类型
            //domBody[0]["cwhname"] = "2001"; //仓库，string类型
            //domBody[0]["editprop"] = "A"; //编辑属性：A表新增，M表修改，D表删除，string类型
            //domBody[0]["cbatchproperty2"] = ""; //批次属性2，string类型
            //domBody[0]["cbatchproperty3"] = ""; //批次属性3，string类型
            //domBody[0]["cbatchproperty4"] = ""; //批次属性4，string类型
            //domBody[0]["cbatchproperty5"] = ""; //批次属性5，string类型
            //domBody[0]["cvmivencode"] = ""; //代管商编码，string类型
            //domBody[0]["cvmivenname"] = ""; //代管商，string类型
            //domBody[0]["cinvouchtype"] = ""; //对应入库单类型，string类型
            //domBody[0]["cposition"] = ""; //货位编码，string类型
            //domBody[0]["cposname"] = ""; //货位名称，string类型
            //domBody[0]["igroupno"] = "1"; //组号，string类型
            //domBody[0]["cbsysbarcode"] = ""; //单据行条码，string类型
            //domBody[0]["cbatchproperty6"] = ""; //批次属性6，string类型
            //domBody[0]["cbatchproperty7"] = ""; //批次属性7，string类型
            //domBody[0]["cbatchproperty8"] = ""; //批次属性8，string类型
            //domBody[0]["cbatchproperty1"] = ""; //批次属性1，string类型
            //domBody[0]["cbatchproperty9"] = ""; //批次属性9，string类型
            //domBody[0]["cbatchproperty10"] = ""; //批次属性10，string类型
            //domBody[0]["cciqbookcode"] = ""; //手册号，string类型
            //domBody[0]["cbmemo"] = ""; //备注，string类型
            //domBody[0]["irowno"] = 1; //行号，string类型
            //domBody[0]["iavquantity"] = "999"; //数量，double类型

            //domBody[1]["autoid"] = ""; //主关键字段，int类型
            //domBody[1]["cinvcode"] = "210402003016"; //存货编码，string类型
            //domBody[1]["cwhname"] = "2001"; //仓库，string类型
            //domBody[1]["editprop"] = "A"; //编辑属性：A表新增，M表修改，D表删除，string类型
            //domBody[1]["cbatchproperty2"] = ""; //批次属性2，string类型
            //domBody[1]["cbatchproperty3"] = ""; //批次属性3，string类型
            //domBody[1]["cbatchproperty4"] = ""; //批次属性4，string类型
            //domBody[1]["cbatchproperty5"] = ""; //批次属性5，string类型
            //domBody[1]["cvmivencode"] = ""; //代管商编码，string类型
            //domBody[1]["cvmivenname"] = ""; //代管商，string类型
            //domBody[1]["cinvouchtype"] = ""; //对应入库单类型，string类型
            //domBody[1]["cposition"] = ""; //货位编码，string类型
            //domBody[1]["cposname"] = ""; //货位名称，string类型
            //domBody[1]["igroupno"] = "1"; //组号，string类型
            //domBody[1]["cbsysbarcode"] = ""; //单据行条码，string类型
            //domBody[1]["cbatchproperty6"] = ""; //批次属性6，string类型
            //domBody[1]["cbatchproperty7"] = ""; //批次属性7，string类型
            //domBody[1]["cbatchproperty8"] = ""; //批次属性8，string类型
            //domBody[1]["cbatchproperty1"] = ""; //批次属性1，string类型
            //domBody[1]["cbatchproperty9"] = ""; //批次属性9，string类型
            //domBody[1]["cbatchproperty10"] = ""; //批次属性10，string类型
            //domBody[1]["cciqbookcode"] = ""; //手册号，string类型
            //domBody[1]["cbmemo"] = ""; //备注，string类型
            //domBody[1]["irowno"] = 2; //行号，string类型
            //domBody[1]["iavquantity"] = "999"; //数量，double类型

            ///***************************** 以下是非必输字段 ****************************/
            ////domBody[0]["bavtype"] = ""; //类型，int类型
            ////domBody[0]["cinvname"] = ""; //存货名称，string类型
            ////domBody[0]["cinvstd"] = ""; //规格型号，string类型
            ////domBody[0]["cinvm_unit"] = ""; //主计量单位，string类型
            ////domBody[0]["cfree1"] = ""; //存货自由项1，string类型
            ////domBody[0]["cfree2"] = ""; //存货自由项2，string类型
            ////domBody[0]["cavbatch"] = ""; //批号，string类型
            ////domBody[0]["iavnum"] = ""; //件数，double类型
            ////domBody[0]["iinvexchrate"] = ""; //换算率，double类型
            ////domBody[0]["iavquantity"] = ""; //数量，double类型
            ////domBody[0]["iavaprice"] = ""; //金额，double类型
            ////domBody[0]["iavpcost"] = ""; //计划单价／售价，double类型
            ////domBody[0]["iavpprice"] = ""; //计划金额／售价金额，double类型
            ////domBody[0]["cavcode"] = ""; //单据编号，string类型
            ////domBody[0]["cwhcode"] = ""; //仓库编码，string类型
            ////domBody[0]["rdsid"] = ""; //对应入库单id，int类型
            ////domBody[0]["id"] = ""; //自动编号，int类型
            ////domBody[0]["cfree3"] = ""; //存货自由项3，string类型
            ////domBody[0]["cfree4"] = ""; //存货自由项4，string类型
            ////domBody[0]["cfree5"] = ""; //存货自由项5，string类型
            ////domBody[0]["cfree6"] = ""; //存货自由项6，string类型
            ////domBody[0]["cfree7"] = ""; //存货自由项7，string类型
            ////domBody[0]["cfree8"] = ""; //存货自由项8，string类型
            ////domBody[0]["cfree9"] = ""; //存货自由项9，string类型
            ////domBody[0]["cfree10"] = ""; //存货自由项10，string类型
            ////domBody[0]["cinvouchcode"] = ""; //对应入库单号，string类型
            ////domBody[0]["cdefine34"] = ""; //表体自定义项13，int类型
            ////domBody[0]["cdefine35"] = ""; //表体自定义项14，int类型
            ////domBody[0]["cinvdefine13"] = ""; //存货自定义项13，string类型
            ////domBody[0]["cinvdefine14"] = ""; //存货自定义项14，string类型
            ////domBody[0]["cbvencode"] = ""; //供应商编码，string类型
            ////domBody[0]["cvenname"] = ""; //供应商，string类型
            ////domBody[0]["imassdate"] = ""; //保质期，int类型
            ////domBody[0]["cassunit"] = ""; //库存单位码，string类型
            ////domBody[0]["dmadedate"] = ""; //生产日期，DateTime类型
            ////domBody[0]["corufts"] = ""; //对应单据时间戳，string类型
            ////domBody[0]["cmassunit"] = ""; //保质期单位，int类型
            ////domBody[0]["cinva_unit"] = ""; //库存单位，string类型
            ////domBody[0]["iexpiratdatecalcu"] = ""; //有效期推算方式，int类型
            ////domBody[0]["cexpirationdate"] = ""; //有效期至，string类型
            ////domBody[0]["dexpirationdate"] = ""; //有效期计算项，string类型
            ////domBody[0]["bcosting"] = ""; //是否核算，string类型
            ////domBody[0]["iavacost"] = ""; //单价，double类型
            ////domBody[0]["cdefine22"] = ""; //表体自定义项1，string类型
            ////domBody[0]["cdefine28"] = ""; //表体自定义项7，string类型
            ////domBody[0]["cdefine29"] = ""; //表体自定义项8，string类型
            ////domBody[0]["cdefine30"] = ""; //表体自定义项9，string类型
            ////domBody[0]["cdefine31"] = ""; //表体自定义项10，string类型
            ////domBody[0]["cdefine32"] = ""; //表体自定义项11，string类型
            ////domBody[0]["cdefine33"] = ""; //表体自定义项12，string类型
            ////domBody[0]["cinvdefine4"] = ""; //存货自定义项4，string类型
            ////domBody[0]["cinvdefine5"] = ""; //存货自定义项5，string类型
            ////domBody[0]["cinvdefine6"] = ""; //存货自定义项6，string类型
            ////domBody[0]["cinvdefine7"] = ""; //存货自定义项7，string类型
            ////domBody[0]["cinvdefine8"] = ""; //存货自定义项8，string类型
            ////domBody[0]["cinvdefine9"] = ""; //存货自定义项9，string类型
            ////domBody[0]["cinvdefine10"] = ""; //存货自定义项10，string类型
            ////domBody[0]["cinvdefine11"] = ""; //存货自定义项11，string类型
            ////domBody[0]["cinvdefine12"] = ""; //存货自定义项12，string类型
            ////domBody[0]["cdefine23"] = ""; //表体自定义项2，string类型
            ////domBody[0]["cinvaddcode"] = ""; //存货代码，string类型
            ////domBody[0]["creplaceitem"] = ""; //替换件，string类型
            ////domBody[0]["cinvdefine1"] = ""; //存货自定义项1，string类型
            ////domBody[0]["cinvdefine2"] = ""; //存货自定义项2，string类型
            ////domBody[0]["cinvdefine3"] = ""; //存货自定义项3，string类型
            ////domBody[0]["cdefine24"] = ""; //表体自定义项3，string类型
            ////domBody[0]["cdefine25"] = ""; //表体自定义项4，string类型
            ////domBody[0]["cdefine26"] = ""; //表体自定义项5，double类型
            ////domBody[0]["cdefine27"] = ""; //表体自定义项6，double类型
            ////domBody[0]["citemcode"] = ""; //项目编码，string类型
            ////domBody[0]["cname"] = ""; //项目，string类型
            ////domBody[0]["citem_class"] = ""; //项目大类编码，string类型
            ////domBody[0]["citemcname"] = ""; //项目大类名称，string类型
            ////domBody[0]["ddisdate"] = ""; //失效日期，DateTime类型
            ////domBody[0]["cdefine36"] = ""; //表体自定义项15，DateTime类型
            ////domBody[0]["cdefine37"] = ""; //表体自定义项16，DateTime类型
            ////domBody[0]["cinvdefine15"] = ""; //存货自定义项15，string类型
            ////domBody[0]["cinvdefine16"] = ""; //存货自定义项16，string类型

            ////给普通参数domPosition赋值。此参数的数据类型为System.Object，此参数按引用传递，表示货位：传空
            //broker.AssignNormalValue("domPosition", null);

            ////该参数errMsg为OUT型参数，由于其数据类型为System.String，为一般值类型，因此不必传入一个参数变量。在API调用返回时，可以通过GetResult("errMsg")获取其值

            //////给普通参数cnnFrom赋值。此参数的数据类型为ADODB.Connection，此参数按引用传递，表示连接对象,如果由调用方控制事务，则需要设置此连接对象，否则传空
            //broker.AssignNormalValue("cnnFrom", null);

            ////该参数VouchId为INOUT型普通参数。此参数的数据类型为System.String，此参数按值传递。在API调用返回时，可以通过GetResult("VouchId")获取其值
            //broker.AssignNormalValue("VouchId", "");

            ////该参数domMsg为OUT型参数，由于其数据类型为MSXML2.IXMLDOMDocument2，非一般值类型，因此必须传入一个参数变量。在API调用返回时，可以直接使用该参数
            ////MSXML2.IXMLDOMDocument2 domMsg = new MSXML2.IXMLDOMDocument2;
            ////MSXML2.IXMLDOMDocument2 domMsg = new MSXML2.DOMDocument();
            //MSXML2.IXMLDOMDocument2 domMsg = new MSXML2.DOMDocumentClass();
            //broker.AssignNormalValue("domMsg", domMsg);

            ////给普通参数bCheck赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否控制可用量。
            //broker.AssignNormalValue("bCheck", false);

            ////给普通参数bBeforCheckStock赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示检查可用量
            //broker.AssignNormalValue("bBeforCheckStock", false);

            ////给普通参数bIsRedVouch赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否红字单据
            //broker.AssignNormalValue("bIsRedVouch", false);

            ////给普通参数sAddedState赋值。此参数的数据类型为System.String，此参数按值传递，表示传空字符串
            //broker.AssignNormalValue("sAddedState", "");

            ////给普通参数bReMote赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否远程：转入false
            //broker.AssignNormalValue("bReMote", false);

            #endregion

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
            System.Boolean result = Convert.ToBoolean(broker.GetReturnValue());

            //获取out/inout参数值

            //获取普通OUT参数errMsg。此返回值数据类型为System.String，在使用该参数之前，请判断是否为空
            System.String errMsgRet = broker.GetResult("errMsg") as System.String;

            //获取普通INOUT参数VouchId。此返回值数据类型为System.String，在使用该参数之前，请判断是否为空
            System.String VouchIdRet = broker.GetResult("VouchId") as System.String;

            //获取普通OUT参数domMsg。此返回值数据类型为MSXML2.IXMLDOMDocument2，在使用该参数之前，请判断是否为空
            //MSXML2.IXMLDOMDocument2 domMsgRet = Convert.ToObject(broker.GetResult("domMsg"));

            //结束本次调用，释放API资源
            broker.Release();
            return errMsgRet;


            #endregion
        }
    }
}
