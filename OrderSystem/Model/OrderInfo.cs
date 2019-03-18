/* 
 *  作者：ECHO
 *  创建时间：2015-10-23
 *  类说明：订单相关管理实体类
 */


namespace Model
{
    /// <summary>
    /// 订单相关管理实体类
    /// </summary>
    public class OrderInfo
    {
        private string ddate;
        /// <summary>
        /// 单据日期
        /// </summary>
        public string Ddate
        {
            get { return ddate; }
            set { ddate = value; }
        }

        private string ccuscode;
        /// <summary>
        /// 客户编码
        /// </summary>
        public string Ccuscode
        {
            get { return ccuscode; }
            set { ccuscode = value; }
        }

        private string cdefine1;
        /// <summary>
        /// 自定义项1
        /// </summary>
        public string Cdefine1
        {
            get { return cdefine1; }
            set { cdefine1 = value; }
        }

        private string cdefine2;
        /// <summary>
        /// 自定义项2
        /// </summary>
        public string Cdefine2
        {
            get { return cdefine2; }
            set { cdefine2 = value; }
        }

        private string cdefine11;
        /// <summary>
        /// 自定义项11
        /// </summary>
        public string Cdefine11
        {
            get { return cdefine11; }
            set { cdefine11 = value; }
        }

        private string cdefine8;
        /// <summary>
        /// 自定义项8
        /// </summary>
        public string Cdefine8
        {
            get { return cdefine8; }
            set { cdefine8 = value; }
        }

        private string cdefine9;
        /// <summary>
        /// 自定义项9
        /// </summary>
        public string Cdefine9
        {
            get { return cdefine9; }
            set { cdefine9 = value; }
        }

        private string cdefine10;
        /// <summary>
        /// 自定义项10
        /// </summary>
        public string Cdefine10
        {
            get { return cdefine10; }
            set { cdefine10 = value; }
        }

        private string cdefine3;
        /// <summary>
        /// 自定义项3
        /// </summary>
        public string Cdefine3
        {
            get { return cdefine3; }
            set { cdefine3 = value; }
        }

        private string cdefine12;
        /// <summary>
        /// 自定义项12
        /// </summary>
        public string Cdefine12
        {
            get { return cdefine12; }
            set { cdefine12 = value; }
        }

        private string cdefine13;
        /// <summary>
        /// 自定义项13
        /// </summary>
        public string Cdefine13
        {
            get { return cdefine13; }
            set { cdefine13 = value; }
        }

        private string dpredatebt;
        /// <summary>
        /// 预发货日期
        /// </summary>
        public string Dpredatebt
        {
            get { return dpredatebt; }
            set { dpredatebt = value; }
        }

        private string dpremodatebt;
        /// <summary>
        /// 预完工日期
        /// </summary>
        public string Dpremodatebt
        {
            get { return dpremodatebt; }
            set { dpremodatebt = value; }
        }

        private string ccusname;
        /// <summary>
        /// 客户名称
        /// </summary>
        public string Ccusname
        {
            get { return ccusname; }
            set { ccusname = value; }
        }

        private string cinvoicecompany;
        /// <summary>
        /// 开票单位编码
        /// </summary>
        public string Cinvoicecompany
        {
            get { return cinvoicecompany; }
            set { cinvoicecompany = value; }
        }

        private string cmemo;
        /// <summary>
        /// 备注
        /// </summary>
        public string Cmemo
        {
            get { return cmemo; }
            set { cmemo = value; }
        }

        private string cpersoncode;
        /// <summary>
        /// 用户id
        /// </summary>
        public string Cpersoncode
        {
            get { return cpersoncode; }
            set { cpersoncode = value; }
        }

        private string datCreateTime;
        /// <summary>
        /// 创建日期
        /// </summary>
        public string DatCreateTime
        {
            get { return datCreateTime; }
            set { datCreateTime = value; }
        }

        private string lngopUserId;
        /// <summary>
        /// 业务员编码,用户ID
        /// </summary>
        public string LngopUserId
        {
            get { return lngopUserId; }
            set { lngopUserId = value; }
        }

        private int bytStatus;
        /// <summary>
        /// 单据状态
        /// </summary>
        public int BytStatus
        {
            get { return bytStatus; }
            set { bytStatus = value; }
        }

        private string strRemarks;
        /// <summary>
        /// 单据备注
        /// </summary>
        public string StrRemarks
        {
            get { return strRemarks; }
            set { strRemarks = value; }
        }

        private string cinvcode;
        /// <summary>
        /// 存货编码
        /// </summary>
        public string Cinvcode
        {
            get { return cinvcode; }
            set { cinvcode = value; }
        }

        private double iquantity;
        /// <summary>
        /// 数量
        /// </summary>
        public double Iquantity
        {
            get { return iquantity; }
            set { iquantity = value; }
        }

        private double inum;
        /// <summary>
        /// 辅计量数量
        /// </summary>
        public double Inum
        {
            get { return inum; }
            set { inum = value; }
        }

        private double iquotedprice;
        /// <summary>
        /// 报价
        /// </summary>
        public double Iquotedprice
        {
            get { return iquotedprice; }
            set { iquotedprice = value; }
        }

        private double iunitprice;
        /// <summary>
        /// 原币无税单价
        /// </summary>
        public double Iunitprice
        {
            get { return iunitprice; }
            set { iunitprice = value; }
        }

        private double itaxunitprice;
        /// <summary>
        /// 原币含税单价
        /// </summary>
        public double Itaxunitprice
        {
            get { return itaxunitprice; }
            set { itaxunitprice = value; }
        }

        private double imoney;
        /// <summary>
        /// 原币无税金额
        /// </summary>
        public double Imoney
        {
            get { return imoney; }
            set { imoney = value; }
        }

        private double itax;
        /// <summary>
        /// 原币税额
        /// </summary>
        public double Itax
        {
            get { return itax; }
            set { itax = value; }
        }

        private double isum;
        /// <summary>
        /// 原币价税合计
        /// </summary>
        public double Isum
        {
            get { return isum; }
            set { isum = value; }
        }

        private double inatunitprice;
        /// <summary>
        /// 本币无税单价
        /// </summary>
        public double Inatunitprice
        {
            get { return inatunitprice; }
            set { inatunitprice = value; }
        }

        private double inatmoney;
        /// <summary>
        /// 本币无税金额
        /// </summary>
        public double Inatmoney
        {
            get { return inatmoney; }
            set { inatmoney = value; }
        }

        private double inattax;
        /// <summary>
        /// 本币税额
        /// </summary>
        public double Inattax
        {
            get { return inattax; }
            set { inattax = value; }
        }

        private double inatsum;
        /// <summary>
        /// 本币价税合计
        /// </summary>
        public double Inatsum
        {
            get { return inatsum; }
            set { inatsum = value; }
        }

        private double kl;
        /// <summary>
        /// 扣率
        /// </summary>
        public double Kl
        {
            get { return kl; }
            set { kl = value; }
        }

        private double itaxrate;
        /// <summary>
        /// 税率
        /// </summary>
        public double Itaxrate
        {
            get { return itaxrate; }
            set { itaxrate = value; }
        }

        private string cdefine22;
        /// <summary>
        /// 表体自定义项22
        /// </summary>
        public string Cdefine22
        {
            get { return cdefine22; }
            set { cdefine22 = value; }
        }

        private double iinvexchrate;
        /// <summary>
        /// 换算率
        /// </summary>
        public double Iinvexchrate
        {
            get { return iinvexchrate; }
            set { iinvexchrate = value; }
        }

        private string cunitid;
        /// <summary>
        /// 计量单位编码
        /// </summary>
        public string Cunitid
        {
            get { return cunitid; }
            set { cunitid = value; }
        }

        private int irowno;
        /// <summary>
        /// 行号
        /// </summary>
        public int Irowno
        {
            get { return irowno; }
            set { irowno = value; }
        }

        private int lngopOrderId;
        /// <summary>
        /// 行号
        /// </summary>
        public int LngopOrderId
        {
            get { return lngopOrderId; }
            set { lngopOrderId = value; }
        }

        private string strBillNo;
        /// <summary>
        /// DL网单号
        /// </summary>
        public string StrBillNo
        {
            get { return strBillNo; }
            set { strBillNo = value; }
        }

        private string strU8BillNo;
        /// <summary>
        /// U8单据编号(样品资料)
        /// </summary>
        public string StrU8BillNo
        {
            get { return strU8BillNo; }
            set { strU8BillNo = value; }
        }

        private string cinvname;
        /// <summary>
        /// 存货名称
        /// </summary>
        public string Cinvname
        {
            get { return cinvname; }
            set { cinvname = value; }
        }

        private double idiscount;
        /// <summary>
        /// 原币折扣额 
        /// </summary>
        public double Idiscount
        {
            get { return idiscount; }
            set { idiscount = value; }
        }

        private double inatdiscount;
        /// <summary>
        /// 本币折扣额
        /// </summary>
        public double Inatdiscount
        {
            get { return inatdiscount; }
            set { inatdiscount = value; }
        }

        private string cSCCode;
        /// <summary>
        /// 发运方式编码
        /// </summary>
        public string CSCCode
        {
            get { return cSCCode; }
            set { cSCCode = value; }
        }

        private string cComUnitName;
        /// <summary>
        /// 基本计量单位名称
        /// </summary>
        public string CComUnitName
        {
            get { return cComUnitName; }
            set { cComUnitName = value; }
        }

        private string cInvDefine1;
        /// <summary>
        /// 大包装单位名称 
        /// </summary>
        public string CInvDefine1
        {
            get { return cInvDefine1; }
            set { cInvDefine1 = value; }
        }

        private string cInvDefine2;
        /// <summary>
        /// 小包装单位名称 
        /// </summary>
        public string CInvDefine2
        {
            get { return cInvDefine2; }
            set { cInvDefine2 = value; }
        }

        private string cInvDefine8;
        /// <summary>
        /// 行政区
        /// </summary>
        public string CInvDefine8
        {
            get { return cInvDefine8; }
            set { cInvDefine8 = value; }
        }

        private double cInvDefine13;
        /// <summary>
        /// 大包装换算率
        /// </summary>
        public double CInvDefine13
        {
            get { return cInvDefine13; }
            set { cInvDefine13 = value; }
        }

        private double cInvDefine14;
        /// <summary>
        /// 小包装换算率
        /// </summary>
        public double CInvDefine14
        {
            get { return cInvDefine14; }
            set { cInvDefine14 = value; }
        }

        private string unitGroup;
        /// <summary>
        /// 单位换算率组
        /// </summary>
        public string UnitGroup
        {
            get { return unitGroup; }
            set { unitGroup = value; }
        }

        private double cComUnitQTY;
        /// <summary>
        /// 基本单位数量
        /// </summary>
        public double CComUnitQTY
        {
            get { return cComUnitQTY; }
            set { cComUnitQTY = value; }
        }

        private double cInvDefine1QTY;
        /// <summary>
        /// 大包装单位数量
        /// </summary>
        public double CInvDefine1QTY
        {
            get { return cInvDefine1QTY; }
            set { cInvDefine1QTY = value; }
        }

        private double cInvDefine2QTY;
        /// <summary>
        /// 小包装单位数量
        /// </summary>
        public double CInvDefine2QTY
        {
            get { return cInvDefine2QTY; }
            set { cInvDefine2QTY = value; }
        }

        private string cn1cComUnitName;
        /// <summary>
        /// 销售单位名称
        /// </summary>
        public string Cn1cComUnitName
        {
            get { return cn1cComUnitName; }
            set { cn1cComUnitName = value; }
        }

        private string strManagers;
        /// <summary>
        /// 操作员
        /// </summary>
        public string StrManagers
        {
            get { return strManagers; }
            set { strManagers = value; }
        }

        private string datDeliveryDate;
        /// <summary>
        /// 交货日期
        /// </summary>
        public string DatDeliveryDate
        {
            get { return datDeliveryDate; }
            set { datDeliveryDate = value; }
        }

        private string strLoadingWays;
        /// <summary>
        /// 装车方式
        /// </summary>
        public string StrLoadingWays
        {
            get { return strLoadingWays; }
            set { strLoadingWays = value; }
        }

        private string cSTCode;
        /// <summary>
        /// 销售类型
        /// </summary>
        public string CSTCode
        {
            get { return cSTCode; }
            set { cSTCode = value; }
        }

        private string lngopUseraddressId;
        /// <summary>
        /// 地址id
        /// </summary>
        public string LngopUseraddressId
        {
            get { return lngopUseraddressId; }
            set { lngopUseraddressId = value; }
        }

        private string beginDate;
        /// <summary>
        /// 开始日期
        /// </summary>
        public string BeginDate
        {
            get { return beginDate; }
            set { beginDate = value; }
        }

        private string endDate;
        /// <summary>
        /// 截止日期
        /// </summary>
        public string EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }

        private string cpreordercode;
        /// <summary>
        /// 预订单号(酬宾订单,特殊订单)
        /// </summary>
        public string Cpreordercode
        {
            get { return cpreordercode; }
            set { cpreordercode = value; }
        }

        private int lngBillType;
        /// <summary>
        /// 订单类型(1,普通,2,酬宾,3,特殊)
        /// </summary>
        public int LngBillType
        {
            get { return lngBillType; }
            set { lngBillType = value; }
        }

        private string iaoids;
        /// <summary>
        /// 预订单表体autoid,iaoids
        /// </summary>
        public string Iaoids
        {
            get { return iaoids; }
            set { iaoids = value; }
        }

        private string cdiscountname;
        /// <summary>
        /// 价格等级,酬宾类型(cdiscountname)
        /// </summary>
        public string Cdiscountname
        {
            get { return cdiscountname; }
            set { cdiscountname = value; }
        }

        private string x1;
        /// <summary>
        /// 价格等级,酬宾类型(cdiscountname)
        /// </summary>
        public string X1
        {
            get { return x1; }
            set { x1 = value; }
        }

        private string lngopUserExId;
        /// <summary>
        /// 子账户id（lngopUserExId）
        /// </summary>
        public string LngopUserExId
        {
            get { return lngopUserExId; }
            set { lngopUserExId = value; }
        }

        private string strAllAcount;
        /// <summary>
        /// 子账户编码（AllAcount）
        /// </summary>
        public string StrAllAcount
        {
            get { return strAllAcount; }
            set { strAllAcount = value; }
        }


         private string txtCustomer;
        public string TxtCustomer{
           get { return txtCustomer; }
            set { txtCustomer = value; }
        }

        private string cDefine22;
        /// <summary>
        /// 表体自定义项22
        /// </summary>
        public string CDefine22
        {
            get { return cDefine22; }
            set { cDefine22 = value; }
        }

        private string strBillName;
        public string StrBillName
        {
            get { return strBillName; }
            set { strBillName = value; }
            }

        private string cDefine24;
        public string CDefine24
        {
            get { return cDefine24; }
            set { cDefine24 = value; }
        }
 

        #region Order构造函数

        #region 新增加订单表头
        /// <summary>
        /// 新增加订单表头数据
        /// </summary>
        /// <param name="lngopUserId"></param>
        /// <param name="datCreateTime"></param>
        /// <param name="bytStatus"></param>
        /// <param name="strRemarks"></param>
        /// <param name="ccuscode"></param>
        /// <param name="cdefine1"></param>
        /// <param name="cdefine2"></param>
        /// <param name="cdefine3"></param>
        /// <param name="cdefine9"></param>
        /// <param name="cdefine10"></param>
        /// <param name="cdefine11"></param>
        /// <param name="cdefine12"></param>
        /// <param name="cdefine13"></param>
        /// <param name="ccusname"></param>
        /// <param name="cpersoncode"></param>
        /// <param name="cSCCode"></param>
        /// <param name="datDeliveryDate"></param>
        /// <param name="strLoadingWays"></param>
        /// <param name="cSTCode"></param>
        /// <param name="lngopUseraddressId"></param>
        /// <param name="strU8BillNo"></param>
        public OrderInfo(string lngopUserId, string datCreateTime, int bytStatus, string strRemarks, string ccuscode, string cdefine1, string cdefine2, string cdefine3, string cdefine9, string cdefine10, string cdefine11, string cdefine12, string cdefine13, string ccusname, string cpersoncode, string cSCCode, string datDeliveryDate, string strLoadingWays, string cSTCode, string lngopUseraddressId, string strU8BillNo)
        {
            this.lngopUserId = lngopUserId;
            this.datCreateTime = datCreateTime;
            this.bytStatus = bytStatus;
            this.strRemarks = strRemarks;
            this.ccuscode = ccuscode;
            this.cdefine1 = cdefine1;
            this.cdefine2 = cdefine2;
            this.cdefine3 = cdefine3;
            this.cdefine9 = cdefine9;
            this.cdefine10 = cdefine10;
            this.cdefine11 = cdefine11;
            this.cdefine12 = cdefine12;
            this.cdefine13 = cdefine13;
            this.ccusname = ccusname;
            this.cpersoncode = cpersoncode;
            this.cSCCode = cSCCode;
            this.datDeliveryDate = datDeliveryDate;
            this.strLoadingWays = strLoadingWays;
            this.cSTCode = cSTCode;
            this.lngopUseraddressId = lngopUseraddressId;
            this.strU8BillNo = strU8BillNo;

        }
        #endregion

        #region 新增加订单表头,2016-04-05,添加cdiscountname,(酬宾类型)
        /// <summary>
        /// 新增加订单表头,2016-04-05,添加cdiscountname,(酬宾类型)
        /// </summary>
        /// <param name="lngopUserId"></param>
        /// <param name="datCreateTime"></param>
        /// <param name="bytStatus"></param>
        /// <param name="strRemarks"></param>
        /// <param name="ccuscode"></param>
        /// <param name="cdefine1"></param>
        /// <param name="cdefine2"></param>
        /// <param name="cdefine3"></param>
        /// <param name="cdefine9"></param>
        /// <param name="cdefine10"></param>
        /// <param name="cdefine11"></param>
        /// <param name="cdefine12"></param>
        /// <param name="cdefine13"></param>
        /// <param name="ccusname"></param>
        /// <param name="cpersoncode"></param>
        /// <param name="cSCCode"></param>
        /// <param name="datDeliveryDate"></param>
        /// <param name="strLoadingWays"></param>
        /// <param name="cSTCode"></param>
        /// <param name="lngopUseraddressId"></param>
        /// <param name="strU8BillNo"></param>
        /// <param name="cdiscountname"></param>
        /// <param name="cdefine8"></param>
        /// <param name="lngopUserExId"></param>
        /// <param name="allAcount"></param>
        public OrderInfo(string lngopUserId, string datCreateTime, int bytStatus, string strRemarks, string ccuscode, string cdefine1, string cdefine2, string cdefine3, string cdefine9, string cdefine10, string cdefine11, string cdefine12, string cdefine13, string ccusname, string cpersoncode, string cSCCode, string datDeliveryDate, string strLoadingWays, string cSTCode, string lngopUseraddressId, string strU8BillNo, string cdiscountname, string cdefine8, string lngopUserExId, string strAllAcount)
        {
            this.lngopUserId = lngopUserId;
            this.datCreateTime = datCreateTime;
            this.bytStatus = bytStatus;
            this.strRemarks = strRemarks;
            this.ccuscode = ccuscode;
            this.cdefine1 = cdefine1;
            this.cdefine2 = cdefine2;
            this.cdefine3 = cdefine3;
            this.cdefine9 = cdefine9;
            this.cdefine10 = cdefine10;
            this.cdefine11 = cdefine11;
            this.cdefine12 = cdefine12;
            this.cdefine13 = cdefine13;
            this.ccusname = ccusname;
            this.cpersoncode = cpersoncode;
            this.cSCCode = cSCCode;
            this.datDeliveryDate = datDeliveryDate;
            this.strLoadingWays = strLoadingWays;
            this.cSTCode = cSTCode;
            this.lngopUseraddressId = lngopUseraddressId;
            this.strU8BillNo = strU8BillNo;
            this.cdiscountname = cdiscountname;
            this.cdefine8 = cdefine8;
            this.lngopUserExId = lngopUserExId;
            this.strAllAcount = strAllAcount;
        }
        #endregion

        #region 新增加订单表头(预订单参照生成订单)
        /// <summary>
        /// 新增加订单表头数据
        /// </summary>
        /// <param name="lngopUserId"></param>
        /// <param name="datCreateTime"></param>
        /// <param name="bytStatus"></param>
        /// <param name="strRemarks"></param>
        /// <param name="ccuscode"></param>
        /// <param name="cDefine1"></param>
        /// <param name="cdefine2"></param>
        /// <param name="cdefine11"></param>
        /// <param name="cdefine8"></param>
        /// <param name="cDefine9"></param>
        /// <param name="cdefine10"></param>
        /// <param name="cdefine3"></param>
        /// <param name="cDefine12"></param>
        /// <param name="ccusname"></param>
        /// <param name="cpersoncode"></param>
        /// <param name="cSCCode"></param>
        /// <param name="lngBillnoType"></param>
        /// <param name="cdefine8"></param>
        public OrderInfo(string lngopUserId, string datCreateTime, int bytStatus, string strRemarks, string ccuscode, string cdefine1, string cdefine2, string cdefine3, string cdefine9, string cdefine10, string cdefine11, string cdefine12, string cdefine13, string ccusname, string cpersoncode, string cSCCode, string datDeliveryDate, string strLoadingWays, string cSTCode, string lngopUseraddressId, string strU8BillNo, int lngBillType, string cdefine8, string lngopUserExId, string strAllAcount)
        {
            this.lngopUserId = lngopUserId;
            this.datCreateTime = datCreateTime;
            this.bytStatus = bytStatus;
            this.strRemarks = strRemarks;
            this.ccuscode = ccuscode;
            this.cdefine1 = cdefine1;
            this.cdefine2 = cdefine2;
            this.cdefine3 = cdefine3;
            this.cdefine9 = cdefine9;
            this.cdefine10 = cdefine10;
            this.cdefine11 = cdefine11;
            this.cdefine12 = cdefine12;
            this.cdefine13 = cdefine13;
            this.ccusname = ccusname;
            this.cpersoncode = cpersoncode;
            this.cSCCode = cSCCode;
            this.datDeliveryDate = datDeliveryDate;
            this.strLoadingWays = strLoadingWays;
            this.cSTCode = cSTCode;
            this.lngopUseraddressId = lngopUseraddressId;
            this.strU8BillNo = strU8BillNo;
            this.lngBillType = lngBillType;
            this.cdefine8 = cdefine8;
            this.lngopUserExId = lngopUserExId;
            this.strAllAcount = strAllAcount;
        }
        #endregion

        #region 新增加订单表体
        /// <summary>
        /// 新增加订单表体数据
        /// </summary>
        /// <param name="lngopOrderId"></param>
        /// <param name="cinvcode"></param>
        /// <param name="iquantity"></param>
        /// <param name="inum"></param>
        /// <param name="iquotedprice"></param>
        /// <param name="iunitprice"></param>
        /// <param name="itaxunitprice"></param>
        /// <param name="imoney"></param>
        /// <param name="itax"></param>
        /// <param name="isum"></param>
        /// <param name="inatunitprice"></param>
        /// <param name="inatmoney"></param>
        /// <param name="inattax"></param>
        /// <param name="inatsum"></param>
        /// <param name="kl"></param>
        /// <param name="itaxrate"></param>
        /// <param name="cdefine22"></param>
        /// <param name="iinvexchrate"></param>
        /// <param name="cunitid"></param>
        /// <param name="irowno"></param>
        /// <param name="cinvname"></param>
        /// <param name="idiscount"></param>
        /// <param name="inatdiscount"></param>
        /// <param name="cComUnitName"></param>
        /// <param name="cInvDefine1"></param>
        /// <param name="cInvDefine2"></param>
        /// <param name="cInvDefine13"></param>
        /// <param name="cInvDefine14"></param>
        /// <param name="unitGroup"></param>
        /// <param name="cComUnitQTY"></param>
        /// <param name="cInvDefine1QTY"></param>
        /// <param name="cInvDefine2QTY"></param>
        /// <param name="cn1cComUnitName"></param>
        /// 
        public OrderInfo(int lngopOrderId, string cinvcode, double iquantity, double inum, double iquotedprice, double iunitprice, double itaxunitprice, double imoney, double itax, double isum, double inatunitprice, double inatmoney, double inattax, double inatsum, double kl, double itaxrate, string cdefine22, double iinvexchrate, string cunitid, int irowno, string cinvname, double idiscount, double inatdiscount, string cComUnitName, string cInvDefine1, string cInvDefine2, double cInvDefine13, double cInvDefine14, string unitGroup, double cComUnitQTY, double cInvDefine1QTY, double cInvDefine2QTY, string cn1cComUnitName, string cDefine24)
        {
            this.lngopOrderId = lngopOrderId;
            this.cinvcode = cinvcode;
            this.iquantity = iquantity;
            this.inum = inum;
            this.iquotedprice = iquotedprice;
            this.iunitprice = iunitprice;
            this.itaxunitprice = itaxunitprice;
            this.imoney = imoney;
            this.itax = itax;
            this.isum = isum;
            this.inatunitprice = inatunitprice;
            this.inatmoney = inatmoney;
            this.inattax = inattax;
            this.inatsum = inatsum;
            this.kl = kl;
            this.itaxrate = itaxrate;
            this.cdefine22 = cdefine22;
            this.iinvexchrate = iinvexchrate;
            this.cunitid = cunitid;
            this.irowno = irowno;
            this.cinvname = cinvname;
            this.idiscount = idiscount;
            this.inatdiscount = inatdiscount;
            this.cComUnitName = cComUnitName;
            this.cInvDefine1 = cInvDefine1;
            this.cInvDefine2 = cInvDefine2;
            this.cInvDefine13 = cInvDefine13;
            this.cInvDefine14 = cInvDefine14;
            this.unitGroup = unitGroup;
            this.cComUnitQTY = cComUnitQTY;
            this.cInvDefine1QTY = cInvDefine1QTY;
            this.cInvDefine2QTY = cInvDefine2QTY;
            this.cn1cComUnitName = cn1cComUnitName;
            this.cDefine24 = cDefine24;
        }
        #endregion

        #region 修改订单表头
        /// <summary>
        /// 修改订单表头数据
        /// </summary>
        /// <param name="strBillNo"></param>
        /// <param name="lngopUserId"></param>
        /// <param name="datCreateTime"></param>
        /// <param name="bytStatus"></param>
        /// <param name="strRemarks"></param>
        /// <param name="ccuscode"></param>
        /// <param name="cdefine1"></param>
        /// <param name="cdefine2"></param>
        /// <param name="cdefine3"></param>
        /// <param name="cdefine9"></param>
        /// <param name="cdefine10"></param>
        /// <param name="cdefine11"></param>
        /// <param name="cdefine12"></param>
        /// <param name="cdefine13"></param>
        /// <param name="ccusname"></param>
        /// <param name="cpersoncode"></param>
        /// <param name="cSCCode"></param>
        /// <param name="datDeliveryDate"></param>
        /// <param name="strLoadingWays"></param>
        /// <param name="lngopUseraddressId"></param>
        public OrderInfo(string strBillNo, string lngopUserId, string datCreateTime, int bytStatus, string strRemarks, string ccuscode, string cdefine1, string cdefine2, string cdefine3, string cdefine9, string cdefine10, string cdefine11, string cdefine12, string cdefine13, string ccusname, string cpersoncode, string cSCCode, string datDeliveryDate, string strLoadingWays, string lngopUseraddressId)
        {
            this.strBillNo = strBillNo;
            this.lngopUserId = lngopUserId;
            this.datCreateTime = datCreateTime;
            this.bytStatus = bytStatus;
            this.strRemarks = strRemarks;
            this.ccuscode = ccuscode;
            this.cdefine1 = cdefine1;
            this.cdefine2 = cdefine2;
            this.cdefine3 = cdefine3;
            this.cdefine9 = cdefine9;
            this.cdefine10 = cdefine10;
            this.cdefine11 = cdefine11;
            this.cdefine12 = cdefine12;
            this.cdefine13 = cdefine13;
            this.ccusname = ccusname;
            this.cpersoncode = cpersoncode;
            this.cSCCode = cSCCode;
            this.datDeliveryDate = datDeliveryDate;
            this.strLoadingWays = strLoadingWays;
            this.lngopUseraddressId = lngopUseraddressId;
        }
        #endregion

        #region 修改订单表头 ,2016-04-05,添加cdiscountname,(酬宾类型)
        /// <summary>
        /// 修改订单表头数据
        /// </summary>
        /// <param name="strBillNo"></param>
        /// <param name="lngopUserId"></param>
        /// <param name="datCreateTime"></param>
        /// <param name="bytStatus"></param>
        /// <param name="strRemarks"></param>
        /// <param name="ccuscode"></param>
        /// <param name="cdefine1"></param>
        /// <param name="cdefine2"></param>
        /// <param name="cdefine3"></param>
        /// <param name="cdefine9"></param>
        /// <param name="cdefine10"></param>
        /// <param name="cdefine11"></param>
        /// <param name="cdefine12"></param>
        /// <param name="cdefine13"></param>
        /// <param name="ccusname"></param>
        /// <param name="cpersoncode"></param>
        /// <param name="cSCCode"></param>
        /// <param name="datDeliveryDate"></param>
        /// <param name="strLoadingWays"></param>
        /// <param name="lngopUseraddressId"></param>
        /// <param name="cdiscountname"></param>
        public OrderInfo(string strBillNo, string lngopUserId, string datCreateTime, int bytStatus, string strRemarks, string ccuscode, string cdefine1, string cdefine2, string cdefine3, string cdefine9, string cdefine10, string cdefine11, string cdefine12, string cdefine13, string ccusname, string cpersoncode, string cSCCode, string datDeliveryDate, string strLoadingWays, string lngopUseraddressId, string cdiscountname)
        {
            this.strBillNo = strBillNo;
            this.lngopUserId = lngopUserId;
            this.datCreateTime = datCreateTime;
            this.bytStatus = bytStatus;
            this.strRemarks = strRemarks;
            this.ccuscode = ccuscode;
            this.cdefine1 = cdefine1;
            this.cdefine2 = cdefine2;
            this.cdefine3 = cdefine3;
            this.cdefine9 = cdefine9;
            this.cdefine10 = cdefine10;
            this.cdefine11 = cdefine11;
            this.cdefine12 = cdefine12;
            this.cdefine13 = cdefine13;
            this.ccusname = ccusname;
            this.cpersoncode = cpersoncode;
            this.cSCCode = cSCCode;
            this.datDeliveryDate = datDeliveryDate;
            this.strLoadingWays = strLoadingWays;
            this.lngopUseraddressId = lngopUseraddressId;
            this.cdiscountname = cdiscountname;
        }
        #endregion

        #region 新增加预订单生成普通订单表体
        /// <summary>
        /// 新增加预订单生成普通订单表体
        /// </summary>
        /// <param name="lngopOrderId"></param>
        /// <param name="cinvcode"></param>
        /// <param name="iquantity"></param>
        /// <param name="inum"></param>
        /// <param name="iquotedprice"></param>
        /// <param name="iunitprice"></param>
        /// <param name="itaxunitprice"></param>
        /// <param name="imoney"></param>
        /// <param name="itax"></param>
        /// <param name="isum"></param>
        /// <param name="inatunitprice"></param>
        /// <param name="inatmoney"></param>
        /// <param name="inattax"></param>
        /// <param name="inatsum"></param>
        /// <param name="kl"></param>
        /// <param name="itaxrate"></param>
        /// <param name="cdefine22"></param>
        /// <param name="iinvexchrate"></param>
        /// <param name="cunitid"></param>
        /// <param name="irowno"></param>
        /// <param name="cinvname"></param>
        /// <param name="idiscount"></param>
        /// <param name="inatdiscount"></param>
        /// <param name="cComUnitName"></param>
        /// <param name="cInvDefine1"></param>
        /// <param name="cInvDefine2"></param>
        /// <param name="cInvDefine13"></param>
        /// <param name="cInvDefine14"></param>
        /// <param name="unitGroup"></param>
        /// <param name="cComUnitQTY"></param>
        /// <param name="cInvDefine1QTY"></param>
        /// <param name="cInvDefine2QTY"></param>
        /// <param name="cn1cComUnitName"></param>
        /// <param name="cpreordercode"></param>
        /// <param name="iaoids"></param>
        /// <param name="cDefine24"></param>
        public OrderInfo(int lngopOrderId, string cinvcode, double iquantity, double inum, double iquotedprice, double iunitprice, double itaxunitprice, double imoney, double itax, double isum, double inatunitprice, double inatmoney, double inattax, double inatsum, double kl, double itaxrate, string cdefine22, double iinvexchrate, string cunitid, int irowno, string cinvname, double idiscount, double inatdiscount, string cComUnitName, string cInvDefine1, string cInvDefine2, double cInvDefine13, double cInvDefine14, string unitGroup, double cComUnitQTY, double cInvDefine1QTY, double cInvDefine2QTY, string cn1cComUnitName, string cpreordercode, string iaoids, string cDefine24)
        {
            this.lngopOrderId = lngopOrderId;
            this.cinvcode = cinvcode;
            this.iquantity = iquantity;
            this.inum = inum;
            this.iquotedprice = iquotedprice;
            this.iunitprice = iunitprice;
            this.itaxunitprice = itaxunitprice;
            this.imoney = imoney;
            this.itax = itax;
            this.isum = isum;
            this.inatunitprice = inatunitprice;
            this.inatmoney = inatmoney;
            this.inattax = inattax;
            this.inatsum = inatsum;
            this.kl = kl;
            this.itaxrate = itaxrate;
            this.cdefine22 = cdefine22;
            this.iinvexchrate = iinvexchrate;
            this.cunitid = cunitid;
            this.irowno = irowno;
            this.cinvname = cinvname;
            this.idiscount = idiscount;
            this.inatdiscount = inatdiscount;
            this.cComUnitName = cComUnitName;
            this.cInvDefine1 = cInvDefine1;
            this.cInvDefine2 = cInvDefine2;
            this.cInvDefine13 = cInvDefine13;
            this.cInvDefine14 = cInvDefine14;
            this.unitGroup = unitGroup;
            this.cComUnitQTY = cComUnitQTY;
            this.cInvDefine1QTY = cInvDefine1QTY;
            this.cInvDefine2QTY = cInvDefine2QTY;
            this.cn1cComUnitName = cn1cComUnitName;
            this.cpreordercode = cpreordercode;
            this.iaoids = iaoids;
            this.cDefine24 = cDefine24;
        }
        #endregion

        #region 修改订单表头(样品资料订单表头)
        /// <summary>
        /// 修改订单表头数据
        /// </summary> 
        /// <param name="strBillNo">订单编号</param>
        /// <param name="strRemarks">备注</param>
        /// <param name="strLoadingWays">装车方式</param>
        public OrderInfo(string strBillNo, string strRemarks, string strLoadingWays, int bytStatus)
        {
            this.strBillNo = strBillNo;
            this.strRemarks = strRemarks;
            this.strLoadingWays = strLoadingWays;
            this.bytStatus = bytStatus;
        }
        #endregion


        #region 正式订单表体model
        /// <summary>
        /// 2016-01-12 create by cj
        /// </summary>
        /// <param name="irowno"></param>
        /// <param name="cinvcode"></param>
        /// <param name="cinvname"></param>
        /// <param name="iquantity"></param>
        /// <param name="iquotedprice"></param>
        /// <param name="kl"></param>
        /// <param name="cComUnitName"></param>
        /// <param name="cInvDefine1"></param>
        /// <param name="cInvDefine2"></param>
        /// <param name="cInvDefine13"></param>
        /// <param name="cInvDefine14"></param>
        /// <param name="unitGroup"></param>
        /// <param name="cComUnitQTY"></param>
        /// <param name="cInvDefine2QTY"></param>
        /// <param name="cInvDefine1QTY"></param>
        /// <param name="itaxunitprice"></param>
        /// <param name="cunitid"></param>
        /// <param name="cDefine22"></param>
        public OrderInfo(int irowno, string cinvcode, string cinvname, double iquantity, double iquotedprice, double kl, string cComUnitName,
                           string cInvDefine1, string cInvDefine2, double cInvDefine13, double cInvDefine14, string unitGroup, double cComUnitQTY,
                            double cInvDefine2QTY, double cInvDefine1QTY, double itaxunitprice, string cunitid, string cDefine22)
        {

            this.irowno = irowno;
            this.cinvcode = cinvcode;
            this.cinvname = cinvname;
            this.iquantity = iquantity;
            this.iquotedprice = iquotedprice;
            this.kl = kl;
            this.cComUnitName = cComUnitName;
            this.cInvDefine1 = cInvDefine1;
            this.cInvDefine2 = cInvDefine2;
            this.cInvDefine13 = cInvDefine13;
            this.cInvDefine14 = cInvDefine14;
            this.unitGroup = unitGroup;
            this.cComUnitQTY = cComUnitQTY;
            this.cInvDefine2QTY = cInvDefine2QTY;
            this.cInvDefine1QTY = cInvDefine1QTY;
            this.itaxunitprice = itaxunitprice;
            this.cunitid = cunitid;
            this.cDefine22 = cDefine22;

        }
        #endregion
        #endregion

        #region 新增临时订单表头
        #region 新增加订单表头,2016-04-05,添加cdiscountname,(酬宾类型)
        /// <summary>
        /// 新增加订单表头,2016-04-05,添加cdiscountname,(酬宾类型)
        /// </summary>
        /// <param name="lngopUserId"></param>
        /// <param name="datCreateTime"></param>
        /// <param name="bytStatus"></param>
        /// <param name="strRemarks"></param>
        /// <param name="ccuscode"></param>
        /// <param name="cdefine1"></param>
        /// <param name="cdefine2"></param>
        /// <param name="cdefine3"></param>
        /// <param name="cdefine9"></param>
        /// <param name="cdefine10"></param>
        /// <param name="cdefine11"></param>
        /// <param name="cdefine12"></param>
        /// <param name="cdefine13"></param>
        /// <param name="ccusname"></param>
        /// <param name="cpersoncode"></param>
        /// <param name="cSCCode"></param>
        /// <param name="datDeliveryDate"></param>
        /// <param name="strLoadingWays"></param>
        /// <param name="cSTCode"></param>
        /// <param name="lngopUseraddressId"></param>
        /// <param name="strU8BillNo"></param>
        /// <param name="cdiscountname"></param>
        /// <param name="cdefine8"></param>
        /// <param name="lngopUserExId"></param>
        /// <param name="allAcount"></param>
        public OrderInfo(string strBillName, string lngopUserId, string datCreateTime, int bytStatus, string strRemarks, string ccuscode, string cdefine1, string cdefine2, string cdefine3, string cdefine9, string cdefine10, string cdefine11, string cdefine12, string cdefine13, string ccusname, string cpersoncode, string cSCCode, string datDeliveryDate, string strLoadingWays, string cSTCode, string lngopUseraddressId, string strU8BillNo, string cdiscountname, string cdefine8, string lngopUserExId, string strAllAcount)
        {
            this.strBillName = strBillName;
            this.lngopUserId = lngopUserId;
            this.datCreateTime = datCreateTime;
            this.bytStatus = bytStatus;
            this.strRemarks = strRemarks;
            this.ccuscode = ccuscode;
            this.cdefine1 = cdefine1;
            this.cdefine2 = cdefine2;
            this.cdefine3 = cdefine3;
            this.cdefine9 = cdefine9;
            this.cdefine10 = cdefine10;
            this.cdefine11 = cdefine11;
            this.cdefine12 = cdefine12;
            this.cdefine13 = cdefine13;
            this.ccusname = ccusname;
            this.cpersoncode = cpersoncode;
            this.cSCCode = cSCCode;
            this.datDeliveryDate = datDeliveryDate;
            this.strLoadingWays = strLoadingWays;
            this.cSTCode = cSTCode;
            this.lngopUseraddressId = lngopUseraddressId;
            this.strU8BillNo = strU8BillNo;
            this.cdiscountname = cdiscountname;
            this.cdefine8 = cdefine8;
            this.lngopUserExId = lngopUserExId;
            this.strAllAcount = strAllAcount;
        }
        #endregion
        #endregion

    //    #region 新增特殊订单表头（用于从前台传过来的表头数据实例化）

    //    public OrderInfo(string ccuscode, string strRemarks, string cdefine3, string cSCCode, string datDeliveryDate, string strLoadingWays, string lngopUseraddressId, string cdefine8)
    //    {
    //        this.ccuscode = ccuscode;
    //        this.strRemarks = strRemarks;
    //        this.cSCCode = cSCCode;
    //        this.datDeliveryDate = datDeliveryDate;
    //        this.strLoadingWays = strLoadingWays;
    //        this.lngopUseraddressId = lngopUseraddressId;
    //        this.cdefine8 = cdefine8;  //行政区
    //        this.cdefine3 = cdefine3;  //车型
    //    }
    //    #endregion

    }
}
