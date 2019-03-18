using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BLL
{
    public class WeiXin
    {
        DAL.SQLHelper1 sqlhelper = new DAL.SQLHelper1();
        DAL.SQLHelper sqlh = new DAL.SQLHelper();
        DAL.DbDao DbDao = new DAL.DbDao();


        #region 根据电话号码获取Dl_opuser里的用户信息
        public DataTable GetIdByPhone(string phone)
        {
            string sql = string.Empty;
            sql = @"select strUserLevel,lngopUserId,strUserName,bb.cCusCode,cCusPPerson,cCusPhone,strLoginName,aa.strStatus,0 'lngopUserExId',strLoginName 'strAllAcount'  from Dl_opUser aa
left join Customer bb on aa.cCusCode=bb.cCusCode
where bb.cCusPhone like @phonelike  
union all
select dd.strUserLevel,dd.lngopUserId,strUserName,bb.cCusCode,cCusPPerson,dd.strSubPhone,dd.strLoginName,dd.strStatus,lngopUserExId,strAllAccount 'strAllAcount'  from Dl_opUser aa
left join Customer bb on aa.cCusCode=bb.cCusCode
left join Dl_opUser_Ex dd on aa.strLoginName=dd.strLoginName
where   dd.strSubPhone like @phonelike   ";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@phonelike","%"+phone+"%")
            };
            return sqlh.ExecuteQuery(sql, paras, CommandType.Text);
        }
        #endregion


        #region 当有一个手机号绑定多个账号时，用户选择一个用户名登录
        public DataTable SelectUserToLogin(string phone, string ccuscode)
        {
            string sql = @"select strUserLevel,lngopUserId,strUserName,bb.cCusCode,cCusPPerson,cCusPhone,strLoginName,aa.strStatus,0 'lngopUserExId',strLoginName 'strAllAcount'  from Dl_opUser aa
left join Customer bb on aa.cCusCode=bb.cCusCode
where bb.cCusPhone like @phonelike AND aa.strLoginName=@ccuscode
union all
select dd.strUserLevel,dd.lngopUserId,strUserName,bb.cCusCode,cCusPPerson,dd.strSubPhone,dd.strLoginName,dd.strStatus,lngopUserExId,strAllAccount 'strAllAcount'  from Dl_opUser aa
left join Customer bb on aa.cCusCode=bb.cCusCode
left join Dl_opUser_Ex dd on aa.strLoginName=dd.strLoginName
where   dd.strSubPhone like @phonelike  AND aa.strLoginName=@ccuscode";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@phonelike","%"+phone+"%"),
            new SqlParameter("@ccuscode",ccuscode)
            };
            return sqlh.ExecuteQuery(sql, paras, CommandType.Text);
        }
        #endregion


        #region 查询历史订单列表
        public DataTable GetHistoryOrderList(string lngopUserID, string start_day, string end_day)
        {
            DataTable dt = new DataTable();
            string sql = "select  lngopOrderId,datCreateTime,bytStatus,strbillno from dl_oporder where lngopUserID=@lngopUserID and datCreateTime between @start_day and @end_day";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@lngopUserID",lngopUserID) ,
                new SqlParameter("@start_day",start_day) ,
                new SqlParameter("@end_day",end_day) 
         
           };

            dt = sqlh.ExecuteQuery(sql, paras, CommandType.Text);
            return dt;
        }
        #endregion


        #region 查询历史订单详情
        public DataTable GetOrderInfo(string OrderId)
        {
            DataTable dt = new DataTable();
            string sql = @"select a.*,b.*,c.cInvStd from dl_oporderdetail a
                          left JOIN dl_oporder b ON a.lngopOrderId=b.lngopOrderId
                          LEFT JOIN inventory c ON a.cinvcode=c.cInvCode
                          WHERE b.lngopOrderId=@lngopOrderId";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@lngopOrderId",OrderId) 
           };

            dt = sqlh.ExecuteQuery(sql, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 获取安全库存信息
        public DataTable GetSafeStock(string date)
        {

            // string sql = "select * from DL_U8SafeStockWarn where cCheckDate=@date";
            string sql = "DLproc_U8SafeStockWarn";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@cCheckDate",date) 
           };

            return sqlh.ExecuteQuery(sql, paras, CommandType.StoredProcedure);

        }
        #endregion

        #region 获取采购申请单未到货入库的数据
        public DataTable DLproc_PUAppWarnBySel(string date)
        {
            string sql = "DLproc_PUAppWarnBySel";
            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@dDate",date)
            };
            return sqlh.ExecuteQuery(sql, paras, CommandType.StoredProcedure);
        }
        #endregion

        #region 按cCode查询已入库采购单详情
        public DataTable  DLproc_PUAppDetail(string cCode)
        {
            string sql = "DLproc_CGRKD_WX_BySel";
            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@cCode",cCode)
            };
            return sqlh.ExecuteQuery(sql, paras, CommandType.StoredProcedure);

        }
        #endregion

    }
}
