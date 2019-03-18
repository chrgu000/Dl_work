/*
 *创建人：ECHO 
 *创建时间：2015-10-23
 *说明：订单相关操作类
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
{   /// <summary>
    /// 订单相关操作类
    /// </summary>
    public class OrderDAO
    {
        private SQLHelper sqlhelper = null;

        public OrderDAO()
        {
            sqlhelper = new SQLHelper();
        }

        #region 新增订单表头[DLproc_NewOrderByIns]
        public DataTable DLproc_NewOrderByIns(OrderInfo oi, string areaid, string iaddresstype, string chdefine21,string cWhCode)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_NewOrder_All_Warehouse_ByIns";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@lngopUserId",oi.LngopUserId),
            new SqlParameter("@datCreateTime",oi.DatCreateTime),
            new SqlParameter("@bytStatus",oi.BytStatus),
            new SqlParameter("@strRemarks",oi.StrRemarks),
            new SqlParameter("@ccuscode",oi.Ccuscode),
            new SqlParameter("@cdefine1",oi.Cdefine1),
            new SqlParameter("@cdefine2",oi.Cdefine2),
            new SqlParameter("@cdefine3",oi.Cdefine3),
            new SqlParameter("@cdefine9",oi.Cdefine9),
            new SqlParameter("@cdefine10",oi.Cdefine10),
            new SqlParameter("@cdefine11",oi.Cdefine11),
            new SqlParameter("@cdefine12",oi.Cdefine12),
            new SqlParameter("@cdefine13",oi.Cdefine13),
            new SqlParameter("@ccusname",oi.Ccusname),
            new SqlParameter("@cpersoncode",oi.Cpersoncode),
            new SqlParameter("@cSCCode",oi.CSCCode),
            new SqlParameter("@datDeliveryDate",oi.DatDeliveryDate),
            new SqlParameter("@strLoadingWays",oi.StrLoadingWays),
            new SqlParameter("@cSTCode",oi.CSTCode),
            new SqlParameter("@lngopUseraddressId",oi.LngopUseraddressId),
            new SqlParameter("@strU8BillNo",oi.StrU8BillNo),
            new SqlParameter("@cdiscountname",oi.Cdiscountname),
            new SqlParameter("@cdefine8",oi.Cdefine8), 
            new SqlParameter("@lngopUserExId",oi.LngopUserExId),
            new SqlParameter("@strAllAcount",oi.StrAllAcount),
            new SqlParameter("@chdefine49",areaid),
            new  SqlParameter("@iaddresstype",iaddresstype),
            new SqlParameter("@chdefine21",chdefine21),
            new SqlParameter("@chdefine51",cWhCode)  //仓库编码
            };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 新增加订单表头(样品资料)[DLproc_NewSampleOrderByIns]
        public DataTable DLproc_NewSampleOrderByIns(string strBillNo, string strRemarks, string strLoadingWays, string lngopUserExId, string strAllAcount)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_NewSampleOrderByIns";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@strU8BillNo",strBillNo),
            new SqlParameter("@strRemarks",strRemarks),
            new SqlParameter("@strLoadingWays",strLoadingWays),
            new SqlParameter("@lngopUserExId",lngopUserExId),
            new SqlParameter("@strAllAcount",strAllAcount)
            };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 新增订单表头(预订单参照生成)[DLproc_NewYYOrderByIns]
        public DataTable DLproc_NewYYOrderByIns(OrderInfo oi, string areaid, string iaddresstype, string chdefine21)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_NewYYOrderByIns";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@lngopUserId",oi.LngopUserId),
            new SqlParameter("@datCreateTime",oi.DatCreateTime),
            new SqlParameter("@bytStatus",oi.BytStatus),
            new SqlParameter("@strRemarks",oi.StrRemarks),
            new SqlParameter("@ccuscode",oi.Ccuscode),
            new SqlParameter("@cdefine1",oi.Cdefine1),
            new SqlParameter("@cdefine2",oi.Cdefine2),
            new SqlParameter("@cdefine3",oi.Cdefine3),
            new SqlParameter("@cdefine9",oi.Cdefine9),
            new SqlParameter("@cdefine10",oi.Cdefine10),
            new SqlParameter("@cdefine11",oi.Cdefine11),
            new SqlParameter("@cdefine12",oi.Cdefine12),
            new SqlParameter("@cdefine13",oi.Cdefine13),
            new SqlParameter("@ccusname",oi.Ccusname),
            new SqlParameter("@cpersoncode",oi.Cpersoncode),
            new SqlParameter("@cSCCode",oi.CSCCode),
            new SqlParameter("@datDeliveryDate",oi.DatDeliveryDate),
            new SqlParameter("@strLoadingWays",oi.StrLoadingWays),
            new SqlParameter("@cSTCode",oi.CSTCode),
            new SqlParameter("@lngopUseraddressId",oi.LngopUseraddressId),
            new SqlParameter("@strU8BillNo",oi.StrU8BillNo),
            new SqlParameter("@lngBillType",oi.LngBillType),
            new SqlParameter("@cdefine8",oi.Cdefine8),
            new SqlParameter("@lngopUserExId",oi.LngopUserExId),
            new SqlParameter("@strAllAcount",oi.StrAllAcount),
            new SqlParameter("@chdefine49",areaid),
            new SqlParameter("@iaddresstype",iaddresstype),
            new SqlParameter("@chdefine21",chdefine21)

            };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 新增订单表头(预订单)[DLproc_NewPreOrderByIns]
        public DataTable DLproc_NewPreOrderByIns(OrderInfo oi)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_NewOrderByIns";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@lngopUserId",oi.LngopUserId),
            new SqlParameter("@datCreateTime",oi.DatCreateTime),
            new SqlParameter("@bytStatus",oi.BytStatus),
            new SqlParameter("@strRemarks",oi.StrRemarks),
            new SqlParameter("@ccuscode",oi.Ccuscode),
            new SqlParameter("@cdefine1",oi.Cdefine1),
            new SqlParameter("@cdefine2",oi.Cdefine2),
            new SqlParameter("@cdefine3",oi.Cdefine3),
            new SqlParameter("@cdefine9",oi.Cdefine9),
            new SqlParameter("@cdefine10",oi.Cdefine10),
            new SqlParameter("@cdefine11",oi.Cdefine11),
            new SqlParameter("@cdefine12",oi.Cdefine12),
            new SqlParameter("@cdefine13",oi.Cdefine13),
            new SqlParameter("@ccusname",oi.Ccusname),
            new SqlParameter("@cpersoncode",oi.Cpersoncode),
            new SqlParameter("@cSCCode",oi.CSCCode),
            new SqlParameter("@datDeliveryDate",oi.DatDeliveryDate),
            new SqlParameter("@strLoadingWays",oi.StrLoadingWays),
            new SqlParameter("@cSTCode",oi.CSTCode),
            new SqlParameter("@lngopUseraddressId",oi.LngopUseraddressId),
            new SqlParameter("@strU8BillNo",oi.StrU8BillNo)
            };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 新增订单表体[DLproc_NewOrderDetailByIns]
        public bool DLproc_NewOrderDetailByIns(OrderInfo oi, string cbdefine16)
        {
            bool flag = false;
            string cmdText = "DLproc_NewOrderDetailByIns";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@lngopOrderId",oi.LngopOrderId),
            new SqlParameter("@cinvcode",oi.Cinvcode),
            new SqlParameter("@iquantity",oi.Iquantity),
            new SqlParameter("@inum",oi.Inum),
            new SqlParameter("@iquotedprice",oi.Iquotedprice),
            new SqlParameter("@iunitprice",oi.Iunitprice),
            new SqlParameter("@itaxunitprice",oi.Itaxunitprice),
            new SqlParameter("@imoney",oi.Imoney),
            new SqlParameter("@itax",oi.Itax),
            new SqlParameter("@isum",oi.Isum),
            new SqlParameter("@inatunitprice",oi.Inatunitprice),
            new SqlParameter("@inatmoney",oi.Inatmoney),
            new SqlParameter("@inattax",oi.Inattax),
            new SqlParameter("@inatsum",oi.Inatsum),
            new SqlParameter("@kl",oi.Kl),
            new SqlParameter("@itaxrate",oi.Itaxrate),
            new SqlParameter("@cdefine22",oi.Cdefine22),
            new SqlParameter("@iinvexchrate",oi.Iinvexchrate),
            new SqlParameter("@cunitid",oi.Cunitid),
            new SqlParameter("@irowno",oi.Irowno),
            new SqlParameter("@cinvname",oi.Cinvname),
            new SqlParameter("@idiscount",oi.Idiscount),
            new SqlParameter("@inatdiscount",oi.Inatdiscount),
            new SqlParameter("@cComUnitName",oi.CComUnitName),
            new SqlParameter("@cInvDefine1",oi.CInvDefine1),
            new SqlParameter("@cInvDefine2",oi.CInvDefine2),
            new SqlParameter("@cInvDefine13",oi.CInvDefine13),
            new SqlParameter("@cInvDefine14",oi.CInvDefine14),
            new SqlParameter("@unitGroup",oi.UnitGroup),
            new SqlParameter("@cComUnitQTY",oi.CComUnitQTY),
            new SqlParameter("@cInvDefine1QTY",oi.CInvDefine1QTY),
            new SqlParameter("@cInvDefine2QTY",oi.CInvDefine2QTY),
            new SqlParameter("@cn1cComUnitName",oi.Cn1cComUnitName),
            new SqlParameter("@cDefine24",oi.CDefine24),
            new SqlParameter("@cbdefine16",cbdefine16)
            };
            int res = sqlhelper.ExecuteNonQuery(cmdText, paras, CommandType.StoredProcedure);
            if (res > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion

        #region 新增订单表体(预订单参照生成)[DLproc_NewYYOrderDetailByIns]
        public bool DLproc_NewYYOrderDetailByIns(OrderInfo oi, string cbdefine16, string cDefine24)
        {
            bool flag = false;
            string cmdText = "DLproc_NewYYOrderDetailByIns";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@lngopOrderId",oi.LngopOrderId),
            new SqlParameter("@cinvcode",oi.Cinvcode),
            new SqlParameter("@iquantity",oi.Iquantity),
            new SqlParameter("@inum",oi.Inum),
            new SqlParameter("@iquotedprice",oi.Iquotedprice),
            new SqlParameter("@iunitprice",oi.Iunitprice),
            new SqlParameter("@itaxunitprice",oi.Itaxunitprice),
            new SqlParameter("@imoney",oi.Imoney),
            new SqlParameter("@itax",oi.Itax),
            new SqlParameter("@isum",oi.Isum),
            new SqlParameter("@inatunitprice",oi.Inatunitprice),
            new SqlParameter("@inatmoney",oi.Inatmoney),
            new SqlParameter("@inattax",oi.Inattax),
            new SqlParameter("@inatsum",oi.Inatsum),
            new SqlParameter("@kl",oi.Kl),
            new SqlParameter("@itaxrate",oi.Itaxrate),
            new SqlParameter("@cdefine22",oi.Cdefine22),
            new SqlParameter("@iinvexchrate",oi.Iinvexchrate),
            new SqlParameter("@cunitid",oi.Cunitid),
            new SqlParameter("@irowno",oi.Irowno),
            new SqlParameter("@cinvname",oi.Cinvname),
            new SqlParameter("@idiscount",oi.Idiscount),
            new SqlParameter("@inatdiscount",oi.Inatdiscount),
            new SqlParameter("@cComUnitName",oi.CComUnitName),
            new SqlParameter("@cInvDefine1",oi.CInvDefine1),
            new SqlParameter("@cInvDefine2",oi.CInvDefine2),
            new SqlParameter("@cInvDefine13",oi.CInvDefine13),
            new SqlParameter("@cInvDefine14",oi.CInvDefine14),
            new SqlParameter("@unitGroup",oi.UnitGroup),
            new SqlParameter("@cComUnitQTY",oi.CComUnitQTY),
            new SqlParameter("@cInvDefine1QTY",oi.CInvDefine1QTY),
            new SqlParameter("@cInvDefine2QTY",oi.CInvDefine2QTY),
            new SqlParameter("@cn1cComUnitName",oi.Cn1cComUnitName),
            new SqlParameter("@cpreordercode",oi.Cpreordercode),
            new SqlParameter("@iaoids",oi.Iaoids)  ,
            new SqlParameter("@cbdefine16",cbdefine16),
             new SqlParameter("@cDefine24",cDefine24)
            };
            int res = sqlhelper.ExecuteNonQuery(cmdText, paras, CommandType.StoredProcedure);
            if (res > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion

        #region 获取订单(状态bytStatus)[DLproc_UnauditedOrderBySel]
        /// <summary>
        /// 获取订单(状态bytStatus)[DLproc_UnauditedOrderBySel]
        /// </summary>
        /// <param name="bytStatus"></param>
        /// <returns></returns>
        public DataTable DLproc_UnauditedOrderBySel(int bytStatus)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_UnauditedOrderBySel";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@bytStatus",bytStatus)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 获取订单(未审核,操作员strManagers)[DLproc_UnauditedOrderManagersBySel]
        /// <summary>
        /// 获取订单(未审核,操作员strManagers)[DLproc_UnauditedOrderManagersBySel]
        /// </summary>
        /// <param name="strManagers">操作员</param>
        /// <returns></returns>
        public DataTable DLproc_UnauditedOrderManagersBySel(string strManagers, int lngBillType)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_UnauditedOrderManagersBySel";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@strManagers",strManagers),
           new SqlParameter("@lngBillType",lngBillType)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 获取订单(未审核,操作员strManagers,发运方式)[DLproc_UnauditedOrderManagers_U20BySel]
        /// <summary>
        /// 获取订单(未审核,操作员strManagers,发运方式)[DLproc_UnauditedOrderManagers_U20BySel]
        /// </summary>
        /// <param name="strManagers">操作员</param>
        /// <returns></returns>
        public DataTable DLproc_UnauditedOrderManagers_U20BySel(string strManagers, int lngBillType, string cSCCode)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_UnauditedOrderManagers_U20BySel";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@strManagers",strManagers),
           new SqlParameter("@lngBillType",lngBillType),
           new SqlParameter("@cSCCode",cSCCode)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 获取审核的订单号最近的订单号，用于定位(未分配,操作员,发运方式,审核的订单号)[DLproc_UnauditedOrderManagers_U20_strbillnoBySel]
        /// <summary>
        /// 获取审核的订单号最近的订单号，用于定位(未分配,操作员,发运方式,审核的订单号)[DLproc_UnauditedOrderManagers_U20_strbillnoBySel]
        /// </summary>
        /// <param name="strManagers">操作员</param>
        /// <returns></returns>
        public DataTable DLproc_UnauditedOrderManagers_U20_strbillnoBySel(string strManagers, int lngBillType, string cSCCode, string strbillno)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_UnauditedOrderManagers_U20_strbillnoBySel";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@strManagers",strManagers),
           new SqlParameter("@lngBillType",lngBillType),
           new SqlParameter("@cSCCode",cSCCode),
           new SqlParameter("@strbillno",strbillno)        
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
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
            bool flag = false;
            string sql = "update Dl_opOrder set strRejectRemarks=@strRejectRemarks where strBillNo=@strBillNo";
            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@strBillNo",strBillNo),
            new SqlParameter("@strRejectRemarks",strRejectRemarks)
            };
            int res = sqlhelper.ExecuteNonQuery(sql, paras, CommandType.Text);
            if (res > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion

        #region 获取预订单(未审核,操作员strManagers)[DLproc_UnauditedPreOrderManagersBySel]
        /// <summary>
        /// 获取预订单(未审核,操作员strManagers)[DLproc_UnauditedPreOrderManagersBySel]
        /// </summary>
        /// <param name="strManagers">操作员</param>
        /// <returns></returns>
        public DataTable DLproc_UnauditedPreOrderManagersBySel(string strManagers, int lngBillType)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_UnauditedPreOrderManagersBySel";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@strManagers",strManagers),
           new SqlParameter("@lngBillType",lngBillType)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 更新未审核订单状态[DL_OrderStatusByUp]
        public bool DL_OrderStatusByUp(string cSOCode, string strBillNo)
        {
            bool flag = false;
            string sql = "update Dl_opOrder set strBillNo=2,cSOCode=@cSOCode where strBillNo=@strBillNo";
            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@cSOCode",cSOCode),
            new SqlParameter("@strBillNo",strBillNo)
            };
            int res = sqlhelper.ExecuteNonQuery(sql, paras, CommandType.Text);
            if (res > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion

        #region 补充更新生成的U8订单的表体数据[DL_OrderDetailByUp]
        public bool DL_OrderDetailByUp(string cSOCode, string strBillNo)
        {
            bool flag = false;
            string sql = "update Dl_opOrder set strBillNo=2,cSOCode=@cSOCode where strBillNo=@strBillNo";
            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@cSOCode",cSOCode),
            new SqlParameter("@strBillNo",strBillNo)
            };
            int res = sqlhelper.ExecuteNonQuery(sql, paras, CommandType.Text);
            if (res > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion

        #region 生成U8订单表头[DLproc_NewOrderU8ByIns]
        public bool DLproc_NewOrderU8ByIns(string strBillNo)
        {
            bool flag = false;
            string cmdText = "DLproc_NewOrderU8ByIns";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@strBillNo",strBillNo)
            };
            int res = sqlhelper.ExecuteNonQuery(cmdText, paras, CommandType.StoredProcedure);
            if (res > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion

        #region 生成U8订单表头[DLproc_NewYOrderU8ByIns]
        public bool DLproc_NewYOrderU8ByIns(string strBillNo)
        {
            bool flag = false;
            string cmdText = "DLproc_NewYOrderU8ByIns";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@strBillNo",strBillNo)
            };
            int res = sqlhelper.ExecuteNonQuery(cmdText, paras, CommandType.StoredProcedure);
            if (res > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion

        #region 生成U8订单表体[DLproc_NewOrderDetailU8ByIns]
        public bool DLproc_NewOrderDetailU8ByIns()
        {
            bool flag = false;
            string cmdText = "DLproc_NewOrderDetailU8ByIns";
            int res = sqlhelper.ExecuteNonQuery(cmdText, CommandType.StoredProcedure);
            if (res > 0)
            {
                flag = true;
            }
            return flag;
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
            DataTable dt = new DataTable();
            string cmdText = "DLproc_QuasiOrderDetailBySel";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@cinvcode",cinvcode),
           new SqlParameter("@ccuscode",ccuscode)       
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
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
            DataTable dt = new DataTable();
            string cmdText = "select cCusCode from Customer where cCusCode=@ccuscode and cCusName=@ccusname";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@ccuscode",ccuscode),
           new SqlParameter("@ccusname",ccusname)       
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
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
        public DataTable DLproc_QuasiOrderDetailModifyBySel(string cinvcode, string cCusCode, string strBillNo, string areaid)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_QuasiOrderDetailModifyBySel";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@cinvcode",cinvcode),
           new SqlParameter("@cCusCode",cCusCode),
           new SqlParameter("@strBillNo",strBillNo),
           new SqlParameter("@cArea",areaid)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
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
            DataTable dt = new DataTable();
            string cmdText = "DLproc_OrderDetailModifyStockQtyCompareBySel";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@cinvcode",cinvcode),
           new SqlParameter("@strBillNo",strBillNo)       
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
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
            DataTable dt = new DataTable();
            string cmdText = "DLproc_QuasiYOrderDetailBySel";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@cinvcode",cinvcode),
           new SqlParameter("@cpreordercode",cpreordercode)       
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 获取顾客信用额度[DLproc_getCusCreditInfo]
        /// <summary>
        /// 获取顾客信用额度[DLproc_getCusCreditInfo]
        /// </summary>
        /// <param name="cCusCode">顾客编码</param>
        /// <returns></returns>
        public DataTable DLproc_getCusCreditInfo(string cCusCode)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_getCusCreditInfo";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@cCusCode",cCusCode)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 获取顾客剩余信用额度(修改状态,需传入单据号,扣除其金额)[DLproc_getCusCreditInfoWithBillno]
        /// <summary>
        /// 获取顾客剩余信用额度(修改状态,需传入单据号,扣除其金额)[DLproc_getCusCreditInfoWithBillno]
        /// </summary>
        /// <param name="cCusCode">顾客编码</param>
        ///  <param name="strBillNo">单据编码</param>
        /// <returns></returns>
        public DataTable DLproc_getCusCreditInfoWithBillno(string cCusCode, string strBillNo)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_getCusCreditInfoWithBillno";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@cCusCode",cCusCode),
           new SqlParameter("@strBillNo",strBillNo)           
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 获取顾客预订单剩余信用额度[DLproc_getPreOrderCusCreditInfo]
        /// <summary>
        /// 获取顾客预订单剩余信用额度[DLproc_getPreOrderCusCreditInfo]
        /// </summary>
        /// <param name="cCusCode">顾客编码</param>
        /// <returns></returns>
        public DataTable DLproc_getPreOrderCusCreditInfo(string cCusCode, int lngBillType)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_getPreOrderCusCreditInfo";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@cCusCode",cCusCode),
           new SqlParameter("@lngBillType",lngBillType)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 驳回提交的订单[DL_RejectOrderBillByUpd]
        public bool DL_RejectOrderBillByUpd(string strBillNo, string strManagers)
        {
            bool flag = false;
            string cmdText = "update Dl_opOrder set bytStatus=3,strManagers=@strManagers,datRejectTime=GETDATE() where strBillNo=@strBillNo";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@strBillNo",strBillNo),           
            new SqlParameter("@strManagers",strManagers)
            };
            int res = sqlhelper.ExecuteNonQuery(cmdText, paras, CommandType.Text);
            if (res > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion

        #region 驳回提交的订单（标记3小时过期,写入到期时间）[DL_RejectOrderBillForExpTimeByUpd]
        public bool DL_RejectOrderBillForExpTimeByUpd(string strBillNo, string strManagers)
        {
            bool flag = false;
            string cmdText = "update Dl_opOrder set bytStatus=3,strManagers=@strManagers,datRejectTime=GETDATE(),datExpTime=DATEADD(MI,180,GETDATE()) where strBillNo=@strBillNo";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@strBillNo",strBillNo),           
            new SqlParameter("@strManagers",strManagers)
            };
            int res = sqlhelper.ExecuteNonQuery(cmdText, paras, CommandType.Text);
            if (res > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion

        #region 驳回提交的预订单[DL_RejectYOrderBillByUpd]
        public bool DL_RejectYOrderBillByUpd(string strBillNo, string strManagers)
        {
            bool flag = false;
            string cmdText = "update Dl_opPreOrder set bytStatus=3,strManagers=@strManagers where strBillNo=@strBillNo";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@strBillNo",strBillNo),           
            new SqlParameter("@strManagers",strManagers)
            };
            int res = sqlhelper.ExecuteNonQuery(cmdText, paras, CommandType.Text);
            if (res > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion

        #region 作废提交的预订单[DL_InvalidYOrderBillByUpd]
        public bool DL_InvalidYOrderBillByUpd(string strBillNo, string strManagers, string InvalidPersonCode)
        {
            bool flag = false;
            string cmdText = "update Dl_opPreOrder set bytStatus=99,strManagers=@strManagers,InvalidTime=getdate(),InvalidPersonCode=@InvalidPersonCode where strBillNo=@strBillNo";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@strBillNo",strBillNo),           
            new SqlParameter("@strManagers",strManagers),
            new SqlParameter("@InvalidPersonCode",InvalidPersonCode)
            };
            int res = sqlhelper.ExecuteNonQuery(cmdText, paras, CommandType.Text);
            if (res > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion

        #region 查询订单(DL编号)[DL_OrderBillBySel]
        /// <summary>
        /// 查询订单(DL编号)[DL_OrderBillBySel]
        /// </summary>
        /// <param name="strBillNo">DL编号</param>
        /// <returns></returns>
        public DataTable DL_OrderBillBySel(string strBillNo)
        {
            DataTable dt = new DataTable();
            string cmdText = "select dod.isum,dod.iquotedprice*dod.iquantity 'cComUnitAmount',dod.*,iv.cInvStd,convert(varchar(10),do.datCreateTime,120) 'datCreateTime',do.ccusname,do.strRemarks,do.cSCCode,do.cdefine11,do.cpersoncode,do.datBillTime,do.strManagers,do.datDeliveryDate,do.strLoadingWays,do.cSTCode,do.cdefine3,do.lngopUserId,dou.cCusCode,do.ccuscode,do.ccuscode  'ccuscode1',dou.strUserName,do.datBillTime,do.cSTCode,dod.irowno,do.lngBillType,do.lngopUseraddressId,isnull(doq.strQQName,'') strQQName,dod.isum xx,do.bytStatus  from Dl_opOrder do left join Dl_opOrderDetail dod on do.lngopOrderId=dod.lngopOrderId left join Dl_opUser dou on dou.lngopUserId=do.lngopUserId left join Inventory iv on iv.cInvCode=dod.cinvcode left join Dl_opQQService doq on doq.strOpUserId=do.strManagers  where do.strBillNo=@strBillNo order by dod.irowno";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@strBillNo",strBillNo)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 更新手工在U8下的订单的已确认(cCusOAddress)字段值(U8编号)[DL_U8OrderBillConfirmByUpd]
        /// <summary>
        /// 更新手工在U8下的订单的已确认(cCusOAddress)字段值(U8编号)[DL_U8OrderBillConfirmByUpd]
        /// </summary>
        /// <param name="strBillNo">U8编号</param>
        /// <returns></returns>
        public bool DL_U8OrderBillConfirmByUpd(string strBillNo)
        {
            bool flag = false;
            string cmdText = "update SO_SOMain_extradefine set chdefine19='已确认' from SO_SOMain_extradefine aa left join SO_SOMain bb on aa.ID=bb.ID where bb.cSOCode=@strBillNo";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@strBillNo",strBillNo)
            };
            int res = sqlhelper.ExecuteNonQuery(cmdText, paras, CommandType.Text);
            if (res > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion

        #region 查询订单(U8编号)[DL_U8OrderBillBySel]
        /// <summary>
        /// 查询订单(U8编号)[DL_U8OrderBillBySel]
        /// </summary>
        /// <param name="strBillNo">U8编号</param>
        /// <returns></returns>
        public DataTable DL_U8OrderBillBySel(string strBillNo)
        {
            DataTable dt = new DataTable();
            string cmdText = "select aa.cSOCode 'strBillNo','多联' 'Biller',convert(varchar(10),aa.dDate,120) 'datCreateTime',aa.cCusName 'ccusname',aa.cMemo 'strRemarks','' strQQName,cSCCode,cpersoncode,isnull(aa.cDefine1,'')+';'+isnull(aa.cDefine13,'')+';'+isnull(aa.cDefine10,'')+';'+isnull(aa.cDefine2,'')+';'+isnull(aa.cDefine3,'')+' '+isnull(aa.cDefine9,'')+';'+isnull(aa.cDefine12,'')+';'+isnull(aa.cDefine11,'')+';'+isnull(aa.cDefine3,'') 'cdefine11',bb.cInvCode,bb.cInvName 'cinvname',inv.cInvStd,bb.iQuantity 'iquantity',bb.cDefine22 'cdefine22',iQuotedPrice 'iquotedprice',iQuotedPrice * iQuantity 'cComUnitAmount',bb.irowno,bb.iSum 'isum'   from SO_SOMain aa left join SO_SODetails bb on aa.ID=bb.ID left join Inventory inv on inv.cInvCode=bb.cInvCode left join SO_SOMain_extradefine cc on aa.ID=cc.ID where aa.cSOCode=@strBillNo order by bb.irowno";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@strBillNo",strBillNo)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 查询订单(DL编号,单据类型)[DL_OrderBillBySel]
        /// <summary>
        /// 查询订单(DL编号)[DL_OrderBillBySel]
        /// </summary>
        /// <param name="strBillNo">DL编号</param>
        /// <returns></returns>
        public DataTable DL_OrderBillBySel(string strBillNo, int lngBillType)
        {
            DataTable dt = new DataTable();
            //string cmdText = "select dod.iquotedprice*dod.iquantity 'cComUnitAmount',dod.*,iv.cInvStd,convert(varchar(10),do.datCreateTime,120) 'datCreateTime',do.ccusname,do.strRemarks,do.cSCCode,do.cdefine11,do.cpersoncode,do.datBillTime,do.strManagers,do.datDeliveryDate,do.strLoadingWays,do.cSTCode,do.cdefine3,do.lngopUserId,dou.cCusCode,do.ccuscode,do.ccusname,dou.strUserName,do.datBillTime,do.cSTCode,dod.irowno  from Dl_opOrder do left join Dl_opOrderDetail dod on do.lngopOrderId=dod.lngopOrderId left join Dl_opUser dou on dou.lngopUserId=do.lngopUserId left join Inventory iv on iv.cInvCode=dod.cinvcode  where do.strBillNo=@strBillNo and lngBillType=@lngBillType order by dod.irowno";
            string cmdText = @"select dod.iquotedprice*dod.iquantity 'cComUnitAmount',dod.*,iv.cInvStd,convert(varchar(10),do.datCreateTime,120) 'datCreateTime',
do.ccusname,do.strRemarks,do.cSCCode,do.cdefine11,do.cpersoncode,do.datBillTime,do.strManagers,do.datDeliveryDate,do.strLoadingWays,
do.cSTCode,do.cdefine3,do.lngopUserId,dou.cCusCode,do.ccuscode,do.ccusname,dou.strUserName,do.datBillTime,do.cSTCode,dod.irowno,iv.cPackingType,(dod.cinvcode+dod.cpreordercode) as itemid
from Dl_opOrder do 
left join Dl_opOrderDetail dod 
on do.lngopOrderId=dod.lngopOrderId 
left join Dl_opUser dou 
on dou.lngopUserId=do.lngopUserId 
left join Inventory iv on iv.cInvCode=dod.cinvcode 
where do.strBillNo='CZTS20170402190'   order by dod.irowno";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@strBillNo",strBillNo),
           new SqlParameter("@lngBillType",lngBillType)         
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 查询预订单(DL编号)[DL_PreOrderBillBySel]
        /// <summary>
        /// 查询预订单(DL编号)[DL_OrderBillBySel]
        /// </summary>
        /// <param name="strBillNo">DL编号</param>
        /// <returns></returns>
        public DataTable DL_PreOrderBillBySel(string strBillNo)
        {
            DataTable dt = new DataTable();
            string cmdText = "select dod.cdefine22,do.ddate,dod.iquotedprice*dod.iquantity 'cComUnitAmount',dod.*,iv.cInvStd, do.ccusname, convert(varchar(20),do.datBillTime,120) 'datBillTime',do.strManagers, do.lngopUserId,dou.cCusCode,do.ccuscode,do.ccusname,dou.strUserName,do.datBillTime, dod.irowno  from Dl_opPreOrder do left join Dl_opPreOrderDetail dod on do.lngPreOrderId=dod.lngPReOrderId  left join Dl_opUser dou on dou.lngopUserId=do.lngopUserId left join Inventory iv on iv.cInvCode=dod.cinvcode  where do.strBillNo=@strBillNo order by dod.irowno ";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@strBillNo",strBillNo)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 审核提交的订单[DL_CheckOrderBillByUpd]
        public bool DL_CheckOrderBillByUpd(string strBillNo)
        {
            bool flag = false;
            string cmdText = "update Dl_opOrder set bytStatus=2 where strBillNo=@strBillNo";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@strBillNo",strBillNo)
            };
            int res = sqlhelper.ExecuteNonQuery(cmdText, paras, CommandType.Text);
            if (res > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion

        #region 作废订单[DL_InvalidOrderByUpd]
        public bool DL_InvalidOrderByUpd(string strBillNo, string lngopUserId)
        {
            bool flag = false;
            string cmdText = " update Dl_opOrder set bytStatus=99,InvalidTime=GETDATE(),InvalidPersonCode=@lngopUserId where strBillNo=@strBillNo";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@strBillNo",strBillNo),
            new SqlParameter("@lngopUserId",lngopUserId)
            };
            int res = sqlhelper.ExecuteNonQuery(cmdText, paras, CommandType.Text);
            if (res > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion

        #region 变更订单状态(顾客确认界面中将订单变更为修改状态,即被驳回状态)[DL_ModifyOrderByUpd]
        public bool DL_ModifyOrderByUpd(string strBillNo)
        {
            bool flag = false;
            string cmdText = "update Dl_opOrder set bytStatus=3 where strBillNo=@strBillNo";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@strBillNo",strBillNo)
            };
            int res = sqlhelper.ExecuteNonQuery(cmdText, paras, CommandType.Text);
            if (res > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion

        #region 查询订单表体信息(客户)[DL_DisposeOrderBySel]
        /// <summary>
        /// 查询订单表体信息(客户)[DL_DisposeOrderBySel]
        /// </summary>
        /// <param name="bytStatus"></param>
        /// <param name="ccuscode"></param>
        /// <returns></returns>
        public DataTable DL_DisposeOrderBySel(int bytStatus, string ccuscode)
        {
            DataTable dt = new DataTable();
            string cmdText = "select dod.iquotedprice*dod.iquantity 'cComUnitAmount',dod.*,iv.cInvStd,convert(varchar(10),do.datCreateTime,120) 'datCreateTime',do.ccusname,do.strRemarks,do.cSCCode,do.cdefine11,do.cpersoncode  from Dl_opOrder do inner join Dl_opOrderDetail dod on do.lngopOrderId=dod.lngopOrderId left join Inventory iv on iv.cInvCode=dod.cinvcode  where do.bytStatus=@bytStatus and do.ccuscode like @ccuscode";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@bytStatus",bytStatus),
           new SqlParameter("@ccuscode",ccuscode)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 获取订单表头信息(状态bytStatus,客户编码,)[DL_UnauditedOrderBySel]
        /// <summary>
        /// 获取订单表头信息(状态bytStatus,客户编码,)[DL_UnauditedOrderBySel]
        /// </summary>
        /// <param name="bytStatus"></param>
        /// <returns></returns>
        public DataTable DL_UnauditedOrderBySel(int bytStatus, string ccuscode)
        {
            DataTable dt = new DataTable();
            string cmdText = "select do.strRejectRemarks,do.strBillNo,do.cSOCode,du.strUserName,do.ccusname,cDefine11,strRemarks,convert(varchar(10),do.datCreateTime,120) 'datCreateTime',do.RelateU8NO,case  when do.cSTCode='00' and lngBillType=0 then '普通订单' when do.cSTCode='01' then '样品订单' when lngBillType=1 then '酬宾订单' when lngBillType=2 then '特殊订单'  end cSTCode,do.cSTCode cSTCode1   from Dl_opOrder do inner join Dl_opUser  du on do.lngopUserId=du.lngopUserId where do.bytStatus=@bytStatus and do.ccuscode like @ccuscode union all select '',aa.cSOCode 'strBillNo',aa.cSOCode,REPLACE(@ccuscode,'%','') 'strUserName',cCusCode,'','',convert(varchar(10),aa.dDate,120) 'datCreateTime',cc.chdefine2,cc.chdefine13,''   from SO_SOMain aa left join SO_SOMain_extradefine cc on aa.ID=cc.ID where aa.cCusCode like @ccuscode and cc.chdefine13!='网上下单' and isnull(cc.chdefine19,0)!='已确认'";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@bytStatus",bytStatus),
           new SqlParameter("@ccuscode",ccuscode)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
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
            DataTable dt = new DataTable();
            //string cmdText = "select  do.lngopOrderId,do.strManagers, do.strRejectRemarks,do.strBillNo,do.cSOCode,du.strUserName,do.ccusname,cDefine11,strRemarks,convert(varchar(10),do.datCreateTime,120) 'datCreateTime',do.RelateU8NO,case  when do.cSTCode='00' and lngBillType=0 then '普通订单' when do.cSTCode='01' then '样品订单' when lngBillType=1 then '酬宾订单' when lngBillType=2 then '特殊订单'  end cSTCode,do.cSTCode cSTCode1   from Dl_opOrder do inner join Dl_opUser  du on do.lngopUserId=du.lngopUserId where do.bytStatus=@bytStatus and do.lngopUserId=@lngopUserId and isnull(do.lngopUserExId,'0')=@lngopUserExId ";
            string cmdText = @"SELECT do.lngopOrderId, do.cSCCode,do.strManagers,ex.recaptionTimes, do.strRejectRemarks,do.strBillNo,do.cSOCode,du.strUserName,do.ccusname,cDefine11,strRemarks,convert(varchar(10),do.datCreateTime,120) 'datCreateTime',do.RelateU8NO,
                                CASE  when do.cSTCode='00' and lngBillType=0 then '普通订单' when do.cSTCode='01' then '样品订单' when lngBillType=1 then '酬宾订单' when lngBillType=2 then '特殊订单'  end cSTCode,do.cSTCode cSTCode1  
                                from Dl_opOrder do 
                                INNER join Dl_opUser  du on do.lngopUserId=du.lngopUserId 
                                LEFT JOIN dl_oporder_ex ex ON do.lngopOrderId=ex.lngopOrderId
                                WHERE do.bytStatus=@bytStatus and do.lngopUserId=@lngopUserId and isnull(do.lngopUserExId,'0')=@lngopUserExId";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@bytStatus",bytStatus),
           new SqlParameter("@lngopUserId",lngopUserId),
           new SqlParameter("@lngopUserExId",lngopUserExId)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 获取预订单表头信息(状态bytStatus,客户编码,)[DL_UnauditedPreOrderBySel]
        /// <summary>
        /// 获取预订单表头信息(状态bytStatus,客户编码,)[DL_UnauditedPreOrderBySel]
        /// </summary>
        /// <param name="bytStatus"></param>
        /// <returns></returns>
        public DataTable DL_UnauditedPreOrderBySel(int bytStatus, string ccuscode)
        {
            DataTable dt = new DataTable();
            string cmdText = "select do.strBillNo,du.strUserName,do.ccusname,ddate from Dl_opPreOrder do inner join Dl_opUser  du on do.lngopUserId=du.lngopUserId where do.bytStatus=@bytStatus and do.ccuscode like @ccuscode order by do.strBillNo desc ";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@bytStatus",bytStatus),
           new SqlParameter("@ccuscode",ccuscode)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 查询订单表头数据,修改专用(DL编号)[DL_OrderModifyBySel]
        /// <summary>
        /// 查询订单表头数据,修改专用(DL编号)[DL_OrderModifyBySel]
        /// </summary>
        /// <param name="strBillNo">DL编号</param>
        /// <returns></returns>
        public DataTable DL_OrderModifyBySel(string strBillNo)
        {
            DataTable dt = new DataTable();
            string cmdText = "select * from Dl_opOrder where strBillNo=@strBillNo";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@strBillNo",strBillNo)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 查询订单表体数据,修改专用(DL编号)[DL_OrderDetailModifyBySel]
        /// <summary>
        /// 查询订单表头数据,修改专用(DL编号)[DL_OrderDetailModifyBySel]
        /// </summary>
        /// <param name="strBillNo">DL编号</param>
        /// <returns></returns>
        public DataTable DLproc_OrderDetailModifyBySel(string strBillNo)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_OrderDetailModifyBySel";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@strBillNo",strBillNo)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 查询参照订单表体数据,修改参照订单专用,读取原订单数据(DL编号)[DLproc_OrderYDetailModifyBySel]
        /// <summary>
        /// 查询参照订单表体数据,修改参照订单专用,读取原订单数据(DL编号)[DLproc_OrderYDetailModifyBySel]
        /// </summary>
        /// <param name="strBillNo">DL编号</param>
        /// <returns></returns>
        public DataTable DLproc_OrderYDetailModifyBySel(string strBillNo)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_OrderYDetailModifyBySel";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@strBillNo",strBillNo)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 更新订单表头[DLproc_NewOrderByUpd]
        public DataTable DLproc_NewOrderByUpd(OrderInfo oi, string areaid, string iaddresstype, string chdefine21, string cdefine8,string cWhCode)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_NewOrder_All_Warehouse_ByUpd";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@strBillNo",oi.StrBillNo),
            new SqlParameter("@lngopUserId",oi.LngopUserId),
            new SqlParameter("@datCreateTime",oi.DatCreateTime),
            new SqlParameter("@bytStatus",oi.BytStatus),
            new SqlParameter("@strRemarks",oi.StrRemarks),
            new SqlParameter("@ccuscode",oi.Ccuscode),
            new SqlParameter("@cdefine1",oi.Cdefine1),
            new SqlParameter("@cdefine2",oi.Cdefine2),
            new SqlParameter("@cdefine3",oi.Cdefine3),
            new SqlParameter("@cdefine9",oi.Cdefine9),
            new SqlParameter("@cdefine10",oi.Cdefine10),
            new SqlParameter("@cdefine11",oi.Cdefine11),
            new SqlParameter("@cdefine12",oi.Cdefine12),
            new SqlParameter("@cdefine13",oi.Cdefine13),
            new SqlParameter("@ccusname",oi.Ccusname),
            new SqlParameter("@cpersoncode",oi.Cpersoncode),
            new SqlParameter("@cSCCode",oi.CSCCode),
            new SqlParameter("@datDeliveryDate",oi.DatDeliveryDate),
            new SqlParameter("@strLoadingWays",oi.StrLoadingWays),
            new SqlParameter("@lngopUseraddressId",oi.LngopUseraddressId),
            new SqlParameter("@cdiscountname",oi.Cdiscountname),
            new SqlParameter("@chdefine49",areaid),
            new SqlParameter("@iaddresstype",iaddresstype),
            new SqlParameter("@chdefine21",chdefine21),
            new SqlParameter("@cdefine8",cdefine8),
            new SqlParameter("@chdefine51",cWhCode)

            };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 修改更新订单表头(酬宾订单/特殊订单))[DLproc_NewYOrderByUpd]
        public DataTable DLproc_NewYOrderByUpd(OrderInfo oi, string areaid, string iaddresstype, string chdefine21, string cdefine8)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_NewYOrderByUpd";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@strBillNo",oi.StrBillNo),
            new SqlParameter("@lngopUserId",oi.LngopUserId),
            new SqlParameter("@datCreateTime",oi.DatCreateTime),
            new SqlParameter("@bytStatus",oi.BytStatus),
            new SqlParameter("@strRemarks",oi.StrRemarks),
            new SqlParameter("@ccuscode",oi.Ccuscode),
            new SqlParameter("@cdefine1",oi.Cdefine1),
            new SqlParameter("@cdefine2",oi.Cdefine2),
            new SqlParameter("@cdefine3",oi.Cdefine3),
            new SqlParameter("@cdefine9",oi.Cdefine9),
            new SqlParameter("@cdefine10",oi.Cdefine10),
            new SqlParameter("@cdefine11",oi.Cdefine11),
            new SqlParameter("@cdefine12",oi.Cdefine12),
            new SqlParameter("@cdefine13",oi.Cdefine13),
            new SqlParameter("@ccusname",oi.Ccusname),
            new SqlParameter("@cpersoncode",oi.Cpersoncode),
            new SqlParameter("@cSCCode",oi.CSCCode),
            new SqlParameter("@datDeliveryDate",oi.DatDeliveryDate),
            new SqlParameter("@strLoadingWays",oi.StrLoadingWays),
            new SqlParameter("@lngopUseraddressId",oi.LngopUseraddressId),
            new SqlParameter("@chdefine49",areaid),
            new SqlParameter("@iaddresstype",iaddresstype),
            new SqlParameter("@chdefine21",chdefine21),
            new SqlParameter("@cdefine8",cdefine8)
            };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 修改更新订单表头(样品资料)[DLproc_SampleOrderByUpd]
        public DataTable DLproc_SampleOrderByUpd(OrderInfo oi)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_SampleOrderByUpd";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@strBillNo",oi.StrBillNo),
            new SqlParameter("@strRemarks",oi.StrRemarks),
            new SqlParameter("@strLoadingWays",oi.StrLoadingWays),
            new SqlParameter("@bytStatus",oi.BytStatus)
            };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 更新订单表体[DLproc_NewOrderDetailByUpd]
        public bool DLproc_NewOrderDetailByUpd(OrderInfo oi)
        {
            bool flag = false;
            string cmdText = "DLproc_NewOrderDetailByUpd";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@lngopOrderId",oi.LngopOrderId),
            new SqlParameter("@cinvcode",oi.Cinvcode),
            new SqlParameter("@iquantity",oi.Iquantity),
            new SqlParameter("@inum",oi.Inum),
            new SqlParameter("@iquotedprice",oi.Iquotedprice),
            new SqlParameter("@iunitprice",oi.Iunitprice),
            new SqlParameter("@itaxunitprice",oi.Itaxunitprice),
            new SqlParameter("@imoney",oi.Imoney),
            new SqlParameter("@itax",oi.Itax),
            new SqlParameter("@isum",oi.Isum),
            new SqlParameter("@inatunitprice",oi.Inatunitprice),
            new SqlParameter("@inatmoney",oi.Inatmoney),
            new SqlParameter("@inattax",oi.Inattax),
            new SqlParameter("@inatsum",oi.Inatsum),
            new SqlParameter("@kl",oi.Kl),
            new SqlParameter("@itaxrate",oi.Itaxrate),
            new SqlParameter("@cdefine22",oi.Cdefine22),
            new SqlParameter("@iinvexchrate",oi.Iinvexchrate),
            new SqlParameter("@cunitid",oi.Cunitid),
            new SqlParameter("@irowno",oi.Irowno),
            new SqlParameter("@cinvname",oi.Cinvname),
            new SqlParameter("@idiscount",oi.Idiscount),
            new SqlParameter("@inatdiscount",oi.Inatdiscount),
            new SqlParameter("@cComUnitName",oi.CComUnitName),
            new SqlParameter("@cInvDefine1",oi.CInvDefine1),
            new SqlParameter("@cInvDefine2",oi.CInvDefine2),
            new SqlParameter("@cInvDefine13",oi.CInvDefine13),
            new SqlParameter("@cInvDefine14",oi.CInvDefine14),
            new SqlParameter("@unitGroup",oi.UnitGroup),
            new SqlParameter("@cComUnitQTY",oi.CComUnitQTY),
            new SqlParameter("@cInvDefine1QTY",oi.CInvDefine1QTY),
            new SqlParameter("@cInvDefine2QTY",oi.CInvDefine2QTY),
            new SqlParameter("@cn1cComUnitName",oi.Cn1cComUnitName)
            };
            int res = sqlhelper.ExecuteNonQuery(cmdText, paras, CommandType.StoredProcedure);
            if (res > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion

        #region 更新订单表体[DL_NewOrderDetailByDel]
        public bool DL_NewOrderDetailByDel(int lngopOrderId)
        {
            bool flag = true;
            string cmdText = "Delete from Dl_opOrderDetail where lngopOrderId=@lngopOrderId";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@lngopOrderId",lngopOrderId)            
            };
            int res = sqlhelper.ExecuteNonQuery(cmdText, paras, CommandType.Text);
            if (res > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion

        #region 检测订单专属操作员[DL_ManagersOrderBillBySel]
        public DataTable DL_ManagersOrderBillBySel(string strBillNo)
        {
            DataTable dt = new DataTable();
            string cmdText = "select strBillNo from Dl_opOrder where strBillNo=@strBillNo";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@strBillNo",strBillNo)   
            };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 检测预订单专属操作员[DL_ManagersPreOrderBillBySel]
        public DataTable DL_ManagersPreOrderBillBySel(string strBillNo)
        {
            DataTable dt = new DataTable();
            string cmdText = "select strBillNo from Dl_opPreOrder where strBillNo=@strBillNo";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@strBillNo",strBillNo)   
            };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 绑定订单专属操作员[DL_ManagersOrderBillByUpd]
        public bool DL_ManagersOrderBillByUpd(string strBillNo, string strManagers)
        {
            bool flag = false;
            string cmdText = "update Dl_opOrder set strManagers=@strManagers where strBillNo=@strBillNo or RelateU8NO =@strBillNo ";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@strBillNo",strBillNo),
            new SqlParameter("@strManagers",strManagers)
            };
            int res = sqlhelper.ExecuteNonQuery(cmdText, paras, CommandType.Text);
            if (res > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion

        #region 绑定预订单专属操作员[DL_ManagersPreOrderBillByUpd]
        public bool DL_ManagersPreOrderBillByUpd(string strBillNo, string strManagers)
        {
            bool flag = false;
            string cmdText = "update Dl_opPreOrder set strManagers=@strManagers where strBillNo=@strBillNo";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@strBillNo",strBillNo),
            new SqlParameter("@strManagers",strManagers)
            };
            int res = sqlhelper.ExecuteNonQuery(cmdText, paras, CommandType.Text);
            if (res > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion

        #region 用于查询我的工作详情[DLproc_MyWorkBySel]
        /// <summary>
        /// 用于查询我的工作详情[DLproc_MyWorkBySel]
        /// </summary>
        /// <param name="cinvcode">存货编码</param>
        /// <returns></returns>
        public DataTable DLproc_MyWorkBySel(string strBillNo, string beginDate, string endDate, string cSTCode, int bytStatus, string lngopUserId)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_MyWorkBySel";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@strBillNo",strBillNo),
           new SqlParameter("@beginDate",beginDate),
           new SqlParameter("@endDate",endDate),
           new SqlParameter("@cSTCode",cSTCode),
           new SqlParameter("@bytStatus",bytStatus),
           new SqlParameter("@strManagers",lngopUserId) 
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 用于查询顾客的酬宾(特殊)订单详情[DLproc_MyWorkPreYOrderForCustomerBySel]
        /// <summary>
        /// 用于查询顾客的酬宾(特殊)订单详情[DLproc_MyWorkPreYOrderForCustomerBySel]
        /// </summary>
        /// <param name="cinvcode">存货编码</param>
        /// <returns></returns>
        public DataTable DLproc_MyWorkPreYOrderForCustomerBySel(string strBillNo, string beginDate, string endDate, int bytStatus, string lngopUserId, string lngBillType)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_MyWorkPreYOrderForCustomerBySel";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@strBillNo",strBillNo),
           new SqlParameter("@beginDate",beginDate),
           new SqlParameter("@endDate",endDate),
           new SqlParameter("@bytStatus",bytStatus),
           new SqlParameter("@ConstcCusCode",lngopUserId),
           new SqlParameter("@lngBillType",lngBillType)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 用于查询我的工作详情(预订单)[DLproc_MyWorkPreOrderBySel]
        /// <summary>
        /// 用于查询我的工作详情[DLproc_MyWorkBySel]
        /// </summary>
        /// <param name="cinvcode">存货编码</param>
        /// <returns></returns>
        public DataTable DLproc_MyWorkPreOrderBySel(string strBillNo, string beginDate, string endDate, int bytStatus, string lngopUserId)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_MyWorkPreOrderBySel";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@strBillNo",strBillNo),
           new SqlParameter("@beginDate",beginDate),
           new SqlParameter("@endDate",endDate),
           new SqlParameter("@bytStatus",bytStatus),
           new SqlParameter("@strManagers",lngopUserId) 
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
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
            DataTable dt = new DataTable();
            string cmdText = "select isnull(cCusEmail,0)  from Customer where cCusCode=@ccuscode and cCusEmail='1'";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@ccuscode",ccuscode)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 查询订单(DL编号,用于样品资料自动填写内容)[DL_OrderBillYPZLBySel]
        /// <summary>
        /// 查询订单(DL编号,用于样品资料自动填写内容)[DL_OrderBillYPZLBySel]
        /// </summary>
        /// <param name="strBillNo">DL编号</param>
        /// <returns></returns>
        public DataTable DL_OrderBillYPZLBySel(string strBillNo)
        {
            DataTable dt = new DataTable();
            string cmdText = "select ccuscode,ccusname,cpersoncode,cdefine11,left(cdefine11,2) TxtcSCCode,strRemarks,cdefine3,strLoadingWays,datDeliveryDate,convert(varchar(13),datDeliveryDate,120) datDeliveryDateText  from Dl_opOrder where strBillNo=@strBillNo";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@strBillNo",strBillNo)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
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
            DataTable dt = new DataTable();
            //   string cmdText = "select *,case when lngBillType='1' then '酬宾订单'  when lngBillType='2' then '特殊订单' else cSTCode end 'billtype'  from (select aa.*,isnull(bb.strBillNo,' ') strBillNo,CASE WHEN aa.chdefine13 is null THEN   '自制单'  ELSE  aa.chdefine13 end 'XDFS',bb.lngBillType   from (select case som.cSTCode when '00' then '普通订单' when '01' then '样品资料' end cSTCode,som.cSOCode,isnull(somex.chdefine2,' ') chdefine2,chdefine13,convert(varchar(10),som.dDate,120) dDate,som.cCusName,som.cMemo from SO_SOMain som left join SO_SOMain_extradefine somex on som.ID=somex.ID where som.cCusCode like @ccuscode and isnull(somex.chdefine19,0)!='未确认') aa left join (select strBillNo,cSOCode,lngBillType from Dl_opOrder where ccuscode like @ccuscode and cSOCode !=' ' and bytStatus<=4) bb on aa.cSOCode=bb.cSOCode )   as hh where dDate between @beginDate and @endDate UNION ALL SELECT '参照订单',strBillNo,'','网上下单',CONVERT(VARCHAR(10),datBillTime,120),ccusname,strRemarks,strBillNo,'网上下单',0,'普通订单' FROM dbo.Dl_opOrder WHERE bytStatus<=4 AND strBillNo LIKE 'czts%' AND ccuscode like @ccuscode AND (CONVERT(VARCHAR(10),datBillTime,120) between @beginDate and @endDate) order by cSOCode desc";
            string cmdText = "DLproc_PreviousOrderBySel";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@ccuscode",ccuscode),
           new SqlParameter("@beginDate",beginDate),
           new SqlParameter("@endDate",endDate)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 顾顾客历史订单（V2.0，包含物流信息）[DL_PreviousOrder_LOGBySel]
        /// <summary>
        /// 顾客历史订单（V2.0，包含物流信息）[DL_PreviousOrder_LOGBySel]
        /// </summary>
        /// <param name="ccuscode">顾客大类编号(登录code)</param>
        /// <returns></returns>
        public DataTable DL_PreviousOrder_LOGBySel(string ccuscode, string beginDate, string endDate)
        {
            DataTable dt = new DataTable();
            string cmdText = "select *,case when lngBillType='1' then '酬宾订单'  when lngBillType='2' then '特殊订单' else cSTCode end 'billtype'  from (select aa.*,isnull(bb.strBillNo,' ') strBillNo,CASE WHEN aa.chdefine13 is null THEN   '自制单'  ELSE  aa.chdefine13 end 'XDFS',bb.lngBillType   from (select case som.cSTCode when '00' then '普通订单' when '01' then '样品资料' end cSTCode,som.cSOCode,isnull(somex.chdefine2,' ') chdefine2,chdefine13,convert(varchar(10),som.dDate,120) dDate,som.cCusName,som.cMemo from SO_SOMain som left join SO_SOMain_extradefine somex on som.ID=somex.ID where som.cCusCode like @ccuscode and isnull(somex.chdefine19,0)!='未确认') aa left join (select strBillNo,cSOCode,lngBillType from Dl_opOrder where ccuscode like @ccuscode and cSOCode !=' ' and bytStatus!=99) bb on aa.cSOCode=bb.cSOCode )   as hh where dDate between @beginDate and @endDate order by cSOCode desc";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@ccuscode",ccuscode),
           new SqlParameter("@beginDate",beginDate),
           new SqlParameter("@endDate",endDate)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
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
            DataTable dt = new DataTable();
            string cmdText = "select strBillNo,cSOCode,datBillTime from Dl_opOrder where ccuscode like  @ccuscode and lngBillType=0 and cSTCode='00'   order by datBillTime";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@ccuscode",ccuscode),
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
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
            DataTable dt = new DataTable();
            string cmdText = "select  isnull(aa.RelateU8NO,'') 'chdefine2',case when cSTCode='00' and lngBillType=0 then '普通订单'  when cSTCode='01' and lngBillType=0 then '样品资料' when cSTCode='00' and lngBillType=1 then '参照酬宾订单' when cSTCode='00' and lngBillType=2 then '参照特殊订单' end 'cSTCode',isnull(cSOCode,'') 'cSOCode',convert(varchar(10),aa.datCreateTime,120) dDate,cCusName,aa.strRemarks cMemo,strBillNo,'自制单' 'XDFS',	lngBillType,bytStatus	,case when cSTCode='00' and lngBillType=0 then '普通订单'  when cSTCode='01' and lngBillType=0 then '样品资料' when cSTCode='00' and lngBillType=1 then '参照酬宾订单' when cSTCode='00' and lngBillType=2 then '参照特殊订单' end billtype from Dl_opOrder aa  where aa.ccuscode like @ccuscode and aa.bytStatus>=90 and (aa.datCreateTime between @beginDate and @endDate)";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@ccuscode",ccuscode),
           new SqlParameter("@beginDate",beginDate),
           new SqlParameter("@endDate",endDate)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 查询普通U8订单[DL_OrderU8BillBySel]
        /// <summary>
        /// 查询U8订单[DL_OrderU8BillBySel]
        /// </summary>
        /// <param name="strU8BillNo">U8编号</param>
        /// <returns></returns>
        public DataTable DL_OrderU8BillBySel(string strU8BillNo)
        {
            DataTable dt = new DataTable();
            //string cmdText = "select aa.cMaker,pp.cPersonName,case aa.cSCCode when '00' then '自提' when '01' then '配送' end cSCCode,isnull(aa.cMemo,'') 'cMemo',isnull(aa.cdefine9,'')+' '+isnull(aa.cdefine12,'')+' '+isnull(aa.cdefine11,'')+' '+isnull(aa.cdefine1,'')+' '+isnull(aa.cdefine13,'')+' '+isnull(aa.cdefine2,'')+' '+isnull(aa.cdefine10,'')+' '+isnull(aa.cdefine3,'') 'cdefine11',bb.cInvName,bb.iQuantity,bb.cDefine22,ii.cInvStd,isnull(doq.strQQName,'') strQQName,bb.iSum,do.datBillTime,do.datAuditordTime  from SO_SOMain aa inner join SO_SODetails bb on aa.ID=bb.ID left join Person pp on aa.cPersonCode=pp.cPersonCode left join Inventory ii on bb.cInvCode=ii.cInvCode left join Dl_opOrder do on do.cSOCode=aa.cSOCode left join Dl_opQQService doq on doq.strOpUserId=do.strManagers  where aa.cSOCode=@strU8BillNo";
            string cmdText = "select bb.iTaxUnitPrice,  aa.cMaker,pp.cPersonName,case aa.cSCCode when '00' then '自提' when '01' then '配送' end cSCCode,isnull(aa.cMemo,'') 'cMemo',isnull(aa.cdefine9,'')+' '+isnull(aa.cdefine12,'')+' '+isnull(aa.cdefine11,'')+' '+isnull(aa.cdefine1,'')+' '+isnull(aa.cdefine13,'')+' '+isnull(aa.cdefine2,'')+' '+isnull(aa.cdefine10,'')+' '+isnull(aa.cdefine3,'') 'cdefine11',bb.cInvName,bb.iQuantity,bb.cDefine22,ii.cInvStd,isnull(doq.strQQName,'') strQQName,bb.iSum,do.datBillTime,do.datAuditordTime  from SO_SOMain aa inner join SO_SODetails bb on aa.ID=bb.ID left join Person pp on aa.cPersonCode=pp.cPersonCode left join Inventory ii on bb.cInvCode=ii.cInvCode left join Dl_opOrder do on do.cSOCode=aa.cSOCode left join Dl_opQQService doq on doq.strOpUserId=do.strManagers  where aa.cSOCode=@strU8BillNo";

            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@strU8BillNo",strU8BillNo)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 查询历史订单中的参照订单数据[DL_OrderCZTSBillBySel]
        /// <summary>
        /// 查询历史订单中的参照订单数据[DL_OrderCZTSBillBySel]
        /// </summary>
        /// <param name="strU8BillNo">U8编号</param>
        /// <returns></returns>
        public DataTable DL_OrderCZTSBillBySel(string strU8BillNo)
        {
            DataTable dt = new DataTable();
            string cmdText = "SELECT bb.iTaxUnitPrice,cc.strLoginName cMaker,aa.cpersoncode cPersonName,case aa.cSCCode when '00' then '自提' when '01' then '配送' end cSCCode,aa.strRemarks cMemo,aa.cdefine11,bb.cinvcode cInvCode,bb.cinvname cInvName,bb.iquantity iQuantity,bb.cDefine22,dd.cInvStd,'' strQQName,bb.iSum,aa.datBillTime,aa.datAuditordTime FROM dl_oporder aa LEFT JOIN dbo.Dl_opOrderDetail bb ON aa.lngopOrderId=bb.lngopOrderId LEFT JOIN dbo.Dl_opUser cc ON cc.lngopUserId=aa.strManagers LEFT JOIN dbo.Inventory dd ON dd.cInvCode=bb.cinvcode WHERE aa.strBillNo=@strU8BillNo ";

            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@strU8BillNo",strU8BillNo)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 新增酬宾预订单表头[DLproc_NewYOrderByIns]
        /// <summary>
        /// 新增酬宾预订单表头[DLproc_NewYOrderByIns]
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
        public DataTable DLproc_NewYOrderByIns(string ddate, string lngopUserId, int bytStatus, string ccuscode, string ccusname, int lngBillType, string cdiscountname, string cMemo, string lngopUserExId, string strAllAcount,string datDeliveryDate)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_NewYOrderByIns";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@ddate",ddate),
            new SqlParameter("@lngopUserId",lngopUserId),
            new SqlParameter("@bytStatus",bytStatus),
            new SqlParameter("@ccuscode",ccuscode),
            new SqlParameter("@ccusname",ccusname),
            new SqlParameter("@lngBillType",lngBillType),
            new SqlParameter("@cdiscountname",cdiscountname),
            new SqlParameter("@cMemo",cMemo),
            new SqlParameter("@lngopUserExId",lngopUserExId),
            new SqlParameter("@strAllAcount",strAllAcount)
            ,new SqlParameter("@datDeliveryDate",datDeliveryDate)         
            };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 新增订单表体[DLproc_NewYOrderDetailByIns]
        public bool DLproc_NewYOrderDetailByIns(OrderInfo oi)
        {
            bool flag = false;
            string cmdText = "DLproc_NewYOrderDetailByIns";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@lngPreOrderId",oi.LngopOrderId),
            new SqlParameter("@cinvcode",oi.Cinvcode),
            new SqlParameter("@iquantity",oi.Iquantity),
            new SqlParameter("@inum",oi.Inum),
            new SqlParameter("@iquotedprice",oi.Iquotedprice),
            new SqlParameter("@iunitprice",oi.Iunitprice),
            new SqlParameter("@itaxunitprice",oi.Itaxunitprice),
            new SqlParameter("@imoney",oi.Imoney),
            new SqlParameter("@itax",oi.Itax),
            new SqlParameter("@isum",oi.Isum),
            new SqlParameter("@inatunitprice",oi.Inatunitprice),
            new SqlParameter("@inatmoney",oi.Inatmoney),
            new SqlParameter("@inattax",oi.Inattax),
            new SqlParameter("@inatsum",oi.Inatsum),
            new SqlParameter("@kl",oi.Kl),
            new SqlParameter("@itaxrate",oi.Itaxrate),
            new SqlParameter("@cdefine22",oi.Cdefine22),
            new SqlParameter("@iinvexchrate",oi.Iinvexchrate),
            new SqlParameter("@cunitid",oi.Cunitid),
            new SqlParameter("@irowno",oi.Irowno),
            new SqlParameter("@cinvname",oi.Cinvname),
            new SqlParameter("@idiscount",oi.Idiscount),
            new SqlParameter("@inatdiscount",oi.Inatdiscount),
            new SqlParameter("@cComUnitName",oi.CComUnitName),
            new SqlParameter("@cInvDefine1",oi.CInvDefine1),
            new SqlParameter("@cInvDefine2",oi.CInvDefine2),
            new SqlParameter("@cInvDefine13",oi.CInvDefine13),
            new SqlParameter("@cInvDefine14",oi.CInvDefine14),
            new SqlParameter("@unitGroup",oi.UnitGroup),
            new SqlParameter("@cComUnitQTY",oi.CComUnitQTY),
            new SqlParameter("@cInvDefine1QTY",oi.CInvDefine1QTY),
            new SqlParameter("@cInvDefine2QTY",oi.CInvDefine2QTY),
            new SqlParameter("@cn1cComUnitName",oi.Cn1cComUnitName),
            new SqlParameter("@cDefine24",oi.CDefine24)
            };
            int res = sqlhelper.ExecuteNonQuery(cmdText, paras, CommandType.StoredProcedure);
            if (res > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion

        #region 获取预订单(状态bytStatus,单据类型lngBillType)[DLproc_UnauditedpreOrderBySel]
        /// <summary>
        ///  获取预订单(状态bytStatus,单据类型lngBillType)[DLproc_UnauditedpreOrderBySel]
        /// </summary>
        /// <param name="bytStatus"></param>
        /// <returns></returns>
        public DataTable DLproc_UnauditedpreOrderBySel(int bytStatus, int lngBillType)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_UnauditedpreOrderBySel";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@bytStatus",bytStatus),
           new SqlParameter("@lngBillType",lngBillType)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 新增顾客对账单[DL_NewSOAByIns]
        public bool DL_NewSOAByIns(string ccuscode, string ccusname, string strEndDate, double dblAmount, string strUper, string strOper, string strOperName, int intPeriod)
        {
            bool flag = false;
            string cmdText = "insert into Dl_opU8SOA (ccuscode,ccusname,strEndDate,datSendTime,dblAmount,strUper,strOper,strOperName,bytCheck,datCheckTime,intperiodid,intPeriod,intPeriodYear) select @ccuscode,@ccusname,@strEndDate,GETDATE(),@dblAmount,@strUper,@strOper,@strOperName,0,'1900-01-01',0,@intPeriod,year(@strEndDate)";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@ccuscode",ccuscode),
            new SqlParameter("@ccusname",ccusname),
            new SqlParameter("@strEndDate",strEndDate),
            new SqlParameter("@dblAmount",dblAmount),
            new SqlParameter("@strUper",strUper),
            new SqlParameter("@strOper",strOper),
            new SqlParameter("@strOperName",strOperName),
            new SqlParameter("@intPeriod",intPeriod)
            };
            int res = sqlhelper.ExecuteNonQuery(cmdText, paras, CommandType.Text);
            if (res > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion

        #region 顾客确认对账单[DL_ConfimSOAByUpd]
        public bool DL_ConfimSOAByUpd(string id)
        {
            bool flag = false;
            string cmdText = "print 'start';update Dl_opU8SOA set bytCheck=1,datCheckTime=GETDATE() where lngSOAid=@id";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@id",id)
            };
            int res = sqlhelper.ExecuteNonQuery(cmdText, paras, CommandType.Text);
            if (res > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion

        #region 返回酬宾订单控制参数(活动是否开启)[DL_PreOrderSettingBySel]
        public bool DL_PreOrderSettingBySel(string cCusCode)
        {
            bool flag = false;
            //string cmdText = "select 1 from Dl_opPreOrderControl where IsEnable=1 and IsTimeControl=1 and ( getdate() between  datStartTime and datEndTime) union all select 2 from Customer where cCusDefine1 = 1 and cCusCode = @cCusCode";
            string cmdText = "select 1 from Dl_opPreOrderControl where IsEnable=1 and IsTimeControl=1 and ( getdate() between  datStartTime and datEndTime) and (strcCusCode='000000' or strcCusCode=@cCusCode) union all select 2 from Customer where cCusDefine1 = 1 and cCusCode = @cCusCode";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@cCusCode",cCusCode)
            };
            DataTable res = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            if (res.Rows.Count > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion

        #region  获取对应的U8订单编号,用于发送给顾客[DL_OrderBillNoForU8OrderBillNoByIns]
        /// <summary>
        /// 获取对应的U8订单编号,用于发送给顾客[DL_OrderBillNoForU8OrderBillNoByIns]
        /// </summary>
        /// <param name="strBillNo">网单号</param>
        /// <returns></returns>
        public DataTable DL_OrderBillNoForU8OrderBillNoByIns(string strBillNo)
        {
            string cmdText = "select cSOCode from Dl_opOrder where strBillNo=@strBillNo";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@strBillNo",strBillNo)
            };
            DataTable dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
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
            DataTable dt = new DataTable();
            string cmdText = "DLproc_QuasiPriceBySel";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@cinvcode",cinvcode),
           new SqlParameter("@ccuscode",ccuscode)       
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
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
            DataTable dt = new DataTable();
            string cmdText = "select ccusname,strRemarks,cSCCode,cdefine11,cpersoncode,strLoadingWays,cdefine3,lngopUserId,ccuscode,lngopUseraddressId,cdefine9,cdefine12,cdefine1,cdefine13,cdefine2,cdefine10,bb.cinvcode from Dl_opOrder aa left join Dl_opOrderDetail bb on aa.lngopOrderId=bb.lngopOrderId where aa.strBillNo=@strBillNo";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@strBillNo",strBillNo),
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
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
            DataTable dt = new DataTable();
            string cmdText = "DLproc_ReferencePreviousOrderWithCusInvLimitedBySel";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@strBillNo",strBillNo),
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
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
            bool flag = false;
            string cmdText = "select lngopOrderId from dl_oporder where strBillNo in ( select top(1) RelateU8NO from dl_oporder where strBillNo=@strBillNo) and len(cSOCode)>5  and bytStatus=4";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@strBillNo",strBillNo),
           };
            DataTable dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            if (dt.Rows.Count > 0)
            {
                flag = true;
            }
            return flag;
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
            bool flag = false;
            string sql = "DLproc_TreeListSingleCustomerByIns";
            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@ccuscode",ccuscode)
            };
            int res = sqlhelper.ExecuteNonQuery(sql, paras, CommandType.StoredProcedure);
            if (res > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion

        #region 更新所有允限销分类表[DL_CodeClassByUp]
        /// <summary>
        /// 更新所有允限销分类表[DL_CodeClassByUp]
        /// </summary>
        /// <returns></returns>
        public bool DL_CodeClassByUp()
        {
            bool flag = false;
            string sql = "DLproc_TreeListByIns";
            int res = sqlhelper.ExecuteNonQuery(sql, CommandType.StoredProcedure);
            if (res > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion

        #region 新增加临时订单表头[DLproc_AddOrderBackByIns]
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
        public DataTable DLproc_AddOrderBackByIns(string lngopUserId, string strBillName, int bytStatus, string strRemarks, string ccuscode, string cdefine1, string cdefine2, string cdefine3, string cdefine9, string cdefine10, string cdefine11, string cdefine12, string cdefine13, string ccusname, string cpersoncode, string cSCCode, string strLoadingWays, string cSTCode, string lngopUseraddressId, string RelateU8NO, int lngBillType, string cWhCode)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_AddOrderBack_All_Warehouse_ByIns";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@lngopUserId",lngopUserId),
           new SqlParameter("@strBillName",strBillName),
           new SqlParameter("@bytStatus",bytStatus),
           new SqlParameter("@strRemarks",strRemarks),
           new SqlParameter("@ccuscode",ccuscode),
           new SqlParameter("@cdefine1",cdefine1),
           new SqlParameter("@cdefine2",cdefine2),
           new SqlParameter("@cdefine3",cdefine3),
           new SqlParameter("@cdefine9",cdefine9),
           new SqlParameter("@cdefine10",cdefine10),
           new SqlParameter("@cdefine11",cdefine11),
           new SqlParameter("@cdefine12",cdefine12),
           new SqlParameter("@cdefine13",cdefine13),
           new SqlParameter("@ccusname",ccusname),
           new SqlParameter("@cpersoncode",cpersoncode),
           new SqlParameter("@cSCCode",cSCCode),
           new SqlParameter("@strLoadingWays",strLoadingWays),
           new SqlParameter("@cSTCode",cSTCode),
           new SqlParameter("@lngopUseraddressId",lngopUseraddressId),
           new SqlParameter("@RelateU8NO",RelateU8NO),
           new SqlParameter("@lngBillType",lngBillType),
           new SqlParameter("@chdefine51",cWhCode)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 新增加临时订单表体[DLproc_AddOrderBackDetailByIns]
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
        /// <param name="irowno"></param>
        /// <returns></returns>
        public bool DLproc_AddOrderBackDetailByIns(int lngopOrderBackId, string cinvcode, string cinvname, double cComUnitQTY, double cInvDefine1QTY, double cInvDefine2QTY, double cInvDefineQTY, double cComUnitAmount, string pack, string irowno)
        {
            bool flag = false;
            string cmdText = "DLproc_AddOrderBackDetailByIns";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@lngopOrderBackId",lngopOrderBackId),
           new SqlParameter("@cinvcode",cinvcode),
           new SqlParameter("@cinvname",cinvname),
           new SqlParameter("@cComUnitQTY",cComUnitQTY),
           new SqlParameter("@cInvDefine1QTY",cInvDefine1QTY),
           new SqlParameter("@cInvDefine2QTY",cInvDefine2QTY),
           new SqlParameter("@cInvDefineQTY",cInvDefineQTY),
           new SqlParameter("@cComUnitAmount",cComUnitAmount),           
           new SqlParameter("@pack",pack),
           new SqlParameter("@irowno",irowno)
           
           };
            int res = sqlhelper.ExecuteNonQuery(cmdText, paras, CommandType.StoredProcedure);
            if (res > 0)
            {
                flag = true;
            }
            return flag;
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
            string cmdText = "select strBillName,datBillTime,lngopOrderBackId from Dl_opOrderBack where lngopUserId=@lngopUserId order by datBillTime desc ";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@lngopUserId",lngopUserId)
            };
            DataTable dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 获取顾客对应的临时订单明细[DLproc_ReferenceOrderBackWithCusInvLimitedBySel]
        /// <summary>
        /// 获取顾客对应的临时订单明细[DLproc_ReferenceOrderBackWithCusInvLimitedBySel]
        /// </summary>
        /// <param name="lngopOrderBackId">临时订单内码</param>
        /// <returns></returns>
        public DataTable DLproc_BackOrderandPrvOrdercInvCodeIsBeLimitedBySel(string lngopOrderBackId, int iShowType, int iBillType)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_BackOrderandPrvOrdercInvCodeIsBeLimited_All_Warehouse_BySel";
            SqlParameter[] paras = new SqlParameter[] { 
            new SqlParameter("@id",lngopOrderBackId),
            new SqlParameter("@iShowType",iShowType),
            new SqlParameter("@iBillType",iBillType)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
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
            bool flag = false;
            string cmdText = "delete from Dl_opOrderBack where lngopOrderBackId=@lngopOrderBackId delete from Dl_opOrderBackDetail where lngopOrderBackId=@lngopOrderBackId";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@lngopOrderBackId",lngopOrderBackId)
           };
            int res = sqlhelper.ExecuteNonQuery(cmdText, paras, CommandType.Text);
            if (res > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion

        #region 删除自动保存的临时订单信息[DLproc_DelAutoSaveOrderBackByDel]
        /// <summary>
        /// 删除自动保存的临时订单信息[DLproc_DelAutoSaveOrderBackByDel]
        /// </summary>
        /// <returns></returns>
        public bool DLproc_DelAutoSaveOrderBackByDel(int lngopUserId)
        {
            bool flag = false;
            string cmdText = "DLproc_DelAutoSaveOrderBackByDel";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@lngopUserId",lngopUserId)
           };
            int res = sqlhelper.ExecuteNonQuery(cmdText, paras, CommandType.StoredProcedure);
            if (res > 0)
            {
                flag = true;
            }
            return flag;
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
            DataTable dt = new DataTable();
            string cmdText = "select aa.ddate,aa.ccusname,aa.ccuscode,aa.datBillTime,bb.cinvcode,bb.cinvname,cc.cInvStd,bb.UnitGroup,bb.iquantity,bb.cDefine22,bb.iquotedprice,bb.iquotedprice*bb.iquantity cComUnitAmount,bb.itaxunitprice,bb.isum xx from Dl_opPreOrder aa left join Dl_opPreOrderDetail bb on aa.lngPreOrderId=bb.lngPreOrderId left join Inventory cc on bb.cinvcode=cc.cInvCode where strBillNo=@strBillNo";

            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@strBillNo",strBillNo)
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 驳回提交的订单(顾客自取回，在接单员接收处理之前)[DL_RejectOrderBillSelfByUpd]
        public bool DL_RejectOrderBillSelfByUpd(string strBillNo, string strManagers)
        {
            bool flag = false;
            string cmdText = "update Dl_opOrder set bytStatus=3,strManagers=@strManagers,datRejectTime=GETDATE() where strBillNo=@strBillNo and len(strManagers)<2";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@strBillNo",strBillNo),           
            new SqlParameter("@strManagers",strManagers)
            };
            int res = sqlhelper.ExecuteNonQuery(cmdText, paras, CommandType.Text);
            if (res > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion

        #region 查询当月前未确认的订单（如不确认则不能下单）[DL_NotConfirmedSOABySel]
        public DataTable DL_NotConfirmedSOABySel(string ccuscod)
        {
            DataTable dt = new DataTable();
            string cmdText = @"SELECT COUNT(*) AS ccount ,'账单' AS 'type'  FROM Dl_opU8SOA WHERE datediff(month,datSendTime,GETDATE())>0 and bytCheck=0 and ccuscode like @ccuscod 
                                UNION  
                                SELECT COUNT(*) ,'延期欠款通知单' FROM dbo.Dl_opArrear WHERE ((bytStatus=21 AND  datediff(MINUTE,dReviewDate,GETDATE())>2880 ) 
                                OR bytStatus IN (22,32,43) ) AND ccuscode like @ccuscod";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@ccuscod",ccuscod)
            };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 查询当前是否开启订单开放时间管理[DL_OrderENTimeControlBySel]
        public DataTable DL_OrderENTimeControlBySel()
        {
            DataTable dt = new DataTable();
            string cmdText = "select * from Dl_opSystemConfiguration where convert(time,getdate()) between OrderBeginTime and OrderEndTime and OrderTimeControl=1";
            dt = sqlhelper.ExecuteQuery(cmdText, CommandType.Text);
            return dt;
        }
        #endregion

        #region 查询是否存在未参照完的存货，返回数量[DLproc_PerOrderCinvcodeLeftBySel]

        public DataTable DLproc_PerOrderCinvcodeLeftBySel(string lngopUserExId, string lngopUserId, string ccuscode, string cinvcode)
        {
            string cmdText = "DLproc_PerOrderCinvcodeLeftBySel";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@lngopUserExId",lngopUserExId),
           new SqlParameter("@lngopUserId",lngopUserId),
           new SqlParameter("@ccuscode",ccuscode),
           new SqlParameter("@cinvcode",cinvcode)           
           };
            DataTable dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
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
            DataTable dt = new DataTable();
            string cmdText = "DLproc_PreOrderSubmitForCheckBySel";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@PreOrderCheck",PreOrderCheck)     
           };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 查询当前订单是否存在过期[DL_OrderIsExpBySel]
        public bool DL_OrderIsExpBySel(string strBillNo)
        {
            DataTable dt = new DataTable();
            bool flag = false;
            string cmdText = "select 1 from Dl_opOrder where strBillNo=@strBillNo and DATEDIFF(MI,GETDATE(),ISNULL(datExpTime,GETDATE()))>=0";
            SqlParameter[] paras = new SqlParameter[] { 
            new SqlParameter("@strBillNo",strBillNo)     
            };
            dt = sqlhelper.ExecuteQuery(cmdText, CommandType.Text);
            if (dt.Rows.Count > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion

    }
}
