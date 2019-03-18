using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DAL;
using Model;

namespace BLL
{
    public class LoginManager
    {
        private LoginDAO ldao = null;

        public LoginManager()
        {
            ldao = new LoginDAO();
        }

        #region 用户登录是否成功[Login]
        /// <summary>
        /// 用户登录是否成功
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public DataTable Login(string username, string pwd)
        {
            return ldao.Login(username, pwd);
        }
        #endregion

        #region 用户登录是否成功（子账户）[SubLogin]
        /// <summary>
        /// 用户登录是否成功（子账户）[SubLogin]
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="phone">手机号码</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public DataTable SubLogin(string username, string phone, string pwd)
        {
            return ldao.SubLogin(username, phone, pwd);
        }
        #endregion

        #region 用户短信发送[LoginSms]
        /// <summary>
        /// 用户短信发送
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        public DataTable LoginSms(string username)
        {
            return ldao.LoginSms(username);
        }
        #endregion

        #region 用户短信发送(根据订单编号查找手机号码)[LoginSmsByBillNo]
        /// <summary>
        /// 用户短信发送(根据订单编号查找手机号码)[LoginSmsByBillNo]
        /// </summary>
        /// <param name="strBillNo">订单编号</param>
        /// <returns></returns>
        public DataTable LoginSmsByBillNo(string strBillNo)
        {
            return ldao.LoginSmsByBillNo(strBillNo);
        }
        #endregion

        #region 获取用户电话号码(操作员设置)[GetPhoneNo]
        /// <summary>
        /// 获取用户电话号码(操作员设置)[GetPhoneNo]
        /// </summary>
        /// <param name="cCusCode">用户名</param>
        /// <returns></returns>
        public DataTable GetPhoneNo(string cCusCode)
        {
            return ldao.LoginSms(cCusCode);
        }
        #endregion

        #region 获取用户电话号码(主账户+子账户)[GetSubPhoneNo]
        /// <summary>
        /// 获取用户电话号码(主账户+子账户)[GetSubPhoneNo]
        /// </summary>
        /// <param name="cCusCode">用户名</param>
        /// <param name="phone">手机号码</param>
        /// <returns></returns>
        public DataTable GetSubPhoneNo(string cCusCode, string phone)
        {
            return ldao.GetSubPhoneNo(cCusCode, phone);
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
            return ldao.GetOrderSubPhoneNo(lngopUserId, lngopUserExId);
        }
        #endregion


    }
}
