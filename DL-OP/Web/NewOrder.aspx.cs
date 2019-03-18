using BLL;
using DevExpress.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model;
using System.Text.RegularExpressions;
using DevExpress.Web.ASPxTreeList;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

public partial class NewOrder : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["login"] == null)
        {
            Response.Write("<script>top.window.location='login.aspx'</script>");   //跳转到登陆页
        }
        //开启时间管理
        DataTable timecontrol = new OrderManager().DL_OrderENTimeControlBySel();
        if (timecontrol.Rows.Count < 1)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('" + ConfigurationManager.AppSettings["datOrderTime"].ToString() + "');window.parent.location.href='AllBlank.aspx';</script>");
            return;
        }

        //逾期30天未确认,欠款未确认
        DataTable confimsoa = new OrderManager().DL_NotConfirmedSOABySel(Session["ConstcCusCode"].ToString() + "%");
        for (int i = 0; i < confimsoa.Rows.Count; i++)
        {
            if (Convert.ToInt16(confimsoa.Rows[i][0].ToString()) > 0)
            {
                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('尊敬的客户您好！您的上月对账单请确认！若未确认，将无法进行下单业务！');window.parent.location.href='ConfirmationSOA-Guide.aspx';</script>");
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('尊敬的客户，您有" + confimsoa.Rows[0][0].ToString() + "张账单未确认，" + confimsoa.Rows[1][0].ToString() + "张延期欠款通知单未确认！若未确认，将无法进行下单业务！');window.parent.location.href='ConfirmationSOA-Guide.aspx';</script>");
                return;
            }
        }


        //页面样式
        if (Convert.ToInt32(HFMain.Value.ToString()) == 0)
        {
            BtnMain.Text = "收起↑";
            this.main.Style.Add("display", "block");
        }
        else
        {
            BtnMain.Text = "展开↓";
            this.main.Style.Add("display", "none");   //隐藏Div,显示this.DivSOA.Style.Add("display", "block");            
        }

        if (Session["GridViewHeigh"] != null)
        {
            OrderGrid.Settings.VerticalScrollableHeight = Convert.ToInt32(Session["GridViewHeigh"].ToString());
        }
        if (Session["GridViewFontSize"] != null)
        {
            OrderGrid.Font.Size = Convert.ToInt32(Session["GridViewFontSize"].ToString());
        }

        //刷新临时订单表数据
        DataTable dtBackOrderGridView = new OrderManager().DL_GetOrderBackBySel(Convert.ToInt32(Session["lngopUserId"].ToString()));
        BackOrderGridView.DataSource = dtBackOrderGridView;
        BackOrderGridView.DataBind();

        #region 判断session是否存在,并且建立datatable,用于记录选择项目,gridselect
        if (Session["gridselect"] == null)
        {
            DataTable dts = new DataTable();
            dts.Columns.Add("cInvCode"); //编码    0            
            //dt.Rows.Add(new object[] { "0"});
            Session["gridselect"] = dts;
        }
        #endregion
        //绑定treelist数据源
        string treecuscode = "";
        if (Session["KPDWcCusCode"] == null)
        {
            treecuscode = Session["ConstcCusCode"].ToString();
        }
        else
        {
            treecuscode = Session["KPDWcCusCode"].ToString();
        }
        DataTable dttree = new SearchManager().DLproc_InventoryBySel("00", treecuscode);
        treeList.KeyFieldName = "KeyFieldName";
        treeList.ParentFieldName = "ParentFieldName";
        treeList.DataSource = dttree;
        /*展开第一级node*/
        treeList.DataBind();
        treeList.ExpandToLevel(1);
        //绑定gridview数据源
        DataTable dt1 = new DataTable();
        if (Session["ordertreelistgrid"] != null)
        {
            dt1 = new SearchManager().DLproc_TreeListDetailsAllBySel(Session["ordertreelistgrid"].ToString(), Session["KPDWcCusCode"].ToString(), 1);
            TreeDetail.DataSource = dt1;
            TreeDetail.DataBind();

            //删除当前绑定的数据,因为切换分类,所以需要删除
            DataTable dtst = (DataTable)Session["gridselect"];
            #region 原始方式
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                for (int j = 0; j < dtst.Rows.Count; j++)
                {
                    if (TreeDetail.GetRowValues(i, "cInvCode") != null && TreeDetail.GetRowValues(i, "cInvCode").ToString() == dtst.Rows[j][0].ToString())
                    {
                        TreeDetail.Selection.SelectRow(i);
                        dtst.Rows[j].Delete();
                        dtst.AcceptChanges();
                    }
                }
            }
            #endregion
            #region 新方式
            //for (int j = 0; j < dtst.Rows.Count; j++)   //循环已选商品,并给其赋予 SetSelectionByKey,选中状态(部分为选商品在下面处理)
            //{
            //    TreeDetail.Selection.SetSelectionByKey(dtst.Rows[j]["cInvCode"].ToString(), true);
            //}
            //if (TreeDetail.Selection.Count > 0)
            //{
            //    for (int i = 0; i < TreeDetail.Selection.Count; i++) //获取已勾选的商品,并且在Session["gridselect"]=dtst中删除,然后再赋值给Session["gridselect"]
            //    {
            //        string delcinvcode = TreeDetail.GetSelectedFieldValues("cInvCode")[i].ToString()  ;
            //        for (int k = 0; k < dtst.Rows.Count; k++)
            //        {
            //            if (delcinvcode == dtst.Rows[k][0].ToString())
            //            {
            //                dtst.Rows[k].Delete();
            //                dtst.AcceptChanges();
            //                //break;
            //            }
            //        }
            //    }
            //}
            ////for (int i = 0; i < TreeDetail.VisibleRowCount; i++)    //获取已勾选的商品,并且在Session["gridselect"]=dtst中删除,然后再赋值给Session["gridselect"]
            ////{
            ////    if (TreeDetail.Selection.IsRowSelected(i))
            ////    {
            ////        string delcinvcode = TreeDetail.GetRowValues(i, "cInvCode").ToString();
            ////        for (int k = 0; k < dtst.Rows.Count; k++)
            ////        {
            ////            if (delcinvcode== dtst.Rows[k].ToString())
            ////            {
            ////                dtst.Rows[k].Delete();
            ////                dtst.AcceptChanges();
            ////                break;
            ////            }
            ////        }
            ////    }
            ////}
            #endregion

        }


        //绑定历史订单表
        DataTable PreviousOrderAgainGridViewdt = new OrderManager().DL_GeneralPreviousOrderBySel(Session["ConstcCusCode"].ToString() + "%");
        PreviousOrderAgainGridView.DataSource = PreviousOrderAgainGridViewdt;
        PreviousOrderAgainGridView.DataBind();

        string strComboShippingMethod = ComboShippingMethod.Text;
        if (strComboShippingMethod == "自提")
        {
            //绑定GridViewShippingMethod数据,自提
            DataTable griddtadd = new BasicInfoManager().DLproc_UserAddressZTBySelGroup(Session["lngopUserId"].ToString());
            GridViewShippingMethod.DataSource = griddtadd;
            GridViewShippingMethod.DataBind();
        }
        else
        {
            //绑定GridViewShippingMethod数据,配送
            DataTable griddtadd = new BasicInfoManager().DLproc_UserAddressPSBySelGroup(Session["lngopUserId"].ToString());
            GridViewShippingMethod.DataSource = griddtadd;
            GridViewShippingMethod.DataBind();
        }


        ////绑定销售类型
        //DataTable CombocSTCodeGrid = new DataTable();
        //CombocSTCodeGrid.Columns.Add("cSTCode", Type.GetType("System.String"));
        //CombocSTCodeGrid.Rows.Add("普通销售");
        //CombocSTCodeGrid.Rows.Add("样品资料");
        //CombocSTCodeGridView.DataSource = CombocSTCodeGrid;
        //CombocSTCodeGridView.DataBind();
        //绑定 TxtRelateU8NO数据
        //DataTable RelateU8NO = new SearchManager().DL_ComboCustomerU8NOBySel(Session["cCusCode"].ToString());
        //TxtRelateU8NOGridView.DataSource = RelateU8NO;
        //TxtRelateU8NOGridView.DataBind();
        //if (HttpContext.Current.Request.Cookies["TxtRelateU8NO"] != null)
        //{
        //    TxtRelateU8NO.Text = Server.UrlDecode(HttpContext.Current.Request.Cookies["TxtRelateU8NO"].Value);
        //}
        //if (Session["TxtRelateU8NO"] != null)
        //{
        //    TxtRelateU8NO.Text = Session["TxtRelateU8NO"].ToString();
        //}

        //griddt.Rows.Add(new object[] { array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7], array[8], "0", "0", "0", "0", "88", "9" });
        //OrderGrid修改提示功能
        OrderGrid.JSProperties.Remove("cpAlertMsg");//先清空,修改grid提示信息
        OrderGrid.JSProperties["cpAlertMsg"] = "";

        DataTable griddt = new DataTable();

        if (IsPostBack)  //回传
        {
            String target = Request.Form["__EVENTTARGET"];
            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('"+target+"');</script>");
            return;
        }
        else   //非回传
        {
            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('温馨提示：尊敬的经销商伙伴们，若您已付款参与公司2016年开业红包，请务必注意以您打款告知我们销帐的帐户名义下订单。若您用其他帐户下单，将无法享受到红包带给您的福利！ \\n温馨提示：尊敬的经销商伙伴们，若您已付款参与公司2016年开业红包，请在下单时务必注意：请将“2015年最后一期酬宾活动”涉及产品，与非酬宾活动产品分开下在两张订单上，切勿放在同一订单！ \\n温馨提示：尊敬的经销商伙伴们，若您已付款参与公司2016年开业红包且未提完货，则您下单界面显示的信用余额将临时显示为还可享用红包的余额。待您提完红包货物后，信用余额将恢复！ ');</script>");

            //Session["ordertreelistgrid"] = "";

            //绑定送货信息grid表
            //griddt = new BasicInfoManager().DLproc_UserAddressPSBySelGroup(Session["lngopUserId"].ToString());
            //GridViewShippingMethod.DataSource = griddt;
            //GridViewShippingMethod.DataBind();
            //            string strComboShippingMethod = TxtOrderShippingMethod.Text;
            //if (strComboShippingMethod == "自提")
            //{
            //    //绑定GridViewShippingMethod数据,自提
            //    DataTable griddtadd = new BasicInfoManager().DLproc_UserAddressZTBySelGroup(Session["lngopUserId"].ToString());
            //    GridViewShippingMethod.DataSource = griddtadd;
            //    GridViewShippingMethod.DataBind();
            //}
            //else
            //{
            //    //绑定GridViewShippingMethod数据,配送
            //    DataTable griddtadd = new BasicInfoManager().DLproc_UserAddressPSBySelGroup(Session["lngopUserId"].ToString());
            //    GridViewShippingMethod.DataSource = griddtadd;
            //    GridViewShippingMethod.DataBind();

            //绑定CustomerGrid,顾客开票信息
            DataTable DtComboCustomer = new DataTable();
            DtComboCustomer = new SearchManager().DL_ComboCustomerBySel(Session["ConstcCusCode"].ToString() + "%");
            CustomerGrid.DataSource = DtComboCustomer;
            CustomerGrid.DataBind();
            //绑定车型信息grid表
            DataTable cdefine3Griddt = new BasicInfoManager().DL_cdefine3BySel();
            cdefine3Grid.DataSource = cdefine3Griddt;
            cdefine3Grid.DataBind();
            //表头字段赋值,地址信息
            if (Session["TxtOrderShippingMethod"] != null)
            {
                TxtOrderShippingMethod.Text = Session["TxtOrderShippingMethod"].ToString();
            }
            //表头字段赋值,顾客开票信息
            if (Session["TxtCustomer"] != null)
            {
                TxtCustomer.Text = Session["TxtCustomer"].ToString();
            }
            //表头字段赋值,业务员信息
            if (Session["cCusPPerson"] != null)
            {
                TxtSalesman.Text = Session["cCusPPerson"].ToString();
            }
            //表头字段赋值,销售类型信息
            //if (Session["cSTCode"] != null)
            //{
            //    if (Session["cSTCode"].ToString() == "00")
            //    {
            //        TxtcSTCode.Text = "普通销售";
            //        Label1.Visible = false;
            //        TxtRelateU8NO.Visible = false;
            //        BtnTxtRelateU8NOChose.Visible = false;
            //        ASPxButton2.Visible = true;
            //        ASPxButton1.Visible = true;
            //        ASPxButton3.Visible = true;
            //        DeliveryDate.ReadOnly = false;
            //    }
            //    else
            //    {
            //        TxtcSTCode.Text = "样品资料";
            //        Label1.Visible = true;
            //        TxtRelateU8NO.Visible = true;
            //        BtnTxtRelateU8NOChose.Visible = true;
            //        ASPxButton2.Visible = false;
            //        ASPxButton1.Visible = false;
            //        ASPxButton3.Visible = false;
            //        DeliveryDate.ReadOnly = true;
            //    }
            //}
            //表头字段赋值,发运方式信息
            if (Session["TxtcSCCode"] != null)
            {
                TxtcSCCode.Text = Session["TxtcSCCode"].ToString();
            }
            //表头字段赋值,车型信息
            if (Session["cdefine3"] != null)
            {
                Txtcdefine3.Text = Session["cdefine3"].ToString();
            }

            //备注cookie判断,读取
            if (HttpContext.Current.Request.Cookies["TxtOrderMark"] != null)
            {
                TxtOrderMark.Text = Server.UrlDecode(HttpContext.Current.Request.Cookies["TxtOrderMark"].Value);
            }
            //装车方式cookie判断,读取
            if (HttpContext.Current.Request.Cookies["TxtLoadingWays"] != null)
            {
                TxtLoadingWays.Text = Server.UrlDecode(HttpContext.Current.Request.Cookies["TxtLoadingWays"].Value);
            }
            //提货时间cookie判断,读取
            if (HttpContext.Current.Request.Cookies["DeliveryDate"] != null)
            {
                DeliveryDate.Text = Server.UrlDecode(HttpContext.Current.Request.Cookies["DeliveryDate"].Value);
            }
            //绑定ComboCustomer
            //DataTable DtComboCustomer = new DataTable();
            //DtComboCustomer = new SearchManager().DL_ComboCustomerBySel(Session["ConstcCusCode"].ToString());
            //CustomerGrid.DataSource = DtComboCustomer;
            //CustomerGrid.DataBind();

            //表头字段赋值,顾客信用额度     ,2016-04-05,增加返回两个值,
            DataTable CusCreditDt = new DataTable();
            CusCreditDt = new OrderManager().DLproc_getCusCreditInfo(Session["KPDWcCusCode"].ToString());//默认登录客户编码
            TxtCusCredit.Text = CusCreditDt.Rows[0]["iCusCreLine"].ToString();
            TxtCBLX.Text = CusCreditDt.Rows[0]["cdiscountname"].ToString();         //2016-04-05,添加 酬宾类型 显示
            //2016-04-26添加,增加一个文本框,用于显示信用额,现金顾客
            if (TxtCusCredit != null && Convert.ToDouble(TxtCusCredit.Text) == -99999999)
            {
                TxtCusCredit1.Text = "现金顾客";
            }
            else
            {
                TxtCusCredit1.Text = TxtCusCredit.Text;
            }


        }
        //TxtBiller.Text = Session["strUserName"].ToString(); //绑定制单人
        TxtBillDate.Text = System.DateTime.Now.ToString("d");   //绑定制单日期

        #region 判断session是否存在,并且建立datatable
        if (Session["ordergrid"] == null)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("cInvCode"); //编码    0
            dt.Columns.Add("cInvName"); //名称    1
            dt.Columns.Add("cInvStd");  //规格    2    
            dt.Columns.Add("cComUnitName"); //基本单位  3
            dt.Columns.Add("cInvDefine1"); //大包装单位  4
            dt.Columns.Add("cInvDefine2"); //小包装单位  5
            dt.Columns.Add("cInvDefine13");  //大包装换算率   6  
            dt.Columns.Add("cInvDefine14"); //小包装换算率    7
            dt.Columns.Add("UnitGroup"); //单位换算率组   8     
            dt.Columns.Add("cComUnitQTY"); //基本单位数量 9
            dt.Columns.Add("cInvDefine1QTY"); //大包装单位数量 10
            dt.Columns.Add("cInvDefine2QTY"); //小包装单位数量 11
            dt.Columns.Add("cInvDefineQTY"); //包装量数量汇总,包装量  12
            dt.Columns.Add("cComUnitPrice"); //基本单位单价(报价)   13
            dt.Columns.Add("cComUnitAmount"); //基本单位金额(报价)  14
            dt.Columns.Add("pack"); //包装量换算结果  15
            dt.Columns.Add("ExercisePrice"); //基本单位单价(执行价格)   16
            dt.Columns.Add("Stock"); //可用库存量   17
            dt.Columns.Add("kl"); //扣率   18
            dt.Columns.Add("cComUnitCode"); //基本单位编码   19
            dt.Columns.Add("iTaxRate"); //销项税率   20
            dt.Columns.Add("cn1cComUnitName"); //销售单位名称   21
            dt.Columns.Add("irowno", typeof(Int32)); //行号，序号   22
            dt.Columns.Add("iLowSumRel"); //是否超出最低库存，23（1未超出，0超出）
            dt.Columns.Add("cDefine24"); //活动类型，24
            //dt.Rows.Add(new object[] { "dasdsad", "张1", "98", "94","","","","","" });
            //dt.Rows.Add(new object[] { "fdsfdfdsf", "张2", "99", "94","","","","","" });
            Session["ordergrid"] = dt;
        }
        DataTable dtt = (DataTable)Session["ordergrid"];
        OrderGrid.DataSource = dtt;
        OrderGrid.DataBind();
        #endregion

        if (Request.QueryString["id"] == null)
        {
            #region code判断,code==null,return
            if (Request.QueryString["code"] == null)
            {
                /*第一次加载页面!!!!第一次加载页面!!!第一次加载页面*/
                //绑定 TxtRelateU8NO数据

                //设置默认业务员:
                //Session["cCusPPerson"] = "1249"; //业务员:操健
                TxtSalesman.Text = Session["cCusPPerson"].ToString();
                //默认选择第一个客户单位(本身单位)
                if (TxtCustomer.Text == "")
                {
                    TxtCustomer.Text = Session["strUserName"].ToString();
                    Session["TxtCustomer"] = Session["strUserName"].ToString();
                }
                //绑定GridViewShippingMethod数据,配送方式数据
                //griddt = new BasicInfoManager().DLproc_UserAddressPSBySelGroup(Session["lngopUserId"].ToString());
                //GridViewShippingMethod.DataSource = griddt;
                //GridViewShippingMethod.DataBind();
                //绑定CustomerGrid,顾客开票信息
                DataTable DtComboCustomer = new DataTable();
                //DtComboCustomer = new SearchManager().DL_ComboCustomerBySel(Session["ConstcCusCode"].ToString() + "%");
                DtComboCustomer = new SearchManager().DL_ComboCustomerBySel(Session["ConstcCusCode"].ToString() + "%");
                CustomerGrid.DataSource = DtComboCustomer;
                CustomerGrid.DataBind();
                //20170120注释
                //DataTable nValidTimeData = new OrderManager().DL_InValidTimeDataBySel(CustomerGrid.GetRowValues(CustomerGrid.FocusedRowIndex, "cCusCode").ToString());
                //if (nValidTimeData.Rows.Count > 0 && nValidTimeData.Rows[0]["Isvaild"].ToString() == "1")
                //{
                //    LabHDNR.Text = "  活动金额:" + nValidTimeData.Rows[0][0].ToString() + ",当前剩余可处理车数:" + nValidTimeData.Rows[0][1].ToString() + "车";
                //}
                //else if (nValidTimeData.Rows.Count > 0 && nValidTimeData.Rows[0]["Isvaild"].ToString() == "0")
                //{
                //    LabHDNR.Text = "￥";
                //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('今天该开票单位的活动时间已关闭！');</script>");
                //    return;
                //}
                //else
                //{
                //    LabHDNR.Text = "￥";
                //}
                LabHDNR.Text = "￥";
                //绑定车型信息grid表
                DataTable cdefine3Griddt = new BasicInfoManager().DL_cdefine3BySel();
                cdefine3Grid.DataSource = cdefine3Griddt;
                cdefine3Grid.DataBind();
                return;
            }
            else
            {
                //删除选择项后绑定数据
                griddt = (DataTable)Session["ordergrid"];
                for (int i = 0; i < griddt.Rows.Count; i++)
                {
                    if (griddt.Rows[i][0].ToString() == Request.QueryString["code"].ToString())
                    {
                        griddt.Rows[i].Delete();
                        griddt.AcceptChanges();
                        OrderGrid.DataSource = griddt;
                        OrderGrid.DataBind();
                        return;
                    }
                }
            }
            #endregion
        }



        #region 目前不会执行以下代码
        //string id = Request.QueryString["id"].ToString();
        ////获取物料详细信息,单位,价格等
        //DataTable iddt = new OrderManager().DLproc_QuasiOrderDetailBySel(id, Session["cCusCode"].ToString());
        ////获取传递过来的参数,并存入数组
        ////string[] array = id.Split('|');   //10-25取消传参,改为下面的查询后在绑定

        ////检测是否重复添加物料
        //griddt = (DataTable)Session["ordergrid"];
        //if (griddt.Rows.Count > 0)
        //{
        //    //Label1.Text = griddt.Rows.Count.ToString();
        //    for (int i = 0; i < griddt.Rows.Count; i++)
        //    {
        //        if (griddt.Rows[i][0].ToString() == id.ToString())
        //        {
        //            //如果已经存在该物料,提示并且返回
        //            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('该物料已经被添加！');</script>");
        //            OrderGrid.DataSource = griddt;
        //            OrderGrid.DataBind();
        //            return;
        //        }
        //    }
        //}
        ////将传递过来的数据放入datatable中,并且绑定gridview
        ////griddt.Rows.Add(new object[] { array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7], array[8], "0", "0", "0", "0", "88", "9" });
        //string[] array = { "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" };
        //array[0] = iddt.Rows[0][3].ToString();//存货编码
        //array[1] = iddt.Rows[0][0].ToString();//存货名称
        //array[2] = iddt.Rows[0][1].ToString();//存货规格
        //array[3] = iddt.Rows[0][2].ToString();//基本单位
        //array[17] = iddt.Rows[0][9].ToString(); //库存可用量
        //array[18] = iddt.Rows[0][8].ToString(); //扣率
        //array[19] = iddt.Rows[0][11].ToString(); //基本单位编码
        //array[20] = iddt.Rows[0][12].ToString(); //销项税率
        //array[21] = iddt.Rows[0][13].ToString(); //销售单位名称
        //array[22] = iddt.Rows[0][13].ToString(); //行号未完成！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！
        ////10.25从传递过来的参数分解数组,改为传递物料的编码进行查询,在循环添加
        //if (iddt.Rows[0]["cInvCode"].ToString().Substring(0, 2) == "32")
        //{
        //    array[4] = iddt.Rows[0][2].ToString();
        //    array[5] = iddt.Rows[0][2].ToString();
        //    array[6] = iddt.Rows[0][4].ToString();
        //    array[7] = iddt.Rows[0][4].ToString();
        //    array[8] = Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[0][4].ToString()), 2)) + iddt.Rows[0][2].ToString() + "=" + Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[0][4].ToString()), 2)) + iddt.Rows[0][2].ToString() + "=" + Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[0][4].ToString()), 2)) + iddt.Rows[0][2].ToString();
        //    array[9] = "0";
        //    array[10] = "0";
        //    array[11] = "0";
        //    array[12] = "0";
        //    array[13] = iddt.Rows[0][15].ToString();
        //    array[14] = "0";
        //    array[15] = "0";
        //    array[16] = iddt.Rows[0][14].ToString();
        //}
        //if (iddt.Rows.Count == 1 && iddt.Rows[0]["cInvCode"].ToString().Substring(0, 2) != "32")
        //{
        //    array[4] = iddt.Rows[0][2].ToString();
        //    array[5] = iddt.Rows[0][2].ToString();
        //    array[6] = iddt.Rows[0][4].ToString();
        //    array[7] = iddt.Rows[0][4].ToString();
        //    array[8] = Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[0][4].ToString()), 2)) + iddt.Rows[0][2].ToString() + "=" + Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[0][4].ToString()), 2)) + iddt.Rows[0][2].ToString() + "=" + Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[0][4].ToString()), 2)) + iddt.Rows[0][2].ToString();
        //    array[9] = "0";
        //    array[10] = "0";
        //    array[11] = "0";
        //    array[12] = "0";
        //    array[13] = iddt.Rows[0][15].ToString();
        //    array[14] = "0";
        //    array[15] = "0";
        //    array[16] = iddt.Rows[0][14].ToString();
        //}
        //if (iddt.Rows.Count == 2 && iddt.Rows[0]["cInvCode"].ToString().Substring(0, 2) != "32")
        //{
        //    array[4] = iddt.Rows[1][2].ToString();
        //    array[5] = iddt.Rows[1][2].ToString();
        //    array[6] = iddt.Rows[1][4].ToString();
        //    array[7] = iddt.Rows[1][4].ToString();
        //    array[8] = Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[1][4].ToString()), 2)) + iddt.Rows[0][2].ToString() + "=" + Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[1][4].ToString()) / Convert.ToDouble(iddt.Rows[1][4].ToString()), 2)) + iddt.Rows[1][2].ToString() + "=" + Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[1][4].ToString()) / Convert.ToDouble(iddt.Rows[1][4].ToString()), 2)) + iddt.Rows[1][2].ToString();
        //    array[9] = "0";
        //    array[10] = "0";
        //    array[11] = "0";
        //    array[12] = "0";
        //    array[13] = iddt.Rows[0][15].ToString();
        //    array[14] = "0";
        //    array[15] = "0";
        //    array[16] = iddt.Rows[0][14].ToString();
        //}
        //if (iddt.Rows.Count >= 3 && iddt.Rows[0]["cInvCode"].ToString().Substring(0, 2) != "32")
        //{
        //    array[4] = iddt.Rows[2][2].ToString();
        //    array[5] = iddt.Rows[1][2].ToString();
        //    array[6] = iddt.Rows[2][4].ToString();
        //    array[7] = iddt.Rows[1][4].ToString();
        //    array[8] = Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[2][4].ToString()), 2)) + iddt.Rows[0][2].ToString() + "=" + Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[2][4].ToString()) / Convert.ToDouble(iddt.Rows[1][4].ToString()), 2)) + iddt.Rows[1][2].ToString() + "=" + Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[0][4].ToString()), 2)) + iddt.Rows[2][2].ToString();
        //    array[9] = "0";
        //    array[10] = "0";
        //    array[11] = "0";
        //    array[12] = "0";
        //    array[13] = iddt.Rows[0][15].ToString();
        //    array[14] = "0";
        //    array[15] = "0";
        //    array[16] = iddt.Rows[0][14].ToString();
        //}
        //griddt.Rows.Add(new object[] { array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7], array[8], array[9], array[10], array[11], array[12], array[13], array[14], array[15], array[16], array[17], array[18], array[19], array[20], array[21] });
        //Session["ordergrid"] = griddt;
        //OrderGrid.DataSource = griddt;
        //OrderGrid.DataBind();
        #endregion
    }

    protected void btOK_Click(object sender, EventArgs e)   //选择完成送货信息
    {
        if (GridViewShippingMethod.GetRowValues(GridViewShippingMethod.FocusedRowIndex, "ShippingInformation") != null)
        {
            string ShippingInformation = GridViewShippingMethod.GetRowValues(GridViewShippingMethod.FocusedRowIndex, "ShippingInformation").ToString();
            TxtOrderShippingMethod.Text = ShippingInformation.ToString();
            TxtcSCCode.Text = ComboShippingMethod.Value.ToString();
            Session["TxtOrderShippingMethod"] = TxtOrderShippingMethod.Text.ToString();//送货信息赋值
            Session["lngopUseraddressId"] = GridViewShippingMethod.GetRowValues(GridViewShippingMethod.FocusedRowIndex, "lngopUseraddressId").ToString();//送货地址ID
            Session["TxtcSCCode"] = TxtcSCCode.Text.ToString(); //发运方式编码赋值
            Session["cdefine9"] = GridViewShippingMethod.GetRowValues(GridViewShippingMethod.FocusedRowIndex, "strConsigneeName").ToString();//收货人姓名,	 
            Session["cdefine12"] = GridViewShippingMethod.GetRowValues(GridViewShippingMethod.FocusedRowIndex, "strConsigneeTel").ToString();//收货人电话,	 		
            Session["cdefine11"] = GridViewShippingMethod.GetRowValues(GridViewShippingMethod.FocusedRowIndex, "strReceivingAddress").ToString();//收货人地址,	 	 
            Session["cdefine10"] = GridViewShippingMethod.GetRowValues(GridViewShippingMethod.FocusedRowIndex, "strCarplateNumber").ToString();//车牌号,	 				
            Session["cdefine1"] = GridViewShippingMethod.GetRowValues(GridViewShippingMethod.FocusedRowIndex, "strDriverName").ToString();//司机姓名,	 		
            Session["cdefine13"] = GridViewShippingMethod.GetRowValues(GridViewShippingMethod.FocusedRowIndex, "strDriverTel").ToString();//司机电话,	 		
            Session["cdefine2"] = GridViewShippingMethod.GetRowValues(GridViewShippingMethod.FocusedRowIndex, "strIdCard").ToString();//司机身份证
            Session["cdefine8"] = GridViewShippingMethod.GetRowValues(GridViewShippingMethod.FocusedRowIndex, "strDistrict").ToString();//配送行政区           
        }
        else
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('请选择送货地址！');</script>");
        }
    }

    protected void ComboShippingMethod_SelectedIndexChanged(object sender, EventArgs e)     //送货信息选择变化,刷新
    {
        string SHFS = ComboShippingMethod.Value.ToString();
        DataTable dt = new DataTable();
        if (SHFS == "自提")
        {
            dt = new BasicInfoManager().DLproc_UserAddressZTBySelGroup(Session["lngopUserId"].ToString());
        }
        else
        {
            dt = new BasicInfoManager().DLproc_UserAddressPSBySelGroup(Session["lngopUserId"].ToString());
        }
        GridViewShippingMethod.DataSource = dt;
        GridViewShippingMethod.DataBind();
    }

    protected void GridViewShippingMethod_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e) //生成Grid的序号,以及计算执行金额
    {
        //if (e.Column.Caption == "序号" && e.IsGetData)
        //{
        //    e.Value = (e.ListSourceRowIndex + 1).ToString();
        //}

        if (e.Column.Caption == "执行金额" && e.IsGetData)
        {
            //e.Value = (e.ListSourceRowIndex + 1).ToString();
            //计算执行金额
            e.Value = Convert.ToDouble(OrderGrid.GetRowValues(e.ListSourceRowIndex, "cInvDefineQTY")) * Convert.ToDouble(OrderGrid.GetRowValues(e.ListSourceRowIndex, "ExercisePrice"));
        }
    }

    protected void GridViewShippingMethod_FocusedRowChanged(object sender, EventArgs e)
    {
        //List<object> values = OrderGrid.GetSelectedFieldValues(new string[] { OrderGrid.KeyFieldName });
        ////List values = ASPxGridView2.GetSelectedFieldValues(new string[] {'数据库中字段'});

        //string data = "";
        //for (int i = 0; i < values.Count; i++)
        //{
        //    data += values[i].ToString() + "\\";
        //}
    }

    //protected void TxtOrderMark_TextChanged(object sender, EventArgs e) //订单备注文本变更赋值
    //{
    //    Session["TxtOrderMark"] = TxtOrderMark.Text.ToString();
    //}

    protected void BtnCustomerOk_Click(object sender, EventArgs e)      //客户信息选择事件
    {
        //20170107，添加检测当前开票单位是否在有效时间内，才能下单，否则提交不了
        //bool IsValidTime = new OrderManager().DL_IsValidTimeBySel(CustomerGrid.GetRowValues(CustomerGrid.FocusedRowIndex, "cCusCode").ToString());
        //if (IsValidTime == false)
        //{
        //    LabHDNR.Text = "￥";
        //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('今天该开票单位的活动时间已关闭！');</script>");
        //    return;
        //}
        //20170120注释
        //DataTable nValidTimeData = new OrderManager().DL_InValidTimeDataBySel(CustomerGrid.GetRowValues(CustomerGrid.FocusedRowIndex, "cCusCode").ToString());
        //if (nValidTimeData.Rows.Count > 0 && nValidTimeData.Rows[0]["Isvaild"].ToString() == "1")
        //{
        //    LabHDNR.Text = "￥  活动金额:" + nValidTimeData.Rows[0][0].ToString() + ",当前剩余可处理车数:" + nValidTimeData.Rows[0][1].ToString() + "车";
        //}
        //else if (nValidTimeData.Rows.Count > 0 && nValidTimeData.Rows[0]["Isvaild"].ToString() == "0")
        //{
        //    LabHDNR.Text = "￥ 请使用其他开票单位进行购物!";
        //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('今天该开票单位的活动时间已关闭！');</script>");
        //}
        //else
        //{
        //    LabHDNR.Text = "￥";
        //}
        LabHDNR.Text = "￥";
        //开票单位名称
        string cCusName = CustomerGrid.GetRowValues(CustomerGrid.FocusedRowIndex, "cCusName").ToString();
        TxtCustomer.Text = cCusName.ToString();
        Session["TxtCustomer"] = TxtCustomer.Text.ToString();
        //开票单位编码
        Session["KPDWcCusCode"] = CustomerGrid.GetRowValues(CustomerGrid.FocusedRowIndex, "cCusCode").ToString();
        //业务员
        TxtSalesman.Text = CustomerGrid.GetRowValues(CustomerGrid.FocusedRowIndex, "cCusPPerson").ToString();
        Session["cCusPPerson"] = TxtSalesman.Text.ToString();
        //重新选择开票单位后,重新读取开票单位的存货单价,并且计算金额,重新绑定表体grid,11-01
        DataTable griddata = new DataTable();
        griddata = (DataTable)Session["ordergrid"];
        //如果不为空,则遍历,给价格重新赋值
        if (griddata != null)
        {
            if (griddata.Rows.Count > 0)
            {
                for (int i = 0; i < griddata.Rows.Count; i++)   //重新取价
                {
                    DataTable iddt = new OrderManager().DLproc_QuasiOrderDetailBySel(griddata.Rows[i]["cInvCode"].ToString(), Session["KPDWcCusCode"].ToString());
                    griddata.Rows[i]["cComUnitPrice"] = iddt.Rows[0]["Quote"].ToString();//报价
                    griddata.Rows[i]["ExercisePrice"] = iddt.Rows[0]["ExercisePrice"].ToString();//执行价格
                    griddata.Rows[i]["cComUnitAmount"] = Convert.ToDecimal(iddt.Rows[0]["Quote"].ToString()) * Convert.ToDecimal(griddata.Rows[i]["cInvDefineQTY"].ToString());//报价金额
                    griddata.Rows[i]["Stock"] = Convert.ToDouble(iddt.Rows[0]["fAvailQtty"].ToString());//可用库存量
                    /*11-10添加*/
                    griddata.Rows[i]["kl"] = Convert.ToDouble(iddt.Rows[0]["Rate"].ToString()); //扣率   18 Rate
                    griddata.Rows[i]["iTaxRate"] = Convert.ToDouble(iddt.Rows[0]["iTaxRate"].ToString()); //销项税率   20 iTaxRate
                    griddata.Rows[i]["cDefine24"] = iddt.Rows[0]["cDefine24"].ToString();//活动类型20170316
                }
                string yxxstring = "商品：";
                for (int i = griddata.Rows.Count - 1; i >= 0; i--)   //遍历允限销
                {
                    bool yxx = new OrderManager().DLproc_cInvCodeIsBeLimitedBySel(griddata.Rows[i]["cInvCode"].ToString(), Session["KPDWcCusCode"].ToString(), 1);
                    if (yxx == false)
                    {
                        yxxstring = yxxstring + griddata.Rows[i]["cInvName"].ToString() + "，";
                        griddata.Rows.RemoveAt(i);
                    }
                }
                if (yxxstring != "商品：")
                {
                    for (int i = 0; i < griddata.Rows.Count; i++)   //  更新序号
                    {
                        griddata.Rows[i]["irowno"] = i + 1;
                    }
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('" + yxxstring + " 未找到该开票单位的商品信息！已自动删除商品！');</script>");
                }
                //重新绑定
                Inventory.DataBind();
                OrderGrid.DataSource = griddata;
                OrderGrid.DataBind();
                Session["ordergrid"] = griddata;
            }
        }
        //重新获取信用额度
        DataTable CusCreditDt = new DataTable();
        CusCreditDt = new OrderManager().DLproc_getCusCreditInfo(Session["KPDWcCusCode"].ToString());//默认登录客户编码
        TxtCusCredit.Text = CusCreditDt.Rows[0]["iCusCreLine"].ToString();
        TxtCBLX.Text = CusCreditDt.Rows[0]["cdiscountname"].ToString();         //2016-04-26,添加 酬宾类型 显示
        //2016-04-26添加,增加一个文本框,用于显示信用额,现金顾客
        if (TxtCusCredit != null && Convert.ToDouble(TxtCusCredit.Text) == -99999999)
        {
            TxtCusCredit1.Text = "现金顾客";
        }
        else
        {
            TxtCusCredit1.Text = TxtCusCredit.Text;
        }


    }

    protected void Btncdefine3Ok_Click(object sender, EventArgs e)      //车型信息选择事件
    {
        //车型名称
        string cdefine3 = cdefine3Grid.GetRowValues(cdefine3Grid.FocusedRowIndex, "cValue").ToString();
        Txtcdefine3.Text = cdefine3.ToString();
        Session["cdefine3"] = cdefine3.ToString();
    }

    public string GetNewBillNo()
    {

        return "";
    }

    protected void BtnSaveOrder_Click(object sender, EventArgs e)   //保存并提交订单按钮事件,生成正式订单☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆
    {

        ////创建datatable数据;
        DataTable griddata = new DataTable();
        griddata = (DataTable)Session["ordergrid"];

        #region 检测数据有效性

        #region 2016-09-13,检查是否存在未参照完的酬宾订单的商品
        string Strpreorderleft = "行：";
        if (griddata.Rows.Count > 0)
        {
            for (int i = 0; i < griddata.Rows.Count; i++)
            {
                DataTable preorderleft = new OrderManager().DLproc_PerOrderCinvcodeLeftBySel(Session["lngopUserExId"].ToString(), Session["lngopUserId"].ToString(), Session["KPDWcCusCode"].ToString(), griddata.Rows[i][0].ToString());
                if (Convert.ToDouble(preorderleft.Rows[0][0].ToString()) > 0)
                {
                    Strpreorderleft = Strpreorderleft + griddata.Rows[i]["irowno"].ToString() + "," + griddata.Rows[i]["cInvName"].ToString() + "数量：" + preorderleft.Rows[0][0].ToString() + ";";
                }
            }
        }
        //提示有未完成的酬宾订单
        if (Strpreorderleft.ToString() != "行：")
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('" + Strpreorderleft.ToString() + "存在未参照完的酬宾订单，请先将酬宾订单的商品参照完毕后在普通订单中购买该商品！" + "');</script>");
            return;
        }
        #endregion


        #region 检测grid是否处于编辑状态,请先保存grid,(无效方法,待验证)
        //if (OrderGrid.IsEditing)
        //{
        //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('请先保存订单明细表！');</script>");
        //    return;
        //}
        #endregion

        //1,检测是否必填
        if (TxtOrderShippingMethod.Text == "")
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('请选择送货方式！');</script>");
            return;
        }
        if (TxtcSCCode.Text == "")
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('请选择发运方式！');</script>");
            return;
        }
        if (TxtOrderShippingMethod.Text.Substring(0, 2).ToString() == "自提" && string.IsNullOrEmpty(Txtztadd.Text))
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('自提必须选择行政区！');</script>");
            return;
        }
        //if (TxtSalesman.Text == "")
        //{
        //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('请填写业务员！');</script>");
        //    return;
        //}
        if (Txtcdefine3.Text == "")
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('请选择车型！');</script>");
            return;
        }
        //if (TxtcSTCode.Text == "样品资料" && TxtRelateU8NO.Text == "")
        //{
        //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('请选择样品资料关联的正式订单号！');</script>");
        //    return;
        //}
        //if (DeliveryDate.Text == "")
        //{
        //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('请选择交货日期！');</script>");
        //    return;
        //}
        //2,检测是否有数据
        //if (TxtcSTCode.Text != "样品资料")
        //{
        if (Convert.ToDateTime(DeliveryDate.Value) <= DateTime.Now && DeliveryDate.Value != null) //检验交货日期
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('提货时间不能小于当前时间！');</script>");
            return;
        }
        //}
        if (griddata.Rows.Count <= 0)   //检查是否有商品明细
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('请先添加商品到订单明细表,保存后再提交订单！');</script>");
            return;
        }

        //20170107，添加检测当前开票单位是否在有效时间内，才能下单，否则提交不了
        //bool IsValidTime = new OrderManager().DL_IsValidTimeBySel(Session["KPDWcCusCode"].ToString());
        //if (IsValidTime == false)
        //{
        //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('今天该开票单位的活动时间已关闭！');</script>");
        //    return;
        //}

        //20170120注释
        //DataTable nValidTimeData = new OrderManager().DL_InValidTimeDataBySel(CustomerGrid.GetRowValues(CustomerGrid.FocusedRowIndex, "cCusCode").ToString());
        //if (nValidTimeData.Rows.Count > 0 && nValidTimeData.Rows[0]["Isvaild"].ToString() == "1")
        //{
        //    LabHDNR.Text = "￥  活动金额:" + nValidTimeData.Rows[0][0].ToString() + ",当前剩余可处理车数:" + nValidTimeData.Rows[0][1].ToString() + "车";
        //}
        //else if (nValidTimeData.Rows.Count > 0 && nValidTimeData.Rows[0]["Isvaild"].ToString() == "0")
        //{
        //    LabHDNR.Text = "￥ 请使用其他开票单位进行购物!";
        //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('今天该开票单位的活动时间已关闭！');</script>");
        //    return;
        //}
        //else
        //{
        //    LabHDNR.Text = "￥";
        //}
        LabHDNR.Text = "￥";
        //检测价格等级变化
        #region 检测价格等级变化
        if (griddata.Rows.Count > 0)
        {
            int PriceUpdateCount = 0;
            for (int i = 0; i < griddata.Rows.Count; i++)
            {
                DataTable dtprice = new OrderManager().DLproc_QuasiPriceBySel(griddata.Rows[i]["cInvCode"].ToString(), Session["KPDWcCusCode"].ToString());
                if (dtprice.Rows.Count > 0)
                {
                    //报价更新,同时更新报价金额
                    if (Convert.ToDouble(dtprice.Rows[0]["Quote"].ToString()) != Convert.ToDouble(griddata.Rows[i]["cComUnitPrice"].ToString()))
                    {
                        double qty = Convert.ToDouble(griddata.Rows[i]["cComUnitQTY"].ToString());
                        double price = Math.Round(Convert.ToDouble(dtprice.Rows[0]["Quote"].ToString()), 6);
                        DataRow dr = griddata.Rows[i];
                        dr.BeginEdit();
                        dr[13] = price;
                        dr[14] = price * qty;
                        dr.EndEdit();
                        PriceUpdateCount = PriceUpdateCount + 1;
                    }
                    //执行价格更新，20170316更新活动类型
                    if (Convert.ToDouble(dtprice.Rows[0]["ExercisePrice"].ToString()) != Convert.ToDouble(griddata.Rows[i]["ExercisePrice"].ToString()))
                    {
                        //Math.Round(Convert.ToDouble(dr[16].ToString()), 6);
                        //double qty = Convert.ToDouble(griddata.Rows[i]["cComUnitQTY"].ToString());
                        double price = Math.Round(Convert.ToDouble(dtprice.Rows[0]["ExercisePrice"].ToString()), 6);
                        DataRow dr = griddata.Rows[i];
                        dr.BeginEdit();
                        dr[16] = price;
                        dr[24] = dtprice.Rows[0]["cDefine24"].ToString();
                        dr.EndEdit();
                        PriceUpdateCount = PriceUpdateCount + 1;
                    }
                }
            }
            if (PriceUpdateCount > 0)
            {
                Session["ordergrid"] = griddata;
                OrderGrid.DataSource = griddata;
                OrderGrid.DataBind();
                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('注意：商品价格已更新，请核对后重新提交订单！');</script>");
                //return;
            }
        }
        #endregion

        //3,检测信用额度是否满足,并且检测是否存在数量为0的商品信息
        double CusCredit = 0;
        Hashtable ht = (Hashtable)Session["SysSetting"];
        for (int i = 0; i < griddata.Rows.Count; i++)
        {
            if (0 == Convert.ToDouble(griddata.Rows[i]["cInvDefineQTY"].ToString()))
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('第" + (i + 1) + "行,存在数量为0的商品,请检查！');</script>");
                return;
            }
            //CusCredit += Convert.ToDouble(griddata.Rows[i]["cComUnitAmount"].ToString());//原取值方式,报价
            //2016-01-18修改,根据SysSetting中的IsExercisePrice的参数来取值

            if (ht["IsExercisePrice"].ToString() == "0")   //报价金额
            {
                CusCredit += Convert.ToDouble(griddata.Rows[i]["cComUnitAmount"].ToString());
            }
            else//执行价金额
            {
                CusCredit += Convert.ToDouble(griddata.Rows[i]["ExercisePrice"].ToString()) * Convert.ToDouble(griddata.Rows[i]["cInvDefineQTY"].ToString());//执行单价*基本单位汇总数量
            }

        }
        //信用额度,20151129变更,增加临时授权,Customer中的cCusPostCode ,1为临时授权,授权后设置为 null(恢复操作在生成网上订单的存储过程中完成)
        DataTable ExtraCredit = new DataTable();
        ExtraCredit = new OrderManager().DL_ExtraCreditBySel(Session["KPDWcCusCode"].ToString());
        if (ExtraCredit.Rows.Count > 0)
        {

        }
        else
        {
            DataTable CusCreditDt = new OrderManager().DLproc_getCusCreditInfo(Session["KPDWcCusCode"].ToString());//开票单位编码,获取当前单位的信用余额
            TxtCusCredit.Text = CusCreditDt.Rows[0]["iCusCreLine"].ToString();
            TxtCBLX.Text = CusCreditDt.Rows[0]["cdiscountname"].ToString();         //2016-04-05,添加 酬宾类型 显示
            //2016-04-26添加,增加一个文本框,用于显示信用额,现金顾客
            if (TxtCusCredit != null && Convert.ToDouble(TxtCusCredit.Text) == -99999999)
            {
                TxtCusCredit1.Text = "现金顾客";
            }
            else
            {
                TxtCusCredit1.Text = TxtCusCredit.Text;
            }
            //TxtCusCredit.Text = CusCreditDt.Rows[0]["iCusCreLine"].ToString();//信用额
            if (CusCredit > Convert.ToDouble(CusCreditDt.Rows[0]["iCusCreLine"].ToString()) && Convert.ToDouble(CusCreditDt.Rows[0]["iCusCreLine"].ToString()) != -99999999)
            //if (CusCredit > Convert.ToDouble(TxtCusCredit.Text) && Convert.ToDouble(TxtCusCredit.Text) != -99999999)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('已经超过您开票单位的信用额度:" + Convert.ToString((Convert.ToDouble(CusCreditDt.Rows[0]["iCusCreLine"].ToString()) - CusCredit)) + "！');</script>");
                return;
            }
        }

        //4,检测可用库存量是否正确,先更新库存,再调用OrderGrid_RowValidating(暂时无法调用,遍历解决)
        //if (TxtcSTCode.Text != "样品资料")
        //{
        DataTable stockdt = new DataTable();
        bool d = false;
        for (int i = 0; i < griddata.Rows.Count; i++)
        {
            stockdt = new OrderManager().DLproc_QuasiOrderDetailBySel(griddata.Rows[i]["cInvCode"].ToString(), Session["cCusCode"].ToString());
            griddata.Rows[i]["Stock"] = stockdt.Rows[0]["fAvailQtty"].ToString();
            if (Convert.ToDouble(griddata.Rows[i]["Stock"].ToString()) < Convert.ToDouble(griddata.Rows[i]["cInvDefineQTY"].ToString()))
            {
                //更新背景色
                d = true;
            }
        }
        //先绑定数据,ordergrid表体绑定最新库存数据
        OrderGrid.DataSource = griddata;
        OrderGrid.DataBind();
        //如果不满足库存检测,则提示并退出
        if (d)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('超过可用库存量！可用库存量已更新,请重新调整订单商品数量！');</script>");
            return;
        }
        //}

        #endregion

        //检测2016-03-08,检测顾客名称和编码是否一致
        DataTable checkcuscodeandcusname = new OrderManager().DL_CheckCuscodeAndCusnameBySel(Session["KPDWcCusCode"].ToString(), TxtCustomer.Text);
        if (checkcuscodeandcusname.Rows.Count < 1)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('开票单位名称与编码不符合，请重新选择该开票单位！');</script>");
            return;
        }
        //return;

        #region 插入表头数据
        string cdiscountname = TxtCBLX.Text.ToString(); //2016-04-05,添加字段,(酬宾类型)
        string lngopUserId = Session["lngopUserId"].ToString(); //用户id
        string datCreateTime = TxtBillDate.Text.ToString(); //创建日期
        int bytStatus = 1;  //单据状态
        string ccuscode = Session["KPDWcCusCode"].ToString();   //开票单位编码
        string cdefine9 = Session["cdefine9"].ToString();   //自定义项1,收货人姓名,cDefine9
        string cdefine12 = Session["cdefine12"].ToString();   //自定义项2,收货人电话,cDefine12
        string cdefine11 = TxtOrderShippingMethod.Text.ToString();   //自定义项11,收货人地址,cDefine11
        //string cDefine11 = Session["lngopUseraddressId"].ToString();
        string cdefine1 = Session["cdefine1"].ToString();   //自定义项8,司机姓名,cDefine1
        string cdefine13 = Session["cdefine13"].ToString();   //自定义项9,司机电话,cDefine13
        string cdefine2 = Session["cdefine2"].ToString();  //自定义项10,司机身份证,cDefine2
        string cdefine3 = Txtcdefine3.Text.ToString();  //自定义项3,汽车类型,cdefine3
        string cdefine10 = Session["cdefine10"].ToString();  //自定义项12,车牌号,cDefine10
        string dpredatebt = TxtBillDate.Text.ToString();    //预发货日期 
        string dpremodatebt = TxtBillDate.Text.ToString();  //预完工日期 
        string ccusname = TxtCustomer.Text; //客户名称             
        string strRemarks = TxtOrderMark.Text.ToString();   //备注 
        string cpersoncode = TxtSalesman.Text.ToString();   //业务员编码
        string cSCCode = "00";    //发运方式编码
        string cdefine8 = "";       //自提行政区
        if (TxtOrderShippingMethod.Text.Substring(0, 2).ToString() == "自提")
        {
            cdefine8 = Txtztadd.Text.ToString(); //自提行政区
        }
        else
        {
            if (Session["cdefine8"] != null)
            {
                cdefine8 = Session["cdefine8"].ToString();//配送行政区
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('配送行政区丢失，请重新选择一次配送信息！');</script>");
                return;
            }

        }

        if (Session["TxtcSCCode"].ToString() == "配送" || Session["TxtcSCCode"].ToString() == "01")     //发运方式编码,00:自提,01:厂车配送
        {
            Session["TxtcSCCode"] = "01";
            cSCCode = "01";
        }
        else
        {
            Session["TxtcSCCode"] = "00";
        }
        //11-18新增字段赋值
        string strLoadingWays = TxtLoadingWays.Text.ToString();     //装车方式
        string cSTCode = "00";     //销售类型编码    
        //if (TxtcSTCode.Text == "普通销售")
        //{
        //    cSTCode = "00";
        //}
        //else
        //{
        //    cSTCode = "01";
        //}
        string datDeliveryDate = ""; //交货日期
        string lngopUseraddressId = Session["lngopUseraddressId"].ToString();
        //11-26增加,样品资料对应正式订单号
        //string strTxtRelateU8NO = TxtRelateU8NO.Text;
        string strTxtRelateU8NO = "";
        if (DeliveryDate.Value == null)
        {
            datDeliveryDate = DateTime.Now.ToString();
        }
        else
        {
            datDeliveryDate = DeliveryDate.Value.ToString();
            //if (Convert.ToDateTime(DateTime.Now.ToString()) > Convert.ToDateTime(datDeliveryDate))
            //{
            //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('提货时间不能大于当前时间！');</script>");
            //    return;
            //}
        }
        string lngopUserExId = Session["lngopUserExId"].ToString();
        string strAllAcount = Session["strAllAcount"].ToString();
        //插入表头数据,DL表中
        OrderInfo oi = new OrderInfo(lngopUserId, datCreateTime, bytStatus, strRemarks, ccuscode, cdefine1, cdefine2, cdefine3, cdefine9, cdefine10, cdefine11, cdefine12, cdefine13, ccusname, cpersoncode, cSCCode, datDeliveryDate, strLoadingWays, cSTCode, lngopUseraddressId, strTxtRelateU8NO, cdiscountname, cdefine8, lngopUserExId, strAllAcount);
        DataTable lngopOrderIdDt = new DataTable();
        lngopOrderIdDt = new OrderManager().DLproc_NewOrderByIns(oi);
        #endregion

        #region 插入表体数据
        #region 遍历并且插入表体数据
        //////创建datatable数据;
        //DataTable griddata = new DataTable();
        //griddata = (DataTable)Session["ordergrid"];
        int lngopOrderId = Convert.ToInt32(lngopOrderIdDt.Rows[0]["lngopOrderId"].ToString());   //订单id,从表头中获取:表头插入后,返回表头id
        string strBillNo = lngopOrderIdDt.Rows[0]["strBillNo"].ToString();   //订单编号,插入表头数据时自动生成,用于反馈给用户
        string cinvcode = "";   //存货编码
        double iquantity = 1;   //存货数量
        double inum = 1;        //辅计量数量   
        double iquotedprice = 1;//报价 
        double iunitprice = 1;  //原币无税单价
        double itaxunitprice = 1;//原币含税单价
        double imoney = 1;      //原币无税金额 
        double itax = 1;        //原币税额 
        double isum = 1;        //原币价税合计
        double idiscount = 1;   //原币折扣额 
        double inatunitprice = 1;//本币无税单价
        double inatmoney = 1;   //本币无税金额
        double inattax = 1;     //本币税额 
        double inatsum = 1;     //本币价税合计
        double inatdiscount = 1;   //本币折扣额  
        double kl = 1;          //扣率 
        double itaxrate = 16;    //税率 
        string cDefine22 = "";  //表体自定义项22,包装量
        double iinvexchrate = 1;//换算率 
        string cunitid = "";    //计量单位编码
        int irowno = 1; //行号,从1开始,自增长
        string cinvname = "";   //存货名称 
        //11-09新增保存字段
        string cComUnitName = "";       //基本单位名称
        string cInvDefine1 = "";        //大包装单位名称         
        string cInvDefine2 = "";        //小包装单位名称 
        double cInvDefine13 = 0;       //大包装换算率
        double cInvDefine14 = 0;       //小包装换算率
        string unitGroup = "";          //单位换算率组
        double cComUnitQTY = 0;        //基本单位数量
        double cInvDefine1QTY = 0;     //大包装单位数量
        double cInvDefine2QTY = 0;     //小包装单位数量
        string cn1cComUnitName = "";    //销售单位名称
        string cDefine24 = "";    //活动类型

        for (int i = 0; i < griddata.Rows.Count; i++)
        {
            //string strName = griddata.Rows[i]["字段名"].ToString();
            DataRow dr = griddata.Rows[i];  //定义当前行数据
            //赋值
            //irowno = i + 1;             //行号
            irowno = Convert.ToInt32(dr[22].ToString());     //行号
            cinvcode = dr[0].ToString();//存货编码
            cinvname = dr[1].ToString();//存货名称
            iquantity = Convert.ToDouble(dr[12].ToString());   //存货数量
            iquotedprice = Math.Round(Convert.ToDouble(dr[13].ToString()), 6);//报价,保留5位小数,四舍五入
            kl = Convert.ToDouble(dr[18].ToString());          //扣率
            //11-09新增字段赋值
            cComUnitName = dr["cComUnitName"].ToString();       //基本单位名称
            cInvDefine1 = dr["cInvDefine1"].ToString();        //大包装单位名称         
            cInvDefine2 = dr["cInvDefine2"].ToString();        //小包装单位名称 
            cInvDefine13 = Convert.ToDouble(dr["cInvDefine13"].ToString());       //大包装换算率
            cInvDefine14 = Convert.ToDouble(dr["cInvDefine14"].ToString());       //小包装换算率
            unitGroup = dr["unitGroup"].ToString();          //单位换算率组
            cComUnitQTY = Convert.ToDouble(dr["cComUnitQTY"].ToString());        //基本单位数量
            cInvDefine1QTY = Convert.ToDouble(dr["cInvDefine1QTY"].ToString());     //大包装单位数量
            cInvDefine2QTY = Convert.ToDouble(dr["cInvDefine2QTY"].ToString());     //小包装单位数量
            cDefine24 = dr["cDefine24"].ToString();     //活动类型

            //计算销售单位(辅助)的换算率
            if (dr[21].ToString() == dr[4].ToString())  //大包装单位换算率
            {
                iinvexchrate = Convert.ToDouble(dr[6].ToString());
            }
            else if (dr[21].ToString() == dr[5].ToString()) //小包装单位换算率
            {
                iinvexchrate = Convert.ToDouble(dr[7].ToString());
            }
            else  //无换算单位
            {
                iinvexchrate = 1;//换算率 
            }
            //计算赋值
            //换算结果:辅计量数量 =存货数量/换算率,四舍五入,保留两位小数
            inum = Math.Round(iquantity / iinvexchrate, 2);  //辅计量数量 

            /*VER:V1,11-03*/
            //iunitprice = Math.Round(Convert.ToDouble(dr[16].ToString()) / 1.17, 4);  //原币无税单价,=原币含税单价/1.17,保留四位小数,四舍五入
            //itaxunitprice = Convert.ToDouble(dr[16].ToString());//原币含税单价,即执行价格dt.Columns.Add("ExercisePrice"); //基本单位单价(执行价格)   16
            //imoney = iunitprice * iquantity;      //原币无税金额 =原币无税单价*数量
            //itax = Math.Round((itaxunitprice - iunitprice) * iquantity, 2);        //原币税额 ;税额=(含税单价-无税单价)*数量 [四舍五入],保留两位小数
            //isum = Math.Round(itaxunitprice * iquantity, 5);        //原币价税合计;=原币含税单价*数量,保留四位小数,四舍五入
            //idiscount = (iquotedprice - itaxunitprice) * iquantity;   //原币折扣额 ;=(报价 -原币含税单价)*数量

            /*VER:V2,11-04*/
            //金额=单价*数量,保留两位
            //税额=金额/1.17*0.17 保留2位
            //无税金额=金额-税额,保留两位
            //无税单价=无税金额/数量,保留5位
            //折扣额=报价*数量-金额,保留两位
            itaxunitprice = Math.Round(Convert.ToDouble(dr[16].ToString()), 6);//原币含税单价,即执行价格dt.Columns.Add("ExercisePrice"); //基本单位单价(执行价格)   16,保留5位
            isum = Math.Round(itaxunitprice * iquantity, 2);        //金额,原币价税合计=原币含税单价*数量,保留2位小数,四舍五入
            itax = Math.Round(isum / 1.17 * 0.17, 2);        //原币税额 ;税额=金额/1.17*0.17 保留2位, 四舍五入
            imoney = Math.Round(isum - itax, 2);      //原币无税金额 =金额-税额,保留2位,四舍五入
            iunitprice = Math.Round(imoney / iquantity, 6);  //原币无税单价=无税金额/数量,保留5位小数,四舍五入                 
            idiscount = Math.Round(iquotedprice * iquantity - isum, 2);   //原币折扣额=报价*数量-金额,保留两位

            inatunitprice = iunitprice;//本币无税单价
            inatmoney = imoney;   //本币无税金额
            inattax = itax;     //本币税额 
            inatsum = isum;     //本币价税合计
            inatdiscount = idiscount;   //本币折扣额 

            itaxrate = Convert.ToDouble(dr[20].ToString());    //税率 
            cDefine22 = dr[15].ToString();  //表体自定义项22,包装量

            cunitid = dr[19].ToString();    //基本计量单位编码
            cn1cComUnitName = dr["cn1cComUnitName"].ToString();    //销售单位名称
            //插入表体数据
            OrderInfo oiEntry = new OrderInfo(lngopOrderId, cinvcode, iquantity, inum, iquotedprice, iunitprice, itaxunitprice, imoney, itax, isum, inatunitprice, inatmoney, inattax, inatsum, kl, itaxrate, cDefine22, iinvexchrate, cunitid, irowno, cinvname, idiscount, inatdiscount, cComUnitName, cInvDefine1, cInvDefine2, cInvDefine13, cInvDefine14, unitGroup, cComUnitQTY, cInvDefine1QTY, cInvDefine2QTY, cn1cComUnitName, cDefine24);
            bool c = new OrderManager().DLproc_NewOrderDetailByIns(oiEntry);
        }

        #endregion
        #endregion

        #region 集成IM消息
        byte[] data = new byte[1024];
        //设置服务IP，设置TCP端口号
        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("192.168.0.251"), 9001);
        //定义网络类型，数据连接类型和网络协议UDP
        Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        //发送消息

        string welcome = "0;" + ccuscode + ";" + ccusname + ";" + strBillNo + ";新增普通订单"; //根据Dl_opOrderBillNoSetting表中定义IMType类型
        data = Encoding.UTF8.GetBytes(welcome);
        server.SendTo(data, data.Length, SocketFlags.None, ipep);
        #endregion

        #region 保存后清空grid数据,并提示
        //清空数据
        if (Session["ordergrid"] != null)
        {
            Session.Contents.Remove("ordergrid");
        }
        Session.Contents.Remove("TxtOrderShippingMethod");
        Session.Contents.Remove("TxtcSCCode");
        TxtOrderShippingMethod.Text = "";
        TxtcSCCode.Text = "";
        TxtCusCredit.Text = "";
        Txtztadd.Text = "";


        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单 " + strBillNo + " 已经提交,请在已提交订单中查询订单处理进度!');</script>");
        Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script>alert('订单" + strBillNo + " 已经提交,请在已提交订单中查询订单处理进度！');{location.href='NewOrder.aspx'}</script>");
        #endregion


    }

    protected void OrderGrid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)    //表体表格数据更新，保存按钮&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
    {
        //int index = OrderGrid.DataKeys[e.NewEditIndex].Value;//获取主键的值
        //取值 用e.NewValues[索引]
        //string lngopUseraddressId = Convert.ToString(e.Keys[0]);

        ////创建datatable数据;
        DataTable griddata = new DataTable();
        griddata = (DataTable)Session["ordergrid"];
        //序号,行号,当前操作行,
        int i = Convert.ToInt32(e.NewValues["irowno"].ToString());
        DataRow dr = griddata.Rows[i - 1];

        Double sRate = Convert.ToDouble(dr[7].ToString());//获取小包装换算率
        Double bRate = Convert.ToDouble(dr[6].ToString());//获取大包装换算率
        Double cComUnitPrice = Convert.ToDouble(dr[13].ToString());  //基本单位单价
        //检测0,如果为空,则默认为0
        if (e.NewValues["cComUnitQTY"] == null)
        {
            e.NewValues["cComUnitQTY"] = 0;
        }
        if (e.NewValues["cInvDefine2QTY"] == null)
        {
            e.NewValues["cInvDefine2QTY"] = 0;
        }
        if (e.NewValues["cInvDefine1QTY"] == null)
        {
            e.NewValues["cInvDefine1QTY"] = 0;
        }
        Double cComUnitQTY = Convert.ToDouble(e.NewValues["cComUnitQTY"].ToString()); //基本单位数量
        Double cInvDefine2QTY = Convert.ToDouble(e.NewValues["cInvDefine2QTY"].ToString());  //小包装数量
        Double cInvDefine1QTY = Convert.ToDouble(e.NewValues["cInvDefine1QTY"].ToString());  //大包装数量

        //包装总量,基本单位
        Double amount = Convert.ToDouble((cComUnitQTY + cInvDefine2QTY * sRate + cInvDefine1QTY * bRate).ToString("N2"));
        //总金额
        Double cComUnitAmount = amount * cComUnitPrice;
        //包装结果
        string pack = Math.Floor(amount / bRate) + dr[4].ToString() + Math.Floor((((amount * 10 * 10) % (bRate * 10 * 10)) / (sRate * 10 * 10))) + dr[5].ToString() + Math.Floor((((amount * 10 * 10) % (sRate * 10 * 10)) / 10) / 10) + dr[3].ToString(); //包装量换算结果  15

        //更新
        ////创建datatable数据;
        dr.BeginEdit();
        dr[9] = cComUnitQTY.ToString();
        dr[10] = cInvDefine1QTY.ToString();
        dr[11] = cInvDefine2QTY.ToString();
        dr[12] = amount.ToString();
        dr[14] = cComUnitAmount.ToString();
        dr[15] = pack.ToString();
        dr.EndEdit();
        //退出编辑
        OrderGrid.CancelEdit();//结束编辑状态
        e.Cancel = true;
        //重新绑定
        OrderGrid.DataSource = griddata;
        OrderGrid.DataBind();
        Session["ordergrid"] = griddata;

        //存储数据至数据库中,待下次恢复用
        //存储表头信息

        //Session["AutoSaveInfo"] = "(于" + DateTime.Now.ToLocalTime().ToString() + "自动保存)";
        //if (Session["AutoSaveInfo"] != null)
        //{
        //    //OrderGrid.SettingsCommandButton.UpdateButton.Text = "保存购物信息           " + Session["AutoSaveInfo"].ToString();
        //    OrderGrid.SettingsText.Title = "订单明细表" + Session["AutoSaveInfo"].ToString();
        //}    

    }

    protected void OrderGrid_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)    //保存前,验证grid表体数据有效性
    {
        OrderGrid.JSProperties.Remove("cpAlertMsg");//先清空
        OrderGrid.JSProperties["cpAlertMsg"] = "";
        //检测0是否输入空值
        //if (e.NewValues["cComUnitQTY"] == null || e.NewValues["cInvDefine2QTY"] == null || e.NewValues["cInvDefine1QTY"] == null)
        //{
        //    e.RowError = "保存失败!数量不能为空!";
        //    OrderGrid.JSProperties.Remove("cpAlertMsg");//先清空
        //    OrderGrid.JSProperties["cpAlertMsg"] = "保存失败!数量不能为空!";
        //    return;
        //}
        //检测0,如果为空,则默认为0
        if (e.NewValues["cComUnitQTY"] == null)
        {
            e.NewValues["cComUnitQTY"] = 0;
        }
        if (e.NewValues["cInvDefine2QTY"] == null)
        {
            e.NewValues["cInvDefine2QTY"] = 0;
        }
        if (e.NewValues["cInvDefine1QTY"] == null)
        {
            e.NewValues["cInvDefine1QTY"] = 0;
        }
        //检测1,基本单位是否输入正确
        if (e.NewValues["cComUnitName"].ToString() == "米")
        {
            if (Convert.ToDouble((Convert.ToDouble(e.NewValues["cComUnitQTY"].ToString()) * 1000) % (Convert.ToDouble(e.NewValues["cInvDefine14"].ToString()) * 1000)).ToString("0.00") != "0.00")
            {
                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script>alert('请输入正确的倍数！');</script>");
                //e.Errors.Add(OrderGrid.Columns["cComUnitQTY"], "qingshuru" + e.NewValues["cInvDefine14"].ToString() + "debeishu!");
                e.RowError = "请输入" + e.NewValues["cInvDefine14"].ToString() + "的倍数!";
                //throw new Exception("请输入" + e.NewValues["cInvDefine14"].ToString() + "的倍数!");
                OrderGrid.JSProperties.Remove("cpAlertMsg");//先清空
                OrderGrid.JSProperties["cpAlertMsg"] = "保存失败!请输入" + e.NewValues["cInvDefine14"].ToString() + "的倍数!";
                return;
            }
        }

        //检测2,库存可用量是否正确
        //if (TxtcSTCode.Text != "样品资料")
        //{
        if (Convert.ToDouble(e.NewValues["cComUnitQTY"].ToString()) + Convert.ToDouble(e.NewValues["cInvDefine1QTY"].ToString()) * Convert.ToDouble(e.NewValues["cInvDefine13"].ToString()) + Convert.ToDouble(e.NewValues["cInvDefine2QTY"].ToString()) * Convert.ToDouble(e.NewValues["cInvDefine14"].ToString()) > Convert.ToDouble(e.NewValues["Stock"].ToString()))
        //if (Convert.ToDouble(e.NewValues["cInvDefineQTY"].ToString()) > Convert.ToDouble(e.NewValues["Stock"].ToString())) 
        {
            e.Errors.Add(OrderGrid.Columns["cInvDefineQTY"], "库存不足!");//库存不足
            e.RowError = "库存不足!";
            OrderGrid.JSProperties.Remove("cpAlertMsg");//先清空
            OrderGrid.JSProperties["cpAlertMsg"] = "库存不足!";
            return;
            //throw new Exception("库存不足!");
        }
        //}

        //检测3,检测是否存在数量为0的存货信息
        //string strSplit = Regex.Replace(e.NewValues["cInvDefineQTY"].ToString(), "[0-9]", "", RegexOptions.IgnoreCase);
        if (0 == Convert.ToDouble(e.NewValues["cComUnitQTY"].ToString()) + Convert.ToDouble(e.NewValues["cInvDefine1QTY"].ToString()) + Convert.ToDouble(e.NewValues["cInvDefine2QTY"].ToString()))
        {
            e.Errors.Add(OrderGrid.Columns["cInvDefineQTY"], "check qty!");//存在数量为0的商品,请检查
            e.RowError = "存在数量为0的商品,请检查!";
            OrderGrid.JSProperties.Remove("cpAlertMsg");//先清空
            OrderGrid.JSProperties["cpAlertMsg"] = "存在数量为0的商品,请检查!";
            return;
            //throw new Exception("存在数量为0的商品,请检查!");
        }
        OrderGrid.JSProperties.Remove("cpAlertMsg");//先清空
        OrderGrid.JSProperties["cpAlertMsg"] = "订单明细保存成功!";

    }

    protected void BtnCombocSTCodeOk_Click(object sender, EventArgs e)  //选择销售类型
    {
        //string cSTCode = CombocSTCodeGridView.GetRowValues(CombocSTCodeGridView.FocusedRowIndex, "cSTCode").ToString();
        //TxtcSTCode.Text = cSTCode.ToString();
        ////Session["cSTCode"] = cSTCode.ToString();

        //if (TxtcSTCode.Text.ToString() == "普通销售")
        //{
        //    Session["cSTCode"] = "00";
        //    TxtRelateU8NO.Text = "";
        //    Label1.Visible = false;
        //    TxtRelateU8NO.Visible = false;
        //    BtnTxtRelateU8NOChose.Visible = false;
        //    //启用选择按钮
        //    ASPxButton2.Visible = true;
        //    ASPxButton1.Visible = true;
        //    ASPxButton3.Visible = true;
        //    DeliveryDate.ReadOnly = false;
        //}
        //else
        //{
        //    Session["cSTCode"] = "01";
        //    //绑定 TxtRelateU8NO数据
        //    DataTable RelateU8NO = new SearchManager().DL_ComboCustomerU8NOBySel(Session["cCusCode"].ToString());
        //    TxtRelateU8NOGridView.DataSource = RelateU8NO;
        //    TxtRelateU8NOGridView.DataBind();
        //    Label1.Visible = true;
        //    TxtRelateU8NO.Visible = true;
        //    BtnTxtRelateU8NOChose.Visible = true;
        //    //禁用选择按钮
        //    ASPxButton2.Visible = false;
        //    ASPxButton1.Visible = false;
        //    ASPxButton3.Visible = false;
        //    DeliveryDate.ReadOnly = true;
        //}

        //if (Session["ordergrid"] != null)
        //{
        //    Session.Remove("ordergrid");

        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("cInvCode"); //编码    0
        //    dt.Columns.Add("cInvName"); //名称    1
        //    dt.Columns.Add("cInvStd");  //规格    2    
        //    dt.Columns.Add("cComUnitName"); //基本单位  3
        //    dt.Columns.Add("cInvDefine1"); //大包装单位  4
        //    dt.Columns.Add("cInvDefine2"); //小包装单位  5
        //    dt.Columns.Add("cInvDefine13");  //大包装换算率   6  
        //    dt.Columns.Add("cInvDefine14"); //小包装换算率    7
        //    dt.Columns.Add("UnitGroup"); //单位换算率组   8     
        //    dt.Columns.Add("cComUnitQTY"); //基本单位数量 9
        //    dt.Columns.Add("cInvDefine1QTY"); //大包装单位数量 10
        //    dt.Columns.Add("cInvDefine2QTY"); //小包装单位数量 11
        //    dt.Columns.Add("cInvDefineQTY"); //包装量数量汇总,包装量  12
        //    dt.Columns.Add("cComUnitPrice"); //基本单位单价(报价)   13
        //    dt.Columns.Add("cComUnitAmount"); //基本单位金额  14
        //    dt.Columns.Add("pack"); //包装量换算结果  15
        //    dt.Columns.Add("ExercisePrice"); //基本单位单价(执行价格)   16
        //    dt.Columns.Add("Stock"); //可用库存量   17
        //    dt.Columns.Add("kl"); //扣率   18
        //    dt.Columns.Add("cComUnitCode"); //基本单位编码   19
        //    dt.Columns.Add("iTaxRate"); //销项税率   20
        //    dt.Columns.Add("cn1cComUnitName"); //销售单位名称   21
        //    Session["ordergrid"] = dt;
        //}
        ////重新绑定grid
        //OrderGrid.DataBind();
        ////刷新物料树
        ////Response.Write("<script type='text/javascript'>window.parent.location.href='login.aspx';</script>");
        ////Response.Write("<script type='text/javascript'>window.parent.frames['OrderLeft'].location.reload();</script>");
    }

    protected void BtnTxtRelateU8NOOk_Click(object sender, EventArgs e) //选择样品资料关联U8订单号
    {
        //string strBillNo = TxtRelateU8NOGridView.GetRowValues(TxtRelateU8NOGridView.FocusedRowIndex, "strBillNo").ToString();
        //TxtRelateU8NO.Text = strBillNo;
        //Session["TxtRelateU8NO"] = strBillNo;
        ////读取关联的U8订单信息
        //DataTable AuthOrderInfo = new DataTable();
        //AuthOrderInfo = new OrderManager().DL_OrderBillYPZLBySel(strBillNo);
        ////文本框赋值
        //TxtCustomer.Text = AuthOrderInfo.Rows[0]["ccusname"].ToString();  //开票单位
        //Session["ccusname"] = TxtCustomer.Text;
        //TxtSalesman.Text = AuthOrderInfo.Rows[0]["cpersoncode"].ToString();    //业务员
        //Session["cpersoncode"] = TxtSalesman.Text;
        //TxtOrderShippingMethod.Text = AuthOrderInfo.Rows[0]["cdefine11"].ToString();  //送货方式
        //Session["TxtOrderShippingMethod"] = TxtOrderShippingMethod.Text;
        //TxtcSCCode.Text = AuthOrderInfo.Rows[0]["TxtcSCCode"].ToString();   //发运方式
        //Session["TxtcSCCode"] = TxtcSCCode.Text;
        //TxtOrderMark.Text = AuthOrderInfo.Rows[0]["strRemarks"].ToString();    //备注
        //HttpCookie CookieTxtOrderMark = new HttpCookie("TxtOrderMark");
        //CookieTxtOrderMark.Value = TxtOrderMark.Text;
        //System.Web.HttpContext.Current.Response.Cookies.Add(CookieTxtOrderMark);

        //Txtcdefine3.Text = AuthOrderInfo.Rows[0]["cdefine3"].ToString();   //车型
        //Session["cdefine3"] = Txtcdefine3.Text;
        //TxtLoadingWays.Text = AuthOrderInfo.Rows[0]["strLoadingWays"].ToString();   //装车方式

        ////HttpCookie cookie = new HttpCookie("TxtLoadingWays");
        ////cookie.Values.Set("", TxtLoadingWays.Text);
        ////Response.SetCookie(cookie);
        //HttpCookie CookieTxtLoadingWays = new HttpCookie("TxtLoadingWays");
        //CookieTxtLoadingWays.Value = TxtLoadingWays.Text;
        //System.Web.HttpContext.Current.Response.Cookies.Add(CookieTxtLoadingWays);

        //DeliveryDate.Value = AuthOrderInfo.Rows[0]["datDeliveryDate"].ToString();   //提货时间
        //DeliveryDate.Text = AuthOrderInfo.Rows[0]["datDeliveryDateText"].ToString();   //提货时间
        //HttpCookie CookieDeliveryDate = new HttpCookie("DeliveryDate");
        //CookieDeliveryDate.Value = AuthOrderInfo.Rows[0]["datDeliveryDateText"].ToString();
        //System.Web.HttpContext.Current.Response.Cookies.Add(CookieDeliveryDate);

    }

    protected void OrderGrid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e) //Grid行准备,库存颜色标识
    {
        //if (e.RowType != DevExpress.Web.ASPxGridView.) return;
        if (e.GetValue("Stock") != null)
        {
            string Stock = e.GetValue("Stock").ToString();
            float StockInt = float.Parse(Stock);
            if (StockInt == 0)
            {
                //e.Row.Cells[11].Style.Add("color", "Red");
                e.Row.Style.Add("color", "Red");
            }
            if (e.GetValue("cInvDefineQTY") != null)
            {
                string lngcInvDefineQTY = e.GetValue("cInvDefineQTY").ToString();
                float lngcInvDefineQTYInt = float.Parse(lngcInvDefineQTY);
                if (StockInt < lngcInvDefineQTYInt)
                {
                    //e.Row.Cells[11].Style.Add("color", "Red");
                    e.Row.Style.Add("color", "DodgerBlue");
                }
            }
            //string iLowSumRel = e.GetValue("iLowSumRel").ToString();          //20170316注释
            //if (iLowSumRel == "1")
            //{
            //    e.Row.Style.Add("color", "Pink");
            //}
        }
    }

    protected void BtnInvOK_Click(object sender, EventArgs e)   //选择商品后,将商品传递到grid中,并自动保存
    {
        DataTable dtst = (DataTable)Session["gridselect"];  //获取选中行的值,保存
        //string Strpreorderleft = "行：";
        if (TreeDetail.Selection.Count > 0)
        {
            for (int i = 0; i < TreeDetail.Selection.Count; i++)
            {
                dtst.Rows.Add(new object[] { TreeDetail.GetSelectedFieldValues("cInvCode")[i].ToString() });
            }
            Session["gridselect"] = dtst;
        }

        DataTable YOrderGrid = (DataTable)Session["ordergrid"];

        //1.排除重复的物料
        for (int i = 0; i < OrderGrid.VisibleRowCount; i++)
        {
            for (int j = 0; j < dtst.Rows.Count; j++)
            {
                if (OrderGrid.GetRowValues(i, "cInvCode").ToString() == dtst.Rows[j]["cInvCode"].ToString())
                {
                    dtst.Rows[j].Delete();
                    dtst.AcceptChanges();
                    //break;
                    j = 99999;
                }
            }
        }

        int va = 0;
        //2.将选择的新物料查询出对应的基础数据资料,并且传入YOrder中
        for (int i = 0; i < dtst.Rows.Count; i++)
        {
            va = 0;
            //20160722,提交订单是会提示   已添加项。字典中的关键字：01030100101   所添加的关键字：01030100101，删除session中的重复项
            //判断如果有重复的，即跳过添加
            for (int j = 0; j < YOrderGrid.Rows.Count; j++)
            {
                if (dtst.Rows[i]["cInvCode"].ToString() == YOrderGrid.Rows[j]["cInvCode"].ToString())
                {
                    va = 1;
                }
            }
            if (0 == va)
            {
                //检测未参照的酬宾订单数据，20170104取消
                //DataTable preorderleft = new OrderManager().DLproc_PerOrderCinvcodeLeftBySel(Session["lngopUserExId"].ToString(), Session["lngopUserId"].ToString(),Session["KPDWcCusCode"].ToString(), dtst.Rows[i][0].ToString());
                //Int32 dblpreorderleft=Convert.ToInt32(preorderleft.Rows[0][0].ToString());
                DataTable iddt = new OrderManager().DLproc_QuasiOrderDetailBySel(dtst.Rows[i][0].ToString(), Session["KPDWcCusCode"].ToString());
                #region 将传递过来的数据放入datatable中,并且绑定gridview
                //griddt.Rows.Add(new object[] { array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7], array[8], "0", "0", "0", "0", "88", "9" });
                string[] array = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24" };
                array[0] = iddt.Rows[0]["cInvCode"].ToString();//存货编码
                array[1] = iddt.Rows[0]["cInvName"].ToString();//存货名称
                array[2] = iddt.Rows[0]["cInvStd"].ToString();//存货规格
                array[3] = iddt.Rows[0]["cComUnitName"].ToString();//基本单位
                array[17] = iddt.Rows[0]["fAvailQtty"].ToString(); //库存可用量
                array[18] = iddt.Rows[0]["Rate"].ToString(); //扣率
                array[19] = iddt.Rows[0]["cComUnitCode"].ToString(); //基本单位编码
                array[20] = iddt.Rows[0]["iTaxRate"].ToString(); //销项税率
                array[21] = iddt.Rows[0]["cn1cComUnitName"].ToString(); //销售单位名称
                int dtc = YOrderGrid.Rows.Count + 1;    //dtc  行数 
                array[22] = dtc.ToString();//行号
                array[23] = iddt.Rows[0]["iLowSumRel"].ToString(); //最低库存，23
                array[24] = iddt.Rows[0]["cDefine24"].ToString(); //活动类型，24
                //20170104取消，检查是否有酬宾订单数据
                //if ( dblpreorderleft  > 0) 
                //{
                //    Strpreorderleft = Strpreorderleft + dtc.ToString() + "," + iddt.Rows[0]["cInvName"].ToString() + "数量：" + preorderleft.Rows[0][0].ToString() + ";";
                //}
                //10.25从传递过来的参数分解数组,改为传递物料的编码进行查询,在循环添加
                if (iddt.Rows[0]["cInvCode"].ToString().Substring(0, 2) == "32")
                {
                    array[4] = iddt.Rows[0]["cComUnitName"].ToString();
                    array[5] = iddt.Rows[0]["cComUnitName"].ToString();
                    array[6] = iddt.Rows[0]["iChangRate"].ToString();
                    array[7] = iddt.Rows[0]["iChangRate"].ToString();
                    array[8] = Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[0][4].ToString()), 2)) + iddt.Rows[0][2].ToString() + "=" + Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[0][4].ToString()), 2)) + iddt.Rows[0][2].ToString() + "=" + Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[0][4].ToString()), 2)) + iddt.Rows[0][2].ToString();
                    array[9] = "0";
                    array[10] = "0";
                    array[11] = "0";
                    array[12] = "0";
                    array[13] = iddt.Rows[0]["Quote"].ToString();
                    array[14] = "0";
                    array[15] = "0";
                    array[16] = iddt.Rows[0]["ExercisePrice"].ToString();
                }
                if (iddt.Rows.Count == 1 && iddt.Rows[0]["cInvCode"].ToString().Substring(0, 2) != "32")
                {
                    array[4] = iddt.Rows[0]["cComUnitName"].ToString();
                    array[5] = iddt.Rows[0]["cComUnitName"].ToString();
                    array[6] = iddt.Rows[0]["iChangRate"].ToString();
                    array[7] = iddt.Rows[0]["iChangRate"].ToString();
                    array[8] = Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[0][4].ToString()), 2)) + iddt.Rows[0][2].ToString() + "=" + Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[0][4].ToString()), 2)) + iddt.Rows[0][2].ToString() + "=" + Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[0][4].ToString()), 2)) + iddt.Rows[0][2].ToString();
                    array[9] = "0";
                    array[10] = "0";
                    array[11] = "0";
                    array[12] = "0";
                    array[13] = iddt.Rows[0]["Quote"].ToString();
                    array[14] = "0";
                    array[15] = "0";
                    array[16] = iddt.Rows[0]["ExercisePrice"].ToString();
                }
                if (iddt.Rows.Count == 2 && iddt.Rows[0]["cInvCode"].ToString().Substring(0, 2) != "32")
                {
                    array[4] = iddt.Rows[1]["cComUnitName"].ToString();
                    array[5] = iddt.Rows[1]["cComUnitName"].ToString();
                    array[6] = iddt.Rows[1]["iChangRate"].ToString();
                    array[7] = iddt.Rows[1]["iChangRate"].ToString();
                    array[8] = Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[1][4].ToString()), 2)) + iddt.Rows[0][2].ToString() + "=" + Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[1][4].ToString()) / Convert.ToDouble(iddt.Rows[1][4].ToString()), 2)) + iddt.Rows[1][2].ToString() + "=" + Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[1][4].ToString()) / Convert.ToDouble(iddt.Rows[1][4].ToString()), 2)) + iddt.Rows[1][2].ToString();
                    array[9] = "0";
                    array[10] = "0";
                    array[11] = "0";
                    array[12] = "0";
                    array[13] = iddt.Rows[0]["Quote"].ToString();
                    array[14] = "0";
                    array[15] = "0";
                    array[16] = iddt.Rows[0]["ExercisePrice"].ToString();
                }
                if (iddt.Rows.Count >= 3 && iddt.Rows[0]["cInvCode"].ToString().Substring(0, 2) != "32")
                {
                    array[4] = iddt.Rows[2]["cComUnitName"].ToString();
                    array[5] = iddt.Rows[1]["cComUnitName"].ToString();
                    array[6] = iddt.Rows[2]["iChangRate"].ToString();
                    array[7] = iddt.Rows[1]["iChangRate"].ToString();
                    array[8] = Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[2][4].ToString()), 2)) + iddt.Rows[0][2].ToString() + "=" + Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[2][4].ToString()) / Convert.ToDouble(iddt.Rows[1][4].ToString()), 2)) + iddt.Rows[1][2].ToString() + "=" + Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[0][4].ToString()), 2)) + iddt.Rows[2][2].ToString();
                    array[9] = "0";
                    array[10] = "0";
                    array[11] = "0";
                    array[12] = "0";
                    array[13] = iddt.Rows[0]["Quote"].ToString();
                    array[14] = "0";
                    array[15] = "0";
                    array[16] = iddt.Rows[0]["ExercisePrice"].ToString();
                }
                #endregion

                YOrderGrid.Rows.Add(new object[] { array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7], array[8], array[9], array[10], array[11], array[12], array[13], array[14], array[15], array[16], array[17], array[18], array[19], array[20], array[21], array[22], array[23], array[24] });
            }
        }

        //DataView dv = YOrderGrid.DefaultView;
        //DataTable distable = dv.ToTable(true, new string[] { "cInvCode" });
        ////通过distable表，删除YOrderGrid中重复的数据
        //for (int i = 0; i < length; i++)
        //{

        //}
        //更新顺序
        for (int i = 0; i < YOrderGrid.Rows.Count; i++)
        {
            YOrderGrid.Rows[i]["irowno"] = i + 1;
        }
        Session["ordergrid"] = YOrderGrid;
        OrderGrid.DataSource = YOrderGrid;
        OrderGrid.DataBind();

        //3.清除选择
        TreeDetail.Selection.UnselectAll(); //清除所有选择项
        //清除session数据
        Session.Remove("gridselect");

        //2016-03-28添加,自动保存功能,选择后,插入自动保存数据
        #region 保存临时订单
        //获取表头信息
        string lngopUserId = Session["lngopUserId"].ToString(); //用户id
        int bytStatus = 12;  //单据状态(临时订单),11,手工保存,12自动保存
        string ccuscode = Session["KPDWcCusCode"].ToString();   //开票单位编码
        string cdefine9 = "";
        if (Session["cdefine9"] != null)
        {
            cdefine9 = Session["cdefine9"].ToString();   //自定义项1,收货人姓名,cDefine9
        }
        string cdefine12 = "";
        if (Session["cdefine12"] != null)
        {
            cdefine12 = Session["cdefine12"].ToString();   //自定义项2,收货人电话,cDefine12
        }
        string cdefine11 = TxtOrderShippingMethod.Text.ToString();   //自定义项11,收货人地址,cDefine11
        string cdefine1 = "";
        if (Session["cdefine1"] != null)
        {
            cdefine1 = Session["cdefine1"].ToString();   //自定义项8,司机姓名,cDefine1
        }
        string cdefine13 = "";
        if (Session["cdefine13"] != null)
        {
            cdefine13 = Session["cdefine13"].ToString();   //自定义项9,司机电话,cDefine13
        }
        string cdefine2 = "";
        if (Session["cdefine2"] != null)
        {
            cdefine2 = Session["cdefine2"].ToString();   //自定义项10,司机身份证,cDefine2
        }
        string cdefine3 = "";
        if (Session["cdefine3"] != null)
        {
            cdefine3 = Session["cdefine3"].ToString();   //自定义项3,汽车类型,cdefine3
        }
        string cdefine10 = "";
        if (Session["cdefine10"] != null)
        {
            cdefine10 = Session["cdefine10"].ToString();   //自定义项12,车牌号,cDefine10
        }
        string ccusname = TxtCustomer.Text; //客户名称             
        string strRemarks = TxtOrderMark.Text.ToString().Replace("\r", ",").Replace("\n", ",").Replace("\t", ",").Replace(" ", ",").Replace(",,,,", ",").Replace(",,,", ",").Replace(",,", ",");   //备注 
        string cpersoncode = TxtSalesman.Text.ToString();   //业务员编码
        string cSCCode = "";    //发运方式编码
        if (Session["TxtcSCCode"] != null)
        {
            if (Session["TxtcSCCode"].ToString() == "配送" || Session["TxtcSCCode"].ToString() == "01")     //发运方式编码,00:自提,01:厂车配送
            {
                Session["TxtcSCCode"] = "01";
                cSCCode = "01";
            }
            else
            {
                Session["TxtcSCCode"] = "00";
                cSCCode = "00";
            }
        }
        //11-18新增字段赋值
        string strLoadingWays = TxtLoadingWays.Text.ToString();     //装车方式
        string cSTCode = "00";     //销售类型编码  
        string lngopUseraddressId = "";
        if (Session["lngopUseraddressId"] != null)
        {
            lngopUseraddressId = Session["lngopUseraddressId"].ToString();
        }
        int lngBillType = 0;
        string RelateU8NO = "";
        string strBillName = "系统自动保存";
        //if (TxtSaveBackOrderName != null)
        //{
        //    strBillName = TxtSaveBackOrderName.Text.Trim().ToString();
        //}

        //插入表头信息
        DataTable DTlngopOrderBackId = new OrderManager().DLproc_AddOrderBackByIns(lngopUserId, strBillName, bytStatus, strRemarks, ccuscode, cdefine1, cdefine2, cdefine3, cdefine9, cdefine10, cdefine11, cdefine12, cdefine13, ccusname, cpersoncode, cSCCode, strLoadingWays, cSTCode, lngopUseraddressId, RelateU8NO, lngBillType);
        if (DTlngopOrderBackId.Rows.Count < 1)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('自动保存临时订单失败,请联系系统管理员！');</script>");
            return;
        }
        //获取表头返回的表id
        Int32 lngopOrderBackId = Convert.ToInt32(DTlngopOrderBackId.Rows[0]["lngopOrderBackId"].ToString());
        //获取表体信息,并插入
        if (OrderGrid.VisibleRowCount <= 0)
        {
            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('保存成功！');</script>");
            //return;
        }
        else
        {
            //DataTable DTordergird = (DataTable)Session["ordergrid"];
            string cinvcode = "";
            string cinvname = "";
            double cComUnitQTY = 0;
            double cInvDefine1QTY = 0;
            double cInvDefine2QTY = 0;
            double cInvDefineQTY = 0;
            double cComUnitAmount = 0;
            string pack = "";
            string irowno = "";
            for (int i = 0; i < OrderGrid.VisibleRowCount; i++)
            {
                cinvcode = OrderGrid.GetRowValues(i, "cInvCode").ToString();
                cinvname = OrderGrid.GetRowValues(i, "cInvName").ToString();
                cComUnitQTY = Convert.ToDouble(OrderGrid.GetRowValues(i, "cComUnitQTY").ToString());
                cInvDefine1QTY = Convert.ToDouble(OrderGrid.GetRowValues(i, "cInvDefine1QTY").ToString());
                cInvDefine2QTY = Convert.ToDouble(OrderGrid.GetRowValues(i, "cInvDefine2QTY").ToString());
                cInvDefineQTY = Convert.ToDouble(OrderGrid.GetRowValues(i, "cInvDefineQTY").ToString());
                cComUnitAmount = Convert.ToDouble(OrderGrid.GetRowValues(i, "cComUnitAmount").ToString());
                pack = OrderGrid.GetRowValues(i, "pack").ToString();
                irowno = OrderGrid.GetRowValues(i, "irowno").ToString();
                bool c = new OrderManager().DLproc_AddOrderBackDetailByIns(lngopOrderBackId, cinvcode, cinvname, cComUnitQTY, cInvDefine1QTY, cInvDefine2QTY, cInvDefineQTY, cComUnitAmount, pack, irowno);
            }
            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('保存成功！');</script>");
            //return;
        }
        //删除其他自动保存数据
        bool cDel = new OrderManager().DLproc_DelAutoSaveOrderBackByDel(Convert.ToInt32(Session["lngopUserId"].ToString()));
        //刷新临时表数据
        DataTable dtBackOrderGridView = new OrderManager().DL_GetOrderBackBySel(Convert.ToInt32(Session["lngopUserId"].ToString()));
        BackOrderGridView.DataSource = dtBackOrderGridView;
        BackOrderGridView.DataBind();
        #endregion
        Session["AutoSaveInfo"] = "(于" + DateTime.Now.ToLocalTime().ToString() + "自动保存)";
        if (Session["AutoSaveInfo"] != null)
        {
            //OrderGrid.SettingsCommandButton.UpdateButton.Text = "保存购物信息           " + Session["AutoSaveInfo"].ToString();
            OrderGrid.SettingsText.Title = "订单明细表" + Session["AutoSaveInfo"].ToString();
        }
        //提示有未完成的酬宾订单，20170104取消
        //if (Strpreorderleft.ToString() != "行：")
        //{
        //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('" + Strpreorderleft.ToString() + "存在未参照完的酬宾订单，请先将酬宾订单的商品参照完毕后在普通订单中购买该商品！" + "');</script>");
        //}
    }

    /*ASPxTreeList的FocuseNodeChnaged事件来处理选择Node时的逻辑,需要引用using DevExpress.Web.ASPxTreeList;*/
    protected void treeList_CustomDataCallback(object sender, TreeListCustomDataCallbackEventArgs e)  //获取选中行的值,保存
    {
        DataTable dtst = (DataTable)Session["gridselect"];  //获取选中行的值,保存
        if (TreeDetail.Selection.Count > 0)
        {
            for (int i = 0; i < TreeDetail.Selection.Count; i++) //获取选中的数据
            {
                dtst.Rows.Add(new object[] { TreeDetail.GetSelectedFieldValues("cInvCode")[i].ToString() });
            }
            Session["gridselect"] = dtst;
            //ASPxGridView1.DataSource = dtst;//测试用
            //ASPxGridView1.DataBind();
        }

        /// 设置选中时节点的背景色


        string key = e.Argument.ToString();
        TreeListNode node = treeList.FindNodeByKeyValue(key);
        //if (node.ChildNodes.Count == 0)       //末级节点
        //{
        //    e.Result = GetEntryText(node);//获取node的code值    
        //}
        if (node.Level >= 2)
        {
            e.Result = GetEntryText(node);//获取node的code值          
        }

    }

    protected string GetEntryText(TreeListNode node)    //树节点调用
    {
        if (node != null)
        {
            string KeyFieldName = node["KeyFieldName"].ToString();
            Session["ordertreelistgrid"] = KeyFieldName.ToString();//赋值给grid查询
            return KeyFieldName.Trim();
            //查询并绑定gridview

        }
        return string.Empty;
    }

    protected void btn_Click(object sender, EventArgs e)    //前台按钮调用
    {
        ////DataTable dtst = (DataTable)Session["gridselect"];
        ////for (int i = 0; i < TreeDetail.Selection.Count; i++)
        ////{
        ////    dtst.Rows.Add(new object[] { TreeDetail.GetSelectedFieldValues("cInvCode")[i].ToString() });
        ////}
        ////Session["gridselect"] = dtst;
        ////绑定gridview数据源
        //if (Session["ordertreelistgrid"] != null)
        //{
        //    DataTable dt1 = new SearchManager().DLproc_TreeListDetailsAllBySel(Session["ordertreelistgrid"].ToString(), Session["KPDWcCusCode"].ToString());
        //    TreeDetail.DataSource = dt1;
        //    TreeDetail.DataBind();
        //    //删除当前绑定的数据,因为切换分类,所以需要删除
        //    DataTable dtst = (DataTable)Session["gridselect"];
        //    for (int i = 0; i < dt1.Rows.Count; i++)
        //    {
        //        for (int j = 0; j < dtst.Rows.Count; j++)
        //        {
        //            if (TreeDetail.GetRowValues(i, "cInvCode") != null && TreeDetail.GetRowValues(i, "cInvCode").ToString() == dtst.Rows[j][0].ToString())
        //            {
        //                TreeDetail.Selection.SelectRow(i);
        //                dtst.Rows[j].Delete();
        //                dtst.AcceptChanges();
        //            }
        //        }
        //    }
        //}

    }

    protected void BtnInv_Reset_Click(object sender, EventArgs e) //重置,清除选择项
    {
        TreeDetail.Selection.UnselectAll(); //清除所有选择项
        //清除session数据
        Session.Remove("gridselect");
    }

    protected void OrderGrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)    //删除表体数据
    {
        string cInvCode = e.Values["cInvCode"].ToString();
        DataTable dtt = (DataTable)Session["ordergrid"];
        for (int i = 0; i < dtt.Rows.Count; i++)
        {
            if (cInvCode == dtt.Rows[i][0].ToString())
            {
                dtt.Rows[i].Delete();
                dtt.AcceptChanges();
                break;
            }
        }
        for (int j = 0; j < dtt.Rows.Count; j++)
        {
            dtt.Rows[j]["irowno"] = j + 1;
        }
        Session["ordergrid"] = dtt;
        OrderGrid.DataSource = dtt;
        OrderGrid.DataBind();
        e.Cancel = true;




        //Session["AutoSaveInfo"] = "(于" + DateTime.Now.ToLocalTime().ToString() + "自动保存)";
        //if (Session["AutoSaveInfo"] != null)
        //{
        //    //OrderGrid.SettingsCommandButton.UpdateButton.Text = "保存购物信息           " + Session["AutoSaveInfo"].ToString();
        //    OrderGrid.SettingsText.Title = "订单明细表" + Session["AutoSaveInfo"].ToString();
        //}    
    }

    //protected void treeList_Load(object sender, EventArgs e)
    //{
    //    //绑定treelist数据源
    //    DataTable dttree = new SearchManager().DLproc_InventoryBySel(Session["cSTCode"].ToString());
    //    treeList.KeyFieldName = "KeyFieldName";
    //    treeList.ParentFieldName = "ParentFieldName";
    //    treeList.DataSource = dttree;
    //    /*展开第一级node*/
    //    treeList.DataBind();
    //    treeList.ExpandToLevel(1);
    //    ////绑定gridview数据源
    //    //DataTable dt1 = new SearchManager().DLproc_TreeListDetailsAllBySel(Session["ordertreelistgrid"].ToString(), Session["KPDWcCusCode"].ToString());
    //    //TreeDetail.DataSource = dt1;
    //    //TreeDetail.DataBind();
    //}

    protected void OrderGrid_Init(object sender, EventArgs e)   //设置订单明细表初始化,显示报价金额.执行金额
    {
        //初始化,设置金额合计启用报价合计还是执行价合计?
        Hashtable ht = (Hashtable)Session["SysSetting"];
        //    if (ht.Contains("IsExercisePrice"))
        //{

        //}
        if (ht["IsExercisePrice"].ToString() == "0")   //报价金额
        {
            OrderGrid.TotalSummary[2].FieldName = "cComUnitAmount";
        }
        else//执行价金额
        {
            OrderGrid.TotalSummary[2].FieldName = "xx";
        }
        //OrderGrid.SettingsText.Title = "订单明细表";
        if (Session["AutoSaveInfo"] != null)
        {
            //OrderGrid.SettingsCommandButton.UpdateButton.Text = "保存购物信息           " + Session["AutoSaveInfo"].ToString();
            OrderGrid.SettingsText.Title = "订单明细表" + Session["AutoSaveInfo"].ToString();
        }
    }

    protected void BtnMain_Click(object sender, EventArgs e) //表头展开,收起
    {
        int aa = System.Math.Abs(Convert.ToInt32(HFMain.Value.ToString()) - 1);
        HFMain.Value = aa.ToString();
        if (aa == 0)
        {
            BtnMain.Text = "收起↑";
            this.main.Style.Add("display", "block");
        }
        else
        {
            BtnMain.Text = "展开↓";
            this.main.Style.Add("display", "none");   //隐藏Div,显示this.DivSOA.Style.Add("display", "block");            
        }
    }

    protected void BtnOrderGridHeigh_Click(object sender, EventArgs e) //设置订单明细表高度
    {
        Session["GridViewHeigh"] = GridViewHeigh.Value;
        Session["GridViewFontSize"] = GridViewFontSize.Value;
        if (Session["GridViewHeigh"] != null)
        {
            OrderGrid.Settings.VerticalScrollableHeight = Convert.ToInt32(Session["GridViewHeigh"].ToString());
        }
        else
        {
            OrderGrid.Settings.VerticalScrollableHeight = 200;
        }
        if (Session["GridViewFontSize"] != null)
        {
            OrderGrid.Font.Size = Convert.ToInt32(Session["GridViewFontSize"].ToString());
        }
        else
        {
            OrderGrid.Font.Size = 9;
        }
    }

    protected void BtnPreviousOrderAgainOk_Click(object sender, EventArgs e) //提取历史订单信息,赋值
    {
        //选择历史订单
        DataTable gridselectdt = (DataTable)Session["gridselect"];
        gridselectdt.Rows.Clear();
        Session["gridselect"] = gridselectdt;
        //获取数据,并且覆盖数据
        TxtOrderMark.Text = "";
        TxtLoadingWays.Text = "";
        DataTable YOrderGrid = (DataTable)Session["ordergrid"];
        YOrderGrid.Rows.Clear();
        //1.表头数据
        //DataTable PreviousOrderAgainDt = new OrderManager().DL_BuyAgainBySel(PreviousOrderAgainGridView.GetRowValues(PreviousOrderAgainGridView.FocusedRowIndex, "strBillNo").ToString());
        //DataTable PreviousOrderAgainDt = new OrderManager().DLproc_ReferencePreviousOrderWithCusInvLimitedBySel(PreviousOrderAgainGridView.GetRowValues(PreviousOrderAgainGridView.FocusedRowIndex, "strBillNo").ToString());
        DataTable PreviousOrderAgainDt = new OrderManager().DLproc_BackOrderandPrvOrdercInvCodeIsBeLimitedBySel(PreviousOrderAgainGridView.GetRowValues(PreviousOrderAgainGridView.FocusedRowIndex, "lngopOrderId").ToString(), 1, 2);
        //表头赋值
        Session["TxtCustomer"] = PreviousOrderAgainDt.Rows[0]["ccusname"].ToString();
        TxtCustomer.Text = PreviousOrderAgainDt.Rows[0]["ccusname"].ToString();
        TxtOrderMark.Text = PreviousOrderAgainDt.Rows[0]["strRemarks"].ToString();
        Session["TxtcSCCode"] = PreviousOrderAgainDt.Rows[0]["cSCCode"].ToString();
        TxtcSCCode.Text = PreviousOrderAgainDt.Rows[0]["cSCCode"].ToString();
        Session["TxtOrderShippingMethod"] = PreviousOrderAgainDt.Rows[0]["cdefine11"].ToString();
        TxtOrderShippingMethod.Text = PreviousOrderAgainDt.Rows[0]["cdefine11"].ToString();
        Session["cCusPPerson"] = PreviousOrderAgainDt.Rows[0]["cpersoncode"].ToString();
        TxtSalesman.Text = PreviousOrderAgainDt.Rows[0]["cpersoncode"].ToString();
        TxtLoadingWays.Text = PreviousOrderAgainDt.Rows[0]["strLoadingWays"].ToString();
        Session["cdefine3"] = PreviousOrderAgainDt.Rows[0]["cdefine3"].ToString();
        Txtcdefine3.Text = PreviousOrderAgainDt.Rows[0]["cdefine3"].ToString();
        //Session["lngopUserId
        Session["KPDWcCusCode"] = PreviousOrderAgainDt.Rows[0]["ccuscode"].ToString();
        Session["lngopUseraddressId"] = PreviousOrderAgainDt.Rows[0]["lngopUseraddressId"].ToString();
        Session["cdefine9"] = PreviousOrderAgainDt.Rows[0]["cdefine9"].ToString();
        Session["cdefine12"] = PreviousOrderAgainDt.Rows[0]["cdefine12"].ToString();
        Session["cdefine1"] = PreviousOrderAgainDt.Rows[0]["cdefine1"].ToString();
        Session["cdefine13"] = PreviousOrderAgainDt.Rows[0]["cdefine13"].ToString();
        Session["cdefine2"] = PreviousOrderAgainDt.Rows[0]["cdefine2"].ToString();
        Session["cdefine10"] = PreviousOrderAgainDt.Rows[0]["cdefine10"].ToString();
        //重新获取信用额度
        DataTable CusCreditDt = new DataTable();
        CusCreditDt = new OrderManager().DLproc_getCusCreditInfo(Session["KPDWcCusCode"].ToString());//默认登录客户编码
        TxtCusCredit.Text = CusCreditDt.Rows[0]["iCusCreLine"].ToString();
        //2.表体数据
        //Session["ordergrid"] 
        //2.将选择的新物料查询出对应的基础数据资料,并且传入YOrder中
        string bLimitedcInvName = "";
        for (int i = 0; i < PreviousOrderAgainDt.Rows.Count; i++)
        {
            //bLimit(0,限销,1,允销)原方法
            if (PreviousOrderAgainDt.Rows[i]["bLimited"].ToString() == "False" || PreviousOrderAgainDt.Rows[i]["bLimited"].ToString() == "0")
            {
                bLimitedcInvName = bLimitedcInvName + "/" + PreviousOrderAgainDt.Rows[i]["cinvname"].ToString();
            }
            else
            {
                DataTable iddt = new OrderManager().DLproc_QuasiOrderDetailBySel(PreviousOrderAgainDt.Rows[i]["cinvcode"].ToString(), Session["KPDWcCusCode"].ToString());
                #region 将传递过来的数据放入datatable中,并且绑定gridview
                //griddt.Rows.Add(new object[] { array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7], array[8], "0", "0", "0", "0", "88", "9" });
                string[] array = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "21", "22", "23", "24", "25" };
                array[0] = iddt.Rows[0]["cInvCode"].ToString();//存货编码
                array[1] = iddt.Rows[0]["cInvName"].ToString();//存货名称
                array[2] = iddt.Rows[0]["cInvStd"].ToString();//存货规格
                array[3] = iddt.Rows[0]["cComUnitName"].ToString();//基本单位
                array[17] = iddt.Rows[0]["fAvailQtty"].ToString(); //库存可用量
                array[18] = iddt.Rows[0]["Rate"].ToString(); //扣率
                array[19] = iddt.Rows[0]["cComUnitCode"].ToString(); //基本单位编码
                array[20] = iddt.Rows[0]["iTaxRate"].ToString(); //销项税率
                array[21] = iddt.Rows[0]["cn1cComUnitName"].ToString(); //销售单位名称
                int dtc = YOrderGrid.Rows.Count + 1;    //dtc  行数 
                array[22] = dtc.ToString();//行号
                array[23] = "1";//是否超出
                array[24] = iddt.Rows[0]["cDefine24"].ToString(); //活动类型
                array[25] = iddt.Rows[0]["cDefine24"].ToString(); //活动类型
                //10.25从传递过来的参数分解数组,改为传递物料的编码进行查询,在循环添加
                if (iddt.Rows[0]["cInvCode"].ToString().Substring(0, 2) == "32")
                {
                    array[4] = iddt.Rows[0]["cComUnitName"].ToString();
                    array[5] = iddt.Rows[0]["cComUnitName"].ToString();
                    array[6] = iddt.Rows[0]["iChangRate"].ToString();
                    array[7] = iddt.Rows[0]["iChangRate"].ToString();
                    array[8] = Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[0][4].ToString()), 2)) + iddt.Rows[0][2].ToString() + "=" + Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[0][4].ToString()), 2)) + iddt.Rows[0][2].ToString() + "=" + Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[0][4].ToString()), 2)) + iddt.Rows[0][2].ToString();
                    array[9] = "0";
                    array[10] = "0";
                    array[11] = "0";
                    array[12] = "0";
                    array[13] = iddt.Rows[0]["Quote"].ToString();
                    array[14] = "0";
                    array[15] = "0";
                    array[16] = iddt.Rows[0]["ExercisePrice"].ToString();
                }
                if (iddt.Rows.Count == 1 && iddt.Rows[0]["cInvCode"].ToString().Substring(0, 2) != "32")
                {
                    array[4] = iddt.Rows[0]["cComUnitName"].ToString();
                    array[5] = iddt.Rows[0]["cComUnitName"].ToString();
                    array[6] = iddt.Rows[0]["iChangRate"].ToString();
                    array[7] = iddt.Rows[0]["iChangRate"].ToString();
                    array[8] = Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[0][4].ToString()), 2)) + iddt.Rows[0][2].ToString() + "=" + Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[0][4].ToString()), 2)) + iddt.Rows[0][2].ToString() + "=" + Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[0][4].ToString()), 2)) + iddt.Rows[0][2].ToString();
                    array[9] = "0";
                    array[10] = "0";
                    array[11] = "0";
                    array[12] = "0";
                    array[13] = iddt.Rows[0]["Quote"].ToString();
                    array[14] = "0";
                    array[15] = "0";
                    array[16] = iddt.Rows[0]["ExercisePrice"].ToString();
                }
                if (iddt.Rows.Count == 2 && iddt.Rows[0]["cInvCode"].ToString().Substring(0, 2) != "32")
                {
                    array[4] = iddt.Rows[1]["cComUnitName"].ToString();
                    array[5] = iddt.Rows[1]["cComUnitName"].ToString();
                    array[6] = iddt.Rows[1]["iChangRate"].ToString();
                    array[7] = iddt.Rows[1]["iChangRate"].ToString();
                    array[8] = Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[1][4].ToString()), 2)) + iddt.Rows[0][2].ToString() + "=" + Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[1][4].ToString()) / Convert.ToDouble(iddt.Rows[1][4].ToString()), 2)) + iddt.Rows[1][2].ToString() + "=" + Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[1][4].ToString()) / Convert.ToDouble(iddt.Rows[1][4].ToString()), 2)) + iddt.Rows[1][2].ToString();
                    array[9] = "0";
                    array[10] = "0";
                    array[11] = "0";
                    array[12] = "0";
                    array[13] = iddt.Rows[0]["Quote"].ToString();
                    array[14] = "0";
                    array[15] = "0";
                    array[16] = iddt.Rows[0]["ExercisePrice"].ToString();
                }
                if (iddt.Rows.Count >= 3 && iddt.Rows[0]["cInvCode"].ToString().Substring(0, 2) != "32")
                {
                    array[4] = iddt.Rows[2]["cComUnitName"].ToString();
                    array[5] = iddt.Rows[1]["cComUnitName"].ToString();
                    array[6] = iddt.Rows[2]["iChangRate"].ToString();
                    array[7] = iddt.Rows[1]["iChangRate"].ToString();
                    array[8] = Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[2][4].ToString()), 2)) + iddt.Rows[0][2].ToString() + "=" + Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[2][4].ToString()) / Convert.ToDouble(iddt.Rows[1][4].ToString()), 2)) + iddt.Rows[1][2].ToString() + "=" + Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[0][4].ToString()), 2)) + iddt.Rows[2][2].ToString();
                    array[9] = "0";
                    array[10] = "0";
                    array[11] = "0";
                    array[12] = "0";
                    array[13] = iddt.Rows[0]["Quote"].ToString();
                    array[14] = "0";
                    array[15] = "0";
                    array[16] = iddt.Rows[0]["ExercisePrice"].ToString();
                }
                #endregion
                YOrderGrid.Rows.Add(new object[] { array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7], array[8], array[9], array[10], array[11], array[12], array[13], array[14], array[15], array[16], array[17], array[18], array[19], array[20], array[21], array[22], array[23], array[24] });
            }
        }
        Session["ordergrid"] = YOrderGrid;
        OrderGrid.DataSource = YOrderGrid;
        OrderGrid.DataBind();
        if (bLimitedcInvName != "")
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('未找到商品:" + bLimitedcInvName + "的信息！剩余商品信息提取完成！');</script>");
        }
        else
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('信息全部提取完成！');</script>");
        }

    }

    protected void BtnBackOrderOK_Click(object sender, EventArgs e)   //提取临时订单信息
    {
        if (BackOrderGridView.GetRowValues(BackOrderGridView.FocusedRowIndex, "lngopOrderBackId") == null)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('请选择一个临时订单！');</script>");
            return;
        }
        //提取临时订单信息
        //BackOrderGridView.GetRowValues(BackOrderGridView.FocusedRowIndex, "lngopOrderBackId").ToString();
        //DataTable DTBackOrderGridView = new OrderManager().DLproc_ReferenceOrderBackWithCusInvLimitedBySel(Convert.ToInt32(BackOrderGridView.GetRowValues(BackOrderGridView.FocusedRowIndex, "lngopOrderBackId").ToString()));
        DataTable DTBackOrderGridView = new OrderManager().DLproc_BackOrderandPrvOrdercInvCodeIsBeLimitedBySel(BackOrderGridView.GetRowValues(BackOrderGridView.FocusedRowIndex, "lngopOrderBackId").ToString(), 1, 1);
        //表头赋值
        Session["TxtCustomer"] = DTBackOrderGridView.Rows[0]["ccusname"].ToString();
        TxtCustomer.Text = DTBackOrderGridView.Rows[0]["ccusname"].ToString();
        TxtOrderMark.Text = DTBackOrderGridView.Rows[0]["strRemarks"].ToString();
        Session["TxtcSCCode"] = DTBackOrderGridView.Rows[0]["cSCCode"].ToString();
        TxtcSCCode.Text = DTBackOrderGridView.Rows[0]["cSCCode"].ToString();
        Session["TxtOrderShippingMethod"] = DTBackOrderGridView.Rows[0]["cdefine11"].ToString();
        TxtOrderShippingMethod.Text = DTBackOrderGridView.Rows[0]["cdefine11"].ToString();
        Session["cCusPPerson"] = DTBackOrderGridView.Rows[0]["cpersoncode"].ToString();
        TxtSalesman.Text = DTBackOrderGridView.Rows[0]["cpersoncode"].ToString();
        TxtLoadingWays.Text = DTBackOrderGridView.Rows[0]["strLoadingWays"].ToString();
        Session["cdefine3"] = DTBackOrderGridView.Rows[0]["cdefine3"].ToString();
        Txtcdefine3.Text = DTBackOrderGridView.Rows[0]["cdefine3"].ToString();
        //Session["lngopUserId
        Session["KPDWcCusCode"] = DTBackOrderGridView.Rows[0]["ccuscode"].ToString();
        Session["lngopUseraddressId"] = DTBackOrderGridView.Rows[0]["lngopUseraddressId"].ToString();
        Session["cdefine9"] = DTBackOrderGridView.Rows[0]["cdefine9"].ToString();
        Session["cdefine12"] = DTBackOrderGridView.Rows[0]["cdefine12"].ToString();
        Session["cdefine1"] = DTBackOrderGridView.Rows[0]["cdefine1"].ToString();
        Session["cdefine13"] = DTBackOrderGridView.Rows[0]["cdefine13"].ToString();
        Session["cdefine2"] = DTBackOrderGridView.Rows[0]["cdefine2"].ToString();
        Session["cdefine10"] = DTBackOrderGridView.Rows[0]["cdefine10"].ToString();
        //重新获取信用额度
        DataTable CusCreditDt = new DataTable();
        CusCreditDt = new OrderManager().DLproc_getCusCreditInfo(Session["KPDWcCusCode"].ToString());//默认登录客户编码
        TxtCusCredit.Text = CusCreditDt.Rows[0]["iCusCreLine"].ToString();

        //表体赋值,有null值
        string bLimitedcInvName = "";
        DataTable YOrderGrid = (DataTable)Session["ordergrid"];
        YOrderGrid.Rows.Clear();
        for (int i = 0; i < DTBackOrderGridView.Rows.Count; i++)
        {
            if (DTBackOrderGridView.Rows[i]["cinvcode"] != null)
            {
                //bLimit(0,限销,1,允销)
                if (DTBackOrderGridView.Rows[i]["bLimited"].ToString() == "False" || DTBackOrderGridView.Rows[i]["bLimited"].ToString() == "0")
                {
                    bLimitedcInvName = bLimitedcInvName + "/" + DTBackOrderGridView.Rows[i]["cinvname"].ToString();
                }
                else
                {
                    DataTable iddt = new OrderManager().DLproc_QuasiOrderDetailBySel(DTBackOrderGridView.Rows[i]["cinvcode"].ToString(), Session["KPDWcCusCode"].ToString());
                    #region 将传递过来的数据放入datatable中,并且绑定gridview
                    //griddt.Rows.Add(new object[] { array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7], array[8], "0", "0", "0", "0", "88", "9" });
                    string[] array = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24" };
                    array[0] = iddt.Rows[0]["cInvCode"].ToString();//存货编码
                    array[1] = iddt.Rows[0]["cInvName"].ToString();//存货名称
                    array[2] = iddt.Rows[0]["cInvStd"].ToString();//存货规格
                    array[3] = iddt.Rows[0]["cComUnitName"].ToString();//基本单位
                    array[17] = iddt.Rows[0]["fAvailQtty"].ToString(); //库存可用量
                    array[18] = iddt.Rows[0]["Rate"].ToString(); //扣率
                    array[19] = iddt.Rows[0]["cComUnitCode"].ToString(); //基本单位编码
                    array[20] = iddt.Rows[0]["iTaxRate"].ToString(); //销项税率
                    array[21] = iddt.Rows[0]["cn1cComUnitName"].ToString(); //销售单位名称
                    array[9] = DTBackOrderGridView.Rows[i]["cComUnitQTY"].ToString();//基本单位数量
                    array[10] = DTBackOrderGridView.Rows[i]["cInvDefine1QTY"].ToString();//大包装单位数量
                    array[11] = DTBackOrderGridView.Rows[i]["cInvDefine2QTY"].ToString();//小包装单位数量
                    array[12] = DTBackOrderGridView.Rows[i]["cInvDefineQTY"].ToString();//包装量数量汇总,包装量  12
                    array[13] = iddt.Rows[0]["Quote"].ToString();//基本单位单价(报价)
                    array[14] = DTBackOrderGridView.Rows[i]["cComUnitAmount"].ToString();//基本单位金额(报价)
                    array[15] = DTBackOrderGridView.Rows[i]["pack"].ToString();//包装量换算结果
                    array[16] = iddt.Rows[0]["ExercisePrice"].ToString();//基本单位单价(执行价格)
                    int dtc = YOrderGrid.Rows.Count + 1;    //dtc  行数 
                    array[22] = dtc.ToString();//行号
                    array[23] = "1";//是否超出最低库存
                    array[24] = iddt.Rows[0]["cDefine24"].ToString(); //活动类型
                    //10.25从传递过来的参数分解数组,改为传递物料的编码进行查询,在循环添加
                    if (iddt.Rows[0]["cInvCode"].ToString().Substring(0, 2) == "32")
                    {
                        array[4] = iddt.Rows[0]["cComUnitName"].ToString();
                        array[5] = iddt.Rows[0]["cComUnitName"].ToString();
                        array[6] = iddt.Rows[0]["iChangRate"].ToString();
                        array[7] = iddt.Rows[0]["iChangRate"].ToString();
                        array[8] = Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[0][4].ToString()), 2)) + iddt.Rows[0][2].ToString() + "=" + Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[0][4].ToString()), 2)) + iddt.Rows[0][2].ToString() + "=" + Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[0][4].ToString()), 2)) + iddt.Rows[0][2].ToString();

                    }
                    if (iddt.Rows.Count == 1 && iddt.Rows[0]["cInvCode"].ToString().Substring(0, 2) != "32")
                    {
                        array[4] = iddt.Rows[0]["cComUnitName"].ToString();
                        array[5] = iddt.Rows[0]["cComUnitName"].ToString();
                        array[6] = iddt.Rows[0]["iChangRate"].ToString();
                        array[7] = iddt.Rows[0]["iChangRate"].ToString();
                        array[8] = Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[0][4].ToString()), 2)) + iddt.Rows[0][2].ToString() + "=" + Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[0][4].ToString()), 2)) + iddt.Rows[0][2].ToString() + "=" + Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[0][4].ToString()), 2)) + iddt.Rows[0][2].ToString();
                    }
                    if (iddt.Rows.Count == 2 && iddt.Rows[0]["cInvCode"].ToString().Substring(0, 2) != "32")
                    {
                        array[4] = iddt.Rows[1]["cComUnitName"].ToString();
                        array[5] = iddt.Rows[1]["cComUnitName"].ToString();
                        array[6] = iddt.Rows[1]["iChangRate"].ToString();
                        array[7] = iddt.Rows[1]["iChangRate"].ToString();
                        array[8] = Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[1][4].ToString()), 2)) + iddt.Rows[0][2].ToString() + "=" + Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[1][4].ToString()) / Convert.ToDouble(iddt.Rows[1][4].ToString()), 2)) + iddt.Rows[1][2].ToString() + "=" + Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[1][4].ToString()) / Convert.ToDouble(iddt.Rows[1][4].ToString()), 2)) + iddt.Rows[1][2].ToString();
                    }
                    if (iddt.Rows.Count >= 3 && iddt.Rows[0]["cInvCode"].ToString().Substring(0, 2) != "32")
                    {
                        array[4] = iddt.Rows[2]["cComUnitName"].ToString();
                        array[5] = iddt.Rows[1]["cComUnitName"].ToString();
                        array[6] = iddt.Rows[2]["iChangRate"].ToString();
                        array[7] = iddt.Rows[1]["iChangRate"].ToString();
                        array[8] = Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[2][4].ToString()), 2)) + iddt.Rows[0][2].ToString() + "=" + Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[2][4].ToString()) / Convert.ToDouble(iddt.Rows[1][4].ToString()), 2)) + iddt.Rows[1][2].ToString() + "=" + Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[0][4].ToString()), 2)) + iddt.Rows[2][2].ToString();
                    }
                    #endregion
                    YOrderGrid.Rows.Add(new object[] { array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7], array[8], array[9], array[10], array[11], array[12], array[13], array[14], array[15], array[16], array[17], array[18], array[19], array[20], array[21], array[22], array[23], array[24] });
                }

            }
        }
        Session["ordergrid"] = YOrderGrid;
        OrderGrid.DataSource = YOrderGrid;
        OrderGrid.DataBind();
        if (bLimitedcInvName != "")
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('未找到商品:" + bLimitedcInvName + "的信息！剩余商品信息提取完成！');</script>");
        }
        else
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('信息全部提取完成！');</script>");
        }

    }

    protected void BtnSaveBackOrderNameOK_Click(object sender, EventArgs e) //保存临时订单
    {
        #region 保存临时订单
        //获取表头信息
        string lngopUserId = Session["lngopUserId"].ToString(); //用户id
        int bytStatus = 11;  //单据状态(临时订单)
        string ccuscode = Session["KPDWcCusCode"].ToString();   //开票单位编码
        string cdefine9 = "";
        if (Session["cdefine9"] != null)
        {
            cdefine9 = Session["cdefine9"].ToString();   //自定义项1,收货人姓名,cDefine9
        }
        string cdefine12 = "";
        if (Session["cdefine12"] != null)
        {
            cdefine12 = Session["cdefine12"].ToString();   //自定义项2,收货人电话,cDefine12
        }
        string cdefine11 = TxtOrderShippingMethod.Text.ToString();   //自定义项11,收货人地址,cDefine11
        string cdefine1 = "";
        if (Session["cdefine1"] != null)
        {
            cdefine1 = Session["cdefine1"].ToString();   //自定义项8,司机姓名,cDefine1
        }
        string cdefine13 = "";
        if (Session["cdefine13"] != null)
        {
            cdefine13 = Session["cdefine13"].ToString();   //自定义项9,司机电话,cDefine13
        }
        string cdefine2 = "";
        if (Session["cdefine2"] != null)
        {
            cdefine2 = Session["cdefine2"].ToString();   //自定义项10,司机身份证,cDefine2
        }
        string cdefine3 = "";
        if (Session["cdefine3"] != null)
        {
            cdefine3 = Session["cdefine3"].ToString();   //自定义项3,汽车类型,cdefine3
        }
        string cdefine10 = "";
        if (Session["cdefine10"] != null)
        {
            cdefine10 = Session["cdefine10"].ToString();   //自定义项12,车牌号,cDefine10
        }
        string ccusname = TxtCustomer.Text; //客户名称             
        string strRemarks = TxtOrderMark.Text.ToString();   //备注 
        string cpersoncode = TxtSalesman.Text.ToString();   //业务员编码
        string cSCCode = "";    //发运方式编码
        if (Session["TxtcSCCode"] != null)
        {
            if (Session["TxtcSCCode"].ToString() == "配送" || Session["TxtcSCCode"].ToString() == "01")     //发运方式编码,00:自提,01:厂车配送
            {
                Session["TxtcSCCode"] = "01";
                cSCCode = "01";
            }
            else
            {
                Session["TxtcSCCode"] = "00";
                cSCCode = "00";
            }
        }
        //11-18新增字段赋值
        string strLoadingWays = TxtLoadingWays.Text.ToString();     //装车方式
        string cSTCode = "00";     //销售类型编码  
        string lngopUseraddressId = "";
        if (Session["lngopUseraddressId"] != null)
        {
            lngopUseraddressId = Session["lngopUseraddressId"].ToString();
        }
        int lngBillType = 0;
        string RelateU8NO = "";
        string strBillName = "临时订单";
        if (TxtSaveBackOrderName != null)
        {
            if (TxtSaveBackOrderName.Text != "")
            {
                strBillName = TxtSaveBackOrderName.Text.Trim().ToString();
            }
        }
        //插入表头信息
        DataTable DTlngopOrderBackId = new OrderManager().DLproc_AddOrderBackByIns(lngopUserId, strBillName, bytStatus, strRemarks, ccuscode, cdefine1, cdefine2, cdefine3, cdefine9, cdefine10, cdefine11, cdefine12, cdefine13, ccusname, cpersoncode, cSCCode, strLoadingWays, cSTCode, lngopUseraddressId, RelateU8NO, lngBillType);
        if (DTlngopOrderBackId.Rows.Count < 1)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('保存临时订单失败,请联系系统管理员！');</script>");
            return;
        }
        //获取表头返回的表id
        Int32 lngopOrderBackId = Convert.ToInt32(DTlngopOrderBackId.Rows[0]["lngopOrderBackId"].ToString());
        //获取表体信息,并插入
        if (OrderGrid.VisibleRowCount <= 0)
        {
            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('保存成功！');</script>");
            //return;
        }
        else
        {
            //DataTable DTordergird = (DataTable)Session["ordergrid"];
            string cinvcode = "";
            string cinvname = "";
            double cComUnitQTY = 0;
            double cInvDefine1QTY = 0;
            double cInvDefine2QTY = 0;
            double cInvDefineQTY = 0;
            double cComUnitAmount = 0;
            string pack = "";
            string irowno = "1";
            for (int i = 0; i < OrderGrid.VisibleRowCount; i++)
            {
                cinvcode = OrderGrid.GetRowValues(i, "cInvCode").ToString();
                cinvname = OrderGrid.GetRowValues(i, "cInvName").ToString();
                cComUnitQTY = Convert.ToDouble(OrderGrid.GetRowValues(i, "cComUnitQTY").ToString());
                cInvDefine1QTY = Convert.ToDouble(OrderGrid.GetRowValues(i, "cInvDefine1QTY").ToString());
                cInvDefine2QTY = Convert.ToDouble(OrderGrid.GetRowValues(i, "cInvDefine2QTY").ToString());
                cInvDefineQTY = Convert.ToDouble(OrderGrid.GetRowValues(i, "cInvDefineQTY").ToString());
                cComUnitAmount = Convert.ToDouble(OrderGrid.GetRowValues(i, "cComUnitAmount").ToString());
                pack = OrderGrid.GetRowValues(i, "pack").ToString();
                irowno = OrderGrid.GetRowValues(i, "irowno").ToString();
                bool c = new OrderManager().DLproc_AddOrderBackDetailByIns(lngopOrderBackId, cinvcode, cinvname, cComUnitQTY, cInvDefine1QTY, cInvDefine2QTY, cInvDefineQTY, cComUnitAmount, pack, irowno);
            }
            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('保存成功！');</script>");
            //return;
        }
        //刷新临时表数据
        DataTable dtBackOrderGridView = new OrderManager().DL_GetOrderBackBySel(Convert.ToInt32(Session["lngopUserId"].ToString()));
        BackOrderGridView.DataSource = dtBackOrderGridView;
        BackOrderGridView.DataBind();
        #endregion
        TxtSaveBackOrderName.Text = "";
        Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('临时订单保存成功！');</script>");
        return;
    }

    protected void BackOrderGridView_Load(object sender, EventArgs e)   //Load读取临时订单列表信息
    {
        DataTable dtBackOrderGridView = new OrderManager().DL_GetOrderBackBySel(Convert.ToInt32(Session["lngopUserId"].ToString()));
        BackOrderGridView.DataSource = dtBackOrderGridView;
        BackOrderGridView.DataBind();
    }

    protected void BackOrderGridView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)//删除临时订单列表信息
    {
        int lngopOrderBackId = 0;
        lngopOrderBackId = Convert.ToInt32(BackOrderGridView.GetRowValues(BackOrderGridView.FocusedRowIndex, "lngopOrderBackId").ToString());
        //删除焦点行数据
        bool c = new OrderManager().DL_DelOrderBackDetailByDel(lngopOrderBackId);
        e.Cancel = true;
        //重新取值
        DataTable dtBackOrderGridView = new OrderManager().DL_GetOrderBackBySel(Convert.ToInt32(Session["lngopUserId"].ToString()));
        BackOrderGridView.DataSource = dtBackOrderGridView;
        BackOrderGridView.DataBind();

    }

    protected void TreeListxzq_CustomJSProperties(object sender, TreeListCustomJSPropertiesEventArgs e)    //行政区树结构
    {
        ASPxTreeList treeList = sender as ASPxTreeList;
        Hashtable nameTable = new Hashtable();
        foreach (TreeListNode node in treeList.GetVisibleNodes())
            //nameTable.Add(node.Key, string.Format("{0} {1}", node["vsimpleName"], node["vdescription"]));
            nameTable.Add(node.Key, string.Format("{0}", node["vdescription"]));
        e.Properties["cpvsimpleName"] = nameTable;
        treeList.ExpandToLevel(0);
    }

    protected void btxzqOK_Click(object sender, EventArgs e)        //选择 行政区
    {
        if (xzq_GV.GetRowValues(xzq_GV.FocusedRowIndex, "xzq") != null)
        {
            string xzq = xzq_GV.GetRowValues(xzq_GV.FocusedRowIndex, "xzq").ToString();
            Txtztadd.Text = xzq.ToString();
        }
        else
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('请选行政区！');</script>");
        }
    }

    protected void xzq_GV_Init(object sender, EventArgs e)
    {
        DataTable dt = new BasicInfoManager().DL_UserAddressZTXZQBySel(Session["ConstcCusCode"].ToString());
        xzq_GV.DataSource = dt;
        xzq_GV.DataBind();
    }       //初始化行政区

    protected void xzq_SelectedIndexChanged(object sender, EventArgs e)     //送货行政区变化,刷新
    {
        DataTable dt = new BasicInfoManager().DL_UserAddressZTXZQBySel(Session["ConstcCusCode"].ToString());
        xzq_GV.DataSource = dt;
        xzq_GV.DataBind();
    }

    protected void OrderGrid_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)  //顺序调整按钮
    {
        if (e.ButtonID == "RowsNoBtnUp")
        {
            //int irow = OrderGrid.FocusedRowIndex + 1;
            //int icount = OrderGrid.VisibleRowCount;
            //if (irow != 1)
            //{
            //DataTable dt = (DataTable)Session["ordergrid"];
            //DataTable dt2 = new DataTable();
            //dt2 = dt.Copy();
            //dt2.Rows.Clear();
            //dt2.ImportRow(dt.Rows[irow - 1]);//这是加入的是第一行
            //DataRow drr = dt.NewRow();
            //drr.ItemArray = dt2.Rows[0].ItemArray;
            ////dt.Rows[irow - 1].Delete();
            //dt.Rows.Remove(dt.Rows[irow - 1]);
            //dt.Rows.InsertAt(drr, irow - 2);

            ////dt.AcceptChanges();
            ////更新顺序
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    dt.Rows[i]["irowno"] = i + 1;
            //}
            //Session["ordergrid"] = dt;
            //OrderGrid.DataSource = dt;
            //OrderGrid.DataBind();
            //}
            // 按钮1方法
            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('一点击按钮！');</script>");
            int irow = OrderGrid.FocusedRowIndex + 1;
            int icount = OrderGrid.VisibleRowCount;
            if (irow != 1)
            {
                DataTable dt = (DataTable)Session["ordergrid"];
                //dt.Columns["irowno"].DataType = typeof(int);//指定irowno为Int类型，包含数据的情况下不能改变
                dt.Rows[irow - 1]["irowno"] = irow - 1;
                dt.Rows[irow - 2]["irowno"] = irow;
                DataTable newdt = dt.Clone();  // 克隆dt 的结构，包括所有 dt 架构和约束,并无数据； 
                //newdt.Columns["irowno"].DataType = typeof(int);//指定irowno为Int类型
                DataRow[] rows = dt.Select("1 = 1", "irowno asc");  // 从dt 中查询符合条件的记录；
                foreach (DataRow row in rows)  // 将查询的结果添加到dt中； 
                {
                    newdt.Rows.Add(row.ItemArray);
                }
                for (int i = 0; i < newdt.Rows.Count; i++)
                {
                    newdt.Rows[i]["irowno"] = i + 1;
                }
                OrderGrid.DataSource = newdt;
                OrderGrid.DataBind();
                Session["ordergrid"] = newdt;
            }
        }
        if (e.ButtonID == "RowsNoBtnDown")
        {
            //int irow = OrderGrid.FocusedRowIndex + 1;
            //int icount = OrderGrid.VisibleRowCount;
            //if (irow != icount)
            //{
            //    DataTable dt = (DataTable)Session["ordergrid"];
            //    DataTable dt2 = new DataTable();
            //    dt2 = dt.Copy();
            //    dt2.Rows.Clear();
            //    dt2.ImportRow(dt.Rows[irow - 1]);//这是加入的是第一行
            //    DataRow drr = dt.NewRow();
            //    drr.ItemArray = dt2.Rows[0].ItemArray;
            //    //dt.Rows[irow - 1].Delete();
            //    dt.Rows.Remove(dt.Rows[irow - 1]);
            //    dt.Rows.InsertAt(drr, irow);

            //    //dt.AcceptChanges();
            //    //更新顺序
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        dt.Rows[i]["irowno"] = i + 1;
            //    }
            //    Session["ordergrid"] = dt;
            //    OrderGrid.DataSource = dt;
            //    OrderGrid.DataBind();
            //}
            // 按钮1方法
            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('一点击按钮！');</script>");
            int irow = OrderGrid.FocusedRowIndex + 1;
            int icount = OrderGrid.VisibleRowCount;
            if (irow != icount)
            {
                DataTable dt = (DataTable)Session["ordergrid"];
                //dt.Columns["irowno"].DataType = typeof(int);//指定irowno为Int类型，包含数据的情况下不能改变
                dt.Rows[irow - 1]["irowno"] = irow + 1;
                dt.Rows[irow]["irowno"] = irow;
                DataTable newdt = dt.Clone();  // 克隆dt 的结构，包括所有 dt 架构和约束,并无数据； 
                //newdt.Columns["irowno"].DataType = typeof(int);//指定irowno为Int类型
                DataRow[] rows = dt.Select("1 = 1", "irowno asc");  // 从dt 中查询符合条件的记录；
                foreach (DataRow row in rows)  // 将查询的结果添加到dt中； 
                {
                    newdt.Rows.Add(row.ItemArray);
                }
                for (int i = 0; i < newdt.Rows.Count; i++)
                {
                    newdt.Rows[i]["irowno"] = i + 1;
                }
                OrderGrid.DataSource = newdt;
                OrderGrid.DataBind();
                Session["ordergrid"] = newdt;
            }
        }
    }


}