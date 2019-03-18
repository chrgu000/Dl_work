using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace U8API.Entity
{
    public class APIRet
    {
        //执行结果
        private bool success;
        //错误信息
        private string errMsg;
        //单据ID
        private string vouchId;

        public bool Success
        {
            set { success = value; }
            get { return success; }
        }
        public string ErrMsg
        {
            set { errMsg = value; }
            get
            {
                if (errMsg == null)
                {
                    return "";
                }
                return errMsg;
            }
        }
        public string VouchId
        {
            set { vouchId = value; }
            get
            {
                if (vouchId == null)
                {
                    return "";
                }
                return vouchId;
            }
        }
    }
}
