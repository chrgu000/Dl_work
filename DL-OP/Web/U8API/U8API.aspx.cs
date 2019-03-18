using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


using System.Text;
using System.Runtime.InteropServices;
using UFIDA.U8.MomServiceCommon;
using UFIDA.U8.U8MOMAPIFramework;
using UFIDA.U8.U8APIFramework;
using UFIDA.U8.U8APIFramework.Meta;
using UFIDA.U8.U8APIFramework.Parameter;
using MSXML2;
using BLL;
using U8API;


public partial class U8API_U8API : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {


        //string aa = new FHD_U8API().AddFHD(TextBox1.Text);
        //Label1.Text = aa;
        #region U8API调用


        //第一步：构造u8login对象并登陆(引用U8API类库中的Interop.U8Login.dll)
        //如果当前环境中有login对象则可以省去第一步
        U8Login.clsLogin u8Login = new U8Login.clsLogin();
        String sSubId = "AS";
        String sAccID = "(default)@006";
        String sYear = "2017";
        String sUserID = "1226";
        String sPassword = "123";
        String sDate = "2017-04-12";
        String sServer = "192.168.0.250";
        String sSerial = "";
        if (!u8Login.Login(ref sSubId, ref sAccID, ref sYear, ref sUserID, ref sPassword, ref sDate, ref sServer, ref sSerial))
        {
            Console.WriteLine("登陆失败，原因：" + u8Login.ShareString);
            Marshal.FinalReleaseComObject(u8Login);
            return;
        }

        //第二步：构造环境上下文对象，传入login，并按需设置其它上下文参数
        U8EnvContext envContext = new U8EnvContext();
        envContext.U8Login = u8Login;

        //销售所有接口均支持内部独立事务和外部事务，默认内部事务
        //如果是外部事务，则需要传递ADO.Connection对象，并将IsIndependenceTransaction属性设置为false
        //envContext.BizDbConnection = new ADO.Connection();
        //envContext.IsIndependenceTransaction = false;

        //设置上下文参数
        envContext.SetApiContext("VoucherType", 9); //上下文数据类型：int，含义：单据类型：9

        //第三步：设置API地址标识(Url)
        //当前API：新增或修改的地址标识为：U8API/Consignment/Save
        U8ApiAddress myApiAddress = new U8ApiAddress("U8API/Consignment/Save");

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
        string sql = "select cast(null as nvarchar(2)) as editprop,a.*,b.* from Sales_FHD_T a  left join DispatchList_extradefine b on a.DLID=b.DLID where 1=0";
        rs.Open(sql, conn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockOptimistic, -1);
        rs.Save(domhead, ADODB.PersistFormatEnum.adPersistXML);
        U8APIHelper.FormatDom(ref domhead, "A");
        broker.AssignNormalValue("DomHead", domhead);

        string strBillNo = TextBox1.Text.ToString();
        DataTable dtHead = new BLL.OrderManager().DL_NewOrderToDispHU8BySel(strBillNo);
        DataTable dtBody = new BLL.OrderManager().DL_NewOrderToDispBU8BySel(strBillNo);
        string dlid = "0";
        if (dtHead.Rows.Count > 0)
        {
            ENUtil.SetToDomH(domhead, "dlid", dlid);
            ENUtil.SetToDomH(domhead, "editprop", "A");
            ENUtil.SetToDomH(domhead, "csysbarcode", dtHead.Rows[0]["csysbarcode"].ToString());
            ENUtil.SetToDomH(domhead, "cdlcode", dtHead.Rows[0]["cDLCode"].ToString());
            ENUtil.SetToDomH(domhead, "cstcode", dtHead.Rows[0]["cstcode"].ToString());
            ENUtil.SetToDomH(domhead, "ddate", dtHead.Rows[0]["ddate"].ToString());
            ENUtil.SetToDomH(domhead, "csocode", dtHead.Rows[0]["csocode"].ToString());
            ENUtil.SetToDomH(domhead, "ccuscode", dtHead.Rows[0]["ccuscode"].ToString());
            ENUtil.SetToDomH(domhead, "csccode", dtHead.Rows[0]["csccode"].ToString());
            ENUtil.SetToDomH(domhead, "cmemo", dtHead.Rows[0]["cmemo"].ToString());
            ENUtil.SetToDomH(domhead, "cdefine2", dtHead.Rows[0]["cdefine2"].ToString());
            ENUtil.SetToDomH(domhead, "breturnflag", dtHead.Rows[0]["breturnflag"].ToString());
            ENUtil.SetToDomH(domhead, "dlid", "0");
            ENUtil.SetToDomH(domhead, "cmaker", dtHead.Rows[0]["cmaker"].ToString());
            ENUtil.SetToDomH(domhead, "cdefine3", dtHead.Rows[0]["cdefine3"].ToString());
            ENUtil.SetToDomH(domhead, "cdefine4", dtHead.Rows[0]["cdefine4"].ToString());
            ENUtil.SetToDomH(domhead, "cdefine9", dtHead.Rows[0]["cdefine9"].ToString());
            ENUtil.SetToDomH(domhead, "cdefine10", dtHead.Rows[0]["cdefine10"].ToString());
            ENUtil.SetToDomH(domhead, "cdefine11", dtHead.Rows[0]["cdefine11"].ToString());
            ENUtil.SetToDomH(domhead, "cdefine12", dtHead.Rows[0]["cdefine12"].ToString());
            ENUtil.SetToDomH(domhead, "cdefine13", dtHead.Rows[0]["cdefine13"].ToString());
            ENUtil.SetToDomH(domhead, "cdefine14", dtHead.Rows[0]["cdefine14"].ToString());
            ENUtil.SetToDomH(domhead, "ccusname", dtHead.Rows[0]["ccusname"].ToString());
            ENUtil.SetToDomH(domhead, "cinvoicecompany", dtHead.Rows[0]["cinvoicecompany"].ToString());
            ENUtil.SetToDomH(domhead, "bsaleoutcreatebill", dtHead.Rows[0]["bsaleoutcreatebill"].ToString());
            ENUtil.SetToDomH(domhead, "cbustype", dtHead.Rows[0]["cbustype"].ToString());
            ENUtil.SetToDomH(domhead, "ivtid", dtHead.Rows[0]["ivtid"].ToString());
            ENUtil.SetToDomH(domhead, "cvouchtype", dtHead.Rows[0]["cvouchtype"].ToString());
            ENUtil.SetToDomH(domhead, "cdepcode", dtHead.Rows[0]["cdepcode"].ToString());
            ENUtil.SetToDomH(domhead, "cpersoncode", dtHead.Rows[0]["cpersoncode"].ToString());
            ENUtil.SetToDomH(domhead, "cexch_name", dtHead.Rows[0]["cexch_name"].ToString());
            ENUtil.SetToDomH(domhead, "iexchrate", dtHead.Rows[0]["iexchrate"].ToString());
            ENUtil.SetToDomH(domhead, "itaxrate", dtHead.Rows[0]["itaxrate"].ToString());
            ENUtil.SetToDomH(domhead, "bfirst", dtHead.Rows[0]["bfirst"].ToString());
            ENUtil.SetToDomH(domhead, "sbvid", dtHead.Rows[0]["sbvid"].ToString());
            ENUtil.SetToDomH(domhead, "isale", dtHead.Rows[0]["isale"].ToString());
            ENUtil.SetToDomH(domhead, "iflowid", dtHead.Rows[0]["iflowid"].ToString());
            ENUtil.SetToDomH(domhead, "bcredit", dtHead.Rows[0]["bcredit"].ToString());
            ENUtil.SetToDomH(domhead, "bmustbook", dtHead.Rows[0]["bmustbook"].ToString());
            ENUtil.SetToDomH(domhead, "iverifystate", dtHead.Rows[0]["iverifystate"].ToString());
            ENUtil.SetToDomH(domhead, "iswfcontrolled", dtHead.Rows[0]["iswfcontrolled"].ToString());
            ENUtil.SetToDomH(domhead, "bcashsale", dtHead.Rows[0]["bcashsale"].ToString());
            ENUtil.SetToDomH(domhead, "bsigncreate", dtHead.Rows[0]["bsigncreate"].ToString());
            ENUtil.SetToDomH(domhead, "bneedbill", dtHead.Rows[0]["bneedbill"].ToString());
            ENUtil.SetToDomH(domhead, "baccswitchflag", dtHead.Rows[0]["baccswitchflag"].ToString());
            ENUtil.SetToDomH(domhead, "bnottogoldtax", dtHead.Rows[0]["bnottogoldtax"].ToString());
            ENUtil.SetToDomH(domhead, "icreditdays", "0");

        }

        //方法一是直接传入MSXML2.DOMDocumentClass对象
        MSXML2.DOMDocument domBody = new MSXML2.DOMDocumentClass();
        ADODB.Recordset rs1 = new ADODB.RecordsetClass();
        sql = "select cast(null as nvarchar(2)) as editprop,a.*,b.* from Sales_FHD_W a left join DispatchLists_extradefine b on a.DLID=b.iDLsID where 1=0";
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
                ENUtil.SetToDomB(domBody, rownum, "dlid", dlid);
                ENUtil.SetToDomB(domBody, rownum, "editprop", "A");
                ENUtil.SetToDomB(domBody, rownum, "cwhcode", dtBody.Rows[i]["cwhcode"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "cinvcode", dtBody.Rows[i]["cinvcode"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "iquantity", dtBody.Rows[i]["iquantity"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "inum", dtBody.Rows[i]["inum"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "iquotedprice", dtBody.Rows[i]["iquotedprice"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "iunitprice", dtBody.Rows[i]["iunitprice"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "itaxunitprice", dtBody.Rows[i]["itaxunitprice"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "imoney", dtBody.Rows[i]["imoney"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "itax", dtBody.Rows[i]["itax"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "isum", dtBody.Rows[i]["isum"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "idiscount", dtBody.Rows[i]["idiscount"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "inatunitprice", dtBody.Rows[i]["inatunitprice"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "inatmoney", dtBody.Rows[i]["inatmoney"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "inattax", dtBody.Rows[i]["inattax"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "inatsum", dtBody.Rows[i]["inatsum"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "inatdiscount", dtBody.Rows[i]["inatdiscount"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "isosid", dtBody.Rows[i]["isosid"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "idlsid", dtBody.Rows[i]["idlsid"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "kl", dtBody.Rows[i]["kl"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "kl2", dtBody.Rows[i]["kl2"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "cinvname", dtBody.Rows[i]["cinvname"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "cdefine22", dtBody.Rows[i]["cdefine22"].ToString());
                //ENUtil.SetToDomB(domBody,rownum, "cdefine27 - bb.iquantity", dtBody.Rows[i]["cdefine27 - bb.iquantity"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "cdefine27", dtBody.Rows[i]["cdefine27"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "iinvexchrate", dtBody.Rows[i]["iinvexchrate"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "cunitid", dtBody.Rows[i]["cunitid"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "cgroupcode", dtBody.Rows[i]["cgroupcode"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "igrouptype", dtBody.Rows[i]["igrouptype"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "csocode", dtBody.Rows[i]["csocode"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "cordercode", dtBody.Rows[i]["cordercode"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "iorderrowno", dtBody.Rows[i]["iorderrowno"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "irowno", dtBody.Rows[i]["irowno"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "idemandtype", dtBody.Rows[i]["idemandtype"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "cdemandcode", dtBody.Rows[i]["cdemandcode"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "cdemandid", dtBody.Rows[i]["cdemandid"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "idemandseq", dtBody.Rows[i]["idemandseq"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "cbsysbarcode", dtBody.Rows[i]["cbsysbarcode"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "itb", dtBody.Rows[i]["itb"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "tbquantity", dtBody.Rows[i]["tbquantity"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "itaxrate", dtBody.Rows[i]["itaxrate"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "fsaleprice", dtBody.Rows[i]["fsaleprice"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "bgsp", dtBody.Rows[i]["bgsp"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "cmassunit", dtBody.Rows[i]["cmassunit"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "bqaneedcheck", dtBody.Rows[i]["bqaneedcheck"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "bqaurgency", dtBody.Rows[i]["bqaurgency"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "bcosting", dtBody.Rows[i]["bcosting"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "fcusminprice", dtBody.Rows[i]["fcusminprice"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "iexpiratdatecalcu", dtBody.Rows[i]["iexpiratdatecalcu"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "bneedsign", dtBody.Rows[i]["bneedsign"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "frlossqty", dtBody.Rows[i]["frlossqty"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "bsaleprice", dtBody.Rows[i]["bsaleprice"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "bgift", dtBody.Rows[i]["bgift"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "bmpforderclosed", dtBody.Rows[i]["bmpforderclosed"].ToString());
                ENUtil.SetToDomB(domBody, rownum, "biacreatebill", dtBody.Rows[i]["biacreatebill"].ToString());

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
            return;
        }

        //第七步：获取返回结果

        //获取返回值
        //获取普通返回值。此返回值数据类型为System.String，此参数按值传递，表示成功返回空串
        System.String result = broker.GetReturnValue() as System.String;
        Label1.Text = result;
        //获取out/inout参数值

        //获取普通INOUT参数vNewID。此返回值数据类型为string，在使用该参数之前，请判断是否为空
        string vNewIDRet = broker.GetResult("vNewID") as string;
        Label1.Text = vNewIDRet;
        //结束本次调用，释放API资源
        broker.Release();
        return;

        #endregion
    }
}