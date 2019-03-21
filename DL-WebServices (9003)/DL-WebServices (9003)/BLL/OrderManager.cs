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

        #region 查询网上订单普通订单表数据（包含表头，扩展表头，表体）[DLproc_NewOrderU8BySel]
        /// <summary>
        /// 查询网上订单普通订单表数据（包含表头，扩展表头，表体）[DLproc_NewOrderU8BySel]
        /// </summary>
        /// <returns></returns>
        public DataTable DLproc_NewOrderU8BySel(string strBillNo)
        {
            return odao.DLproc_NewOrderU8BySel(strBillNo);
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

        #region 查询入库单表体数据，生成形态转换单表体数据[DL_NewOrderToDispBU8BySel]
        /// <summary>
        /// 查询入库单表体数据，生成形态转换单表体数据[DL_NewOrderToDispBU8BySel]
        /// </summary>
        /// <returns></returns>
        public DataTable DLproc_rdrecord10_to_AssemVouchBySel(string strBillNo)
        {
            return odao.DLproc_rdrecord10_to_AssemVouchBySel(strBillNo);
        }
        #endregion

        #region 写入错误信息[DL_ErrByIns]
        /// <summary>
        /// 写入错误信息[DL_ErrByIns]
        /// </summary>
        /// <returns></returns>
        public bool DL_ErrByIns(string strBillNo, string Err)
        {
            return odao.DL_ErrByIns(strBillNo, Err);
        }
        #endregion

        #region 查询U8订单表头数据(不包含表头扩展表数据)[DL_U8OrderDataBySel]
        /// <summary>
        /// 查询U8订单表头数据(不包含表头扩展表数据)[DL_U8OrderDataBySel]
        /// </summary>
        /// <returns></returns>
        public DataTable DL_U8OrderDataBySel(string U8csocode)
        {
            return odao.DL_U8OrderDataBySel(U8csocode);
        }
        #endregion


    }
}
