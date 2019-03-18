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

        #region 执行不带参数的增删改SQL语句或者存储过程[ExecuteNonQueryForOutTime]
        /// <summary>
        /// 执行不带参数的增删改SQL语句或者存储过程
        /// </summary>
        /// <param name="cmdText">增删改SQL语句或者存储过程</param>
        /// <param name="ct">命令类型</param>
        /// <returns></returns>
        public int ExecuteNonQueryForOutTime(string cmdText, CommandType ct,int OutTime)
        {
            int res;
            try
            {
                cmd = new SqlCommand(cmdText, GetConn());
                cmd.CommandTimeout = OutTime;
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
            using (cmd = new SqlCommand(cmdText, GetConn()))
            {
                cmd.CommandType = ct;
                cmd.CommandTimeout = 600;
                cmd.Parameters.AddRange(paras);
                res = cmd.ExecuteNonQuery();
            }
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
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
            cmd = new SqlCommand(cmdText, GetConn());
            cmd.CommandTimeout = 600;
            cmd.CommandType = ct;
            
            using (sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                dt.Load(sdr);
            }
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
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
            cmd = new SqlCommand(cmdText, GetConn());
            cmd.CommandTimeout = 600;
            cmd.CommandType = ct;
            cmd.Parameters.AddRange(paras);
            using (sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                dt.Load(sdr);
            }
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
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

    }
}
