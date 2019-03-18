using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DAL;
using Model;
using System.Web;


namespace BLL
{
    #region 简单封装中国短信接口规范v1.2
    /*
类名：ChinaSms
说明：简单封装中国短信接口规范v1.2
更新历史：
*/
    public class SendSMS
    {
            private string comName;
            private string comPwd;

            public SendSMS()
            {
                this.comName = "duolian";
                this.comPwd = "duolian123";
            }

            public SendSMS(String name, String pwd)
            {
                this.comName = name;
                this.comPwd = pwd;
            }

            /*
            说明:封装单发接口
            参数:
              dst:目标手机号码
              msg:发送短信内容
            返回值:
              true:发送成功;
              false:发送失败
            */
            public bool SingleSend(String dst, String msg)
            {
                string sUrl = null;  //接口规范中的地址
                string sMsg = null;  //调用结果
                msg = System.Web.HttpUtility.UrlEncode(msg, System.Text.Encoding.GetEncoding("gb2312"));
                comName = System.Web.HttpUtility.UrlEncode(comName, System.Text.Encoding.GetEncoding("gb2312"));
                //备用IP地址为203.81.21.13
                sUrl = "http:" + "//203.81.21.34//send/gsend.asp?name=" + comName + "&pwd=" + comPwd + "&dst=" + dst + "&txt=ccdx&msg=" + msg;
                sMsg = GetUrl(sUrl);
                //Console.WriteLine(sMsg);

                if (sMsg.Substring(0, 5) != "num=0")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            /*通用调用接口*/
            public String GetUrl(String urlString)
            {
                string sMsg = "";		//引用的返回字符串
                try
                {
                    System.Net.HttpWebResponse rs = (System.Net.HttpWebResponse)System.Net.HttpWebRequest.Create(urlString).GetResponse();
                    System.IO.StreamReader sr = new System.IO.StreamReader(rs.GetResponseStream(), System.Text.Encoding.Default);
                    sMsg = sr.ReadToEnd();
                }
                catch
                {
                    return sMsg;
                }
                return sMsg;
            }
        }
    #endregion

}
