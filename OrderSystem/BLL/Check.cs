using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Model;
using System.IO;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
 

namespace BLL
{
    public class Check
    {
        //ReInfo flag返回值:
        //0:一般为数据有问题,客户需要重新修改
        //1:数据正确,提交成功
        //4:try catch 里程序报错,写入日志,并返回提示
        //7:有限销商品,返回限销的商品编码及名称,前台自动删除限销商品并给出提示
        product pro = new product();
        OrderManager order = new OrderManager();
        public string sKey = "duoliana";
        DAL.SQLHelper sqlhper = new DAL.SQLHelper();

        #region 检查允限销
        public ReInfo Check_limit(DataTable check_dt, string kpdw, int iShowType)
        {
            ReInfo ri = new ReInfo();
            ri.flag = "1";
            ri.limit_code = new List<string>();
            ri.limit_name = new List<string>();
            for (int i = 0; i < check_dt.Rows.Count; i++)
            {
                if (!pro.DLproc_cInvCodeIsBeLimitedBySel(check_dt.Rows[i]["cInvCode"].ToString(), kpdw, iShowType))
                {
                    ri.flag = "7";
                    ri.limit_code.Add(check_dt.Rows[i]["cInvCode"].ToString());
                    ri.limit_name.Add(check_dt.Rows[i]["cInvName"].ToString());

                }
            }
            return ri;
        }
        #endregion

        #region 检查允限销
        public JObject Check_limit(string cInvCode, string kpdw, int iShowType)
        {
            JObject jo = new JObject();
            jo["flag"] = 1;

            if (!pro.DLproc_cInvCodeIsBeLimitedBySel(cInvCode, kpdw, iShowType))
            {
                jo["flag"] = 0;
                jo["message"] = "该开票单位不能购买该产品！";

            }

            return jo;
        }
        #endregion

        #region 订单时间控制
        public ReInfo Check_Time(string ConstcCusCode)
        {
            ReInfo ri = new ReInfo();
            ri.flag = "1";
            //开启时间管理
            //DataTable timecontrol = order.DL_OrderENTimeControlBySel();
            //if (timecontrol.Rows.Count < 1)
            //{
            //    ri.flag = "5";
            //    ri.message = "订单开放时间为每日8:00 - 21:00!";
            //    return ri;
            //}

            DataTable TimeControl = pro.DLproc_OPTimeCheckBySel();
            if (TimeControl.Rows[0]["flag"].ToString() != "1")
            {
                ri.flag = "5";
                ri.message = TimeControl.Rows[0]["msg"].ToString();
                return ri;
            }
            //逾期30天未确认
            DataTable dt = order.DL_NotConfirmedSOABySel(ConstcCusCode + "%");

            ri.flag = "1";
            ri.message = "尊敬的客户： <br>";
            string type = string.Empty;
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["ccount"].ToString() != "0")
                {
                    ri.flag = "5";
                    ri.message += "您有：" + dr["ccount"] + "张" + dr["type"] + "<br>";
                    type = dr["type"].ToString();
                }
            }
            ri.message += "为了不影响您的正常下单，请尽快对" + type + "进行确认。";
            // ri.message = "尊敬的客户您好！您有对账单或延期结款通知单未确认！若未确认，将无法进行下单业务！";

            return ri;
        }
        #endregion

        #region 检查是否有对账单未确认
        public ReInfo Check__NotConfirmedSOA(string ccuscode)
        {
            ReInfo ri = new ReInfo();
            DataTable dt = order.DL_NotConfirmedSOABySel(ccuscode);
            if (dt.Rows.Count > 0)
            {
                ri.flag = "5";
                ri.dt = dt;
            }
            return ri;
        }
        #endregion

        #region 错误日志记录
        public void WriteLog(ReInfo ri)
        {
            string sFilePath = ConfigurationManager.AppSettings.Get("logPath");
            string sFileName = DateTime.Now.ToString("yyyy-MM-dd") + ".log";
            sFileName = sFilePath + "\\" + sFileName; //文件的绝对路径
            if (!Directory.Exists(sFilePath))//验证路径是否存在
            {
                Directory.CreateDirectory(sFilePath);
                //不存在则创建
            }
            FileStream fs;
            StreamWriter sw;
            if (File.Exists(sFileName))
            //验证文件是否存在，有则追加，无则创建
            {
                fs = new FileStream(sFileName, FileMode.Append, FileAccess.Write);
            }
            else
            {
                fs = new FileStream(sFileName, FileMode.Create, FileAccess.Write);
            }
            sw = new StreamWriter(fs);
            sw.WriteLine("---------------------------------------------------------------------------------------------------------------------------------------------");
            foreach (var item in ri.list_msg)
            {
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "   ---   " + item);
            }

            sw.Close();
            fs.Close();

        }
        #endregion

        #region 错误日志记录
        public void WriteLog(JObject jo)
        {
            string sFilePath = ConfigurationManager.AppSettings.Get("logPath");
            string sFileName = DateTime.Now.ToString("yyyy-MM-dd") + ".log";
            sFileName = sFilePath + "\\" + sFileName; //文件的绝对路径
            if (!Directory.Exists(sFilePath))//验证路径是否存在
            {
                Directory.CreateDirectory(sFilePath);
                //不存在则创建
            }
            FileStream fs;
            StreamWriter sw;
            if (File.Exists(sFileName))
            //验证文件是否存在，有则追加，无则创建
            {
                fs = new FileStream(sFileName, FileMode.Append, FileAccess.Write);
            }
            else
            {
                fs = new FileStream(sFileName, FileMode.Create, FileAccess.Write);
            }
            sw = new StreamWriter(fs);
            sw.WriteLine("---------------------------------------------------------------------------------------------------------------------------------------------");
            sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "   ---   " + jo["message"]);
            sw.Close();
            fs.Close();

        }
        #endregion



        #region 加密
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="pToEncrypt"></param>
        /// <returns>dd</returns>
        public string EncryptDES(string pToEncrypt)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                byte[] inputByteArray = Encoding.UTF8.GetBytes(pToEncrypt);
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Convert.ToBase64String(ms.ToArray());
                ms.Close();
                return str;
            }
        }
        #endregion

        #region DES解密
        public string DecryptDES(string pToDecrypt)
        {
            byte[] inputByteArray = Convert.FromBase64String(pToDecrypt);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                MemoryStream ms = new MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
                return str;
            }
        }
        #endregion

        #region 跳转登录验证
        public ReInfo Direct_Login(string username, string phone, string pwd)
        {
            ReInfo ri = new ReInfo();
            DataTable dt = new product().Direct_Login(username, phone, pwd);
            bool b = false;
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["strStatus"].ToString() == "0")
                {
                    ri.flag = "0";
                    ri.message = "该帐号已被禁用";
                    return ri;
                }
                b = true;
                #region HttpContext.Current.Session赋值
                HttpContext.Current.Session["pwd"] = pwd;
                HttpContext.Current.Session["strUserLevel"] = dt.Rows[0]["strUserLevel"].ToString();       //权限HttpContext.Current.Session
                HttpContext.Current.Session["lngopUserId"] = dt.Rows[0]["lngopUserId"].ToString();         //登录用户id
                HttpContext.Current.Session["strUserName"] = dt.Rows[0]["strUserName"].ToString();         //用户名称
                HttpContext.Current.Session["KPDWcCusCode"] = dt.Rows[0]["cCusCode"].ToString();           //开票单位编码
                HttpContext.Current.Session["cCusCode"] = dt.Rows[0]["cCusCode"].ToString();               //用户编码
                HttpContext.Current.Session["login"] = "1";                                     //
                HttpContext.Current.Session["ConstcCusCode"] = dt.Rows[0]["cCusCode"].ToString();          //常量,登录用户编码,不变更
                HttpContext.Current.Session["cSTCode"] = "0";                                              //销售类型:默认,普通销售 
                HttpContext.Current.Session["cCusPPerson"] = dt.Rows[0]["cCusPPerson"].ToString();  //业务员
                HttpContext.Current.Session["cCusPhone"] = dt.Rows[0]["cCusPhone"].ToString();  //顾客电话号码*
                HttpContext.Current.Session["strLoginName"] = dt.Rows[0]["strLoginName"].ToString();  //登录编码,2016-03-30增加
                HttpContext.Current.Session["lngopUserExId"] = dt.Rows[0]["lngopUserExId"].ToString();  //子账户id,2016-08-26增加
                HttpContext.Current.Session["strAllAcount"] = dt.Rows[0]["strAllAcount"].ToString();  //子账户编码,2016-08-26增加，如无子账户则为主账户编码,不变更的常量          	
                //创建一个HASHTABLE
                Hashtable ht = new Hashtable();//创建一个Hashtable实例
                ht.Add("IsExercisePrice", dt.Rows[0]["IsExercisePrice"].ToString());//添加key/键值对
                HttpContext.Current.Session["SysSetting"] = ht;
                #endregion
            }

            if (b)
            {
                //登录成功,3,客户登录,跳转
                if ("3" == HttpContext.Current.Session["strUserLevel"].ToString())
                {

                    //写入HttpContext.Current.Application中
                    if (HttpContext.Current.Application["Online"] != null)
                    {
                        Hashtable hOnline = (Hashtable)HttpContext.Current.Application["Online"];
                        //删除当前顾客的记录
                        if (hOnline.Contains(HttpContext.Current.Session["strAllAcount"].ToString()))  //判断哈希表是否包含特定键,其返回值为true或false
                        {
                            hOnline.Remove(HttpContext.Current.Session["strAllAcount"].ToString());//移除一个key/value键值对
                        }
                        string sId = HttpContext.Current.Session.SessionID.ToString();  //获取当前HttpContext.Current.Session的id
                        hOnline.Add(HttpContext.Current.Session["strAllAcount"].ToString(), sId);//添加key/value键值对
                        HttpContext.Current.Application["Online"] = hOnline;
                    }
                    else
                    {
                        Hashtable hOnline = new Hashtable();
                        string sId = HttpContext.Current.Session.SessionID.ToString();  //获取当前HttpContext.Current.Session的id
                        hOnline.Add(HttpContext.Current.Session["strAllAcount"].ToString(), sId);//添加key/value键值对
                        HttpContext.Current.Application["Online"] = hOnline;
                    }


                }
            }
            ri.flag = "1";
            return ri;
        }
        #endregion

        #region 获取用户名,手机号码和当前时间,拼接为字符串,并加密
        public string Get_Token()
        {
            string ccuscode = HttpContext.Current.Session["cCusCode"].ToString();
            string phone = HttpContext.Current.Session["cCusPhone"].ToString();
            string pwd = HttpContext.Current.Session["pwd"].ToString();
            string time = DateTime.Now.ToString();
            string token = ccuscode + "|" + pwd + "|" + phone + "|" + time;
            HttpContext.Current.Session["token"] = EncryptDES(token);
            return EncryptDES(token);
        }
        #endregion

        #region 两个时间计算时间差，返回JObject
        public JObject DateDiff(string d1, string d2)
        {
            DateTime date1 = DateTime.Parse(d1);
            DateTime date2 = DateTime.Parse(d2);
            TimeSpan ts = date1.Subtract(date2).Duration();
            JObject jo = new JObject();
            jo["min"] = ts.TotalMinutes;
            jo["day"] = ts.TotalDays;
            jo["sec"] = ts.TotalSeconds;
            jo["hou"] = ts.TotalHours;
            return jo;
        }
        #endregion

        #region 根据电话号码获取Dl_opuser里的ID
        public DataTable GetIdByPhone(string phone)
        {
            string sql = "select lngopUserId,strLoginName from dl_opuser where struserTel=@phone";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@phone",phone)
            };
            return sqlhper.ExecuteQuery(sql, paras, CommandType.Text);

        }
        #endregion

        public string dis_Token()
        {
            string token = HttpContext.Current.Request.QueryString["token"];
            //    5 token = HttpContext.Current.Session["token"].ToString();
            token = token.Replace(" ", "+");
            token = new Check().DecryptDES(token);
            return token;
        }

        #region 缺货需求订单时间控制
        public JObject XOrderTimeControl()
        {
            JObject jo = new JObject();
            DataTable dt = pro.XOrderTimeControl();

            if (dt.Rows.Count > 0)
            {
                jo["flag"] = dt.Rows[0]["flag"].ToString();
                jo["message"] = dt.Rows[0]["message"].ToString();
            }
            else
            {
                jo["flag"] = 0;
                jo["message"] = "程序异常";
            }
            return jo;
        }
        #endregion

        #region 用户信用检测
        public JObject XOrderBehaviorCheck(string cCusCode)
        {
            DataTable dt = pro.GetUserBehavior(cCusCode,"2018-07-01",DateTime.Now.ToString());
            JObject jo = new JObject();
            double score = 0.00;
            if (dt.Rows.Count == 0)
            {
                jo["flag"] = 0;
                jo["message"] = "你的账号还未添加初始信用，请联系客服处理";
                
            }
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    score += double.Parse(dr["Point"].ToString());
                    //score = dr["flag"].ToString() == "+" ? score + double.Parse(dr["Point"].ToString()) : score - double.Parse(dr["Point"].ToString());
                }
                double behaviorScore =double.Parse(ConfigurationManager.AppSettings["BehaviorScore"]);
                if (score > behaviorScore)
                {
                    jo["flag"] = 1;
                }
                else {
                    jo["flag"] = 0;
                    jo["message"] = "您的信用分为" + score + ",<br/>小于或等于信用要求的" + behaviorScore + "分,<br/>已暂停缺货需求下单功能。";
                }
                 
            }

            jo["score"] = score;
            return jo;
        }
        #endregion

    }
}
