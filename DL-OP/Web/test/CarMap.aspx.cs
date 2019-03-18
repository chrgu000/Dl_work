using BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;

public partial class test_CarMap : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //string strbillno = Request.QueryString["strBillNo"].ToString();
        //DataTable dt = new SearchManager().DL_OrderLOGBySel(strbillno);
        //if (dt.Rows.Count > 0)
        //{
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {

        //创建data
        DataTable data = new DataTable();
        DataColumn XY = new DataColumn();
        //该列的数据类型
        XY.DataType = System.Type.GetType("System.String");
        //该列得名称
        XY.ColumnName = "XY";
        // 将所有的列添加到table上
        data.Columns.Add(XY);
        DataRow row = data.NewRow();
        //将此行添加到table中
        data.Rows.Add("103.92566,30.56991");
        data.Rows.Add("103.916275,30.609936");

        //创建data
        DataTable dt = new DataTable();
        DataColumn datDatetime = new DataColumn();
        //该列的数据类型
        datDatetime.DataType = System.Type.GetType("System.String");
        //该列得名称
        datDatetime.ColumnName = "datDatetime";
        //该列得默认值
        datDatetime.DefaultValue = 999;
        //创建table的第二列
        DataColumn lbs = new DataColumn();
        //该列的数据类型
        lbs.DataType = System.Type.GetType("System.String");
        //该列得名称
        lbs.ColumnName = "lbs";
        //该列得默认值
        lbs.DefaultValue = "";
        //创建table的第三列
        DataColumn local = new DataColumn();
        //该列的数据类型
        local.DataType = System.Type.GetType("System.String");
        //该列得名称
        local.ColumnName = "local";
        //该列得默认值
        local.DefaultValue = "";
        // 将所有的列添加到table上
        dt.Columns.Add(datDatetime);
        dt.Columns.Add(lbs);
        dt.Columns.Add(local);

        for (int i = 0; i < data.Rows.Count; i++)
        {
            #region 使用LINQ读取
            string sURL = "http://restapi.amap.com/v3/assistant/coordinate/convert?output=xml&key=e3c3f60c1ceb5de69b94e06d0afdda45&locations=" + data.Rows[i]["XY"].ToString()+ "&coordsys=baidu";
            XDocument oXDoc = XDocument.Load(sURL);
            var qurey = from x in oXDoc.Descendants()
                        where x.NodeType == XmlNodeType.Element
                        select new
                        {
                            ElementName = x.Name.ToString(),
                            ElementValue = x.Value
                        };
            foreach (var elementInfo in qurey)
            {
                //HttpContext.Current.Response.Write(string.Format("ElementName->{0} ElementValue->{1}<br />", elementInfo.ElementName, elementInfo.ElementValue));
                if (elementInfo.ElementName == "locations")
                {
                    HttpContext.Current.Response.Write(string.Format("{0}</br>", elementInfo.ElementValue));
                    string sURL11 = "http://restapi.amap.com/v3/geocode/regeo?output=xml&location=" + elementInfo.ElementValue + "&key=e3c3f60c1ceb5de69b94e06d0afdda45&radius=0&extensions=all ";
                    XDocument oXDoc11 = XDocument.Load(sURL11);
                    var qurey11 = from x1 in oXDoc11.Descendants()
                                  where x1.NodeType == XmlNodeType.Element
                                  select new
                                  {
                                      ElementName = x1.Name.ToString(),
                                      ElementValue = x1.Value
                                  };
                    foreach (var elementInfo11 in qurey11)
                    {
                        //HttpContext.Current.Response.Write(string.Format("ElementName->{0} ElementValue->{1}<br />", elementInfo11.ElementName, elementInfo11.ElementValue));
                        if (elementInfo11.ElementName == "formatted_address")
                        {
                            //DataRow row = dt.NewRow();
                            //将此行添加到table中
                            dt.Rows.Add("2016-08-28 18:00:00.000", elementInfo.ElementValue, elementInfo11.ElementValue);
                        }
                    }

                }
            }
            #endregion
        }


        

        //绑定grid
        ASPxGridView1.DataSource = dt;
        ASPxGridView1.DataBind();


        #region 使用XmlReader构造函数
        //string sURL = "http://restapi.amap.com/v3/assistant/coordinate/convert?output=xml&key=e3c3f60c1ceb5de69b94e06d0afdda45&locations=103.92566,30.56991&coordsys=baidu";
        //using (XmlReader read = XmlReader.Create(sURL))
        //{
        //    //read.ReadToDescendant("locations");
        //    //string date = read.GetAttribute("locations");
        //    //HttpContext.Current.Response.Write(string.Format("ElementValue->{0}<br />", date));
        //    while (read.Read())
        //    {
        //        switch (read.NodeType)
        //        {
        //            case XmlNodeType.Element:
        //                HttpContext.Current.Response.Write(string.Format("ElementName->{0} <br />", read.Name));
        //                break;
        //            case XmlNodeType.Text:
        //                HttpContext.Current.Response.Write(string.Format("ElementText->{0}<br />", read.Value));
        //                break;
        //            case XmlNodeType.CDATA:
        //                HttpContext.Current.Response.Write(string.Format("ElementCDATA->{0}<br />", read.Value));
        //                break;
        //            //other
        //        }
        //    }
        //}
        #endregion


        //    }
        //}

    }
}