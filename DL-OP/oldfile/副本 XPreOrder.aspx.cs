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

public partial class XPreOrder : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //OrderGrid修改提示功能
        OrderGrid.JSProperties.Remove("cpAlertMsg");//先清空,修改grid提示信息
        OrderGrid.JSProperties["cpAlertMsg"] = "";

        TxtBillDate.Text = System.DateTime.Now.ToString("d");   //绑定制单日期
        #region 判断session是否存在,并且建立datatable,XOrder
        if (Session["XOrder"] == null)
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
            //dt.Rows.Add(new object[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21" });//测试用
            Session["XOrder"] = dt;
            OrderGrid.DataSource = dt;
            OrderGrid.DataBind();
        }
        DataTable dtt = (DataTable)Session["XOrder"];
        OrderGrid.DataSource = dtt;
        OrderGrid.DataBind();
        #endregion

        #region 判断session是否存在,并且建立datatable,用于记录选择项目,gridselect
        if (Session["Xgridselect"] == null)
        {
            DataTable dts = new DataTable();
            dts.Columns.Add("cInvCode"); //编码    0            
            //dt.Rows.Add(new object[] { "0"});
            Session["Xgridselect"] = dts;
        }
        #endregion

        if (!IsPostBack)  //非回传
        {
            Session["Xordertreelistgrid"] = "";
        }
        //绑定treelist数据源
        DataTable dttree = new SearchManager().DLproc_InventoryBySel("00","abc");
        treeList.KeyFieldName = "KeyFieldName";
        treeList.ParentFieldName = "ParentFieldName";
        treeList.DataSource = dttree;
        /*展开第一级node*/
        treeList.DataBind();
        treeList.ExpandToLevel(1);
        //绑定gridview数据源
        DataTable dt1 = new SearchManager().DLproc_TreeListDetailsNoQTYBySel(Session["Xordertreelistgrid"].ToString(), Session["ConstcCusCode"].ToString());
        TreeDetail.DataSource = dt1;
        TreeDetail.DataBind();

        //删除当前绑定的数据
        DataTable dtst = (DataTable)Session["Xgridselect"];
        for (int i = 0; i < dt1.Rows.Count; i++)
        {
            for (int j = 0; j < dtst.Rows.Count; j++)
            {
                if (TreeDetail.GetRowValues(i, "cInvCode").ToString() == dtst.Rows[j][0].ToString())
                {
                    TreeDetail.Selection.SelectRow(i);
                    dtst.Rows[j].Delete();
                    dtst.AcceptChanges();
                }
            }
        }
        //ASPxGridView1.DataSource = dtst;//测试用
        //ASPxGridView1.DataBind();
    }

    protected void GridViewShippingMethod_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e) //生成Grid的序号
    {
        if (e.Column.Caption == "序号" && e.IsGetData)
            e.Value = (e.ListSourceRowIndex + 1).ToString();
    }

    protected void BtnCustomerOk_Click(object sender, EventArgs e)      //客户信息选择事件(开票单位)
    {
        //开票单位名称
        string cCusName = CustomerGrid.GetRowValues(CustomerGrid.FocusedRowIndex, "cCusName").ToString();
        TxtCustomer.Text = cCusName.ToString();
        Session["XOrderCustomer"] = TxtCustomer.Text.ToString();
        //开票单位编码
        Session["XOrderKPDWcCusCode"] = CustomerGrid.GetRowValues(CustomerGrid.FocusedRowIndex, "cCusCode").ToString();
        //重新选择开票单位后,重新读取开票单位的存货单价,并且计算金额,重新绑定表体grid,11-01
        DataTable griddata = new DataTable();
        griddata = (DataTable)Session["XOrder"];
        //如果不为空,则遍历,给价格重新赋值
        if (griddata != null)
        {
            if (griddata.Rows.Count > 0)
            {
                for (int i = 0; i < griddata.Rows.Count; i++)
                {
                    DataTable iddt = new OrderManager().DLproc_QuasiOrderDetailBySel(griddata.Rows[i]["cInvCode"].ToString(), Session["XOrderKPDWcCusCode"].ToString());
                    griddata.Rows[i]["cComUnitPrice"] = iddt.Rows[0]["Quote"].ToString();//报价
                    griddata.Rows[i]["ExercisePrice"] = iddt.Rows[0]["ExercisePrice"].ToString();//执行价格
                    griddata.Rows[i]["cComUnitAmount"] = Convert.ToDecimal(iddt.Rows[0]["Quote"].ToString()) * Convert.ToDecimal(griddata.Rows[i]["cInvDefineQTY"].ToString());//报价金额
                    griddata.Rows[i]["Stock"] = 0;//可用库存量
                    /*11-10添加*/
                    griddata.Rows[i]["kl"] = Convert.ToDouble(iddt.Rows[0]["Rate"].ToString()); //扣率   18 Rate
                    griddata.Rows[i]["iTaxRate"] = Convert.ToDouble(iddt.Rows[0]["iTaxRate"].ToString()); //销项税率   20 iTaxRate
                }
                //重新绑定
                OrderGrid.DataSource = griddata;
                OrderGrid.DataBind();
                Session["XOrder"] = griddata;
            }
        }
        ////重新获取信用额度
        //DataTable CusCreditDt = new DataTable();
        //CusCreditDt = new OrderManager().DLproc_getPreOrderCusCreditInfo(Session["XOrderKPDWcCusCode"].ToString(),2);//开票单位编码
        //TxtCusCredit.Text = CusCreditDt.Rows[0]["iCusCreLine"].ToString();
    }

    protected void BtnSaveOrder_Click(object sender, EventArgs e)   //保存并提交订单按钮事件
    {

        ////创建datatable数据;
        DataTable griddata = new DataTable();
        griddata = (DataTable)Session["XOrder"];

        #region 检测数据有效性
        //1,检测是否必填
        if (TxtCustomer.Text == "")
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('请选择开票单位！');</script>");
            return;
        }
        //2,检测是否有数据
        if (griddata.Rows.Count <= 0)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('请先添加商品到订单明细表,保存后再提交订单！');</script>");
            return;
        }
        //3,检测信用额度是否满足,并且检测是否存在数量为0的商品信息
        double CusCredit = 0;
        for (int i = 0; i < griddata.Rows.Count; i++)
        {
            if (0 == Convert.ToDouble(griddata.Rows[i]["cInvDefineQTY"].ToString()))
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('第" + (i + 1) + "行,存在数量为0的商品,请检查！');</script>");
                return;
            }
            //CusCredit += Convert.ToDouble(griddata.Rows[i]["cComUnitAmount"].ToString());
        }
        ////信用额度,20151129变更,增加临时授权,Customer中的cCusPostCode ,1为临时授权,授权后设置为 null(恢复操作在存储过程中完成)
        //DataTable ExtraCredit = new DataTable();
        //ExtraCredit = new OrderManager().DL_ExtraCreditBySel(Session["XOrderKPDWcCusCode"].ToString());
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
        #endregion

        #region 插入表头数据

        string lngopUserId = Session["lngopUserId"].ToString(); //用户id
        string ddate = TxtBillDate.Text.ToString(); //创建日期
        int bytStatus = 1;  //单据状态
        int lngBillType = 2;  //单据类型，特殊订单2
        string ccuscode = Session["XOrderKPDWcCusCode"].ToString();   //开票单位编码
        string ccusname = TxtCustomer.Text; //客户名称    

        //插入表头数据,DL表中
        DataTable lngopOrderIdDt = new DataTable();
        lngopOrderIdDt = new OrderManager().DLproc_NewYOrderByIns(ddate, lngopUserId, bytStatus, ccuscode, ccusname, lngBillType);
        #endregion

        #region 插入表体数据
        #region 遍历并且插入表体数据

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
        //赋值!!!!!!!!!!!!!!!!!!!!!!
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
            OrderInfo oiEntry = new OrderInfo(lngopOrderId, cinvcode, iquantity, inum, iquotedprice, iunitprice, itaxunitprice, imoney, itax, isum, inatunitprice, inatmoney, inattax, inatsum, kl, itaxrate, cDefine22, iinvexchrate, cunitid, irowno, cinvname, idiscount, inatdiscount, cComUnitName, cInvDefine1, cInvDefine2, cInvDefine13, cInvDefine14, unitGroup, cComUnitQTY, cInvDefine1QTY, cInvDefine2QTY, cn1cComUnitName);
            bool c = new OrderManager().DLproc_NewYOrderDetailByIns(oiEntry);
        }

        #endregion
        #endregion

        #region 保存后清空grid数据,并提示
        //清空数据
        if (Session["XOrder"] != null)
        {
            Session.Contents.Remove("XOrder");
        }
        Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script>alert('特殊订单" + strBillNo + " 已经提交,请在已提交订单中查询订单处理进度！');{location.href='XPreOrder.aspx'}</script>");
        #endregion
    }

    protected void OrderGrid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)    //表体表格数据更新
    {
        ////创建datatable数据;
        DataTable griddata = new DataTable();
        griddata = (DataTable)Session["XOrder"];
        //序号,行号,当前操作行,
        int i = Convert.ToInt32(e.NewValues["hh"].ToString());
        DataRow dr = griddata.Rows[i - 1];
        Double sRate = Convert.ToDouble(dr[7].ToString());//获取小包装换算率
        Double bRate = Convert.ToDouble(dr[6].ToString());//获取大包装换算率
        Double cComUnitPrice = Convert.ToDouble(dr[13].ToString());  //基本单位单价
        Double cComUnitQTY = Convert.ToDouble(e.NewValues["cComUnitQTY"].ToString()); //基本单位数量
        Double cInvDefine2QTY = Convert.ToDouble(e.NewValues["cInvDefine2QTY"].ToString());  //小包装数量
        Double cInvDefine1QTY = Convert.ToDouble(e.NewValues["cInvDefine1QTY"].ToString());  //大包装数量
        //包装总量,基本单位
        Double amount = Convert.ToDouble((cComUnitQTY + cInvDefine2QTY * sRate + cInvDefine1QTY * bRate).ToString("N2"));
        //总金额
        Double cComUnitAmount = amount * cComUnitPrice;
        //包装结果
        string pack = Math.Floor(amount / bRate) + dr[4].ToString() + Math.Floor((amount % bRate) / sRate) + dr[5].ToString() + Math.Floor((((amount * 10 * 10) % (sRate * 10 * 10)) / 10) / 10) + dr[3].ToString(); //包装量换算结果  15
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
        Session["XOrder"] = griddata;
    }

    protected void OrderGrid_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)    //保存前,验证grid表体数据有效性
    {
        OrderGrid.JSProperties.Remove("cpAlertMsg");//先清空
        OrderGrid.JSProperties["cpAlertMsg"] = "";
        //检测1,基本单位是否输入正确
        if (e.NewValues["cComUnitName"].ToString() == "米")
        {
            if (Convert.ToDouble((Convert.ToDouble(e.NewValues["cComUnitQTY"].ToString()) * 1000) % (Convert.ToDouble(e.NewValues["cInvDefine14"].ToString()) * 1000)).ToString("0.00") != "0.00")
            {
                e.RowError = "请输入" + e.NewValues["cInvDefine14"].ToString() + "的倍数!";
                //OrderGrid.JSProperties.Remove("cpAlertMsg");//先清空
                //OrderGrid.JSProperties["cpAlertMsg"] = "保存失败!请输入" + e.NewValues["cInvDefine14"].ToString() + "的倍数!";
                return;
            }
        }

        //检测2,检测是否存在数量为0的存货信息
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

    protected void CustomerGrid_Load(object sender, EventArgs e)    //开票单位表绑定数据
    {
        //绑定ComboCustomer
        DataTable DtComboCustomer = new DataTable();
        DtComboCustomer = new SearchManager().DL_ComboCustomerBySel(Session["ConstcCusCode"].ToString() + "%");
        CustomerGrid.DataSource = DtComboCustomer;
        CustomerGrid.DataBind();
    }

    protected void BtnInvOK_Click(object sender, EventArgs e)   //选择商品后,将商品传递到grid中
    {
        DataTable dtst = (DataTable)Session["Xgridselect"];  //获取选中行的值,保存
        if (TreeDetail.Selection.Count > 0)
        {
            for (int i = 0; i < TreeDetail.Selection.Count; i++)
            {
                dtst.Rows.Add(new object[] { TreeDetail.GetSelectedFieldValues("cInvCode")[i].ToString() });
            }
            Session["Xgridselect"] = dtst;
        }

        DataTable YOrderGrid = (DataTable)Session["XOrder"];
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

        //2.将选择的新物料查询出对应的基础数据资料,并且传入XOrder中
        for (int i = 0; i < dtst.Rows.Count; i++)
        {
            DataTable iddt = new OrderManager().DLproc_QuasiOrderDetailBySel(dtst.Rows[i][0].ToString(), "010101");
            #region 将传递过来的数据放入datatable中,并且绑定gridview
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
            #endregion
            YOrderGrid.Rows.Add(new object[] { array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7], array[8], array[9], array[10], array[11], array[12], array[13], array[14], array[15], array[16], array[17], array[18], array[19], array[20], array[21] });
        }
        Session["XOrder"] = YOrderGrid;
        OrderGrid.DataSource = YOrderGrid;
        OrderGrid.DataBind();

        //3,清除选择和session
        TreeDetail.Selection.UnselectAll(); //清除所有选择项
        //清除session数据
        Session.Remove("Xgridselect");

    }

    /*ASPxTreeList的FocuseNodeChnaged事件来处理选择Node时的逻辑,需要引用using DevExpress.Web.ASPxTreeList;*/
    protected void treeList_CustomDataCallback(object sender, TreeListCustomDataCallbackEventArgs e)
    {
        DataTable dtst = (DataTable)Session["Xgridselect"];  //获取选中行的值,保存
        if (TreeDetail.Selection.Count > 0)
        {
            for (int i = 0; i < TreeDetail.Selection.Count; i++)
            {
                dtst.Rows.Add(new object[] { TreeDetail.GetSelectedFieldValues("cInvCode")[i].ToString() });
            }
            Session["Xgridselect"] = dtst;
            //ASPxGridView1.DataSource = dtst;//测试用
            //ASPxGridView1.DataBind();
        }

        /// 设置选中时节点的背景色


        string key = e.Argument.ToString();
        TreeListNode node = treeList.FindNodeByKeyValue(key);
        e.Result = GetEntryText(node);//获取node的code值
    }

    protected string GetEntryText(TreeListNode node)    //树节点调用
    {
        if (node != null)
        {
            string KeyFieldName = node["KeyFieldName"].ToString();
            Session["Xordertreelistgrid"] = KeyFieldName.ToString();//赋值给grid查询
            return KeyFieldName.Trim();
        }
        return string.Empty;
    }

    protected void btn_Click(object sender, EventArgs e)
    {
        //DataTable dtst = (DataTable)Session["Xgridselect"];
        //for (int i = 0; i < TreeDetail.Selection.Count; i++)
        //{
        //    dtst.Rows.Add(new object[] { TreeDetail.GetSelectedFieldValues("cInvCode")[i].ToString() });
        //}
        //Session["Xgridselect"] = dtst;

    }

    protected void BtnInv_Reset_Click(object sender, EventArgs e)
    {
        TreeDetail.Selection.UnselectAll(); //清除所有选择项
        //清除session数据
        Session.Remove("Xgridselect");
    }

    protected void OrderGrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)    //删除表体数据
    {
        string cInvCode = e.Values["cInvCode"].ToString();
        DataTable dtt = (DataTable)Session["XOrder"];
        for (int i = 0; i < dtt.Rows.Count; i++)
        {
            if (cInvCode == dtt.Rows[i][0].ToString())
            {
                dtt.Rows[i].Delete();
                dtt.AcceptChanges();
                break;
            }
        }
        Session["XOrder"] = dtt;
        OrderGrid.DataSource = dtt;
        OrderGrid.DataBind();
        e.Cancel = true;
    }
}