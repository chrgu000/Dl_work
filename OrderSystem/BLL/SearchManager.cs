/*
 *创建人：ECHO 
 *创建时间：2015-10-09
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
    public class SearchManager
    {
        private SearchDAO sdao = null;

        public SearchManager()
        {
            sdao = new SearchDAO();
        }

        #region 获取所有物料[DL_InventoryAllBySel]
        /// <summary>
        /// KeyFieldName	ParentFieldName	NodeName
        /// 01	            null	        产成品
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public DataTable DL_InventoryAllBySel()
        {
            return sdao.DL_InventoryAllBySel();
        }
        #endregion

        #region 获取物料树结构[DLproc_InventoryBySel]
        /// <summary>
        /// KeyFieldName	ParentFieldName	NodeName
        /// 01	            null	        产成品
        /// </summary>
        /// <param name="cSTCode">销售类型</param>
        /// <param name="ccuscode">顾客编码</param>
        /// <returns></returns>
        public DataTable DLproc_InventoryBySel(string cSTCode, string ccuscode)
        {
            return sdao.DLproc_InventoryBySel(cSTCode, ccuscode);
        }
        #endregion

        #region 获取物料树明细[DLproc_TreeListDetailsBySel]
        /// <summary>
        /// 获取物料树明细[DLproc_TreeListDetailsBySel]
        /// </summary>
        /// <param name="n">物料大类代码</param>
        /// <returns></returns>
        public DataTable DLproc_TreeListDetailsBySel(string cInvCCode, string cCusCode)
        {
            return sdao.DLproc_TreeListDetailsBySel(cInvCCode, cCusCode);
        }
        #endregion

        #region 获取物料树明细(全部,包含大类)[DLproc_TreeListDetailsAllBySel]
        /// <summary>
        /// 获取物料树明细(全部,包含大类)[DLproc_TreeListDetailsAllBySel]
        /// </summary>
        /// <param name="cInvCCode">物料大类代码</param>
        /// <param name="cCusCode">顾客代码</param>
        /// <returns></returns>
        public DataTable DLproc_TreeListDetailsAllBySel(string cInvCCode, string cCusCode)
        {
            return sdao.DLproc_TreeListDetailsAllBySel(cInvCCode, cCusCode);
        }
        #endregion

        #region 获取物料树明细(全部,包含大类,以及可用量)[DLproc_TreeListDetailsAll_iqty_BySel]
        /// <summary>
        /// 获取物料树明细(全部,包含大类,以及可用量)[DLproc_TreeListDetailsAll_iqty_BySel]
        /// </summary>
        /// <param name="cInvCCode">物料大类代码</param>
        /// <param name="cCusCode">顾客代码</param>
        /// <returns></returns>
        public DataTable DLproc_TreeListDetailsAll_iqty_BySel(string cInvCCode, string cCusCode)
        {
            return sdao.DLproc_TreeListDetailsAll_iqty_BySel(cInvCCode, cCusCode);
        }
        #endregion

        #region 获取物料(预订单参照生成酬宾,特殊订单)树明细[DLproc_TreeListPreDetailsBySel]
        /// <summary>
        /// 获取物料(预订单参照生成酬宾,特殊订单)树明细[DLproc_TreeListPreDetailsBySel]
        /// </summary>
        /// <param name="strBillNo">预订单编号</param>
        /// <param name="lngBillType">单据类型,1酬宾,2特殊</param>
        /// <returns></returns>
        public DataTable DLproc_TreeListPreDetailsBySel(string strBillNo, int lngBillType)
        {
            return sdao.DLproc_TreeListPreDetailsBySel(strBillNo, lngBillType);
        }
        #endregion

        #region [修改预订单]获取物料(预订单参照生成酬宾,特殊订单)树明细[DLproc_TreeListPreDetailsModifyBySel]
        /// <summary>
        /// [修改预订单]获取物料(预订单参照生成酬宾,特殊订单)树明细[DLproc_TreeListPreDetailsModifyBySel]
        /// </summary>
        /// <param name="strBillNo">预订单编号</param>
        /// <param name="lngBillType">单据类型,1酬宾,2特殊</param>
        /// <returns></returns>
        public DataTable DLproc_TreeListPreDetailsModifyBySel(string strBillNo, string OrderBillNo, int lngBillType)
        {
            return sdao.DLproc_TreeListPreDetailsModifyBySel(strBillNo, OrderBillNo, lngBillType);
        }
        #endregion

        #region 获取预订单物料树明细[DLproc_TreeListDetailsNoQTYBySel]
        /// <summary>
        /// 获取预订单物料树明细[DLproc_TreeListDetailsNoQTYBySel]
        /// </summary>
        /// <param name="n">物料大类代码</param>
        /// <returns></returns>
        public DataTable DLproc_TreeListDetailsNoQTYBySel(string cInvCCode, string cCusCode)
        {
            return sdao.DLproc_TreeListDetailsNoQTYBySel(cInvCCode, cCusCode);
        }
        #endregion

        #region 获取顾客抬头[DL_ComboCustomerBySel]
        /// <summary>
        /// 获取顾客抬头,业务员
        /// </summary>
        /// <param name="cCusCode">登录顾客编码</param>
        /// <returns></returns>
        public DataTable DL_ComboCustomerAllBySel(string cCusCode)
        {
            return sdao.DL_ComboCustomerAllBySel(cCusCode);
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
            return sdao.DL_ComboCustomerBySel(cCusCode);
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
            return sdao.DL_BillTypeBySel(StrBillNo);
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
            return sdao.DL_SOAforIdBySel(id);
        }
        #endregion

        #region 获取顾客关联的已完成(所有)系统订单编号[DL_ComboCustomerU8NOBySel]
        /// <summary>
        /// 获取顾客关联的已完成(所有)系统订单编号[DL_ComboCustomerU8NOBySel]
        /// </summary>
        /// <param name="cCusCode">登录顾客编码</param>
        /// <returns></returns>
        public DataTable DL_ComboCustomerU8NOBySel(string cCusCode)
        {
            return sdao.DL_ComboCustomerU8NOBySel(cCusCode);
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
            return sdao.DL_HR_CT007BySel();
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
            return sdao.DL_PreOrder();
        }
        #endregion

        #region 获取预订单树结构[DL_PreOrderTreeBySel]
        /// <summary>
        /// KeyFieldName	ParentFieldName	NodeName
        /// 01	            null	        产成品
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public DataTable DL_PreOrderTreeBySel(string cCusCode, string lngopUserId, string lngopUserExId, int lngBillType)
        {
            return sdao.DL_PreOrderTreeBySel(cCusCode, lngopUserId, lngopUserExId, lngBillType);
        }
        #endregion

        #region 获取顾客对账单信息[DLproc_U8SOASearchBySel]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strComboPeriodYear"></param>
        /// <param name="cCusCode"></param>
        /// <param name="intPeriod"></param>
        /// <param name="intCheck"></param>
        /// <returns></returns>
        public DataTable DLproc_U8SOASearchBySel(string strComboPeriodYear, string cCusCode, string intPeriod, string intCheck, string strconccuscode)
        {
            return sdao.DLproc_U8SOASearchBySel(strComboPeriodYear, cCusCode, intPeriod, intCheck, strconccuscode);
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
            return sdao.DLproc_SOADetailforSearchBySel(cCusCode, beginDate, endDate);
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
            return sdao.DLproc_U8SOASearchOfOperBySel(ccuscode, intPeriod, intCheck);
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
            return sdao.DLproc_SOADetailforCustomerBySel(datebegin, dateend, ccuscode);
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
            return sdao.DLproc_OrderExecuteBySel(strBillNo, ccuscode, begindate, enddate, showtype, FHStatus, Acount);
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
            return sdao.DLproc_OrderExecuteFordatAuditordTimeBySel(strBillNo, ccuscode, begindate, enddate, showtype, FHStatus, Acount);
        }
        #endregion

        #region 更新酬宾订单的活动时间[DL_PreOrderSettingTimeByUpd]
        /// <summary>
        /// 更新酬宾订单的活动时间[DL_PreOrderSettingTimeByUpd]
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public bool DL_PreOrderSettingTimeByUpd(string datStartTime, string datEndTime)
        {
            return sdao.DL_PreOrderSettingTimeByUpd(datStartTime, datEndTime);
        }
        #endregion

        #region 查询酬宾订单的活动时间[DL_PreOrderSettingTimeBySel]
        /// <summary>
        /// 查询酬宾订单的活动时间[DL_PreOrderSettingTimeBySel]
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public DataTable DL_PreOrderSettingTimeBySel()
        {
            return sdao.DL_PreOrderSettingTimeBySel();
        }
        #endregion

        #region 查询顾客登录编码(根据订单编号)[DL_cCusCodeOnOrderBillNoBySel]
        /// <summary>
        /// 查询顾客登录编码(根据订单编号)[DL_cCusCodeOnOrderBillNoBySel]
        /// </summary>
        /// <param name="n"></param>
        /// <returns>ccuscode</returns>
        public DataTable DL_cCusCodeOnOrderBillNoBySel(string BillNo)
        {
            return sdao.DL_cCusCodeOnOrderBillNoBySel(BillNo);
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
            return sdao.DL_CZTSCloseByUpd(BillNo);
        }
        #endregion

        #region 查询顾客登录编码(根据预订单编号)[DL_cCusCodeOnPreOrderBillNoBySel]
        /// <summary>
        /// 查询顾客登录编码(根据预订单编号)[DL_cCusCodeOnPreOrderBillNoBySel]
        /// </summary>
        /// <param name="n"></param>
        /// <returns>ccuscode</returns>
        public DataTable DL_cCusCodeOnPreOrderBillNoBySel(string BillNo)
        {
            return sdao.DL_cCusCodeOnPreOrderBillNoBySel(BillNo);
        }
        #endregion

        #region 查询微信id信息[DL_GetWXCropIdBySel]
        /// <summary>
        /// 查询微信id信息[DL_GetWXCropIdBySel]
        /// </summary>
        /// <returns></returns>
        public DataTable DL_GetWXCropIdBySel()
        {
            return sdao.DL_GetWXCropIdBySel();
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
            return sdao.DL_IsExistCustomerBySel(cCusCode);
        }
        #endregion



        /////////////////////////////////////////////////////////////////////----------------------------------------------------------------------------------------
        #region 获取服务器的日期,星期[T_Tdatewk]
        /// <summary>
        /// 获取服务器的日期,星期
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public DataTable T_Tdatewk()
        {
            return sdao.T_Tdatewk();
        }
        #endregion



    }
}
