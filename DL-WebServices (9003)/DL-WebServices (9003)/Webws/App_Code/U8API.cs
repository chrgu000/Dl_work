using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Collections;
using mas.ecloud.sdkclient;
using mas.ecloud.Model;
using System.Web.Services;
using Newtonsoft.Json;
using BLL;
using System.Net;
using System.IO;
using System.Data;
using Model;
using System.Data.SqlClient;


/// <summary>
/// U8API 的摘要说明
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
// [System.Web.Script.Services.ScriptService]
public class U8API : System.Web.Services.WebService
{

    public U8API()
    {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }

    /// <summary>
    /// 审核调用U8发货单的api生成发货单
    /// </summary>
    /// <param name="strBillNo">网上订单的参照订单编号</param>
    /// <returns></returns>
    [WebMethod(Description = "U8FHD_API")]
    public string U8FHD_API(string strBillNo)
    {
        string aa = "";
        for (int i = 1; i < 4; i++) //循环拆分发货单,1.非金花大井,2.金花,3.大井
        {
            aa = new FHD_U8API().AddFHDAPI(strBillNo, i);
            if (!string.IsNullOrEmpty(aa))
            {
                //写入错误信息
                bool d = new OrderManager().DL_ErrByIns(strBillNo, aa);
                break;  //退出循环
            }
            if (i == 3)   //完成所有分单api成功调用之后，更新网上czts订单状态为已审核
            {
                //更新网上订单状态
                bool c = new OrderManager().DL_CZTSOrderAuthByUpd(strBillNo);
            }
        }
        return aa;
    }

    /// <summary>
    /// 审核调用U8发货单的api生成发货单
    /// </summary>
    /// <param name="strBillNo">网上订单的参照订单编号</param>
    /// <returns></returns>
    [WebMethod(Description = "U8XTZHD_API")]
    public string U8XTZHD_API(string strBillNo)
    {
        string aa = "";
        aa = new XTZHD_U8API().AddXTZHDAPI(strBillNo);
        return aa;
    }

    /// <summary>
    /// 审核调用U8销售订单的api生成销售订单
    /// </summary>
    /// <param name="strBillNo">网上订单的订单编号</param>
    /// <returns></returns>
    [WebMethod(Description = "U8XSDD_API")]
    public string U8XSDD_API(string strBillNo)
    {
        string aa = "";
        aa = new XSDD_U8API().AddXSDDAPI(strBillNo);
        return aa;
    }


    /// <summary>
    /// 审核调用U8特殊销售订单的api生成销售订单
    /// </summary>
    /// <param name="strBillNo">网上订单的订单编号</param>
    /// <returns></returns>
    [WebMethod(Description = "U8TSTSXSDD_API")]
    public string U8TSXSDD_API(string strBillNo)
    {
        string aa = "";
        aa = new TSXSDD().AddTSXSDDAPI(strBillNo);
        return aa;
    }

    /// <summary>
    /// 打开后关闭指定的U8销售订单
    /// </summary>
    /// <param name="U8csocode">U8销售订单单号</param>
    /// <returns>返回空为成功</returns>
    [WebMethod(Description = "U8OpenAndCloseOrder_API")]
    public string U8OpenAndCloseOrder_API(string U8csocode)
    {
        string aa = "";
        aa = new XSDD_U8API().OpenAndCloseU8Order(U8csocode, false);
        if (!string.IsNullOrEmpty(aa))
        {
            return aa;
        }
        else
        {
            aa = new XSDD_U8API().OpenAndCloseU8Order(U8csocode, true);
        }
        return aa;
    }

    /// <summary>
    /// 打开后关闭有预留数量并且已经被关闭的U8销售订单
    /// </summary>
    /// <param name="U8csocode">U8销售订单单号</param>
    /// <returns>返回空为成功</returns>
    [WebMethod(Description = "U8OpenAndCloseOrderDataSet_API")]
    public string U8OpenAndCloseOrderDataSet_API()
    {
        string aa = "";
        string U8csocode = "";
        //查询出有预留数量并且已经被关闭的U8销售订单

        //关闭此订单
        aa = new XSDD_U8API().OpenAndCloseU8Order(U8csocode, false);
        if (!string.IsNullOrEmpty(aa))
        {
            return aa;
        }
        else
        {
            aa = new XSDD_U8API().OpenAndCloseU8Order(U8csocode, true);
        }
        return aa;
    }




}
