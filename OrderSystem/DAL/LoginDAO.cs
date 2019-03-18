/*
 *create by echo
 * createTime:2015-10-09
 * explain:
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Model;

namespace DAL
{
    public class LoginDAO
    {
        private SQLHelper sqlhelper = null;

        public LoginDAO()
        {
            sqlhelper = new SQLHelper();
        }

        #region 检查用户登录是否成功(Login)
        /// <summary>
        /// 检查用户登录是否成功(Login)
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns>登录是否成功</returns>
        public DataTable Login(string username, string pwd)
        {
            DataTable dt = null;

            string cmdText = "select aa.*,bb.cCusPPerson,isnull(bb.cCusPhone,' ') 'cCusPhone',cc.*,0 lngopUserExId,aa.strUserName strAllAcount  from Dl_opUser aa left join Customer bb on aa.cCusCode=bb.cCusCode left join Dl_opSystemConfiguration cc on 1=1  where aa.strLoginName=@username and aa.strUserPwd=@pwd";
            SqlParameter[] paras = new SqlParameter[] { 
            new SqlParameter("@username",username),
            new SqlParameter("@pwd",pwd)
             };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 检查用户登录是否成功(SubLogin)
        /// <summary>
        /// 检查用户登录是否成功(SubLogin)
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="phone">手机号码</param>
        /// <param name="pwd">密码</param>
        /// <returns>登录是否成功</returns>
        public DataTable SubLogin(string username, string phone, string pwd)
        {
            DataTable dt = null;
            string cmdText = "DLproc_SubCustomerPhoneNoLogin";
            SqlParameter[] paras = new SqlParameter[] { 
            new SqlParameter("@cCusCode",username),
            new SqlParameter("@phone",phone),
            new SqlParameter("@pwd",pwd)
             };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 用户短信发送[LoginSms]
        /// <summary>
        /// 用户短信发送[LoginSms]
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>电话号码</returns>
        public DataTable LoginSms(string username)
        {
            DataTable dt = null;

            string cmdText = "select isnull(bb.cCusPhone,' ') 'cCusPhone' from Dl_opUser aa left join Customer bb on aa.cCusCode=bb.cCusCode where aa.strLoginName=@username";
            SqlParameter[] paras = new SqlParameter[] { 
            new SqlParameter("@username",username)
             };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 获取用户电话号码(主账户+子账户de第一条号码)[GetSubPhoneNo]
        /// <summary>
        /// 获取用户电话号码(主账户+子账户de第一条号码)[GetSubPhoneNo]
        /// </summary>
        /// <param name="cCusCode">用户名</param>
        /// <param name="phone">手机号码</param>
        /// <returns></returns>
        public DataTable GetSubPhoneNo(string username, string phone)
        {
            DataTable dt = null;
            string cmdText = "DLproc_GetSubCustomerPhoneNo";
            SqlParameter[] paras = new SqlParameter[] { 
            new SqlParameter("@cCusCode",username),
            new SqlParameter("@phone",phone)
             };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 获取订单用户电话号码(主账户+操作子账户)[GetOrderSubPhoneNo]
        /// <summary>
        /// 获取订单用户电话号码(主账户+操作子账户)[GetOrderSubPhoneNo]
        /// </summary>
        /// <param name="lngopUserId">用户主id</param>
        /// <param name="lngopUserExId">子账户id</param>
        /// <returns></returns>
        public DataTable GetOrderSubPhoneNo(string lngopUserId, string lngopUserExId)
        {
            DataTable dt = null;
            string cmdText = "select isnull(bb.cCusPhone,' ') cCusPhone from Dl_opUser aa left join Customer bb on aa.cCusCode=bb.cCusCode where aa.lngopUserId=@lngopUserId union all select isnull(strSubPhone,' ') from Dl_opUser_Ex where lngopUserId=@lngopUserId and lngopUserExId=@lngopUserExId";
            SqlParameter[] paras = new SqlParameter[] { 
            new SqlParameter("@lngopUserId",lngopUserId),
            new SqlParameter("@lngopUserExId",lngopUserExId)
             };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 用户短信发送(根据订单编号查找手机号码)[LoginSmsByBillNo]
        /// <summary>
        /// 用户短信发送(根据订单编号查找手机号码)[LoginSmsByBillNo]
        /// </summary>
        /// <param name="strBillNo">订单号</param>
        /// <returns>电话号码</returns>
        public DataTable LoginSmsByBillNo(string strBillNo)
        {
            DataTable dt = null;
            string cmdText = "select isnull(cCusPhone,' ') 'cCusPhone'  from Customer where cCusCode=(select ccuscode from Dl_opOrder where strBillNo=@strBillNo)";
            SqlParameter[] paras = new SqlParameter[] { 
            new SqlParameter("@strBillNo",strBillNo)
             };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 获取用户电话号码(操作员设置)[GetPhoneNo]
        /// <summary>
        ///  获取用户电话号码(操作员设置)[GetPhoneNo]
        /// </summary>
        /// <param name="cCusCode">用户名</param>
        /// <returns>电话号码</returns>
        public DataTable GetPhoneNo(string cCusCode)
        {
            //DataTable dt = null;
            string cmdText = "select isnull(bb.cCusPhone,' ') 'cCusPhone' from Dl_opUser aa left join Customer bb on aa.cCusCode=bb.cCusCode where aa.strLoginName=@cCusCode";
            SqlParameter[] paras = new SqlParameter[] { 
            new SqlParameter("@cCusCode",cCusCode)
             };
            DataTable dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion


    }

}
