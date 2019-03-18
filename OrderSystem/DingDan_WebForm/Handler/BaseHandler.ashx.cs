//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Converters;
//using System.Data;
//using BLL;
//using System.Web.SessionState;
//using System.Text;
//using Model;
//using System.Web.Script.Serialization;
//using System.ServiceModel.Web;
//using System.Runtime.Serialization.Json;
//using System.IO;
//using Newtonsoft.Json.Linq;


//namespace DingDan_WebForm
//{
//    /// <summary>
//    /// BaseHandler 的摘要说明
//    /// </summary>
//    public class BaseHandler : DingDan_WebForm.Handler.Base
//    {
//        BLL.product pro = new product();
//        BLL.OrderManager order = new OrderManager();
//        public void ProcessRequest(HttpContext context)
//        {
//            string ConstcCusCode = context.Session["ConstcCusCode"].ToString();

//            ReInfo re = new ReInfo();
//            switch (context.Request.Form["Action"])
//            {
//                //case "Get_Product_Class":
//                //    context.Response.Write(Get_Product_Class());
//                //    break;
//                //case "Get_Product_List":
//                //    context.Response.Write(Get_Product_List(context.Request.Form["cInvCCode"]));
//                //    break;
//                case "DLproc_QuasiOrderDetailBySel":
//                    context.Response.Write(DLproc_QuasiOrderDetailBySel(context.Request.Form["codes"]));
//                    break;
//                //case "Change_KPDW":
//                //    string kpdw = context.Request.Form["kpdw"];
//                //    string c = context.Request.Form["codes"];
//                //    ReInfo r = Change_KPDW(kpdw, c);
//                //    context.Response.Write(JsonConvert.SerializeObject(r));
//                //    break;
//                case "DLproc_NewOrderByIns":
//                    ReInfo ri = Check_Buy_list();
//                    if (ri.flag == "1")
//                    {
//                        context.Response.Write(JsonConvert.SerializeObject(ri));
//                    }
//                    else if (ri.flag == "0")
//                    {
//                        string[] back = DLproc_NewOrderByIns();
//                        DLproc_NewOrderDetailByIns(back);
//                        re.message = back[1];
//                        re.flag = "0";
//                        context.Response.Write(JsonConvert.SerializeObject(re));
//                    }
//                    else if (ri.flag == "2")
//                    {
//                        context.Response.Write(JsonConvert.SerializeObject(ri));
//                    }

//                    break;
//                case "DLproc_AddOrderBackByIns":
//                    ReInfo ri1 = DLproc_AddOrderBackByIns();
//                    if (ri1.flag == "1")
//                    {
//                        context.Response.Write("保存临时订单失败！");
//                    }
//                    else if (ri1.flag == "0")
//                    {
//                        context.Response.Write("保存临时订单成功！");
//                    }

//                    break;
//                //case "DL_GetOrderBackBySel":
//                //    DataTable dt = new DataTable();
//                //    dt = pro.DL_GetOrderBackBySel(Convert.ToInt32(context.Session["lngopUserId"].ToString()));
//                //    context.Response.Write(JsonConvert.SerializeObject(dt, new DataTableConverter()));
//                //    break;
//                case "DL_DelOrderBackDetailByDel":
//                    context.Response.Write(JsonConvert.SerializeObject(DL_DelOrderBackDetailByDel()));
//                    break;
//                case "Get_BackOrder":
//                    context.Response.Write(JsonConvert.SerializeObject(Get_BackOrder()));
//                    break;
//                //case "DL_GeneralPreviousOrderBySel":
//                //    DataTable historyorder = DL_GeneralPreviousOrderBySel(context.Session["ConstcCusCode"].ToString());
//                //    context.Response.Write(JsonConvert.SerializeObject(historyorder, new DataTableConverter()));
//                //    break;
//                //case "DLproc_ReferencePreviousOrderWithCusInvLimitedBySel":
//                //    context.Response.Write(JsonConvert.SerializeObject(DLproc_ReferencePreviousOrderWithCusInvLimitedBySel(), new DataTableConverter()));
//                //    break;
//                case "Get_RejectOrder":
//                    context.Response.Write(JsonConvert.SerializeObject(Get_RejectOrder(), new DataTableConverter()));
//                    break;
//                case "Get_RejectOrder_Detail":
//                    context.Response.Write(JsonConvert.SerializeObject(Get_RejectOrder_Detail(context.Request.Form["strBillNo"]), new DataTableConverter()));
//                    break;
//                case "Cancel_Order":
//                    context.Response.Write(JsonConvert.SerializeObject(Cancel_Order()));
//                    break;
//                case "DLproc_OrderDetailModifyBySel":
//                    //    context.Response.Write(JsonConvert.SerializeObject(DLproc_OrderDetailModifyBySel(), new DataTableConverter()));
//                    context.Response.Write(JsonConvert.SerializeObject(DLproc_OrderDetailModifyBySel()));
//                    break;
//                case "Write_ModifyOrder_strBillNo":
//                    Write_ModifyOrder_strBillNo();
//                    break;
//                case "Refresh_fAvailQtty":
//                    context.Response.Write(JsonConvert.SerializeObject(Refresh_fAvailQtty()));
//                    break;
//                case "DLproc_NewOrderByUpd":
//                    context.Response.Write(JsonConvert.SerializeObject(DLproc_NewOrderByUpd()));
//                    break;
//                case "DLproc_SampleOrderByUpd":    //更新样品订单表头信息
//                    context.Response.Write(JsonConvert.SerializeObject(DLproc_SampleOrderByUpd()));
//                    break;
//                case "Get_SampleOrder_Title":  //获取样品订单相关联的主订单表头
//                    context.Response.Write(JsonConvert.SerializeObject(Get_SampleOrder_Title(), new DataTableConverter())); //查询订单(DL编号,用于样品资料自动填写内容)
//                    break;
//                case "Insert_SampleOrder": //插入样品订单 
//                    context.Response.Write(JsonConvert.SerializeObject(Insert_SampleOrder()));
//                    break;
//                // case "Get_BaseInfo":   // 获取订单初使化时的客户基本信息，如开票单位，信用，车型 
//                // context.Response.Write(JsonConvert.SerializeObject(Get_BaseInfo()));
//                //  break;
//                case "Get_KPDW":
//                    context.Response.Write(JsonConvert.SerializeObject(Get_KPDW(), new DataTableConverter()));
//                    break;
//                case "DLproc_U8SOASearchBySel":// 获取用户账单列表
//                    context.Response.Write(JsonConvert.SerializeObject(DLproc_U8SOASearchBySel()));
//                    break;
//                case "DL_ConfimSOAByUpd": //确认账单
//                    context.Response.Write(JsonConvert.SerializeObject(DL_ConfimSOAByUpd()));
//                    break;
//                case "DLproc_SOADetailforCustomerBySel": //获取账单明细
//                    context.Response.Write(JsonConvert.SerializeObject(DLproc_SOADetailforCustomerBySel()));
//                    break;
//                case "DLproc_OrderExecuteBySel": //查询订单执行情况 
//                    context.Response.Write(JsonConvert.SerializeObject(DLproc_OrderExecuteBySel()));
//                    break;
//                //case "DLproc_UserAddressZTBySelGroup": //获取地址及行政区
//                //    context.Response.Write(JsonConvert.SerializeObject(DLproc_UserAddressZTBySelGroup()));
//                //    break;
//                //case "DLproc_QuasiOrderDetailBySel_new": //获取产品详细列表，新
//                //    context.Response.Write(JsonConvert.SerializeObject(DLproc_QuasiOrderDetailBySel_new(), new DataTableConverter()));
//                //    break;
//                case "DLproc_NewYOrderByIns": //新增特殊订单
//                    context.Response.Write(JsonConvert.SerializeObject(DLproc_NewYOrderByIns()));
//                    break;
//                case "DLproc_MyWorkPreYOrderForCustomerBySel": //查询特殊订单列表
//                    context.Response.Write(JsonConvert.SerializeObject(DLproc_MyWorkPreYOrderForCustomerBySel()));
//                    break;
//                case "DL_XOrderBillDetailBySel": //查看某个特殊订单明细
//                    context.Response.Write(JsonConvert.SerializeObject(DL_XOrderBillDetailBySel()));
//                    break;
//                case "DL_PreOrderTreeBySel": //在新增特殊订单时，查询可用于参照的特殊订单号
//                    context.Response.Write(JsonConvert.SerializeObject(DL_PreOrderTreeBySel()));
//                    break;
//                case "DLproc_TreeListPreDetailsBySel": //根据选择的特殊订单号查询详细产品列表
//                    context.Response.Write(JsonConvert.SerializeObject(DLproc_TreeListPreDetailsBySel()));
//                    break;
//                case "DLproc_QuasiYOrderDetail_TSBySel": //根据选择的特殊订单里的产品，获取详细产品价格等信息
//                    context.Response.Write(JsonConvert.SerializeObject(DLproc_QuasiYOrderDetail_TSBySel()));
//                    break;
//                case "DLproc_NewYYOrderByIns"://参照特殊订单后提交普通订单
//                    context.Response.Write(JsonConvert.SerializeObject(DLproc_NewYYOrderByIns()));
//                    break;
//                //case "DLproc_NewOrderByIns_new": //新增普通订单，新
//                //    context.Response.Write(JsonConvert.SerializeObject(DLproc_NewOrderByIns_new()));
//                //    break;
//                //case "DLproc_ReferencePreviousOrderWithCusInvLimitedBySel_new": //获取历史订单产品列表,新
//                //    context.Response.Write(JsonConvert.SerializeObject(DLproc_ReferencePreviousOrderWithCusInvLimitedBySel_new()));
//                //    break;
//                //case "DLproc_ReferenceOrderBackWithCusInvLimitedBySel_new": //获取临时订单产品列表,新
//                //    context.Response.Write(JsonConvert.SerializeObject(DLproc_ReferenceOrderBackWithCusInvLimitedBySel_new()));
//                //    break;
//                case "DLproc_AddOrderBackByIns_new": //保存临时订单，新
//                    context.Response.Write(JsonConvert.SerializeObject(DLproc_AddOrderBackByIns_new()));
//                    break;
//                case "DL_UnauditedOrder_SubBySel":// 获取历史订单列表
//                    context.Response.Write(JsonConvert.SerializeObject(DL_UnauditedOrder_SubBySel()));
//                    break;
//                case "DL_OrderBillBySel"://获取订单明细
//                    context.Response.Write(JsonConvert.SerializeObject(DL_OrderBillBySel()));
//                    break;
//                case "DL_PreviousOrderBySel": //获取历史订单列表
//                    context.Response.Write(JsonConvert.SerializeObject(DL_PreviousOrderBySel()));
//                    break;
//                case "DL_OrderU8BillBySel": //根据正式订单号查询订单明细
//                    context.Response.Write(JsonConvert.SerializeObject(DL_OrderU8BillBySel()));
//                    break;
//                case "DL_UnauditedOrderBySel"://获取待确认订单列表
//                    context.Response.Write(JsonConvert.SerializeObject(DL_UnauditedOrderBySel()));
//                    break;
//                case "DL_OrderU8BillBySel_new": //根据待确认订单号查询订单明细
//                    context.Response.Write(JsonConvert.SerializeObject(DL_OrderU8BillBySel_new()));
//                    break;
//                case "DL_U8OrderBillConfirmByUpd": //确认U8里的订单
//                    context.Response.Write(JsonConvert.SerializeObject(DL_U8OrderBillConfirmByUpd()));
//                    break;
//            }

//        }

//        public override void AjaxProcess(HttpContext context)
//        {
//            ReInfo ri = new ReInfo();
//            string Action = HttpContext.Current.Request["Action"];
//            if (string.IsNullOrEmpty(Action))
//            {
//                ri.flag = "0";
//                ri.message = "错误的方法!";
//                context.Response.Write(JsonConvert.SerializeObject(ri));
//                return;
//            }
//            var method = this.GetType().GetMethod(Action);
//            if (method == null)
//            {
//                ri.flag = "0";
//                ri.message = "错误的方法!";
//                context.Response.Write(JsonConvert.SerializeObject(ri));
//                return;
//            }
//            method.Invoke(this, new object[] { });
//            //base.AjaxProcess();
//        }

//        #region 取出产品分类
//        /// <summary>
//        /// 取出产品分类
//        /// </summary>
//        /// <param name="ConstcCusCode"></param>
//        /// <returns></returns>
//        public void Get_Product_Class()
//        {
//            ReInfo ri = new ReInfo();
//            string cSTCode = HttpContext.Current.Request.Form["cSTCode"].ToString();
//            string kpdw = HttpContext.Current.Request.Form["kpdw"];
//            if (string.IsNullOrEmpty(kpdw) || kpdw == "0")
//            {
//                ri.flag = "0";
//                ri.message = "请先选择开票单位!";
//                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
//                return;
//            }
//            DataTable dt = pro.DL_InventoryBySel(HttpContext.Current.Request.Form["cSTCode"].ToString(), HttpContext.Current.Request.Form["kpdw"]);
//            dt.Columns["KeyFieldName"].ColumnName = "id";
//            dt.Columns["ParentFieldName"].ColumnName = "pid";
//            dt.Columns["NodeName"].ColumnName = "name";
//            ri.flag = "1";
//            ri.dt = dt;
//            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));

//        }
//        #endregion


//        //#region 根据分类获取商品列表
//        ///// <summary>
//        ///// 根据分类获取商品列表
//        ///// </summary>
//        ///// <param name="ConstcCusCode"></param>
//        ///// <returns></returns>
//        //public void Get_Product_List()
//        //{
//        //    ReInfo ri = new ReInfo();
//        //    string cInvCCode = HttpContext.Current.Request.Form["cInvCCode"].ToString();
//        //    string kpdw = HttpContext.Current.Request.Form["kpdw"];
//        //    if (string.IsNullOrEmpty(kpdw) || kpdw == "0")
//        //    {
//        //        ri.flag = "0";
//        //        ri.message = "请先选择开票单位!";
//        //        HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
//        //        return;
//        //    }
//        //    DataTable dt = pro.DL_TreeListDetailsAllBySel(cInvCCode, kpdw);
//        //    ri.dt = dt;
//        //    ri.flag = "1";
//        //    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
//        //    return;

//        //}
//        //#endregion


//        #region 根据选中商品获取购物清单
//        /// <summary>
//        /// 根据选中商品获取购物清单
//        /// </summary>
//        /// <param name="ConstcCusCode"></param>
//        /// <returns></returns>
//        public string DLproc_QuasiOrderDetailBySel(string codes)
//        {
//            string[] code = codes.Split(',');
//            List<string> list = new List<string>();
//            StringBuilder html = new StringBuilder();
//            //for (int i = 0; i < code.Length; i++)
//            //{
//            //    list.Add( JsonConvert.SerializeObject(order.DLproc_QuasiOrderDetailBySel(code[i], cCuscode), new DataTableConverter()));
//            //}

//            for (int i = 0; i < code.Length; i++)
//            {
//                DataTable dt = new DataTable();
//                dt = order.DLproc_QuasiOrderDetailBySel(code[i], HttpContext.Current.Request.Form["kpdw"]);
//                html.Append(Get_html(dt));
//            }

//            //  string str = JsonConvert.SerializeObject(list);
//            return html.ToString();

//        }
//        #endregion

//        #region 拼接buy_list的html
//        public string Get_html(DataTable dt)
//        {
//            StringBuilder sb = new StringBuilder();
//            if (dt.Rows.Count == 3)
//            {
//                sb.Append(@"<tr><td class='ui-widget-content'><a href='javascript:void(0)'  class='del_this'>删除</a>
//                                <a href='javascript:void(0)'  class='up_this'>上移</a>  <a href='javascript:void(0)'  class='down_this'>下移</a></td>");
//                string unit_s = Convert.ToDouble(dt.Rows[0]["iChangRate"].ToString()).ToString();
//                string unit_m = Convert.ToDouble(dt.Rows[1]["iChangRate"].ToString()).ToString();
//                string unit_b = Convert.ToDouble(dt.Rows[2]["iChangRate"].ToString()).ToString();
//                sb.Append("<td code='" + dt.Rows[0]["cInvCode"] + "' unit_s='" + unit_s + "' unit_m='" + unit_m + "' unit_b='" + unit_b + "'>2</td>");
//                sb.Append("<td   kl='" + dt.Rows[0]["Rate"].ToString() + "' cInvDefine13='" + dt.Rows[1]["iChangRate"].ToString() +
//                            "' cInvDefine14='" + dt.Rows[2]["iChangRate"].ToString()
//                   + "' iTaxRate='" + dt.Rows[0]["itaxrate"].ToString() + "' cunitid='" + dt.Rows[0]["cComUnitCode"].ToString() + "'>"
//                        + dt.Rows[0]["cInvName"].ToString() + "</td>");
//                sb.Append("<td>" + dt.Rows[0]["cInvStd"].ToString() + "</td>");
//                sb.Append("<td>" + unit_b + dt.Rows[0]["cComUnitName"].ToString() + "=" + (Convert.ToDouble(unit_b) / Convert.ToDouble(unit_m)).ToString() + dt.Rows[1]["cComUnitName"].ToString()
//                    + "=" + unit_s + dt.Rows[2]["cComUnitName"].ToString() + "</td>");
//                sb.Append("<td class='in'></td>");
//                sb.Append("<td>" + dt.Rows[0]["cComUnitName"].ToString() + "</td>");
//                sb.Append("<td class='in'></td>");
//                sb.Append("<td>" + dt.Rows[1]["cComUnitName"].ToString() + "</td>");
//                sb.Append("<td class='in'></td>");
//                sb.Append("<td>" + dt.Rows[2]["cComUnitName"].ToString() + "</td>");
//                sb.Append("<td></td>");
//                sb.Append("<td>" + Convert.ToDouble(dt.Rows[0]["fAvailQtty"].ToString()).ToString() + "</td>");
//                sb.Append("<td></td>");
//                sb.Append("<td>" + dt.Rows[0]["Quote"].ToString() + "</td>");
//                sb.Append("<td></td>");
//                sb.Append("<td style='display:none'>" + dt.Rows[0]["ExercisePrice"].ToString() + "</td>");
//                sb.Append("<td style='display:none'></td></tr>");
//            }
//            else if (dt.Rows.Count == 2)
//            {
//                sb.Append(@"<tr><td class='ui-widget-content'><a href='javascript:void(0)'  class='del_this'>删除</a>
//                                <a href='javascript:void(0)'  class='up_this'>上移</a>  <a href='javascript:void(0)'  class='down_this'>下移</a></td>");
//                string unit_s = Convert.ToDouble(dt.Rows[0]["iChangRate"].ToString()).ToString();
//                string unit_m = Convert.ToDouble(dt.Rows[1]["iChangRate"].ToString()).ToString();
//                string unit_b = Convert.ToDouble(dt.Rows[1]["iChangRate"].ToString()).ToString();
//                sb.Append("<td code='" + dt.Rows[0]["cInvCode"] + "' unit_s='" + unit_s + "' unit_m='" + unit_m + "' unit_b='" + unit_b + "'>2</td>");
//                sb.Append("<td   kl='" + dt.Rows[0]["Rate"].ToString() + "' cInvDefine13='" + dt.Rows[1]["iChangRate"].ToString() +
//                         "' cInvDefine14='" + dt.Rows[1]["iChangRate"].ToString()
//                 + "' iTaxRate='" + dt.Rows[0]["itaxrate"].ToString() + "' cunitid='" + dt.Rows[0]["cComUnitCode"].ToString() + "'>"
//                 + dt.Rows[0]["cInvName"].ToString() + "</td>");
//                sb.Append("<td>" + dt.Rows[0]["cInvStd"].ToString() + "</td>");
//                sb.Append("<td>" + unit_b + dt.Rows[0]["cComUnitName"].ToString() + "=" + unit_s + dt.Rows[1]["cComUnitName"].ToString()
//                    + "=" + unit_s + dt.Rows[1]["cComUnitName"].ToString() + "</td>");
//                sb.Append("<td class='in'></td>");
//                sb.Append("<td>" + dt.Rows[0]["cComUnitName"].ToString() + "</td>");
//                sb.Append("<td class='in'></td>");
//                sb.Append("<td>" + dt.Rows[1]["cComUnitName"].ToString() + "</td>");
//                sb.Append("<td class='in'></td>");
//                sb.Append("<td>" + dt.Rows[1]["cComUnitName"].ToString() + "</td>");
//                sb.Append("<td></td>");
//                sb.Append("<td>" + Convert.ToDouble(dt.Rows[0]["fAvailQtty"].ToString()).ToString() + "</td>");
//                sb.Append("<td></td>");
//                sb.Append("<td>" + dt.Rows[0]["Quote"].ToString() + "</td>");
//                sb.Append("<td></td>");
//                sb.Append("<td style='display:none'>" + dt.Rows[0]["ExercisePrice"].ToString() + "</td>");
//                sb.Append("<td style='display:none'></td></tr>");
//            }
//            else if (dt.Rows.Count == 1)
//            {
//                sb.Append(@"<tr><td class='ui-widget-content'><a href='javascript:void(0)'  class='del_this'>删除</a>
//                                <a href='javascript:void(0)'  class='up_this'>上移</a>  <a href='javascript:void(0)'  class='down_this'>下移</a></td>");
//                string unit_s = Convert.ToDouble(dt.Rows[0]["iChangRate"].ToString()).ToString();
//                string unit_m = Convert.ToDouble(dt.Rows[0]["iChangRate"].ToString()).ToString();
//                string unit_b = Convert.ToDouble(dt.Rows[0]["iChangRate"].ToString()).ToString();
//                sb.Append("<td code='" + dt.Rows[0]["cInvCode"] + "' unit_s='" + unit_s + "' unit_m='" + unit_m + "' unit_b='" + unit_b + "'>2</td>");
//                sb.Append("<td   kl='" + dt.Rows[0]["Rate"].ToString() + "' cInvDefine13='" + dt.Rows[0]["iChangRate"].ToString() +
//                         "' cInvDefine14='" + dt.Rows[0]["iChangRate"].ToString()
//                  + "' iTaxRate='" + dt.Rows[0]["itaxrate"].ToString() + "' cunitid='" + dt.Rows[0]["cComUnitCode"].ToString() + "'>"
//                 + dt.Rows[0]["cInvName"].ToString() + "</td>");
//                sb.Append("<td>" + dt.Rows[0]["cInvStd"].ToString() + "</td>");
//                sb.Append("<td>" + unit_b + dt.Rows[0]["cComUnitName"].ToString() + "=" + unit_s + dt.Rows[0]["cComUnitName"].ToString()
//                    + "=" + unit_s + dt.Rows[0]["cComUnitName"].ToString() + "</td>");
//                sb.Append("<td class='in'></td>");
//                sb.Append("<td>" + dt.Rows[0]["cComUnitName"].ToString() + "</td>");
//                sb.Append("<td class='in'></td>");
//                sb.Append("<td>" + dt.Rows[0]["cComUnitName"].ToString() + "</td>");
//                sb.Append("<td class='in'></td>");
//                sb.Append("<td>" + dt.Rows[0]["cComUnitName"].ToString() + "</td>");
//                sb.Append("<td></td>");
//                sb.Append("<td>" + Convert.ToDouble(dt.Rows[0]["fAvailQtty"].ToString()).ToString() + "</td>");
//                sb.Append("<td></td>");
//                sb.Append("<td>" + dt.Rows[0]["Quote"].ToString() + "</td>");
//                sb.Append("<td></td>");
//                sb.Append("<td style='display:none'>" + dt.Rows[0]["ExercisePrice"].ToString() + "</td>");
//                sb.Append("<td style='display:none'></td></tr>");
//            }
//            return sb.ToString();
//        }
//        #endregion

//        #region 正式订单插入表头信息
//        public string[] DLproc_NewOrderByIns()
//        {
//            OrderInfo oi = Get_HeadInfo();
//            #region 2016-09-13,检查是否存在未参照完的酬宾订单的商品
//            //string Strpreorderleft = "行：";
//            //if (griddata.Rows.Count > 0)
//            //{
//            //    for (int i = 0; i < griddata.Rows.Count; i++)
//            //    {
//            //        DataTable preorderleft = new OrderManager().DLproc_PerOrderCinvcodeLeftBySel(Session["lngopUserExId"].ToString(), Session["lngopUserId"].ToString(), Session["KPDWcCusCode"].ToString(), griddata.Rows[i][0].ToString());
//            //        if (Convert.ToDouble(preorderleft.Rows[0][0].ToString()) > 0)
//            //        {
//            //            Strpreorderleft = Strpreorderleft + griddata.Rows[i]["irowno"].ToString() + "," + griddata.Rows[i]["cInvName"].ToString() + "数量：" + preorderleft.Rows[0][0].ToString() + ";";
//            //        }
//            //    }
//            //}
//            ////提示有未完成的酬宾订单
//            //if (Strpreorderleft.ToString() != "行：")
//            //{
//            //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('" + Strpreorderleft.ToString() + "存在未参照完的酬宾订单，请先将酬宾订单的商品参照完毕后在普通订单中购买该商品！" + "');</script>");
//            //    return;
//            //}
//            #endregion
//            DataTable lngopOrderIdDt = new DataTable();
//            lngopOrderIdDt = new OrderManager().DLproc_NewOrderByIns(oi);
//            int lngopOrderId = Convert.ToInt32(lngopOrderIdDt.Rows[0]["lngopOrderId"].ToString());
//            string strBillNo = lngopOrderIdDt.Rows[0]["strBillNo"].ToString();
//            // string back = lngopOrderId + "," + strBillNo;
//            string[] back = new string[]{
//            lngopOrderId.ToString(),
//            strBillNo
//            };
//            return back;
//        }
//        #endregion

//        #region 正式订单插入表体数据
//        public bool DLproc_NewOrderDetailByIns(string[] back)
//        {
//            List<Buy_list> list = Get_BodyInfo();
//            #region 遍历并且插入表体数据
//            //////创建datatable数据;
//            //DataTable griddata = new DataTable();
//            //griddata = (DataTable)Session["ordergrid"];
//            int lngopOrderId = Convert.ToInt32(back[0]);   //订单id,从表头中获取:表头插入后,返回表头id
//            string strBillNo = back[1];   //订单编号,插入表头数据时自动生成,用于反馈给用户
//            string cinvcode = "";   //存货编码
//            double iquantity = 1;   //存货数量
//            double inum = 1;        //辅计量数量   
//            double iquotedprice = 1;//报价 
//            double iunitprice = 1;  //原币无税单价
//            double itaxunitprice = 1;//原币含税单价
//            double imoney = 1;      //原币无税金额 
//            double itax = 1;        //原币税额 
//            double isum = 1;        //原币价税合计
//            double idiscount = 1;   //原币折扣额 
//            double inatunitprice = 1;//本币无税单价
//            double inatmoney = 1;   //本币无税金额
//            double inattax = 1;     //本币税额 
//            double inatsum = 1;     //本币价税合计
//            double inatdiscount = 1;   //本币折扣额  
//            double kl = 1;          //扣率 
//            double itaxrate = 17;    //税率 
//            string cDefine22 = "";  //表体自定义项22,包装量
//            double iinvexchrate = 1;//换算率 
//            string cunitid = "";    //计量单位编码
//            int irowno = 1; //行号,从1开始,自增长
//            string cinvname = "";   //存货名称 
//            //11-09新增保存字段
//            string cComUnitName = "";       //基本单位名称
//            string cInvDefine1 = "";        //大包装单位名称         
//            string cInvDefine2 = "";        //小包装单位名称 
//            double cInvDefine13 = 0;       //大包装换算率
//            double cInvDefine14 = 0;       //小包装换算率
//            string unitGroup = "";          //单位换算率组
//            double cComUnitQTY = 0;        //基本单位数量
//            double cInvDefine1QTY = 0;     //大包装单位数量
//            double cInvDefine2QTY = 0;     //小包装单位数量
//            string cn1cComUnitName = "";    //销售单位名称
//            int len = list.Count;
//            for (int i = 0; i < len; i++)
//            {
//                //string strName = griddata.Rows[i]["字段名"].ToString();

//                //赋值
//                //irowno = i + 1;             //行号
//                irowno = list[i].irowno;     //行号
//                cinvcode = list[i].cinvcode;//存货编码
//                cinvname = list[i].cinvname;//存货名称
//                iquantity = list[i].iquantity;   //存货数量
//                iquotedprice = Math.Round(list[i].iquotedprice, 6);//报价,保留5位小数,四舍五入
//                kl = list[i].kl;          //扣率
//                //11-09新增字段赋值
//                cComUnitName = list[i].cComUnitName;       //基本单位名称
//                cInvDefine1 = list[i].cInvDefine1;        //大包装单位名称         
//                cInvDefine2 = list[i].cInvDefine2;        //小包装单位名称 
//                cInvDefine13 = list[i].cInvDefine13;       //大包装换算率
//                cInvDefine14 = list[i].cInvDefine14;       //小包装换算率
//                unitGroup = list[i].unitGroup;          //单位换算率组
//                cComUnitQTY = list[i].cComUnitQTY;        //基本单位数量
//                cInvDefine1QTY = list[i].cInvDefine1QTY;     //大包装单位数量
//                cInvDefine2QTY = list[i].cInvDefine2QTY;     //小包装单位数量


//                iinvexchrate = 1;//换算率 

//                //计算赋值
//                //换算结果:辅计量数量 =存货数量/换算率,四舍五入,保留两位小数
//                inum = 1; //辅计量数量 

//                /*VER:V1,11-03*/
//                //iunitprice = Math.Round(Convert.ToDouble(dr[16].ToString()) / 1.17, 4);  //原币无税单价,=原币含税单价/1.17,保留四位小数,四舍五入
//                //itaxunitprice = Convert.ToDouble(dr[16].ToString());//原币含税单价,即执行价格dt.Columns.Add("ExercisePrice"); //基本单位单价(执行价格)   16
//                //imoney = iunitprice * iquantity;      //原币无税金额 =原币无税单价*数量
//                //itax = Math.Round((itaxunitprice - iunitprice) * iquantity, 2);        //原币税额 ;税额=(含税单价-无税单价)*数量 [四舍五入],保留两位小数
//                //isum = Math.Round(itaxunitprice * iquantity, 5);        //原币价税合计;=原币含税单价*数量,保留四位小数,四舍五入
//                //idiscount = (iquotedprice - itaxunitprice) * iquantity;   //原币折扣额 ;=(报价 -原币含税单价)*数量

//                /*VER:V2,11-04*/
//                //金额=单价*数量,保留两位
//                //税额=金额/1.17*0.17 保留2位
//                //无税金额=金额-税额,保留两位
//                //无税单价=无税金额/数量,保留5位
//                //折扣额=报价*数量-金额,保留两位
//                itaxunitprice = list[i].itaxunitprice;//原币含税单价,即执行价格dt.Columns.Add("ExercisePrice"); //基本单位单价(执行价格)   16,保留5位
//                isum = Math.Round(itaxunitprice * iquantity, 2);        //金额,原币价税合计=原币含税单价*数量,保留2位小数,四舍五入
//                itax = Math.Round(isum / 1.17 * 0.17, 2);        //原币税额 ;税额=金额/1.17*0.17 保留2位, 四舍五入
//                imoney = Math.Round(isum - itax, 2);      //原币无税金额 =金额-税额,保留2位,四舍五入
//                iunitprice = Math.Round(imoney / iquantity, 6);  //原币无税单价=无税金额/数量,保留5位小数,四舍五入                 
//                idiscount = Math.Round(iquotedprice * iquantity - isum, 2);   //原币折扣额=报价*数量-金额,保留两位

//                inatunitprice = iunitprice;//本币无税单价
//                inatmoney = imoney;   //本币无税金额
//                inattax = itax;     //本币税额 
//                inatsum = isum;     //本币价税合计
//                inatdiscount = idiscount;   //本币折扣额 

//                itaxrate = list[i].itaxrate;    //税率 
//                cDefine22 = list[i].cDefine22;  //表体自定义项22,包装量

//                cunitid = list[i].cunitid;    //基本计量单位编码
//                cn1cComUnitName = "";    //销售单位名称
//                //插入表体数据
//               // OrderInfo oiEntry = new OrderInfo(lngopOrderId, cinvcode, iquantity, inum, iquotedprice, iunitprice, itaxunitprice, imoney, itax, isum, inatunitprice, inatmoney, inattax, inatsum, kl, itaxrate, cDefine22, iinvexchrate, cunitid, irowno, cinvname, idiscount, inatdiscount, cComUnitName, cInvDefine1, cInvDefine2, cInvDefine13, cInvDefine14, unitGroup, cComUnitQTY, cInvDefine1QTY, cInvDefine2QTY, cn1cComUnitName);
//              //  bool c = new OrderManager().DLproc_NewOrderDetailByIns(oiEntry);

//            }

//            return true;
//            #endregion
//        }
//        #endregion

//        //#region 判断库存
//        //public ReInfo Check_fAvailQtty(List<Buy_list> li, string cCuscode)
//        //{

//        //    ReInfo list=new ReInfo();
//        //    list.messages = new List<string[]>();
//        //    int len=li.Count;
//        //    List<double[]> Search_list = new List<double[]>();
//        //   for (int i = 0; i < len; i++)
//        //    {
//        //        DataTable dt = new DataTable();
//        //      dt= order.DLproc_QuasiOrderDetailBySel(li[i].cinvcode, cCuscode);
//        //      double fAvailQtty = Convert.ToDouble(dt.Rows[0]["fAvailQtty"].ToString());
//        //      if (fAvailQtty == 0 || fAvailQtty < li[i].iquantity)
//        //      {
//        //          list.flag = "1";
//        //          string[] str =new string[]{ li[i].cinvcode, dt.Rows[0]["fAvailQtty"].ToString() };
//        //          list.messages.Add(str);
//        //      }

//        //    }
//        //   return list;


//        //}
//        //#endregion

//        #region 获取订单初使化时的客户基本信息，如开票单位，信用，车型
//        public void Get_BaseInfo()
//        {
//            ReInfo ri = new ReInfo();
//            //  ri.message = DLproc_getCusCreditInfo().Rows[0]["iCusCreLine"].ToString(); 
//            //ri.datatable = new BasicInfoManager().DL_cdefine3BySel();  
//            //  ri.dt = new SearchManager().DL_ComboCustomerBySel(HttpContext.Current.Session["ConstcCusCode"].ToString() + "%");
//            //ri.list_dt = new List<DataTable>();
//            //ri.list_dt.Add(new BasicInfoManager().DL_cdefine3BySel()); //获取车型
//            //ri.list_dt.Add(new SearchManager().DL_ComboCustomerBySel(HttpContext.Current.Session["ConstcCusCode"].ToString() + "%")); //获取开票单位
//            //ri.list_dt.Add(DLproc_getCusCreditInfo());  //获取信用
//            ri.kpdw_dt = new SearchManager().DL_ComboCustomerBySel(HttpContext.Current.Session["ConstcCusCode"].ToString() + "%");//获取开票单位
//            ri.CusCredit_dt = new OrderManager().DLproc_getCusCreditInfo(HttpContext.Current.Session["ConstcCusCode"].ToString()); //获取开票单位
//            ri.CarType_dt = new BasicInfoManager().DL_cdefine3BySel();//获取车型
//            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));

//        }
//        #endregion

//        #region 判断正式订单库存及信用
//        public ReInfo Check_Buy_list()
//        {
//            List<Buy_list> li = Get_BodyInfo();
//            ReInfo list = new ReInfo();
//            list.messages = new List<string[]>();
//            int len = li.Count;
//            List<int[]> Search_list = new List<int[]>(); //用于存储遍历查询的数据，减少查询次数
//            for (int i = 0; i < len; i++)
//            {
//                DataTable dt = new DataTable();
//                dt = order.DLproc_QuasiOrderDetailBySel(li[i].cinvcode, HttpContext.Current.Session["ConstcCusCode"].ToString());
//                int[] d = new int[]{
//                     Convert.ToInt32(dt.Rows[0]["cInvCode"]),   //产品ID
//                     Convert.ToInt32(dt.Rows[0]["fAvailQtty"]), //产品库存
//                     Convert.ToInt32(dt.Rows[0]["ExercisePrice"]) //产品执行单价
//                };
//                Search_list.Add(d);
//            }

//            #region 第一步，判断库存
//            for (int i = 0; i < len; i++)
//            {
//                if (Search_list[i][1] == 0 || Search_list[i][1] < li[i].iquantity)
//                {
//                    list.flag = "1";
//                    string[] str = new string[] { li[i].cinvcode, Search_list[i][1].ToString() };
//                    list.messages.Add(str);
//                }
//            }
//            #endregion
//            if (list.flag == "1")
//            {
//                return list;
//            }
//            #region 第二步，判断信用
//            else
//            {
//                double d = 0;
//                DataTable dt = new DataTable();

//                dt = new OrderManager().DLproc_getCusCreditInfo(HttpContext.Current.Session["KPDWcCusCode"].ToString());
//                //如果不为现金用户，需要判断购买金额是否大于信用
//                if (dt.Rows[0]["iCusCreLine"].ToString() != "-99999999.000000")
//                {
//                    for (int i = 0; i < len; i++)
//                    {
//                        d = d + li[i].iquantity * Search_list[i][2];
//                    }
//                    if (d > Convert.ToDouble(dt.Rows[0]["iCusCreLine"]))
//                    {
//                        list.flag = "2";

//                    }
//                    else
//                    {
//                        list.flag = "0";
//                    }
//                }



//                return list;
//            }
//            #endregion



//        }
//        #endregion

//        #region 判断修改订单库存及信用
//        public ReInfo Check_Modify_Buy_list(string cCusCode, string strBillNo)
//        {
//            List<Buy_list> li = Get_BodyInfo();
//            ReInfo list = new ReInfo();
//            list.messages = new List<string[]>();
//            int len = li.Count;
//            List<int[]> Search_list = new List<int[]>(); //用于存储遍历查询的数据，减少查询次数
//            for (int i = 0; i < len; i++)
//            {
//                DataTable dt = new DataTable();
//                dt = order.DLproc_QuasiOrderDetailBySel(li[i].cinvcode, HttpContext.Current.Session["ConstcCusCode"].ToString());
//                int[] d = new int[]{
//                     Convert.ToInt32(dt.Rows[0]["cInvCode"]),   //产品ID
//                     Convert.ToInt32(dt.Rows[0]["fAvailQtty"]), //产品库存
//                     Convert.ToInt32(dt.Rows[0]["ExercisePrice"]) //产品执行单价
//                };
//                Search_list.Add(d);
//            }

//            #region 第一步，判断库存
//            for (int i = 0; i < len; i++)
//            {
//                if (Search_list[i][1] == 0 || Search_list[i][1] < li[i].iquantity)
//                {
//                    list.flag = "1";
//                    string[] str = new string[] { li[i].cinvcode, Search_list[i][1].ToString() };
//                    list.messages.Add(str);
//                }
//            }
//            #endregion
//            if (list.flag == "1")
//            {
//                return list;
//            }
//            #region 第二步，判断信用
//            else
//            {
//                double d = 0;
//                DataTable dt = new DataTable();

//                dt = new OrderManager().DLproc_getCusCreditInfoWithBillno(cCusCode, strBillNo);
//                //如果不为现金用户，需要判断购买金额是否大于信用
//                if (dt.Rows[0]["iCusCreLine"].ToString() != "-99999999.000000")
//                {
//                    for (int i = 0; i < len; i++)
//                    {
//                        d = d + li[i].iquantity * Search_list[i][2];
//                    }
//                    if (d > Convert.ToDouble(dt.Rows[0]["iCusCreLine"]))
//                    {
//                        list.flag = "2";

//                    }
//                    else
//                    {
//                        list.flag = "0";
//                    }
//                }



//                return list;
//            }
//            #endregion



//        }
//        #endregion

//        #region 获取订单表体信息
//        public List<Buy_list> Get_BodyInfo()
//        {

//            string list = HttpContext.Current.Request.Form["buy_list"];
//            List<Buy_list> li = JsonConvert.DeserializeObject<List<Buy_list>>(list);
//            return li;
//        }
//        #endregion

//        #region 获取订单表头信息
//        public OrderInfo Get_HeadInfo()
//        {
//            string strBillName = HttpContext.Current.Request.Form["temp_name"];
//            string cdefine3 = HttpContext.Current.Request.Form["cdefine3"];
//            string ccuscode = HttpContext.Current.Request.Form["ccuscode"];
//            string ccusname = HttpContext.Current.Request.Form["ccusname"];
//            string cdefine11 = HttpContext.Current.Request.Form["cdefine11"];
//            string cdefine12 = HttpContext.Current.Request.Form["cdefine12"];
//            string cdiscountname = HttpContext.Current.Request.Form["cdiscountname"];
//            string strreceivingaddress = HttpContext.Current.Request.Form["strreceivingaddress"];
//            string cdefine10 = HttpContext.Current.Request.Form["cdefine10"];
//            string lngopUseraddressId = HttpContext.Current.Request.Form["lngopUseraddressId"];
//            string cdefine1 = HttpContext.Current.Request.Form["cdefine1"];
//            string cdefine13 = HttpContext.Current.Request.Form["cdefine13"];
//            string cdefine2 = HttpContext.Current.Request.Form["cdefine2"];
//            string strdistrict = HttpContext.Current.Request.Form["strdistrict"];
//            string cdefine9 = HttpContext.Current.Request.Form["cdefine9"];
//            string cdefine8 = HttpContext.Current.Request.Form["txtArea"];
//            string strRemarks = HttpContext.Current.Request.Form["strRemarks"];
//            string datDeliveryDate = HttpContext.Current.Request.Form["datDeliveryDate"];
//            string strLoadingWays = HttpContext.Current.Request.Form["strLoadingWays"];
//            string cSCCode = HttpContext.Current.Request.Form["cSCCode"];
//            string cpersoncode = HttpContext.Current.Request.Form["cpersoncode"];
//            string lngopUserId = HttpContext.Current.Session["lngopUserId"].ToString();
//            string datCreateTime = DateTime.Now.ToString();
//            int bytStatus = 1;
//            string cSTCode = "00";     //销售类型编码  
//            string strTxtRelateU8NO = "";
//            string lngopUserExId = HttpContext.Current.Session["lngopUserExId"].ToString();
//            string strAllAcount = HttpContext.Current.Session["strAllAcount"].ToString();
//            OrderInfo oi = new OrderInfo(strBillName, lngopUserId, datCreateTime, bytStatus, strRemarks, ccuscode, cdefine1, cdefine2, cdefine3, cdefine9, cdefine10, cdefine11, cdefine12, cdefine13, ccusname, cpersoncode, cSCCode, datDeliveryDate, strLoadingWays, cSTCode, lngopUseraddressId, strTxtRelateU8NO, cdiscountname, cdefine8, lngopUserExId, strAllAcount);
//            return oi;
//        }
//        #endregion

//        #region 获取修改订单表头信息
//        public OrderInfo Get_ModifyHeadInfo()
//        {
//            string strBillNo = HttpContext.Current.Request.Form["strBillNo"];
//            string cdefine3 = HttpContext.Current.Request.Form["cdefine3"];
//            string ccuscode = HttpContext.Current.Request.Form["ccuscode"];
//            string ccusname = HttpContext.Current.Request.Form["ccusname"];
//            string cdefine11 = HttpContext.Current.Request.Form["cdefine11"];
//            string cdefine12 = HttpContext.Current.Request.Form["cdefine12"];
//            string cdiscountname = HttpContext.Current.Request.Form["cdiscountname"];
//            string strreceivingaddress = HttpContext.Current.Request.Form["strreceivingaddress"];
//            string cdefine10 = HttpContext.Current.Request.Form["cdefine10"];
//            string lngopUseraddressId = HttpContext.Current.Request.Form["lngopUseraddressId"];
//            string cdefine1 = HttpContext.Current.Request.Form["cdefine1"];
//            string cdefine13 = HttpContext.Current.Request.Form["cdefine13"];
//            string cdefine2 = HttpContext.Current.Request.Form["cdefine2"];
//            string cdefine9 = HttpContext.Current.Request.Form["cdefine9"];
//            string cdefine8 = HttpContext.Current.Request.Form["txtArea"];
//            string strRemarks = HttpContext.Current.Request.Form["strRemarks"];
//            string datDeliveryDate = HttpContext.Current.Request.Form["datDeliveryDate"];
//            string strLoadingWays = HttpContext.Current.Request.Form["strLoadingWays"];
//            string cSCCode = HttpContext.Current.Request.Form["cSCCode"];
//            string cpersoncode = HttpContext.Current.Request.Form["cpersoncode"];
//            string lngopUserId = HttpContext.Current.Session["lngopUserId"].ToString();
//            string datCreateTime = DateTime.Now.ToString();
//            int bytStatus = 1;
//            //  OrderInfo oi = new OrderInfo(strBillNo, lngopUserId, datCreateTime, bytStatus, strRemarks, ccuscode, cdefine1, cdefine2, cdefine3, cdefine9, cdefine10, cdefine11, cdefine12, cdefine13, ccusname, cpersoncode, cSCCode, datDeliveryDate, strLoadingWays, lngopUseraddressId);
//            OrderInfo oi = new OrderInfo(strBillNo, lngopUserId, datCreateTime, bytStatus, strRemarks, ccuscode, cdefine1, cdefine2, cdefine3, cdefine9, cdefine10, cdefine11, cdefine12, cdefine13, ccusname, cpersoncode, cSCCode, datDeliveryDate, strLoadingWays, lngopUseraddressId, cdiscountname);
//            return oi;
//        }
//        #endregion


//        #region 保存临时订单
//        public ReInfo DLproc_AddOrderBackByIns()
//        {
//            ReInfo ri = new ReInfo();
//            List<Buy_list> list = Get_BodyInfo();
//            OrderInfo oi = Get_HeadInfo();
//            int lngBillType = 0;
//            string RelateU8NO = "";
//            oi.Cdefine1 = oi.Cdefine1 != null ? oi.Cdefine1 : "";
//            oi.Cdefine2 = oi.Cdefine2 != null ? oi.Cdefine2 : "";
//            oi.Cdefine3 = oi.Cdefine3 != null ? oi.Cdefine3 : "";
//            oi.Cdefine9 = oi.Cdefine9 != null ? oi.Cdefine9 : "";
//            oi.Cdefine10 = oi.Cdefine10 != null ? oi.Cdefine10 : "";
//            oi.Cdefine11 = oi.Cdefine11 != null ? oi.Cdefine11 : "";
//            oi.Cdefine12 = oi.Cdefine12 != null ? oi.Cdefine12 : "";
//            oi.Cdefine13 = oi.Cdefine13 != null ? oi.Cdefine13 : "";
//            oi.Cpersoncode = oi.Cpersoncode != null ? oi.Cpersoncode : "";
//            oi.LngopUseraddressId = oi.LngopUseraddressId != null ? oi.LngopUseraddressId : "";
//            DataTable dt = new DataTable();
//            dt = new OrderManager().DLproc_AddOrderBackByIns(HttpContext.Current.Session["lngopUserId"].ToString(), oi.StrBillName, oi.BytStatus, oi.StrRemarks, HttpContext.Current.Session["KPDWcCusCode"].ToString(),
//                                                                      oi.Cdefine1, oi.Cdefine2, oi.Cdefine3, oi.Cdefine9, oi.Cdefine10, oi.Cdefine11, oi.Cdefine12, oi.Cdefine13, oi.Ccusname, oi.Cpersoncode, oi.CSCCode,
//                                                                      oi.StrLoadingWays, oi.CSTCode, oi.LngopUseraddressId, RelateU8NO, lngBillType);
//            if (dt.Rows.Count < 1)
//            {
//                ri.flag = "1";
//                ri.message = "保存临时订单失败！";
//                return ri;
//            }
//            if (list.Count > 1)
//            {
//                int len = list.Count;
//                Int32 lngopOrderBackId = Convert.ToInt32(dt.Rows[0]["lngopOrderBackId"].ToString());
//                for (int i = 0; i < len; i++)
//                {
//                    new OrderManager().DLproc_AddOrderBackDetailByIns(lngopOrderBackId, list[i].cinvcode, list[i].cinvname, list[i].cComUnitQTY, list[i].cInvDefine1QTY, list[i].cInvDefine2QTY,
//                                                                                 list[i].iquantity, list[i].iquantity * list[i].iquotedprice, list[i].cDefine22, list[i].irowno.ToString());
//                }
//            }

//            ri.flag = "0";
//            ri.message = "保存临时订单成功！";
//            return ri;

//        }
//        #endregion


//        #region 前台更换开票单位后，重新获取数据
//        public void Change_KPDW()
//        {
//            ReInfo ri = new ReInfo();
//            string kpdw = HttpContext.Current.Request.Form["kpdw"];
//            string c = HttpContext.Current.Request.Form["codes"];
//            ri.list_dt = new List<DataTable>();
//            ri.list_msg = new List<string>();
//            ri.CusCredit_dt = order.DLproc_getCusCreditInfo(kpdw);

//            if (!string.IsNullOrEmpty(c))
//            {
//                ri.messages = new List<string[]>();
//                string[] code = c.Split(',');
//                for (int i = 0; i < code.Length; i++)
//                {

//                    if (!pro.DL_cInvCodeIsPLPBySel(code[i], kpdw,1))
//                    {
//                        ri.list_msg.Add(code[i]);
//                    }
//                    else
//                    {
//                        ri.list_dt.Add(order.DLproc_QuasiOrderDetailBySel(code[i], kpdw));
//                    }
//                }
//            }
//            //        DataTable dt = order.DLproc_QuasiOrderDetailBySel(code[i], kpdw);
//            //        string[] str = new string[]{
//            //                        code[i],
//            //                       Convert.ToDouble(dt.Rows[0]["fAvailQtty"].ToString()).ToString(),
//            //                        Convert.ToDouble(dt.Rows[0]["Quote"].ToString()).ToString(),
//            //                          Convert.ToDouble(dt.Rows[0]["ExercisePrice"].ToString()).ToString()
//            //                 };
//            //        ri.messages.Add(str);
//            //    }

//            //}

//            //ri.msg = new string[] { 
//            //    TxtCusCredit,
//            //    cdiscountname,
//            //    html.ToString()
//            //};



//            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
//        }

//        #endregion



//        #region 删除临时订单
//        public ReInfo DL_DelOrderBackDetailByDel()
//        {
//            ReInfo ri = new ReInfo();
//            int lngopOrderBackId = Convert.ToInt32(HttpContext.Current.Request.Form["lngopOrderBackId"].ToString());
//            bool c = new OrderManager().DL_DelOrderBackDetailByDel(lngopOrderBackId);
//            if (c)
//            {
//                ri.flag = "0";
//                ri.message = "删除成功！";

//            }
//            else
//            {
//                ri.flag = "1";
//                ri.message = "删除失败，请重试或联系管理员！";
//            }
//            return ri;
//        }
//        #endregion

//        #region 提取临时订单表头+表体明细
//        public ReInfo Get_BackOrder()
//        {
//            ReInfo ri = new ReInfo();
//            int lngopOrderBackId = Convert.ToInt32(HttpContext.Current.Request.Form["lngopOrderBackId"].ToString());
//            DataTable dt = new OrderManager().DLproc_ReferenceOrderBackWithCusInvLimitedBySel(lngopOrderBackId);
//            ri.flag = "0";
//            ri.message = Get_BackOrder_Body_Html(dt);
//            ri.msg = new string[6]{
//                dt.Rows[0]["strRemarks"].ToString(),//1、备注
//                dt.Rows[0]["cSCCode"].ToString(),     //2、送货方式
//                dt.Rows[0]["strLoadingWays"].ToString(),     //3、装车方式
//                dt.Rows[0]["cdefine3"].ToString(),     //4、车型下拉
//                dt.Rows[0]["lngopUseraddressId"].ToString(),     //5、送货方式
//                dt.Rows[0]["ccuscode"].ToString()     //6、开票单位
//          };
//            return ri;
//        }
//        #endregion

//        #region 拼接临时订单表体HTML
//        public string Get_BackOrder_Body_Html(DataTable OrderBack)
//        {

//            int len = OrderBack.Rows.Count;
//            StringBuilder sb = new StringBuilder();
//            for (int i = 0; i < OrderBack.Rows.Count; i++)
//            {
//                if (OrderBack.Rows[i]["bLimited"].ToString() != "1")
//                {

//                    DataTable dt = new OrderManager().DLproc_QuasiOrderDetailBySel(OrderBack.Rows[i]["cinvcode"].ToString(), HttpContext.Current.Session["KPDWcCusCode"].ToString());
//                    if (dt.Rows.Count == 3)
//                    {
//                        sb.Append(@"<tr><td class='ui-widget-content'><a href='javascript:void(0)'  class='del_this'>删除</a>
//                                <a href='javascript:void(0)'  class='up_this'>上移</a>  <a href='javascript:void(0)'  class='down_this'>下移</a></td>");
//                        string unit_s = Convert.ToDouble(dt.Rows[0]["iChangRate"].ToString()).ToString();
//                        string unit_m = Convert.ToDouble(dt.Rows[1]["iChangRate"].ToString()).ToString();
//                        string unit_b = Convert.ToDouble(dt.Rows[2]["iChangRate"].ToString()).ToString();
//                        sb.Append("<td code='" + dt.Rows[0]["cInvCode"] + "' unit_s='" + unit_s + "' unit_m='" + unit_m + "' unit_b='" + unit_b + "'>" + OrderBack.Rows[i]["irowno"].ToString() + "</td>");
//                        sb.Append("<td   kl='" + dt.Rows[0]["Rate"].ToString() + "' cInvDefine13='" + dt.Rows[1]["iChangRate"].ToString() +
//                                    "' cInvDefine14='" + dt.Rows[2]["iChangRate"].ToString()
//                           + "' iTaxRate='" + dt.Rows[0]["itaxrate"].ToString() + "' cunitid='" + dt.Rows[0]["cComUnitCode"].ToString() + "'>"
//                                + dt.Rows[0]["cInvName"].ToString() + "</td>");
//                        sb.Append("<td>" + dt.Rows[0]["cInvStd"].ToString() + "</td>");
//                        sb.Append("<td>" + unit_b + dt.Rows[0]["cComUnitName"].ToString() + "=" + (Convert.ToDouble(unit_b) / Convert.ToDouble(unit_m)).ToString() + dt.Rows[1]["cComUnitName"].ToString()
//                            + "=" + unit_s + dt.Rows[2]["cComUnitName"].ToString() + "</td>");
//                        sb.Append("<td class='in'>" + Convert.ToDouble(OrderBack.Rows[i]["cComUnitQTY"].ToString()).ToString() + "</td>");
//                        sb.Append("<td>" + dt.Rows[0]["cComUnitName"].ToString() + "</td>");
//                        sb.Append("<td class='in'>" + Convert.ToDouble(OrderBack.Rows[i]["cInvDefine2QTY"].ToString()).ToString() + "</td>");
//                        sb.Append("<td>" + dt.Rows[1]["cComUnitName"].ToString() + "</td>");
//                        sb.Append("<td class='in'>" + Convert.ToDouble(OrderBack.Rows[i]["cInvDefine1QTY"].ToString()).ToString() + "</td>");
//                        sb.Append("<td>" + dt.Rows[2]["cComUnitName"].ToString() + "</td>");
//                        sb.Append("<td>" + OrderBack.Rows[i]["cInvDefineQTY"].ToString() + "</td>");
//                        sb.Append("<td>" + Convert.ToDouble(dt.Rows[0]["fAvailQtty"].ToString()).ToString() + "</td>");
//                        sb.Append("<td>" + OrderBack.Rows[i]["pack"].ToString() + "</td>");
//                        sb.Append("<td>" + dt.Rows[0]["Quote"].ToString() + "</td>");
//                        sb.Append("<td>" + Convert.ToDouble(dt.Rows[0]["Quote"].ToString()) * Convert.ToDouble(OrderBack.Rows[i]["cInvDefineQTY"].ToString()) + "</td>");
//                        sb.Append("<td style='display:none'>" + dt.Rows[0]["ExercisePrice"].ToString() + "</td>");
//                        sb.Append("<td style='display:none'></td></tr>");
//                    }
//                    else if (dt.Rows.Count == 2)
//                    {
//                        sb.Append(@"<tr><td class='ui-widget-content'><a href='javascript:void(0)'  class='del_this'>删除</a>
//                                <a href='javascript:void(0)'  class='up_this'>上移</a>  <a href='javascript:void(0)'  class='down_this'>下移</a></td>");
//                        string unit_s = Convert.ToDouble(dt.Rows[0]["iChangRate"].ToString()).ToString();
//                        string unit_m = Convert.ToDouble(dt.Rows[1]["iChangRate"].ToString()).ToString();
//                        string unit_b = Convert.ToDouble(dt.Rows[1]["iChangRate"].ToString()).ToString();
//                        sb.Append("<td code='" + dt.Rows[0]["cInvCode"] + "' unit_s='" + unit_s + "' unit_m='" + unit_m + "' unit_b='" + unit_b + "'>" + OrderBack.Rows[i]["irowno"].ToString() + "</td>");
//                        sb.Append("<td   kl='" + dt.Rows[0]["Rate"].ToString() + "' cInvDefine13='" + dt.Rows[1]["iChangRate"].ToString() +
//                                 "' cInvDefine14='" + dt.Rows[1]["iChangRate"].ToString()
//                         + "' iTaxRate='" + dt.Rows[0]["itaxrate"].ToString() + "' cunitid='" + dt.Rows[0]["cComUnitCode"].ToString() + "'>"
//                         + dt.Rows[0]["cInvName"].ToString() + "</td>");
//                        sb.Append("<td>" + dt.Rows[0]["cInvStd"].ToString() + "</td>");
//                        sb.Append("<td>" + unit_b + dt.Rows[0]["cComUnitName"].ToString() + "=" + unit_s + dt.Rows[1]["cComUnitName"].ToString()
//                            + "=" + unit_s + dt.Rows[1]["cComUnitName"].ToString() + "</td>");
//                        sb.Append("<td class='in'>" + Convert.ToDouble(OrderBack.Rows[i]["cComUnitQTY"].ToString()).ToString() + "</td>");
//                        sb.Append("<td>" + dt.Rows[0]["cComUnitName"].ToString() + "</td>");
//                        sb.Append("<td class='in'>" + Convert.ToDouble(OrderBack.Rows[i]["cInvDefine2QTY"].ToString()).ToString() + "</td>");
//                        sb.Append("<td>" + dt.Rows[1]["cComUnitName"].ToString() + "</td>");
//                        sb.Append("<td class='in'>" + Convert.ToDouble(OrderBack.Rows[i]["cInvDefine1QTY"].ToString()).ToString() + "</td>");
//                        sb.Append("<td>" + dt.Rows[1]["cComUnitName"].ToString() + "</td>");
//                        sb.Append("<td>" + OrderBack.Rows[i]["cInvDefineQTY"].ToString() + "</td>");
//                        sb.Append("<td>" + Convert.ToDouble(dt.Rows[0]["fAvailQtty"].ToString()).ToString() + "</td>");
//                        sb.Append("<td>" + OrderBack.Rows[i]["pack"].ToString() + "</td>");
//                        sb.Append("<td>" + dt.Rows[0]["Quote"].ToString() + "</td>");
//                        sb.Append("<td>" + Convert.ToDouble(dt.Rows[0]["Quote"].ToString()) * Convert.ToDouble(OrderBack.Rows[i]["cInvDefineQTY"].ToString()) + "</td>");
//                        sb.Append("<td style='display:none'>" + dt.Rows[0]["ExercisePrice"].ToString() + "</td>");
//                        sb.Append("<td style='display:none'></td></tr>");
//                    }
//                    else if (dt.Rows.Count == 1)
//                    {
//                        sb.Append(@"<tr><td class='ui-widget-content'><a href='javascript:void(0)'  class='del_this'>删除</a>
//                                <a href='javascript:void(0)'  class='up_this'>上移</a>  <a href='javascript:void(0)'  class='down_this'>下移</a></td>");
//                        string unit_s = Convert.ToDouble(dt.Rows[0]["iChangRate"].ToString()).ToString();
//                        string unit_m = Convert.ToDouble(dt.Rows[0]["iChangRate"].ToString()).ToString();
//                        string unit_b = Convert.ToDouble(dt.Rows[0]["iChangRate"].ToString()).ToString();
//                        sb.Append("<td code='" + dt.Rows[0]["cInvCode"] + "' unit_s='" + unit_s + "' unit_m='" + unit_m + "' unit_b='" + unit_b + "'>" + OrderBack.Rows[i]["irowno"].ToString() + "</td>");
//                        sb.Append("<td   kl='" + dt.Rows[0]["Rate"].ToString() + "' cInvDefine13='" + dt.Rows[0]["iChangRate"].ToString() +
//                                 "' cInvDefine14='" + dt.Rows[0]["iChangRate"].ToString()
//                          + "' iTaxRate='" + dt.Rows[0]["itaxrate"].ToString() + "' cunitid='" + dt.Rows[0]["cComUnitCode"].ToString() + "'>"
//                         + dt.Rows[0]["cInvName"].ToString() + "</td>");
//                        sb.Append("<td>" + dt.Rows[0]["cInvStd"].ToString() + "</td>");
//                        sb.Append("<td>" + unit_b + dt.Rows[0]["cComUnitName"].ToString() + "=" + unit_s + dt.Rows[0]["cComUnitName"].ToString()
//                            + "=" + unit_s + dt.Rows[0]["cComUnitName"].ToString() + "</td>");
//                        sb.Append("<td class='in'>" + Convert.ToDouble(OrderBack.Rows[i]["cComUnitQTY"].ToString()).ToString() + "</td>");
//                        sb.Append("<td>" + dt.Rows[0]["cComUnitName"].ToString() + "</td>");
//                        sb.Append("<td class='in'>" + Convert.ToDouble(OrderBack.Rows[i]["cInvDefine2QTY"].ToString()).ToString() + "</td>");
//                        sb.Append("<td>" + dt.Rows[0]["cComUnitName"].ToString() + "</td>");
//                        sb.Append("<td class='in'>" + Convert.ToDouble(OrderBack.Rows[i]["cInvDefine1QTY"].ToString()).ToString() + "</td>");
//                        sb.Append("<td>" + dt.Rows[0]["cComUnitName"].ToString() + "</td>");
//                        sb.Append("<td>" + OrderBack.Rows[i]["cInvDefineQTY"].ToString() + "</td>");
//                        sb.Append("<td>" + Convert.ToDouble(dt.Rows[0]["fAvailQtty"].ToString()).ToString() + "</td>");
//                        sb.Append("<td>" + OrderBack.Rows[i]["pack"].ToString() + "</td>");
//                        sb.Append("<td>" + dt.Rows[0]["Quote"].ToString() + "</td>");
//                        sb.Append("<td>" + Convert.ToDouble(dt.Rows[0]["Quote"].ToString()) * Convert.ToDouble(OrderBack.Rows[i]["cInvDefineQTY"].ToString()) + "</td>");
//                        sb.Append("<td style='display:none'>" + dt.Rows[0]["ExercisePrice"].ToString() + "</td>");
//                        sb.Append("<td style='display:none'></td></tr>");
//                    }
//                }
//            }
//            return sb.ToString();
//        }
//        #endregion

//        #region 提取历史订单列表
//        public void DL_GeneralPreviousOrderBySel()
//        {

//            DataTable dt = pro.DL_GeneralPreviousOrderBySel(HttpContext.Current.Session["ConstcCusCode"].ToString() + "%");
//            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(dt));
//            return;
//        }

//        #endregion

//        #region 获取历史订单表头+表体
//        public void DLproc_ReferencePreviousOrderWithCusInvLimitedBySel()
//        {
//            DataTable dt = new DataTable();
//            dt = new OrderManager().DLproc_ReferencePreviousOrderWithCusInvLimitedBySel(HttpContext.Current.Request.Form["strBillNo"].ToString());
//            ReInfo ri = new ReInfo();
//            ri.flag = "0";
//            string kpdw = HttpContext.Current.Request.Form["kpdw"];

//            ri.list_dt = new List<DataTable>();
//            ri.list_msg = new List<string>();

//            ri.msg = new string[5]{
//                dt.Rows[0]["strRemarks"].ToString(),       //1、备注
//                dt.Rows[0]["cSCCode"].ToString(),          //2、送货方式
//                dt.Rows[0]["strLoadingWays"].ToString(),   //3、装车方式
//                dt.Rows[0]["cdefine3"].ToString(),        //4、车型下拉
//                dt.Rows[0]["lngopUseraddressId"].ToString(), //5、送货地址
//          };
//            List<string> codes = new List<string>();
//            if (dt.Rows.Count > 0)
//            {

//                ri.messages = new List<string[]>();
//                for (int i = 0; i < dt.Rows.Count; i++)
//                {

//                    if (!pro.DL_cInvCodeIsPLPBySel(dt.Rows[i]["cinvcode"].ToString(), kpdw,1))
//                    {
//                        ri.list_msg.Add(dt.Rows[i]["cinvname"].ToString());
//                    }
//                    else
//                    {
//                        codes.Add(dt.Rows[i]["cinvcode"].ToString());

//                    }
//                }
//            }

//            ri.dt = DLproc_QuasiOrderDetailBySel_new(codes.ToArray(), kpdw);
//            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
//            return;
//        }
//        #endregion

//        #region 获取被驳回订单列表
//        DataTable Get_RejectOrder()
//        {
//            DataTable dt = new OrderManager().DL_UnauditedOrder_SubBySel(3, HttpContext.Current.Session["lngopUserId"].ToString(), HttpContext.Current.Session["lngopUserExId"].ToString());
//            return dt;
//        }
//        #endregion

//        #region 获取被驳回订单详细
//        DataTable Get_RejectOrder_Detail(string strBillNo)
//        {
//            DataTable dt = new OrderManager().DL_OrderBillBySel(strBillNo);
//            return dt;
//        }
//        #endregion

//        #region 作废订单
//        ReInfo Cancel_Order()
//        {
//            string strBillNo = HttpContext.Current.Request.Form["strBillNo"];
//            ReInfo ri = new ReInfo();
//            bool c = new OrderManager().DL_InvalidOrderByUpd(strBillNo, HttpContext.Current.Session["lngopUserId"].ToString());
//            if (c)
//            {
//                ri.flag = "1";
//            }
//            else
//            {
//                ri.flag = "0";
//            }
//            return ri;
//        }
//        #endregion

//        #region 将要编辑的订单号写入session
//        protected void Write_ModifyOrder_strBillNo()
//        {
//            HttpContext.Current.Session["ModifyOrder_strBillNo"] = HttpContext.Current.Request.Form["strBillNo"];
//        }
//        #endregion

//        #region 修改订单界面获取订单详情
//        ReInfo DLproc_OrderDetailModifyBySel()
//        {
//            ReInfo ri = new ReInfo();
//            ri.messages = new List<string[]>();
//            List<string> li = new List<string>();
//            DataTable dt = new OrderManager().DLproc_OrderDetailModifyBySel(HttpContext.Current.Request.Form["strBillNo"].ToString());
//            DataTable limitDt = new OrderManager().DLproc_ReferencePreviousOrderWithCusInvLimitedBySel(HttpContext.Current.Request.Form["strBillNo"].ToString());
//            for (int i = 0; i < dt.Rows.Count; i++)
//            {
//                if (dt.Rows[i]["cInvCode"].ToString() == limitDt.Rows[i]["cinvcode"].ToString())
//                {
//                    if (limitDt.Rows[i]["bLimited"].ToString() == "0" || limitDt.Rows[i]["bLimited"].ToString() == "False")
//                    {
//                        li.Add(limitDt.Rows[i]["cInvName"].ToString());

//                        dt.Rows[i].Delete();
//                    }
//                }

//            }
//            ri.msg = li.ToArray();
//            dt.AcceptChanges();
//            ri.datatable = dt;
//            DataTable dt1 = new OrderManager().DL_OrderModifyBySel(HttpContext.Current.Request.Form["strBillNo"].ToString());
//            ri.dt = dt1;
//            //拼接信用
//            ri.message = DLproc_getCusCreditInfo().Rows[0]["iCusCreLine"].ToString();
//            return ri;
//        }
//        #endregion

//        #region 刷新库存及执行价格
//        ReInfo Refresh_fAvailQtty()
//        {
//            DataTable dt = new DataTable();
//            ReInfo ri = new ReInfo();
//            ri.messages = new List<string[]>();
//            string code = HttpContext.Current.Request.Form["codes"];
//            if (!string.IsNullOrEmpty(code))
//            {
//                string[] codes = code.Split(',');
//                foreach (var c in codes)
//                {
//                    dt = new OrderManager().DLproc_QuasiOrderDetailBySel(c, HttpContext.Current.Request.Form["cCusCode"]);
//                    string[] s = new string[]{
//                    c,
//                    dt.Rows[0]["fAvailQtty"].ToString(),
//                     dt.Rows[0]["ExercisePrice"].ToString()
//                    };
//                    ri.messages.Add(s);
//                }
//            }

//            return ri;
//        }
//        #endregion

//        #region 提交修改后的普通订单
//        ReInfo DLproc_NewOrderByUpd()
//        {

//            #region 2016-09-13,检查是否存在未参照完的酬宾订单的商品
//            //string Strpreorderleft = "行：";
//            //if (griddata.Rows.Count > 0)
//            //{
//            //    for (int i = 0; i < griddata.Rows.Count; i++)
//            //    {
//            //        DataTable preorderleft = new OrderManager().DLproc_PerOrderCinvcodeLeftBySel(Session["lngopUserExId"].ToString(), Session["lngopUserId"].ToString(), Session["KPDWcCusCode"].ToString(), griddata.Rows[i][0].ToString());
//            //        if (Convert.ToDouble(preorderleft.Rows[0][0].ToString()) > 0)
//            //        {
//            //            Strpreorderleft = Strpreorderleft + griddata.Rows[i]["irowno"].ToString() + "," + griddata.Rows[i]["cInvName"].ToString() + "数量：" + preorderleft.Rows[0][0].ToString() + ";";
//            //        }
//            //    }
//            //}
//            ////提示有未完成的酬宾订单
//            //if (Strpreorderleft.ToString() != "行：")
//            //{
//            //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('" + Strpreorderleft.ToString() + "存在未参照完的酬宾订单，请先将酬宾订单的商品参照完毕后在普通订单中购买该商品！" + "');</script>");
//            //    return;
//            //}
//            #endregion
//            ReInfo ri = new ReInfo();
//            ReInfo re = new ReInfo();
//            OrderInfo oi = Get_ModifyHeadInfo(); //获取表头信息
//            re = Check_Modify_Buy_list(oi.Ccuscode, oi.StrBillNo); //检测库存及信用额，flag为0才继续
//            if (re.flag != "0")
//            {
//                return re;
//            }

//            DataTable lngopOrderIdDt = new DataTable();
//            lngopOrderIdDt = new OrderManager().DLproc_NewOrderByUpd(oi);  //更新数据库表头数据
//            if (lngopOrderIdDt.Rows.Count == 0)
//            {
//                ri.flag = "1";
//                ri.message = "保存订单失败，请重试或联系管理员！";
//                return ri;
//            }
//            int lngopOrderId = Convert.ToInt32(lngopOrderIdDt.Rows[0]["lngopOrderId"].ToString());
//            string strBillNo = oi.StrBillNo;
//            bool b = new OrderManager().DL_NewOrderDetailByDel(lngopOrderId);
//            if (!b)
//            {
//                ri.flag = "1";
//                ri.message = "保存订单失败，请重试或联系管理员！";
//                return ri;
//            }


//            string[] back = new string[]{
//            lngopOrderId.ToString(),
//            strBillNo
//            };



//            b = DLproc_NewOrderDetailByIns(back); //插入表体数据
//            if (b)
//            {
//                ri.flag = "0";
//                ri.message = strBillNo;
//            }
//            return ri;
//        }
//        #endregion

//        #region 更新样品订单
//        ReInfo DLproc_SampleOrderByUpd()
//        {
//            ReInfo ri = new ReInfo();
//            //获取表头信息
//            OrderInfo oi = new OrderInfo(
//                HttpContext.Current.Request.Form["strBillNo"],
//                HttpContext.Current.Request.Form["strRemarks"],
//                HttpContext.Current.Request.Form["strLoadingWays"],
//                1
//                );
//            DataTable lngopOrderIdDt = new DataTable();
//            lngopOrderIdDt = new OrderManager().DLproc_SampleOrderByUpd(oi);  //更新数据库表头数据
//            if (lngopOrderIdDt.Rows.Count == 0)
//            {
//                ri.flag = "1";
//                ri.message = "保存订单失败，请重试或联系管理员！";
//                return ri;
//            }
//            int lngopOrderId = Convert.ToInt32(lngopOrderIdDt.Rows[0]["lngopOrderId"].ToString());
//            string strBillNo = oi.StrBillNo;

//            bool b = new OrderManager().DL_NewOrderDetailByDel(lngopOrderId); //删除订单老数据

//            //if (!b)
//            //{
//            //    ri.flag = "1";
//            //    ri.message = "保存订单失败，请重试或联系管理员！";
//            //    return ri;
//            //}


//            string[] back = new string[]{
//            lngopOrderId.ToString(),
//            strBillNo
//            };

//            b = DLproc_NewOrderDetailByIns(back); //插入表体数据
//            if (b)
//            {
//                ri.flag = "0";
//                ri.message = strBillNo;
//            }
//            return ri;
//        }
//        #endregion

//        #region 获取新增样品订单关联的主订单表头信息
//        DataTable Get_SampleOrder_Title()
//        {
//            DataTable dt = new OrderManager().DL_OrderModifyBySel(HttpContext.Current.Request.Form["strBillNo"]);
//            return dt;
//        }
//        #endregion

//        #region　插入样品订单
//        ReInfo Insert_SampleOrder()
//        {
//            ReInfo ri = new ReInfo();

//            string strBillNo = HttpContext.Current.Request.Form["strBillNo"];
//            string strRemarks = HttpContext.Current.Request.Form["strRemarks"];
//            string strLoadingWays = HttpContext.Current.Request.Form["strLoadingWays"];
//            string lngopUserExId = HttpContext.Current.Session["lngopUserExId"].ToString();
//            string strAllAcount = HttpContext.Current.Session["strAllAcount"].ToString();
//            //插入表头数据
//            DataTable dt = new OrderManager().DLproc_NewSampleOrderByIns(strBillNo, strRemarks, strLoadingWays, lngopUserExId, strAllAcount);
//            if (dt.Rows.Count != 1)
//            {
//                ri.flag = "0";
//                ri.message = "提交样品订单失败，请重试或联系管理员！";
//            }

//            //插入表体数据
//            int lngopOrderId = Convert.ToInt32(dt.Rows[0]["lngopOrderId"].ToString());
//            strBillNo = dt.Rows[0]["strBillNo"].ToString();
//            string[] back = new string[]{
//            lngopOrderId.ToString(),
//            strBillNo
//            };

//            bool b = DLproc_NewOrderDetailByIns(back); //插入表体数据
//            if (b)
//            {
//                ri.flag = "0";
//                ri.message = strBillNo;
//            }
//            else
//            {
//                ri.flag = "1";
//                ri.message = "提交样品订单失败，请重试或联系管理员！";
//            }
//            return ri;

//        }

//        #endregion

//        #region 获取信用
//        DataTable DLproc_getCusCreditInfo()
//        {
//            DataTable dt = new OrderManager().DLproc_getCusCreditInfo(HttpContext.Current.Session["ConstcCusCode"].ToString());//默认登录客户编码
//            return dt;
//        }
//        #endregion

//        #region 获取信用
//        DataTable DLproc_getCusCreditInfo(string kpdw)
//        {
//            DataTable dt = new OrderManager().DLproc_getCusCreditInfo(kpdw);//传入的开票单位
//            return dt;
//        }
//        #endregion

//        #region 获取开票单位
//        DataTable Get_KPDW()
//        {
//            DataTable dt = new SearchManager().DL_ComboCustomerAllBySel(HttpContext.Current.Session["ConstcCusCode"].ToString() + "%");

//            return dt;

//        }
//        #endregion

//        #region 获取用户账单列表
//        ReInfo DLproc_U8SOASearchBySel()
//        {
//            ReInfo ri = new ReInfo();
//            DataTable dt = new SearchManager().DLproc_U8SOASearchBySel(
//                HttpContext.Current.Request.Form["year"],
//                HttpContext.Current.Request.Form["kpdw"],
//                HttpContext.Current.Request.Form["month"],
//                HttpContext.Current.Request.Form["check"],
//                HttpContext.Current.Session["ConstcCusCode"].ToString()

//                );
//            ri.dt = dt;
//            return ri;
//        }
//        #endregion

//        #region 确认账单
//        ReInfo DL_ConfimSOAByUpd()
//        {
//            ReInfo ri = new ReInfo();
//            bool b = new OrderManager().DL_ConfimSOAByUpd(HttpContext.Current.Request.Form["id"]);
//            if (b)
//            {
//                ri.flag = "0";
//                ri.message = "账单确认成功，感谢使用！";
//            }
//            else
//            {
//                ri.flag = "1";
//                ri.message = "账单确认失败，请联系系统管理员！";
//            }
//            return ri;
//        }
//        #endregion

//        #region 获取账单明细
//        ReInfo DLproc_SOADetailforCustomerBySel()
//        {
//            ReInfo ri = new ReInfo();
//            string kpdw = HttpContext.Current.Request.Form["kpdw"];
//            if (string.IsNullOrEmpty(kpdw))
//            {
//                ri.flag = "0";
//                ri.message = "单位不能为空！";
//                return ri;
//            }
//            string start_time = HttpContext.Current.Request.Form["start_date"];
//            string end_time = HttpContext.Current.Request.Form["end_date"];
//            if (string.IsNullOrEmpty(start_time))
//            {
//                start_time = "2015-12-01";
//            }
//            if (string.IsNullOrEmpty(end_time))
//            {
//                end_time = "2099-01-01";
//            }
//            if (Convert.ToDateTime(start_time) > Convert.ToDateTime(end_time))
//            {
//                ri.flag = "0";
//                ri.message = "结束时间不能大于开始时间！";
//                return ri;
//            }
//            ri.flag = "1";
//            ri.dt = new SearchManager().DLproc_SOADetailforCustomerBySel(start_time, end_time, kpdw);
//            return ri;

//        }
//        #endregion

//        #region 查询订单执行情况
//        ReInfo DLproc_OrderExecuteBySel()
//        {
//            ReInfo ri = new ReInfo();
//            string timetype = HttpContext.Current.Request.Form["timetype"];
//            if (string.IsNullOrEmpty(timetype))
//            {
//                ri.flag = "0";
//                ri.message = "查询类别不合法！";
//                return ri;
//            }
//            string start_time = HttpContext.Current.Request.Form["start_date"];
//            string end_time = HttpContext.Current.Request.Form["end_date"];
//            if (string.IsNullOrEmpty(start_time))
//            {
//                start_time = "2015-12-01";
//            }
//            if (string.IsNullOrEmpty(end_time))
//            {
//                end_time = "2099-01-01";
//            }
//            if (Convert.ToDateTime(start_time) > Convert.ToDateTime(end_time))
//            {
//                ri.flag = "0";
//                ri.message = "结束时间不能大于开始时间！";
//                return ri;
//            }
//            string strbillno = HttpContext.Current.Request.Form["strbillno"];
//            string kpdw = HttpContext.Current.Request.Form["kpdw"];
//            if (kpdw == "0")
//            {
//                kpdw = HttpContext.Current.Session["ConstcCusCode"].ToString() + "%";
//            }
//            string showtype = HttpContext.Current.Request.Form["showtype"];
//            string fhtype = HttpContext.Current.Request.Form["fhtype"];
//            if (timetype == "0") //下单时间查询
//            {
//                DataTable dt = new SearchManager().DLproc_OrderExecuteBySel(strbillno, kpdw, start_time, end_time, showtype, fhtype);
//                ri.flag = "1";
//                ri.dt = dt;

//            }
//            else if (timetype == "1")  //审核时间查询
//            {
//                DataTable dt = new SearchManager().DLproc_OrderExecuteFordatAuditordTimeBySel(strbillno, kpdw, start_time, end_time, showtype, fhtype);
//                ri.flag = "1";
//                ri.dt = dt;

//            }
//            return ri;
//        }
//        #endregion

//        #region 获取地址下拉菜单信息
//        public void DLproc_UserAddressZTBySelGroup()
//        {
//            ReInfo ri = new ReInfo();
//            ri.list_dt = new List<DataTable>();
//            string shfs = HttpContext.Current.Request.Form["shfs"];
//            if (shfs == "自提")
//            {
//                ri.list_dt.Add(new BasicInfoManager().DLproc_UserAddressZTBySelGroup(HttpContext.Current.Session["lngopUserId"].ToString()));
//            }
//            else
//            {
//                ri.list_dt.Add(new BasicInfoManager().DLproc_UserAddressPSBySelGroup(HttpContext.Current.Session["lngopUserId"].ToString()));
//            }
//            ri.list_dt.Add(new BasicInfoManager().DL_UserAddressZTXZQBySel(HttpContext.Current.Session["ConstcCusCode"].ToString()));
//            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
//        }
//        #endregion

//        #region 返回产品详细列表，新
//        public void DLproc_QuasiOrderDetailBySel_new()
//        {
//            ReInfo ri = new ReInfo();
//            string code = HttpContext.Current.Request.Form["codes"];
//            DataTable dt = new DataTable();
//            DataTable new_dt = new DataTable();
//            string kpdw = HttpContext.Current.Request.Form["kpdw"];

//            if (string.IsNullOrEmpty(kpdw) || kpdw == "0")
//            {
//                ri.flag = "0";
//                ri.message = "请先选择开票单位!";
//                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
//                return;
//            }
//            if (!string.IsNullOrEmpty(code)) //判断数组是否为空
//            {
//                string[] codes = code.Split(',');

//                for (int i = 0; i < codes.Length; i++)
//                {
//                    dt = new OrderManager().DLproc_QuasiOrderDetailBySel(codes[i], kpdw);
//                    if (i == 0)
//                    {
//                        new_dt = dt.Clone();
//                        new_dt.Columns.Add("cinvdefine13", typeof(float)); //大包装换算
//                        new_dt.Columns.Add("cinvdefine14", typeof(float));//小包装换算
//                        new_dt.Columns.Add("cInvDefine1", typeof(string));//大包装单位
//                        new_dt.Columns.Add("cInvDefine2", typeof(string));//小包装单位

//                    }
//                    new_dt.Rows.Add(dt.Rows[0].ItemArray);
//                    if (dt.Rows.Count == 3)
//                    {

//                        new_dt.Rows[i]["cinvdefine13"] = Convert.ToDouble(dt.Rows[2]["iChangRate"].ToString()).ToString();
//                        new_dt.Rows[i]["cinvdefine14"] = Convert.ToDouble(dt.Rows[1]["iChangRate"].ToString()).ToString();
//                        new_dt.Rows[i]["cInvDefine2"] = dt.Rows[1]["cComUnitName"].ToString();
//                        new_dt.Rows[i]["cInvDefine1"] = dt.Rows[2]["cComUnitName"].ToString();
//                    }
//                    else if (dt.Rows.Count == 2)
//                    {
//                        new_dt.Rows[i]["cinvdefine13"] = Convert.ToDouble(dt.Rows[1]["iChangRate"].ToString()).ToString();
//                        new_dt.Rows[i]["cinvdefine14"] = Convert.ToDouble(dt.Rows[1]["iChangRate"].ToString()).ToString();
//                        new_dt.Rows[i]["cInvDefine2"] = dt.Rows[1]["cComUnitName"].ToString();
//                        new_dt.Rows[i]["cInvDefine1"] = dt.Rows[1]["cComUnitName"].ToString();
//                    }
//                    else if (dt.Rows.Count == 1)
//                    {
//                        new_dt.Rows[i]["cinvdefine13"] = Convert.ToDouble(dt.Rows[0]["iChangRate"].ToString()).ToString();
//                        new_dt.Rows[i]["cinvdefine14"] = Convert.ToDouble(dt.Rows[0]["iChangRate"].ToString()).ToString();
//                        new_dt.Rows[i]["cInvDefine2"] = dt.Rows[0]["cComUnitName"].ToString();
//                        new_dt.Rows[i]["cInvDefine1"] = dt.Rows[0]["cComUnitName"].ToString();
//                    }


//                }
//            }
//            ri.flag = "1";
//            ri.dt = new_dt;
//            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
//            return;
//        }
//        #endregion

//        #region 返回产品详细列表，新
//        private DataTable DLproc_QuasiOrderDetailBySel_new(string[] codes, string kpdw)
//        {


//            DataTable dt = new DataTable();
//            DataTable new_dt = new DataTable();




//            for (int i = 0; i < codes.Length; i++)
//            {
//                dt = new OrderManager().DLproc_QuasiOrderDetailBySel(codes[i], kpdw);
//                if (i == 0)
//                {
//                    new_dt = dt.Clone();
//                    new_dt.Columns.Add("cinvdefine13", typeof(float)); //大包装换算
//                    new_dt.Columns.Add("cinvdefine14", typeof(float));//小包装换算
//                    new_dt.Columns.Add("cInvDefine1", typeof(string));//大包装单位
//                    new_dt.Columns.Add("cInvDefine2", typeof(string));//小包装单位

//                }
//                new_dt.Rows.Add(dt.Rows[0].ItemArray);
//                if (dt.Rows.Count == 3)
//                {

//                    new_dt.Rows[i]["cinvdefine13"] = Convert.ToDouble(dt.Rows[2]["iChangRate"].ToString()).ToString();
//                    new_dt.Rows[i]["cinvdefine14"] = Convert.ToDouble(dt.Rows[1]["iChangRate"].ToString()).ToString();
//                    new_dt.Rows[i]["cInvDefine2"] = dt.Rows[1]["cComUnitName"].ToString();
//                    new_dt.Rows[i]["cInvDefine1"] = dt.Rows[2]["cComUnitName"].ToString();
//                }
//                else if (dt.Rows.Count == 2)
//                {
//                    new_dt.Rows[i]["cinvdefine13"] = Convert.ToDouble(dt.Rows[1]["iChangRate"].ToString()).ToString();
//                    new_dt.Rows[i]["cinvdefine14"] = Convert.ToDouble(dt.Rows[1]["iChangRate"].ToString()).ToString();
//                    new_dt.Rows[i]["cInvDefine2"] = dt.Rows[1]["cComUnitName"].ToString();
//                    new_dt.Rows[i]["cInvDefine1"] = dt.Rows[1]["cComUnitName"].ToString();
//                }
//                else if (dt.Rows.Count == 1)
//                {
//                    new_dt.Rows[i]["cinvdefine13"] = Convert.ToDouble(dt.Rows[0]["iChangRate"].ToString()).ToString();
//                    new_dt.Rows[i]["cinvdefine14"] = Convert.ToDouble(dt.Rows[0]["iChangRate"].ToString()).ToString();
//                    new_dt.Rows[i]["cInvDefine2"] = dt.Rows[0]["cComUnitName"].ToString();
//                    new_dt.Rows[i]["cInvDefine1"] = dt.Rows[0]["cComUnitName"].ToString();
//                }


//            }


//            return new_dt;
//        }
//        #endregion

//        #region 新增特殊订单
//        ReInfo DLproc_NewYOrderByIns()
//        {
//            ReInfo ri = new ReInfo();
//            string date = HttpContext.Current.Request.Form["date"];
//            string ccuscode = HttpContext.Current.Request.Form["ccuscode"];
//            if (string.IsNullOrEmpty(ccuscode) || ccuscode == "0")
//            {
//                ri.flag = "0";
//                ri.message = "请选择开票单位!";
//                return ri;
//            }
//            string lngopUserId = HttpContext.Current.Session["lngopUserId"].ToString(); //用户id
//            int bytStatus = 1;  //单据状态
//            int lngBillType = 2;  //单据类型，酬宾订单1，特殊订单2
//            string ccusname = HttpContext.Current.Request.Form["ccusname"]; //客户名称    
//            //string cdiscountname = TxtCBLX.Text;//促销类型,2016-04-26添加
//            string cdiscountname = HttpContext.Current.Request.Form["cdiscountname"] + "-特殊订单";
//            string cMemo = HttpContext.Current.Request.Form["strRemarks"];
//            string lngopUserExId = HttpContext.Current.Session["lngopUserExId"].ToString(); //20160826添加
//            string strAllAcount = HttpContext.Current.Session["strAllAcount"].ToString();   //20160826添加

//            //插入表头数据,DL表中
//            DataTable lngopOrderIdDt = new DataTable();
//            lngopOrderIdDt = new OrderManager().DLproc_NewYOrderByIns(date, lngopUserId, bytStatus, ccuscode, ccusname, lngBillType, cdiscountname, cMemo, lngopUserExId, strAllAcount);

//            if (lngopOrderIdDt.Rows.Count == 0)
//            {
//                ri.flag = "0";
//                ri.message = "订单提交失败，请重试或联系管理员!";
//                return ri;
//            }

//            List<Buy_list> list = JsonConvert.DeserializeObject<List<Buy_list>>(HttpContext.Current.Request.Form["buy_list"]);
//            //验证订单的每个商品数量是否为0或小于起订量
//            foreach (var item in list)
//            {
//                Buy_list bl = item;
//                if (bl.iquantity == 0 || bl.iquantity < bl.MinOrderQTY)
//                {
//                    ri.flag = "0";
//                    ri.message = "有未输入数量的商品或小于起量！";
//                    return ri;
//                }
//            }

//            DataTable dt = new DataTable();

//            //插入表体数据
//            int lngopOrderId = Convert.ToInt32(lngopOrderIdDt.Rows[0]["lngopOrderId"].ToString());   //订单id,从表头中获取:表头插入后,返回表头id
//            string strBillNo = lngopOrderIdDt.Rows[0]["strBillNo"].ToString();   //订单编号,插入表头数据时自动生成,用于反馈给用户

//            foreach (var item in list)
//            {
//                dt = new OrderManager().DLproc_QuasiOrderDetailBySel(item.cinvcode, ccuscode); //重新获取商品价格信息
//                DataRow dr = dt.Rows[0];

//                string cinvcode = item.cinvcode;   //存货编码
//                double iquantity = item.iquantity;   //购买货数量
//                double iinvexchrate = 1;//换算率 
//                double inum = 1;        //辅计量数量   
//                //计算销售单位(辅助)的换算率
//                if (dr["cn1cComUnitName"].ToString() == item.cInvDefine1)  //大包装单位换算率
//                {
//                    iinvexchrate = Convert.ToDouble(item.cInvDefine13);
//                }
//                else if (dr["cn1cComUnitName"].ToString() == item.cInvDefine2) //小包装单位换算率
//                {
//                    iinvexchrate = Convert.ToDouble(item.cInvDefine14);
//                }
//                else  //无换算单位
//                {
//                    iinvexchrate = 1;//换算率 
//                }
//                //计算赋值
//                //换算结果:辅计量数量 =存货数量/换算率,四舍五入,保留两位小数
//                inum = Math.Round(iquantity / iinvexchrate, 2);  //辅计量数量 
//                double iquotedprice = Convert.ToDouble(dr["Quote"].ToString());//报价 保留5位小数,四舍五入
//                double itaxunitprice = Convert.ToDouble(dr["ExercisePrice"].ToString());//原币含税单价 即执行价格dt.Columns.Add("ExercisePrice"); //基本单位单价(执行价格)   16,保留5位
//                double isum = Math.Round(itaxunitprice * iquantity, 2);     //原币价税合计    //金额,原币价税合计=原币含税单价*数量,保留2位小数,四舍五入 
//                double itax = Math.Round(isum / 1.17 * 0.17, 2);        //原币税额 ;税额=金额/1.17*0.17 保留2位, 四舍五入    
//                double imoney = Math.Round(isum - itax, 2);      //原币无税金额 =金额-税额,保留2位,四舍五入
//                double iunitprice = Math.Round(imoney / iquantity, 6);  //原币无税单价=无税金额/数量,保留5位小数,四舍五入        
//                double idiscount = Math.Round(iquotedprice * iquantity - isum, 2);   //原币折扣额=报价*数量-金额,保留两位
//                double inatunitprice = iunitprice;//本币无税单价
//                double inatmoney = imoney;   //本币无税金额
//                double inattax = itax;     //本币税额 
//                double inatsum = isum;     //本币价税合计
//                double inatdiscount = idiscount;   //本币折扣额 
//                double kl = Convert.ToDouble(dr["Rate"].ToString());          //扣率 
//                double itaxrate = Convert.ToDouble(dr["iTaxRate"].ToString());    //税率 
//                string cDefine22 = item.cDefine22;  //表体自定义项22,包装量
//                string cunitid = item.cunitid;    //计量单位编码
//                int irowno = item.irowno; //行号,从1开始,自增长
//                string cinvname = item.cinvname;   //存货名称 
//                string cComUnitName = item.cComUnitName;       //基本单位名称
//                string cInvDefine1 = item.cInvDefine1;        //大包装单位名称         
//                string cInvDefine2 = item.cInvDefine2;        //小包装单位名称 
//                double cInvDefine13 = item.cInvDefine13;       //大包装换算率
//                double cInvDefine14 = item.cInvDefine14;       //小包装换算率
//                string unitGroup = item.unitGroup;          //单位换算率组
//                double cComUnitQTY = item.cComUnitQTY;        //基本单位数量
//                double cInvDefine1QTY = item.cInvDefine1QTY;     //大包装单位数量
//                double cInvDefine2QTY = item.cInvDefine2QTY;     //小包装单位数量
//                string cn1cComUnitName = dr["cn1cComUnitName"].ToString();    //销售单位名称

//                //OrderInfo oi = new OrderInfo(
//                //    lngopOrderId, cinvcode, iquantity, inum, iquotedprice, iunitprice, itaxunitprice, imoney, itax, isum, inatunitprice, inatmoney, inattax, inatsum, kl, itaxrate, cDefine22, iinvexchrate, cunitid, irowno, cinvname, idiscount, inatdiscount, cComUnitName, cInvDefine1, cInvDefine2, cInvDefine13, cInvDefine14, unitGroup, cComUnitQTY, cInvDefine1QTY, cInvDefine2QTY, cn1cComUnitName);

//                //bool c = new OrderManager().DLproc_NewYOrderDetailByIns(oi);
//                //if (!c)
//                //{
//                //    ri.flag = "0";
//                //    ri.message = "订单提交失败，请重试或联系管理员！";
//                //    return ri;
//                //}
//            }

//            ri.flag = "1";
//            ri.message = strBillNo;
//            return ri;
//        }
//        #endregion

//        #region 查询特殊订单列表
//        ReInfo DLproc_MyWorkPreYOrderForCustomerBySel()
//        {
//            ReInfo ri = new ReInfo();
//            string start_time = HttpContext.Current.Request.Form["start_date"];
//            string end_time = HttpContext.Current.Request.Form["end_date"];
//            if (string.IsNullOrEmpty(start_time))
//            {
//                start_time = "2015-12-01";
//            }
//            if (string.IsNullOrEmpty(end_time))
//            {
//                end_time = "2099-01-01";
//            }
//            if (Convert.ToDateTime(start_time) > Convert.ToDateTime(end_time))
//            {
//                ri.flag = "0";
//                ri.message = "结束时间不能大于开始时间！";
//                return ri;
//            }
//            string strBillNo = HttpContext.Current.Request.Form["strbillno"];
//            int OrderStatus = Convert.ToInt32(HttpContext.Current.Request.Form["orderstatus"]);
//            ri.dt = new OrderManager().DLproc_MyWorkPreYOrderForCustomerBySel(strBillNo, start_time, end_time, OrderStatus, HttpContext.Current.Session["ConstcCusCode"].ToString(), "2");
//            return ri;

//        }
//        #endregion

//        #region 返回某个特殊订单明细
//        ReInfo DL_XOrderBillDetailBySel()
//        {
//            ReInfo ri = new ReInfo();
//            string strBillNo = HttpContext.Current.Request.Form["strBillNo"];
//            ri.dt = new OrderManager().DL_XOrderBillDetailBySel(strBillNo);
//            return ri;
//        }
//        #endregion

//        #region 在新增特殊订单时，查询可用于参照的特殊订单号
//        ReInfo DL_PreOrderTreeBySel()
//        {
//            ReInfo ri = new ReInfo();
//            ri.dt = new SearchManager().DL_PreOrderTreeBySel(HttpContext.Current.Session["KPDWcCusCode"].ToString(), HttpContext.Current.Session["lngopUserId"].ToString(), HttpContext.Current.Session["lngopUserExId"].ToString(), 2);
//            ri.flag = "1";
//            return ri;
//        }
//        #endregion

//        #region 根据特殊订单号查询详细产品列表
//        ReInfo DLproc_TreeListPreDetailsBySel()
//        {
//            ReInfo ri = new ReInfo();
//            ri.dt = new SearchManager().DLproc_TreeListPreDetailsBySel(HttpContext.Current.Request.Form["strBillNo"], 2);
//            return ri;
//        }
//        #endregion

//        #region 根据选择的特殊订单里的产品，获取详细产品价格等信息
//        ReInfo DLproc_QuasiYOrderDetail_TSBySel()
//        {
//            ReInfo ri = new ReInfo();
//            string kpdw = HttpContext.Current.Request.Form["kpdw"];

//            if (!string.IsNullOrEmpty(kpdw) || kpdw != "0")
//            {
//                ri.dt = new OrderManager().DLproc_getCusCreditInfo(kpdw);
//            }
//            string code = HttpContext.Current.Request.Form["itemids"];
//            if (string.IsNullOrEmpty(code) || code.Length == 0)
//            {
//                return ri;
//            }
//            ri.list_dt = new List<DataTable>();
//            string[] codes = code.Split(',');
//            string cInvCode = ""; //产品编码
//            string preOrderId = ""; //订单编号
//            string[] it = new string[2]; //根据itemid截取字符串
//            foreach (var item in codes)
//            {
//                it = item.Split('Y');
//                cInvCode = it[0];
//                preOrderId = "Y" + it[1];
//                ri.list_dt.Add(new product().DLproc_QuasiYOrderDetail_TSBySel(cInvCode, preOrderId));
//            }


//            return ri;
//        }
//        #endregion

//        #region 参照特殊订单后提交普通订单
//        ReInfo DLproc_NewYYOrderByIns()
//        {
//            ReInfo ri = new ReInfo();
//            ri.list_msg = new List<string>();
//            ri.flag = "1";
//            //获取表头数据实例化为model
//            form_Data formData = JsonConvert.DeserializeObject<form_Data>(HttpContext.Current.Request.Form["formData"]);

//            //获取表体数据实例化model
//            List<Buy_list> listData = JsonConvert.DeserializeObject<List<Buy_list>>(HttpContext.Current.Request.Form["listData"]);

//            //根据表体数据itemid重新生成Table，计算金额，用于验证信用、是否大于库存、是否大于可用量、是否有未填写数量的商品
//            DataTable Check_Dt = new DataTable(); //用于验证的table
//            DataTable dt = new DataTable();
//            string cInvCode = ""; //产品编码
//            string preOrderId = ""; //订单编号
//            for (int i = 0; i < listData.Count; i++)
//            {
//                string[] code = listData[i].itemid.Split('Y');
//                cInvCode = code[0];
//                preOrderId = "Y" + code[1];
//                dt = new product().DLproc_QuasiYOrderDetail_TSBySel(cInvCode, preOrderId);
//                if (i == 0)  //第一次的时候clone表格结构
//                {
//                    Check_Dt = dt.Clone();
//                    Check_Dt.Columns.Add("rowType", typeof(int)); //添加行状态列，用于前台赋值颜色，1为购买量为零；2为购买量大于库存量；3为购买量大于可用量
//                }
//                Check_Dt.Rows.Add(dt.Rows[0].ItemArray);
//                Check_Dt.Rows[i]["cComUnitQTY"] = listData[i].cComUnitQTY;  //基本数量赋值
//                Check_Dt.Rows[i]["cInvDefine2QTY"] = listData[i].cInvDefine2QTY;  //小包装数量赋值
//                Check_Dt.Rows[i]["cInvDefine1QTY"] = listData[i].cInvDefine1QTY;  //大包装数量赋值
//                Check_Dt.Rows[i]["cDefine22"] = listData[i].cDefine22;   //包装结果赋值
//                Check_Dt.Rows[i]["iquantity"] = listData[i].iquantity;   //汇总数量赋值
//                Check_Dt.Rows[i]["irowno"] = listData[i].irowno;             //行号赋值
//                Check_Dt.Rows[i]["rowType"] = 0;                //行默认状态赋值
//            }

//            #region 验证表单开始

//            dt = new OrderManager().DLproc_getCusCreditInfo(formData.ccuscode);// 重新获取信用
//            double CusCredit = Convert.ToDouble(dt.Rows[0]["iCusCreLine"].ToString()); //客户信用
//            double listCredit = 0;   //用于累计表单金额
//            int rowType = 0; //用于记录表体中是否有不合格数据

//            foreach (DataRow dr in Check_Dt.Rows)
//            {
//                listCredit += Convert.ToDouble(dr["iquantity"].ToString()) * Convert.ToDouble(dr["itaxunitprice"].ToString());
//                if (Convert.ToDouble(dr["iquantity"].ToString()) == 0)
//                {
//                    dr["rowType"] = 1;
//                }
//                if (Convert.ToDouble(dr["iquantity"].ToString()) > Convert.ToDouble(dr["fAvailQtty"].ToString()))
//                {
//                    dr["rowType"] = 2;
//                }
//                if (Convert.ToDouble(dr["iquantity"].ToString()) > Convert.ToDouble(dr["iquantity"].ToString()))  //字段错误！！iquantity应为realqty
//                {
//                    dr["rowType"] = 3;
//                }
//                rowType += Convert.ToInt32(dr["rowType"].ToString());
//            }
//            if (CusCredit != -99999999.000000)  //-99999999.000000为现金用户
//            {
//                if (listCredit - CusCredit > 0)
//                {
//                    ri.flag = "0";
//                    ri.list_msg.Add("你的购买金额已超过信用" + Math.Round((listCredit - CusCredit), 2));
//                    ri.dt = Check_Dt;
//                }
//                if (rowType != 0)
//                {
//                    ri.flag = "0";
//                    ri.list_msg.Add("订单列表里有不合法数据，请重新输入!");
//                }
//                if (ri.flag == "0")
//                {
//                    return ri;
//                }
//            }
//            else
//            {
//                if (rowType != 0)
//                {
//                    ri.flag = "0";
//                    ri.list_msg.Add("订单列表里有不合法数据，请重新输入!");
//                }
//                if (ri.flag == "0")
//                {
//                    return ri;
//                }
//            }
//            #endregion

//            //验证通过后，开始提交表单
//            #region 获取表头信息，提交表头数据
//            DataTable Address_Dt = new product().Get_AddressById(formData.lngopUseraddressId);
//            OrderInfo oi = new OrderInfo(
//                                            HttpContext.Current.Session["lngopUserId"].ToString(), //客户ID
//                                            DateTime.Now.ToString(),                               //订单时间                     
//                                            1,                                                     //订单状态         
//                                            formData.strRemarks,                                   //备注     
//                                            formData.ccuscode,                                     //开票单位ID 
//                                            Address_Dt.Rows[0]["strDriverName"].ToString(),        //司机姓名 
//                                            Address_Dt.Rows[0]["strIdCard"].ToString(),            //司机身份证
//                                            formData.carType,                                      //车型
//                                            Address_Dt.Rows[0]["strConsigneeName"].ToString(),     //收货人姓名
//                                            Address_Dt.Rows[0]["strCarplateNumber"].ToString(),     //车牌号
//                                            formData.txtAddress,                                    //地址信息
//                                            Address_Dt.Rows[0]["strConsigneeTel"].ToString(),      //收货人电话
//                                            Address_Dt.Rows[0]["strDriverTel"].ToString(),         //司机电话
//                                            formData.ccusname,                                     //开票单位名称
//                                            formData.ccuspperson,                                  //业务员编号 
//                                            formData.csccode,                                      //送货方式
//                                            formData.datDeliveryDate,                              //提货时间
//                                            formData.strLoadingWays,                               //装车方式
//                                            "00",                                                  //销售类型编码    
//                                            formData.lngopUseraddressId,                           //地址ID
//                                            "",                                                    //strTxtRelateU8NO
//                                            2,                                                     //lngBillType 特殊订单
//                                            formData.txtArea,                                      //行政区
//                                            HttpContext.Current.Session["lngopUserExId"].ToString(),//用户ExID
//                                            HttpContext.Current.Session["strAllAcount"].ToString()
//                                       );
//            DataTable lngopOrderIdDt = new OrderManager().DLproc_NewYYOrderByIns(oi);

//            if (lngopOrderIdDt.Rows.Count < 0)
//            {
//                ri.flag = "0";
//                ri.list_msg.Add("订单提交出错，请重试或联系管理员！");
//                return ri;
//            }
//            #endregion

//            #region 提交表体数据
//            int lngopOrderId = Convert.ToInt32(lngopOrderIdDt.Rows[0]["lngopOrderId"].ToString());
//            //  OrderInfo oiEntry1 = new OrderInfo(lngopOrderId, cinvcode, iquantity, inum, iquotedprice, iunitprice, itaxunitprice, imoney, itax, isum, inatunitprice, inatmoney, inattax, inatsum, kl, itaxrate, cDefine22, iinvexchrate, cunitid, irowno, cinvname, idiscount, inatdiscount, cComUnitName, cInvDefine1, cInvDefine2, cInvDefine13, cInvDefine14, unitGroup, cComUnitQTY, cInvDefine1QTY, cInvDefine2QTY, cn1cComUnitName, cpreordercode, autoid);
//            foreach (DataRow dr in Check_Dt.Rows)
//            {
//                string cinvcode = dr["cinvcode"].ToString();   //存货编码
//                double iquantity = Convert.ToDouble(dr["iquantity"].ToString());   //购买货数量
//                double iinvexchrate = 1;//换算率 
//                double inum = 1;        //辅计量数量   
//                //计算销售单位(辅助)的换算率
//                if (dr["cn1cComUnitName"].ToString() == dr["cInvDefine1"].ToString())  //大包装单位换算率
//                {
//                    iinvexchrate = Convert.ToDouble(dr["cInvDefine13"].ToString());
//                }
//                else if (dr["cn1cComUnitName"].ToString() == dr["cInvDefine2"].ToString()) //小包装单位换算率
//                {
//                    iinvexchrate = Convert.ToDouble(dr["cInvDefine14"].ToString());
//                }
//                else  //无换算单位
//                {
//                    iinvexchrate = 1;//换算率 
//                }
//                //计算赋值
//                //换算结果:辅计量数量 =存货数量/换算率,四舍五入,保留两位小数
//                inum = Math.Round(iquantity / iinvexchrate, 2);  //辅计量数量 
//                double iquotedprice = Convert.ToDouble(dr["iquotedprice"].ToString());//报价 保留5位小数,四舍五入
//                double itaxunitprice = Convert.ToDouble(dr["itaxunitprice"].ToString());//原币含税单价 即执行价格dt.Columns.Add("ExercisePrice"); //基本单位单价(执行价格)   16,保留5位
//                double isum = Math.Round(itaxunitprice * iquantity, 2);     //原币价税合计    //金额,原币价税合计=原币含税单价*数量,保留2位小数,四舍五入 
//                double itax = Math.Round(isum / 1.17 * 0.17, 2);        //原币税额 ;税额=金额/1.17*0.17 保留2位, 四舍五入    
//                double imoney = Math.Round(isum - itax, 2);      //原币无税金额 =金额-税额,保留2位,四舍五入
//                double iunitprice = Math.Round(imoney / iquantity, 6);  //原币无税单价=无税金额/数量,保留5位小数,四舍五入        
//                double idiscount = Math.Round(iquotedprice * iquantity - isum, 2);   //原币折扣额=报价*数量-金额,保留两位
//                double inatunitprice = iunitprice;//本币无税单价
//                double inatmoney = imoney;   //本币无税金额
//                double inattax = itax;     //本币税额 
//                double inatsum = isum;     //本币价税合计
//                double inatdiscount = idiscount;   //本币折扣额 
//                double kl = Convert.ToDouble(dr["kl"].ToString());          //扣率 
//                double itaxrate = Convert.ToDouble(dr["itaxrate"].ToString());    //税率 
//                string cDefine22 = dr["cDefine22"].ToString();  //表体自定义项22,包装量
//                string cunitid = dr["cunitid"].ToString();    //计量单位编码
//                int irowno = Convert.ToInt32(dr["irowno"].ToString()); //行号,从1开始,自增长
//                string cinvname = dr["cinvname"].ToString();   //存货名称 
//                string cComUnitName = dr["cComUnitName"].ToString();       //基本单位名称
//                string cInvDefine1 = dr["cInvDefine1"].ToString();        //大包装单位名称         
//                string cInvDefine2 = dr["cInvDefine2"].ToString();        //小包装单位名称 
//                double cInvDefine13 = Convert.ToDouble(dr["cInvDefine13"].ToString());       //大包装换算率
//                double cInvDefine14 = Convert.ToDouble(dr["cInvDefine14"].ToString());       //小包装换算率
//                string unitGroup = dr["unitGroup"].ToString();          //单位换算率组
//                double cComUnitQTY = Convert.ToDouble(dr["cComUnitQTY"].ToString());        //基本单位数量
//                double cInvDefine1QTY = Convert.ToDouble(dr["cInvDefine1QTY"].ToString());     //大包装单位数量
//                double cInvDefine2QTY = Convert.ToDouble(dr["cInvDefine2QTY"].ToString());     //小包装单位数量
//                string cn1cComUnitName = dr["cn1cComUnitName"].ToString();    //销售单位名称
//                string cpreordercode = dr["ccode"].ToString();    //销售预订单号
//                string autoid = dr["iaoids"].ToString();    //预订单号id
//                string cDefine24 = dr["cDefine24"].ToString();
//                OrderInfo oiEntry = new OrderInfo(lngopOrderId, cinvcode, iquantity, inum, iquotedprice, iunitprice, itaxunitprice, imoney, itax, isum, inatunitprice, inatmoney, inattax, inatsum, kl, itaxrate, cDefine22, iinvexchrate, cunitid, irowno, cinvname, idiscount, inatdiscount, cComUnitName, cInvDefine1, cInvDefine2, cInvDefine13, cInvDefine14, unitGroup, cComUnitQTY, cInvDefine1QTY, cInvDefine2QTY, cn1cComUnitName, cpreordercode, autoid, cDefine24);
//                //OrderInfo oiEntry=new OrderInfo(lngopOrderId,dr["cinvcode"].ToString(),Convert.ToDouble(dr["iquantity"].ToString()),Convert.ToDouble(dr["inum"].ToString()),Convert.ToDouble(dr["iquotedprice"].ToString()),Convert.ToDouble(dr["iunitprice"].ToString()),
//                //                                                Convert.ToDouble(dr["itaxunitprice"].ToString()),Convert.ToDouble(dr["imoney"].ToString()),Convert.ToDouble(dr["itax"].ToString()),Convert.ToDouble(dr["isum"].ToString()),
//                //                                                Convert.ToDouble(dr["inatunitprice"].ToString()),
//                //                                                Convert.ToDouble(dr["inatmoney"].ToString()),Convert.ToDouble(dr["inattax"].ToString()),Convert.ToDouble(dr["inatsum"].ToString()),Convert.ToDouble(dr["kl"].ToString()),Convert.ToDouble(dr["itaxrate"].ToString()),dr["cDefine22"].ToString(),
//                //                                                Convert.ToDouble(dr["iinvexchrate"].ToString()),dr["cunitid"].ToString(),Convert.ToInt32(dr["irowno"].ToString()),dr["cinvname"].ToString(),Convert.ToDouble(dr["idiscount"].ToString()),Convert.ToDouble(dr["inatdiscount"].ToString()),
//                //                                                dr["cComUnitName"].ToString(),dr["cInvDefine1"].ToString(),dr["cInvDefine2"].ToString(),Convert.ToDouble(dr["cInvDefine13"].ToString()),Convert.ToDouble(dr["cInvDefine14"].ToString()),
//                //                                                dr["unitGroup"].ToString(),Convert.ToDouble(dr["cComUnitQTY"].ToString()),Convert.ToDouble(dr["cInvDefine1QTY"].ToString()),Convert.ToDouble(dr["cInvDefine2QTY"].ToString()),
//                //                                                dr["cn1cComUnitName"].ToString(), dr["ccode"].ToString(), dr["iaoids"].ToString());
//                new OrderManager().DLproc_NewYYOrderDetailByIns(oiEntry);

//            }
//            ri.flag = "1";
//            ri.message = lngopOrderIdDt.Rows[0]["strBillNo"].ToString();
//            #endregion
//            return ri;
//        }

//        #endregion

//        #region 新增普通订单，新
//        public void DLproc_NewOrderByIns_new()
//        {
//            ReInfo ri = new ReInfo();
//            ri.list_msg = new List<string>();
//            ri.flag = "1";
//            //获取表头数据实例化为model
//            form_Data formData = JsonConvert.DeserializeObject<form_Data>(HttpContext.Current.Request.Form["formData"]);

//            //获取表体数据实例化model
//            List<Buy_list> listData = JsonConvert.DeserializeObject<List<Buy_list>>(HttpContext.Current.Request.Form["listData"]);

//            //根据表体数据itemid重新生成Table，计算金额，用于验证信用、是否大于库存、是否大于可用量、是否有未填写数量的商品
//            DataTable Check_Dt = new DataTable(); //用于验证的table
//            DataTable dt = new DataTable();
//            for (int i = 0; i < listData.Count; i++)
//            {
//                dt = new OrderManager().DLproc_QuasiOrderDetailBySel(listData[i].cinvcode, formData.ccuscode);

//                if (i == 0) //第一条数据时clone表结构
//                {
//                    Check_Dt = dt.Clone();
//                    Check_Dt.Columns.Add("cComUnitQTY", typeof(string));  //基本数量 
//                    Check_Dt.Columns.Add("cInvDefine2QTY", typeof(string));//小包装数量 
//                    Check_Dt.Columns.Add("cInvDefine1QTY", typeof(string)); //大包装数量
//                    Check_Dt.Columns.Add("cDefine22", typeof(string));      //包装结果
//                    Check_Dt.Columns.Add("iquantity", typeof(string));       //汇总数量
//                    Check_Dt.Columns.Add("unitGroup", typeof(string));    //包装组
//                    Check_Dt.Columns.Add("irowno", typeof(string));           //行号
//                    Check_Dt.Columns.Add("cInvDefine13", typeof(string));    //大包装换算率
//                    Check_Dt.Columns.Add("cInvDefine14", typeof(string));    //小包装换算率
//                    Check_Dt.Columns.Add("cInvDefine1", typeof(string));    //大包装单位
//                    Check_Dt.Columns.Add("cInvDefine2", typeof(string));    //小包装单位
//                    Check_Dt.Columns.Add("rowType", typeof(int));       //行状态

//                }
//                Check_Dt.Rows.Add(dt.Rows[0].ItemArray);
//                Check_Dt.Rows[i]["cComUnitQTY"] = listData[i].cComUnitQTY;  //基本数量赋值
//                Check_Dt.Rows[i]["cInvDefine2QTY"] = listData[i].cInvDefine2QTY;  //小包装数量赋值
//                Check_Dt.Rows[i]["cInvDefine1QTY"] = listData[i].cInvDefine1QTY;  //大包装数量赋值
//                Check_Dt.Rows[i]["cDefine22"] = listData[i].cDefine22;   //包装结果赋值
//                Check_Dt.Rows[i]["iquantity"] = listData[i].iquantity;   //汇总数量赋值
//                Check_Dt.Rows[i]["unitGroup"] = listData[i].unitGroup;    //单位组赋值
//                Check_Dt.Rows[i]["irowno"] = listData[i].irowno;             //行号赋值


//                Check_Dt.Rows[i]["rowType"] = 0;                //行默认状态赋值
//                if (dt.Rows.Count == 3)
//                {
//                    Check_Dt.Rows[i]["cInvDefine1"] = dt.Rows[2]["cComUnitName"].ToString();//大包装单位
//                    Check_Dt.Rows[i]["cInvDefine2"] = dt.Rows[1]["cComUnitName"].ToString();//小包装单位
//                    Check_Dt.Rows[i]["cInvDefine13"] = dt.Rows[2]["iChangRate"].ToString();//大包装换算率
//                    Check_Dt.Rows[i]["cInvDefine14"] = dt.Rows[1]["iChangRate"].ToString();//小包装换算率
//                }
//                else if (dt.Rows.Count == 2)
//                {
//                    Check_Dt.Rows[i]["cInvDefine1"] = dt.Rows[1]["cComUnitName"].ToString();
//                    Check_Dt.Rows[i]["cInvDefine2"] = dt.Rows[1]["cComUnitName"].ToString();
//                    Check_Dt.Rows[i]["cInvDefine13"] = dt.Rows[1]["iChangRate"].ToString();
//                    Check_Dt.Rows[i]["cInvDefine14"] = dt.Rows[1]["iChangRate"].ToString();
//                }
//                else
//                {
//                    Check_Dt.Rows[i]["cInvDefine1"] = dt.Rows[0]["cComUnitName"].ToString();
//                    Check_Dt.Rows[i]["cInvDefine2"] = dt.Rows[0]["cComUnitName"].ToString();
//                    Check_Dt.Rows[i]["cInvDefine13"] = dt.Rows[0]["iChangRate"].ToString();
//                    Check_Dt.Rows[i]["cInvDefine14"] = dt.Rows[0]["iChangRate"].ToString();
//                }
//            }


//            #region 验证表单开始

//            dt = new OrderManager().DLproc_getCusCreditInfo(formData.ccuscode);// 重新获取信用
//            double CusCredit = Convert.ToDouble(dt.Rows[0]["iCusCreLine"].ToString()); //客户信用
//            double listCredit = 0;   //用于累计表单金额
//            int rowType = 0; //用于记录表体中是否有不合格数据

//            foreach (DataRow dr in Check_Dt.Rows)
//            {
//                listCredit += Convert.ToDouble(dr["iquantity"].ToString()) * Convert.ToDouble(dr["ExercisePrice"].ToString());
//                if (Convert.ToDouble(dr["fAvailQtty"].ToString()) == 0)
//                {
//                    dr["rowType"] = 1;
//                }
//                else if (Convert.ToDouble(dr["iquantity"].ToString()) > Convert.ToDouble(dr["fAvailQtty"].ToString()))
//                {
//                    dr["rowType"] = 2;
//                }

//                rowType += Convert.ToInt32(dr["rowType"].ToString());
//            }
//            if (CusCredit != -99999999.000000)  //-99999999.000000为现金用户
//            {
//                if (listCredit - CusCredit > 0)
//                {
//                    ri.flag = "0";
//                    ri.list_msg.Add("你的购买金额已超过信用" + Math.Round((listCredit - CusCredit), 2));

//                }
//                if (rowType != 0)
//                {
//                    ri.flag = "0";
//                    ri.list_msg.Add("订单列表里有不合法数据，请重新输入!");
//                }
//                if (ri.flag == "0")
//                {
//                    ri.dt = Check_Dt;
//                    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
//                    return;
//                }
//            }
//            else
//            {
//                if (rowType != 0)
//                {
//                    ri.flag = "0";
//                    ri.list_msg.Add("订单列表里有不合法数据，请重新输入!");

//                }
//                if (ri.flag == "0")
//                {
//                    ri.dt = Check_Dt;
//                    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
//                    return;
//                }
//            }
//            #endregion

//            //验证通过后，开始提交表单
//            #region 获取表头信息，提交表头数据
//            DataTable Address_Dt = new product().Get_AddressById(formData.lngopUseraddressId);
//            OrderInfo oi = new OrderInfo(
//                                            HttpContext.Current.Session["lngopUserId"].ToString(), //客户ID
//                                            DateTime.Now.ToString(),                               //订单时间                     
//                                            1,                                                     //订单状态         
//                                            formData.strRemarks,                                   //备注     
//                                            formData.ccuscode,                                     //开票单位ID 
//                                            Address_Dt.Rows[0]["strDriverName"].ToString(),        //司机姓名 
//                                            Address_Dt.Rows[0]["strIdCard"].ToString(),            //司机身份证
//                                            formData.carType,                                      //车型
//                                            Address_Dt.Rows[0]["strConsigneeName"].ToString(),     //收货人姓名
//                                            Address_Dt.Rows[0]["strCarplateNumber"].ToString(),     //车牌号
//                                            formData.txtAddress,                                    //地址信息
//                                            Address_Dt.Rows[0]["strConsigneeTel"].ToString(),      //收货人电话
//                                            Address_Dt.Rows[0]["strDriverTel"].ToString(),         //司机电话
//                                            formData.ccusname,                                     //开票单位名称
//                                            formData.ccuspperson,                                  //业务员编号 
//                                            formData.csccode,                                      //送货方式
//                                            formData.datDeliveryDate,                              //提货时间
//                                            formData.strLoadingWays,                               //装车方式
//                                            "00",                                                  //销售类型编码    
//                                            formData.lngopUseraddressId,                           //地址ID
//                                            "",                                                   //strTxtRelateU8NO
//                                            dt.Rows[0]["cdiscountname"].ToString(),                //lngBillType 特殊订单
//                                            formData.txtArea,                                      //行政区
//                                            HttpContext.Current.Session["lngopUserExId"].ToString(),//用户ExID
//                                            HttpContext.Current.Session["strAllAcount"].ToString()
//                                       );
//            DataTable lngopOrderIdDt = new OrderManager().DLproc_NewOrderByIns(oi);

//            if (lngopOrderIdDt.Rows.Count < 0)
//            {
//                ri.flag = "0";
//                ri.list_msg.Add("订单提交出错，请重试或联系管理员！");
//                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
//                return;
//            }
//            #endregion

//            #region 提交表体数据
//            int lngopOrderId = Convert.ToInt32(lngopOrderIdDt.Rows[0]["lngopOrderId"].ToString());
//            //  OrderInfo oiEntry1 = new OrderInfo(lngopOrderId, cinvcode, iquantity, inum, iquotedprice, iunitprice, itaxunitprice, imoney, itax, isum, inatunitprice, inatmoney, inattax, inatsum, kl, itaxrate, cDefine22, iinvexchrate, cunitid, irowno, cinvname, idiscount, inatdiscount, cComUnitName, cInvDefine1, cInvDefine2, cInvDefine13, cInvDefine14, unitGroup, cComUnitQTY, cInvDefine1QTY, cInvDefine2QTY, cn1cComUnitName, cpreordercode, autoid);
//            foreach (DataRow dr in Check_Dt.Rows)
//            {
//                string cinvcode = dr["cinvcode"].ToString();   //存货编码
//                double iquantity = Convert.ToDouble(dr["iquantity"].ToString());   //购买货数量
//                double iinvexchrate = 1;//换算率 
//                double inum = 1;        //辅计量数量   
//                //计算销售单位(辅助)的换算率
//                if (dr["cn1cComUnitName"].ToString() == dr["cInvDefine1"].ToString())  //大包装单位换算率
//                {
//                    iinvexchrate = Convert.ToDouble(dr["cInvDefine13"].ToString());
//                }
//                else if (dr["cn1cComUnitName"].ToString() == dr["cInvDefine2"].ToString()) //小包装单位换算率
//                {
//                    iinvexchrate = Convert.ToDouble(dr["cInvDefine14"].ToString());
//                }
//                else  //无换算单位
//                {
//                    iinvexchrate = 1;//换算率 
//                }
//                //计算赋值
//                //换算结果:辅计量数量 =存货数量/换算率,四舍五入,保留两位小数
//                inum = Math.Round(iquantity / iinvexchrate, 2);  //辅计量数量 
//                double iquotedprice = Convert.ToDouble(dr["Quote"].ToString());//报价 保留5位小数,四舍五入
//                double itaxunitprice = Convert.ToDouble(dr["ExercisePrice"].ToString());//原币含税单价 即执行价格dt.Columns.Add("ExercisePrice"); //基本单位单价(执行价格)   16,保留5位
//                double isum = Math.Round(itaxunitprice * iquantity, 2);     //原币价税合计    //金额,原币价税合计=原币含税单价*数量,保留2位小数,四舍五入 
//                double itax = Math.Round(isum / 1.17 * 0.17, 2);        //原币税额 ;税额=金额/1.17*0.17 保留2位, 四舍五入    
//                double imoney = Math.Round(isum - itax, 2);      //原币无税金额 =金额-税额,保留2位,四舍五入
//                double iunitprice = Math.Round(imoney / iquantity, 6);  //原币无税单价=无税金额/数量,保留5位小数,四舍五入        
//                double idiscount = Math.Round(iquotedprice * iquantity - isum, 2);   //原币折扣额=报价*数量-金额,保留两位
//                double inatunitprice = iunitprice;//本币无税单价
//                double inatmoney = imoney;   //本币无税金额
//                double inattax = itax;     //本币税额 
//                double inatsum = isum;     //本币价税合计
//                double inatdiscount = idiscount;   //本币折扣额 
//                double kl = Convert.ToDouble(dr["Rate"].ToString());          //扣率 
//                double itaxrate = Convert.ToDouble(dr["iTaxRate"].ToString());    //税率 
//                string cDefine22 = dr["cDefine22"].ToString();  //表体自定义项22,包装量
//                string cunitid = dr["cComUnitCode"].ToString();    //计量单位编码
//                int irowno = Convert.ToInt32(dr["irowno"].ToString()); //行号,从1开始,自增长
//                string cinvname = dr["cInvName"].ToString();   //存货名称 
//                string cComUnitName = dr["cComUnitName"].ToString();       //基本单位名称
//                string cInvDefine1 = dr["cInvDefine1"].ToString();        //大包装单位名称         
//                string cInvDefine2 = dr["cInvDefine2"].ToString();        //小包装单位名称 
//                double cInvDefine13 = Convert.ToDouble(dr["cInvDefine13"].ToString());       //大包装换算率
//                double cInvDefine14 = Convert.ToDouble(dr["cInvDefine14"].ToString());       //小包装换算率
//                string unitGroup = dr["unitGroup"].ToString();          //单位换算率组
//                double cComUnitQTY = Convert.ToDouble(dr["cComUnitQTY"].ToString());        //基本单位数量
//                double cInvDefine1QTY = Convert.ToDouble(dr["cInvDefine1QTY"].ToString());     //大包装单位数量
//                double cInvDefine2QTY = Convert.ToDouble(dr["cInvDefine2QTY"].ToString());     //小包装单位数量
//                string cn1cComUnitName = dr["cn1cComUnitName"].ToString();    //销售单位名称


//             //   OrderInfo oiEntry = new OrderInfo(lngopOrderId, cinvcode, iquantity, inum, iquotedprice, iunitprice, itaxunitprice, imoney, itax, isum, inatunitprice, inatmoney, inattax, inatsum, kl, itaxrate, cDefine22, iinvexchrate, cunitid, irowno, cinvname, idiscount, inatdiscount, cComUnitName, cInvDefine1, cInvDefine2, cInvDefine13, cInvDefine14, unitGroup, cComUnitQTY, cInvDefine1QTY, cInvDefine2QTY, cn1cComUnitName);

//            //    new OrderManager().DLproc_NewOrderDetailByIns(oiEntry);

//            }
//            ri.flag = "1";
//            ri.message = lngopOrderIdDt.Rows[0]["strBillNo"].ToString();
//            #endregion
//            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
//            return;
//        }
//        #endregion

//        //#region 获取历史订单表头+表体，新
//        //public void DLproc_ReferencePreviousOrderWithCusInvLimitedBySel_new()
//        //{
//        //    DataTable dt = new DataTable();
//        //    DataTable Detail_dt = new DataTable();
//        //    ReInfo ri = new ReInfo();
//        //    dt = new OrderManager().DLproc_ReferencePreviousOrderWithCusInvLimitedBySel(HttpContext.Current.Request.Form["strBillNo"].ToString()); //历史订单基本信息
//        //    ri.dt = dt;
//        //    string ccuscode = ri.dt.Rows[0]["ccuscode"].ToString();
//        //    ri.list_msg = new List<string>();
//        //    ri.flag = "0";
//        //    ri.message = "0";
//        //    int rowNum = 0;

//        //    for (int i = 0; i < ri.dt.Rows.Count; i++)
//        //    {
//        //        if (ri.dt.Rows[i]["bLimited"].ToString() == "0" || ri.dt.Rows[i]["bLimited"].ToString() == "False")
//        //        {
//        //            ri.flag = "2";
//        //            ri.list_msg.Add(ri.dt.Rows[i]["cinvname"].ToString());
//        //        }
//        //        else
//        //        {
//        //            dt = new OrderManager().DLproc_QuasiOrderDetailBySel(ri.dt.Rows[i]["cinvcode"].ToString(), ccuscode);
//        //            if (ri.message != "5") //没有clone表结构时要先clone结构
//        //            {
//        //                Detail_dt = dt.Clone();
//        //                Detail_dt.Columns.Add("cinvdefine13", typeof(string));    //大包装换算率
//        //                Detail_dt.Columns.Add("cinvdefine14", typeof(string));    //小包装换算率
//        //                Detail_dt.Columns.Add("cInvDefine1", typeof(string));    //大包装单位
//        //                Detail_dt.Columns.Add("cInvDefine2", typeof(string));    //小包装单位
//        //                ri.message = "5";
//        //            }
//        //            Detail_dt.Rows.Add(dt.Rows[0].ItemArray);
//        //            if (dt.Rows.Count == 3)
//        //            {
//        //                Detail_dt.Rows[rowNum]["cInvDefine1"] = dt.Rows[2]["cComUnitName"].ToString();//大包装单位
//        //                Detail_dt.Rows[rowNum]["cInvDefine2"] = dt.Rows[1]["cComUnitName"].ToString();//小包装单位
//        //                Detail_dt.Rows[rowNum]["cinvdefine13"] = dt.Rows[2]["iChangRate"].ToString();//大包装换算率
//        //                Detail_dt.Rows[rowNum]["cinvdefine14"] = dt.Rows[1]["iChangRate"].ToString();//小包装换算率
//        //            }
//        //            else if (dt.Rows.Count == 2)
//        //            {
//        //                Detail_dt.Rows[rowNum]["cInvDefine1"] = dt.Rows[1]["cComUnitName"].ToString();
//        //                Detail_dt.Rows[rowNum]["cInvDefine2"] = dt.Rows[1]["cComUnitName"].ToString();
//        //                Detail_dt.Rows[rowNum]["cinvdefine13"] = dt.Rows[1]["iChangRate"].ToString();
//        //                Detail_dt.Rows[rowNum]["cinvdefine14"] = dt.Rows[1]["iChangRate"].ToString();
//        //            }
//        //            else
//        //            {
//        //                Detail_dt.Rows[rowNum]["cInvDefine1"] = dt.Rows[0]["cComUnitName"].ToString();
//        //                Detail_dt.Rows[rowNum]["cInvDefine2"] = dt.Rows[0]["cComUnitName"].ToString();
//        //                Detail_dt.Rows[rowNum]["cinvdefine13"] = dt.Rows[0]["iChangRate"].ToString();
//        //                Detail_dt.Rows[rowNum]["cinvdefine14"] = dt.Rows[0]["iChangRate"].ToString();
//        //            }
//        //            rowNum++;
//        //        }
//        //    }
//        //    ri.msg = new string[6]{
//        //        ri.dt.Rows[0]["strRemarks"].ToString(),       //1、备注
//        //        ri.dt.Rows[0]["cSCCode"].ToString(),          //2、送货方式
//        //        ri.dt.Rows[0]["strLoadingWays"].ToString(),   //3、装车方式
//        //        ri.dt.Rows[0]["cdefine3"].ToString(),        //4、车型下拉
//        //        ri.dt.Rows[0]["lngopUseraddressId"].ToString(), //5、送货地址
//        //        ri.dt.Rows[0]["ccuscode"].ToString()           //开票账户
//        //  };
//        //    ri.dt = null;
//        //    ri.datatable = Detail_dt;
//        //    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
//        //}
//        //#endregion

//        #region 提取临时订单表头+表体明细，新
//        public void DLproc_ReferenceOrderBackWithCusInvLimitedBySel_new()
//        {
//            ReInfo ri = new ReInfo();
//            DataTable Detail_dt = new DataTable();
//            int lngopOrderBackId = Convert.ToInt32(HttpContext.Current.Request.Form["lngopOrderBackId"].ToString());
//            DataTable dt = new OrderManager().DLproc_ReferenceOrderBackWithCusInvLimitedBySel(lngopOrderBackId);
//            if (dt.Rows.Count == 0)
//            {
//                ri.flag = "0";
//                ri.message = "提取临时订单失败，请重试或联系管理员！";
//                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
//                return;
//            }
//            string kpdw = HttpContext.Current.Request.Form["kpdw"];
//            ri.msg = new string[5]{
//                dt.Rows[0]["strRemarks"].ToString(),      //1、备注
//                dt.Rows[0]["cSCCode"].ToString(),        //2、送货方式
//                dt.Rows[0]["strLoadingWays"].ToString(), //3、装车方式
//                dt.Rows[0]["cdefine3"].ToString(),     //4、车型下拉
//                dt.Rows[0]["lngopUseraddressId"].ToString(),//5、送货方式
                
//          };
//            if (string.IsNullOrEmpty(dt.Rows[0]["cinvcode"].ToString()))
//            {
//                ri.flag = "1";
//                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
//                return;
//            }


//            ri.list_dt = new List<DataTable>();
//            ri.list_msg = new List<string>();
//            List<string> codes = new List<string>();
//            if (dt.Rows.Count > 0)
//            {

//                ri.messages = new List<string[]>();
//                for (int i = 0; i < dt.Rows.Count; i++)
//                {

//                    if (!pro.DL_cInvCodeIsPLPBySel(dt.Rows[i]["cinvcode"].ToString(), kpdw,1))
//                    {
//                        ri.list_msg.Add(dt.Rows[i]["cinvname"].ToString());
//                    }
//                    else
//                    {
//                        codes.Add(dt.Rows[i]["cinvcode"].ToString());

//                    }
//                }
//            }

//            ri.dt = DLproc_QuasiOrderDetailBySel_new(codes.ToArray(), kpdw);
//            ri.datatable = dt;
//            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
//            return;
//        }
//        #endregion

//        #region 保存临时订单，新
//        ReInfo DLproc_AddOrderBackByIns_new()
//        {
//            ReInfo ri = new ReInfo();

//            //获取表头数据实例化为model
//            form_Data formData = JsonConvert.DeserializeObject<form_Data>(HttpContext.Current.Request.Form["formData"]);

//            //获取表体数据实例化model
//            List<Buy_list> listData = JsonConvert.DeserializeObject<List<Buy_list>>(HttpContext.Current.Request.Form["listData"]);

//            DataTable dt = new DataTable();

//            if (string.IsNullOrEmpty(formData.lngopUseraddressId) || formData.lngopUseraddressId == "0")
//            {
//                //保存临时订单表头
//                dt = new OrderManager().DLproc_AddOrderBackByIns(
//                                                                          HttpContext.Current.Session["lngopUserId"].ToString(),
//                                                                          HttpContext.Current.Request.Form["temp_name"],
//                                                                          1, //bytStatus
//                                                                          formData.strRemarks,                                   //备注     
//                                                                          formData.ccuscode,                                     //开票单位ID 
//                                                                          "",        //司机姓名 
//                                                                          "",            //司机身份证
//                                                                          formData.carType,                                      //车型
//                                                                          "",     //收货人姓名
//                                                                          "",     //车牌号
//                                                                          formData.txtAddress,                                    //地址信息
//                                                                          "",      //收货人电话
//                                                                          "",         //司机电话
//                                                                          formData.ccusname,                                     //开票单位名称
//                                                                          string.IsNullOrEmpty(formData.ccuspperson) ? "" : formData.ccuspperson,  //业务员编号 
//                                                                          formData.csccode,                                      //送货方式
//                                                                          formData.strLoadingWays,                               //装车方式
//                                                                          "00",                                                  //销售类型编码    
//                                                                          formData.lngopUseraddressId,                           //地址ID
//                                                                          "",                                                    //strTxtRelateU8NO
//                                                                          0                                                     //lngBillType  
//                                                                          );
//            }
//            else
//            {
//                //保存临时订单表头
//                DataTable Address_Dt = new product().Get_AddressById(formData.lngopUseraddressId);
//                dt = new OrderManager().DLproc_AddOrderBackByIns(
//                                                                          HttpContext.Current.Session["lngopUserId"].ToString(),
//                                                                          HttpContext.Current.Request.Form["temp_name"],
//                                                                          1, //bytStatus
//                                                                          formData.strRemarks,                                   //备注     
//                                                                          formData.ccuscode,                                     //开票单位ID 
//                                                                          Address_Dt.Rows[0]["strDriverName"].ToString(),        //司机姓名 
//                                                                          Address_Dt.Rows[0]["strIdCard"].ToString(),            //司机身份证
//                                                                          formData.carType,                                      //车型
//                                                                          Address_Dt.Rows[0]["strConsigneeName"].ToString(),     //收货人姓名
//                                                                          Address_Dt.Rows[0]["strCarplateNumber"].ToString(),     //车牌号
//                                                                          formData.txtAddress,                                    //地址信息
//                                                                          Address_Dt.Rows[0]["strConsigneeTel"].ToString(),      //收货人电话
//                                                                          Address_Dt.Rows[0]["strDriverTel"].ToString(),         //司机电话
//                                                                          formData.ccusname,                                     //开票单位名称
//                                                                          string.IsNullOrEmpty(formData.ccuspperson) ? "" : formData.ccuspperson,    //业务员编号 
//                                                                          formData.csccode,                                      //送货方式
//                                                                          formData.strLoadingWays,                               //装车方式
//                                                                          "00",                                                  //销售类型编码    
//                                                                          formData.lngopUseraddressId,                           //地址ID
//                                                                          "",                                                    //strTxtRelateU8NO
//                                                                          0                                                     //lngBillType  
//                                                                          );
//            }



//            if (dt.Rows.Count == 0)
//            {
//                ri.flag = "0";
//                ri.message = "保存订单失败，请重试或联系管理员！";
//                return ri;
//            }

//            if (listData.Count > 0)
//            {

//                Int32 lngopOrderBackId = Convert.ToInt32(dt.Rows[0]["lngopOrderBackId"].ToString());
//                for (int i = 0; i < listData.Count; i++)
//                {
//                    new OrderManager().DLproc_AddOrderBackDetailByIns(
//                                                                        lngopOrderBackId,
//                                                                        listData[i].cinvcode,
//                                                                        listData[i].cinvname,
//                                                                        listData[i].cComUnitQTY,
//                                                                        listData[i].cInvDefine1QTY,
//                                                                        listData[i].cInvDefine2QTY,
//                                                                        listData[i].iquantity,
//                                                                        0,
//                                                                        listData[i].cDefine22,
//                                                                        listData[i].irowno.ToString()
//                                                                       );
//                }
//            }
//            ri.flag = "1";
//            ri.message = "保存临时订单成功！";
//            return ri;
//        }
//        #endregion

//        #region 获取历史订单列表
//        ReInfo DL_UnauditedOrder_SubBySel()
//        {
//            ReInfo ri = new ReInfo();

//            ri.dt = new OrderManager().DL_UnauditedOrder_SubBySel(1, HttpContext.Current.Session["lngopUserId"].ToString(), HttpContext.Current.Session["lngopUserExId"].ToString());
//            //初始化,设置金额合计启用报价合计还是执行价合计?
//            System.Collections.Hashtable ht = (System.Collections.Hashtable)HttpContext.Current.Session["SysSetting"];
//            if (ht["IsExercisePrice"].ToString() == "0")   //报价金额
//            {
//                ri.message = "0";
//            }
//            else//执行价金额
//            {
//                ri.message = "1";
//            }
//            return ri;
//        }
//        #endregion

//        #region 获取订单明细
//        ReInfo DL_OrderBillBySel()
//        {
//            ReInfo ri = new ReInfo();
//            ri.dt = new OrderManager().DL_OrderBillBySel(HttpContext.Current.Request.Form["strBillNo"]);
//            return ri;
//        }
//        #endregion

//        #region   获取历史订单列表
//        ReInfo DL_PreviousOrderBySel()
//        {
//            ReInfo ri = new ReInfo();
//            string start_time = HttpContext.Current.Request.Form["start_date"];
//            string end_time = HttpContext.Current.Request.Form["end_date"];
//            if (string.IsNullOrEmpty(start_time))
//            {
//                start_time = "2015-12-01";
//            }
//            if (string.IsNullOrEmpty(end_time))
//            {
//                end_time = "2099-01-01";
//            }
//            if (Convert.ToDateTime(start_time) > Convert.ToDateTime(end_time))
//            {
//                ri.flag = "0";
//                ri.message = "结束时间不能大于开始时间！";
//                return ri;
//            }
//            ri.dt = new OrderManager().DL_PreviousOrderBySel(HttpContext.Current.Session["ConstcCusCode"].ToString(), start_time, end_time);
//            return ri;

//        }
//        #endregion

//        #region 根据正式订单号查询历史订单明细
//        ReInfo DL_OrderU8BillBySel()
//        {
//            ReInfo ri = new ReInfo();
//            ri.dt = new OrderManager().DL_OrderU8BillBySel(HttpContext.Current.Request.Form["strBillNo"]);
//            return ri;
//        }
//        #endregion

//        #region 获取待确认订单列表
//        ReInfo DL_UnauditedOrderBySel()
//        {
//            ReInfo ri = new ReInfo();
//            ri.dt = new product().DL_UnauditedOrderBySel(2, HttpContext.Current.Session["ConstcCusCode"].ToString() + "%");
//            return ri;
//        }
//        #endregion

//        #region 根据U8单据号查询明细
//        ReInfo DL_OrderU8BillBySel_new()
//        {
//            ReInfo ri = new ReInfo();
//            ri.dt = new product().DL_OrderU8BillBySel(HttpContext.Current.Request.Form["strBillNo"]);
//            return ri;
//        }
//        #endregion

//        #region //确认未确认的U8订单
//        ReInfo DL_U8OrderBillConfirmByUpd()
//        {
//            ReInfo ri = new ReInfo();
//            bool b = new OrderManager().DL_U8OrderBillConfirmByUpd(HttpContext.Current.Request.Form["strBillNo"]);
//            if (!b)
//            {
//                ri.flag = "0";
//                ri.message = "订单确认失败,请重试或联系管理员!";
//                return ri;
//            }
//            ri.flag = "1";
//            ri.message = "订单确认成功!";
//            return ri;
//        }
//        #endregion

//        #region 获取临时订单列表
//        public void DL_GetOrderBackBySel()
//        {
//            DataTable dt = new DataTable();
//            dt = pro.DL_GetOrderBackBySel(Convert.ToInt32(HttpContext.Current.Session["lngopUserId"].ToString()));
//            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(dt));
//            return;
//        }

//        #endregion

//    }
//}