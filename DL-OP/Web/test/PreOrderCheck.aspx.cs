using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using BLL;

public partial class test_PreOrderCheck : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        DataColumn cinvcode = new DataColumn();
        //该列的数据类型
        cinvcode.DataType = System.Type.GetType("System.String");
        //该列得名称
        cinvcode.ColumnName = "cinvcode";
        //该列得默认值
        cinvcode.DefaultValue = 999;
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
        dt.Columns.Add(cinvcode);
        dt.Columns.Add(qty);
        dt.Columns.Add(preorderno);
        dt.Columns.Add(orderno);
        dt.Columns.Add(irowsno);
        dt.Rows.Add("1", 1,"1","1",1);
        dt.Rows.Add("2", 2, "2", "2",2);
        dt.Rows.Add("3", 3, "3", "3",3);
        DataTable sx = new OrderManager().DLproc_PreOrderSubmitForCheckBySel(dt);
        TextBox1.Text = sx.Rows[0][0].ToString();
    }
}