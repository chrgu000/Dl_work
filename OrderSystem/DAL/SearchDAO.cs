/*
 *创建人：ECHO 
 *创建时间：2015-10-09
 *说明：报表查询操作类
 * 版权所有：
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class SearchDAO
    {
        private SQLHelper sqlhelper = null;

        public SearchDAO()
        {
            sqlhelper = new SQLHelper();
        }

        #region 获取所有物料[DL_InventoryAllBySel]
        /// <summary>
        /// KeyFieldName	ParentFieldName	NodeName
        /// 01	            null	        产成品
        /// </summary>
        /// <returns></returns>
        public DataTable DL_InventoryAllBySel()
        {
            DataTable dt = new DataTable();
            string cmdText = "select cInvCode,cInvName,cInvStd from Inventory";
            SqlParameter[] paras = new SqlParameter[] {  
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 获取物料树结构[DLproc_InventoryBySel]
        /// <summary>
        /// <param name="cSTCode">销售类型</param>
        /// <param name="ccuscode">顾客编码</param>
        /// </summary>
        /// <returns></returns>
        public DataTable DLproc_InventoryBySel(string cSTCode, string ccuscode)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_InventoryBySel";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@cSTCode",cSTCode),
           new SqlParameter("@ccuscode",ccuscode)  
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 获取物料树明细[DLproc_TreeListDetailsBySel]
        /// <summary>
        /// 获取物料树明细[DLproc_TreeListDetailsBySel]
        /// </summary>
        /// <param name="cInvCCode">物料大类编码</param>
        /// <param name="cCusCode">开票单位编码</param>
        /// <returns></returns>
        public DataTable DLproc_TreeListDetailsBySel(string cInvCCode, string cCusCode)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_TreeListDetailsBySel";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@cInvCCode",cInvCCode),
           new SqlParameter("@cCusCode",cCusCode)           
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 获取物料树明细(包含大类)[DLproc_TreeListDetailsAllBySel]
        /// <summary>
        /// 获取物料树明细
        /// </summary>
        /// <param name="cInvCCode">物料大类编码</param>
        /// <param name="cCusCode">顾客编码</param>
        /// <returns></returns>
        public DataTable DLproc_TreeListDetailsAllBySel(string cInvCCode, string cCusCode)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_TreeListDetailsAllBySel";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@cInvCCode",cInvCCode),
           new SqlParameter("@cCusCode",cCusCode)           
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 获取物料树明细(包含大类,以及数量)[DLproc_TreeListDetailsAll_iqty_BySel]
        /// <summary>
        /// 获取物料树明细
        /// </summary>
        /// <param name="cInvCCode">物料大类编码</param>
        /// <param name="cCusCode">顾客编码</param>
        /// <returns></returns>
        public DataTable DLproc_TreeListDetailsAll_iqty_BySel(string cInvCCode, string cCusCode)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_TreeListDetailsAll_iqty_BySel";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@cInvCCode",cInvCCode),
           new SqlParameter("@cCusCode",cCusCode)           
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 获取预订单的参照订单树明细[DLproc_TreeListPreDetailsBySel]
        /// <summary>
        /// 获取预订单的参照订单树明细[DLproc_TreeListPreDetailsBySel]
        /// </summary>
        /// <param name="strBillNo">预订单编号</param>
        ///  <param name="lngBillType">单据类型,1酬宾,2特殊</param>
        /// <returns></returns>
        public DataTable DLproc_TreeListPreDetailsBySel(string strBillNo, int lngBillType)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_TreeListPreDetailsBySel";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@strBillNo",strBillNo) ,
           new SqlParameter("@lngBillType",lngBillType)              
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region [修改预订单]获取物料(预订单参照生成酬宾,特殊订单)树明细[DLproc_TreeListPreDetailsModifyBySel]
        /// <summary>
        /// [修改预订单]获取物料(预订单参照生成酬宾,特殊订单)树明细[DLproc_TreeListPreDetailsModifyBySel]
        /// </summary>
        /// <param name="strBillNo">预订单编号</param>
        /// <param name="OrderBillNo">被修改的订单编号</param>
        /// <param name="lngBillType">单据类型,1酬宾,2特殊</param>
        /// <returns></returns>
        public DataTable DLproc_TreeListPreDetailsModifyBySel(string strBillNo, string OrderBillNo, int lngBillType)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_TreeListPreDetailsModifyBySel";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@strBillNo",strBillNo) ,
           new SqlParameter("@OrderBillNo",OrderBillNo) ,
           new SqlParameter("@lngBillType",lngBillType)              
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 获取预订单物料树明细[DLproc_TreeListDetailsNoQTYBySel]
        /// <summary>
        /// 获取预订单物料树明细[DLproc_TreeListDetailsNoQTYBySel]
        /// </summary>
        /// <param name="cInvCCode">物料大类编码</param>
        /// <returns></returns>
        public DataTable DLproc_TreeListDetailsNoQTYBySel(string cInvCCode, string cCusCode)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_TreeListDetailsNoQTYBySel";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@cInvCCode",cInvCCode),
           new SqlParameter("@cCusCode",cCusCode)           
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 获取顾客抬头[DL_ComboCustomerAllBySel]
        /// <summary>
        /// 获取顾客抬头,业务员
        /// </summary>
        /// <param name="cCusCode">登录顾客编码</param>
        /// <returns></returns>
        public DataTable DL_ComboCustomerAllBySel(string cCusCode)
        {
            DataTable dt = new DataTable();
            string cmdText = "select cCusCode,cCusName,cCusPPerson from Customer where cCusCode like @cCusCode ";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@cCusCode",cCusCode)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion       

        #region 获取顾客抬头（排除禁用客户）[DL_ComboCustomerBySel]
        /// <summary>
        /// 获取顾客抬头,业务员（排除禁用客户）
        /// </summary>
        /// <param name="cCusCode">登录顾客编码</param>
        /// <returns></returns>
        public DataTable DL_ComboCustomerBySel(string cCusCode)
        {
            DataTable dt = new DataTable();
            string cmdText = "select cCusCode,cCusName,cCusPPerson from Customer where cCusCode like @cCusCode and dEndDate is null";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@cCusCode",cCusCode)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 获取单据类型[DL_BillTypeBySel]
        /// <summary>
        /// 获取单据类型[DL_BillTypeBySel]
        /// </summary>
        /// <param name="StrBillNo">订单编号</param>
        /// <returns></returns>
        public DataTable DL_BillTypeBySel(string StrBillNo)
        {
            DataTable dt = new DataTable();
            string cmdText = "select strBillNo, case when  cSTCode='00' and lngBillType='0' then '普通订单' when  cSTCode='01' and lngBillType='0' then '样品订单' when  lngBillType='1' then '酬宾订单' when  lngBillType='2' then '特殊订单' end 'BillType',cSTCode,lngBillType from Dl_opOrder where strBillNo=@StrBillNo";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@StrBillNo",StrBillNo)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 获取账单信息(顾客,根据ID)[DL_SOAforIdBySel]
        /// <summary>
        /// 获取账单信息(顾客,根据ID)[DL_SOAforIdBySel]
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public DataTable DL_SOAforIdBySel(string id)
        {
            DataTable dt = new DataTable();
            string cmdText = "select *,convert(varchar(10),strEndDate,120) 'ddate'  from Dl_opU8SOA where lngSOAid=@id";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@id",id)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 获取顾客关联的已完成系统订单编号[DL_ComboCustomerU8NOBySel]
        /// <summary>
        /// 获取顾客关联的已完成系统订单编号[DL_ComboCustomerU8NOBySel]
        /// </summary>
        /// <param name="cCusCode">登录顾客编码</param>
        /// <returns></returns>
        public DataTable DL_ComboCustomerU8NOBySel(string cCusCode)
        {
            DataTable dt = new DataTable();
            //string cmdText = "select do.cSOCode,do.strBillNo,do.datBillTime  from Dl_opOrder do left join Dl_opUser dou on do.lngopUserId=dou.lngopUserId where dou.strLoginName=@cCusCode and cSOCode is not null and cSOCode!=' ' and cSOCode!=''  and do.cSTCode='00' group by do.cSOCode,do.strBillNo,do.datBillTime  order by do.cSOCode desc";
            string cmdText = "  select do.cSOCode,do.strBillNo,do.datBillTime  from Dl_opOrder do left join Dl_opUser dou on do.lngopUserId=dou.lngopUserId where dou.strLoginName=@cCusCode and do.cSTCode='00' group by do.cSOCode,do.strBillNo,do.datBillTime  order by do.datBillTime desc";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@cCusCode",cCusCode)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 获取行政区域树结构[DL_HR_CT007BySel]
        /// <summary>
        /// KeyFieldName	ParentFieldName	NodeName
        /// 01	            null	        产成品
        /// </summary>
        /// <returns></returns>
        public DataTable DL_HR_CT007BySel()
        {
            DataTable dt = new DataTable();
            string cmdText = "select ccodeID,cpCodeID,vsimpleName,vdescription from HR_CT007";
            dt = sqlhelper.ExecuteQuery(cmdText, CommandType.Text);
            return dt;
        }
        #endregion

        #region 获取是否开启酬宾订单（日期控制）[DL_PreOrder]
        /// <summary>
        /// KeyFieldName	ParentFieldName	NodeName
        /// 01	            null	        产成品
        /// </summary>
        /// <returns></returns>
        public DataTable DL_PreOrder()
        {
            DataTable dt = new DataTable();
            string cmdText = "select * from Dl_opPreOrderControl where GETDATE() between convert(datetime,datStartTime,120) and convert(datetime,datEndTime,120) and  IsEnable=1 and IsTimeControl=1";
            dt = sqlhelper.ExecuteQuery(cmdText, CommandType.Text);
            return dt;
        }
        #endregion

        #region 获取预订单树结构(含子账号)[DL_PreOrderTreeBySel]
        /// <summary>
        /// KeyFieldName	ParentFieldName	NodeName
        /// 01	            null	        产成品
        /// </summary>
        /// <returns></returns>
        public DataTable DL_PreOrderTreeBySel(string cCusCode,string lngopUserId, string lngopUserExId, int lngBillType)
        {
            DataTable dt = new DataTable();
            //string cmdText = "select * from (select '请选择' 'strBillNo' union all select cc.strBillNo from SA_PreOrderDetails bb inner join SA_PreOrderMain aa on aa.ID=bb.ID left join Dl_opPreOrder cc on cc.ccode=aa.cCode where aa.cCusCode=@cCusCode and cc.lngopUserId=@lngopUserId and isnull(cc.lngopUserExId,'0')=@lngopUserExId and  bb.cSCloser is null and (isnull(iQuantity,0)-isnull(fdhquantity,0))>0 and strBillNo is not null and cc.lngBillType=@lngBillType) hh group by  hh.strBillNo order by  hh.strBillNo  desc ";
             //string cmdText ="select * from (select '请选择' 'strBillNo' union all select cc.strBillNo from SA_PreOrderDetails bb inner join SA_PreOrderMain aa on aa.ID=bb.ID left join Dl_opPreOrder cc on cc.ccode=aa.cCode where aa.cCusCode=@cCusCode and cc.lngopUserId=@lngopUserId and  bb.cSCloser is null and (isnull(iQuantity,0)-isnull(fdhquantity,0))>0 and strBillNo is not null and cc.lngBillType=@lngBillType) hh group by  hh.strBillNo order by  hh.strBillNo  desc ";
           // string cmdText = "select * from (SELECT '请选择' 'strBillNo' union all SELECT cc.strBillNo from SA_PreOrderDetails bb INNER join SA_PreOrderMain aa on aa.ID=bb.ID LEFT join Dl_opPreOrder cc on cc.ccode=aa.cCode LEFT JOIN dbo.SO_SODetails dd ON dd.cpreordercode =aa.cCode AND dd.iaoids=bb.autoid LEFT JOIN dbo.DispatchLists ee ON ee.cCode=dd.cSOCode AND ee.cInvCode=dd.cInvCode AND dd.iaoids=ee.iSOsID  WHERE aa.cCusCode=@cCusCode AND cc.lngopUserId=@lngopUserId and  bb.cSCloser is null AND strBillNo is not null and cc.lngBillType=@lngBillType GROUP BY cc.strBillNo HAVING avg(ISNULL(bb.iQuantity,0))-SUM(ISNULL(ee.iQuantity,0)) >0 ) hh group by  hh.strBillNo order by  hh.strBillNo  desc ";
            string cmdText = "select * from (SELECT '请选择' 'strBillNo' UNION all SELECT cc.strBillNo from SA_PreOrderDetails bb INNER join SA_PreOrderMain aa on aa.ID=bb.ID LEFT join Dl_opPreOrder cc on cc.ccode=aa.cCode LEFT JOIN dbo.SO_SODetails dd ON dd.cpreordercode =aa.cCode AND dd.iaoids=bb.autoid LEFT JOIN dbo.DispatchLists ee ON ee.cCode=dd.cSOCode AND ee.cInvCode=dd.cInvCode AND dd.iaoids=ee.iSOsID LEFT JOIN dbo.SO_SOMain ff ON ff.id=dd.ID  WHERE aa.cCusCode=@cCusCode AND cc.lngopUserId=@lngopUserId and  bb.cSCloser is null AND strBillNo is not null and cc.lngBillType=@lngBillType AND ff.cCloser IS NULL GROUP BY cc.strBillNo HAVING avg(ISNULL(bb.iQuantity,0))-SUM(ISNULL(ee.iQuantity,0)) >0 UNION ALL SELECT bb.cSOCode FROM dbo.Dl_opPreOrder aa LEFT JOIN dbo.SO_SOMain bb ON aa.ccode=bb.cSOCode  where aa.ccode LIKE 'TS%' AND bb.cCloser IS NULL AND bb.cCusCode=@cCusCode  AND aa.lngBillType=@lngBillType AND aa.lngopUserId=@lngopUserId AND aa.bytStatus=4 AND bb.cCloser IS NULL) hh group by  hh.strBillNo order by  hh.strBillNo  desc ";
            SqlParameter[] paras = new SqlParameter[] { 
            new SqlParameter("@cCusCode",cCusCode),
            new SqlParameter("@lngopUserId",lngopUserId), 
            new SqlParameter("@lngopUserExId",lngopUserExId),
            new SqlParameter("@lngBillType",lngBillType)             
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 获取顾客对账单信息(顾客查询)[DLproc_U8SOASearchBySel]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cCusCode"></param>
        /// <param name="intPeriod"></param>
        /// <param name="intCheck"></param>
        /// <returns></returns>
        public DataTable DLproc_U8SOASearchBySel(string strComboPeriodYear, string cCusCode, string intPeriod, string intCheck, string strconccuscode)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_U8SOASearchBySel";
            SqlParameter[] paras = new SqlParameter[] { 
            new SqlParameter("@strComboPeriodYear",strComboPeriodYear),               
            new SqlParameter("@cCusCode",cCusCode), 
            new SqlParameter("@intPeriod",intPeriod),
            new SqlParameter("@intCheck",intCheck),
            new SqlParameter("@strconccuscode",strconccuscode)             
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 获取顾客对账单明细信息（名字账单）[DLproc_SOADetailforSearchBySel]
        /// <summary>
        /// 获取顾客对账单明细信息（名字账单）[DLproc_SOADetailforSearchBySel]
        /// </summary>
        /// <param name="cCusCode">顾客编码</param>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">截止日期</param>
        /// <returns></returns>
        public DataTable DLproc_SOADetailforSearchBySel(string cCusCode, string beginDate, string endDate)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_SOADetailforSearchBySel";
            SqlParameter[] paras = new SqlParameter[] { 
            new SqlParameter("@cCusCode",cCusCode), 
            new SqlParameter("@beginDate",beginDate),
            new SqlParameter("@endDate",endDate)         
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 用于顾客查询客户的欠款分析（对账单）,操作员自定义条件查询[DLproc_U8SOASearchOfOperBySel]
        /// <summary>
        /// 用于顾客查询客户的欠款分析（对账单）,操作员自定义条件查询[DLproc_U8SOASearchOfOperBySel]
        /// </summary>
        /// <param name="ccuscode">顾客编号</param>
        /// <param name="intPeriod">账单期间</param>
        /// <param name="intCheck">是否已发</param>
        /// <returns></returns>
        public DataTable DLproc_U8SOASearchOfOperBySel(string ccuscode, string intPeriod, string intCheck)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_U8SOASearchOfOperBySel";
            SqlParameter[] paras = new SqlParameter[] { 
            new SqlParameter("@ccuscode",ccuscode), 
            new SqlParameter("@intPeriod",intPeriod),
            new SqlParameter("@intCheck",intCheck)         
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 用于顾客自定义条件查询账单往来明细[DLproc_SOADetailforCustomerBySel]
        /// <summary>
        /// 用于顾客自定义条件查询账单往来明细[DLproc_SOADetailforCustomerBySel]
        /// </summary>
        ///<param name="datebegin">开始日期</param>
        /// <param name="dateend">截至日期</param>
        /// <param name="ccuscode">顾客编号</param>
        /// <returns></returns>
        public DataTable DLproc_SOADetailforCustomerBySel(string datebegin, string dateend, string ccuscode)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_SOADetailforCustomerBySel";
            SqlParameter[] paras = new SqlParameter[] { 
            new SqlParameter("@datebegin",datebegin), 
            new SqlParameter("@dateend",dateend),
            new SqlParameter("@ccuscode",ccuscode)         
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 订单执行情况明细表(按下单时间查询)[DLproc_OrderExecuteBySel]
        /// <summary>
        /// 订单执行情况明细表[DLproc_OrderExecuteBySel]
        /// </summary>
        /// <param name="strBillNo">DL网单号</param>
        /// <param name="ccuscode">顾客编码</param>
        /// <param name="begindate">开始时间</param>
        /// <param name="enddate">结束时间</param>
        /// <param name="showtype">显示明细(1)</param>
        /// <param name="FHStatus">包含未发货(1)</param>
        /// <returns></returns>
        public DataTable DLproc_OrderExecuteBySel(string strBillNo, string ccuscode, string begindate, string enddate, string showtype, string FHStatus, string Acount)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_OrderExecuteBySel";
            SqlParameter[] paras = new SqlParameter[] { 
            new SqlParameter("@strBillNo",strBillNo), 
            new SqlParameter("@ccuscode",ccuscode), 
            new SqlParameter("@begindate",begindate), 
            new SqlParameter("@enddate",enddate), 
            new SqlParameter("@showtype",showtype), 
            new SqlParameter("@FHStatus",FHStatus),
            new SqlParameter("@strAllAcount",Acount)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 订单执行情况明细表(按审核时间查询)[DLproc_OrderExecuteFordatAuditordTimeBySel]
        /// <summary>
        /// 订单执行情况明细表[DLproc_OrderExecuteFordatAuditordTimeBySel]
        /// </summary>
        /// <param name="strBillNo">DL网单号</param>
        /// <param name="ccuscode">顾客编码</param>
        /// <param name="begindate">开始时间</param>
        /// <param name="enddate">结束时间</param>
        /// <param name="showtype">显示明细(1)</param>
        /// <param name="FHStatus">包含未发货(1)</param>
        /// <returns></returns>
        public DataTable DLproc_OrderExecuteFordatAuditordTimeBySel(string strBillNo, string ccuscode, string begindate, string enddate, string showtype, string FHStatus, string Acount)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_OrderExecuteFordatAuditordTimeBySel";
            SqlParameter[] paras = new SqlParameter[] { 
            new SqlParameter("@strBillNo",strBillNo), 
            new SqlParameter("@ccuscode",ccuscode), 
            new SqlParameter("@begindate",begindate), 
            new SqlParameter("@enddate",enddate), 
            new SqlParameter("@showtype",showtype), 
            new SqlParameter("@FHStatus",FHStatus) ,
            new SqlParameter("@strAllAcount",Acount)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 更新酬宾订单的活动时间[DL_PreOrderSettingTimeByUpd]
        /// <summary>
        /// 更新酬宾订单的活动时间[DL_PreOrderSettingTimeByUpd]
        /// 01	            null	        产成品
        /// </summary>
        /// <returns></returns>
        public bool DL_PreOrderSettingTimeByUpd(string datStartTime, string datEndTime)
        {
            bool flag = false;
            string cmdText = "update Dl_opPreOrderControl set datStartTime=@datStartTime,datEndTime=@datEndTime";
            SqlParameter[] paras = new SqlParameter[] { 
            new SqlParameter("@datStartTime",datStartTime), 
            new SqlParameter("@datEndTime",datEndTime)             
           };
            int res = sqlhelper.ExecuteNonQuery(cmdText, paras, CommandType.Text);
            if (res > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion

        #region 查询酬宾订单的活动时间[DL_PreOrderSettingTimeBySel]
        /// <summary>
        /// KeyFieldName	ParentFieldName	NodeName
        /// 01	            null	        产成品
        /// </summary>
        /// <returns></returns>
        public DataTable DL_PreOrderSettingTimeBySel()
        {
            DataTable dt = new DataTable();
            string cmdText = "select * from Dl_opPreOrderControl where strcCusCode='000000'";
            // SqlParameter[] paras = new SqlParameter[] { 
            // new SqlParameter("@cCusCode",cCusCode), 
            // new SqlParameter("@lngBillType",lngBillType)             
            //};
            dt = sqlhelper.ExecuteQuery(cmdText, CommandType.Text);
            return dt;
        }
        #endregion

        #region 查询顾客登录编码(根据订单编号)[DL_cCusCodeOnOrderBillNoBySel]
        /// <summary>
        /// 查询顾客登录编码(根据订单编号)[DL_cCusCodeOnOrderBillNoBySel]
        /// </summary>
        /// <returns></returns>
        public DataTable DL_cCusCodeOnOrderBillNoBySel(string BillNo)
        {
            DataTable dt = new DataTable();
            string cmdText = "select left(isnull(ccuscode,''),6) ccuscode,lngopUserId,isnull(lngopUserExId,'0') lngopUserExId from Dl_opOrder where strBillNo=@strBillNo";
            SqlParameter[] paras = new SqlParameter[] { 
             new SqlParameter("@strBillNo",BillNo)       
            };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 特殊订单审核(审核关闭)[DL_CZTSCloseByUpd]
        /// <summary>
        /// 特殊订单审核(审核关闭)[DL_CZTSCloseByUpd]
        /// </summary>
        /// <param name="n"></param>
        /// <returns>ccuscode</returns>
        public bool DL_CZTSCloseByUpd(string BillNo)
        {
            bool flag = false;
            string cmdText = "  UPDATE dbo.Dl_opOrder SET bytStatus=96 WHERE strBillNo=@strBillNo";
            SqlParameter[] paras = new SqlParameter[] { 
             new SqlParameter("@strBillNo",BillNo)       
            };
            int res = sqlhelper.ExecuteNonQuery(cmdText, paras, CommandType.Text);
            if (res>0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion

        #region 查询顾客登录编码(根据预订单编号)[DL_cCusCodeOnPreOrderBillNoBySel]
        /// <summary>
        /// 查询顾客登录编码(根据预订单编号)[DL_cCusCodeOnPreOrderBillNoBySel]
        /// </summary>
        /// <returns></returns>
        public DataTable DL_cCusCodeOnPreOrderBillNoBySel(string BillNo)
        {
            DataTable dt = new DataTable();
            string cmdText = "select left(isnull(ccuscode,''),6) ccuscode,lngopUserId,isnull(lngopUserExId,'0') lngopUserExId from Dl_opPreOrder where strBillNo=@strBillNo";
            SqlParameter[] paras = new SqlParameter[] { 
             new SqlParameter("@strBillNo",BillNo)       
            };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 查询微信id信息[DL_GetWXCropIdBySel]
        /// <summary>
        /// 查询微信id信息[DL_GetWXCropIdBySel]
        /// </summary>
        /// <returns></returns>
        public DataTable DL_GetWXCropIdBySel()
        {
            DataTable dt = new DataTable();
            string cmdText = "select * from Dl_opSystemConfiguration";
            dt = sqlhelper.ExecuteQuery(cmdText, CommandType.Text);
            return dt;
        }
        #endregion

        #region 查询是否有该顾客信息[DL_IsExistCustomerBySel]
        /// <summary>
        /// 查询是否有该顾客信息[DL_IsExistCustomerBySel]
        /// </summary>
        /// <param name="cCusCode">登录顾客编码</param>
        /// <returns></returns>
        public DataTable DL_IsExistCustomerBySel(string cCusCode)
        {
            DataTable dt = new DataTable();
            string cmdText = "select * from dl_opuser where ccuscode like @cCusCode";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@cCusCode",cCusCode)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion


        #region 查询服务器日期,星期[T_Tdatewk]
        /// <summary>
        /// 查询服务器日期,星期
        /// </summary>
        /// <returns></returns>
        public DataTable T_Tdatewk()
        {
            DataTable dt = new DataTable();
            string cmdText = "T_Tdatewk";
            dt = sqlhelper.ExecuteQuery(cmdText, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 查询物流任务[T_Search]
        /// <summary>
        /// 查询物流任务
        /// </summary>
        /// <param name="ftable">发布范围</param>
        /// <param name="fuser">发布人</param>
        /// <param name="fdate">有效性</param>
        /// <returns></returns>
        public DataTable T_Search(string ftable, string fuser, string fdate)
        {
            DataTable dt = new DataTable();
            string cmdText = "T_Search";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@ftable",ftable),
           new SqlParameter("@fuser",fuser),
           new SqlParameter("@fdate",fdate)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 客户端查询报价[T_Search_Lsp_Bg]
        /// <summary>
        /// 客户端查询报价信息
        /// </summary>
        /// <param name="fuser">报价人</param>
        /// <param name="fsdate">开始日期</param>
        /// <param name="fedate">结束日期</param>
        /// <returns></returns>
        public DataTable T_Search_Lsp_Bg(string fuser, string fsdate, string fedate)
        {
            DataTable dt = new DataTable();
            string cmdText = "T_Search_Lsp_Bg";
            SqlParameter[] paras = new SqlParameter[] { 
            new SqlParameter("@fuser",fuser),
            new SqlParameter("@fsdate",fsdate),
            new SqlParameter("@fedate",fedate)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 取出所有可以行进修改的报价(国际)[T_NonQuotation_inter_SelectAll]
        /// <summary>
        /// 取出所有可以行进修改的报价(国际)
        /// </summary>
        /// <param name="fuser">当前用户/物流商</param>
        /// <returns></returns>
        public DataTable T_NonQuotation_inter_SelectAll(string fuser)
        {
            DataTable dt = new DataTable();
            string cmdText = "T_NonQuotation_inter_SelectAll";
            SqlParameter[] paras = new SqlParameter[] { 
            new SqlParameter("@fuser",fuser)
            };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 取出所有可以行进修改的报价(国内)[T_NonQuotation_SelectAll]
        /// <summary>
        /// 取出所有可以行进修改的报价(国内)
        /// </summary>
        /// <param name="fuser">当前用户/物流商</param>
        /// <returns></returns>
        public DataTable T_NonQuotation_SelectAll(string fuser)
        {
            DataTable dt = new DataTable();
            string cmdText = "T_NonQuotation_SelectAll";
            SqlParameter[] paras = new SqlParameter[] { 
            new SqlParameter("@fuser",fuser)
            };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 取出所有可以报价的任务,用于报价(国际)[T_TaskInterQuotation_SelectAll]
        /// <summary>
        /// 取出所有可以报价的任务,用于报价(国际)
        /// </summary>
        /// <param name="fuser">当前用户/物流商</param>
        /// <returns></returns>
        public DataTable T_TaskInterQuotation_SelectAll(string fuser)
        {
            DataTable dt = new DataTable();
            string cmdText = "T_TaskInterQuotation_SelectAll";
            SqlParameter[] paras = new SqlParameter[] { 
            new SqlParameter("@fuser",fuser)
            };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 取出所有可以报价的任务,用于报价(国内)[T_TaskQuotation_SelectAll]
        /// <summary>
        /// 取出所有可以报价的任务,用于报价(国内)
        /// </summary>
        /// <param name="fuser">当前用户/物流商</param>
        /// <returns></returns>
        public DataTable T_TaskQuotation_SelectAll(string fuser)
        {
            DataTable dt = new DataTable();
            string cmdText = "T_TaskQuotation_SelectAll";
            SqlParameter[] paras = new SqlParameter[] { 
            new SqlParameter("@fuser",fuser)
            };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

    }
}
