using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
 


namespace BLL
{
   public class WhareHouse
    {
       DAL.SQLHelper sqlhelper = new DAL.SQLHelper();

       public DataTable CurrentStock(string code) {
           string sql = "DLPROC_CurrentStock";
           SqlParameter[] paras = new SqlParameter[]{
                  new SqlParameter("@code",code)
           };
           return sqlhelper.ExecuteQuery(sql, paras, CommandType.StoredProcedure);
       }
    }
}
