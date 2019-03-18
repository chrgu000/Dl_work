using System;
using System.Collections;
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
using DevExpress.Web.ASPxTreeList;

public partial class test_xingzhengqu : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void TreeList_CustomJSProperties(object sender, TreeListCustomJSPropertiesEventArgs e)    //行政区树结构
    {
        ASPxTreeList treeList = sender as ASPxTreeList;
        Hashtable nameTable = new Hashtable();
        foreach (TreeListNode node in treeList.GetVisibleNodes())
            //nameTable.Add(node.Key, string.Format("{0} {1}", node["vsimpleName"], node["vdescription"]));
            nameTable.Add(node.Key, string.Format("{0}", node["vdescription"]));
        e.Properties["cpvsimpleName"] = nameTable;
        treeList.ExpandToLevel(0);
    }

}