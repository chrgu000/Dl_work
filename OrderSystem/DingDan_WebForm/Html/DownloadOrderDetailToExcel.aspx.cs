using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.SS.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.Util;
using System.IO;
using System.Data;

namespace DingDan_WebForm.Html
{
    public partial class DownloadOrderDetailToExcel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            try
            {
                string strbillno = HttpContext.Current.Request.QueryString["strbillno"];
                if (string.IsNullOrEmpty(strbillno))
                {
                    Response.Write("<script>alert('参数不正确！')</script>");
                    Response.End();
                    return;
                }

                DataTable dt = new BLL.product().GetOrderDetail(strbillno);
                if (dt.Rows.Count == 0)
                {
                    Response.Write("<script>alert('查询失败，请重试或联系管理员！')</script>");
                    Response.End();
                    return;
                }

                if (dt.Rows[0]["lngopUserId"].ToString() != HttpContext.Current.Session["lngopUserId"].ToString())
                {
                    Response.Write("<script>alert('你没有权限查看该订单！')</script>");
                    Response.End();
                    return;
                }

                using (FileStream stm = new FileStream(HttpContext.Current.Server.MapPath("~/tpl/OrderExcelTpl.xls"), FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    HSSFWorkbook workbook = new HSSFWorkbook(stm);
                    stm.Close();
                    ISheet sheet = workbook.GetSheetAt(0);
                    DataRow dr = dt.Rows[0];


                    IRow row = sheet.GetRow(2);
                    row.GetCell(1).SetCellValue(strbillno);
                    row.GetCell(4).SetCellValue(dr["ccusname"].ToString()); //开票单位

                    row = sheet.GetRow(3);
                    row.GetCell(1).SetCellValue(dr["datCreateTime"].ToString()); //下单时间

                    if (dr["datAuditordTime"].ToString() != "1905-6-21 0:00:00")
                    {
                        row.GetCell(4).SetCellValue(dr["datAuditordTime"].ToString()); //审核时间
                    }


                    row.GetCell(7).SetCellValue(dr["strAllAcount"].ToString()); //下单账号

                    row = sheet.GetRow(4);
                    row.GetCell(1).SetCellValue(dr["datDeliveryDate"].ToString()); //提货时间
                    row.GetCell(7).SetCellValue(dr["cdefine3"].ToString()); //车型

                    row = sheet.GetRow(5);
                    string cdefine11 = "";
                    if (dr["iAddressType"].ToString() == "1")
                    {
                        cdefine11 = dr["cdefine11"].ToString() + "," + dr["cdefine8"].ToString();
                    }
                    else if (dr["iAddressType"].ToString() == "2")
                    {
                        cdefine11 = dr["cdefine11"].ToString() + "  " + dr["psxx"].ToString();
                    }
                    else
                    {
                        cdefine11 = dr["cdefine11"].ToString();
                    }
                    row.GetCell(1).SetCellValue(cdefine11); //送货地址

                    row = sheet.GetRow(6);
                    row.GetCell(1).SetCellValue(dr["strLoadingWays"].ToString()); //装车方式

                    row = sheet.GetRow(7);
                    row.GetCell(1).SetCellValue(dr["strRemarks"].ToString()); //备注


                    //表体样式
                    ICellStyle cellStyle = workbook.CreateCellStyle();
                    IFont fontStyle = workbook.CreateFont();
                    cellStyle.Alignment = HorizontalAlignment.Center;//【Center】居中  
                    cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                    cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                    cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                    cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin; 
                    fontStyle.FontName = "华文楷体";
                    fontStyle.FontHeightInPoints = 12;
                    cellStyle.SetFont(fontStyle);




                    double sum = 0;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        row = sheet.CreateRow(11 + i);
                        row.Height = 600;
                        row.CreateCell(0).SetCellValue(int.Parse(dt.Rows[i]["irowno"].ToString()));
                        row.CreateCell(1).SetCellValue(dt.Rows[i]["cinvname"].ToString());
                        row.CreateCell(2).SetCellValue(dt.Rows[i]["cInvStd"].ToString());
                        row.CreateCell(3).SetCellValue(double.Parse(dt.Rows[i]["iquantity"].ToString()));
                        row.CreateCell(4).SetCellValue(dt.Rows[i]["cComUnitName"].ToString());
                        row.CreateCell(5).SetCellValue(dt.Rows[i]["cdefine22"].ToString());
                        row.CreateCell(6).SetCellValue(double.Parse(dt.Rows[i]["itaxunitprice"].ToString()));
                        row.CreateCell(7).SetCellValue(double.Parse(dt.Rows[i]["isum"].ToString()));
                        //row.CreateCell(7).SetCellValue(dt.Rows[i]["isum"].ToString());
                        row.GetCell(0).CellStyle = cellStyle;
                        row.GetCell(1).CellStyle = cellStyle;
                        row.GetCell(2).CellStyle = cellStyle;
                        row.GetCell(3).CellStyle = cellStyle;
                        row.GetCell(4).CellStyle = cellStyle;
                        row.GetCell(5).CellStyle = cellStyle;
                        row.GetCell(6).CellStyle = cellStyle;
                        row.GetCell(7).CellStyle = cellStyle;
                       
                        sum += double.Parse(dt.Rows[i]["isum"].ToString());
                    }

                    row = sheet.GetRow(2);
                    row.GetCell(7).SetCellValue(sum);                  //合计


                    MemoryStream ms = new MemoryStream();
                    workbook.Write(ms);
                    HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment; filename=多联网上订单" + strbillno + ".xls"));
                    HttpContext.Current.Response.BinaryWrite(ms.ToArray());
                    workbook = null;
                    ms.Close();
                    ms.Dispose();



                }
            }
            catch (Exception)
            {

                Response.Write("<script>alert('查询失败，请重试或联系管理员！')</script>");
                Response.End();
                return;
            }



        }
    }
}