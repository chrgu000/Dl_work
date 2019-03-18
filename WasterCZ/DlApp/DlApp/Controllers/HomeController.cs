using DlApp.Common;
using DlApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;

namespace DlApp.Controllers
{
    public class HomeController : Controller
    {
        private UFLogic logic = new UFLogic();

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            Decimal maozhong = 0M; //测试重量：0.00030899M;
            ViewBag.Maozhong = maozhong;
            //地磅参数配置
            ViewBag.port = logic.getOptions(Const.Option_Key9).First().value;
            ViewBag.rate = logic.getOptions(Const.Option_Key10).First().value;
            ViewBag.interval = logic.getOptions(Const.Option_Key11).First().value;

            //派工信息为空
            List<DL_U8_pgshift> sts = new List<DL_U8_pgshift>();
            ViewData["pgshift"] = sts;

            // 取得皮重信息
            ViewData["tare"] = logic.getTares();

            // 取得不合格品原因
            ViewData["reasons"] = logic.getReasons();
            return View();
        }

        /// <summary>
        /// 判断是否允许手动输入工号
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllowInputAction()
        {
            JsonResult jr = new JsonResult();
            UIOptions ret = new UIOptions();
            try
            {
                //是否允许手动录入工号
                ret.key1 = "0";
                List<DL_U8_Options> rs = logic.getOptions(Const.Option_Key1);
                if (rs != null && rs.Count > 0)
                {
                    ret.key1 = rs[0].value;
                }
                //是否允许手动录入毛重
                ret.key13 = "0";
                List<DL_U8_Options> ds = logic.getOptions(Const.Option_Key13);
                if (ds != null && ds.Count > 0)
                {
                    ret.key13 = ds[0].value;
                }
                ret.success = true;
            }
            catch (Exception ex)
            {
                ret.success = false;
                ret.key1 = "0";
                ret.key13 = "0";
                Console.WriteLine(ex.ToString());
            }
            jr.Data = ret;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }
        /// Added By A04 2019-02-26
        /// <summary>
        /// 检验员工
        /// </summary>
        /// <returns></returns>
        public String JudgePwdIsMatch(String userID, String passWord)
        {

            String nowData = DateTime.Now.ToString("yyyy-MM-dd");
            U8Login.clsLogin u8Login = new U8Login.clsLogin();
            #region  正式库
            String sSubId = "AS";
            String sAccID = "005";
            String sYear = "2016";
            String sUserID = userID;
            String sPassword = passWord;
            String sDate = nowData;
            String sServer = "192.168.0.250";
            String sSerial = "";
            #endregion

            #region  测试库
            //String sSubId = "AS";
            //String sAccID = "126";
            //String sYear = "2016";
            //String sUserID = userID;
            //String sPassword = passWord;
            //String sDate = nowData;
            //String sServer = "192.168.0.240";
            //String sSerial = "";
            #endregion


            if (!u8Login.Login(ref sSubId, ref sAccID, ref sYear, ref sUserID, ref sPassword, ref sDate, ref sServer, ref sSerial))
            {
                return u8Login.ShareString;
            }
            return "1";
        }

        /// <summary>
        /// 根据不合格原因判断检验员工工号
        /// </summary>
        /// <returns></returns>
        private Entities db = new Entities();
        public string JudgeStaffNoIsMatch(string reasonId,string staffNo)
        {
            String isMatch = "0";
            List<U8CUSTDEF_0046_E002> rs = db.U8CUSTDEF_0046_E002.SqlQuery("SELECT * FROM dbo.U8CUSTDEF_0046_E002 where U8CUSTDEF_0046_E001_PK=" + reasonId).ToList();
            if(rs.Count == 0)
            {
                isMatch = "1";
            }
            else
            {
                foreach(U8CUSTDEF_0046_E002 item in rs)
                {
                    if(item.cStaffName == staffNo)
                    {
                        isMatch = "1";
                    }
                }
            }
            return isMatch;
        }
        /// end

        /// <summary>
        /// 称重人员工号输入后查询当日生产派工信息
        /// </summary>
        /// <param name="code">称重人编号</param>
        /// <returns></returns>
        public JsonResult GetPgShiftAction(String code, DateTime datebefore)
        {
            JsonResult jr = new JsonResult();
            UIRet<DL_U8_pgshift> ret = new UIRet<DL_U8_pgshift>();
            try
            {
                // add datebefore参数 by chenyinghao 2017.4.25
                List<DL_U8_pgshift> rs = logic.getVPgShift(code.Trim(), datebefore);
                ret.success = true;
                ret.rs = rs;
            }
            catch (Exception ex)
            {
                ret.success = false;
                ret.msg = ex.ToString();
                Console.WriteLine(ex.ToString());
            }
            jr.Data = ret;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }

        /// <summary>
        /// 手动开启手动录入时口令验证
        /// </summary>
        /// <param name="pass"></param>
        /// <returns></returns>
        public JsonResult ValidateInputPassAction(String pass)
        {
            JsonResult jr = new JsonResult();
            UIRet<String> ret = new UIRet<String>();
            try
            {
                Entities db = new Entities();
                DL_U8_Users user = db.DL_U8_Users.Where(u => u.username.Equals("admin")).FirstOrDefault();
                if (user == null || user.password == null || !user.password.Equals(pass))
                {
                    ret.success = false;
                }
                else
                {
                    ret.success = true;
                }
            }
            catch (Exception ex)
            {
                ret.success = false;
                ret.msg = ex.ToString();
                Console.WriteLine(ex.ToString());
            }
            jr.Data = ret;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }
    }
}
