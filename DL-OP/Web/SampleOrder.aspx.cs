﻿using BLL;
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

public partial class SampleOrder : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        #region 判断session是否存在,并且建立datatable,用于记录选择项目,gridselect
        if (Session["Samplegridselect"] == null)
        {
            DataTable dts = new DataTable();
            dts.Columns.Add("cInvCode"); //编码    0            
            //dt.Rows.Add(new object[] { "0"});
            Session["Samplegridselect"] = dts;
        }
        #endregion
        //绑定treelist数据源
        DataTable dttree = new SearchManager().DLproc_InventoryBySel("01", Session["KPDWcCusCode"].ToString());
        treeList.KeyFieldName = "KeyFieldName";
        treeList.ParentFieldName = "ParentFieldName";
        treeList.DataSource = dttree;
        /*展开第一级node*/
        treeList.DataBind();
        treeList.ExpandToLevel(1);
        //绑定gridview数据源
        DataTable dt1 = new DataTable();
        if (Session["Sampleordertreelistgrid"] != null)
        {
            dt1 = new SearchManager().DLproc_TreeListDetailsBySel(Session["Sampleordertreelistgrid"].ToString(), Session["KPDWcCusCode"].ToString());
            TreeDetail.DataSource = dt1;
            TreeDetail.DataBind();

            //删除当前绑定的数据,因为切换分类,所以需要删除
            DataTable dtst = (DataTable)Session["Samplegridselect"];
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


        //绑定 TxtRelateU8NO数据
        DataTable RelateU8NO = new SearchManager().DL_ComboCustomerU8NOBySel(Session["cCusCode"].ToString());
        TxtRelateU8NOGridView.DataSource = RelateU8NO;
        TxtRelateU8NOGridView.DataBind();
        if (HttpContext.Current.Request.Cookies["TxtRelateU8NO"] != null)
        {
            TxtRelateU8NO.Text = Server.UrlDecode(HttpContext.Current.Request.Cookies["TxtRelateU8NO"].Value);
        }
        if (Session["TxtRelateU8NO"] != null)
        {
            TxtRelateU8NO.Text = Session["TxtRelateU8NO"].ToString();
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
            #region 赋值
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
            Session["cSTCode"] = "01";
            TxtcSTCode.Text = "样品资料";

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
            #endregion


            ////表头字段赋值,顾客信用额度
            //DataTable CusCreditDt = new DataTable();
            //CusCreditDt = new OrderManager().DLproc_getCusCreditInfo(Session["KPDWcCusCode"].ToString());//默认登录客户编码
            //TxtCusCredit.Text = CusCreditDt.Rows[0]["iCusCreLine"].ToString();
        }
        TxtBiller.Text = Session["strUserName"].ToString(); //绑定制单人
        TxtBillDate.Text = System.DateTime.Now.ToString("d");   //绑定制单日期

        #region 判断session是否存在,并且建立datatable
        if (Session["Sampleordergrid"] == null)
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
            dt.Columns.Add("cComUnitAmount"); //基本单位金额  14
            dt.Columns.Add("pack"); //包装量换算结果  15
            dt.Columns.Add("ExercisePrice"); //基本单位单价(执行价格)   16
            dt.Columns.Add("Stock"); //可用库存量   17
            dt.Columns.Add("kl"); //扣率   18
            dt.Columns.Add("cComUnitCode"); //基本单位编码   19
            dt.Columns.Add("iTaxRate"); //销项税率   20
            dt.Columns.Add("cn1cComUnitName"); //销售单位名称   21
            //dt.Rows.Add(new object[] { "dasdsad", "张1", "98", "94","","","","","" });
            //dt.Rows.Add(new object[] { "fdsfdfdsf", "张2", "99", "94","","","","","" });
            Session["Sampleordergrid"] = dt;
        }
        DataTable dtt = (DataTable)Session["Sampleordergrid"];
        OrderGrid.DataSource = dtt;
        OrderGrid.DataBind();
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
                ////默认选择第一个客户单位(本身单位)
                //TxtCustomer.Text = Session["strUserName"].ToString();
                //Session["TxtCustomer"] = Session["strUserName"].ToString();


                return;
            }
            else
            {
                //删除选择项后绑定数据
                griddt = (DataTable)Session["Sampleordergrid"];
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
        }

        string id = Request.QueryString["id"].ToString();
        //获取物料详细信息,单位,价格等
        DataTable iddt = new OrderManager().DLproc_QuasiOrderDetailBySel(id, Session["cCusCode"].ToString());
        //获取传递过来的参数,并存入数组
        //string[] array = id.Split('|');   //10-25取消传参,改为下面的查询后在绑定

        //将传递过来的数据放入datatable中,并且绑定gridview
        //griddt.Rows.Add(new object[] { array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7], array[8], "0", "0", "0", "0", "88", "9" });
        string[] array = { "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" };
        array[0] = iddt.Rows[0][3].ToString();//存货编码
        array[1] = iddt.Rows[0][0].ToString();//存货名称
        array[2] = iddt.Rows[0][1].ToString();//存货规格
        array[3] = iddt.Rows[0][2].ToString();//基本单位
        array[17] = iddt.Rows[0][9].ToString(); //库存可用量
        array[18] = iddt.Rows[0][8].ToString(); //扣率
        array[19] = iddt.Rows[0][11].ToString(); //基本单位编码
        array[20] = iddt.Rows[0][12].ToString(); //销项税率
        array[21] = iddt.Rows[0][13].ToString(); //销售单位名称
        //10.25从传递过来的参数分解数组,改为传递物料的编码进行查询,在循环添加
        if (iddt.Rows[0]["cInvCode"].ToString().Substring(0, 2) == "32")
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
        if (iddt.Rows.Count == 1 && iddt.Rows[0]["cInvCode"].ToString().Substring(0, 2) != "32")
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
        if (iddt.Rows.Count == 2 && iddt.Rows[0]["cInvCode"].ToString().Substring(0, 2) != "32")
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
        if (iddt.Rows.Count >= 3 && iddt.Rows[0]["cInvCode"].ToString().Substring(0, 2) != "32")
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
        griddt.Rows.Add(new object[] { array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7], array[8], array[9], array[10], array[11], array[12], array[13], array[14], array[15], array[16], array[17], array[18], array[19], array[20], array[21] });
        Session["Sampleordergrid"] = griddt;
        OrderGrid.DataSource = griddt;
        OrderGrid.DataBind();
    }

    protected void GridViewShippingMethod_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e) //生成Grid的序号
    {
        if (e.Column.Caption == "序号" && e.IsGetData)
            e.Value = (e.ListSourceRowIndex + 1).ToString();
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


    public string GetNewBillNo()
    {

        return "";
    }

    protected void BtnSaveOrder_Click(object sender, EventArgs e)   //保存并提交订单按钮事件
    {

        ////创建datatable数据;
        DataTable griddata = new DataTable();
        griddata = (DataTable)Session["Sampleordergrid"];

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
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('请选择送货方式,选择样品资料关联的正式订单号！');</script>");
            return;
        }
        if (TxtcSCCode.Text == "")
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('请选择发运方式,选择样品资料关联的正式订单号！');</script>");
            return;
        }
        if (TxtSalesman.Text == "")
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('请填写业务员,选择样品资料关联的正式订单号！');</script>");
            return;
        }
        if (TxtRelateU8NO.Text == "")
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('请选择样品资料关联的正式订单号！');</script>");
            return;
        }
        //if (DeliveryDate.Text == "")
        //{
        //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('请选择交货日期！');</script>");
        //    return;
        //}
        //2,检测是否有数据
        //if (TxtcSTCode.Text != "样品资料")
        //{
        //    if (Convert.ToDateTime(DeliveryDate.Value) <= DateTime.Now && DeliveryDate.Value != null) //检验交货日期
        //    {
        //        Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('交货日期不能小于当前时间！');</script>");
        //        return;
        //    }
        //}
        if (griddata.Rows.Count <= 0)   //检查是否有商品明细
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('请先添加商品到订单明细表,保存后再提交订单！');</script>");
            return;
        }
        //3,检测信用额度是否满足,并且检测是否存在数量为0的商品信息
        //double CusCredit = 0;
        for (int i = 0; i < griddata.Rows.Count; i++)
        {
            if (0 == Convert.ToDouble(griddata.Rows[i]["cInvDefineQTY"].ToString()))
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('第" + (i + 1) + "行,存在数量为0的商品,请检查！');</script>");
                return;
            }
        }
        //    CusCredit += Convert.ToDouble(griddata.Rows[i]["cComUnitAmount"].ToString());
        //}
        //信用额度,20151129变更,增加临时授权,Customer中的cCusPostCode ,1为临时授权,授权后设置为 null(恢复操作在存储过程中完成)
        //DataTable ExtraCredit = new DataTable();
        //ExtraCredit = new OrderManager().DL_ExtraCreditBySel(Session["KPDWcCusCode"].ToString());
        //if (ExtraCredit.Rows.Count > 0)
        //{

        //}
        //else
        //{
        //    if (CusCredit > Convert.ToDouble(TxtCusCredit.Text) && Convert.ToDouble(TxtCusCredit.Text) != -99999999)
        //    {
        //        Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('已经超过您开票单位的信用额度:" + Convert.ToString((Convert.ToDouble(TxtCusCredit.Text) - CusCredit)) + "！');</script>");
        //        return;
        //    }
        //}

        //4,检测可用库存量是否正确,先更新库存,再调用OrderGrid_RowValidating(暂时无法调用,遍历解决)
        //if (TxtcSTCode.Text != "样品资料")
        //{
        //    DataTable stockdt = new DataTable();
        //    bool d = false;
        //    for (int i = 0; i < griddata.Rows.Count; i++)
        //    {
        //        stockdt = new OrderManager().DLproc_QuasiOrderDetailBySel(griddata.Rows[i]["cInvCode"].ToString(), Session["cCusCode"].ToString());
        //        griddata.Rows[i]["Stock"] = stockdt.Rows[0]["fAvailQtty"].ToString();
        //        if (Convert.ToDouble(griddata.Rows[i]["Stock"].ToString()) < Convert.ToDouble(griddata.Rows[i]["cInvDefineQTY"].ToString()))
        //        {
        //            d = true;
        //        }
        //    }
        //先绑定数据,ordergrid表体绑定最新库存数据
        //    OrderGrid.DataSource = griddata;
        //    OrderGrid.DataBind();
        //    //如果不满足库存检测,则提示并退出
        //    if (d)
        //    {
        //        Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('超过可用库存量！可用库存量已更新,请重新调整订单商品数量！');</script>");
        //        return;
        //    }
        //}

        #endregion

        //检测
        //return;
        #region 插入表头数据
        string lngopUserId = Session["lngopUserId"].ToString(); //用户id
        string datCreateTime = TxtBillDate.Text.ToString(); //创建日期
        int bytStatus = 1;  //单据状态
        //////string ccuscode = Session["KPDWcCusCode"].ToString();   //开票单位编码
        //////if (true)
        //////{

        //////}
        //////string cdefine9 = Session["cdefine9"].ToString();   //自定义项1,收货人姓名,cDefine9
        //////string cdefine12 = Session["cdefine12"].ToString();   //自定义项2,收货人电话,cDefine12
        //////string cdefine11 = TxtOrderShippingMethod.Text.ToString();   //自定义项11,收货人地址,cDefine11
        ////////string cDefine11 = Session["lngopUseraddressId"].ToString();
        //////string cdefine1 = Session["cdefine1"].ToString();   //自定义项8,司机姓名,cDefine1
        //////string cdefine13 = Session["cdefine13"].ToString();   //自定义项9,司机电话,cDefine13
        //////string cdefine2 = Session["cdefine2"].ToString();  //自定义项10,司机身份证,cDefine2
        //////string cdefine3 = Txtcdefine3.Text.ToString();  //自定义项3,汽车类型,cdefine3
        //////string cdefine10 = Session["cdefine10"].ToString();  //自定义项12,车牌号,cDefine10
        //////string dpredatebt = TxtBillDate.Text.ToString();    //预发货日期 
        //////string dpremodatebt = TxtBillDate.Text.ToString();  //预完工日期 
        //////string ccusname = TxtCustomer.Text; //客户名称             
        //////string strRemarks = TxtOrderMark.Text.ToString();   //备注 
        //////string cpersoncode = TxtSalesman.Text.ToString();   //业务员编码
        //////string cSCCode = "00";    //发运方式编码
        //////if (Session["TxtcSCCode"].ToString() == "配送")     //发运方式编码,00:自提,01:厂车配送
        //////{
        //////    Session["TxtcSCCode"] = "01";
        //////    cSCCode = "01";
        //////}
        //////else
        //////{
        //////    Session["TxtcSCCode"] = "00";
        //////}
        ////////11-18新增字段赋值
        //////string strLoadingWays = TxtLoadingWays.Text.ToString();     //装车方式
        //////string cSTCode = "";     //销售类型编码    
        //////if (TxtcSTCode.Text == "普通销售")
        //////{
        //////    cSTCode = "00";
        //////}
        //////else
        //////{
        //////    cSTCode = "01";
        //////}
        //////string datDeliveryDate = ""; //交货日期
        //////string lngopUseraddressId = Session["lngopUseraddressId"].ToString();
        ////////11-26增加,样品资料对应正式订单号
        //////string strTxtRelateU8NO = TxtRelateU8NO.Text;
        //////if (DeliveryDate.Value == null)
        //////{
        //////    datDeliveryDate = DateTime.Now.ToString();
        //////}
        //////else
        //////{
        //////    datDeliveryDate = DeliveryDate.Value.ToString();
        //////}

        //插入表头数据,DL表中
        //OrderInfo oi = new OrderInfo(lngopUserId, datCreateTime, bytStatus, strRemarks, ccuscode, cdefine1, cdefine2, cdefine3, cdefine9, cdefine10, cdefine11, cdefine12, cdefine13, ccusname, cpersoncode, cSCCode, datDeliveryDate, strLoadingWays, cSTCode, lngopUseraddressId, strTxtRelateU8NO);
        //DataTable lngopOrderIdDt = new DataTable();
        //lngopOrderIdDt = new OrderManager().DLproc_NewOrderByIns(oi);
        /*20151221增加,复制strbillno数据*/
        string strTxtRelateU8NO = TxtRelateU8NO.Text;
        string strRemarks = TxtOrderMark.Text;
        string strLoadingWays = TxtLoadingWays.Text;
        string lngopUserExId = Session["lngopUserExId"].ToString();
        string strAllAcount = Session["strAllAcount"].ToString();
        DataTable lngopOrderIdDt = new OrderManager().DLproc_NewSampleOrderByIns(strTxtRelateU8NO, strRemarks, strLoadingWays, lngopUserExId, strAllAcount);

        #endregion

        #region 插入表体数据
        #region 遍历并且插入表体数据
        //////创建datatable数据;
        //DataTable griddata = new DataTable();
        //griddata = (DataTable)Session["Sampleordergrid"];
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
        double itaxrate = 17;    //税率 
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
        string cDefine24 = "样品资料";

        for (int i = 0; i < griddata.Rows.Count; i++)
        {
            //string strName = griddata.Rows[i]["字段名"].ToString();
            DataRow dr = griddata.Rows[i];  //定义当前行数据
            //赋值
            irowno = i + 1;             //行号
            cinvcode = dr[0].ToString();//存货编码
            cinvname = dr[1].ToString();//存货名称
            iquantity = Convert.ToDouble(dr[12].ToString());   //存货数量
            iquotedprice = Math.Round(Convert.ToDouble(dr[13].ToString()), 5);//报价,保留5位小数,四舍五入
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
            itaxunitprice = Math.Round(Convert.ToDouble(dr[16].ToString()), 5);//原币含税单价,即执行价格dt.Columns.Add("ExercisePrice"); //基本单位单价(执行价格)   16,保留5位
            isum = Math.Round(itaxunitprice * iquantity, 2);        //金额,原币价税合计=原币含税单价*数量,保留2位小数,四舍五入
            itax = Math.Round(isum / 1.17 * 0.17, 2);        //原币税额 ;税额=金额/1.17*0.17 保留2位, 四舍五入
            imoney = Math.Round(isum - itax, 2);      //原币无税金额 =金额-税额,保留2位,四舍五入
            iunitprice = Math.Round(imoney / iquantity, 5);  //原币无税单价=无税金额/数量,保留5位小数,四舍五入                 
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

        string welcome = "1;" + Session["ConstcCusCode"].ToString() + ";" + Session["strUserName"].ToString() + ";" + strBillNo + ";新增样品订单"; //根据Dl_opOrderBillNoSetting表中定义IMType类型
        data = Encoding.UTF8.GetBytes(welcome);
        server.SendTo(data, data.Length, SocketFlags.None, ipep);
        #endregion

        #region 保存后清空grid数据,并提示
        //清空数据
        if (Session["Sampleordergrid"] != null)
        {
            Session.Contents.Remove("Sampleordergrid");
        }
        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单 " + strBillNo + " 已经提交,请在已提交订单中查询订单处理进度!');</script>");
        Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script>alert('订单" + strBillNo + " 已经提交,请在已提交订单中查询订单处理进度！');{location.href='SampleOrder.aspx'}</script>");
        #endregion
    }

    protected void OrderGrid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)    //表体表格数据更新
    {
        //int index = OrderGrid.DataKeys[e.NewEditIndex].Value;//获取主键的值
        //取值 用e.NewValues[索引]
        //string lngopUseraddressId = Convert.ToString(e.Keys[0]);

        ////创建datatable数据;
        DataTable griddata = new DataTable();
        griddata = (DataTable)Session["Sampleordergrid"];
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
        string pack = "";
        if (sRate == 0 && bRate == 0)     //没有小包装,没有大包装
        {
            pack = amount + dr[3].ToString(); //包装量换算结果  15
        }
        if (sRate > 0 && bRate == 0)     //有小包装,没有大包装
        {
            pack = Math.Floor(amount / sRate) + dr[5].ToString() + Math.Floor((((amount * 10 * 10) % (sRate * 10 * 10)) / 10) / 10) + dr[3].ToString(); //包装量换算结果  15
        }
        if (sRate > 0 && bRate > 0)     //有小包装,有大包装
        {
            pack = Math.Floor(amount / bRate) + dr[4].ToString() + Math.Floor((amount % bRate) / sRate) + dr[5].ToString() + Math.Floor((((amount * 10 * 10) % (sRate * 10 * 10)) / 10) / 10) + dr[3].ToString(); //包装量换算结果  15
        }


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
        Session["Sampleordergrid"] = griddata;

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

        //检测2,库存可用量是否正确
        //if (TxtcSTCode.Text != "样品资料")
        //{
        //    if (Convert.ToDouble(e.NewValues["cComUnitQTY"].ToString()) + Convert.ToDouble(e.NewValues["cInvDefine1QTY"].ToString()) + Convert.ToDouble(e.NewValues["cInvDefine2QTY"].ToString()) > Convert.ToDouble(e.NewValues["Stock"].ToString()))
        //    {
        //        e.Errors.Add(OrderGrid.Columns["cInvDefineQTY"], "库存不足!");//库存不足
        //        e.RowError = "库存不足!";
        //        OrderGrid.JSProperties.Remove("cpAlertMsg");//先清空
        //        OrderGrid.JSProperties["cpAlertMsg"] = "库存不足!";
        //        return;
        //        //throw new Exception("库存不足!");
        //    }
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

    protected void BtnTxtRelateU8NOOk_Click(object sender, EventArgs e) //选择样品资料关联U8订单号
    {
        string strBillNo = TxtRelateU8NOGridView.GetRowValues(TxtRelateU8NOGridView.FocusedRowIndex, "strBillNo").ToString();
        TxtRelateU8NO.Text = strBillNo;
        Session["TxtRelateU8NO"] = strBillNo;
        //读取关联的U8订单信息
        DataTable AuthOrderInfo = new DataTable();
        AuthOrderInfo = new OrderManager().DL_OrderBillYPZLBySel(strBillNo);
        //文本框赋值
        TxtCustomer.Text = AuthOrderInfo.Rows[0]["ccusname"].ToString();  //开票单位
        Session["ccusname"] = TxtCustomer.Text;
        TxtSalesman.Text = AuthOrderInfo.Rows[0]["cpersoncode"].ToString();    //业务员
        Session["cpersoncode"] = TxtSalesman.Text;
        TxtOrderShippingMethod.Text = AuthOrderInfo.Rows[0]["cdefine11"].ToString();  //送货方式
        Session["TxtOrderShippingMethod"] = TxtOrderShippingMethod.Text;
        TxtcSCCode.Text = AuthOrderInfo.Rows[0]["TxtcSCCode"].ToString();   //发运方式
        Session["TxtcSCCode"] = TxtcSCCode.Text;
        //TxtOrderMark.Text = AuthOrderInfo.Rows[0]["strRemarks"].ToString();    //备注
        //HttpCookie CookieTxtOrderMark = new HttpCookie("TxtOrderMark");
        //CookieTxtOrderMark.Value = TxtOrderMark.Text;
        //System.Web.HttpContext.Current.Response.Cookies.Add(CookieTxtOrderMark);

        Txtcdefine3.Text = AuthOrderInfo.Rows[0]["cdefine3"].ToString();   //车型
        Session["cdefine3"] = Txtcdefine3.Text;
        //TxtLoadingWays.Text = AuthOrderInfo.Rows[0]["strLoadingWays"].ToString();   //装车方式

        ////HttpCookie cookie = new HttpCookie("TxtLoadingWays");
        ////cookie.Values.Set("", TxtLoadingWays.Text);
        ////Response.SetCookie(cookie);
        //HttpCookie CookieTxtLoadingWays = new HttpCookie("TxtLoadingWays");
        //CookieTxtLoadingWays.Value = TxtLoadingWays.Text;
        //System.Web.HttpContext.Current.Response.Cookies.Add(CookieTxtLoadingWays);

        DeliveryDate.Value = AuthOrderInfo.Rows[0]["datDeliveryDate"].ToString();   //提货时间
        DeliveryDate.Text = AuthOrderInfo.Rows[0]["datDeliveryDateText"].ToString();   //提货时间
        HttpCookie CookieDeliveryDate = new HttpCookie("DeliveryDate");
        CookieDeliveryDate.Value = AuthOrderInfo.Rows[0]["datDeliveryDateText"].ToString();
        System.Web.HttpContext.Current.Response.Cookies.Add(CookieDeliveryDate);

    }

    //protected void OrderGrid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
    //{
    //    //if (e.RowType != DevExpress.Web.ASPxGridView.) return;
    //    if (e.GetValue("Stock") != null)
    //    {
    //        string Stock = e.GetValue("Stock").ToString();
    //        float StockInt = float.Parse(Stock);
    //        if (StockInt == 0)
    //        {
    //            //e.Row.Cells[11].Style.Add("color", "Red");
    //            e.Row.Style.Add("color", "Red");
    //        }
    //    }
    //}

    protected void BtnInvOK_Click(object sender, EventArgs e)   //选择商品后,将商品传递到grid中
    {
        DataTable dtst = (DataTable)Session["Samplegridselect"];  //获取选中行的值,保存

        if (TreeDetail.Selection.Count > 0)
        {
            for (int i = 0; i < TreeDetail.Selection.Count; i++)
            {
                dtst.Rows.Add(new object[] { TreeDetail.GetSelectedFieldValues("cInvCode")[i].ToString() });
            }
            Session["Samplegridselect"] = dtst;
        }

        DataTable YOrderGrid = (DataTable)Session["Sampleordergrid"];

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
        //2.将选择的新物料查询出对应的基础数据资料,并且传入YOrder中
        for (int i = 0; i < dtst.Rows.Count; i++)
        {
            DataTable iddt = new OrderManager().DLproc_QuasiOrderDetailBySel(dtst.Rows[i][0].ToString(), Session["KPDWcCusCode"].ToString());
            #region 将传递过来的数据放入datatable中,并且绑定gridview
            //griddt.Rows.Add(new object[] { array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7], array[8], "0", "0", "0", "0", "88", "9" });
            string[] array = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21" };
            array[0] = iddt.Rows[0]["cInvCode"].ToString();//存货编码
            array[1] = iddt.Rows[0]["cInvName"].ToString();//存货名称
            array[2] = iddt.Rows[0]["cInvStd"].ToString();//存货规格
            array[3] = iddt.Rows[0]["cComUnitName"].ToString();//基本单位
            array[17] = iddt.Rows[0]["fAvailQtty"].ToString(); //库存可用量
            array[18] = iddt.Rows[0]["Rate"].ToString(); //扣率
            array[19] = iddt.Rows[0]["cComUnitCode"].ToString(); //基本单位编码
            array[20] = iddt.Rows[0]["iTaxRate"].ToString(); //销项税率
            array[21] = iddt.Rows[0]["cn1cComUnitName"].ToString(); //销售单位名称
            //10.25从传递过来的参数分解数组,改为传递物料的编码进行查询,在循环添加
            //if (iddt.Rows[0]["cInvCode"].ToString().Substring(0, 2) == "32")
            //{
            //    array[4] = iddt.Rows[0]["cComUnitName"].ToString();
            //    array[5] = iddt.Rows[0]["cComUnitName"].ToString();
            //    array[6] = iddt.Rows[0]["iChangRate"].ToString();
            //    array[7] = iddt.Rows[0]["iChangRate"].ToString();
            //    array[8] = Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[0][4].ToString()), 2)) + iddt.Rows[0][2].ToString() + "=" + Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[0][4].ToString()), 2)) + iddt.Rows[0][2].ToString() + "=" + Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[0][4].ToString()), 2)) + iddt.Rows[0][2].ToString();
            //    array[9] = "0";
            //    array[10] = "0";
            //    array[11] = "0";
            //    array[12] = "0";
            //    array[13] = iddt.Rows[0]["Quote"].ToString();
            //    array[14] = "0";
            //    array[15] = "0";
            //    array[16] = iddt.Rows[0]["ExercisePrice"].ToString();
            //}
            if (iddt.Rows.Count == 1 && iddt.Rows[0]["cInvCode"].ToString().Substring(0, 2) == "32")
            {
                //array[4] = iddt.Rows[0]["cComUnitName"].ToString();
                //array[5] = iddt.Rows[0]["cComUnitName"].ToString();
                //array[6] = iddt.Rows[0]["iChangRate"].ToString();
                //array[7] = iddt.Rows[0]["iChangRate"].ToString();
                array[4] = "";
                array[5] = "";
                array[6] = "0";
                array[7] = "0";
                array[8] = Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[0][4].ToString()), 2)) + iddt.Rows[0][2].ToString();
                array[9] = "0";
                array[10] = "0";
                array[11] = "0";
                array[12] = "0";
                array[13] = iddt.Rows[0]["Quote"].ToString();
                array[14] = "0";
                array[15] = "0";
                array[16] = iddt.Rows[0]["ExercisePrice"].ToString();
            }
            if (iddt.Rows.Count == 2 && iddt.Rows[0]["cInvCode"].ToString().Substring(0, 2) == "32")
            {
                //array[4] = iddt.Rows[1]["cComUnitName"].ToString();
                array[4] = "";
                array[5] = iddt.Rows[1]["cComUnitName"].ToString();
                //array[6] = iddt.Rows[1]["iChangRate"].ToString();
                array[6] = "0";
                array[7] = iddt.Rows[1]["iChangRate"].ToString();
                array[8] = Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[1][4].ToString()), 2)) + iddt.Rows[0][2].ToString() + "=" + Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[1][4].ToString()) / Convert.ToDouble(iddt.Rows[1][4].ToString()), 2)) + iddt.Rows[1][2].ToString();
                array[9] = "0";
                array[10] = "0";
                array[11] = "0";
                array[12] = "0";
                array[13] = iddt.Rows[0]["Quote"].ToString();
                array[14] = "0";
                array[15] = "0";
                array[16] = iddt.Rows[0]["ExercisePrice"].ToString();
            }
            if (iddt.Rows.Count == 3 && iddt.Rows[0]["cInvCode"].ToString().Substring(0, 2) == "32")
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
            if (iddt.Rows.Count > 3 && iddt.Rows[0]["cInvCode"].ToString().Substring(0, 2) == "32")
            {
                //array[4] = iddt.Rows[2]["cComUnitName"].ToString();
                //array[5] = iddt.Rows[1]["cComUnitName"].ToString();
                //array[6] = iddt.Rows[2]["iChangRate"].ToString();
                //array[7] = iddt.Rows[1]["iChangRate"].ToString();
                array[4] = "";
                array[5] = "";
                array[6] = "0";
                array[7] = "0";
                array[8] = Convert.ToString(Math.Round(Convert.ToDouble(iddt.Rows[0][4].ToString()), 2)) + iddt.Rows[0][2].ToString();
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
            YOrderGrid.Rows.Add(new object[] { array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7], array[8], array[9], array[10], array[11], array[12], array[13], array[14], array[15], array[16], array[17], array[18], array[19], array[20], array[21] });
        }
        Session["Sampleordergird"] = YOrderGrid;
        OrderGrid.DataSource = YOrderGrid;
        OrderGrid.DataBind();

        TreeDetail.Selection.UnselectAll(); //清除所有选择项
        //清除session数据
        Session.Remove("Samplegridselect");
    }

    /*ASPxTreeList的FocuseNodeChnaged事件来处理选择Node时的逻辑,需要引用using DevExpress.Web.ASPxTreeList;*/
    protected void treeList_CustomDataCallback(object sender, TreeListCustomDataCallbackEventArgs e)
    {
        DataTable dtst = (DataTable)Session["Samplegridselect"];  //获取选中行的值,保存
        if (TreeDetail.Selection.Count > 0)
        {
            for (int i = 0; i < TreeDetail.Selection.Count; i++)
            {
                dtst.Rows.Add(new object[] { TreeDetail.GetSelectedFieldValues("cInvCode")[i].ToString() });
            }
            Session["Samplegridselect"] = dtst;
            //ASPxGridView1.DataSource = dtst;//测试用
            //ASPxGridView1.DataBind();
        }

        /// 设置选中时节点的背景色


        string key = e.Argument.ToString();
        TreeListNode node = treeList.FindNodeByKeyValue(key);
        if (node.ChildNodes.Count == 0)       //末级节点
        {
            e.Result = GetEntryText(node);//获取node的code值    
        }
    }

    protected string GetEntryText(TreeListNode node)    //树节点调用
    {
        if (node != null)
        {
            string KeyFieldName = node["KeyFieldName"].ToString();
            Session["Sampleordertreelistgrid"] = KeyFieldName.ToString();//赋值给grid查询
            return KeyFieldName.Trim();
            //查询并绑定gridview

        }
        return string.Empty;
    }



    protected void btn_Click(object sender, EventArgs e)
    {
        //DataTable dtst = (DataTable)Session["Samplegridselect"];
        //for (int i = 0; i < TreeDetail.Selection.Count; i++)
        //{
        //    dtst.Rows.Add(new object[] { TreeDetail.GetSelectedFieldValues("cInvCode")[i].ToString() });
        //}
        //Session["Samplegridselect"] = dtst;
        //绑定gridview数据源
        if (Session["Sampleordertreelistgrid"] != null)
        {
            DataTable dt1 = new SearchManager().DLproc_TreeListDetailsBySel(Session["Sampleordertreelistgrid"].ToString(), Session["KPDWcCusCode"].ToString());
            TreeDetail.DataSource = dt1;
            TreeDetail.DataBind();
            //删除当前绑定的数据,因为切换分类,所以需要删除
            DataTable dtst = (DataTable)Session["Samplegridselect"];
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
        Session.Remove("Samplegridselect");
    } //清除选择项中的所有选项

    protected void OrderGrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)    //删除表体数据
    {
        string cInvCode = e.Values["cInvCode"].ToString();
        DataTable dtt = (DataTable)Session["Sampleordergrid"];
        for (int i = 0; i < dtt.Rows.Count; i++)
        {
            if (cInvCode == dtt.Rows[i][0].ToString())
            {
                dtt.Rows[i].Delete();
                dtt.AcceptChanges();
                break;
            }
        }
        Session["Sampleordergird"] = dtt;
        OrderGrid.DataSource = dtt;
        OrderGrid.DataBind();
        e.Cancel = true;
    }

    //protected void treeList_Load(object sender, EventArgs e)
    //{
    //    //绑定treelist数据源
    //    DataTable dttree = new SearchManager().DLproc_InventoryBySel(Session["cSTCode"].ToString(), "aaa");
    //    treeList.KeyFieldName = "KeyFieldName";
    //    treeList.ParentFieldName = "ParentFieldName";
    //    treeList.DataSource = dttree;
    //    /*展开第一级node*/
    //    treeList.DataBind();
    //    treeList.ExpandToLevel(1);
    //    ////绑定gridview数据源
    //    //DataTable dt1 = new SearchManager().DLproc_TreeListDetailsBySel(Session["Sampleordertreelistgrid"].ToString(), Session["KPDWcCusCode"].ToString());
    //    //TreeDetail.DataSource = dt1;
    //    //TreeDetail.DataBind();
    //}

    protected void ASPxMenu2_ItemClick(object source, MenuItemEventArgs e)
    {
        Session.Remove("Ytreelistgrid");
        Session.Remove("Yordergrid");
        Session.Remove("Order_Y_gridselect");
        Session.Remove("TxtRelateU8NO");
        Session.Remove("Sampleordergrid");
        TxtOrderShippingMethod.Text = "";
        TxtcSCCode.Text = "";
        Txtcdefine3.Text = "";
        TxtRelateU8NO.Text = "";

        //清除order.aspx中的送货地址信息
        if (Session["TxtOrderShippingMethod"] != null)
        {
            Session.Contents.Remove("TxtOrderShippingMethod");
        }
        //清除order.aspx中的订单表体信息
        if (Session["Sampleordergrid"] != null)
        {
            Session.Contents.Remove("ordergrid");
        }
        //清除order.aspx中的物料选择
        if (Session["Samplegridselect"] != null)
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

    protected void OrderGrid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e) //锁定单元格(数量)
    {

        //if (e.Column.FieldName == "UnitGroup")  //单位组
        //{
        //    if (e.Value != null)
        //    {
        //        var text = e.Value.ToString();
        //        int count = (text.Length - text.Replace("=", "").Length) / "=".Length;
        //        Session["UnitGroupCount"] = count;
        //    }
        //}
        //if (e.Column.FieldName == "cInvDefine2QTY")    //小包装单位数量
        //{
        //    if (Session["UnitGroupCount"] != null)
        //    {
        //        if (Session["UnitGroupCount"].ToString() == "1" || Session["UnitGroupCount"].ToString() == "2" || Session["UnitGroupCount"].ToString() == "0")
        //        {

        //        }
        //        else
        //        {
        //            e.Editor.ReadOnly = true;
        //            //e.Editor.BackColor = System.Drawing.Color.Brown;
        //        }
        //    }
        //}
        //if (e.Column.FieldName == "cInvDefine1QTY")    //大包装单位数量
        //{
        //    if (Session["UnitGroupCount"] != null && Session["UnitGroupCount"].ToString() == "2")
        //    {

        //    }
        //    else
        //    {
        //        e.Editor.ReadOnly = true;
        //        //e.Editor.BackColor = System.Drawing.Color.Brown;
        //    }
        //}

    }
    protected void OrderGrid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)  //  改变背景色
    {
        if (e.DataColumn.Caption == "单位组")
        {
            if (e.CellValue != null)
            {
                var text = e.CellValue.ToString();
                int count = (text.Length - text.Replace("=", "").Length) / "=".Length;
                Session["UnitGroupCount_HtmlDataCellPrepared"] = count;
            }
            //e.Cell.BackColor = System.Drawing.Color.Brown;
        }
        if (e.DataColumn.FieldName == "cInvDefine2")    //小包装单位
        {
            if (Session["UnitGroupCount_HtmlDataCellPrepared"] != null)
            {
                if (Session["UnitGroupCount_HtmlDataCellPrepared"].ToString() == "1" || Session["UnitGroupCount_HtmlDataCellPrepared"].ToString() == "2")
                {

                }
                else
                {
                    e.Cell.BackColor = System.Drawing.Color.White;

                }
            }
        }
        if (e.DataColumn.FieldName == "cInvDefine1")    //大包装单位
        {
            if (Session["UnitGroupCount_HtmlDataCellPrepared"] != null && Session["UnitGroupCount_HtmlDataCellPrepared"].ToString() == "2")
            {

            }
            else
            {
                e.Cell.BackColor = System.Drawing.Color.White;

            }
        }
        if (e.DataColumn.FieldName == "cInvDefine2QTY")    //小包装单位数量
        {
            if (Session["UnitGroupCount_HtmlDataCellPrepared"] != null)
            {
                if (Session["UnitGroupCount_HtmlDataCellPrepared"].ToString() == "1" || Session["UnitGroupCount_HtmlDataCellPrepared"].ToString() == "2")
                {

                }
                else
                {
                    e.Cell.ForeColor = System.Drawing.Color.White;
                    e.Cell.BackColor = System.Drawing.Color.White;

                }
            }
        }
        if (e.DataColumn.FieldName == "cInvDefine1QTY")    //大包装单位数量
        {
            if (Session["UnitGroupCount_HtmlDataCellPrepared"] != null && Session["UnitGroupCount_HtmlDataCellPrepared"].ToString() == "2")
            {

            }
            else
            {
                e.Cell.ForeColor = System.Drawing.Color.White;
                e.Cell.BackColor = System.Drawing.Color.White;

            }
        }

    }
}