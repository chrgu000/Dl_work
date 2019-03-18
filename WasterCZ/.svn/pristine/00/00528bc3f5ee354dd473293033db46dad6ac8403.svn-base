using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DlApp.Common
{
    public static class Utils
    {
        /// <summary>
        /// 四舍五入
        /// </summary>
        /// <param name="value">参数</param>
        /// <param name="digit">保留位数</param>
        /// <returns></returns>
        public static double C1Round(double value, int digit)
        {
            double vt = Math.Pow(10, digit);
            double vx = value * vt;

            vx += 0.5;
            return (Math.Floor(vx) / vt);
        }

        /// <summary>
        /// 封装未Json对象返回
        /// </summary>
        /// <param name="ret"></param>
        /// <returns></returns>
        public static JsonResult JsonUtil(Object ret)
        {
            JsonResult jr = new JsonResult();
            jr.Data = ret;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }
    }
}