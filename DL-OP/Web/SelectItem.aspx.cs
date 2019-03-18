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

public partial class SelectItem : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        #region 判断session是否存在,并且建立datatable,用于记录选择项目,gridselect
        if (Session["gridselect"] == null)
        {
            DataTable dts = new DataTable();
            dts.Columns.Add("cInvCode"); //编码    0            
            //dt.Rows.Add(new object[] { "0"});
            Session["gridselect"] = dts;
        }
        #endregion
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
            dt1 = new SearchManager().DLproc_TreeListDetailsAllBySel(Session["ordertreelistgrid"].ToString(), Session["KPDWcCusCode"].ToString(),1);
            TreeDetail.DataSource = dt1;
            TreeDetail.DataBind();
        }
        //添加商品

    }

    /*ASPxTreeList的FocuseNodeChnaged事件来处理选择Node时的逻辑,需要引用using DevExpress.Web.ASPxTreeList;*/
    protected void treeList_CustomDataCallback(object sender, TreeListCustomDataCallbackEventArgs e)
    {
        //DataTable dtst = (DataTable)Session["gridselect"];  //获取选中行的值,保存
        //if (TreeDetail.Selection.Count > 0)
        //{
        //    for (int i = 0; i < TreeDetail.Selection.Count; i++)
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
        if (node.Level >= 1)
        {
            e.Result = GetEntryText(node);//获取node的code值          
        }

    }

    protected string GetEntryText(TreeListNode node)    //树节点调用
    {
        if (node != null)
        {
            string KeyFieldName = node["KeyFieldName"].ToString();
            Session["ordertreelistgrid"] = KeyFieldName.ToString();//赋值给grid查询
            return KeyFieldName.Trim();
            //查询并绑定gridview

        }
        return string.Empty;
    }

    protected void btn_Click(object sender, EventArgs e)    //前台按钮调用
    {
        ////DataTable dtst = (DataTable)Session["gridselect"];
        ////for (int i = 0; i < TreeDetail.Selection.Count; i++)
        ////{
        ////    dtst.Rows.Add(new object[] { TreeDetail.GetSelectedFieldValues("cInvCode")[i].ToString() });
        ////}
        ////Session["gridselect"] = dtst;
        ////绑定gridview数据源
        //if (Session["ordertreelistgrid"] != null)
        //{
        //    DataTable dt1 = new SearchManager().DLproc_TreeListDetailsAllBySel(Session["ordertreelistgrid"].ToString(), Session["KPDWcCusCode"].ToString());
        //    TreeDetail.DataSource = dt1;
        //    TreeDetail.DataBind();
        //    //删除当前绑定的数据,因为切换分类,所以需要删除
        //    DataTable dtst = (DataTable)Session["gridselect"];
        //    for (int i = 0; i < dt1.Rows.Count; i++)
        //    {
        //        for (int j = 0; j < dtst.Rows.Count; j++)
        //        {
        //            if (TreeDetail.GetRowValues(i, "cInvCode") != null && TreeDetail.GetRowValues(i, "cInvCode").ToString() == dtst.Rows[j][0].ToString())
        //            {
        //                TreeDetail.Selection.SelectRow(i);
        //                dtst.Rows[j].Delete();
        //                dtst.AcceptChanges();
        //            }
        //        }
        //    }
        //}
    }



}