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

     

        #region 查询当前订单是否存在过期[DL_OrderIsExpBySel]
        public bool DL_OrderIsExpBySel(string strBillNo)
        {
            DataTable dt = new DataTable();
            bool flag = false;
            string cmdText = "select '1' sl from Dl_opOrder where (DATEDIFF(MI,GETDATE(),ISNULL(datExpTime,GETDATE()))>=0) and strBillNo = @strBillNo ";
            SqlParameter[] paras = new SqlParameter[] { 
            new SqlParameter("@strBillNo",strBillNo)     
            };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            if (dt.Rows.Count > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion


        #region 查询参照特殊订单查询-表头[DL_NewOrderToDispHU8BySel]
        public DataTable DL_NewOrderToDispHU8BySel(string strBillNo)
        {
            DataTable dt = new DataTable();

            string cmdText = @"SELECT '||SA01|' + aa.strBillNo csysbarcode,aa.strBillNo cdlcode,'04' cstcode,CONVERT(varchar(10), GETDATE(), 120) ddate,     
'TS' + RIGHT(bb.cpreordercode, 11) csocode,aa.ccuscode ccuscode,csccode csccode,aa.strRemarks cmemo,cdefine2 cdefine2,0 breturnflag,       
0 dlid,ISNULL(cc.strUserName, '王俊杰') cmaker,cdefine3 cdefine3,CONVERT(VARCHAR(50),aa.datDeliveryDate,120) cdefine4,cdefine9,cdefine10,cdefine11,cdefine12,cdefine13,       
aa.strLoadingWays cdefine14,aa.ccusname ccusname,aa.ccuscode cinvoicecompany,0 bsaleoutcreatebill,N'普通销售' cbustype,131397 ivtid,N'05' cvouchtype,        
c.cCusDepart cdepcode,c.cCusPPerson cpersoncode,N'人民币' cexch_name,1 iexchrate,17 itaxrate,0 bfirst,0 sbvid,0 isale,0 iflowid,0 bcredit,0 bmustbook,       
0 iverifystate,1 iswfcontrolled,0 bcashsale,0 bsigncreate,1 bneedbill,0 baccswitchflag,0 bnottogoldtax         
FROM dbo.Dl_opOrder aa LEFT JOIN dbo.Dl_opOrderDetail bb ON aa.lngopOrderId = bb.lngopOrderId          
LEFT JOIN dbo.Dl_opUser cc ON cc.lngopUserId = aa.strManagers LEFT JOIN Customer c ON c.cCusCode=aa.ccuscode WHERE AA.strBillNo=@strBillNo ";
            SqlParameter[] paras = new SqlParameter[] { 
            new SqlParameter("@strBillNo",strBillNo)     
            };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion


        #region 查询参照特殊订单查询-表体-非金花,大井[DL_NewOrderToDispBU8BySel]
        public DataTable DL_NewOrderToDispBU8BySel(string strBillNo)
        {
            DataTable dt = new DataTable();

            string cmdText = @"SELECT dlid,cwhcode,cinvcode,iquantity,inum,iquotedprice,iunitprice,itaxunitprice,imoney,itax,isum,idiscount, 
             inatunitprice,inatmoney,inattax,inatsum,inatdiscount,isosid,idlsid,kl,kl2,cinvname,cdefine22, 
             cdefine27,iinvexchrate,cunitid,csocode,cordercode,iorderrowno,irowno,idemandtype, 
             cdemandcode,cdemandid,idemandseq,cbsysbarcode + '|' + CONVERT(varchar(10), irowno) cbsysbarcode,0 itb,0 tbquantity,17 itaxrate, 
             0 fsaleprice,0 bgsp,0 cmassunit,0 bqaneedcheck,0 bqaurgency,1 bcosting,0 fcusminprice,0 iexpiratdatecalcu, 
             1 bneedsign,0 frlossqty,1 bsaleprice,0 bgift,0 bmpforderclosed,0 biacreatebill,bb.iGroupType,bb.cGroupCode,cDefine25
             FROM (SELECT dlid,cwhcode,cinvcode,iquantity,inum,iquotedprice,iunitprice,itaxunitprice,imoney,itax,isum, 
             idiscount,inatunitprice,inatmoney,inattax,inatsum,inatdiscount,isosid,idlsid,kl,kl2,cinvname,cdefine22, 
             cdefine23,cdefine27,iinvexchrate,cunitid,csocode,cordercode,iorderrowno,aa.cDefine25,
             ROW_NUMBER() OVER (ORDER BY aa.dlid ASC) AS irowno,idemandtype,cdemandcode,cdemandid,idemandseq,cbsysbarcode,aa.iGroupType,aa.cGroupCode
             FROM (SELECT 0 dlid,cc.cDefWareHouse cwhcode,bb.cinvcode,bb.iquantity,bb.inum,bb.iquotedprice,bb.iunitprice, 
             bb.itaxunitprice,bb.imoney,bb.itax,bb.isum,bb.idiscount,bb.inatunitprice,bb.inatmoney,bb.inattax,bb.inatsum, 
             bb.inatdiscount,dd.isosid,0 idlsid,dd.kl,dd.kl2,bb.cinvname,bb.cdefine22,dd.cdefine23,dd.cdefine27, 
             dd.iinvexchrate,dd.cunitid,dd.csocode,dd.csocode cordercode,dd.iRowNo iorderrowno,dd.idemandtype,dd.cDefine25,
             dd.cSOCode cdemandcode,dd.iSOsID cdemandid,dd.iRowNo idemandseq,'||SA01|'  cbsysbarcode,cc.iGroupType,cc.cgroupcode 
             FROM dbo.Dl_opOrder aa LEFT JOIN dbo.Dl_opOrderDetail bb ON aa.lngopOrderId = bb.lngopOrderId  
             LEFT JOIN inventory cc ON cc.cInvCode = bb.cinvcode LEFT JOIN dbo.SO_SODetails dd  
             ON dd.cSOCode = 'TS' + RIGHT(bb.cpreordercode, 11) AND dd.iaoids = bb.iaoids  
             WHERE aa.strBillNo = @strBillNo AND cc.cDefWareHouse != '0501' and cc.cDefWareHouse !='0301')AS aa)AS bb";
            SqlParameter[] paras = new SqlParameter[] { 
            new SqlParameter("@strBillNo",strBillNo)     
            };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 查询参照特殊订单查询-表体-金花[DL_NewOrderToDispB_JH_U8BySel]
        public DataTable DL_NewOrderToDispB_JH_U8BySel(string strBillNo)
        {
            DataTable dt = new DataTable();

            string cmdText = @"SELECT dlid,cwhcode,cinvcode,iquantity,inum,iquotedprice,iunitprice,itaxunitprice,imoney,itax,isum,idiscount, 
             inatunitprice,inatmoney,inattax,inatsum,inatdiscount,isosid,idlsid,kl,kl2,cinvname,cdefine22, 
             cdefine27,iinvexchrate,cunitid,csocode,cordercode,iorderrowno,irowno,idemandtype, 
             cdemandcode,cdemandid,idemandseq,cbsysbarcode + '|' + CONVERT(varchar(10), irowno) cbsysbarcode,0 itb,0 tbquantity,17 itaxrate, 
             0 fsaleprice,0 bgsp,0 cmassunit,0 bqaneedcheck,0 bqaurgency,1 bcosting,0 fcusminprice,0 iexpiratdatecalcu, 
             1 bneedsign,0 frlossqty,1 bsaleprice,0 bgift,0 bmpforderclosed,0 biacreatebill,bb.iGroupType,bb.cGroupCode,cDefine25
             FROM (SELECT dlid,cwhcode,cinvcode,iquantity,inum,iquotedprice,iunitprice,itaxunitprice,imoney,itax,isum, 
             idiscount,inatunitprice,inatmoney,inattax,inatsum,inatdiscount,isosid,idlsid,kl,kl2,cinvname,cdefine22, 
             cdefine23,cdefine27,iinvexchrate,cunitid,csocode,cordercode,iorderrowno,aa.cDefine25,
             ROW_NUMBER() OVER (ORDER BY aa.dlid ASC) AS irowno,idemandtype,cdemandcode,cdemandid,idemandseq,cbsysbarcode,aa.iGroupType,aa.cGroupCode
             FROM (SELECT 0 dlid,cc.cDefWareHouse cwhcode,bb.cinvcode,bb.iquantity,bb.inum,bb.iquotedprice,bb.iunitprice, 
             bb.itaxunitprice,bb.imoney,bb.itax,bb.isum,bb.idiscount,bb.inatunitprice,bb.inatmoney,bb.inattax,bb.inatsum, 
             bb.inatdiscount,dd.isosid,0 idlsid,dd.kl,dd.kl2,bb.cinvname,bb.cdefine22,dd.cdefine23,dd.cdefine27, 
             dd.iinvexchrate,dd.cunitid,dd.csocode,dd.csocode cordercode,dd.iRowNo iorderrowno,dd.idemandtype,dd.cDefine25,
             dd.cSOCode cdemandcode,dd.iSOsID cdemandid,dd.iRowNo idemandseq,'||SA01|'  cbsysbarcode,cc.iGroupType,cc.cgroupcode 
             FROM dbo.Dl_opOrder aa LEFT JOIN dbo.Dl_opOrderDetail bb ON aa.lngopOrderId = bb.lngopOrderId  
             LEFT JOIN inventory cc ON cc.cInvCode = bb.cinvcode LEFT JOIN dbo.SO_SODetails dd  
             ON dd.cSOCode = 'TS' + RIGHT(bb.cpreordercode, 11) AND dd.iaoids = bb.iaoids  
             WHERE aa.strBillNo = @strBillNo AND cc.cDefWareHouse = '0501')AS aa)AS bb";
            SqlParameter[] paras = new SqlParameter[] { 
            new SqlParameter("@strBillNo",strBillNo)     
            };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 查询参照特殊订单查询-表体-大井[DL_NewOrderToDispB_DJ_U8BySel]
        public DataTable DL_NewOrderToDispB_DJ_U8BySel(string strBillNo)
        {
            DataTable dt = new DataTable();

            string cmdText = @"SELECT dlid,cwhcode,cinvcode,iquantity,inum,iquotedprice,iunitprice,itaxunitprice,imoney,itax,isum,idiscount, 
             inatunitprice,inatmoney,inattax,inatsum,inatdiscount,isosid,idlsid,kl,kl2,cinvname,cdefine22, 
             cdefine27,iinvexchrate,cunitid,csocode,cordercode,iorderrowno,irowno,idemandtype, 
             cdemandcode,cdemandid,idemandseq,cbsysbarcode + '|' + CONVERT(varchar(10), irowno) cbsysbarcode,0 itb,0 tbquantity,17 itaxrate, 
             0 fsaleprice,0 bgsp,0 cmassunit,0 bqaneedcheck,0 bqaurgency,1 bcosting,0 fcusminprice,0 iexpiratdatecalcu, 
             1 bneedsign,0 frlossqty,1 bsaleprice,0 bgift,0 bmpforderclosed,0 biacreatebill,bb.iGroupType,bb.cGroupCode,cDefine25
             FROM (SELECT dlid,cwhcode,cinvcode,iquantity,inum,iquotedprice,iunitprice,itaxunitprice,imoney,itax,isum, 
             idiscount,inatunitprice,inatmoney,inattax,inatsum,inatdiscount,isosid,idlsid,kl,kl2,cinvname,cdefine22, 
             cdefine23,cdefine27,iinvexchrate,cunitid,csocode,cordercode,iorderrowno,aa.cDefine25,
             ROW_NUMBER() OVER (ORDER BY aa.dlid ASC) AS irowno,idemandtype,cdemandcode,cdemandid,idemandseq,cbsysbarcode,aa.iGroupType,aa.cGroupCode
             FROM (SELECT 0 dlid,cc.cDefWareHouse cwhcode,bb.cinvcode,bb.iquantity,bb.inum,bb.iquotedprice,bb.iunitprice, 
             bb.itaxunitprice,bb.imoney,bb.itax,bb.isum,bb.idiscount,bb.inatunitprice,bb.inatmoney,bb.inattax,bb.inatsum, 
             bb.inatdiscount,dd.isosid,0 idlsid,dd.kl,dd.kl2,bb.cinvname,bb.cdefine22,dd.cdefine23,dd.cdefine27, 
             dd.iinvexchrate,dd.cunitid,dd.csocode,dd.csocode cordercode,dd.iRowNo iorderrowno,dd.idemandtype,dd.cDefine25,
             dd.cSOCode cdemandcode,dd.iSOsID cdemandid,dd.iRowNo idemandseq,'||SA01|'  cbsysbarcode,cc.iGroupType,cc.cgroupcode 
             FROM dbo.Dl_opOrder aa LEFT JOIN dbo.Dl_opOrderDetail bb ON aa.lngopOrderId = bb.lngopOrderId  
             LEFT JOIN inventory cc ON cc.cInvCode = bb.cinvcode LEFT JOIN dbo.SO_SODetails dd  
             ON dd.cSOCode = 'TS' + RIGHT(bb.cpreordercode, 11) AND dd.iaoids = bb.iaoids  
             WHERE aa.strBillNo = @strBillNo AND cc.cDefWareHouse = '0301')AS aa)AS bb";
            SqlParameter[] paras = new SqlParameter[] { 
            new SqlParameter("@strBillNo",strBillNo)     
            };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 更新网上订单的参照订单状态为已审核[DL_CZTSOrderAuthByUpd]
        public bool DL_CZTSOrderAuthByUpd(string strBillNo)
        {
            DataTable dt = new DataTable();
            bool flag = false;
            string cmdText = "UPDATE dbo.Dl_opOrder SET bytStatus=4 ,datAuditordTime=GETDATE() WHERE strBillNo= @strBillNo ";
            SqlParameter[] paras = new SqlParameter[] { 
            new SqlParameter("@strBillNo",strBillNo)     
            };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            if (dt.Rows.Count > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion

        #region 更新审核的参照特殊订单生成的U8发货单的流水号[DLproc_CZTSFHDLSHByUpd]
        public bool DLproc_CZTSFHDLSHByUpd(string strBillNo)
        {
            DataTable dt = new DataTable();
            bool flag = false;
            string cmdText = "DLproc_CZTSFHDLSHByUpd";
            SqlParameter[] paras = new SqlParameter[] { 
            new SqlParameter("@strBillNo",strBillNo)     
            };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            if (dt.Rows.Count > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion

        #region 查询入库单表体数据，生成形态转换单表体数据[DL_NewOrderToDispBU8BySel]
        /// <summary>
        /// 查询入库单表体数据，生成形态转换单表体数据[DL_NewOrderToDispBU8BySel]
        /// </summary>
        /// <returns></returns>
        public DataTable DLproc_rdrecord10_to_AssemVouchBySel(string strBillNo)
        {
            DataTable dt = new DataTable();

            string cmdText = "DLproc_rdrecord10_to_AssemVouchBySel";
            SqlParameter[] paras = new SqlParameter[] { 
            new SqlParameter("@strBillNo",strBillNo)     
            };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 查询网上订单普通订单表数据（包含表头，扩展表头，表体）[DLproc_NewOrderU8BySel]
        /// <summary>
        /// 查询网上订单普通订单表数据（包含表头，扩展表头，表体）[DLproc_NewOrderU8BySel]
        /// </summary>
        /// <returns></returns>
        public DataTable DLproc_NewOrderU8BySel(string strBillNo)
        {
            DataTable dt = new DataTable();

            string cmdText = "DLproc_NewOrderU8BySel";
            SqlParameter[] paras = new SqlParameter[] { 
            new SqlParameter("@strBillNo",strBillNo)     
            };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 查询网上订单特殊订单表数据（包含表头，扩展表头，表体）[DLproc_NewYOrderU8_TSBySel]
        /// <summary>
        /// 查询网上订单特殊订单表数据（包含表头，扩展表头，表体）[DLproc_NewYOrderU8_TSBySel]
        /// </summary>
        /// <returns></returns>
        public DataTable DLproc_NewYOrderU8_TSBySel(string strBillNo)
        {
            DataTable dt = new DataTable();

            string cmdText = "DLproc_NewYOrderU8_TSBySel";
            SqlParameter[] paras = new SqlParameter[] { 
            new SqlParameter("@strBillNo",strBillNo)     
            };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 写入错误信息[DL_ErrByIns]
        public bool DL_ErrByIns(string strBillNo, string Err)
        {
            bool flag = false;
            string cmdText = "INSERT INTO Dl_opSysError SELECT @strBillNo,@Err,getdate() ";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@strBillNo",strBillNo)  ,
            new SqlParameter("@Err",Err)     
            };
            int res = sqlhelper.ExecuteNonQuery(cmdText, paras, CommandType.Text);
            if (res > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion

        #region 查询U8订单表头数据(不包含表头扩展表数据)[DL_U8OrderDataBySel]
        public DataTable DL_U8OrderDataBySel(string U8csocode)
        {
            DataTable dt = new DataTable();
            string cmdText = @"SELECT * FROM dbo.SO_SOMain WHERE cSOCode=@U8csocode";
            SqlParameter[] paras = new SqlParameter[] { 
            new SqlParameter("@U8csocode",U8csocode)     
            };
            dt = sqlhelper.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion


    }
}
