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
using Model;


namespace DAL
{
    /// <summary>
    /// 基础资料操作类
    /// </summary>
    public class BasicInfoDAO
    {
        private SQLHelper sqlhelper = null;

        public BasicInfoDAO()
        {
            sqlhelper = new SQLHelper();
        }

        #region 用户修改密码[Update_UserPWD]
        /// <summary>
        /// 用户修改密码
        /// </summary>
        /// <param name="ba">用户ID,密码</param>
        /// <returns></returns>
        public bool Update_UserPWD(BasicInfo ba)
        {
            bool flag = false;
            string sql = "update Dl_opUser set strUserPwd=@strUserPwd where lngopUserId=@lngopUserId";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@lngopUserId",ba.LngopUserId),
            new SqlParameter("@strUserPwd",ba.StrUserPwd)
            };
            int res = sqlhelper.ExecuteNonQuery(sql, paras, CommandType.Text);
            if (res > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion

        #region 更新二维码扫码登录的表[Update_QrCodeLogin]
        /// <summary>
        /// 更新二维码扫码登录的表[Update_QrCodeLogin]
        /// </summary>
        /// <param name="sessionID">sessionID</param>
        /// <returns></returns>
        public bool Update_QrCodeLogin(string sessionID, string bFlag, string userid)
        {
            bool flag = false;
            string sql = "update QrCodeLogin set bFlag=@bFlag,logintime=getdate(),userid=@userid where sessionID=@sessionID";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@sessionID",sessionID),
            new SqlParameter("@bFlag",bFlag),
            new SqlParameter("@userid",userid)
            };
            int res = sqlhelper.ExecuteNonQuery(sql, paras, CommandType.Text);
            if (res > 0)
            {
                flag = true;
            }
            return flag;
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
            string sql = "SELECT * FROM dbo.QrCodeLogin WHERE sessionID=@sessionID AND bFlag=1";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@sessionID",sessionID)
            };
            DataTable dt = sqlhelper.ExecuteQuery(sql, paras, CommandType.Text);

            return dt;
        }
        #endregion

        #region 用户修改密码(子账户专用修改密码)[Update_SubUserPWD]
        /// <summary>
        /// 用户修改密码(子账户专用修改密码)[Update_SubUserPWD]
        /// </summary>
        /// <param name="ba">用户ID,密码</param>
        /// <returns></returns>
        public bool Update_SubUserPWD(string lngopUserExId, string strUserPwd)
        {
            bool flag = false;
            string sql = "update Dl_opUser_Ex set strUserPwd=@strUserPwd where lngopUserExId=@lngopUserExId";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@lngopUserExId",lngopUserExId),
            new SqlParameter("@strUserPwd",strUserPwd)
            };
            int res = sqlhelper.ExecuteNonQuery(sql, paras, CommandType.Text);
            if (res > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion

        #region 用户地址操作函数
        #region 获取配送方式(自提)[DLproc_UserAddressZTBySel]
        /// <summary>
        /// 获取配送方式(自提)
        /// </summary>
        /// <param name="lngopUserId">用户编码</param>
        /// <returns></returns>
        public DataTable DLproc_UserAddressZTBySel(string lngopUserId)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_UserAddressZTBySel";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@lngopUserId",lngopUserId)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion
        #region 获取配送方式(配送)[DLproc_UserAddressPSBySel]
        /// <summary>
        /// 获取配送方式(配送)
        /// </summary>
        /// <param name="lngopUserId">用户编码</param>
        /// <returns></returns>
        public DataTable DLproc_UserAddressPSBySel(string lngopUserId)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_UserAddressPSBySel";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@lngopUserId",lngopUserId)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion
        #region 获取行政区(自提)[DL_UserAddressZTXZQBySel]
        /// <summary>
        /// 获取配送方式(配送)
        /// </summary>
        /// <param name="ccuscode">用户编码</param>
        /// <returns></returns>
        public DataTable DL_UserAddressZTXZQBySel(string ccuscode)
        {
            DataTable dt = new DataTable();
            string cmdText = "select * from Dl_opUserAddress_ex where ccuscode=@ccuscode";
            SqlParameter[] paras = new SqlParameter[] {
           new SqlParameter("@ccuscode",ccuscode)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion
        #region 获取配送方式(自提)[DLproc_UserAddressZTBySelGroup]
        /// <summary>
        /// 获取配送方式(自提)
        /// </summary>
        /// <param name="lngopUserId">用户编码</param>
        /// <returns></returns>
        public DataTable DLproc_UserAddressZTBySelGroup(string lngopUserId)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_UserAddressZTBySelGroup";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@lngopUserId",lngopUserId)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion
        #region 获取配送方式(配送)[DLproc_UserAddressPSBySelGroup]
        /// <summary>
        /// 获取配送方式(配送)
        /// </summary>
        /// <param name="lngopUserId">用户编码</param>
        /// <returns></returns>
        public DataTable DLproc_UserAddressPSBySelGroup(string lngopUserId)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_UserAddressPSBySelGroup";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@lngopUserId",lngopUserId)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion
        #region 新增加(自提)[Insert_UserAddressZT]
        /// <summary>
        /// 新增加(自提)
        /// </summary>
        /// <param name="lngopUserId"></param>
        /// <param name="strCarplateNumber"></param>
        /// <param name="strDriverName"></param>
        /// <param name="strDriverTel"></param>
        /// <param name="strIdCard"></param>
        /// <param name="lngCreater"></param>
        /// <returns></returns>
        public bool Insert_UserAddressZT(string lngopUserId, string strDistributionType, string strCarplateNumber, string strDriverName, string strDriverTel, string strIdCard, string lngCreater)
        {
            bool flag = false;
            string sql = "insert into Dl_opUserAddress (lngopUserId,strDistributionType,strCarplateNumber,strDriverName,strDriverTel,strIdCard,lngCreater) values (@lngopUserId,@strDistributionType,@strCarplateNumber,@strDriverName,@strDriverTel,@strIdCard,@lngCreater)";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@lngopUserId",lngopUserId),
            new SqlParameter("@strDistributionType",strDistributionType),            
            new SqlParameter("@strCarplateNumber",strCarplateNumber),
            new SqlParameter("@strDriverName",strDriverName),
            new SqlParameter("@strDriverTel",strDriverTel),
            new SqlParameter("@strIdCard",strIdCard),
            new SqlParameter("@lngCreater",lngCreater)
            };
            int res = sqlhelper.ExecuteNonQuery(sql, paras, CommandType.Text);
            if (res > 0)
            {
                flag = true;
            }
            return flag;
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
            bool flag = false;
            string sql = "insert into Dl_opUserAddress (lngopUserId,strDistributionType,strConsigneeName,strConsigneeTel,strReceivingAddress,strDistrict) values (@lngopUserId,@strDistributionType,@strConsigneeName,@strConsigneeTel,@strReceivingAddress,@strDistrict)";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@lngopUserId",lngopUserId),
            new SqlParameter("@strDistributionType",strDistributionType),
            new SqlParameter("@strConsigneeName",strConsigneeName),
            new SqlParameter("@strConsigneeTel",strConsigneeTel),
            new SqlParameter("@strReceivingAddress",strReceivingAddress),
            new SqlParameter("@strDistrict",strDistrict)            
            };
            int res = sqlhelper.ExecuteNonQuery(sql, paras, CommandType.Text);
            if (res > 0)
            {
                flag = true;
            }
            return flag;
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
            bool flag = false;
            string sql = "insert into Dl_opUserAddress_ex (ccuscode,xzq,ccuscode_xzq) values (@ccuscode,@xzq,@ccuscode_xzq)";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@ccuscode",ccuscode),
            new SqlParameter("@xzq",xzq),
            new SqlParameter("@ccuscode_xzq",ccuscode_xzq)
            };
            int res = sqlhelper.ExecuteNonQuery(sql, paras, CommandType.Text);
            if (res > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion
        #region 修改(自提)[Update_UserAddressZT]
        /// <summary>
        /// 修改(自提)
        /// </summary>
        /// <param name="ba"></param>
        /// <returns></returns>
        public bool Update_UserAddressZT(BasicInfo ba)
        {
            bool flag = false;
            string sql = "update Dl_opUserAddress set strCarplateNumber=@strCarplateNumber,strDriverName=@strDriverName,strDriverTel=@strDriverTel,strIdCard=@strIdCard where lngopUseraddressId=@id";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@id",ba.Id),
            new SqlParameter("@strCarplateNumber",ba.StrCarplateNumber),
            new SqlParameter("@strDriverName",ba.StrDriverName),
            new SqlParameter("@strDriverTel",ba.StrDriverTel),
            new SqlParameter("@strIdCard",ba.StrIdCard)
            };
            int res = sqlhelper.ExecuteNonQuery(sql, paras, CommandType.Text);
            if (res > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion
        #region 修改(配送)[Update_UserAddressPS]
        /// <summary>
        /// 修改(配送)
        /// </summary>
        /// <param name="ba"></param>
        /// <returns></returns>
        public bool Update_UserAddressPS(BasicInfo ba)
        {
            bool flag = false;
            string sql = "update Dl_opUserAddress set strConsigneeName=@strConsigneeName,strConsigneeTel=@strConsigneeTel,strReceivingAddress=@strReceivingAddress,strDistrict=@strDistrict where lngopUseraddressId=@id";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@id",ba.Id),
            new SqlParameter("@strConsigneeName",ba.StrConsigneeName),
            new SqlParameter("@strConsigneeTel",ba.StrConsigneeTel),
            new SqlParameter("@strReceivingAddress",ba.StrReceivingAddress),
            new SqlParameter("@strDistrict",ba.StrDistrict)
            };
            int res = sqlhelper.ExecuteNonQuery(sql, paras, CommandType.Text);
            if (res > 0)
            {
                flag = true;
            }
            return flag;
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
            bool flag = false;
            string sql = " select 1 from HR_CT007 where vdescription='" + strDistrict + "' and bChildFlag=0";
            DataTable dt = sqlhelper.ExecuteQuery(sql, CommandType.Text);
            if (dt.Rows.Count > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion

        #region 判断起运地是否已经存在
        /// <summary>
        /// 判断基础资料是否已经存在
        /// </summary>
        /// <param name="caName">起运地名称</param>
        /// <returns></returns>
        public bool IsExists_StartPlace(string caName)
        {
            bool flag = false;
            string sql = "select * from T_StartPlace where FStartPlace='" + caName + "'";
            DataTable dt = sqlhelper.ExecuteQuery(sql, CommandType.Text);
            if (dt.Rows.Count > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion


        #region 获取车型[DL_cdefine3BySel]
        /// <summary>
        /// 获取车型[DL_cdefine3BySel]
        /// </summary>
        /// <returns></returns>
        public DataTable DL_cdefine3BySel()
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT cValue FROM UserDefine WHERE cID='03'";
            dt = sqlhelper.ExecuteQuery(cmdText, CommandType.Text);
            return dt;
        }
        #endregion


        #region 生成账单[DLproc_U8SOABySel]
        /// <summary>
        /// 生成账单[DLproc_U8SOABySel]
        /// </summary>
        /// <returns></returns>
        public DataTable DLproc_U8SOABySel(string ccuscode, string ddate)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_U8SOABySel";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@ccuscode",ccuscode),
           new SqlParameter("@ddate",ddate)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
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
            DataTable dt = new DataTable();
            string cmdText = "DLproc_U8SOAForDateBySel";
            SqlParameter[] paras = new SqlParameter[] {
           new SqlParameter("@ccuscode",ccuscode),
           new SqlParameter("@ddate",ddate)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 顾客删除收货地址(后台隐藏,将bytstatus改为1)[DL_UserAddressByDel]
        /// <summary>
        /// 顾客删除收货地址(后台隐藏,将bytstatus改为1)[DL_UserAddressByDel]
        /// </summary>
        /// <returns></returns>
        public bool DL_UserAddressByDel(string lngopUseraddressId)
        {
            bool flag = false;
            DataTable dt = new DataTable();
            string cmdText = "update Dl_opUserAddress set bytStatus=1 where lngopUseraddressId=@lngopUseraddressId";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@lngopUseraddressId",lngopUseraddressId)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            if (dt.Rows.Count > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion

        #region 顾客删除自提行政区[DL_UserAddress_exByDel]
        /// <summary>
        /// 顾客删除自提行政区[DL_UserAddress_exByDel]
        /// </summary>
        /// <returns></returns>
        public bool DL_UserAddress_exByDel(string lngopUseraddress_exId)
        {
            bool flag = false;
            DataTable dt = new DataTable();
            string cmdText = "delete Dl_opUserAddress_ex  where lngopUseraddress_exId=@lngopUseraddress_exId";
            SqlParameter[] paras = new SqlParameter[] {
           new SqlParameter("@lngopUseraddress_exId",lngopUseraddress_exId)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            if (dt.Rows.Count > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion

        #region 查询账单发送设置[DL_SOASettingBySel]
        /// <summary>
        /// 查询账单发送设置[DL_SOASettingBySel]
        /// </summary>
        /// <returns></returns>
        public DataTable DL_SOASettingBySel()
        {
            DataTable dt = new DataTable();
            string cmdText = "select * from Dl_opSOASetting order by lngopSOASettingId";
            dt = sqlhelper.ExecuteQuery(cmdText, CommandType.Text);
            return dt;
        }
        #endregion

        #region 查询账单自动发送时间配置[DL_SOAAutoSendBySel]
        /// <summary>
        /// 查询账单自动发送时间配置[DL_SOAAutoSendBySel]
        /// </summary>
        /// <returns></returns>
        public DataTable DL_SOAAutoSendBySel()
        {
            DataTable dt = new DataTable();
            //string cmdText = "select aa.strLoginName,bb.cCusCode,bb.cCusName,isnull(cc.ccdefine1,0) SOASendTime from Dl_opUser aa left join Customer bb on aa.cCusCode=left(bb.cCusCode,6) left join Customer_extradefine cc on bb.cCusCode=cc.cCusCode  where aa.strUserLevel=3 and strStatus=1 and bb.iCusCreLine!=0 order by bb.cCusCode";
            string cmdText = "select aa.strLoginName,bb.cCusCode,bb.cCusName,isnull(cc.ccdefine1,0) SOASendTime from Dl_opUser aa left join Customer bb on aa.cCusCode=left(bb.cCusCode,6) left join Customer_extradefine cc on bb.cCusCode=cc.cCusCode  where aa.strUserLevel=3 and strStatus=1 AND bb.cCusCode IS NOT NULL  order by bb.cCusCode";
            dt = sqlhelper.ExecuteQuery(cmdText, CommandType.Text);
            return dt;
        }
        #endregion

        #region 查询账单自动发送时间配置[DL_SOAAutoSendBySel]
        /// <summary>
        /// 查询账单自动发送时间配置[DL_SOAAutoSendBySel]
        /// </summary>
        /// <param name="cuscode">顾客编码(%)</param>
        /// <returns></returns>
        public DataTable DL_SOAAutoSendBySel(string cuscode)
        {
            DataTable dt = new DataTable();
            string cmdText = "select bb.*,aa.cCusName from Customer aa left join Customer_extradefine bb on aa.cCusCode=bb.cCusCode where bb.cCusCode like @cuscode order by bb.cCusCode";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@cuscode",cuscode)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 设置账单自动发送时间配置[DL_SOAAutoSendByUpd]
        /// <summary>
        /// 设置账单自动发送时间配置[DL_SOAAutoSendByUpd]
        /// </summary>
        /// <returns></returns>
        public bool DL_SOAAutoSendByUpd(string cCusCode, string ccdefine1)
        {
            bool flag = false;
            string cmdText = "update Customer_extradefine set ccdefine1=@ccdefine1 where cCusCode=@cCusCode";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@cCusCode",cCusCode),
            new SqlParameter("@ccdefine1",ccdefine1)
            };
            int res = sqlhelper.ExecuteNonQuery(cmdText, paras, CommandType.Text);
            if (res > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion

        #region 设置活动数量[DL_HDNRByUpd]
        /// <summary>
        /// 设置活动数量[DL_HDNRByUpd]
        /// </summary>
        /// <returns></returns>
        public bool DL_HDNRByUpd(string cInvCode, string NewiQuantity)
        {
            bool flag = false;
            string cmdText = "update Dl_opSysSalesPolicy_LimitedSupply set iQuantity=@NewiQuantity where cInvCode=@cInvCode";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@cInvCode",cInvCode),
            new SqlParameter("@NewiQuantity",NewiQuantity)
            };
            int res = sqlhelper.ExecuteNonQuery(cmdText, paras, CommandType.Text);
            if (res > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion

        #region 增加新/变更的顾客电话号码[DL_NewCustomerPhoneNoByIns]
        /// <summary>
        /// 增加新/变更的顾客电话号码[DL_NewCustomerPhoneNoByIns]
        /// </summary>
        /// <returns></returns>
        public bool DL_NewCustomerPhoneNoByIns(string PhoneNo, string cCusCode)
        {
            bool flag = false;
            string cmdText = "update  Customer set cCusPhone=@PhoneNo where cCusCode=@cCusCode";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@PhoneNo",PhoneNo),
            new SqlParameter("@cCusCode",cCusCode)
            };
            int res = sqlhelper.ExecuteNonQuery(cmdText, paras, CommandType.Text);
            if (res > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion

        #region 获取所有登录顾客名称,代码[DL_AllCustomerInfoBySel]
        /// <summary>
        /// 获取所有登录顾客名称,代码[DL_AllCustomerInfoBySel]
        /// </summary>
        /// <returns></returns>
        public DataTable DL_AllCustomerInfoBySel()
        {
            DataTable dt = new DataTable();
            string cmdText = "select * from Dl_opUser where strUserLevel=3 order by strLoginName";
            dt = sqlhelper.ExecuteQuery(cmdText, CommandType.Text);
            return dt;
        }
        #endregion

        #region 用于通知内容(普通内容) [DLproc_NewsBySel]
        /// <summary>
        /// 用于通知内容(普通内容) [DLproc_NewsBySel]
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public DataTable DLproc_NewsBySel()
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_NewsBySel";
            dt = sqlhelper.ExecuteQuery(cmdText, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 用于通知内容(重要内容) [DLproc_News1BySel]
        /// <summary>
        /// 用于通知内容(重要内容) [DLproc_News1BySel]
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public DataTable DLproc_News1BySel()
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_News1BySel";
            dt = sqlhelper.ExecuteQuery(cmdText, CommandType.StoredProcedure);
            return dt;
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
            DataTable dt = new DataTable();
            string cmdText = "select * from Dl_opWXMsgRegister where strccuscode=@concuscode";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@concuscode",concuscode)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
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
            bool flag = false;
            string cmdText = "insert into Dl_opWXMsgRegister (strccusname,strccuscode,strWXName,strWXPhoneNum,strWXAcount) select @strccusname,@strccuscode,@strWXName,@strWXPhoneNum,@strWXAcount";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@strccusname",strccusname),
           new SqlParameter("@strccuscode",strccuscode),
           new SqlParameter("@strWXName",strWXName),
           new SqlParameter("@strWXPhoneNum",strWXPhoneNum),
           new SqlParameter("@strWXAcount",strWXAcount)
           };
            int res = sqlhelper.ExecuteNonQuery(cmdText, paras, CommandType.Text);
            if (res > 0)
            {
                flag = true;
            }
            return flag;
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
            bool flag = false;
            string cmdText = "insert into Dl_opUser (strLoginName,strUserPwd,strUserName,strUserTel,strUserMail,strUserQQ,strUserIdCard,strUserCompName,strUserCompAddress,cCusCode,strStatus,strUserLevel,dlbCreditLine,dblAvailableCreditLine,datCreatetime,lngCreater) select @ccuscode,'098f6bcd4621d373cade4e832627b4f6',cCusName,' ',' ',' ',' ',' ',' ',@ccuscode,1,3,0,0,getdate(),0 from Customer where cCusCode= @ccuscode";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@ccuscode",ccuscode)
           };
            int res = sqlhelper.ExecuteNonQuery(cmdText, paras, CommandType.Text);
            if (res > 0)
            {
                flag = true;
            }
            return flag;
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
            DataTable dt = new DataTable();
            string cmdText = "select * from Dl_opWXAccount where strWXUserId=@strWXUserId";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@strWXUserId",strWXUserId)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 获取微信token[DL_GetWXTokenBySel]
        /// <summary>
        /// 获取微信token[DL_GetWXTokenBySel]
        /// </summary>
        /// <returns></returns>
        public DataTable DL_GetWXTokenBySel()
        {
            DataTable dt = new DataTable();
            string cmdText = "select top(1) access_token from Dl_opSystemConfiguration";
            SqlParameter[] paras = new SqlParameter[] { 
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 恭喜发财[DLproc_Send500WBySel]
        /// <summary>
        /// 恭喜发财[DLproc_Send500WBySel]
        /// </summary>
        /// <returns></returns>
        public DataTable DLproc_Send500WBySel()
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_Send500WBySel";
            SqlParameter[] paras = new SqlParameter[] { 
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion


    }
}




