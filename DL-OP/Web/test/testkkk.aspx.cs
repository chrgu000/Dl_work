using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Model;
using System.Data;
using System.Data.SqlClient;
using DevExpress.Web;


public partial class testkkk : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            gridztbind();
        }

    }
    protected void gridzt_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)   //自提(修改)
    {
        //取值 用e.NewValues[索引]
        string lngopUseraddressId = Convert.ToString(e.Keys[0]);
        //string strIdCard = e.NewValues[0].ToString();
        string strIdCard = ((Int32.Parse(e.NewValues[3].ToString()) + Int32.Parse(e.NewValues[2].ToString()))* Int32.Parse(e.NewValues[1].ToString())).ToString();
        string strDriverTel = e.NewValues[1].ToString();
        string strDriverName = e.NewValues[2].ToString();
        string strCarplateNumber = e.NewValues[3].ToString();
        //更新
        BasicInfo bi = new BasicInfo(lngopUseraddressId, strCarplateNumber, strDriverName, strDriverTel, strIdCard);
        bool c = new BasicInfoManager().Update_UserAddressZT(bi);
        gridzt.CancelEdit();//结束编辑状态
        e.Cancel = true;
        gridztbind();//重新绑定自提表
    }
    protected void gridztbind()//绑定自提表
    {
        DataTable dt = new DataTable();
        dt = new BasicInfoManager().DLproc_UserAddressZTBySel("2");
        gridzt.DataSource = dt;
        gridzt.DataBind();
    }
    protected void gridzt_FocusedRowChanged(object sender, EventArgs e)
    {
        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('OK！');</script>");
    }
}