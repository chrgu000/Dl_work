/* 
 *  作者：ECHO
 *  创建时间：2015-10-13
 *  类说明：基础资料管理实体类
 */

namespace Model
{
    /// <summary>
    /// 基础资料管理实体类
    /// </summary>
    public class BasicInfo
    {
        private string id;
        /// <summary>
        /// 主键,自增值
        /// </summary>
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        private string fuser;
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Fuser
        {
            get { return fuser; }
            set { fuser = value; }
        }

        private string fdate;
        /// <summary>
        /// 创建日期
        /// </summary>
        public string Fdate
        {
            get { return fdate; }
            set { fdate = value; }
        }

        private string lngopUserId;
        /// <summary>
        /// 用户ID
        /// </summary>
        public string LngopUserId
        {
            get { return lngopUserId; }
            set { lngopUserId = value; }
        }

        private string strDistributionType;
        /// <summary>
        /// 送货方式
        /// </summary>
        public string StrDistributionType
        {
            get { return strDistributionType; }
            set { strDistributionType = value; }
        }

        private string strConsigneeName;
        /// <summary>
        /// 收货人
        /// </summary>
        public string StrConsigneeName
        {
            get { return strConsigneeName; }
            set { strConsigneeName = value; }
        }

        private string strConsigneeTel;
        /// <summary>
        /// 收货人电话
        /// </summary>
        public string StrConsigneeTel
        {
            get { return strConsigneeTel; }
            set { strConsigneeTel = value; }
        }

        private string strReceivingAddress;
        /// <summary>
        /// 收货人地址
        /// </summary>
        public string StrReceivingAddress
        {
            get { return strReceivingAddress; }
            set { strReceivingAddress = value; }
        }

        private string strCarplateNumber;
        /// <summary>
        /// 车牌号
        /// </summary>
        public string StrCarplateNumber
        {
            get { return strCarplateNumber; }
            set { strCarplateNumber = value; }
        }

        private string strDriverName;
        /// <summary>
        /// 司机姓名
        /// </summary>
        public string StrDriverName
        {
            get { return strDriverName; }
            set { strDriverName = value; }
        }

        private string strDriverTel;
        /// <summary>
        /// 司机电话
        /// </summary>
        public string StrDriverTel
        {
            get { return strDriverTel; }
            set { strDriverTel = value; }
        }

        private string strIdCard;
        /// <summary>
        /// 司机身份证
        /// </summary>
        public string StrIdCard
        {
            get { return strIdCard; }
            set { strIdCard = value; }
        }


        #region BasicInfo构造函数
        /// <summary>
        /// 基础资料管理
        /// </summary>
        /// <param name="strReceivingAddress">起运地,运输模式(国内),运输模式(国际),车型,柜型,装运港,币种</param>
        /// <param name="fuser">用户</param>
        public BasicInfo(string id, string strReceivingAddress, string fuser)
        {
            this.id = id;
            this.strReceivingAddress = strReceivingAddress;
            this.fuser = fuser;
        }

        public BasicInfo(string id, string strReceivingAddress, string fuser, string fdate)
        {
            this.id = id;
            this.strReceivingAddress = strReceivingAddress;
            this.fuser = fuser;
            this.fdate = fdate;
        }
        #endregion
    }
}
