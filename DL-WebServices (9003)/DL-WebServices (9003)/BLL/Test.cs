using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using System.Data;

namespace BLL
{
    public class Test
    {
        public void DLproc_shiwutest1()
        {
            DAL.SQLHelper sqlhelper = new DAL.SQLHelper();
            int res = sqlhelper.ExecuteNonQuery("DLproc_shiwutest1", CommandType.StoredProcedure);

        }

        public void DLproc_shiwutest2()
        {
            DAL.SQLHelper sqlhelper = new DAL.SQLHelper();
            int res = sqlhelper.ExecuteNonQuery("DLproc_shiwutest2", CommandType.StoredProcedure);

        }
    }

}
