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
using System.Text.RegularExpressions;
using DevExpress.Web.ASPxTreeList;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Configuration;

public partial class OrderModify : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //开启时间管理
        DataTable timecontrol = new OrderManager().DL_OrderENTimeControlBySel();
        if (timecontrol.Rows.Count < 1)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('" + ConfigurationManager.AppSettings["datOrderTime"].ToString() + "');window.parent.location.href='AllBlank.aspx';</script>");
            return;
        }

        //逾期30天未确认//延期欠款天未确认
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


        #region 判断session是否存在,并且建立datatable,用于记录选择项目,gridselectordermodify
        if (Session["gridselectordermodify"] == null)
        {
            DataTable dts = new DataTable();
            dts.Columns.Add("cInvCode"); //编码    0            
            //dt.Rows.Add(new object[] { "0"});
            Session["gridselectordermodify"] = dts;
        }
        #endregion
        //绑定treelist数据源
        string treecuscode = "";
        //if (Session["ModifyTxtCustomer"] == null)
        //{
        //    treecuscode = Session["ConstcCusCode"].ToString();
        //}
        //else
        //{
        //    treecuscode = Session["ModifyTxtCustomer"].ToString();
        //}
        treecuscode = Session["ModifyKPDWcCusCode"].ToString();
        DataTable dttree = new SearchManager().DLproc_InventoryBySel("00", treecuscode);
        treeList.KeyFieldName = "KeyFieldName";
        treeList.ParentFieldName = "ParentFieldName";
        treeList.DataSource = dttree;
        /*展开第一级node*/
        treeList.DataBind();
        treeList.ExpandToLevel(1);
        //绑定gridview数据源
        DataTable dt1 = new DataTable();
        if (Session["ordertreelistgridmodify"] != null)
        {
            dt1 = new SearchManager().DLproc_TreeListDetailsAllBySel(Session["ordertreelistgridmodify"].ToString(), Session["KPDWcCusCode"].ToString(), 1);
            TreeDetail.DataSource = dt1;
            TreeDetail.DataBind();
            //删除当前绑定的数据,因为切换分类,所以需要删除
            DataTable dtst = (DataTable)Session["gridselectordermodify"];
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
        }

        //OrderGrid修改提示功能
        OrderGrid.JSProperties.Remove("cpAlertMsg");//先清空
        OrderGrid.JSProperties["cpAlertMsg"] = "订单明细保存成功!";

        DataTable griddt = new DataTable();
        if (IsPostBack)  //回传
        {
            String target = Request.Form["__EVENTTARGET"];
            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('"+target+"');</script>");
            return;
        }
        else   //非回传,打开新的页面(添加/删除 物料)
        {
            //设置送货方式为,配送
            ComboShippingMethod.Value = "配送";
            ComboShippingMethod.Text = "配送";
            //绑定送货信息grid表
            griddt = new BasicInfoManager().DLproc_UserAddressPSBySelGroup(Session["lngopUserId"].ToString());
            GridViewShippingMethod.DataSource = griddt;
            GridViewShippingMethod.DataBind();
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
            if (Session["ModifyTxtOrderShippingMethod"] != null)
            {
                TxtOrderShippingMethod.Text = Session["ModifyTxtOrderShippingMethod"].ToString();
            }
            //表头字段赋值,顾客开票单位名称信息
            if (Session["ModifyTxtCustomer"] != null)
            {
                TxtCustomer.Text = Session["ModifyTxtCustomer"].ToString();
            }
            //表头字段赋值,业务员信息
            if (Session["ModifycCusPPerson"] != null)
            {
                TxtSalesman.Text = Session["ModifycCusPPerson"].ToString();
            }

            //备注cookie判断,读取
            if (HttpContext.Current.Request.Cookies["ModifyTxtOrderMark"] != null)
            {
                TxtOrderMark.Text = Server.UrlDecode(HttpContext.Current.Request.Cookies["ModifyTxtOrderMark"].Value);
            }
            else
            {
                //HttpCookie cookie = new HttpCookie("ModifyTxtOrderMark");//初使化并设置Cookie的名称
                //DateTime dt = DateTime.Now;
                //TimeSpan ts = new TimeSpan(0, 0, 180, 0, 0);//过期时间为1分钟
                //cookie.Expires = dt.Add(ts);//设置过期时间
                //cookie.Values.Add("ModifyTxtOrderMark", "userid_value");
                //Response.AppendCookie(cookie);
            }
            //装车方式cookie判断,读取
            if (HttpContext.Current.Request.Cookies["ModifyTxtLoadingWays"] != null)
            {
                TxtLoadingWays.Text = Server.UrlDecode(HttpContext.Current.Request.Cookies["ModifyTxtLoadingWays"].Value);
            }
            //提货时间cookie判断,读取
            if (HttpContext.Current.Request.Cookies["ModifyDeliveryDate"] != null)
            {
                DeliveryDate.Value = Server.UrlDecode(HttpContext.Current.Request.Cookies["ModifyDeliveryDate"].Value);
            }


            //表头字段赋值,顾客信用额度  ,2016-04-05,增加返回两个值,
            DataTable CusCreditDt = new DataTable();
            CusCreditDt = new OrderManager().DLproc_getCusCreditInfoWithBillno(Session["ModifyKPDWcCusCode"].ToString(), Session["OrderFrameModifyURL"].ToString());//开票单位编码
            TxtCusCredit.Text = CusCreditDt.Rows[0]["iCusCreLine"].ToString();
            TxtCBLX.Text = CusCreditDt.Rows[0]["cdiscountname"].ToString();         //2016-04-05,添加 酬宾类型 显示
        }
        TxtBiller.Text = Session["strUserName"].ToString(); //绑定制单人
        TxtBillDate.Text = System.DateTime.Now.ToString("d");   //绑定制单日期



        if (Request.QueryString["id"] == null)  //添加
        {
            if (Request.QueryString["code"] == null)    //删除
            {
                /*第一次加载页面!!!!第一次加载页面!!!第一次加载页面*/
                #region /*第一次加载页面!!!!第一次加载页面!!!第一次加载页面*/
                string StrBillNo = Session["OrderFrameModifyURL"].ToString();
                Session["ModifycCusCode"] = Session["cCusCode"].ToString();
                //01,
                #region 判断session是否存在,并且建立datatable
                if (Session["ModifyOrderGrid"] == null)
                {
                    //DataTable dt = new DataTable();
                    //dt.Columns.Add("cInvCode"); //编码    0
                    //dt.Columns.Add("cInvName"); //名称    1
                    //dt.Columns.Add("cInvStd");  //规格    2    
                    //dt.Columns.Add("cComUnitName"); //基本单位  3
                    //dt.Columns.Add("cInvDefine1"); //大包装单位  4
                    //dt.Columns.Add("cInvDefine2"); //小包装单位  5
                    //dt.Columns.Add("cInvDefine13");  //大包装换算率   6  
                    //dt.Columns.Add("cInvDefine14"); //小包装换算率    7
                    //dt.Columns.Add("UnitGroup"); //单位换算率组   8     
                    //dt.Columns.Add("cComUnitQTY"); //基本单位数量 9
                    //dt.Columns.Add("cInvDefine1QTY"); //大包装单位数量 10
                    //dt.Columns.Add("cInvDefine2QTY"); //小包装单位数量 11
                    //dt.Columns.Add("cInvDefineQTY"); //包装量数量汇总,包装量  12
                    //dt.Columns.Add("cComUnitPrice"); //基本单位单价(报价)   13
                    //dt.Columns.Add("cComUnitAmount"); //基本单位金额  14
                    //dt.Columns.Add("pack"); //包装量换算结果  15
                    //dt.Columns.Add("ExercisePrice"); //基本单位单价(执行价格)   16
                    //dt.Columns.Add("Stock"); //可用库存量   17
                    //dt.Columns.Add("kl"); //扣率   18
                    //dt.Columns.Add("cComUnitCode"); //基本单位编码   19
                    //dt.Columns.Add("iTaxRate"); //销项税率   20
                    //dt.Columns.Add("cn1cComUnitName"); //销售单位名称   21
                    ////dt.Rows.Add(new object[] { "dasdsad", "张1", "98", "94","","","","","" });
                    ////dt.Rows.Add(new object[] { "fdsfdfdsf", "张2", "99", "94","","","","","" });
                    //Session["ModifyOrderGrid"] = dt;
                    //绑定表体信息OrderGrid
                    DataTable dt = new OrderManager().DLproc_OrderDetailModifyBySel(StrBillNo);
                    //取消只读
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        dt.Columns[i].ReadOnly = false;
                    }
                    OrderGrid.DataSource = dt;
                    OrderGrid.DataBind();
                    Session["ModifyOrderGrid"] = dt;
                }
                #endregion
                //读取Session["OrderFrameModifyURL"](网单号)的内容//加载传递过来的OrderFrameModifyURL=strbillno,网单号,查询该单据的数据,并绑定到grid中
                DataTable ModifyOrderGriddt = new OrderManager().DL_OrderModifyBySel(StrBillNo);
                //送货信息赋值
                Session["ModifylngopUseraddressId"] = ModifyOrderGriddt.Rows[0]["lngopUseraddressId"].ToString();//送货地址ID
                Session["ModifycDefine9"] = ModifyOrderGriddt.Rows[0]["cdefine9"].ToString();//收货人姓名,	 
                Session["ModifycDefine12"] = ModifyOrderGriddt.Rows[0]["cdefine12"].ToString();//收货人电话,	 		
                Session["ModifycDefine11"] = ModifyOrderGriddt.Rows[0]["cdefine11"].ToString();//收货人地址,	 	 
                Session["ModifycDefine10"] = ModifyOrderGriddt.Rows[0]["cdefine10"].ToString();//车牌号,	 				
                Session["ModifycDefine1"] = ModifyOrderGriddt.Rows[0]["cdefine1"].ToString();//司机姓名,	 		
                Session["ModifycDefine13"] = ModifyOrderGriddt.Rows[0]["cdefine13"].ToString();//司机电话,	 		
                Session["ModifycDefine2"] = ModifyOrderGriddt.Rows[0]["cdefine2"].ToString();//司机身份证
                //过期时间：
                TxtdatExpTime.Text = ModifyOrderGriddt.Rows[0]["datExpTime1"].ToString();//到期时间
                //Session["ModifyTxtcdefine3"] = ModifyOrderGriddt.Rows[0]["cdefine3"].ToString();//车型 
                //绑定业务员:0037
                TxtSalesman.Text = ModifyOrderGriddt.Rows[0]["cpersoncode"].ToString();
                if (Session["ModifyTxtSalesman"] == null)
                {
                    Session["ModifyTxtSalesman"] = ModifyOrderGriddt.Rows[0]["cpersoncode"].ToString();
                }
                //开票单位 ccusname              
                if (Session["ModifyTxtCustomer"] == null)
                {
                    Session["ModifyTxtCustomer"] = ModifyOrderGriddt.Rows[0]["ccusname"].ToString();
                    Session["ModifyKPDWcCusCode"] = ModifyOrderGriddt.Rows[0]["ccuscode"].ToString();
                    TxtCustomer.Text = ModifyOrderGriddt.Rows[0]["ccusname"].ToString();
                }
                else
                {
                    TxtCustomer.Text = Session["ModifyTxtCustomer"].ToString();
                }
                //制单人,制单日期
                TxtBiller.Text = Session["strUserName"].ToString(); //绑定制单人
                TxtBillDate.Text = System.DateTime.Now.ToString("d");   //绑定制单日期
                //送货地址
                if (Session["ModifyTxtOrderShippingMethod"] == null)
                {
                    Session["ModifyTxtOrderShippingMethod"] = ModifyOrderGriddt.Rows[0]["cdefine11"].ToString();
                    TxtOrderShippingMethod.Text = ModifyOrderGriddt.Rows[0]["cdefine11"].ToString();
                }
                else
                {
                    TxtOrderShippingMethod.Text = Session["ModifyTxtOrderShippingMethod"].ToString();
                }
                //备注
                if (Session["ModifyTxtOrderMark"] == null)
                {
                    Session["ModifyTxtOrderMark"] = ModifyOrderGriddt.Rows[0]["strRemarks"].ToString();
                    TxtOrderMark.Text = ModifyOrderGriddt.Rows[0]["strRemarks"].ToString();
                }
                else
                {
                    TxtOrderMark.Text = Session["ModifyTxtOrderMark"].ToString();
                }
                //装车方式
                if (Session["ModifyTxtLoadingWays"] == null)
                {
                    Session["ModifyTxtLoadingWays"] = ModifyOrderGriddt.Rows[0]["strLoadingWays"].ToString();
                    TxtLoadingWays.Text = ModifyOrderGriddt.Rows[0]["strLoadingWays"].ToString();
                }
                else
                {
                    TxtLoadingWays.Text = Session["ModifyTxtLoadingWays"].ToString();
                }
                //交货时间
                if (Session["ModifyDeliveryDate"] == null)
                {
                    Session["ModifyDeliveryDate"] = ModifyOrderGriddt.Rows[0]["datDeliveryDate"].ToString();
                    DeliveryDate.Value = ModifyOrderGriddt.Rows[0]["datDeliveryDate"].ToString();
                }
                else
                {
                    DeliveryDate.Value = Session["ModifyDeliveryDate"].ToString();
                }
                //发运方式
                if (ModifyOrderGriddt.Rows[0]["cSCCode"].ToString() == "00")
                {
                    TxtcSCCode.Text = "自提";
                    Session["ModifyTxtcSCCode"] = "自提";
                }
                else
                {
                    TxtcSCCode.Text = "配送";
                    Session["ModifyTxtcSCCode"] = "配送";
                }
                //订单编号
                TxtOrderBillNo.Text = ModifyOrderGriddt.Rows[0]["strBillNo"].ToString();
                Session["ModifyTxtOrderBillNo"] = ModifyOrderGriddt.Rows[0]["strBillNo"].ToString();
                //销售类型
                if (ModifyOrderGriddt.Rows[0]["cSTCode"].ToString() == "00")
                {
                    TxtcSTCode.Text = "普通销售";
                }
                else
                {
                    TxtcSTCode.Text = "样品资料";
                }
                Session["ModifyTxtcSTCode"] = TxtcSTCode.Text.ToString();
                //车型
                if (Session["ModifyTxtcdefine3"] == null)
                {
                    Session["ModifyTxtcdefine3"] = ModifyOrderGriddt.Rows[0]["cdefine3"].ToString();
                    Txtcdefine3.Text = ModifyOrderGriddt.Rows[0]["cdefine3"].ToString();
                }
                else
                {
                    Txtcdefine3.Text = Session["ModifyTxtcdefine3"].ToString();
                }
                //下单时间
                TxtBillTime.Text = ModifyOrderGriddt.Rows[0]["datBillTime"].ToString();
                Session["ModifyTxtBillTime"] = ModifyOrderGriddt.Rows[0]["datBillTime"].ToString();

                //绑定GridViewShippingMethod数据,配送方式数据
                griddt = new BasicInfoManager().DLproc_UserAddressPSBySelGroup(Session["lngopUserId"].ToString());
                GridViewShippingMethod.DataSource = griddt;
                GridViewShippingMethod.DataBind();
                //绑定CustomerGrid,顾客开票信息
                DataTable DtComboCustomer = new DataTable();
                DtComboCustomer = new SearchManager().DL_ComboCustomerBySel(Session["ModifycCusCode"].ToString() + "%");
                CustomerGrid.DataSource = DtComboCustomer;
                CustomerGrid.DataBind();
                //绑定车型信息grid表
                DataTable cdefine3Griddt = new BasicInfoManager().DL_cdefine3BySel();
                cdefine3Grid.DataSource = cdefine3Griddt;
                cdefine3Grid.DataBind();
                return;     //结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束
            }
                #endregion

            //结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束结束

            #region 删除物料操作,2016之后不会操作,id==null时触发
            else
            {
                //发运方式
                TxtcSCCode.Text = Session["ModifyTxtcSCCode"].ToString();
                //备注
                TxtOrderMark.Text = Session["ModifyTxtOrderMark"].ToString();
                //订单编号
                TxtOrderBillNo.Text = Session["ModifyTxtOrderBillNo"].ToString();
                //备注
                TxtOrderMark.Text = Session["ModifyTxtOrderMark"].ToString();
                /*2015-11-22add*/
                //装车方式
                TxtLoadingWays.Text = Session["ModifyTxtLoadingWays"].ToString();
                //交货日期
                DeliveryDate.Value = Session["ModifyDeliveryDate"].ToString();
                //销售类型
                TxtcSTCode.Text = Session["ModifyTxtcSTCode"].ToString();
                //车型
                Txtcdefine3.Text = Session["ModifyTxtcdefine3"].ToString();
                //下单时间
                TxtBillTime.Text = Session["ModifyTxtBillTime"].ToString();

                /*重新读取cookie内容*/
                //备注cookie判断,读取
                if (HttpContext.Current.Request.Cookies["ModifyTxtOrderMark"] != null)
                {
                    TxtOrderMark.Text = Server.UrlDecode(HttpContext.Current.Request.Cookies["ModifyTxtOrderMark"].Value);
                }
                //装车方式cookie判断,读取
                if (HttpContext.Current.Request.Cookies["ModifyTxtLoadingWays"] != null)
                {
                    TxtLoadingWays.Text = Server.UrlDecode(HttpContext.Current.Request.Cookies["ModifyTxtLoadingWays"].Value);
                }
                //提货时间cookie判断,读取
                if (HttpContext.Current.Request.Cookies["ModifyDeliveryDate"] != null)
                {
                    DeliveryDate.Value = Server.UrlDecode(HttpContext.Current.Request.Cookies["ModifyDeliveryDate"].Value);
                }

                //删除选择项后绑定数据
                griddt = (DataTable)Session["ModifyOrderGrid"];
                for (int i = 0; i < griddt.Rows.Count; i++)
                {
                    if (griddt.Rows[i][0].ToString() == Request.QueryString["code"].ToString())
                    {
                        griddt.Rows[i].Delete();
                        griddt.AcceptChanges();
                        OrderGrid.DataSource = griddt;
                        OrderGrid.DataBind();
                        //绑定session数据
                        Session["ModifyOrderGrid"] = griddt;

                        TxtOrderBillNo.Text = Session["OrderFrameModifyURL"].ToString();
                        return;     //结束
                    }
                }
            }
            #endregion

        }

        //发运方式
        TxtcSCCode.Text = Session["ModifyTxtcSCCode"].ToString();
        //订单编号
        TxtOrderBillNo.Text = Session["ModifyTxtOrderBillNo"].ToString();
        //备注
        TxtOrderMark.Text = Session["ModifyTxtOrderMark"].ToString();
        /*2015-11-22add*/
        //发运方式
        TxtcSCCode.Text = Session["ModifyTxtcSCCode"].ToString();
        //装车方式
        TxtLoadingWays.Text = Session["ModifyTxtLoadingWays"].ToString();
        //交货日期
        DeliveryDate.Value = Session["ModifyDeliveryDate"].ToString();
        //销售类型
        TxtcSTCode.Text = Session["ModifyTxtcSTCode"].ToString();
        //车型
        Txtcdefine3.Text = Session["ModifyTxtcdefine3"].ToString();
        //下单时间
        TxtBillTime.Text = Session["ModifyTxtBillTime"].ToString();
        /*重新读取cookie内容*/
        //备注cookie判断,读取
        if (HttpContext.Current.Request.Cookies["ModifyTxtOrderMark"] != null)
        {
            TxtOrderMark.Text = Server.UrlDecode(HttpContext.Current.Request.Cookies["ModifyTxtOrderMark"].Value);
        }
        //装车方式cookie判断,读取
        if (HttpContext.Current.Request.Cookies["ModifyTxtLoadingWays"] != null)
        {
            TxtLoadingWays.Text = Server.UrlDecode(HttpContext.Current.Request.Cookies["ModifyTxtLoadingWays"].Value);
        }
        //提货时间cookie判断,读取
        if (HttpContext.Current.Request.Cookies["ModifyDeliveryDate"] != null)
        {
            DeliveryDate.Value = Server.UrlDecode(HttpContext.Current.Request.Cookies["ModifyDeliveryDate"].Value);
        }


        string id = Request.QueryString["id"].ToString();
        //获取物料详细信息,单位,价格等
        //DataTable iddt = new OrderManager().DLproc_QuasiOrderDetailBySel(id, Session["ModifycCusCode"].ToString());
        DataTable iddt = new OrderManager().DLproc_QuasiOrderDetailModifyBySel(id, Session["ModifycCusCode"].ToString(), Session["ModifyTxtOrderBillNo"].ToString());

        //获取传递过来的参数,并存入数组
        //string[] array = id.Split('|');   //10-25取消传参,改为下面的查询后在绑定

        //检测是否重复添加物料
        griddt = (DataTable)Session["ModifyOrderGrid"];
        if (griddt.Rows.Count > 0)
        {
            //Label1.Text = griddt.Rows.Count.ToString();
            for (int i = 0; i < griddt.Rows.Count; i++)
            {
                if (griddt.Rows[i][0].ToString() == id.ToString())
                {
                    //如果已经存在该物料,提示并且返回
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('该物料已经被添加！');</script>");
                    OrderGrid.DataSource = griddt;
                    OrderGrid.DataBind();
                    return;
                }
            }
        }
        //将传递过来的数据放入datatable中,并且绑定gridview
        //griddt.Rows.Add(new object[] { array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7], array[8], "0", "0", "0", "0", "88", "9" });
        string[] array = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24" };
        array[0] = iddt.Rows[0][3].ToString();//存货编码
        array[1] = iddt.Rows[0][0].ToString();//存货名称
        array[2] = iddt.Rows[0][1].ToString();//存货规格
        array[3] = iddt.Rows[0][2].ToString();//基本单位
        array[17] = iddt.Rows[0][9].ToString(); //库存可用量
        array[18] = iddt.Rows[0][8].ToString(); //扣率
        array[19] = iddt.Rows[0][11].ToString(); //基本单位编码
        array[20] = iddt.Rows[0][12].ToString(); //销项税率
        array[21] = iddt.Rows[0][13].ToString(); //销售单位名称
        array[22] = Convert.ToString(Convert.ToInt32(griddt.Rows.Count) + 1);
        array[23] = iddt.Rows[0]["iLowSumRel"].ToString(); //最低库存，23
        array[24] = iddt.Rows[0]["cDefine24"].ToString(); //活动类型，24
        //10.25从传递过来的参数分解数组,改为传递物料的编码进行查询,在循环添加
        if (iddt.Rows.Count == 1)
        {
            array[4] = iddt.Rows[0][2].ToString();
            array[5] = iddt.Rows[0][2].ToString();
            array[6] = iddt.Rows[0][4].ToString();
            array[7] = iddt.Rows[0][4].ToString();
            array[8] = Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[0][4].ToString()), 2)) + iddt.Rows[0][2].ToString() + "=" + Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[0][4].ToString()), 2)) + iddt.Rows[0][2].ToString() + "=" + Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[0][4].ToString()), 2)) + iddt.Rows[0][2].ToString();
            array[9] = "0";
            array[10] = "0";
            array[11] = "0";
            array[12] = "0";
            array[13] = iddt.Rows[0][15].ToString();
            array[14] = "0";
            array[15] = "0";
            array[16] = iddt.Rows[0][14].ToString();
        }
        if (iddt.Rows.Count == 2)
        {
            array[4] = iddt.Rows[1][2].ToString();
            array[5] = iddt.Rows[1][2].ToString();
            array[6] = iddt.Rows[1][4].ToString();
            array[7] = iddt.Rows[1][4].ToString();
            array[8] = Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[1][4].ToString()), 2)) + iddt.Rows[0][2].ToString() + "=" + Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[1][4].ToString()) / Convert.ToDouble(iddt.Rows[1][4].ToString()), 2)) + iddt.Rows[1][2].ToString() + "=" + Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[1][4].ToString()) / Convert.ToDouble(iddt.Rows[1][4].ToString()), 2)) + iddt.Rows[1][2].ToString();
            array[9] = "0";
            array[10] = "0";
            array[11] = "0";
            array[12] = "0";
            array[13] = iddt.Rows[0][15].ToString();
            array[14] = "0";
            array[15] = "0";
            array[16] = iddt.Rows[0][14].ToString();
        }
        if (iddt.Rows.Count >= 3)
        {
            array[4] = iddt.Rows[2][2].ToString();
            array[5] = iddt.Rows[1][2].ToString();
            array[6] = iddt.Rows[2][4].ToString();
            array[7] = iddt.Rows[1][4].ToString();
            array[8] = Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[2][4].ToString()), 2)) + iddt.Rows[0][2].ToString() + "=" + Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[2][4].ToString()) / Convert.ToDouble(iddt.Rows[1][4].ToString()), 2)) + iddt.Rows[1][2].ToString() + "=" + Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[0][4].ToString()), 2)) + iddt.Rows[2][2].ToString();
            array[9] = "0";
            array[10] = "0";
            array[11] = "0";
            array[12] = "0";
            array[13] = iddt.Rows[0][15].ToString();
            array[14] = "0";
            array[15] = "0";
            array[16] = iddt.Rows[0][14].ToString();
        }
        griddt.Rows.Add(new object[] { array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7], array[8], array[9], array[10], array[11], array[12], array[13], array[14], array[15], array[16], array[17], array[18], array[19], array[20], array[21], array[22], array[23], array[24] });
        Session["ModifyOrderGrid"] = griddt;
        OrderGrid.DataSource = griddt;
        OrderGrid.DataBind();
    }

    protected void btOK_Click(object sender, EventArgs e)   //选择完成送货信息
    {
        string ShippingInformation = GridViewShippingMethod.GetRowValues(GridViewShippingMethod.FocusedRowIndex, "ShippingInformation").ToString();
        TxtOrderShippingMethod.Text = ShippingInformation.ToString();
        TxtcSCCode.Text = ComboShippingMethod.Value.ToString();
        Session["ModifyTxtOrderShippingMethod"] = TxtOrderShippingMethod.Text.ToString();//送货信息赋值
        Session["ModifyTxtcSCCode"] = TxtcSCCode.Text.ToString(); //发运方式编码赋值
        /*2015-11-22add*/
        Session["Modifycdefine1"] = GridViewShippingMethod.GetRowValues(GridViewShippingMethod.FocusedRowIndex, "strDriverName").ToString();//司机姓名,	 
        Session["Modifycdefine2"] = GridViewShippingMethod.GetRowValues(GridViewShippingMethod.FocusedRowIndex, "strIdCard").ToString();//司机身份证 
        Session["Modifycdefine9"] = GridViewShippingMethod.GetRowValues(GridViewShippingMethod.FocusedRowIndex, "strConsigneeName").ToString();//收货人姓名,
        Session["Modifycdefine10"] = GridViewShippingMethod.GetRowValues(GridViewShippingMethod.FocusedRowIndex, "strCarplateNumber").ToString();//车牌号,
        Session["Modifycdefine11"] = GridViewShippingMethod.GetRowValues(GridViewShippingMethod.FocusedRowIndex, "strReceivingAddress").ToString();//收货人地址,
        Session["Modifycdefine12"] = GridViewShippingMethod.GetRowValues(GridViewShippingMethod.FocusedRowIndex, "strConsigneeTel").ToString();//收货人电话,
        Session["Modifycdefine13"] = GridViewShippingMethod.GetRowValues(GridViewShippingMethod.FocusedRowIndex, "strDriverTel").ToString();//司机电话,
        Session["ModifylngopUseraddressId"] = GridViewShippingMethod.GetRowValues(GridViewShippingMethod.FocusedRowIndex, "lngopUseraddressId").ToString();//送货地址ID
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

    protected void GridViewShippingMethod_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e) //生成Grid的序号
    {
        if (e.Column.Caption == "序号" && e.IsGetData)
        {
            e.Value = (e.ListSourceRowIndex + 1).ToString();
        }

        if (e.Column.Caption == "执行金额" && e.IsGetData)
        {
            //e.Value = (e.ListSourceRowIndex + 1).ToString();
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
    protected void TxtOrderMark_TextChanged(object sender, EventArgs e) //订单备注文本变更赋值
    {
        Session["ModifyTxtOrderMark"] = TxtOrderMark.Text.ToString();
    }

    protected void BtnCustomerOk_Click(object sender, EventArgs e)      //客户信息选择事件
    {
        //开票单位名称
        string cCusName = CustomerGrid.GetRowValues(CustomerGrid.FocusedRowIndex, "cCusName").ToString();
        TxtCustomer.Text = cCusName.ToString();
        Session["ModifyTxtCustomer"] = TxtCustomer.Text.ToString();
        //开票单位编码
        Session["ModifyKPDWcCusCode"] = CustomerGrid.GetRowValues(CustomerGrid.FocusedRowIndex, "cCusCode").ToString();
        //业务员
        //TxtSalesman.Text = CustomerGrid.GetRowValues(CustomerGrid.FocusedRowIndex, "cCusPPerson").ToString();
        //Session["cCusPPerson"] = TxtSalesman.Text.ToString();
        //重新选择开票单位后,重新读取开票单位的存货单价,并且计算金额,重新绑定表体grid,11-01
        DataTable griddata = new DataTable();
        griddata = (DataTable)Session["ModifyOrderGrid"];
        //如果不为空,则遍历赋值
        if (griddata.Rows.Count > 0)
        {
            for (int i = 0; i < griddata.Rows.Count; i++)       //重新取价
            {
                //DataTable iddt = new OrderManager().DLproc_QuasiOrderDetailBySel(griddata.Rows[i]["cInvCode"].ToString(), Session["ModifyKPDWcCusCode"].ToString());
                DataTable iddt = new OrderManager().DLproc_QuasiOrderDetailModifyBySel(griddata.Rows[i]["cInvCode"].ToString(), Session["ModifyKPDWcCusCode"].ToString(), Session["ModifyTxtOrderBillNo"].ToString());
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
                bool yxx = new OrderManager().DLproc_cInvCodeIsBeLimitedBySel(griddata.Rows[i]["cInvCode"].ToString(), Session["KPDWcCusCode"].ToString(), 1); //20170421新允限销,增加存货档案属性判断
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
            OrderGrid.DataSource = griddata;
            OrderGrid.DataBind();
            Session["ModifyOrderGrid"] = griddata;
        }
        //重新获取信用额度
        DataTable CusCreditDt = new DataTable();
        CusCreditDt = new OrderManager().DLproc_getCusCreditInfoWithBillno(Session["ModifyKPDWcCusCode"].ToString(), Session["OrderFrameModifyURL"].ToString());//开票单位编码
        TxtCusCredit.Text = CusCreditDt.Rows[0]["iCusCreLine"].ToString();

    }

    public string GetNewBillNo()
    {

        return "";
    }

    protected void BtnSaveOrder_Click(object sender, EventArgs e)   //保存并提交订单按钮事件
    {
        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script>alert('该功能正在完善,订单保存成功,返回处理界面！');{window.parent.location ='PendingOrder.aspx'}</script>");
        //Response.Redirect(Request.QueryString["ourl"].ToString());
        //return;
        ////创建datatable数据;
        DataTable griddata = new DataTable();
        griddata = (DataTable)Session["ModifyOrderGrid"];

        #region 检测数据有效性

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
        if (TxtSalesman.Text == "")
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('请填写业务员！');</script>");
            return;
        }
        if (DeliveryDate.Text == "")
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('请选择交货日期！');</script>");
            return;
        }
        //2,检测是否有数据
        if (Convert.ToDateTime(DeliveryDate.Value) <= DateTime.Now && DeliveryDate.Value != null) //检验交货日期
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('提货时间不能小于当前时间！');</script>");
            return;
        }
        if (griddata.Rows.Count <= 0)
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
        //if (nValidTimeData.Rows.Count > 0 && nValidTimeData.Rows[0]["Isvaild"].ToString() == "0")
        //{
        //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('今天该开票单位的活动时间已关闭！');</script>");
        //    return;
        //}


        //2.1 检测是否过期，Session["ModifyTxtOrderBillNo"].ToString()
        string stb = Session["ModifyTxtOrderBillNo"].ToString();
        bool bExp = new OrderManager().DL_OrderIsExpBySel(stb.ToString());
        if (bExp == false)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单已过期！请重新下单！');</script>");
            return;
        }

        //检测价格等级变化
        #region 检测价格等级变化
        if (griddata.Rows.Count > 0)
        {
            int PriceUpdateCount = 0;
            for (int i = 0; i < griddata.Rows.Count; i++)
            {
                DataTable dtprice = new OrderManager().DLproc_QuasiPriceBySel(griddata.Rows[i]["cInvCode"].ToString(), Session["ModifyKPDWcCusCode"].ToString());
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
                    //执行价格更新
                    if (Convert.ToDouble(dtprice.Rows[0]["ExercisePrice"].ToString()) != Convert.ToDouble(griddata.Rows[i]["ExercisePrice"].ToString()))
                    {
                        //double qty = Convert.ToDouble(griddata.Rows[i]["cComUnitQTY"].ToString());
                        double price = Math.Round(Convert.ToDouble(dtprice.Rows[0]["ExercisePrice"].ToString()), 6); ;
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
                Session["ModifyOrderGrid"] = griddata;
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
            //CusCredit += Convert.ToDouble(griddata.Rows[i]["cComUnitAmount"].ToString());
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
        //信用额度,20151129变更,增加临时授权,Customer中的cCusPostCode ,1为临时授权,授权后设置为 null(恢复操作在存储过程中完成)
        DataTable ExtraCredit = new DataTable();
        ExtraCredit = new OrderManager().DL_ExtraCreditBySel(Session["ModifyKPDWcCusCode"].ToString());
        if (ExtraCredit.Rows.Count > 0)
        {

        }
        else
        {
            DataTable CusCreditDt = new DataTable();
            CusCreditDt = new OrderManager().DLproc_getCusCreditInfoWithBillno(Session["ModifyKPDWcCusCode"].ToString(), Session["OrderFrameModifyURL"].ToString());//开票单位编码
            TxtCusCredit.Text = CusCreditDt.Rows[0]["iCusCreLine"].ToString();
            TxtCBLX.Text = CusCreditDt.Rows[0]["cdiscountname"].ToString();         //2016-04-05,添加 酬宾类型 显示
            //TxtCusCredit.Text = CusCreditDt.Rows[0]["iCusCreLine"].ToString();//信用额
            if (CusCredit > Convert.ToDouble(CusCreditDt.Rows[0]["iCusCreLine"].ToString()) && Convert.ToDouble(CusCreditDt.Rows[0]["iCusCreLine"].ToString()) != -99999999)
            //if (CusCredit > Convert.ToDouble(TxtCusCredit.Text) && Convert.ToDouble(TxtCusCredit.Text) != -99999999)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('已经超过您开票单位的信用额度:" + Convert.ToString((Convert.ToDouble(CusCreditDt.Rows[0]["iCusCreLine"].ToString()) - CusCredit)) + "！');</script>");
                return;
            }
        }
        //4,检测可用库存量是否正确,先更新库存,再调用OrderGrid_RowValidating(暂时无法调用,遍历解决)
        if (TxtcSTCode.Text != "样品资料")
        {
            DataTable stockdt = new DataTable();
            bool d = false;
            for (int i = 0; i < griddata.Rows.Count; i++)
            {
                //Session["ModifyTxtOrderBillNo"] 
                stockdt = new OrderManager().DLproc_OrderDetailModifyStockQtyCompareBySel(griddata.Rows[i]["cInvCode"].ToString(), TxtOrderBillNo.Text);
                griddata.Rows[i]["Stock"] = stockdt.Rows[0]["qty"].ToString();
                if (Convert.ToDouble(griddata.Rows[i]["Stock"].ToString()) < Convert.ToDouble(griddata.Rows[i]["cInvDefineQTY"].ToString()))
                {
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
        }
        #endregion

        //检测2016-03-08,检测顾客名称和编码是否一致
        DataTable checkcuscodeandcusname = new OrderManager().DL_CheckCuscodeAndCusnameBySel(Session["ModifyKPDWcCusCode"].ToString(), TxtCustomer.Text);
        if (checkcuscodeandcusname.Rows.Count < 1)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('开票单位名称与编码不符合，请重新选择该开票单位！');</script>");
            return;
        }
        //return;

        #region 使用SqlBulkCopy批量更新
        ////创建datatable数据;
        //DataTable griddata = new DataTable();
        //griddata = (DataTable)Session["ModifyOrderGrid"];
        ////插入数据
        //SqlTransaction tran = null;//声明一个事务对象  
        //try
        //{
        //    using (SqlConnection conn = new SqlConnection("server=.;uid=sa;pwd=sa;database=Test;"))
        //    {
        //        conn.Open();//打开链接  
        //        using (tran = conn.BeginTransaction())
        //        {
        //            using (SqlBulkCopy copy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, tran))
        //            {
        //                copy.DestinationTableName = "AnDequan.dbo.[User]";  //指定服务器上目标表的名称  
        //                copy.WriteToServer(griddata);                      //执行把DataTable中的数据写入DB  
        //                tran.Commit();                                      //提交事务  
        //                //return true;                                        //返回True 执行成功！  
        //            }
        //        }
        //    }
        //}
        //catch (Exception ex)
        //{
        //    if (null != tran)
        //        tran.Rollback();
        //    //LogHelper.Add(ex);  
        //    //return false;//返回False 执行失败！  
        //}
        #endregion

        #region 更新表头数据
        string cdiscountname = TxtCBLX.Text.ToString(); //2016-04-05,添加字段,(酬宾类型)
        string strBillNo = Session["ModifyTxtOrderBillNo"].ToString();	//获取传递过来的DL网单号
        string lngopUserId = Session["lngopUserId"].ToString(); //用户id
        string datCreateTime = TxtBillDate.Text.ToString(); //创建日期
        int bytStatus = 1;  //单据状态
        string ccuscode = Session["ModifyKPDWcCusCode"].ToString();   //开票单位编码
        string cdefine1 = Session["Modifycdefine1"].ToString();   //自定义项1,司机姓名
        string cdefine2 = Session["Modifycdefine2"].ToString();   //自定义项2,司机身份证
        string cdefine3 = Session["ModifyTxtcdefine3"].ToString();  //自定义项3,汽车类型
        string cdefine9 = Session["Modifycdefine9"].ToString();   //自定义项9,收货人姓名,
        string cdefine10 = Session["Modifycdefine10"].ToString();  //自定义项10,车牌号
        string cdefine11 = TxtOrderShippingMethod.Text.ToString();   //自定义项11,收货人地址
        string cdefine12 = Session["Modifycdefine12"].ToString();  //自定义项12,收货人电话,
        string cdefine13 = Session["Modifycdefine13"].ToString();   //自定义项8,司机电话
        string dpredatebt = TxtBillDate.Text.ToString();    //预发货日期 
        string dpremodatebt = TxtBillDate.Text.ToString();  //预完工日期 
        string ccusname = TxtCustomer.Text; //客户名称             
        string strRemarks = TxtOrderMark.Text.ToString();   //备注 
        string cpersoncode = TxtSalesman.Text.ToString();   //业务员编码
        string cSCCode = "00";    //发运方式编码
        if (Session["ModifyTxtcSCCode"].ToString() == "配送")     //发运方式编码,00:自提,01:厂车配送
        {
            Session["ModifyTxtcSCCode"] = "01";
            cSCCode = "01";
        }
        else
        {
            Session["ModifyTxtcSCCode"] = "00";
        }
        //11-21新增字段赋值
        string strLoadingWays = TxtLoadingWays.Text.ToString();     //装车方式
        string cSTCode = Session["ModifyTxtcSTCode"].ToString();     //销售类型编码
        string datDeliveryDate = ""; //交货日期
        if (DeliveryDate.Value != null)
        {
            datDeliveryDate = DeliveryDate.Value.ToString();
        }
        else
        {
            if (HttpContext.Current.Request.Cookies["ModifyDeliveryDate"] != null)
            {
                datDeliveryDate = Server.UrlDecode(HttpContext.Current.Request.Cookies["ModifyDeliveryDate"].Value);
            }
            else
            {
                datDeliveryDate = Session["ModifyDeliveryDate"].ToString();
            }
        }


        string lngopUseraddressId = Session["ModifylngopUseraddressId"].ToString();
        //更新表头数据,DL表中

        OrderInfo oi = new OrderInfo(strBillNo, lngopUserId, datCreateTime, bytStatus, strRemarks, ccuscode, cdefine1, cdefine2, cdefine3, cdefine9, cdefine10, cdefine11, cdefine12, cdefine13, ccusname, cpersoncode, cSCCode, datDeliveryDate, strLoadingWays, lngopUseraddressId, cdiscountname);
        DataTable lngopOrderIdDt = new DataTable();
        lngopOrderIdDt = new OrderManager().DLproc_NewOrderByUpd(oi);
        #endregion

        #region 更新表体数据
        #region 先删除原表体数据后,遍历并且插入新表体数据
        //////创建datatable数据;
        //DataTable griddata = new DataTable();
        //griddata = (DataTable)Session["ModifyOrderGrid"];
        int lngopOrderId = Convert.ToInt32(lngopOrderIdDt.Rows[0]["lngopOrderId"].ToString());   //订单id,从表头中获取:表头插入后,返回表头id
        // string strBillNo = lngopOrderIdDt.Rows[0]["strBillNo"].ToString();   //订单编号,插入表头数据时自动生成,用于反馈给用户
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
        double itaxrate = 17;    //税率 
        string cdefine22 = "";  //表体自定义项22,包装量
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
        string cDefine24 = "";          //活动类型
        //插入数据前,先清除以前的老数据*********************
        bool x = new OrderManager().DL_NewOrderDetailByDel(lngopOrderId);
        //循环插入表体数据
        for (int i = 0; i < griddata.Rows.Count; i++)
        {
            //string strName = griddata.Rows[i]["字段名"].ToString();
            DataRow dr = griddata.Rows[i];  //定义当前行数据
            //赋值
            irowno = i + 1;             //行号
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
            cn1cComUnitName = dr["cn1cComUnitName"].ToString();    //销售单位名称
            cDefine24 = dr["cDefine24"].ToString();     //活动类型
            //计算销售单位的换算率
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
            cdefine22 = dr[15].ToString();  //表体自定义项22,包装量

            cunitid = dr[19].ToString();    //基本计量单位编码

            //插入表体数据
            OrderInfo oiEntry = new OrderInfo(lngopOrderId, cinvcode, iquantity, inum, iquotedprice, iunitprice, itaxunitprice, imoney, itax, isum, inatunitprice, inatmoney, inattax, inatsum, kl, itaxrate, cdefine22, iinvexchrate, cunitid, irowno, cinvname, idiscount, inatdiscount, cComUnitName, cInvDefine1, cInvDefine2, cInvDefine13, cInvDefine14, unitGroup, cComUnitQTY, cInvDefine1QTY, cInvDefine2QTY, cn1cComUnitName, cDefine24);
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

        string welcome = "0;" + ccuscode + ";" + ccusname + ";" + strBillNo + ";修改普通订单"; //根据Dl_opOrderBillNoSetting表中定义IMType类型
        data = Encoding.UTF8.GetBytes(welcome);
        server.SendTo(data, data.Length, SocketFlags.None, ipep);
        #endregion

        #region 保存后清空grid数据和cookie,并提示
        //清空数据
        if (Session["ModifyOrderGrid"] != null)
        {
            Session.Contents.Remove("ModifyOrderGrid");
        }
        //清除送货地址信息
        if (Session["ModifyTxtOrderShippingMethod"] != null)
        {
            Session.Contents.Remove("ModifyTxtOrderShippingMethod");
        }
        //清除的开票单位信息
        if (Session["ModifyTxtCustomer"] != null)
        {
            Session.Contents.Remove("ModifyTxtCustomer");
        }
        //清除的业务员信息
        if (Session["ModifycCusPPerson"] != null)
        {
            Session.Contents.Remove("ModifycCusPPerson");
        }
        //清除开票单位编码信息
        if (Session["ModifycCusPPerson"] != null)
        {
            Session.Contents.Remove("ModifyKPDWcCusCode");
        }
        //清除发运方式编码
        if (Session["ModifycCusPPerson"] != null)
        {
            Session.Contents.Remove("ModifyTxtcSCCode");
        }
        //清除备注 
        if (Session["ModifyTxtOrderMark"] != null)
        {
            Session.Contents.Remove("ModifyTxtOrderMark");
        }
        //清除编号 
        if (Session["ModifyTxtOrderBillNo"] != null)
        {
            Session.Contents.Remove("ModifyTxtOrderBillNo");
        }
        //清除PendingOrder中的Session["OrderFrameModifyURL"]
        if (Session["OrderFrameModifyURL"] != null)
        {
            Session.Contents.Remove("OrderFrameModifyURL");
        }
        //获取客户端的Cookie对象,备注
        HttpCookie ModifyTxtOrderMark = Request.Cookies["ModifyTxtOrderMark"];
        if (ModifyTxtOrderMark != null)
        {
            TimeSpan ts = new TimeSpan(-1, 0, 0, 0);
            ModifyTxtOrderMark.Expires = DateTime.Now.Add(ts);//删除整个Cookie，只要把过期时间设置为现在
            Response.AppendCookie(ModifyTxtOrderMark);
        }

        //获取客户端的Cookie对象,装车方式
        HttpCookie ModifyTxtLoadingWays = Request.Cookies["ModifyTxtLoadingWays"];
        if (ModifyTxtLoadingWays != null)
        {
            TimeSpan ts = new TimeSpan(-1, 0, 0, 0);
            ModifyTxtLoadingWays.Expires = DateTime.Now.Add(ts);//删除整个Cookie，只要把过期时间设置为现在
            Response.AppendCookie(ModifyTxtLoadingWays);
        }

        //获取客户端的Cookie对象,交货日期
        HttpCookie ModifyDeliveryDate = Request.Cookies["ModifyDeliveryDate"];
        if (ModifyDeliveryDate != null)
        {
            TimeSpan ts = new TimeSpan(-1, 0, 0, 0);
            ModifyDeliveryDate.Expires = DateTime.Now.Add(ts);//删除整个Cookie，只要把过期时间设置为现在
            Response.AppendCookie(ModifyDeliveryDate);
        }

        Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script>alert('订单" + strBillNo + " 已经修改成功,请在 待审核订单 中查询订单处理进度！');{parent.location.href='PendingOrder.aspx'}</script>");
        #endregion
    }

    protected void OrderGrid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)    //表体表格数据更新
    {
        //int index = OrderGrid.DataKeys[e.NewEditIndex].Value;//获取主键的值
        //取值 用e.NewValues[索引]
        //string lngopUseraddressId = Convert.ToString(e.Keys[0]);

        ////创建datatable数据;
        DataTable griddata = new DataTable();
        griddata = (DataTable)Session["ModifyOrderGrid"];
        //序号,行号,当前操作行,
        //int i = Convert.ToInt32(e.NewValues["irowno"].ToString());
        int i = Convert.ToInt32(e.NewValues["hh"].ToString());
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
        Double amount = cComUnitQTY + cInvDefine2QTY * sRate + cInvDefine1QTY * bRate;
        //总金额
        Double cComUnitAmount = amount * cComUnitPrice;
        //包装结果
        //string pack = Math.Floor((amount * 10 * 10) / (bRate * 10 * 10)) + dr[4].ToString() + Math.Floor(((amount * 10 * 10) % (bRate * 10 * 10)) / (sRate * 10 * 10)) + dr[5].ToString() + Math.Floor(((amount * 10 * 10) % (sRate * 10 * 10)))/100 + dr[3].ToString(); //包装量换算结果  15
        string pack = Math.Floor((amount * 10 * 10) / (bRate * 10 * 10)) + dr[4].ToString() + Math.Floor(((amount * 10 * 10) % (bRate * 10 * 10)) / (sRate * 10 * 10)) + dr[5].ToString() + Math.Floor(((amount * 10 * 10) % (sRate * 10 * 10))) / 100 + dr[3].ToString(); //包装量换算结果  15
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
        Session["ModifyOrderGrid"] = griddata;

    }

    protected void OrderGrid_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)    //保存前,验证grid表体数据有效性
    {
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
                e.Errors.Add(OrderGrid.Columns["cComUnitQTY"], "请输入" + e.NewValues["cInvDefine14"].ToString() + "的倍数!");
                e.RowError = "请输入" + e.NewValues["cInvDefine14"].ToString() + "的倍数!";
                //throw new Exception("请输入" + e.NewValues["cInvDefine14"].ToString() + "的倍数!");
                OrderGrid.JSProperties.Remove("cpAlertMsg");//先清空
                OrderGrid.JSProperties["cpAlertMsg"] = "保存失败!";
                return;
            }
        }
        //检测2,库存可用量是否正确
        if (TxtcSTCode.Text != "样品资料")
        {
            if (Convert.ToDouble(e.NewValues["cComUnitQTY"].ToString()) + Convert.ToDouble(e.NewValues["cInvDefine1QTY"].ToString()) * Convert.ToDouble(e.NewValues["cInvDefine13"].ToString()) + Convert.ToDouble(e.NewValues["cInvDefine2QTY"].ToString()) * Convert.ToDouble(e.NewValues["cInvDefine14"].ToString()) > Convert.ToDouble(e.NewValues["Stock"].ToString()))
            {
                e.Errors.Add(OrderGrid.Columns["cInvDefineQTY"], "库存不足!");
                e.RowError = "库存不足!";
                //throw new Exception("库存不足!");
                OrderGrid.JSProperties.Remove("cpAlertMsg");//先清空
                OrderGrid.JSProperties["cpAlertMsg"] = "保存失败!";
                return;
            }
        }
        //检测3,检测是否存在数量为0的存货信息
        //string strSplit = Regex.Replace(e.NewValues["cInvDefineQTY"].ToString(), "[0-9]", "", RegexOptions.IgnoreCase);
        if (0 == Convert.ToDouble(e.NewValues["cComUnitQTY"].ToString()) + Convert.ToDouble(e.NewValues["cInvDefine1QTY"].ToString()) + Convert.ToDouble(e.NewValues["cInvDefine2QTY"].ToString()))
        {
            e.Errors.Add(OrderGrid.Columns["cInvDefineQTY"], "存在数量为0的商品,请检查!");
            e.RowError = "存在数量为0的商品,请检查!";
            //throw new Exception("存在数量为0的商品,请检查!");
            OrderGrid.JSProperties.Remove("cpAlertMsg");//先清空
            OrderGrid.JSProperties["cpAlertMsg"] = "保存失败!";
            return;
        }

    }

    protected void Btncdefine3Ok_Click(object sender, EventArgs e)      //车型信息选择事件
    {
        //车型名称
        string cdefine3 = cdefine3Grid.GetRowValues(cdefine3Grid.FocusedRowIndex, "cValue").ToString();
        Txtcdefine3.Text = cdefine3.ToString();
        Session["ModifyTxtcdefine3"] = cdefine3.ToString();
    }

    protected void BtnInvOK_Click(object sender, EventArgs e)   //选择商品后,将商品传递到grid中
    {
        DataTable dtst = (DataTable)Session["gridselectordermodify"];  //获取选中行的值,保存

        if (TreeDetail.Selection.Count > 0)
        {
            for (int i = 0; i < TreeDetail.Selection.Count; i++)
            {
                dtst.Rows.Add(new object[] { TreeDetail.GetSelectedFieldValues("cInvCode")[i].ToString() });
            }
            Session["gridselectordermodify"] = dtst;
        }

        DataTable YOrderGrid = (DataTable)Session["ModifyOrderGrid"];

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
                //DataTable iddt = new OrderManager().DLproc_QuasiOrderDetailBySel(dtst.Rows[i][0].ToString(), Session["KPDWcCusCode"].ToString());
                DataTable iddt = new OrderManager().DLproc_QuasiOrderDetailModifyBySel(dtst.Rows[i][0].ToString(), Session["KPDWcCusCode"].ToString(), Session["ModifyTxtOrderBillNo"].ToString());
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
        Session["ModifyOrderGrid"] = YOrderGrid;
        OrderGrid.DataSource = YOrderGrid;
        OrderGrid.DataBind();

        //3.清除选择
        TreeDetail.Selection.UnselectAll(); //清除所有选择项
        //清除session数据
        Session.Remove("gridselectordermodify");
    }

    /*ASPxTreeList的FocuseNodeChnaged事件来处理选择Node时的逻辑,需要引用using DevExpress.Web.ASPxTreeList;*/
    protected void treeList_CustomDataCallback(object sender, TreeListCustomDataCallbackEventArgs e)
    {
        DataTable dtst = (DataTable)Session["gridselectordermodify"];  //获取选中行的值,保存
        if (TreeDetail.Selection.Count > 0)
        {
            for (int i = 0; i < TreeDetail.Selection.Count; i++)
            {
                dtst.Rows.Add(new object[] { TreeDetail.GetSelectedFieldValues("cInvCode")[i].ToString() });
            }
            Session["gridselectordermodify"] = dtst;
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
            Session["ordertreelistgridmodify"] = KeyFieldName.ToString();//赋值给grid查询
            return KeyFieldName.Trim();
            //查询并绑定gridview

        }
        return string.Empty;
    }



    protected void btn_Click(object sender, EventArgs e)
    {
        //DataTable dtst = (DataTable)Session["gridselectordermodify"];
        //for (int i = 0; i < TreeDetail.Selection.Count; i++)
        //{
        //    dtst.Rows.Add(new object[] { TreeDetail.GetSelectedFieldValues("cInvCode")[i].ToString() });
        //}
        //Session["gridselectordermodify"] = dtst;
        //绑定gridview数据源
        if (Session["ordertreelistgridmodify"] != null)
        {
            DataTable dt1 = new SearchManager().DLproc_TreeListDetailsAllBySel(Session["ordertreelistgridmodify"].ToString(), Session["KPDWcCusCode"].ToString(), 1);
            TreeDetail.DataSource = dt1;
            TreeDetail.DataBind();
            //删除当前绑定的数据,因为切换分类,所以需要删除
            DataTable dtst = (DataTable)Session["gridselectordermodify"];
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
        }

    }


    protected void BtnInv_Reset_Click(object sender, EventArgs e)
    {
        TreeDetail.Selection.UnselectAll(); //清除所有选择项
        //清除session数据
        Session.Remove("gridselectordermodify");
    }

    protected void OrderGrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)    //删除表体数据
    {
        string cInvCode = e.Values["cInvCode"].ToString();
        DataTable dtt = (DataTable)Session["ModifyOrderGrid"];
        for (int i = 0; i < dtt.Rows.Count; i++)
        {
            if (cInvCode == dtt.Rows[i][0].ToString())
            {
                dtt.Rows[i].Delete();
                dtt.AcceptChanges();
                break;
            }
        }
        Session["ModifyOrderGrid"] = dtt;
        OrderGrid.DataSource = dtt;
        OrderGrid.DataBind();
        e.Cancel = true;
    }



    protected void OrderGrid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)  //增加序号
    {
        //if (e.VisibleIndex >= 0 && e.DataColumn.Caption == "序号")
        //{
        //    e.Cell.Text = (e.VisibleIndex + 1).ToString();
        //}
    }
    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        //清除session
        //Session.Remove("SampleOrderModify_StrBillNo");  //单据编号
        //Session.Remove("SampleOrderModify_ordergrid");  //表体明细数据
        //Session.Remove("SampleOrderModify_gridselect"); //选择的商品明细数据
        //Session.Remove("SampleOrderModify_KPDWcode");   //开票单位编码
        //Session.Remove("SampleOrderModify_treelistgrid");   //选择树的大类编码
        Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script>{window.parent.location='PendingOrder.aspx'}</script>");
    }
    protected void BtnInvalidOrder_Click(object sender, EventArgs e)
    {
        string strBillNo = Session["ModifyTxtOrderBillNo"].ToString();
        bool c = new OrderManager().DL_InvalidOrderByUpd(strBillNo, Session["lngopUserId"].ToString());
        if (c)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script>alert('订单" + strBillNo + " 已经作废！');{window.parent.location='PendingOrder.aspx'}</script>");
        }
        else
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script>alert('订单作废失败,请联系管理员！')");
        }
    }
    protected void OrderGrid_Init(object sender, EventArgs e)
    {
        //初始化,设置金额合计启用报价合计还是执行价合计?
        Hashtable ht = (Hashtable)Session["SysSetting"];
        if (ht["IsExercisePrice"].ToString() == "0")   //报价金额
        {
            OrderGrid.TotalSummary[1].FieldName = "cComUnitAmount";
        }
        else//执行价金额
        {
            OrderGrid.TotalSummary[1].FieldName = "xx";
        }
    }
}