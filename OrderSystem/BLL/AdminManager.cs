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
using System.Collections;

namespace BLL
{
    public class AdminManager
    {

        DAL.SQLHelper sqlhper = new DAL.SQLHelper();
        DAL.DbDao DbDao = new DAL.DbDao();

        #region 获取存储过程参数
        public DataTable Get_ProcParas(string ProcName)
        {
            string cmdText = @"  select  p.name, s.name AS type from sys.parameters p  left join sys.types s on p.user_type_id = s.user_type_id  where object_id in( object_id(@ProcName))";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@ProcName",ProcName)
          };
            return sqlhper.ExecuteQuery(cmdText, paras, CommandType.Text);
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

        #region 根据查询出的存储过程参数拼接SqlParameter[]
        public SqlParameter[] Get_SqlParas(DataTable dt, Dictionary<string, string> dic)
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
                if (string.IsNullOrEmpty(dic[row["name"].ToString().Substring(1)]) && DbDao.IsNumber(row["type"].ToString()))
                {
                    sp.Value = 0;
                }
                else
                {
                    sp.Value = dic[row["name"].ToString().Substring(1)];
                }
                sp.ParameterName = row["name"].ToString();
                list.Add(sp);
            }
            return list.ToArray();
        }
        #endregion

        #region 获取所有用户列表
        public DataTable Get_AllcCuscode()
        {

            string cmdText = @"SELECT bb.cCusCode,bb.cCusName FROM dl_opuser aa
                            LEFT JOIN customer bb ON LEFT(bb.cCusCode,6) = aa.cCusCode
                            WHERE aa.strUserLevel=3";
            SqlParameter[] paras = new SqlParameter[]{
             };

            return sqlhper.ExecuteQuery(cmdText, paras, CommandType.Text);

        }
        #endregion

        #region 获取所有用户及子用户
        public DataSet GetAllUserCode()
        {

            string sql = @"SELECT bb.cCusCode,bb.cCusName,bb.ccusphone FROM dl_opuser aa
                            right JOIN customer bb ON LEFT(bb.cCusCode,6) = aa.cCusCode
                            WHERE aa.strUserLevel=3 AND LEN(bb.ccuscode)=6;";
            sql += "SELECT strloginname AS ccuscode,strAllAccount,strSubPhone FROM dl_opuser_ex where struserlevel=3";

            return sqlhper.ExecuteDataSet(sql, CommandType.Text);
        }
        #endregion

        #region 新增延期通知单
        public bool DLproc_NewArrearByIns(string ProcName, Dictionary<string, string> dic, DataTable arrBody)
        {
            bool b = false;
            DataTable dt = Get_ProcParas(ProcName);
            List<SqlCommand> cmds = new List<SqlCommand>();
            SqlParameter[] paras = Get_SqlParas(dt, dic);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "DLproc_NewArrearByIns";
            foreach (SqlParameter sp in paras)
            {
                cmd.Parameters.Add(sp);
            }
            cmd.CommandType = CommandType.StoredProcedure;
            cmds.Add(cmd);
            string str = sqlhper.SqlTransBulk(cmds.ToArray(), arrBody);
            if (str == "")
            {
                b = true;
            }
            else
            {
                Model.ReInfo ri = new Model.ReInfo();
                ri.list_msg = new List<string>();
                ri.list_msg.Add(DateTime.Now.ToString());
                ri.list_msg.Add(str);
                ri.dt = arrBody;
                new Check().WriteLog(ri);

            }
            return b;
        }
        #endregion

        #region 获取延期通知单列表
        public DataTable Get_ArrearList(string start_date, string end_date, string bytstatus)
        {
            string sql = "";

            if (bytstatus == "0")
            {
                sql = "select b.cCusPerson,* from dl_oparrear a INNER JOIN Customer b ON a.cCusCode=b.cCusCode where   dDate>=@start_date and dDate<=@end_date order by id desc";
            }
            else
            {
                sql = "select b.cCusPerson,* from dl_oparrear a INNER JOIN Customer b ON a.cCusCode=b.cCusCode where bytstatus=@bytstatus and dDate>=@start_date and dDate<=@end_date order by id desc";
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
                new SqlParameter("@bytstatus",bytstatus)
            };
            return sqlhper.ExecuteQuery(sql, paras, CommandType.Text);
        }
        #endregion

        #region 根据通知单号查询详情
        public DataTable Get_ArrearDetail(string code)
        {
            // string sql = @" select * from dl_oparreardetail a INNER JOIN dbo.Dl_opArrear b ON a.id=b.Id WHERE b.cCode=@code";
            string sql = @"select d.ccusperson,c.strUserName, a.*,b.*from dl_oparreardetail a 
                            INNER JOIN dbo.Dl_opArrear b ON a.id=b.Id 
                            left JOIN dl_opuser c ON b.cSaleConfirm=c.lngopUserId
                            LEFT JOIN customer d ON b.cCusCode=d.cCusCode
                            WHERE b.cCode=@code";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@code",code)
            };
            return sqlhper.ExecuteQuery(sql, paras, CommandType.Text);
        }
        #endregion

        #region 延期通知单取回
        public ReInfo BackArrear(string code, string lngopUserId)
        {
            DataTable dt = Get_OneArrear(code);
            ReInfo ri = new ReInfo();
            string bytstatus = dt.Rows[0]["bytstatus"].ToString();
            if (bytstatus != "11" && bytstatus != "21" && bytstatus != "22")
            {
                ri.flag = "0";
                ri.message = "该通知单已无法取回！";
                return ri;
            }
            //if (dt.Rows[0]["cMake"].ToString() == lngopUserId)
            //{
            //    ri.flag = "0";
            //    ri.message = "不需要取回自己创建的通知单！";
            //    return ri;
            //}
            string time = DateTime.Now.ToString();
            string sql = "update dl_oparrear set bytstatus=11 , cmake=@cmake,dDate=@time,cReview='',dReviewDate='' where ccode=@code";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@code",code),
            new SqlParameter("@cmake",lngopUserId),
            new SqlParameter("@time",time)
            };
            int a = sqlhper.ExecuteNonQuery(sql, paras, CommandType.Text);
            if (a > 0)
            {
                ri.flag = "1";
                ri.message = " 取回成功！";
            }
            else
            {
                ri.flag = "0";
                ri.message = "取回失败！";
            }
            return ri;
        }
        #endregion

        #region 延期通知单作废
        public ReInfo CancelArrear(string code, string id)
        {
            DataTable dt = Get_OneArrear(code);
            ReInfo ri = new ReInfo();
            string bytstatus = dt.Rows[0]["bytstatus"].ToString();
            if (bytstatus != "21" && bytstatus != "22" && bytstatus != "11")
            {
                ri.flag = "0";
                ri.message = "该通知单已无法作废！";
                return ri;
            }
            string time = DateTime.Now.ToString();
            string sql = "update dl_oparrear set bytstatus=99,cMake=@id,dDate=@time where ccode=@code";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@code",code),
            new SqlParameter("@id",id),
            new SqlParameter("@time",time)
            };
            int a = sqlhper.ExecuteNonQuery(sql, paras, CommandType.Text);
            if (a > 0)
            {
                ri.flag = "1";
                ri.message = "作废成功！";
            }
            else
            {
                ri.flag = "0";
                ri.message = "作废失败！";
            }
            return ri;
        }
        #endregion

        #region 收款员重新开通网上下单功能
        public ReInfo RenewArrear(string code, string id)
        {
            DataTable dt = Get_OneArrear(code);
            ReInfo ri = new ReInfo();
            string bytstatus = dt.Rows[0]["bytstatus"].ToString();
            if (bytstatus != "43")
            {
                ri.flag = "0";
                ri.message = "该通知单状态不正确！";
                return ri;
            }
            string time = DateTime.Now.ToString();
            string sql = "update dl_oparrear set bytstatus=51,cModify=@id,dModifyDate=@time where ccode=@code";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@code",code),
            new SqlParameter("@id",id),
            new SqlParameter("@time",time)
            };
            int a = sqlhper.ExecuteNonQuery(sql, paras, CommandType.Text);
            if (a > 0)
            {
                ri.flag = "1";
                ri.message = "该客户网单功能已开通！";
            }
            else
            {
                ri.flag = "0";
                ri.message = "网单功能开通失败！";
            }
            return ri;
        }
        #endregion

        #region 提取一条延期通知单
        public DataTable Get_OneArrear(string code)
        {
            string sql = "select * from dl_oparrear where cCode=@code ";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@code",code)
            };
            return sqlhper.ExecuteQuery(sql, paras, CommandType.Text);
        }
        #endregion

        #region 销售经理更新违约金意见
        public ReInfo Update_WYJ(string id, string wyjvalue, string code)
        {
            ReInfo ri = new ReInfo();
            string time = DateTime.Now.ToString();
            string sql = "update dl_oparrear set bDeductionLiquidatedDamages=@wyjvalue ,dDeductionLiquidatedDamagesDate=@time,bytStatus=21,cSaleConfirm=@id where ccode=@code";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@code",code),
             new SqlParameter("@wyjvalue",wyjvalue),
              new SqlParameter("@time",time),
               new SqlParameter("@id",id)
            };
            int a = sqlhper.ExecuteNonQuery(sql, paras, CommandType.Text);
            if (a > 0)
            {
                ri.flag = "1";
                ri.message = "提交成功！";
            }
            else
            {
                ri.flag = "0";
                ri.message = "提交失败！";
            }
            return ri;
        }
        #endregion

        #region 销售经理更新停止发货意见
        public ReInfo Update_Stop(string id, string stopvalue, string cMemo, string code)
        {
            ReInfo ri = new ReInfo();
            string time = DateTime.Now.ToString();
            string sql = "";
            if (stopvalue == "0")
            {
                sql = "update dl_oparrear set bStopShipment=@stopvalue ,dStopShipmentDate=@time,cMemo=@cMemo,bytStatus=22,cSaleConfirm=@id where ccode=@code";

            }
            else
            {
                sql = "update dl_oparrear set bStopShipment=@stopvalue ,dStopShipmentDate=@time,cMemo=@cMemo,bytStatus=23,cSaleConfirm=@id where ccode=@code";

            }
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@code",code),
             new SqlParameter("@stopvalue",stopvalue),
              new SqlParameter("@time",time),
               new SqlParameter("@id",id),
               new SqlParameter("@cMemo",cMemo)
            };
            int a = sqlhper.ExecuteNonQuery(sql, paras, CommandType.Text);
            if (a > 0)
            {
                ri.flag = "1";
                ri.message = "提交成功！";
                if (stopvalue == "0")
                {

                }
                ri.msg = new string[]{
                "3"
                };
            }
            else
            {
                ri.flag = "0";
                ri.message = "提交失败！";
            }
            return ri;
        }
        #endregion

        #region 销售经理确认通知单
        public ReInfo Arrear_confirm(string id, string code, int bytstatus)
        {
            ReInfo ri = new ReInfo();
            string time = DateTime.Now.ToString();

            string sql = "update dl_oparrear set  bytStatus=@bytstatus,cSaleConfirm=@id,dSaleConfirm=@time where ccode=@code";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@code",code),
              new SqlParameter("@time",time),
               new SqlParameter("@id",id),
               new SqlParameter("@bytstatus",bytstatus)
            };
            int a = sqlhper.ExecuteNonQuery(sql, paras, CommandType.Text);
            if (a > 0)
            {
                ri.flag = "1";
                ri.message = "提交成功！";
            }
            else
            {
                ri.flag = "0";
                ri.message = "提交失败！";
            }
            return ri;

        }
        #endregion

        #region 销售经理确认通知单(逾期三个月)
        public ReInfo Arrear_confirm(string id, string code, int bytstatus, string stopvalue, string cMemo)
        {
            ReInfo ri = new ReInfo();
            string time = DateTime.Now.ToString();
            string sql = "";
            if (stopvalue == "0")
            {
                sql = "update dl_oparrear set bStopShipment=0 ,dStopShipmentDate=@time,cMemo=@cMemo,bytStatus=42,cSaleConfirm=@id,dSaleConfirm=@time where ccode=@code";

            }
            else
            {
                sql = "update dl_oparrear set bStopShipment=1 ,dStopShipmentDate=@time,cMemo=@cMemo,bytStatus=43,cSaleConfirm=@id,dSaleConfirm=@time where ccode=@code";

            }
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@code",code),
            new SqlParameter("@time",time),
            new SqlParameter("@id",id),
            new SqlParameter("@bytstatus",bytstatus),
            new SqlParameter("@cMemo",cMemo)
            };
            int a = sqlhper.ExecuteNonQuery(sql, paras, CommandType.Text);
            if (a > 0)
            {
                ri.flag = "1";
                ri.message = "提交成功！";
            }
            else
            {
                ri.flag = "0";
                ri.message = "提交失败！";
            }
            return ri;

        }
        #endregion

        #region 复核通知单
        public ReInfo ConfirmArrear(string id, string code)
        {
            DataTable dt = Get_OneArrear(code);
            ReInfo ri = new ReInfo();
            string bytstatus = dt.Rows[0]["bytstatus"].ToString();
            if (bytstatus != "11")
            {
                ri.flag = "0";
                ri.message = "该通知单无法复核！";
                return ri;
            }
            string sql = "update dl_oparrear set bytstatus=@byt,cReview=@id,dReviewDate=@time where ccode=@code";
            int byt = 0;
            if (double.Parse(dt.Rows[0]["iPreviousThreeMonthMoney"].ToString()) > 0 || double.Parse(dt.Rows[0]["iThreeMonthMoney"].ToString()) > 0)
            //if (dt.Rows[0]["cRemark"].ToString().Trim().Length > 0)
            {
                byt = 22;
            }
            else
            {
                byt = 21;
            }
            string time = DateTime.Now.ToString();
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@code",code),
            new SqlParameter("@id",id),
            new SqlParameter("@time",time),
            new SqlParameter("@byt",byt)
            };
            int a = sqlhper.ExecuteNonQuery(sql, paras, CommandType.Text);
            if (a > 0)
            {
                ri.flag = "1";
                ri.message = byt.ToString();
            }
            else
            {
                ri.flag = "0";
                ri.message = "复核失败！";
            }
            return ri;

        }
        #endregion

        #region 修改延期通知单
        public ReInfo DLproc_NewArrearByUpd(string ProcName, Dictionary<string, string> dic, DataTable arrBody)
        {
            DataTable dt = Get_OneArrear(dic["cCode"]);
            ReInfo ri = new ReInfo();
            string bytstatus = dt.Rows[0]["bytstatus"].ToString();
            if (bytstatus != "11")
            {
                ri.flag = "0";
                ri.message = "该通知单状态已改变，无法修改！";
                return ri;
            }


            dt = Get_ProcParas(ProcName);
            List<SqlCommand> cmds = new List<SqlCommand>();
            SqlParameter[] paras = Get_SqlParas(dt, dic);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "DLproc_NewArrearByUpd";
            foreach (SqlParameter sp in paras)
            {
                cmd.Parameters.Add(sp);
            }
            cmd.CommandType = CommandType.StoredProcedure;
            cmds.Add(cmd);
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "delete from dl_oparreardetail where id=(select id from dl_oparrear where cCode=@cCode)";

            cmd.Parameters.Add(new SqlParameter("cCode", dic["cCode"]));
            cmds.Add(cmd);
            string str = sqlhper.SqlTransBulk(cmds.ToArray(), arrBody);
            if (str == "")
            {

                ri.flag = "1";
                ri.message = "通知单修改成功！";

            }
            else
            {

                ri.flag = "0";
                ri.message = "通知单修改失败！";
                ri.list_msg = new List<string>();
                ri.list_msg.Add(DateTime.Now.ToString());
                ri.list_msg.Add(str);
                ri.dt = arrBody;
                new Check().WriteLog(ri);

            }
            return ri;
        }
        #endregion

        #region 根据电话号码获取Dl_opuser里的ID
        public string GetIdByPhone(string phone)
        {
            string id = "";
            string sql = "select lngopUserId,strLoginName from dl_opuser where struserTel=@phone";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@phone",phone)
            };
            DataTable dt = new DataTable();
            dt = sqlhper.ExecuteQuery(sql, paras, CommandType.Text);
            if (dt.Rows.Count > 0)
            {
                id = dt.Rows[0]["lngopUserId"].ToString();
            }
            return id;
        }
        #endregion

        #region 根据延期通知单号，获取主账号的电话及通知单状态
        public DataTable Get_PhoneByArrearCode(string code)
        {
            string sql = @"SELECT a.cCusCode,a.bytStatus,b.cCusPhone FROM dl_oparrear a 
                            INNER JOIN customer b
                            ON a.cCusCode=b.cCusCode
                            WHERE a.cCode=@code";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@code",code)
            };
            return sqlhper.ExecuteQuery(sql, paras, CommandType.Text);
        }
        #endregion

        #region 获取预约配置表
        public DataTable Get_MAASetiing()
        {
            string sql = "select * from dl_opmaasetting";
            return sqlhper.ExecuteQuery(sql, CommandType.Text);
        }
        #endregion

        #region 保存预约配置
        public int Save_MAASetting(JObject jo)
        {
            string sql = @"update   dl_opMAASetting set  iQty=@iQty,dAheadTime=@dAheadTime,datStartTime=@datStartTime,datEndTime=@datEndTime,datValidStartTime=@datValidStartTime,
                        datValidEndTime=@datValidEndTime,cMaker=@strUserName where cCode=@cCode";
            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@iQty",jo["iQty"].ToString()),
                new SqlParameter("@dAheadTime",jo["dAheadTime"].ToString()),
                new SqlParameter("@datStartTime",jo["datStartTime"].ToString()),
                new SqlParameter("@datEndTime",jo["datEndTime"].ToString()),
                new SqlParameter("@datValidStartTime",jo["datValidStartTime"].ToString()),
                new SqlParameter("@datValidEndTime",jo["datValidEndTime"].ToString()),
                new SqlParameter("@strUserName",jo["strUserName"].ToString()),
                new SqlParameter("@cCode",jo["cCode"].ToString())

            };
            return sqlhper.ExecuteNonQuery(sql, paras, CommandType.Text);
        }
        #endregion


        #region  获取可审核生成U8发货单的预约号列表
        public DataTable Get_ToU8AuditList()
        {
            //            string sql = @"select b.bytStatus as status,* from dbo.Dl_opMAAOrderDetail a
            //                        INNER JOIN dl_opmaaorder b ON a.MAAOrderID=b.MAAOrderID
            //                        INNER JOIN dl_opOrder d on a.lngopOrderId=d.lngopOrderId
            //                        WHERE b.cCusCode=@cCusCode and iType=@iType";
            string sql = "DLproc_MAAForManagerBySel";
            //SqlParameter[] paras = new SqlParameter[]{
            //new SqlParameter("@cCusCode",cCusCode),
            //new SqlParameter("@iType",iType)
            //};
            return sqlhper.ExecuteQuery(sql, CommandType.StoredProcedure);
        }
        #endregion


        #region 获取待审核的特殊预约申请列表
        public DataTable Get_SpecialAuditList(string id, string strLoginName)
        {
            //            string sql = @"select b.bytStatus as status,* from dbo.Dl_opMAAOrderDetail a
            //                        INNER JOIN dl_opmaaorder b ON a.MAAOrderID=b.MAAOrderID
            //                        INNER JOIN dbo.Dl_opOrderDetail c ON a.lngopOrderId=c.lngopOrderId
            //                        INNER JOIN dl_opOrder d on a.lngopOrderId=d.lngopOrderId
            //                        INNER JOIN inventory e on c.cinvcode=e.cinvcode
            //                        WHERE b.bytStatus=1 and iType=2";
            string sql = "DLproc_MAAMyWorkFlowBySel";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@Id",id),
            new SqlParameter("@cOPName",strLoginName)
            };
            return sqlhper.ExecuteQuery(sql, paras, CommandType.StoredProcedure);
        }
        #endregion

        #region 审核特殊预约申请
        public bool AuditSpecialMAA(string id, string MAAID, string User, string opdata, string cMemo)
        {
            bool b = false;
            // string sql = "update dl_opmaaorder set bytStatus=4 where MAAOrderId=@MAAID";
            string sql = "DLproc_MAAHandleMyWorkFlowByUpd";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@MAAOrderId ",MAAID),
            new SqlParameter("@Id",id),
            new SqlParameter("@cOPName",User),
            new SqlParameter("@opdata",opdata),
            new SqlParameter("@cMemo",cMemo),
            new SqlParameter("@appid",22)
            };
            int a = sqlhper.ExecuteNonQuery(sql, paras, CommandType.StoredProcedure);
            if (a > 0)
            {
                b = true;
            }
            return b;
        }
        #endregion


        #region 获取特殊预约的审批意见
        public DataTable AuditSpecialMAAInfo(string MAAOrderId)
        {
            string sql = "DLproc_MAAWorkFlowOpinionBySel";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@MAAOrderId",MAAOrderId)
            };
            return sqlhper.ExecuteQuery(sql, paras, CommandType.StoredProcedure);
        }
        #endregion

        #region 获取时间控制列表
        public DataTable GetTimeControlList()
        {
            string sql = @"select * from Dl_opSystemTimeControl order by lngSystemTimeControlId desc";
            return sqlhper.ExecuteQuery(sql, CommandType.Text);
        }
        #endregion

        #region 删除一条时间控制记录
        public bool DelTimeControlList(string id)
        {
            bool b = false;
            string sql = "delete Dl_opSystemTimeControl where lngSystemTimeControlId=@id";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@id",id)
            };
            int a = sqlhper.ExecuteNonQuery(sql, paras, CommandType.Text);
            if (a > 0)
            {
                b = true;
            }
            return b;
        }
        #endregion

        #region 新增一条时间控制
        public bool AddTimeControl(string datBeginTime, string datEndTime)
        {
            bool b = false;
            string sql = "insert into dl_opsystemtimecontrol(datBeginTime, datEndTime) values(@datBeginTime,@datEndTime)";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@datBeginTime",datBeginTime),
            new SqlParameter("@datEndTime",datEndTime)
            };

            int a = sqlhper.ExecuteNonQuery(sql, paras, CommandType.Text);
            if (a > 0)
            {
                b = true;
            }
            return b;
        }
        #endregion

        #region 数据库锁列表
        public DataTable GetSqlLockList()
        {
            string sql = "DLproc_BlockSQLBySel";
            return sqlhper.ExecuteQuery(sql, CommandType.StoredProcedure);

        }
        #endregion

        #region 杀数据库进程
        public bool KillLock(string id)
        {
            bool b = false;
            string sql = "dlproc_opKillLock";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@id",int.Parse(id))
            };
            int a = sqlhper.ExecuteNonQuery(sql, paras, CommandType.StoredProcedure);
            if (a > 0)
            {
                b = true;
            }
            return true;
        }
        #endregion

        #region 门卫登录
        public DataTable guardLogin(string name, string pwd)
        {
            string sql = "select top 1 * from dl_opuser where strloginname=@name and struserpwd=@pwd and strStatus=1";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@name",name),
            new SqlParameter("@pwd",pwd)
          };

            DataTable dt = sqlhper.ExecuteQuery(sql, paras, CommandType.Text);
            return dt;
        }
        #endregion

        #region 车辆进厂时门卫进行确认并写入进厂时间和放行人
        public bool MAACarInUpd(string cMAACode, string CarInUser)
        {
            bool b = false;
            string CarInDate = DateTime.Now.ToString();
            string sql = "update dl_opMAAorder set datcarin=@carindate,carinuser=@carinuser where cMAACode=@cmaacode";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@carindate",CarInDate),
            new SqlParameter("@cmaacode",cMAACode),
            new SqlParameter("@carinuser",CarInUser)
          };

            int a = sqlhper.ExecuteNonQuery(sql, paras, CommandType.Text);
            if (a > 0)
            {
                b = true;
            }
            return b;
        }
        #endregion

        #region 车辆进厂时门卫进行确认并写入进厂时间和放行人
        public bool MAACarOutUpd(string cMAACode, string CarOutUser)
        {
            bool b = false;
            string CarOutDate = DateTime.Now.ToString();
            string sql = "update dl_opMAAorder set datcarout=@caroutdate,caroutuser=@caroutuser where cMAACode=@cmaacode";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@caroutdate",CarOutDate),
            new SqlParameter("@cmaacode",cMAACode),
            new SqlParameter("@caroutuser",CarOutUser)
          };

            int a = sqlhper.ExecuteNonQuery(sql, paras, CommandType.Text);
            if (a > 0)
            {
                b = true;
            }
            return b;
        }
        #endregion

        #region 订单重量查询
        public DataTable GetOrderWeight(string NumberType, string Number)
        {
            string sql = string.Empty;
            if (NumberType == "1")
            {
                sql = @"SELECT round(SUM( a.iquantity*b.iInvWeight)/1000000,6) AS weight,a.cinvcode,  a.cinvname,b.cInvStd,a.iquantity,a.cdefine22,c.ccusname  FROM dbo.Dl_opOrderDetail a
                            INNER JOIN inventory b ON a.cinvcode=b.cInvCode
                            INNER JOIN dl_oporder c ON a.lngopOrderId=c.lngopOrderId
                            WHERE c.csocode=@Number
                            GROUP BY  a.cinvname,b.cInvStd,a.iquantity ,a.cdefine22,c.ccusname,a.cinvcode
                            ORDER BY weight desc";
            }
            else
            {
                sql = @"SELECT round(SUM( a.iquantity*b.iInvWeight)/1000000,6) AS weight,a.cinvcode,  a.cinvname,b.cInvStd,a.iquantity,a.cdefine22,c.ccusname  FROM dbo.Dl_opOrderDetail a
                            INNER JOIN inventory b ON a.cinvcode=b.cInvCode
                            INNER JOIN dl_oporder c ON a.lngopOrderId=c.lngopOrderId
                            WHERE c.strbillno=@Number
                            GROUP BY  a.cinvname,b.cInvStd,a.iquantity ,a.cdefine22,c.ccusname,a.cinvcode
                            ORDER BY weight desc";
            }

            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@Number",Number)
            };
            return sqlhper.ExecuteQuery(sql, paras, CommandType.Text);

        }
        #endregion

        #region 后台新增预约时获取所有配送订单
        public DataTable GetMAAOrders(string ccuscode, string cSCCode)
        {
            string sql = "DLproc_MAAKYDDBySel";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@ccuscode",ccuscode),
             new SqlParameter("@cSCCode",cSCCode)
            };
            //  return sqlh.ExecuteQuery(sql, CommandType.Text);
            return sqlhper.ExecuteQuery(sql, paras, CommandType.StoredProcedure);
        }
        #endregion

        #region 提交预约信息
        public JObject DLproc_NewMAAOrderByIns(string ProcName, JObject jo, DataTable arrBody)
        {
            DataTable dt = Get_ProcParas(ProcName);
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
            j = sqlhper.SqlTransBulk(cmds.ToArray(), arrBody, "MAAOrderID");
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

        #region 获取预约号列表
        public DataTable Get_MAAList(string iType)
        {
            string sql = @"select b.bytStatus as status,b.datBillTime as createtime,  a.*,b.*,c.iquantity,c.cdefine22,c.cinvname,d.*,e.cInvStd,e.iInvWeight
from dbo.Dl_opMAAOrderDetail a
INNER JOIN dl_opmaaorder b ON a.MAAOrderID=b.MAAOrderID
INNER JOIN dbo.Dl_opOrderDetail c ON a.lngopOrderId=c.lngopOrderId
INNER JOIN dl_opOrder d on a.lngopOrderId=d.lngopOrderId
INNER JOIN inventory e on c.cinvcode=e.cinvcode
WHERE  b.cSCCode!='00' and b.iType=1 ";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@iType",iType)
            };
            return sqlhper.ExecuteQuery(sql, paras, CommandType.Text);
        }
        #endregion

        #region 获取产品现存量
        public DataTable DLproc_TreeListDetailsAll_iqty_BySel(string cInvCCode, string cCusCode)
        {
            string sql = "DLproc_TreeListDetailsAll_iqty_BySel";
            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@cInvCCode",cInvCCode),
            new SqlParameter("@cCusCode",cCusCode)
            };
            DataTable dt = sqlhper.ExecuteQuery(sql, paras, CommandType.StoredProcedure);
            return dt;
        }
        #endregion

        #region 根据预约ID获取该预约的主表记录
        //public DataTable MAAOrderId(string MAAOrderId)
        //{
        //    string sql = "select * from dl_opMAAorder where MAAOrderID=@MAAOrderId";
        //   // SqlParameter[] paras
        //}
        #endregion

        #region 获取用户所有的反馈
        public DataTable GetAllFeedBack()
        {
            string sql = @"SELECT a.*,b.strUsername as replayer FROM Dl_opCusFeedback  a
 left join dl_opuser b on a.creplayer=b.strloginname
  order by lngopCusFeedbackId  desc";
            return sqlhper.ExecuteQuery(sql, CommandType.Text);
        }
        #endregion

        #region 管理员回复反馈
        public bool SaveFeedBack(string content, string ccuscode, string lngopCusFeedbackId)
        {
            string nowtime = DateTime.Now.ToString();
            string sql = "update   Dl_opCusFeedback set cReplayer=@ccuscode,cReplay=@content,datReplayTime=@nowtime,bytStatus=4 where lngopCusFeedbackId=@lngopCusFeedbackId";
            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@ccuscode",ccuscode),
                 new SqlParameter("@content",content),
                 new SqlParameter("@nowtime",nowtime),
                  new SqlParameter("@lngopCusFeedbackId",lngopCusFeedbackId)
            };

            bool b = false;
            int a = sqlhper.ExecuteNonQuery(sql, paras, CommandType.Text);
            if (a > 0)
            {
                b = true;
            }
            return b;
        }
        #endregion

        #region superadmin后台主页获取数据
        public JObject SuperAdminIndex()
        {
            string sql = @"select ccuscode from dl_opcusfeedback where cReplay is null;
                            exec DLproc_BlockSQLBySel";
            JObject jo = new JObject();
            DataSet ds = sqlhper.ExecuteDataSet(sql, CommandType.Text);
            jo["unreplay"] = ds.Tables[0].Rows.Count;
            jo["sqllock"] = ds.Tables[1].Rows.Count;
            jo["flag"] = 1;
            return jo;
        }
        #endregion

        #region 模拟客户登录
        public DataTable SimulateLogin(string phone, string ccuscode)
        {
            //string sql = "DLproc_SubCustomerPhoneNoLogin_OP";
            //            string sql = string.Empty;
            //            if (type == "1")
            //            {
            //                sql = @"select strUserLevel,lngopUserId,strUserName,bb.cCusCode,cCusPPerson,cCusPhone,strLoginName,aa.strStatus,0 'lngopUserExId',strLoginName 'strAllAcount'  from Dl_opUser aa
            //left join Customer bb on aa.cCusCode=bb.cCusCode
            //where bb.cCusPhone like @phonelike  
            //union all
            //select dd.strUserLevel,dd.lngopUserId,strUserName,bb.cCusCode,cCusPPerson,dd.strSubPhone,dd.strLoginName,dd.strStatus,lngopUserExId,strAllAccount 'strAllAcount'  from Dl_opUser aa
            //left join Customer bb on aa.cCusCode=bb.cCusCode
            //left join Dl_opUser_Ex dd on aa.strLoginName=dd.strLoginName
            //where   dd.strSubPhone like @phonelike   ";
            //                SqlParameter[] paras = new SqlParameter[]{
            //            new SqlParameter("@phonelike","%"+phone+"%")
            //            };
            //                return sqlhper.ExecuteQuery(sql, paras, CommandType.Text);
            //            }
            //            else
            //            {
            //                sql = @"select strUserLevel,lngopUserId,strUserName,bb.cCusCode,cCusPPerson,cCusPhone,strLoginName,aa.strStatus,0 'lngopUserExId',strLoginName 'strAllAcount'  from Dl_opUser aa
            //left join Customer bb on aa.cCusCode=bb.cCusCode
            //where bb.cCusPhone like @phonelike AND aa.strLoginName=@ccuscode
            //union all
            //select dd.strUserLevel,dd.lngopUserId,strUserName,bb.cCusCode,cCusPPerson,dd.strSubPhone,dd.strLoginName,dd.strStatus,lngopUserExId,strAllAccount 'strAllAcount'  from Dl_opUser aa
            //left join Customer bb on aa.cCusCode=bb.cCusCode
            //left join Dl_opUser_Ex dd on aa.strLoginName=dd.strLoginName
            //where   dd.strSubPhone like @phonelike  AND aa.strLoginName=@ccuscode";
            //                SqlParameter[] paras = new SqlParameter[]{
            //            new SqlParameter("@phonelike","%"+phone+"%"),
            //            new SqlParameter("@ccuscode",ccuscode)
            //            };
            //                return sqlhper.ExecuteQuery(sql, paras, CommandType.Text);

            //            }

            //            string sql = string.Empty;
            //            if (ccuscode.Length == 6)
            //            {
            //                sql = @"SELECT b.lngopUserId,b.strLoginName,b.strUserLevel,a.ccuscode,a.ccuspperson FROM customer a
            //                        LEFT JOIN dbo.Dl_opUser b
            //                        ON a.ccuscode=b.strLoginName
            //                        WHERE a.ccuscode=@ccuscode AND ccusphone LIKE @phonelike ";
            //                SqlParameter[] paras = new SqlParameter[]{
            //            new SqlParameter("@phonelike","%"+phone+"%"),
            //            new SqlParameter("@ccuscode",ccuscode)
            //            };
            //                return sqlhper.ExecuteQuery(sql, paras, CommandType.Text);
            //            }
            //            else
            //            {
            //                sql = @"  select * from dl_opuser_ex a
            //                          LEFT JOIN customer b 
            //                          ON a.strloginname=b.ccuscode
            //                          WHERE strallaccount=@ccuscode ";
            //                SqlParameter[] paras = new SqlParameter[]{
            //            new SqlParameter("@ccuscode",ccuscode)
            //            };
            //                return sqlhper.ExecuteQuery(sql, paras, CommandType.Text);

            //            }

            string sql = @"select strUserLevel,lngopUserId,strUserName,bb.cCusCode,cCusPPerson,cCusPhone,strLoginName,aa.strStatus,0 'lngopUserExId',strLoginName 'strAllAcount'  from Dl_opUser aa
                        left join Customer bb on aa.cCusCode=bb.cCusCode
                        where bb.cCusPhone like @phonelike  AND bb.cCusCode=@ccuscode
                        union all
                        select dd.strUserLevel,dd.lngopUserId,strUserName,bb.cCusCode,cCusPPerson,dd.strSubPhone,dd.strLoginName,dd.strStatus,lngopUserExId,strAllAccount 'strAllAcount'  from Dl_opUser aa
                        left join Customer bb on aa.cCusCode=bb.cCusCode
                        left join Dl_opUser_Ex dd on aa.strLoginName=dd.strLoginName
                        where   dd.strSubPhone like @phonelike  AND bb.cCusCode=@ccuscode";

            SqlParameter[] paras = new SqlParameter[]{
            new SqlParameter("@ccuscode",ccuscode),
            new SqlParameter("@phonelike","%"+phone+"%")
                };
            return sqlhper.ExecuteQuery(sql, paras, CommandType.Text);


        }

        #endregion


        #region 后台提交普通订单
        public JObject DLproc_NewOrderByIns_Admin(string ProcName, JObject jo)
        {

            DataTable dt = Get_ProcParas(ProcName);
            List<SqlCommand> cmds = new List<SqlCommand>();
            SqlParameter[] paras = Get_SqlParas(dt, jo);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcName;
            foreach (SqlParameter sp in paras)
            {
                cmd.Parameters.Add(sp);
            }
            cmd.CommandType = CommandType.StoredProcedure;
            cmds.Add(cmd);
            dt = GetBodyDataTable(jo["BodyData"].ToArray(), jo["ccuscode"].ToString(), jo["chdefine49"].ToString());
            dt.TableName = "dl_oporderdetail";
            JObject j = new JObject();

            //拼接表体table

            j = sqlhper.SqlTransBulk(cmds.ToArray(), dt, "lngopOrderId");
            if (j["flag"].ToString() != "1")
            {
                Model.ReInfo ri = new Model.ReInfo();
                ri.list_msg = new List<string>();
                ri.list_msg.Add(DateTime.Now.ToString());
                ri.list_msg.Add(j["ErrMsg"].ToString());
                ri.dt = dt;
                new Check().WriteLog(ri);
            }

            return j;
        }
        #endregion

        #region 拼接普通订单表体数据
        public DataTable GetBodyDataTable(Array BodyData, string kpdw, string areaid)
        {
            DataTable dt = new DataTable();
            JObject jo = new JObject();
            JObject j = new JObject();
            string sql = "select top 1 * from dl_oporderdetail";
            DataTable BaseTable = sqlhper.ExecuteQuery(sql, CommandType.Text);
            BaseTable.Rows.RemoveAt(0);
            int index = 1;
            foreach (var item in BodyData)
            {

                DataRow dr = BaseTable.NewRow();

                j = JObject.Parse(item.ToString());
                dt = new product().DLproc_QuasiOrderDetailBySel(j["cInvCode"].ToString(), kpdw, areaid);
                BaseTable.Rows.Add(dr);
                dr["cinvcode"] = j["cInvCode"].ToString();
                dr["iquantity"] = j["cInvDefineQTY"].ToString();
                dr["irowno"] = index;
                index++;
                dr["cinvname"] = j["cInvName"].ToString();
                dr["cComUnitName"] = j["cComUnitName"].ToString();
                dr["cInvDefine1"] = j["cInvDefine1"].ToString();
                dr["cInvDefine2"] = j["cInvDefine2"].ToString();
                dr["cInvDefine13"] = j["cinvdefine13"].ToString();
                dr["cInvDefine14"] = j["cinvdefine14"].ToString();
                dr["UnitGroup"] = j["UnitGroup"].ToString();

                dr["cComUnitQTY"] = j["cComUnitQTY"].ToString();

                dr["cInvDefine1QTY"] = j["cInvDefine1QTY"].ToString();
                dr["cInvDefine2QTY"] = j["cInvDefine2QTY"].ToString();
                dr["cn1cComUnitName"] = j["cn1cComUnitName"].ToString();
                dr["cdefine22"] = j["pack"].ToString();

                dr["cn1cComUnitName"] = dt.Rows[0]["cn1cComUnitName"];
                dr["cunitid"] = dt.Rows[0]["cComUnitCode"];
                double iquantity = Convert.ToDouble(dr["iquantity"].ToString());   //购买货数量
                double iinvexchrate = 1;//换算率 
                double inum = 1;        //辅计量数量   
                //计算销售单位(辅助)的换算率
                if (dr["cn1cComUnitName"].ToString() == dr["cInvDefine1"].ToString())  //大包装单位换算率
                {
                    iinvexchrate = Convert.ToDouble(dr["cInvDefine13"].ToString());
                }
                else if (dr["cn1cComUnitName"].ToString() == dr["cInvDefine2"].ToString()) //小包装单位换算率
                {
                    iinvexchrate = Convert.ToDouble(dr["cInvDefine14"].ToString());
                }
                else  //无换算单位
                {
                    iinvexchrate = 1;//换算率 
                }
                dr["iinvexchrate"] = iinvexchrate;
                inum = Math.Round(iquantity / iinvexchrate, 2);    //辅计量数量 
                dr["inum"] = inum;
                double iquotedprice = Convert.ToDouble(dt.Rows[0]["Quote"].ToString());//报价 保留5位小数,四舍五入
                dr["iquotedprice"] = iquotedprice;//报价 保留5位小数,四舍五入
                double itaxunitprice = Convert.ToDouble(dt.Rows[0]["ExercisePrice"].ToString());//原币含税单价 即执行价格dt.Columns.Add("ExercisePrice"); //基本单位单价(执行价格)   16,保留5位
                dr["itaxunitprice"] = itaxunitprice;
                double isum = Math.Round(itaxunitprice * iquantity, 2);     //原币价税合计    //金额,原币价税合计=原币含税单价*数量,保留2位小数,四舍五入 
                dr["isum"] = isum;
                double itax = Math.Round(isum / 1.17 * 0.17, 2);        //原币税额 ;税额=金额/1.17*0.17 保留2位, 四舍五入    
                dr["itax"] = itax;
                double imoney = Math.Round(isum - itax, 2);      //原币无税金额 =金额-税额,保留2位,四舍五入
                dr["imoney"] = imoney;
                double iunitprice = Math.Round(imoney / iquantity, 6);  //原币无税单价=无税金额/数量,保留5位小数,四舍五入        
                dr["iunitprice"] = iunitprice;
                double idiscount = Math.Round(iquotedprice * iquantity - isum, 2);   //原币折扣额=报价*数量-金额,保留两位
                dr["idiscount"] = idiscount;
                double inatunitprice = iunitprice;//本币无税单价
                dr["inatunitprice"] = inatunitprice;
                double inatmoney = imoney;   //本币无税金额
                dr["inatmoney"] = inatmoney;
                double inattax = itax;     //本币税额 
                dr["inattax"] = inattax;
                double inatsum = isum;     //本币价税合计
                dr["inatsum"] = inatsum;
                double inatdiscount = idiscount;   //本币折扣额 
                dr["inatdiscount"] = inatdiscount;
                double kl = Convert.ToDouble(dt.Rows[0]["Rate"].ToString());          //扣率 
                dr["kl"] = kl;
                double itaxrate = Convert.ToDouble(dt.Rows[0]["iTaxRate"].ToString());    //税率 
                dr["itaxrate"] = itaxrate;
                dr["cDefine24"] = dt.Rows[0]["cDefine24"].ToString() == "无" ? null : "";//活动类型
                dr["cbdefine16"] = dt.Rows[0]["cbdefine16"]; //扣率3
            }
            return BaseTable;
        }
        #endregion

        #region 后台提交特殊订单
        public JObject DLproc_NewYOrderByIns_Admin(string ProcName, JObject jo)
        {

            DataTable dt = Get_ProcParas(ProcName);
            List<SqlCommand> cmds = new List<SqlCommand>();
            SqlParameter[] paras = Get_SqlParas(dt, jo);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = ProcName;
            foreach (SqlParameter sp in paras)
            {
                cmd.Parameters.Add(sp);
            }
            cmd.CommandType = CommandType.StoredProcedure;
            cmds.Add(cmd);

            dt = GetPREBodyDataTable(jo["BodyData"].ToArray(), jo["ccuscode"].ToString());
            dt.TableName = "dl_oppreorderdetail";
            JObject j = new JObject();

            //拼接表体table

            j = sqlhper.SqlTransBulk(cmds.ToArray(), dt, "lngPreOrderId");
            if (j["flag"].ToString() != "1")
            {
                Model.ReInfo ri = new Model.ReInfo();
                ri.list_msg = new List<string>();
                ri.list_msg.Add(DateTime.Now.ToString());
                ri.list_msg.Add(j["ErrMsg"].ToString());
                ri.dt = dt;
                new Check().WriteLog(ri);
            }
            return j;
        }
        #endregion

        #region 拼接特殊订单表体数据
        public DataTable GetPREBodyDataTable(Array BodyData, string kpdw)
        {
            DataTable dt = new DataTable();
            JObject jo = new JObject();
            JObject j = new JObject();
            string sql = "select top 1 * from dl_oppreorderdetail";
            DataTable BaseTable = sqlhper.ExecuteQuery(sql, CommandType.Text);
            BaseTable.Rows.RemoveAt(0);
            int index = 1;
            foreach (var item in BodyData)
            {

                DataRow dr = BaseTable.NewRow();

                j = JObject.Parse(item.ToString());
                dt = new product().DLproc_QuasiOrderDetailBySel(j["cInvCode"].ToString(), kpdw, "0");
                BaseTable.Rows.Add(dr);
                dr["cinvcode"] = j["cInvCode"].ToString();
                dr["iquantity"] = j["cInvDefineQTY"].ToString();
                dr["irowno"] = index;
                index++;
                dr["cinvname"] = j["cInvName"].ToString();
                dr["cComUnitName"] = j["cComUnitName"].ToString();
                dr["cInvDefine1"] = j["cInvDefine1"].ToString();
                dr["cInvDefine2"] = j["cInvDefine2"].ToString();
                dr["cInvDefine13"] = j["cinvdefine13"].ToString();
                dr["cInvDefine14"] = j["cinvdefine14"].ToString();
                dr["UnitGroup"] = j["UnitGroup"].ToString();

                dr["cComUnitQTY"] = j["cComUnitQTY"].ToString();

                dr["cInvDefine1QTY"] = j["cInvDefine1QTY"].ToString();
                dr["cInvDefine2QTY"] = j["cInvDefine2QTY"].ToString();
                dr["cn1cComUnitName"] = j["cn1cComUnitName"].ToString();
                dr["cdefine22"] = j["pack"].ToString();

                dr["cn1cComUnitName"] = dt.Rows[0]["cn1cComUnitName"];
                dr["cunitid"] = dt.Rows[0]["cComUnitCode"];
                double iquantity = Convert.ToDouble(dr["iquantity"].ToString());   //购买货数量
                double iinvexchrate = 1;//换算率 
                double inum = 1;        //辅计量数量   
                //计算销售单位(辅助)的换算率
                if (dr["cn1cComUnitName"].ToString() == dr["cInvDefine1"].ToString())  //大包装单位换算率
                {
                    iinvexchrate = Convert.ToDouble(dr["cInvDefine13"].ToString());
                }
                else if (dr["cn1cComUnitName"].ToString() == dr["cInvDefine2"].ToString()) //小包装单位换算率
                {
                    iinvexchrate = Convert.ToDouble(dr["cInvDefine14"].ToString());
                }
                else  //无换算单位
                {
                    iinvexchrate = 1;//换算率 
                }
                dr["iinvexchrate"] = iinvexchrate;
                inum = Math.Round(iquantity / iinvexchrate, 2);    //辅计量数量 
                dr["inum"] = inum;
                double iquotedprice = Convert.ToDouble(dt.Rows[0]["Quote"].ToString());//报价 保留5位小数,四舍五入
                dr["iquotedprice"] = iquotedprice;//报价 保留5位小数,四舍五入
                double itaxunitprice = Convert.ToDouble(dt.Rows[0]["ExercisePrice"].ToString());//原币含税单价 即执行价格dt.Columns.Add("ExercisePrice"); //基本单位单价(执行价格)   16,保留5位
                dr["itaxunitprice"] = itaxunitprice;
                double isum = Math.Round(itaxunitprice * iquantity, 2);     //原币价税合计    //金额,原币价税合计=原币含税单价*数量,保留2位小数,四舍五入 
                dr["isum"] = isum;
                double itax = Math.Round(isum / 1.17 * 0.17, 2);        //原币税额 ;税额=金额/1.17*0.17 保留2位, 四舍五入    
                dr["itax"] = itax;
                double imoney = Math.Round(isum - itax, 2);      //原币无税金额 =金额-税额,保留2位,四舍五入
                dr["imoney"] = imoney;
                double iunitprice = Math.Round(imoney / iquantity, 6);  //原币无税单价=无税金额/数量,保留5位小数,四舍五入        
                dr["iunitprice"] = iunitprice;
                double idiscount = Math.Round(iquotedprice * iquantity - isum, 2);   //原币折扣额=报价*数量-金额,保留两位
                dr["idiscount"] = idiscount;
                double inatunitprice = iunitprice;//本币无税单价
                dr["inatunitprice"] = inatunitprice;
                double inatmoney = imoney;   //本币无税金额
                dr["inatmoney"] = inatmoney;
                double inattax = itax;     //本币税额 
                dr["inattax"] = inattax;
                double inatsum = isum;     //本币价税合计
                dr["inatsum"] = inatsum;
                double inatdiscount = idiscount;   //本币折扣额 
                dr["inatdiscount"] = inatdiscount;
                double kl = Convert.ToDouble(dt.Rows[0]["Rate"].ToString());          //扣率 
                dr["kl"] = kl;
                double itaxrate = Convert.ToDouble(dt.Rows[0]["iTaxRate"].ToString());    //税率 
                dr["itaxrate"] = itaxrate;
                dr["cDefine24"] = dt.Rows[0]["cDefine24"].ToString() == "无" ? null : "";//活动类型
            }
            return BaseTable;
        }
        #endregion

        #region 获取代客户下单列表


        public DataTable GetBuyOrderList()
        {
            string sql = @"SELECT a.*,b.*,c.cInvStd FROM dL_oporder a
INNER JOIN dl_oporderdetail b 
ON a.lngopOrderId=b.lngopOrderId
INNER JOIN inventory c 
ON b.cinvcode=c.cInvCode
WHERE adminCreateOrderName IS NOT NULL
order by a.lngoporderid desc";
            //sql = "SELECT * FROM dL_oporder";
            return sqlhper.ExecuteQuery(sql,  CommandType.Text);
        }
        #endregion

        #region 代客户下单后作废订单
        public JObject CancelOrder(string lngopOrderId, string AdminlngopUserId, string AdminstrUserName )
        {
            JObject jo = new JObject();
            string sql = "select * from dl_oporder where lngoporderid=@lngopOrderId";
            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@lngopOrderId",lngopOrderId)
            };
            DataTable dt = sqlhper.ExecuteQuery(sql, paras, CommandType.Text);
            if (dt.Rows.Count == 0)
            {
                jo["flag"] = 0;
                jo["message"] = "获取订单出错，请重试或联系管理员！";
                return jo;
            }
            if (dt.Rows[0]["bytStatus"].ToString()!="2")
            {
                jo["flag"] = 0;
                jo["message"] = "该订单状态不为‘待确认’，不能在此处作废！";
                return jo;
            }
            
            if (dt.Rows[0]["adminCreateOrderName"].ToString() != AdminstrUserName)
            {
                jo["flag"] = 0;
                jo["message"] = "只能由创建该订单的工作人员作废此订单！";
                return jo;
            }
           
            string cmdText = " update Dl_opOrder set bytStatus=99,InvalidTime=GETDATE(),InvalidPersonCode=@AdminlngopUserId where lngopOrderId=@lngopOrderId";
            paras = new SqlParameter[]{
            new SqlParameter("@AdminlngopUserId",AdminlngopUserId),
            new SqlParameter("@lngopOrderId",lngopOrderId)
            };
            int res = sqlhper.ExecuteNonQuery(cmdText, paras, CommandType.Text);
            if (res > 0)
            {
                jo["flag"] =1;
                jo["message"] = "作废成功！";
                
            }
            else {
                jo["flag"] = 0;
                jo["message"] = "作废失败，请重试或联系管理员！";
                
            }
            return jo;
        }
        #endregion

        #region 获取数据库表
        public DataTable GetSqlTables(string TableName, string type)
        {
            string sql = @"SELECT d.crdate AS createtime, TableName = case when a.colorder = 1 then d.name 
                   else '' end, 
       TableDes = case when a.colorder = 1 then isnull(f.value, '') 
                     else '' end
FROM syscolumns a 
       inner join sysobjects d 
          on a.id = d.id 
             and d.xtype = 'U' 
             and d.name <> 'sys.extended_properties'
       left join sys.extended_properties   f 
         on a.id = f.major_id 
            and f.minor_id = 0
Where (case when a.colorder = 1 then d.name else '' end) <>''";
            string condition = "";
            if (type == "0")
            {
                condition = " and d.name like '%" + TableName + "%'";
            }
            else
            {
                condition = " and d.name like '" + TableName + "'";
            }
            sql += condition;

            return sqlhper.ExecuteQuery(sql, CommandType.Text);
        }
        #endregion

        #region 获取数据库表详情
        public DataTable GetSqlTableDetail(string TableName)
        {
            string sql = @"SELECT 
    表名       = d.name,
    表说明     =  isnull(f.value,''),
    字段序号   = a.colorder,
    字段名     = a.name,
    标识       = case when COLUMNPROPERTY( a.id,a.name,'IsIdentity')=1 then '√'else '' end,
    主键       = case when exists(SELECT 1 FROM sysobjects where xtype='PK' and parent_obj=a.id and name in (
                     SELECT name FROM sysindexes WHERE indid in( SELECT indid FROM sysindexkeys WHERE id = a.id AND colid=a.colid))) then '√' else '' end,
    类型       = b.name,
    占用字节数 = a.length,
    长度       = COLUMNPROPERTY(a.id,a.name,'PRECISION'),
    小数位数   = isnull(COLUMNPROPERTY(a.id,a.name,'Scale'),0),
    允许空     = case when a.isnullable=1 then '√'else '' end,
    默认值     = isnull(e.text,''),
    字段说明   = isnull(g.[value],'')
FROM 
    syscolumns a
left join 
    systypes b 
on 
    a.xusertype=b.xusertype
inner join 
    sysobjects d 
on 
    a.id=d.id  and d.xtype='U' and  d.name<>'dtproperties'
left join 
    syscomments e 
on 
    a.cdefault=e.id
left join 
sys.extended_properties   g 
on 
    a.id=G.major_id and a.colid=g.minor_id  
left join
sys.extended_properties f
on 
    d.id=f.major_id and f.minor_id=0
where 
    d.name='" + TableName + "'  order by 字段名";


            return sqlhper.ExecuteQuery(sql, CommandType.Text);
        }
        #endregion

        #region 更新数据库表说明
        public JObject UpdateSqlTableDescription(string Description, string TableName, string Column, string ColumnName)
        {
            JObject jo = new JObject();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_updateextendedproperty";
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@name","MS_Description"),
                new SqlParameter("@value",Description),
                new SqlParameter("@level0type","user"),
                new SqlParameter("@level0name","dbo"),
                new SqlParameter("@level1type","table"),
                new SqlParameter("@level1name",TableName),
                new SqlParameter("@level2type",Column),
                new SqlParameter("@level2name",ColumnName)
                };
            cmd.Parameters.AddRange(paras);

            jo = sqlhper.SqlTran(cmd);
            if (jo["flag"].ToString() == "0")
            {
                SqlCommand cmd1 = new SqlCommand();

                cmd1.CommandText = "sp_addextendedproperty";
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd1.Parameters.AddRange(paras);
                jo = new DAL.SQLHelper().SqlTran(cmd1);
            }
            return jo;
        }
        #endregion

        #region 多联工牌查询
        public DataTable GetInfoByEmployeeCode(JArray ja)
        {

            string sql = @"SELECT H.cPsn_Num employeecode,  ''''+H.cPsn_Num cPsn_Num , H.cPsn_Name employeename, H.cDepName strdepartmentname,H.vjobname strtitlename 
FROM hr_hi_person_view H WHERE H.cpsn_num in (0935,'0036','ad.5s')
order BY H.cDept_num,H.vjobname,H.cPsn_Num";
            string s = string.Join(",", ja); ;
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT H.cPsn_Num employeecode,  ''''+H.cPsn_Num cPsn_Num , H.cPsn_Name employeename, H.cDepName strdepartmentname,H.vjobname strtitlename FROM hr_hi_person_view H WHERE H.cpsn_num in (");
            sb.Append(s);
            sb.Append(") order BY H.cDept_num,H.vjobname,H.cPsn_Num");
            return sqlhper.ExecuteQuery(sb.ToString(), CommandType.Text);


        }
        #endregion

        #region  新增发送短信客户分组
        public bool AddAdminCustomerGroup(string groupName, string groupParent, string userId)
        {
            string sql = "insert into dl_opAdminCustomerGroup (groupname,pid,createrid) values(@groupName,@groupParent,@userId)";
            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@groupName",groupName),
                new SqlParameter("@groupParent",groupParent),
                new SqlParameter("@userId",userId),
            };
            int a = sqlhper.ExecuteNonQuery(sql, paras, CommandType.Text);
            bool b = false;
            if (a > 0)
            {
                b = true;
            }
            return b;
        }
        #endregion

        #region 根据分组创建者ID获取所有发送短信客户分组
        public DataTable GetAllAdminCustomerGroupByCreateId(string userId)
        {
            //string sql = "select * from dl_opAdminCustomerGroup where createrId=@userId";
            string sql = @"SELECT a.*,ISNULL(b.groupName ,'根目录') AS parentName FROM dl_opAdminCustomerGroup a 
  LEFT JOIN dl_opAdminCustomerGroup b 
  ON a.pid=b.id  where a.createrId=@userId  ORDER BY id desc";
            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@userId",userId)
            };
            return sqlhper.ExecuteQuery(sql, paras, CommandType.Text);

        }
        #endregion


        #region 删除发送短信的客户分组
        public bool DelAdminCustomerGroupById(string groupId)
        {

            string sql = @"with temp as
                        (select * from dl_opAdminCustomerGroup 
                        where id=@groupId union all select b.* from temp
                        inner join dl_opAdminCustomerGroup b on b.pid=temp.id)
                        select * from temp";
            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@groupId",groupId),
            };
            DataTable dt = sqlhper.ExecuteQuery(sql, paras, CommandType.Text);
            bool b = false;

            JObject jo = new JObject();
            if (dt.Rows.Count == 0)
            {
                return b;
            }

            List<string> list = new List<string>();
            foreach (DataRow item in dt.Rows)
            {
                list.Add(item["id"].ToString());
            }
            string delNum = string.Join(",", list);

            sql = "delete dl_opAdminCustomerGroup where id in ( " + delNum + ")";

            int a = sqlhper.ExecuteNonQuery(sql, CommandType.Text);

            if (a > 0)
            {
                b = true;
            }
            return b;
        }
        #endregion

        #region 编辑发送短信的客户分组
        public bool EditAdminCustomerGroup(string groupId, string groupName, string groupParentId)
        {
            string sql = "update dl_opAdminCustomerGroup set groupName=@groupName,pid=@groupParentId where id=@groupId";
            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@groupId",groupId),
                new SqlParameter("@groupName",groupName),
                new SqlParameter("@groupParentId",groupParentId),
            };
            int a = sqlhper.ExecuteNonQuery(sql, paras, CommandType.Text);
            bool b = false;
            if (a > 0)
            {
                b = true;
            }
            return b;
        }
        #endregion

        #region  根据分组创建者ID获取所有发送短信客户
        public DataTable GetAllAdminCustomerByCreateId(string userId)
        {
            string sql = @"  SELECT a.*,b.pid,b.groupName FROM dbo.dl_opAdminCustomer a
                                    LEFT JOIN dbo.dl_opAdminCustomerGroup b ON a.GroupId=b.id 
                                    where a.createrId=@userId ORDER BY id desc";
            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@userId",userId)
            };
            return sqlhper.ExecuteQuery(sql, paras, CommandType.Text);

        }

        #endregion

        #region 添加发送短信的客户
        public bool AddAdminCustomer(string customerName, string customerPhone, string customerGroupId, string userId)
        {
            string sql = "insert into dl_opAdminCustomer (customerName,customerPhone,groupid,createrid) values(@customerName,@customerPhone,@customerGroupId,@userId)";
            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@customerName",customerName),
                new SqlParameter("@customerPhone",customerPhone),
                new SqlParameter("@customerGroupId",customerGroupId),
                new SqlParameter("@userId",userId),
            };
            int a = sqlhper.ExecuteNonQuery(sql, paras, CommandType.Text);
            bool b = false;
            if (a > 0)
            {
                b = true;
            }
            return b;
        }
        #endregion


        #region 删除发送短信的客户分组
        public bool DelAdminCustomerById(string customerId)
        {

            string sql = @"delete dl_opAdminCustomer where id=@customerId";
            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@customerId",customerId),
            };
            bool b = false;

            JObject jo = new JObject();


            int a = sqlhper.ExecuteNonQuery(sql, paras, CommandType.Text);

            if (a > 0)
            {
                b = true;
            }
            return b;
        }
        #endregion

        #region 编辑发送短信的客户分组
        public bool EditAdminCustomer(string customerId, string customerName, string customerPhone, string customerGroupId)
        {
            string sql = "update dl_opAdminCustomer set customerName=@customerName,customerPhone=@customerPhone,groupId=@customerGroupId where id=@customerId";
            SqlParameter[] paras = new SqlParameter[]{
                new SqlParameter("@customerId",customerId),
                new SqlParameter("@customerName",customerName),
                new SqlParameter("@customerPhone",customerPhone),
                new SqlParameter("@customerGroupId",customerGroupId),
            };
            int a = sqlhper.ExecuteNonQuery(sql, paras, CommandType.Text);
            bool b = false;
            if (a > 0)
            {
                b = true;
            }
            return b;
        }
        #endregion

    }

}
