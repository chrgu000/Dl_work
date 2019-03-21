using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using System.Data;
using System.Text;

namespace BLL
{
   public class Check
    {
       CheckDAO ckDao = new CheckDAO();
    
       /// <summary>
       /// 检测网上订单与U8销售订单产品数量及总金额是否相同
       /// </summary>
       /// <param name="cSOCode">U8订单号</param>
       /// <returns></returns>
       public string Check_OpOrder_Num(string cSOCode)
       {
           DataSet ds = ckDao.Check_OpOrder_Num(cSOCode);
           DataTable orderTable = ds.Tables[0];
           DataTable U8Table = ds.Tables[1];
           int orderTableCount = orderTable.Rows.Count;
           int U8TableCount = U8Table.Rows.Count;

           StringBuilder msg = new System.Text.StringBuilder();
           if (orderTableCount == 0)
           {
               msg.Append("网上订单子表行数为零");
               return msg.ToString();
           }
           if (U8TableCount == 0)
           {
               msg.Append("U8订单子表行数为零");
               return msg.ToString();
           }
           if (orderTableCount != U8TableCount)
           {
               msg.Append("网上订单子表行数与U8订单子表行数不符");
               return msg.ToString();
           }
           for (int i = 0; i < orderTableCount; i++)
           {
               if (double.Parse(orderTable.Rows[i]["num"].ToString())-double.Parse(U8Table.Rows[i]["num"].ToString())!=0)
               {
                   msg.Append(orderTable.Rows[i]["cinvcode"]);
                   msg.Append("产品数量不符|");
               }
               if (double.Parse(orderTable.Rows[i]["money"].ToString()) - double.Parse(U8Table.Rows[i]["money"].ToString()) != 0)
               {
                   msg.Append(orderTable.Rows[i]["cinvcode"]);
                   msg.Append("产品金额不符|");
               }
           }
           return msg.ToString();
       }
    }
}
