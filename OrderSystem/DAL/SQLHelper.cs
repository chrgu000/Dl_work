/*
 *创建人：ECHO 
 *创建时间：2015-10-09
 *说明：数据库助手类
 * 版权所有：
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace DAL
{
    public class SQLHelper
    {
        private SqlConnection conn = null;
        private SqlCommand cmd = null;
        private SqlDataReader sdr = null;
    

        #region 定义数据库的连接[connStr]
        /// <summary>
        /// 定义数据库的连接
        /// </summary>
        public SQLHelper()
        {
            string connStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            conn = new SqlConnection(connStr);
        }
        #endregion


        #region 检查数据库的连接状态[GetConn]
        /// <summary>
        /// 检查数据库的连接状态
        /// </summary>
        /// <returns></returns>
        private SqlConnection GetConn()
        {
            conn = new SqlConnection( ConfigurationManager.ConnectionStrings["connStr"].ConnectionString);
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            return conn;
        }
        #endregion


        #region 执行不带参数的增删改SQL语句或者存储过程[ExecuteNonQuery]
        /// <summary>
        /// 执行不带参数的增删改SQL语句或者存储过程
        /// </summary>
        /// <param name="cmdText">增删改SQL语句或者存储过程</param>
        /// <param name="ct">命令类型</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string cmdText, CommandType ct)
        {
            int res;
            try
            {
                cmd = new SqlCommand(cmdText, GetConn());
                cmd.CommandTimeout = 600;
                cmd.CommandType = ct;
                res = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return res;
        }
        #endregion


        #region 执行带参数的增删改SQL语句或者存储过程[ExecuteNonQuery]
        /// <summary>
        /// 执行带参数的增删改SQL语句或者存储过程
        /// </summary>
        /// <param name="cmdText">增删改SQL语句或者存储过程</param>
        /// <param name="paras">参数集合</param>
        /// <param name="ct">命令类型</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string cmdText, SqlParameter[] paras, CommandType ct)
        {
            int res;
            try
            {
                cmd = new SqlCommand(cmdText, GetConn());
                cmd.CommandType = ct;
                cmd.CommandTimeout = 600;
                cmd.Parameters.AddRange(paras);
                res = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            //using (cmd = new SqlCommand(cmdText, GetConn()))
            //{
            //    cmd.CommandType = ct;
            //    cmd.CommandTimeout = 600;
            //    cmd.Parameters.AddRange(paras);
            //    res = cmd.ExecuteNonQuery();
            //}
            return res;
        }
        #endregion


        #region 执行查询SQL语句或者存储过程[ExecuteQuery]
        /// <summary>
        /// 执行查询SQL语句或者存储过程
        /// </summary>
        /// <param name="cmdText">查询SQL语句或者存储过程</param>
        /// <param name="ct">命令类型</param>
        /// <returns></returns>
        public DataTable ExecuteQuery(string cmdText, CommandType ct)
        {
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand(cmdText, GetConn());
                cmd.CommandTimeout = 600;
                cmd.CommandType = ct;
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                //using (sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                //{
                //    dt.Load(sdr);
                //}
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return dt;
        }
        #endregion


        #region 执行带参数的查询SQL语句或者存储过程[ExecuteQuery]
        /// <summary>
        /// 执行带参数的查询SQL语句或者存储过程
        /// </summary>
        /// <param name="cmdText">查询SQL语句或者存储过程</param>
        /// <param name="paras">参数集合</param>
        /// <param name="ct">命令类型</param>
        /// <returns></returns>
        public DataTable ExecuteQuery(string cmdText, SqlParameter[] paras, CommandType ct)
        {
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand(cmdText, GetConn());
                cmd.CommandTimeout = 600;
                cmd.CommandType = ct;
                cmd.Parameters.AddRange(paras);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                //using (sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                //{
                //    dt.Load(sdr);
                //}
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return dt;

        }
        #endregion


        public string ExecuteQueryProc(string cmdText, SqlParameter[] paras, CommandType ct)
        {
            DataTable dt = new DataTable();
            cmd = new SqlCommand(cmdText, GetConn());
            cmd.CommandTimeout = 600;
            cmd.CommandType = ct;
            cmd.Parameters.AddRange(paras);
            SqlParameter input = cmd.Parameters.Add("@INPUT", SqlDbType.Float);
            using (sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                dt.Load(sdr);
            }
            return input.ToString();
        }


        #region 事务
        public JObject SqlTran(SqlCommand cmd)
        {
          
            conn = GetConn();
            JObject jo = new JObject();
            SqlTransaction Tran = conn.BeginTransaction();
            try
            {

                cmd.Connection = conn;
                cmd.Transaction = Tran;
                cmd.ExecuteNonQuery();
                Tran.Commit();
                jo["flag"] = 1;
            }
            catch (Exception err)
            {

                Tran.Rollback();
                jo["flag"] = 0;
                jo["message"] = err.ToString();
            }
            finally
            {
                conn.Close();
                Tran.Dispose();
                conn.Dispose();

            }
            return jo;
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


        #region 插入表头及SqlBulkcopy插入表体
        public string SqlTransBulk(SqlCommand[] cmds, DataTable dt)
        {
            string flag = "";
            SqlConnection conn = GetConn();
            SqlTransaction Tran = conn.BeginTransaction();
            SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, Tran);
            try
            {
                string id = "";
                for (int i = 0; i < cmds.Length; i++)
                {
                    cmds[i].Connection = conn;
                    cmds[i].Transaction = Tran;
                    if (i == 0)
                    {
                        id = cmds[i].ExecuteScalar().ToString();

                    }
                    else
                    {
                        cmds[i].ExecuteNonQuery();
                    }

                }
                foreach (DataRow dr in dt.Rows)
                {
                    dr["Id"] = id;
                }

                sqlbulkcopy.DestinationTableName = dt.TableName;
                sqlbulkcopy.BatchSize = dt.Rows.Count;
                if (dt != null && dt.Rows.Count != 0)
                {
                    sqlbulkcopy.WriteToServer(dt);
                }

                Tran.Commit();

            }
            catch (Exception err)
            {
                flag = err.ToString();
                Tran.Rollback();
            }
            finally
            {
                conn.Close();
                sqlbulkcopy.Close();
                Tran.Dispose();
                conn.Dispose();

            }
            return flag;
        }
        #endregion

        #region 插入表头及SqlBulkcopy插入表体
        public JObject SqlTransBulk(SqlCommand[] cmds, DataTable dt, string MAAOrderId)
        {

            string id = string.Empty;
            string code = string.Empty;
            JObject jo = new JObject();
            jo["flag"] = "1";
            SqlConnection conn = GetConn();
            SqlTransaction Tran = conn.BeginTransaction();
            SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, Tran);
            try
            {
                for (int i = 0; i < cmds.Length; i++)
                {
                    cmds[i].Connection = conn;
                    cmds[i].Transaction = Tran;
                    if (i == 0)
                    {
                        //  id = cmds[i].ExecuteScalar().ToString();
                        SqlDataReader sdr = cmds[i].ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                id = sdr[0].ToString();
                                code = sdr[1].ToString();
                                jo["code"] = code;
                                jo["orderId"] = id;
                                 
                            }
                            sdr.Close();
                        }


                    }
                    else
                    {
                        cmds[i].ExecuteNonQuery();
                    }

                }
                foreach (DataRow dr in dt.Rows)
                {
                    dr[MAAOrderId] = id;
                }

                sqlbulkcopy.DestinationTableName = dt.TableName;
                sqlbulkcopy.BatchSize = dt.Rows.Count;
                if (dt != null && dt.Rows.Count != 0)
                {
                    sqlbulkcopy.WriteToServer(dt);
                }

                Tran.Commit();


            }
            catch (Exception err)
            {
                jo["flag"] = "0";
                jo["ErrMsg"] = err.ToString();
                Tran.Rollback();
            }
            finally
            {
                conn.Close();
                sqlbulkcopy.Close();
                Tran.Dispose();
                conn.Dispose();

            }

            return jo;
        }
        #endregion


        #region SqlBulkCopy批量插入数据
        /// <summary>
        /// SqlBulkCopy批量插入数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool SqlBulkCopy(DataTable dt)
        {
            bool flag = true;
            SqlConnection conn = GetConn();
            SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(conn);
            try
            {
                sqlbulkcopy.DestinationTableName = dt.TableName;
                sqlbulkcopy.BatchSize = dt.Rows.Count;
                if (dt != null && dt.Rows.Count != 0)
                {
                    sqlbulkcopy.WriteToServer(dt);
                }
            }
            catch (Exception)
            {
                flag = false;
                throw;
            }
            finally
            {
                conn.Close();
                sqlbulkcopy.Close();
                conn.Dispose();

            }
            return flag;
        }
        #endregion

        #region SqlBulkCopy批量插入数据
        /// <summary>
        /// SqlBulkCopy批量插入数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public JObject BulkCopy(DataTable dt)
        {
            JObject jo = new JObject();

            SqlConnection conn = GetConn();
            SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(conn);
            try
            {
                sqlbulkcopy.DestinationTableName = dt.TableName;
                sqlbulkcopy.BatchSize = dt.Rows.Count;
                if (dt != null && dt.Rows.Count != 0)
                {
                    sqlbulkcopy.WriteToServer(dt);
                }
                jo["flag"] = "1";
            }
            catch (Exception err)
            {
                jo["flag"] = "0";
                jo["message"] = err.ToString();

            }
            finally
            {
                conn.Close();
                sqlbulkcopy.Close();
                conn.Dispose();

            }
            return jo;
        }
        #endregion

        #region 执行多条SqlCommand后批量插入DataTable
        public JObject BulkCopy(SqlCommand[] cmds, DataTable dt)
        {

            JObject jo = new JObject();
            SqlConnection conn = GetConn();
            SqlTransaction Tran = conn.BeginTransaction();
            SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, Tran);
            try
            {

                for (int i = 0; i < cmds.Length; i++)
                {
                    cmds[i].Connection = conn;
                    cmds[i].Transaction = Tran;

                    cmds[i].ExecuteNonQuery();


                }


                sqlbulkcopy.DestinationTableName = dt.TableName;
                sqlbulkcopy.BatchSize = dt.Rows.Count;
                if (dt != null && dt.Rows.Count != 0)
                {
                    sqlbulkcopy.WriteToServer(dt);
                }

                Tran.Commit();
                jo["flag"] = "1";
            }
            catch (Exception err)
            {
                jo["flag"] = "0";
                jo["message"] = err.ToString();
                Tran.Rollback();
            }
            finally
            {
                conn.Close();
                sqlbulkcopy.Close();
                Tran.Dispose();
                conn.Dispose();

            }
            return jo;
        }
        #endregion

        #region 批量执行SQL语句，返回DataSet
        /// <summary>
        /// 批量执行SQL语句，返回DataSet
        /// </summary>
        /// <param name="sql">SQL语句，或拼接的存储过程</param>
        /// <param name="ct">类型</param>
        /// <returns></returns>

        public DataSet ExecuteDataSet(string sql, CommandType ct)
        {
            using (SqlConnection con = GetConn())
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    sda.Fill(ds);
                    return ds;
                }
            }
        }
        #endregion
    }
}
