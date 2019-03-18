/*
 *创建人：ECHO 
 *创建时间：2015-10-13
 *说明：基础资料操作类
 * 版权所有：
 */
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
    public class BasicInfoManager
    {
        private BasicInfoDAO bdao = null;

        public BasicInfoManager()
        {
            bdao = new BasicInfoDAO();
        }

        #region 用户修改密码[Update_UserPWD]
        /// <summary>
        /// 用户修改密码
        /// </summary>
        /// <param name="ba">用户id,密码</param>
        /// <returns></returns>
        public bool Update_UserPWD(BasicInfo ba)
        {
            return bdao.Update_UserPWD(ba);
        }
        #endregion

        #region 更新二维码扫码登录的表[Update_QrCodeLogin]
        /// <summary>
        /// 新二维码扫码登录的表[Update_QrCodeLogin]
        /// </summary>
        /// <param name="sessionID">sessionID</param>
        /// <returns></returns>
        public bool Update_QrCodeLogin(string sessionID, string bFlag, string userid)
        {
            return bdao.Update_QrCodeLogin(sessionID, bFlag, userid);
        }
        #endregion

        #region 获取二维码登录信息[DL_QrCodeLoginBySel]
        /// <summary>
        /// 获取二维码登录信息[DL_QrCodeLoginBySel]
        /// </summary>
        /// <param name="sessionID">sessionID</param>
        /// <returns></returns>
        public DataTable DL_QrCodeLoginBySel(string sessionID)
        {
            return bdao.DL_QrCodeLoginBySel(sessionID);
        }
        #endregion


        #region 用户修改密码(子账户专用修改密码)[Update_SubUserPWD]
        /// <summary>
        /// 用户修改密码
        /// </summary>
        /// <param name="lngopUserExId">子用户id </param>
        /// <param name="strUserPwd">密码</param>
        /// <returns></returns>
        public bool Update_SubUserPWD(string lngopUserExId, string strUserPwd)
        {
            return bdao.Update_SubUserPWD(lngopUserExId, strUserPwd);
        }
        #endregion

        #region 用户地址操作函数
        #region 获取配送方式(自提)[DLproc_UserAddressZTBySel]
        /// <summary>
        /// 获取配送方式(自提)[DLproc_UserAddressZTBySel]
        /// </summary>
        /// <param name="lngopUserId">用户id</param>
        /// <returns></returns>
        public DataTable DLproc_UserAddressZTBySel(string lngopUserId)
        {
            return bdao.DLproc_UserAddressZTBySel(lngopUserId);
        }
        #endregion
        #region 获取配送方式(配送)[DLproc_UserAddressPSBySel]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngopUserId">用户id</param>
        /// <returns></returns>
        public DataTable DLproc_UserAddressPSBySel(string lngopUserId)
        {
            return bdao.DLproc_UserAddressPSBySel(lngopUserId);
        }
        #endregion
        #region 获取行政区(自提)[DL_UserAddressZTXZQBySel]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ccuscode">用户id</param>
        /// <returns></returns>
        public DataTable DL_UserAddressZTXZQBySel(string ccuscode)
        {
            return bdao.DL_UserAddressZTXZQBySel(ccuscode);
        }
        #endregion
        #region 获取配送方式(自提)[DLproc_UserAddressZTBySelGroup]
        /// <summary>
        /// 获取配送方式(自提)[DLproc_UserAddressZTBySel]
        /// </summary>
        /// <param name="lngopUserId">用户id</param>
        /// <returns></returns>
        public DataTable DLproc_UserAddressZTBySelGroup(string lngopUserId)
        {
            return bdao.DLproc_UserAddressZTBySelGroup(lngopUserId);
        }
        #endregion
        #region 获取配送方式(配送)[DLproc_UserAddressPSBySelGroup]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngopUserId">用户id</param>
        /// <returns></returns>
        public DataTable DLproc_UserAddressPSBySelGroup(string lngopUserId)
        {
            return bdao.DLproc_UserAddressPSBySelGroup(lngopUserId);
        }
        #endregion

        #region 新增加(自提)[Insert_UserAddressZT]
        /// <summary>
        /// 新增加(自提)
        /// </summary>
        /// <param name="lngopUserId">用户id</param>
        /// <param name="strCarplateNumber">车牌号</param>
        /// <param name="strDriverName">司机姓名</param>
        /// <param name="strDriverTel">司机电话</param>
        /// <param name="strIdCard">司机身份证</param>
        /// <param name="lngCreater">创建人</param>
        /// <returns></returns>
        public bool Insert_UserAddressZT(string lngopUserId, string strDistributionType, string strCarplateNumber, string strDriverName, string strDriverTel, string strIdCard, string lngCreater)
        {
            return bdao.Insert_UserAddressZT(lngopUserId, strDistributionType, strCarplateNumber, strDriverName, strDriverTel, strIdCard, lngCreater);
        }
        #endregion
        #region 新增加(配送)[Insert_UserAddressPS]
        /// <summary>
        /// 新增加(配送)
        /// </summary>
        /// <param name="lngopUserId">用户id</param>
        /// <param name="strConsigneeName">收货人姓名</param>
        /// <param name="strConsigneeTel">收货人电话</param>
        /// <param name="strReceivingAddress">收货地址</param>
        /// <returns></returns>
        public bool Insert_UserAddressPS(string lngopUserId, string strDistributionType, string strConsigneeName, string strConsigneeTel, string strReceivingAddress, string strDistrict)
        {
            return bdao.Insert_UserAddressPS(lngopUserId, strDistributionType, strConsigneeName, strConsigneeTel, strReceivingAddress, strDistrict);
        }
        #endregion
        #region 新增加行政区域(自提)[Insert_UserAddressZTXZQ]
        /// <summary>
        /// 新增加(自提)
        /// </summary>
        /// <param name="ccuscode">用户编码</param>
        /// <param name="xzq">行政区</param>
        /// <param name="ccuscode_xzq">用户编码+行政区</param>
        /// <returns></returns>
        public bool Insert_UserAddressZTXZQ(string ccuscode, string xzq, string ccuscode_xzq)
        {
            return bdao.Insert_UserAddressZTXZQ(ccuscode, xzq, ccuscode_xzq);
        }
        #endregion
        #region  修改(自提)[Update_UserAddressZT]
        /// <summary>
        /// 修改(自提)
        /// </summary>
        /// <param name="ba"></param>
        /// <returns></returns>
        public bool Update_UserAddressZT(BasicInfo ba)
        {
            return bdao.Update_UserAddressZT(ba);
        }
        #endregion
        #region  修改(配送)[Update_UserAddressPS]
        /// <summary>
        /// 修改(配送)
        /// </summary>
        /// <param name="ba"></param>
        /// <returns></returns>
        public bool Update_UserAddressPS(BasicInfo ba)
        {
            return bdao.Update_UserAddressPS(ba);
        }
        #endregion
        #endregion

        #region 判断行政区域是否末级节点[DL_strDistrict]
        /// <summary>
        /// 判断行政区域是否末级节点[DL_strDistrict]
        /// </summary>
        /// <param name="strDistrict">行政区域</param>
        /// <returns></returns>
        public bool IsExists_strDistrict(string strDistrict)
        {
            return bdao.IsExists_strDistrict(strDistrict);
        }
        #endregion

        #region 获取车型[DL_cdefine3BySel]
        /// <summary>
        /// 获取车型[DL_cdefine3BySel]
        /// </summary>
        /// <returns></returns>
        public DataTable DL_cdefine3BySel()
        {
            return bdao.DL_cdefine3BySel();
        }
        #endregion

        #region 生成账单[DLproc_U8SOABySel]
        /// <summary>
        /// 生成账单[DLproc_U8SOABySel]
        /// </summary>
        /// <returns></returns>
        public DataTable DLproc_U8SOABySel(string ccuscode, string ddate)
        {
            return bdao.DLproc_U8SOABySel(ccuscode, ddate);
        }
        #endregion

        #region 生成账单(手工)[DLproc_U8SOAForDateBySel]
        /// <summary>
        /// 生成账单(手工)[DLproc_U8SOAForDateBySel]
        /// </summary>
        /// <param name="ccuscode">顾客编码</param>
        /// <param name="ddate">账单日期</param>
        /// <returns></returns>
        public DataTable DLproc_U8SOAForDateBySel(string ccuscode, string ddate)
        {
            return bdao.DLproc_U8SOAForDateBySel(ccuscode, ddate);
        }
        #endregion

        #region 顾客删除收货地址(后台隐藏,将bytstatus改为1)[DL_UserAddressByDel]
        /// <summary>
        /// 顾客删除收货地址(后台隐藏,将bytstatus改为1)[DL_UserAddressByDel]
        /// </summary>
        /// <returns></returns>
        public bool DL_UserAddressByDel(string lngopUseraddressId)
        {
            return bdao.DL_UserAddressByDel(lngopUseraddressId);
        }
        #endregion

        #region 顾客删除自提行政区[DL_UserAddress_exByDel]
        /// <summary>
        /// 顾客删除自提行政区[DL_UserAddress_exByDel]
        /// </summary>
        /// <returns></returns>
        public bool DL_UserAddress_exByDel(string lngopUseraddress_exId)
        {
            return bdao.DL_UserAddress_exByDel(lngopUseraddress_exId);
        }
        #endregion

        #region 查询账单发送设置[DL_SOASettingBySel]
        /// <summary>
        /// 查询账单发送设置[DL_SOASettingBySel]
        /// </summary>
        /// <returns></returns>
        public DataTable DL_SOASettingBySel()
        {
            return bdao.DL_SOASettingBySel();
        }
        #endregion

        #region 查询账单自动发送时间配置[DL_SOAAutoSendBySel]
        /// <summary>
        /// 查询账单自动发送时间配置[DL_SOASettingBySel]
        /// </summary>
        /// <returns></returns>
        public DataTable DL_SOAAutoSendBySel()
        {
            return bdao.DL_SOAAutoSendBySel();
        }
        #endregion

        #region 查询账单自动发送时间配置[DL_SOAAutoSendBySel]
        /// <summary>
        /// 查询账单自动发送时间配置[DL_SOASettingBySel]
        /// </summary>
        /// <returns></returns>
        public DataTable DL_SOAAutoSendBySel(string cuscode)
        {
            return bdao.DL_SOAAutoSendBySel(cuscode);
        }
        #endregion

        #region 设置账单自动发送时间配置[DL_SOAAutoSendByUpd]
        /// <summary>
        /// 设置账单自动发送时间配置[DL_SOAAutoSendByUpd]
        /// </summary>
        /// <returns></returns>
        public bool DL_SOAAutoSendByUpd(string cCusCode, string ccdefine1)
        {
            return bdao.DL_SOAAutoSendByUpd(cCusCode, ccdefine1);
        }
        #endregion

        #region 设置活动数量[DL_HDNRByUpd]
        /// <summary>
        /// 设置活动数量[DL_HDNRByUpd]
        /// </summary>
        /// <returns></returns>
        public bool DL_HDNRByUpd(string cInvCode, string NewiQuantity)
        {
            return bdao.DL_HDNRByUpd(cInvCode, NewiQuantity);
        }
        #endregion

        #region 增加新/变更的顾客电话号码[DL_NewCustomerPhoneNoByIns]
        /// <summary>
        /// 增加新/变更的顾客电话号码[DL_NewCustomerPhoneNoByIns]
        /// </summary>
        /// <returns></returns>
        public bool DL_NewCustomerPhoneNoByIns(string PhoneNo, string cCusCode)
        {
            return bdao.DL_NewCustomerPhoneNoByIns(PhoneNo, cCusCode);
        }
        #endregion

        #region 获取所有登录顾客名称,代码[DL_AllCustomerInfoBySel]
        /// <summary>
        /// 获取所有登录顾客名称,代码[DL_AllCustomerInfoBySel]
        /// </summary>
        /// <returns></returns>
        public DataTable DL_AllCustomerInfoBySel()
        {
            return bdao.DL_AllCustomerInfoBySel();
        }
        #endregion

        #region 用于通知内容(普通内容) [DLproc_NewsBySel]
        /// <summary>
        /// 于通知内容(普通内容) [DLproc_NewsBySel]
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public DataTable DLproc_NewsBySel()
        {
            return bdao.DLproc_NewsBySel();
        }
        #endregion

        #region 用于通知内容(重要内容) [DLproc_News1BySel]
        /// <summary>
        /// 用于通知内容 [DLproc_NewsBySel]
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public DataTable DLproc_News1BySel()
        {
            return bdao.DLproc_News1BySel();
        }
        #endregion

        #region 获取顾客登记的微信号信息[DL_GetCustomerWXInfoBySel]
        /// <summary>
        /// 获取顾客登记的微信号信息[DL_GetCustomerWXInfoBySel]
        /// </summary>
        /// <param name="concuscode">顾客编码</param>
        /// <returns></returns>
        public DataTable DL_GetCustomerWXInfoBySel(string concuscode)
        {
            return bdao.DL_GetCustomerWXInfoBySel(concuscode);
        }
        #endregion

        #region 添加顾客登记的微信号信息[DL_CustomerWXInfoByIns]
        /// <summary>
        /// 添加顾客登记的微信号信息[DL_CustomerWXInfoByIns]
        /// </summary>
        /// <param name="strccusname">顾客名称（常用）</param>
        /// <param name="strccuscode">顾客编码（常用）</param>
        /// <param name="strWXName">姓名</param>
        /// <param name="strWXPhoneNum">手机号码</param>
        /// <param name="strWXAcount">微信账号</param>
        /// <returns></returns>
        public bool DL_CustomerWXInfoByIns(string strccusname, string strccuscode, string strWXName, string strWXPhoneNum, string strWXAcount)
        {
            return bdao.DL_CustomerWXInfoByIns(strccusname, strccuscode, strWXName, strWXPhoneNum, strWXAcount);
        }
        #endregion

        #region 新增用户[DL_AddNewCustomerByIns]
        /// <summary>
        ///  新增用户[DL_AddNewCustomerByIns]
        /// </summary>
        /// <param name="ccuscode">顾客编码</param>
        /// <returns></returns>
        public bool DL_AddNewCustomerByIns(string ccuscode)
        {
            return bdao.DL_AddNewCustomerByIns(ccuscode);
        }
        #endregion

        #region 获取微信帐号对应绑定的网上订单的登录用户信息[DL_GetWXOpBindInfoBySel]
        /// <summary>
        /// 获取微信帐号对应绑定的网上订单的登录用户信息[DL_GetWXOpBindInfoBySel]
        /// </summary>
        /// <param name="strWXUserId">企业号的微信帐号</param>
        /// <returns></returns>
        public DataTable DL_GetWXOpBindInfoBySel(string strWXUserId)
        {
            return bdao.DL_GetWXOpBindInfoBySel(strWXUserId);
        }
        #endregion

        #region 获取微信token[DL_GetWXTokenBySel]
        /// <summary>
        /// 获取微信token[DL_GetWXTokenBySel]
        /// </summary>
        /// <param name="strWXUserId">企业号的微信帐号</param>
        /// <returns></returns>
        public DataTable DL_GetWXTokenBySel()
        {
            return bdao.DL_GetWXTokenBySel();
        }
        #endregion

        #region 恭喜发财[DLproc_Send500WBySel]
        /// <summary>
        /// 恭喜发财[DLproc_Send500WBySel]
        /// </summary>
        /// <param name="strWXUserId">企业号的微信帐号</param>
        /// <returns></returns>
        public DataTable DLproc_Send500WBySel()
        {
            return bdao.DLproc_Send500WBySel();
        }
        #endregion


    }
}
