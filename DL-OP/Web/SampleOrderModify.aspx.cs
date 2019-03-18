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

public partial class SampleOrderModify : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //OrderGrid修改提示功能
        OrderGrid.JSProperties.Remove("cpAlertMsg");//先清空,修改grid提示信息
        OrderGrid.JSProperties["cpAlertMsg"] = "";

        if (!IsPostBack)
        {
            string StrBillNo = Session["SampleOrderModify_StrBillNo"].ToString();
            #region 绑定表头数据
            //绑定表头数据
            DataTable dt = new OrderManager().DL_OrderBillBySel(StrBillNo);
            TxtOrderBillNo.Text = StrBillNo;
            Session["SampleOrderModify_KPDWcode"] = dt.Rows[0]["ccuscode"].ToString();
            TxtBiller.Text = dt.Rows[0]["strUserName"].ToString();
            TxtBillDate.Text = dt.Rows[0]["datCreateTime"].ToString();
            TxtCustomer.Text = dt.Rows[0]["ccusname"].ToString();
            TxtSalesman.Text = dt.Rows[0]["cpersoncode"].ToString();
            TxtOrderShippingMethod.Text = dt.Rows[0]["cdefine11"].ToString();
            if (dt.Rows[0]["cSTCode"].ToString() == "00")
            {
                TxtcSCCode.Text = "配送";
            }
            if (dt.Rows[0]["cSTCode"].ToString() == "01")
            {
                TxtcSCCode.Text = "自提";
            }

            TxtOrderMark.Text = dt.Rows[0]["strRemarks"].ToString();
            Txtcdefine3.Text = dt.Rows[0]["cdefine3"].ToString();
            TxtLoadingWays.Text = dt.Rows[0]["strLoadingWays"].ToString();
            TxtBillTime.Text = dt.Rows[0]["datBillTime"].ToString();
            TxtDate.Text = dt.Rows[0]["datDeliveryDate"].ToString();
            #endregion
        }
        #region 绑定表体数据
        #region 判断session是否存在,并且建立datatable
        if (Session["SampleOrderModify_ordergrid"] == null)
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
            //Session["SampleOrderModify_ordergrid"] = dt;
            //绑定表体信息OrderGrid
            DataTable dt = new OrderManager().DLproc_OrderDetailModifyBySel(Session["SampleOrderModify_StrBillNo"].ToString());
            //取消只读
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                dt.Columns[i].ReadOnly = false;
            }
            //OrderGrid.DataSource = dt;
            //OrderGrid.DataBind();
            Session["SampleOrderModify_ordergrid"] = dt;
        }
        DataTable dtt = (DataTable)Session["SampleOrderModify_ordergrid"];
        OrderGrid.DataSource = dtt;
        OrderGrid.DataBind();
        #endregion
        #endregion

        #region 树操作
        #region 判断session是否存在,并且建立datatable,用于记录选择项目,gridselect
        if (Session["SampleOrderModify_gridselect"] == null)
        {
            DataTable dts = new DataTable();
            dts.Columns.Add("cInvCode"); //编码    0            
            //dt.Rows.Add(new object[] { "0"});
            Session["SampleOrderModify_gridselect"] = dts;
        }
        #endregion
        //绑定treelist数据源
        DataTable dttree = new SearchManager().DLproc_InventoryBySel("01", Session["SampleOrderModify_KPDWcode"].ToString());
        treeList.KeyFieldName = "KeyFieldName";
        treeList.ParentFieldName = "ParentFieldName";
        treeList.DataSource = dttree;
        /*展开第一级node*/
        treeList.DataBind();
        treeList.ExpandToLevel(1);
        //绑定gridview数据源
        DataTable dt1 = new DataTable();
        if (Session["SampleOrderModify_treelistgrid"] != null)
        {
            dt1 = new SearchManager().DLproc_TreeListDetailsBySel(Session["SampleOrderModify_treelistgrid"].ToString(), Session["SampleOrderModify_KPDWcode"].ToString());
            TreeDetail.DataSource = dt1;
            TreeDetail.DataBind();

            //删除当前绑定的数据,因为切换分类,所以需要删除
            DataTable dtst = (DataTable)Session["SampleOrderModify_gridselect"];
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
        #endregion

    }

    protected void BtnInvalidOrder_Click(object sender, EventArgs e)
    {
        string strBillNo = Session["SampleOrderModify_StrBillNo"].ToString();
        bool c = new OrderManager().DL_InvalidOrderByUpd(strBillNo, Session["lngopUserId"].ToString());
        if (c)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script>alert('订单" + strBillNo + " 已经作废！');{window.location='PendingOrder.aspx'}</script>");
        }
        else
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script>alert('订单作废失败,请联系管理员！')");
        }
    }


    protected void BtnSaveOrder_Click(object sender, EventArgs e)   //保存并提交订单按钮事件
    {
        ////创建datatable数据;
        DataTable griddata = new DataTable();
        griddata = (DataTable)Session["SampleOrderModify_ordergrid"];
        #region 检测数据有效性
        //1,检测是否必填

        //2,检测是否有数据
        if (griddata.Rows.Count <= 0)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('请先添加商品到订单明细表,保存后再提交订单！');</script>");
            return;
        }
        //3,检测信用额度是否满足,并且检测是否存在数量为0的商品信息

        //先绑定数据,ordergrid表体绑定最新库存数据
        //OrderGrid.DataSource = griddata;
        //OrderGrid.DataBind();
        //如果不满足库存检测,则提示并退出

        #endregion

        //检测
        //return;

        #region 更新表头数据

        //11-21新增字段赋值
        //string lngopUseraddressId = Session["ModifylngopUseraddressId"].ToString();
        //更新表头数据,DL表中
        string strBillNo = Session["SampleOrderModify_StrBillNo"].ToString();
        string strRemarks = TxtOrderMark.Text;
        string strLoadingWays = TxtLoadingWays.Text;
        OrderInfo oi = new OrderInfo(strBillNo, strRemarks, strLoadingWays,1);
        DataTable lngopOrderIdDt = new DataTable();
        lngopOrderIdDt = new OrderManager().DLproc_SampleOrderByUpd(oi);   //返回order的主表id
        #endregion

        #region 更新表体数据
        #region 先删除原表体数据后,遍历并且插入新表体数据
        //////创建datatable数据;
        //DataTable griddata = new DataTable();
        //griddata = (DataTable)Session["SampleOrderModify_ordergrid"];
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
        string cDefine24 = "样品资料";

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
            cn1cComUnitName = dr["cn1cComUnitName"].ToString();    //销售单位名称
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

        string welcome = "1;" + Session["ConstcCusCode"].ToString() + ";" + Session["strUserName"].ToString() + ";" + strBillNo + ";修改样品订单"; //根据Dl_opOrderBillNoSetting表中定义IMType类型
        data = Encoding.UTF8.GetBytes(welcome);
        server.SendTo(data, data.Length, SocketFlags.None, ipep);
        #endregion

        #region 保存后清空grid数据和cookie,并提示

        Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script>alert('订单" + strBillNo + " 已经修改成功,请在已提交订单中查询订单处理进度！');{window.location='PendingOrder.aspx'}</script>");
        #endregion
    }

    protected void GridViewShippingMethod_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e) //生成Grid的序号
    {
        if (e.Column.Caption == "序号" && e.IsGetData)
            e.Value = (e.ListSourceRowIndex + 1).ToString();
    }

    protected void OrderGrid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)    //表体表格数据更新
    {
        //int index = OrderGrid.DataKeys[e.NewEditIndex].Value;//获取主键的值
        //取值 用e.NewValues[索引]
        //string lngopUseraddressId = Convert.ToString(e.Keys[0]);

        ////创建datatable数据;
        DataTable griddata = new DataTable();
        griddata = (DataTable)Session["SampleOrderModify_ordergrid"];
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
        //string pack = Math.Floor(amount / bRate) + dr[4].ToString() + Math.Floor((amount % bRate) / sRate) + dr[5].ToString() + Math.Floor((amount % sRate)) + dr[3].ToString(); //包装量换算结果  15
        string pack = "";
        if (sRate == 0 && bRate == 0)     //没有小包装,没有大包装
        {
            pack = amount + dr[3].ToString(); //包装量换算结果  15
        }
        if (sRate > 0 && bRate == 0)     //有小包装,没有大包装
        {
            pack = Math.Floor((amount * 10 * 10) / (sRate * 10 * 10)) + dr[5].ToString() + Math.Floor((((amount * 10 * 10) % (sRate * 10 * 10)) / 10) / 10) + dr[3].ToString(); //包装量换算结果  15
        }
        if (sRate > 0 && bRate > 0)     //有小包装,有大包装
        {
            pack = Math.Floor((amount * 10 * 10) / (sRate * 10 * 10)) + dr[4].ToString() + Math.Floor(((amount*10*10) % (bRate*10*10)) / (sRate*10*10)) + dr[5].ToString() + Math.Floor((((amount * 10 * 10) % (sRate * 10 * 10)) / 10) / 10) + dr[3].ToString(); //包装量换算结果  15
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
        Session["SampleOrderModify_ordergrid"] = griddata;

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
        if (e.NewValues["cInvDefine14"].ToString() == "0")    //没有小包装换算率的
        {
            if (Convert.ToDouble((Convert.ToDouble(e.NewValues["cComUnitQTY"].ToString())) % 1) != 0)
            {
                e.RowError = "请输入整数!";
                OrderGrid.JSProperties.Remove("cpAlertMsg");//先清空
                OrderGrid.JSProperties["cpAlertMsg"] = "保存失败!请输入整数!";
                return;
            }
        }
        else
        {
            if (Convert.ToDouble((Convert.ToDouble(e.NewValues["cComUnitQTY"].ToString()) * 1000) % (Convert.ToDouble(e.NewValues["cInvDefine14"].ToString()) * 1000)).ToString("0.00") != "0.00")
            {
                e.RowError = "请输入" + e.NewValues["cInvDefine14"].ToString() + "的倍数!";
                OrderGrid.JSProperties.Remove("cpAlertMsg");//先清空
                OrderGrid.JSProperties["cpAlertMsg"] = "保存失败!请输入" + e.NewValues["cInvDefine14"].ToString() + "的倍数!";
                return;
            }
        }
        //if (e.NewValues["cComUnitName"].ToString() == "米")
        //{
        //    if (Convert.ToDouble((Convert.ToDouble(e.NewValues["cComUnitQTY"].ToString()) * 1000) % (Convert.ToDouble(e.NewValues["cInvDefine14"].ToString()) * 1000)).ToString("0.00") != "0.00")
        //    {
        //        //e.Errors.Add(OrderGrid.Columns["cComUnitQTY"], "请输入" + e.NewValues["cInvDefine14"].ToString() + "的倍数!");
        //        e.RowError = "请输入" + e.NewValues["cInvDefine14"].ToString() + "的倍数!";
        //        //throw new Exception("请输入" + e.NewValues["cInvDefine14"].ToString() + "的倍数!");
        //        OrderGrid.JSProperties.Remove("cpAlertMsg");//先清空
        //        OrderGrid.JSProperties["cpAlertMsg"] = "保存失败!请输入" + e.NewValues["cInvDefine14"].ToString() + "的倍数!";
        //        return;
        //    }
        //}
        //if (e.NewValues["cComUnitName"].ToString() != "米")
        //{
        //    if (Convert.ToDouble((Convert.ToDouble(e.NewValues["cComUnitQTY"].ToString()) * 1000) % (Convert.ToDouble(e.NewValues["cInvDefine14"].ToString()) * 1000)).ToString("0.00") != "0.00")
        //    {
        //        //e.Errors.Add(OrderGrid.Columns["cComUnitQTY"], "请输入" + e.NewValues["cInvDefine14"].ToString() + "的倍数!");
        //        e.RowError = "请输入" + e.NewValues["cInvDefine13"].ToString() + "的倍数!";
        //        //throw new Exception("请输入" + e.NewValues["cInvDefine14"].ToString() + "的倍数!");
        //        OrderGrid.JSProperties.Remove("cpAlertMsg");//先清空
        //        OrderGrid.JSProperties["cpAlertMsg"] = "保存失败!请输入" + e.NewValues["cInvDefine13"].ToString() + "的倍数!";
        //        return;
        //    }
        //}
        //检测2,库存可用量是否正确
        //if (TxtcSTCode.Text != "样品资料")
        //{
        //    if (Convert.ToDouble(e.NewValues["cComUnitQTY"].ToString()) + Convert.ToDouble(e.NewValues["cInvDefine1QTY"].ToString()) + Convert.ToDouble(e.NewValues["cInvDefine2QTY"].ToString()) > Convert.ToDouble(e.NewValues["Stock"].ToString()))
        //    {
        //        //e.Errors.Add(OrderGrid.Columns["cInvDefineQTY"], "库存不足!");
        //        e.RowError = "库存不足!";
        //        //throw new Exception("库存不足!");
        //        OrderGrid.JSProperties.Remove("cpAlertMsg");//先清空
        //        OrderGrid.JSProperties["cpAlertMsg"] = "保存失败!库存不足!";
        //        return;
        //    }
        //}
        //检测3,检测是否存在数量为0的存货信息
        //string strSplit = Regex.Replace(e.NewValues["cInvDefineQTY"].ToString(), "[0-9]", "", RegexOptions.IgnoreCase);
        if (0 == Convert.ToDouble(e.NewValues["cComUnitQTY"].ToString()) + Convert.ToDouble(e.NewValues["cInvDefine1QTY"].ToString()) + Convert.ToDouble(e.NewValues["cInvDefine2QTY"].ToString()))
        {
            e.Errors.Add(OrderGrid.Columns["cInvDefineQTY"], "存在数量为0的商品,请检查!");
            e.RowError = "存在数量为0的商品,请检查!";
            throw new Exception("存在数量为0的商品,请检查!");
        }

        OrderGrid.JSProperties.Remove("cpAlertMsg");//先清空
        OrderGrid.JSProperties["cpAlertMsg"] = "订单明细保存成功!";
    }

    protected void OrderGrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)    //删除表体数据
    {
        string cInvCode = e.Values["cInvCode"].ToString();
        DataTable dtt = (DataTable)Session["SampleOrderModify_ordergrid"];
        for (int i = 0; i < dtt.Rows.Count; i++)
        {
            if (cInvCode == dtt.Rows[i][0].ToString())
            {
                dtt.Rows[i].Delete();
                dtt.AcceptChanges();
                break;
            }
        }
        Session["SampleOrderModify_ordergrid"] = dtt;
        OrderGrid.DataSource = dtt;
        OrderGrid.DataBind();
        e.Cancel = true;
    }

    protected void BtnInv_Reset_Click(object sender, EventArgs e)
    {
        TreeDetail.Selection.UnselectAll(); //清除所有选择项
        //清除session数据
        Session.Remove("SampleOrderModify_gridselect");
    } //清除选择项中的所有选项

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

    protected void BtnInvOK_Click(object sender, EventArgs e)   //选择商品后,将商品传递到grid中
    {
        DataTable dtst = (DataTable)Session["SampleOrderModify_gridselect"];  //获取选中行的值,保存
        if (TreeDetail.Selection.Count > 0)
        {
            for (int i = 0; i < TreeDetail.Selection.Count; i++)
            {
                dtst.Rows.Add(new object[] { TreeDetail.GetSelectedFieldValues("cInvCode")[i].ToString() });
            }
            Session["SampleOrderModify_gridselect"] = dtst;
        }
        DataTable YOrderGrid = (DataTable)Session["SampleOrderModify_ordergrid"];
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
            DataTable iddt = new OrderManager().DLproc_QuasiOrderDetailBySel(dtst.Rows[i][0].ToString(), Session["SampleOrderModify_KPDWcode"].ToString());
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
            if (iddt.Rows.Count == 1 && iddt.Rows[0]["cInvCode"].ToString().Substring(0, 2) == "32")
            {
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
        Session["SampleOrderModify_ordergird"] = YOrderGrid;
        OrderGrid.DataSource = YOrderGrid;
        OrderGrid.DataBind();
        TreeDetail.Selection.UnselectAll(); //清除所有选择项
        //清除session数据
        Session.Remove("SampleOrderModify_gridselect");
    }

    /*ASPxTreeList的FocuseNodeChnaged事件来处理选择Node时的逻辑,需要引用using DevExpress.Web.ASPxTreeList;*/
    protected void treeList_CustomDataCallback(object sender, TreeListCustomDataCallbackEventArgs e)
    {
        DataTable dtst = (DataTable)Session["SampleOrderModify_gridselect"];  //获取选中行的值,保存
        if (TreeDetail.Selection.Count > 0)
        {
            for (int i = 0; i < TreeDetail.Selection.Count; i++)
            {
                dtst.Rows.Add(new object[] { TreeDetail.GetSelectedFieldValues("cInvCode")[i].ToString() });
            }
            Session["SampleOrderModify_gridselect"] = dtst;
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
            Session["SampleOrderModify_treelistgrid"] = KeyFieldName.ToString();//赋值给grid查询
            return KeyFieldName.Trim();
            //查询并绑定gridview

        }
        return string.Empty;
    }

    protected void btn_Click(object sender, EventArgs e)
    {
        //DataTable dtst = (DataTable)Session["SampleOrderModify_gridselect"];
        //for (int i = 0; i < TreeDetail.Selection.Count; i++)
        //{
        //    dtst.Rows.Add(new object[] { TreeDetail.GetSelectedFieldValues("cInvCode")[i].ToString() });
        //}
        //Session["SampleOrderModify_gridselect"] = dtst;
        //绑定gridview数据源
        if (Session["SampleOrderModify_treelistgrid"] != null)
        {
            DataTable dt1 = new SearchManager().DLproc_TreeListDetailsBySel(Session["SampleOrderModify_treelistgrid"].ToString(), Session["SampleOrderModify_KPDWcode"].ToString());
            TreeDetail.DataSource = dt1;
            TreeDetail.DataBind();
            //删除当前绑定的数据,因为切换分类,所以需要删除
            DataTable dtst = (DataTable)Session["SampleOrderModify_gridselect"];
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

    protected void BtnCancel_Click(object sender, EventArgs e)
    {

        //清除session
        Session.Remove("SampleOrderModify_StrBillNo");  //单据编号
        Session.Remove("SampleOrderModify_ordergrid");  //表体明细数据
        Session.Remove("SampleOrderModify_gridselect"); //选择的商品明细数据
        Session.Remove("SampleOrderModify_KPDWcode");   //开票单位编码
        Session.Remove("SampleOrderModify_treelistgrid");   //选择树的大类编码
        Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script>{window.location='PendingOrder.aspx'}</script>");

    }

}