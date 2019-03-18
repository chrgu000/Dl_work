using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

//需要添加以下命名空间
using UFIDA.U8.MomServiceCommon;
using UFIDA.U8.U8MOMAPIFramework;
using UFIDA.U8.U8APIFramework;
using UFIDA.U8.U8APIFramework.Meta;
using UFIDA.U8.U8APIFramework.Parameter;
using MSXML2;
using U8API.Entity;

namespace U8APIProject
{
    public class U8Logon
    {
        /// <summary>
        /// 登录U8
        /// </summary>
        /// <param name="enlogon">登录配置</param>
        /// <returns></returns>
        public static Object Logon(ENLogon logon)
        {
            //第一步：构造u8login对象并登陆(引用U8API类库中的Interop.U8Login.dll)
            //如果当前环境中有login对象则可以省去第一步
            U8Login.clsLogin u8Login = new U8Login.clsLogin();
            String sSubId = "DP";
            String sAccID = logon.sAccID;
            String sYear = logon.sYear;
            String sUserID = logon.sUserID;
            String sPassword = logon.sPassword;
            String sDate = logon.sDate;
            String sServer = logon.sServer;
            String sSerial = "";
            if (!u8Login.Login(ref sSubId, ref sAccID, ref sYear, ref sUserID, ref sPassword, ref sDate, ref sServer, ref sSerial))
            {
                String msg = "登陆失败：" + u8Login.ShareString;
                Console.WriteLine(msg);
                Marshal.FinalReleaseComObject(u8Login);
                return null;
            }
            return u8Login;
        }

        /// <summary>
        /// 注销U8
        /// </summary>
        /// <returns></returns>
        public static void Shutdown(Object u8Login)
        {
            try
            {
                U8Login.clsLogin log = u8Login as U8Login.clsLogin;
                log.ShutDown();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

    }
}
