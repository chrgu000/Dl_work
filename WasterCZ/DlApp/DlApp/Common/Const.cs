using DlApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DlApp.Common
{
    public class Const
    {
        /// <summary>
        /// 数据库配置项
        /// </summary>
        public static string Option_Key1 = "key1"; //是否允许手动输入工号（0：不允许、1：允许）
        public static string Option_Key2 = "key2"; //产成品判断标识：存货档案的存货分类编码以01、02开头
        public static string Option_Key3 = "key3"; //默认不良品处理方式编码（例：00013-成品报废-降级）
        public static string Option_Key4 = "key4"; //默认产成品入库仓库编码（例：2001-成品不合格品库）
        public static string Option_Key5 = "key5"; //重量计量单位（克）
        public static string Option_Key6 = "key6"; //重量计量单位（千克）
        public static string Option_Key7 = "key7"; //默认产成品入库收发类别（例：0103-自制产成品入库）
        public static string Option_Key8 = "key8"; //默认不良品原因原因码前缀（例：blp）
        public static string Option_Key9 = "key9"; //地磅默认端口号
        public static string Option_Key10 = "key10";//地磅默认波特率
        public static string Option_Key11 = "key11";//地磅称重结果获取频率（单位：秒）
        public static string Option_Key12 = "key12";//中班班次代号
        public static string Option_Key13 = "key13";//是否允许手动输入毛重（0：不允许、1：允许）

        /// <summary>
        /// Web.config文件配置项
        /// </summary>
        public static string sUserID = "sUserID";//默认登录账号
        public static string sPassword = "sPassword";//默认登录密码
        public static string sServer = "sServer";//用友服务器地址
        public static string sAccID = "sAccID";//默认账套
        public static string sUserName = "sUserName";//默认登录人员姓名
    }
}