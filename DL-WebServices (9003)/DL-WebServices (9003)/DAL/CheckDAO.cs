using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DAL
{
  public  class CheckDAO
    {
      SQLHelper sqlhelper = new SQLHelper();
      public DataSet Check_OpOrder_Num(string cSOCode)
      {
          StringBuilder sql = new System.Text.StringBuilder();
          sql.Append(@"SELECT bb.cinvcode,SUM(bb.iquantity) num,SUM(bb.isum) money FROM dl_oporder aa
INNER JOIN dbo.Dl_opOrderDetail bb
ON aa.lngopOrderId=bb.lngopOrderId
WHERE aa.cSOCode='");
          sql.Append(cSOCode);
          sql.Append(@"'GROUP BY  bb.cinvcode 
ORDER BY bb.cinvcode;
SELECT cinvcode,SUM(iquantity) num,SUM(isum) money FROM dbo.SO_SODetails 
WHERE  cSOCode='");
          sql.Append(cSOCode);
          sql.Append(@"'GROUP BY  cinvcode 
ORDER BY cinvcode");

          return sqlhelper.ExecuteDataSet(sql.ToString(), CommandType.Text);
      }
    }
}
