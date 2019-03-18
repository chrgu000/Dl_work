using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BLL
{
    public class product
    {
        DAL.SQLHelper1 sqlhelper = new DAL.SQLHelper1();
        DAL.SQLHelper sqlh = new DAL.SQLHelper();
        DAL.DbDao DbDao = new DAL.DbDao();
        public bool isExist(string name, string pass)
        {
            //string sql = "select * from users where name='"+name+"' and pass='"+pass+"'";
            string sql = "select * from users where name=@name and pass=@pass ";
            CommandType cmdType = CommandType.Text;
            SqlParameter[] pms = new SqlParameter[]{
                  new SqlParameter("@name",name),
                  new SqlParameter("@pass",pass)
           };
            return DAL.SQLHelper1.ExecuteScalar(sql, cmdType, pms);
        }

        public bool insertUser(string name, string pass)
        {
            string sql = "insert into users values(@name,@pass)";
            CommandType cmdType = CommandType.Text;
            SqlParameter[] pms = new SqlParameter[]{
                  new SqlParameter("@name",name),
                  new SqlParameter("@pass",pass)
           };
            if (DAL.SQLHelper1.ExecuteNonQuery(sql, cmdType, pms) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool insertUser_proc(string name, string pass)
        {
            string sql = "proc_insert_user";
            CommandType cmdType = CommandType.StoredProcedure;
            SqlParameter[] pms = new SqlParameter[]{
                  new SqlParameter("@name",name),
                  new SqlParameter("@pass",pass)
           };
            if (DAL.SQLHelper1.ExecuteNonQuery(sql, cmdType, pms) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public DataTable Get_Addr()
        {
            string sql = "select * from inventoryclass";
            CommandType cmdType = CommandType.Text;
            DataTable dt = new DataTable();
            dt = DAL.SQLHelper1.ExecuteApdater(sql, cmdType, null);
            return dt;
        }

        public DataTable Get_Product_class()
        {
            //  string sql = "    select cinvccode,cinvcname,iinvcgrade from inventoryclass  order by iinvcgrade,cinvccode";
            string sql = @"select cinvccode as id,cinvcname as name, case iinvcgrade 
                                when '1' then '0'
                                when '2' then left(cinvccode,2)
                                when '3' then left(cinvccode,4)
                                when '4' then left(cinvccode,6)
                                end as pid,iinvcgrade
                                from inventoryclass";
            CommandType cmdType = CommandType.Text;
            DataTable dt = new DataTable();
            dt = DAL.SQLHelper1.ExecuteApdater(sql, cmdType, null);
            return dt;

        }

        public DataTable Get_Product()
        {
            string sql = "  select cinvcode as id,cinvname as name ,cinvstd as type,cinvdefine4 as unit from inventory ";
            CommandType cmdType = CommandType.Text;
            DataTable dt = new DataTable();
            dt = DAL.SQLHelper1.ExecuteApdater(sql, cmdType, null);
            return dt;

        }



        #region 检测订单时间是否开放
        public DataTable DLproc_OPTimeCheckBySel()
        {
            string sql = "DLproc_OPTimeCheckBySel";
            return sqlh.ExecuteQuery(sql, CommandType.StoredProcedure);
        }
        #endregion

        #region 2017-09-05添加，同时获取 开票单位、信用、车型、送货地址、行政区,返回DataSet
        public DataSet GetAllBaseInfo(string ConstcCusCode, string lngopUserId)
        {
            string cCusCode = ConstcCusCode + "%";
            string sql = "select cCusCode,cCusName,cCusPPerson from Customer where SUBSTRING(cCusCode,0,7)='" + ConstcCusCode + "' and dEndDate is NULL ;";  //获取开票单位
            sql += "exec DLproc_getCusCreditInfo '" + ConstcCusCode + "' ;";  //获取信用
            sql += "SELECT cValue FROM UserDefine WHERE cID='03';";//获取车型
            sql += "select * from Dl_opUserAddress where lngopuserid=" + lngopUserId + ";";
            sql += "select * from dl_opuseraddress_ex where ccuscode='" + ConstcCusCode + "'";
            DataSet ds = sqlh.ExecuteDataSet(sql, CommandType.Text);
            ds.Tables[0].TableName = "Kpdw_dt";
            ds.Tables[1].TableName = "Credit_dt";
            ds.Tables[2].TableName = "CarType_dt";
            ds.Tables[3].TableName = "UserAddress_dt";
            ds.Tables[4].TableName = "Area_dt";
            return ds;

        }
        #endregion

        #region 2017-09-06添加，根据iAddressType获取不同的数据，0为所有，1为自提，2为配送，3为自提需配送
        public DataSet GetAddressByType(string lngopUserID, string AddressType, string ccuscode)
        {
            string sql = string.Empty;
            if (AddressType == "0")
            {
                sql = "select * from dl_opuseraddress where  bytstatus=0 and  lngopUserID=" + lngopUserID + ";";

            }
            else
            {
                sql = "select * from dl_opuseraddress where bytstatus=0 and lngopUserID=" + lngopUserID + "  and iaddresstype=" + AddressType + ";";

            }
            sql += "   SELECT * FROM dbo.Dl_opUserAddress_ex where ccuscode='" + ccuscode + "';";

            DataSet ds = sqlh.ExecuteDataSet(sql, CommandType.Text);
            ds.Tables[0].TableName = "address_dt";
            ds.Tables[1].TableName = "area_dt";
            return ds;
        }
        #endregion

        #region 获取现金用户余额
        public DataTable DL_getCusCreditInfo(string kpdw)
        {
            string sql = "DL_getCusCreditInfo";
            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@cCusCode",kpdw)
           };
            return sqlh.ExecuteQuery(sql, paras, CommandType.StoredProcedure);
        }
        #endregion

        #region 获取顾客抬头（排除禁用客户）[DL_ComboCustomerBySel]
        /// <summary>
        /// 获取顾客抬头,业务员（排除禁用客户）
        /// </summary>
        /// <param name="cCusCode">登录顾客编码</param>
        /// <returns></returns>
        public DataTable DL_ComboCustomerBySel(string cCusCode)
        {
            DataTable dt = new DataTable();
            string cmdText = "select cCusCode,cCusName,cCusPPerson from Customer where cCusCode like @cCusCode and dEndDate is null";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@cCusCode",cCusCode)
           };
            dt = sqlh.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 获取商品树
        /// <summary>
        /// 获取商品树
        /// </summary>
        /// <returns></returns>
        public DataTable DL_InventoryBySel(string cSTCode, string ccuscode)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_InventoryBySel";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@cSTCode",cSTCode),
           new SqlParameter("@cCusCode",ccuscode)
           };

            dt = sqlh.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 根据分类获取商品列表
        /// <summary>
        /// 根据分类获取商品列表
        /// </summary>
        /// <returns></returns>
        public DataTable DL_TreeListDetailsAllBySel(string cInvCCode, string cCuscode, string iShowType)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_TreeListDetailsAllBySel";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@cInvCCode",cInvCCode),
           new SqlParameter("@cCusCode",cCuscode),
           new SqlParameter("@iShowType",iShowType)
           };
            dt = sqlh.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 根据分类获取商品列表_需求订单
        public DataTable Get_Product_List_CPXQ(string cInvCCode, string cCuscode, string iShowType)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_TreeListDetailsAll_CPXQBySel";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@cInvCCode",cInvCCode),
           new SqlParameter("@cCusCode",cCuscode),
           new SqlParameter("@iShowType",iShowType)
           };
            dt = sqlh.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 获取临时订单列表
        /// <summary>
        /// 根据分类获取商品列表
        /// </summary>
        /// <returns></returns>
        public DataTable DL_GetOrderBackBySel(int lngopUserId)
        {
            DataTable dt = new DataTable();
            string cmdText = "select strBillName,CONVERT(varchar(100), datBillTime, 120) as datBillTime,lngopOrderBackId from Dl_opOrderBack where lngopUserId=@lngopUserId order by datBillTime desc ";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@lngopUserId",lngopUserId)
         
           };
            dt = sqlh.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 获取临时订单表头明细
        /// <summary>
        /// 根据分类获取商品列表
        /// </summary>
        /// <returns></returns>
        public DataTable DL_GetOrderBack_Title(int lngopOrderBackId)
        {
            DataTable dt = new DataTable();
            string cmdText = "select * from Dl_opOrderBack where lngopOrderBackId=@lngopOrderBackId";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@lngopOrderBackId",lngopOrderBackId)
         
           };
            dt = sqlh.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 获取临时订单表体明细
        /// <summary>
        /// 根据分类获取商品列表
        /// </summary>
        /// <returns></returns>
        public DataTable DL_GetOrderBack_Body(int lngopOrderBackId)
        {
            DataTable dt = new DataTable();
            string cmdText = "select * from Dl_opOrderBackDetail where lngopOrderBackId=@lngopOrderBackId";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@lngopOrderBackId",lngopOrderBackId)
         
           };
            dt = sqlh.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 获取历史订单
        public DataTable DL_GeneralPreviousOrderBySel(string ccuscode)
        {
            DataTable dt = new DataTable();
            string cmdText = "select lngoporderid,strBillNo,cSOCode,CONVERT(varchar(100), datBillTime, 120) as datBillTime from Dl_opOrder where datediff(day,datBillTime,GETDATE())<90 and ccuscode like  @ccuscode and lngBillType=0 and cSTCode='00'   order by datBillTime desc";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@ccuscode",ccuscode),
           };
            dt = sqlh.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 获取样品订单表头信息
        public DataTable Get_SampleOrder_Title(string strbillno)
        {
            DataTable dt = new DataTable();
            string cmdText = "select* from dl_oporder where strbillno=@strbillno";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@strbillno",strbillno),
           };
            dt = sqlh.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 获取特殊订单明细
        public DataTable DLproc_QuasiYOrderDetail_TSBySel(string cInvCode, string preOrderId, string areaid)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_QuasiYOrderDetail_TSBySel";
            SqlParameter[] paras = new SqlParameter[]{
             new SqlParameter("@cInvCode",cInvCode),
               new SqlParameter("@cpreordercode",preOrderId),
               new SqlParameter("@cArea",areaid)
             };
            dt = sqlh.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 根据地址ID，获取地址信息
        /// <summary>
        /// 根据分类获取商品列表
        /// </summary>
        /// <returns></returns>
        public DataTable Get_AddressById(string AddressId)
        {
            DataTable dt = new DataTable();
            string cmdText = "select * from Dl_opUserAddress where lngopUseraddressId=@AddressId";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@AddressId",AddressId)
         
           };
            dt = sqlh.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 获取客户待确认订单列表
        /// <summary>
        /// 获取客户待确认订单列表
        /// </summary>
        /// <returns></returns>
        public DataTable DL_UnauditedOrderBySel(int bytStatus, string ccuscode)
        {
            DataTable dt = new DataTable();
            string cmdText = "select do.strRejectRemarks,do.strBillNo,do.cSOCode,du.strUserName,do.ccusname,cDefine11,strRemarks,convert(varchar(10),do.datCreateTime,120) 'datCreateTime',do.RelateU8NO,case  when do.cSTCode='00' and   lngBillType=0 then '普通订单' when do.cSTCode='01' then '样品订单' when lngBillType=1 then '酬宾订单'when lngBillType=2 then '特殊订单'  end cSTCode,do.cSTCode cSTCode1   from Dl_opOrder do inner join     Dl_opUser  du on do.lngopUserId=du.lngopUserId where do.bytStatus=@bytStatus and do.ccuscode like  @ccuscode union all select '',aa.cSOCode 'strBillNo',aa.cSOCode,aa.cCusName 'strUserName',cCusName,'','',convert(varchar(10),aa.dDate,120) 'datCreateTime',cc.chdefine2,cc.chdefine13,''   from SO_SOMain aa left join SO_SOMain_extradefine cc on aa.ID=cc.ID where aa.cCusCode like @ccuscode and cc.chdefine13!='网上下单' and isnull(cc.chdefine19,0)!='已确认'";
            SqlParameter[] paras = new SqlParameter[] { 
                   new SqlParameter("@bytStatus",bytStatus),
                   new SqlParameter("@ccuscode",ccuscode)
                  };
            dt = sqlh.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 获取客户待确认订单列表
        /// <summary>
        /// 获取客户待确认订单列表
        /// </summary>
        /// <returns></returns>
        public DataTable DL_OrderU8BillBySel(string strBillNo)
        {
            DataTable dt = new DataTable();
            string cmdText = @" select bb.cCusName, bb.cSOCode, bb.cSCCode,aa.iNum,aa.iQuantity,aa.iSum,aa.cDefine22,aa.cInvName,bb.cDefine1,bb.cDefine2,bb.cDefine10,bb.cDefine11,bb.cDefine12,bb.cDefine13,
                                  CONVERT(nvarchar(20),bb.dDate,23) as 'CreateDate',bb.cMemo,cc.cInvStd
                                  from SO_SODetails aa left join SO_SOMain bb on aa.csocode=bb.cSOCode left join inventory  cc on aa.cInvCode=cc.cInvCode
                                   where aa.csocode=@strBillNo";
            SqlParameter[] paras = new SqlParameter[] { 
                   new SqlParameter("@strBillNo",strBillNo)
                  };
            dt = sqlh.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 获取客户待确认订单列表
        public DataTable DL_OrderU8BillBySel_New(string strAllAcount)
        {
            DataTable dt = new DataTable();
            string cmdText = @" SELECT * FROM dbo.Dl_opOrder aa
                                LEFT JOIN dbo.Dl_opOrderDetail bb ON aa.lngopOrderId=bb.lngopOrderId
                                WHERE aa.strAllAcount=@strAllAcount AND aa.bytStatus=2";
            SqlParameter[] paras = new SqlParameter[] { 
                   new SqlParameter("@strAllAcount",strAllAcount)
                  };
            dt = sqlh.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion

        //#region 用于查询该物料是否允限销 [DLproc_cInvCodeIsBeLimitedBySel]
        ///// <summary>
        ///// 用于查询该物料是否允限销 [DLproc_cInvCodeIsBeLimitedBySel]
        ///// </summary>
        ///// <param name="cinvcode">存货编码</param>
        ///// <param name="ccuscode">客户编码</param>
        ///// <returns></returns>
        ////public bool DLproc_cInvCodeIsBeLimitedBySel(string cinvcode, string ccuscode)
        ////{
        ////    DataTable dt = new DataTable();
        ////    bool flag = false;
        ////    string cmdText = "SELECT CASE WHEN  SUM(bLimited)=1 THEN 'no' ELSE 'ok' END 'res' FROM (SELECT TOP(1) bLimited FROM dbo.SA_CusInvLimited WHERE cCusCode=@ccuscode UNION ALL SELECT 1 FROM dbo.SA_CusInvLimited WHERE cCusCode=@ccuscode AND cInvCode=@cinvcode ) AS kk";
        ////    SqlParameter[] paras = new SqlParameter[] { 
        ////   new SqlParameter("@cinvcode",cinvcode),
        ////   new SqlParameter("@ccuscode",ccuscode)       
        ////   };
        ////    dt = sqlh.ExecuteQuery(cmdText, paras, CommandType.Text);
        ////    if (dt.Rows[0][0].ToString() == "ok")
        ////    {
        ////        flag = true;
        ////    }
        ////    return flag;
        ////}
        //public bool DLproc_cInvCodeIsBeLimitedBySel(string cinvcode, string ccuscode)
        //{
        //    DataTable dt = new DataTable();
        //    bool flag = false;
        //    string cmdText = "SELECT res FROM (SELECT CASE WHEN  ISNULL(SUM(bLimited),1)=1 THEN 'no' ELSE 'ok' END 'res' FROM (SELECT TOP(1) bLimited FROM dbo.SA_CusInvLimited WHERE cCusCode=@ccuscode UNION ALL SELECT 1 FROM dbo.SA_CusInvLimited WHERE cCusCode=@ccuscode AND cInvCode=@cinvcode ) AS kk UNION ALL SELECT CASE WHEN COUNT(*)>0 THEN 'ok' ELSE 'no' END 'res' FROM dbo.Inventory aa LEFT JOIN dbo.Inventory_extradefine bb ON aa.cInvCode=bb.cInvCode WHERE aa.cInvCode=@cinvcode and (bb.cidefine5='全部显示' OR bb.cidefine5='仅普通订单显示' OR  (aa.cSRPolicy='PE' AND bb.cidefine5 IS null) ) ) AS aa GROUP BY res";
        //    SqlParameter[] paras = new SqlParameter[] { 
        //   new SqlParameter("@cinvcode",cinvcode),
        //   new SqlParameter("@ccuscode",ccuscode)       
        //   };
        //    dt = sqlh.ExecuteQuery(cmdText, paras, CommandType.Text);
        //    if (dt.Rows[0][0].ToString() == "ok" && dt.Rows.Count == 1)
        //    {
        //        flag = true;
        //    }
        //    return flag;
        //}

        //#endregion

        #region 用于查询左边菜单传过来的物料详情 [DLproc_QuasiOrderDetailBySel]
        /// <summary>
        /// 用于查询左边菜单传过来的物料详情 [DLproc_QuasiOrderDetailBySel]
        /// </summary>
        /// <param name="cinvcode">存货编码</param>
        /// <returns></returns>
        public DataTable DLproc_QuasiOrderDetailBySel(string cinvcode, string ccuscode, string areaid)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_QuasiOrderDetailBySel";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@cinvcode",cinvcode),
           new SqlParameter("@ccuscode",ccuscode),
           new SqlParameter("@cArea",areaid)
           };
            dt = sqlh.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 用于查询修改的订单的商品的库存可用量详情 [DLproc_QuasiOrderDetailModifyBySel]
        /// <summary>
        /// 用于查询修改的订单的商品的库存可用量详情 [DLproc_QuasiOrderDetailModifyBySel]
        /// </summary>
        /// <param name="cinvcode">存货编码</param>
        /// <param name="cCusCode">存货编码</param>
        /// <param name="strBillNo">存货编码</param>
        /// <returns></returns>
        public DataTable DLproc_QuasiOrderDetailModifyBySel(string cinvcode, string cCusCode, string strBillNo, string areaid)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_QuasiOrderDetailModifyBySel";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@cinvcode",cinvcode),
           new SqlParameter("@cCusCode",cCusCode),
           new SqlParameter("@strBillNo",strBillNo),
           new SqlParameter("@cArea",areaid)
           };
            dt = sqlh.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 生成用于验证的新Datatable
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listData">表单数据</param>
        /// <param name="kpdw">开票单位编码</param>
        /// <param name="areaid">行政区编码</param>
        /// <returns></returns>
        public DataTable Return_Check_Dt(List<Buy_list> listData, string kpdw, string areaid)
        {
            //根据表体数据itemid重新生成Table，计算金额，用于验证信用、是否大于库存、是否大于可用量、是否有未填写数量的商品
            DataTable Check_Dt = new DataTable(); //用于验证的table
            DataTable dt = new DataTable();
            for (int i = 0; i < listData.Count; i++)
            {
                dt = DLproc_QuasiOrderDetailBySel(listData[i].cinvcode, kpdw, areaid);

                if (i == 0) //第一条数据时clone表结构
                {
                    Check_Dt = dt.Clone();
                    Check_Dt.Columns.Add("cComUnitQTY", typeof(string));  //基本数量 
                    Check_Dt.Columns.Add("cInvDefine2QTY", typeof(string));//小包装数量 
                    Check_Dt.Columns.Add("cInvDefine1QTY", typeof(string)); //大包装数量
                    Check_Dt.Columns.Add("cDefine22", typeof(string));      //包装结果
                    Check_Dt.Columns.Add("iquantity", typeof(string));       //汇总数量
                    Check_Dt.Columns.Add("unitGroup", typeof(string));    //包装组
                    Check_Dt.Columns.Add("irowno", typeof(string));           //行号
                    Check_Dt.Columns.Add("cInvDefine13", typeof(string));    //大包装换算率
                    Check_Dt.Columns.Add("cInvDefine14", typeof(string));    //小包装换算率
                    Check_Dt.Columns.Add("cInvDefine1", typeof(string));    //大包装单位
                    Check_Dt.Columns.Add("cInvDefine2", typeof(string));    //小包装单位
                    Check_Dt.Columns.Add("rowType", typeof(int));       //行状态
                    // Check_Dt.Columns.Add("iKL3", typeof(int));       //行政区扣率


                }
                Check_Dt.Rows.Add(dt.Rows[0].ItemArray);
                Check_Dt.Rows[i]["cComUnitQTY"] = listData[i].cComUnitQTY;  //基本数量赋值
                Check_Dt.Rows[i]["cInvDefine2QTY"] = listData[i].cInvDefine2QTY;  //小包装数量赋值
                Check_Dt.Rows[i]["cInvDefine1QTY"] = listData[i].cInvDefine1QTY;  //大包装数量赋值
                Check_Dt.Rows[i]["cDefine22"] = listData[i].cDefine22;   //包装结果赋值
                Check_Dt.Rows[i]["iquantity"] = listData[i].iquantity;   //汇总数量赋值
                Check_Dt.Rows[i]["unitGroup"] = listData[i].unitGroup;    //单位组赋值
                Check_Dt.Rows[i]["irowno"] = listData[i].irowno;             //行号赋值


                Check_Dt.Rows[i]["rowType"] = 0;                //行默认状态赋值
                if (dt.Rows.Count >= 3)
                {
                    Check_Dt.Rows[i]["cInvDefine1"] = dt.Rows[2]["cComUnitName"].ToString();//大包装单位
                    Check_Dt.Rows[i]["cInvDefine2"] = dt.Rows[1]["cComUnitName"].ToString();//小包装单位
                    Check_Dt.Rows[i]["cInvDefine13"] = dt.Rows[2]["iChangRate"].ToString();//大包装换算率
                    Check_Dt.Rows[i]["cInvDefine14"] = dt.Rows[1]["iChangRate"].ToString();//小包装换算率
                }
                else if (dt.Rows.Count == 2)
                {
                    Check_Dt.Rows[i]["cInvDefine1"] = dt.Rows[1]["cComUnitName"].ToString();
                    Check_Dt.Rows[i]["cInvDefine2"] = dt.Rows[1]["cComUnitName"].ToString();
                    Check_Dt.Rows[i]["cInvDefine13"] = dt.Rows[1]["iChangRate"].ToString();
                    Check_Dt.Rows[i]["cInvDefine14"] = dt.Rows[1]["iChangRate"].ToString();
                }
                else
                {
                    Check_Dt.Rows[i]["cInvDefine1"] = dt.Rows[0]["cComUnitName"].ToString();
                    Check_Dt.Rows[i]["cInvDefine2"] = dt.Rows[0]["cComUnitName"].ToString();
                    Check_Dt.Rows[i]["cInvDefine13"] = dt.Rows[0]["iChangRate"].ToString();
                    Check_Dt.Rows[i]["cInvDefine14"] = dt.Rows[0]["iChangRate"].ToString();
                }
            }

            return Check_Dt;
        }

        #endregion

        #region 生成用于验证的新Datatable
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listData">表单数据</param>
        /// <param name="kpdw">开票单位编码</param>
        /// <returns></returns>
        public DataTable Return_Check_Dt(List<Buy_list> listData, string kpdw, string strBillNo, string areaid)
        {
            //根据表体数据itemid重新生成Table，计算金额，用于验证信用、是否大于库存、是否大于可用量、是否有未填写数量的商品
            DataTable Check_Dt = new DataTable(); //用于验证的table
            DataTable dt = new DataTable();
            for (int i = 0; i < listData.Count; i++)
            {
                dt = DLproc_QuasiOrderDetailModifyBySel(listData[i].cinvcode, kpdw, strBillNo, areaid);

                if (i == 0) //第一条数据时clone表结构
                {
                    Check_Dt = dt.Clone();
                    Check_Dt.Columns.Add("cComUnitQTY", typeof(string));  //基本数量 
                    Check_Dt.Columns.Add("cInvDefine2QTY", typeof(string));//小包装数量 
                    Check_Dt.Columns.Add("cInvDefine1QTY", typeof(string)); //大包装数量
                    Check_Dt.Columns.Add("cDefine22", typeof(string));      //包装结果
                    Check_Dt.Columns.Add("iquantity", typeof(string));       //汇总数量
                    Check_Dt.Columns.Add("unitGroup", typeof(string));    //包装组
                    Check_Dt.Columns.Add("irowno", typeof(string));           //行号
                    Check_Dt.Columns.Add("cInvDefine13", typeof(string));    //大包装换算率
                    Check_Dt.Columns.Add("cInvDefine14", typeof(string));    //小包装换算率
                    Check_Dt.Columns.Add("cInvDefine1", typeof(string));    //大包装单位
                    Check_Dt.Columns.Add("cInvDefine2", typeof(string));    //小包装单位
                    Check_Dt.Columns.Add("rowType", typeof(int));       //行状态

                }
                Check_Dt.Rows.Add(dt.Rows[0].ItemArray);
                Check_Dt.Rows[i]["cComUnitQTY"] = listData[i].cComUnitQTY;  //基本数量赋值
                Check_Dt.Rows[i]["cInvDefine2QTY"] = listData[i].cInvDefine2QTY;  //小包装数量赋值
                Check_Dt.Rows[i]["cInvDefine1QTY"] = listData[i].cInvDefine1QTY;  //大包装数量赋值
                Check_Dt.Rows[i]["cDefine22"] = listData[i].cDefine22;   //包装结果赋值
                Check_Dt.Rows[i]["iquantity"] = listData[i].iquantity;   //汇总数量赋值
                Check_Dt.Rows[i]["unitGroup"] = listData[i].unitGroup;    //单位组赋值
                Check_Dt.Rows[i]["irowno"] = listData[i].irowno;             //行号赋值


                Check_Dt.Rows[i]["rowType"] = 0;                //行默认状态赋值
                if (dt.Rows.Count >= 3)
                {
                    Check_Dt.Rows[i]["cInvDefine1"] = dt.Rows[2]["cComUnitName"].ToString();//大包装单位
                    Check_Dt.Rows[i]["cInvDefine2"] = dt.Rows[1]["cComUnitName"].ToString();//小包装单位
                    Check_Dt.Rows[i]["cInvDefine13"] = dt.Rows[2]["iChangRate"].ToString();//大包装换算率
                    Check_Dt.Rows[i]["cInvDefine14"] = dt.Rows[1]["iChangRate"].ToString();//小包装换算率
                }
                else if (dt.Rows.Count == 2)
                {
                    Check_Dt.Rows[i]["cInvDefine1"] = dt.Rows[1]["cComUnitName"].ToString();
                    Check_Dt.Rows[i]["cInvDefine2"] = dt.Rows[1]["cComUnitName"].ToString();
                    Check_Dt.Rows[i]["cInvDefine13"] = dt.Rows[1]["iChangRate"].ToString();
                    Check_Dt.Rows[i]["cInvDefine14"] = dt.Rows[1]["iChangRate"].ToString();
                }
                else
                {
                    Check_Dt.Rows[i]["cInvDefine1"] = dt.Rows[0]["cComUnitName"].ToString();
                    Check_Dt.Rows[i]["cInvDefine2"] = dt.Rows[0]["cComUnitName"].ToString();
                    Check_Dt.Rows[i]["cInvDefine13"] = dt.Rows[0]["iChangRate"].ToString();
                    Check_Dt.Rows[i]["cInvDefine14"] = dt.Rows[0]["iChangRate"].ToString();
                }
            }

            return Check_Dt;
        }

        #endregion



        #region 用于查询该物料是否允限销 [DLproc_cInvCodeIsBeLimitedBySel]
        /// <summary>
        /// 用于查询该物料是否允限销 [DLproc_cInvCodeIsBeLimitedBySel]
        /// </summary>
        /// <param name="cinvcode">存货编码</param>
        /// <param name="ccuscode">客户编码</param>
        /// <param name="iShowType">单据类型，1普通，2特殊</param>
        /// <returns></returns>
        public bool DLproc_cInvCodeIsBeLimitedBySel(string cinvcode, string ccuscode, int iShowType)
        {
            DataTable dt = new DataTable();
            bool flag = false;
            string cmdText = "DLproc_cInvCodeIsBeLimitedBySel";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@cinvcode",cinvcode),
           new SqlParameter("@ccuscode",ccuscode),
           new SqlParameter("@iShowType",iShowType)
           };
            dt = sqlh.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            if (dt.Rows[0][0].ToString() == "ok" && dt.Rows.Count == 1)
            {
                flag = true;
            }
            return flag;
        }
        #endregion

        #region 修改密码前验证原密码
        public bool Check_pwd(string lngopUserId, string strUserPwd)
        {
            bool flag = false;
            string sql = "select 1 from dl_opuser where lngopUserId=@lngopUserId and strUserPwd=@strUserPwd";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@lngopUserId",lngopUserId),
            new SqlParameter("@strUserPwd",strUserPwd)
            };
            DataTable dt = sqlh.ExecuteQuery(sql, paras, CommandType.Text);
            if (dt.Rows.Count > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion

        #region 修改密码前验证原密码
        public bool Check_pwd_sub(string lngopUserExId, string strSubPwd)
        {
            bool flag = false;
            string sql = "select 1 from dl_opuser_ex where lngopUserExId=@lngopUserExId and strSubPwd=@strSubPwd";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@lngopUserExId",lngopUserExId),
            new SqlParameter("@strSubPwd",strSubPwd)
            };
            DataTable dt = sqlh.ExecuteQuery(sql, paras, CommandType.Text);
            if (dt.Rows.Count > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion

        #region 用户修改密码[Update_UserPWD]
        public bool Update_UserPWD(string lngopUserId, string strUserPwd)
        {
            bool flag = false;
            string sql = "update Dl_opUser set strUserPwd=@strUserPwd where lngopUserId=@lngopUserId";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@lngopUserId",lngopUserId),
            new SqlParameter("@strUserPwd",strUserPwd)
            };
            int res = sqlh.ExecuteNonQuery(sql, paras, CommandType.Text);
            if (res > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion

        #region 用户修改密码(子账户专用修改密码)[Update_SubUserPWD]
        public bool Update_SubUserPWD(string lngopUserExId, string strSubPwd)
        {
            bool flag = false;
            string sql = "update Dl_opUser_Ex set strSubPwd=@strSubPwd where lngopUserExId=@lngopUserExId";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@lngopUserExId",lngopUserExId),
            new SqlParameter("@strSubPwd",strSubPwd)
            };
            int res = sqlh.ExecuteNonQuery(sql, paras, CommandType.Text);
            if (res > 0)
            {
                flag = true;
            }
            return flag;
        }
        #endregion

        #region 获取用户所有的收货地址
        public DataSet Get_User_Address(string lngopUserId)
        {
            string sql = "select * from Dl_opUserAddress where lngopUserId=" + lngopUserId + " and bytStatus=0 order by lngopUseraddressId desc;select * from HR_CT007";
            //SqlParameter[] paras = new SqlParameter[]{
            //new SqlParameter("@lngopUserId",lngopUserId)
            //};
            DataSet ds = sqlh.ExecuteDataSet(sql, CommandType.Text);
            ds.Tables[0].TableName = "opUserAddress";
            ds.Tables[1].TableName = "CT007";
            return ds;
        }
        #endregion

        #region 获取用户自提行政区
        public DataTable Get_User_Area(string ccuscode)
        {
            string sql = "select * from dl_opuseraddress_ex where ccuscode=@ccuscode order by lngopUseraddress_exId desc";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@ccuscode",ccuscode)
            };
            return sqlh.ExecuteQuery(sql, paras, CommandType.Text);
        }
        #endregion

        #region 跳转登录,不验证密码
        public DataTable Direct_Login(string ccuscode, string phone, string pwd)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_SubCustomerPhoneNoLogin";
            SqlParameter[] paras = new SqlParameter[] { 
            new SqlParameter("@cCusCode",ccuscode),
            new SqlParameter("@phone",phone),
            new SqlParameter("@pwd",pwd)
             };
            dt = sqlh.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 获取修改参照特殊订单时的订单详情
        public DataTable DLproc_QuasiModifyOrderDetail_CZTSBySel(string strBillNo)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_QuasiModifyOrderDetail_CZTSBySel";
            SqlParameter[] paras = new SqlParameter[]{
             new SqlParameter("@strBillNo",strBillNo)
             };
            dt = sqlh.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }

        #endregion

        #region 传入DataTable,输出需要传出的列数据
        public DataTable Get_Filter_Column(DataTable dt, params string[] column)
        {
            DataView dv = dt.DefaultView;
            return dv.ToTable("newDT", true, column);
        }
        #endregion

        #region 修改参照特殊订单时 ,传入特殊订单号,返回该订单里商品的可用量
        public DataTable DLproc_TreeListPreDetailsModify_CZTSBySel(string strBillNo)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_TreeListPreDetailsModify_CZTSBySel";
            string lngBillType = "1";
            SqlParameter[] paras = new SqlParameter[]{
             new SqlParameter("@strBillNo",strBillNo),
             new SqlParameter("@lngBillType",lngBillType)
             };
            dt = sqlh.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 修改参照特殊订单时 ,选择商品后，确定，查询该商品信息
        public DataTable DLproc_QuasiYOrderDetailModify_TSBySel(string cinvcode, string cpreordercode, string strBillNoCZTS, string areaid)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_QuasiYOrderDetailModify_TSBySel";

            SqlParameter[] paras = new SqlParameter[]{
               new SqlParameter("@cinvcode",cinvcode),
             new SqlParameter("@cpreordercode",cpreordercode),
             new SqlParameter("@strBillNoCZTS",strBillNoCZTS),
             new SqlParameter("@cArea",areaid)

             };
            dt = sqlh.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 新增参照特殊订单时 ,选择商品后，确定，查询该商品信息
        public DataTable DLproc_TreeListPreDetails_TSBySel(string strBillNo)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_TreeListPreDetails_TSBySel";
            string lngBillType = "1";
            SqlParameter[] paras = new SqlParameter[]{
             new SqlParameter("@strBillNo",strBillNo),
             new SqlParameter("@lngBillType",lngBillType)
             };
            dt = sqlh.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        public DataTable test(string cCusCode)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_getCusCreditInfo";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@cCusCode","010104")
           };


            //    dt = new DAL.SQLHelper().ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            dt = DAL.SQLHelper1.ExecuteApdater(cmdText, CommandType.StoredProcedure, paras);
            return dt;
        }

        #region 根据账户登录Session获取主账户及所有的子账户
        public DataTable Get_Acount(string cCusCode)
        {
            DataTable dt = new DataTable();
            string cmdText = "select cCusCode from v_cCusPhoneAll where cCusCode like @cCusCode";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@cCusCode",cCusCode)
           };
            dt = DAL.SQLHelper1.ExecuteApdater(cmdText, CommandType.StoredProcedure, paras);
            return dt;
        }
        #endregion

        #region 获取延期通知单列表
        public DataTable Get_ArrearList(string start_date, string end_date, string bytstatus, string cCusCode)
        {
            string sql = "";

            if (bytstatus == "0")
            {
                sql = "select * from dl_oparrear where  cCusCode like @cCusCode and   dDate>=@start_date and dDate<=@end_date and bytstatus in ('21','22','32','43') order by id desc";
            }
            else
            {
                sql = "select * from dl_oparrear where cCusCode like @cCusCode and  bytstatus in ('31','41','42','51') and dDate>=@start_date and dDate<=@end_date order by id desc";
            }
            if (string.IsNullOrEmpty(start_date))
            {
                start_date = "2017-01-01";
            }
            if (string.IsNullOrEmpty(end_date))
            {
                end_date = "2055-01-01";
            }
            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@start_date",start_date),
                new SqlParameter("@end_date",end_date+" 23:59:59"),
                new SqlParameter("@bytstatus",bytstatus),
                new SqlParameter("@cCusCode",cCusCode)
            };
            return sqlh.ExecuteQuery(sql, paras, CommandType.Text);
        }
        #endregion

        #region 客户确认延期通知单
        public ReInfo CusConfirmArrear(string code, string strAllAcount, string dExtensionDateStart, string dExtensionDateEnd)
        {
            DataTable dt = new AdminManager().Get_OneArrear(code);
            ReInfo ri = new ReInfo();
            if (dt.Rows[0]["ccuscode"].ToString() != strAllAcount)
            {
                ri.flag = "0";
                ri.message = "子账号不能确认通知单！";
                return ri;
            }
            int bytstatus = 31;
            if (dt.Rows[0]["bytstatus"].ToString() == "22")
            {
                bytstatus = 32;
            }
            dt = new DataTable();
            string time = DateTime.Now.ToString();
            string cmdText = "update dl_oparrear set bytstatus=@bytstatus, cCusConfirm=@strAllAcount,dExtensionDateStart=@dExtensionDateStart,dExtensionDateEnd=@dExtensionDateEnd,dCusConfirmDate=@time where ccode=@code ";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@strAllAcount",strAllAcount),
           new SqlParameter("@bytstatus",bytstatus),
           new SqlParameter("@time",time),
           new SqlParameter("@code",code),
           new SqlParameter("@dExtensionDateStart",dExtensionDateStart),
           new SqlParameter("@dExtensionDateEnd",dExtensionDateEnd)
           };
            int a = sqlh.ExecuteNonQuery(cmdText, paras, CommandType.Text);
            ri.flag = "1";

            ri.message = bytstatus.ToString();
            return ri;
        }
        #endregion

        #region 取回待审核订单
        public ReInfo RecaptionOrder(string lngoporderid, string strbillno, string strAllAcount)
        {
            DataTable dt = GetOrderById(lngoporderid);
            ReInfo ri = new ReInfo();
            if (dt.Rows.Count == 0)
            {
                ri.message = "未能查询到该订单信息！";
                ri.flag = "0";
                return ri;
            }
            if (!string.IsNullOrEmpty(dt.Rows[0]["strManagers"].ToString().Trim()))
            {
                ri.message = "该订单已被客服接收处理，如要修改订单，请联系客服驳回！";
                ri.flag = "0";
                return ri;
            }
            if (dt.Rows[0]["bytStatus"].ToString() != "1")
            {
                ri.message = "该订单状态已改变，无法取回！";
                ri.flag = "0";
                return ri;
            }
            if (dt.Rows[0]["recaptionTimes"].ToString() == "3")
            {
                ri.message = "订单最多只能取回3次！";
                ri.flag = "0";
                return ri;
            }
            if (dt.Rows[0]["strAllAcount"].ToString() != strAllAcount)
            {
                ri.message = "你没有权限取回该订单！";
                ri.flag = "0";
                return ri;
            }

            //写入dl_oporder_ex表及修改dl_oporder表里订单状态
            List<SqlCommand> cmds = new List<SqlCommand>();
            string sql = string.Empty;
            SqlCommand cmd = new SqlCommand();
            if (dt.Rows[0]["recaptionTimes"].ToString().Trim() == "")
            {
                sql = "insert into dl_oporder_ex(lngoporderid,strbillno,strAllAcount,recaptionTimes,recaptionDate) values(@lngoporderid,@strbillno,@strAllAcount,1,GETDATE())";
                SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@lngoporderid",lngoporderid),
                new SqlParameter("@strbillno",strbillno),
                new SqlParameter("@strAllAcount",strAllAcount)
                };
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                cmd.Parameters.AddRange(paras);
            }
            else
            {
                int recaptionTimes = int.Parse(dt.Rows[0]["recaptionTimes"].ToString()) + 1;
                string recaptionDate = DateTime.Now.ToString();
                sql = "update dl_oporder_ex set recaptionTimes=@recaptionTimes,recaptionDate=GETDATE() where lngoporderid=@lngoporderid";
                SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@recaptionTimes",recaptionTimes),
                new SqlParameter("@lngoporderid",lngoporderid)
                };
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                cmd.Parameters.AddRange(paras);
            }
            cmds.Add(cmd);


            cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "update dl_oporder set bytStatus=3 where lngoporderid=@lngoporderid";
            cmd.Parameters.Add(new SqlParameter("@lngoporderid", lngoporderid));

            cmds.Add(cmd);
            bool b = sqlh.SqlTrans(cmds);
            if (b)
            {
                ri.flag = "1";
                ri.message = "取回成功！";

            }
            else
            {
                ri.flag = "0";
                ri.message = "取回失败！";
            }
            return ri;
        }
        #endregion

        #region 取回订单前根据订单ID查询订单表头
        public DataTable GetOrderById(string lngopOrderId)
        {
            string sql = "SELECT * FROM dl_oporder a left JOIN dl_oporder_ex  b ON a.lngopOrderId=b.lngopOrderId WHERE a.lngopOrderId=@lngopOrderId";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@lngopOrderId",lngopOrderId)
            };
            return sqlh.ExecuteQuery(sql, paras, CommandType.Text);
        }
        #endregion

        #region 根据订单号查询订单表头
        public DataTable GetOrderByBillNo(string strbillno)
        {
            string sql = @"SELECT * FROM dl_oporder aa
            left JOIN dl_oporder_ex  bb ON aa.lngopOrderId=bb.lngopOrderId
            inner join dl_oporderdetail cc on aa.lngopOrderId=cc.lngopOrderId
            WHERE aa.strbillno=@strbillno";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@strbillno",strbillno)
            };
            return sqlh.ExecuteQuery(sql, paras, CommandType.Text);
        }
        #endregion

        #region 获取可更改车牌号的订单列表
        public DataTable Get_ModifyShippingMethod_list(string lngopUserId, string lngopUserExId)
        {
            DataTable dt = new DataTable();
            //string cmdText = "select  do.lngopOrderId,do.strManagers, do.strRejectRemarks,do.strBillNo,do.cSOCode,du.strUserName,do.ccusname,cDefine11,strRemarks,convert(varchar(10),do.datCreateTime,120) 'datCreateTime',do.RelateU8NO,case  when do.cSTCode='00' and lngBillType=0 then '普通订单' when do.cSTCode='01' then '样品订单' when lngBillType=1 then '酬宾订单' when lngBillType=2 then '特殊订单'  end cSTCode,do.cSTCode cSTCode1   from Dl_opOrder do inner join Dl_opUser  du on do.lngopUserId=du.lngopUserId where do.bytStatus=@bytStatus and do.lngopUserId=@lngopUserId and isnull(do.lngopUserExId,'0')=@lngopUserExId ";
            string cmdText = @"SELECT do.lngopOrderId,do.lngopUseraddressId,do.cdefine3, do.cSCCode,do.strManagers,ex.recaptionTimes, do.strRejectRemarks,do.strBillNo,do.cSOCode,du.strUserName,do.ccusname,cDefine11,strRemarks,convert(varchar(10),do.datCreateTime,120) 'datCreateTime',do.RelateU8NO,
                                CASE  when do.cSTCode='00' and lngBillType=0 then '普通订单' when do.cSTCode='01' then '样品订单' when lngBillType=1 then '酬宾订单' when lngBillType=2 then '特殊订单'  end cSTCode,do.cSTCode cSTCode1  
                                from Dl_opOrder do 
                                INNER join Dl_opUser  du on do.lngopUserId=du.lngopUserId 
                                LEFT JOIN dl_oporder_ex ex ON do.lngopOrderId=ex.lngopOrderId
                                WHERE do.bytStatus=1 and do.strManagers=''and csccode='00' and do.lngopUserId=@lngopUserId and isnull(do.lngopUserExId,'0')=@lngopUserExId
                                order by do.lngopOrderId desc";
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@lngopUserId",lngopUserId),
           new SqlParameter("@lngopUserExId",lngopUserExId)
           };
            dt = sqlh.ExecuteQuery(cmdText, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 更改订单送货信息
        public ReInfo ModifyShippingMethod(string lngopOrderId, string cCSCode, string addressId, string strRemark, string carType)
        {
            ReInfo ri = new ReInfo();
            DataTable dt = GetOrderById(lngopOrderId);
            if (!string.IsNullOrEmpty(dt.Rows[0]["strManagers"].ToString().Trim()))
            {
                ri.message = "该订单已被客服接收处理，如要修改订单，请联系客服驳回！";
                ri.flag = "0";
                return ri;
            }
            if (dt.Rows[0]["bytStatus"].ToString() != "1")
            {
                ri.message = "该订单状态已改变，无法取回！";
                ri.flag = "0";
                return ri;
            }

            //根据地址ID取出地址详细信息
            DataTable Addr_dt = Get_AddressById(addressId);
            if (cCSCode == "00")
            {
                string sql = @"update dl_oporder set cdefine1=@cdefine1,cdefine2=@cdefine2,cdefine10=@cdefine10,cdefine13=@cdefine13,
                                cdefine11=@cdefine11,cdefine9='',cdefine12='' ,strRemarks=@strRemark,cdefine3=@carType,lngopUseraddressId=@addressId
                                where lngopOrderId=@lngopOrderId";
                SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@lngopOrderId",lngopOrderId),
                new SqlParameter("@carType",carType),
                new SqlParameter("@strRemark",strRemark),
                new SqlParameter("@addressId",addressId),
                new SqlParameter("@cdefine1",Addr_dt.Rows[0]["strDriverName"].ToString()),
                new SqlParameter("@cdefine2",Addr_dt.Rows[0]["strIdCard"].ToString()),
                new SqlParameter("@cdefine10",Addr_dt.Rows[0]["strCarplateNumber"].ToString()),
                new SqlParameter("@cdefine13",Addr_dt.Rows[0]["strDriverTel"].ToString()),
                new SqlParameter("@cdefine11","自提,车牌号:"+Addr_dt.Rows[0]["strCarplateNumber"].ToString()+",司机姓名:"+Addr_dt.Rows[0]["strDriverName"].ToString()+",司机电话:"+Addr_dt.Rows[0]["strDriverTel"].ToString()+",司机身份证:"+Addr_dt.Rows[0]["strIdCard"].ToString()),
           };
                int t = sqlh.ExecuteNonQuery(sql, paras, CommandType.Text);
                if (t > 0)
                {
                    ri.flag = "1";
                    ri.message = "更改成功！";

                }
                else
                {
                    ri.flag = "0";
                    ri.message = "更改失败！";
                }
                return ri;
            }
            else
            {
                string sql = @"update dl_oporder set cdefine1='',cdefine2='',cdefine10='',cdefine13='',lngopUseraddressId=@addressId,
                               cdefine11=@cdefine11,cdefine9=@cdefine9,cdefine12=cdefine12 ,strRemarks=@strRemark,cdefine3=@carType  where lngopOrderId=@lngopOrderId";
                SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@lngopOrderId",lngopOrderId),
                new SqlParameter("@strRemark",strRemark),
                new SqlParameter("@carType",carType),
                new SqlParameter("@addressId",addressId),
                new SqlParameter("@cdefine9",Addr_dt.Rows[0]["strConsigneeName"].ToString()),
                new SqlParameter("@cdefine12",Addr_dt.Rows[0]["strConsigneeTel"].ToString()),
                new SqlParameter("@cdefine11","配送,"+Addr_dt.Rows[0]["strConsigneeName"].ToString()+","+Addr_dt.Rows[0]["strConsigneeTel"].ToString()+","+Addr_dt.Rows[0]["strDistrict"].ToString()+Addr_dt.Rows[0]["strReceivingAddress"].ToString()),
           };
                int t = sqlh.ExecuteNonQuery(sql, paras, CommandType.Text);
                if (t > 0)
                {
                    ri.flag = "1";
                    ri.message = "更改成功！";

                }
                else
                {
                    ri.flag = "0";
                    ri.message = "更改失败！";
                }
                return ri;
            }

        }
        #endregion

        #region 新增行政区前，先判断是否已存在行政区
        public bool IsExists_xzq(string ccuscode, string ccodeid)
        {
            bool b = false;
            string sql = "select * from dl_opuseraddress_ex where ccuscode=@ccuscode and ccodeid=@ccodeid";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@ccuscode",ccuscode),
            new SqlParameter("@ccodeid",ccodeid)
            };
            DataTable dt = sqlh.ExecuteQuery(sql, paras, CommandType.Text);
            if (dt.Rows.Count > 0)
            {
                b = true;
            }
            return b;
        }
        #endregion

        #region 新增行政区
        public bool Insert_UserArea(string ccuscode, string area, string ccodeID)
        {
            bool b = false;
            string sql = "insert into Dl_opUserAddress_ex (ccuscode,xzq,ccuscode_xzq,ccodeID) values(@ccuscode,@area,@ccuscode+@area,@ccodeID)";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@ccuscode",ccuscode),
            new SqlParameter("@area",area),
             new SqlParameter("@ccodeID",ccodeID)
            };
            int a = sqlh.ExecuteNonQuery(sql, paras, CommandType.Text);
            if (a > 0)
            {
                b = true;
            }
            return b;
        }
        #endregion

        #region 提交问卷
        public bool SubmitQuestion(string name, string phone, string ccuscode, JObject jo)
        {
            bool b = false;
            string sql = @"insert into dl_opquestion(name,phone,question1,question2,question3,question4,question5,question6,question7,question8,question9,question10,question11,question12,ccuscode) 
            values(@name,@phone, @question1,@question2,@question3,@question4,@question5,@question6,@question7,@question8,@question9,@question10,@question11,@question12,@ccuscode)";

            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@ccuscode",ccuscode),
                new SqlParameter("@name",name),
                new SqlParameter("@phone",phone),
                new SqlParameter("@question1",jo["question1"]==null?"":jo["question1"].ToString()),
                new SqlParameter("@question2",jo["question2"]==null?"":jo["question2"].ToString()),
                new SqlParameter("@question3",jo["question3"]==null?"":jo["question3"].ToString()),
                new SqlParameter("@question4",jo["question4"]==null?"":jo["question4"].ToString()),
                new SqlParameter("@question5",jo["question5"]==null?"":jo["question5"].ToString()),
                new SqlParameter("@question6",jo["question6"]==null?"":jo["question6"].ToString()),
                new SqlParameter("@question7",jo["question7"]==null?"":jo["question7"].ToString()),
                new SqlParameter("@question8",jo["question8"]==null?"":jo["question8"].ToString()),
                new SqlParameter("@question9",jo["question9"]==null?"":jo["question9"].ToString()),
                new SqlParameter("@question10",jo["question10"]==null?"":jo["question10"].ToString()),
                new SqlParameter("@question11",jo["question11"]==null?"":jo["question11"].ToString()),
                new SqlParameter("@question12",jo["question12"]==null?"":jo["question12"].ToString())
            };
            int a = sqlh.ExecuteNonQuery(sql, paras, CommandType.Text);
            if (a > 0)
            {
                b = true;
            }
            return b;
        }
        #endregion

        #region 提交网上订单调查问卷
        public bool SubmitOrderQuestion(string satisfy, string cMemo, string ccuscode, string strallacount)
        {
            bool b = false;
            string sql = @"insert into dl_opquestion_20170728(satisfy,ccuscode,strallacount,cmemo) 
            values(@satisfy,@ccuscode,@strallacount,@cMemo)";
            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@satisfy",satisfy),
                  new SqlParameter("@cMemo",cMemo),
                    new SqlParameter("@ccuscode",ccuscode),
                      new SqlParameter("@strallacount",strallacount)
 
            };
            int a = sqlh.ExecuteNonQuery(sql, paras, CommandType.Text);
            if (a > 0)
            {
                b = true;
            }
            return b;
        }
        #endregion

        #region 提交网上订单调查问卷－－20180110
        public bool SubmitOrderQuestion20180110(JObject jo)
        {
            bool b = false;
            string sql = @"insert into dl_oporderquestion_20180110(question1,question2,question3,question4,question5,question6,question7,question8,question9,question10,question4_t,question5_t,question6_t,strAllAcount,phone) 
            values(@question1,@question2,@question3,@question4,@question5,@question6,@question7,@question8,@question9,@question10,@question4_t,@question5_t,@question6_t,@strAllAcount,@phone)";

            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@strAllAcount",jo["strAllAcount"]==null?"":jo["strAllAcount"].ToString()),
                new SqlParameter("@phone",jo["phone"]==null?"":jo["phone"].ToString()),
                new SqlParameter("@question4_t",jo["question4_t"]==null?"":jo["question4_t"].ToString()),
                new SqlParameter("@question5_t",jo["question5_t"]==null?"":jo["question5_t"].ToString()),
                new SqlParameter("@question6_t",jo["question6_t"]==null?"":jo["question6_t"].ToString()),
                new SqlParameter("@question1",jo["question1"]==null?"":jo["question1"].ToString()),
                new SqlParameter("@question2",jo["question2"]==null?"":jo["question2"].ToString()),
                new SqlParameter("@question3",jo["question3"]==null?"":jo["question3"].ToString()),
                new SqlParameter("@question4",jo["question4"]==null?"":jo["question4"].ToString()),
                new SqlParameter("@question5",jo["question5"]==null?"":jo["question5"].ToString()),
                new SqlParameter("@question6",jo["question6"]==null?"":jo["question6"].ToString()),
                new SqlParameter("@question7",jo["question7"]==null?"":jo["question7"].ToString()),
                new SqlParameter("@question8",jo["question8"]==null?"":jo["question8"].ToString()),
                new SqlParameter("@question9",jo["question9"]==null?"":jo["question9"].ToString()),
                new SqlParameter("@question10",jo["question10"]==null?"":jo["question10"].ToString())
 
            };
            int a = sqlh.ExecuteNonQuery(sql, paras, CommandType.Text);
            if (a > 0)
            {
                b = true;
            }
            return b;
        }
        #endregion

        #region 提交网上订单调查问卷－－20180808
        public bool SubmitOrderQuestion20180808(JObject jo)
        {
            bool b = false;
            string sql = @"insert into dl_oporderquestion_20180808(question1,question2,question3,question4,question5,question6,question7,question8,question9,question10,question1_t,question2_t,question3_t,
            question4_t,question5_t,question6_t,question7_t,question8_t,question9_t,strAllAcount,phone) 
            values(@question1,@question2,@question3,@question4,@question5,@question6,@question7,@question8,@question9,@question10,
@question1_t,@question2_t,@question3_t,@question4_t,@question5_t,@question6_t,@question7_t,@question8_t,@question9_t,@strAllAcount,@phone)";

            SqlParameter[] paras = new SqlParameter[]{
                    new SqlParameter("@strAllAcount",jo["strAllAcount"]==null?"":jo["strAllAcount"].ToString()),
                    new SqlParameter("@phone",jo["phone"]==null?"":jo["phone"].ToString()),
                    new SqlParameter("@question1",jo["question1"]==null?"":jo["question1"].ToString()),
                    new SqlParameter("@question2",jo["question2"]==null?"":jo["question2"].ToString()),
                    new SqlParameter("@question3",jo["question3"]==null?"":jo["question3"].ToString()),
                    new SqlParameter("@question4",jo["question4"]==null?"":jo["question4"].ToString()),
                    new SqlParameter("@question5",jo["question5"]==null?"":jo["question5"].ToString()),
                    new SqlParameter("@question6",jo["question6"]==null?"":jo["question6"].ToString()),
                    new SqlParameter("@question7",jo["question7"]==null?"":jo["question7"].ToString()),
                    new SqlParameter("@question8",jo["question8"]==null?"":jo["question8"].ToString()),
                    new SqlParameter("@question9",jo["question9"]==null?"":jo["question9"].ToString()),
                    new SqlParameter("@question10",jo["question10"]==null?"":jo["question10"].ToString()),
                    new SqlParameter("@question1_t",jo["question4_t"]==null?"":jo["question1_t"].ToString()),
                    new SqlParameter("@question2_t",jo["question4_t"]==null?"":jo["question2_t"].ToString()),
                    new SqlParameter("@question3_t",jo["question4_t"]==null?"":jo["question3_t"].ToString()),
                    new SqlParameter("@question4_t",jo["question4_t"]==null?"":jo["question4_t"].ToString()),
                    new SqlParameter("@question5_t",jo["question5_t"]==null?"":jo["question5_t"].ToString()),
                    new SqlParameter("@question6_t",jo["question6_t"]==null?"":jo["question6_t"].ToString()),
                    new SqlParameter("@question7_t",jo["question4_t"]==null?"":jo["question7_t"].ToString()),
                    new SqlParameter("@question8_t",jo["question4_t"]==null?"":jo["question8_t"].ToString()),
                    new SqlParameter("@question9_t",jo["question4_t"]==null?"":jo["question9_t"].ToString()),
            
            };
            int a = sqlh.ExecuteNonQuery(sql, paras, CommandType.Text);
            if (a > 0)
            {
                b = true;
            }
            return b;
        }
        #endregion

        #region 提交网上订单调查问卷－－20181225
        public bool SubmitOrderQuestion20181225(JObject jo)
        {
            bool b = false;
            string sql = @"INSERT INTO dbo.dl_oporderquestion_20181225 ( question1 ,question2 ,question3 ,question4 ,question5 ,question6 ,question7 ,question8 ,question9 ,question10 ,question11 ,question12 ,
question13 ,question14 ,question15 ,question16 ,question17 ,question18 ,question19 ,question20 ,question21 ,question22 ,question23 ,question24 ,question25 ,question26 ,question27 ,
question28 ,question29 ,question30 ,question31 ,question32 ,question33 ,question34 ,question35 ,question36 ,question1_t ,question2_t ,question3_t ,question4_t ,question5_t ,question6_t ,
question7_t ,question8_t ,question9_t ,question10_t ,question11_t ,question12_t ,question13_t ,question14_t ,question15_t ,question16_t ,question17_t ,question18_t ,question19_t ,
question20_t ,question21_t ,question22_t ,question23_t ,question24_t ,question25_t ,question26_t ,question27_t ,question28_t ,question29_t ,question30_t ,question31_t ,question32_t ,
question33_t ,question34_t ,question35_t ,question36_t ,question37_t ,strAllAcount ,phone )
VALUES (@question1 ,@question2 ,@question3 ,@question4 ,@question5 ,@question6 ,@question7 ,@question8 ,@question9 ,@question10 ,@question11 ,@question12 ,@question13 ,
@question14 ,@question15 ,@question16 ,@question17 ,@question18 ,@question19 ,@question20 ,@question21 ,@question22 ,@question23 ,@question24 ,@question25 ,@question26 ,
@question27 ,@question28 ,@question29 ,@question30 ,@question31 ,@question32 ,@question33 ,@question34 ,@question35 ,@question36 ,@question1_t ,@question2_t ,@question3_t ,
@question4_t ,@question5_t ,@question6_t ,@question7_t ,@question8_t ,@question9_t ,@question10_t ,@question11_t ,@question12_t ,@question13_t ,@question14_t ,@question15_t ,
@question16_t ,@question17_t ,@question18_t ,@question19_t ,@question20_t ,@question21_t ,@question22_t ,@question23_t ,@question24_t ,@question25_t ,@question26_t ,@question27_t ,
@question28_t ,@question29_t ,@question30_t ,@question31_t ,@question32_t ,@question33_t ,@question34_t ,@question35_t ,@question36_t ,@question37_t ,@strAllAcount ,@phone)";

            SqlParameter[] paras = new SqlParameter[]{
                    new SqlParameter("@strAllAcount",jo["strAllAcount"]==null?"":jo["strAllAcount"].ToString()),
                    new SqlParameter("@phone",jo["phone"]==null?"":jo["phone"].ToString()),
                    new SqlParameter("@question1",jo["question1"]==null?"":jo["question1"].ToString()),
                    new SqlParameter("@question2",jo["question2"]==null?"":jo["question2"].ToString()),
                    new SqlParameter("@question3",jo["question3"]==null?"":jo["question3"].ToString()),
                    new SqlParameter("@question4",jo["question4"]==null?"":jo["question4"].ToString()),
                    new SqlParameter("@question5",jo["question5"]==null?"":jo["question5"].ToString()),
                    new SqlParameter("@question6",jo["question6"]==null?"":jo["question6"].ToString()),
                    new SqlParameter("@question7",jo["question7"]==null?"":jo["question7"].ToString()),
                    new SqlParameter("@question8",jo["question8"]==null?"":jo["question8"].ToString()),
                    new SqlParameter("@question9",jo["question9"]==null?"":jo["question9"].ToString()),
                    new SqlParameter("@question10",jo["question10"]==null?"":jo["question10"].ToString()),
                    new SqlParameter("@question11",jo["question11"]==null?"":jo["question11"].ToString()),
                    new SqlParameter("@question12",jo["question12"]==null?"":jo["question12"].ToString()),
                    new SqlParameter("@question13",jo["question13"]==null?"":jo["question13"].ToString()),
                    new SqlParameter("@question14",jo["question14"]==null?"":jo["question14"].ToString()),
                    new SqlParameter("@question15",jo["question15"]==null?"":jo["question15"].ToString()),
                    new SqlParameter("@question16",jo["question16"]==null?"":jo["question16"].ToString()),
                    new SqlParameter("@question17",jo["question17"]==null?"":jo["question17"].ToString()),
                    new SqlParameter("@question18",jo["question18"]==null?"":jo["question18"].ToString()),
                    new SqlParameter("@question19",jo["question19"]==null?"":jo["question19"].ToString()),
                    new SqlParameter("@question20",jo["question20"]==null?"":jo["question20"].ToString()),
                    new SqlParameter("@question21",jo["question21"]==null?"":jo["question21"].ToString()),
                    new SqlParameter("@question22",jo["question22"]==null?"":jo["question22"].ToString()),
                    new SqlParameter("@question23",jo["question23"]==null?"":jo["question23"].ToString()),
                    new SqlParameter("@question24",jo["question24"]==null?"":jo["question24"].ToString()),
                    new SqlParameter("@question25",jo["question25"]==null?"":jo["question25"].ToString()),
                    new SqlParameter("@question26",jo["question26"]==null?"":jo["question26"].ToString()),
                    new SqlParameter("@question27",jo["question27"]==null?"":jo["question27"].ToString()),
                    new SqlParameter("@question28",jo["question28"]==null?"":jo["question28"].ToString()),
                    new SqlParameter("@question29",jo["question29"]==null?"":jo["question29"].ToString()),
                    new SqlParameter("@question30",jo["question30"]==null?"":jo["question30"].ToString()),
                    new SqlParameter("@question31",jo["question31"]==null?"":jo["question31"].ToString()),
                    new SqlParameter("@question32",jo["question32"]==null?"":jo["question32"].ToString()),
                    new SqlParameter("@question33",jo["question33"]==null?"":jo["question33"].ToString()),
                    new SqlParameter("@question34",jo["question34"]==null?"":jo["question34"].ToString()),
                    new SqlParameter("@question35",jo["question35"]==null?"":jo["question35"].ToString()),
                    new SqlParameter("@question36",jo["question36"]==null?"":jo["question36"].ToString()),
                    new SqlParameter("@question1_t",jo["question1_t"]==null?"":jo["question1_t"].ToString()),
                    new SqlParameter("@question2_t",jo["question2_t"]==null?"":jo["question2_t"].ToString()),
                    new SqlParameter("@question3_t",jo["question3_t"]==null?"":jo["question3_t"].ToString()),
                    new SqlParameter("@question4_t",jo["question4_t"]==null?"":jo["question4_t"].ToString()),
                    new SqlParameter("@question5_t",jo["question5_t"]==null?"":jo["question5_t"].ToString()),
                    new SqlParameter("@question6_t",jo["question6_t"]==null?"":jo["question6_t"].ToString()),
                    new SqlParameter("@question7_t",jo["question7_t"]==null?"":jo["question7_t"].ToString()),
                    new SqlParameter("@question8_t",jo["question8_t"]==null?"":jo["question8_t"].ToString()),
                    new SqlParameter("@question9_t",jo["question9_t"]==null?"":jo["question9_t"].ToString()),
                    new SqlParameter("@question10_t",jo["question10_t"]==null?"":jo["question10_t"].ToString()),
                    new SqlParameter("@question11_t",jo["question11_t"]==null?"":jo["question11_t"].ToString()),
                    new SqlParameter("@question12_t",jo["question12_t"]==null?"":jo["question12_t"].ToString()),
                    new SqlParameter("@question13_t",jo["question13_t"]==null?"":jo["question13_t"].ToString()),
                    new SqlParameter("@question14_t",jo["question14_t"]==null?"":jo["question14_t"].ToString()),
                    new SqlParameter("@question15_t",jo["question15_t"]==null?"":jo["question15_t"].ToString()),
                    new SqlParameter("@question16_t",jo["question16_t"]==null?"":jo["question16_t"].ToString()),
                    new SqlParameter("@question17_t",jo["question17_t"]==null?"":jo["question17_t"].ToString()),
                    new SqlParameter("@question18_t",jo["question18_t"]==null?"":jo["question18_t"].ToString()),
                    new SqlParameter("@question19_t",jo["question19_t"]==null?"":jo["question19_t"].ToString()),
                    new SqlParameter("@question20_t",jo["question20_t"]==null?"":jo["question20_t"].ToString()),
                    new SqlParameter("@question21_t",jo["question21_t"]==null?"":jo["question21_t"].ToString()),
                    new SqlParameter("@question22_t",jo["question22_t"]==null?"":jo["question22_t"].ToString()),
                    new SqlParameter("@question23_t",jo["question23_t"]==null?"":jo["question23_t"].ToString()),
                    new SqlParameter("@question24_t",jo["question24_t"]==null?"":jo["question24_t"].ToString()),
                    new SqlParameter("@question25_t",jo["question25_t"]==null?"":jo["question25_t"].ToString()),
                    new SqlParameter("@question26_t",jo["question26_t"]==null?"":jo["question26_t"].ToString()),
                    new SqlParameter("@question27_t",jo["question27_t"]==null?"":jo["question27_t"].ToString()),
                    new SqlParameter("@question28_t",jo["question28_t"]==null?"":jo["question28_t"].ToString()),
                    new SqlParameter("@question29_t",jo["question29_t"]==null?"":jo["question29_t"].ToString()),
                    new SqlParameter("@question30_t",jo["question30_t"]==null?"":jo["question30_t"].ToString()),
                    new SqlParameter("@question31_t",jo["question31_t"]==null?"":jo["question31_t"].ToString()),
                    new SqlParameter("@question32_t",jo["question32_t"]==null?"":jo["question32_t"].ToString()),
                    new SqlParameter("@question33_t",jo["question33_t"]==null?"":jo["question33_t"].ToString()),
                    new SqlParameter("@question34_t",jo["question34_t"]==null?"":jo["question34_t"].ToString()),
                    new SqlParameter("@question35_t",jo["question35_t"]==null?"":jo["question35_t"].ToString()),
                    new SqlParameter("@question36_t",jo["question36_t"]==null?"":jo["question36_t"].ToString()),
                    new SqlParameter("@question37_t",jo["question37_t"]==null?"":jo["question37_t"].ToString()),
            };
            int a = sqlh.ExecuteNonQuery(sql, paras, CommandType.Text);
            if (a > 0)
            {
                b = true;
            }
            return b;
        }
        #endregion

        #region 判断该用户是否参与过网单调查
        public bool isSubmitQuestion(string strAllAcount)
        {
            string sql = "select top 1 strAllAcount  from dl_oporderquestion_20180110 where strAllAcount=@strAllAcount";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@strAllAcount",strAllAcount)
            };
            DataTable dt = sqlh.ExecuteQuery(sql, paras, CommandType.Text);
            bool b = false;
            if (dt.Rows.Count > 0)
            {
                b = true;
            }
            return b;
        }
        #endregion

        #region 客户登录时，记录登录信息
        public bool LoginInfo(JObject jo)
        {
            bool b = false;
            string sql = @"insert into dl_opLoginInfo(ccuscode,strAllAcount,loginPhone,screenHeight,screenWidth,clientIp,cityId,cityName,browser,ua)
                        values(@ccuscode,@strAllAcount,@loginPhone,@screenHeight,@screenWidth,@clientIp,@cityId,@cityName,@browser,@ua)";

            SqlParameter[] paras = new SqlParameter[]{
             new SqlParameter("@ccuscode",jo["ccuscode"]==null?"":jo["ccuscode"].ToString()),
                new SqlParameter("@strAllAcount",jo["strallacount"]==null?"":jo["strallacount"].ToString()),
                new SqlParameter("@loginPhone",jo["loginPhone"]==null?"":jo["loginPhone"].ToString()),
                new SqlParameter("@screenHeight",jo["screenHeight"]==null?"":jo["screenHeight"].ToString()),
                new SqlParameter("@screenWidth",jo["screenWidth"]==null?"":jo["screenWidth"].ToString()),
                new SqlParameter("@clientIp",jo["clientIp"]==null?"":jo["clientIp"].ToString()),
                new SqlParameter("@cityId",jo["cityId"]==null?"":jo["cityId"].ToString()),
                new SqlParameter("@cityName",jo["cityName"]==null?"":jo["cityName"].ToString()),
                new SqlParameter("@browser",jo["browser"]==null?"":jo["browser"].ToString()),
                new SqlParameter("@ua",jo["ua"]==null?"":jo["ua"].ToString())
            };


            int a = sqlh.ExecuteNonQuery(sql, paras, CommandType.Text);
            if (a > 0)
            {
                b = true;
            }
            return b;

        }
        #endregion

        public DataSet Get_MAA(string ccuscode, string iType, string lngopuserid)
        {
            string sql = "exec DLproc_MAAKYDDBySel '" + ccuscode + "','00';"; //可预约订单
            sql += "exec DLproc_MAATimeBySel_V2 " + iType + ";";                 //可预约时间段
            sql += @"SELECT a.* FROM dbo.Dl_opUserAddress a                    
                LEFT JOIN dbo.Dl_opUser b  ON a.lngopUserId=b.lngopUserId
                WHERE a.lngopUserId=" + lngopuserid + " AND strDistributionType='自提' ORDER BY a.lngopUseraddressId desc;";     //自提地址
            sql += "SELECT cValue FROM UserDefine WHERE cID='03'";           //车辆类型
            DataSet ds = sqlh.ExecuteDataSet(sql, CommandType.Text);
            ds.Tables[0].TableName = "ordersTable";
            ds.Tables[1].TableName = "timeTable";
            ds.Tables[2].TableName = "ztInfo";
            ds.Tables[3].TableName = "carTypes";

            return ds;
        }

        #region 获取预约初始化订单数据
        public DataTable Get_MAAOrders(string ccuscode, string cSCCode)
        {
            //            string sql = @"select  a.lngopOrderId,a.strBillNo,a.datCreateTime,a.bytStatus,d.cCusName,
            //                                    b.cinvname,b.iquantity,b.cComUnitName,b.cdefine22,c.iInvWeight,c.cInvStd
            //                                    from dl_oporder a 
            //                                    INNER JOIN dbo.Dl_opOrderDetail b 
            //                                    ON a.lngopOrderId=b.lngopOrderId
            //                                    INNER JOIN dbo.Inventory c
            //                                    ON b.cinvcode=c.cInvCode
            //                                    INNER JOIN dbo.Customer d
            //                                    ON a.ccuscode=d.cCusCode
            //                                    WHERE a.datCreateTime>'2018-01-11'
            //                                    AND a.lngopUserId=92
            //                                    ORDER BY a.lngopOrderId DESC";
            //            return sqlh.ExecuteQuery(sql, CommandType.Text);
            string sql = "DLproc_MAAKYDDBySel";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@ccuscode",ccuscode),
             new SqlParameter("@cSCCode",cSCCode)
            };
            return sqlh.ExecuteQuery(sql, paras, CommandType.StoredProcedure);


        }
        #endregion

        #region 获取预约初始化时间段数据
        public DataTable Get_MAATimes(string iType)
        {
            string sql = "dbo.DLproc_MAATimeBySel_V2";
            //   sql = "DLproc_MAATimeBySel";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@iType",iType)
            };
            return sqlh.ExecuteQuery(sql, paras, CommandType.StoredProcedure);

        }
        #endregion

        #region 提交普通预约信息前检测数据是否合法
        public DataTable DLproc_MAACheckDataBySel(JObject jo)
        {
            string sql = "DLproc_MAACheckDataBySel";
            SqlParameter[] paras = new SqlParameter[]{
                   new SqlParameter("@cCode",jo["cCode"].ToString()),
                   new SqlParameter("@datDate",jo["datDate"].ToString()),
                   new SqlParameter("@cCusCode",jo["cCusCode"].ToString()),
                   new SqlParameter("@lngopOrderId",jo["o"].ToString()),
                   new SqlParameter("@cCarNumber",jo["cCarNumber"].ToString())
            };
            return sqlh.ExecuteQuery(sql, paras, CommandType.StoredProcedure);
        }
        #endregion

        #region 提交预约信息
        public JObject DLproc_NewMAAOrderByIns(string ProcName, JObject jo, DataTable arrBody)
        {
            bool b = false;
            DataTable dt = new AdminManager().Get_ProcParas(ProcName);
            List<SqlCommand> cmds = new List<SqlCommand>();
            SqlParameter[] paras = Get_SqlParas(dt, jo);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "DLproc_NewMAAOrderByIns";
            foreach (SqlParameter sp in paras)
            {
                cmd.Parameters.Add(sp);
            }
            cmd.CommandType = CommandType.StoredProcedure;
            cmds.Add(cmd);
            JObject j = new JObject();
            j = sqlh.SqlTransBulk(cmds.ToArray(), arrBody, "MAAOrderID");
            if (j["flag"].ToString() == "1")
            {
                return j;
            }

            else
            {
                Model.ReInfo ri = new Model.ReInfo();
                ri.list_msg = new List<string>();
                ri.list_msg.Add(DateTime.Now.ToString());
                ri.list_msg.Add(j["ErrMsg"].ToString());
                ri.dt = arrBody;
                new Check().WriteLog(ri);

            }
            return j;
        }
        #endregion

        #region 根据查询出的存储过程参数拼接SqlParameter[]
        public SqlParameter[] Get_SqlParas(DataTable dt, JObject jo)
        {
            List<SqlParameter> list = new List<SqlParameter>();

            foreach (DataRow row in dt.Rows)
            {
                SqlParameter sp = new SqlParameter();
                sp.SqlDbType = DbDao.Get_SqlDbType(row["type"].ToString());
                if (sp.SqlDbType == SqlDbType.DateTime)
                {
                    sp.SqlDbType = SqlDbType.VarChar;
                }
                if (jo[row["name"].ToString().Substring(1)] == null && DbDao.IsNumber(row["type"].ToString()))
                {
                    sp.Value = 0;
                }
                else
                {
                    sp.Value = jo[row["name"].ToString().Substring(1)];
                }
                sp.ParameterName = row["name"].ToString();
                list.Add(sp);
            }
            return list.ToArray();
        }
        #endregion

        #region 获取预约号列表
        public DataTable Get_MAAList(string cCusCode, string iType)
        {
            string sql = @"select b.bytStatus as status,* from dbo.Dl_opMAAOrderDetail a
                        INNER JOIN dl_opmaaorder b ON a.MAAOrderID=b.MAAOrderID
                        INNER JOIN dbo.Dl_opOrderDetail c ON a.lngopOrderId=c.lngopOrderId
                        INNER JOIN dl_opOrder d on a.lngopOrderId=d.lngopOrderId
                        INNER JOIN inventory e on c.cinvcode=e.cinvcode
                        WHERE b.cCusCode=@cCusCode and iType=@iType";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@cCusCode",cCusCode),
            new SqlParameter("@iType",iType)
            };
            return sqlh.ExecuteQuery(sql, paras, CommandType.Text);
        }
        #endregion

        #region 追加预约订单
        public bool AddToMAAOrder(DataTable dt)
        {
            return sqlh.SqlBulkCopy(dt);
        }
        #endregion

        #region 手机端获取一条预约信息
        public DataTable MAA_Info(string id)
        {
            string sql = @"select  a.datBillTime as MAAcreatetime,* from dl_opMAAorder a 
                        inner join dl_opmaaorderdetail b 
                        ON a.MAAOrderID=b.MAAOrderID
                        INNER JOIN dbo.Dl_opOrderDetail c
                        ON b.lngopOrderId=c.lngopOrderId
                        INNER JOIN dl_oporder d
                        ON b.lngopOrderId=d.lngopOrderId
                        WHERE a.MAAOrderID=@MAAOrderId";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@MAAOrderId",id)
            
            };
            return sqlh.ExecuteQuery(sql, paras, CommandType.Text);
        }
        #endregion

        #region 2017-08-23增加  插入特殊订单扩展表 Dl_opPreOrderDetail_Ex
        public void Insert_Dl_opPreOrderDetail_Ex(string strBillNo)
        {
            string sql = @"SELECT '' AS 'lngPreOrderDetail_ExId',b.lngPreOrderDetailId, '' AS bSend,null AS bSendTime  FROM dl_oppreorder a
                        INNER JOIN dbo.Dl_opPreOrderDetail b ON a.lngPreOrderId=b.lngPreOrderId
                        WHERE a.strBillNo=@strBillNo";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@strBillNo",strBillNo)
            };
            DataTable dt = sqlh.ExecuteQuery(sql, paras, CommandType.Text);

            dt.TableName = "Dl_opPreOrderDetail_Ex";
            sqlh.SqlBulkCopy(dt);

        }
        #endregion

        #region 客户提交反馈信息
        public bool SaveFeedBack(string question, string phone, string ccuscode, string strallacount)
        {
            string sql = "insert into Dl_opCusFeedback (cCusCode,cContent,cContacts,cPhone,bytStatus) values(@cCusCode,@cContent,@cContacts,@cPhone,1)";
            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@cCusCode",ccuscode),
                 new SqlParameter("@cContent",question),
                  new SqlParameter("@cContacts",strallacount),
                   new SqlParameter("@cPhone",phone),
            };

            bool b = false;
            int a = sqlh.ExecuteNonQuery(sql, paras, CommandType.Text);
            if (a > 0)
            {
                b = true;
            }
            return b;
        }
        #endregion

        #region 获取该用户所有的反馈
        public DataTable GetAllFeedBack(string ccuscode)
        {
            string sql = @"   SELECT a.*,b.strUsername as replayer FROM Dl_opCusFeedback  a
 left join dl_opuser b on a.creplayer=b.strloginname
   where a.ccuscode=@ccuscode order by lngopCusFeedbackId  desc";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@ccuscode",ccuscode)
            };
            return sqlh.ExecuteQuery(sql, paras, CommandType.Text);
        }
        #endregion

        #region 获取手机端订单信息
        public DataTable OrderInfo(string orderId, string orderType)
        {
            string sql = string.Empty;
            if (orderType == "1")
            {
                sql = @"  select a.*,b.*,c.cInvStd from dl_oporderdetail a
                          left JOIN dl_oporder b ON a.lngopOrderId=b.lngopOrderId
                          LEFT JOIN inventory c ON a.cinvcode=c.cInvCode
                          WHERE b.lngopOrderId=@orderId";
            }
            else
            {
                sql = @"select c.cInvStd,c.cInvName,a.*,b.* from dbo.Dl_opPreOrderDetail a
                        left JOIN dl_opPreOrder b ON a.lngPreOrderId=b.lngPreOrderId
                        LEFT JOIN inventory c ON a.cinvcode=c.cInvCode
                        WHERE b.lngPreOrderId=@orderId";
            }
            SqlParameter[] paras = new SqlParameter[] { 
           new SqlParameter("@orderId",orderId)
           };
            return sqlh.ExecuteQuery(sql, paras, CommandType.Text);
        }
        #endregion

        #region 获取用户自定义编码
        public DataTable GetCodeConfig(string strAllAcount)
        {
            string sql = @"  select * from Dl_opInvCodeConfig where strAllAcount=@strAllAcount";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@strAllAcount",strAllAcount)
            };
            return sqlh.ExecuteQuery(sql, paras, CommandType.Text);
        }
        #endregion


        #region 保存客户编码，Type:1为清空以前的编码后添加，2为直接添加
        public JObject SaveCodeConfig(JArray ja, string type, string strAllAcount)
        {
            string sql = string.Empty;
            JObject jo = new JObject();
            if (type == "1")
            {
                SqlCommand cmd1 = new SqlCommand();
                cmd1.CommandText = "delete Dl_opInvCodeConfig where strAllAcount=@strAllAcount";
                SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@strAllAcount",strAllAcount)
                };
                cmd1.Parameters.AddRange(paras);
                List<SqlCommand> cmds = new List<SqlCommand>();

                cmds.Add(cmd1);
                DataTable dt = BuildCodeConfigTable(ja, strAllAcount);
                jo = sqlh.BulkCopy(cmds.ToArray(), dt);

            }
            else if (type == "2")
            {
                DataTable dt = BuildCodeConfigTable(ja, strAllAcount);
                jo = sqlh.BulkCopy(dt);
            }
            return jo;
        }
        #endregion


        #region 拼接客户自定义编码表
        public DataTable BuildCodeConfigTable(JArray ja, string strAllAcount)
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("strAllAcount", typeof(string));
            dt.Columns.Add("cInvCode", typeof(string));
            dt.Columns.Add("cInvName", typeof(string));
            dt.Columns.Add("cInvStd", typeof(string));
            dt.Columns.Add("cGroupCode", typeof(string));
            dt.Columns.Add("UnitGroup", typeof(string));
            dt.Columns.Add("cComUnitName", typeof(string));
            dt.Columns.Add("cCusInvCode", typeof(string));
            dt.Columns.Add("datTime", typeof(string));

            JObject j = new JObject();
            string time = DateTime.Now.ToString();
            foreach (var item in ja)
            {
                DataRow dr = dt.NewRow();
                j = JObject.Parse(item.ToString());
                dr["strAllAcount"] = strAllAcount;
                dr["cInvCode"] = j["cInvCode"];
                dr["cInvName"] = j["cInvName"];
                dr["cInvStd"] = j["cInvStd"];
                dr["cGroupCode"] = j["cGroupCode"];
                dr["UnitGroup"] = j["UnitGroup"];
                dr["cComUnitName"] = j["cComUnitName"];
                dr["cCusInvCode"] = j["cCusInvCode"];
                dr["datTime"] = time;
                dt.Rows.Add(dr);
            }
            dt.TableName = "Dl_opInvCodeConfig";
            return dt;

        }
        #endregion

        #region 获取用户自定义编码时选择商品分类
        public DataTable GetCodeConfigProductClass(string ccuscode)
        {
            string sql = "SELECT DISTINCT KeyFieldName as id,ParentFieldName as pid,NodeName as name FROM Dl_opInventoryClass WHERE ccuscode LIKE @ccuscode AND (KeyFieldName LIKE '01%' OR KeyFieldName LIKE '02%') ORDER BY id";

            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@ccuscode",ccuscode+"%")
                };
            return sqlh.ExecuteQuery(sql, paras, CommandType.Text);

        }
        #endregion


        #region 用户自定义编码时，获取详细产品列表
        public DataTable GetCodeConfigProducts(string ccuscode, string strAllAcount, string code)
        {
            string sql = "DLproc_CustomerInvCodeShowBySel";
            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@cInvCCode",code),
                new SqlParameter("@strAllAcount",strAllAcount),
                new SqlParameter("@cCusCode",ccuscode),

                };
            return sqlh.ExecuteQuery(sql, paras, CommandType.StoredProcedure);

        }
        #endregion

        #region 通过用户自定义编码查找对应的产品代码
        public JObject GetcInvCodeByCusInvCode(string code, string strAllAcount)
        {
            string sql = "select * from Dl_opInvCodeConfig where strAllAcount=@strAllAcount and cCusInvCode=@code";
            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@strAllAcount",strAllAcount),
                new SqlParameter("@code",code)
                };
            DataTable dt = sqlh.ExecuteQuery(sql, paras, CommandType.Text);
            JObject jo = new JObject();
            if (dt.Rows.Count == 0)
            {
                jo["message"] = "未查询到该用户编码对应的产品！";
                jo["flag"] = 0;
            }
            else
            {
                jo["message"] = dt.Rows[0]["cInvCode"].ToString();
                jo["flag"] = 1;
            }
            return jo;
        }
        #endregion

        #region 获取客户所有订单列表（不超过三个月）
        public DataTable GetAllOrders(string strallacount, string start_time, string end_time, string userType, string bytstatus, string kpdw)
        {
            string sql = "";
            if (userType == "0")
            {
                sql = @"select * from dl_oporder where lngopuserid=@strallacount and datCreateTime between  @start_time and @end_time   ";
            }
            else
            {
                sql = @"select * from dl_oporder where strAllAcount=@strallacount and datCreateTime between  @start_time and @end_time ";
            }

            if (bytstatus == "1")
            {
                sql += " and bytstatus=1";
            }
            else if (bytstatus == "2")
            {
                sql += " and bytstatus=2";
            }
            else if (bytstatus == "3")
            {
                sql += " and bytstatus=3";
            }
            else if (bytstatus == "4")
            {
                sql += " and bytstatus=4";
            }
            else if (bytstatus == "99")
            {
                sql += " and bytstatus>89";
            }

            if (kpdw != "0")
            {
                sql += " and ccuscode=@kpdw";
            }
            sql += " order by lngoporderid desc";
            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@strallacount",strallacount),
                new SqlParameter("@start_time",start_time),
                new SqlParameter("@end_time",end_time+" 23:59:59"),
                new SqlParameter("@kpdw",kpdw)
                };
            return sqlh.ExecuteQuery(sql, paras, CommandType.Text);

        }

        #endregion

        #region 根据网上订单号获取订单详情
        public DataTable GetOrderDetail(string strbillno)
        {
            string sql = @" SELECT a.iquantity,a.cComUnitName,a.cdefine22,a.itaxunitprice,a.isum,a.irowno,a.cinvname,b.*,c.cInvStd FROM dbo.Dl_opOrderDetail a 
INNER JOIN dl_oporder b 
ON a.lngopOrderId=b.lngopOrderId
INNER JOIN inventory c  ON c.cInvCode=a.cinvcode
WHERE b.strBillNo=@strbillno 
ORDER BY a.irowno";
            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@strbillno",strbillno)
                };
            DataTable dt = sqlh.ExecuteQuery(sql, paras, CommandType.Text);
            dt.Columns.Add("psxx", typeof(string));
            dt.Rows[0]["psxx"] = "";
            if (dt.Rows.Count > 0 && dt.Rows[0]["cSCCode"].ToString() == "01")
            {

                if (strbillno.Substring(0, 2).ToString().ToUpper() == "DL")
                {
                    string cSOCode = strbillno.ToUpper().Replace("DL", "w");
                    sql = @"SELECT '配送信息:'+ISNULL(cdefine1,'')+' '+ISNULL(cdefine13,'')+' '+ISNULL(cdefine2,'')+' '+ISNULL(cdefine10,'')+' '+isnull(cdefine3,'')AS psxx FROM dbo.SO_SOMain WHERE cSOCode=@cSOCode";
                    paras = new SqlParameter[]{
                       new SqlParameter("@cSOCode",cSOCode)
                   };
                    DataTable d = sqlh.ExecuteQuery(sql, paras, CommandType.Text);
                    if (d.Rows.Count > 0)
                    {
                        dt.Rows[0]["psxx"] = d.Rows[0]["psxx"];
                    }
                }
                else if (strbillno.Substring(0, 2).ToString().ToUpper() == "CZ")
                {

                    sql = @" SELECT '配送信息:'+ISNULL(cdefine1,'')+' '+ISNULL(cdefine13,'')+' '+ISNULL(cdefine2,'')+' '+ISNULL(cdefine10,'')+' '+isnull(cdefine3,'')AS psxx FROM dbo.DispatchList WHERE cdlCode=@cdlCode";
                    paras = new SqlParameter[]{
                       new SqlParameter("@cdlCode",strbillno)
                   };
                    DataTable d = sqlh.ExecuteQuery(sql, paras, CommandType.Text);
                    if (d.Rows.Count > 0)
                    {
                        dt.Rows[0]["psxx"] = d.Rows[0]["psxx"];
                    }

                }
            }
            return dt;
        }

        #endregion

        #region 客服帮助客户下单后，客户确认订单
        public JObject ConfirmOrder(string orderId, string orderType, string strAllAcount)
        {
            JObject jo = new JObject();
            if (orderType == "1")
            {
                string sql = "select * from dl_oporder where lngoporderid=@lngoporderid";
                SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@lngoporderid",orderId)
                };
                DataTable dt = sqlh.ExecuteQuery(sql, paras, CommandType.Text);
                if (dt.Rows.Count == 0)
                {
                    jo["flag"] = 0;
                    jo["message"] = "未查询到该订单，请重试或联系管理员！";
                    return jo;
                }
                if (dt.Rows[0]["strAllAcount"].ToString() != strAllAcount)
                {
                    jo["flag"] = 0;
                    jo["message"] = "你没有权限确认该订单！";
                    return jo;
                }
                if (dt.Rows[0]["bytStatus"].ToString() != "2")
                {
                    jo["flag"] = 0;
                    jo["message"] = "该订单状态不正确，请重试或联系管理员！";
                    return jo;
                }
                sql = "update dl_oporder set bytstatus=1,confirmordertime=getdate(),confirmorderallacount=@strAllAcount where lngoporderid=@lngoporderid";
                paras = new SqlParameter[]{
                new SqlParameter("@lngoporderid",orderId),
                new SqlParameter("@strAllAcount",strAllAcount)
                };
                int a = sqlh.ExecuteNonQuery(sql, paras, CommandType.Text);
                if (a > 0)
                {
                    jo["flag"] = 1;
                    jo["message"] = "确认成功！";
                    return jo;
                }
                else
                {
                    jo["flag"] = 0;
                    jo["message"] = "确认失败，请重试或联系管理员！";
                    return jo;
                }
            }
            else if (orderType == "2")
            {
                string sql = "select * from dl_oppreorder where lngPreOrderId=@lngPreOrderId";
                SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@lngPreOrderId",orderId)
                };
                DataTable dt = sqlh.ExecuteQuery(sql, paras, CommandType.Text);
                if (dt.Rows.Count == 0)
                {
                    jo["flag"] = 0;
                    jo["message"] = "未查询到该订单，请重试或联系管理员！";
                    return jo;
                }
                if (dt.Rows[0]["strAllAcount"].ToString() != strAllAcount)
                {
                    jo["flag"] = 0;
                    jo["message"] = "你没有权限确认该订单！";
                    return jo;
                }
                if (dt.Rows[0]["bytStatus"].ToString() != "2")
                {
                    jo["flag"] = 0;
                    jo["message"] = "该订单状态不正确，请重试或联系管理员！";
                    return jo;
                }
                sql = "update dl_oppreorder set bytstatus=1,confirmordertime=getdate(),confirmorderallacount=@strAllAcount where lngPreOrderId=@lngPreOrderId";
                paras = new SqlParameter[]{
                new SqlParameter("@lngPreOrderId",orderId),
                new SqlParameter("@strAllAcount",strAllAcount)
                };
                int a = sqlh.ExecuteNonQuery(sql, paras, CommandType.Text);
                if (a > 0)
                {
                    jo["flag"] = 1;
                    jo["message"] = "确认成功！";
                    return jo;
                }
                else
                {
                    jo["flag"] = 0;
                    jo["message"] = "确认失败，请重试或联系管理员！";
                    return jo;
                }
            }

            return jo;

        }
        #endregion

        #region 根据strAllAcount获取客户仓库
        public DataTable GetWareHouseBycCusCode(string cCusCode)
        {
            //            string sql = @"SELECT ISNULL(cWhCode,'CD01') cWhCode,ISNULL(cWhName,'成都仓库') cWhName FROM dbo.Warehouse a right JOIN 
            //(SELECT * FROM  [dbo].[Dl_FUN_Split] ((SELECT TOP 1 cWhCode FROM customer WHERE cCusCode=@cCusCode),'|')) t
            //ON a.cWhCode=t.col";
            //            sql = @"DECLARE @c VARCHAR(50);
            //SELECT TOP 1 @c=cWhCode FROM customer WHERE cCusCode=@cCusCode;
            //SELECT ISNULL(cWhCode,'CD01') cWhCode,ISNULL(cWhName,'成都仓库') cWhName FROM dbo.Warehouse a right JOIN 
            //(SELECT * FROM  [dbo].[Dl_FUN_Split] (@c,'|')) t
            //ON a.cWhCode=t.col";
            string sql = @"SELECT ISNULL(cWhCode,'CD01') cWhCode,ISNULL(cWhName,'成都仓库') cWhName FROM dbo.Warehouse a right JOIN 
(SELECT * FROM  [dbo].[Dl_FUN_Split] ((SELECT TOP 1 ccdefine11 FROM Customer_extradefine WHERE cCusCode=@cCusCode),'|')) t
ON a.cWhCode=t.col";
            sql = @"DECLARE @c VARCHAR(50);
SELECT TOP 1 @c=ccdefine11 FROM Customer_extradefine WHERE cCusCode=@cCusCode;
SELECT ISNULL(cWhCode,'CD01') cWhCode,ISNULL(cWhName,'成都仓库') cWhName FROM dbo.Warehouse a right JOIN 
(SELECT * FROM  [dbo].[Dl_FUN_Split] (@c,'|')) t
ON a.cWhCode=t.col";
            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@cCusCode",cCusCode)
                };
            return sqlh.ExecuteQuery(sql, paras, CommandType.Text);
        }
        #endregion

        #region 20180418添加，批量查询生成用于验证的新Datatable
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listData">表单数据</param>
        /// <param name="kpdw">开票单位编码</param>
        /// <param name="areaid">行政区编码</param>
        /// <param name="cWhCode">仓库编码</param>
        /// <returns></returns>
        public DataTable Get_Check_Dt(List<Buy_list> listData, string kpdw, string areaid, string cWhCode)
        {
            //根据表体数据itemid重新生成Table，计算金额，用于验证信用、是否大于库存、是否大于可用量、是否有未填写数量的商品
            DataTable Check_Dt = new DataTable(); //用于验证的table
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < listData.Count; i++)
            {
                sb.Append(listData[i].cinvcode + "|");
            }
            string QueryString = sb.ToString().TrimEnd('|');
            Check_Dt = DLproc_QuasiOrderDetail_All_Warehouse_BySel(QueryString, kpdw, areaid, cWhCode);
            Check_Dt.Columns.Add("cComUnitQTY", typeof(string));  //基本数量 
            Check_Dt.Columns.Add("cInvDefine2QTY", typeof(string));//小包装数量 
            Check_Dt.Columns.Add("cInvDefine1QTY", typeof(string)); //大包装数量
            Check_Dt.Columns.Add("cDefine22", typeof(string));      //包装结果
            Check_Dt.Columns.Add("iquantity", typeof(string));       //汇总数量
            Check_Dt.Columns.Add("unitGroup", typeof(string));    //包装组
            Check_Dt.Columns.Add("irowno", typeof(string));           //行号
            Check_Dt.Columns.Add("rowType", typeof(int));       //行状态
            for (int i = 0; i < listData.Count; i++)
            {
                if (Check_Dt.Rows[i]["cInvCode"].ToString() == listData[i].cinvcode.ToString())
                {
                    Check_Dt.Rows[i]["cComUnitQTY"] = listData[i].cComUnitQTY;  //基本数量赋值
                    Check_Dt.Rows[i]["cInvDefine2QTY"] = listData[i].cInvDefine2QTY;  //小包装数量赋值
                    Check_Dt.Rows[i]["cInvDefine1QTY"] = listData[i].cInvDefine1QTY;  //大包装数量赋值
                    Check_Dt.Rows[i]["cDefine22"] = listData[i].cDefine22;   //包装结果赋值
                    Check_Dt.Rows[i]["iquantity"] = listData[i].iquantity;   //汇总数量赋值
                    Check_Dt.Rows[i]["unitGroup"] = listData[i].unitGroup;    //单位组赋值
                    Check_Dt.Rows[i]["irowno"] = listData[i].irowno;             //行号赋值
                    Check_Dt.Rows[i]["rowType"] = 0;
                }
            }
            return Check_Dt;
        }

        #endregion

        #region 20180424添加，查询临时订单生成的新Datatable
        public DataTable Get_BackOrder_Dt(DataTable Datatable, string kpdw, string areaid, string cWhCode)
        {

            DataTable Check_Dt = new DataTable(); //用于验证的table
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < Datatable.Rows.Count; i++)
            {
                sb.Append(Datatable.Rows[i]["cinvcode"] + "|");
            }
            string QueryString = sb.ToString().TrimEnd('|');
            Check_Dt = DLproc_QuasiOrderDetail_All_Warehouse_BySel(QueryString, kpdw, areaid, cWhCode);
            Check_Dt.Columns.Add("cComUnitQTY", typeof(string));  //基本数量 
            Check_Dt.Columns.Add("cInvDefine2QTY", typeof(string));//小包装数量 
            Check_Dt.Columns.Add("cInvDefine1QTY", typeof(string)); //大包装数量
            Check_Dt.Columns.Add("cDefine22", typeof(string));      //包装结果
            Check_Dt.Columns.Add("iquantity", typeof(string));       //汇总数量
            Check_Dt.Columns.Add("unitGroup", typeof(string));    //包装组
            Check_Dt.Columns.Add("irowno", typeof(string));           //行号
            Check_Dt.Columns.Add("rowType", typeof(int));       //行状态
            for (int i = 0; i < Datatable.Rows.Count; i++)
            {
                if (Check_Dt.Rows[i]["cInvCode"].ToString() == Datatable.Rows[i]["cinvcode"].ToString())
                {
                    Check_Dt.Rows[i]["cComUnitQTY"] = Datatable.Rows[i]["cComUnitQTY"];  //基本数量赋值
                    Check_Dt.Rows[i]["cInvDefine2QTY"] = Datatable.Rows[i]["cInvDefine2QTY"];  //小包装数量赋值
                    Check_Dt.Rows[i]["cInvDefine1QTY"] = Datatable.Rows[i]["cInvDefine1QTY"];  //大包装数量赋值
                    Check_Dt.Rows[i]["cDefine22"] = Datatable.Rows[i]["pack"];   //包装结果赋值
                    Check_Dt.Rows[i]["iquantity"] = Datatable.Rows[i]["cInvDefineQTY"];   //汇总数量赋值
                    Check_Dt.Rows[i]["irowno"] = Datatable.Rows[i]["irowno"];             //行号赋值
                }
            }
            return Check_Dt;
        }
        #endregion

        #region 20180427添加，查询临时订单生成的新Datatable
        public DataTable Get_HistoryOrder_Dt(DataTable Datatable, string kpdw, string areaid, string cWhCode)
        {

            DataTable Check_Dt = new DataTable(); //用于验证的table
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < Datatable.Rows.Count; i++)
            {
                sb.Append(Datatable.Rows[i]["cinvcode"] + "|");
            }
            string QueryString = sb.ToString().TrimEnd('|');
            Check_Dt = DLproc_QuasiOrderDetail_All_Warehouse_BySel(QueryString, kpdw, areaid, cWhCode);
            Check_Dt.Columns.Add("cComUnitQTY", typeof(string));  //基本数量 
            Check_Dt.Columns.Add("cInvDefine2QTY", typeof(string));//小包装数量 
            Check_Dt.Columns.Add("cInvDefine1QTY", typeof(string)); //大包装数量
            Check_Dt.Columns.Add("cDefine22", typeof(string));      //包装结果
            Check_Dt.Columns.Add("iquantity", typeof(string));       //汇总数量
            Check_Dt.Columns.Add("unitGroup", typeof(string));    //包装组
            Check_Dt.Columns.Add("irowno", typeof(string));           //行号
            Check_Dt.Columns.Add("rowType", typeof(int));       //行状态
            for (int i = 0; i < Datatable.Rows.Count; i++)
            {
                if (Check_Dt.Rows[i]["cInvCode"].ToString() == Datatable.Rows[i]["cinvcode"].ToString())
                {
                    Check_Dt.Rows[i]["cComUnitQTY"] = Datatable.Rows[i]["cComUnitQTY"];  //基本数量赋值
                    Check_Dt.Rows[i]["cInvDefine2QTY"] = Datatable.Rows[i]["cInvDefine2QTY"];  //小包装数量赋值
                    Check_Dt.Rows[i]["cInvDefine1QTY"] = Datatable.Rows[i]["cInvDefine1QTY"];  //大包装数量赋值
                    Check_Dt.Rows[i]["cDefine22"] = Datatable.Rows[i]["cdefine22"];   //包装结果赋值
                    Check_Dt.Rows[i]["iquantity"] = Datatable.Rows[i]["iquantity"];   //汇总数量赋值
                    Check_Dt.Rows[i]["irowno"] = Datatable.Rows[i]["irowno"];             //行号赋值
                }
            }
            return Check_Dt;
        }
        #endregion

        #region 传入的拼接存货编码批量查询出不带价格的库存量
        /// <summary>
        /// 
        /// </summary>
        /// <param name="QueryString">传入的拼接存货编码</param>
        /// <param name="cCusCode">客户编码</param>
        /// <param name="cArea">地区编码</param>
        /// <param name="cWhCode">仓库编码</param>
        /// <returns></returns>
        public DataTable DLproc_QuasiOrderDetail_All_Warehouse_BySel(string QueryString, string cCusCode, string cArea, string cWhCode)
        {
            string sql = "DLproc_QuasiOrderDetail_All_Warehouse_BySel";
            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@cinvcode",QueryString),
                new SqlParameter("@cCusCode",cCusCode),
                new SqlParameter("@cArea",cArea),
                new SqlParameter("@chdefine51",cWhCode),
                };
            return sqlh.ExecuteQuery(sql, paras, CommandType.StoredProcedure);
        }
        #endregion

        #region 修改普通订单和样品订单界面获取订单详情
        public DataTable GetModifyOrderDetail(string strBillNo)
        {
            JObject jo = new JObject();
            string sql = @" SELECT ISNULL(cc.ccdefine11,'CD01') cWhCode,aa.*,bb.* FROM dl_oporder aa
                            INNER JOIN dbo.Dl_opOrderDetail bb
                            ON aa.lngopOrderId=bb.lngopOrderId
                            INNER JOIN Customer_extradefine cc
                             ON aa.ccuscode=cc.ccuscode
                            WHERE aa.strBillNo=@strBillNo";
            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@strBillNo",strBillNo),
                };
            return sqlh.ExecuteQuery(sql, paras, CommandType.Text);
        }
        #endregion

        #region 修改普通订单库存查询
        public DataTable DLproc_QuasiOrderDetailModify_All_Warehouse_BySel(string strbillno, string cinvcode, string ccuscode, string chdefine51, string cArea)
        {
            string sql = "DLproc_QuasiOrderDetailModify_All_Warehouse_BySel";
            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@strbillno",strbillno),
                new SqlParameter("@cinvcode",cinvcode),
                new SqlParameter("@ccuscode",ccuscode),
                new SqlParameter("@chdefine51",chdefine51),
                new SqlParameter("@cArea",cArea)
                };
            return sqlh.ExecuteQuery(sql, paras, CommandType.StoredProcedure);
        }
        #endregion



        #region 返回修改普通订单生成的Table
        public DataTable Get_ModifyOrder_Dt(string strbillno, string cinvcode, string ccuscode, string chdefine51, string cArea, DataTable dt)
        {
            DataTable datatable = DLproc_QuasiOrderDetailModify_All_Warehouse_BySel(strbillno, cinvcode, ccuscode, chdefine51, cArea);
            datatable.Columns.Add("cComUnitQTY", typeof(string));  //基本数量 
            datatable.Columns.Add("cInvDefine2QTY", typeof(string));//小包装数量 
            datatable.Columns.Add("cInvDefine1QTY", typeof(string)); //大包装数量
            datatable.Columns.Add("cDefine22", typeof(string));      //包装结果
            datatable.Columns.Add("iquantity", typeof(string));       //汇总数量
            datatable.Columns.Add("UnitGroup", typeof(string));    //包装组
            datatable.Columns.Add("irowno", typeof(string));           //行号
            datatable.Columns.Add("rowType", typeof(int));       //行状态
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (datatable.Rows[i]["cInvCode"].ToString() == dt.Rows[i]["cinvcode"].ToString())
                {
                    datatable.Rows[i]["cComUnitQTY"] = dt.Rows[i]["cComUnitQTY"];  //基本数量赋值
                    datatable.Rows[i]["cInvDefine2QTY"] = dt.Rows[i]["cInvDefine2QTY"];  //小包装数量赋值
                    datatable.Rows[i]["cInvDefine1QTY"] = dt.Rows[i]["cInvDefine1QTY"];  //大包装数量赋值
                    datatable.Rows[i]["cDefine22"] = dt.Rows[i]["cdefine22"];   //包装结果赋值
                    datatable.Rows[i]["iquantity"] = dt.Rows[i]["iquantity"];   //汇总数量赋值
                    datatable.Rows[i]["UnitGroup"] = dt.Rows[i]["unitGroup"];    //单位组赋值
                    datatable.Rows[i]["irowno"] = dt.Rows[i]["irowno"];             //行号赋值
                    datatable.Rows[i]["rowType"] = 0;
                }
            }
            return datatable;

        }
        #endregion


        #region 保存修改普通订单生成的Table
        public DataTable Get_ModifyOrder_Dt(string strbillno, string cinvcode, string ccuscode, string chdefine51, string cArea, List<Buy_list> listData)
        {
            //根据表体数据itemid重新生成Table，计算金额，用于验证信用、是否大于库存、是否大于可用量、是否有未填写数量的商品
            DataTable Check_Dt = new DataTable(); //用于验证的table
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < listData.Count; i++)
            {
                sb.Append(listData[i].cinvcode + "|");
            }
            string QueryString = sb.ToString().TrimEnd('|');
            Check_Dt = DLproc_QuasiOrderDetailModify_All_Warehouse_BySel(strbillno, cinvcode, ccuscode, chdefine51, cArea);
            Check_Dt.Columns.Add("cComUnitQTY", typeof(string));  //基本数量 
            Check_Dt.Columns.Add("cInvDefine2QTY", typeof(string));//小包装数量 
            Check_Dt.Columns.Add("cInvDefine1QTY", typeof(string)); //大包装数量
            Check_Dt.Columns.Add("cDefine22", typeof(string));      //包装结果
            Check_Dt.Columns.Add("iquantity", typeof(string));       //汇总数量
            Check_Dt.Columns.Add("unitGroup", typeof(string));    //包装组
            Check_Dt.Columns.Add("irowno", typeof(string));           //行号
            Check_Dt.Columns.Add("rowType", typeof(int));       //行状态
            for (int i = 0; i < listData.Count; i++)
            {
                if (Check_Dt.Rows[i]["cInvCode"].ToString() == listData[i].cinvcode.ToString())
                {
                    Check_Dt.Rows[i]["cComUnitQTY"] = listData[i].cComUnitQTY;  //基本数量赋值
                    Check_Dt.Rows[i]["cInvDefine2QTY"] = listData[i].cInvDefine2QTY;  //小包装数量赋值
                    Check_Dt.Rows[i]["cInvDefine1QTY"] = listData[i].cInvDefine1QTY;  //大包装数量赋值
                    Check_Dt.Rows[i]["cDefine22"] = listData[i].cDefine22;   //包装结果赋值
                    Check_Dt.Rows[i]["iquantity"] = listData[i].iquantity;   //汇总数量赋值
                    Check_Dt.Rows[i]["unitGroup"] = listData[i].unitGroup;    //单位组赋值
                    Check_Dt.Rows[i]["irowno"] = listData[i].irowno;             //行号赋值
                    Check_Dt.Rows[i]["rowType"] = 0;
                }
            }
            return Check_Dt;
        }
        #endregion

        #region 20180424, 获取临时订单和历史订单详情
        public DataTable DLproc_BackOrderandPrvOrdercInvCodeIsBeLimited_All_Warehouse_BySel(string id, string iShowType, string iBillType)
        {
            string sql = "DLproc_BackOrderandPrvOrdercInvCodeIsBeLimited_All_Warehouse_BySel";
            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@id",id),
                 new SqlParameter("@iShowType",iShowType),
                  new SqlParameter("@iBillType",iBillType),
                };
            return sqlh.ExecuteQuery(sql, paras, CommandType.StoredProcedure);

        }
        #endregion

        #region 订单重量查询
        public DataTable GetOrderWeight(string NumberType, string Number)
        {
            string sql = string.Empty;
            if (NumberType == "1")
            {
                sql = @"SELECT round(SUM( a.iquantity*b.iInvWeight)/1000000,6) AS weight,a.cinvcode,  a.cinvname,b.cInvStd,a.iquantity,a.cdefine22,c.ccusname ,c.strAllAcount FROM dbo.Dl_opOrderDetail a
                            INNER JOIN inventory b ON a.cinvcode=b.cInvCode
                            INNER JOIN dl_oporder c ON a.lngopOrderId=c.lngopOrderId
                            WHERE c.csocode=@Number
                            GROUP BY  a.cinvname,b.cInvStd,a.iquantity ,a.cdefine22,c.ccusname,a.cinvcode,c.strAllAcount
                            ORDER BY weight desc";
            }
            else
            {
                sql = @"SELECT round(SUM( a.iquantity*b.iInvWeight)/1000000,6) AS weight,a.cinvcode,  a.cinvname,b.cInvStd,a.iquantity,a.cdefine22,c.ccusname  ,c.strAllAcount FROM dbo.Dl_opOrderDetail a
                            INNER JOIN inventory b ON a.cinvcode=b.cInvCode
                            INNER JOIN dl_oporder c ON a.lngopOrderId=c.lngopOrderId
                            WHERE c.strbillno=@Number
                            GROUP BY  a.cinvname,b.cInvStd,a.iquantity ,a.cdefine22,c.ccusname,a.cinvcode,c.strAllAcount
                            ORDER BY weight desc";
            }

            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@Number",Number)
            };
            return sqlh.ExecuteQuery(sql, paras, CommandType.Text);

        }
        #endregion

        #region 新增需求订单表头
        public DataTable DLproc_NewOrder_X_ByIns(OrderInfo oi, string areaid, string iaddresstype, string chdefine21, string cWhCode, string lngBillType)
        {
            DataTable dt = new DataTable();
            string cmdText = "DLproc_NewOrder_X_ByIns";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@lngopUserId",oi.LngopUserId),
            new SqlParameter("@datCreateTime",oi.DatCreateTime),
            new SqlParameter("@bytStatus",oi.BytStatus),
            new SqlParameter("@strRemarks",oi.StrRemarks),
            new SqlParameter("@ccuscode",oi.Ccuscode),
            new SqlParameter("@cdefine1",oi.Cdefine1),
            new SqlParameter("@cdefine2",oi.Cdefine2),
            new SqlParameter("@cdefine3",oi.Cdefine3),
            new SqlParameter("@cdefine9",oi.Cdefine9),
            new SqlParameter("@cdefine10",oi.Cdefine10),
            new SqlParameter("@cdefine11",oi.Cdefine11),
            new SqlParameter("@cdefine12",oi.Cdefine12),
            new SqlParameter("@cdefine13",oi.Cdefine13),
            new SqlParameter("@ccusname",oi.Ccusname),
            new SqlParameter("@cpersoncode",oi.Cpersoncode),
            new SqlParameter("@cSCCode",oi.CSCCode),
            new SqlParameter("@datDeliveryDate",oi.DatDeliveryDate),
            new SqlParameter("@strLoadingWays",oi.StrLoadingWays),
            new SqlParameter("@cSTCode",oi.CSTCode),
            new SqlParameter("@lngopUseraddressId",oi.LngopUseraddressId),
            new SqlParameter("@strU8BillNo",oi.StrU8BillNo),
            new SqlParameter("@cdiscountname",oi.Cdiscountname),
            new SqlParameter("@cdefine8",oi.Cdefine8), 
            new SqlParameter("@lngopUserExId",oi.LngopUserExId),
            new SqlParameter("@strAllAcount",oi.StrAllAcount),
            new SqlParameter("@chdefine49",areaid),
            new  SqlParameter("@iaddresstype",iaddresstype),
            new SqlParameter("@chdefine21",chdefine21),
            new SqlParameter("@chdefine51",cWhCode),  //仓库编码
            new SqlParameter("@lngBillType",lngBillType) //订单类型
            };
            dt = sqlh.ExecuteQuery(cmdText, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 获取缺货需求订单列表
        public DataSet GetXOrderList(string strAllAcount, string lngopUserId, string status)
        {
            // string sql = "select top 100 * from dl_oporder where strAllAcount=@strAllAcount";

            StringBuilder sb = new StringBuilder();
            sb.Append("exec DLproc_GetXOrderList '" + strAllAcount + "', '" + status + "';"); //获取列表
            sb.Append(" SELECT cValue FROM UserDefine WHERE cID='03';");//获取车型"
            sb.Append("select * from dl_opuseraddress where  bytstatus=0 and iaddresstype=1 and  lngopUserID=" + lngopUserId + ";");//获取自提地址列表

            DataSet ds = sqlh.ExecuteDataSet(sb.ToString(), CommandType.Text);
            ds.Tables[0].TableName = "XOrderList";
            ds.Tables[1].TableName = "CarTypeList";
            ds.Tables[2].TableName = "AddressList";
            return ds;
        }
        #endregion

        #region 作废需求订单
        public JObject CancelXOrder(string strBillNo, string strAllAcount, string lngopUserID)
        {

            JObject jo = new JObject();
            string sql = "DLproc_CancelXOrder";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@strBillNo",strBillNo),
           new SqlParameter("@strAllAcount",strAllAcount),
            new SqlParameter("@lngopUserID",lngopUserID),
            };
            DataTable dt = sqlh.ExecuteQuery(sql, paras, CommandType.StoredProcedure);
            if (dt.Rows.Count > 0)
            {
                jo["flag"] = dt.Rows[0]["flag"].ToString();
                jo["message"] = dt.Rows[0]["message"].ToString();
            }
            return jo;
        }
        #endregion

        #region 更改缺货需求订单车辆信息
        public JObject UpdateXOrderAddress(string strAllAcount, string strBillNo, string addressId, string carType, string strLoadingWays, string strRemarks)
        {

            JObject jo = new JObject();

            string sql = "DLproc_UpdateXOrderAddress";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@strBillNo",strBillNo),
            new SqlParameter("@strAllAcount",strAllAcount),
            new SqlParameter("@addressId",addressId),
            new SqlParameter("@carType",carType),
            new SqlParameter("@strLoadingWays",strLoadingWays),
            new SqlParameter("@strRemarks",strRemarks),
            };
            DataTable dt = sqlh.ExecuteQuery(sql, paras, CommandType.StoredProcedure);
            if (dt.Rows.Count > 0)
            {
                jo["flag"] = dt.Rows[0]["flag"].ToString();
                jo["message"] = dt.Rows[0]["message"].ToString();
                jo["error"] = dt.Rows[0]["error"].ToString();
            }
            return jo;
        }
        #endregion

        #region 缺货需求订单时间控制
        public DataTable XOrderTimeControl()
        {
            string sql = "DLproc_OPXOrderTimeCheck";
            return sqlh.ExecuteQuery(sql, CommandType.StoredProcedure);
        }
        #endregion

        #region 获取客户信用
        public DataTable GetUserBehavior(string cCusCode, string datStartDate, string datEndDate)
        {
            string sql = "DLproc_CreditBehavior_ListBySel";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@cCusCode",cCusCode),
            new SqlParameter("@datStartDate",datStartDate),
            new SqlParameter("@datEndDate",datEndDate),
            };
            return sqlh.ExecuteQuery(sql, paras, CommandType.StoredProcedure);
        }
        #endregion

        #region  新增普通订单保存前，检测正在进行中的需求订单里是否包括该订单里的产品
        public DataTable CheckCodeInXOrder(string cCusCode, string codes)
        {
            string sql = "DLproc_CheckCodeInXOrder";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@cCusCode",cCusCode),
            new SqlParameter("@codes",codes),
            };
            return sqlh.ExecuteQuery(sql, paras, CommandType.StoredProcedure);
        }
        #endregion
    }

}
