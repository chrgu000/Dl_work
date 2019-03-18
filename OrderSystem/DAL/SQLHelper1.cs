using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace DAL
{
   public class SQLHelper1
    {
       private static readonly string conStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        public static int ExecuteNonQuery(string sql, CommandType cmdType, params SqlParameter[] pms)
        {
            
            using (SqlConnection con = new SqlConnection(conStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.CommandType = cmdType;
                    if (pms != null)
                    {
                        cmd.Parameters.AddRange(pms);
                    }
                    con.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }
        public static bool ExecuteScalar(string sql, CommandType cmdType, params SqlParameter[] pms)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.CommandType = cmdType;
                    if (pms != null)
                    {
                        cmd.Parameters.AddRange(pms);
                    }
                    con.Open();
                    if (cmd.ExecuteScalar() == null)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }

                }
            }
        }
        public static SqlDataReader ExecuteReader(string sql, CommandType cmdType, params SqlParameter[] pms)
        {
            SqlConnection con = new SqlConnection(conStr);
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.CommandType = cmdType;
                if (pms != null)
                {
                    cmd.Parameters.AddRange(pms);
                }
                con.Open();
                SqlDataReader sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return sdr;
            }
        }
        public static DataTable ExecuteApdater(string sql, CommandType cmdType, params SqlParameter[] pms)
        {
           // SqlConnection con = new SqlConnection(conStr);
            using (SqlConnection con = new SqlConnection(conStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    if (pms != null)
                    {
                        cmd.Parameters.AddRange(pms);
                    }
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public   DataSet ExecuteDataSet(string strSql, SqlParameter[] paras,CommandType ct)
        {
            SqlConnection sqlConn = new SqlConnection(conStr);
            SqlCommand sqlCmd = new SqlCommand(strSql, sqlConn);
            sqlCmd.CommandType =ct;
            sqlCmd.Parameters.AddRange(paras);
            SqlDataAdapter sqlAdp = new SqlDataAdapter(sqlCmd);

            DataSet ds = new DataSet();
            sqlAdp.Fill(ds);
            return ds;
        }

        public DataSet ExecuteDataSet(string sql, CommandType ct)
        {
            using (SqlConnection con = new SqlConnection(conStr))
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
    }
}
