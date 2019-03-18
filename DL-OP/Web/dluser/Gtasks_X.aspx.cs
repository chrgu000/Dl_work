using BLL;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model;

using System.Configuration;
using System.Runtime.InteropServices;
using UFIDA.U8.MomServiceCommon;
using UFIDA.U8.U8MOMAPIFramework;
using UFIDA.U8.U8APIFramework;
using UFIDA.U8.U8APIFramework.Meta;
using UFIDA.U8.U8APIFramework.Parameter;
using MSXML2;
using System.Web.UI;
using System.Configuration;
using System.Collections;
using System.IO;
using System.Net;
//using ADODB;
using DAL;

public partial class dluser_Gtasks_X : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string strManagers = Session["lngopUserId"].ToString();

            #region 审核通过,bytstatus=4
            object gridOrder = GridOrder;
            if (Request.QueryString["billno"] != null)
            {
                SQLHelper sqlhelp = new SQLHelper();
                AffairSQLHelper asqlhelp = new AffairSQLHelper();
                ADODB.Connection conn = new ADODB.Connection();
                //conn.CommandTimeout = 600;
                //conn.ConnectionTimeout = 600;
                //检测是否已经有专属操作员
                bool d = false;
                string minid = "";
                //插入表头数据    --10.25更新方法,在插入表头数据之后,继续插入表体数据.最后更新dl_oporder表中的U8订单号,cSOCode,并且更新dl_oporder订单状态
                string strBillNo = Request.QueryString["billno"].ToString();
                //bool c = new OrderManager().DLproc_NewYOrderU8ByIns(strBillNo);   //--生成u8预订单TS20170300211
                //bool d = new OrderManager().DLproc_NewYOrderU8_TSByIns(strBillNo);    //生成u8销售订单TS20170300211
                //U8API9003.U8APISoapClient u8api = new U8API9003.U8APISoapClient();  //生成u8销售订单（api），20170608
                #region 20170617本地api生成特殊销售订单
                TSXSDD tsxsdd = new TSXSDD();
                DataTable dtHead = new BLL.OrderManager().DLproc_NewYOrderU8_TSBySel(strBillNo);

                #region U8验证
                U8Login.clsLogin u8Login = new U8Login.clsLogin();
                String sSerial = "";
                String sSubId = System.Web.Configuration.WebConfigurationManager.AppSettings["sSubId"];
                String sYear = "2015";
                //String sDate = "2018-12-10";
                String sDate = DateTime.Now.ToShortDateString();
                String sAccID = System.Web.Configuration.WebConfigurationManager.AppSettings["sAccID"].ToString();
                String sUserID = System.Web.Configuration.WebConfigurationManager.AppSettings["sUserID"];
                String sPassword = System.Web.Configuration.WebConfigurationManager.AppSettings["sPassword"];
                String sServer = System.Web.Configuration.WebConfigurationManager.AppSettings["sServer"];
                if (!u8Login.Login(ref sSubId, ref sAccID, ref sYear, ref sUserID, ref sPassword, ref sDate, ref sServer, ref sSerial))
                {
                    Console.WriteLine("登陆失败，原因：" + u8Login.ShareString);
                    Marshal.FinalReleaseComObject(u8Login);
                    return;
                }
                U8EnvContext envContext = new U8EnvContext();
                envContext.U8Login = u8Login;
                string strConn = envContext.U8Login.UfDbName;
                #endregion


                //本地sql事务
                SqlConnection DBconn = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString);//实例化数据连  
                DBconn.Open();//打开数据库连接  
                SqlCommand command = DBconn.CreateCommand();
                SqlTransaction transaction = null;
                transaction = DBconn.BeginTransaction();
                command.Connection = DBconn;
                command.Transaction = transaction;

                //打开conn
                conn.Open(strConn, "sa", envContext.U8Login.SysPassword, 0);
                //开始事务
                conn.BeginTrans();
                int count = 0;
                try
                {
                    //先生成U8预订单
                    //bool c = new OrderManager().DLproc_NewYOrderU8ByIns(strBillNo);   //--生成u8预订单TS20170300211
                    //List<SqlCommand> cmds = new List<SqlCommand>();
                    //SqlCommand cmd = new SqlCommand();
                    //cmd.CommandText = "DLproc_NewYOrderU8ByIns";
                    //SqlParameter[] pars = new SqlParameter[]{
                    //new SqlParameter("@strBillNo",strBillNo)
                    //};
                    //cmd.Parameters.AddRange(pars);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmds.Add(cmd);
                    //cmd.CommandText = "DLproc_shiwutest1";
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmds.Add(cmd);
                    //bool c = asqlhelp.SqlTrans(cmds);
                    object ob = new object();
                    conn.Execute("DLproc_NewYOrderU8ByIns '" + strBillNo+"'", out ob);

                    ADODB.Recordset rs9 = new ADODB.RecordsetClass();
                    string sql = "SELECT MIN(bb.autoid) 'id' FROM dbo.SA_PreOrderMain aa LEFT JOIN dbo.SA_PreOrderDetails bb ON bb.ID = aa.ID WHERE aa.cCode=REPLACE('" + strBillNo + "','TS','Y')";
                    rs9.Open(sql, conn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockOptimistic, -1);
                    minid = rs9.Fields[0].Value.ToString();
                    //command.CommandText = "DLproc_NewYOrderU8ByIns";
                    //SqlParameter[] pars = new SqlParameter[]{
                    //new SqlParameter("@strBillNo",strBillNo)
                    //};
                    //command.Parameters.AddRange(pars);
                    //command.CommandType = CommandType.StoredProcedure;
                    //count = command.ExecuteNonQuery();

                    //command.CommandText = "SELECT MIN(bb.autoid) 'id' FROM dbo.SA_PreOrderMain aa LEFT JOIN dbo.SA_PreOrderDetails bb ON bb.ID = aa.ID WHERE aa.cCode=REPLACE(@strBillNo,'TS','Y')";
                    //command.CommandType = CommandType.Text;
                    //minid = command.ExecuteScalar().ToString();
                    //for (int i = 0; i < dtHead.Rows.Count; i++)
                    //{
                    //    dtHead.Rows[i]["iaoids"] = Convert.ToString(Convert.ToInt32(minid) + i);
                    //}
                    //api生成U8销售订单
                    //string minid = "";
                    string aa = tsxsdd.AddTSXSDDAPI(dtHead, conn, minid);

                    //command.CommandText = "DLproc_shiwutest1";
                    //count = command.ExecuteNonQuery();
                    //if (!string.IsNullOrEmpty(aa))
                    //{
                    //    //写入错误信息
                    //    bool ee = new OrderManager().DL_ErrByIns(strBillNo, aa);
                    //    d = false;
                    //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + "审核失败，错误说明：" + aa + "！请联系帅哥管理员！');</script>");
                    //}
                    //bool eee = new OrderManager().DLproc_NewOrderU8APIByUpd(strBillNo);
                    if (!string.IsNullOrEmpty(aa))
                    {
                        conn.RollbackTrans();
                        transaction.Rollback();
                        d = false;
                    }
                    else
                    {
                        conn.CommitTrans();
                        transaction.Commit();
                        d = true;
                        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + "审核成功！');</script>");
                    }
                }
                catch
                {
                    conn.RollbackTrans();
                    transaction.Rollback();
                    //trans.Rollback();//如果前面有异常则事务回滚  
                    d = false;
                    //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + "生成正式订单失败！数据已回滚！');</script>");
                }
                finally
                {
                    DBconn.Close();
                    conn.Close();
                }
                #endregion

                //插入表体数据    --插入表头数据时,已经写入!

                //更新dlorder表中该单据状态值为已审核,并且更新DL订单的表头上的正式订单号     --插入表头数据时,已经更新!

                //重新绑定
                DataTable dt = new DataTable();
                dt = new OrderManager().DLproc_UnauditedPreOrderManagersBySel(strManagers, 2);
                GridOrder.DataSource = dt;
                GridOrder.DataBind();

                if (d)
                {
                    /*20151217增加:审核通过后发送短信给顾客*/
                    #region 审核通过后发送短信给顾客 v1.0;2016-01-30 修改
                    // 查询数据库中是否存在该用户，如果有则发送短信，如果没有则提示
                    //DataTable dttel = new LoginManager().LoginSmsByBillNo(strBillNo);   //老式查询方式,
                    DataTable dtcus = new SearchManager().DL_cCusCodeOnPreOrderBillNoBySel(strBillNo); //查找订单对应的顾客登录编码,然后发送短信
                    //DataTable dttel = new GetPhoneNo().GetCustomerPhoneNo(dtcus.Rows[0]["ccuscode"].ToString());
                    DataTable dttel = new GetPhoneNo().GetSubCustomerPhoneNo(dtcus.Rows[0]["lngopUserId"].ToString(), dtcus.Rows[0]["lngopUserExId"].ToString());
                    if (dttel.Rows.Count < 1)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + "已经审核,请在U8中确认！未查找到对应顾客电话号码,审核短信未发送！');</script>");
                        return;
                    }
                    //string telNo = dttel.Rows[0]["cCusPhone"].ToString();
                    int ResultOk = 0; //  记录结果
                    int ResultNo = 0; //  记录结果
                    //获取U8编号
                    DataTable U8strBillNo = new OrderManager().DL_OrderBillNoForU8OrderBillNoByIns(strBillNo);
                    string Result = "";
                    //发送短信
                    #region 商讯·中国 发送短信
                    //短信内容
                    //string SmsTxt = "【多联网上订单】尊敬的客户,您的网单号：" + strBillNo + "正式单号：" + U8strBillNo + "已经通过审核！谢谢您的支持。 ";
                    string SmsTxt = "【多联网上订单】网单号：" + strBillNo + "<br>" + DateTime.Now.ToString("g");
                    //发送
                    //ChinaSms sms = new ChinaSms("duolian", "duolian123");
                    //SendSMS sms = new SendSMS();    //20170425注释，启用新的短信平台发送短信
                    //SendSMS2Customer9003.SendSMS2CustomerSoapClient sms = new SendSMS2Customer9003.SendSMS2CustomerSoapClient();
                    DataTable dtID = new OrderManager().DLproc_OrderBillBySel(strBillNo, 2);
                    QYH9003.WeiXinSoapClient sms = new QYH9003.WeiXinSoapClient();
                    //bool re = sms.SingleSend(telNo, SmsTxt);
                    for (int i = 0; i < dttel.Rows.Count; i++)
                    {
                        //bool re = sms.SingleSend(dttel.Rows[i]["PhoneNo"].ToString(), SmsTxt.ToString());   //20170425注释，启用新的短信平台发送短信
                        //bool re = sms.SendSMS(dttel.Rows[i]["PhoneNo"].ToString(), SmsTxt.ToString());
                        //bool wxmessage = sms.SendQY_Message_Text(dttel.Rows[i]["PhoneNo"].ToString(), "", "", "20", SmsTxt.ToString());
                        //bool re = sms.SendQY_Message_Text(dttel.Rows[i]["PhoneNo"].ToString(), "", "", "20", SmsTxt.ToString());
                        string wxre = sms.SendMsg_TextCard_EncryUrl(dttel.Rows[i]["PhoneNo"].ToString(), "", "", "20", "特殊订单审核", SmsTxt.ToString(), dtID.Rows[0]["ID"].ToString(), dtID.Rows[0]["type"].ToString());
                        bool re = sms.Check_WXRel(wxre);  
                        if (re == false)
                        {
                            ResultNo = ResultNo + 1;
                            Result = Result + dttel.Rows[i]["PhoneNo"].ToString() + ";";
                        }
                        else
                        {
                            ResultOk = ResultOk + 1;
                        }
                    }
                    //bool wxmessageecho = sms.SendQY_Message_Text("15308078836", "", "", "20", SmsTxt.ToString());
                    string wxmessageecho = sms.SendMsg_TextCard_EncryUrl("15308078836", "", "", "20", "特殊订单审核", SmsTxt.ToString(), dtID.Rows[0]["ID"].ToString(), dtID.Rows[0]["type"].ToString());
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + "已经审核,请在U8中确认！成功发送" + ResultOk + "条信息,发送失败" + ResultNo + "条信息,号码:" + Result + "！');</script>");

                    #endregion
                    #endregion
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + "审核失败，数据已回滚！请联系管理员！');</script>");
                }
            }
            #endregion

            #region 驳回,bytstatus=3
            if (Request.QueryString["dbillno"] != null)
            {
                //检测是否已经有专属操作员

                //驳回订单审核,标记状态为3,reject
                string strBillNo = Request.QueryString["dbillno"].ToString();
                bool c = new OrderManager().DL_RejectOrderBillByUpd(strBillNo, strManagers);
                //重新绑定显示
                DataTable dt = new DataTable();
                dt = new OrderManager().DLproc_UnauditedPreOrderManagersBySel(strManagers, 2);
                GridOrder.DataSource = dt;
                GridOrder.DataBind();

                /*20160826增加:审核通过后发送短信给顾客*/
                #region 审核通过后发送短信给顾客 v1.0;2016-01-30 修改
                // 查询数据库中是否存在该用户，如果有则发送短信，如果没有则提示
                //DataTable dttel = new LoginManager().LoginSmsByBillNo(strBillNo);   //老式查询方式,
                DataTable dtcus = new SearchManager().DL_cCusCodeOnPreOrderBillNoBySel(strBillNo); //查找订单对应的顾客登录编码,然后发送短信
                //DataTable dttel = new GetPhoneNo().GetCustomerPhoneNo(dtcus.Rows[0]["ccuscode"].ToString());
                DataTable dttel = new GetPhoneNo().GetSubCustomerPhoneNo(dtcus.Rows[0]["lngopUserId"].ToString(), dtcus.Rows[0]["lngopUserExId"].ToString());
                if (dttel.Rows.Count < 1)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + "已经驳回,待客户接收！未查找到对应顾客电话号码,驳回提示短信未发送！');</script>");
                    return;
                }
                //string telNo = dttel.Rows[0]["cCusPhone"].ToString();
                int ResultOk = 0; //  记录结果
                int ResultNo = 0; //  记录结果
                //获取U8编号
                DataTable U8strBillNo = new OrderManager().DL_OrderBillNoForU8OrderBillNoByIns(strBillNo);
                string Result = "";
                //发送短信
                #region 商讯·中国 发送短信
                //短信内容
                //string SmsTxt = "【多联网上订单】尊敬的客户,您的网单号：" + strBillNo + "已经被驳回，请尽快修正后重新提交订单！谢谢您的支持。 ";
                string SmsTxt = "【多联网上订单】网单号：" + strBillNo + "请尽快修正后重新提交订单! " +"<br>" + DateTime.Now.ToString("g");
                DataTable dtID = new OrderManager().DLproc_OrderBillBySel(strBillNo, 2);
                QYH9003.WeiXinSoapClient sms = new QYH9003.WeiXinSoapClient();
                //发送
                //ChinaSms sms = new ChinaSms("duolian", "duolian123");
                //SendSMS sms = new SendSMS();    //20170425注释，启用新的短信平台发送短信
                //SendSMS2Customer9003.SendSMS2CustomerSoapClient sms = new SendSMS2Customer9003.SendSMS2CustomerSoapClient();
                //bool re = sms.SingleSend(telNo, SmsTxt);
                for (int i = 0; i < dttel.Rows.Count; i++)
                {
                    //bool re = sms.SingleSend(dttel.Rows[i]["PhoneNo"].ToString(), SmsTxt.ToString());   //20170425注释，启用新的短信平台发送短信
                    //bool re = sms.SendSMS(dttel.Rows[i]["PhoneNo"].ToString(), SmsTxt.ToString());
                    //bool wxmessage = sms.SendQY_Message_Text(dttel.Rows[i]["PhoneNo"].ToString(), "", "", "20", SmsTxt.ToString());
                    //bool re = sms.SendQY_Message_Text(dttel.Rows[i]["PhoneNo"].ToString(), "", "", "20", SmsTxt.ToString());
                    string wxre = sms.SendMsg_TextCard_EncryUrl(dttel.Rows[i]["PhoneNo"].ToString(), "", "", "20", "特殊订单驳回", SmsTxt.ToString(), dtID.Rows[0]["ID"].ToString(), dtID.Rows[0]["type"].ToString());
                    bool re = sms.Check_WXRel(wxre);  
                    if (re == false)
                    {
                        ResultNo = ResultNo + 1;
                        Result = Result + dttel.Rows[i]["PhoneNo"].ToString() + ";";
                    }
                    else
                    {
                        ResultOk = ResultOk + 1;
                    }
                }
                //bool wxmessageecho = sms.SendQY_Message_Text("15308078836", "", "", "20", SmsTxt.ToString());
                string wxmessageecho = sms.SendMsg_TextCard_EncryUrl("15308078836", "", "", "20", "特殊订单驳回", SmsTxt.ToString(), dtID.Rows[0]["ID"].ToString(), dtID.Rows[0]["type"].ToString());
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + " 已经驳回,待客户接收！成功发送" + ResultOk + "条信息,发送失败" + ResultNo + "条信息,号码:" + Result + "！');</script>");

                #endregion
                #endregion

                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + "已经驳回,待客户接收！');</script>");
            }
            #endregion

            #region 协助修改,bytstatus=2
            if (Request.QueryString["mbillno"] != null)
            {
                //检测是否已经有专属操作员

                //协助顾客修改订单,订单返回给顾客确认,标记状态为2,reject
                string strBillNo = Request.QueryString["mbillno"].ToString();
                //bool c = new OrderManager().DLproc_ModifyOrderBillByUpd(strBillNo, strManagers);
                //重新绑定显示
                DataTable dt = new DataTable();
                dt = new OrderManager().DLproc_UnauditedPreOrderManagersBySel(strManagers, 2);
                GridOrder.DataSource = dt;
                GridOrder.DataBind();
            }
            #endregion

            #region 作废订单,bytstatus=99
            if (Request.QueryString["Ibillno"] != null)
            {
                //检测是否已经有专属操作员

                //协助顾客修改订单,订单返回给顾客确认,标记状态为99,Invalid
                string strBillNo = Request.QueryString["Ibillno"].ToString();
                bool c = new OrderManager().DL_InvalidYOrderBillByUpd(strBillNo, strManagers, strManagers);
                //重新绑定显示
                DataTable dt = new DataTable();
                dt = new OrderManager().DLproc_UnauditedPreOrderManagersBySel(strManagers, 2);
                GridOrder.DataSource = dt;
                GridOrder.DataBind();
                #region 审核通过后发送短信给顾客 v1.0;2016-01-30 增加
                // 查询数据库中是否存在该用户，如果有则发送短信，如果没有则提示
                //DataTable dttel = new LoginManager().LoginSmsByBillNo(strBillNo);   //老式查询方式,
                DataTable dtcus = new SearchManager().DL_cCusCodeOnPreOrderBillNoBySel(strBillNo); //查找订单对应的顾客登录编码,然后发送短信
                //DataTable dttel = new GetPhoneNo().GetCustomerPhoneNo(dtcus.Rows[0]["ccuscode"].ToString());
                DataTable dttel = new GetPhoneNo().GetSubCustomerPhoneNo(dtcus.Rows[0]["lngopUserId"].ToString(), dtcus.Rows[0]["lngopUserExId"].ToString());
                if (dttel.Rows.Count < 1)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + "已经作废！未查找到对应顾客电话号码,审核短信未发送！');</script>");
                    return;
                }
                //string telNo = dttel.Rows[0]["cCusPhone"].ToString();
                int ResultOk = 0; //  记录结果
                int ResultNo = 0; //  记录结果
                string Result = "";
                //发送短信
                #region 商讯·中国 发送短信
                //短信内容
                //string SmsTxt = "【多联网上订单】尊敬的客户,您的网单号：" + strBillNo + "已取消！客服热线：4008786333！ ";
                string SmsTxt = "【多联网上订单】网单号：" + strBillNo+ "已取消！客服热线：4008786333！"  + "<br>" + DateTime.Now.ToString("g");
                DataTable dtID = new OrderManager().DLproc_OrderBillBySel(strBillNo, 2);
                QYH9003.WeiXinSoapClient sms = new QYH9003.WeiXinSoapClient();
                //发送
                //SendSMS sms = new SendSMS();    //20170425注释，启用新的短信平台发送短信
                //SendSMS2Customer9003.SendSMS2CustomerSoapClient sms = new SendSMS2Customer9003.SendSMS2CustomerSoapClient();
                for (int i = 0; i < dttel.Rows.Count; i++)
                {
                    //bool re = sms.SingleSend(dttel.Rows[i]["PhoneNo"].ToString(), SmsTxt.ToString()); //20170425注释，启用新的短信平台发送短信
                    //bool re = sms.SendSMS(dttel.Rows[i]["PhoneNo"].ToString(), SmsTxt.ToString());
                    //bool wxmessage = sms.SendQY_Message_Text(dttel.Rows[i]["PhoneNo"].ToString(), "", "", "20", SmsTxt.ToString());
                    //bool re = sms.SendQY_Message_Text(dttel.Rows[i]["PhoneNo"].ToString(), "", "", "20", SmsTxt.ToString());
                    string wxre = sms.SendMsg_TextCard_EncryUrl(dttel.Rows[i]["PhoneNo"].ToString(), "", "", "20", "特殊订单取消", SmsTxt.ToString(), dtID.Rows[0]["ID"].ToString(), dtID.Rows[0]["type"].ToString());
                    bool re = sms.Check_WXRel(wxre);  				
                    if (re == false)
                    {
                        ResultNo = ResultNo + 1;
                        Result = Result + dttel.Rows[i]["PhoneNo"].ToString() + ";";
                    }
                    else
                    {
                        ResultOk = ResultOk + 1;
                    }
                }
                //bool wxmessageecho = sms.SendQY_Message_Text("15308078836", "", "", "20", SmsTxt.ToString());
                string wxmessageecho = sms.SendMsg_TextCard_EncryUrl("15308078836", "", "", "20", "特殊订单取消", SmsTxt.ToString(), dtID.Rows[0]["ID"].ToString(), dtID.Rows[0]["type"].ToString());
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + "已经作废！成功发送" + ResultOk + "条信息,发送失败" + ResultNo + "条信息,号码:" + Result + "！');</script>");
                #endregion
                #endregion
            }
            #endregion

            #region 查看订单信息
            if (Request.QueryString["vbillno"] != null)
            {
                //查看订单状态
                string strBillNo = Request.QueryString["vbillno"].ToString();
                DataTable dt = new OrderManager().DL_PreOrderBillBySel(strBillNo);
                //绑定表头字段,text    
                TxtCustomer.Text = dt.Rows[0]["ccusname"].ToString();
                TxtOrderBillNo.Text = strBillNo;
                TxtBillTime.Text = dt.Rows[0]["ddate"].ToString();//下单时间        
                //绑定表体字段,grid
                ViewOrderGrid.DataSource = dt;
                ViewOrderGrid.DataBind();
                //绑定grid
                DataTable dtgrid = new DataTable();
                dtgrid = new OrderManager().DLproc_UnauditedPreOrderManagersBySel(strManagers, 2);
                GridOrder.DataSource = dtgrid;
                GridOrder.DataBind();
            }
            #endregion

        }
    }

    protected void BtnRefresh_Click(object sender, EventArgs e) //刷新,我处理的订单
    {
        DataTable dt = new DataTable();
        string strManagers = Session["lngopUserId"].ToString();
        dt = new OrderManager().DLproc_UnauditedPreOrderManagersBySel(strManagers, 2);
        GridOrder.DataSource = dt;
        GridOrder.DataBind();
    }

    protected void GridOrder_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
    { }



}