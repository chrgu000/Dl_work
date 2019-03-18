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

public partial class Order_X : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //开启时间管理
        DataTable timecontrol = new OrderManager().DL_OrderENTimeControlBySel();
        if (timecontrol.Rows.Count < 1)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('" + ConfigurationManager.AppSettings["datOrderTime"].ToString() + "');window.parent.location.href='AllBlank.aspx';</script>");
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

        #region 判断session是否存在,并且建立datatable,用于记录选择项目,Order_Y_gridselect
        if (Session["Order_Y_gridselect"] == null)
        {
            DataTable dts = new DataTable();
            dts.Columns.Add("cInvCode"); //编码    0   
            dts.Columns.Add("cpreordercode"); //预订单号    1  
            dts.Columns.Add("itemid"); //编码+预订单号    2
            dts.Columns.Add("realqty"); //可用量    3  
            dts.Columns.Add("iaoids"); //预订单id    3  
            //dt.Rows.Add(new object[] { "0"});
            Session["Order_Y_gridselect"] = dts;
        }
        #endregion
        //绑定treelist数据源
        DataTable dtlist = new SearchManager().DL_PreOrderTreeBySel(Session["KPDWcCusCode"].ToString(), Session["lngopUserId"].ToString(), Session["lngopUserExId"].ToString(), 2);
        treeList.KeyFieldName = "strBillNo";
        treeList.ParentFieldName = "strBillNo";
        treeList.DataSource = dtlist;
        /*展开第一级node*/
        treeList.DataBind();
        treeList.ExpandToLevel(1);
        //绑定gridview数据源
        if (Session["Ytreelistgrid"] != null)
        {
            //DataTable dt1 = new SearchManager().DLproc_TreeListPreDetailsBySel(Session["Ytreelistgrid"].ToString(), 2);
            DataTable dt1 = new SearchManager().DLproc_TreeListPreDetails_TSBySel(Session["Ytreelistgrid"].ToString(), 2);
            TreeDetail.DataSource = dt1;
            TreeDetail.DataBind();

            //删除当前绑定的数据,因为切换订单号,所以需要删除
            DataTable dtst = (DataTable)Session["Order_Y_gridselect"];
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                for (int j = 0; j < dtst.Rows.Count; j++)
                {
                    if (TreeDetail.GetRowValues(i, "itemid").ToString() == dtst.Rows[j][2].ToString())
                    {
                        TreeDetail.Selection.SelectRow(i);
                        dtst.Rows[j].Delete();
                        dtst.AcceptChanges();
                    }
                }
            }
        }

        //开票单位默认赋值
        if (TxtCustomer.Text == "")
        {
            TxtCustomer.Text = Session["strUserName"].ToString();
            Session["KPDWcCusCode"] = Session["ConstcCusCode"].ToString();
        }

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
            Session["Ytreelistgrid"] = "";
            //绑定CustomerGrid,顾客开票信息iaoids
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
            //表头字段赋值,顾客信用额度
            DataTable CusCreditDt = new DataTable();
            CusCreditDt = new OrderManager().DLproc_getCusCreditInfo(Session["KPDWcCusCode"].ToString());//默认登录客户编码
            TxtCusCredit.Text = CusCreditDt.Rows[0]["iCusCreLine"].ToString();
        }
        TxtBiller.Text = Session["strUserName"].ToString(); //绑定制单人
        TxtBillDate.Text = System.DateTime.Now.ToString("d");   //绑定制单日期

        #region 判断session是否存在,并且建立datatable
        if (Session["Yordergrid"] == null)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("cinvcode"); //编码    0
            dt.Columns.Add("cinvname"); //名称    1
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
            dt.Columns.Add("cComUnitAmount"); //基本单位金额  14
            dt.Columns.Add("pack"); //包装量换算结果  15
            dt.Columns.Add("ExercisePrice"); //基本单位单价(执行价格)   16
            dt.Columns.Add("fAvailQtty"); //可用库存量   17
            dt.Columns.Add("kl"); //扣率   18
            dt.Columns.Add("cComUnitCode"); //基本单位编码   19
            dt.Columns.Add("iTaxRate"); //销项税率   20
            dt.Columns.Add("cn1cComUnitName"); //销售单位名称   21
            dt.Columns.Add("ccode"); //预订单号   22
            dt.Columns.Add("itemid"); //编码+预订单号    23
            dt.Columns.Add("realqty"); //可用量    24
            dt.Columns.Add("iaoids"); //预订单autoid     25
            //dt.Rows.Add(new object[] { "dasdsad", "张1", "98", "94","","","","","" });
            //dt.Rows.Add(new object[] { "fdsfdfdsf", "张2", "99", "94","","","","","" });
            Session["Yordergrid"] = dt;
        }
        #endregion

        if (Request.QueryString["id"] == null)
        {
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
                //绑定车型信息grid表
                DataTable cdefine3Griddt = new BasicInfoManager().DL_cdefine3BySel();
                cdefine3Grid.DataSource = cdefine3Griddt;
                cdefine3Grid.DataBind();

                return;
            }
            else
            {
                //删除选择项后绑定数据
                //griddt = (DataTable)Session["Yordergrid"];
                //for (int i = 0; i < griddt.Rows.Count; i++)
                //{
                //    if (griddt.Rows[i][0].ToString() == Request.QueryString["code"].ToString())
                //    {
                //        griddt.Rows[i].Delete();
                //        griddt.AcceptChanges();
                //        OrderGrid.DataSource = griddt;
                //        OrderGrid.DataBind();
                //        return;
                //    }
                //}
            }
        }

        //string id = Request.QueryString["id"].ToString();
        ////获取物料详细信息,单位,价格等
        //DataTable iddt = new OrderManager().DLproc_QuasiOrderDetailBySel(id, Session["cCusCode"].ToString());
        //获取传递过来的参数,并存入数组
        //string[] array = id.Split('|');   //10-25取消传参,改为下面的查询后在绑定

        //检测是否重复添加物料
        //griddt = (DataTable)Session["Yordergrid"];
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
        //将传递过来的数据放入datatable中,并且绑定gridview
        //griddt.Rows.Add(new object[] { array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7], array[8], "0", "0", "0", "0", "88", "9" });
        //string[] array = { "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" };
        //array[0] = iddt.Rows[0][3].ToString();//存货编码
        //array[1] = iddt.Rows[0][0].ToString();//存货名称
        //array[2] = iddt.Rows[0][1].ToString();//存货规格
        //array[3] = iddt.Rows[0][2].ToString();//基本单位
        //array[17] = iddt.Rows[0][9].ToString(); //库存可用量
        //array[18] = iddt.Rows[0][8].ToString(); //扣率
        //array[19] = iddt.Rows[0][11].ToString(); //基本单位编码
        //array[20] = iddt.Rows[0][12].ToString(); //销项税率
        //array[21] = iddt.Rows[0][13].ToString(); //销售单位名称
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
        //Session["Yordergrid"] = griddt;
        //OrderGrid.DataSource = griddt;
        //OrderGrid.DataBind();
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

    protected void BtnCustomerOk_Click(object sender, EventArgs e)      //客户信息选择事件
    {
        //开票单位名称
        string cCusName = CustomerGrid.GetRowValues(CustomerGrid.FocusedRowIndex, "cCusName").ToString();
        TxtCustomer.Text = cCusName.ToString();
        Session["TxtCustomer"] = TxtCustomer.Text.ToString();
        //开票单位编码
        Session["KPDWcCusCode"] = CustomerGrid.GetRowValues(CustomerGrid.FocusedRowIndex, "cCusCode").ToString();
        //业务员
        TxtSalesman.Text = CustomerGrid.GetRowValues(CustomerGrid.FocusedRowIndex, "cCusPPerson").ToString();
        Session["cCusPPerson"] = TxtSalesman.Text.ToString();
        /*20151211,重新选择开票单位后,清除表体信息,并重新绑定*/
        DataTable griddata = new DataTable();
        griddata = (DataTable)Session["Yordergrid"];
        griddata.Rows.Clear();
        OrderGrid.DataSource = griddata;
        OrderGrid.DataBind();
        Session["Yordergrid"] = griddata;
        Session["Ytreelistgrid"] = null;
        TreeDetail.DataSource = null;
        TreeDetail.DataBind();

        ////重新选择开票单位后,重新读取开票单位的存货单价,并且计算金额,重新绑定表体grid,11-01
        //DataTable griddata = new DataTable();
        //griddata = (DataTable)Session["Yordergrid"];
        ////如果不为空,则遍历,给价格重新赋值
        //if (griddata != null)
        //{
        //    if (griddata.Rows.Count > 0)
        //    {
        //        for (int i = 0; i < griddata.Rows.Count; i++)
        //        {
        //            DataTable iddt = new OrderManager().DLproc_QuasiOrderDetailBySel(griddata.Rows[i]["cInvCode"].ToString(), Session["KPDWcCusCode"].ToString());
        //            griddata.Rows[i]["cComUnitPrice"] = iddt.Rows[0]["Quote"].ToString();//报价
        //            griddata.Rows[i]["ExercisePrice"] = iddt.Rows[0]["ExercisePrice"].ToString();//执行价格
        //            griddata.Rows[i]["cComUnitAmount"] = Convert.ToDecimal(iddt.Rows[0]["Quote"].ToString()) * Convert.ToDecimal(griddata.Rows[i]["cInvDefineQTY"].ToString());//报价金额
        //            griddata.Rows[i]["Stock"] = Convert.ToDouble(iddt.Rows[0]["fAvailQtty"].ToString());//可用库存量
        //            /*11-10添加*/
        //            griddata.Rows[i]["kl"] = Convert.ToDouble(iddt.Rows[0]["Rate"].ToString()); //扣率   18 Rate
        //            griddata.Rows[i]["iTaxRate"] = Convert.ToDouble(iddt.Rows[0]["iTaxRate"].ToString()); //销项税率   20 iTaxRate
        //        }
        //        //重新绑定
        //        OrderGrid.DataSource = griddata;
        //        OrderGrid.DataBind();
        //        Session["Yordergrid"] = griddata;
        //    }
        //}
        //重新获取信用额度
        DataTable CusCreditDt = new DataTable();
        CusCreditDt = new OrderManager().DLproc_getCusCreditInfo(Session["KPDWcCusCode"].ToString());//默认登录客户编码
        TxtCusCredit.Text = CusCreditDt.Rows[0]["iCusCreLine"].ToString();



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

    protected void BtnSaveOrder_Click(object sender, EventArgs e)   //保存并提交订单按钮事件
    {

        ////创建datatable数据;
        DataTable griddata = new DataTable();
        griddata = (DataTable)Session["Yordergrid"];

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
        if (TxtOrderShippingMethod.Text.Substring(0, 2).ToString() == "自提" && string.IsNullOrEmpty(Txtztadd.Text))
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('自提必须选择行政区！');</script>");
            return;
        }
        if (TxtcSCCode.Text == "")
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('请选择发运方式！');</script>");
            return;
        }
        if (Txtcdefine3.Text == "")
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('请选择车型！');</script>");
            return;
        }
        if (TxtSalesman.Text == "")
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('请填写业务员！');</script>");
            return;
        }
        //if (DeliveryDate.Text == "")
        //{
        //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('请选择交货日期！');</script>");
        //    return;
        //}
        //2,检测是否有数据
        if (TxtcSTCode.Text != "样品资料")
        {
            if (Convert.ToDateTime(DeliveryDate.Value) <= DateTime.Now && DeliveryDate.Value != null) //检验交货日期
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('交货日期不能小于当前时间！');</script>");
                return;
            }
        }
        if (griddata.Rows.Count <= 0)   //检查是否有商品明细
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('请先添加商品到订单明细表,保存后再提交订单！');</script>");
            return;
        }
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
        //信用额度,20151129变更,增加临时授权,Customer中的cCusPostCode ,1为临时授权,授权后设置为 null(恢复操作在存储过程中完成)
        DataTable ExtraCredit = new DataTable();
        ExtraCredit = new OrderManager().DL_ExtraCreditBySel(Session["KPDWcCusCode"].ToString());
        if (ExtraCredit.Rows.Count > 0)
        {

        }
        else
        {
            //DataTable CusCreditDt = new OrderManager().DLproc_getCusCreditInfo(Session["KPDWcCusCode"].ToString());//开票单位编码
            ////TxtCusCredit.Text = CusCreditDt.Rows[0]["iCusCreLine"].ToString();//信用额
            //if (CusCredit > Convert.ToDouble(CusCreditDt.Rows[0]["iCusCreLine"].ToString()) && Convert.ToDouble(CusCreditDt.Rows[0]["iCusCreLine"].ToString()) != -99999999)
            ////if (CusCredit > Convert.ToDouble(TxtCusCredit.Text) && Convert.ToDouble(TxtCusCredit.Text) != -99999999)
            //{
            //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('已经超过您开票单位的信用额度:" + Convert.ToString((Convert.ToDouble(CusCreditDt.Rows[0]["iCusCreLine"].ToString()) - CusCredit)) + "！');</script>");
            //    return;
            //}
        }

        //4,检测可用库存量是否正确,先更新库存,再调用OrderGrid_RowValidating(暂时无法调用,遍历解决)
        DataTable stockdt = new DataTable();
        bool d = false;
        bool ee = false;
        for (int i = 0; i < griddata.Rows.Count; i++)
        {
            //stockdt = new OrderManager().DLproc_QuasiOrderDetail_TSBySel(griddata.Rows[i]["cInvCode"].ToString(), Session["cCusCode"].ToString());
            stockdt = new OrderManager().DLproc_QuasiYOrderDetail_TSBySel(griddata.Rows[i]["cInvCode"].ToString(), griddata.Rows[i]["ccode"].ToString(),"0");
            griddata.Rows[i]["fAvailQtty"] = stockdt.Rows[0]["fAvailQtty"].ToString(); //库存可用量
            griddata.Rows[i]["realqty"] = stockdt.Rows[0]["realqty"].ToString(); //订单可用量
            if (Convert.ToDouble(griddata.Rows[i]["fAvailQtty"].ToString()) < Convert.ToDouble(griddata.Rows[i]["cInvDefineQTY"].ToString()))
            {
                d = true;
            }
            if (Convert.ToDouble(griddata.Rows[i]["realqty"].ToString()) < Convert.ToDouble(griddata.Rows[i]["cInvDefineQTY"].ToString()))
            {
                ee = true;
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
        if (ee)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('超过订单可用量！订单可用量已更新,请重新调整订单商品数量！');</script>");
            return;
        }
        #endregion

        //检测
        //return;

        #region 插入表头数据
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
        if (Session["TxtcSCCode"].ToString() == "配送")     //发运方式编码,00:自提,01:厂车配送
        {
            Session["TxtcSCCode"] = "01";
            cSCCode = "01";
        }
        else
        {
            Session["TxtcSCCode"] = "00";
        }
        string cdefine8 = "";       //自提行政区
        if (TxtOrderShippingMethod.Text.Substring(0, 2).ToString() == "自提")
        {
            cdefine8 = Txtztadd.Text.ToString(); //自提行政区
        }
        else
        {
            cdefine8 = Session["cdefine8"].ToString();//配送行政区
        }
        //11-18新增字段赋值
        string strLoadingWays = TxtLoadingWays.Text.ToString();     //装车方式
        string cSTCode = "00";     //销售类型编码    
        string datDeliveryDate = ""; //交货日期
        string lngopUseraddressId = Session["lngopUseraddressId"].ToString();
        if (DeliveryDate.Value == null)
        {
            datDeliveryDate = DateTime.Now.ToString();
        }
        else
        {
            datDeliveryDate = DeliveryDate.Value.ToString();
        }
        string strTxtRelateU8NO = "";
        int lngBillType = 2;
        string lngopUserExId = Session["lngopUserExId"].ToString();
        string strAllAcount = Session["strAllAcount"].ToString();
        //插入表头数据,DL表中
        OrderInfo oi = new OrderInfo(lngopUserId, datCreateTime, bytStatus, strRemarks, ccuscode, cdefine1, cdefine2, cdefine3, cdefine9, cdefine10, cdefine11, cdefine12, cdefine13, ccusname, cpersoncode, cSCCode, datDeliveryDate, strLoadingWays, cSTCode, lngopUseraddressId, strTxtRelateU8NO, lngBillType, cdefine8, lngopUserExId, strAllAcount);
        DataTable lngopOrderIdDt = new DataTable();
        lngopOrderIdDt = new OrderManager().DLproc_NewYYOrderByIns(oi);
        #endregion

        #region 插入表体数据
        #region 遍历并且插入表体数据
        //////创建datatable数据;
        //DataTable griddata = new DataTable();
        //griddata = (DataTable)Session["Yordergrid"];
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
        string cpreordercode = "";          //预订单号
        string autoid = "";      //预订单autoid 
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
            cpreordercode = dr["ccode"].ToString();    //销售预订单号
            autoid = dr["iaoids"].ToString();    //预订单号id
            //插入表体数据
            OrderInfo oiEntry = new OrderInfo(lngopOrderId, cinvcode, iquantity, inum, iquotedprice, iunitprice, itaxunitprice, imoney, itax, isum, inatunitprice, inatmoney, inattax, inatsum, kl, itaxrate, cDefine22, iinvexchrate, cunitid, irowno, cinvname, idiscount, inatdiscount, cComUnitName, cInvDefine1, cInvDefine2, cInvDefine13, cInvDefine14, unitGroup, cComUnitQTY, cInvDefine1QTY, cInvDefine2QTY, cn1cComUnitName, cpreordercode, autoid);
            bool c = new OrderManager().DLproc_NewYYOrderDetailByIns(oiEntry);
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

        string welcome = "3;" + ccuscode + ";" + ccusname + ";" + strBillNo + ";新增参照特殊订单";    //根据Dl_opOrderBillNoSetting表中定义IMType类型
        data = Encoding.UTF8.GetBytes(welcome);
        server.SendTo(data, data.Length, SocketFlags.None, ipep);
        #endregion

        #region 保存后清空grid数据,并提示
        //清空数据
        if (Session["Yordergrid"] != null)
        {
            Session.Contents.Remove("Yordergrid");
            Session.Contents.Remove("Order_Y_gridselect");
        }
        Session.Contents.Remove("TxtOrderShippingMethod");
        Session.Contents.Remove("TxtcSCCode");
        TxtOrderShippingMethod.Text = "";
        TxtcSCCode.Text = "";
        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单 " + strBillNo + " 已经提交,请在已提交订单中查询订单处理进度!');</script>");
        Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script>alert('订单" + strBillNo + " 已经提交,请在已提交订单中查询订单处理进度！');{location.href='Order_X.aspx'}</script>");
        #endregion
    }

    protected void OrderGrid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)    //表体表格数据更新
    {
        //int index = OrderGrid.DataKeys[e.NewEditIndex].Value;//获取主键的值
        //取值 用e.NewValues[索引]
        //string lngopUseraddressId = Convert.ToString(e.Keys[0]);

        ////创建datatable数据;
        DataTable griddata = new DataTable();
        griddata = (DataTable)Session["Yordergrid"];
        //序号,行号,当前操作行,
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
        Double amount = Convert.ToDouble((cComUnitQTY + cInvDefine2QTY * sRate + cInvDefine1QTY * bRate).ToString("N2"));
        //总金额
        Double cComUnitAmount = amount * cComUnitPrice;
        //包装结果
        //string pack = Math.Floor(amount / bRate) + dr[4].ToString() + Math.Floor((amount % bRate) / sRate) + dr[5].ToString() + Math.Floor((((amount * 10 * 10) % (sRate * 10 * 10)) / 10) / 10) + dr[3].ToString(); //包装量换算结果  15
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
        Session["Yordergrid"] = griddata;

    }

    protected void OrderGrid_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)    //保存前,验证grid表体数据有效性
    {
        OrderGrid.JSProperties.Remove("cpAlertMsg");//先清空
        OrderGrid.JSProperties["cpAlertMsg"] = "";
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

        //检测2,库存余量是否正确,先检测是否超过可用两
        if (Convert.ToDouble(e.NewValues["cComUnitQTY"].ToString()) + Convert.ToDouble(e.NewValues["cInvDefine1QTY"].ToString()) * Convert.ToDouble(e.NewValues["cInvDefine13"].ToString()) + Convert.ToDouble(e.NewValues["cInvDefine2QTY"].ToString()) * Convert.ToDouble(e.NewValues["cInvDefine14"].ToString()) > Convert.ToDouble(e.NewValues["realqty"].ToString()))
        {
            e.Errors.Add(OrderGrid.Columns["realqty"], "超过可用量!");//超过可用量
            e.RowError = "超过可用量!";
            OrderGrid.JSProperties.Remove("cpAlertMsg");//先清空
            OrderGrid.JSProperties["cpAlertMsg"] = "超过可用量!";
            return;
            //throw new Exception("超过可用量!");
        }
        if (Convert.ToDouble(e.NewValues["cComUnitQTY"].ToString()) + Convert.ToDouble(e.NewValues["cInvDefine1QTY"].ToString()) * Convert.ToDouble(e.NewValues["cInvDefine13"].ToString()) + Convert.ToDouble(e.NewValues["cInvDefine2QTY"].ToString()) * Convert.ToDouble(e.NewValues["cInvDefine14"].ToString()) > Convert.ToDouble(e.NewValues["fAvailQtty"].ToString()))
        {
            e.Errors.Add(OrderGrid.Columns["fAvailQtty"], "库存不足!");//库存不足
            e.RowError = "库存不足!";
            OrderGrid.JSProperties.Remove("cpAlertMsg");//先清空
            OrderGrid.JSProperties["cpAlertMsg"] = "库存不足!";
            return;
            //throw new Exception("库存不足!");
        }

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

    protected void OrderGrid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
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
        }
    }


    /*ASPxTreeList的FocuseNodeChnaged事件来处理选择Node时的逻辑,需要引用using DevExpress.Web.ASPxTreeList;*/
    protected void treeList_CustomDataCallback(object sender, TreeListCustomDataCallbackEventArgs e)
    {
        DataTable dtst = (DataTable)Session["Order_Y_gridselect"];  //获取选中行的值,保存
        if (TreeDetail.Selection.Count > 0)
        {
            for (int i = 0; i < TreeDetail.Selection.Count; i++)
            {
                dtst.Rows.Add(new object[] { TreeDetail.GetSelectedFieldValues("cInvCode")[i].ToString(), TreeDetail.GetSelectedFieldValues("cCode")[i].ToString(), TreeDetail.GetSelectedFieldValues("itemid")[i].ToString(), TreeDetail.GetSelectedFieldValues("realqty")[i].ToString(), TreeDetail.GetSelectedFieldValues("iaoids")[i].ToString() });
            }
            Session["Order_Y_gridselect"] = dtst;
        }

        string key = e.Argument.ToString();
        TreeListNode node = treeList.FindNodeByKeyValue(key);
        e.Result = GetEntryText(node);//获取node的code值
    }

    protected string GetEntryText(TreeListNode node)
    {
        if (node != null)
        {
            string KeyFieldName = node["strBillNo"].ToString();
            Session["Ytreelistgrid"] = KeyFieldName.ToString();//赋值给grid查询           
            return KeyFieldName.Trim();
        }
        return string.Empty;
    }

    protected void treeList_HtmlDataCellPrepared(object sender, TreeListHtmlDataCellEventArgs e)
    {

    }

    protected void btn_Click(object sender, EventArgs e)
    {
        //DataTable dtst = (DataTable)Session["Order_Y_gridselect"];
        //for (int i = 0; i < TreeDetail.Selection.Count; i++)
        //{
        //    dtst.Rows.Add(new object[] { TreeDetail.GetSelectedFieldValues("cInvCode")[i].ToString() });
        //}
        //Session["Order_Y_gridselect"] = dtst;

    }

    protected void TreeDetail_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
    {

        //TreeDetail.Selection.SelectRow(0);
    }

    protected void TreeDetail_SelectionChanged(object sender, EventArgs e)
    {

    }

    protected void BtnInv_Reset_Click(object sender, EventArgs e)
    {
        TreeDetail.Selection.UnselectAll(); //清除所有选择项
        //清除session数据
        Session.Remove("Order_Y_gridselect");
    }

    protected void OrderGrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)    //删除表体数据
    {
        string cInvCode = e.Values["cinvcode"].ToString();
        DataTable dtt = (DataTable)Session["Yordergrid"];
        for (int i = 0; i < dtt.Rows.Count; i++)
        {
            if (cInvCode == dtt.Rows[i][0].ToString())
            {
                dtt.Rows[i].Delete();
                dtt.AcceptChanges();
                break;
            }
        }
        Session["Yordergrid"] = dtt;
        OrderGrid.DataSource = dtt;
        OrderGrid.DataBind();
        e.Cancel = true;
    }

    protected void BtnInvOK_Click(object sender, EventArgs e)   //选择商品后,将商品传递到grid中
    {
        DataTable dtst = (DataTable)Session["Order_Y_gridselect"];  //获取选中行的值,保存
        if (TreeDetail.Selection.Count > 0)
        {
            for (int i = 0; i < TreeDetail.Selection.Count; i++)
            {
                dtst.Rows.Add(new object[] { TreeDetail.GetSelectedFieldValues("cInvCode")[i].ToString(), TreeDetail.GetSelectedFieldValues("cCode")[i].ToString(), TreeDetail.GetSelectedFieldValues("itemid")[i].ToString(), TreeDetail.GetSelectedFieldValues("realqty")[i].ToString(), TreeDetail.GetSelectedFieldValues("iaoids")[i].ToString() });
            }
            Session["Order_Y_gridselect"] = dtst;
        }

        DataTable YOrderGrid = (DataTable)Session["Yordergrid"];
        //1.排除重复的物料
        for (int i = 0; i < OrderGrid.VisibleRowCount; i++)
        {
            for (int j = 0; j < dtst.Rows.Count; j++)
            {
                if (OrderGrid.GetRowValues(i, "itemid").ToString() == dtst.Rows[j]["itemid"].ToString())
                {
                    dtst.Rows[j].Delete();
                    dtst.AcceptChanges();
                    //break;
                    j = 99999;
                }
            }
        }

        //2.将选择的新物料查询出对应的基础数据资料,并且传入YOrder中
        for (int i = 0; i < dtst.Rows.Count; i++)
        {
            DataTable iddt = new OrderManager().DLproc_QuasiYOrderDetail_TSBySel(dtst.Rows[i][0].ToString(), dtst.Rows[i][1].ToString(),"0");
            #region 将传递过来的数据放入datatable中,并且绑定gridview
            //griddt.Rows.Add(new object[] { array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7], array[8], "0", "0", "0", "0", "88", "9" });
            string[] array = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25" };
            array[0] = iddt.Rows[0]["cinvcode"].ToString();//存货编码
            array[1] = iddt.Rows[0]["cinvname"].ToString();//存货名称
            array[2] = iddt.Rows[0]["cInvStd"].ToString();//存货规格
            array[3] = iddt.Rows[0]["cComUnitName"].ToString();//基本单位
            array[4] = iddt.Rows[0]["cInvDefine1"].ToString();//大包装单位
            array[5] = iddt.Rows[0]["cInvDefine2"].ToString();//小包装单位
            array[6] = iddt.Rows[0]["cInvDefine13"].ToString();//大包装换算率
            array[7] = iddt.Rows[0]["cInvDefine14"].ToString();//小包装换算率
            array[8] = iddt.Rows[0]["UnitGroup"].ToString();//单位换算率组
            array[9] = "0";//基本单位数量=可用量
            array[10] = "0";//大包装单位数量
            array[11] = "0";//小包装单位数量
            //array[9] = iddt.Rows[0]["cComUnitQTY"].ToString();//基本单位数量
            //array[9] = dtst.Rows[i]["realqty"].ToString();//基本单位数量=可用量
            //array[10] = iddt.Rows[0]["cInvDefine1QTY"].ToString();//大包装单位数量
            //array[11] = iddt.Rows[0]["cInvDefine2QTY"].ToString();//小包装单位数量
            array[12] = iddt.Rows[0]["iquantity"].ToString();//包装量数量汇总
            array[13] = iddt.Rows[0]["iquotedprice"].ToString();//基本单位单价
            array[14] = iddt.Rows[0]["isum"].ToString();//基本单位金额
            array[15] = iddt.Rows[0]["cDefine22"].ToString();//包装量换算结果
            array[16] = iddt.Rows[0]["itaxunitprice"].ToString();//基本单位单价
            array[17] = iddt.Rows[0]["fAvailQtty"].ToString(); //可用库存量
            array[18] = iddt.Rows[0]["kl"].ToString(); //扣率
            array[19] = iddt.Rows[0]["cComUnitCode"].ToString(); //基本单位编码
            array[20] = iddt.Rows[0]["iTaxRate"].ToString(); //销项税率
            array[21] = iddt.Rows[0]["cn1cComUnitName"].ToString(); //销售单位名称 
            array[22] = iddt.Rows[0]["ccode"].ToString(); //预订单号
            array[23] = iddt.Rows[0]["itemid"].ToString(); //编码+预订单号  
            array[24] = iddt.Rows[0]["realqty"].ToString(); //可用量 20170331改成查询该预订单下所有销售订单的发货单，得出剩余可用数量
            //array[24] = dtst.Rows[i]["realqty"].ToString(); //可用量 
            array[25] = dtst.Rows[i]["iaoids"].ToString(); //预订单id            
            #endregion
            YOrderGrid.Rows.Add(new object[] { array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7], array[8], array[9], array[10], array[11], array[12], array[13], array[14], array[15], array[16], array[17], array[18], array[19], array[20], array[21], array[22], array[23], array[24], array[25] });
        }
        Session["Yordergrid"] = YOrderGrid;
        OrderGrid.DataSource = YOrderGrid;
        OrderGrid.DataBind();

        TreeDetail.Selection.UnselectAll(); //清除所有选择项
        //清除session数据
        Session.Remove("Order_Y_gridselect");
    }

    protected void ASPxMenu1_ItemClick(object source, MenuItemEventArgs e)
    {
        Session.Remove("Ytreelistgrid");
        Session.Remove("Yordergrid");
        Session.Remove("Order_Y_gridselect");
        Session.Remove("TxtRelateU8NO");
        Session.Remove("Sampleordergrid");
        TxtOrderShippingMethod.Text = "";
        TxtcSCCode.Text = "";
        Txtcdefine3.Text = "";

        //清除order.aspx中开票单位编码
        if (Session["KPDWcCusCode"] != null)
        {
            //Session.Contents.Remove("KPDWcCusCode");
            Session["KPDWcCusCode"] = Session["ConstcCusCode"].ToString();
        }
        //清除order.aspx中的送货地址信息
        if (Session["TxtOrderShippingMethod"] != null)
        {
            Session.Contents.Remove("TxtOrderShippingMethod");
        }
        //清除order.aspx中的订单表体信息
        if (Session["ordergrid"] != null)
        {
            Session.Contents.Remove("ordergrid");
        }
        //清除order.aspx中的物料选择
        if (Session["gridselect"] != null)
        {
            Session.Contents.Remove("ordergrid");
        }
        //清除order.aspx中的开票单位信息
        if (Session["TxtCustomer"] != null)
        {
            Session.Contents.Remove("TxtCustomer");
        }

        //清除order.aspx中的发运方式编码
        if (Session["TxtcSCCode"] != null)
        {
            Session.Contents.Remove("TxtcSCCode");
        }
        //清除order.aspx中的车型信息
        if (Session["cdefine3"] != null)
        {
            Session.Contents.Remove("cdefine3");
        }
        //清除order.aspx中的销售类型
        Session["cSTCode"] = 0;

        //获取客户端的Cookie对象,备注
        HttpCookie TxtOrderMark = Request.Cookies["TxtOrderMark"];
        if (TxtOrderMark != null)
        {
            TimeSpan ts = new TimeSpan(-1, 0, 0, 0);
            TxtOrderMark.Expires = DateTime.Now.Add(ts);//删除整个Cookie，只要把过期时间设置为现在
            Response.AppendCookie(TxtOrderMark);
        }

        //获取客户端的Cookie对象,装车方式
        HttpCookie TxtLoadingWays = Request.Cookies["TxtLoadingWays"];
        if (TxtLoadingWays != null)
        {
            TimeSpan ts = new TimeSpan(-1, 0, 0, 0);
            TxtLoadingWays.Expires = DateTime.Now.Add(ts);//删除整个Cookie，只要把过期时间设置为现在
            Response.AppendCookie(TxtLoadingWays);
        }

        //获取客户端的Cookie对象,交货日期
        HttpCookie DeliveryDate = Request.Cookies["DeliveryDate"];
        if (DeliveryDate != null)
        {
            TimeSpan ts = new TimeSpan(-1, 0, 0, 0);
            DeliveryDate.Expires = DateTime.Now.Add(ts);//删除整个Cookie，只要把过期时间设置为现在
            Response.AppendCookie(DeliveryDate);
        }

        #region 新增普通订单
        if (e.Item.Text == "新增普通订单")
        {
            Response.Redirect("OrderFrame.aspx");
        }
        #endregion

        #region 新增样品资料订单
        if (e.Item.Text == "新增样品资料订单")
        {
            Response.Write("<script>window.open('SampleOrder.aspx','_self')</script>");
        }
        #endregion

        #region 参照酬宾订单
        if (e.Item.Text == "参照酬宾订单")
        {
            Response.Write("<script>window.open('Order_Y.aspx','_self')</script>");
        }
        #endregion

        #region 参照特殊订单
        if (e.Item.Text == "参照特殊订单")
        {
            Response.Write("<script>window.open('Order_X.aspx','_self')</script>");
        }
        #endregion
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

}