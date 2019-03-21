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

        private string strUserPwd;
        /// <summary>
        /// 密码
        /// </summary>
        public string StrUserPwd
        {
            get { return strUserPwd; }
            set { strUserPwd = value; }
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

        private string strDistrict;
        /// <summary>
        /// 行政区域,省市区
        /// </summary>
        public string StrDistrict
        {
            get { return strDistrict; }
            set { strDistrict = value; }
        }

        private string ps;
        /// <summary>
        /// 行政区域,省市区
        /// </summary>
        public string Ps
        {
            get { return ps; }
            set { ps = value; }
        }


        #region BasicInfo构造函数

        #region 修改密码
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="lngopUserId">用户id</param>
        /// <param name="strUserPwd">用户密码</param>
        public BasicInfo(string lngopUserId, string strUserPwd)
        {
            this.lngopUserId = lngopUserId;
            this.strUserPwd = strUserPwd;
        }
        #endregion

        #region 修改送货方式(配送)
        /// <summary>
        /// 修改送货方式(配送)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="strConsigneeName">收货人姓名</param>
        /// <param name="strConsigneeTel">收货人电话</param>
        /// <param name="strReceivingAddress">收货地址</param>
        /// <param name="strDistrict">行政区域</param>
        public BasicInfo(string id, string strConsigneeName, string strConsigneeTel, string strReceivingAddress, string strDistrict, string ps)
        {
            this.id = id;
            this.strConsigneeName = strConsigneeName;
            this.strConsigneeTel = strConsigneeTel;
            this.strReceivingAddress = strReceivingAddress;
            this.strDistrict = strDistrict;
            this.ps = ps;
        }
        #endregion

        #region 修改送货方式(自提)
        /// <summary>
        /// 修改送货方式(自提)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="strCarplateNumber">车牌号</param>
        /// <param name="strDriverName">司机姓名</param>
        /// <param name="strDriverTel">司机电话</param>
        /// <param name="strIdCard">司机身份证</param>
        public BasicInfo(string id, string strCarplateNumber, string strDriverName, string strDriverTel, string strIdCard)
        {
            this.id = id;
            this.strCarplateNumber = strCarplateNumber;
            this.strDriverName = strDriverName;
            this.strDriverTel = strDriverTel;
            this.strIdCard = strIdCard;
        }
        #endregion

        #endregion
    }

    public class WX_MSG {
        private long errcode;

        public long Errcode
        {
            get { return errcode; }
            set { errcode = value; }
        }

        private string errmsg;

        public string Errmsg
        {
            get { return errmsg; }
            set { errmsg = value; }
        }
    }
}
