﻿using DlApp.Common;
using DlApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using U8API.Entity;
using U8APIProject;

namespace DlApp.Controllers
{
    public class UFController : Controller
    {
        /// <summary>
        /// 确认称重
        /// </summary>
        /// <param name="ps">生产订单工序班次资料ID：moroutingshiftid （xxx,xxx形式）</param>
        /// <param name="code1">操作工工号</param>
        /// <param name="code2">确认人员工号</param>
        /// <param name="jz">净重</param>
        /// <param name="reason">不良品原因码编码  cReasonCode</param>
        /// <param name="wuliaoBs">物料标识</param>
        /// <returns></returns>
        //add by chenyinghao ，增加datebefore参数
        public JsonResult WeighedAction(String ps, String code1, String code2, decimal jz, String reason, String wuliaoBs, DateTime datebefore)
        {

            UFLogic logic = new UFLogic();
            Entities db = new Entities();
            Object u8Login = null;
            UIRet<Object> ret = new UIRet<Object>();
            try
            {
                #region "输入数据验证"
                string[] pgsArr = ps.Split(',');
                List<int?> pgsLst = new List<int?>();
                foreach (string item in pgsArr)
                {
                    if (item.Equals("")) continue;
                    pgsLst.Add(Convert.ToInt32(item));
                }
                List<DL_U8_pgshift> pgs = db.DL_U8_pgshift.AsNoTracking().Where(u => pgsLst.Contains(u.moroutingshiftid)).ToList();
                if (pgs == null || pgs.Count == 0)
                {
                    ret.success = false;
                    ret.msg = "派工信息查询异常。";
                    return Utils.JsonUtil(ret);
                }

                decimal totalQty = 0M; //总完工数量
                string cinvcode = pgs.First().cinvcode;
                foreach (DL_U8_pgshift pg in pgs)
                {
                    totalQty += Convert.ToDecimal(pg.qty);
                    if (!pg.cinvcode.Equals(cinvcode))
                    {
                        ret.success = false;
                        ret.msg = "请选取相同产品编码的派工单。";
                        return Utils.JsonUtil(ret);
                    }
                }
                code1 = code1.Trim();
                code2 = code2.Trim();
                wuliaoBs = (wuliaoBs == null || wuliaoBs.Equals("")) ? "1" : wuliaoBs; //默认干净料
                #endregion

                #region "U8API连接验证"
                ENLogon logon = new ENLogon();
                DateTime cvTime = new DateTime(datebefore.Year, datebefore.Month, datebefore.Day);
                logon.sUserID = logic.getWebConfig(Const.sUserID);
                logon.sPassword = logic.getWebConfig(Const.sPassword);
                logon.sServer = logic.getWebConfig(Const.sServer);
                logon.sAccID = logic.getWebConfig(Const.sAccID);
                //update by chenyinghao  2017/6/19*******cvTime从界面控件获取,审核日期是登录注册日期**************//
                //logon.sDate = DateTime.Now.ToShortDateString();
                logon.sDate = cvTime.ToShortDateString();
                //logon.sYear = DateTime.Now.Year.ToString();
                logon.sYear = cvTime.Year.ToString();
                //update by chenyinghao  2017/6/19*******end**************************************************//
                u8Login = U8Logon.Logon(logon);
                if (u8Login == null)
                {
                    ret.success = false;
                    ret.msg = "U8登录失败，请确认配置是否正确。";
                    return Utils.JsonUtil(ret);
                }
                #endregion

                #region "基础数据准备"
                //制单人（配置文件指定）
                string makerId = logon.sUserID;
                string makerName = logic.getWebConfig(Const.sUserName);

                //查询确认人员信息
                hr_hi_person psn = db.hr_hi_person.Where(u => u.cPsn_Num == code2).FirstOrDefault();

                //查询确认人员部门
                Department dep = db.Department.Where(u => u.cDepCode == psn.cDept_num).FirstOrDefault();

                //查询选取的不良品原因

                // A04 修改于 2019/03/11   查询原因表更改
                // 原代码
                //Reason rea = db.Reason.Where(u => u.cReasonCode == reason).FirstOrDefault();
                // 新代码
                U8CUSTDEF_0046_E001 rea = db.U8CUSTDEF_0046_E001.Where(u => u.cReason == reason).FirstOrDefault();
                // end

                //查询不良品处理方式
                DL_U8_Options op = db.DL_U8_Options.Where(u => u.key == Const.Option_Key3).FirstOrDefault();
                QMSCRAPDISPOSE qmScrapDispose = db.QMSCRAPDISPOSE.Where(u => u.CSCRAPDISCODE == op.value).FirstOrDefault();
                if (qmScrapDispose == null)
                {
                    U8Logon.Shutdown(u8Login);
                    ret.success = false;
                    ret.msg = "默认不良品处理方式未指定。";
                    return Utils.JsonUtil(ret);
                }

                //查询产成品入库默认仓库
                String whCode = logic.getOptions(Const.Option_Key4).First().value;
                Warehouse wh = db.Warehouse.Where(u => u.cWhCode == whCode).FirstOrDefault();
                if (wh == null)
                {
                    U8Logon.Shutdown(u8Login);
                    ret.success = false;
                    ret.msg = "默认产成品入库仓库未指定。";
                    return Utils.JsonUtil(ret);
                }

                //查询产成品入库默认收发类别
                String rdCode = logic.getOptions(Const.Option_Key7).First().value;
                Rd_Style rdStyle = db.Rd_Style.Where(u => u.cRdCode == rdCode).FirstOrDefault();
                if (rdStyle == null)
                {
                    U8Logon.Shutdown(u8Login);
                    ret.success = false;
                    ret.msg = "默认产成品入库收发类别未指定。";
                    return Utils.JsonUtil(ret);
                }

                //查询中班班次代号
                String bcCode = logic.getOptions(Const.Option_Key12).First().value;
                #endregion

                #region "业务流程"
                decimal reportWg = 0M;  //已处理重量
                foreach (DL_U8_pgshift pgshift in pgs)
                {
                    #region "数量分摊"
                    //当班派工生产任务存在多张订单，按完工数量比例进行分摊
                    decimal wg = 0M;
                    if (pgs.IndexOf(pgshift) == pgs.Count - 1)
                    {
                        wg = jz - reportWg;
                    }
                    else
                    {
                        wg = (Convert.ToDecimal(pgshift.qty) / totalQty) * jz;
                    }
                    wg = decimal.Round(wg, 6, MidpointRounding.AwayFromZero);
                    reportWg += wg;
                    #endregion

                    #region "日期"
                    //(20161216)根据派工单的开工日期和班次进行判断。如果是月底最后一天的中班，进行不合格品处理时，
                    //系统自动生成的不合格品检验单、不良品处理单、产成品入库单、报工单、材料出库单日期均为次月的第一天。

                    //update by chenyinghao  2017/4/20*******cTime从界面获取***************************//
                    // DateTime cTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day); //当天日期
                    DateTime cTime = new DateTime(datebefore.Year, datebefore.Month, datebefore.Day);//控件日期（精确到天）
                    //update by chenyinghao  2017/4/20*******conTime参数更改，当前文档conTime都是由 DateTime.Now更改***************************//
                    DateTime conTime = new DateTime(datebefore.Year, datebefore.Month, datebefore.Day);//控件日期（精确到分秒）
                    //update by chenyinghao  2017/4/20*******end***************************//
                    DateTime startdate = Convert.ToDateTime(pgshift.startdate);//开工日期
                    DateTime d1 = new DateTime(startdate.Year, startdate.Month, 1).AddMonths(1); //开工日期次月第一天
                    DateTime d2 = d1.AddDays(-1);//开工日期月最后一天                  
                    if (startdate == d2 && pgshift.bccode != null && pgshift.bccode.Equals(bcCode))
                    {
                        cTime = d1;
                    }
                    #endregion

                    #region "数据准备"
                    //查询工作中心
                    sfc_workcenter wc = db.sfc_workcenter.Where(u => u.WcCode == pgshift.wccode).FirstOrDefault();

                    //存货档案
                    Inventory inv = db.Inventory.Where(u => u.cInvCode == pgshift.cinvcode).FirstOrDefault();

                    //查询生产订单明细资料
                    mom_orderdetail orderds = db.mom_orderdetail.Where(u => u.MoDId == pgshift.modid).FirstOrDefault();
                    v_fc_refermoroutingmixedlist refermt = db.v_fc_refermoroutingmixedlist.Where(u => u.modid == pgshift.modid).FirstOrDefault();

                    //查询生产部门
                    Department Mdept = db.Department.Where(u => u.cDepCode == orderds.MDeptCode).FirstOrDefault();

                    //查询产品报检单是否有效[产品检验单参照报检单过程SQL]
                    //QM_QPROINSPECTLIST inspectls = db.QM_QPROINSPECTLIST.Where(u => !u.CVERIFIER.Equals("") && (u.BFLAG == null || u.BFLAG == false) && u.SOURCEAUTOID == orderds.MoDId).FirstOrDefault();
                    QM_QPROINSPECTLIST inspectls = db.QM_QPROINSPECTLIST.Where(u => !u.CVERIFIER.Equals("") && (u.BFLAG == false) && u.SOURCEAUTOID == orderds.MoDId).FirstOrDefault();
                    if (inspectls == null)
                    {
                        U8Logon.Shutdown(u8Login);
                        ret.success = false;
                        ret.msg = " 生产订单[" + pgshift.mocode + "][行号：" + orderds.SortSeq + "]的产品报检单不存在或已失效。";
                        return Utils.JsonUtil(ret);
                    }

                    //查询产品报检单
                    QMINSPECTVOUCHER inspect = db.QMINSPECTVOUCHER.Where(u => u.ID == inspectls.ID).FirstOrDefault();
                    QMINSPECTVOUCHERS inspects = db.QMINSPECTVOUCHERS.Where(u => u.SOURCEAUTOID == pgshift.modid && u.ID == inspectls.ID).FirstOrDefault();
                    if (inspect == null || inspects == null)
                    {
                        U8Logon.Shutdown(u8Login);
                        ret.success = false;
                        ret.msg = " 生产订单[" + pgshift.mocode + "][行号：" + orderds.SortSeq + "]的产品报检单不存在。";
                        return Utils.JsonUtil(ret);
                    }

                    //查询质量检验方案子表
                    List<QMCHECKPROJECTS> checkps = db.QMCHECKPROJECTS.Where(u => u.ID == inv.iQTMethod).ToList();

                    //主计量单位
                    ComputationUnit unit = db.ComputationUnit.Where(u => u.cComunitCode == inv.cComUnitCode).FirstOrDefault();

                    //可用数量
                    decimal fAvaQuantity = Convert.ToDecimal(pgshift.moqty);
                    int cnt = db.Database.SqlQuery<int>("select count(moid) from fc_MoRoutingBilldetail a where a.MoId=@p0", pgshift.moid).First();
                    if (cnt > 0)
                    {
                        fAvaQuantity = db.Database.SqlQuery<decimal>("select min(fAvaQuantity) from fc_MoRoutingBilldetail a where a.MoId=@p0", pgshift.moid).SingleOrDefault();
                    }

                    //报废数量
                    Decimal ScrapQty = logic.calcQtyFromWeight(inv, wg, pgshift.cinvcode);

                    //检验人员工号
                    String employcode = pgshift.employcode;
                    if (employcode == null || employcode.Equals(""))
                    {
                        employcode = code1;
                    }
                    #endregion

                    #region "单据1~单据3"
                    UIVouch vc = new UIVouch();
                    fc_MoRoutingBill bill = new fc_MoRoutingBill(); //工序报工表头
                    fc_MoRoutingBilldetail billdetail = new fc_MoRoutingBilldetail(); //工序报工表体
                    sfc_optransform transf = new sfc_optransform(); //转序单资料
                    QMCHECKVOUCHER checkVc = new QMCHECKVOUCHER();//产品检验单主表
                    List<QMCHECKVOUCHERS> checkVcsList = new List<QMCHECKVOUCHERS>();//产品检验单子表
                    QMREJECTVOUCHER reject = new QMREJECTVOUCHER(); //不良品处理单表头
                    QMREJECTVOUCHERS rejects = new QMREJECTVOUCHERS();//不良品处理单明细
                    using (var scope = new TransactionScope())
                    {
                        try
                        {
                            #region "【单据1】报工单（审核）"
                            try
                            {
                                //（连番）报工单表头ID、单据编号(B + 10位流水)
                                vc = logic.getNewVouch(logon.sAccID, "fc_MoRoutingBill");
                                var maxVouchCode = db.Database.SqlQuery<String>("select max(cVouchCode) from fc_MoRoutingBill where cVouchCode like 'BG%'").SingleOrDefault();
                                Int32 newMID = vc.iFartherId;
                                String newVoucherCode = logic.getMaxCode("BG", maxVouchCode);

                                //（连番）报工单表体ID 
                                Int32 newMDId = vc.iChildId;

                                //（连番）转序单资料ID 
                                vc = logic.getNewVouch(logon.sAccID, "sfc_optransform");
                                var maxDocCode = db.Database.SqlQuery<String>("select max(DocCode) from sfc_optransform where DocCode like 'ZX%'").SingleOrDefault();
                                Int32 newTId = vc.iFartherId;
                                String newDocCode = logic.getMaxCode("ZX", maxDocCode);

                                //插入工序报工表头
                                bill.MID = newMID;
                                bill.cVouchCode = newVoucherCode;
                                bill.cVouchDate = cTime;
                                bill.CreateUser = makerName;
                                bill.CreateDate = cTime;
                                bill.WcId = wc.WcId;
                                bill.MoId = pgshift.moid;
                                bill.Remark = "";
                                bill.VT_ID = 31062;
                                bill.IsSingle = 0;
                                bill.CreateTime = conTime;
                                bill.cVouchType = "FC91";
                                bill.Define3 = pgshift.ceqcode; //设备编码（机台号）
                                db.fc_MoRoutingBill.Add(bill);
                                db.SaveChanges();

                                //插入工序报工表体
                                billdetail.MID = newMID;
                                billdetail.cVerifier = makerName;
                                billdetail.VerifierDate = cTime;
                                billdetail.MoId = pgshift.moid;
                                billdetail.MoDId = pgshift.modid;
                                billdetail.MoRoutingDId = pgshift.moroutingdid;//生产订单工艺路线明细ID 
                                billdetail.MoRoutingShiftId = pgshift.moroutingshiftid;//生产订单工序派工ID 
                                billdetail.EmployCode = employcode;
                                billdetail.ScrapQty = ScrapQty;
                                billdetail.fAvaQuantity = fAvaQuantity - billdetail.ScrapQty;
                                billdetail.ActualDueDate = cTime;
                                billdetail.Status = 1;
                                billdetail.VerifierTime = conTime;
                                billdetail.TransformId = newTId;
                                billdetail.OpCode = refermt.opcode;//标准工序代码
                                billdetail.OpSeq = refermt.opseq;//工序行号 
                                billdetail.MoRoutingId = refermt.moroutingid;//工序ID 
                                billdetail.OutMoRoutingDId = refermt.moroutingdid;//移出工序ID 
                                billdetail.opDescription = refermt.opdescription;//移出工序说明
                                billdetail.OpStatus = 1;//移出状态 
                                billdetail.Status = 1;
                                billdetail.MDId = newMDId;
                                billdetail.dutyclasscode = pgshift.dutyclasscode;
                                billdetail.PerWorkHrD = 1;
                                billdetail.InAuxOpQty = 0;
                                billdetail.InChangeRate = 0;
                                billdetail.QualifiedQty = 0;
                                billdetail.RefusedQty = 0;
                                billdetail.MachiningQty = 0;
                                billdetail.DeclareQty = 0;
                                billdetail.qmscraptqty = 0;
                                billdetail.qmqualifiedqty = 0;
                                billdetail.qmrefusedqty = 0;
                                db.fc_MoRoutingBilldetail.Add(billdetail);
                                db.SaveChanges();

                                //插入转序单资料
                                transf.TransformId = newTId;
                                transf.DocCode = newDocCode;
                                transf.DocDate = cTime;
                                transf.MoId = pgshift.moid;
                                transf.MoDId = pgshift.modid;
                                transf.MoRoutingId = pgshift.moroutingid;
                                transf.MoRoutingDId = pgshift.moroutingdid;
                                transf.TransformType = 1;//移动类型(1:正向/2:反向) 
                                transf.OpStatus = 1;//转出状态
                                transf.TransOutQty = ScrapQty;
                                transf.InMoRoutingDId = pgshift.moroutingdid;
                                transf.QualifiedQty = 0;
                                transf.ScrapQty = ScrapQty;
                                transf.RefusedQty = 0;
                                transf.DeclareQty = 0;
                                transf.MachiningQty = 0;
                                transf.DeclaredQty = 0;
                                transf.CreateDate = cTime;
                                transf.CreateUser = makerName;
                                transf.UpdCount = 0;
                                transf.QcFlag = false;
                                transf.OutQcFlag = false;
                                transf.VTid = 30389;
                                transf.QcType = 1;
                                transf.Status = 1;
                                transf.BFedFlag = false;
                                transf.CreateTime = cTime;
                                transf.iPrintCount = 0;
                                transf.RefDocType = 1;
                                transf.RefDocDId = billdetail.MID;
                                transf.RefDocCode = bill.cVouchCode;
                                transf.ReworkFlag = false;
                                db.sfc_optransform.Add(transf);
                                db.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                scope.Dispose();
                                U8Logon.Shutdown(u8Login);
                                ret.success = false;
                                ret.msg = "工序报工异常：" + ex.Message;
                                return Utils.JsonUtil(ret);
                            }
                            #endregion

                            #region "【单据2】产品检验单（审核）"
                            try
                            {
                                //插入产品检验单主表
                                //（连番）表头ID、单据编号(JC + 10位流水)
                                var maxCheckID = db.Database.SqlQuery<Int32>("select max(ID) from QMCHECKVOUCHER").SingleOrDefault();
                                var maxCheckCode = db.Database.SqlQuery<String>("select max(CCHECKCODE) from QMCHECKVOUCHER where CCHECKCODE like 'JC%'").SingleOrDefault();
                                Int32 newCheckID = maxCheckID + 1;
                                String newCheckCode = logic.getMaxCode("JC", maxCheckCode);

                                checkVc.CHECKGUID = Guid.NewGuid();
                                checkVc.CVOUCHTYPE = "QM04";
                                checkVc.ID = newCheckID;
                                checkVc.CCHECKCODE = newCheckCode;
                                checkVc.INSPECTID = inspect.ID;
                                checkVc.CINSPECTCODE = inspect.CINSPECTCODE;
                                checkVc.INSPECTAUTOID = inspects.AUTOID;
                                checkVc.SOURCEAUTOID = inspects.SOURCEAUTOID;
                                checkVc.SOURCEID = inspect.CSOURCEID;
                                checkVc.SOURCECODE = inspect.CSOURCECODE;
                                checkVc.CSOURCE = inspect.CSOURCE;
                                checkVc.CINSPECTDEPCODE = inspect.CINSPECTDEPCODE;
                                checkVc.CINSPECTPERSON = inspect.CMAKER;
                                checkVc.DDATE = cTime;
                                checkVc.CTIME = conTime.ToShortTimeString();
                                checkVc.CDEPCODE = psn.cDept_num;
                                checkVc.CINVCODE = inspects.CINVCODE;
                                checkVc.CUNITID = inspects.CUNITID;
                                //checkVc.ITESTSTYLE = inspects.ITESTSTYLE;
                                checkVc.ITESTSTYLE = 0; //检验方式：默认全检
                                checkVc.PROJECTID = inv.iQTMethod;
                                checkVc.FCHANGRATE = inspects.FCHANGRATE;
                                //checkVc.FQUANTITY = inspects.FQUANTITY;
                                checkVc.FQUANTITY = billdetail.ScrapQty;//报检数量：默认报废数量
                                checkVc.FNUM = decimal.Round(Convert.ToDecimal(checkVc.FQUANTITY / checkVc.FCHANGRATE), 6, MidpointRounding.AwayFromZero);
                                checkVc.FREGQUANTITY = 0;
                                checkVc.FREGNUM = 0;
                                checkVc.FDISQUANTITY = billdetail.ScrapQty;
                                checkVc.FDISNUM = decimal.Round(Convert.ToDecimal(checkVc.FDISQUANTITY / checkVc.FCHANGRATE), 6, MidpointRounding.AwayFromZero);
                                checkVc.CCHECKPERSONCODE = psn.cPsn_Num;
                                checkVc.CMAKER = makerName;
                                checkVc.CVERIFIER = makerName;
                                checkVc.IVTID = 354;
                                checkVc.BREJFLAG = true;//后续流程会生成
                                checkVc.CWHCODE = inspects.CWHCODE;
                                checkVc.ISOORDERAUTOID = inspects.ISOORDERAUTOID;
                                checkVc.IPROORDERID = inspects.IPROORDERID;
                                checkVc.IPROORDERAUTOID = inspects.IPROORDERAUTOID;
                                checkVc.CCUSCODE = inspect.CCUSCODE;
                                checkVc.CCONTRACTCODE = inspects.CCONTRACTCODE;
                                checkVc.CPOSITION = inspects.CPOSITION;
                                checkVc.BEXIGENCY = inspects.BEXIGENCY;
                                checkVc.CMEMO = "";
                                checkVc.CPROCESSAUTOID = inspect.CPROCESSAUTOID;
                                checkVc.IWORKCENTER = inspect.IWORKCENTER;
                                checkVc.CCONTRACTSTRCODE = inspects.CCONTRACTSTRCODE;
                                checkVc.CBYPRODUCT = inspects.CBYPRODUCT;
                                checkVc.CCHECKTYPECODE = inspect.CCHECKTYPECODE;
                                checkVc.CMASSUNIT = inspects.CMASSUNIT;
                                checkVc.IMASSDATE = inspects.IMASSDATE;
                                checkVc.CSOORDERCODE = inspects.CSOORDERCODE;
                                checkVc.CPROORDERCODE = inspects.CPROORDERCODE;
                                checkVc.IVERIFYSTATE = 1;
                                checkVc.PcsTransType = inspects.PcsTransType;
                                checkVc.CVMIVENCODE = inspects.CVMIVENCODE;
                                checkVc.iReturnCount = 0;
                                checkVc.iVerifyStateNew = 0;
                                checkVc.IsWfControlled = 0;
                                //update by chenyinghao  2017/5/17*******contime控件时间***************************//
                                //checkVc.DVERIFYDATE = cTime;
                                checkVc.DVERIFYDATE = conTime;
                                //update  end  2017/5/17*******contime控件时间***************************//
                                checkVc.DMAKETIME = conTime;
                                checkVc.DVERIFYTIME = conTime;
                                checkVc.CSOURCEPROORDERCODE = inspects.CSOURCEPROORDERCODE;
                                checkVc.ISOURCEPROORDERROWNO = inspects.ISOURCEPROORDERROWNO;
                                checkVc.ISOURCEPROORDERID = inspects.ISOURCEPROORDERID;
                                checkVc.ISOURCEPROORDERAUTOID = inspects.ISOURCEPROORDERAUTOID;
                                checkVc.iExpiratDateCalcu = inspects.iExpiratDateCalcu;
                                checkVc.cExpirationdate = inspects.cExpirationdate;
                                checkVc.dExpirationdate = inspects.dExpirationdate;
                                checkVc.cBatchProperty1 = inspects.cBatchProperty1;
                                checkVc.cBatchProperty2 = inspects.cBatchProperty2;
                                checkVc.cBatchProperty3 = inspects.cBatchProperty3;
                                checkVc.cBatchProperty4 = inspects.cBatchProperty4;
                                checkVc.cBatchProperty5 = inspects.cBatchProperty5;
                                checkVc.cBatchProperty6 = inspects.cBatchProperty6;
                                checkVc.cBatchProperty7 = inspects.cBatchProperty7;
                                checkVc.cBatchProperty8 = inspects.cBatchProperty8;
                                checkVc.cBatchProperty9 = inspects.cBatchProperty9;
                                checkVc.cBatchProperty10 = inspects.cBatchProperty10;
                                checkVc.IORDERTYPE = inspects.IORDERTYPE;
                                checkVc.IORDERDID = inspects.IORDERDID;
                                checkVc.IORDERSEQ = inspects.IORDERSEQ;
                                checkVc.ISOORDERTYPE = inspects.ISOORDERTYPE;
                                checkVc.CORDERCODE = inspects.CORDERCODE;
                                checkVc.DINSPECTDATE = inspect.DDATE;
                                checkVc.CINSPECTTIME = inspect.CTIME;
                                checkVc.DPLANCOMPLETEDATE = inspect.DDATE;
                                checkVc.FPLANCOMPLETEDAYS = 0;
                                checkVc.iPrintCount = 0;
                                checkVc.BMERGECHECKFLAG = false;
                                checkVc.BWITHINSPECTINFO = false;
                                checkVc.BPRODUCECHECKDETAIL = false;
                                checkVc.CSYSBARCODE = "||QMCJ|" + checkVc.CCHECKCODE;
                                checkVc.FCVOUCHERCODE = bill.cVouchCode;
                                checkVc.FCVOUCHERSID = billdetail.MDId;
                                checkVc.CREASONCODE = rea.cReasonU8;
                                db.QMCHECKVOUCHER.Add(checkVc);
                                db.SaveChanges();

                                //插入检验单子表
                                //（连番）表体ID 
                                var maxCheckDId = db.Database.SqlQuery<Int32>("select max(AUTOID) from QMCHECKVOUCHERS").SingleOrDefault();
                                Int32 newCheckDId = maxCheckDId + 1;
                                foreach (QMCHECKPROJECTS checkp in checkps)
                                {
                                    QMCHECKVOUCHERS checkVcs = new QMCHECKVOUCHERS();
                                    checkVcs.ID = checkVc.ID;
                                    checkVcs.AUTOID = newCheckDId;
                                    checkVcs.BGUIDETYPE = checkp.BGUIDETYPE;
                                    checkVcs.CCHKITEMCODE = checkp.CCHKITEMCODE;
                                    checkVcs.CCHKGUIDECODE = checkp.CCHKGUIDECODE;
                                    checkVcs.CINPORTGRADE = checkp.CINPORTGRADE;
                                    checkVcs.CSTANDARD = checkp.CSTANDARD;
                                    checkVcs.FUPPERLIMIT = checkp.IUPPERLIMIT;
                                    checkVcs.FLOWERLIMIT = checkp.ILOWERLIMIT;
                                    checkVcs.CGUIDEUNIT = unit.cComUnitName;
                                    checkVcs.DCHECKDATE = cTime;
                                    checkVcs.CCHECKTIME = conTime.ToShortTimeString();
                                    checkVcsList.Add(checkVcs);
                                    db.QMCHECKVOUCHERS.Add(checkVcs);
                                    newCheckDId++;
                                }
                                db.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                scope.Dispose();
                                U8Logon.Shutdown(u8Login);
                                ret.success = false;
                                ret.msg = "产品检验单保存异常：" + ex.Message;
                                return Utils.JsonUtil(ret);
                            }
                            #endregion

                            #region "【单据3】不良品处理单（审核）"
                            try
                            {
                                //（连番）表头ID、单据编号(BL + 10位流水)
                                var maxRejectID = db.Database.SqlQuery<Int32>("select max(ID) from QMREJECTVOUCHER").SingleOrDefault();
                                var maxRejectCode = db.Database.SqlQuery<String>("select max(CREJECTCODE) from QMREJECTVOUCHER where CREJECTCODE like 'BL%'").SingleOrDefault();
                                Int32 newRejectID = maxRejectID + 1;
                                String newRejectCode = logic.getMaxCode("BL", maxRejectCode);

                                //插入不良品处理单表头
                                reject.REJECTGUID = Guid.NewGuid();
                                reject.ID = newRejectID;
                                reject.CVOUCHTYPE = "QM06";
                                reject.CREJECTCODE = newRejectCode;
                                reject.SOURCEAUTOID = checkVc.SOURCEAUTOID;
                                reject.SOURCEID = checkVc.SOURCEID;
                                reject.SOURCECODE = checkVc.SOURCECODE;
                                reject.CSOURCE = checkVc.CSOURCE;
                                reject.CHECKID = checkVc.ID;
                                reject.CCHECKCODE = checkVc.CCHECKCODE;
                                reject.DCHECKDATE = checkVc.DDATE;
                                reject.CWHCODE = checkVc.CWHCODE;
                                reject.CCHECKPERSON = checkVc.CCHECKPERSONCODE;
                                reject.DDATE = cTime;
                                reject.CTIME = conTime.ToShortTimeString();
                                reject.CINVCODE = checkVc.CINVCODE;
                                reject.CVENCODE = checkVc.CVENCODE;
                                reject.CINSPECTDEPCODE = checkVc.CINSPECTDEPCODE;
                                reject.CITEMCLASS = checkVc.CITEMCLASS;
                                reject.CITEMCODE = checkVc.CITEMCODE;
                                reject.CITEMCNAME = checkVc.CITEMCNAME;
                                reject.CITEMNAME = checkVc.CITEMNAME;
                                reject.CUNITID = checkVc.CUNITID;
                                reject.FCHANGRATE = checkVc.FCHANGRATE;
                                reject.CBATCH = checkVc.CBATCH;
                                reject.DPRODATE = checkVc.DPRODATE;
                                reject.DVDATE = checkVc.DVDATE;
                                reject.FSUMQUANTITY = checkVc.FDISQUANTITY;
                                reject.FSUMNUM = checkVc.FDISNUM;
                                reject.IVTID = 356;
                                reject.CMAKER = checkVc.CMAKER;
                                reject.CVERIFIER = checkVc.CVERIFIER;
                                reject.IVERIFYSTATE = 1;
                                reject.CFREE1 = checkVc.CFREE1;
                                reject.CFREE2 = checkVc.CFREE2;
                                reject.CFREE3 = checkVc.CFREE3;
                                reject.CFREE4 = checkVc.CFREE4;
                                reject.CFREE5 = checkVc.CFREE5;
                                reject.CFREE6 = checkVc.CFREE6;
                                reject.CFREE7 = checkVc.CFREE7;
                                reject.CFREE8 = checkVc.CFREE8;
                                reject.CFREE9 = checkVc.CFREE9;
                                reject.CFREE10 = checkVc.CFREE10;
                                reject.CDEFINE1 = checkVc.CDEFINE1;
                                reject.CDEFINE2 = checkVc.CDEFINE2;
                                reject.CDEFINE3 = checkVc.CDEFINE3;
                                reject.CDEFINE4 = checkVc.CDEFINE4;
                                reject.CDEFINE5 = checkVc.CDEFINE5;
                                reject.CDEFINE6 = checkVc.CDEFINE6;
                                reject.CDEFINE7 = checkVc.CDEFINE7;
                                reject.CDEFINE8 = checkVc.CDEFINE8;
                                reject.CDEFINE9 = checkVc.CDEFINE9;
                                reject.CDEFINE10 = checkVc.CDEFINE10;
                                reject.CDEFINE11 = checkVc.CDEFINE11;
                                reject.CDEFINE12 = checkVc.CDEFINE12;
                                reject.CDEFINE13 = checkVc.CDEFINE13;
                                reject.CDEFINE14 = checkVc.CDEFINE14;
                                reject.CDEFINE15 = checkVc.CDEFINE15;
                                reject.CDEFINE16 = checkVc.CDEFINE16;
                                reject.IPROORDERID = checkVc.IPROORDERID;
                                reject.CPROORDERCODE = checkVc.CPROORDERCODE;
                                reject.IPROORDERAUTOID = checkVc.IPROORDERAUTOID;
                                reject.CCUSCODE = checkVc.CCUSCODE;
                                reject.CCONTRACTCODE = checkVc.CCONTRACTCODE;
                                reject.CCONTRACTSTRCODE = checkVc.CCONTRACTSTRCODE;
                                reject.IWORKCENTER = checkVc.IWORKCENTER;
                                reject.CPROCESSAUTOID = checkVc.CPROCESSAUTOID;
                                reject.CBYPRODUCT = checkVc.CBYPRODUCT;
                                reject.BEXIGENCY = checkVc.BEXIGENCY;
                                reject.CPOSITION = checkVc.CPOSITION;
                                reject.IMASSDATE = checkVc.IMASSDATE;
                                reject.CMASSUNIT = checkVc.CMASSUNIT;
                                reject.CCHECKTYPECODE = checkVc.CCHECKTYPECODE;
                                reject.PcsTransType = checkVc.PcsTransType;
                                reject.CVMIVENCODE = checkVc.CVMIVENCODE;
                                reject.iReturnCount = checkVc.iReturnCount;
                                reject.iVerifyStateNew = checkVc.iVerifyStateNew;
                                reject.IsWfControlled = checkVc.IsWfControlled;
                                //update by chenyinghao  2017/5/17*******contime控件时间***************************//
                                //reject.DVERIFYDATE = cTime;
                                reject.DVERIFYDATE = conTime;
                                //update by end  2017/5/17*******contime控件时间***************************//
                                reject.DMAKETIME = conTime;
                                reject.DVERIFYTIME = conTime;
                                reject.CSOURCEPROORDERCODE = checkVc.CSOURCEPROORDERCODE;
                                reject.ISOURCEPROORDERROWNO = checkVc.ISOURCEPROORDERROWNO;
                                reject.ISOURCEPROORDERID = checkVc.ISOURCEPROORDERID;
                                reject.ISOURCEPROORDERAUTOID = checkVc.ISOURCEPROORDERAUTOID;
                                reject.iExpiratDateCalcu = checkVc.iExpiratDateCalcu;
                                reject.cExpirationdate = checkVc.cExpirationdate;
                                reject.dExpirationdate = checkVc.dExpirationdate;
                                reject.cBatchProperty1 = checkVc.cBatchProperty1;
                                reject.cBatchProperty2 = checkVc.cBatchProperty2;
                                reject.cBatchProperty3 = checkVc.cBatchProperty3;
                                reject.cBatchProperty4 = checkVc.cBatchProperty4;
                                reject.cBatchProperty5 = checkVc.cBatchProperty5;
                                reject.cBatchProperty6 = checkVc.cBatchProperty6;
                                reject.cBatchProperty7 = checkVc.cBatchProperty7;
                                reject.cBatchProperty8 = checkVc.cBatchProperty8;
                                reject.cBatchProperty9 = checkVc.cBatchProperty9;
                                reject.cBatchProperty10 = checkVc.cBatchProperty10;
                                //20170310 MOD S
                                //reject.IORDERTYPE = checkVc.IORDERTYPE;  //需求跟踪方式
                                //reject.CSOORDERCODE = checkVc.CSOORDERCODE; //需求跟踪号
                                //reject.ISOORDERAUTOID = checkVc.ISOORDERAUTOID; //需求跟踪子表ID 
                                //reject.IORDERDID = checkVc.IORDERDID;   //销售订单id 
                                //reject.IORDERSEQ = checkVc.IORDERSEQ;   //销售订单行号 
                                //reject.CORDERCODE = checkVc.CORDERCODE; //销售订单号
                                reject.IORDERTYPE = 0;  //需求跟踪方式
                                //20170310 MOD E
                                reject.ISOORDERTYPE = checkVc.ISOORDERTYPE;
                                reject.CRETURNREASONCODE = checkVc.CRETURNREASONCODE;
                                reject.iPrintCount = checkVc.iPrintCount;
                                reject.CCLEANVER = checkVc.CCLEANVER;
                                reject.PFCODE = checkVc.PFCODE;
                                reject.BMERGECHECKFLAG = checkVc.BMERGECHECKFLAG;
                                reject.MERGECHECKAUTOID = checkVcsList.First().AUTOID;
                                reject.CSYSBARCODE = "||QMCR|" + reject.CREJECTCODE;
                                reject.cCurrentAuditor = checkVc.cCurrentAuditor;
                                reject.FCVOUCHERCODE = checkVc.FCVOUCHERCODE;
                                reject.FCVOUCHERSID = checkVc.FCVOUCHERSID;
                                reject.PLANLOTNUMBER = checkVc.PLANLOTNUMBER;
                                db.QMREJECTVOUCHER.Add(reject);
                                db.SaveChanges();

                                //（连番）表体ID 
                                var maxRejectsId = db.Database.SqlQuery<Int32>("select max(AUTOID) from QMREJECTVOUCHERS").SingleOrDefault();
                                Int32 newRejectsId = maxRejectsId + 1;

                                //插入不良品处理单明细
                                rejects.ID = reject.ID;
                                rejects.AUTOID = newRejectsId;
                                rejects.CREASONCODE = checkVc.CREASONCODE;
                                rejects.FQUANTITY = checkVc.FDISQUANTITY;
                                rejects.FNUM = checkVc.FDISNUM;
                                rejects.CSCRAPDISCODE = qmScrapDispose.CSCRAPDISCODE;
                                rejects.IDISPOSEFLOW = qmScrapDispose.IDISPOSEFLOW;
                                rejects.CDIMINVCODE = reject.CINVCODE;
                                rejects.CDIMUNITID = reject.CUNITID;
                                rejects.FDIMCHANGRATE = reject.FCHANGRATE;
                                rejects.FDIMQUANTITY = rejects.FQUANTITY;
                                rejects.FDIMNUM = rejects.FNUM;
                                rejects.CDEPCODE = checkVc.CDEPCODE;
                                rejects.iDIMExpiratDateCalcu = 0;
                                rejects.cDIMExpirationdate = reject.cExpirationdate;
                                rejects.dDIMExpirationdate = reject.dExpirationdate;
                                rejects.CPUPOCODE = psn.cDept_num;
                                rejects.CBWHCODE = reject.CWHCODE;
                                rejects.CBSYSBARCODE = reject.CSYSBARCODE + "|1";
                                db.QMREJECTVOUCHERS.Add(rejects);
                                db.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                scope.Dispose();
                                ret.success = false;
                                ret.msg = "不良品处理单保存异常：" + ex.Message;
                                return Utils.JsonUtil(ret);
                            }
                            #endregion

                            //提交事务
                            scope.Complete();
                        }
                        catch (Exception ex)
                        {
                            scope.Dispose();
                            U8Logon.Shutdown(u8Login);
                            ret.success = false;
                            ret.msg = "工序报工、产品检验单、不良品处理单保存异常：" + ex.Message;
                            return Utils.JsonUtil(ret);
                        }
                    }
                    #endregion

                    #region "单据4：调用U8API插入产成品入库单"
                    DOMAPI10 api10 = new DOMAPI10();
                    //产成品入库单表头
                    API10Hd hd = new API10Hd();
                    hd.dnmaketime = conTime;
                    hd.dveridate = conTime;
                    hd.ddate = cTime;
                    hd.iproorderid = pgshift.moid.ToString();
                    hd.cmpocode = pgshift.mocode;
                    hd.cdepname = Mdept.cDepName;
                    hd.crdname = rdStyle.cRdName;
                    hd.cchkperson = psn.cPsn_Name;
                    hd.cbustype = "成品入库";
                    hd.cpersonname = pgshift.employname;
                    hd.csource = "产品不良品处理单";
                    hd.brdflag = rdStyle.bRdFlag.ToString();
                    hd.crdcode = rdStyle.cRdCode;
                    hd.cdepcode = Mdept.cDepCode;
                    hd.cpersoncode = billdetail.EmployCode;
                    hd.cdefine2 = billdetail.EmployCode;
                    hd.cmaker = makerName;
                    hd.vt_id = 131522;
                    hd.cdefine16 = Convert.ToDouble(wg);  //画面自定义项：净重
                    hd.cdefine5 = wuliaoBs;    //画面自定义项：物料标识(1：干净料、2：脏料)

                    /////////////////// 20181107 余长城 修改 按工作中心分开入到注塑不合格品库，或挤塑不合格品库 ycc
                    //001	管材生产    2001    挤塑车间不合格品库
                    //002	管件生产    2004    注塑车间不合格品库

                    hd.cwhname = wh.cWhName;
                    hd.cwhcode = wh.cWhCode;

                    //if (wc.WcCode == "001")
                    //{
                    //    hd.cwhname = "挤塑车间不合格品库";
                    //    hd.cwhcode = "2001";
                    //}

                    //if (wc.WcCode == "002")
                    //{
                    //    hd.cwhname = "注塑车间不合格品库";
                    //    hd.cwhcode = "2004";
                    //}
                    /////////////////// 20181107 余长城 修改 按工作中心分开入到注塑不合格品库，或挤塑不合格品库 ycc

                    //产成品入库单表体
                    List<API10Bd> bds = new List<API10Bd>();
                    API10Bd bd = new API10Bd();
                    bd.cinvcode = reject.CINVCODE;
                    bd.inum = Convert.ToDouble(rejects.FNUM);
                    bd.iquantity = Convert.ToDouble(rejects.FQUANTITY);
                    bd.innum = Convert.ToDouble(rejects.FDIMNUM);
                    bd.inquantity = Convert.ToDouble(rejects.FDIMQUANTITY);
                    bd.cassunit = rejects.CDIMUNITID;
                    bd.ccheckcode = checkVc.CCHECKCODE;
                    bd.icheckidbaks = checkVc.ID.ToString();
                    bd.crejectcode = reject.CREJECTCODE;
                    bd.irejectids = rejects.AUTOID.ToString();
                    bd.ccheckpersoncode = psn.cPsn_Num;
                    bd.ccheckpersonname = psn.cPsn_Name;
                    bd.dcheckdate = checkVc.DDATE.ToString();
                    bd.iinvexchrate = Convert.ToDouble(rejects.FDIMCHANGRATE);
                    bd.iexpiratdatecalcu = Convert.ToInt32(reject.iExpiratDateCalcu);
                    bd.cexpirationdate = reject.cExpirationdate;
                    bd.dexpirationdate = reject.dExpirationdate.ToString();
                    bd.dvdate = reject.DVDATE;
                    bd.cmworkcentercode = wc.WcCode; //工作中心编码
                    bd.cmworkcenter = wc.Description;   //工作中心

                    bd.impoids = Convert.ToInt32(reject.SOURCEAUTOID);//生产订单子表ID
                    bd.imoseq = pgshift.moseq.ToString();   //生产订单行号 
                    bd.cmocode = pgshift.mocode;    //生产订单号
                    bd.cmolotcode = orderds.MoLotCode;  //生产批号
                    bd.iopseq = pgshift.opseq;  //生产订单工序行号
                    bd.copdesc = pgshift.description; //工序说明


                    /////////////////////////////////////////////////////////// 20180713 添加项目信息
                    //CostItemName(U871)  项目名称 
                    //CostItemCode(U871)  项目编码


                    bd.cname = orderds.CostItemName;
                    bd.citemcode = orderds.CostItemCode;

                    string CostItemName =  db.Database.SqlQuery<string>("select citemname FROM fitemss99 where citemcode='"+ orderds.CostItemCode + "' AND citemname ='"+ orderds.CostItemName + "'").First();

                    if (!(string.IsNullOrEmpty(CostItemName)))   //能找到数据
                    {
                        bd.citem_class = db.Database.SqlQuery<string>(" select citem_class from fitem where ctable='fitemss99'").First();
                        bd.citemcname = db.Database.SqlQuery<string>(" select citem_name  from fitem where ctable='fitemss99'").First();
                    }
                         
                    //cName(U8100)  项目名称
                    //cItemCode(U8100)  项目编码
                    //cItem_class(U8100)  项目大类编码
                    //cItemCName(U8100)  项目大类名称

                    /////////////////////////////////////////////////////////// 20180713 添加项目信息



                        //20170310 DEL S 不合格品入库不能预留库存
                        //bd.iordertype = orderds.OrderType.Value;  //销售订单类别 
                        //bd.iorderdid = orderds.OrderDId.Value;   //销售订单子表ID 
                        //bd.iorderseq = orderds.OrderSeq.ToString(); //销售订单行号
                        //bd.iordercode = orderds.OrderCode;  //销售订单号

                        //bd.isotype = orderds.SoType.Value;  //需求跟踪方式
                        //bd.isodid = orderds.SoDId;  //需求跟踪子表ID（* 销售订单子表ID *）
                        //bd.isoseq = orderds.SoSeq.ToString();   //需求跟踪行号
                        //bd.csocode = orderds.SoCode;    //需求跟踪号
                        //20170310 DEL E
                        bd.irowno = "1";
                    bds.Add(bd);
                    try
                    {
                        APIRet apire = api10.Add(u8Login, hd, bds);
                        if (apire.Success)
                        {
                            //产成品入库单表头扩展表
                            try
                            {
                                Int32 vid = Convert.ToInt32(apire.VouchId);
                                rdrecord10_extradefine rdEx = db.rdrecord10_extradefine.Where(u => u.ID == vid).SingleOrDefault();
                                if (rdEx == null)
                                {
                                    rdEx = new rdrecord10_extradefine();
                                    rdEx.ID = vid;
                                    //20170310 MOD S
                                    //rdEx.chdefine37 = "是"; //是否报工
                                    rdEx.chdefine37 = "否"; //是否报工
                                    //20170310 MOD E
                                    db.rdrecord10_extradefine.Add(rdEx);
                                }
                                else
                                {
                                    //20170310 MOD S
                                    //rdEx.chdefine37 = "是"; //是否报工
                                    rdEx.chdefine37 = "否"; //是否报工
                                    //20170310 MOD E
                                    db.rdrecord10_extradefine.Attach(rdEx);
                                    var setEntry = ((IObjectContextAdapter)db).ObjectContext.ObjectStateManager.GetObjectStateEntry(rdEx);
                                    setEntry.SetModifiedProperty("chdefine37");
                                }
                                db.SaveChanges();
                            }
                            catch (Exception ex) { Console.WriteLine(ex.ToString()); }

                            //产成品入库单审核
                            string msg = apire.ErrMsg;
                            msg = api10.Audit(u8Login, apire.VouchId);
                            ret.success = true;
                            ret.msg = msg;
                        }
                        else
                        {
                            ret.success = false;
                            ret.msg = apire.ErrMsg;
                        }
                    }
                    catch (Exception ex)
                    {
                        ret.success = false;
                        ret.msg = "U8API调用异常：" + ex.Message;
                    }
                    if (!ret.success)
                    {
                        using (var scope = new TransactionScope())
                        {
                            //删除不良品处理单
                            db.QMREJECTVOUCHER.Remove(reject);
                            db.QMREJECTVOUCHERS.Remove(rejects);
                            db.SaveChanges();

                            //删除产品检验单
                            foreach (QMCHECKVOUCHERS checkVcs in checkVcsList)
                            {
                                db.QMCHECKVOUCHERS.Remove(checkVcs);
                            }
                            db.QMCHECKVOUCHER.Remove(checkVc);
                            db.SaveChanges();

                            //删除工序报工
                            db.sfc_optransform.Remove(transf);
                            db.fc_MoRoutingBilldetail.Remove(billdetail);
                            db.fc_MoRoutingBill.Remove(bill);
                            db.SaveChanges();

                            scope.Complete();
                        }
                        U8Logon.Shutdown(u8Login);
                        return Utils.JsonUtil(ret);
                    }
                    #endregion
                }
                #endregion
            }
            catch (Exception ex)
            {
                ret.success = false;
                ret.msg = "系统异常：" + ex.Message;
            }
            U8Logon.Shutdown(u8Login);
            return Utils.JsonUtil(ret);
        }
    }

}
