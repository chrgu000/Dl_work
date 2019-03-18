using DlApp.Common;
using DlApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DlApp.Controllers
{
    /// <summary>
    /// 业务操作模块
    /// </summary>
    public class UFLogic
    {
        private Entities db = new Entities();

        /// <summary>
        /// 查询数据库配置表
        /// </summary>
        /// <param name="Key">Key</param>
        /// <returns></returns>
        public List<DL_U8_Options> getOptions(string Key)
        {
            List<DL_U8_Options> rs = db.DL_U8_Options.Where(u => u.key == Key).ToList();
            return rs;
        }

        /// <summary>
        /// 查询Web.config
        /// </summary>
        /// <param name="Key">Key</param>
        /// <returns></returns>
        public String getWebConfig(string Key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[Key];
        }

        /// <summary>
        /// 查询皮重管理表
        /// </summary>
        /// <returns></returns>
        public List<DL_U8_Tare> getTares()
        {
            return db.DL_U8_Tare.ToList();
        }

        /// Rewriter By A04  2019-02-28
        /// 原代码内容
        /// <summary>
        /// [U8]查询不合格品原因
        /// </summary>
        /// <returns></returns>
        //public List<Reason> getReasons()
        //{
        //    List<DL_U8_Options> ds = getOptions(Const.Option_Key8);
        //    string strSql = "";
        //    if (ds != null && ds.Count > 0)
        //    {
        //        string precode = ds.First().value;
        //        strSql = " and cReasonCode like '" + precode + "%'";
        //    }

        //    List<Reason> rs = db.Reason.SqlQuery("select * from reason where iReasontype = 1" + strSql).ToList();
        //    return rs;
        //}

        /// 新代码
        /// <summary>
        /// 查询不合格品原因
        /// </summary>
        /// <returns></returns>
        public List<U8CUSTDEF_0046_E001> getReasons()
        {
            List<U8CUSTDEF_0046_E001> rs = db.U8CUSTDEF_0046_E001.SqlQuery("SELECT * FROM dbo.U8CUSTDEF_0046_E001").ToList();
            return rs;
        }
        /// end

        /// <summary>
        /// [U8]查询指定人员的派工信息
        /// </summary>
        /// <param name="psnCode">人员编码</param>
        /// <returns></returns>
        //add 参数datebefore by chengyinghao
        public List<DL_U8_pgshift> getVPgShift(string psnCode,DateTime datebefore)
        {
            //取得产成品的判断条件
            //String strSql = "";
            //DateTime date = new DateTime(datebefore.Year, datebefore.Month, datebefore.Day);
            string date = datebefore.ToString("yyyy-MM-dd");   
            List<DL_U8_Options> ds = getOptions(Const.Option_Key2);
            //***********delete by chenyinghao  2017/5/31***不区分产品********************//
            //if (ds != null && ds.Count > 0)
            //{
            //    strSql += " and (";
            //    foreach (DL_U8_Options re in ds)
            //    {
            //        strSql += "cInvCCode LIKE '" + re.value + "%'";
            //        strSql += " or ";
            //    }
            //    strSql += "1=0)";
            //}
            //***********delete by chenyinghao  2017/5/31***********************//
            

            //取得派工单
            //视图没有主键，必须使用AsNoTracking，否则会把所有数据映射为第一条
            //检索条件1：人员编码
            //检索条件2：人员所属班组
            //检索条件3：派工时间检索 date， update by chenyinghao 2014.4.25

            
            //string sql = "select * from DL_U8_pgshift where  ([employcode]='" + psnCode + "'";
            ////sql = sql + " or dutyclasscode IN (select cDutyClass from DL_U8_vPersonDutyClass a where a.cPsn_Num='" + psnCode + "')";   ////查询当时排班信息中的排班   20180716
            //sql = sql + " or dutyclasscode IN (select cDutyClass from DL_U8_vPersonDutyClass2 a where a.cPsn_Num='" + psnCode + "' and datediff(day,a.dDutyDate,'" + date + "') =0  )";
            //sql = sql + ") and ([startdate]='" + date + "') ";
            //sql = sql + strSql;

            //20180108 加快速度 余长城
            string sql = "exec DL_DL_U8_pgshift '" + psnCode + "','" + date + "'";
            List<DL_U8_pgshift> rs = db.DL_U8_pgshift.SqlQuery(sql).AsNoTracking().ToList();
            return rs;
        }

        /// <summary>
        /// 称重重量转换为数量
        /// </summary>
        /// <param name="inv">存货档案</param>
        /// <param name="weight">称重净重(kg)</param>
        /// <param name="cinvcode">存货编码</param>
        /// <returns></returns>
        public decimal calcQtyFromWeight(Inventory inv, decimal weight, String cinvcode)
        {
            //库存默认计量单位
            ComputationUnit unitST = db.ComputationUnit.Where(u => u.cComunitCode == inv.cSTComUnitCode).FirstOrDefault();

            //重量计量单位
            ComputationUnit unitW = db.ComputationUnit.Where(u => u.cComunitCode == inv.cWUnit).FirstOrDefault();

            //主计量单位
            ComputationUnit unitCom = db.ComputationUnit.Where(u => u.cComunitCode == inv.cComUnitCode).FirstOrDefault();

            //重量单位(克、千克)
            DL_U8_Options opg = db.DL_U8_Options.Where(u => u.key == Const.Option_Key5).FirstOrDefault();
            DL_U8_Options opkg = db.DL_U8_Options.Where(u => u.key == Const.Option_Key6).FirstOrDefault();

            //转换
            if (unitW != null && unitW.cComUnitName.Trim().Equals(opg.value))
            {
                weight = weight * 1000;
            }

            //计算结果小数位
            Int32 scale = 0;
            if (unitCom != null && unitCom.cComUnitName.Trim().Equals("米"))
            {
                scale = 2;
            }

            //计算重量
            decimal v = weight / Convert.ToDecimal(inv.iInvWeight);
            v = decimal.Round(v, scale, MidpointRounding.AwayFromZero);
            return v;
        }

        /// <summary>
        /// 获取新单据号
        /// </summary>
        /// <param name="prex"></param>
        /// <returns></returns>
        public string getMaxCode(string prex, String maxCode)
        {
            string newCode = "";
            if (maxCode == null || maxCode.Equals(""))
            {
                newCode = "0000000001";
            }
            else
            {
                newCode = (Convert.ToInt32(maxCode.Replace(prex, "")) + 1).ToString().PadLeft(10, '0');
            }
            newCode = prex + newCode;
            return newCode;
        }

        /// <summary>
        /// 单据ID连番
        /// </summary>
        /// <param name="cAccId"></param>
        /// <param name="cVouchType"></param>
        public UIVouch getNewVouch(String cAccId, String cVouchType)
        {
            UIVouch vc = new UIVouch();
            System.Data.Objects.ObjectParameter iFatherId = new System.Data.Objects.ObjectParameter("iFatherId", System.Data.DbType.Int32);
            System.Data.Objects.ObjectParameter iChildId = new System.Data.Objects.ObjectParameter("iChildId", System.Data.DbType.Int32);
            db.sp_GetID("", cAccId, cVouchType, 1, iFatherId, iChildId, true);
            vc.iFartherId = (Int32)iFatherId.Value;
            vc.iChildId = (Int32)iChildId.Value;
            return vc;
        }
    }
}