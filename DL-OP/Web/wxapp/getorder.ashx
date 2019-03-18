<%@ WebHandler Language="C#" Class="getorder" %>

using System;
using System.Web;
using System.Web.Script.Serialization;
using System.Data;
using BLL;
using System.Net;

public class getorder : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string id = "0";
        string strBillNo = "";
        string ccuscode = "0";
        string datetype = "0";
        string begin = "0";
        string end = "0";
        string strshowtype = "0";
        string strFHStatus = "0";
        //判断提交方式
        if (context.Request.RequestType.ToLower() == "get")
        {
            //id = context.Request.QueryString["id"];
            //strBillNo = context.Request.QueryString["strBillNo"];
            ccuscode = context.Request.QueryString["ccuscode"];
            datetype = context.Request.QueryString["datetype"];
            strFHStatus = context.Request.QueryString["strFHStatus"];
            begin = context.Request.QueryString["begin"];
            end = context.Request.QueryString["end"];
        }
        else
        {
            //id = context.Request.Form["id"];
            //strBillNo = context.Request.Form["strBillNo"];
            ccuscode = context.Request.Form["ccuscode"];
            datetype = context.Request.Form["datetype"];
            strFHStatus = context.Request.Form["strFHStatus"];
            begin = context.Request.Form["begin"];
            end = context.Request.Form["end"];
        }
        //获取日期
        DateTime dt = DateTime.Now;  //当前时间  
        switch (datetype)
        {
            case "1":
                begin = System.DateTime.Now.ToString("d") + " 0:00:00";
                end = System.DateTime.Now.AddDays(+1).ToString("d") + " 0:00:00";
                break;
            case "2":
                begin = System.DateTime.Now.AddDays(-1).ToString("d") + " 0:00:00";
                end = System.DateTime.Now.ToString("d") + " 0:00:00";
                break;
            case "3":
                begin = dt.AddDays(1 - Convert.ToInt32(dt.DayOfWeek.ToString("d"))).ToString() + " 0:00:00";  //本周周一 
                DateTime startWeek = dt.AddDays(1 - Convert.ToInt32(dt.DayOfWeek.ToString("d")));  //本周周一  
                end = startWeek.AddDays(6).ToString() + " 0:00:00";  //本周周日  
                break;
            case "4":
                begin = dt.AddDays(1 - dt.Day).ToString() + " 0:00:00";  //本月月初
                DateTime startMonth = dt.AddDays(1 - dt.Day);  //本月月初  
                end = startMonth.AddMonths(1).AddDays(-1).ToString() + " 0:00:00";  //本月月末  
                break;
            case "5":
                begin = begin + " 0:00:00";
                end = end + " 0:00:00";
                break;
        }
        if (strFHStatus.ToString() == "全部订单")
        {
            strFHStatus = "1";
        }

        //查询数据

        DataTable dtOrderExecute = new SearchManager().DLproc_OrderExecuteBySel(strBillNo, ccuscode, begin, end, strshowtype, strFHStatus,"");
        double ddje = 0;
        double fhje = 0;
        double thje = 0;
        int icount = 0;
        string datagridjson = "";
        //string datagridjson = " [";
        if (dtOrderExecute.Rows.Count > 0)
        {
            icount = dtOrderExecute.Rows.Count;
            for (int i = 0; i < dtOrderExecute.Rows.Count; i++)
            {
                ddje = Convert.ToDouble(dtOrderExecute.Rows[i]["isum"].ToString()) + ddje;                //订单金额合计
                fhje = Convert.ToDouble(dtOrderExecute.Rows[i]["U8iFHMoney"].ToString()) + fhje;                //发货金额合计
                thje = Convert.ToDouble(dtOrderExecute.Rows[i]["U8iTHMoney"].ToString()) + thje;                //退货金额合计
                string datagridjsonstrBillNo = "\"strBillNo" + "\":\"" + dtOrderExecute.Rows[i]["strBillNo"].ToString() + "\",";
                string datagridjsondatBillTime = "\"datBillTime" + "\":\"" + dtOrderExecute.Rows[i]["datBillTime"].ToString() + "\",";
                string datagridjsoncdefine11 = "\"cdefine11" + "\":\"" + dtOrderExecute.Rows[i]["cdefine11"].ToString() + "\",";
                string datagridjsonisum = "\"isum" + "\":\"" + dtOrderExecute.Rows[i]["isum"].ToString() + "\",";
                string datagridjsonU8iFHMoney = "\"U8iFHMoney" + "\":\"" + dtOrderExecute.Rows[i]["U8iFHMoney"].ToString() + "\",";
                string datagridjsonU8iTHMoney = "\"U8iTHMoney" + "\":\"" + dtOrderExecute.Rows[i]["U8iTHMoney"].ToString() + "\"";
                datagridjson = datagridjson + "{" + datagridjsonstrBillNo + datagridjsondatBillTime + datagridjsoncdefine11 + datagridjsonisum + datagridjsonU8iFHMoney + datagridjsonU8iTHMoney + "},";
            }
        }
        datagridjson = "[{" + "\"strBillNo" + "\":\"" + "合计" + "\"," + "\"datBillTime" + "\":\"" + "合计订单:" + icount+"条记录" + "\"," + "\"cdefine11" + "\":\"" + "合计" + "\"," + "\"isum" + "\":\"" + ddje.ToString() + "\"," + "\"U8iFHMoney" + "\":\"" + fhje.ToString() + "\"," + "\"U8iTHMoney" + "\":\"" + thje.ToString() + "\"" + "}," + datagridjson;
        datagridjson = datagridjson.TrimEnd(',');
        datagridjson = datagridjson + "]";
        //输出 json 字符串
        context.Response.Write(datagridjson.ToString());
        context.Response.End();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}