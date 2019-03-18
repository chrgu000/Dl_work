using BLL;
using DevExpress.Web;
using System;
using System.Collections;
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

public partial class dluser_Allquery : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //绑定treelist数据源
        string treecuscode = "";
        if (Session["KPDWcCusCode"] == null)
        {
            treecuscode = Session["ConstcCusCode"].ToString();
        }
        else
        {
            treecuscode = Session["KPDWcCusCode"].ToString();
        }
        DataTable dttree = new SearchManager().DLproc_InventoryBySel("00", treecuscode);
        treeList.KeyFieldName = "KeyFieldName";
        treeList.ParentFieldName = "ParentFieldName";
        treeList.DataSource = dttree;
        /*展开第一级node*/
        treeList.DataBind();
        treeList.ExpandToLevel(1);
        //绑定gridview数据源
        DataTable dt1 = new DataTable();
        if (Session["ordertreelistgrid"] != null)
        {
            dt1 = new SearchManager().DLproc_TreeListDetailsAll_iqty_BySel(Session["ordertreelistgrid"].ToString(), Session["KPDWcCusCode"].ToString());
            TreeDetail.DataSource = dt1;
            TreeDetail.DataBind();
        }
    }

    /*ASPxTreeList的FocuseNodeChnaged事件来处理选择Node时的逻辑,需要引用using DevExpress.Web.ASPxTreeList;*/
    protected void treeList_CustomDataCallback(object sender, TreeListCustomDataCallbackEventArgs e)  //获取选中行的值,保存
    {
        //DataTable dtst = (DataTable)Session["gridselect"];  //获取选中行的值,保存
        //if (TreeDetail.Selection.Count > 0)
        //{
        //    for (int i = 0; i < TreeDetail.Selection.Count; i++) //获取选中的数据
        //    {
        //        dtst.Rows.Add(new object[] { TreeDetail.GetSelectedFieldValues("cInvCode")[i].ToString() });
        //    }
        //    Session["gridselect"] = dtst;
        //    //ASPxGridView1.DataSource = dtst;//测试用
        //    //ASPxGridView1.DataBind();
        //}

        /// 设置选中时节点的背景色


        string key = e.Argument.ToString();
        TreeListNode node = treeList.FindNodeByKeyValue(key);
        //if (node.ChildNodes.Count == 0)       //末级节点
        //{
        //    e.Result = GetEntryText(node);//获取node的code值    
        //}
        if (node.Level >= 2)
        {
            e.Result = GetEntryText(node);//获取node的code值          
        }
        else
        {
            //Session["ordertreelistgrid"] = "";
            Session.Remove("ordertreelistgrid");
        }

    }

    protected string GetEntryText(TreeListNode node)    //树节点调用
    {
        if (node != null)
        {
            string KeyFieldName = node["KeyFieldName"].ToString();
            Session["ordertreelistgrid"] = KeyFieldName.ToString();//赋值给grid查询
            //查询并绑定gridview
            //DataTable dt1 = new SearchManager().DLproc_TreeListDetailsAll_iqty_BySel(Session["ordertreelistgrid"].ToString(), Session["KPDWcCusCode"].ToString());
            //TreeDetail.DataSource = dt1;
            //TreeDetail.DataBind();
            return KeyFieldName.Trim();

        }
        return string.Empty;
    }
    protected void btn_Click(object sender, EventArgs e)    //前台按钮调用
    {

    }

}