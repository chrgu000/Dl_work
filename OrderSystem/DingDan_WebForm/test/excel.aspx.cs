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
using System.IO;
using NPOI.HSSF.Util;
using NPOI.SS.Util;
using System.Text;


namespace DingDan_WebForm.test
{
    public partial class excel : System.Web.UI.Page
    {
   
        protected void Page_Load(object sender, EventArgs e)
        {
           
            //using (FileStream stm = File.OpenWrite(@"c:/myMergeCell.xls"))
            using (FileStream stm = new FileStream(HttpContext.Current.Server.MapPath("~/tpl/OrderExcelTpl.xls"), FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
              //  wb.Write(stm);
                HSSFWorkbook workbook = new HSSFWorkbook(stm);
                stm.Close();
                ISheet sheet = workbook.GetSheetAt(0);
                IRow row = sheet.GetRow(11);
                row.CreateCell(0).SetCellValue("123");
                row.CreateCell(1).SetCellValue("222222");
                row.CreateCell(2).SetCellValue("123333333");
                row.CreateCell(3).SetCellValue("444444444");
                row.CreateCell(4).SetCellValue("55555555");
                MemoryStream ms = new MemoryStream();
                workbook.Write(ms);
                Response.AddHeader("Content-Disposition", string.Format("attachment; filename=EmptyWorkbook.xls"));
                Response.BinaryWrite(ms.ToArray()); workbook = null; ms.Close(); ms.Dispose();
                //IRow row = s.CreateRow(3);
                //s.CreateRow(3).CreateCell(0).SetCellValue("asdfasdf");
                //s.GetRow(3).CreateCell(1).SetCellValue("1111111111");
                //s.GetRow(3).CreateCell(2).SetCellValue("会员卡号：会员卡号长度为3~20位,且只能数字或者英文字母会员卡号：会员卡号长度为3~20位,且只能数字或者英文字母");
                // ICellStyle notesStyle = hssfworkbookDown.CreateCellStyle();
                //notesStyle.WrapText = true;//设置换行这个要先设置
                //notesStyle.VerticalAlignment = VerticalAlignment.Center;
                //s.GetRow(3).GetCell(1).CellStyle = notesStyle;
                //s.GetRow(3).GetCell(2).CellStyle = notesStyle;
                //using (FileStream ss = File.OpenWrite(@"c:/myMergeCell1.xls"))
                //{
                //    hssfworkbookDown.Write(ss);
                //}

  
            }

            //WriteToExcel(@"c:/2.xls");
            //HSSFWorkbook wk = new HSSFWorkbook();
            ////创建一个名称为mySheet的表
            //ISheet tb = wk.CreateSheet("mySheet");
            ////创建一行，此行为第二行
            //IRow row = tb.CreateRow(1);
            //for (int i = 0; i < 20; i++)
            //{
            //    ICell cell = row.CreateCell(i);  //在第二行中创建单元格
            //    cell.SetCellValue(i);//循环往第二行的单元格中添加数据
            //}
            //using (FileStream fs = File.OpenWrite(@"c:/myxls.xls")) //打开一个xls文件，如果没有则自行创建，如果存在myxls.xls文件则在创建是不要打开该文件！
            //{
            //    wk.Write(fs);   //向打开的这个xls文件中写入mySheet表并保存。
              
            //}
           
            //IWorkbook wb = new HSSFWorkbook();
            ////创建表  
            //ISheet sh = wb.CreateSheet("zhiyuan");
            
            ////设置单元的宽度  
            //sh.SetColumnWidth(0, 15 * 256);
            //sh.SetColumnWidth(1, 35 * 256);
            //sh.SetColumnWidth(2, 15 * 256);
            //sh.SetColumnWidth(3, 10 * 256);
            //int i = 0;
            //#region 练习合并单元格
            //sh.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 3));

            ////CellRangeAddress（）该方法的参数次序是：开始行号，结束行号，开始列号，结束列号。

            //IRow row0 = sh.CreateRow(0);
            //row0.Height = 20 * 20;
            //ICell icell1top0 = row0.CreateCell(0);
            //icell1top0.CellStyle = Getcellstyle(wb, stylexls.头);
            //icell1top0.SetCellValue("标题合并单元");
            //#endregion
            //i++;
            //#region 设置表头
            //IRow row1 = sh.CreateRow(2);
            //row1.Height = 20 * 20;
           
            //ICell icell1top = row1.CreateCell(0);
            //icell1top.CellStyle = Getcellstyle(wb, stylexls.头);
            //icell1top.SetCellValue("网站名");

            //ICellStyle style = wb.CreateCellStyle();
            //style.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
            //style.FillPattern = FillPattern.SolidForeground;
            //icell1top.CellStyle = style;

            //ICell icell2top = row1.CreateCell(1);
            //icell2top.CellStyle = Getcellstyle(wb, stylexls.头);
            //icell2top.SetCellValue("网址");

            //ICell icell3top = row1.CreateCell(2);
            //icell3top.CellStyle = Getcellstyle(wb, stylexls.头);
            //icell3top.SetCellValue("百度快照");

            //ICell icell4top = row1.CreateCell(3);
            //icell4top.CellStyle = Getcellstyle(wb, stylexls.头);
            //icell4top.SetCellValue("百度收录");
            //#endregion

            //using (FileStream stm =File.OpenWrite(@"c:/myMergeCell.xls"))
            //{
            //    wb.Write(stm);
            //    //HSSFWorkbook hssfworkbookDown = new HSSFWorkbook(stm);
            //    //stm.Close();
            //    //ISheet s = hssfworkbookDown.GetSheetAt(0);
            //    // s.GetRow(1).GetCell(1).SetCellValue("asdfasdf");
            //    // using (FileStream ss = File.OpenWrite(@"c:/myMergeCell1.xls"))
            //    // {
            //    //     hssfworkbookDown.Write(ss);
            //    // }
            //}
        }

        #region 定义单元格常用到样式的枚举
        public enum stylexls
        {
            头,
            url,
            时间,
            数字,
            钱,
            百分比,
            中文大写,
            科学计数法,
            默认
        }
        #endregion


        #region 定义单元格常用到样式
        static ICellStyle Getcellstyle(IWorkbook wb, stylexls str)
        {
            ICellStyle cellStyle = wb.CreateCellStyle();

            //定义几种字体  
            //也可以一种字体，写一些公共属性，然后在下面需要时加特殊的  
            IFont font12 = wb.CreateFont();
            font12.FontHeightInPoints = 10;
            font12.FontName = "微软雅黑";


            IFont font = wb.CreateFont();
            font.FontName = "微软雅黑";
            //font.Underline = 1;下划线  
            HSSFColor a = new HSSFColor();
          
            IFont fontcolorblue = wb.CreateFont();
            fontcolorblue.Color = HSSFColor.Green.Index;
            fontcolorblue.IsItalic = true;//下划线  
            fontcolorblue.FontName = "微软雅黑";


            //边框  
            cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Dotted;
            cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Hair;
            cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Hair;
            cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Dotted;
            //边框颜色  
            cellStyle.BottomBorderColor = HSSFColor.OliveGreen.Blue.Index;
            cellStyle.TopBorderColor = HSSFColor.OliveGreen.Blue.Index;

            //背景图形，我没有用到过。感觉很丑  
            //cellStyle.FillBackgroundColor = HSSFColor.OLIVE_GREEN.BLUE.index;  
            //cellStyle.FillForegroundColor = HSSFColor.OLIVE_GREEN.BLUE.index;  
            cellStyle.FillForegroundColor = HSSFColor.White.Index;
            // cellStyle.FillPattern = FillPatternType.NO_FILL;  
            cellStyle.FillBackgroundColor = HSSFColor.Blue.Index;

            //水平对齐  
            cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;

            //垂直对齐  
            cellStyle.VerticalAlignment = VerticalAlignment.Center;

            //自动换行  
            cellStyle.WrapText = true;

            //缩进;当设置为1时，前面留的空白太大了。希旺官网改进。或者是我设置的不对  
            cellStyle.Indention = 0;

            //上面基本都是设共公的设置  
            //下面列出了常用的字段类型  
            switch (str)
            {
                case stylexls.头:
                    // cellStyle.FillPattern = FillPatternType.LEAST_DOTS;  
                    cellStyle.SetFont(font12);
                    break;
                case stylexls.时间:
                    IDataFormat datastyle = wb.CreateDataFormat();

                    cellStyle.DataFormat = datastyle.GetFormat("yyyy/mm/dd");
                    cellStyle.SetFont(font);
                    break;
                case stylexls.数字:
                    cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00");
                    cellStyle.SetFont(font);
                    break;
                case stylexls.钱:
                    IDataFormat format = wb.CreateDataFormat();
                    cellStyle.DataFormat = format.GetFormat("￥#,##0");
                    cellStyle.SetFont(font);
                    break;
                //case stylexls.url:
                //    fontcolorblue.Underline = 1;
                //    cellStyle.SetFont(fontcolorblue);
                //    break;
                case stylexls.百分比:
                    cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00%");
                    cellStyle.SetFont(font);
                    break;
                case stylexls.中文大写:
                    IDataFormat format1 = wb.CreateDataFormat();
                    cellStyle.DataFormat = format1.GetFormat("[DbNum2][$-804]0");
                    cellStyle.SetFont(font);
                    break;
                case stylexls.科学计数法:
                    cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00E+00");
                    cellStyle.SetFont(font);
                    break;
                case stylexls.默认:
                    cellStyle.SetFont(font);
                    break;
            }
            return cellStyle;


        }
        #endregion  

        public void WriteToExcel(string filePath)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
           
            // 新增試算表。
          ISheet sh1=  workbook.CreateSheet("試算表 A");
            workbook.CreateSheet("試算表 B");
            workbook.CreateSheet("試算表 C");
          ICell cell1=  sh1.CreateRow(0).CreateCell(0);
          IRow row;
          ICell cell;
          HSSFCellStyle styleF = (HSSFCellStyle)workbook.CreateCellStyle();  
           HSSFFont font =(HSSFFont) workbook.CreateFont();  
           font.FontName = "宋体";//字体  
           font.FontHeightInPoints = 25;//字号  
           font.Color = HSSFColor.Black.Index;//颜色  
           font.Underline = NPOI.SS.UserModel.FontUnderlineType.Double;//下划线  
          // font.IsStrikeout = true;//删除线  
          // font.IsItalic = true;//斜体  
           font.IsBold = true;//加粗  
  
           styleF.SetFont(font); 
          for (int i = 0; i < 10; i++)
          {
              row = sh1.CreateRow(i);
             
              row.Height = (short)((i + 2) * 200);
              sh1.SetColumnWidth(i, (i + 1) * 2560);
              for (int j = 0; j < 10; j++)
              {
                  cell = row.CreateCell(j);
                  cell.SetCellValue(j);
                  cell.CellStyle.SetFont(font);
              }
          }
          MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename=EmptyWorkbook.xls"));
            Response.BinaryWrite(ms.ToArray()); workbook = null; ms.Close(); ms.Dispose();
      
        }
    }
}