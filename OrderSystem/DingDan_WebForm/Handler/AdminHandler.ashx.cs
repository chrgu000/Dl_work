using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Data;
using BLL;
using System.Web.SessionState;
using System.Text;
using Model;
using System.Web.Script.Serialization;
using System.ServiceModel.Web;
using System.Runtime.Serialization.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Data.SqlClient;
using System.Net;
using System.Configuration;
using System.EnterpriseServices;
using System.Web.Security;


namespace DingDan_WebForm.Handler
{
    /// <summary>
    /// ProductHandler 的摘要说明
    /// </summary>
    public class AdminHandler : AdminBase
    {
        product pro = new product();
        AdminManager am = new AdminManager();
        OrderManager order = new OrderManager();
        Check check = new Check();
        Common common = new Common();
        //SMSSend8222.SendSMS2CustomerSoapClient SMSSend8222 = new SMSSend8222.SendSMS2CustomerSoapClient();
        WeiXin9003.WeiXinSoapClient wx9003 = new WeiXin9003.WeiXinSoapClient();
        SMSSend9003.SendSMS2CustomerSoapClient SMSSend9003 = new SMSSend9003.SendSMS2CustomerSoapClient();
        IsoDateTimeConverter timejson = new IsoDateTimeConverter
        {
            DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss"
        };
        public ReInfo errMsg = new ReInfo()
        {
            list_msg = new List<string>(),
            flag = "0",
            message = "程序出现错误,请重试或联系管理员!!"
        };


        public override void AjaxProcess(HttpContext context)
        {
            ReInfo ri = new ReInfo();
            string Action = HttpContext.Current.Request.Form["Action"];
            if (string.IsNullOrEmpty(Action))
            {
                ri.flag = "0";
                ri.message = "错误的方法!";
                context.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }
            var method = this.GetType().GetMethod(Action);
            if (method == null)
            {
                ri.flag = "0";
                ri.message = "错误的方法!";
                context.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }
            try
            {
                method.Invoke(this, new object[] { });
            }
            catch (Exception err)
            {

                errMsg.list_msg.Add("Action:" + HttpContext.Current.Request.Form["Action"]);
                errMsg.list_msg.Add("AdmincCusCode:" + HttpContext.Current.Session["AdmincCusCode"].ToString());
                errMsg.list_msg.Add(err.ToString());
                check.WriteLog(errMsg);
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(errMsg));
                return;
            }


        }

        #region 测试专用
        public void test()
        {
            string id = HttpContext.Current.Request.Form["id"];
            JObject jo = new JObject();
            jo["Now"] = DateTime.Now.ToString();
            jo["id"] = id;
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
        }
        #endregion


        #region 判断是否有权限打开后台
        /// <summary>
        /// 判断是否有权限打开后台
        /// </summary>
        [Description("判断是否有权限打开后台")]
        public void Check_Level()
        {
            ReInfo ri = new ReInfo();
            if (HttpContext.Current.Session["AdminstrUserLevel"].ToString() != "2")
            {
                ri.flag = "0";
                ri.message = "没有权限！";

            }
            else
            {
                ri.flag = "1";
                ri.message = HttpContext.Current.Session["AdminstrUserName"].ToString();
            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            return;

        }
        #endregion

        #region 获取所有用户列表
        public void Get_AllcCuscode()
        {

            ReInfo ri = new ReInfo();
            ri.flag = "1";
            ri.message = HttpContext.Current.Session["AdminlngopUserId"].ToString();
            ri.dt = am.Get_AllcCuscode();
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            return;
        }
        #endregion

        #region 获取所有用户及子用户
        public void GetAllUserCode()
        {

            JObject jo = new JObject();
            DataSet ds = am.GetAllUserCode();
            jo["flag"] = 1;
            jo["MainCode"] = JToken.Parse(JsonConvert.SerializeObject(ds.Tables[0]));
            jo["SubCode"] = JToken.Parse(JsonConvert.SerializeObject(ds.Tables[1]));
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        //#region 获取延期通知单列表
        //public void Get_ArrearList()
        //{
        //    ReInfo ri = new ReInfo();
        //    string start_date = HttpContext.Current.Request.Form["start_date"];
        //    string end_date = HttpContext.Current.Request.Form["end_date"];
        //    string bytstatus = HttpContext.Current.Request.Form["bytstatus"];
        //    ri.dt = am.Get_ArrearList(start_date, end_date, bytstatus);
        //    ri.flag = "1";
        //    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
        //}
        //#endregion

        #region 新增延期通知单
        public void DLproc_NewArrearByIns()
        {
            ReInfo ri = new ReInfo();
            DataTable dt = am.Get_ProcParas("DLproc_NewArrearByIns");
            var acti = HttpContext.Current.Request.Form["Action"];
            string arrHead = HttpContext.Current.Request.Form["arrHead"];
            string arrBody = HttpContext.Current.Request.Form["arrBody"];

            //表头数据
            Dictionary<string, string> dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(arrHead);
            dic["cMake"] = HttpContext.Current.Session["AdminlngopUserId"].ToString();

            //表体数据
            List<Dictionary<string, string>> arrBodyList = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(arrBody);
            dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[]{
                new DataColumn("Id",typeof(int)),
                new DataColumn("AutoId",typeof(int)),
                new DataColumn("cSubCusCode",typeof(string)),
                new DataColumn("cSubCusName",typeof(string)),
                new DataColumn("iSumSubCusCode",typeof(double))
            });
            for (int i = 0; i < arrBodyList.Count; i++)
            {
                dt.Rows.Add();
                dt.Rows[i]["cSubCusCode"] = arrBodyList[i]["cSubCusCode"];
                dt.Rows[i]["cSubCusName"] = arrBodyList[i]["cSubCusName"];
                dt.Rows[i]["iSumSubCusCode"] = double.Parse(arrBodyList[i]["iSumSubCusCode"]);
            }

            dt.TableName = "dl_oparreardetail";
            bool b = am.DLproc_NewArrearByIns("DLproc_NewArrearByIns", dic, dt);
            if (b)
            {
                ri.flag = "1";
                ri.message = "数据写入成功！";
            }
            else
            {
                ri.flag = "0";
                ri.message = "数据写入失败！";
            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            //   HttpContext.Current.Response.Write(JsonConvert.SerializeObject(arrHead));
        }
        #endregion

        #region 根据延期通知单号查询详情
        public void Get_ArrearDetail()
        {
            string code = HttpContext.Current.Request.Form["code"];
            ReInfo ri = new ReInfo();
            DataTable dt = am.Get_ArrearDetail(code);
            if (dt.Rows.Count > 0)
            {
                ri.flag = "1";
                ri.dt = dt;

            }
            else
            {
                ri.flag = "0";
                ri.message = "数据异常！";
            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
        }
        #endregion

        #region 延期通知单取回
        public void BackArrear()
        {
            string code = HttpContext.Current.Request.Form["code"];
            string lngopUserId = HttpContext.Current.Session["AdminlngopUserId"].ToString();
            ReInfo ri = new ReInfo();
            ri = am.BackArrear(code, lngopUserId);
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
        }
        #endregion

        #region 延期通知单作废
        public void CancelArrear()
        {
            string code = HttpContext.Current.Request.Form["code"];
            string id = HttpContext.Current.Session["AdminlngopUserId"].ToString();
            ReInfo ri = new ReInfo();
            ri = am.CancelArrear(code, id);
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
        }
        #endregion

        #region 收款员重新开通网上下单功能
        public void RenewArrear()
        {
            string code = HttpContext.Current.Request.Form["code"];
            string lngopUserId = HttpContext.Current.Session["AdminlngopUserId"].ToString();
            ReInfo ri = new ReInfo();
            ri = am.RenewArrear(code, lngopUserId);
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
        }
        #endregion

        #region 修改延期通知单
        public void DLproc_NewArrearByUpd()
        {
            ReInfo ri = new ReInfo();
            DataTable dt = am.Get_ProcParas("DLproc_NewArrearByIns");
            // var acti = HttpContext.Current.Request.Form["Action"];
            string arrHead = HttpContext.Current.Request.Form["arrHead"];
            string arrBody = HttpContext.Current.Request.Form["arrBody"];

            //表头数据
            Dictionary<string, string> dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(arrHead);
            dic["cMake"] = HttpContext.Current.Session["AdminlngopUserId"].ToString();
            dic["cCode"] = HttpContext.Current.Request.Form["code"];

            //表体数据
            List<Dictionary<string, string>> arrBodyList = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(arrBody);
            dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[]{
                new DataColumn("Id",typeof(int)),
                new DataColumn("AutoId",typeof(int)),
                new DataColumn("cSubCusCode",typeof(string)),
                new DataColumn("cSubCusName",typeof(string)),
                new DataColumn("iSumSubCusCode",typeof(double))
            });
            for (int i = 0; i < arrBodyList.Count; i++)
            {
                dt.Rows.Add();
                dt.Rows[i]["cSubCusCode"] = arrBodyList[i]["cSubCusCode"];
                dt.Rows[i]["cSubCusName"] = arrBodyList[i]["cSubCusName"];
                dt.Rows[i]["iSumSubCusCode"] = double.Parse(arrBodyList[i]["iSumSubCusCode"]);
            }

            dt.TableName = "dl_oparreardetail";
            ri = am.DLproc_NewArrearByUpd("DLproc_NewArrearByUpd", dic, dt);

            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
        }
        #endregion

        #region 复核延期通知单
        public void ConfirmArrear()
        {
            string code = HttpContext.Current.Request.Form["code"];
            string lngopUserId = HttpContext.Current.Session["AdminlngopUserId"].ToString();
            ReInfo ri = new ReInfo();
            ri = am.ConfirmArrear(lngopUserId, code);
            if (ri.flag == "1")
            {
                //获取该通知单主账号电话
                DataTable dt = am.Get_PhoneByArrearCode(code);
                string msg = string.Empty;
                string nums = string.Empty;
                if (ri.message == "21")
                {
                    msg = "您有一张延期未结清账款通知单，请于两日内登录网上订单系统进行确认。如未按期确认，将影响您网上下单业务以及年终考评，感谢您的支持！";
                }
                else
                {
                    msg = "您的货款已连续三月未结清，系统已自动控制下单。请及时到网上订单【报表】->【欠款延期通知单】里进行确认，如有疑问，请咨询收款员！";
                }
                nums = dt.Rows[0]["cCusPhone"].ToString();
                string[] num_arr = nums.Split(';');
                nums = string.Join(",", num_arr);
                SMSSend9003.SendSMS(nums, msg);
                foreach (var num in num_arr)
                {
                    SMSSend9003.SendQY_Message_Text(num, "", "", "20", msg);
                }
                ri.message = "复核成功！";
                //if (dt.Rows[0]["bytStatus"].ToString() == "2")
                //{
                //    msg = "您有一张延期未结清账款通知单，请于两日内登录网上订单系统进行确认。如未按期确认，将影响您网上下单业务以及年终考评，感谢您的支持！";
                //    nums = dt.Rows[0]["cCusPhone"].ToString();
                //    string[] num_arr = nums.Split(';');
                //    nums = string.Join(",", num_arr);
                //    SMSSend9003.SendSMS(nums, msg);
                //    foreach (var num in num_arr)
                //    {
                //        SMSSend9003.SendQY_Message_Text(num, "", "", "22", msg);
                //    }
                //}
                //else if (dt.Rows[0]["bytStatus"].ToString() == "3")
                //{
                //    msg = "您有一张逾期三个月的延期通知单需要处理！";
                //    nums = ConfigurationManager.AppSettings["Manager"];
                //    string[] num_arr = nums.Split('|');
                //    nums = string.Join(",", num_arr);
                //    SMSSend9003.SendSMS(nums, msg);
                //    foreach (var num in num_arr)
                //    {
                //        SMSSend9003.SendQY_Message_Text(num, "", "", "22", msg);
                //    }
                //}


            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
        }

        #endregion

        //    public string PostWebRequest(string postUrl, string paramData, Encoding dataEncode)
        //    {
        //        string ret = string.Empty;
        //        try
        //        {
        //            byte[] byteArray = Encoding.UTF8.GetBytes(paramData); //转化
        //            HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
        //            webReq.Method = "POST";
        //            webReq.ContentType = "application/x-www-form-urlencoded";

        //            webReq.ContentLength = byteArray.Length;
        //            Stream newStream = webReq.GetRequestStream();
        //            newStream.Write(byteArray, 0, byteArray.Length);//写入参数
        //            newStream.Close();
        //            HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
        //            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
        //            ret = sr.ReadToEnd();
        //            sr.Close();
        //            response.Close();
        //            newStream.Close();
        //        }
        //        catch (Exception ex)
        //        {
        //            return ex.Message;
        //        }
        //        return ret;
        //    }

        //    public string GeWebRequest(string url)
        //{
        //    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

        //    httpWebRequest.ContentType = "application/json";
        //    httpWebRequest.Method = "GET";
        //    httpWebRequest.Timeout = 20000;

        //    //byte[] btBodys = Encoding.UTF8.GetBytes(body);
        //    //httpWebRequest.ContentLength = btBodys.Length;
        //    //httpWebRequest.GetRequestStream().Write(btBodys, 0, btBodys.Length);

        //    HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        //    StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
        //    string responseContent = streamReader.ReadToEnd();

        //    httpWebResponse.Close();
        //    streamReader.Close();

        //    return responseContent;
        //}

        #region 微信登录
        public void Manager_Login()
        {
            ReInfo ri = new ReInfo();
            string code = HttpContext.Current.Request.Form["code"];
            //string re = new WeiXin8222.WeiXinSoapClient().Get_UserIdByAuthCode(code);
            string re = new WeiXin9003.WeiXinSoapClient().Get_UserIdByAuthCode(code);
            JObject jo = (JObject)JsonConvert.DeserializeObject(re);

            if (!string.IsNullOrEmpty((string)jo["UserId"]))
            {
                string manager = ConfigurationManager.AppSettings["Manager"];
                string[] manager_arr = manager.Split('|');
                int d = Array.IndexOf(manager_arr, (string)jo["UserId"]);
                if (d == -1)
                {
                    ri.flag = "0";
                    ri.message = "没有权限！";
                    ri.msg = new string[]{
                                    code,
                                    (string)jo["UserId"]
                                  };
                    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                    return;
                }
                else
                {
                    HttpContext.Current.Session["WXUserId"] = (string)jo["UserId"];
                    HttpContext.Current.Session["AdminlngopUserId"] = am.GetIdByPhone((string)jo["UserId"]);
                    ri.flag = "1";
                    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                    return;
                }

            }
            else
            {
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                return;
            }

        }
        #endregion

        #region 获取预约配置表
        public void Get_MAASetiing()
        {
            DataTable dt = am.Get_MAASetiing();
            JObject jo = new JObject();
            jo["flag"] = 1;
            jo["setting_table"] = JToken.Parse(JsonConvert.SerializeObject(dt));
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 保存预约配置
        public void Save_MAASetting()
        {
            JObject jo = JObject.Parse(HttpContext.Current.Request.Form["data"]);
            jo["strUserName"] = HttpContext.Current.Session["AdminstrUserName"].ToString();
            int a = am.Save_MAASetting(jo);
            jo = new JObject();
            if (a > 0)
            {
                jo["flag"] = 1;
            }
            else
            {
                jo["flag"] = 0;

            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;

        }
        #endregion

        #region 获取可审核生成U8发货单的预约号列表
        public void Get_ToU8AuditList()
        {
            DataTable dt = am.Get_ToU8AuditList();
            JObject jo = new JObject();
            jo["flag"] = 1;
            jo["maalist"] = JToken.Parse(JsonConvert.SerializeObject(dt));
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 获取待审核的特殊预约申请列表
        public void Get_SpecialAuditList()
        {
            string strLoginName = HttpContext.Current.Session["AdminstrLoginName"].ToString();
            string MAAID = "";
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["MAAID"]))
            {
                MAAID = HttpContext.Current.Request.Form["MAAID"];
            }
            DataTable dt = am.Get_SpecialAuditList(MAAID, strLoginName);
            JObject jo = new JObject();
            jo["flag"] = 1;
            jo["specialList"] = JToken.Parse(JsonConvert.SerializeObject(dt));
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 审核特殊预约申请
        public void AuditSpecialMAA()
        {
            JObject jo = new JObject();
            string MAAID = HttpContext.Current.Request.Form["MAAID"];
            string User = HttpContext.Current.Session["AdminstrLoginName"].ToString();
            string opdata = HttpContext.Current.Request.Form["opdata"];
            string cMemo = HttpContext.Current.Request.Form["cMemo"];
            string id = HttpContext.Current.Request.Form["id"];
            bool b = am.AuditSpecialMAA(id, MAAID, User, opdata, cMemo);
            if (b)
            {
                jo["flag"] = "1";
                jo["message"] = "审核成功！";
            }
            else
            {
                jo["flag"] = "0";
                jo["message"] = "审核失败！";
            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 获取特殊预约的审批意见
        public void AuditSpecialMAAInfo()
        {
            JObject jo = new JObject();
            string MAAOrderId = HttpContext.Current.Request.Form["MAAOrderID"];
            DataTable dt = am.AuditSpecialMAAInfo(MAAOrderId);
            jo["info"] = JToken.Parse(JsonConvert.SerializeObject(dt));
            jo["flag"] = 1;
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 获取时间控制列表
        public void GetTimeControlList()
        {
            JObject jo = new JObject();
            jo["flag"] = 1;
            jo["TimeTable"] = JToken.Parse(JsonConvert.SerializeObject(am.GetTimeControlList()));
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 删除一条时间控制记录
        public void DelTimeControlList()
        {
            JObject jo = new JObject();
            string id = HttpContext.Current.Request.Form["id"];
            bool b = am.DelTimeControlList(id);
            if (b)
            {
                jo["flag"] = "1";
                jo["message"] = "删除成功！";
            }
            else
            {
                jo["flag"] = "0";
                jo["message"] = "删除失败！";
            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 新增一条时间控制
        public void AddTimeControl()
        {
            string start_date = HttpContext.Current.Request.Form["start_date"];
            string end_date = HttpContext.Current.Request.Form["end_date"];
            JObject jo = new JObject();
            bool b = am.AddTimeControl(start_date, end_date);
            if (b)
            {
                jo["flag"] = 1;
            }
            else
            {
                jo["flag"] = 0;
                jo["message"] = "出现错误，请重试或联系管理员！";
            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 数据库锁列表
        public void GetSqlLockList()
        {
            JObject jo = new JObject();
            jo["flag"] = 1;
            jo["LockTable"] = JToken.Parse(JsonConvert.SerializeObject(am.GetSqlLockList()));
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 杀数据库进程
        public void KillLock()
        {
            JObject jo = new JObject();
            string id = HttpContext.Current.Request.Form["id"];
            bool b = am.KillLock(id);
            if (b)
            {
                jo["flag"] = 1;
            }
            else
            {
                jo["flag"] = 0;
                jo["message"] = "出现错误，请重试或联系管理员！";
            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 门卫登录
        public void guardLogin()
        {
            JObject jo = new JObject();
            string name = HttpContext.Current.Request.Form["name"];
            string pwd = HttpContext.Current.Request.Form["password"];
            pwd = FormsAuthentication.HashPasswordForStoringInConfigFile(pwd, "MD5");
            DataTable dt = am.guardLogin(name, pwd);
            if (dt.Rows.Count > 0)
            {
                HttpContext.Current.Session["AdminlngopUserId"] = dt.Rows[0]["lngopUserId"].ToString();
                HttpContext.Current.Session["AdminstrLoginName"] = dt.Rows[0]["strLoginName"].ToString();
                HttpContext.Current.Session["AdminstrUserLevel"] = dt.Rows[0]["strUserLevel"].ToString();
                jo["flag"] = 1;
            }
            else
            {
                jo["flag"] = 0;
                jo["message"] = "登陆信息错误！";
            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;

        }
        #endregion

        #region 车辆进厂时门卫进行确认并写入进厂时间和放行人
        public void MAACarInUpd()
        {
            JObject jo = new JObject();
            if (HttpContext.Current.Session["AdminstrUserLevel"] == null || HttpContext.Current.Session["AdminstrUserLevel"].ToString() != "2")
            {
                jo["flag"] = 0;
                jo["message"] = "没有权限！";
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                return;
            }
            string cMAACode = HttpContext.Current.Request.Form["cMAACode"];
            bool b = am.MAACarInUpd(cMAACode, HttpContext.Current.Session["AdminstrLoginName"].ToString());
            if (b)
            {
                jo["flag"] = 1;
            }
            else
            {
                jo["flag"] = 0;
                jo["message"] = "确认信息出错，请重试或联系管理员！";
            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 车辆出厂时门卫进行确认并写入出厂时间和放行人
        public void MAACarOutUpd()
        {
            JObject jo = new JObject();
            if (HttpContext.Current.Session["AdminstrUserLevel"] == null || HttpContext.Current.Session["AdminstrUserLevel"].ToString() != "2")
            {
                jo["flag"] = 0;
                jo["message"] = "没有权限！";
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                return;
            }
            string cMAACode = HttpContext.Current.Request.Form["cMAACode"];
            bool b = am.MAACarOutUpd(cMAACode, HttpContext.Current.Session["AdminstrLoginName"].ToString());
            if (b)
            {
                jo["flag"] = 1;
            }
            else
            {
                jo["flag"] = 0;
                jo["message"] = "确认信息出错，请重试或联系管理员！";
            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 订单重量查询
        public void GetOrderWeight()
        {
            string Number = HttpContext.Current.Request.Form["Number"];
            string NumberType = HttpContext.Current.Request.Form["NumberType"];
            DataTable dt = am.GetOrderWeight(NumberType, Number);
            JObject jo = new JObject();
            jo["flag"] = 1;
            jo["table"] = JToken.Parse(JsonConvert.SerializeObject(dt));
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;

        }
        #endregion

        #region 获取延期通知单列表
        public void Get_ArrearList()
        {
            ReInfo ri = new ReInfo();
            string start_date = HttpContext.Current.Request.Form["start_date"];
            string end_date = HttpContext.Current.Request.Form["end_date"];
            string bytstatus = HttpContext.Current.Request.Form["bytstatus"];
            ri.dt = am.Get_ArrearList(start_date, end_date, bytstatus);
            ri.flag = "1";
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
        }
        #endregion

        #region 销售经理更新停止发货意见
        public void Update_Stop()
        {
            string code = HttpContext.Current.Request.Form["code"];
            string stopvalue = HttpContext.Current.Request.Form["StopValue"];
            string id = HttpContext.Current.Session["AdminlngopUserId"].ToString();
            string cMemo = HttpContext.Current.Request.Form["cMemo"].ToString();
            ReInfo ri = am.Update_Stop(id, stopvalue, cMemo, code);
            if (ri.flag == "1")
            {
                //获取该通知单主账号电话
                DataTable dt = am.Get_PhoneByArrearCode(code);
                string msg = string.Empty;
                if (dt.Rows[0]["bytStatus"].ToString() == "22")
                {
                    msg = "您有一张延期未结清账款通知单，请于两日内登录网上订单系统进行确认。如未按期确认，将影响您网上下单业务以及年终考评，感谢您的支持！";
                }
                else if (dt.Rows[0]["bytStatus"].ToString() == "23")
                {
                    msg = "您的货款已连续三月未结清，系统已自动控制下单。如有疑问，请及时咨询销售部负责人！";
                }

                string nums = dt.Rows[0]["cCusPhone"].ToString();
                string[] num_arr = nums.Split(';');
                nums = string.Join(",", num_arr);
                SMSSend9003.SendSMS(nums, msg);
                foreach (var num in num_arr)
                {
                    SMSSend9003.SendQY_Message_Text(num, "", "", "20", msg);
                }
            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            return;

        }
        #endregion

        #region 销售经理确认通知单
        public void Arrear_confirm()
        {
            ReInfo ri = new ReInfo();
            string code = HttpContext.Current.Request.Form["code"];
            string id = HttpContext.Current.Session["AdminlngopUserId"].ToString();
            string bytstatus = HttpContext.Current.Request.Form["bytStatus"];
            if (bytstatus != "31" && bytstatus != "32")
            {
                ri.flag = "0";
                ri.message = "该通知单状态不正确！";
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }
            int byt = 0;
            if (bytstatus == "31")
            {

                byt = 41;
                ri = am.Arrear_confirm(id, code, byt);
            }
            else if (bytstatus == "32")
            {
                if (HttpContext.Current.Request.Form["stopValue"] == "0")
                {
                    byt = 42;
                }
                else
                {
                    byt = 43;
                }
                ri = am.Arrear_confirm(id, code, byt, HttpContext.Current.Request.Form["stopValue"], HttpContext.Current.Request.Form["cMemo"]);
                if (ri.flag == "1")
                {

                    //获取该通知单主账号电话
                    DataTable dt = am.Get_PhoneByArrearCode(code);
                    string msg = string.Empty;
                    if (byt == 42)
                    {
                        msg = "您的网上下单业务已经恢复，感谢您的支持！";
                    }
                    else if (byt == 43)
                    {
                        msg = "您的货款已连续三月未结清，系统已自动控制下单。如有疑问，请及时咨询销售部负责人！";
                    }

                    string nums = dt.Rows[0]["cCusPhone"].ToString();
                    string[] num_arr = nums.Split(';');
                    nums = string.Join(",", num_arr);
                    SMSSend9003.SendSMS(nums, msg);
                    foreach (var num in num_arr)
                    {
                        SMSSend9003.SendQY_Message_Text(num, "", "", "20", msg);
                    }
                }
            }


            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            return;

        }
        #endregion

        #region 后台新增预约时获取所有配送订单
        public void GetPSOrders()
        {
            JObject jo = new JObject();
            string cSCCode = HttpContext.Current.Request.Form["cSCCode"];
            jo["flag"] = 1;
            jo["ordersTable"] = jo["ordersTable"] = JToken.Parse(JsonConvert.SerializeObject(am.GetMAAOrders("", cSCCode)));
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 获取预约界面初始化的时间段和可预约订单列表
        public void GetAdminMAA()
        {
            JObject jo = new JObject();
            string iType = HttpContext.Current.Request.Form["iType"];

            jo["timeTable"] = JToken.Parse(JsonConvert.SerializeObject(pro.Get_MAATimes(iType)));
            jo["ordersTable"] = JToken.Parse(JsonConvert.SerializeObject(am.GetMAAOrders("", "01")));
            jo["carTypes"] = JToken.Parse(JsonConvert.SerializeObject(new BasicInfoManager().DL_cdefine3BySel()));
            jo["shipping"] = JToken.Parse(JsonConvert.SerializeObject(common.GetShippingMethod()));
            jo["flag"] = 1;
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 提交预约信息
        public void DLproc_NewMAAOrderByIns()
        {
            string info = HttpContext.Current.Request.Form["info"];
            string cCusName = HttpContext.Current.Session["AdminstrUserName"].ToString();
            string cCusCode = HttpContext.Current.Session["AdminstrLoginName"].ToString();
            string strAllAcount = HttpContext.Current.Session["AdminstrAllAcount"].ToString();
            string iType = HttpContext.Current.Request.Form["iType"];

            JObject jo = new JObject();
            jo = JObject.Parse(info);
            jo["cCusCode"] = cCusCode;
            jo["cCusName"] = cCusName;
            jo["strAllAcount"] = strAllAcount;
            jo["iType"] = iType;
            jo["bytStatus"] = 4;
            if (jo["cSCCode"].ToString() != "00")
            {
                jo["ShippingMethod"] = "01";
            }
            else
            {
                jo["ShippingMethod"] = "00";
            }
            //检测数据
            List<string> list = new List<string>();
            foreach (string item in jo["orders"])
            {
                list.Add(item);
            }
            jo["o"] = string.Join(",", list);
            DataTable dt = pro.DLproc_MAACheckDataBySel(jo);

            //检测不通过时返回错误信息
            JObject j = new JObject();
            if (dt.Rows[0]["flag"].ToString() == "0")
            {
                j["flag"] = "0";
                j["message"] = dt.Rows[0]["ErrMsg"].ToString();
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(j));
                return;
            }


            //拼接MAAOderdetail表
            ArrayList arr = new ArrayList();
            foreach (int item in jo["orders"])
            {
                arr.Add(item);
            }
            DataTable MAAOrderDetail = return_MAAOrderDetail(arr);

            //插入数据
            JObject obj = pro.DLproc_NewMAAOrderByIns("DLproc_NewMAAOrderByIns", jo, MAAOrderDetail);

            if (obj["flag"].ToString() == "1")
            {
                j["flag"] = "1";
                j["cMAACode"] = obj["cMAACode"];

            }
            else
            {
                j["flag"] = "0";
                j["message"] = "预约失败，请重试或联系管理员！";
            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(j));
            return;
        }
        #endregion

        #region 拼接MAAOderDetail表
        public DataTable return_MAAOrderDetail(ArrayList list)
        {
            DataTable MAAOrderDetail = new DataTable();
            MAAOrderDetail.Columns.Add("MAAOrderDetailID", typeof(int));
            MAAOrderDetail.Columns.Add("MAAOrderID", typeof(int));
            MAAOrderDetail.Columns.Add("lngopOrderId", typeof(int));
            MAAOrderDetail.Columns.Add("SubbytStatus", typeof(int));
            MAAOrderDetail.Columns.Add("datAddTime", typeof(DateTime));
            MAAOrderDetail.Columns.Add("datModifyTime", typeof(DateTime));

            for (int i = 0; i < list.Count; i++)
            {
                MAAOrderDetail.Rows.Add(1, 1, list[i], 4, DateTime.Now, DateTime.Now);
            }
            MAAOrderDetail.TableName = "dl_opMAAOrderDetail";
            return MAAOrderDetail;
        }
        #endregion

        #region 获取预约号列表
        public void Get_MAAList()
        {
            string cCusCode = HttpContext.Current.Session["AdmincCusCode"].ToString();
            string iType = HttpContext.Current.Request.Form["iType"];
            JObject jo = new JObject();
            jo["flag"] = "1";
            jo["MAAList"] = JToken.Parse(JsonConvert.SerializeObject(am.Get_MAAList(iType)));
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 取出产品分类
        /// <summary>
        /// 取出产品分类
        /// </summary>
        /// <param name="ConstcCusCode"></param>
        /// <returns></returns>
        public void GetProductClass()
        {

            JObject jo = new JObject();
            DataTable dt = pro.DL_InventoryBySel("0", "999999");
            dt.Columns["KeyFieldName"].ColumnName = "id";
            dt.Columns["ParentFieldName"].ColumnName = "pid";
            dt.Columns["NodeName"].ColumnName = "name";
            jo["flag"] = "1";
            jo["proClass"] = JToken.Parse(JsonConvert.SerializeObject(dt));
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 根据分类获取商品列表
        /// <summary>
        /// 根据分类获取商品列表
        /// </summary>
        /// <param name="ConstcCusCode"></param>
        /// <returns></returns>
        public void GetProductList()
        {
            JObject jo = new JObject();
            string cInvCCode = HttpContext.Current.Request.Form["cInvCCode"].ToString();
            DataTable dt = pro.DL_TreeListDetailsAllBySel(cInvCCode, "999999", "1");
            jo["flag"] = "1";
            jo["proList"] = JToken.Parse(JsonConvert.SerializeObject(dt));
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 获取产品现存量
        public void GetStock()
        {

            string cInvCCode = HttpContext.Current.Request.Form["cInvCCode"];
            DataTable dt = am.DLproc_TreeListDetailsAll_iqty_BySel(cInvCCode, "999999");
            JObject jo = new JObject();
            jo["flag"] = 1;
            jo["proList"] = JToken.Parse(JsonConvert.SerializeObject(dt));
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 根据预约ID获取该预约的主表记录
        //public void MAAOrderId() {
        //    string MAAOrderId = HttpContext.Current.Request.Form["MAAOrderId"];
        //    DataTable dt = new DataTable();
        //    dt=am.MAAOrderId(MAAOrderId);
        //}
        #endregion

        #region 获取所有的反馈
        public void GetAllFeedBack()
        {
            DataTable dt = am.GetAllFeedBack();
            JObject jo = new JObject();
            jo["flag"] = 1;
            jo["data"] = JToken.Parse(JsonConvert.SerializeObject(dt));
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 管理员回复反馈
        public void SaveFeedBack()
        {
            string lngopCusFeedbackId = HttpContext.Current.Request.Form["lngopCusFeedbackId"];
            string content = HttpContext.Current.Request.Form["content"];
            string ccuscode = HttpContext.Current.Session["AdminstrLoginName"].ToString();
            bool b = am.SaveFeedBack(content, ccuscode, lngopCusFeedbackId);
            JObject jo = new JObject();
            if (b)
            {
                jo["flag"] = 1;
                //SMSSend9003.SendSMS2CustomerSoapClient s = new SMSSend9003.SendSMS2CustomerSoapClient();
                //s.SendQY_Message_Text("13438904933", "", "", "22", "你收到一条新反馈");

            }
            else
            {
                jo["flag"] = 0;
                jo["message"] = "提交错误，请重试或联系管理员！";

            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;

        }
        #endregion

        #region superadmin后台主页获取数据
        public void SuperAdminIndex()
        {
            JObject jo = am.SuperAdminIndex();
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region  获取权限DataSet
        public void GetAllRole()
        {
            JObject jo = new JObject();
            DataSet ds = new DataSet();
            string xmlPath = "test/test.xml";
            string systemPath = System.AppDomain.CurrentDomain.BaseDirectory;
            string path = systemPath + xmlPath;
            ds.ReadXml(path);
            jo["flag"] = 1;
            jo["DataSet"] = JToken.Parse(JsonConvert.SerializeObject(ds));
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 权限操作增删改
        public void ModifyRol()
        {
            JObject jo = new JObject();
            DataSet ds = new DataSet();
            string tablename = HttpContext.Current.Request.Form["tablename"];
            string even = HttpContext.Current.Request.Form["event"];
            string xmlPath = "test/test.xml";
            string systemPath = System.AppDomain.CurrentDomain.BaseDirectory;
            string path = systemPath + xmlPath;
            ds.ReadXml(path);

            #region 对User表操作

            if (tablename == "user")
            {
                DataTable dt = ds.Tables["user"];
                string loginname = HttpContext.Current.Request.Form["loginname"];
                DataView dv = dt.DefaultView;
                dv.RowFilter = "loginname = " + loginname;

                if (even == "edit")
                {

                    if (dv.Count > 1)
                    {
                        jo["flag"] = 0;
                        jo["message"] = "存在多条相同登录账号的记录，请核实！";
                        HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                        return;
                    }
                    else if (dv.Count == 0)
                    {
                        jo["flag"] = 0;
                        jo["message"] = "未能获取该条登录账号的记录，请核实！";
                        HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                        return;
                    }
                    dv[0]["groupid"] = HttpContext.Current.Request.Form["groupid"];
                    dv[0]["user_Text"] = HttpContext.Current.Request.Form["user_Text"];

                }
                else if (even == "del")
                {

                    if (dv.Count > 1)
                    {
                        jo["flag"] = 0;
                        jo["message"] = "存在多条相同登录账号的记录，请核实！";
                        HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                        return;
                    }
                    else if (dv.Count == 0)
                    {
                        jo["flag"] = 0;
                        jo["message"] = "未能获取该条登录账号的记录，请核实！";
                        HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                        return;
                    }

                    dv.Delete(0);


                }
                else if (even == "add")
                {
                    if (dv.Count > 0)
                    {
                        jo["flag"] = 0;
                        jo["message"] = "已存在此登录账号，请勿重复添加！";
                        HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                        return;
                    }
                    DataRow dr = dt.NewRow();
                    dr["loginname"] = HttpContext.Current.Request.Form["loginname"];
                    dr["user_Text"] = HttpContext.Current.Request.Form["username"];
                    dr["groupid"] = HttpContext.Current.Request.Form["groupid"];
                    dt.Rows.Add(dr);

                }


                ds.WriteXml(path);
            }
            #endregion

            #region 对Group表操作
            if (tablename == "group")
            {
                DataTable dt = ds.Tables["group"];
                string groupid = HttpContext.Current.Request.Form["groupid"];
                DataView dv = dt.DefaultView;
                dv.RowFilter = "groupid = " + groupid;

                if (even == "edit")
                {

                    if (dv.Count > 1)
                    {
                        jo["flag"] = 0;
                        jo["message"] = "存在多条相同用户组ID的记录，请核实！";
                        HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                        return;
                    }
                    else if (dv.Count == 0)
                    {
                        jo["flag"] = 0;
                        jo["message"] = "未能获取该条用户组ID的记录，请核实！";
                        HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                        return;
                    }
                    dv[0]["groupid"] = HttpContext.Current.Request.Form["groupid"];
                    dv[0]["descraption"] = HttpContext.Current.Request.Form["descraption"];
                    dv[0]["methodids"] = HttpContext.Current.Request.Form["methodids"];

                }
                else if (even == "del")
                {

                    if (dv.Count > 1)
                    {
                        jo["flag"] = 0;
                        jo["message"] = "存在多条相同用户组记录，请核实！";
                        HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                        return;
                    }
                    else if (dv.Count == 0)
                    {
                        jo["flag"] = 0;
                        jo["message"] = "未能获取该条用户组的记录，请核实！";
                        HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                        return;
                    }

                    dv.Delete(0);


                }
                else if (even == "add")
                {
                    if (dv.Count > 0)
                    {
                        jo["flag"] = 0;
                        jo["message"] = "已存在此用户组ID，请勿重复添加！";
                        HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                        return;
                    }
                    DataRow dr = dt.NewRow();
                    dr["groupid"] = HttpContext.Current.Request.Form["groupid"];
                    dr["descraption"] = HttpContext.Current.Request.Form["descraption"];
                    dr["methodids"] = HttpContext.Current.Request.Form["methodids"];
                    dt.Rows.Add(dr);

                }


                ds.WriteXml(path);
            }
            #endregion

            #region 对Method表操作
            else if (tablename == "method")
            {
                DataTable dt = ds.Tables["method"];
                string methodid = HttpContext.Current.Request.Form["methodid"];
                string methodname = HttpContext.Current.Request.Form["methodname"];
                DataView dv_id = new DataView(dt);
                dv_id.RowFilter = "methodid = " + methodid;
                DataView dv_name = new DataView(dt);
                dv_name.RowFilter = "methodname like '" + methodname + "'";


                if (even == "edit")
                {
                    if (dv_id.Count > 1)
                    {
                        jo["flag"] = 0;
                        jo["message"] = "存在多条相同方法ID的记录，请核实！";
                        HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                        return;
                    }
                    else if (dv_id.Count == 0)
                    {
                        jo["flag"] = 0;
                        jo["message"] = "未能获取该条方法ID的记录，请核实！";
                        HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                        return;
                    }
                    if (dv_name.Count > 1)
                    {
                        jo["flag"] = 0;
                        jo["message"] = "存在多条相同方法名的记录，请核实！";
                        HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                        return;
                    }
                    else if (dv_name.Count == 0)
                    {
                        jo["flag"] = 0;
                        jo["message"] = "未能获取该条方法名的记录，请核实！";
                        HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                        return;
                    }

                    dv_id[0]["methodname"] = HttpContext.Current.Request.Form["methodname"];
                    dv_id[0]["descraption"] = HttpContext.Current.Request.Form["descraption"];


                }
                else if (even == "del")
                {

                    if (dv_id.Count > 1)
                    {
                        jo["flag"] = 0;
                        jo["message"] = "存在多条相同方法ID的记录，请核实！";
                        HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                        return;
                    }
                    else if (dv_id.Count == 0)
                    {
                        jo["flag"] = 0;
                        jo["message"] = "未能获取该条方法ID的记录，请核实！";
                        HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                        return;
                    }
                    if (dv_name.Count > 1)
                    {
                        jo["flag"] = 0;
                        jo["message"] = "存在多条相同方法名的记录，请核实！";
                        HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                        return;
                    }
                    else if (dv_name.Count == 0)
                    {
                        jo["flag"] = 0;
                        jo["message"] = "未能获取该条方法名的记录，请核实！";
                        HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                        return;
                    }
                    dv_id.Delete(0);
                }
                else if (even == "add")
                {
                    if (dv_id.Count > 0)
                    {
                        jo["flag"] = 0;
                        jo["message"] = "已存在此方法ID，请勿重复添加！";
                        HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                        return;
                    }
                    if (dv_name.Count > 0)
                    {
                        jo["flag"] = 0;
                        jo["message"] = "已存在此方法名，请勿重复添加！";
                        HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                        return;
                    }
                    DataRow dr = dt.NewRow();
                    dr["methodid"] = methodid;
                    dr["methodname"] = methodname;
                    dr["methodgroupid"] = HttpContext.Current.Request.Form["methodgroupid"]; ;
                    dr["descraption"] = HttpContext.Current.Request.Form["descraption"];
                    dr["method_Text"] = methodname;
                    dt.Rows.Add(dr);

                }


                ds.WriteXml(path);
            }
            #endregion

            jo["flag"] = 1;
            jo["DataSet"] = JToken.Parse(JsonConvert.SerializeObject(ds));
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 模拟客户登录
        public void SimulateLogin()
        {
            string phone = HttpContext.Current.Request.Form["phone"];
            string ccuscode = HttpContext.Current.Request.Form["ccuscode"];
            JObject jo = new JObject();
            if (!string.IsNullOrEmpty(phone) && !string.IsNullOrEmpty(ccuscode))
            {
                DataTable dt = am.SimulateLogin(phone, ccuscode);

                if (dt.Rows.Count == 0)
                {
                    jo["flag"] = 0;
                    jo["message"] = "未查询到客户数据，登录失败，请重试或联系管理员！";
                    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                    return;
                }
                else {
                    
                    HttpContext.Current.Session["login"] = "1";
                    HttpContext.Current.Session["strLoginPhone"] = phone;
                    HttpContext.Current.Session["strUserLevel"] = dt.Rows[0]["strUserLevel"].ToString();       //权限HttpContext.Current.Session
                    HttpContext.Current.Session["lngopUserId"] = dt.Rows[0]["lngopUserId"].ToString();         //登录用户id
                    HttpContext.Current.Session["cCusCode"] = dt.Rows[0]["cCusCode"].ToString();               //用户编码
                    HttpContext.Current.Session["ConstcCusCode"] = dt.Rows[0]["cCusCode"].ToString();          //常量,登录用户编码,不变更
                    HttpContext.Current.Session["cCusPhone"] = phone;                                     //顾客电话号码*
                    HttpContext.Current.Session["strLoginName"] = dt.Rows[0]["strLoginName"].ToString();  //登录编码,2016-03-30增加
                    HttpContext.Current.Session["lngopUserExId"] = dt.Rows[0]["lngopUserExId"].ToString();  //子账户id,2016-08-26增加
                    HttpContext.Current.Session["strAllAcount"] =ccuscode;  //子账户编码,2016-08-26增加，如无子账户则为主账户编码,不变更的常量  
                }
                 
                jo["flag"] = 1;
                jo["message"] = "登录成功";
            }
            else
            {
                jo["flag"] = 0;
                jo["message"] = "客户信息有误，请重试或联系管理员！";

            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
            //  am.SimulateLogin()

            //string phone = HttpContext.Current.Request.Form["phone"];
            //string type = HttpContext.Current.Request.Form["type"];
            //string ccuscode = HttpContext.Current.Request.Form["ccuscode"];
            //DataTable dt = am.SimulateLogin(phone, type, ccuscode);
            //JObject jo = new JObject();
            //if (dt.Rows.Count == 0)
            //{
            //    jo["flag"] = 0;
            //    jo["message"] = "未获取该电话号码的客户，请重新核对！";
            //    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            //    return;
            //}
            //else if (dt.Rows.Count > 1)
            //{
            //    jo["flag"] = 2;
            //    jo["users"] = JToken.Parse(JsonConvert.SerializeObject(dt));
            //    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            //    return;
            //}
            //else
            //{
            //    if (dt.Rows[0]["strUserLevel"].ToString() != "3")
            //    {
            //        jo["flag"] = 0;
            //        jo["message"] = "该电话号码不属于客户，请重新核对！";
            //        HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            //        return;
            //    }
            //    else
            //    {
            //        HttpContext.Current.Session["login"] = "1";
            //        HttpContext.Current.Session["strLoginPhone"] = phone;
            //        HttpContext.Current.Session["strUserLevel"] = dt.Rows[0]["strUserLevel"].ToString();       //权限HttpContext.Current.Session
            //        HttpContext.Current.Session["lngopUserId"] = dt.Rows[0]["lngopUserId"].ToString();         //登录用户id
            //        HttpContext.Current.Session["strUserName"] = dt.Rows[0]["strUserName"].ToString();         //用户名称
            //        HttpContext.Current.Session["cCusCode"] = dt.Rows[0]["cCusCode"].ToString();               //用户编码
            //        HttpContext.Current.Session["ConstcCusCode"] = dt.Rows[0]["cCusCode"].ToString();          //常量,登录用户编码,不变更
            //        HttpContext.Current.Session["cCusPPerson"] = dt.Rows[0]["cCusPPerson"].ToString();  //业务员
            //        HttpContext.Current.Session["cCusPhone"] = dt.Rows[0]["cCusPhone"].ToString();  //顾客电话号码*
            //        HttpContext.Current.Session["strLoginName"] = dt.Rows[0]["strLoginName"].ToString();  //登录编码,2016-03-30增加
            //        HttpContext.Current.Session["lngopUserExId"] = dt.Rows[0]["lngopUserExId"].ToString();  //子账户id,2016-08-26增加
            //        HttpContext.Current.Session["strAllAcount"] = dt.Rows[0]["strAllAcount"].ToString();  //子账户编码,2016-08-26增加，如无子账户则为主账户编码,不变更的常量   

            //        jo["flag"] = 1;
            //        HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            //        return;
            //    }
            //}

        }


        #endregion

        #region 后台提交普通订单
        public void DLproc_NewOrderByIns_Admin()
        {
            string head = HttpContext.Current.Request.Form["HeadData"];
            JObject jo = new JObject();
            JObject HeadData = JObject.Parse(head);
            HeadData["cpersoncode"] = HeadData["ccuspperson"];
            HeadData["lngopUserId"] = HttpContext.Current.Session["lngopUserId"].ToString();
            //HeadData["datCreateTime"] = DateTime.Now.ToString();
            HeadData["bytStatus"] = 2;
            HeadData["lngopUserExId"] = HttpContext.Current.Session["lngopUserExId"].ToString();
            HeadData["strallAcount"] = HttpContext.Current.Session["strAllAcount"].ToString();
            HeadData["strU8BillNo"] = "";
            HeadData["cdiscountname"] = order.DLproc_getCusCreditInfo(HeadData["ccuscode"].ToString()).Rows[0]["cdiscountname"].ToString();
            HeadData["strManagers"] = HttpContext.Current.Session["AdminlngopUserId"].ToString();
            DataTable Address_Dt = pro.Get_AddressById(HeadData["lngopUseraddressId"].ToString());
            HeadData["cdefine1"] = Address_Dt.Rows[0]["strDriverName"].ToString();
            HeadData["cdefine2"] = Address_Dt.Rows[0]["strIdCard"].ToString();
            HeadData["cdefine9"] = Address_Dt.Rows[0]["strConsigneeName"].ToString();
            HeadData["cdefine10"] = Address_Dt.Rows[0]["strCarplateNumber"].ToString();
            HeadData["cdefine12"] = Address_Dt.Rows[0]["strConsigneeTel"].ToString();
            HeadData["cdefine13"] = Address_Dt.Rows[0]["strDriverTel"].ToString();
            //HeadData["adminCreateOrderTime"] = DateTime.Now.ToString();
            HeadData["adminCreateOrderName"] = HttpContext.Current.Session["AdminstrUserName"].ToString();


            jo = am.DLproc_NewOrderByIns_Admin("DLproc_NewOrderByIns_Admin", HeadData);


            //string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx85ee38394e42f0b7&redirect_uri=http://dl.duolian.com:8001/WeiXin/html/RoleCheck.html&response_type=code&scope=snsapi_userinfo&state=order_info.html?orderCode={0}&connect_redirect=1#wechat_redirect";
            JObject j = new JObject();
            j["orderType"] = 1;
            j["orderId"] = jo["orderId"];
            string url = String.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx85ee38394e42f0b7&redirect_uri=http://dl.duolian.com:8001/WeiXin/html/RoleCheck.html&response_type=code&scope=snsapi_userinfo&state=order_info.html?orderCode={0}&connect_redirect=1#wechat_redirect", check.EncryptDES(JsonConvert.SerializeObject(j)));
            string a=   wx9003.SendMsg_TextCard(HttpContext.Current.Session["cCusPhone"].ToString(), "", "", "1000003", "订单确认", "你有一张普通订单：" + jo["code"].ToString() + " 需要确认", url);
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 下单成功后，给客户发送消息，等待客户确认订单
        public void SendByOrderWXMsg() { 
            
        }
        #endregion

        #region 后台提交特殊订单
        public void DLproc_NewYOrderByIns_Admin()
        {
            string head = HttpContext.Current.Request.Form["HeadData"];
            JObject jo = new JObject();
            JObject HeadData = JObject.Parse(head);
            HeadData["ddate"] = DateTime.Now.ToString();
            HeadData["lngopUserId"] = HttpContext.Current.Session["lngopUserId"].ToString();
            HeadData["lngBillType"] = 2;
            HeadData["bytStatus"] = 2;
            HeadData["lngopUserExId"] = HttpContext.Current.Session["lngopUserExId"].ToString();
            HeadData["strAllAcount"] = HttpContext.Current.Session["strAllAcount"].ToString();
            HeadData["cdiscountname"] = order.DLproc_getCusCreditInfo(HeadData["ccuscode"].ToString()).Rows[0]["cdiscountname"].ToString() + "-特殊订单";
            HeadData["strManagers"] = HttpContext.Current.Session["AdminlngopUserId"].ToString();
            HeadData["adminCreateOrderName"] = HttpContext.Current.Session["AdminstrUserName"].ToString();
            jo = am.DLproc_NewYOrderByIns_Admin("DLproc_NewYOrderByIns_Admin", HeadData);


            JObject j = new JObject();
            j["orderType"] = 2;
            j["orderId"] = jo["orderId"];
            string url = String.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx85ee38394e42f0b7&redirect_uri=http://dl.duolian.com:8001/WeiXin/html/RoleCheck.html&response_type=code&scope=snsapi_userinfo&state=order_info.html?orderCode={0}&connect_redirect=1#wechat_redirect", check.EncryptDES(JsonConvert.SerializeObject(j)));
            wx9003.SendMsg_TextCard(HttpContext.Current.Session["cCusPhone"].ToString(), "", "", "1000003", "订单确认", "你有一张特殊订单：" + jo["code"].ToString() + " 需要确认", url);
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 获取代客户下单列表
        public void GetBuyOrderList() {
            JObject jo = new JObject();
            jo["flag"] = 1;
            jo["data"] = JsonConvert.SerializeObject(am.GetBuyOrderList(),timejson);
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;

        }
        #endregion

        #region 代客户下单后作废订单
        public void CancelOrder()
        {
            string lngopOrderId = HttpContext.Current.Request.Form["lngopOrderId"];

            JObject jo = am.CancelOrder(lngopOrderId, HttpContext.Current.Session["AdminlngopUserId"].ToString(), HttpContext.Current.Session["AdminstrUserName"].ToString());
         
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 获取数据库表（Type 0为模糊查询，1为精确查询）
        public void GetSqlTables()
        {
            JObject jo = new JObject();
            string TableName = HttpContext.Current.Request.Form["TableName"];

            jo["flag"] = 1;
            jo["table"] = JToken.Parse(JsonConvert.SerializeObject(am.GetSqlTables(TableName, HttpContext.Current.Request.Form["Type"])));
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 获取数据库表详情
        public void GetSqlTableDetail()
        {
            JObject jo = new JObject();
            string TableName = HttpContext.Current.Request.Form["TableName"];

            jo["flag"] = 1;
            jo["table"] = JToken.Parse(JsonConvert.SerializeObject(am.GetSqlTableDetail(TableName)));
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;

        }

        #endregion

        #region 更新数据库表和字段说明，参数Column和ColumnName为null时为更新表说明，否则为更新表中的字段说明
        public void UpdateSqlTableDescription()
        {

            string Description = HttpContext.Current.Request.Form["Description"];
            string TableName = HttpContext.Current.Request.Form["TableName"];
            string Column = HttpContext.Current.Request.Form["Column"];
            string ColumnName = HttpContext.Current.Request.Form["ColumnName"];
            JObject jo = new JObject();

            jo = am.UpdateSqlTableDescription(Description, TableName, Column, ColumnName);
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 新增发送短信客户分组
        public void AddAdminCustomerGroup()
        {
            JObject jo = new JObject();
            string groupName = HttpContext.Current.Request.Form["groupName"];
            string groupParent = HttpContext.Current.Request.Form["groupParent"];
            string userId = HttpContext.Current.Session["AdminlngopUserId"].ToString();

            if (!am.AddAdminCustomerGroup(groupName, groupParent, userId))
            {
                jo["flag"] = 0;
                jo["message"] = "新增出错,请重试或联系管理员!";
            }
            else
            {
                jo["flag"] = 1;
            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 根据分组创建者ID获取所有发送短信客户分组
        public void GetAllAdminCustomerGroupByCreateId()
        {
            string userId = HttpContext.Current.Session["AdminlngopUserId"].ToString();
            JObject jo = new JObject();
            jo["flag"] = 1;
            jo["group"] = JToken.Parse(JsonConvert.SerializeObject(am.GetAllAdminCustomerGroupByCreateId(userId), timejson));
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 删除发送短信的客户分组
        public void DelAdminCustomerGroupById()
        {
            string groupId = HttpContext.Current.Request.Form["id"];
            JObject jo = new JObject();
            if (!am.DelAdminCustomerGroupById(groupId))
            {
                jo["flag"] = 0;
                jo["message"] = "删除出错,请重试或联系管理员!";
            }
            else
            {
                jo["flag"] = 1;
            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 编辑发送短信的客户分组
        public void EditAdminCustomerGroup()
        {
            JObject jo = new JObject();
            string groupId = HttpContext.Current.Request.Form["groupId"];
            string groupName = HttpContext.Current.Request.Form["groupName"];
            string groupParentId = HttpContext.Current.Request.Form["groupParentId"];
            if (!am.EditAdminCustomerGroup(groupId, groupName, groupParentId))
            {
                jo["flag"] = 0;
                jo["message"] = "新增出错,请重试或联系管理员!";
            }
            else
            {
                jo["flag"] = 1;
            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region  根据分组创建者ID获取所有发送短信客户
        public void GetAllAdminCustomerByCreateId()
        {
            JObject jo = new JObject();
            string userId = HttpContext.Current.Session["AdminlngopUserId"].ToString();
            jo["flag"] = 1;
            jo["customers"] = JToken.Parse(JsonConvert.SerializeObject(am.GetAllAdminCustomerByCreateId(userId), timejson));
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 添加发送短信的客户
        public void AddAdminCustomer()
        {
            JObject jo = new JObject();
            string userId = HttpContext.Current.Session["AdminlngopUserId"].ToString();
            string customerName = HttpContext.Current.Request.Form["customerName"];
            string customerPhone = HttpContext.Current.Request.Form["customerPhone"];
            string customerGroupId = HttpContext.Current.Request.Form["customerGroupId"];
            if (!am.AddAdminCustomer(customerName, customerPhone, customerGroupId, userId))
            {
                jo["flag"] = 0;
                jo["message"] = "新增出错,请重试或联系管理员!";
            }
            else
            {
                jo["flag"] = 1;
            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 删除发送短信的客户
        public void DelAdminCustomerById()
        {
            string customerId = HttpContext.Current.Request.Form["customerId"];
            JObject jo = new JObject();
            if (!am.DelAdminCustomerById(customerId))
            {
                jo["flag"] = 0;
                jo["message"] = "删除出错,请重试或联系管理员!";
            }
            else
            {
                jo["flag"] = 1;
            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 发送短信
        public void AdminSendSMS()
        {
            string phones = HttpContext.Current.Request.Form["phones"];
            string content = HttpContext.Current.Request.Form["content"];
            JObject jo = new JObject();
            bool b = SMSSend9003.SendSMS(phones, content);
            if (!b)
            {
                jo["flag"] = 0;
                jo["message"] = "发送短信出现错误,请重试或联系管理员!";
            }
            else
            {
                jo["flag"] = 1;
            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 编辑发送短信的客户
        public void EditAdminCustomer()
        {
            JObject jo = new JObject();
            string customerId = HttpContext.Current.Request.Form["customerId"];
            string customerName = HttpContext.Current.Request.Form["customerName"];
            string customerPhone = HttpContext.Current.Request.Form["customerPhone"];
            string customerGroupId = HttpContext.Current.Request.Form["customerGroupId"];
            if (!am.EditAdminCustomer(customerId, customerName, customerPhone, customerGroupId))
            {
                jo["flag"] = 0;
                jo["message"] = "编辑出错,请重试或联系管理员!";
            }
            else
            {
                jo["flag"] = 1;
            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

    }

}