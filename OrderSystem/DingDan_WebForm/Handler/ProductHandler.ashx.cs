using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Data;
using BLL;
using System.Web.SessionState;
using System.Text;
using Model;
using System.Web.Script.Serialization;
using System.ServiceModel.Web;
using System.Runtime.Serialization.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Configuration;


namespace DingDan_WebForm.Handler
{
    /// <summary>
    /// ProductHandler 的摘要说明
    /// </summary>
    public class ProductHandler : Base
    {
        product pro = new product();
        OrderManager order = new OrderManager();
        Check check = new Check();
        AdminManager am = new AdminManager();
        IsoDateTimeConverter timejson = new IsoDateTimeConverter
        {
            DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss"
        };
        string[] checkAction = {
            "Get_BaseInfo",
            "GetAllBaseInfo",
            "DLproc_NewOrderByIns_new",
            "DLproc_BackOrderandPrvOrdercInvCodeIsBeLimitedBySel",
            "DLproc_ReferencePreviousOrderWithCusInvLimitedBySel",
            "DLproc_QuasiOrderDetailBySel_new",
            "Change_KPDW",
            "Get_Product_List",
            "DL_GeneralPreviousOrderBySel",
            "DL_GetOrderBackBySel",
            "Insert_SampleOrder",
            "Return_Check_Dt",
            "DLproc_QuasiOrderDetailModifyBySel",
            "DLproc_QuasiYOrderDetail_TSBySel",
            "DL_PreOrderTreeBySel",
            "DLproc_NewYYOrderByIns",
            "DLproc_NewYOrderByUpd",
            "DLproc_NewYOrderByIns",
            "DLproc_OrderDetailModifyBySel",
            "DLproc_NewOrderByUpd",
            "Refresh",
             };
        public ReInfo errMsg = new ReInfo()
        {
            list_msg = new List<string>(),
            flag = "0",
            message = "程序出现错误,请重试或联系管理员!!"
        };


        public override void AjaxProcess(HttpContext context)
        {
            ReInfo ri = new ReInfo();
            string Action = HttpContext.Current.Request.Form["Action"];
            if (string.IsNullOrEmpty(Action))
            {
                ri.flag = "0";
                ri.message = "错误的方法!";
                context.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }
            var method = this.GetType().GetMethod(Action);
            if (method == null)
            {
                ri.flag = "0";
                ri.message = "错误的方法!";
                context.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }
            try
            {
                if (Array.IndexOf(checkAction, Action) != -1)
                {
                    ri = check.Check_Time(HttpContext.Current.Session["ConstcCusCode"].ToString());
                    if (ri.flag != "1")
                    {
                        HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                        return;
                    }

                }
                //执行Action方法
                method.Invoke(this, new object[] { });
            }
            catch (Exception err)
            {

                errMsg.list_msg.Add("Action:" + HttpContext.Current.Request.Form["Action"]);
                errMsg.list_msg.Add("ConstcCusCode:" + HttpContext.Current.Session["ConstcCusCode"].ToString());
                errMsg.list_msg.Add(err.ToString());
                check.WriteLog(errMsg);
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(errMsg));
                return;
            }


        }


        #region 测试专用
        public void test()
        {
            string id = HttpContext.Current.Request.Form["id"];
            JObject jo = new JObject();
            jo["Now"] = DateTime.Now.ToString();
            jo["id"] = id;
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
        }
        #endregion

        #region 获取订单初使化时的客户基本信息，如开票单位，信用，车型
        public void Get_BaseInfo()
        {
            try
            {
                ReInfo ri = new ReInfo();
                string ConstcCusCode = HttpContext.Current.Session["ConstcCusCode"].ToString();
                //ri = check.Check_Time(ConstcCusCode);
                //if (ri.flag != "1")
                //{
                //    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                //    return;
                //}
                ri.kpdw_dt = new SearchManager().DL_ComboCustomerBySel(ConstcCusCode + "%");//获取开票单位
                ri.CusCredit_dt = order.DLproc_getCusCreditInfo(ConstcCusCode);  //获取信用

                ri.CarType_dt = new BasicInfoManager().DL_cdefine3BySel();//获取车型
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            }
            catch (Exception err)
            {
                errMsg.list_msg.Add("Action:" + HttpContext.Current.Request.Form["Action"]);
                errMsg.list_msg.Add("ConstcCusCode:" + HttpContext.Current.Session["ConstcCusCode"].ToString());
                errMsg.list_msg.Add(err.ToString());
                check.WriteLog(errMsg);
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(errMsg));
            }
            //ReInfo ri = new ReInfo();
            //ri.kpdw_dt = new SearchManager().DL_ComboCustomerBySel(HttpContext.Current.Session["ConstcCusCode"].ToString() + "%");//获取开票单位
            //ri.CusCredit_dt = order.DLproc_getCusCreditInfo(HttpContext.Current.Session["ConstcCusCode"].ToString());  //获取信用
            //ri.CarType_dt = new BasicInfoManager().DL_cdefine3BySel();//获取车型
            //HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));

        }
        #endregion

        #region 2017-09-05添加，同时获取 开票单位、信用、车型，返回DataSet
        public void GetAllBaseInfo()
        {
            ReInfo ri = new ReInfo();
            string ConstcCusCode = HttpContext.Current.Session["ConstcCusCode"].ToString();
            string lngopUserID = HttpContext.Current.Session["lngopUserId"].ToString();
            //ri = check.Check_Time(ConstcCusCode);
            //if (ri.flag != "1")
            //{
            //    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            //    return;
            //}
            DataSet ds = pro.GetAllBaseInfo(ConstcCusCode, lngopUserID);
            JObject jo = new JObject();
            jo["flag"] = 1;
            jo["DataSet"] = JToken.Parse(JsonConvert.SerializeObject(ds));
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;

        }
        #endregion

        #region 2017-09-06添加，根据iAddressType获取不同的数据，0为所有，1为自提，2为配送，3为自提需配送
        public void GetAddressByType()
        {
            string lngopUserID = HttpContext.Current.Session["lngopUserId"].ToString();
            string AddressType = HttpContext.Current.Request.Form["AddressType"];
            string ccuscode = HttpContext.Current.Session["ConstcCusCode"].ToString();
            JObject jo = new JObject();
            DataSet ds = pro.GetAddressByType(lngopUserID, AddressType, ccuscode);
            jo["flag"] = 1;
            jo["DataSet"] = JToken.Parse(JsonConvert.SerializeObject(ds));
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 提交普通订单，新
        public void DLproc_NewOrderByIns_new()
        {
            try
            {
                ReInfo ri = new ReInfo();
                //ri = check.Check_Time(HttpContext.Current.Session["ConstcCusCode"].ToString());
                //if (ri.flag != "1")
                //{
                //    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                //    return;
                //}
                ri.flag = "1";
                //获取表头数据实例化为model
                form_Data formData = JsonConvert.DeserializeObject<form_Data>(HttpContext.Current.Request.Form["formData"]);
                string kpdw = formData.ccuscode;
                //获取表体数据实例化model
                List<Buy_list> listData = JsonConvert.DeserializeObject<List<Buy_list>>(HttpContext.Current.Request.Form["listData"]);

                //根据表体数据itemid重新生成Table，计算金额，用于验证信用、是否大于库存、是否大于可用量、是否有未填写数量的商品
                DataTable Check_Dt = pro.Get_Check_Dt(listData, kpdw, formData.areaid, formData.cWhCode); //用于验证的table
                DataTable dt = new DataTable();

                #region 验证表单开始

                //验证商品允限销
                ri = check.Check_limit(Check_Dt, kpdw, 1);
                if (ri.flag != "1")
                {
                    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                    return;
                }

                dt = order.DLproc_getCusCreditInfo(kpdw);// 重新获取信用
                double CusCredit = Convert.ToDouble(dt.Rows[0]["iCusCreLine"].ToString()); //客户信用
                double listCredit = 0;   //用于累计表单金额
                int rowType = 0; //用于记录表体中是否有不合格数据
                ri.list_msg = new List<string>();
                foreach (DataRow dr in Check_Dt.Rows)
                {
                    listCredit += Convert.ToDouble(dr["iquantity"].ToString()) * Convert.ToDouble(dr["ExercisePrice"].ToString());
                    if (Convert.ToDouble(dr["fAvailQtty"].ToString()) == 0)
                    {
                        dr["rowType"] = 1;
                    }
                    else if (Convert.ToDouble(dr["iquantity"].ToString()) > Convert.ToDouble(dr["fAvailQtty"].ToString()))
                    {
                        dr["rowType"] = 2;
                    }

                    rowType += Convert.ToInt32(dr["rowType"].ToString());
                }
                if (CusCredit != -99999999.000000)  //-99999999.000000为现金用户
                {
                    if (listCredit - CusCredit > 0)
                    {
                        ri.flag = "0";
                        ri.list_msg.Add("你的购买金额已超过信用" + Math.Round((listCredit - CusCredit), 2));

                    }
                    if (rowType != 0)
                    {
                        ri.flag = "0";
                        ri.list_msg.Add("订单列表里有产品数量为0或者大于库存，请重新输入!");
                    }
                    if (ri.flag == "0")
                    {
                        ri.dt = Check_Dt;
                        HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                        return;
                    }
                }
                else
                {
                    if (rowType != 0)
                    {
                        ri.flag = "0";
                        ri.list_msg.Add("订单列表里有产品数量为0或者大于库存，请重新输入!");

                    }
                    if (ri.flag == "0")
                    {
                        ri.dt = Check_Dt;
                        HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                        return;
                    }
                }
                #endregion

                //验证通过后，开始提交表单
                #region 获取表头信息，提交表头数据
                DataTable Address_Dt = new product().Get_AddressById(formData.lngopUseraddressId);
                OrderInfo oi = new OrderInfo(
                                                HttpContext.Current.Session["lngopUserId"].ToString(), //客户ID
                                                DateTime.Now.ToString(),                               //订单时间                     
                                                1,                                                     //订单状态         
                                                formData.strRemarks,                                   //备注     
                                                formData.ccuscode,                                     //开票单位ID 
                                                Address_Dt.Rows[0]["strDriverName"].ToString(),        //司机姓名 
                                                Address_Dt.Rows[0]["strIdCard"].ToString(),            //司机身份证
                                                formData.carType,                                      //车型
                                                Address_Dt.Rows[0]["strConsigneeName"].ToString(),     //收货人姓名
                                                Address_Dt.Rows[0]["strCarplateNumber"].ToString(),     //车牌号
                                                formData.txtAddress,                                    //地址信息
                                                Address_Dt.Rows[0]["strConsigneeTel"].ToString(),      //收货人电话
                                                Address_Dt.Rows[0]["strDriverTel"].ToString(),         //司机电话
                                                formData.ccusname,                                     //开票单位名称
                                                formData.ccuspperson,                                  //业务员编号 
                                                formData.csccode,                                      //送货方式
                                                formData.datDeliveryDate,                              //提货时间
                                                formData.strLoadingWays,                               //装车方式
                                                "00",                                                  //销售类型编码    
                                                formData.lngopUseraddressId,                           //地址ID
                                                "",                                                   //strTxtRelateU8NO
                                                dt.Rows[0]["cdiscountname"].ToString(),                //lngBillType 特殊订单
                                                formData.txtArea,                                      //行政区
                                                HttpContext.Current.Session["lngopUserExId"].ToString(),//用户ExID
                                                HttpContext.Current.Session["strAllAcount"].ToString() //子账号编码

                                           );

                //#region 测试事务提交订单
                //JObject jo = new JObject();
                //JObject HeadData = JObject.Parse(HttpContext.Current.Request.Form["formData"]);
                //HeadData["lngopUserId"] = HttpContext.Current.Session["lngopUserId"].ToString();
                //HeadData["bytStatus"] = 2;
                //HeadData["lngopUserExId"] = HttpContext.Current.Session["lngopUserExId"].ToString();
                //HeadData["strallAcount"] = HttpContext.Current.Session["strAllAcount"].ToString();
                //HeadData["strU8BillNo"] = "";
                //HeadData["cdiscountname"] = order.DLproc_getCusCreditInfo(HeadData["ccuscode"].ToString()).Rows[0]["cdiscountname"].ToString();
                //HeadData["cdefine1"] = Address_Dt.Rows[0]["strDriverName"].ToString();
                //HeadData["cdefine2"] = Address_Dt.Rows[0]["strIdCard"].ToString();
                //HeadData["cdefine9"] = Address_Dt.Rows[0]["strConsigneeName"].ToString();
                //HeadData["cdefine10"] = Address_Dt.Rows[0]["strCarplateNumber"].ToString();
                //HeadData["cdefine12"] = Address_Dt.Rows[0]["strConsigneeTel"].ToString();
                //HeadData["cdefine13"] = Address_Dt.Rows[0]["strDriverTel"].ToString();
                //jo = am.DLproc_NewOrderByIns_Admin("DLproc_NewOrderByIns_Admin", HeadData);
                //#endregion


                DataTable lngopOrderIdDt = order.DLproc_NewOrderByIns(oi, formData.areaid, formData.iaddresstype, formData.chdefine21, formData.cWhCode);

                if (lngopOrderIdDt.Rows.Count < 0)
                {
                    ri.flag = "0";
                    ri.list_msg.Add("订单提交出错，请重试或联系管理员！");
                    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                    return;
                }
                #endregion

                #region 提交表体数据
                int lngopOrderId = Convert.ToInt32(lngopOrderIdDt.Rows[0]["lngopOrderId"].ToString());
                Insert_listData(Check_Dt, lngopOrderId);
              
                ri.flag = "1";
                ri.message = lngopOrderIdDt.Rows[0]["strBillNo"].ToString();
                #endregion
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }
            catch (Exception err)
            {

                errMsg.list_msg.Add("Action:" + HttpContext.Current.Request.Form["Action"]);
                errMsg.list_msg.Add("ConstcCusCode:" + HttpContext.Current.Session["ConstcCusCode"].ToString());
                errMsg.list_msg.Add(err.ToString());
                check.WriteLog(errMsg);
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(errMsg));
                return;
            }

        }
        #endregion

        #region 获取历史订单表头+表体
        public void DLproc_ReferencePreviousOrderWithCusInvLimitedBySel()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = order.DLproc_BackOrderandPrvOrdercInvCodeIsBeLimitedBySel(HttpContext.Current.Request.Form["lngoporderid"].ToString(), 1, 2);
                ReInfo ri = new ReInfo();
                ri.flag = "0";
                ri.list_dt = new List<DataTable>();
                ri.list_msg = new List<string>();

                string kpdw = dt.Rows[0]["ccuscode"].ToString();

                if (!string.IsNullOrEmpty(kpdw))
                {
                    ri.CusCredit_dt = order.DLproc_getCusCreditInfo(kpdw);  //获取信用
                }
                ri.msg = new string[6]{
                dt.Rows[0]["strRemarks"].ToString(),      //1、备注
                dt.Rows[0]["cSCCode"].ToString(),        //2、送货方式
                dt.Rows[0]["strLoadingWays"].ToString(), //3、装车方式
                dt.Rows[0]["cdefine3"].ToString(),     //4、车型下拉
                dt.Rows[0]["lngopUseraddressId"].ToString(),//5、送货方式
                dt.Rows[0]["ccuscode"].ToString()        //6、 开票单位
                
          };
                List<string> codes = new List<string>();
                if (dt.Rows.Count > 0)
                {

                    ri.messages = new List<string[]>();

                    //for (int i = 0; i < dt.Rows.Count; i++)
                    //{

                    //    if (!pro.DLproc_cInvCodeIsBeLimitedBySel(dt.Rows[i]["cinvcode"].ToString(), kpdw,1))
                    //    {
                    //        ri.list_msg.Add(dt.Rows[i]["cinvname"].ToString());
                    //    }
                    //    else
                    //    {
                    //        codes.Add(dt.Rows[i]["cinvcode"].ToString());

                    //    }
                    //}
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["bLimited"].ToString() != "1")
                        {

                            ri.list_msg.Add(dt.Rows[i]["cinvname"].ToString());
                            dt.Rows[i].Delete();
                        }
                        else
                        {
                            codes.Add(dt.Rows[i]["cinvcode"].ToString());

                        }
                    }
                    dt.AcceptChanges();
                }

                ri.dt = DLproc_QuasiOrderDetailBySel_new(codes.ToArray(), kpdw);
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }
            catch (Exception err)
            {
                ReInfo ri = new ReInfo();
                ri.flag = "4";
                ri.message = "程序出现错误,请联系管理员!";
                errMsg.list_msg.Add("Action:" + HttpContext.Current.Request.Form["Action"]);
                errMsg.list_msg.Add("ConstcCusCode:" + HttpContext.Current.Session["ConstcCusCode"].ToString());
                errMsg.list_msg.Add(err.ToString());
                check.WriteLog(errMsg);
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }

        }
        #endregion

        #region 提取临时订单表头+表体明细，新
        public void DLproc_BackOrderandPrvOrdercInvCodeIsBeLimitedBySel()
        {
            ReInfo ri = new ReInfo();
            DataTable Detail_dt = new DataTable();
            string lngopOrderBackId = HttpContext.Current.Request.Form["lngopOrderBackId"].ToString();
            DataTable dt = order.DLproc_BackOrderandPrvOrdercInvCodeIsBeLimitedBySel(lngopOrderBackId, 1, 1);
            if (dt.Rows.Count == 0)
            {
                ri.flag = "0";
                ri.message = "提取临时订单失败，请重试或联系管理员！";
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }
            string kpdw = dt.Rows[0]["ccuscode"].ToString();
            string cWhCode = dt.Rows[0]["cWhCode"].ToString();
            if (cWhCode == "" || cWhCode == null)
            {
                cWhCode = "CD01";
            }
            if (!string.IsNullOrEmpty(kpdw))
            {
                ri.CusCredit_dt = order.DLproc_getCusCreditInfo(kpdw);  //获取信用
            }
            ri.msg = new string[6]{
                dt.Rows[0]["strRemarks"].ToString(),      //1、备注
                dt.Rows[0]["cSCCode"].ToString(),        //2、送货方式
                dt.Rows[0]["strLoadingWays"].ToString(), //3、装车方式
                dt.Rows[0]["cdefine3"].ToString(),     //4、车型下拉
                dt.Rows[0]["lngopUseraddressId"].ToString(),//5、送货方式
                dt.Rows[0]["ccuscode"].ToString()        //6、 开票单位
                
          };
            if (string.IsNullOrEmpty(dt.Rows[0]["cinvcode"].ToString()) || string.IsNullOrEmpty(kpdw))
            {
                ri.flag = "1";
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }


            ri.list_dt = new List<DataTable>();
            ri.list_msg = new List<string>();
            List<string> codes = new List<string>();
            ri.limit_name = new List<string>();
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    if (!pro.DLproc_cInvCodeIsBeLimitedBySel(dt.Rows[i]["cinvcode"].ToString(), kpdw, 1)) {

            //        ri.limit_name.Add(dt.Rows[i]["cinvname"].ToString());
            //    }
            //           else
            //    {
            //        codes.Add(dt.Rows[i]["cinvcode"].ToString());

            //    }
            //}
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["bLimited"].ToString() != "1")
                {

                    ri.limit_name.Add(dt.Rows[i]["cinvname"].ToString());
                    dt.Rows[i].Delete();
                }
                else
                {
                    codes.Add(dt.Rows[i]["cinvcode"].ToString());

                }
            }
            dt.AcceptChanges();
            ri.dt = DLproc_QuasiOrderDetailBySel_new(codes.ToArray(), kpdw);
            ri.datatable = dt;
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            return;
        }
        #endregion

        #region 手动保存临时订单，新
        public void DLproc_AddOrderBackByIns_new()
        {
            ReInfo ri = new ReInfo();

            //获取表头数据实例化为model
            form_Data formData = JsonConvert.DeserializeObject<form_Data>(HttpContext.Current.Request.Form["formData"]);

            //获取表体数据实例化model
            List<Buy_list> listData = JsonConvert.DeserializeObject<List<Buy_list>>(HttpContext.Current.Request.Form["listData"]);

            DataTable dt = new DataTable();

            if (string.IsNullOrEmpty(formData.lngopUseraddressId) || formData.lngopUseraddressId == "0")
            {
                //保存临时订单表头
                dt = order.DLproc_AddOrderBackByIns(
                                                                          HttpContext.Current.Session["lngopUserId"].ToString(),
                                                                          HttpContext.Current.Request.Form["temp_name"],
                                                                          11, //bytStatus
                                                                          formData.strRemarks,                                   //备注     
                                                                          formData.ccuscode,                                     //开票单位ID 
                                                                          "",        //司机姓名 
                                                                          "",            //司机身份证
                                                                          formData.carType,                                      //车型
                                                                          "",     //收货人姓名
                                                                          "",     //车牌号
                                                                          formData.txtAddress,                                    //地址信息
                                                                          "",      //收货人电话
                                                                          "",         //司机电话
                                                                          formData.ccusname,                                     //开票单位名称
                                                                          string.IsNullOrEmpty(formData.ccuspperson) ? "" : formData.ccuspperson,  //业务员编号 
                                                                          formData.csccode,                                      //送货方式
                                                                          formData.strLoadingWays,                               //装车方式
                                                                          "00",                                                  //销售类型编码    
                                                                          formData.lngopUseraddressId,                           //地址ID
                                                                          "",                                                    //strTxtRelateU8NO
                                                                          0,                                                    //lngBillType  
                                                                           formData.cWhCode                                       //仓库编码
                                                                          );
            }
            else
            {
                //保存临时订单表头
                DataTable Address_Dt = new product().Get_AddressById(formData.lngopUseraddressId);
                dt = order.DLproc_AddOrderBackByIns(
                                                                          HttpContext.Current.Session["lngopUserId"].ToString(),
                                                                          HttpContext.Current.Request.Form["temp_name"],
                                                                          11, //bytStatus
                                                                          formData.strRemarks,                                   //备注     
                                                                          formData.ccuscode,                                     //开票单位ID 
                                                                          Address_Dt.Rows[0]["strDriverName"].ToString(),        //司机姓名 
                                                                          Address_Dt.Rows[0]["strIdCard"].ToString(),            //司机身份证
                                                                          formData.carType,                                      //车型
                                                                          Address_Dt.Rows[0]["strConsigneeName"].ToString(),     //收货人姓名
                                                                          Address_Dt.Rows[0]["strCarplateNumber"].ToString(),     //车牌号
                                                                          formData.txtAddress,                                    //地址信息
                                                                          Address_Dt.Rows[0]["strConsigneeTel"].ToString(),      //收货人电话
                                                                          Address_Dt.Rows[0]["strDriverTel"].ToString(),         //司机电话
                                                                          formData.ccusname,                                     //开票单位名称
                                                                          string.IsNullOrEmpty(formData.ccuspperson) ? "" : formData.ccuspperson,    //业务员编号 
                                                                          formData.csccode,                                      //送货方式
                                                                          formData.strLoadingWays,                               //装车方式
                                                                          "00",                                                  //销售类型编码    
                                                                          formData.lngopUseraddressId,                           //地址ID
                                                                          "",                                                    //strTxtRelateU8NO
                                                                          0,                                                     //lngBillType  
                                                                          formData.cWhCode                                       //仓库编码
                                                                          );
            }



            if (dt.Rows.Count == 0)
            {
                ri.flag = "0";
                ri.message = "保存订单失败，请重试或联系管理员！";
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }

            if (listData.Count > 0)
            {

                Int32 lngopOrderBackId = Convert.ToInt32(dt.Rows[0]["lngopOrderBackId"].ToString());
                for (int i = 0; i < listData.Count; i++)
                {
                    order.DLproc_AddOrderBackDetailByIns(
                                                                        lngopOrderBackId,
                                                                        listData[i].cinvcode,
                                                                        listData[i].cinvname,
                                                                        listData[i].cComUnitQTY,
                                                                        listData[i].cInvDefine1QTY,
                                                                        listData[i].cInvDefine2QTY,
                                                                        listData[i].iquantity,
                                                                        0,
                                                                        listData[i].cDefine22,
                                                                        listData[i].irowno.ToString()
                                                                       );
                }
            }
            ri.flag = "1";
            ri.message = "保存临时订单成功！";
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            return;
        }
        #endregion

        #region 自动保存临时订单
        public void DLproc_AddOrderBackByIns_auto()
        {
            ReInfo ri = new ReInfo();

            //获取表头数据实例化为model
            form_Data formData = JsonConvert.DeserializeObject<form_Data>(HttpContext.Current.Request.Form["formData"]);

            //获取表体数据实例化model
            List<Buy_list> listData = JsonConvert.DeserializeObject<List<Buy_list>>(HttpContext.Current.Request.Form["listData"]);

            DataTable dt = new DataTable();

            if (string.IsNullOrEmpty(formData.lngopUseraddressId) || formData.lngopUseraddressId == "0")
            {
                //保存临时订单表头
                dt = order.DLproc_AddOrderBackByIns(
                                                                          HttpContext.Current.Session["lngopUserId"].ToString(),
                                                                          HttpContext.Current.Request.Form["temp_name"],
                                                                          12,                                                    //bytStatus
                                                                          formData.strRemarks,                                   //备注     
                                                                          formData.ccuscode,                                     //开票单位ID 
                                                                          "",        //司机姓名 
                                                                          "",            //司机身份证
                                                                          formData.carType,                                      //车型
                                                                          "",     //收货人姓名
                                                                          "",     //车牌号
                                                                          formData.txtAddress,                                    //地址信息
                                                                          "",      //收货人电话
                                                                          "",         //司机电话
                                                                          formData.ccusname,                                     //开票单位名称
                                                                          string.IsNullOrEmpty(formData.ccuspperson) ? "" : formData.ccuspperson,  //业务员编号 
                                                                          formData.csccode,                                      //送货方式
                                                                          formData.strLoadingWays,                               //装车方式
                                                                          "00",                                                  //销售类型编码    
                                                                          formData.lngopUseraddressId,                           //地址ID
                                                                          "",                                                    //strTxtRelateU8NO
                                                                          0,                                                     //lngBillType  
                                                                           formData.cWhCode                                       //仓库编码
                                                                          );
            }
            else
            {
                //保存临时订单表头
                DataTable Address_Dt = new product().Get_AddressById(formData.lngopUseraddressId);
                dt = order.DLproc_AddOrderBackByIns(
                                                                          HttpContext.Current.Session["lngopUserId"].ToString(),
                                                                          HttpContext.Current.Request.Form["temp_name"],
                                                                          12, //bytStatus
                                                                          formData.strRemarks,                                   //备注     
                                                                          formData.ccuscode,                                     //开票单位ID 
                                                                          Address_Dt.Rows[0]["strDriverName"].ToString(),        //司机姓名 
                                                                          Address_Dt.Rows[0]["strIdCard"].ToString(),            //司机身份证
                                                                          formData.carType,                                      //车型
                                                                          Address_Dt.Rows[0]["strConsigneeName"].ToString(),     //收货人姓名
                                                                          Address_Dt.Rows[0]["strCarplateNumber"].ToString(),     //车牌号
                                                                          formData.txtAddress,                                    //地址信息
                                                                          Address_Dt.Rows[0]["strConsigneeTel"].ToString(),      //收货人电话
                                                                          Address_Dt.Rows[0]["strDriverTel"].ToString(),         //司机电话
                                                                          formData.ccusname,                                     //开票单位名称
                                                                          string.IsNullOrEmpty(formData.ccuspperson) ? "" : formData.ccuspperson,    //业务员编号 
                                                                          formData.csccode,                                      //送货方式
                                                                          formData.strLoadingWays,                               //装车方式
                                                                          "00",                                                  //销售类型编码    
                                                                          formData.lngopUseraddressId,                           //地址ID
                                                                          "",                                                    //strTxtRelateU8NO
                                                                          0,                                                     //lngBillType  
                                                                          formData.cWhCode                                       //仓库编码
                                                                          );
            }



            if (dt.Rows.Count == 0)
            {
                ri.flag = "0";
                ri.message = "保存订单失败，请重试或联系管理员！";
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }

            if (listData.Count > 0)
            {

                Int32 lngopOrderBackId = Convert.ToInt32(dt.Rows[0]["lngopOrderBackId"].ToString());
                for (int i = 0; i < listData.Count; i++)
                {
                    order.DLproc_AddOrderBackDetailByIns(
                                                                        lngopOrderBackId,
                                                                        listData[i].cinvcode,
                                                                        listData[i].cinvname,
                                                                        listData[i].cComUnitQTY,
                                                                        listData[i].cInvDefine1QTY,
                                                                        listData[i].cInvDefine2QTY,
                                                                        listData[i].iquantity,
                                                                        0,
                                                                        listData[i].cDefine22,
                                                                        listData[i].irowno.ToString()
                                                                       );
                }
            }

            new OrderManager().DLproc_DelAutoSaveOrderBackByDel(Convert.ToInt32(HttpContext.Current.Session["lngopUserId"].ToString()));
            ri.flag = "1";
            ri.message = "于" + DateTime.Now.ToString() + "自动保存临时订单成功！";
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            return;
        }
        #endregion

        #region 返回普通订单产品详细列表，新
        public void DLproc_QuasiOrderDetailBySel_new()
        {
            ReInfo ri = new ReInfo();
            string code = HttpContext.Current.Request.Form["codes"];
            DataTable dt = new DataTable();
            DataTable new_dt = new DataTable();
            string kpdw = HttpContext.Current.Request.Form["kpdw"];
            string strBillNo = HttpContext.Current.Request.Form["strBillNo"];
            string areaid = HttpContext.Current.Request.Form["areaid"];
            if (string.IsNullOrEmpty(kpdw) || kpdw == "0")
            {
                ri.flag = "0";
                ri.message = "请先选择开票单位!";
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }
            string isModify = HttpContext.Current.Request.Form["isModify"];

            if (!string.IsNullOrEmpty(code)) //判断数组是否为空
            {
                string[] codes = code.Split(',');

                for (int i = 0; i < codes.Length; i++)
                {
                    if (isModify == "1")
                    {
                        dt = order.DLproc_QuasiOrderDetailModifyBySel(codes[i], kpdw, strBillNo, areaid);
                    }
                    else
                    {
                        dt = pro.DLproc_QuasiOrderDetailBySel(codes[i], kpdw, areaid);
                    }
                    if (i == 0)
                    {
                        new_dt = dt.Clone();
                        new_dt.Columns.Add("cinvdefine13", typeof(float)); //大包装换算
                        new_dt.Columns.Add("cinvdefine14", typeof(float));//小包装换算
                        new_dt.Columns.Add("cInvDefine1", typeof(string));//大包装单位
                        new_dt.Columns.Add("cInvDefine2", typeof(string));//小包装单位

                    }
                    new_dt.Rows.Add(dt.Rows[0].ItemArray);
                    if (dt.Rows.Count >= 3)
                    {

                        new_dt.Rows[i]["cinvdefine13"] = Convert.ToDouble(dt.Rows[2]["iChangRate"].ToString()).ToString();
                        new_dt.Rows[i]["cinvdefine14"] = Convert.ToDouble(dt.Rows[1]["iChangRate"].ToString()).ToString();
                        new_dt.Rows[i]["cInvDefine2"] = dt.Rows[1]["cComUnitName"].ToString();
                        new_dt.Rows[i]["cInvDefine1"] = dt.Rows[2]["cComUnitName"].ToString();
                    }
                    else if (dt.Rows.Count == 2)
                    {
                        new_dt.Rows[i]["cinvdefine13"] = Convert.ToDouble(dt.Rows[1]["iChangRate"].ToString()).ToString();
                        new_dt.Rows[i]["cinvdefine14"] = Convert.ToDouble(dt.Rows[1]["iChangRate"].ToString()).ToString();
                        new_dt.Rows[i]["cInvDefine2"] = dt.Rows[1]["cComUnitName"].ToString();
                        new_dt.Rows[i]["cInvDefine1"] = dt.Rows[1]["cComUnitName"].ToString();
                    }
                    else if (dt.Rows.Count == 1)
                    {
                        new_dt.Rows[i]["cinvdefine13"] = Convert.ToDouble(dt.Rows[0]["iChangRate"].ToString()).ToString();
                        new_dt.Rows[i]["cinvdefine14"] = Convert.ToDouble(dt.Rows[0]["iChangRate"].ToString()).ToString();
                        new_dt.Rows[i]["cInvDefine2"] = dt.Rows[0]["cComUnitName"].ToString();
                        new_dt.Rows[i]["cInvDefine1"] = dt.Rows[0]["cComUnitName"].ToString();
                    }


                }

            }

            ri.flag = "1";
            ri.dt = new_dt;
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            return;
        }
        #endregion

        #region 获取地址下拉菜单信息
        public void DLproc_UserAddressZTBySelGroup()
        {
            ReInfo ri = new ReInfo();
            ri.list_dt = new List<DataTable>();
            string shfs = HttpContext.Current.Request.Form["shfs"];
            if (shfs == "自提")
            {
                ri.list_dt.Add(new BasicInfoManager().DLproc_UserAddressZTBySelGroup(HttpContext.Current.Session["lngopUserId"].ToString()));
            }
            else
            {
                ri.list_dt.Add(new BasicInfoManager().DLproc_UserAddressPSBySelGroup(HttpContext.Current.Session["lngopUserId"].ToString()));
            }
            ri.list_dt.Add(new BasicInfoManager().DL_UserAddressZTXZQBySel(HttpContext.Current.Session["ConstcCusCode"].ToString()));
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
        }
        #endregion

        #region 返回普通订单产品详细列表，新
        private DataTable DLproc_QuasiOrderDetailBySel_new(string[] codes, string kpdw)
        {
            DataTable dt = new DataTable();
            DataTable new_dt = new DataTable();
            for (int i = 0; i < codes.Length; i++)
            {
                dt = pro.DLproc_QuasiOrderDetailBySel(codes[i], kpdw, "0");
                if (i == 0)
                {
                    new_dt = dt.Clone();
                    new_dt.Columns.Add("cinvdefine13", typeof(float)); //大包装换算
                    new_dt.Columns.Add("cinvdefine14", typeof(float));//小包装换算
                    new_dt.Columns.Add("cInvDefine1", typeof(string));//大包装单位
                    new_dt.Columns.Add("cInvDefine2", typeof(string));//小包装单位

                }
                new_dt.Rows.Add(dt.Rows[0].ItemArray);
                if (dt.Rows.Count >= 3)
                {
                    new_dt.Rows[i]["cinvdefine13"] = Convert.ToDouble(dt.Rows[2]["iChangRate"].ToString()).ToString();
                    new_dt.Rows[i]["cinvdefine14"] = Convert.ToDouble(dt.Rows[1]["iChangRate"].ToString()).ToString();
                    new_dt.Rows[i]["cInvDefine2"] = dt.Rows[1]["cComUnitName"].ToString();
                    new_dt.Rows[i]["cInvDefine1"] = dt.Rows[2]["cComUnitName"].ToString();
                }
                else if (dt.Rows.Count == 2)
                {
                    new_dt.Rows[i]["cinvdefine13"] = Convert.ToDouble(dt.Rows[1]["iChangRate"].ToString()).ToString();
                    new_dt.Rows[i]["cinvdefine14"] = Convert.ToDouble(dt.Rows[1]["iChangRate"].ToString()).ToString();
                    new_dt.Rows[i]["cInvDefine2"] = dt.Rows[1]["cComUnitName"].ToString();
                    new_dt.Rows[i]["cInvDefine1"] = dt.Rows[1]["cComUnitName"].ToString();
                }
                else if (dt.Rows.Count == 1)
                {
                    new_dt.Rows[i]["cinvdefine13"] = Convert.ToDouble(dt.Rows[0]["iChangRate"].ToString()).ToString();
                    new_dt.Rows[i]["cinvdefine14"] = Convert.ToDouble(dt.Rows[0]["iChangRate"].ToString()).ToString();
                    new_dt.Rows[i]["cInvDefine2"] = dt.Rows[0]["cComUnitName"].ToString();
                    new_dt.Rows[i]["cInvDefine1"] = dt.Rows[0]["cComUnitName"].ToString();
                }
            }
            return new_dt;
        }
        #endregion

        #region 前台更换开票单位后，重新获取数据
        public void Change_KPDW()
        {
            try
            {
                ReInfo ri = new ReInfo();
                string kpdw = HttpContext.Current.Request.Form["kpdw"];
                string c = HttpContext.Current.Request.Form["codes"];
                string areaid = HttpContext.Current.Request.Form["areaid"];
                string cWhCode = HttpContext.Current.Request.Form["cWhCode"];
                string strBillNo = HttpContext.Current.Request.Form["strBillNo"];
                //判断切换后的仓库是否可选，若没在开票单位的可选仓库里，则重置为“CD01”
                DataTable WareHouse_Dt = pro.GetWareHouseBycCusCode(kpdw);
                List<string> WareHouse_Arr = new List<string>();
                foreach (DataRow dr in WareHouse_Dt.Rows)
                {
                    WareHouse_Arr.Add(dr["cWhCode"].ToString());
                }
                if (Array.IndexOf(WareHouse_Arr.ToArray(), cWhCode) == -1)
                {
                    cWhCode = "CD01";

                }
                ri.cWhCode = cWhCode;
                ri.WareHouse_dt = WareHouse_Dt;
                ri.list_dt = new List<DataTable>();
                ri.list_msg = new List<string>();
                DataTable dt = pro.DL_getCusCreditInfo(kpdw);
                if (dt.Rows.Count > 0)
                {
                    ri.message = dt.Rows[0]["CusCredit"].ToString();
                }

                if (HttpContext.Current.Request.Form["isModify"] == "0" && HttpContext.Current.Request.Form["isSpecial"] == "0")
                {
                    ri.CusCredit_dt = order.DLproc_getCusCreditInfo(kpdw);

                    if (!string.IsNullOrEmpty(c))
                    {
                        ri.messages = new List<string[]>();
                        string[] code = c.Split(',');
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < code.Length; i++)
                        {

                            if (!pro.DLproc_cInvCodeIsBeLimitedBySel(code[i], kpdw, 1))
                            {
                                ri.list_msg.Add(code[i]);
                            }
                            else
                            {
                                sb.Append(code[i] + "|");

                            }
                        }

                        ri.datatable = pro.DLproc_QuasiOrderDetail_All_Warehouse_BySel(sb.ToString().TrimEnd('|'), kpdw, areaid, cWhCode);

                    }
                }
                else if (HttpContext.Current.Request.Form["isModify"] == "1" && HttpContext.Current.Request.Form["isSpecial"] == "0")
                {
                    ri.CusCredit_dt = order.DLproc_getCusCreditInfoWithBillno(kpdw, strBillNo);
                    if (!string.IsNullOrEmpty(c))
                    {
                        ri.messages = new List<string[]>();
                        string[] code = c.Split(',');
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < code.Length; i++)
                        {
                            if (!pro.DLproc_cInvCodeIsBeLimitedBySel(code[i], kpdw, 1))
                            {
                                ri.list_msg.Add(code[i]);
                            }
                            else
                            {
                                sb.Append(code[i] + "|");
                            }
                        }

                        ri.datatable = pro.DLproc_QuasiOrderDetailModify_All_Warehouse_BySel(strBillNo, sb.ToString().TrimEnd('|'), kpdw, cWhCode, areaid);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(c))
                    {
                        ri.messages = new List<string[]>();
                        string[] code = c.Split(',');
                        for (int i = 0; i < code.Length; i++)
                        {

                            if (!pro.DLproc_cInvCodeIsBeLimitedBySel(code[i], kpdw, 1))
                            {
                                ri.list_msg.Add(code[i]);
                            }
                            else
                            {
                                ri.list_dt.Add(pro.DLproc_QuasiOrderDetailBySel(code[i], kpdw, areaid));
                            }
                        }
                    }
                }


                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }
            catch (Exception err)
            {
                ReInfo ri = new ReInfo();
                ri.flag = "4";
                ri.message = "程序出现错误,请联系管理员!";
                errMsg.list_msg.Add("Action:" + HttpContext.Current.Request.Form["Action"]);
                errMsg.list_msg.Add("ConstcCusCode:" + HttpContext.Current.Session["ConstcCusCode"].ToString());
                errMsg.list_msg.Add(err.ToString());
                check.WriteLog(errMsg);
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }
        }

        #endregion

        #region 2017-09-15 前台更换开票单位或行政区后，重新获取数据
        public void Change_KPDW_new()
        {

            JObject jo = new JObject();
            string kpdw = HttpContext.Current.Request.Form["kpdw"];
            string c = HttpContext.Current.Request.Form["codes"];
            string areaid = HttpContext.Current.Request.Form["areaid"];

            List<string> list_msg = new List<string>();

            DataTable list_dt = new DataTable();
            DataTable dt = new DataTable();
            list_dt.Columns.AddRange(new DataColumn[]{
                new DataColumn ("code",typeof(string)),
                new DataColumn ("fAvailQtty",typeof(string)),
                new DataColumn("ExercisePrice",typeof(string))
            });


            if (!string.IsNullOrEmpty(c))
            {
                string[] code = c.Split(',');
                for (int i = 0; i < code.Length; i++)
                {
                    if (!pro.DLproc_cInvCodeIsBeLimitedBySel(code[i], kpdw, 1))
                    {
                        list_msg.Add(code[i]);
                    }
                    else
                    {
                        dt = pro.DLproc_QuasiOrderDetailBySel(code[i], kpdw, areaid);
                        list_dt.Rows.Add();
                        int len = list_dt.Rows.Count - 1;
                        list_dt.Rows[len]["code"] = dt.Rows[0]["cInvCode"];
                        list_dt.Rows[len]["fAvailQtty"] = dt.Rows[0]["fAvailQtty"];
                        list_dt.Rows[len]["ExercisePrice"] = dt.Rows[0]["ExercisePrice"];
                    }
                }
            }
            jo["flag"] = 1;
            jo["list_msg"] = JToken.Parse(JsonConvert.SerializeObject(list_msg));
            jo["list_dt"] = JToken.Parse(JsonConvert.SerializeObject(list_dt));
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;



        }

        #endregion

        #region 根据分类获取商品列表
        /// <summary>
        /// 根据分类获取商品列表
        /// </summary>
        /// <param name="ConstcCusCode"></param>
        /// <returns></returns>
        public void Get_Product_List()
        {
            try
            {
                ReInfo ri = new ReInfo();
                string ConstcCusCode = HttpContext.Current.Session["ConstcCusCode"].ToString();
                string cInvCCode = HttpContext.Current.Request.Form["cInvCCode"].ToString();
                string kpdw = HttpContext.Current.Request.Form["kpdw"];
                string iShowType = HttpContext.Current.Request.Form["iShowType"];
                if (string.IsNullOrEmpty(kpdw) || kpdw == "0")
                {
                    ri.flag = "0";
                    ri.message = "请先选择开票单位!";
                    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                    return;
                }
                DataTable dt = pro.DL_TreeListDetailsAllBySel(cInvCCode, kpdw, iShowType);
                ri.dt = dt;
                ri.flag = "1";
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }
            catch (Exception err)
            {

                errMsg.list_msg.Add("Action:" + HttpContext.Current.Request.Form["Action"]);
                errMsg.list_msg.Add("ConstcCusCode:" + HttpContext.Current.Session["ConstcCusCode"].ToString());
                errMsg.list_msg.Add(err.ToString());
                check.WriteLog(errMsg);
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(errMsg));
                return;
            }



        }
        #endregion

        #region 根据分类获取商品列表_需求订单
        public void Get_Product_List_CPXQ() {
            ReInfo ri = new ReInfo();
            string ConstcCusCode = HttpContext.Current.Session["ConstcCusCode"].ToString();
            string cInvCCode = HttpContext.Current.Request.Form["cInvCCode"].ToString();
            string kpdw = HttpContext.Current.Request.Form["kpdw"];
            string iShowType = HttpContext.Current.Request.Form["iShowType"];
            if (string.IsNullOrEmpty(kpdw) || kpdw == "0")
            {
                ri.flag = "0";
                ri.message = "请先选择开票单位!";
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }
            DataTable dt = pro.Get_Product_List_CPXQ(cInvCCode, kpdw, iShowType);
            ri.dt = dt;
            ri.flag = "1";
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            return;
        }
        #endregion

        #region 取出产品分类
        /// <summary>
        /// 取出产品分类
        /// </summary>
        /// <param name="ConstcCusCode"></param>
        /// <returns></returns>
        public void Get_Product_Class()
        {
            try
            {
                ReInfo ri = new ReInfo();

                string ConstcCusCode = HttpContext.Current.Session["ConstcCusCode"].ToString();
                //ri = check.Check_Time(ConstcCusCode);
                //if (ri.flag != "1")
                //{
                //    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                //    return;
                //}
                string cSTCode = HttpContext.Current.Request.Form["cSTCode"].ToString();
                string kpdw = HttpContext.Current.Request.Form["kpdw"];
                if (string.IsNullOrEmpty(kpdw) || kpdw == "0")
                {
                    ri.flag = "0";
                    ri.message = "请先选择开票单位!";
                    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                    return;
                }
                DataTable dt = pro.DL_InventoryBySel(HttpContext.Current.Request.Form["cSTCode"].ToString(), HttpContext.Current.Request.Form["kpdw"]);
                dt.Columns["KeyFieldName"].ColumnName = "id";
                dt.Columns["ParentFieldName"].ColumnName = "pid";
                dt.Columns["NodeName"].ColumnName = "name";
                ri.flag = "1";
                ri.dt = dt;
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }
            catch (Exception err)
            {

                errMsg.list_msg.Add("Action:" + HttpContext.Current.Request.Form["Action"]);
                errMsg.list_msg.Add("ConstcCusCode:" + HttpContext.Current.Session["ConstcCusCode"].ToString());
                errMsg.list_msg.Add(err.ToString());
                check.WriteLog(errMsg);
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(errMsg));
                return;
            }


        }
        #endregion

        #region 提取历史订单列表
        public void DL_GeneralPreviousOrderBySel()
        {
            ReInfo ri = new ReInfo();
            string ConstcCusCode = HttpContext.Current.Session["ConstcCusCode"].ToString();
            //ri = check.Check_Time(ConstcCusCode);
            //if (ri.flag != "1")
            //{
            //    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            //    return;
            //}
            DataTable dt = pro.DL_GeneralPreviousOrderBySel(HttpContext.Current.Session["ConstcCusCode"].ToString() + "%");
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(dt));
            return;
        }

        #endregion

        #region 获取临时订单列表
        public void DL_GetOrderBackBySel()
        {
            DataTable dt = new DataTable();
            dt = pro.DL_GetOrderBackBySel(Convert.ToInt32(HttpContext.Current.Session["lngopUserId"].ToString()));
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(dt));
            return;
        }

        #endregion

        #region 删除临时订单
        public void DL_DelOrderBackDetailByDel()
        {
            ReInfo ri = new ReInfo();
            int lngopOrderBackId = Convert.ToInt32(HttpContext.Current.Request.Form["lngopOrderBackId"].ToString());
            bool c = order.DL_DelOrderBackDetailByDel(lngopOrderBackId);
            if (c)
            {
                ri.flag = "0";
                ri.message = "删除成功！";

            }
            else
            {
                ri.flag = "1";
                ri.message = "删除失败，请重试或联系管理员！";
            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            return;
        }
        #endregion

        #region 获取新增样品订单关联的主订单表头信息
        public void Get_SampleOrder_Title()
        {
            ReInfo ri = new ReInfo();
            string strBillNo = HttpContext.Current.Request.Form["strBillNo"];
            ri.dt = order.DL_OrderModifyBySel(strBillNo);
            ri.CusCredit_dt = order.DLproc_getCusCreditInfoWithBillno(ri.dt.Rows[0]["ccuscode"].ToString(), strBillNo);  //获取信用
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            return;
        }
        #endregion

        #region 新增样品订单
        public void Insert_SampleOrder()
        {
            ReInfo ri = new ReInfo();

            string strBillNo = HttpContext.Current.Request.Form["strBillNo"];
            string strRemarks = HttpContext.Current.Request.Form["strRemarks"];
            string strLoadingWays = HttpContext.Current.Request.Form["strLoadingWays"];
            string lngopUserExId = HttpContext.Current.Session["lngopUserExId"].ToString();
            string strAllAcount = HttpContext.Current.Session["strAllAcount"].ToString();
            string kpdw = HttpContext.Current.Request.Form["kpdw"];
            string areaid = HttpContext.Current.Request.Form["areaid"];
            //插入表头数据
            DataTable dt = order.DLproc_NewSampleOrderByIns(strBillNo, strRemarks, strLoadingWays, lngopUserExId, strAllAcount);
            if (dt.Rows.Count != 1)
            {
                ri.flag = "0";
                ri.message = "提交样品订单失败，请重试或联系管理员！";
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }

            string li = HttpContext.Current.Request.Form["buy_list"];
            DataTable Check_Dt = new DataTable();
            if (!string.IsNullOrEmpty(li))
            {
                List<Buy_list> listData = JsonConvert.DeserializeObject<List<Buy_list>>(li);
                Check_Dt = pro.Return_Check_Dt(listData, kpdw, areaid);
            }



            //插入表体数据
            int lngopOrderId = Convert.ToInt32(dt.Rows[0]["lngopOrderId"].ToString());
            strBillNo = dt.Rows[0]["strBillNo"].ToString();

            Insert_listData(Check_Dt, lngopOrderId);

            ri.flag = "1";
            ri.message = strBillNo;

            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            return;

        }

        #endregion

        #region 生成用于验证的新Datatable
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listData">表单数据</param>
        /// <param name="kpdw">开票单位编码</param>
        /// <returns></returns>
        public DataTable Return_Check_Dt(List<Buy_list> listData, string kpdw)
        {
            //根据表体数据itemid重新生成Table，计算金额，用于验证信用、是否大于库存、是否大于可用量、是否有未填写数量的商品
            DataTable Check_Dt = new DataTable(); //用于验证的table
            DataTable dt = new DataTable();
            for (int i = 0; i < listData.Count; i++)
            {
                dt = order.DLproc_QuasiOrderDetailBySel(listData[i].cinvcode, kpdw);

                if (i == 0) //第一条数据时clone表结构
                {
                    Check_Dt = dt.Clone();
                    Check_Dt.Columns.Add("cComUnitQTY", typeof(string));  //基本数量 
                    Check_Dt.Columns.Add("cInvDefine2QTY", typeof(string));//小包装数量 
                    Check_Dt.Columns.Add("cInvDefine1QTY", typeof(string)); //大包装数量
                    Check_Dt.Columns.Add("cDefine22", typeof(string));      //包装结果
                    Check_Dt.Columns.Add("iquantity", typeof(string));       //汇总数量
                    Check_Dt.Columns.Add("unitGroup", typeof(string));    //包装组
                    Check_Dt.Columns.Add("irowno", typeof(string));           //行号
                    Check_Dt.Columns.Add("cInvDefine13", typeof(string));    //大包装换算率
                    Check_Dt.Columns.Add("cInvDefine14", typeof(string));    //小包装换算率
                    Check_Dt.Columns.Add("cInvDefine1", typeof(string));    //大包装单位
                    Check_Dt.Columns.Add("cInvDefine2", typeof(string));    //小包装单位
                    Check_Dt.Columns.Add("rowType", typeof(int));       //行状态

                }
                Check_Dt.Rows.Add(dt.Rows[0].ItemArray);
                Check_Dt.Rows[i]["cComUnitQTY"] = listData[i].cComUnitQTY;  //基本数量赋值
                Check_Dt.Rows[i]["cInvDefine2QTY"] = listData[i].cInvDefine2QTY;  //小包装数量赋值
                Check_Dt.Rows[i]["cInvDefine1QTY"] = listData[i].cInvDefine1QTY;  //大包装数量赋值
                Check_Dt.Rows[i]["cDefine22"] = listData[i].cDefine22;   //包装结果赋值
                Check_Dt.Rows[i]["iquantity"] = listData[i].iquantity;   //汇总数量赋值
                Check_Dt.Rows[i]["unitGroup"] = listData[i].unitGroup;    //单位组赋值
                Check_Dt.Rows[i]["irowno"] = listData[i].irowno;             //行号赋值


                Check_Dt.Rows[i]["rowType"] = 0;                //行默认状态赋值
                if (dt.Rows.Count >= 3)
                {
                    Check_Dt.Rows[i]["cInvDefine1"] = dt.Rows[2]["cComUnitName"].ToString();//大包装单位
                    Check_Dt.Rows[i]["cInvDefine2"] = dt.Rows[1]["cComUnitName"].ToString();//小包装单位
                    Check_Dt.Rows[i]["cInvDefine13"] = dt.Rows[2]["iChangRate"].ToString();//大包装换算率
                    Check_Dt.Rows[i]["cInvDefine14"] = dt.Rows[1]["iChangRate"].ToString();//小包装换算率
                }
                else if (dt.Rows.Count == 2)
                {
                    Check_Dt.Rows[i]["cInvDefine1"] = dt.Rows[1]["cComUnitName"].ToString();
                    Check_Dt.Rows[i]["cInvDefine2"] = dt.Rows[1]["cComUnitName"].ToString();
                    Check_Dt.Rows[i]["cInvDefine13"] = dt.Rows[1]["iChangRate"].ToString();
                    Check_Dt.Rows[i]["cInvDefine14"] = dt.Rows[1]["iChangRate"].ToString();
                }
                else
                {
                    Check_Dt.Rows[i]["cInvDefine1"] = dt.Rows[0]["cComUnitName"].ToString();
                    Check_Dt.Rows[i]["cInvDefine2"] = dt.Rows[0]["cComUnitName"].ToString();
                    Check_Dt.Rows[i]["cInvDefine13"] = dt.Rows[0]["iChangRate"].ToString();
                    Check_Dt.Rows[i]["cInvDefine14"] = dt.Rows[0]["iChangRate"].ToString();
                }
            }
            return Check_Dt;
        }

        #endregion

        #region 生成用于验证的新Datatable
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listData">表单数据</param>
        /// <param name="kpdw">开票单位编码</param>
        /// <returns></returns>
        public DataTable Return_Check_Dt(List<Buy_list> listData, string kpdw, string strBillNo, string areaid)
        {
            //根据表体数据itemid重新生成Table，计算金额，用于验证信用、是否大于库存、是否大于可用量、是否有未填写数量的商品
            DataTable Check_Dt = new DataTable(); //用于验证的table
            DataTable dt = new DataTable();
            for (int i = 0; i < listData.Count; i++)
            {
                dt = pro.DLproc_QuasiOrderDetailModifyBySel(listData[i].cinvcode, kpdw, strBillNo, areaid);

                if (i == 0) //第一条数据时clone表结构
                {
                    Check_Dt = dt.Clone();
                    Check_Dt.Columns.Add("cComUnitQTY", typeof(string));  //基本数量 
                    Check_Dt.Columns.Add("cInvDefine2QTY", typeof(string));//小包装数量 
                    Check_Dt.Columns.Add("cInvDefine1QTY", typeof(string)); //大包装数量
                    Check_Dt.Columns.Add("cDefine22", typeof(string));      //包装结果
                    Check_Dt.Columns.Add("iquantity", typeof(string));       //汇总数量
                    Check_Dt.Columns.Add("unitGroup", typeof(string));    //包装组
                    Check_Dt.Columns.Add("irowno", typeof(string));           //行号
                    Check_Dt.Columns.Add("cInvDefine13", typeof(string));    //大包装换算率
                    Check_Dt.Columns.Add("cInvDefine14", typeof(string));    //小包装换算率
                    Check_Dt.Columns.Add("cInvDefine1", typeof(string));    //大包装单位
                    Check_Dt.Columns.Add("cInvDefine2", typeof(string));    //小包装单位
                    Check_Dt.Columns.Add("rowType", typeof(int));       //行状态

                }
                Check_Dt.Rows.Add(dt.Rows[0].ItemArray);
                Check_Dt.Rows[i]["cComUnitQTY"] = listData[i].cComUnitQTY;  //基本数量赋值
                Check_Dt.Rows[i]["cInvDefine2QTY"] = listData[i].cInvDefine2QTY;  //小包装数量赋值
                Check_Dt.Rows[i]["cInvDefine1QTY"] = listData[i].cInvDefine1QTY;  //大包装数量赋值
                Check_Dt.Rows[i]["cDefine22"] = listData[i].cDefine22;   //包装结果赋值
                Check_Dt.Rows[i]["iquantity"] = listData[i].iquantity;   //汇总数量赋值
                Check_Dt.Rows[i]["unitGroup"] = listData[i].unitGroup;    //单位组赋值
                Check_Dt.Rows[i]["irowno"] = listData[i].irowno;             //行号赋值


                Check_Dt.Rows[i]["rowType"] = 0;                //行默认状态赋值
                if (dt.Rows.Count >= 3)
                {
                    Check_Dt.Rows[i]["cInvDefine1"] = dt.Rows[2]["cComUnitName"].ToString();//大包装单位
                    Check_Dt.Rows[i]["cInvDefine2"] = dt.Rows[1]["cComUnitName"].ToString();//小包装单位
                    Check_Dt.Rows[i]["cInvDefine13"] = dt.Rows[2]["iChangRate"].ToString();//大包装换算率
                    Check_Dt.Rows[i]["cInvDefine14"] = dt.Rows[1]["iChangRate"].ToString();//小包装换算率
                }
                else if (dt.Rows.Count == 2)
                {
                    Check_Dt.Rows[i]["cInvDefine1"] = dt.Rows[1]["cComUnitName"].ToString();
                    Check_Dt.Rows[i]["cInvDefine2"] = dt.Rows[1]["cComUnitName"].ToString();
                    Check_Dt.Rows[i]["cInvDefine13"] = dt.Rows[1]["iChangRate"].ToString();
                    Check_Dt.Rows[i]["cInvDefine14"] = dt.Rows[1]["iChangRate"].ToString();
                }
                else
                {
                    Check_Dt.Rows[i]["cInvDefine1"] = dt.Rows[0]["cComUnitName"].ToString();
                    Check_Dt.Rows[i]["cInvDefine2"] = dt.Rows[0]["cComUnitName"].ToString();
                    Check_Dt.Rows[i]["cInvDefine13"] = dt.Rows[0]["iChangRate"].ToString();
                    Check_Dt.Rows[i]["cInvDefine14"] = dt.Rows[0]["iChangRate"].ToString();
                }
            }
            return Check_Dt;
        }

        #endregion

        #region 返回修改普通订单时，产品详细列表
        private DataTable DLproc_QuasiOrderDetailModifyBySel(string[] codes, string kpdw, string strBillNo, string areaid)
        {
            DataTable dt = new DataTable();
            DataTable new_dt = new DataTable();
            for (int i = 0; i < codes.Length; i++)
            {
                dt = order.DLproc_QuasiOrderDetailModifyBySel(codes[i], kpdw, strBillNo, areaid);
                if (i == 0)
                {
                    new_dt = dt.Clone();
                    new_dt.Columns.Add("cinvdefine13", typeof(float)); //大包装换算
                    new_dt.Columns.Add("cinvdefine14", typeof(float));//小包装换算
                    new_dt.Columns.Add("cInvDefine1", typeof(string));//大包装单位
                    new_dt.Columns.Add("cInvDefine2", typeof(string));//小包装单位

                }
                new_dt.Rows.Add(dt.Rows[0].ItemArray);
                if (dt.Rows.Count >= 3)
                {

                    new_dt.Rows[i]["cinvdefine13"] = Convert.ToDouble(dt.Rows[2]["iChangRate"].ToString()).ToString();
                    new_dt.Rows[i]["cinvdefine14"] = Convert.ToDouble(dt.Rows[1]["iChangRate"].ToString()).ToString();
                    new_dt.Rows[i]["cInvDefine2"] = dt.Rows[1]["cComUnitName"].ToString();
                    new_dt.Rows[i]["cInvDefine1"] = dt.Rows[2]["cComUnitName"].ToString();
                }
                else if (dt.Rows.Count == 2)
                {
                    new_dt.Rows[i]["cinvdefine13"] = Convert.ToDouble(dt.Rows[1]["iChangRate"].ToString()).ToString();
                    new_dt.Rows[i]["cinvdefine14"] = Convert.ToDouble(dt.Rows[1]["iChangRate"].ToString()).ToString();
                    new_dt.Rows[i]["cInvDefine2"] = dt.Rows[1]["cComUnitName"].ToString();
                    new_dt.Rows[i]["cInvDefine1"] = dt.Rows[1]["cComUnitName"].ToString();
                }
                else if (dt.Rows.Count == 1)
                {
                    new_dt.Rows[i]["cinvdefine13"] = Convert.ToDouble(dt.Rows[0]["iChangRate"].ToString()).ToString();
                    new_dt.Rows[i]["cinvdefine14"] = Convert.ToDouble(dt.Rows[0]["iChangRate"].ToString()).ToString();
                    new_dt.Rows[i]["cInvDefine2"] = dt.Rows[0]["cComUnitName"].ToString();
                    new_dt.Rows[i]["cInvDefine1"] = dt.Rows[0]["cComUnitName"].ToString();
                }


            }


            return new_dt;
        }
        #endregion

        #region 提交表体数据的方法
        public void Insert_listData(DataTable Check_Dt, int lngopOrderId)
        {
            ReInfo ri = new ReInfo();
            ri.flag = "1";
            foreach (DataRow dr in Check_Dt.Rows)
            {
                string cinvcode = dr["cinvcode"].ToString();   //存货编码
                double iquantity = Convert.ToDouble(dr["iquantity"].ToString());   //购买货数量
                double iinvexchrate = 1;//换算率 
                double inum = 1;        //辅计量数量   
                //计算销售单位(辅助)的换算率
                if (dr["cn1cComUnitName"].ToString() == dr["cInvDefine1"].ToString())  //大包装单位换算率
                {
                    iinvexchrate = Convert.ToDouble(dr["cInvDefine13"].ToString());
                }
                else if (dr["cn1cComUnitName"].ToString() == dr["cInvDefine2"].ToString()) //小包装单位换算率
                {
                    iinvexchrate = Convert.ToDouble(dr["cInvDefine14"].ToString());
                }
                else  //无换算单位
                {
                    iinvexchrate = 1;//换算率 
                }
                //计算赋值
                //换算结果:辅计量数量 =存货数量/换算率,四舍五入,保留两位小数
                inum = Math.Round(iquantity / iinvexchrate, 2);  //辅计量数量 
                double iquotedprice = Convert.ToDouble(dr["Quote"].ToString());//报价 保留5位小数,四舍五入
                double itaxunitprice = Convert.ToDouble(dr["ExercisePrice"].ToString());//原币含税单价 即执行价格dt.Columns.Add("ExercisePrice"); //基本单位单价(执行价格)   16,保留5位
                double itaxrate = Convert.ToDouble(dr["iTaxRate"].ToString());    //税率 
                double isum = Math.Round(itaxunitprice * iquantity, 2);     //原币价税合计    //金额,原币价税合计=原币含税单价*数量,保留2位小数,四舍五入 
                double itax = Math.Round(isum / (1 + itaxrate / 100) * (itaxrate / 100), 2);        //原币税额 ;税额=金额/1.17*0.17 保留2位, 四舍五入    
                double imoney = Math.Round(isum - itax, 2);      //原币无税金额 =金额-税额,保留2位,四舍五入
                double iunitprice = Math.Round(imoney / iquantity, 6);  //原币无税单价=无税金额/数量,保留5位小数,四舍五入        
                double idiscount = Math.Round(iquotedprice * iquantity - isum, 2);   //原币折扣额=报价*数量-金额,保留两位
                double inatunitprice = iunitprice;//本币无税单价
                double inatmoney = imoney;   //本币无税金额
                double inattax = itax;     //本币税额 
                double inatsum = isum;     //本币价税合计
                double inatdiscount = idiscount;   //本币折扣额 
                double kl = Convert.ToDouble(dr["Rate"].ToString());          //扣率 
                string cDefine22 = dr["cDefine22"].ToString();  //表体自定义项22,包装量
                string cunitid = dr["cComUnitCode"].ToString();    //计量单位编码
                int irowno = Convert.ToInt32(dr["irowno"].ToString()); //行号,从1开始,自增长
                string cinvname = dr["cInvName"].ToString();   //存货名称 
                string cComUnitName = dr["cComUnitName"].ToString();       //基本单位名称
                string cInvDefine1 = dr["cInvDefine1"].ToString();        //大包装单位名称         
                string cInvDefine2 = dr["cInvDefine2"].ToString();        //小包装单位名称 
                double cInvDefine13 = Convert.ToDouble(dr["cInvDefine13"].ToString());       //大包装换算率
                double cInvDefine14 = Convert.ToDouble(dr["cInvDefine14"].ToString());       //小包装换算率
                string unitGroup = dr["unitGroup"].ToString();          //单位换算率组
                double cComUnitQTY = Convert.ToDouble(dr["cComUnitQTY"].ToString());        //基本单位数量
                double cInvDefine1QTY = Convert.ToDouble(dr["cInvDefine1QTY"].ToString());     //大包装单位数量
                double cInvDefine2QTY = Convert.ToDouble(dr["cInvDefine2QTY"].ToString());     //小包装单位数量
                string cn1cComUnitName = dr["cn1cComUnitName"].ToString();    //销售单位名称
                string cDefine24 = dr["cDefine24"].ToString();
                string cbdefine16 = dr["cbdefine16"].ToString();


                OrderInfo oiEntry = new OrderInfo(lngopOrderId, cinvcode, iquantity, inum, iquotedprice, iunitprice, itaxunitprice, imoney, itax, isum, inatunitprice, inatmoney, inattax, inatsum, kl, itaxrate, cDefine22, iinvexchrate, cunitid, irowno, cinvname, idiscount, inatdiscount, cComUnitName, cInvDefine1, cInvDefine2, cInvDefine13, cInvDefine14, unitGroup, cComUnitQTY, cInvDefine1QTY, cInvDefine2QTY, cn1cComUnitName, cDefine24);

                bool b = order.DLproc_NewOrderDetailByIns(oiEntry, cbdefine16);
                //    if (!b)
                //    {
                //        ri.flag = "0";
                //        ri.message = "商品号:" + cinvcode + "插入失败,请重试或联系管理员!";
                //        return ri;
                //    }

            }
            //return ri;
        }
        #endregion

        #region 根据选择的特殊订单里的产品，获取详细产品价格等信息
        public void DLproc_QuasiYOrderDetail_TSBySel()
        {
            ReInfo ri = new ReInfo();
            string kpdw = HttpContext.Current.Request.Form["kpdw"];
            string isModify = HttpContext.Current.Request.Form["isModify"];
            if (!string.IsNullOrEmpty(kpdw) || kpdw != "0")
            {
                ri.dt = order.DLproc_getCusCreditInfo(kpdw);
            }
            string code = HttpContext.Current.Request.Form["itemids"];
            if (string.IsNullOrEmpty(code) || code.Length == 0)
            {
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }
            ri.list_dt = new List<DataTable>();
            string[] codes = code.Split(',');
            string cInvCode = ""; //产品编码
            string preOrderId = ""; //订单编号
            string[] it = new string[2]; //根据itemid截取字符串
            string areaid = HttpContext.Current.Request.Form["areaid"];
            if (isModify == "0")
            {
                foreach (var item in codes)
                {
                    it = item.Split('Y');
                    cInvCode = it[0];
                    preOrderId = "Y" + it[1];
                    ri.list_dt.Add(pro.DLproc_QuasiYOrderDetail_TSBySel(cInvCode, preOrderId, areaid));
                }
            }
            else if (isModify == "1")
            {
                string strBillNo = HttpContext.Current.Request.Form["strBillNo"];
                foreach (var item in codes)
                {
                    it = item.Split('Y');
                    cInvCode = it[0];
                    preOrderId = "Y" + it[1];
                    ri.list_dt.Add(pro.DLproc_QuasiYOrderDetailModify_TSBySel(cInvCode, preOrderId, strBillNo, areaid));
                }
            }



            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            return;
        }
        #endregion

        #region 在新增特殊订单时，查询可用于参照的特殊订单号
        public void DL_PreOrderTreeBySel()
        {
            ReInfo ri = new ReInfo();
            ri.dt = new SearchManager().DL_PreOrderTreeBySel(HttpContext.Current.Request.Form["kpdw"], HttpContext.Current.Session["lngopUserId"].ToString(), HttpContext.Current.Session["lngopUserExId"].ToString(), 2);
            ri.flag = "1";
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            return;
        }
        #endregion

        #region 根据特殊订单号查询详细产品列表
        public void DLproc_TreeListPreDetailsModify_CZTSBySel()
        {

            ReInfo ri = new ReInfo();
            string isModify = HttpContext.Current.Request.Form["isModify"];
            if (isModify == "0")
            {
                ri.dt = pro.DLproc_TreeListPreDetails_TSBySel(HttpContext.Current.Request.Form["strBillNo"]);
            }
            else if (isModify == "1")
            {
                ri.dt = pro.DLproc_TreeListPreDetailsModify_CZTSBySel(HttpContext.Current.Request.Form["strBillNo"]);

            }
            //  ri.dt = new SearchManager().DLproc_TreeListPreDetailsBySel(HttpContext.Current.Request.Form["strBillNo"], 2);
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            return;
        }
        #endregion

        #region 获取信用
        public void DLproc_getCusCreditInfo()
        {
            ReInfo ri = new ReInfo();
            string kpdw = HttpContext.Current.Request.Form["kpdw"];
            DataTable dt = order.DLproc_getCusCreditInfo(kpdw);//传入的开票单位
            if (dt.Rows.Count == 0)
            {
                ri.flag = "0";
                ri.message = "获取数据失败,请重试或联系管理员!";
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }
            ri.flag = "1";
            ri.CusCredit_dt = dt;
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            return;
        }
        #endregion

        #region 参照特殊订单后提交普通订单
        public void DLproc_NewYYOrderByIns()
        {
            ReInfo ri = new ReInfo();
            ri.list_msg = new List<string>();
            ri.flag = "1";
            //获取表头数据实例化为model
            form_Data formData = JsonConvert.DeserializeObject<form_Data>(HttpContext.Current.Request.Form["formData"]);

            //获取表体数据实例化model
            List<Buy_list> listData = JsonConvert.DeserializeObject<List<Buy_list>>(HttpContext.Current.Request.Form["listData"]);

            //根据表体数据itemid重新生成Table，计算金额，用于验证信用、是否大于库存、是否大于可用量、是否有未填写数量的商品
            DataTable Check_Dt = new DataTable(); //用于验证的table
            DataTable dt = new DataTable();
            string cInvCode = ""; //产品编码
            string preOrderId = ""; //订单编号
            for (int i = 0; i < listData.Count; i++)
            {
                string[] code = listData[i].itemid.Split('Y');
                cInvCode = code[0];
                preOrderId = "Y" + code[1];
                dt = pro.DLproc_QuasiYOrderDetail_TSBySel(cInvCode, preOrderId, formData.areaid);
                if (i == 0)  //第一次的时候clone表格结构
                {
                    Check_Dt = dt.Clone();
                    Check_Dt.Columns.Add("rowType", typeof(int)); //添加行状态列，用于前台赋值颜色，1为购买量为零；2为购买量大于库存量；3为购买量大于可用量
                    Check_Dt.Columns["cComUnitQTY"].ReadOnly = false;
                    Check_Dt.Columns["cInvDefine2QTY"].ReadOnly = false;
                    Check_Dt.Columns["cInvDefine1QTY"].ReadOnly = false;
                    Check_Dt.Columns["cDefine22"].ReadOnly = false;
                    Check_Dt.Columns["iquantity"].ReadOnly = false;
                    Check_Dt.Columns["irowno"].ReadOnly = false;
                }

                Check_Dt.Rows.Add(dt.Rows[0].ItemArray);
                Check_Dt.Rows[i]["cComUnitQTY"] = listData[i].cComUnitQTY;  //基本数量赋值
                Check_Dt.Rows[i]["cInvDefine2QTY"] = listData[i].cInvDefine2QTY;  //小包装数量赋值
                Check_Dt.Rows[i]["cInvDefine1QTY"] = listData[i].cInvDefine1QTY;  //大包装数量赋值
                Check_Dt.Rows[i]["cDefine22"] = listData[i].cDefine22;   //包装结果赋值
                Check_Dt.Rows[i]["iquantity"] = listData[i].iquantity;   //汇总数量赋值
                Check_Dt.Rows[i]["irowno"] = listData[i].irowno;             //行号赋值
                Check_Dt.Rows[i]["rowType"] = 0;                //行默认状态赋值
            }

            #region 验证表单开始

            //  dt = order.DLproc_getCusCreditInfo(formData.ccuscode);// 重新获取信用
            //  double CusCredit = Convert.ToDouble(dt.Rows[0]["iCusCreLine"].ToString()); //客户信用
            //   double listCredit = 0;   //用于累计表单金额
            int rowType = 0; //用于记录表体中是否有不合格数据

            foreach (DataRow dr in Check_Dt.Rows)
            {
                // listCredit += Convert.ToDouble(dr["iquantity"].ToString()) * Convert.ToDouble(dr["itaxunitprice"].ToString());
                if (Convert.ToDouble(dr["iquantity"].ToString()) == 0)
                {
                    dr["rowType"] = 1;
                }
                if (Convert.ToDouble(dr["iquantity"].ToString()) > Convert.ToDouble(dr["fAvailQtty"].ToString()))
                {
                    dr["rowType"] = 2;
                }
                if (Convert.ToDouble(dr["iquantity"].ToString()) > Convert.ToDouble(dr["realqty"].ToString()))  //字段错误！！iquantity应为realqty
                {
                    dr["rowType"] = 3;
                }
                rowType += Convert.ToInt32(dr["rowType"].ToString());
            }

            if (rowType != 0)
            {
                ri.flag = "0";
                ri.list_msg.Add("订单列表里有产品数量为0或者大于库存，请重新输入!");
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }

            #endregion

            //验证通过后，开始提交表单
            #region 获取表头信息，提交表头数据
            DataTable Address_Dt = new product().Get_AddressById(formData.lngopUseraddressId);
            OrderInfo oi = new OrderInfo(
                                            HttpContext.Current.Session["lngopUserId"].ToString(), //客户ID
                                            DateTime.Now.ToString(),                               //订单时间                     
                                            1,                                                     //订单状态         
                                            formData.strRemarks,                                   //备注     
                                            formData.ccuscode,                                     //开票单位ID 
                                            Address_Dt.Rows[0]["strDriverName"].ToString(),        //司机姓名 
                                            Address_Dt.Rows[0]["strIdCard"].ToString(),            //司机身份证
                                            formData.carType,                                      //车型
                                            Address_Dt.Rows[0]["strConsigneeName"].ToString(),     //收货人姓名
                                            Address_Dt.Rows[0]["strCarplateNumber"].ToString(),     //车牌号
                                            formData.txtAddress,                                    //地址信息
                                            Address_Dt.Rows[0]["strConsigneeTel"].ToString(),      //收货人电话
                                            Address_Dt.Rows[0]["strDriverTel"].ToString(),         //司机电话
                                            formData.ccusname,                                     //开票单位名称
                                            formData.ccuspperson,                                  //业务员编号 
                                            formData.csccode,                                      //送货方式
                                            formData.datDeliveryDate,                              //提货时间
                                            formData.strLoadingWays,                               //装车方式
                                            "00",                                                  //销售类型编码    
                                            formData.lngopUseraddressId,                           //地址ID
                                            "",                                                    //strTxtRelateU8NO
                                            2,                                                     //lngBillType 特殊订单
                                            formData.txtArea,                                      //行政区
                                            HttpContext.Current.Session["lngopUserExId"].ToString(),//用户ExID
                                            HttpContext.Current.Session["strAllAcount"].ToString()
                                       );
            DataTable lngopOrderIdDt = order.DLproc_NewYYOrderByIns(oi, formData.areaid, formData.iaddresstype, formData.chdefine21);

            if (lngopOrderIdDt.Rows.Count < 0)
            {
                ri.flag = "0";
                ri.list_msg.Add("订单提交出错，请重试或联系管理员！");
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }
            #endregion

            #region 提交表体数据
            int lngopOrderId = Convert.ToInt32(lngopOrderIdDt.Rows[0]["lngopOrderId"].ToString());
            //  OrderInfo oiEntry1 = new OrderInfo(lngopOrderId, cinvcode, iquantity, inum, iquotedprice, iunitprice, itaxunitprice, imoney, itax, isum, inatunitprice, inatmoney, inattax, inatsum, kl, itaxrate, cDefine22, iinvexchrate, cunitid, irowno, cinvname, idiscount, inatdiscount, cComUnitName, cInvDefine1, cInvDefine2, cInvDefine13, cInvDefine14, unitGroup, cComUnitQTY, cInvDefine1QTY, cInvDefine2QTY, cn1cComUnitName, cpreordercode, autoid);
            foreach (DataRow dr in Check_Dt.Rows)
            {
                string cinvcode = dr["cinvcode"].ToString();   //存货编码
                double iquantity = Convert.ToDouble(dr["iquantity"].ToString());   //购买货数量
                double iinvexchrate = 1;//换算率 
                double inum = 1;        //辅计量数量   
                //计算销售单位(辅助)的换算率
                if (dr["cn1cComUnitName"].ToString() == dr["cInvDefine1"].ToString())  //大包装单位换算率
                {
                    iinvexchrate = Convert.ToDouble(dr["cInvDefine13"].ToString());
                }
                else if (dr["cn1cComUnitName"].ToString() == dr["cInvDefine2"].ToString()) //小包装单位换算率
                {
                    iinvexchrate = Convert.ToDouble(dr["cInvDefine14"].ToString());
                }
                else  //无换算单位
                {
                    iinvexchrate = 1;//换算率 
                }
                //计算赋值
                //换算结果:辅计量数量 =存货数量/换算率,四舍五入,保留两位小数
                inum = Math.Round(iquantity / iinvexchrate, 2);  //辅计量数量 
                double itaxrate = Convert.ToDouble(dr["itaxrate"].ToString());    //税率 

                double iquotedprice = Convert.ToDouble(dr["iquotedprice"].ToString());//报价 保留5位小数,四舍五入
                double itaxunitprice = Convert.ToDouble(dr["itaxunitprice"].ToString());//原币含税单价 即执行价格dt.Columns.Add("ExercisePrice"); //基本单位单价(执行价格)   16,保留5位
                double isum = Math.Round(itaxunitprice * iquantity, 2);     //原币价税合计    //金额,原币价税合计=原币含税单价*数量,保留2位小数,四舍五入 
                double itax = Math.Round(isum / (1 + itaxrate / 100) * (itaxrate / 100), 2);        //原币税额 ;税额=金额/1.17*0.17 保留2位, 四舍五入    
                double imoney = Math.Round(isum - itax, 2);      //原币无税金额 =金额-税额,保留2位,四舍五入
                double iunitprice = Math.Round(imoney / iquantity, 6);  //原币无税单价=无税金额/数量,保留5位小数,四舍五入        
                double idiscount = Math.Round(iquotedprice * iquantity - isum, 2);   //原币折扣额=报价*数量-金额,保留两位
                double inatunitprice = iunitprice;//本币无税单价
                double inatmoney = imoney;   //本币无税金额
                double inattax = itax;     //本币税额 
                double inatsum = isum;     //本币价税合计
                double inatdiscount = idiscount;   //本币折扣额 
                double kl = Convert.ToDouble(dr["kl"].ToString());          //扣率 
                string cDefine22 = dr["cDefine22"].ToString();  //表体自定义项22,包装量
                string cunitid = dr["cunitid"].ToString();    //计量单位编码
                int irowno = Convert.ToInt32(dr["irowno"].ToString()); //行号,从1开始,自增长
                string cinvname = dr["cinvname"].ToString();   //存货名称 
                string cComUnitName = dr["cComUnitName"].ToString();       //基本单位名称
                string cInvDefine1 = dr["cInvDefine1"].ToString();        //大包装单位名称         
                string cInvDefine2 = dr["cInvDefine2"].ToString();        //小包装单位名称 
                double cInvDefine13 = Convert.ToDouble(dr["cInvDefine13"].ToString());       //大包装换算率
                double cInvDefine14 = Convert.ToDouble(dr["cInvDefine14"].ToString());       //小包装换算率
                string unitGroup = dr["unitGroup"].ToString();          //单位换算率组
                double cComUnitQTY = Convert.ToDouble(dr["cComUnitQTY"].ToString());        //基本单位数量
                double cInvDefine1QTY = Convert.ToDouble(dr["cInvDefine1QTY"].ToString());     //大包装单位数量
                double cInvDefine2QTY = Convert.ToDouble(dr["cInvDefine2QTY"].ToString());     //小包装单位数量
                string cn1cComUnitName = dr["cn1cComUnitName"].ToString();    //销售单位名称
                string cpreordercode = dr["ccode"].ToString();    //销售预订单号
                string autoid = dr["iaoids"].ToString();    //预订单号id
                string cDefine24 = dr["cDefine24"].ToString();
                string cbdefine16 = dr["cbdefine16"].ToString();
                OrderInfo oiEntry = new OrderInfo(lngopOrderId, cinvcode, iquantity, inum, iquotedprice, iunitprice, itaxunitprice, imoney, itax, isum, inatunitprice, inatmoney, inattax, inatsum, kl, itaxrate, cDefine22, iinvexchrate, cunitid, irowno, cinvname, idiscount, inatdiscount, cComUnitName, cInvDefine1, cInvDefine2, cInvDefine13, cInvDefine14, unitGroup, cComUnitQTY, cInvDefine1QTY, cInvDefine2QTY, cn1cComUnitName, cpreordercode, autoid, cDefine24);
                //OrderInfo oiEntry=new OrderInfo(lngopOrderId,dr["cinvcode"].ToString(),Convert.ToDouble(dr["iquantity"].ToString()),Convert.ToDouble(dr["inum"].ToString()),Convert.ToDouble(dr["iquotedprice"].ToString()),Convert.ToDouble(dr["iunitprice"].ToString()),
                //                                                Convert.ToDouble(dr["itaxunitprice"].ToString()),Convert.ToDouble(dr["imoney"].ToString()),Convert.ToDouble(dr["itax"].ToString()),Convert.ToDouble(dr["isum"].ToString()),
                //                                                Convert.ToDouble(dr["inatunitprice"].ToString()),
                //                                                Convert.ToDouble(dr["inatmoney"].ToString()),Convert.ToDouble(dr["inattax"].ToString()),Convert.ToDouble(dr["inatsum"].ToString()),Convert.ToDouble(dr["kl"].ToString()),Convert.ToDouble(dr["itaxrate"].ToString()),dr["cDefine22"].ToString(),
                //                                                Convert.ToDouble(dr["iinvexchrate"].ToString()),dr["cunitid"].ToString(),Convert.ToInt32(dr["irowno"].ToString()),dr["cinvname"].ToString(),Convert.ToDouble(dr["idiscount"].ToString()),Convert.ToDouble(dr["inatdiscount"].ToString()),
                //                                                dr["cComUnitName"].ToString(),dr["cInvDefine1"].ToString(),dr["cInvDefine2"].ToString(),Convert.ToDouble(dr["cInvDefine13"].ToString()),Convert.ToDouble(dr["cInvDefine14"].ToString()),
                //                                                dr["unitGroup"].ToString(),Convert.ToDouble(dr["cComUnitQTY"].ToString()),Convert.ToDouble(dr["cInvDefine1QTY"].ToString()),Convert.ToDouble(dr["cInvDefine2QTY"].ToString()),
                //                                                dr["cn1cComUnitName"].ToString(), dr["ccode"].ToString(), dr["iaoids"].ToString());
                order.DLproc_NewYYOrderDetailByIns(oiEntry, cbdefine16, cDefine24);

            }
            ri.flag = "1";
            ri.message = lngopOrderIdDt.Rows[0]["strBillNo"].ToString();
            #endregion
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            return;
        }

        #endregion

        #region 提交修改后的参照特殊订单
        public void DLproc_NewYOrderByUpd()
        {
            ReInfo ri = new ReInfo();
            ri.list_msg = new List<string>();
            ri.flag = "1";
            string strBillNo = HttpContext.Current.Request.Form["strBillNo"];
            string kpdw = HttpContext.Current.Request.Form["kpdw"];
            //获取表头数据实例化为model
            form_Data formData = JsonConvert.DeserializeObject<form_Data>(HttpContext.Current.Request.Form["formData"]);

            //获取表体数据实例化model
            List<Buy_list> listData = JsonConvert.DeserializeObject<List<Buy_list>>(HttpContext.Current.Request.Form["listData"]);

            //根据表体数据itemid重新生成Table，计算金额，用于验证信用、是否大于库存、是否大于可用量、是否有未填写数量的商品
            DataTable Check_Dt = new DataTable(); //用于验证的table
            DataTable dt = new DataTable();
            string cInvCode = ""; //产品编码
            string preOrderId = ""; //订单编号
            for (int i = 0; i < listData.Count; i++)
            {
                string[] code = listData[i].itemid.Split('Y');
                cInvCode = code[0];
                preOrderId = "Y" + code[1];
                dt = pro.DLproc_QuasiYOrderDetailModify_TSBySel(cInvCode, preOrderId, strBillNo, formData.areaid);
                if (i == 0)  //第一次的时候clone表格结构
                {
                    Check_Dt = dt.Clone();
                    Check_Dt.Columns.Add("rowType", typeof(int)); //添加行状态列，用于前台赋值颜色，1为购买量为零；2为购买量大于库存量；3为购买量大于可用量
                    Check_Dt.Columns["cComUnitQTY"].ReadOnly = false;
                    Check_Dt.Columns["cInvDefine2QTY"].ReadOnly = false;
                    Check_Dt.Columns["cInvDefine1QTY"].ReadOnly = false;
                    Check_Dt.Columns["cDefine22"].ReadOnly = false;
                    Check_Dt.Columns["iquantity"].ReadOnly = false;
                    Check_Dt.Columns["irowno"].ReadOnly = false;

                }

                Check_Dt.Rows.Add(dt.Rows[0].ItemArray);
                Check_Dt.Rows[i]["cComUnitQTY"] = listData[i].cComUnitQTY;  //基本数量赋值
                Check_Dt.Rows[i]["cInvDefine2QTY"] = listData[i].cInvDefine2QTY;  //小包装数量赋值
                Check_Dt.Rows[i]["cInvDefine1QTY"] = listData[i].cInvDefine1QTY;  //大包装数量赋值
                Check_Dt.Rows[i]["cDefine22"] = listData[i].cDefine22;   //包装结果赋值
                Check_Dt.Rows[i]["iquantity"] = listData[i].iquantity;   //汇总数量赋值
                Check_Dt.Rows[i]["irowno"] = listData[i].irowno;             //行号赋值
                Check_Dt.Rows[i]["rowType"] = 0;                //行默认状态赋值
            }

            #region 验证表单开始

            //  dt = order.DLproc_getCusCreditInfo(kpdw);// 重新获取信用
            //   double CusCredit = Convert.ToDouble(dt.Rows[0]["iCusCreLine"].ToString()); //客户信用
            // double listCredit = 0;   //用于累计表单金额
            int rowType = 0; //用于记录表体中是否有不合格数据

            foreach (DataRow dr in Check_Dt.Rows)
            {
                //listCredit += Convert.ToDouble(dr["iquantity"].ToString()) * Convert.ToDouble(dr["itaxunitprice"].ToString());
                if (Convert.ToDouble(dr["iquantity"].ToString()) == 0)
                {
                    dr["rowType"] = 1;
                }
                if (Convert.ToDouble(dr["iquantity"].ToString()) > Convert.ToDouble(dr["fAvailQtty"].ToString()))
                {
                    dr["rowType"] = 2;
                }
                if (Convert.ToDouble(dr["iquantity"].ToString()) > Convert.ToDouble(dr["realqty"].ToString()))  //字段错误！！iquantity应为realqty
                {
                    dr["rowType"] = 3;
                }
                rowType += Convert.ToInt32(dr["rowType"].ToString());
            }


            if (rowType != 0)
            {
                ri.flag = "0";
                ri.list_msg.Add("订单列表里有产品数量为0或者大于库存，请重新输入!");
                ri.dt = pro.Get_Filter_Column(Check_Dt, "itemid", "iquantity", "fAvailQtty", "realqty", "rowType");
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }






            #endregion

            //验证通过后，开始提交表单
            #region 获取表头信息，提交表头数据
            DataTable Address_Dt = new product().Get_AddressById(formData.lngopUseraddressId);
            OrderInfo oi = new OrderInfo(
                                            strBillNo,                                             //订单编号
                                            HttpContext.Current.Session["lngopUserId"].ToString(), //客户ID
                                            DateTime.Now.ToString(),                               //订单时间                     
                                            1,                                                     //订单状态         
                                            formData.strRemarks,                                   //备注     
                                            kpdw,                                                  //开票单位ID 
                                            Address_Dt.Rows[0]["strDriverName"].ToString(),        //司机姓名 
                                            Address_Dt.Rows[0]["strIdCard"].ToString(),            //司机身份证
                                            formData.carType,                                      //车型
                                            Address_Dt.Rows[0]["strConsigneeName"].ToString(),     //收货人姓名
                                            Address_Dt.Rows[0]["strCarplateNumber"].ToString(),    //车牌号
                                            formData.txtAddress,                                   //地址信息
                                            Address_Dt.Rows[0]["strConsigneeTel"].ToString(),      //收货人电话
                                            Address_Dt.Rows[0]["strDriverTel"].ToString(),         //司机电话
                                            HttpContext.Current.Request.Form["ccusname"],         //开票单位名称
                                            HttpContext.Current.Request.Form["cpersoncode"],       //业务员编号 
                                            formData.csccode,                                      //送货方式
                                            formData.datDeliveryDate,                              //提货时间
                                            formData.strLoadingWays,                               //装车方式
                                            formData.lngopUseraddressId

                                       );
            //OrderInfo oi = new OrderInfo(strBillNo, lngopUserId, datCreateTime, bytStatus, strRemarks, ccuscode, cdefine1, cdefine2, cdefine3, cdefine9, cdefine10, cdefine11, cdefine12, 
            //    cdefine13, ccusname, cpersoncode, cSCCode, datDeliveryDate, strLoadingWays, lngopUseraddressId);

            DataTable lngopOrderIdDt = order.DLproc_NewYOrderByUpd(oi, formData.areaid, formData.iaddresstype, formData.chdefine21, formData.txtArea);

            if (lngopOrderIdDt.Rows.Count < 0)
            {
                ri.flag = "0";
                ri.list_msg.Add("订单提交出错，请重试或联系管理员！");
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }
            #endregion

            #region 提交表体数据
            int lngopOrderId = Convert.ToInt32(lngopOrderIdDt.Rows[0]["lngopOrderId"].ToString());
            //  OrderInfo oiEntry1 = new OrderInfo(lngopOrderId, cinvcode, iquantity, inum, iquotedprice, iunitprice, itaxunitprice, imoney, itax, isum, inatunitprice, inatmoney, inattax, inatsum, kl, itaxrate, cDefine22, iinvexchrate, cunitid, irowno, cinvname, idiscount, inatdiscount, cComUnitName, cInvDefine1, cInvDefine2, cInvDefine13, cInvDefine14, unitGroup, cComUnitQTY, cInvDefine1QTY, cInvDefine2QTY, cn1cComUnitName, cpreordercode, autoid);
            bool x = new OrderManager().DL_NewOrderDetailByDel(lngopOrderId);
            foreach (DataRow dr in Check_Dt.Rows)
            {
                string cinvcode = dr["cinvcode"].ToString();   //存货编码
                double iquantity = Convert.ToDouble(dr["iquantity"].ToString());   //购买货数量
                double iinvexchrate = 1;//换算率 
                double inum = 1;        //辅计量数量   
                //计算销售单位(辅助)的换算率
                if (dr["cn1cComUnitName"].ToString() == dr["cInvDefine1"].ToString())  //大包装单位换算率
                {
                    iinvexchrate = Convert.ToDouble(dr["cInvDefine13"].ToString());
                }
                else if (dr["cn1cComUnitName"].ToString() == dr["cInvDefine2"].ToString()) //小包装单位换算率
                {
                    iinvexchrate = Convert.ToDouble(dr["cInvDefine14"].ToString());
                }
                else  //无换算单位
                {
                    iinvexchrate = 1;//换算率 
                }
                //计算赋值
                //换算结果:辅计量数量 =存货数量/换算率,四舍五入,保留两位小数
                inum = Math.Round(iquantity / iinvexchrate, 2);  //辅计量数量 
                double itaxrate = Convert.ToDouble(dr["itaxrate"].ToString());    //税率 

                double iquotedprice = Convert.ToDouble(dr["iquotedprice"].ToString());//报价 保留5位小数,四舍五入
                double itaxunitprice = Convert.ToDouble(dr["itaxunitprice"].ToString());//原币含税单价 即执行价格dt.Columns.Add("ExercisePrice"); //基本单位单价(执行价格)   16,保留5位
                double isum = Math.Round(itaxunitprice * iquantity, 2);     //原币价税合计    //金额,原币价税合计=原币含税单价*数量,保留2位小数,四舍五入 
                double itax = Math.Round(isum / (1 + itaxrate / 100) * (itaxrate / 100), 2);        //原币税额 ;税额=金额/1.17*0.17 保留2位, 四舍五入    
                double imoney = Math.Round(isum - itax, 2);      //原币无税金额 =金额-税额,保留2位,四舍五入
                double iunitprice = Math.Round(imoney / iquantity, 6);  //原币无税单价=无税金额/数量,保留5位小数,四舍五入        
                double idiscount = Math.Round(iquotedprice * iquantity - isum, 2);   //原币折扣额=报价*数量-金额,保留两位
                double inatunitprice = iunitprice;//本币无税单价
                double inatmoney = imoney;   //本币无税金额
                double inattax = itax;     //本币税额 
                double inatsum = isum;     //本币价税合计
                double inatdiscount = idiscount;   //本币折扣额 
                double kl = Convert.ToDouble(dr["kl"].ToString());          //扣率 
                string cDefine22 = dr["cDefine22"].ToString();  //表体自定义项22,包装量
                string cunitid = dr["cunitid"].ToString();    //计量单位编码
                int irowno = Convert.ToInt32(dr["irowno"].ToString()); //行号,从1开始,自增长
                string cinvname = dr["cinvname"].ToString();   //存货名称 
                string cComUnitName = dr["cComUnitName"].ToString();       //基本单位名称
                string cInvDefine1 = dr["cInvDefine1"].ToString();        //大包装单位名称         
                string cInvDefine2 = dr["cInvDefine2"].ToString();        //小包装单位名称 
                double cInvDefine13 = Convert.ToDouble(dr["cInvDefine13"].ToString());       //大包装换算率
                double cInvDefine14 = Convert.ToDouble(dr["cInvDefine14"].ToString());       //小包装换算率
                string unitGroup = dr["unitGroup"].ToString();          //单位换算率组
                double cComUnitQTY = Convert.ToDouble(dr["cComUnitQTY"].ToString());        //基本单位数量
                double cInvDefine1QTY = Convert.ToDouble(dr["cInvDefine1QTY"].ToString());     //大包装单位数量
                double cInvDefine2QTY = Convert.ToDouble(dr["cInvDefine2QTY"].ToString());     //小包装单位数量
                string cn1cComUnitName = dr["cn1cComUnitName"].ToString();    //销售单位名称
                string cpreordercode = dr["ccode"].ToString();    //销售预订单号
                string autoid = dr["iaoids"].ToString();    //预订单号id
                string cDefine24 = dr["cDefine24"].ToString();
                string cbdefine16 = dr["cbdefine16"].ToString();
                OrderInfo oiEntry = new OrderInfo(lngopOrderId, cinvcode, iquantity, inum, iquotedprice, iunitprice, itaxunitprice, imoney, itax, isum, inatunitprice, inatmoney, inattax, inatsum, kl, itaxrate, cDefine22, iinvexchrate, cunitid, irowno, cinvname, idiscount, inatdiscount, cComUnitName, cInvDefine1, cInvDefine2, cInvDefine13, cInvDefine14, unitGroup, cComUnitQTY, cInvDefine1QTY, cInvDefine2QTY, cn1cComUnitName, cpreordercode, autoid, cDefine24);
                //OrderInfo oiEntry=new OrderInfo(lngopOrderId,dr["cinvcode"].ToString(),Convert.ToDouble(dr["iquantity"].ToString()),Convert.ToDouble(dr["inum"].ToString()),Convert.ToDouble(dr["iquotedprice"].ToString()),Convert.ToDouble(dr["iunitprice"].ToString()),
                //                                                Convert.ToDouble(dr["itaxunitprice"].ToString()),Convert.ToDouble(dr["imoney"].ToString()),Convert.ToDouble(dr["itax"].ToString()),Convert.ToDouble(dr["isum"].ToString()),
                //                                                Convert.ToDouble(dr["inatunitprice"].ToString()),
                //                                                Convert.ToDouble(dr["inatmoney"].ToString()),Convert.ToDouble(dr["inattax"].ToString()),Convert.ToDouble(dr["inatsum"].ToString()),Convert.ToDouble(dr["kl"].ToString()),Convert.ToDouble(dr["itaxrate"].ToString()),dr["cDefine22"].ToString(),
                //                                                Convert.ToDouble(dr["iinvexchrate"].ToString()),dr["cunitid"].ToString(),Convert.ToInt32(dr["irowno"].ToString()),dr["cinvname"].ToString(),Convert.ToDouble(dr["idiscount"].ToString()),Convert.ToDouble(dr["inatdiscount"].ToString()),
                //                                                dr["cComUnitName"].ToString(),dr["cInvDefine1"].ToString(),dr["cInvDefine2"].ToString(),Convert.ToDouble(dr["cInvDefine13"].ToString()),Convert.ToDouble(dr["cInvDefine14"].ToString()),
                //                                                dr["unitGroup"].ToString(),Convert.ToDouble(dr["cComUnitQTY"].ToString()),Convert.ToDouble(dr["cInvDefine1QTY"].ToString()),Convert.ToDouble(dr["cInvDefine2QTY"].ToString()),
                //                                                dr["cn1cComUnitName"].ToString(), dr["ccode"].ToString(), dr["iaoids"].ToString());
                order.DLproc_NewYYOrderDetailByIns(oiEntry, cbdefine16, cDefine24);

            }
            ri.flag = "1";
            ri.message = strBillNo;
            #endregion
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            return;
        }
        #endregion

        #region 获取待审核订单列表
        public void DL_UnauditedOrder_SubBySel()
        {
            ReInfo ri = new ReInfo();

            ri.dt = order.DL_UnauditedOrder_SubBySel(1, HttpContext.Current.Session["lngopUserId"].ToString(), HttpContext.Current.Session["lngopUserExId"].ToString());
            ////初始化,设置金额合计启用报价合计还是执行价合计?
            //System.Collections.Hashtable ht = (System.Collections.Hashtable)HttpContext.Current.Session["SysSetting"];
            //if (ht["IsExercisePrice"].ToString() == "0")   //报价金额
            //{
            //    ri.message = "0";
            //}
            //else//执行价金额
            //{
            //    ri.message = "1";
            //}
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            return;
        }
        #endregion

        #region 获取订单明细
        public void DL_OrderBillBySel()
        {
            ReInfo ri = new ReInfo();
            ri.dt = order.DL_OrderBillBySel(HttpContext.Current.Request.Form["strBillNo"]);
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            return;
        }
        #endregion

        #region   获取历史订单列表
        public void DL_PreviousOrderBySel()
        {
            ReInfo ri = new ReInfo();
            string start_time = HttpContext.Current.Request.Form["start_date"];
            string end_time = HttpContext.Current.Request.Form["end_date"];
            string type = HttpContext.Current.Request.Form["type"];
            if (string.IsNullOrEmpty(start_time) && string.IsNullOrEmpty(end_time))
            {

                start_time = DateTime.Now.AddDays(-90).ToString("yyyy-MM-dd");
                end_time = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else if (!string.IsNullOrEmpty(start_time) && string.IsNullOrEmpty(end_time))
            {

                end_time = Convert.ToDateTime(start_time).AddDays(90).ToString();
            }
            else if (string.IsNullOrEmpty(start_time) && !string.IsNullOrEmpty(end_time))
            {

                start_time = Convert.ToDateTime(end_time).AddDays(-90).ToString();
            }
            if (Convert.ToDateTime(start_time) > Convert.ToDateTime(end_time))
            {
                ri.flag = "0";
                ri.message = "结束时间不能大于开始时间！";
                ri.dt = null;
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }
            double diffday = double.Parse(check.DateDiff(start_time, end_time)["day"].ToString());
            if (diffday > 90)
            {
                ri.flag = "0";
                ri.message = "开始时间与结束时间间隔不能大于3个月！";
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }
            if (type == "0")
            {

                ri.dt = order.DL_PreviousOrderBySel(HttpContext.Current.Session["ConstcCusCode"].ToString() + "%", start_time, end_time);
                ri.flag = "1";
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }
            else
            {
                ri.dt = order.DL_PreviousInvalidOrderBySel(HttpContext.Current.Session["ConstcCusCode"].ToString() + "%", start_time, end_time);
                ri.flag = "1";
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }


        }
        #endregion

        #region 根据正式订单号查询历史订单明细
        public void DL_OrderU8BillBySel()
        {
            ReInfo ri = new ReInfo();
            string strBillNo = HttpContext.Current.Request.Form["strBillNo"];
            if (strBillNo.Substring(0, 4).ToString() == "CZTS")
            {
                ri.dt = order.DL_OrderCZTSBillBySel(strBillNo);
            }
            else
            {
                ri.dt = order.DL_OrderU8BillBySel(strBillNo);
            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            return;
        }
        #endregion

        #region 根据U8单据号查询明细
        public void DL_OrderU8BillBySel_new()
        {
            ReInfo ri = new ReInfo();
            ri.dt = new product().DL_OrderU8BillBySel(HttpContext.Current.Request.Form["strBillNo"]);
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            return;
        }
        #endregion

        #region 确认未确认的U8订单
        public void DL_U8OrderBillConfirmByUpd()
        {
            ReInfo ri = new ReInfo();
            bool b = order.DL_U8OrderBillConfirmByUpd(HttpContext.Current.Request.Form["strBillNo"]);
            if (!b)
            {
                ri.flag = "0";
                ri.message = "订单确认失败,请重试或联系管理员!";
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }
            ri.flag = "1";
            ri.message = "订单确认成功!";
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            return;
        }
        #endregion

        #region 获取待确认订单列表
        public void DL_UnauditedOrderBySel()
        {
            ReInfo ri = new ReInfo();
            ri.dt = new product().DL_UnauditedOrderBySel(2, HttpContext.Current.Session["ConstcCusCode"].ToString() + "%");
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            return;
        }
        #endregion

        #region 查询特殊订单列表
        public void DLproc_MyWorkPreYOrderForCustomerBySel()
        {
            ReInfo ri = new ReInfo();
            string start_time = HttpContext.Current.Request.Form["start_date"];
            string end_time = HttpContext.Current.Request.Form["end_date"];
            if (string.IsNullOrEmpty(start_time))
            {
                start_time = "2015-12-01";
            }
            if (string.IsNullOrEmpty(end_time))
            {
                end_time = "2099-01-01";
            }
            if (Convert.ToDateTime(start_time) > Convert.ToDateTime(end_time))
            {
                ri.flag = "0";
                ri.message = "结束时间不能大于开始时间！";
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }
            string strBillNo = HttpContext.Current.Request.Form["strbillno"];
            int OrderStatus = Convert.ToInt32(HttpContext.Current.Request.Form["orderstatus"]);
            ri.dt = order.DLproc_MyWorkPreYOrderForCustomerBySel(strBillNo, start_time, end_time, OrderStatus, HttpContext.Current.Session["ConstcCusCode"].ToString(), "2");
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            return;

        }
        #endregion

        #region 返回某个特殊订单明细
        public void DL_XOrderBillDetailBySel()
        {
            ReInfo ri = new ReInfo();
            string strBillNo = HttpContext.Current.Request.Form["strBillNo"];
            ri.dt = order.DL_XOrderBillDetailBySel(strBillNo);
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            return;
        }
        #endregion

        #region 新增特殊订单(预订订单20190121)
        public void DLproc_NewYOrderByIns()
        {
            try
            {
                ReInfo ri = new ReInfo();
                string date = DateTime.Now.ToString();
                string ccuscode = HttpContext.Current.Request.Form["ccuscode"];
                if (string.IsNullOrEmpty(ccuscode) || ccuscode == "0")
                {
                    ri.flag = "0";
                    ri.message = "请选择开票单位!";
                    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                    return;
                }
                string lngopUserId = HttpContext.Current.Session["lngopUserId"].ToString(); //用户id
                int bytStatus = 1;  //单据状态
                int lngBillType = 2;  //单据类型，酬宾订单1，特殊订单2
                string ccusname = HttpContext.Current.Request.Form["ccusname"]; //客户名称    
                //string cdiscountname = TxtCBLX.Text;//促销类型,2016-04-26添加
                string cdiscountname = HttpContext.Current.Request.Form["cdiscountname"] + "-特殊订单";
                string cMemo = HttpContext.Current.Request.Form["strRemarks"];
                string lngopUserExId = HttpContext.Current.Session["lngopUserExId"].ToString(); //20160826添加
                string strAllAcount = HttpContext.Current.Session["strAllAcount"].ToString();   //20160826添加
                string datDeliveryDate = HttpContext.Current.Request.Form["datDeliveryDate"];   //20190121添加

                //获取表体数据
                List<Buy_list> list = JsonConvert.DeserializeObject<List<Buy_list>>(HttpContext.Current.Request.Form["buy_list"]);
                DataTable Check_dt = pro.Return_Check_Dt(list, ccuscode, "0");

                #region 验证表体数据 验证订单的每个商品数量是否为0或小于起订量
                foreach (DataRow row in Check_dt.Rows)
                {
                    if (Convert.ToDouble(row["iquantity"].ToString()) == 0 || Convert.ToDouble(row["iquantity"].ToString()) < Convert.ToDouble(row["MinOrderQTY"].ToString()))
                    {
                        ri.flag = "0";
                        ri.message = "订单里有未输入数量或小于起订量的商品,请核实!";
                        HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                        return;
                    }
                }
                #endregion

                //插入表头数据,DL表中
                DataTable lngopOrderIdDt = new DataTable();
                lngopOrderIdDt = order.DLproc_NewYOrderByIns(date, lngopUserId, bytStatus, ccuscode, ccusname, lngBillType, cdiscountname, cMemo, lngopUserExId, strAllAcount, datDeliveryDate);

                if (lngopOrderIdDt.Rows.Count == 0)
                {
                    ri.flag = "0";
                    ri.message = "订单提交失败，请重试或联系管理员!";
                    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                    return;
                }

                //插入表体数据
                int lngopOrderId = Convert.ToInt32(lngopOrderIdDt.Rows[0]["lngopOrderId"].ToString());   //订单id,从表头中获取:表头插入后,返回表头id
                string strBillNo = lngopOrderIdDt.Rows[0]["strBillNo"].ToString();   //订单编号,插入表头数据时自动生成,用于反馈给用户

                foreach (DataRow dr in Check_dt.Rows)
                {
                    string cinvcode = dr["cinvcode"].ToString();   //存货编码
                    double iquantity = Convert.ToDouble(dr["iquantity"].ToString());   //购买货数量
                    double iinvexchrate = 1;//换算率 
                    double inum = 1;        //辅计量数量   
                    //计算销售单位(辅助)的换算率
                    if (dr["cn1cComUnitName"].ToString() == dr["cInvDefine1"].ToString())  //大包装单位换算率
                    {
                        iinvexchrate = Convert.ToDouble(dr["cInvDefine13"].ToString());
                    }
                    else if (dr["cn1cComUnitName"].ToString() == dr["cInvDefine2"].ToString()) //小包装单位换算率
                    {
                        iinvexchrate = Convert.ToDouble(dr["cInvDefine14"].ToString());
                    }
                    else  //无换算单位
                    {
                        iinvexchrate = 1;//换算率 
                    }
                    //计算赋值
                    //换算结果:辅计量数量 =存货数量/换算率,四舍五入,保留两位小数
                    inum = Math.Round(iquantity / iinvexchrate, 2);  //辅计量数量 
                    double itaxrate = Convert.ToDouble(dr["iTaxRate"].ToString());    //税率 

                    double iquotedprice = Convert.ToDouble(dr["Quote"].ToString());//报价 保留5位小数,四舍五入
                    double itaxunitprice = Convert.ToDouble(dr["ExercisePrice"].ToString());//原币含税单价 即执行价格dt.Columns.Add("ExercisePrice"); //基本单位单价(执行价格)   16,保留5位
                    double isum = Math.Round(itaxunitprice * iquantity, 2);     //原币价税合计    //金额,原币价税合计=原币含税单价*数量,保留2位小数,四舍五入 
                    double itax = Math.Round(isum / (1 + itaxrate / 100) * (itaxrate / 100), 2);        //原币税额 ;税额=金额/1.17*0.17 保留2位, 四舍五入    
                    double imoney = Math.Round(isum - itax, 2);      //原币无税金额 =金额-税额,保留2位,四舍五入
                    double iunitprice = Math.Round(imoney / iquantity, 6);  //原币无税单价=无税金额/数量,保留5位小数,四舍五入        
                    double idiscount = Math.Round(iquotedprice * iquantity - isum, 2);   //原币折扣额=报价*数量-金额,保留两位
                    double inatunitprice = iunitprice;//本币无税单价
                    double inatmoney = imoney;   //本币无税金额
                    double inattax = itax;     //本币税额 
                    double inatsum = isum;     //本币价税合计
                    double inatdiscount = idiscount;   //本币折扣额 
                    double kl = Convert.ToDouble(dr["Rate"].ToString());          //扣率 
                    string cDefine22 = dr["cDefine22"].ToString();  //表体自定义项22,包装量
                    string cunitid = dr["cComUnitCode"].ToString();    //计量单位编码
                    int irowno = Convert.ToInt32(dr["irowno"].ToString()); //行号,从1开始,自增长
                    string cinvname = dr["cInvName"].ToString();   //存货名称 
                    string cComUnitName = dr["cComUnitName"].ToString();       //基本单位名称
                    string cInvDefine1 = dr["cInvDefine1"].ToString();        //大包装单位名称         
                    string cInvDefine2 = dr["cInvDefine2"].ToString();        //小包装单位名称 
                    double cInvDefine13 = Convert.ToDouble(dr["cInvDefine13"].ToString());       //大包装换算率
                    double cInvDefine14 = Convert.ToDouble(dr["cInvDefine14"].ToString());       //小包装换算率
                    string unitGroup = dr["unitGroup"].ToString();          //单位换算率组
                    double cComUnitQTY = Convert.ToDouble(dr["cComUnitQTY"].ToString());        //基本单位数量
                    double cInvDefine1QTY = Convert.ToDouble(dr["cInvDefine1QTY"].ToString());     //大包装单位数量
                    double cInvDefine2QTY = Convert.ToDouble(dr["cInvDefine2QTY"].ToString());     //小包装单位数量
                    string cn1cComUnitName = dr["cn1cComUnitName"].ToString();    //销售单位名称
                    string cDefine24 = dr["cDefine24"].ToString();

                    OrderInfo oiEntry = new OrderInfo(lngopOrderId, cinvcode, iquantity, inum, iquotedprice, iunitprice, itaxunitprice, imoney, itax, isum, inatunitprice, inatmoney, inattax, inatsum, kl, itaxrate, cDefine22, iinvexchrate, cunitid, irowno, cinvname, idiscount, inatdiscount, cComUnitName, cInvDefine1, cInvDefine2, cInvDefine13, cInvDefine14, unitGroup, cComUnitQTY, cInvDefine1QTY, cInvDefine2QTY, cn1cComUnitName, cDefine24);

                    bool b = order.DLproc_NewYOrderDetailByIns(oiEntry);

                }
                ri.flag = "1";
                ri.message = strBillNo;

                //2017-08-23增加  插入特殊订单扩展表 Dl_opPreOrderDetail_Ex
                pro.Insert_Dl_opPreOrderDetail_Ex(strBillNo);

                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }
            catch (Exception err)
            {

                errMsg.list_msg.Add("Action:" + HttpContext.Current.Request.Form["Action"]);
                errMsg.list_msg.Add("ConstcCusCode:" + HttpContext.Current.Session["ConstcCusCode"].ToString());
                errMsg.list_msg.Add(err.ToString());
                check.WriteLog(errMsg);
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(errMsg));
                return;
            }



        }
        #endregion

        #region 获取被驳回订单列表
        public void Get_RejectOrder()
        {
            DataTable dt = order.DL_UnauditedOrder_SubBySel(3, HttpContext.Current.Session["lngopUserId"].ToString(), HttpContext.Current.Session["lngopUserExId"].ToString());
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(dt));
            return;
        }
        #endregion

        #region 获取被驳回订单详细
        public void Get_RejectOrder_Detail()
        {
            string strBillNo = HttpContext.Current.Request.Form["strBillNo"];
            DataTable dt = order.DL_OrderBillBySel(strBillNo);
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(dt));
            return;
        }
        #endregion

        #region 作废订单
        public void Cancel_Order()
        {
            string strBillNo = HttpContext.Current.Request.Form["strBillNo"];
            ReInfo ri = new ReInfo();
            bool c = order.DL_InvalidOrderByUpd(strBillNo, HttpContext.Current.Session["lngopUserId"].ToString());
            if (c)
            {
                ri.flag = "1";
            }
            else
            {
                ri.flag = "0";
            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            return;
        }
        #endregion

        #region 修改普通订单和样品订单界面获取订单详情
        public void DLproc_OrderDetailModifyBySel()
        {
            ReInfo ri = new ReInfo();
            ri.messages = new List<string[]>();
            List<string> li = new List<string>();
            string strBillNo = HttpContext.Current.Request.Form["strBillNo"];
            DataTable dt = order.DLproc_OrderDetailModifyBySel(strBillNo);
            ri.dt = order.DL_OrderModifyBySel(strBillNo);
            string kpdw = ri.dt.Rows[0]["ccuscode"].ToString();
            int iShowType = Convert.ToInt32(HttpContext.Current.Request.Form["iShowType"].ToString());

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!pro.DLproc_cInvCodeIsBeLimitedBySel(dt.Rows[i]["cInvCode"].ToString(), kpdw, iShowType))
                {
                    li.Add(dt.Rows[i]["cInvName"].ToString());
                    dt.Rows[i].Delete();
                }
            }
            ri.msg = li.ToArray();
            dt.AcceptChanges();
            ri.datatable = dt;

            ri.CusCredit_dt = order.DLproc_getCusCreditInfoWithBillno(kpdw, strBillNo);  //获取信用
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            return;
        }
        #endregion

        #region 修改参照特殊订单获取订单详情
        public void DLproc_QuasiModifyOrderDetail_CZTSBySel()
        {
            ReInfo ri = new ReInfo();
            string strBillNo = HttpContext.Current.Request.Form["strBillNo"];
            ri.dt = pro.DLproc_QuasiModifyOrderDetail_CZTSBySel(strBillNo);
            string kpdw = ri.dt.Rows[0]["ccuscode"].ToString();
            ri.CusCredit_dt = order.DLproc_getCusCreditInfoWithBillno(kpdw, strBillNo);  //获取信用
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
        }
        #endregion

        #region 更新样品订单
        public void DLproc_SampleOrderByUpd()
        {
            try
            {
                ReInfo ri = new ReInfo();
                //获取表头信息
                string kpdw = HttpContext.Current.Request.Form["kpdw"];
                OrderInfo oi = new OrderInfo(
                    HttpContext.Current.Request.Form["strBillNo"],
                    HttpContext.Current.Request.Form["strRemarks"],
                    HttpContext.Current.Request.Form["strLoadingWays"],
                    1
                    );
                if (string.IsNullOrEmpty(kpdw))
                {
                    ri.flag = "0";
                    ri.message = "开票单位错误!";
                    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                    return;
                }

                //获取表体数据,拼接为check_dt
                string li = HttpContext.Current.Request.Form["listData"];
                DataTable Check_Dt = new DataTable();
                if (!string.IsNullOrEmpty(li))
                {
                    List<Buy_list> listData = JsonConvert.DeserializeObject<List<Buy_list>>(li);
                    Check_Dt = pro.Return_Check_Dt(listData, kpdw, "0");
                }

                ////验证商品允限销
                //ri = check.Check_limit(Check_Dt, kpdw, 1);
                //if (ri.flag != "1")
                //{
                //    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                //    return;
                //}


                DataTable lngopOrderIdDt = new DataTable();
                lngopOrderIdDt = order.DLproc_SampleOrderByUpd(oi);  //更新数据库表头数据
                if (lngopOrderIdDt.Rows.Count == 0)
                {
                    ri.flag = "0";
                    ri.message = "保存订单失败，请重试或联系管理员！";
                    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                    return;
                }
                int lngopOrderId = Convert.ToInt32(lngopOrderIdDt.Rows[0]["lngopOrderId"].ToString());
                string strBillNo = oi.StrBillNo;

                bool b = new OrderManager().DL_NewOrderDetailByDel(lngopOrderId); //删除订单老数据

                string[] back = new string[]{
            lngopOrderId.ToString(),
            strBillNo
            };



                //插入表体数据
                Insert_listData(Check_Dt, lngopOrderId);
                ri.flag = "1";
                ri.message = strBillNo;
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                return;

            }
            catch (Exception err)
            {

                errMsg.list_msg.Add("Action:" + HttpContext.Current.Request.Form["Action"]);
                errMsg.list_msg.Add("ConstcCusCode:" + HttpContext.Current.Session["ConstcCusCode"].ToString());
                errMsg.list_msg.Add(err.ToString());
                check.WriteLog(errMsg);
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(errMsg));
                return;
            }


        }
        #endregion

        #region 提交修改后的普通订单
        public void DLproc_NewOrderByUpd()
        {
            try
            {
                #region 2016-09-13,检查是否存在未参照完的酬宾订单的商品
                //string Strpreorderleft = "行：";
                //if (griddata.Rows.Count > 0)
                //{
                //    for (int i = 0; i < griddata.Rows.Count; i++)
                //    {
                //        DataTable preorderleft = new OrderManager().DLproc_PerOrderCinvcodeLeftBySel(Session["lngopUserExId"].ToString(), Session["lngopUserId"].ToString(), Session["KPDWcCusCode"].ToString(), griddata.Rows[i][0].ToString());
                //        if (Convert.ToDouble(preorderleft.Rows[0][0].ToString()) > 0)
                //        {
                //            Strpreorderleft = Strpreorderleft + griddata.Rows[i]["irowno"].ToString() + "," + griddata.Rows[i]["cInvName"].ToString() + "数量：" + preorderleft.Rows[0][0].ToString() + ";";
                //        }
                //    }
                //}
                ////提示有未完成的酬宾订单
                //if (Strpreorderleft.ToString() != "行：")
                //{
                //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('" + Strpreorderleft.ToString() + "存在未参照完的酬宾订单，请先将酬宾订单的商品参照完毕后在普通订单中购买该商品！" + "');</script>");
                //    return;
                //}
                #endregion
                string strBillNo = HttpContext.Current.Request.Form["strBillNo"];
                ReInfo ri = new ReInfo();

                #region 根据订单号获取订单信息，判断是否超过半小时
                DataTable dt = new DataTable();
                dt = pro.GetOrderByBillNo(strBillNo);
                if (!string.IsNullOrEmpty(dt.Rows[0]["recaptionDate"].ToString()) && string.IsNullOrEmpty(dt.Rows[0]["strManagers"].ToString()))
                {
                    JObject jo = check.DateDiff(dt.Rows[0]["recaptionDate"].ToString(), DateTime.Now.ToString());
                    if (double.Parse(jo["sec"].ToString()) > 1800)
                    {
                        bool c = order.DL_InvalidOrderByUpd(strBillNo, HttpContext.Current.Session["lngopUserId"].ToString());
                        if (c)
                        {
                            ri.flag = "2";
                            ri.message = "订单取回超过半小时，已被系统自动关闭！";
                        }
                        else
                        {
                            ri.flag = "3";
                            ri.message = "订单提交失败！";
                        }
                        HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                        return;
                    }
                }
                #endregion

                //if (dt.Rows[0]["bytStatus"].ToString() != "3")
                //{
                //    ri.flag = "0";
                //    ri.message = "订单状态不正确，请联系管理员";
                //    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                //    return;
                //}

                ri.flag = "1";
                //获取表头数据实例化为model
                form_Data formData = JsonConvert.DeserializeObject<form_Data>(HttpContext.Current.Request.Form["formData"]);
                string kpdw = formData.ccuscode;
                string cWhCode = HttpContext.Current.Request.Form["cWhCode"];
                //获取表体数据实例化model
                List<Buy_list> listData = JsonConvert.DeserializeObject<List<Buy_list>>(HttpContext.Current.Request.Form["listData"]);


                //验证允限销
                ri = check.Check_limit(dt, kpdw, 1);
                if (ri.flag == "7")
                {
                    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                    return;
                }

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < listData.Count; i++)
                {
                    sb.Append(listData[i].cinvcode + "|");
                }

                DataTable check_dt = pro.Get_ModifyOrder_Dt(strBillNo, sb.ToString().TrimEnd('|'), kpdw, cWhCode, formData.areaid, listData);

                #region 验证表单开始
                ri.list_msg = new List<string>();
                // dt = order.DLproc_getCusCreditInfo(formData.ccuscode);
                dt = order.DLproc_getCusCreditInfoWithBillno(formData.ccuscode, strBillNo); // 重新获取信用
                double CusCredit = Convert.ToDouble(dt.Rows[0]["iCusCreLine"].ToString()); //客户信用
                double listCredit = 0;   //用于累计表单金额
                int rowType = 0; //用于记录表体中是否有不合格数据

                foreach (DataRow dr in check_dt.Rows)
                {
                    listCredit += Convert.ToDouble(dr["iquantity"].ToString()) * Convert.ToDouble(dr["ExercisePrice"].ToString());
                    if (Convert.ToDouble(dr["fAvailQtty"].ToString()) == 0)
                    {
                        dr["rowType"] = 1;
                    }
                    else if (Convert.ToDouble(dr["iquantity"].ToString()) > Convert.ToDouble(dr["fAvailQtty"].ToString()))
                    {
                        dr["rowType"] = 2;
                    }

                    rowType += Convert.ToInt32(dr["rowType"].ToString());
                }
                if (CusCredit != -99999999.000000)  //-99999999.000000为现金用户
                {
                    if (listCredit - CusCredit > 0)
                    {
                        ri.flag = "0";
                        ri.list_msg.Add("你的购买金额已超过信用" + Math.Round((listCredit - CusCredit), 2));

                    }
                    if (rowType != 0)
                    {
                        ri.flag = "0";

                        ri.list_msg.Add("订单列表里有产品数量为0或者大于库存，请重新输入!");
                    }
                    if (ri.flag == "0")
                    {
                        ri.dt = check_dt;
                        HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                        return;
                    }
                }
                else
                {
                    if (rowType != 0)
                    {
                        ri.flag = "0";
                        ri.list_msg.Add("订单列表里有产品数量为0或者大于库存，请重新输入!");

                    }
                    if (ri.flag == "0")
                    {
                        ri.dt = check_dt;
                        HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                        return;
                    }
                }
                #endregion

                //验证通过后，开始提交数据
                #region 获取表头信息，提交表头数据
                DataTable Address_Dt = new product().Get_AddressById(formData.lngopUseraddressId);
                //  OrderInfo oi1 = new OrderInfo(strBillNo, lngopUserId, datCreateTime, bytStatus, strRemarks, ccuscode, cdefine1, cdefine2, cdefine3, cdefine9, cdefine10, cdefine11, cdefine12, cdefine13, ccusname, cpersoncode, cSCCode, datDeliveryDate, strLoadingWays, lngopUseraddressId, cdiscountname);

                OrderInfo oi = new OrderInfo(strBillNo,
                                                HttpContext.Current.Session["lngopUserId"].ToString(), //客户ID
                                                DateTime.Now.ToString(),                               //订单时间                     
                                                1,                                                     //订单状态         
                                                formData.strRemarks,                                   //备注     
                                                formData.ccuscode,                                     //开票单位ID 
                                                Address_Dt.Rows[0]["strDriverName"].ToString(),        //司机姓名 
                                                Address_Dt.Rows[0]["strIdCard"].ToString(),            //司机身份证
                                                formData.carType,                                      //车型
                                                Address_Dt.Rows[0]["strConsigneeName"].ToString(),     //收货人姓名
                                                Address_Dt.Rows[0]["strCarplateNumber"].ToString(),     //车牌号
                                                formData.txtAddress,                                    //地址信息
                                                Address_Dt.Rows[0]["strConsigneeTel"].ToString(),      //收货人电话
                                                Address_Dt.Rows[0]["strDriverTel"].ToString(),         //司机电话
                                                formData.ccusname,                                     //开票单位名称
                                                formData.ccuspperson,                                  //业务员编号 
                                                formData.csccode,                                      //送货方式
                                                formData.datDeliveryDate,                              //提货时间
                                                formData.strLoadingWays,                               //装车方式
                                                formData.lngopUseraddressId,                           //地址ID
                                                dt.Rows[0]["cdiscountname"].ToString()               //lngBillType 特殊订单
                                           );
                DataTable lngopOrderIdDt = order.DLproc_NewOrderByUpd(oi, formData.areaid, formData.iaddresstype, formData.chdefine21, formData.txtArea, formData.cWhCode);  //更新数据库表头数据
                int lngopOrderId = Convert.ToInt32(lngopOrderIdDt.Rows[0]["lngopOrderId"].ToString());

                if (lngopOrderIdDt.Rows.Count < 0)
                {
                    ri.flag = "0";
                    ri.list_msg.Add("订单提交出错，请重试或联系管理员！");
                    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                    return;
                }
                #endregion

                #region 插入表体数据
                bool b = order.DL_NewOrderDetailByDel(lngopOrderId);
                if (!b)
                {
                    ri.flag = "0";
                    ri.message = "保存订单失败，请重试或联系管理员！";
                    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                    return;
                }
                Insert_listData(check_dt, lngopOrderId);
                #endregion

                ri.flag = "1";
                ri.message = strBillNo;
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }
            catch (Exception err)
            {

                errMsg.list_msg.Add("Action:" + HttpContext.Current.Request.Form["Action"]);
                errMsg.list_msg.Add("ConstcCusCode:" + HttpContext.Current.Session["ConstcCusCode"].ToString());
                errMsg.list_msg.Add(err.ToString());
                check.WriteLog(errMsg);
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(errMsg));
                return;
            }


        }
        #endregion

        #region 获取开票单位
        public void Get_KPDW()
        {
            ReInfo ri = new ReInfo();
            ri.dt = new SearchManager().DL_ComboCustomerAllBySel(HttpContext.Current.Session["ConstcCusCode"].ToString() + "%");
            ri.datatable = pro.Get_Acount(HttpContext.Current.Session["ConstcCusCode"].ToString() + "%");
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
        }
        #endregion

        #region 获取用户账单列表
        public void DLproc_U8SOASearchBySel()
        {
            ReInfo ri = new ReInfo();
            DataTable dt = new SearchManager().DLproc_U8SOASearchBySel(
                HttpContext.Current.Request.Form["year"],
                HttpContext.Current.Request.Form["kpdw"],
                HttpContext.Current.Request.Form["month"],
                HttpContext.Current.Request.Form["check"],
                HttpContext.Current.Session["ConstcCusCode"].ToString()

                );
            ri.dt = dt;
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
        }
        #endregion

        #region 确认账单
        public void DL_ConfimSOAByUpd()
        {
            ReInfo ri = new ReInfo();
            bool b = order.DL_ConfimSOAByUpd(HttpContext.Current.Request.Form["id"]);
            if (b)
            {
                ri.flag = "1";
                ri.message = "账单确认成功，感谢使用！";
            }
            else
            {
                ri.flag = "0";
                ri.message = "账单确认失败，请联系系统管理员！";
            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
        }
        #endregion

        #region 获取账单明细
        public void DLproc_SOADetailforCustomerBySel()
        {
            ReInfo ri = new ReInfo();
            string kpdw = HttpContext.Current.Request.Form["kpdw"];
            if (string.IsNullOrEmpty(kpdw))
            {
                ri.flag = "0";
                ri.message = "单位不能为空！";
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }
            string start_time = HttpContext.Current.Request.Form["start_date"];
            string end_time = HttpContext.Current.Request.Form["end_date"];
            if (string.IsNullOrEmpty(start_time))
            {
                start_time = "2015-12-01";
            }
            if (string.IsNullOrEmpty(end_time))
            {
                end_time = "2099-01-01";
            }
            if (Convert.ToDateTime(start_time) > Convert.ToDateTime(end_time))
            {
                ri.flag = "0";
                ri.message = "结束时间不能大于开始时间！";
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }
            ri.flag = "1";
            ri.dt = new SearchManager().DLproc_SOADetailforCustomerBySel(start_time, end_time, kpdw);
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            return;

        }
        #endregion

        #region 查询订单执行情况
        public void DLproc_OrderExecuteBySel()
        {
            ReInfo ri = new ReInfo();
            string timetype = HttpContext.Current.Request.Form["timetype"];
            if (string.IsNullOrEmpty(timetype))
            {
                ri.flag = "0";
                ri.message = "查询类别不合法！";
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }
            string start_time = HttpContext.Current.Request.Form["start_date"];
            string end_time = HttpContext.Current.Request.Form["end_date"];
            if (string.IsNullOrEmpty(start_time))
            {
                start_time = "2015-12-01";
            }
            if (string.IsNullOrEmpty(end_time))
            {
                end_time = "2099-01-01";
            }
            if (Convert.ToDateTime(start_time) > Convert.ToDateTime(end_time))
            {
                ri.flag = "0";
                ri.message = "结束时间不能大于开始时间！";
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }
            string strbillno = HttpContext.Current.Request.Form["strbillno"];
            string kpdw = HttpContext.Current.Request.Form["kpdw"];
            string Acount = HttpContext.Current.Request.Form["Acount"] == "0" ? "" : HttpContext.Current.Request.Form["Acount"];
            string all = ConfigurationManager.AppSettings["isSearchAll"];
            Array arr = all.Split('|');
            if (Array.IndexOf(arr, HttpContext.Current.Session["ConstcCusCode"].ToString()) == -1 && HttpContext.Current.Session["strAllAcount"].ToString().Length != 6)
            {
                if (Acount != HttpContext.Current.Session["strAllAcount"].ToString())
                {
                    ri.flag = "0";
                    ri.message = "你不是主账号，只能选择下单账号为【" + HttpContext.Current.Session["strAllAcount"].ToString() + "】后进行查询";
                    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                    return;
                }
            }

            //if (HttpContext.Current.Session["strAllAcount"].ToString().Length != 6 && Acount != HttpContext.Current.Session["strAllAcount"].ToString())
            //{
            //    ri.flag = "0";
            //    ri.message = "你不是主账号，只能选择下单账号为【" + HttpContext.Current.Session["strAllAcount"].ToString() + "】后进行查询";
            //    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            //    return;
            //}
            if (kpdw == "0")
            {
                kpdw = HttpContext.Current.Session["ConstcCusCode"].ToString() + "%";
            }
            string showtype = HttpContext.Current.Request.Form["showtype"];
            string fhtype = HttpContext.Current.Request.Form["fhtype"];
            if (timetype == "0") //下单时间查询
            {
                DataTable dt = new SearchManager().DLproc_OrderExecuteBySel(strbillno, kpdw, start_time, end_time, showtype, fhtype, Acount);

                ri.flag = "1";
                ri.dt = dt;

            }
            else if (timetype == "1")  //审核时间查询
            {
                DataTable dt = new SearchManager().DLproc_OrderExecuteFordatAuditordTimeBySel(strbillno, kpdw, start_time, end_time, showtype, fhtype, Acount);
                ri.flag = "1";
                ri.dt = dt;

            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            return;
        }
        #endregion

        #region 修改被驳回的订单时,获取信用金额
        public void DLproc_getCusCreditInfoWithBillno()
        {
            string kpdw = HttpContext.Current.Request.Form["kpdw"];
            string strBillNo = HttpContext.Current.Request.Form["strBillNo"];
            DataTable dt = order.DLproc_getCusCreditInfoWithBillno(kpdw, strBillNo);
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(dt));
        }
        #endregion

        #region 判断单点登录
        public void check_one()
        {
            ReInfo ri = new ReInfo();
            ri.flag = "1";
            Hashtable hOnline = (Hashtable)HttpContext.Current.Application["Online"];
            //Session.Contents.Remove("login");
            if (hOnline.Contains(HttpContext.Current.Session["strAllAcount"].ToString()))       //判断哈希表是否包含特定键,其返回值为true或false
            {
                //5秒更新在线状况
                string s = (string)hOnline[HttpContext.Current.Session["strAllAcount"].ToString()];
                string sId = HttpContext.Current.Session.SessionID.ToString();  //获取当前session的id
                if (s != sId)
                {
                    HttpContext.Current.Session["login"] = null;
                    HttpContext.Current.Session.Contents.Remove("login");
                    ri.flag = "0";
                    //Warm.Text = "该用户已在其他地点登录!";
                    //  HttpContext.Current.SessionTimer.Enabled = false;
                    //Response.Redirect("logout.aspx?id=1");
                    //System.Web.UI.ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "updateScript", "alert('该用户已在其他地方登录，如非您本人操作，请及时联系管理员！');", true);                
                }

            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            return;
        }
        #endregion

        #region 获取用户所有的收货地址及行政区配置
        public void Get_User_Address()
        {
            JObject jo = new JObject();
            DataSet ds = pro.Get_User_Address(HttpContext.Current.Session["lngopUserId"].ToString());
            jo["flag"] = 1;
            jo["DataSet"] = JToken.Parse(JsonConvert.SerializeObject(ds));
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 删除用户地址
        public void Del_UserAddress()
        {
            ReInfo ri = new ReInfo();
            bool b = new BasicInfoManager().DL_UserAddressByDel(HttpContext.Current.Request.Form["lngopUseraddressId"]);
            //if (!b)
            //{
            //    ri.flag = "0";
            //    ri.message = "删除失败,请重试或联系管理员!";
            //    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            //    return;
            //}
            ri.flag = "1";
            ri.message = "地址删除成功!";
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            return;
        }
        #endregion

        #region 更新自提信息
        public void Update_UserAddressZT()
        {
            ReInfo ri = new ReInfo();
            BasicInfo bi = new BasicInfo(
                HttpContext.Current.Request.Form["lngopUseraddressId"],
                HttpContext.Current.Request.Form["strCarplateNumber"],
                HttpContext.Current.Request.Form["strDriverName"],
                HttpContext.Current.Request.Form["strDriverTel"],
                HttpContext.Current.Request.Form["strIdCard"]
                );
            bool b = new BasicInfoManager().Update_UserAddressZT(bi);
            if (!b)
            {
                ri.flag = "0";
                ri.message = "更新失败,请重试或联系管理员!";
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }
            ri.flag = "1";
            ri.message = "自提信息更新成功!";
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            return;
        }
        #endregion

        #region 更新配送信息
        public void Update_UserAddressPS()
        {
            ReInfo ri = new ReInfo();
            BasicInfo bi = new BasicInfo(
                HttpContext.Current.Request.Form["lngopUseraddressId"],
                HttpContext.Current.Request.Form["strConsigneeName"],
                HttpContext.Current.Request.Form["strConsigneeTel"],
                HttpContext.Current.Request.Form["strReceivingAddress"].Replace("，", "").Replace(",", "").Replace("，", ""),
                HttpContext.Current.Request.Form["area"],
                "ps"
                );
            bool b = new BasicInfoManager().Update_UserAddressPS(bi);
            if (!b)
            {
                ri.flag = "0";
                ri.message = "更新失败,请重试或联系管理员!";
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }
            ri.flag = "1";
            ri.message = "配送信息更新成功!";
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            return;
        }
        #endregion

        #region 更新自提托运信息
        public void Update_UserAddressZTPS()
        {
            ReInfo ri = new ReInfo();
            BasicInfo bi = new BasicInfo(
                HttpContext.Current.Request.Form["lngopUseraddressId"],
                HttpContext.Current.Request.Form["strConsigneeName"],
                HttpContext.Current.Request.Form["strConsigneeTel"],
                HttpContext.Current.Request.Form["strReceivingAddress"].Replace("，", "").Replace(",", "").Replace("，", ""),
                HttpContext.Current.Request.Form["area"],
                "ps"
                );
            bool b = new BasicInfoManager().Update_UserAddressZTPS(bi, HttpContext.Current.Request.Form["ccodeid"]);
            if (!b)
            {
                ri.flag = "0";
                ri.message = "更新失败,请重试或联系管理员!";
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }
            ri.flag = "1";
            ri.message = "自提托运信息更新成功!";
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            return;
        }
        #endregion

        #region 新增自提信息
        public void Insert_UserAddressZT()
        {
            ReInfo ri = new ReInfo();
            bool b = new BasicInfoManager().Insert_UserAddressZT(
                    HttpContext.Current.Session["lngopUserId"].ToString(),
                    "自提",
                    HttpContext.Current.Request.Form["strCarplateNumber"],
                    HttpContext.Current.Request.Form["strDriverName"],
                    HttpContext.Current.Request.Form["strDriverTel"],
                    HttpContext.Current.Request.Form["strIdCard"],
                    HttpContext.Current.Session["lngopUserId"].ToString()
                );
            if (!b)
            {
                ri.flag = "0";
                ri.message = "新增失败,请重试或联系管理员!";
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }
            ri.flag = "1";
            ri.message = "新增自提信息成功!";
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            return;
        }
        #endregion

        #region 新增配送信息
        public void Insert_UserAddressPS()
        {
            ReInfo ri = new ReInfo();
            string address = HttpContext.Current.Request.Form["strReceivingAddress"].ToString().Replace("，", "").Replace(",", "").Replace(" ", "");
            bool b = new BasicInfoManager().Insert_UserAddressPS(
                    HttpContext.Current.Session["lngopUserId"].ToString(),
                    "配送",
                    HttpContext.Current.Request.Form["strConsigneeName"],
                    HttpContext.Current.Request.Form["strConsigneeTel"],
                //  HttpContext.Current.Request.Form["strReceivingAddress"],
                     address,
                    HttpContext.Current.Request.Form["area"],
                    HttpContext.Current.Request.Form["ccodeid"]
                );

            if (!b)
            {
                ri.flag = "0";
                ri.message = "新增失败,请重试或联系管理员!";
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }
            ri.flag = "1";
            ri.message = "新增配送信息成功!";
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            return;
        }
        #endregion

        #region 新增自提托运信息
        public void Insert_UserAddressZTPS()
        {
            ReInfo ri = new ReInfo();
            string address = HttpContext.Current.Request.Form["strReceivingAddress"].ToString().Replace("，", "").Replace(",", "").Replace(" ", "");
            bool b = new BasicInfoManager().Insert_UserAddressZTPS(
                    HttpContext.Current.Session["lngopUserId"].ToString(),
                    "自提托运",
                    HttpContext.Current.Request.Form["strConsigneeName"],
                    HttpContext.Current.Request.Form["strConsigneeTel"],
                //  HttpContext.Current.Request.Form["strReceivingAddress"],
                     address,
                    HttpContext.Current.Request.Form["area"],
                    HttpContext.Current.Request.Form["ccodeid"]
                );

            if (!b)
            {
                ri.flag = "0";
                ri.message = "新增失败,请重试或联系管理员!";
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }
            ri.flag = "1";
            ri.message = "新增自提托运信息成功!";
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            return;
        }
        #endregion

        #region 获取用户行政区
        public void Get_User_Area()
        {
            string ccuscode = HttpContext.Current.Session["ConstcCusCode"].ToString();
            DataTable dt = pro.Get_User_Area(ccuscode);
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(dt));
        }
        #endregion

        #region 删除行政区
        public void DL_UserAddress_exByDel()
        {
            ReInfo ri = new ReInfo();
            string id = HttpContext.Current.Request.Form["area_id"];
            bool b = new BasicInfoManager().DL_UserAddress_exByDel(id);
            //if (!b)
            //{
            //    ri.flag = "0";
            //    ri.message = "删除失败,请重试或联系管理员!";
            //    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            //    return;
            //}
            ri.flag = "1";
            ri.message = "删除成功!";
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            return;

        }
        #endregion

        #region 新增行政区
        public void Insert_UserArea()
        {
            JObject jo = new JObject();
            string ccuscode = HttpContext.Current.Session["ConstcCusCode"].ToString();
            string area = HttpContext.Current.Request.Form["area"];
            string ccodeid = HttpContext.Current.Request.Form["ccodeid"];

            //新增前查询是否已存在行政区
            bool a = pro.IsExists_xzq(ccuscode, ccodeid);
            if (a)
            {
                jo["flag"] = 0;
                jo["message"] = "已存在该行政区，请不要重复添加！";
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                return;
            }

            bool b = pro.Insert_UserArea(ccuscode, area, ccodeid);
            if (!b)
            {
                jo["flag"] = 0;
                jo["message"] = "新增失败,请重试或联系管理员!";
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                return;
            }
            jo["flag"] = 1;
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 获取用户提货配送地址及行政区
        public void Get_UserAddress()
        {
            ReInfo ri = new ReInfo();
            ri.list_dt = new List<DataTable>() { };
            string da = HttpContext.Current.Request.Form["da"];
            DataTable Address_DT = new DataTable();
            if (da == "自提")
            {

                Address_DT = new BasicInfoManager().DLproc_UserAddressZTBySelGroup(HttpContext.Current.Session["lngopUserId"].ToString());
            }
            else
            {

                Address_DT = new BasicInfoManager().DLproc_UserAddressPSBySelGroup(HttpContext.Current.Session["lngopUserId"].ToString());
            }
            DataTable dt = new BasicInfoManager().DL_UserAddressZTXZQBySel(HttpContext.Current.Session["ConstcCusCode"].ToString());
            ri.list_dt.Add(Address_DT);
            ri.list_dt.Add(dt);
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));

        }
        #endregion

        #region 刷新库存
        public void Refresh()
        {
            string code = HttpContext.Current.Request.Form["codes"];
            string page = HttpContext.Current.Request.Form["page"];
            string kpdw = HttpContext.Current.Request.Form["kpdw"];
            string areaid = HttpContext.Current.Request.Form["areaid"];
            string cWhCode = HttpContext.Current.Request.Form["cWhCode"];
            ReInfo ri = new ReInfo();
            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(page))
            {
                ri.flag = "0";
                ri.message = "不能为空!!";
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }

            ri.dt = new DataTable();
            DataTable dt = new DataTable();
            ri.dt.Columns.AddRange(new DataColumn[]{
                new DataColumn ("code",typeof(string)),
                new DataColumn ("fAvailQtty",typeof(string)),
                new DataColumn("realqty",typeof(string)),
                new DataColumn("nOriginalPrice",typeof(string)),
                new DataColumn("ExercisePrice",typeof(string))
            });


            string[] codes = code.Split(',');
            string QueryString = code.Replace(',', '|');
            if (page == "buy")
            {
                ri.dt = pro.DLproc_QuasiOrderDetail_All_Warehouse_BySel(QueryString, kpdw, areaid, cWhCode);
                ri.flag = "1";
            }
            else if (page == "modify_buy")
            {
                string strBillNo = HttpContext.Current.Request.Form["strBillNo"];
                ri.dt = pro.DLproc_QuasiOrderDetailModify_All_Warehouse_BySel(strBillNo, QueryString, kpdw, cWhCode, areaid);
                ri.flag = "1";
            }
            else if (page == "buy_special")
            {
                for (int i = 0; i < codes.Length; i++)
                {
                    string[] c = codes[i].Split('Y');

                    dt = pro.DLproc_QuasiYOrderDetail_TSBySel(c[0], "Y" + c[1], areaid);
                    ri.dt.Rows.Add();
                    ri.dt.Rows[i]["code"] = dt.Rows[0]["itemid"];
                    ri.dt.Rows[i]["realqty"] = Convert.ToDouble(dt.Rows[0]["realqty"].ToString());
                    ri.dt.Rows[i]["fAvailQtty"] = Convert.ToDouble(dt.Rows[0]["fAvailQtty"].ToString());
                    ri.dt.Rows[i]["nOriginalPrice"] = Convert.ToDouble(dt.Rows[0]["iquotedprice"].ToString());
                    ri.dt.Rows[i]["ExercisePrice"] = Convert.ToDouble(dt.Rows[0]["itaxunitprice"].ToString());

                }
                ri.flag = "1";
            }
            else if (page == "modify_special")
            {
                string strBillNo = HttpContext.Current.Request.Form["strBillNo"];
                for (int i = 0; i < codes.Length; i++)
                {
                    string[] c = codes[i].Split('Y');

                    dt = pro.DLproc_QuasiYOrderDetailModify_TSBySel(c[0], "Y" + c[1], strBillNo, areaid);
                    ri.dt.Rows.Add();
                    ri.dt.Rows[i]["code"] = dt.Rows[0]["itemid"];
                    ri.dt.Rows[i]["realqty"] = Convert.ToDouble(dt.Rows[0]["realqty"].ToString());
                    ri.dt.Rows[i]["fAvailQtty"] = Convert.ToDouble(dt.Rows[0]["fAvailQtty"].ToString());
                    ri.dt.Rows[i]["nOriginalPrice"] = Convert.ToDouble(dt.Rows[0]["iquotedprice"].ToString());
                    ri.dt.Rows[i]["ExercisePrice"] = Convert.ToDouble(dt.Rows[0]["itaxunitprice"].ToString());

                }
                ri.flag = "1";
            }

            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));

        }
        #endregion

        #region 获取延期通知单列表
        public void Get_ArrearList()
        {
            ReInfo ri = new ReInfo();
            string start_date = HttpContext.Current.Request.Form["start_date"];
            string end_date = HttpContext.Current.Request.Form["end_date"];
            string bytstatus = HttpContext.Current.Request.Form["bytstatus"];
            string cCusCode = HttpContext.Current.Session["ConstcCusCode"].ToString();
            ri.dt = pro.Get_ArrearList(start_date, end_date, bytstatus, cCusCode + "%");
            ri.flag = "1";
            ri.message = HttpContext.Current.Session["strAllAcount"].ToString();
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
        }
        #endregion

        #region 确认延期通知单
        public void CusConfirmArrear()
        {
            ReInfo ri = new ReInfo();
            string code = HttpContext.Current.Request.Form["code"];
            string cCusCode = HttpContext.Current.Session["strAllAcount"].ToString();
            string dExtensionDateStart = HttpContext.Current.Request.Form["dExtensionDateStart"];
            string dExtensionDateEnd = HttpContext.Current.Request.Form["dExtensionDateEnd"];
            ri = pro.CusConfirmArrear(code, cCusCode, dExtensionDateStart, dExtensionDateEnd);
            if (ri.message == "32")
            {
                string msg = "您收到一张逾期三个月的延期通知单,请及时处理！";
                string nums = ConfigurationManager.AppSettings["ReciveArreaMsg"];
                string[] num_arr = nums.Split('|');
                nums = string.Join(",", num_arr);
                SMSSend9003.SendSMS2CustomerSoapClient SMSSend9003 = new SMSSend9003.SendSMS2CustomerSoapClient();
                SMSSend9003.SendSMS(nums, msg);
                foreach (var num in num_arr)
                {
                    SMSSend9003.SendQY_Message_Text(num, "", "", "20", msg);
                }
            }

            ri.message = "确认成功！";
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
        }
        #endregion

        #region 根据通知单号查询详情
        public void Get_ArrearDetail()
        {
            string code = HttpContext.Current.Request.Form["code"];
            ReInfo ri = new ReInfo();
            DataTable dt = new AdminManager().Get_ArrearDetail(code);
            if (dt.Rows.Count > 0)
            {
                ri.flag = "1";
                ri.dt = dt;

            }
            else
            {
                ri.flag = "0";
                ri.message = "数据异常！";
            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
        }
        #endregion

        #region 取回待审核订单
        public void RecaptionOrder()
        {
            string lngoporderid = HttpContext.Current.Request.Form["lngoporderid"];
            string strbillno = HttpContext.Current.Request.Form["strbillno"];
            string strAllAcount = HttpContext.Current.Session["strAllAcount"].ToString();
            ReInfo ri = new ReInfo();
            ri = pro.RecaptionOrder(lngoporderid, strbillno, strAllAcount);
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
        }
        #endregion

        #region 获取可更改车牌号的订单列表
        public void Get_ModifyShippingMethod_list()
        {
            ReInfo ri = new ReInfo();
            ri.list_dt = new List<DataTable>();
            ri.dt = pro.Get_ModifyShippingMethod_list(HttpContext.Current.Session["lngopUserId"].ToString(), HttpContext.Current.Session["lngopUserExId"].ToString());
            ri.list_dt.Add(new BasicInfoManager().DLproc_UserAddressZTBySelGroup(HttpContext.Current.Session["lngopUserId"].ToString()));
            ri.list_dt.Add(new BasicInfoManager().DLproc_UserAddressPSBySelGroup(HttpContext.Current.Session["lngopUserId"].ToString()));
            ri.list_dt.Add(new BasicInfoManager().DL_UserAddressZTXZQBySel(HttpContext.Current.Session["ConstcCusCode"].ToString()));
            ri.list_dt.Add(new BasicInfoManager().DL_cdefine3BySel());
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            return;
        }
        #endregion

        #region 更改订单送货信息
        public void ModifyShippingMethod()
        {
            ReInfo ri = new ReInfo();
            string lngoporderid = HttpContext.Current.Request.Form["lngoporderid"];
            string cCSCode = HttpContext.Current.Request.Form["cCSCode"];
            string addressId = HttpContext.Current.Request.Form["addressId"];
            string strRemark = HttpContext.Current.Request.Form["strRemark"];
            string carType = HttpContext.Current.Request.Form["carType"];
            ri = pro.ModifyShippingMethod(lngoporderid, cCSCode, addressId, strRemark, carType);
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            return;
        }
        #endregion

        #region 提交财务问卷调查表
        public void SubmitQuestion()
        {
            JObject jo = new JObject();
            string answers = HttpContext.Current.Request.Form["answers"];
            jo = JObject.Parse(answers);
            string ccuscode = string.Empty;
            if (HttpContext.Current.Session["ConstcCusCode"] == null)
            {
                ccuscode = "";
            }
            else
            {
                ccuscode = HttpContext.Current.Session["ConstcCusCode"].ToString();
            }
            string name = HttpContext.Current.Request.Form["name"];
            string phone = HttpContext.Current.Request.Form["phone"];
            bool b = pro.SubmitQuestion(HttpContext.Current.Request.Form["name"], HttpContext.Current.Request.Form["phone"], ccuscode, jo);
            jo = new JObject();
            if (b)
            {
                jo["flag"] = "1";
            }
            else
            {
                jo["flag"] = "0";
            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;


        }
        #endregion

        #region 提交网上订单问卷调查表
        public void SubmitOrderQuestion()
        {
            JObject jo = new JObject();
            string cMemo = HttpContext.Current.Request.Form["cMemo"];
            string satisfy = HttpContext.Current.Request.Form["satisfy"];
            string ccuscode = HttpContext.Current.Session["ConstcCusCode"].ToString();
            string strallacount = HttpContext.Current.Session["strAllAcount"].ToString();

            bool b = pro.SubmitOrderQuestion(satisfy, cMemo, ccuscode, strallacount);
            jo = new JObject();
            if (b)
            {
                jo["flag"] = "1";
            }
            else
            {
                jo["flag"] = "0";
            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 提交网上订单问卷调查表--20180110
        public void SubmitOrderQuestion20180110()
        {
            JObject jo = new JObject();
            string answers = HttpContext.Current.Request.Form["answers"];
            jo = JObject.Parse(answers);
            jo["strAllAcount"] = HttpContext.Current.Session["strAllAcount"].ToString(); ;
            jo["phone"] = HttpContext.Current.Session["strLoginPhone"].ToString();

            bool b = pro.SubmitOrderQuestion20180110(jo);
            jo = new JObject();
            if (b)
            {
                jo["flag"] = "1";
            }
            else
            {
                jo["flag"] = "0";
            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 提交网上订单问卷调查表--20180808
        public void SubmitOrderQuestion20180808()
        {
            JObject jo = new JObject();
            string answers = HttpContext.Current.Request.Form["answers"];
            jo = JObject.Parse(answers);
            jo["strAllAcount"] = HttpContext.Current.Session["strAllAcount"].ToString(); ;
            jo["phone"] = HttpContext.Current.Session["strLoginPhone"].ToString();

            bool b = pro.SubmitOrderQuestion20180808(jo);
            jo = new JObject();
            if (b)
            {
                jo["flag"] = "1";
            }
            else
            {
                jo["flag"] = "0";
            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 提交网上订单问卷调查表--20180808
        public void SubmitOrderQuestion20181225()
        {
            JObject jo = new JObject();
            string answers = HttpContext.Current.Request.Form["answers"];
            jo = JObject.Parse(answers);
            jo["strAllAcount"] = HttpContext.Current.Session["strAllAcount"].ToString(); ;
            jo["phone"] = HttpContext.Current.Session["strLoginPhone"].ToString();

            bool b = pro.SubmitOrderQuestion20181225(jo);
            jo = new JObject();
            if (b)
            {
                jo["flag"] = "1";
            }
            else
            {
                jo["flag"] = "0";
            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 判断该用户是否参与过网单调查
        public void isSubmitQuestion()
        {
            string strAllAcount = HttpContext.Current.Session["strAllAcount"].ToString();
            bool b = pro.isSubmitQuestion(strAllAcount);
            JObject jo = new JObject();
            if (b)
            {
                jo["flag"] = "1";
            }
            else
            {
                jo["flag"] = "0";
            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 客户登录时，记录登录信息
        public void LoginInfo()
        {
            JObject j = new JObject();
            string a = HttpContext.Current.Request.Form["info"];
            j = JObject.Parse(a);
            j["ccuscode"] = HttpContext.Current.Session["ConstcCusCode"].ToString();
            j["strallacount"] = HttpContext.Current.Session["strAllAcount"].ToString();
            j["loginPhone"] = HttpContext.Current.Session["strLoginPhone"].ToString();
            bool b = pro.LoginInfo(j);
            JObject job = new JObject();
            if (b)
            {
                job["flag"] = "1";
            }
            else
            {
                job["flag"] = "0";
            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(job));
        }
        #endregion

        #region 获取预约界面初始化的时间段和可预约订单列表
        public void Get_MAA()
        {
            JObject jo = new JObject();
            JToken j = new JObject();
            string ccuscode = HttpContext.Current.Session["ConstcCusCode"].ToString();
            string iType = HttpContext.Current.Request.Form["iType"];
            string lngopuserid = HttpContext.Current.Session["lngopUserId"].ToString();
            //jo["timeTable"] = JToken.Parse(JsonConvert.SerializeObject(MAATimeTable_Handle(iType)));
            //jo["ordersTable"] = JToken.Parse(JsonConvert.SerializeObject(pro.Get_MAAOrders(ccuscode,"00")));
            //jo["carTypes"] = JToken.Parse(JsonConvert.SerializeObject(new BasicInfoManager().DL_cdefine3BySel()));
            DataSet ds = pro.Get_MAA(ccuscode, iType, lngopuserid);
            jo["flag"] = 1;
            DataTable dt = ds.Tables["timeTable"];
            ds.Tables.Remove("timeTable");
            ds.Tables.Add(MAATimeTable_Handle(dt, iType));
            jo["DataSet"] = JToken.Parse(JsonConvert.SerializeObject(ds));
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 获取预约界面时段列表
        public void Get_MAATimes()
        {
            DataTable dt = new DataTable();
            //  dt = pro.Get_MAATimes();
            string iType = HttpContext.Current.Request.Form["iType"];
            dt = MAATimeTable_Handle(pro.Get_MAATimes(iType), iType);

            JObject jo = new JObject();
            JToken j = new JObject();
            j = JToken.Parse(JsonConvert.SerializeObject(dt));
            jo["timeTable"] = j;
            jo["flag"] = 1;
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 获取预约界面可预约订单列表
        public void Get_MAAOrders()
        {
            DataTable dt = new DataTable();
            string ccuscode = HttpContext.Current.Session["ConstcCusCode"].ToString();
            dt = pro.Get_MAAOrders(ccuscode, "00");
            JObject orders = new JObject();
            JObject jo = new JObject();
            JToken j = new JObject();
            j = JToken.Parse(JsonConvert.SerializeObject(dt));
            jo["ordersTable"] = j;
            jo["flag"] = 1;
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 对获取的预约时段Table进行处理，1为普通预约，2为特殊预约
        public DataTable MAATimeTable_Handle(DataTable dt, string iType)
        {

            if (iType == "1")
            {
                foreach (DataRow dr in dt.Rows)
                {
                    for (int i = 1; i < 9; i++)
                    {
                        if (int.Parse(dr["iQty" + i].ToString()) > 0 && int.Parse(dr["BeTime" + i].ToString()) == 0)
                        {
                            dr["iQty" + i] = 0;
                        }

                    }
                }
            }
            else if (iType == "2")
            {
                foreach (DataRow dr in dt.Rows)
                {
                    for (int i = 1; i < 9; i++)
                    {
                        if (int.Parse(dr["iQty" + i].ToString()) > -1)
                        {
                            if (int.Parse(dr["iQty" + i].ToString()) == 0 && int.Parse(dr["BeTime" + i].ToString()) == 1)
                            {
                                dr["iQty" + i] = 1;
                            }
                            else
                            {
                                dr["iQty" + i] = 0;
                            }

                        }

                    }
                }
            }


            return dt;
        }
        #endregion

        #region 提交预约信息
        public void DLproc_NewMAAOrderByIns()
        {
            string info = HttpContext.Current.Request.Form["info"];
            string cCusName = HttpContext.Current.Session["strUserName"].ToString();
            string cCusCode = HttpContext.Current.Session["ConstcCusCode"].ToString();
            string strAllAcount = HttpContext.Current.Session["strAllAcount"].ToString();
            string iType = HttpContext.Current.Request.Form["iType"];

            JObject jo = new JObject();
            jo = JObject.Parse(info);
            jo["cCusCode"] = cCusCode;
            jo["cCusName"] = cCusName;
            jo["strAllAcount"] = strAllAcount;
            jo["iType"] = iType;
            jo["ShippingMethod"] = "00";
            jo["cSCCode"] = "00";
            if (iType == "1")
            {
                jo["bytStatus"] = 4;
            }
            else if (iType == "2")
            {
                jo["bytStatus"] = 1;
            }
            //检测数据
            List<string> list = new List<string>();
            foreach (string item in jo["orders"])
            {
                list.Add(item);
            }
            jo["o"] = string.Join(",", list);
            DataTable dt = pro.DLproc_MAACheckDataBySel(jo);

            //检测不通过时返回错误信息
            JObject j = new JObject();
            if (dt.Rows[0]["flag"].ToString() == "0")
            {
                j["flag"] = "0";
                j["message"] = dt.Rows[0]["ErrMsg"].ToString();
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(j));
                return;
            }


            //拼接MAAOderdetail表
            ArrayList arr = new ArrayList();
            foreach (int item in jo["orders"])
            {
                arr.Add(item);
            }
            DataTable MAAOrderDetail = return_MAAOrderDetail(arr);

            //插入数据
            JObject obj = pro.DLproc_NewMAAOrderByIns("DLproc_NewMAAOrderByIns", jo, MAAOrderDetail);

            if (obj["flag"].ToString() == "1")
            {
                j["flag"] = "1";
                j["cMAACode"] = obj["cMAACode"];

            }
            else
            {
                j["flag"] = "0";
                j["message"] = "预约失败，请重试或联系管理员！";
            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(j));
            return;
        }
        #endregion

        #region 拼接MAAOder表
        public DataTable return_MAAOrder(JObject jo)
        {
            DataTable MAAOrder = new DataTable();
            MAAOrder.Columns.Add("MAAOrderID", typeof(int));
            MAAOrder.Columns.Add("cCusCode", typeof(string));
            MAAOrder.Columns.Add("cCusName", typeof(string));
            MAAOrder.Columns.Add("strAllAcount", typeof(string));
            MAAOrder.Columns.Add("iType", typeof(int));
            MAAOrder.Columns.Add("datDate", typeof(DateTime));
            MAAOrder.Columns.Add("cName", typeof(string));
            MAAOrder.Columns.Add("cCode", typeof(string));
            MAAOrder.Columns.Add("datStartTime", typeof(string));
            MAAOrder.Columns.Add("datEndTime", typeof(string));
            MAAOrder.Columns.Add("cMAACode", typeof(string));
            MAAOrder.Columns.Add("iQty", typeof(string));
            MAAOrder.Columns.Add("cDriver", typeof(string));
            MAAOrder.Columns.Add("cCarNumber", typeof(string));
            MAAOrder.Columns.Add("cIdentity", typeof(string));
            MAAOrder.Columns.Add("cDriverPhone", typeof(string));
            MAAOrder.Columns.Add("cMemo", typeof(string));
            MAAOrder.Columns.Add("cCarType", typeof(string));


            MAAOrder.Rows.Add(jo["MAAOrderID"], jo["cCusCode"], jo["cCusName"], jo["strAllAcount"], jo["iType"], jo["datDate"], jo["cName"], jo["cCode"], jo["datStartTime"], jo["datEndTime"], jo["cMAACode"],
            jo["iQty"], jo["cDriver"], jo["cCarNumber"], jo["cIdentity"], jo["cDriverPhone"], jo["cMemo"], jo["cCarType"]);
            return MAAOrder;
        }
        #endregion

        #region 拼接MAAOderDetail表
        public DataTable return_MAAOrderDetail(ArrayList list)
        {
            DataTable MAAOrderDetail = new DataTable();
            MAAOrderDetail.Columns.Add("MAAOrderDetailID", typeof(int));
            MAAOrderDetail.Columns.Add("MAAOrderID", typeof(int));
            MAAOrderDetail.Columns.Add("lngopOrderId", typeof(int));
            MAAOrderDetail.Columns.Add("SubbytStatus", typeof(int));
            MAAOrderDetail.Columns.Add("datAddTime", typeof(DateTime));
            MAAOrderDetail.Columns.Add("datModifyTime", typeof(DateTime));

            for (int i = 0; i < list.Count; i++)
            {
                MAAOrderDetail.Rows.Add(1, 1, list[i], 4, DateTime.Now, DateTime.Now);
            }
            MAAOrderDetail.TableName = "dl_opMAAOrderDetail";
            return MAAOrderDetail;
        }
        #endregion

        #region 获取预约号列表
        public void Get_MAAList()
        {
            string cCusCode = HttpContext.Current.Session["ConstcCusCode"].ToString();
            string iType = HttpContext.Current.Request.Form["iType"];
            JObject jo = new JObject();
            jo["flag"] = "1";
            jo["MAAList"] = JToken.Parse(JsonConvert.SerializeObject(pro.Get_MAAList(cCusCode, iType)));
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 追加预约订单
        public void AddToMAAOrder()
        {
            string orders = HttpContext.Current.Request.Form["arr_orderId"];
            string MAAOrderId = HttpContext.Current.Request.Form["MAAOrderId"];
            ArrayList list = new ArrayList();
            Array arr = orders.Split(',');
            foreach (var item in arr)
            {
                list.Add(item);
            }
            DataTable dt = return_MAAOrderDetail(list);
            foreach (DataRow dr in dt.Rows)
            {
                dr["MAAOrderID"] = MAAOrderId;
            }
            bool b = pro.AddToMAAOrder(dt);
            JObject jo = new JObject();
            if (b)
            {
                jo["flag"] = "1";
                jo["message"] = "追加预约订单成功！";
            }
            else
            {
                jo["flag"] = "0";
                jo["message"] = "追加预约订单失败，请重试或联系管理员！";
            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));

        }
        #endregion

        #region 手机端获取一条预约信息
        public void MAA_Info()
        {
            JObject jo = new JObject();
            string id = HttpContext.Current.Request.Form["MAAOrderId"];
            jo["flag"] = "1";
            jo["order"] = JToken.Parse(JsonConvert.SerializeObject(pro.MAA_Info(id)));
            if (HttpContext.Current.Session["strUserLevel"] != null)
            {
                jo["Level"] = HttpContext.Current.Session["strUserLevel"].ToString();
            }

            HttpContext.Current.Response.Write(jo);
        }
        #endregion

        #region 客户提交反馈信息
        public void SaveFeedBack()
        {
            string question = HttpContext.Current.Request.Form["question"];
            string phone = HttpContext.Current.Session["strLoginPhone"].ToString();
            string ccuscode = HttpContext.Current.Session["cCusCode"].ToString();
            string strallacount = HttpContext.Current.Session["strAllAcount"].ToString();
            bool b = pro.SaveFeedBack(question, phone, ccuscode, strallacount);
            JObject jo = new JObject();
            if (b)
            {
                jo["flag"] = 1;
                new SMSSend9003.SendSMS2CustomerSoapClient().SendQY_Message_Text("13438904933", "", "", "22", "你收到一条新反馈");

            }
            else
            {
                jo["flag"] = 0;
                jo["message"] = "提交错误，请重试或联系管理员！";

            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;

        }
        #endregion

        #region 获取该用户所有的反馈

        public void GetAllFeedBack()
        {
            string ccuscode = HttpContext.Current.Session["cCusCode"].ToString();
            DataTable dt = pro.GetAllFeedBack(ccuscode);
            JObject jo = new JObject();
            jo["flag"] = 1;
            jo["data"] = JToken.Parse(JsonConvert.SerializeObject(dt));
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 获取手机端订单信息,1为普通订单，2为特殊订单
        public void OrderInfo()
        {
            string code = HttpContext.Current.Request.Form["orderCode"];
            JObject jo = JObject.Parse(check.DecryptDES(code));
            string orderType = jo["orderType"].ToString();
            string orderId = jo["orderId"].ToString();
            jo["order"] = JToken.Parse(JsonConvert.SerializeObject(pro.OrderInfo(orderId, orderType)));
            jo["flag"] = "1";
            jo["orderType"] = orderType;
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 保存客户编码，Type:1为清空以前的编码后添加，2为直接添加
        public void SaveCodeConfig()
        {
            string strAllAcount = HttpContext.Current.Session["strAllAcount"].ToString();
            string type = HttpContext.Current.Request.Form["type"];
            string data = HttpContext.Current.Request.Form["data"];
            JArray ja = JArray.Parse(data);
            JObject jo = new JObject();
            jo = pro.SaveCodeConfig(ja, type, strAllAcount);
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 获取用户自定义编码
        public void GetCodeConfig()
        {
            string strAllAcount = HttpContext.Current.Session["strAllAcount"].ToString();
            JObject jo = new JObject();

            jo["flag"] = 1;
            jo["table"] = JToken.Parse(JsonConvert.SerializeObject(pro.GetCodeConfig(strAllAcount)));
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 获取用户自定义编码时选择商品分类
        public void GetCodeConfigProductClass()
        {
            string ccuscode = HttpContext.Current.Session["ConstcCusCode"].ToString();
            JObject jo = new JObject();
            jo["flag"] = 1;
            jo["ProductClass"] = JToken.Parse(JsonConvert.SerializeObject(pro.GetCodeConfigProductClass(ccuscode)));
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;

        }
        #endregion

        #region 用户自定义编码时，获取详细产品列表
        public void GetCodeConfigProducts()
        {
            string ccuscode = HttpContext.Current.Session["ConstcCusCode"].ToString();
            string strAllAcount = HttpContext.Current.Session["strAllAcount"].ToString();
            string code = HttpContext.Current.Request.Form["code"];
            JObject jo = new JObject();
            jo["flag"] = 1;
            jo["Products"] = JToken.Parse(JsonConvert.SerializeObject(pro.GetCodeConfigProducts(ccuscode, strAllAcount, code)));
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;

        }
        #endregion

        #region 通过用户自定义编码添加产品
        public void GetcCusInvCode()
        {
            string code = HttpContext.Current.Request.Form["code"];
            string strAllAcount = HttpContext.Current.Session["strAllAcount"].ToString();

            JObject jo = new JObject();
            jo = pro.GetcInvCodeByCusInvCode(code, strAllAcount);
            if (jo["flag"].ToString() == "0")
            {
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                return;
            }
            string cInvCode = jo["message"].ToString();

            string areaid = HttpContext.Current.Request.Form["areaid"];
            string kpdw = HttpContext.Current.Request.Form["kpdw"];

            //检测允限销
            JObject j = new JObject();
            j = check.Check_limit(cInvCode, kpdw, 1);
            if (j["flag"].ToString() != "1")
            {
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(j));
                return;
            }

            DataTable dt = pro.DLproc_QuasiOrderDetailBySel(cInvCode, kpdw, areaid);
            string cWhCode = HttpContext.Current.Request.Form["cWhCode"];
            if (string.IsNullOrEmpty(cWhCode))
            {
                cWhCode = "CD01";
            }
            dt = pro.DLproc_QuasiOrderDetail_All_Warehouse_BySel(cInvCode, kpdw, areaid, cWhCode);
            jo["flag"] = 1;
            jo["table"] = JToken.Parse(JsonConvert.SerializeObject(dt));
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;

            DataTable new_dt = new DataTable();
            new_dt = dt.Clone();
            new_dt.Columns.Add("cinvdefine13", typeof(float)); //大包装换算
            new_dt.Columns.Add("cinvdefine14", typeof(float));//小包装换算
            new_dt.Columns.Add("cInvDefine1", typeof(string));//大包装单位
            new_dt.Columns.Add("cInvDefine2", typeof(string));//小包装单位
            new_dt.Rows.Add(dt.Rows[0].ItemArray);
            if (dt.Rows.Count >= 3)
            {

                new_dt.Rows[0]["cinvdefine13"] = Convert.ToDouble(dt.Rows[2]["iChangRate"].ToString()).ToString();
                new_dt.Rows[0]["cinvdefine14"] = Convert.ToDouble(dt.Rows[1]["iChangRate"].ToString()).ToString();
                new_dt.Rows[0]["cInvDefine2"] = dt.Rows[1]["cComUnitName"].ToString();
                new_dt.Rows[0]["cInvDefine1"] = dt.Rows[2]["cComUnitName"].ToString();
            }
            else if (dt.Rows.Count == 2)
            {
                new_dt.Rows[0]["cinvdefine13"] = Convert.ToDouble(dt.Rows[1]["iChangRate"].ToString()).ToString();
                new_dt.Rows[0]["cinvdefine14"] = Convert.ToDouble(dt.Rows[1]["iChangRate"].ToString()).ToString();
                new_dt.Rows[0]["cInvDefine2"] = dt.Rows[1]["cComUnitName"].ToString();
                new_dt.Rows[0]["cInvDefine1"] = dt.Rows[1]["cComUnitName"].ToString();
            }
            else if (dt.Rows.Count == 1)
            {
                new_dt.Rows[0]["cinvdefine13"] = Convert.ToDouble(dt.Rows[0]["iChangRate"].ToString()).ToString();
                new_dt.Rows[0]["cinvdefine14"] = Convert.ToDouble(dt.Rows[0]["iChangRate"].ToString()).ToString();
                new_dt.Rows[0]["cInvDefine2"] = dt.Rows[0]["cComUnitName"].ToString();
                new_dt.Rows[0]["cInvDefine1"] = dt.Rows[0]["cComUnitName"].ToString();
            }



            jo["flag"] = 1;
            jo["table"] = JToken.Parse(JsonConvert.SerializeObject(new_dt));
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 通过上传Excel自定义编码，批量导入数据
        public void GetcCusInvCodes()
        {
            string codes = HttpContext.Current.Request.Form["codes"];
            JObject jo = new JObject();
            jo["message"] = "";
            JArray ja = JArray.Parse(codes);


            //判断导入的Excel中是否有重复的自定义编码和为空白的自定义编码
            HashSet<String> hash = new HashSet<String>();
            foreach (var item in ja)
            {
                if (item["code"] == null)
                {
                    jo["flag"] = "0";
                    jo["message"] = "导入的Excel中有自定义编码为空的产品，请修改后重新上传！";
                    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                    return;
                }
                string key = item["code"].ToString();
                hash.Add(key);
            }
            if (ja.Count > hash.Count)
            {
                jo["flag"] = "0";
                jo["message"] = "导入的Excel表中有重复项，请删除后重新导入！";
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                return;
            }



            //判断自定义编码是否正确，不正确的编码存入ErrCodes，正确的编码存入TempCodes，增加cInvCode对象
            JArray JaErrMsg = new JArray();
            JArray ErrCodes = new JArray();
            JArray TempCodes = new JArray();
            JObject j = new JObject();
            string strAllAcount = HttpContext.Current.Session["strAllAcount"].ToString();

            foreach (var item in ja)
            {
                j = pro.GetcInvCodeByCusInvCode(item["code"].ToString(), strAllAcount);
                if (j["flag"].ToString() == "0")
                {
                    ErrCodes.Add(item["code"]);
                }
                else
                {
                    item["cInvCode"] = j["message"];
                    TempCodes.Add(item);
                }
            }


            jo["Errcodes"] = ErrCodes;
            if (TempCodes.Count == 0)
            {
                jo["flag"] = 0;
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                return;
            }

            //检测允限销，限销产品编码写入LimitCodes,其余产品拼接为table
            ja = TempCodes;
            string areaid = HttpContext.Current.Request.Form["areaid"];
            string kpdw = HttpContext.Current.Request.Form["kpdw"];
            string cWhCode = HttpContext.Current.Request.Form["cWhCode"];
            if (string.IsNullOrEmpty(cWhCode))
            {
                cWhCode = "CD01";
            }
            DataTable dt = new DataTable();
            DataTable new_dt = new DataTable();
            JArray LimitCodes = new JArray();
            JArray query = new JArray();
            bool b = true;
            TempCodes = new JArray();
            foreach (var item in ja)
            {
                j = check.Check_limit(item["cInvCode"].ToString(), kpdw, 1);
                if (j["flag"].ToString() != "1")
                {
                    LimitCodes.Add(item["code"]);
                }
                else
                {
                    TempCodes.Add(item);
                    query.Add(item["cInvCode"].ToString());
                }
            }

            string QueryString = string.Join("|", query);
            new_dt = pro.DLproc_QuasiOrderDetail_All_Warehouse_BySel(QueryString, kpdw, areaid, cWhCode);
            //foreach (var item in ja)
            //{
            //    j = check.Check_limit(item["cInvCode"].ToString(), kpdw, 1);
            //    if (j["flag"].ToString() != "1")
            //    {
            //        LimitCodes.Add(item["code"]);
            //    }
            //    else
            //    {
            //        TempCodes.Add(item);
            //        dt = pro.DLproc_QuasiOrderDetailBySel(item["cInvCode"].ToString(), kpdw, areaid);
            //        if (b)
            //        {
            //            new_dt = dt.Clone();
            //            new_dt.Columns.Add("cinvdefine13", typeof(float)); //大包装换算
            //            new_dt.Columns.Add("cinvdefine14", typeof(float));//小包装换算
            //            new_dt.Columns.Add("cInvDefine1", typeof(string));//大包装单位
            //            new_dt.Columns.Add("cInvDefine2", typeof(string));//小包装单位
            //            b = false;
            //        }
            //        new_dt.Rows.Add(dt.Rows[0].ItemArray);
            //        int len = new_dt.Rows.Count - 1;
            //        if (dt.Rows.Count >= 3)
            //        {

            //            new_dt.Rows[len]["cinvdefine13"] = Convert.ToDouble(dt.Rows[2]["iChangRate"].ToString()).ToString();
            //            new_dt.Rows[len]["cinvdefine14"] = Convert.ToDouble(dt.Rows[1]["iChangRate"].ToString()).ToString();
            //            new_dt.Rows[len]["cInvDefine2"] = dt.Rows[1]["cComUnitName"].ToString();
            //            new_dt.Rows[len]["cInvDefine1"] = dt.Rows[2]["cComUnitName"].ToString();
            //        }
            //        else if (dt.Rows.Count == 2)
            //        {
            //            new_dt.Rows[len]["cinvdefine13"] = Convert.ToDouble(dt.Rows[1]["iChangRate"].ToString()).ToString();
            //            new_dt.Rows[len]["cinvdefine14"] = Convert.ToDouble(dt.Rows[1]["iChangRate"].ToString()).ToString();
            //            new_dt.Rows[len]["cInvDefine2"] = dt.Rows[1]["cComUnitName"].ToString();
            //            new_dt.Rows[len]["cInvDefine1"] = dt.Rows[1]["cComUnitName"].ToString();
            //        }
            //        else if (dt.Rows.Count == 1)
            //        {
            //            new_dt.Rows[len]["cinvdefine13"] = Convert.ToDouble(dt.Rows[0]["iChangRate"].ToString()).ToString();
            //            new_dt.Rows[len]["cinvdefine14"] = Convert.ToDouble(dt.Rows[0]["iChangRate"].ToString()).ToString();
            //            new_dt.Rows[len]["cInvDefine2"] = dt.Rows[0]["cComUnitName"].ToString();
            //            new_dt.Rows[len]["cInvDefine1"] = dt.Rows[0]["cComUnitName"].ToString();
            //        }
            //    }
            //}

 
            jo["flag"] = 1;
            jo["ja"] = TempCodes;
            jo["LimitCodes"] = LimitCodes;
            jo["table"] = JToken.Parse(JsonConvert.SerializeObject(new_dt));
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;

        }
        #endregion

        #region   获取历史订单列表(userType:0表示该账号为主账号,可以查看该账号下全部订单,1表示只能查看该子账号下的订单)
        public void GetAllOrders()
        {
            JObject jo = new JObject();
            string start_time = HttpContext.Current.Request.Form["start_date"];
            string end_time = HttpContext.Current.Request.Form["end_date"];
            string type = HttpContext.Current.Request.Form["type"];
            if (string.IsNullOrEmpty(start_time) && string.IsNullOrEmpty(end_time))
            {

                start_time = DateTime.Now.AddDays(-90).ToString("yyyy-MM-dd");
                end_time = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else if (!string.IsNullOrEmpty(start_time) && string.IsNullOrEmpty(end_time))
            {

                end_time = Convert.ToDateTime(start_time).AddDays(90).ToString(("yyyy-MM-dd"));
            }
            else if (string.IsNullOrEmpty(start_time) && !string.IsNullOrEmpty(end_time))
            {

                start_time = Convert.ToDateTime(end_time).AddDays(-90).ToString();
            }
            if (Convert.ToDateTime(start_time) > Convert.ToDateTime(end_time))
            {
                jo["flag"] = 0;
                jo["message"] = "结束时间不能大于开始时间！";

                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                return;
            }
            double diffday = double.Parse(check.DateDiff(start_time, end_time)["day"].ToString());
            if (diffday > 90)
            {
                jo["flag"] = 0;
                jo["message"] = "开始时间与结束时间间隔不能大于3个月！";
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                return;
            }

            string bytstatus = HttpContext.Current.Request.Form["bytStatus"];
            string kpdw = HttpContext.Current.Request.Form["kpdw"];
            string userType = "1";
            DataTable dt = new DataTable();
            string all = ConfigurationManager.AppSettings["isSearchAll"];
            Array arr = all.Split('|');
            if (Array.IndexOf(arr, HttpContext.Current.Session["ConstcCusCode"].ToString()) != -1 || HttpContext.Current.Session["strAllAcount"].ToString().Length == 6)
            {
                userType = "0";
                dt = pro.GetAllOrders(HttpContext.Current.Session["lngopUserId"].ToString(), start_time, end_time, userType, bytstatus, kpdw);
            }
            else
            {
                dt = pro.GetAllOrders(HttpContext.Current.Session["strAllAcount"].ToString(), start_time, end_time, userType, bytstatus, kpdw);
            }
            //if (HttpContext.Current.Session["ConstcCusCode"].ToString() == HttpContext.Current.Session["strAllAcount"].ToString())
            //{
            //    userType = "0";
            //    dt = pro.GetAllOrders(HttpContext.Current.Session["lngopUserId"].ToString(), start_time, end_time, userType, bytstatus, kpdw);
            //}
            //else
            //{
            //    dt = pro.GetAllOrders(HttpContext.Current.Session["strAllAcount"].ToString(), start_time, end_time, userType, bytstatus, kpdw);
            //}

            jo["dt"] = JToken.Parse(JsonConvert.SerializeObject(dt));
            jo["flag"] = 1;
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;




        }
        #endregion

        #region 根据网上订单号获取订单详情
        public void GetOrderDetail()
        {
            string strbillno = HttpContext.Current.Request.Form["strBillNo"];
            DataTable dt = pro.GetOrderDetail(strbillno);
            JObject jo = new JObject();

            if (dt.Rows[0]["lngopUserId"].ToString() != HttpContext.Current.Session["lngopUserId"].ToString())
            {
                jo["flag"] = 0;
                jo["message"] = "你没有权限查看该订单！";

            }
            else
            {
                jo["flag"] = 1;
                jo["table"] = JToken.Parse(JsonConvert.SerializeObject(dt));
            }

            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;

        }

        #endregion

        #region 客服帮助客户下单后，客户确认订单
        public void ConfirmOrder()
        {
            string code = HttpContext.Current.Request.Form["orderCode"];
            JObject jo = JObject.Parse(check.DecryptDES(code));
            string orderType = jo["orderType"].ToString();
            string orderId = jo["orderId"].ToString();
            string strAllAcount = HttpContext.Current.Session["strAllAcount"].ToString();
            JObject j = pro.ConfirmOrder(orderId, orderType, strAllAcount);
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(j));
            return;

        }
        #endregion

        #region 多联工牌查询
        public void GetInfoByEmployeeCode()
        {
            string codes = HttpContext.Current.Request.Form["codes"];
            JObject jo = new JObject();
            jo["flag"] = 1;
            JArray ja = JArray.Parse(codes);
            if (ja.Count == 0)
            {
                jo["flag"] = 0;
                jo["message"] = "编码不正确!";
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                return;
            }
            DataTable dt = new BLL.AdminManager().GetInfoByEmployeeCode(ja);
            jo["tb"] = JToken.Parse(JsonConvert.SerializeObject(dt));
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;

        }
        #endregion

        #region 根据客户编码获取客户仓库
        public void GetWareHouseBycCusCode()
        {
            string cCusCode = HttpContext.Current.Request.Form["cCusCode"];
            DataTable dt = pro.GetWareHouseBycCusCode(cCusCode);
            JObject jo = new JObject();
            jo["flag"] = 1;
            jo["data"] = JToken.Parse(JsonConvert.SerializeObject(dt));
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region  20180423 ,修改普通订单和样品订单界面获取订单详情
        public void GetModifyOrderDetail()
        {

            JObject jo = new JObject();
            string strBillNo = HttpContext.Current.Request.Form["strBillNo"];
            DataTable Modify_Dt = pro.GetModifyOrderDetail(strBillNo);
            string kpdw = Modify_Dt.Rows[0]["ccuscode"].ToString();
            List<string> limit_name = new List<string>();
            //删除限销产品
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Modify_Dt.Rows.Count; i++)
            {
                if (!pro.DLproc_cInvCodeIsBeLimitedBySel(Modify_Dt.Rows[i]["cinvcode"].ToString(), kpdw, 1))
                {
                    limit_name.Add(Modify_Dt.Rows[i]["cinvname"].ToString());
                    Modify_Dt.Rows[i].Delete();
                }
                else
                {
                    sb.Append(Modify_Dt.Rows[i]["cinvcode"].ToString() + "|");
                }
            }
            Modify_Dt.AcceptChanges();
            jo["limit_name"] = JToken.Parse(JsonConvert.SerializeObject(limit_name));
            if (Modify_Dt.Rows.Count == 0)
            {
                jo["flag"] = 0;
                jo["message"] = "该订单所有产品已限销";
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                return;
            }

            string areaId = Modify_Dt.Rows[0]["chdefine49"].ToString();
            string[] msg = new string[6]{
                Modify_Dt.Rows[0]["strRemarks"].ToString(),      //1、备注
                Modify_Dt.Rows[0]["cSCCode"].ToString(),        //2、送货方式
                Modify_Dt.Rows[0]["strLoadingWays"].ToString(), //3、装车方式
                Modify_Dt.Rows[0]["cdefine3"].ToString(),     //4、车型下拉
                Modify_Dt.Rows[0]["lngopUseraddressId"].ToString(),//5、送货方式
                Modify_Dt.Rows[0]["ccuscode"].ToString()        //6、 开票单位
                
          };
            jo["msg"] = JToken.Parse(JsonConvert.SerializeObject(msg));


            jo["CusCredit_dt"] = JToken.Parse(JsonConvert.SerializeObject(order.DLproc_getCusCreditInfoWithBillno(kpdw, strBillNo)));  //获取信用

            string cWhCode = Modify_Dt.Rows[0]["cWhCode"].ToString();
            string chdefine51 = Modify_Dt.Rows[0]["chdefine51"].ToString();
            if (!cWhCode.Contains(chdefine51))
            {
                chdefine51 = "CD01";
            }

            DataTable datatable = pro.Get_ModifyOrder_Dt(strBillNo, sb.ToString().TrimEnd('|'), kpdw, chdefine51, areaId, Modify_Dt);
            jo["dt"] = JToken.Parse(JsonConvert.SerializeObject(Modify_Dt));
            jo["datatable"] = JToken.Parse(JsonConvert.SerializeObject(datatable));
            jo["WareHouse_dt"] = JToken.Parse(JsonConvert.SerializeObject(pro.GetWareHouseBycCusCode(kpdw)));
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 20180418，批量查询普通订单存货价格及库存
        public void DLproc_QuasiOrderDetail_All_Warehouse_BySel()
        {
            ReInfo ri = new ReInfo();
            string code = HttpContext.Current.Request.Form["codes"];
            DataTable dt = new DataTable();
            DataTable new_dt = new DataTable();
            string kpdw = HttpContext.Current.Request.Form["kpdw"];
            string strBillNo = HttpContext.Current.Request.Form["strBillNo"];
            string areaid = HttpContext.Current.Request.Form["areaid"];
            string cWhCode = HttpContext.Current.Request.Form["cWhCode"];
            if (string.IsNullOrEmpty(kpdw) || kpdw == "0")
            {
                ri.flag = "0";
                ri.message = "请先选择开票单位!";
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                return;
            }
            string isModify = HttpContext.Current.Request.Form["isModify"];
            string isSpecial = HttpContext.Current.Request.Form["isSpecial"];

            if (!string.IsNullOrEmpty(code)) //判断数组是否为空
            {
                string[] codes = code.Split(',');
                string QueryString = code.Replace(',', '|');
                if (isModify == "0" && isSpecial == "0")
                {
                    dt = pro.DLproc_QuasiOrderDetail_All_Warehouse_BySel(QueryString, kpdw, areaid, cWhCode);
                    ri.flag = "1";
                    ri.dt = dt;
                    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                    return;
                }
                else if (isModify == "1" && isSpecial == "0")
                {
                    dt = pro.DLproc_QuasiOrderDetailModify_All_Warehouse_BySel(strBillNo, QueryString, kpdw, cWhCode, areaid);
                    ri.flag = "1";
                    ri.dt = dt;
                    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
                    return;
                }
            }

            ri.flag = "1";
            ri.dt = new_dt;
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(ri));
            return;
        }
        #endregion

        #region 20180424, 获取临时订单和历史订单详情
        public void DLproc_BackOrderandPrvOrdercInvCodeIsBeLimited_All_Warehouse_BySel()
        {
            string id = HttpContext.Current.Request.Form["id"];
            string iBillType = HttpContext.Current.Request.Form["iBillType"];
            DataTable dt = pro.DLproc_BackOrderandPrvOrdercInvCodeIsBeLimited_All_Warehouse_BySel(id, "1", iBillType);
            string ConstcCusCode = HttpContext.Current.Session["ConstcCusCode"].ToString();
            string lngopUserID = HttpContext.Current.Session["lngopUserId"].ToString();
            JObject jo = new JObject();
            //  DataSet ds = pro.GetAllBaseInfo(ConstcCusCode, lngopUserID);
            //  jo["BaseInfo"] = JToken.Parse(JsonConvert.SerializeObject(ds));
            DataTable ReturnTable = new DataTable();

            string kpdw = string.Empty;
            string cWhCode = string.Empty;
            string areaId = string.Empty;
            string[] msg = new string[6]{
                dt.Rows[0]["strRemarks"].ToString(),      //1、备注
                dt.Rows[0]["cSCCode"].ToString(),        //2、送货方式
                dt.Rows[0]["strLoadingWays"].ToString(), //3、装车方式
                dt.Rows[0]["cdefine3"].ToString(),     //4、车型下拉
                dt.Rows[0]["lngopUseraddressId"].ToString(),//5、送货方式
                dt.Rows[0]["ccuscode"].ToString()        //6、 开票单位
                
          };
            jo["msg"] = JToken.Parse(JsonConvert.SerializeObject(msg));
            List<string> limit_name = new List<string>();
            //删除限销产品
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["bLimited"].ToString() != "1")
                {
                    limit_name.Add(dt.Rows[i]["cinvname"].ToString());
                    dt.Rows[i].Delete();
                }
            }
            dt.AcceptChanges();
            jo["limit_name"] = JToken.Parse(JsonConvert.SerializeObject(limit_name));
            kpdw = dt.Rows[0]["ccuscode"].ToString();
            jo["CusCredit_dt"] = JToken.Parse(JsonConvert.SerializeObject(order.DLproc_getCusCreditInfo(kpdw)));  //获取信用
            //处理临时订单
            if (iBillType == "1")
            {
                if (dt.Rows.Count > 0)
                {

                    cWhCode = dt.Rows[0]["chdefine51"].ToString() == "" ? "CD01" : dt.Rows[0]["chdefine51"].ToString();
                    areaId = "0";
                    ReturnTable = pro.Get_BackOrder_Dt(dt, kpdw, areaId, cWhCode);
                    jo["dt"] = JToken.Parse(JsonConvert.SerializeObject(ReturnTable));

                }
            }
            //处理历史订单
            else if (iBillType == "2")
            {
                cWhCode = dt.Rows[0]["chdefine51"].ToString() == "" ? "CD01" : dt.Rows[0]["chdefine51"].ToString();
                areaId = dt.Rows[0]["chdefine49"].ToString() == "" ? "0" : dt.Rows[0]["chdefine49"].ToString();
                ReturnTable = pro.Get_HistoryOrder_Dt(dt, kpdw, areaId, cWhCode);
                jo["datatable"] = JToken.Parse(JsonConvert.SerializeObject(ReturnTable));
                jo["dt"] = JToken.Parse(JsonConvert.SerializeObject(dt));
            }
            jo["WareHouse_dt"] = JToken.Parse(JsonConvert.SerializeObject(pro.GetWareHouseBycCusCode(kpdw)));
            jo["flag"] = 1;
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 订单重量查询
        public void GetOrderWeight()
        {
            string Number = HttpContext.Current.Request.Form["Number"];
            string NumberType = HttpContext.Current.Request.Form["NumberType"];
            DataTable dt = pro.GetOrderWeight(NumberType, Number);
            JObject jo = new JObject();

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                string strAllAcount = HttpContext.Current.Session["strAllAcount"].ToString();
                string ConstcCusCode = HttpContext.Current.Session["ConstcCusCode"].ToString();
                string code = dr["strAllAcount"].ToString();
                if (code.Substring(0, 6) != ConstcCusCode)
                {
                    jo["flag"] = 0;
                    jo["message"] = "你不能查看其他客户的订单";

                }
                else
                {
                    if (code == strAllAcount)
                    {
                        jo["flag"] = 1;
                        jo["table"] = JToken.Parse(JsonConvert.SerializeObject(dt));
                    }
                    else
                    {
                        if (strAllAcount.Length == 6)
                        {
                            jo["flag"] = 1;
                            jo["table"] = JToken.Parse(JsonConvert.SerializeObject(dt));
                        }
                        else
                        {
                            jo["flag"] = 0;
                            jo["message"] = "只有主账号和提交该订单的账号可查看";
                        }
                    }
                }
            }
            else
            {
                jo["flag"] = 0;
                jo["message"] = "未查询到该订单，请核实订单编号是否正确";
            }

            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;

        }
        #endregion

        #region 新增需求订单
        public void DLproc_NewOrder_X_ByIns()
        {
            JObject jo = new JObject();
            JObject j = new JObject();
            j = check.XOrderTimeControl();
            if (j["flag"].ToString() != "1")
            {
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(j));
                return;
            }
            j = check.XOrderBehaviorCheck(HttpContext.Current.Session["ConstcCusCode"].ToString());
            if (j["flag"].ToString() != "1")
            {
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(j));
                return;
            }
            //获取表头数据实例化为model
            form_Data formData = JsonConvert.DeserializeObject<form_Data>(HttpContext.Current.Request.Form["formData"]);
            string kpdw = formData.ccuscode;
            //获取表体数据实例化model
            List<Buy_list> listData = JsonConvert.DeserializeObject<List<Buy_list>>(HttpContext.Current.Request.Form["listData"]);

            if (listData.Count > 1)
            {
                jo["flag"] = 0;
                jo["message"] = "一张需求订单只能提交一种产品";
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                return;
            }

            //根据表体数据itemid重新生成Table，计算金额，用于验证信用、是否大于库存、是否大于可用量、是否有未填写数量的商品
            DataTable Check_Dt = pro.Get_Check_Dt(listData, kpdw, formData.areaid, formData.cWhCode); //用于验证的table

            #region 验证表单开始

            //验证商品允限销
            jo = check.Check_limit(listData[0].cinvcode, kpdw, 1);
            if (jo["flag"].ToString() != "1")
            {
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                return;
            }

            //检测信用额度
            DataTable dt = order.DLproc_getCusCreditInfo(kpdw);// 重新获取信用
            //double CusCredit = Convert.ToDouble(dt.Rows[0]["iCusCreLine"].ToString()); //客户信用
            //double listCredit =  Convert.ToDouble(Check_Dt.Rows[0]["iquantity"].ToString()) * Convert.ToDouble(Check_Dt.Rows[0]["ExercisePrice"].ToString());   

            //检测信用


            //检测订单量是否等于0
            if (Convert.ToDouble(Check_Dt.Rows[0]["iquantity"].ToString()) == 0)
            {
                jo = new JObject();
                jo["flag"] = 0;
                jo["message"] = "订单数量不能为零";
                HttpContext.Current.Response.Write(jo);
                return;
            }

            //检测订单量是否小于库存，若小于库存则不允许提交
            if (Convert.ToDouble(Check_Dt.Rows[0]["iquantity"].ToString()) <= Convert.ToDouble(Check_Dt.Rows[0]["fAvailQtty"].ToString()))
            {
                jo = new JObject();
                jo["flag"] = 0;
                jo["message"] = "你需要的货物有足够的库存<br>请到普通订单中直接下单";
                HttpContext.Current.Response.Write(jo);
                return;
            }

            //验证通过后，开始提交表单
            DataTable Address_Dt = new product().Get_AddressById(formData.lngopUseraddressId);
            OrderInfo oi = new OrderInfo(
                                            HttpContext.Current.Session["lngopUserId"].ToString(), //客户ID
                                            DateTime.Now.ToString(),                               //订单时间                     
                                            11,                                                     //订单状态         
                                            formData.strRemarks,                                   //备注     
                                            formData.ccuscode,                                     //开票单位ID 
                                            Address_Dt.Rows[0]["strDriverName"].ToString(),        //司机姓名 
                                            Address_Dt.Rows[0]["strIdCard"].ToString(),            //司机身份证
                                            formData.carType,                                      //车型
                                            Address_Dt.Rows[0]["strConsigneeName"].ToString(),     //收货人姓名
                                            Address_Dt.Rows[0]["strCarplateNumber"].ToString(),     //车牌号
                                            formData.txtAddress,                                    //地址信息
                                            Address_Dt.Rows[0]["strConsigneeTel"].ToString(),      //收货人电话
                                            Address_Dt.Rows[0]["strDriverTel"].ToString(),         //司机电话
                                            formData.ccusname,                                     //开票单位名称
                                            formData.ccuspperson,                                  //业务员编号 
                                            formData.csccode,                                      //送货方式
                                            formData.datDeliveryDate,                              //提货时间
                                            formData.strLoadingWays,                               //装车方式
                                            "05",                                                  //销售类型编码    
                                            formData.lngopUseraddressId,                           //地址ID
                                            "",                                                   //strTxtRelateU8NO
                                            dt.Rows[0]["cdiscountname"].ToString(),                //lngBillType 特殊订单
                                            formData.txtArea,                                      //行政区
                                            HttpContext.Current.Session["lngopUserExId"].ToString(),//用户ExID
                                            HttpContext.Current.Session["strAllAcount"].ToString() //子账号编码

                                       );
            DataTable lngopOrderIdDt = pro.DLproc_NewOrder_X_ByIns(oi, formData.areaid, formData.iaddresstype, formData.chdefine21, formData.cWhCode, "5");

            if (lngopOrderIdDt.Rows.Count == 0)
            {
                jo["flag"] = 0;
                jo["message"] = "订单提交出错，请重试或联系管理员！";
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
                return;
            }
            #endregion

            #region 提交表体数据
            int lngopOrderId = Convert.ToInt32(lngopOrderIdDt.Rows[0]["lngopOrderId"].ToString());
            Insert_listData(Check_Dt, lngopOrderId);

            jo["message"] = lngopOrderIdDt.Rows[0]["strBillNo"].ToString();
            #endregion
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;

        }
        #endregion

        #region 获取产品需求订单列表
        public void GetXOrderList()
        {
            string strAllAcount = HttpContext.Current.Session["strAllAcount"].ToString();
            string lngopUserID = HttpContext.Current.Session["lngopUserId"].ToString();
            string status = HttpContext.Current.Request.Form["status"];
            string all = ConfigurationManager.AppSettings["isSearchAll"];
            Array arr = all.Split('|');
            if (Array.IndexOf(arr, HttpContext.Current.Session["ConstcCusCode"].ToString()) != -1)
            {
                strAllAcount = HttpContext.Current.Session["ConstcCusCode"].ToString();
            }
            DataSet ds = pro.GetXOrderList(strAllAcount, lngopUserID, status);
            JObject jo = new JObject();
            jo["flag"] = 1;
            jo["data"] = JToken.Parse(JsonConvert.SerializeObject(ds, timejson));
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 作废需求订单
        public void CancelXOrder()
        {
            string strAllAcount = HttpContext.Current.Session["strAllAcount"].ToString();
            string lngopUserID = HttpContext.Current.Session["lngopUserId"].ToString();
            string strBillNo = HttpContext.Current.Request.Form["strBillNo"];
            JObject jo = new JObject();
            jo = pro.CancelXOrder(strBillNo, strAllAcount, lngopUserID);
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 更改产品需求订单车辆信息
        public void UpdateXOrderAddress()
        {
            string strAllAcount = HttpContext.Current.Session["strAllAcount"].ToString();
            string strBillNo = HttpContext.Current.Request.Form["strBillNo"];

            string addressId = HttpContext.Current.Request.Form["addressId"];
            string carTypeValue = HttpContext.Current.Request.Form["carTypeValue"];
            string strLoadingWays = HttpContext.Current.Request.Form["strLoadingWays"];
            string strRemarks = HttpContext.Current.Request.Form["strRemarks"];
            JObject jo = pro.UpdateXOrderAddress(strAllAcount, strBillNo, addressId, carTypeValue, strLoadingWays, strRemarks);
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 获取客户信用
        public void GetUserBehavior()
        {
            JObject jo = new JObject();
            string cCusCode = HttpContext.Current.Session["ConstcCusCode"].ToString();
            string datStartDate = HttpContext.Current.Request.Form["startDate"];
            string datEndDate = HttpContext.Current.Request.Form["endDate"];
            if (string.IsNullOrEmpty(datStartDate))
            {
                datStartDate = "2018-06-01";
            }
            if (string.IsNullOrEmpty(datEndDate))
            {
                datEndDate = DateTime.Now.ToString();
            }
            else
            {
                datEndDate = datEndDate + " 23:59:59";
            }
            DataTable dt = pro.GetUserBehavior(cCusCode, datStartDate, datEndDate);
            jo["flag"] = 1;
            jo["data"] = JToken.Parse(JsonConvert.SerializeObject(dt, timejson));
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region 需求下单页面加载时，先判断是否满足下单条件（网单时间、需求订单时间、信用）
        public void CheckXOrderPageOpen()
        {
            JObject jo = new JObject();
            jo = check.XOrderTimeControl();
            if (jo["flag"].ToString() == "1")
            {
                jo = check.XOrderBehaviorCheck(HttpContext.Current.Session["ConstcCusCode"].ToString());
            }
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion

        #region  新增普通订单保存前，检测正在进行中的需求订单里是否包括该订单里的产品
        public void CheckCodeInXOrder() {
            string strAllAcount = HttpContext.Current.Session["strAllAcount"].ToString();
            string codes = HttpContext.Current.Request.Form["codes"];
            string all = ConfigurationManager.AppSettings["isSearchAll"];
            Array arr = all.Split('|');
            if (Array.IndexOf(arr, HttpContext.Current.Session["ConstcCusCode"].ToString()) != -1)
            {
                strAllAcount = HttpContext.Current.Session["ConstcCusCode"].ToString();
            }
            DataTable dt = pro.CheckCodeInXOrder(strAllAcount, codes);
            JObject jo = new JObject();
            jo["flag"] = 1;
            jo["data"] = JToken.Parse(JsonConvert.SerializeObject(dt));
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jo));
            return;
        }
        #endregion
    }
}