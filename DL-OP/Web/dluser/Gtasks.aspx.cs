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
using System.Data.SqlClient;
//using ADODB;


public partial class dluser_Gtasks : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string cSCCode = "%";
        if (RadioButton1.Checked == true)
        {
            cSCCode = "00";
        }
        if (RadioButton2.Checked == true)
        {
            cSCCode = "01";
        }

        DataTable dtr = new OrderManager().DLproc_UnauditedOrderManagers_U20BySel(Session["lngopUserId"].ToString(), 0, cSCCode);
        GridOrder.DataSource = dtr;
        GridOrder.DataBind();

        //获取选中的金额
        double cc = 0;
        for (int ii = 0; ii < GridOrder.VisibleRowCount - 1; ii++)
        {
            if (GridOrder.Selection.IsRowSelected(ii))
            {
                cc = cc + Convert.ToDouble(GridOrder.GetRowValues(ii, "isum").ToString());
            }
        }
        maxsum.Text = cc.ToString();

        #region 清除session(样品资料)
        //清除session(样品资料)
        Session.Remove("SampleOrderModify_StrBillNo");  //单据编号
        Session.Remove("SampleOrderModify_ordergrid");  //表体明细数据
        Session.Remove("SampleOrderModify_gridselect"); //选择的商品明细数据
        Session.Remove("SampleOrderModify_KPDWcode");   //开票单位编码
        Session.Remove("SampleOrderModify_treelistgrid");   //选择树的大类编码      
        #endregion

        #region !IsPostBack
        if (!IsPostBack)
        {
            string strManagers = Session["lngopUserId"].ToString();

            #region 审核通过,bytstatus=4

            #region 20170925,新的数据查询方式,参照特殊订单
            if (Request.QueryString["cztsbillno"] != null)
            {
                string strBillNo = Request.QueryString["cztsbillno"].ToString();
                ADODB.Connection conn = new ADODB.Connection();
                if (strBillNo.Substring(0, 4).ToString() == "CZTS")
                {
                    bool c = true;
                    //20170401直接生成销售订单下的发货单，先判断是否有未审核的销售订单
                    DataTable unSHDD = new SearchManager().DL_CZTSCheckBySel(strBillNo);
                    if (unSHDD.Rows.Count > 0)
                    {
                        string uu = "销售订单:";
                        for (int i = 0; i < unSHDD.Rows.Count; i++)
                        {
                            uu = uu + unSHDD.Rows[i][0].ToString() + ",";
                        }
                        uu = uu + "未审核，请先审核对应的销售订单后再审核参照订单！";
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('" + uu + "');</script>");
                        return;
                    }
                    //生成U8发货单
                    U8Login.clsLogin u8Login = new U8Login.clsLogin();
                    String sSerial = "";
                    String sSubId = System.Web.Configuration.WebConfigurationManager.AppSettings["sSubId"];
                    String sYear = "2015";
                    String sDate = "2018-12-10";
                    //String sAccID = System.Web.Configuration.WebConfigurationManager.AppSettings["sAccID"].ToString();
                    String sAccID = System.Web.Configuration.WebConfigurationManager.AppSettings["sAccID"].Replace("test", "test").ToString();
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
                    //获取需要传入的数据
                    DataTable dtHead = new BLL.OrderManager().DL_NewOrderToDispHU8BySel(strBillNo);
                    DataTable dtBody = new BLL.OrderManager().DL_NewOrderToDispU8_V2BySel(strBillNo, 1);//  非金花，大井
                    DataTable dtBody1 = new BLL.OrderManager().DL_NewOrderToDispU8_V2BySel(strBillNo, 2);//金花
                    DataTable dtBody2 = new BLL.OrderManager().DL_NewOrderToDispU8_V2BySel(strBillNo, 3);//大井
                    string strBillNo1 = strBillNo + "-1";
                    string strBillNo2 = strBillNo + "-2";

                    //本地sql事务
                    SqlConnection DBconn = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString);//实例化数据连  
                    DBconn.Open();//打开数据库连接  
                    SqlCommand command = DBconn.CreateCommand();
                    SqlTransaction transaction = null;
                    transaction = DBconn.BeginTransaction();
                    command.Connection = DBconn;
                    command.Transaction = transaction;
                    //int count = 0;
                    string aa1 = "";
                    string aa2 = "";
                    string aa3 = "";
                    //开启事务
                    conn.Open(strConn, "sa", envContext.U8Login.SysPassword, 0);
                    bool u = true;
                    conn.BeginTrans();
                    try
                    {
                        //插入非金花,大井数据
                        if (dtBody.Rows.Count > 0)
                        {
                            aa1 = new FHD_U8API().AddFHDAPI(dtHead, dtBody, strBillNo, conn);
                        }
                        //插入金花数据
                        if (dtBody1.Rows.Count > 0)
                        {
                            aa2 = new FHD_U8API().AddFHDAPI(dtHead, dtBody1, strBillNo1, conn);
                        }
                        //插入大井数据
                        if (dtBody2.Rows.Count > 0)
                        {
                            aa3 = new FHD_U8API().AddFHDAPI(dtHead, dtBody2, strBillNo2, conn);
                        }
                        object ob = new object();
                        string sql = "UPDATE dbo.Dl_opOrder SET bytStatus=4,datAuditordTime=GETDATE() WHERE strBillNo='" + strBillNo + "'";
                        conn.Execute(sql, out ob);
                        conn.Execute("DLproc_CZTSFHDLSHByUpd '" + strBillNo + "'", out ob);

                        if (!string.IsNullOrEmpty(aa1) || !string.IsNullOrEmpty(aa2) || !string.IsNullOrEmpty(aa3))
                        {
                            conn.RollbackTrans();
                            transaction.Rollback();
                            c = false;
                            bool r = new OrderManager().DL_ErrByIns(strBillNo, aa1 + aa2 + aa3);
                        }
                        else
                        {
                            conn.CommitTrans();
                            transaction.Commit();
                            c = true;
                            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + "审核成功！');</script>");
                        }


                    }
                    catch
                    {
                        conn.RollbackTrans();
                        //trans.Rollback();//如果前面有异常则事务回滚  
                        transaction.Rollback();
                        c = false;
                        bool r = new OrderManager().DL_ErrByIns(strBillNo, aa1 + aa2 + aa3);
                        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + "生成正式发货单失败！数据已回滚！');</script>");
                    }
                    finally
                    {
                        conn.Close();
                        DBconn.Close();
                    }
                    if (!string.IsNullOrEmpty(aa1) || !string.IsNullOrEmpty(aa2) || !string.IsNullOrEmpty(aa3))
                    {
                        bool r = new OrderManager().DL_ErrByIns(strBillNo, aa1 + aa2 + aa3);
                    }
                    if (c == false)
                    {
                        //重新绑定
                        DataTable dtt = new DataTable();
                        int lngBillType1 = 0;
                        dtt = new OrderManager().DLproc_UnauditedOrderManagers_U20BySel(strManagers, lngBillType1, cSCCode);
                        GridOrder.DataSource = dtt;
                        GridOrder.DataBind();
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + "生成正式订单失败,数据已回滚！请联系帅哥管理员！');</script>");
                        return;
                    }

                }
            }
            #endregion

            #region 原始审核
            if (Request.QueryString["billno"] != null)
            {
                string strBillNo = Request.QueryString["billno"].ToString();
                ADODB.Connection conn = new ADODB.Connection();

                #region 2016-03-22,调整,样品资料可以任意关联已做订单,在审核前,需要检测,主订单是否已经通过审核
                if (strBillNo.Substring(0, 2).ToString() == "YP")
                {
                    bool yp = new OrderManager().DL_YPForMainOrderStatusBySel(strBillNo);
                    if (yp == false)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + "关联的主订单未通过审核或已关闭作废,请先审核主订单！');</script>");
                        return;
                    }
                }
                #endregion

                #region 2016-02-22,增加,检测订单中的包装结果是否和数量一致

                #endregion

                //检测是否已经有专属操作员

                ////插入表头数据    --10.25更新方法,在插入表头数据之后,继续插入表体数据.最后更新dl_oporder表中的U8订单号,cSOCode,并且更新dl_oporder订单状态
                //string strBillNo = Request.QueryString["billno"].ToString();
                //bool c = new OrderManager().DLproc_NewOrderU8ByIns(strBillNo);
                ////插入表体数据    --插入表头数据时,已经写入!

                ////更新dlorder表中该单据状态值为已审核,并且更新DL订单的表头上的正式订单号     --插入表头数据时,已经更新!
                /*
                ////1107更新,审核,将bytStatus状态更新为2,待用户确认后生成U8订单
                //string strBillNo = Request.QueryString["billno"].ToString();
                //bool c = new OrderManager().DL_CheckOrderBillByUpd(strBillNo);    
                */
                //1115修改,订单专员直接审核通过,生成U8订单,DL订单状态改为2,
                //插入表头数据    --10.25更新方法,在插入表头数据之后,继续插入表体数据.最后更新dl_oporder表中的U8订单号,cSOCode,并且更新dl_oporder订单状态
                //20170116,添加，如果是参照特殊订单审核，则直接关闭，否则，继续按以前流程处理
                bool c = true;

                //20170925启用新的czts生成发货单的查询数据处理,查找实际的预留库存,并自动分配仓库数量,操作员的审核czts调用该方法


                if (strBillNo.Substring(0, 4).ToString() == "CZTS")
                {
                    //更新参照特殊订单,关闭,并提示
                    //bool u = new SearchManager().DL_CZTSCloseByUpd(strBillNo); //20170401关闭，直接生成销售订单下的发货单

                    //20170401直接生成销售订单下的发货单，先判断是否有未审核的销售订单
                    DataTable unSHDD = new SearchManager().DL_CZTSCheckBySel(strBillNo);
                    if (unSHDD.Rows.Count > 0)
                    {
                        string uu = "销售订单:";
                        for (int i = 0; i < unSHDD.Rows.Count; i++)
                        {
                            uu = uu + unSHDD.Rows[i][0].ToString() + ",";
                        }
                        uu = uu + "未审核，请先审核对应的销售订单后再审核参照订单！";
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('" + uu + "');</script>");
                        return;
                    }

                    #region api事务之前的生成U8发货单方式
                    ////生成U8发货单
                    ////bool u = new SearchManager().DLproc_NewOrderToDispU8ByIns(strBillNo);
                    ////if (u == false)
                    ////{
                    ////    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + "审核关闭失败，请联系帅哥管理员！');</script>");
                    ////    return;
                    ////}
                    //bool u = true;
                    //string aa = new FHD_U8API().AddFHD(strBillNo);
                    //if (!string.IsNullOrEmpty(aa))
                    //{
                    //    u = false;
                    //    if (aa.Length > 30)
                    //    {
                    //        aa = aa.Substring(0, 30).ToString();
                    //    }
                    //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + "审核失败，错误说明：" + aa + "！请手工删除U8系统中已经生成的发货单！请联系帅哥管理员！');</script>");
                    //    return;
                    //}
                    ////审核完成之后，更新U8发货单流水号(发货单表头扩展表)
                    //bool fhdlsh = new OrderManager().DLproc_CZTSFHDLSHByUpd(strBillNo);
                    #endregion

                    U8Login.clsLogin u8Login = new U8Login.clsLogin();
                    String sSerial = "";
                    String sSubId = System.Web.Configuration.WebConfigurationManager.AppSettings["sSubId"];
                    String sYear = "2015";
                    String sDate = "2018-12-10";
                    //String sAccID = System.Web.Configuration.WebConfigurationManager.AppSettings["sAccID"].ToString();
                    String sAccID = System.Web.Configuration.WebConfigurationManager.AppSettings["sAccID"].Replace("test", "test").ToString();
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
                    //获取需要传入的数据
                    DataTable dtHead = new BLL.OrderManager().DL_NewOrderToDispHU8BySel(strBillNo);
                    DataTable dtBody = new BLL.OrderManager().DL_NewOrderToDispBU8BySel(strBillNo);//  非金花，大井
                    DataTable dtBody1 = new BLL.OrderManager().DL_NewOrderToDispB_JH_U8BySel(strBillNo);//金花
                    DataTable dtBody2 = new BLL.OrderManager().DL_NewOrderToDispB_DJ_U8BySel(strBillNo);//大井
                    string strBillNo1 = strBillNo + "-1";
                    string strBillNo2 = strBillNo + "-2";

                    //本地sql事务
                    SqlConnection DBconn = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString);//实例化数据连  
                    DBconn.Open();//打开数据库连接  
                    SqlCommand command = DBconn.CreateCommand();
                    SqlTransaction transaction = null;
                    transaction = DBconn.BeginTransaction();
                    command.Connection = DBconn;
                    command.Transaction = transaction;
                    //int count = 0;
                    string aa1 = "";
                    string aa2 = "";
                    string aa3 = "";
                    //开启事务
                    conn.Open(strConn, "sa", envContext.U8Login.SysPassword, 0);
                    bool u = true;
                    conn.BeginTrans();
                    try
                    {
                        //插入非金花,大井数据
                        if (dtBody.Rows.Count > 0)
                        {
                            aa1 = new FHD_U8API().AddFHDAPI(dtHead, dtBody, strBillNo, conn);
                        }
                        //插入金花数据
                        if (dtBody1.Rows.Count > 0)
                        {
                            aa2 = new FHD_U8API().AddFHDAPI(dtHead, dtBody1, strBillNo1, conn);
                        }
                        //插入大井数据
                        if (dtBody2.Rows.Count > 0)
                        {
                            aa3 = new FHD_U8API().AddFHDAPI(dtHead, dtBody2, strBillNo2, conn);
                        }

                        //string aa = new FHD_U8API().AddFHD(strBillNo,conn);
                        //if (!string.IsNullOrEmpty(aa))
                        //{
                        //    u = false;
                        //    if (aa.Length > 30)
                        //    {
                        //        aa = aa.Substring(0, 30).ToString();
                        //    }
                        //    conn.RollbackTrans();
                        //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + "审核失败,数据已回滚，错误说明：" + aa + "！请手工删除U8系统中已经生成的发货单！请联系帅哥管理员！');</script>");
                        //    //return;
                        //}
                        //审核完成之后，更新U8发货单流水号(发货单表头扩展表)
                        //command.CommandText = "DLproc_CZTSFHDLSHByUpd";
                        //SqlParameter[] pars = new SqlParameter[]{
                        //new SqlParameter("@strBillNo",strBillNo)
                        //};
                        //command.Parameters.AddRange(pars);
                        //command.CommandType = CommandType.StoredProcedure;
                        //count = command.ExecuteNonQuery();
                        //bool fhdlsh = new OrderManager().DLproc_CZTSFHDLSHByUpd(strBillNo);
                        object ob = new object();
                        string sql = "UPDATE dbo.Dl_opOrder SET bytStatus=4,datAuditordTime=GETDATE() WHERE strBillNo='" + strBillNo + "'";
                        conn.Execute(sql, out ob);
                        conn.Execute("DLproc_CZTSFHDLSHByUpd '" + strBillNo + "'", out ob);

                        if (!string.IsNullOrEmpty(aa1) || !string.IsNullOrEmpty(aa2) || !string.IsNullOrEmpty(aa3))
                        {
                            conn.RollbackTrans();
                            transaction.Rollback();
                            c = false;
                            bool r = new OrderManager().DL_ErrByIns(strBillNo, aa1 + aa2 + aa3);
                        }
                        else
                        {
                            conn.CommitTrans();
                            transaction.Commit();
                            c = true;
                            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + "审核成功！');</script>");
                        }


                    }
                    catch
                    {
                        conn.RollbackTrans();
                        //trans.Rollback();//如果前面有异常则事务回滚  
                        transaction.Rollback();
                        c = false;
                        bool r = new OrderManager().DL_ErrByIns(strBillNo, aa1 + aa2 + aa3);
                        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + "生成正式发货单失败！数据已回滚！');</script>");
                    }
                    finally
                    {
                        conn.Close();
                        DBconn.Close();
                    }
                    if (!string.IsNullOrEmpty(aa1) || !string.IsNullOrEmpty(aa2) || !string.IsNullOrEmpty(aa3))
                    {
                        bool r = new OrderManager().DL_ErrByIns(strBillNo, aa1 + aa2 + aa3);
                    }
                    if (c == false)
                    {
                        //重新绑定
                        DataTable dtt = new DataTable();
                        int lngBillType1 = 0;
                        //dt = new OrderManager().DLproc_UnauditedOrderManagersBySel(strManagers, lngBillType);
                        dtt = new OrderManager().DLproc_UnauditedOrderManagers_U20BySel(strManagers, lngBillType1, cSCCode);
                        GridOrder.DataSource = dtt;
                        GridOrder.DataBind();
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + "生成正式订单失败,数据已回滚！请联系帅哥管理员！');</script>");
                        return;
                    }

                }
                else
                {
                    //bool c = new OrderManager().DLproc_NewOrderU8ByIns(strBillNo);  //普通销售订单,老方法,插入sql，
                    #region 20170516新方法，调用9003u8api
                    //SendSMS2Customer9003.SendSMS2CustomerSoapClient sms = new SendSMS2Customer9003.SendSMS2CustomerSoapClient();
                    //U8API9003.U8APISoapClient u8api = new U8API9003.U8APISoapClient();
                    //bool c = true;
                    ////先判断是否已经生成了w的订单，如果没有就调用api，如果有，就更新对应网上订单的状态为已审核
                    //bool g = new OrderManager().DL_IsExistsDLBySel(strBillNo);
                    //if (g)
                    //{
                    //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + "已经在U8系统中存在相同编号的订单，已经更新了网上订单状态！');</script>");
                    //    return;
                    //}
                    //string U8XSDD_rel = u8api.U8XSDD_API(strBillNo);

                    //if (!string.IsNullOrEmpty(U8XSDD_rel)) //错误，写入错误日志
                    //{
                    //    //写入错误信息
                    //    bool d = new OrderManager().DL_ErrByIns(strBillNo, U8XSDD_rel);
                    //    c = false;
                    //}
                    //else        //成功调用u8api,更新后续数据即单据状态
                    //{
                    //    bool c_U8API_Upd = new OrderManager().DLproc_NewOrderU8APIByUpd(strBillNo);
                    //}
                    #endregion

                    #region 20170617新方法，调用类，并开启事务
                    //先判断是否已经生成了w的订单，如果没有就调用api，如果有，就更新对应网上订单的状态为已审核,DL_IsExistsDLBySel
                    //bool g = new OrderManager().DL_IsExistsDLBySel(strBillNo);
                    //if (g)
                    //{
                    //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + "已经在U8系统中存在相同编号的订单，已经更新了网上订单状态！');</script>");
                    //    return;
                    //}

                    U8Login.clsLogin u8Login = new U8Login.clsLogin();
                    String sSerial = "";
                    String sSubId = System.Web.Configuration.WebConfigurationManager.AppSettings["sSubId"];
                    String sYear = "2015";
                    String sDate = "2018-12-10";
                    //String sAccID = System.Web.Configuration.WebConfigurationManager.AppSettings["sAccID"].ToString();
                    String sAccID = System.Web.Configuration.WebConfigurationManager.AppSettings["sAccID"].Replace("test", "test").ToString();
                    String sUserID = System.Web.Configuration.WebConfigurationManager.AppSettings["sUserID"];
                    String sPassword = System.Web.Configuration.WebConfigurationManager.AppSettings["sPassword"];
                    String sServer = System.Web.Configuration.WebConfigurationManager.AppSettings["sServer"];
                    if (!u8Login.Login(ref sSubId, ref sAccID, ref sYear, ref sUserID, ref sPassword, ref sDate, ref sServer, ref sSerial))
                    {
                        Console.WriteLine("登陆失败，原因：" + u8Login.ShareString);
                        Marshal.FinalReleaseComObject(u8Login);
                        return;
                    }
                    //如果已经存在相同编号销售订单则退出
                    //conn.Execute("select * from so_somain where csocode='W"+strBillNo.Replace("DL","").Replace("YP","").ToString()+"'",out ob);
                    bool U8Order_IsExists = new BLL.OrderManager().DL_SO_IsExistsBySel("W"+strBillNo.Replace("DL","").Replace("YP","").ToString());
                    if (U8Order_IsExists)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + "已经存在U8销售订单模块,审核失败,请检查！');</script>");
                        return;
                    }
                    U8EnvContext envContext = new U8EnvContext();
                    envContext.U8Login = u8Login;
                    string strConn = envContext.U8Login.UfDbName;
                    string U8XSDD_rel = "";
                    //本地sql事务
                    SqlConnection DBconn = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString);//实例化数据连  
                    DBconn.Open();//打开数据库连接  
                    SqlCommand command = DBconn.CreateCommand();
                    SqlTransaction transaction = null;
                    transaction = DBconn.BeginTransaction();
                    command.Connection = DBconn;
                    command.Transaction = transaction;
                    //int count = 0;
                    DataTable dtHead = new DataTable();
                    conn.Open(strConn, "sa", envContext.U8Login.SysPassword, 0);
                    conn.BeginTrans();
                    dtHead = new BLL.OrderManager().DLproc_NewOrderU8BySel(strBillNo);
                    //bool r1 = new OrderManager().DL_ErrByIns(strBillNo, dtHead.Rows.Count.ToString());
                    //bool r2 = new OrderManager().DL_ErrByIns(strBillNo, conn.ConnectionString.ToString());
                    try
                    {
                        object ob = new object();
                        XSDD xsdd = new XSDD();
                        U8XSDD_rel = xsdd.AddXSDDAPI(dtHead, conn);
                        //if (!string.IsNullOrEmpty(U8XSDD_rel)) //错误，写入错误日志
                        //{
                        //    //写入错误信息
                        //    conn.RollbackTrans();
                        //    bool d = new OrderManager().DL_ErrByIns(strBillNo, U8XSDD_rel);
                        //    c = false;
                        //}
                        //else        //成功调用u8api,更新后续数据即单据状态
                        //{
                        //    bool c_U8API_Upd = new OrderManager().DLproc_NewOrderU8APIByUpd(strBillNo);
                        //}
                        ////成功调用u8api,更新后续数据即单据状态

                        //command.CommandText = "DLproc_NewOrderU8APIByUpd";
                        //SqlParameter[] pars = new SqlParameter[]{
                        //new SqlParameter("@strBillNo",strBillNo)
                        //};
                        //command.Parameters.AddRange(pars);
                        //command.CommandType = CommandType.StoredProcedure;
                        //count = command.ExecuteNonQuery();

                        //object ob = new object();
                        conn.Execute("DLproc_NewOrderU8APIByUpd '" + strBillNo + "'", out ob);

                        if (!string.IsNullOrEmpty(U8XSDD_rel))
                        {
                            conn.RollbackTrans();
                            transaction.Rollback();
                            c = false;
                            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('11111111111111111！');</script>");
                        }
                        else
                        {
                            conn.CommitTrans();
                            //transaction.Commit();
                            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + "审核成功！');</script>");
                        }
                    }
                    catch (System.Exception exa)
                    {
                        conn.RollbackTrans();
                        //trans.Rollback();//如果前面有异常则事务回滚  
                        transaction.Rollback();
                        c = false;
                        bool r = new OrderManager().DL_ErrByIns(strBillNo, exa.ToString());
                        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('" + exa + "');</script>");
                        //throw exa;
                        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + "生成正式订单失败！数据已回滚！');</script>");
                    }
                    finally
                    {
                        conn.Close();
                        DBconn.Close();
                    }
                    if (!string.IsNullOrEmpty(U8XSDD_rel))
                    {
                        bool r = new OrderManager().DL_ErrByIns(strBillNo, U8XSDD_rel);
                    }
                    #endregion

                    if (c == false)
                    {                   
                        //重新绑定
                        DataTable dtt = new DataTable();
                        int lngBillType1 = 0;
                        //dt = new OrderManager().DLproc_UnauditedOrderManagersBySel(strManagers, lngBillType);
                        dtt = new OrderManager().DLproc_UnauditedOrderManagers_U20BySel(strManagers, lngBillType1, cSCCode);
                        GridOrder.DataSource = dtt;
                        GridOrder.DataBind();
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + "生成正式订单失败,数据已回滚！请联系帅哥管理员！');</script>");
                        return;
                    }
                    else
                    {
                        //检测销售订单数据
                        //20180813,检测销售订单与U8订单的行数是否一致,金额是否一致
                        U8API9003_Check.CheckSoapClient u8api_check = new U8API9003_Check.CheckSoapClient();
                        string order_check = u8api_check.Check_OpOrder_Num("W" + strBillNo.Replace("DL", "").Replace("YP", "").ToString());
                        if (order_check == "")
                        {

                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + "生成U8订单成功,但是检测出数据不符,请联系系统管理员检查！');</script>");
                        }
                    }
                }
            #endregion

                //查询对应的U8订单号
                //20170116注释 DataTable u8csocode = new OrderManager().DL_OrderBillNoForU8OrderBillNoByIns(strBillNo);
                //插入表体数据    --插入表头数据时,已经写入!
                //更新dlorder表中该单据状态值为已审核,并且更新DL订单的表头上的正式订单号     --插入表头数据时,已经更新!

                //重新绑定
                DataTable dt = new DataTable();
                int lngBillType = 0;
                //dt = new OrderManager().DLproc_UnauditedOrderManagersBySel(strManagers, lngBillType);
                dt = new OrderManager().DLproc_UnauditedOrderManagers_U20BySel(strManagers, lngBillType, cSCCode);
                GridOrder.DataSource = dt;
                GridOrder.DataBind();

                #region 快速定位到某一页
                DataTable dtstrbillno = new OrderManager().DLproc_UnauditedOrderManagers_U20_strbillnoBySel(strManagers, lngBillType, cSCCode, strBillNo);
                object oValue = null;
                string sKeyFieldName = "strBillNo";
                string sKeyFieldValue = dtstrbillno.Rows[0][0].ToString();
                for (int i = 0; i < GridOrder.VisibleRowCount; i++)
                {
                    oValue = GridOrder.GetRowValues(i, sKeyFieldName);
                    if (oValue != null && oValue.ToString() == sKeyFieldValue)
                    {
                        GridOrder.SettingsBehavior.AllowFocusedRow = false;
                        GridOrder.SettingsBehavior.AllowFocusedRow = true;
                        GridOrder.FocusedRowIndex = -1;
                        GridOrder.Selection.SelectRow(i);
                        bool bBackToPage = GridOrder.MakeRowVisible(sKeyFieldValue);// 可以定位回到某一页去  
                        break;
                    }
                }
                #endregion

                /*20151217增加:审核通过后发送短信给顾客*/

                #region 审核通过后发送短信给顾客 v1.0;2016-01-30 修改
                // 查询数据库中是否存在该用户，如果有则发送短信，如果没有则提示
                //DataTable dttel = new LoginManager().LoginSmsByBillNo(strBillNo);   //老式查询方式,
                DataTable dtcus = new SearchManager().DL_cCusCodeOnOrderBillNoBySel(strBillNo); //查找订单对应的顾客登录编码,然后发送短信
                //DataTable dttel = new GetPhoneNo().GetSubCustomerPhoneNo(dtcus.Rows[0]["ccuscode"].ToString());
                DataTable dttel = new GetPhoneNo().GetSubCustomerPhoneNo(dtcus.Rows[0]["lngopUserId"].ToString(), dtcus.Rows[0]["lngopUserExId"].ToString());
                if (dttel.Rows.Count < 1)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + "已经审核,请在U8中确认！未查找到对应顾客电话号码,审核短信未发送！');</script>");
                    return;
                }
                //string telNo = dttel.Rows[0]["cCusPhone"].ToString();
                int ResultOk = 0; //  记录结果
                int ResultNo = 0; //  记录结果
                string Result = "";
                //发送短信
                #region 商讯·中国 发送短信
                //短信内容
                /*//20170116注释
                string u8billno = "";
                if (u8csocode.Rows.Count > 0)
                {
                    u8billno = u8csocode.Rows[0]["cSOCode"].ToString();
                }
                */
                //string SmsTxt = "【多联网上订单】尊敬的客户,您的网单号：" + strBillNo + " 已经通过审核！谢谢您的支持。 ";
                string SmsTxt = "【多联网上订单】网单号：" + strBillNo + "<br>" + DateTime.Now.ToString("g");
                DataTable dtID = new OrderManager().DLproc_OrderBillBySel(strBillNo, 1);
                //#region 微信企业号发送信息，应用id20，
                //string wxrel = new SendSMS2Customer9001.SendSMS2CustomerSoapClient().SendQY_Message_Text("", "", "11", "20", "【会议演示】尊敬的客户,您的网单号：" + strBillNo + " 已经通过审核，我们将尽快为您安排发货！谢谢您的支持。【多联公司】 ");
                //#endregion

                ///////发送
                //ChinaSms sms = new ChinaSms("duolian", "duolian123");
                //SendSMS sms = new SendSMS(); //20170425注释，启用新的短信平台发送短信
                //SendSMS2Customer9003.SendSMS2CustomerSoapClient sms = new SendSMS2Customer9003.SendSMS2CustomerSoapClient();
                QYH9003.WeiXinSoapClient sms = new QYH9003.WeiXinSoapClient();
                //bool re = sms.SingleSend(telNo, SmsTxt);
                for (int i = 0; i < dttel.Rows.Count; i++)
                {
                    //bool re = sms.SingleSend(dttel.Rows[i]["PhoneNo"].ToString(), SmsTxt.ToString());   //20170425注释，启用新的短信平台发送短信
                    //bool re = sms.SendSMS(dttel.Rows[i]["PhoneNo"].ToString(), SmsTxt.ToString());
                    //bool wxmessage = sms.SendQY_Message_Text(dttel.Rows[i]["PhoneNo"].ToString(), "", "", "20", SmsTxt.ToString());
                    //bool re = sms.SendQY_Message_Text(dttel.Rows[i]["PhoneNo"].ToString(), "", "", "20", SmsTxt.ToString());
                    string wxre = sms.SendMsg_TextCard_EncryUrl(dttel.Rows[i]["PhoneNo"].ToString(), "", "", "20", "订单审核", SmsTxt.ToString(), dtID.Rows[0]["ID"].ToString(), dtID.Rows[0]["type"].ToString());
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
                string wxmessageecho = sms.SendMsg_TextCard_EncryUrl("15308078836", "", "", "20", "订单审核", SmsTxt.ToString(), dtID.Rows[0]["ID"].ToString(), dtID.Rows[0]["type"].ToString());


                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + "已经审核,请在U8中确认！成功发送" + ResultOk + "条信息,发送失败" + ResultNo + "条信息,号码:" + Result + "！');</script>");


                #endregion

                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + "已经审核,请在U8中确认！');</script>");
            }
                #endregion

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
                int lngBillType = 0;
                //dt = new OrderManager().DLproc_UnauditedOrderManagersBySel(strManagers, lngBillType);
                dt = new OrderManager().DLproc_UnauditedOrderManagers_U20BySel(strManagers, lngBillType, cSCCode);
                GridOrder.DataSource = dt;
                GridOrder.DataBind();

                #region 快速定位到某一页
                DataTable dtstrbillno = new OrderManager().DLproc_UnauditedOrderManagers_U20_strbillnoBySel(strManagers, lngBillType, cSCCode, strBillNo);
                object oValue = null;
                string sKeyFieldName = "strBillNo";
                string sKeyFieldValue = dtstrbillno.Rows[0][0].ToString();
                for (int i = 0; i < GridOrder.VisibleRowCount; i++)
                {
                    oValue = GridOrder.GetRowValues(i, sKeyFieldName);
                    if (oValue != null && oValue.ToString() == sKeyFieldValue)
                    {
                        GridOrder.SettingsBehavior.AllowFocusedRow = false;
                        GridOrder.SettingsBehavior.AllowFocusedRow = true;
                        GridOrder.FocusedRowIndex = -1;
                        GridOrder.Selection.SelectRow(i);
                        bool bBackToPage = GridOrder.MakeRowVisible(sKeyFieldValue);// 可以定位回到某一页去  
                        break;
                    }
                }
                #endregion

                #region 驳回后发送短信给顾客 v1.0;2016-01-30 修改
                // 查询数据库中是否存在该用户，如果有则发送短信，如果没有则提示
                DataTable dtcus = new SearchManager().DL_cCusCodeOnOrderBillNoBySel(strBillNo); //查找订单对应的顾客登录编码,然后发送短信
                //DataTable dttel = new GetPhoneNo().GetCustomerPhoneNo(dtcus.Rows[0]["ccuscode"].ToString());    //获取绑定的手机号码
                DataTable dttel = new GetPhoneNo().GetSubCustomerPhoneNo(dtcus.Rows[0]["lngopUserId"].ToString(), dtcus.Rows[0]["lngopUserExId"].ToString());
                if (dttel.Rows.Count < 1)   //如果没有手机号码提示
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + "已经驳回,待客户接收！未查找到对应顾客电话号码,驳回提示短信未发送！');</script>");
                    return;
                }
                int ResultOk = 0; //  记录结果
                int ResultNo = 0; //  记录结果
                string Result = "";
                //发送短信
                #region 商讯·中国 发送短信
                //短信内容
                //string SmsTxt = "【多联网上订单】尊敬的客户,您的网单号：" + strBillNo + "已经被驳回，请尽快修正后重新提交订单！谢谢您的支持。 ";
                string SmsTxt = "【多联网上订单】网单号：" + strBillNo + ",请尽快修改提交!" + "<br>" + DateTime.Now.ToString("g");
                //发送
                //ChinaSms sms = new ChinaSms("duolian", "duolian123");
                //SendSMS sms = new SendSMS();    //20170425注释，启用新的短信平台发送短信
                //SendSMS2Customer9003.SendSMS2CustomerSoapClient sms = new SendSMS2Customer9003.SendSMS2CustomerSoapClient();
                QYH9003.WeiXinSoapClient sms = new QYH9003.WeiXinSoapClient();
                DataTable dtID = new OrderManager().DLproc_OrderBillBySel(strBillNo, 1);
                //bool re = sms.SingleSend(telNo, SmsTxt);
                for (int i = 0; i < dttel.Rows.Count; i++)
                {
                    //bool re = sms.SingleSend(dttel.Rows[i]["PhoneNo"].ToString(), SmsTxt.ToString());   //20170425注释，启用新的短信平台发送短信
                    //bool re = sms.SendSMS(dttel.Rows[i]["PhoneNo"].ToString(), SmsTxt.ToString());
                    //bool wxmessage = sms.SendQY_Message_Text(dttel.Rows[i]["PhoneNo"].ToString(), "", "", "20", SmsTxt.ToString());                   
                    //bool re = sms.SendQY_Message_Text(dttel.Rows[i]["PhoneNo"].ToString(), "", "", "20", SmsTxt.ToString());
                    string wxre = sms.SendMsg_TextCard_EncryUrl(dttel.Rows[i]["PhoneNo"].ToString(), "", "", "20", "订单驳回", SmsTxt.ToString(), dtID.Rows[0]["ID"].ToString(), dtID.Rows[0]["type"].ToString());
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
                string wxmessageecho = sms.SendMsg_TextCard_EncryUrl("15308078836", "", "", "20", "订单驳回", SmsTxt.ToString(), dtID.Rows[0]["ID"].ToString(), dtID.Rows[0]["type"].ToString());
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + " 已经驳回,待客户接收！成功发送" + ResultOk + "条信息,发送失败" + ResultNo + "条信息,号码:" + Result + "！');</script>");

                #endregion
                #endregion

                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + "已经驳回,待客户接收！');</script>");
            }
            #endregion

            #region 限时驳回,bytstatus=3
            if (Request.QueryString["xdbillno"] != null)
            {
                //检测是否已经有专属操作员

                //驳回订单审核,标记状态为3,reject
                string strBillNo = Request.QueryString["xdbillno"].ToString();
                int datExpTime = Convert.ToInt32(ConfigurationManager.AppSettings["datExpTime"].ToString());
                bool c = new OrderManager().DL_RejectOrderBillForExpTimeByUpd(strBillNo, strManagers, datExpTime);
                //重新绑定显示
                DataTable dt = new DataTable();
                int lngBillType = 0;
                //dt = new OrderManager().DLproc_UnauditedOrderManagersBySel(strManagers, lngBillType);
                dt = new OrderManager().DLproc_UnauditedOrderManagers_U20BySel(strManagers, lngBillType, cSCCode);
                GridOrder.DataSource = dt;
                GridOrder.DataBind();

                #region 快速定位到某一页
                DataTable dtstrbillno = new OrderManager().DLproc_UnauditedOrderManagers_U20_strbillnoBySel(strManagers, lngBillType, cSCCode, strBillNo);
                object oValue = null;
                string sKeyFieldName = "strBillNo";
                string sKeyFieldValue = dtstrbillno.Rows[0][0].ToString();
                for (int i = 0; i < GridOrder.VisibleRowCount; i++)
                {
                    oValue = GridOrder.GetRowValues(i, sKeyFieldName);
                    if (oValue != null && oValue.ToString() == sKeyFieldValue)
                    {
                        GridOrder.SettingsBehavior.AllowFocusedRow = false;
                        GridOrder.SettingsBehavior.AllowFocusedRow = true;
                        GridOrder.FocusedRowIndex = -1;
                        GridOrder.Selection.SelectRow(i);
                        bool bBackToPage = GridOrder.MakeRowVisible(sKeyFieldValue);// 可以定位回到某一页去  
                        break;
                    }
                }
                #endregion

                #region 限时驳回后发送短信给顾客 v1.0;2016-01-30 修改
                // 查询数据库中是否存在该用户，如果有则发送短信，如果没有则提示
                DataTable dtcus = new SearchManager().DL_cCusCodeOnOrderBillNoBySel(strBillNo); //查找订单对应的顾客登录编码,然后发送短信
                //DataTable dttel = new GetPhoneNo().GetCustomerPhoneNo(dtcus.Rows[0]["ccuscode"].ToString());    //获取绑定的手机号码
                DataTable dttel = new GetPhoneNo().GetSubCustomerPhoneNo(dtcus.Rows[0]["lngopUserId"].ToString(), dtcus.Rows[0]["lngopUserExId"].ToString());
                if (dttel.Rows.Count < 1)   //如果没有手机号码提示
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + "已经驳回,待客户接收！未查找到对应顾客电话号码,驳回提示短信未发送！');</script>");
                    return;
                }
                int ResultOk = 0; //  记录结果
                int ResultNo = 0; //  记录结果
                string Result = "";
                //发送短信
                #region 商讯·中国 发送短信
                //短信内容
                //string SmsTxt = "【多联网上订单】尊敬的客户,您的网单号：" + strBillNo + "已经被驳回，" + datExpTime + "分钟后将自动过期，请尽快修正后重新提交订单！ ";
                string SmsTxt = "【多联网上订单】网单号：" + strBillNo + "已经被驳回，" + datExpTime + "分钟后将自动过期" + "<br>" + DateTime.Now.ToString("g");
                //发送
                //ChinaSms sms = new ChinaSms("duolian", "duolian123");
                //SendSMS sms = new SendSMS();    //20170425注释，启用新的短信平台发送短信
                //SendSMS2Customer9003.SendSMS2CustomerSoapClient sms = new SendSMS2Customer9003.SendSMS2CustomerSoapClient();
                QYH9003.WeiXinSoapClient sms = new QYH9003.WeiXinSoapClient();
                DataTable dtID = new OrderManager().DLproc_OrderBillBySel(strBillNo, 1);
                //bool re = sms.SingleSend(telNo, SmsTxt);
                for (int i = 0; i < dttel.Rows.Count; i++)
                {
                    //bool re = sms.SingleSend(dttel.Rows[i]["PhoneNo"].ToString(), SmsTxt.ToString());       //20170425注释，启用新的短信平台发送短信
                    //bool re = sms.SendSMS(dttel.Rows[i]["PhoneNo"].ToString(), SmsTxt.ToString());
                    //bool wxmessage = sms.SendQY_Message_Text(dttel.Rows[i]["PhoneNo"].ToString(), "", "", "20", SmsTxt.ToString());
                    string wxre = sms.SendMsg_TextCard_EncryUrl(dttel.Rows[i]["PhoneNo"].ToString(), "", "", "20", "订单限时驳回", SmsTxt.ToString(), dtID.Rows[0]["ID"].ToString(), dtID.Rows[0]["type"].ToString());
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
                string wxmessageecho = sms.SendMsg_TextCard_EncryUrl("15308078836", "", "", "20", "订单限时驳回", SmsTxt.ToString(), dtID.Rows[0]["ID"].ToString(), dtID.Rows[0]["type"].ToString());
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
                int lngBillType = 0;
                //dt = new OrderManager().DLproc_UnauditedOrderManagersBySel(strManagers, lngBillType);
                dt = new OrderManager().DLproc_UnauditedOrderManagers_U20BySel(strManagers, lngBillType, cSCCode);
                GridOrder.DataSource = dt;
                GridOrder.DataBind();
            }
            #endregion

            #region 查看订单信息
            if (Request.QueryString["vbillno"] != null)
            {
                //查看订单状态
                string strBillNo = Request.QueryString["vbillno"].ToString();
                int lngBillType = 0;
                DataTable dt = new OrderManager().DL_OrderBillBySel(strBillNo, lngBillType);
                //绑定表头字段,text
                TxtBillDate.Text = dt.Rows[0]["datCreateTime"].ToString();
                TxtBiller.Text = dt.Rows[0]["ccusname"].ToString();
                TxtCustomer.Text = dt.Rows[0]["ccusname"].ToString();
                TxtOrderBillNo.Text = strBillNo;
                TxtOrderMark.Text = dt.Rows[0]["strRemarks"].ToString();
                switch (dt.Rows[0]["cSCCode"].ToString())
                {
                    case "00":
                        TxtcSCCode.Text = "自提";
                        break;
                    case "01":
                        TxtcSCCode.Text = "配送";
                        break;
                }
                switch (dt.Rows[0]["cSTCode"].ToString())
                {
                    case "00":
                        TxtcSTCode.Text = "普通销售";
                        break;
                    case "01":
                        TxtcSTCode.Text = "样品资料";
                        break;
                }
                Txtcdefine3.Text = dt.Rows[0]["cdefine3"].ToString();   //车型
                TxtLoadingWays.Text = dt.Rows[0]["strLoadingWays"].ToString();  //装车方式
                TxtDeliveryDate.Text = dt.Rows[0]["datDeliveryDate"].ToString();//交货日期
                TxtOrderShippingMethod.Text = dt.Rows[0]["cDefine11"].ToString();//发运方式
                TxtSalesman.Text = dt.Rows[0]["cpersoncode"].ToString();//业务员
                TxtBillTime.Text = dt.Rows[0]["datBillTime"].ToString();//下单时间
                //绑定表体字段,grid
                ViewOrderGrid.DataSource = dt;
                ViewOrderGrid.DataBind();
                //绑定grid
                DataTable dtgrid = new DataTable();
                //int lngBillType = 0;
                //dtgrid = new OrderManager().DLproc_UnauditedOrderManagersBySel(strManagers, lngBillType);
                dtgrid = new OrderManager().DLproc_UnauditedOrderManagers_U20BySel(strManagers, lngBillType, cSCCode);
                GridOrder.DataSource = dtgrid;
                GridOrder.DataBind();
            }
            #endregion

        }
        #endregion
    }

    protected void BtnRefresh_Click(object sender, EventArgs e) //刷新,待我处理的订单
    {
        string cSCCode = "%";
        if (RadioButton1.Checked == true)
        {
            cSCCode = "00";
        }
        if (RadioButton2.Checked == true)
        {
            cSCCode = "01";
        }
        DataTable dt = new DataTable();
        string strManagers = Session["lngopUserId"].ToString();
        int lngBillType = 0;
        //dt = new OrderManager().DLproc_UnauditedOrderManagersBySel(strManagers, lngBillType);
        dt = new OrderManager().DLproc_UnauditedOrderManagers_U20BySel(strManagers, lngBillType, cSCCode);
        GridOrder.DataSource = dt;
        GridOrder.DataBind();

        //获取选中的金额
        double cc = 0;
        for (int ii = 0; ii < GridOrder.VisibleRowCount - 1; ii++)
        {
            if (GridOrder.Selection.IsRowSelected(ii))
            {
                cc = cc + Convert.ToDouble(GridOrder.GetRowValues(ii, "isum").ToString());
            }
        }
        maxsum.Text = cc.ToString();

    }

    protected void GridOrder_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
    {
        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('button！');</script>");
        /*审核事件*/
        //插入表头数据

        //插入表体数据

        //更新DLorder表中该单据状态值为已审核,并且重新绑定grid


    }

    #region 简单封装中国短信接口规范v1.2
    /*
   类名：ChinaSms
   说明：简单封装中国短信接口规范v1.2
   更新历史：
   */
    public class ChinaSms
    {
        private string comName;
        private string comPwd;

        public ChinaSms()
        {
        }

        public ChinaSms(String name, String pwd)
        {
            this.comName = name;
            this.comPwd = pwd;
        }

        /*
        说明:封装单发接口
        参数:
          dst:目标手机号码
          msg:发送短信内容
        返回值:
          true:发送成功;
          false:发送失败
        */
        public bool SingleSend(String dst, String msg)
        {
            string sUrl = null;  //接口规范中的地址
            string sMsg = null;  //调用结果
            msg = System.Web.HttpUtility.UrlEncode(msg, System.Text.Encoding.GetEncoding("gb2312"));
            comName = System.Web.HttpUtility.UrlEncode(comName, System.Text.Encoding.GetEncoding("gb2312"));
            //备用IP地址为203.81.21.13
            sUrl = "http:" + "//203.81.21.34//send/gsend.asp?name=" + comName + "&pwd=" + comPwd + "&dst=" + dst + "&msg=" + msg;
            sMsg = GetUrl(sUrl);
            //Console.WriteLine(sMsg);

            if (sMsg.Substring(0, 5) != "num=0")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /*通用调用接口*/
        public String GetUrl(String urlString)
        {
            string sMsg = "";		//引用的返回字符串
            try
            {
                System.Net.HttpWebResponse rs = (System.Net.HttpWebResponse)System.Net.HttpWebRequest.Create(urlString).GetResponse();
                System.IO.StreamReader sr = new System.IO.StreamReader(rs.GetResponseStream(), System.Text.Encoding.Default);
                sMsg = sr.ReadToEnd();
            }
            catch
            {
                return sMsg;
            }
            return sMsg;
        }
    }
    #endregion

    protected void GridOrder_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
    {
        string cSCCode = "%";
        if (RadioButton1.Checked == true)
        {
            cSCCode = "00";
        }
        if (RadioButton2.Checked == true)
        {
            cSCCode = "01";
        }
        //e.NewValues[""].ToString();
        string strBillNo = e.Keys["strBillNo"].ToString();
        string srtRemarks = e.NewValues["strRejectRemarks"].ToString();
        bool c = new OrderManager().DL_OrderRejRemarksByUpdl(strBillNo, srtRemarks);
        GridOrder.CancelEdit();//结束编辑状态
        e.Cancel = true;

        DataTable dt = new DataTable();
        string strManagers = Session["lngopUserId"].ToString();
        int lngBillType = 0;
        //dt = new OrderManager().DLproc_UnauditedOrderManagersBySel(strManagers, lngBillType);
        dt = new OrderManager().DLproc_UnauditedOrderManagers_U20BySel(strManagers, lngBillType, cSCCode);
        GridOrder.DataSource = dt;
        GridOrder.DataBind();

    }

    /// <summary>
    /// 批量审核订单,生成预约号
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BatchAudit_Click(object sender, EventArgs e)
    {
        string aa = "";
        string bb = "";
        double cc = 0;
        bool unsingle = false;
        #region 检测全部是否满足审核
        for (int i = 0; i < GridOrder.VisibleRowCount - 1; i++)
        {
            if (GridOrder.Selection.IsRowSelected(i))
            {
                aa = aa + "|" + GridOrder.GetRowValues(i, "strBillNo").ToString();
                cc = cc + Convert.ToDouble(GridOrder.GetRowValues(i, "isum").ToString());
                if (bb != "" && bb != GridOrder.GetRowValues(i, "strUserName").ToString())
                {
                    unsingle = true;
                }
                else
                {
                    bb = GridOrder.GetRowValues(i, "strUserName").ToString();
                }
            }
        }

        if (unsingle)   //客户不一致
        {
            maxsum.Text = cc.ToString();
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('选择的客户不一致!');</script>");
            return;
        }

        //if (cc < 30000)      //金额小于30000
        //{
        //    maxsum.Text = cc.ToString();
        //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('所选金额小于30000!');</script>");
        //    return;
        //}
        #endregion
        //return;
        //开始调用api写入
        //先判断是否是dl的订单,是否是czts的订单
        //获取预约流水号
        string strBillType = "13";
        DataTable yuyue = new BLL.OrderManager().DLproc_GetBillNoForData(strBillType);
        string yuyueno = yuyue.Rows[0][0].ToString();
        string strBillNo = "";
        string uu = "销售订单:";    //没有找到对应的已审核的特殊订单
        string orderok = "";    //已成功审核的订单提示
        string warn = "";     //审核最终提示
        string strManagers = Session["lngopUserId"].ToString();
        #region 遍历勾选的订单数据       
        for (int i  = 0; i < GridOrder.VisibleRowCount ; i++)
        {
            #region 开始审核单据,传递数据

            #endregion
            if (GridOrder.Selection.IsRowSelected(i))
            {
                strBillNo = GridOrder.GetRowValues(i, "strBillNo").ToString();
                ADODB.Connection conn = new ADODB.Connection();
                #region 审核通过,bytstatus=4

                #region 说明
                //检测是否已经有专属操作员

                ////插入表头数据    --10.25更新方法,在插入表头数据之后,继续插入表体数据.最后更新dl_oporder表中的U8订单号,cSOCode,并且更新dl_oporder订单状态
                //string strBillNo = Request.QueryString["billno"].ToString();
                //bool c = new OrderManager().DLproc_NewOrderU8ByIns(strBillNo);
                ////插入表体数据    --插入表头数据时,已经写入!

                ////更新dlorder表中该单据状态值为已审核,并且更新DL订单的表头上的正式订单号     --插入表头数据时,已经更新!
                /*
                ////1107更新,审核,将bytStatus状态更新为2,待用户确认后生成U8订单
                //string strBillNo = Request.QueryString["billno"].ToString();
                //bool c = new OrderManager().DL_CheckOrderBillByUpd(strBillNo);    
                */
                //1115修改,订单专员直接审核通过,生成U8订单,DL订单状态改为2,
                //插入表头数据    --10.25更新方法,在插入表头数据之后,继续插入表体数据.最后更新dl_oporder表中的U8订单号,cSOCode,并且更新dl_oporder订单状态
                //20170116,添加，如果是参照特殊订单审核，则直接关闭，否则，继续按以前流程处理		 
                #endregion
                //20170925启用新的czts生成发货单的查询数据处理,查找实际的预留库存,并自动分配仓库数量,操作员的审核czts调用该方法

                #region CZTS审核
                if (strBillNo.Substring(0, 4).ToString() == "CZTS")
                {
                    bool c = true;
                    //更新参照特殊订单,关闭,并提示
                    //bool u = new SearchManager().DL_CZTSCloseByUpd(strBillNo); //20170401关闭，直接生成销售订单下的发货单
                    //20170401直接生成销售订单下的发货单，先判断是否有未审核的销售订单
                    DataTable unSHDD = new SearchManager().DL_CZTSCheckBySel(strBillNo);
                    if (unSHDD.Rows.Count > 0)
                    {
                        for (int ii = 0; ii < unSHDD.Rows.Count; ii++)
                        {
                            uu = uu + unSHDD.Rows[ii][0].ToString() + ",";
                        }
                        continue;
                    }

                    #region api事务之前的生成U8发货单方式
                    ////生成U8发货单
                    ////审核完成之后，更新U8发货单流水号(发货单表头扩展表)
                    //bool fhdlsh = new OrderManager().DLproc_CZTSFHDLSHByUpd(strBillNo);
                    #endregion

                    U8Login.clsLogin u8Login = new U8Login.clsLogin();
                    String sSerial = "";
                    String sSubId = System.Web.Configuration.WebConfigurationManager.AppSettings["sSubId"];
                    String sYear = "2015";
                    String sDate = "2018-12-10";
                    //String sAccID = System.Web.Configuration.WebConfigurationManager.AppSettings["sAccID"].ToString();
                    String sAccID = System.Web.Configuration.WebConfigurationManager.AppSettings["sAccID"].Replace("test", "test").ToString();
                    String sUserID = System.Web.Configuration.WebConfigurationManager.AppSettings["sUserID"];
                    String sPassword = System.Web.Configuration.WebConfigurationManager.AppSettings["sPassword"];
                    String sServer = System.Web.Configuration.WebConfigurationManager.AppSettings["sServer"];
                    if (!u8Login.Login(ref sSubId, ref sAccID, ref sYear, ref sUserID, ref sPassword, ref sDate, ref sServer, ref sSerial))
                    {
                        Console.WriteLine("登陆失败，原因：" + u8Login.ShareString);
                        Marshal.FinalReleaseComObject(u8Login);
                        warn = warn + "登陆失败，原因：" + u8Login.ShareString + ",";
                        continue;
                    }
                    U8EnvContext envContext = new U8EnvContext();
                    envContext.U8Login = u8Login;
                    string strConn = envContext.U8Login.UfDbName;
                    //获取需要传入的数据
                    DataTable dtHead = new BLL.OrderManager().DL_NewOrderToDispHU8BySel(strBillNo);
                    DataTable dtBody = new BLL.OrderManager().DL_NewOrderToDispBU8BySel(strBillNo);//  非金花，大井
                    DataTable dtBody1 = new BLL.OrderManager().DL_NewOrderToDispB_JH_U8BySel(strBillNo);//金花
                    DataTable dtBody2 = new BLL.OrderManager().DL_NewOrderToDispB_DJ_U8BySel(strBillNo);//大井
                    string strBillNo1 = strBillNo + "-1";
                    string strBillNo2 = strBillNo + "-2";
                    //本地sql事务
                    SqlConnection DBconn = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString);//实例化数据连  
                    DBconn.Open();//打开数据库连接  
                    SqlCommand command = DBconn.CreateCommand();
                    SqlTransaction transaction = null;
                    transaction = DBconn.BeginTransaction();
                    command.Connection = DBconn;
                    command.Transaction = transaction;
                    //int count = 0;
                    string aa1 = "";
                    string aa2 = "";
                    string aa3 = "";
                    //开启事务
                    conn.Open(strConn, "sa", envContext.U8Login.SysPassword, 0);
                    bool u = true;
                    conn.BeginTrans();
                    try
                    {
                        //插入非金花,大井数据
                        if (dtBody.Rows.Count > 0)
                        {
                            aa1 = new FHD_U8API().AddFHDAPI(dtHead, dtBody, strBillNo, conn, yuyueno);
                        }
                        //插入金花数据
                        if (dtBody1.Rows.Count > 0)
                        {
                            aa2 = new FHD_U8API().AddFHDAPI(dtHead, dtBody1, strBillNo1, conn, yuyueno);
                        }
                        //插入大井数据
                        if (dtBody2.Rows.Count > 0)
                        {
                            aa3 = new FHD_U8API().AddFHDAPI(dtHead, dtBody2, strBillNo2, conn, yuyueno);
                        }
                        object ob = new object();
                        string sql = "UPDATE dbo.Dl_opOrder SET bytStatus=4,datAuditordTime=GETDATE() WHERE strBillNo='" + strBillNo + "'";
                        conn.Execute(sql, out ob);
                        conn.Execute("DLproc_CZTSFHDLSHByUpd '" + strBillNo + "'", out ob);

                        if (!string.IsNullOrEmpty(aa1) || !string.IsNullOrEmpty(aa2) || !string.IsNullOrEmpty(aa3))
                        {
                            conn.RollbackTrans();
                            transaction.Rollback();
                            c = false;
                            bool r = new OrderManager().DL_ErrByIns(strBillNo, aa1 + aa2 + aa3);
                        }
                        else
                        {
                            conn.CommitTrans();
                            transaction.Commit();
                            c = true;
                        }


                    }
                    catch
                    {
                        conn.RollbackTrans();
                        //trans.Rollback();//如果前面有异常则事务回滚  
                        transaction.Rollback();
                        c = false;
                        bool r = new OrderManager().DL_ErrByIns(strBillNo, aa1 + aa2 + aa3);
                    }
                    finally
                    {
                        conn.Close();
                        DBconn.Close();
                    }
                    if (!string.IsNullOrEmpty(aa1) || !string.IsNullOrEmpty(aa2) || !string.IsNullOrEmpty(aa3)) //生成失败
                    {
                        bool r = new OrderManager().DL_ErrByIns(strBillNo, aa1 + aa2 + aa3);
                    }
                    else   //生成成功
                    {
                        orderok = orderok + strBillNo + ",";
                    }
                }
                #endregion

                #region DL订单审核
                if (strBillNo.Substring(0, 2).ToString() == "DL")
                {
                    bool c = true;
                    #region 20170617新方法，调用类，并开启事务
                    //先判断是否已经生成了w的订单，如果没有就调用api，如果有，就更新对应网上订单的状态为已审核,DL_IsExistsDLBySel
                    bool g = new OrderManager().DL_IsExistsDLBySel(strBillNo);
                    if (g)
                    {
                        warn = warn + ",订单:" + strBillNo + "已经在U8中存在!";
                        continue;
                    }
                    U8Login.clsLogin u8Login = new U8Login.clsLogin();
                    String sSerial = "";
                    String sSubId = System.Web.Configuration.WebConfigurationManager.AppSettings["sSubId"];
                    String sYear = "2015";
                    String sDate = "2018-12-10";
                    //String sAccID = System.Web.Configuration.WebConfigurationManager.AppSettings["sAccID"].ToString();
                    String sAccID = System.Web.Configuration.WebConfigurationManager.AppSettings["sAccID"].Replace("test", "test").ToString();
                    String sUserID = System.Web.Configuration.WebConfigurationManager.AppSettings["sUserID"];
                    String sPassword = System.Web.Configuration.WebConfigurationManager.AppSettings["sPassword"];
                    String sServer = System.Web.Configuration.WebConfigurationManager.AppSettings["sServer"];
                    if (!u8Login.Login(ref sSubId, ref sAccID, ref sYear, ref sUserID, ref sPassword, ref sDate, ref sServer, ref sSerial))
                    {
                        Console.WriteLine("登陆失败，原因：" + u8Login.ShareString);
                        Marshal.FinalReleaseComObject(u8Login);
                        warn = warn + "登陆失败，原因：" + u8Login.ShareString + ",";
                        continue;
                    }
                    U8EnvContext envContext = new U8EnvContext();
                    envContext.U8Login = u8Login;
                    string strConn = envContext.U8Login.UfDbName;
                    string U8XSDD_rel = "";
                    //本地sql事务
                    SqlConnection DBconn = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString);//实例化数据连  
                    DBconn.Open();//打开数据库连接  
                    SqlCommand command = DBconn.CreateCommand();
                    SqlTransaction transaction = null;
                    transaction = DBconn.BeginTransaction();
                    command.Connection = DBconn;
                    command.Transaction = transaction;
                    DataTable dtHead = new DataTable();
                    conn.Open(strConn, "sa", envContext.U8Login.SysPassword, 0);
                    conn.BeginTrans();
                    dtHead = new BLL.OrderManager().DLproc_NewOrderU8BySel(strBillNo);
                    try
                    {
                        XSDD xsdd = new XSDD();
                        U8XSDD_rel = xsdd.AddXSDDAPI(dtHead, conn, yuyueno);
                        object ob = new object();
                        conn.Execute("DLproc_NewOrderU8APIByUpd '" + strBillNo + "'", out ob);

                        if (!string.IsNullOrEmpty(U8XSDD_rel))
                        {
                            conn.RollbackTrans();
                            transaction.Rollback();
                            c = false;
                        }
                        else
                        {
                            conn.CommitTrans();
                        }
                    }
                    catch (System.Exception exa)
                    {
                        conn.RollbackTrans();
                        //trans.Rollback();//如果前面有异常则事务回滚  
                        transaction.Rollback();
                        c = false;
                        bool r = new OrderManager().DL_ErrByIns(strBillNo, exa.ToString());
                    }
                    finally
                    {
                        conn.Close();
                        DBconn.Close();
                    }
                    if (!string.IsNullOrEmpty(U8XSDD_rel))
                    {
                        bool r = new OrderManager().DL_ErrByIns(strBillNo, U8XSDD_rel);
                    }
                    #endregion

                    if (c == false)
                    {
                        warn = warn + ",订单:" + strBillNo + "生成失败.";
                        continue;
                    }
                }
                #endregion

                #endregion
                //查询对应的U8订单号
                //20170116注释 DataTable u8csocode = new OrderManager().DL_OrderBillNoForU8OrderBillNoByIns(strBillNo);
                //插入表体数据    --插入表头数据时,已经写入!
                //更新dlorder表中该单据状态值为已审核,并且更新DL订单的表头上的正式订单号     --插入表头数据时,已经更新!
                /*20151217增加:审核通过后发送短信给顾客*/
                #region 审核通过后发送短信给顾客 v1.0;2016-01-30 修改
                // 查询数据库中是否存在该用户，如果有则发送短信，如果没有则提示
                DataTable dtcus = new SearchManager().DL_cCusCodeOnOrderBillNoBySel(strBillNo); //查找订单对应的顾客登录编码,然后发送短信
                DataTable dttel = new GetPhoneNo().GetSubCustomerPhoneNo(dtcus.Rows[0]["lngopUserId"].ToString(), dtcus.Rows[0]["lngopUserExId"].ToString());
                if (dttel.Rows.Count < 1)
                {
                    orderok = orderok + strBillNo + "已经审核,请在U8中确认！未查找到对应顾客电话号码,审核短信未发送！";
                    continue;
                }
                int ResultOk = 0; //  记录结果
                int ResultNo = 0; //  记录结果
                string Result = "";
                //发送短信
                #region 商讯·中国 发送短信
                //短信内容
                string SmsTxt = "【多联网上订单】网单号：" + strBillNo + "<br>" + DateTime.Now.ToString("g");
                DataTable dtID = new OrderManager().DLproc_OrderBillBySel(strBillNo, 1);
                //#region 微信企业号发送信息，应用id20，
                QYH9003.WeiXinSoapClient sms = new QYH9003.WeiXinSoapClient();
                for (int ii = 0; ii < dttel.Rows.Count; ii++)
                {
                    string wxre = sms.SendMsg_TextCard_EncryUrl(dttel.Rows[ii]["PhoneNo"].ToString(), "", "", "20", "订单审核", SmsTxt.ToString(), dtID.Rows[0]["ID"].ToString(), dtID.Rows[0]["type"].ToString());
                    bool re = sms.Check_WXRel(wxre);
                    if (re == false)
                    {
                        ResultNo = ResultNo + 1;
                        Result = Result + dttel.Rows[ii]["PhoneNo"].ToString() + ";";
                    }
                    else
                    {
                        ResultOk = ResultOk + 1;
                    }
                }
                string wxmessageecho = sms.SendMsg_TextCard_EncryUrl("15308078836", "", "", "20", "订单审核", SmsTxt.ToString(), dtID.Rows[0]["ID"].ToString(), dtID.Rows[0]["type"].ToString());
                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + "已经审核,请在U8中确认！成功发送" + ResultOk + "条信息,发送失败" + ResultNo + "条信息,号码:" + Result + "！');</script>");
                #endregion
                #endregion
            }
            //批处理完毕后输出提示
            //uu = uu + "未审核，请先审核对应的销售订单后再审核参照订单！";
            //bool rr = new OrderManager().DL_ErrByIns(strBillNo, uu);
        }
        #endregion
        //重新绑定
        string cSCCode = "%";
        if (RadioButton1.Checked == true)
        {
            cSCCode = "00";
        }
        if (RadioButton2.Checked == true)
        {
            cSCCode = "01";
        }
        DataTable dt = new DataTable();
        int lngBillType = 0;
        dt = new OrderManager().DLproc_UnauditedOrderManagers_U20BySel(strManagers, lngBillType, cSCCode);
        GridOrder.DataSource = dt;
        GridOrder.DataBind();
        //获取选中的金额
        cc = 0;
        for (int ii = 0; ii < GridOrder.VisibleRowCount - 1; ii++)
        {
            if (GridOrder.Selection.IsRowSelected(ii))
            {
                cc = cc + Convert.ToDouble(GridOrder.GetRowValues(ii, "isum").ToString());
            }
        }
        maxsum.Text = cc.ToString();
        Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('" + uu + warn + orderok + "');</script>");
    }

    /// <summary>
    /// 行准备数据处理
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GridOrder_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
    {
        //if (e.RowType != DevExpress.Web.ASPxGridView.) return;
        if (e.GetValue("ccodeID") != null)
        {
            if (e.GetValue("iAddressType").ToString() == "1" && e.GetValue("ccodeID").ToString() != "0")
            {
                e.Row.Style.Add("color", "White");
                e.Row.BackColor = System.Drawing.Color.Violet;

            }
        }
    }
}