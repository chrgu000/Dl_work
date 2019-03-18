using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace DAL
{
   public class AffairSQLHelper
    {
        private static SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString);
        private SqlCommand cmd = null;
        private SqlDataReader sdr = null;

        #region 检查数据库的连接状态[GetConn]
        /// <summary>
        /// 检查数据库的连接状态
        /// </summary>
        /// <returns></returns>
        private static SqlConnection GetConn()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString);
                conn.Open();
            }
            return conn;
        }
        #endregion

        #region 产生一个事务并开始
        /// <summary>
        /// 产生一个事务并开始
        /// </summary>
        /// <returns>返回此事务</returns>
        public static SqlTransaction BeginTransaction()
        {
            SqlTransaction tran = GetConn().BeginTransaction();
            return tran;
        }
        #endregion

        #region 关闭数据库连接
        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public static void CloseConn()
        {
            conn.Close();
        }
        #endregion

        #region 多条事务
        public bool SqlTrans(List<SqlCommand> cmds)
        {
            bool b = false;
            SqlConnection conn = GetConn();
            SqlTransaction Tran = conn.BeginTransaction();
            try
            {
                foreach (SqlCommand cmd in cmds)
                {
                    cmd.Connection = conn;
                    cmd.Transaction = Tran;
                    cmd.ExecuteNonQuery();

                }
                Tran.Commit();
                b = true;
            }
            catch (Exception)
            {

                Tran.Rollback();
            }
            finally
            {
                conn.Close();
                Tran.Dispose();
                conn.Dispose();

            }
            return b;
        }
        #endregion




    }
}
