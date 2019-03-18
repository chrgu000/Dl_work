using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading;
using DevExpress.Web;
using DevExpress.Web.ASPxTreeList;
using BLL;
using Model;

public partial class OrderMaterial : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["treelistgrid"] = "";
        }
        //绑定treelist数据源
        DataTable dt = new SearchManager().DLproc_InventoryBySel(Session["cSTCode"].ToString(), Session["ModifycCusCode"].ToString());
        treeList.KeyFieldName = "KeyFieldName";
        treeList.ParentFieldName = "ParentFieldName";
        treeList.DataSource = dt;
        /*展开第一级node*/
        treeList.DataBind();
        treeList.ExpandToLevel(1);
        //绑定gridview数据源
        //DataTable dt1 = new SearchManager().DLproc_TreeListDetailsBySel(Session["treelistgrid"].ToString(), Session["cCusCode"].ToString());
        DataTable dt1 = new SearchManager().DLproc_TreeListDetailsAllBySel(Session["treelistgrid"].ToString(), Session["cCusCode"].ToString(),1);
        TreeDetail.DataSource = dt1;
        TreeDetail.DataBind();
    }

    /*ASPxTreeList的FocuseNodeChnaged事件来处理选择Node时的逻辑,需要引用using DevExpress.Web.ASPxTreeList;*/
    protected void treeList_CustomDataCallback(object sender, TreeListCustomDataCallbackEventArgs e)
    {
        string key = e.Argument.ToString();
        TreeListNode node = treeList.FindNodeByKeyValue(key);
        e.Result = GetEntryText(node);//获取node的code值
    }

    protected string GetEntryText(TreeListNode node)
    {
        if (node != null)
        {
            string KeyFieldName = node["KeyFieldName"].ToString();
            Session["treelistgrid"] = KeyFieldName.ToString();//赋值给grid查询
            return KeyFieldName.Trim();
        }
        return string.Empty;
    }



    protected void btn_Click(object sender, EventArgs e)
    {

    }
    protected void TreeDetail_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
    {
        if (e.GetValue("fAvailQtty") != null)
        {
            string fAvailQtty = e.GetValue("fAvailQtty").ToString();
            float fAvailQttyInt = float.Parse(fAvailQtty);
            if (StockControl.Checked)
            {
                if (fAvailQttyInt <= 0)
                {
                    //e.Row.Cells[11].Style.Add("color", "Red");
                    e.Row.Style.Add("display", "none"); //不显示该行
                }

            }
            else
            {
                if (fAvailQttyInt <= 0)
                {
                    e.Row.Style.Add("color", "Red");    //该行标红
                }
            }

        }
    }


}