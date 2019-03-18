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

public partial class OrderYModify : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //开启时间管理
        DataTable timecontrol = new OrderManager().DL_OrderENTimeControlBySel();
        if (timecontrol.Rows.Count < 1)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('非常抱歉，订单开放时间为 8:00 - 21:00 ！');window.parent.location.href='AllBlank.aspx';</script>");
        }
        #region 创建HashTable,OrderYModify_HT
        if (Session["OrderYModify_HT"] == null)    //如果为null,则创建,此session需要在PendingOrder页面中清除
        {

        }
        #endregion

        #region 非回传,绑定字段,(根据单据编号读取信息,并给txt赋值)
        if (!IsPostBack)
        {
            string strBillNo = Session["OrderYModify_srtBillNo"].ToString();
            DataTable dt = new OrderManager().DL_OrderBillBySel(strBillNo);
            TxtBillNo.Text = strBillNo;
            TxtBiller.Text = dt.Rows[0]["strUserName"].ToString();
            //TxtBillDate.Text = dt.Rows[0]["datCreateTime"].ToString();
            TxtKPWDName.Text = dt.Rows[0]["ccusname"].ToString();
            TxtSalesman.Text = dt.Rows[0]["cpersoncode"].ToString();
            TxtOrderShippingMethod.Text = dt.Rows[0]["cdefine11"].ToString();
            switch (dt.Rows[0]["cSCCode"].ToString())
            {
                case "00":
                    TxtcSCCode.Text = "自提";
                    break;
                case "01":
                    TxtcSCCode.Text = "配送";
                    break;
                default:
                    break;
            }
            TxtBillTime.Text = dt.Rows[0]["datBillTime"].ToString();
            Txtcdefine3.Text = dt.Rows[0]["cdefine3"].ToString();
            DeliveryDate.Text = dt.Rows[0]["datCreateTime"].ToString();
            TxtOrderMark.Text = dt.Rows[0]["strRemarks"].ToString();
            TxtLoadingWays.Text = dt.Rows[0]["strLoadingWays"].ToString();
            Session["OrderYModify_KPDWcCusCode"] = dt.Rows[0]["ccuscode1"].ToString();
            Session["OrderYModify_lngopUseraddressId"] = dt.Rows[0]["lngopUseraddressId"].ToString();
        }
        //表头字段赋值,顾客信用额度
        DataTable CusCreditDt = new DataTable();
        CusCreditDt = new OrderManager().DLproc_getCusCreditInfoWithBillno(Session["OrderYModify_KPDWcCusCode"].ToString(), Session["OrderYModify_srtBillNo"].ToString());//开票单位编码
        TxtCusCredit.Text = CusCreditDt.Rows[0]["iCusCreLine"].ToString();
        TxtBillDate.Text = System.DateTime.Now.ToString("d");   //绑定制单日期
        #endregion

        #region 绑定表体数据,判断session是否存在,并且建立datatable
        if (Session["OrderYModify_OrderGrid"] == null)
        {
            //绑定表体信息OrderGrid
            DataTable dt = new OrderManager().DLproc_OrderYDetailModifyBySel(Session["OrderYModify_srtBillNo"].ToString());
            //取消只读
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                dt.Columns[i].ReadOnly = false;
            }
            Session["OrderYModify_OrderGrid"] = dt;
        }
        DataTable dtt = (DataTable)Session["OrderYModify_OrderGrid"];
        OrderGrid.DataSource = dtt;
        OrderGrid.DataBind();
        #endregion

        #region 绑定树
        #region 判断session是否存在,并且建立datatable,用于记录选择项目,Order_Y_gridselect
        if (Session["OrderYModify_GridSelect"] == null)
        {
            DataTable dts = new DataTable();
            dts.Columns.Add("cInvCode"); //编码    0   
            dts.Columns.Add("cpreordercode"); //预订单号    1  
            dts.Columns.Add("itemid"); //编码+预订单号    2
            dts.Columns.Add("realqty"); //可用量    3  
            dts.Columns.Add("iaoids"); //可用量    3  
            //dt.Rows.Add(new object[] { "0"});
            Session["OrderYModify_GridSelect"] = dts;
        }
        #endregion
        //绑定treelist数据源
        DataTable dtlist = new SearchManager().DL_PreOrderTreeBySel(Session["OrderYModify_KPDWcCusCode"].ToString(), Session["lngopUserId"].ToString(), Session["lngopUserExId"].ToString(), 1);
        treeList.KeyFieldName = "strBillNo";
        treeList.ParentFieldName = "strBillNo";
        treeList.DataSource = dtlist;
        /*展开第一级node*/
        treeList.DataBind();
        treeList.ExpandToLevel(1);
        //绑定gridview数据源
        if (Session["OrderYModify_TreeListGrid"] != null)
        {
            DataTable dt1 = new SearchManager().DLproc_TreeListPreDetailsModifyBySel(Session["OrderYModify_TreeListGrid"].ToString(), Session["OrderYModify_srtBillNo"].ToString(), 1);
            TreeDetail.DataSource = dt1;
            TreeDetail.DataBind();

            //删除当前绑定的数据,因为切换订单号,所以需要删除
            DataTable dtst = (DataTable)Session["OrderYModify_GridSelect"];
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
        #endregion

    }



    protected void ComboShippingMethod_SelectedIndexChanged(object sender, EventArgs e)  //送货信息选择变化,刷新
    {
        if (ComboShippingMethod.Value != null)
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
            btOK.Enabled = true;
        }
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

    protected void btOK_Click(object sender, EventArgs e)   //选择送货方式信息
    {
        if (GridViewShippingMethod.GetRowValues(GridViewShippingMethod.FocusedRowIndex, "ShippingInformation") != null)
        {
            string ShippingInformation = GridViewShippingMethod.GetRowValues(GridViewShippingMethod.FocusedRowIndex, "ShippingInformation").ToString();
            TxtOrderShippingMethod.Text = ShippingInformation.ToString();
            TxtcSCCode.Text = ComboShippingMethod.Value.ToString();
            Session["OrderYModify_lngopUseraddressId"] = GridViewShippingMethod.GetRowValues(GridViewShippingMethod.FocusedRowIndex, "lngopUseraddressId").ToString();
        }
        else
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('请选择送货方式！');</script>");
        }

    }

    protected void Btncdefine3Ok_Click(object sender, EventArgs e)   //选择车型信息选择事件
    {
        //车型名称
        string cdefine3 = cdefine3Grid.GetRowValues(cdefine3Grid.FocusedRowIndex, "cValue").ToString();
        Txtcdefine3.Text = cdefine3.ToString();
        Session["cdefine3"] = cdefine3.ToString();
    }

    protected void cdefine3_Load(object sender, EventArgs e)     //绑定车型信息grid表
    {
        //绑定车型信息grid表
        DataTable cdefine3Griddt = new BasicInfoManager().DL_cdefine3BySel();
        cdefine3Grid.DataSource = cdefine3Griddt;
        cdefine3Grid.DataBind();
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
        if (Convert.ToDouble(e.NewValues["cComUnitQTY"].ToString()) + Convert.ToDouble(e.NewValues["cInvDefine1QTY"].ToString()) * Convert.ToDouble(e.NewValues["cInvDefine13"].ToString()) + Convert.ToDouble(e.NewValues["cInvDefine2QTY"].ToString()) * Convert.ToDouble(e.NewValues["cInvDefine14"].ToString()) > Convert.ToDouble(e.NewValues["Stock"].ToString()))
        {
            e.Errors.Add(OrderGrid.Columns["Stock"], "库存不足!");//库存不足
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

    protected void OrderGrid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)    //表体表格数据更新
    {
        //int index = OrderGrid.DataKeys[e.NewEditIndex].Value;//获取主键的值
        //取值 用e.NewValues[索引]
        //string lngopUseraddressId = Convert.ToString(e.Keys[0]);

        ////创建datatable数据;
        DataTable griddata = new DataTable();
        griddata = (DataTable)Session["OrderYModify_OrderGrid"];
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
        Session["OrderYModify_OrderGrid"] = griddata;

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

        DataTable dtst = (DataTable)Session["OrderYModify_GridSelect"];  //获取选中行的值,保存
        if (TreeDetail.Selection.Count > 0)
        {
            for (int i = 0; i < TreeDetail.Selection.Count; i++)
            {
                dtst.Rows.Add(new object[] { TreeDetail.GetSelectedFieldValues("cInvCode")[i].ToString(), TreeDetail.GetSelectedFieldValues("cCode")[i].ToString(), TreeDetail.GetSelectedFieldValues("itemid")[i].ToString(), TreeDetail.GetSelectedFieldValues("realqty")[i].ToString(), TreeDetail.GetSelectedFieldValues("iaoids")[i].ToString() });
            }
            Session["OrderYModify_GridSelect"] = dtst;
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
            Session["OrderYModify_TreeListGrid"] = KeyFieldName.ToString();//赋值给grid查询           
            return KeyFieldName.Trim();
        }
        return string.Empty;
    }   //取树焦点值



    protected void btn_Click(object sender, EventArgs e)    //树焦点变更引发postback
    {

    }

    protected void TreeDetail_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
    {

        //TreeDetail.Selection.SelectRow(0);
    }


    protected void BtnInv_Reset_Click(object sender, EventArgs e)
    {
        TreeDetail.Selection.UnselectAll(); //清除所有选择项
        //清除session数据
        Session.Remove("OrderYModify_GridSelect");
    }   //重置/清除 商品选择

    protected void OrderGrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)    //删除表体数据
    {
        string cInvCode = e.Values["cInvCode"].ToString();
        DataTable dtt = (DataTable)Session["OrderYModify_OrderGrid"];
        for (int i = 0; i < dtt.Rows.Count; i++)
        {
            if (cInvCode == dtt.Rows[i][0].ToString())
            {
                dtt.Rows[i].Delete();
                dtt.AcceptChanges();
                break;
            }
        }
        Session["OrderYModify_OrderGrid"] = dtt;
        OrderGrid.DataSource = dtt;
        OrderGrid.DataBind();
        e.Cancel = true;
    }

    protected void BtnInvOK_Click(object sender, EventArgs e)   //选择商品后,将商品传递到grid中
    {
        DataTable dtst = (DataTable)Session["OrderYModify_GridSelect"];  //获取选中行的值,保存
        if (TreeDetail.Selection.Count > 0)
        {
            for (int i = 0; i < TreeDetail.Selection.Count; i++)
            {
                dtst.Rows.Add(new object[] { TreeDetail.GetSelectedFieldValues("cInvCode")[i].ToString(), TreeDetail.GetSelectedFieldValues("cCode")[i].ToString(), TreeDetail.GetSelectedFieldValues("itemid")[i].ToString(), TreeDetail.GetSelectedFieldValues("realqty")[i].ToString(), TreeDetail.GetSelectedFieldValues("iaoids")[i].ToString() });
            }
            Session["OrderYModify_GridSelect"] = dtst;
        }

        DataTable YOrderGrid = (DataTable)Session["OrderYModify_OrderGrid"];
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
            DataTable iddt = new OrderManager().DLproc_QuasiYOrderDetailBySel(dtst.Rows[i][0].ToString(), dtst.Rows[i][1].ToString());
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
            //array[12] = iddt.Rows[0]["iquantity"].ToString();//包装量数量汇总
            array[12] = "0";//包装量数量汇总
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
            array[24] = dtst.Rows[i]["realqty"].ToString(); //可用量 
            array[25] = dtst.Rows[i]["iaoids"].ToString(); //预订单id

            #endregion
            YOrderGrid.Rows.Add(new object[] { array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7], array[8], array[9], array[10], array[11], array[12], array[13], array[14], array[15], array[16], array[17], array[18], array[19], array[20], array[21], array[22], array[23], array[24], array[25] });
        }
        Session["OrderYModify_OrderGrid"] = YOrderGrid;
        OrderGrid.DataSource = YOrderGrid;
        OrderGrid.DataBind();

        TreeDetail.Selection.UnselectAll(); //清除所有选择项
        //清除session数据
        Session.Remove("OrderYModify_GridSelect");
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



    protected void BtnSaveOrder_Click(object sender, EventArgs e)   //保存并提交订单按钮事件!!!!!!!!!!!!!!!!!!!!!!!!!!!
    {
        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script>alert('该功能正在完善,订单保存成功,返回处理界面！');{window.parent.location ='PendingOrder.aspx'}</script>");
        //Response.Redirect(Request.QueryString["ourl"].ToString());
        //return;
        ////创建datatable数据;
        DataTable griddata = new DataTable();
        griddata = (DataTable)Session["OrderYModify_OrderGrid"];

        #region 检测数据有效性
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
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('交货日期不能小于当前时间！');</script>");
            return;
        }
        if (griddata.Rows.Count <= 0)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('请先添加商品到订单明细表,保存后再提交订单！');</script>");
            return;
        }

        //2.1 检测是否过期，Session["ModifyTxtOrderBillNo"].ToString()
        bool bExp = new OrderManager().DL_OrderIsExpBySel(Session["OrderYModify_srtBillNo"].ToString());
        if (bExp == false)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('订单已过期！请重新下单！');</script>");
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
        ExtraCredit = new OrderManager().DL_ExtraCreditBySel(Session["OrderYModify_KPDWcCusCode"].ToString());
        if (ExtraCredit.Rows.Count > 0)
        {

        }
        else
        {
            //表头字段赋值,顾客信用额度
            DataTable CusCreditDt = new DataTable();
            CusCreditDt = new OrderManager().DLproc_getCusCreditInfoWithBillno(Session["OrderYModify_KPDWcCusCode"].ToString(), Session["OrderYModify_srtBillNo"].ToString());//开票单位编码
            //TxtCusCredit.Text = CusCreditDt.Rows[0]["iCusCreLine"].ToString();//信用额
            if (CusCredit > Convert.ToDouble(CusCreditDt.Rows[0]["iCusCreLine"].ToString()) && Convert.ToDouble(CusCreditDt.Rows[0]["iCusCreLine"].ToString()) != -99999999)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('已经超过您开票单位的信用额度:" + Convert.ToString((Convert.ToDouble(CusCreditDt.Rows[0]["iCusCreLine"].ToString()) - CusCredit)) + "！');</script>");
                return;
            }
        }
        //4,检测可用库存量是否正确,先更新库存,再调用OrderGrid_RowValidating(暂时无法调用,遍历解决)
        DataTable stockdt = new DataTable();
        bool d = false;
        bool f = false;
        string strdtcheckReslx1 = "行";
        string strdtcheckReslx2 = "行";
        int ix = 0;
        //string forcheck = "";   //待检查数据组合，格式：存货1|数量|预订单号,存货2|数量|预订单号,存货3|数量|预订单号,存货4|数量|预订单号,
        //string forcheckbillno = ""; //当前订单编号
        DataTable dtcheck = new DataTable();
        for (int i = 0; i < griddata.Rows.Count; i++)
        {
            //组合待检查的数据
            //forcheck = forcheck + griddata.Rows[i]["cInvCode"].ToString() + "|" + griddata.Rows[i]["cInvDefineQTY"].ToString() + "|" + griddata.Rows[i]["ccode"].ToString()+",";
            //forcheckbillno = TxtBillNo.Text;
            //创建table的第一列
            DataColumn cinvcode_ex1 = new DataColumn();
            //该列的数据类型
            cinvcode_ex1.DataType = System.Type.GetType("System.String");
            //该列得名称
            cinvcode_ex1.ColumnName = "cinvcode";
            //该列得默认值
            cinvcode_ex1.DefaultValue = 999;
            //创建table的第二列
            DataColumn qty = new DataColumn();
            //该列的数据类型
            qty.DataType = System.Type.GetType("System.Double");
            //该列得名称
            qty.ColumnName = "qty";
            //该列得默认值
            qty.DefaultValue = 0;
            //创建table的第三列
            DataColumn preorderno = new DataColumn();
            //该列的数据类型
            preorderno.DataType = System.Type.GetType("System.String");
            //该列得名称
            preorderno.ColumnName = "preorderno";
            //该列得默认值
            preorderno.DefaultValue = "";
            //创建table的第四列
            DataColumn orderno = new DataColumn();
            //该列的数据类型
            orderno.DataType = System.Type.GetType("System.String");
            //该列得名称
            orderno.ColumnName = "orderno";
            //该列得默认值
            orderno.DefaultValue = "";
            //创建table的第五列
            DataColumn irowsno = new DataColumn();
            //该列的数据类型
            irowsno.DataType = System.Type.GetType("System.Int32");
            //该列得名称
            irowsno.ColumnName = "irowsno";
            //该列得默认值
            irowsno.DefaultValue = 0;
            // 将所有的列添加到table上
            dtcheck.Columns.Add(cinvcode_ex1);
            dtcheck.Columns.Add(qty);
            dtcheck.Columns.Add(preorderno);
            dtcheck.Columns.Add(orderno);
            dtcheck.Columns.Add(irowsno);
            //添加记录
            dtcheck.Rows.Add(griddata.Rows[i]["cInvCode"].ToString(), Convert.ToDouble(griddata.Rows[i]["cInvDefineQTY"].ToString()), griddata.Rows[i]["ccode"].ToString(), TxtBillNo.Text, i + 1);

            if (Session["lngopUserId"].ToString() != ConfigurationManager.AppSettings["PrecCusCode"].ToString())
            {
                //Session["ModifyTxtOrderBillNo"] 
                stockdt = new OrderManager().DLproc_OrderDetailModifyStockQtyCompareBySel(griddata.Rows[i]["cInvCode"].ToString(), TxtBillNo.Text);
                griddata.Rows[i]["Stock"] = stockdt.Rows[0]["qty"].ToString();
                if (Convert.ToDouble(griddata.Rows[i]["Stock"].ToString()) < Convert.ToDouble(griddata.Rows[i]["cInvDefineQTY"].ToString()))
                {
                    ix = i + 1;
                    d = true;
                }
            }
        }
        if (Session["lngopUserId"].ToString() == ConfigurationManager.AppSettings["PrecCusCode"].ToString())
        {
            //返回检测结果
            DataTable dtcheckResl = new OrderManager().DLproc_PreOrderSubmitForCheckBySel(dtcheck);
            for (int i = 0; i < dtcheckResl.Rows.Count; i++)
            {
                //更新库存
                griddata.Rows[i]["Stock"] = dtcheckResl.Rows[i]["realkyqty"].ToString();
                //更新预订单可用量
                griddata.Rows[i]["realqty"] = dtcheckResl.Rows[i]["realpreqty"].ToString();
                //反馈
                if (dtcheckResl.Rows[i]["preqtyreslut"].ToString() == "1")
                {
                    strdtcheckReslx1 = strdtcheckReslx1 + dtcheckResl.Rows[i]["irowsno"].ToString() + ";";
                }
                if (dtcheckResl.Rows[i]["stockreslut"].ToString() == "1")
                {
                    strdtcheckReslx2 = strdtcheckReslx2 + dtcheckResl.Rows[i]["irowsno"].ToString() + ";";
                }
            }
            if (strdtcheckReslx1 != "行")
            {
                strdtcheckReslx1 = strdtcheckReslx1 + "超过预订单剩余数量！";
                f = true;
            }
            if (strdtcheckReslx2 != "行")
            {
                strdtcheckReslx2 = strdtcheckReslx2 + "超过可用库存数量！";
                f = true;
            }
        }
        //先绑定数据,ordergrid表体绑定最新库存数据
        OrderGrid.DataSource = griddata;
        OrderGrid.DataBind();
        //多用参照检测结果
        if (f)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('第" + strdtcheckReslx1 + strdtcheckReslx2 + "!数据已更新，请检查！');</script>");
            return;
        }
        //如果不满足库存检测,则提示并退出
        if (d)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('第" + ix + "行超过可用库存量！可用库存量已更新,请重新调整订单商品数量！');</script>");
            return;
        }

        #endregion

        //检测2016-03-08,检测顾客名称和编码是否一致
        DataTable checkcuscodeandcusname = new OrderManager().DL_CheckCuscodeAndCusnameBySel(Session["OrderYModify_KPDWcCusCode"].ToString(), TxtKPWDName.Text);
        if (checkcuscodeandcusname.Rows.Count < 1)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('开票单位名称与编码不符合，请重新选择该开票单位！');</script>");
            return;
        }

        #region 更新表头数据
        string strBillNo = Session["OrderYModify_srtBillNo"].ToString();	//获取传递过来的DL网单号
        //string lngopUserId = Session["lngopUserId"].ToString(); //用户id
        string lngopUserId = ""; //用户id
        string datCreateTime = TxtBillDate.Text.ToString(); //创建日期
        int bytStatus = 1;  //单据状态
        string ccuscode = Session["OrderYModify_KPDWcCusCode"].ToString();   //开票单位编码
        //string cdefine1 = Session["Modifycdefine1"].ToString();   //自定义项1,司机姓名
        //string cdefine2 = Session["Modifycdefine2"].ToString();   //自定义项2,司机身份证
        //string cdefine3 = Session["ModifyTxtcdefine3"].ToString();  //自定义项3,汽车类型
        //string cdefine9 = Session["Modifycdefine9"].ToString();   //自定义项9,收货人姓名,
        //string cdefine10 = Session["Modifycdefine10"].ToString();  //自定义项10,车牌号
        //string cdefine12 = Session["Modifycdefine12"].ToString();  //自定义项12,收货人电话,
        //string cdefine13 = Session["Modifycdefine13"].ToString();   //自定义项8,司机电话
        string cdefine11 = TxtOrderShippingMethod.Text.ToString();   //自定义项11,收货人地址
        string cdefine1 = "";   //自定义项1,司机姓名
        string cdefine2 = "";  //自定义项2,司机身份证
        string cdefine3 = Txtcdefine3.Text;  //自定义项3,汽车类型
        string cdefine9 = "";   //自定义项9,收货人姓名,
        string cdefine10 = "";  //自定义项10,车牌号
        string cdefine12 = "";  //自定义项12,收货人电话,
        string cdefine13 = "";   //自定义项8,司机电话
        string dpredatebt = TxtBillDate.Text.ToString();    //预发货日期 
        string dpremodatebt = TxtBillDate.Text.ToString();  //预完工日期 
        string ccusname = TxtKPWDName.Text; //客户名称,开票单位名称             
        string strRemarks = TxtOrderMark.Text.ToString();   //备注 
        string cpersoncode = TxtSalesman.Text.ToString();   //业务员编码
        string cSCCode = "00";    //发运方式编码
        if (TxtcSCCode.Text == "配送")     //发运方式编码,00:自提,01:厂车配送
        {
            cSCCode = "01";
        }
        else
        {
            cSCCode = "00";
        }
        //11-21新增字段赋值
        string strLoadingWays = TxtLoadingWays.Text.ToString();     //装车方式
        //string cSTCode = Session["ModifyTxtcSTCode"].ToString();     //销售类型编码
        string cSTCode = "00";     //销售类型编码
        string datDeliveryDate = ""; //交货日期
        datDeliveryDate = DeliveryDate.Value.ToString();

        string lngopUseraddressId = Session["OrderYModify_lngopUseraddressId"].ToString();
        //更新表头数据,DL表中

        OrderInfo oi = new OrderInfo(strBillNo, lngopUserId, datCreateTime, bytStatus, strRemarks, ccuscode, cdefine1, cdefine2, cdefine3, cdefine9, cdefine10, cdefine11, cdefine12, cdefine13, ccusname, cpersoncode, cSCCode, datDeliveryDate, strLoadingWays, lngopUseraddressId);
        DataTable lngopOrderIdDt = new DataTable();
        lngopOrderIdDt = new OrderManager().DLproc_NewYOrderByUpd(oi);
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

        string welcome = "2;" + ccuscode + ";" + ccusname + ";" + strBillNo + ";修改参照酬宾订单"; //根据Dl_opOrderBillNoSetting表中定义IMType类型
        data = Encoding.UTF8.GetBytes(welcome);
        server.SendTo(data, data.Length, SocketFlags.None, ipep);
        #endregion

        #region 保存后清空grid数据和cookie,并提示
        //清空数据,目前,该方法在PendingOrder.aspx中完成,当,跳转至该页面时,load中清除所有session数据

        Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script>alert('订单" + strBillNo + " 已经修改成功,请在已提交订单中查询订单处理进度！');{window.location.href='PendingOrder.aspx'}</script>");
        #endregion
    }
    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script>{window.location='PendingOrder.aspx'}</script>");
    }   //取消修改

    protected void BtnInvalidOrder_Click(object sender, EventArgs e)
    {
        string strBillNo = Session["OrderYModify_srtBillNo"].ToString();
        bool c = new OrderManager().DL_InvalidOrderByUpd(strBillNo, Session["lngopUserId"].ToString());
        if (c)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script>alert('订单" + strBillNo + " 已经作废！');{window.location='PendingOrder.aspx'}</script>");
        }
        else
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script>alert('订单作废失败,请联系管理员！')");
        }
    }   //作废订单
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