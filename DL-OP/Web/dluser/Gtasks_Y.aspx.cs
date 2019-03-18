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


public partial class dluser_Gtasks_Y : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string strManagers = Session["lngopUserId"].ToString();

            #region 审核通过,bytstatus=4
            if (Request.QueryString["billno"] != null)
            {
                //检测是否已经有专属操作员
                //插入表头数据    --10.25更新方法,在插入表头数据之后,继续插入表体数据.最后更新dl_oporder表中的U8订单号,cSOCode,并且更新dl_oporder订单状态
                string strBillNo = Request.QueryString["billno"].ToString();
                bool c = new OrderManager().DLproc_NewYOrderU8ByIns(strBillNo);
                //插入表体数据    --插入表头数据时,已经写入!
                //更新dlorder表中该单据状态值为已审核,并且更新DL订单的表头上的正式订单号     --插入表头数据时,已经更新!
                //重新绑定
                DataTable dt = new DataTable();
                dt = new OrderManager().DLproc_UnauditedPreOrderManagersBySel(strManagers, 1);
                GridOrder.DataSource = dt;
                GridOrder.DataBind();

                /*20151217增加:审核通过后发送短信给顾客,2016-01-30修改*/
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
                string Result="";
                //发送短信
                #region 商讯·中国 发送短信
                //短信内容
                string SmsTxt = "【多联网上订单】尊敬的客户,您的网单号：" + strBillNo + "已经通过审核！谢谢您的支持。 ";
                //发送
                //ChinaSms sms = new ChinaSms("duolian", "duolian123");
                //SendSMS sms = new SendSMS();    //20170425注释，启用新的短信平台发送短信
                SendSMS2Customer9003.SendSMS2CustomerSoapClient sms = new SendSMS2Customer9003.SendSMS2CustomerSoapClient();
                //bool re = sms.SingleSend(telNo, SmsTxt);
                for (int i = 0; i < dttel.Rows.Count; i++)
                {
                    //bool re = sms.SingleSend(dttel.Rows[i]["PhoneNo"].ToString(), SmsTxt.ToString());   //20170425注释，启用新的短信平台发送短信
                    bool re = sms.SendSMS(dttel.Rows[i]["PhoneNo"].ToString(), SmsTxt.ToString());
                    if (re==false)
                    {
                        ResultNo = ResultNo + 1;
                        Result=Result+dttel.Rows[i]["PhoneNo"].ToString()+";";
                    }
                    else
                    {
                        ResultOk = ResultOk + 1;
                    }
                }

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + "已经审核,请在U8中确认！成功发送" + ResultOk + "条短信,发送失败" + ResultNo + "条短信,号码:" + Result+ "！');</script>");

                #endregion


                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + "已经审核,请在U8中确认！');</script>");
                #endregion
            }
            #endregion

            #region 驳回,bytstatus=3
            if (Request.QueryString["dbillno"] != null)
            {
                //检测是否已经有专属操作员

                //驳回订单审核,标记状态为3,reject
                string strBillNo = Request.QueryString["dbillno"].ToString();
                bool c = new OrderManager().DL_RejectYOrderBillByUpd(strBillNo, strManagers);
                //重新绑定显示
                DataTable dt = new DataTable();
                dt = new OrderManager().DLproc_UnauditedPreOrderManagersBySel(strManagers,1);
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
                string SmsTxt = "【多联网上订单】尊敬的客户,您的网单号：" + strBillNo + "已经被驳回，请尽快修正后重新提交订单！谢谢您的支持。 ";
                //发送
                //ChinaSms sms = new ChinaSms("duolian", "duolian123");
                //SendSMS sms = new SendSMS();  //20170425注释，启用新的短信平台发送短信
                SendSMS2Customer9003.SendSMS2CustomerSoapClient sms = new SendSMS2Customer9003.SendSMS2CustomerSoapClient();
                //bool re = sms.SingleSend(telNo, SmsTxt);
                for (int i = 0; i < dttel.Rows.Count; i++)
                {
                    //bool re = sms.SingleSend(dttel.Rows[i]["PhoneNo"].ToString(), SmsTxt.ToString()); //20170425注释，启用新的短信平台发送短信
                    bool re = sms.SendSMS(dttel.Rows[i]["PhoneNo"].ToString(), SmsTxt.ToString());
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

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + " 已经驳回,待客户接收！成功发送" + ResultOk + "条短信,发送失败" + ResultNo + "条短信,号码:" + Result + "！');</script>");

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
                dt = new OrderManager().DLproc_UnauditedPreOrderManagersBySel(strManagers,1);
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
                dt = new OrderManager().DLproc_UnauditedPreOrderManagersBySel(strManagers, 1);
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
                string Result="";
                //发送短信
                #region 商讯·中国 发送短信
                //短信内容
                string SmsTxt = "【多联网上订单】尊敬的客户,您的网单号：" + strBillNo + "已取消！客服热线：4008786333！ ";
                //发送
                //SendSMS sms = new SendSMS();    //20170425注释，启用新的短信平台发送短信
                SendSMS2Customer9003.SendSMS2CustomerSoapClient sms = new SendSMS2Customer9003.SendSMS2CustomerSoapClient();
                for (int i = 0; i < dttel.Rows.Count; i++)
                {
                    //bool re = sms.SingleSend(dttel.Rows[i]["PhoneNo"].ToString(), SmsTxt.ToString());   //20170425注释，启用新的短信平台发送短信
                    bool re = sms.SendSMS(dttel.Rows[i]["PhoneNo"].ToString(), SmsTxt.ToString());
                    if (re==false)
                    {
                        ResultNo = ResultNo + 1;
                        Result=Result+dttel.Rows[i]["PhoneNo"].ToString()+";";
                    }
                    else
                    {
                        ResultOk = ResultOk + 1;
                    }
                }

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单:" + strBillNo + "已经作废！成功发送" + ResultOk + "条短信,发送失败" + ResultNo + "条短信,号码:" + Result+ "！');</script>");
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
                TxtBillTime.Text = dt.Rows[0]["datBillTime"].ToString();//下单时间  
                TxtBillDate.Text = dt.Rows[0]["ddate"].ToString();//订单日期       
                  //绑定表体字段,grid
                ViewOrderGrid.DataSource = dt;
                ViewOrderGrid.DataBind();
                //绑定grid
                DataTable dtgrid = new DataTable();
                dtgrid = new OrderManager().DLproc_UnauditedPreOrderManagersBySel(strManagers,1);
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
        dt = new OrderManager().DLproc_UnauditedPreOrderManagersBySel(strManagers,1);
        GridOrder.DataSource = dt;
        GridOrder.DataBind();
    }

    protected void GridOrder_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
    { }

    

}