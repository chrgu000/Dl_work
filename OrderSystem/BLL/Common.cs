using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Model;
using System.IO;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;


namespace BLL
{
  public  class Common
    {
      DAL.SQLHelper sqlhper = new DAL.SQLHelper();
      public DataTable GetShippingMethod() {
          string sql = "select * from ShippingChoice";
          return sqlhper.ExecuteQuery(sql, CommandType.Text);
      }
    }
}
