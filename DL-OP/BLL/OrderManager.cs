/*
 *创建人：ECHO 
 *创建时间：2015-10-23
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
    public class OrderManager
    {
        private OrderDAO odao = null;

        public OrderManager()
        {
            odao = new OrderDAO();
        }

        #region 新增加订单表头[DLproc_NewOrderByIns]
        /// <summary>
        /// 新增加订单表头[DLproc_NewOrderByIns]
        /// </summary>
        /// <param name="od">参数组</param>
        /// <returns></returns>
        public DataTable DLproc_NewOrderByIns(OrderInfo oi)
        {
            return odao.DLproc_NewOrderByIns(oi);
        }
        #endregion

        #region 新增加订单表头(样品资料)[DLproc_NewSampleOrderByIns]
        /// <summary>
        /// 新增加订单表头(样品资料)[DLproc_NewSampleOrderByIns]
        /// </summary>
        /// <param name="strBillNo">关联订单编号</param>
        /// <returns></returns>
        public DataTable DLproc_NewSampleOrderByIns(string strBillNo, string strRemarks, string strLoadingWays, string lngopUserExId, string strAllAcount)
        {
            return odao.DLproc_NewSampleOrderByIns(strBillNo, strRemarks, strLoadingWays, lngopUserExId, strAllAcount);
        }
        #endregion

        #region 新增加订单表头(预订单参照生成)[DLproc_NewYYOrderByIns]
        /// <summary>
        /// 新增加订单表头[DLproc_NewYYOrderByIns]
        /// </summary>
        /// <param name="od">参数组</param>
        /// <returns></returns>
        public DataTable DLproc_NewYYOrderByIns(OrderInfo oi)
        {
            return odao.DLproc_NewYYOrderByIns(oi);
        }
        #endregion

        #region 新增加订单表头(预订单)[DLproc_NewPreOrderByIns]
        /// <summary>
        /// 新增加订单表头[DLproc_NewOrderByIns]
        /// </summary>
        /// <param name="od">参数组</param>
        /// <returns></returns>
        public DataTable DLproc_NewPreOrderByIns(OrderInfo oi)
        {
            return odao.DLproc_NewPreOrderByIns(oi);
        }
        #endregion

        #region 新增加订单表体[DLproc_NewOrderDetailByIns]
        /// <summary>
        /// 新增加订单表体[DLproc_NewOrderDetailByIns]
        /// </summary>
        /// <param name="od">参数组</param>
        /// <returns></returns>
        public bool DLproc_NewOrderDetailByIns(OrderInfo oi)
        {
            return odao.DLproc_NewOrderDetailByIns(oi);
        }
        #endregion

        #region 新增加订单表体(预订单参照生成)[DLproc_NewYYOrderDetailByIns]
        /// <summary>
        /// 新增加订单表体(预订单参照生成)[DLproc_NewYYOrderDetailByIns]
        /// </summary>
        /// <param name="od">参数组</param>
        /// <returns></returns>
        public bool DLproc_NewYYOrderDetailByIns(OrderInfo oi)
        {
            return odao.DLproc_NewYYOrderDetailByIns(oi);
        }
        #endregion

        #region 获取订单(未审核)[DLproc_UnauditedOrderBySel]
        /// <summary>
        /// 获取订单(未审核)[DLproc_UnauditedOrderBySel]
        /// </summary>
        /// <param name="bytStatus">状态</param>
        /// <returns></returns>
        public DataTable DLproc_UnauditedOrderBySel(int bytStatus)
        {
            return odao.DLproc_UnauditedOrderBySel(bytStatus);
        }
        #endregion

        #region 获取订单(未分配,操作员)[DLproc_UnauditedOrderManagersBySel]
        /// <summary>
        /// 获取订单(未分配,操作员)[DLproc_UnauditedOrderManagersBySel]
        /// </summary>
        /// <param name="strManagers">操作员</param>
        /// <returns></returns>
        public DataTable DLproc_UnauditedOrderManagersBySel(string strManagers, int lngBillType)
        {
            return odao.DLproc_UnauditedOrderManagersBySel(strManagers, lngBillType);
        }
        #endregion

        #region 获取订单(未分配,操作员,发运方式)[DLproc_UnauditedOrderManagersBySel]
        /// <summary>
        /// 获取订单(未分配,操作员,发运方式)[DLproc_UnauditedOrderManagersBySel]
        /// </summary>
        /// <param name="strManagers">操作员</param>
        /// <returns></returns>
        public DataTable DLproc_UnauditedOrderManagers_U20BySel(string strManagers, int lngBillType, string cSCCode)
        {
            return odao.DLproc_UnauditedOrderManagers_U20BySel(strManagers, lngBillType, cSCCode);
        }
        #endregion

        #region 审核销售订单时，获取网上订单的销售订单的数据[DLproc_NewOrderU8BySel]
        /// <summary>
        /// 审核销售订单时，获取网上订单的销售订单的数据[DLproc_NewOrderU8BySel]
        /// </summary>
        /// <param name="strBillNo">DL订单编号</param>
        /// <returns></returns>
        public DataTable DLproc_NewOrderU8BySel(string strBillNo)
        {
            return odao.DLproc_NewOrderU8BySel(strBillNo);
        }
        #endregion

        #region 获取审核的订单号最近的订单号，用于定位(未分配,操作员,发运方式,审核的订单号)[DLproc_UnauditedOrderManagers_U20_strbillnoBySel]
        /// <summary>
        /// 获取审核的订单号最近的订单号，用于定位[DLproc_UnauditedOrderManagers_U20_strbillnoBySel]
        /// </summary>
        /// <param name="strManagers">操作员</param>
        /// <returns></returns>
        public DataTable DLproc_UnauditedOrderManagers_U20_strbillnoBySel(string strManagers, int lngBillType, string cSCCode, string strbillno)
        {
            return odao.DLproc_UnauditedOrderManagers_U20_strbillnoBySel(strManagers, lngBillType, cSCCode, strbillno);
        }
        #endregion

        #region 更新订单驳回备注说明[DL_OrderRejRemarksByUpdl]
        /// <summary>
        /// 更新订单驳回备注说明[DL_OrderRejRemarksByUpdl]
        /// </summary>
        /// <param name="strBillNo">订单编号</param>
        /// <param name="strRejectRemarks">备注说明</param>
        /// <returns></returns>
        public bool DL_OrderRejRemarksByUpdl(string strBillNo, string strRejectRemarks)
        {
            return odao.DL_OrderRejRemarksByUpdl(strBillNo, strRejectRemarks);
        }
        #endregion

        #region 获取预订单(未分配,操作员)[DLproc_UnauditedPreOrderManagersBySel]
        /// <summary>
        /// 获取预订单(未分配,操作员)[DLproc_UnauditedPreOrderManagersBySel]
        /// </summary>
        /// <param name="strManagers">操作员</param>
        /// <returns></returns>
        public DataTable DLproc_UnauditedPreOrderManagersBySel(string strManagers, int lngBillType)
        {
            return odao.DLproc_UnauditedPreOrderManagersBySel(strManagers, lngBillType);
        }
        #endregion

        #region 获取预订单(审核状态,单据类型)[DLproc_UnauditedpreOrderBySel]
        /// <summary>
        /// 获取预订单(审核状态,单据类型)[DLproc_UnauditedpreOrderBySel]
        /// </summary>
        /// <param name="bytStatus">状态</param>
        /// <returns></returns>
        public DataTable DLproc_UnauditedpreOrderBySel(int bytStatus, int lngBillType)
        {
            return odao.DLproc_UnauditedpreOrderBySel(bytStatus, lngBillType);
        }
        #endregion

        #region 更新未审核订单状态[DL_OrderStatusByUp]
        /// <summary>
        /// 更新未审核订单状态[DL_OrderStatusByUp]
        /// </summary>
        /// <param name="od">参数组</param>
        /// <returns></returns>
        public bool DL_OrderStatusByUp(string cSOCode, string strBillNo)
        {
            return odao.DL_OrderStatusByUp(cSOCode, strBillNo);
        }
        #endregion

        #region 生成U8订单表头[DLproc_NewOrderU8ByIns]
        /// <summary>
        /// 生成U8订单表头[DLproc_NewOrderU8ByIns]
        /// </summary>
        /// <param name="strBillNo"></param>
        /// <returns></returns>
        public bool DLproc_NewOrderU8ByIns(string strBillNo)
        {
            return odao.DLproc_NewOrderU8ByIns(strBillNo);
        }
        #endregion

        #region 调用U8api生成销售订单之后更新相关订单数据并且更新网上订单状态[DLproc_NewOrderU8APIByUpd]
        /// <summary>
        /// 调用U8api生成销售订单之后更新相关订单数据并且更新网上订单状态[DLproc_NewOrderU8APIByUpd]
        /// </summary>
        /// <param name="strBillNo"></param>
        /// <returns></returns>
        public bool DLproc_NewOrderU8APIByUpd(string strBillNo)
        {
            return odao.DLproc_NewOrderU8APIByUpd(strBillNo);
        }
        #endregion

        #region 生成U8订单表头[DLproc_NewYOrderU8ByIns]
        /// <summary>
        /// 生成U8预订单表头[DLproc_NewOrderU8ByIns]
        /// </summary>
        /// <param name="strBillNo"></param>
        /// <returns></returns>
        public bool DLproc_NewYOrderU8ByIns(string strBillNo)
        {
            return odao.DLproc_NewYOrderU8ByIns(strBillNo);
        }
        #endregion

        #region 生成U8特殊订单表头[DLproc_NewYOrderU8_TSByIns]
        /// <summary>
        /// 生成U8特殊订单表头[DLproc_NewYOrderU8_TSByIns]
        /// </summary>
        /// <param name="strBillNo"></param>
        /// <returns></returns>
        public bool DLproc_NewYOrderU8_TSByIns(string strBillNo)
        {
            return odao.DLproc_NewYOrderU8_TSByIns(strBillNo);
        }
        #endregion

        #region 生成U8订单表体[DLproc_NewOrderDetailU8ByIns]
        /// <summary>
        /// 生成U8订单表体[DLproc_NewOrderDetailU8ByIns]
        /// </summary>
        /// <param name="od">参数组</param>
        /// <returns></returns>
        public bool DLproc_NewOrderDetailU8ByIns()
        {
            return odao.DLproc_NewOrderDetailU8ByIns();
        }
        #endregion

        #region 用于查询左边菜单传过来的物料详情 [DLproc_QuasiOrderDetailBySel]
        /// <summary>
        /// 用于查询左边菜单传过来的物料详情 [DLproc_QuasiOrderDetailBySel]
        /// </summary>
        /// <param name="cinvcode">存货编码</param>
        /// <returns></returns>
        public DataTable DLproc_QuasiOrderDetailBySel(string cinvcode, string ccuscode)
        {
            return odao.DLproc_QuasiOrderDetailBySel(cinvcode, ccuscode);
        }
        #endregion

        #region 用于查询左边菜单传过来的物料详情 [DLproc_QuasiOrderDetail_TSBySel]
        /// <summary>
        /// 用于查询左边菜单传过来的物料详情 [DLproc_QuasiOrderDetail_TSBySel]
        /// </summary>
        /// <param name="cinvcode">存货编码</param>
        /// <returns></returns>
        public DataTable DLproc_QuasiOrderDetail_TSBySel(string cinvcode, string ccuscode)
        {
            return odao.DLproc_QuasiOrderDetail_TSBySel(cinvcode, ccuscode);
        }
        #endregion

        #region 用于查询该物料是否允限销 [DLproc_cInvCodeIsBeLimitedBySel]
        /// <summary>
        /// 用于查询该物料是否允限销 [DL_cInvCodeIsPLPBySel]
        /// </summary>
        /// <param name="cinvcode">存货编码</param>
        /// <param name="ccuscode">客户编码</param>
        /// <param name="iShowType">单据类型，1普通，2特殊</param>
        /// <returns></returns>
        public bool DLproc_cInvCodeIsBeLimitedBySel(string cinvcode, string ccuscode, int iShowType)
        {
            return odao.DLproc_cInvCodeIsBeLimitedBySel(cinvcode, ccuscode, iShowType);
        }
        #endregion

        #region 检测顾客名称和编码是否一致[DL_CheckCuscodeAndCusnameBySel]
        /// <summary>
        ///  检测顾客名称和编码是否一致[DL_CheckCuscodeAndCusnameBySel]
        /// </summary>
        /// <param name="ccuscode">顾客编码</param>
        /// <param name="ccusname">顾客名称</param>
        /// <returns></returns>
        public DataTable DL_CheckCuscodeAndCusnameBySel(string ccuscode, string ccusname)
        {
            return odao.DL_CheckCuscodeAndCusnameBySel(ccuscode, ccusname);
        }
        #endregion

        #region 用于查询修改的订单的商品的库存可用量详情 [DLproc_QuasiOrderDetailModifyBySel]
        /// <summary>
        /// 用于查询修改的订单的商品的库存可用量详情 [DLproc_QuasiOrderDetailModifyBySel]
        /// </summary>
        /// <param name="cinvcode">存货编码</param>
        /// <param name="cCusCode">存货编码</param>
        /// <param name="strBillNo">存货编码</param>
        /// <returns></returns>
        public DataTable DLproc_QuasiOrderDetailModifyBySel(string cinvcode, string cCusCode, string strBillNo)
        {
            return odao.DLproc_QuasiOrderDetailModifyBySel(cinvcode, cCusCode, strBillNo);
        }
        #endregion

        #region 用于用户修改订单时,比较库存是否可用 [DLproc_OrderDetailModifyStockQtyCompareBySel]
        /// <summary>
        /// 用于用户修改订单时,比较库存是否可用 [DLproc_OrderDetailModifyStockQtyCompareBySel]
        /// </summary>
        /// <param name="cinvcode">存货编码</param>
        /// <param name="strBillNo">单据编号</param>
        /// <returns></returns>
        public DataTable DLproc_OrderDetailModifyStockQtyCompareBySel(string cinvcode, string strBillNo)
        {
            return odao.DLproc_OrderDetailModifyStockQtyCompareBySel(cinvcode, strBillNo);
        }
        #endregion

        #region 用于查询参照预订单选择的物料详情 [DLproc_QuasiYOrderDetailBySel]
        /// <summary>
        /// 用于查询参照预订单选择的物料详情 [DLproc_QuasiYOrderDetailBySel]
        /// </summary>
        /// <param name="cinvcode">存货编码</param>
        /// <param name="cpreordercode">预订单号</param>
        /// <returns></returns>
        public DataTable DLproc_QuasiYOrderDetailBySel(string cinvcode, string cpreordercode)
        {
            return odao.DLproc_QuasiYOrderDetailBySel(cinvcode, cpreordercode);
        }
        #endregion

        #region 用于查询参照预订单选择的物料详情(参照特殊订单使用，不考虑LP件) [DLproc_QuasiYOrderDetail_TSBySel]
        /// <summary>
        /// 用于查询参照预订单选择的物料详情(参照特殊订单使用，不考虑LP件) [DLproc_QuasiYOrderDetail_TSBySel]
        /// </summary>
        /// <param name="cinvcode">存货编码</param>
        /// <param name="cpreordercode">预订单号</param>
        /// <returns></returns>
        public DataTable DLproc_QuasiYOrderDetail_TSBySel(string cinvcode, string cpreordercode, string cArea)
        {
            return odao.DLproc_QuasiYOrderDetail_TSBySel(cinvcode, cpreordercode, cArea);
        }
        #endregion

        #region 获取顾客剩余信用额度[DLproc_getCusCreditInfo]
        /// <summary>
        /// 获取顾客剩余信用额度[DLproc_getCusCreditInfo]
        /// </summary>
        /// <param name="ccuscode">顾客编码</param>
        /// <returns></returns>
        public DataTable DLproc_getCusCreditInfo(string ccuscode)
        {
            return odao.DLproc_getCusCreditInfo(ccuscode);
        }
        #endregion

        #region 获取顾客剩余信用额度(修改状态,需传入单据号,扣除其金额)[DLproc_getCusCreditInfoWithBillno]
        /// <summary>
        /// 获取顾客剩余信用额度(修改状态,需传入单据号,扣除其金额)[DLproc_getCusCreditInfoWithBillno]
        /// </summary>
        /// <param name="ccuscode">顾客编码</param>
        /// <param name="strBillNo">单据编码</param>
        /// <returns></returns>
        public DataTable DLproc_getCusCreditInfoWithBillno(string ccuscode, string strBillNo)
        {
            return odao.DLproc_getCusCreditInfoWithBillno(ccuscode, strBillNo);
        }
        #endregion

        #region 获取顾客预订单剩余信用额度[DLproc_getPreOrderCusCreditInfo]
        /// <summary>
        /// 获取顾客预订单剩余信用额度[DLproc_getPreOrderCusCreditInfo]
        /// </summary>
        /// <param name="ccuscode">顾客编码</param>
        /// <returns></returns>
        public DataTable DLproc_getPreOrderCusCreditInfo(string ccuscode, int lngBillType)
        {
            return odao.DLproc_getPreOrderCusCreditInfo(ccuscode, lngBillType);
        }
        #endregion

        #region 驳回提交的订单[DL_RejectOrderBillByUpd]
        /// <summary>
        /// 驳回提交的订单
        /// </summary>
        /// <param name="strBillNo">DL单据号</param>
        /// <returns></returns>
        public bool DL_RejectOrderBillByUpd(string strBillNo, string strManagers)
        {
            return odao.DL_RejectOrderBillByUpd(strBillNo, strManagers);
        }
        #endregion

        #region 驳回提交的订单（标记3小时过期,写入到期时间）[DL_RejectOrderBillForExpTimeByUpd]
        /// <summary>
        /// 驳回提交的订单
        /// </summary>
        /// <param name="strBillNo">DL单据号</param>
        /// <returns></returns>
        public bool DL_RejectOrderBillForExpTimeByUpd(string strBillNo, string strManagers, int datExpTime)
        {
            return odao.DL_RejectOrderBillForExpTimeByUpd(strBillNo, strManagers, datExpTime);
        }
        #endregion

        #region 驳回提交的预订单[DL_RejectYOrderBillByUpd]
        /// <summary>
        /// 驳回提交的预订单[DL_RejectYOrderBillByUpd]
        /// </summary>
        /// <param name="strBillNo">DL单据号</param>
        /// <returns></returns>
        public bool DL_RejectYOrderBillByUpd(string strBillNo, string strManagers)
        {
            return odao.DL_RejectYOrderBillByUpd(strBillNo, strManagers);
        }
        #endregion

        #region 作废提交的预订单[DL_InvalidYOrderBillByUpd]
        /// <summary>
        /// 作废提交的预订单[DL_InvalidYOrderBillByUpd]
        /// </summary>
        /// <param name="strBillNo">DL单据号</param>
        /// <returns></returns>
        public bool DL_InvalidYOrderBillByUpd(string strBillNo, string strManagers, string InvalidPersonCode)
        {
            return odao.DL_InvalidYOrderBillByUpd(strBillNo, strManagers, InvalidPersonCode);
        }
        #endregion

        #region 查询订单(DL编号,单据类型)[DL_OrderBillBySel]
        /// <summary>
        /// 查询订单(DL编号)[DL_OrderBillBySel]
        /// </summary>
        /// <param name="strBillNo">DL单据号</param>
        /// <returns></returns>
        public DataTable DL_OrderBillBySel(string strBillNo, int lngBillType)
        {
            return odao.DL_OrderBillBySel(strBillNo, lngBillType);
        }
        #endregion

        #region 查询订单(返回ID,类型:1是order表中的订单,2是preorder表中的订单)[DLproc_OrderBillBySel]
        /// <summary>
        /// 查询订单(返回ID,类型:1是order表中的订单,2是preorder表中的订单)[DLproc_OrderBillBySel]
        /// </summary>
        /// <param name="strBillNo">DL单据号</param>
        /// <returns></returns>
        public DataTable DLproc_OrderBillBySel(string strBillNo, int lngBillType)
        {
            return odao.DLproc_OrderBillBySel(strBillNo, lngBillType);
        }
        #endregion

        #region 查询订单(DL编号)[DL_OrderBillBySel]
        /// <summary>
        /// 查询订单(DL编号)[DL_OrderBillBySel]
        /// </summary>
        /// <param name="strBillNo">DL单据号</param>
        /// <returns></returns>
        public DataTable DL_OrderBillBySel(string strBillNo)
        {
            return odao.DL_OrderBillBySel(strBillNo);
        }
        #endregion

        #region 查询U8发货单订单(“FHD”+U8发货单编号)[DL_U8FHDOrderBillBySel]
        /// <summary>
        /// 查询U8发货单订单(“FHD”+U8发货单编号)[DL_U8FHDOrderBillBySel]
        /// </summary>
        /// <param name="strBillNo">“FHD”+U8发货单编号</param>
        /// <returns></returns>
        public DataTable DL_U8FHDOrderBillBySel(string strBillNo)
        {
            return odao.DL_U8FHDOrderBillBySel(strBillNo);
        }
        #endregion

        #region 更新手工在U8下的订单的已确认(cCusOAddress)字段值(U8编号)[DL_U8OrderBillConfirmByUpd]
        /// <summary>
        /// 更新手工在U8下的订单的已确认(cCusOAddress)字段值(U8编号)[DL_U8OrderBillConfirmByUpd]
        /// </summary>
        /// <param name="strBillNo">U8单据号</param>
        /// <returns></returns>
        public bool DL_U8OrderBillConfirmByUpd(string strBillNo)
        {
            return odao.DL_U8OrderBillConfirmByUpd(strBillNo);
        }
        #endregion

        #region 更新手工在U8下的发货单的已确认字段值(U8发货单编号)[DL_U8FHDOrderBillConfirmByUpd]
        /// <summary>
        /// 更新手工在U8下的发货单的已确认字段值(U8发货单编号)[DL_U8FHDOrderBillConfirmByUpd]
        /// </summary>
        /// <param name="strBillNo">U8发货单单据号</param>
        /// <returns></returns>
        public bool DL_U8FHDOrderBillConfirmByUpd(string strBillNo)
        {
            return odao.DL_U8FHDOrderBillConfirmByUpd(strBillNo);
        }
        #endregion

        #region 查询订单(U8编号)[DL_U8OrderBillBySel]
        /// <summary>
        /// 查询订单(U8编号)[DL_U8OrderBillBySel]
        /// </summary>
        /// <param name="strBillNo">U8单据号</param>
        /// <returns></returns>
        public DataTable DL_U8OrderBillBySel(string strBillNo)
        {
            return odao.DL_U8OrderBillBySel(strBillNo);
        }
        #endregion

        #region 查询预订单(DL编号)[DL_PreOrderBillBySel]
        /// <summary>
        /// 查询预订单(DL编号)[DL_OrderBillBySel]
        /// </summary>
        /// <param name="strBillNo">DL单据号</param>
        /// <returns></returns>
        public DataTable DL_PreOrderBillBySel(string strBillNo)
        {
            return odao.DL_PreOrderBillBySel(strBillNo);
        }
        #endregion

        #region 审核提交的订单[DL_CheckOrderBillByUpd]
        /// <summary>
        /// 驳回提交的订单
        /// </summary>
        /// <param name="strBillNo">DL单据号</param>
        /// <returns></returns>
        public bool DL_CheckOrderBillByUpd(string strBillNo)
        {
            return odao.DL_CheckOrderBillByUpd(strBillNo);
        }
        #endregion

        #region 作废订单[DL_InvalidOrderByUpd]
        /// <summary>
        /// 作废订单[DL_InvalidOrderByUpd]
        /// </summary>
        /// <param name="strBillNo">DL单据号</param>
        /// <returns></returns>
        public bool DL_InvalidOrderByUpd(string strBillNo, string lngopUserId)
        {
            return odao.DL_InvalidOrderByUpd(strBillNo, lngopUserId);
        }
        #endregion

        #region 变更订单状态(顾客确认界面中将订单变更为修改状态,即被驳回状态)[DL_ModifyOrderByUpd]
        /// <summary>
        /// 变更订单状态(顾客确认界面中将订单变更为修改状态,即被驳回状态)[DL_ModifyOrderByUpd]
        /// </summary>
        /// <param name="strBillNo">DL单据号</param>
        /// <returns></returns>
        public bool DL_ModifyOrderByUpd(string strBillNo)
        {
            return odao.DL_ModifyOrderByUpd(strBillNo);
        }
        #endregion

        #region 获取订单表体信息(单据状态,客户)[DL_DisposeOrderBySel]
        /// <summary>
        /// 获取订单表体信息(单据状态,客户)[DL_DisposeOrderBySel]
        /// </summary>
        /// <param name="bytStatus">单据状态</param>
        /// <param name="ccuscode">客户编码</param>
        /// <returns></returns>
        public DataTable DL_DisposeOrderBySel(int bytStatus, string ccuscode)
        {
            return odao.DL_DisposeOrderBySel(bytStatus, ccuscode);
        }
        #endregion

        #region 获取订单表体表头(单据状态,客户)[DL_UnauditedOrderBySel]
        /// <summary>
        /// 获取订单表体信息(单据状态,客户)[DL_UnauditedOrderBySel]
        /// </summary>
        /// <param name="bytStatus">单据状态</param>
        /// <param name="ccuscode">客户编码</param>
        /// <returns></returns>
        public DataTable DL_UnauditedOrderBySel(int bytStatus, string ccuscode)
        {
            return odao.DL_UnauditedOrderBySel(bytStatus, ccuscode);
        }
        #endregion

        #region 获取订单表头信息【待审核用】(状态bytStatus,客户编码,)[DL_UnauditedOrder_DSH_BySel]
        /// <summary>
        /// 获取订单表头信息【待审核用】(状态bytStatus,客户编码,)[DL_UnauditedOrder_DSH_BySel]
        /// </summary>
        /// <param name="bytStatus">单据状态</param>
        /// <param name="ccuscode">客户编码</param>
        /// <returns></returns>
        public DataTable DL_UnauditedOrder_DSH_BySel(int bytStatus, string ccuscode)
        {
            return odao.DL_UnauditedOrder_DSH_BySel(bytStatus, ccuscode);
        }
        #endregion

        #region 获取订单表头信息(状态bytStatus,客户id,子账户id)[DL_UnauditedOrder_SubBySel]
        /// <summary>
        /// 获取订单表头信息(状态bytStatus,客户id,子账户id)[DL_UnauditedOrder_SubBySel]
        /// </summary>
        /// <param name="bytStatus">单据状态</param>
        /// <param name="lngopUserId">主用户id</param>
        /// <param name="lngopUserExId">子账户id</param>
        /// <returns></returns>
        public DataTable DL_UnauditedOrder_SubBySel(int bytStatus, string lngopUserId, string lngopUserExId)
        {
            return odao.DL_UnauditedOrder_SubBySel(bytStatus, lngopUserId, lngopUserExId);
        }
        #endregion

        #region 获取预订单表体表头(单据状态,客户)[DL_UnauditedPreOrderBySel]
        /// <summary>
        /// 获取预订单表体信息(单据状态,客户)[DL_UnauditedPreOrderBySel]
        /// </summary>
        /// <param name="bytStatus">单据状态</param>
        /// <param name="ccuscode">客户编码</param>
        /// <returns></returns>
        public DataTable DL_UnauditedPreOrderBySel(int bytStatus, string ccuscode)
        {
            return odao.DL_UnauditedPreOrderBySel(bytStatus, ccuscode);
        }
        #endregion

        #region 查询订单表头数据,修改专用(DL编号)[DL_OrderModifyBySel]
        /// <summary>
        /// 查询订单表头数据,修改专用(DL编号)[DL_OrderModifyBySel]
        /// </summary>
        /// <param name="strBillNo">DL单据号</param>
        /// <returns></returns>
        public DataTable DL_OrderModifyBySel(string strBillNo)
        {
            return odao.DL_OrderModifyBySel(strBillNo);
        }
        #endregion

        #region 查询订单表体数据,修改专用(DL编号)[DLproc_OrderDetailModifyBySel]
        /// <summary>
        /// 查询订单表体数据,修改专用(DL编号)[DLproc_OrderDetailModifyBySel]
        /// </summary>
        /// <param name="strBillNo">DL单据号</param>
        /// <returns></returns>
        public DataTable DLproc_OrderDetailModifyBySel(string strBillNo)
        {
            return odao.DLproc_OrderDetailModifyBySel(strBillNo);
        }
        #endregion

        #region 查询参照订单表体数据,修改参照订单专用,读取原订单数据(DL编号)[DLproc_OrderYDetailModifyBySel]
        /// <summary>
        /// 查询参照订单表体数据,修改参照订单专用,读取原订单数据(DL编号)[DLproc_OrderYDetailModifyBySel]
        /// </summary>
        /// <param name="strBillNo">DL单据号</param>
        /// <returns></returns>
        public DataTable DLproc_OrderYDetailModifyBySel(string strBillNo)
        {
            return odao.DLproc_OrderYDetailModifyBySel(strBillNo);
        }
        #endregion

        #region 修改更新订单表头[DLproc_NewOrderByUpd]
        /// <summary>
        /// 修改更新订单表头[DLproc_NewOrderByUpd]
        /// </summary>
        /// <param name="od">参数组</param>
        /// <returns></returns>
        public DataTable DLproc_NewOrderByUpd(OrderInfo oi)
        {
            return odao.DLproc_NewOrderByUpd(oi);
        }
        #endregion

        #region 修改更新订单表头(酬宾订单/特殊订单))[DLproc_NewYOrderByUpd]
        /// <summary>
        /// 修改更新订单表头(酬宾订单/特殊订单))[DLproc_NewYOrderByUpd]
        /// </summary>
        /// <param name="od">参数组</param>
        /// <returns></returns>
        public DataTable DLproc_NewYOrderByUpd(OrderInfo oi)
        {
            return odao.DLproc_NewYOrderByUpd(oi);
        }
        #endregion

        #region 修改更新订单表体[DLproc_NewOrderDetailByUpd]
        /// <summary>
        /// 新增加订单表体[DLproc_NewOrderDetailByUpd]
        /// </summary>
        /// <param name="od">参数组</param>
        /// <returns></returns>
        public bool DLproc_NewOrderDetailByUpd(OrderInfo oi)
        {
            return odao.DLproc_NewOrderDetailByUpd(oi);
        }
        #endregion

        #region 修改更新订单表头(样品资料)[DLproc_SampleOrderByUpd]
        /// <summary>
        /// 修改更新订单表头(样品资料)[DLproc_SampleOrderByUpd]
        /// </summary>
        /// <param name="od">参数组</param>
        /// <returns></returns>
        public DataTable DLproc_SampleOrderByUpd(OrderInfo oi)
        {
            return odao.DLproc_SampleOrderByUpd(oi);
        }
        #endregion

        #region 修改更新订单表体(删除老数据)[DL_NewOrderDetailByDel]
        /// <summary>
        /// 修改更新订单表体(删除老数据)[DL_NewOrderDetailByDel]
        /// </summary>
        /// <param name="lngopOrderId">订单表头id</param>
        /// <returns></returns>
        public bool DL_NewOrderDetailByDel(int lngopOrderId)
        {
            return odao.DL_NewOrderDetailByDel(lngopOrderId);
        }
        #endregion

        #region 检测订单专属操作员[DL_ManagersOrderBillBySel]
        /// <summary>
        /// 检测订单专属操作员[DL_ManagersOrderBillBySel]
        /// </summary>
        /// <param name="strBillNo">DL单据号</param>
        /// <returns></returns>
        public DataTable DL_ManagersOrderBillBySel(string strBillNo)
        {
            return odao.DL_ManagersOrderBillBySel(strBillNo);
        }
        #endregion

        #region 检测预订单专属操作员[DL_ManagersPreOrderBillBySel]
        /// <summary>
        /// 检测订单专属操作员[DL_ManagersOrderBillBySel]
        /// </summary>
        /// <param name="strBillNo">DL单据号</param>
        /// <returns></returns>
        public DataTable DL_ManagersPreOrderBillBySel(string strBillNo)
        {
            return odao.DL_ManagersPreOrderBillBySel(strBillNo);
        }
        #endregion

        #region 绑定订单专属操作员[DL_ManagersOrderBillByUpd]
        /// <summary>
        /// 绑定订单专属操作员[DL_ManagersOrderBillByUpd]
        /// </summary>
        /// <param name="strBillNo">DL单据号</param>
        /// <returns></returns>
        public bool DL_ManagersOrderBillByUpd(string strBillNo, string strManagers)
        {
            return odao.DL_ManagersOrderBillByUpd(strBillNo, strManagers);
        }
        #endregion

        #region 绑定预订单专属操作员[DL_ManagersPreOrderBillByUpd]
        /// <summary>
        /// 绑定预订单专属操作员[DL_ManagersPreOrderBillByUpd]
        /// </summary>
        /// <param name="strBillNo">DL单据号</param>
        /// <returns></returns>
        public bool DL_ManagersPreOrderBillByUpd(string strBillNo, string strManagers)
        {
            return odao.DL_ManagersPreOrderBillByUpd(strBillNo, strManagers);
        }
        #endregion

        #region 用于查询我的工作详情[DLproc_MyWorkBySel]
        /// <summary>
        /// 用于查询我的工作详情[DLproc_MyWorkBySel]
        /// </summary>
        /// <param name="strBillNo">DL单据号</param>
        /// <returns></returns>
        public DataTable DLproc_MyWorkBySel(string strBillNo, string beginDate, string EndDate, string cSTCode, int bytStatus, string lngopUserId)
        {
            return odao.DLproc_MyWorkBySel(strBillNo, beginDate, EndDate, cSTCode, bytStatus, lngopUserId);
        }
        #endregion

        #region 用于查询顾客的酬宾(特殊)订单详情[DLproc_MyWorkPreYOrderForCustomerBySel]
        /// <summary>
        /// 用于查询顾客的酬宾(特殊)订单详情[DLproc_MyWorkPreYOrderForCustomerBySel]
        /// </summary>
        /// <param name="strBillNo">DL单据号</param>
        /// <returns></returns>
        public DataTable DLproc_MyWorkPreYOrderForCustomerBySel(string strBillNo, string beginDate, string endDate, int bytStatus, string lngopUserId, string lngBillType)
        {
            return odao.DLproc_MyWorkPreYOrderForCustomerBySel(strBillNo, beginDate, endDate, bytStatus, lngopUserId, lngBillType);
        }
        #endregion

        #region 用于查询我的工作详情(预订单)[DLproc_MyWorkPreOrderBySel]
        /// <summary>
        /// 用于查询我的工作详情[DLproc_MyWorkBySel]
        /// </summary>
        /// <param name="strBillNo">DL单据号</param>
        /// <returns></returns>
        public DataTable DLproc_MyWorkPreOrderBySel(string strBillNo, string beginDate, string EndDate, int bytStatus, string lngopUserId)
        {
            return odao.DLproc_MyWorkPreOrderBySel(strBillNo, beginDate, EndDate, bytStatus, lngopUserId);
        }
        #endregion

        #region 用于查询是否临时授权信用额度[DL_ExtraCreditBySel]
        /// <summary>
        /// 用于查询是否临时授权信用额度[DL_ExtraCreditBySel]
        /// </summary>
        /// <param name="ccuscode">开票单位编码</param>
        /// <returns></returns>
        public DataTable DL_ExtraCreditBySel(string ccuscode)
        {
            return odao.DL_ExtraCreditBySel(ccuscode);
        }
        #endregion

        #region 查询订单(DL编号,用于样品资料自动填写内容)[DL_OrderBillYPZLBySel]
        /// <summary>
        /// 查询订单(DL编号,用于样品资料自动填写内容)[DL_OrderBillYPZLBySel]
        /// </summary>
        /// <param name="strBillNo">DL单据号</param>
        /// <returns></returns>
        public DataTable DL_OrderBillYPZLBySel(string strBillNo)
        {
            return odao.DL_OrderBillYPZLBySel(strBillNo);
        }
        #endregion

        #region 顾客历史订单[DL_PreviousOrderBySel]
        /// <summary>
        /// 顾客历史订单[DL_PreviousOrderBySel]
        /// </summary>
        /// <param name="ccuscode">顾客大类编号(登录code)</param>
        /// <returns></returns>
        public DataTable DL_PreviousOrderBySel(string ccuscode, string beginDate, string endDate)
        {
            return odao.DL_PreviousOrderBySel(ccuscode, beginDate, endDate);
        }
        #endregion

        #region 顾客历史订单[DLproc_PreviousOrderBySel]
        /// <summary>
        /// 顾客历史订单[DLproc_PreviousOrderBySel]
        /// </summary>
        /// <param name="ccuscode">顾客大类编号(登录code)</param>
        /// <returns></returns>
        public DataTable DLproc_PreviousOrderBySel(string ccuscode, string beginDate, string endDate)
        {
            return odao.DLproc_PreviousOrderBySel(ccuscode, beginDate, endDate);
        }
        #endregion

        #region 顾客历史订单（V2.0，包含物流信息）[DL_PreviousOrder_LOGBySel]
        /// <summary>
        /// 顾客历史订单（V2.0，包含物流信息）[DL_PreviousOrder_LOGBySel]
        /// </summary>
        /// <param name="ccuscode">顾客大类编号(登录code)</param>
        /// <returns></returns>
        public DataTable DL_PreviousOrder_LOGBySel(string ccuscode, string beginDate, string endDate)
        {
            return odao.DL_PreviousOrder_LOGBySel(ccuscode, beginDate, endDate);
        }
        #endregion

        #region 顾客历史订单(普通订单)[DL_GeneralPreviousOrderBySel]
        /// <summary>
        /// 顾客历史订单(普通订单)[DL_GeneralPreviousOrderBySel]
        /// </summary>
        /// <param name="ccuscode">顾客大类编号(登录code%)</param>
        /// <returns></returns>
        public DataTable DL_GeneralPreviousOrderBySel(string ccuscode)
        {
            return odao.DL_GeneralPreviousOrderBySel(ccuscode);
        }
        #endregion

        #region 顾客查询历史作废订单[DL_PreviousInvalidOrderBySel]
        /// <summary>
        /// 顾客查询历史作废订单[DL_PreviousInvalidOrderBySel]
        /// </summary>
        /// <param name="ccuscode">顾客大类编号(登录code)</param>
        /// <returns></returns>
        public DataTable DL_PreviousInvalidOrderBySel(string ccuscode, string beginDate, string endDate)
        {
            return odao.DL_PreviousInvalidOrderBySel(ccuscode, beginDate, endDate);
        }
        #endregion

        #region 查询U8订单[DL_OrderU8BillBySel]
        /// <summary>
        /// 查询订单(U8编号)[DL_OrderU8BillBySel]
        /// </summary>
        /// <param name="strU8BillNo">U8单据号</param>
        /// <returns></returns>
        public DataTable DL_OrderU8BillBySel(string strU8BillNo)
        {
            return odao.DL_OrderU8BillBySel(strU8BillNo);
        }
        #endregion

        #region 查询历史订单中的参照订单数据[DL_OrderCZTSBillBySel]
        /// <summary>
        /// 查询历史订单中的参照订单数据[DL_OrderCZTSBillBySel]
        /// </summary>
        /// <param name="strU8BillNo">U8单据号</param>
        /// <returns></returns>
        public DataTable DL_OrderCZTSBillBySel(string strU8BillNo)
        {
            return odao.DL_OrderCZTSBillBySel(strU8BillNo);
        }
        #endregion

        #region 新增加酬宾预订单表头[DLproc_NewYOrderByIns]
        /// <summary>
        /// 新增加酬宾预订单表头[DLproc_NewYOrderByIns]
        /// </summary>
        /// <param name="ddate"></param>
        /// <param name="lngopUserId"></param>
        /// <param name="bytStatus"></param>
        /// <param name="ccuscode"></param>
        /// <param name="ccusname"></param>
        /// <param name="lngBillType"></param>
        /// <param name="cdiscountname"></param>
        /// <param name="cMemo"></param>
        /// <param name="lngopUserExId"></param>
        /// <param name="strAllAcount"></param>
        /// <returns></returns>
        public DataTable DLproc_NewYOrderByIns(string ddate, string lngopUserId, int bytStatus, string ccuscode, string ccusname, int lngBillType, string cdiscountname, string cMemo, string lngopUserExId, string strAllAcount)
        {
            return odao.DLproc_NewYOrderByIns(ddate, lngopUserId, bytStatus, ccuscode, ccusname, lngBillType, cdiscountname, cMemo, lngopUserExId, strAllAcount);
        }
        #endregion

        #region 新增加酬宾预订单表体[DLproc_NewYOrderDetailByIns]
        /// <summary>
        /// 新增加酬宾预订单表体[DLproc_NewYOrderDetailByIns]
        /// </summary>
        /// <param name="od">参数组</param>
        /// <returns></returns>
        public bool DLproc_NewYOrderDetailByIns(OrderInfo oi)
        {
            return odao.DLproc_NewYOrderDetailByIns(oi);
        }
        #endregion

        #region 新增(发送)顾客对账单[DL_NewSOAByIns]
        /// <summary>
        /// 新增顾客对账单[DL_NewSOAByIns]
        /// </summary>
        /// <param name="od">参数组</param>
        /// <returns></returns>
        public bool DL_NewSOAByIns(string ccuscode, string ccusname, string strEndDate, double dblAmount, string strUper, string strOper, string strOperName, int intPeriod)
        {
            return odao.DL_NewSOAByIns(ccuscode, ccusname, strEndDate, dblAmount, strUper, strOper, strOperName, intPeriod);
        }
        #endregion

        #region 顾客确认对账单[DL_ConfimSOAByUpd]
        /// <summary>
        /// 顾客确认对账单[DL_ConfimSOAByUpd]
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public bool DL_ConfimSOAByUpd(string id)
        {
            return odao.DL_ConfimSOAByUpd(id);
        }
        #endregion

        #region 返回酬宾订单控制参数(活动是否开启)[DL_PreOrderSettingBySel]
        /// <summary>
        /// 返回酬宾订单控制参数(活动是否开启)[DL_PreOrderSettingBySel]
        /// </summary>
        /// <returns></returns>
        public bool DL_PreOrderSettingBySel(string cCusCode)
        {
            return odao.DL_PreOrderSettingBySel(cCusCode);
        }
        #endregion

        #region 获取对应的U8订单编号,用于发送给顾客[DL_OrderBillNoForU8OrderBillNoByIns]
        /// <summary>
        /// 获取对应的U8订单编号,用于发送给顾客[DL_OrderBillNoForU8OrderBillNoByIns]
        /// </summary>
        /// <param name="strBillNo">网单号</param>
        /// <returns></returns>
        public DataTable DL_OrderBillNoForU8OrderBillNoByIns(string strBillNo)
        {
            return odao.DL_OrderBillNoForU8OrderBillNoByIns(strBillNo);
        }
        #endregion

        #region 提交订单前查询价格变化 [DLproc_QuasiPriceBySel]
        /// <summary>
        /// 提交订单前查询价格变化 [DLproc_QuasiPriceBySel]
        /// </summary>
        /// <param name="cinvcode">存货编码</param>
        /// <param name="ccuscode">开票单位</param>
        /// <returns></returns>
        public DataTable DLproc_QuasiPriceBySel(string cinvcode, string ccuscode)
        {
            return odao.DLproc_QuasiPriceBySel(cinvcode, ccuscode);
        }
        #endregion

        #region 获取当前时间是否在有效时间范围内 [DL_IsValidTimeBySel]
        /// <summary>
        /// 获取当前时间是否在有效时间范围内 [DL_IsValidTimeBySel]
        /// </summary>
        /// <param name="kpdw">开票单位编码</param>
        /// <returns></returns>
        public bool DL_IsValidTimeBySel(string kpdw)
        {
            return odao.DL_IsValidTimeBySel(kpdw);
        }
        #endregion

        #region 获取当前时间是否在有效时间范围内的数据 [DL_InValidTimeDataBySel]
        /// <summary>
        /// 获取当前时间是否在有效时间范围内的数据 [DL_InValidTimeDataBySel]
        /// </summary>
        /// <param name="kpdw">开票单位编码</param>
        /// <returns></returns>
        public DataTable DL_InValidTimeDataBySel(string kpdw)
        {
            return odao.DL_InValidTimeDataBySel(kpdw);
        }
        #endregion

        #region 重复购买(普通订单)[DL_BuyAgainBySel]
        /// <summary>
        /// 重复购买(普通订单)[DL_BuyAgainBySel]
        /// </summary>
        /// <param name="strBillNo">网单号</param>
        /// <returns></returns>
        public DataTable DL_BuyAgainBySel(string strBillNo)
        {
            return odao.DL_BuyAgainBySel(strBillNo);
        }
        #endregion

        #region 重复购买(普通订单)[DLproc_ReferencePreviousOrderWithCusInvLimitedBySel]
        /// <summary>
        /// 重复购买(普通订单)[DLproc_ReferencePreviousOrderWithCusInvLimitedBySel]
        /// </summary>
        /// <param name="strBillNo">网单号</param>
        /// <returns></returns>
        public DataTable DLproc_ReferencePreviousOrderWithCusInvLimitedBySel(string strBillNo)
        {
            return odao.DLproc_ReferencePreviousOrderWithCusInvLimitedBySel(strBillNo);
        }
        #endregion

        #region 判断样品订单关联的主订单是否审核通过[DL_YPForMainOrderStatusBySel]
        /// <summary>
        /// 判断样品订单关联的主订单是否审核通过[DL_YPForMainOrderStatusBySel]
        /// </summary>
        /// <param name="strBillNo">网单号</param>
        /// <returns></returns>
        public bool DL_YPForMainOrderStatusBySel(string strBillNo)
        {
            return odao.DL_YPForMainOrderStatusBySel(strBillNo);
        }
        #endregion

        #region 更新顾客允限销分类表[DL_CusCodeClassByUp]
        /// <summary>
        /// 更新顾客允限销分类表[DL_CusCodeClassByUp]
        /// </summary>
        /// <param name="ccuscode">顾客编码</param>
        /// <returns></returns>
        public bool DL_CusCodeClassByUp(string ccuscode)
        {
            return odao.DL_CusCodeClassByUp(ccuscode);
        }
        #endregion

        #region 查询活动内容表数据[DL_opSysSalesPolicy_LimitedSupplyBySel]
        /// <summary>
        /// 查询活动内容表数据[DL_opSysSalesPolicy_LimitedSupplyBySel]
        /// </summary>
        /// <returns></returns>
        public DataTable DL_opSysSalesPolicy_LimitedSupplyBySel()
        {
            return odao.DL_opSysSalesPolicy_LimitedSupplyBySel();
        }
        #endregion

        #region 更新所有允限销分类表[DL_CodeClassByUp]
        /// <summary>
        /// 更新所有允限销分类表[DL_CodeClassByUp]
        /// </summary>
        /// <returns></returns>
        public bool DL_CodeClassByUp()
        {
            return odao.DL_CodeClassByUp();
        }
        #endregion

        #region 新增加临时订单表头,返回表头id[DLproc_AddOrderBackByIns]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngopUserId"></param>
        /// <param name="strBillName"></param>
        /// <param name="bytStatus"></param>
        /// <param name="strRemarks"></param>
        /// <param name="ccuscode"></param>
        /// <param name="cdefine1"></param>
        /// <param name="cdefine2"></param>
        /// <param name="cdefine3"></param>
        /// <param name="cdefine8"></param>
        /// <param name="cdefine9"></param>
        /// <param name="cdefine10"></param>
        /// <param name="cdefine11"></param>
        /// <param name="cdefine12"></param>
        /// <param name="cdefine13"></param>
        /// <param name="ccusname"></param>
        /// <param name="cpersoncode"></param>
        /// <param name="cSCCode"></param>
        /// <param name="strLoadingWays"></param>
        /// <param name="cSTCode"></param>
        /// <param name="lngopUseraddressId"></param>
        /// <param name="RelateU8NO"></param>
        /// <param name="lngBillType"></param>
        /// <returns></returns>
        public DataTable DLproc_AddOrderBackByIns(string lngopUserId, string strBillName, int bytStatus, string strRemarks, string ccuscode, string cdefine1, string cdefine2, string cdefine3, string cdefine9, string cdefine10, string cdefine11, string cdefine12, string cdefine13, string ccusname, string cpersoncode, string cSCCode, string strLoadingWays, string cSTCode, string lngopUseraddressId, string RelateU8NO, int lngBillType)
        {
            return odao.DLproc_AddOrderBackByIns(lngopUserId, strBillName, bytStatus, strRemarks, ccuscode, cdefine1, cdefine2, cdefine3, cdefine9, cdefine10, cdefine11, cdefine12, cdefine13, ccusname, cpersoncode, cSCCode, strLoadingWays, cSTCode, lngopUseraddressId, RelateU8NO, lngBillType);
        }
        #endregion

        #region 新增加临时订单表头[DLproc_AddOrderBackDetailByIns]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngopOrderBackId"></param>
        /// <param name="cinvcode"></param>
        /// <param name="cComUnitQTY"></param>
        /// <param name="cInvDefine1QTY"></param>
        /// <param name="cInvDefine2QTY"></param>
        /// <param name="cInvDefineQTY"></param>
        /// <param name="pack"></param>
        /// <returns></returns>
        public bool DLproc_AddOrderBackDetailByIns(int lngopOrderBackId, string cinvcode, string cinvname, double cComUnitQTY, double cInvDefine1QTY, double cInvDefine2QTY, double cInvDefineQTY, double cComUnitAmount, string pack, string irowno)
        {
            return odao.DLproc_AddOrderBackDetailByIns(lngopOrderBackId, cinvcode, cinvname, cComUnitQTY, cInvDefine1QTY, cInvDefine2QTY, cInvDefineQTY, cComUnitAmount, pack, irowno);
        }
        #endregion

        #region 获取顾客对应的临时订单列表[DL_GetOrderBackBySel]
        /// <summary>
        /// 获取顾客对应的临时订单列表[DL_GetOrderBackBySel]
        /// </summary>
        /// <param name="lngopUserId">顾客登录帐号内码</param>
        /// <returns></returns>
        public DataTable DL_GetOrderBackBySel(int lngopUserId)
        {
            return odao.DL_GetOrderBackBySel(lngopUserId);
        }
        #endregion

        #region 获取顾客对应的临时订单明细[DLproc_ReferenceOrderBackWithCusInvLimitedBySel]
        /// <summary>
        /// 获取顾客对应的临时订单明细[DLproc_ReferenceOrderBackWithCusInvLimitedBySel]
        /// </summary>
        /// <param name="lngopOrderBackId">临时订单内码</param>
        /// <returns></returns>
        public DataTable DLproc_ReferenceOrderBackWithCusInvLimitedBySel(int lngopOrderBackId)
        {
            return odao.DLproc_ReferenceOrderBackWithCusInvLimitedBySel(lngopOrderBackId);
        }
        #endregion

        #region 获取顾客对应的临时订单或者历史订单明细（V2.0）[DLproc_BackOrderandPrvOrdercInvCodeIsBeLimitedBySel]
        /// <summary>
        /// 获取顾客对应的临时订单或者历史订单明细（V2.0）[DLproc_BackOrderandPrvOrdercInvCodeIsBeLimitedBySel]
        /// </summary>
        /// <param name="@id ">订单内码</param>
        /// <param name="@iShowType">1，普通订单，2，特殊订单</param>
        /// <param name="@iBillType">1，临时订单，2，历史订单</param>
        /// <returns></returns>
        public DataTable DLproc_BackOrderandPrvOrdercInvCodeIsBeLimitedBySel(string id, int iShowType, int iBillType)
        {
            return odao.DLproc_BackOrderandPrvOrdercInvCodeIsBeLimitedBySel(id, iShowType, iBillType);
        }
        #endregion

        #region 删除临时订单信息[DL_DelOrderBackDetailByDel]
        /// <summary>
        ///  删除临时订单信息[DL_DelOrderBackDetailByDel]
        /// </summary>
        /// <param name="lngopOrderBackId"></param>
        /// <returns></returns>
        public bool DL_DelOrderBackDetailByDel(int lngopOrderBackId)
        {
            return odao.DL_DelOrderBackDetailByDel(lngopOrderBackId);
        }
        #endregion

        #region 删除自动保存的临时订单信息[DLproc_DelAutoSaveOrderBackByDel]
        /// <summary>
        /// 删除自动保存的临时订单信息[DLproc_DelAutoSaveOrderBackByDel]
        /// </summary>
        /// <returns></returns>
        public bool DLproc_DelAutoSaveOrderBackByDel(int lngopUserId)
        {
            return odao.DLproc_DelAutoSaveOrderBackByDel(lngopUserId);
        }
        #endregion

        #region 查询特殊订单的明细信息[DL_XOrderBillDetailBySel]
        /// <summary>
        /// 查询特殊订单的明细信息[DL_XOrderBillDetailBySel]
        /// </summary>
        /// <param name="strBillNo">特殊订单网单号</param>
        /// <returns></returns>
        public DataTable DL_XOrderBillDetailBySel(string strBillNo)
        {
            return odao.DL_XOrderBillDetailBySel(strBillNo);
        }
        #endregion

        #region 驳回提交的订单(顾客自取回，在接单员接收处理之前)[DL_RejectOrderBillSelfByUpd]
        /// <summary>
        /// 驳回提交的订单(顾客自取回，在接单员接收处理之前)[DL_RejectOrderBillSelfByUpd]
        /// </summary>
        /// <param name="strBillNo">DL单据号</param>
        /// <returns></returns>
        public bool DL_RejectOrderBillSelfByUpd(string strBillNo, string strManagers)
        {
            return odao.DL_RejectOrderBillSelfByUpd(strBillNo, strManagers);
        }
        #endregion

        #region 查询当月前未确认的订单,查询未确认的延期欠款通知单（如不确认则不能下单）[DL_NotConfirmedSOABySel]
        /// <summary>
        /// 查询当月前未确认的订单,查询未确认的延期欠款通知单（如不确认则不能下单）[DL_NotConfirmedSOABySel]
        /// </summary>
        /// <param name="ccuscod">客户登录代码</param>
        /// <returns></returns>
        public DataTable DL_NotConfirmedSOABySel(string ccuscod)
        {
            return odao.DL_NotConfirmedSOABySel(ccuscod);
        }
        #endregion

        #region 查询当前是否开启订单开放时间管理[DL_OrderENTimeControlBySel]
        /// <summary>
        /// 查询当前是否开启订单开放时间管理[DL_OrderENTimeControlBySel]
        /// </summary>
        /// <returns></returns>
        public DataTable DL_OrderENTimeControlBySel()
        {
            return odao.DL_OrderENTimeControlBySel();
        }
        #endregion

        #region 查询是否存在未参照完的存货，返回数量[DLproc_PerOrderCinvcodeLeftBySel]
        /// <summary>
        /// 查询是否存在未参照完的存货，返回数量[DLproc_PerOrderCinvcodeLeftBySel]
        /// </summary>
        /// <param name="lngopUserExId"></param>
        /// <param name="lngopUserId"></param>
        /// <param name="cinvcode"></param>
        /// <returns></returns>
        public DataTable DLproc_PerOrderCinvcodeLeftBySel(string lngopUserExId, string lngopUserId, string ccuscode, string cinvcode)
        {
            return odao.DLproc_PerOrderCinvcodeLeftBySel(lngopUserExId, lngopUserId, ccuscode, cinvcode);
        }
        #endregion

        #region 用于用户提交参照预订单时检查库存和可用量 [DLproc_PreOrderSubmitForCheckBySel]
        /// <summary>
        /// 用于用户提交参照预订单时检查库存和可用量 [DLproc_PreOrderSubmitForCheckBySel]
        /// </summary>
        /// <param name="PreOrderCheck"></param>
        /// <returns></returns>
        public DataTable DLproc_PreOrderSubmitForCheckBySel(DataTable PreOrderCheck)
        {
            return odao.DLproc_PreOrderSubmitForCheckBySel(PreOrderCheck);
        }
        #endregion

        #region 查询当前订单是否存在过期[DL_OrderIsExpBySel]
        /// <summary>
        /// 查询当前订单是否存在过期[DL_OrderIsExpBySel]
        /// </summary>
        /// <returns></returns>
        public bool DL_OrderIsExpBySel(string strBillNo)
        {
            return odao.DL_OrderIsExpBySel(strBillNo);
        }
        #endregion

        #region 查询U8订单数据-表头[DL_U8OrderDataBySel]
        /// <summary>
        /// 查询U8订单数据-表头[DL_U8OrderDataBySel]
        /// </summary>
        /// <returns></returns>
        public DataTable DL_U8OrderDataBySel(string strBillNo)
        {
            return odao.DL_U8OrderDataBySel(strBillNo);
        }
        #endregion

        #region 查询参照特殊订单查询-表头[DL_NewOrderToDispHU8BySel]
        /// <summary>
        /// 查询参照特殊订单查询[DL_NewOrderToDispHU8BySel]
        /// </summary>
        /// <returns></returns>
        public DataTable DL_NewOrderToDispHU8BySel(string strBillNo)
        {
            return odao.DL_NewOrderToDispHU8BySel(strBillNo);
        }
        #endregion

        #region 查询参照特殊订单查询-表体-v2.0传参区分仓库[DL_NewOrderToDispU8_V2BySel]
        /// <summary>
        /// 查询参照特殊订单查询-表体-v2.0传参区分仓库[DL_NewOrderToDispU8_V2BySel]
        /// </summary>
        /// <returns></returns>
        public DataTable DL_NewOrderToDispU8_V2BySel(string strBillNo,int itype)
        {
            return odao.DL_NewOrderToDispU8_V2BySel(strBillNo, itype);
        }
        #endregion

        #region 查询参照特殊订单查询-表体-非金花,大井[DL_NewOrderToDispBU8BySel]
        /// <summary>
        /// 查询参照特殊订单查询-表体-非金花[DL_NewOrderToDispBU8BySel]
        /// </summary>
        /// <returns></returns>
        public DataTable DL_NewOrderToDispBU8BySel(string strBillNo)
        {
            return odao.DL_NewOrderToDispBU8BySel(strBillNo);
        }
        #endregion

        #region 查询参照特殊订单查询-表体-金花[DL_NewOrderToDispB_JH_U8BySel]
        /// <summary>
        /// 查询参照特殊订单查询-表体-金花[DL_NewOrderToDispB_JH_U8BySel]
        /// </summary>
        /// <returns></returns>
        public DataTable DL_NewOrderToDispB_JH_U8BySel(string strBillNo)
        {
            return odao.DL_NewOrderToDispB_JH_U8BySel(strBillNo);
        }
        #endregion

        #region 查询参照特殊订单查询-表体-大井[DL_NewOrderToDispB_DJ_U8BySel]
        /// <summary>
        /// 查询参照特殊订单查询-表体-大井[DL_NewOrderToDispB_DJ_U8BySel]
        /// </summary>
        /// <returns></returns>
        public DataTable DL_NewOrderToDispB_DJ_U8BySel(string strBillNo)
        {
            return odao.DL_NewOrderToDispB_DJ_U8BySel(strBillNo);
        }
        #endregion

        #region 更新审核的参照特殊订单生成的U8发货单的流水号[DLproc_CZTSFHDLSHByUpd]
        /// <summary>
        /// 更新审核的参照特殊订单生成的U8发货单的流水号[DLproc_CZTSFHDLSHByUpd]
        /// </summary>
        /// <returns></returns>
        public bool DLproc_CZTSFHDLSHByUpd(string strBillNo)
        {
            return odao.DLproc_CZTSFHDLSHByUpd(strBillNo);
        }
        #endregion

        #region 更新网上订单的参照订单状态为已审核[DL_CZTSOrderAuthByUpd]
        /// <summary>
        /// 更新网上订单的参照订单状态为已审核[DL_CZTSOrderAuthByUpd]
        /// </summary>
        /// <returns></returns>
        public bool DL_CZTSOrderAuthByUpd(string strBillNo)
        {
            return odao.DL_CZTSOrderAuthByUpd(strBillNo);
        }
        #endregion

        #region 查询是否已经存在对应的DL的W订单,并且更新[DL_IsExistsDLBySel]
        /// <summary>
        /// 查询是否已经存在对应的DL的W订单[DL_IsExistsDLBySel],并且更新,true存在，false不存在,如果存在则更新网上订单的U8销售订单号
        /// </summary>
        /// <returns></returns>
        public bool DL_IsExistsDLBySel(string strBillNo)
        {
            return odao.DL_IsExistsDLBySel(strBillNo);
        }
        #endregion

        #region 写入错误信息[DL_ErrByIns]
        /// <summary>
        /// 写入错误信息[DL_ErrByIns]
        /// </summary>
        /// <returns></returns>
        public bool DL_ErrByIns(string strBillNo,string Err)
        {
            return odao.DL_ErrByIns(strBillNo,Err);
        }
        #endregion

        #region 事务测试1[DLproc_shiwutest1]
        /// <summary>
        /// 事务测试1[DLproc_shiwutest1]
        /// </summary>
        /// <returns></returns>
        public bool DLproc_shiwutest1()
        {
            return odao.DLproc_shiwutest1();
        }
        #endregion

        #region 事务测试2[DLproc_shiwutest2]
        /// <summary>
        /// 事务测试2[DLproc_shiwutest2]
        /// </summary>
        /// <returns></returns>
        public bool DLproc_shiwutest2()
        {
            return odao.DLproc_shiwutest2();
        }
        #endregion

        #region 查询网上订单特殊订单表数据（包含表头，扩展表头，表体）[DLproc_NewYOrderU8_TSBySel]
        /// <summary>
        /// 查询网上订单特殊订单表数据（包含表头，扩展表头，表体）[DLproc_NewYOrderU8_TSBySel]
        /// </summary>
        /// <returns></returns>
        public DataTable DLproc_NewYOrderU8_TSBySel(string strBillNo)
        {
            return odao.DLproc_NewYOrderU8_TSBySel(strBillNo);
        }
        #endregion

        #region 删除U8预订单[DLproc_YOrderU8_TSByDel]
        /// <summary>
        /// 删除U8预订单[DLproc_YOrderU8_TSByDel]
        /// </summary>
        /// <returns></returns>
        public bool DLproc_YOrderU8_TSByDel(string strBillNo)
        {
            return odao.DLproc_YOrderU8_TSByDel(strBillNo);
        }
        #endregion

        #region 查询API日志[DL_LogBySel]
        /// <summary>
        /// 查询API日志[DL_LogBySel]
        /// </summary>
        /// <returns></returns>
        public DataTable DL_LogBySel()
        {
            return odao.DL_LogBySel();
        }
        #endregion

        #region 获取单据编号[DLproc_GetBillNoForData]
        /// <summary>
        /// 获取单据编号[DLproc_GetBillNoForData]
        /// </summary>
        /// <returns></returns>
        public DataTable DLproc_GetBillNoForData(string strBillType)
        {
            return odao.DLproc_GetBillNoForData(strBillType);
        }
        #endregion

        #region 重算订单整单税额[DLproc_SHLByUpd]
        /// <summary>
        /// 重算订单整单税额[DLproc_SHLByUpd]
        /// </summary>
        /// <returns></returns>
        public bool DLproc_SHLByUpd(string strBillNo )
        {
            return odao.DLproc_SHLByUpd(strBillNo);
        }
        #endregion

        #region 查询是否已经存在对应U8销售订单[DL_SO_IsExistsBySel]
        /// <summary>
        /// 查询是否已经存在对应U8销售订单[DL_SO_IsExistsBySel]
        /// </summary>
        /// <returns></returns>
        public bool DL_SO_IsExistsBySel(string cSoCode)
        {
            return odao.DL_SO_IsExistsBySel(cSoCode);
        }
        #endregion

    }
}
