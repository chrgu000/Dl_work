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




public partial class UserAddress : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (!IsPostBack)
        //{
        GridPSbind();   //绑定配送数据表
        GridZTbind();   //绑定自提数据表
        GridZTXZQbind();   //绑定自提数据表
        //GridPS.StartEdit(3);
        //}

    }

    protected void GridPS_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)   //配送(修改)
    {
        //int index = grid.DataKeys[e.NewEditIndex].Value;//获取主键的值

        //取值 用e.NewValues[索引]
        string lngopUseraddressId = Convert.ToString(e.Keys[0]);
        string strConsigneeName = e.NewValues["strConsigneeName"].ToString().Trim();
        string strConsigneeTel = e.NewValues["strConsigneeTel"].ToString().Trim();
        //string strReceivingAddress = e.NewValues[2].ToString();
        //string strDistrict = e.NewValues["strDistrict"].ToString(); //省市区,行政区域
        //string strDistrict =(DevExpress.Web.ASPxEditors.ASPxTextBox)gv_Employees.FindEditFormTemplateControl("txtName");
        //DevExpress.Web.ASPxGridView.FindParentGridTemplateContainer("ASPxTextBox1")
        GridViewDataColumn columnHobbies1 = GridPS.Columns["strReceivingAddress"] as GridViewDataColumn; //取出GridView的Column
        GridViewDataColumn columnHobbies2 = GridPS.Columns["strDistrict"] as GridViewDataColumn; //取出GridView的Column
        //通过ASPxGridView1.FindEditRowCellTemplateControl找出自定义的CheckBox
        ASPxTextBox cbH1 = (GridPS.FindEditRowCellTemplateControl(columnHobbies1, "txtName") as ASPxTextBox);
        ASPxDropDownEdit cbH2 = (GridPS.FindEditRowCellTemplateControl(columnHobbies2, "DropDownEdit") as ASPxDropDownEdit);
        //ASPxPageControl pageControl = GridPS.FindEditFormTemplateControl("txtName") as ASPxPageControl;
        //ASPxTextBox memo = pageControl.FindControl("notesEditor") as ASPxTextBox;
        string strReceivingAddress = cbH1.Text.ToString().Trim();
        string strDistrict = cbH2.Text.ToString().Trim();
        //更新
        strConsigneeName = strConsigneeName.Replace(" ", "");
        strReceivingAddress = strReceivingAddress.Replace(",", ";").Replace("，", ";").Trim();  //2016-04-27修改,过滤地址中的逗号
        BasicInfo bi = new BasicInfo(lngopUseraddressId, strConsigneeName, strConsigneeTel, strReceivingAddress, strDistrict, "ps");
        bool c = new BasicInfoManager().Update_UserAddressPS(bi);
        GridPS.CancelEdit();//结束编辑状态
        e.Cancel = true;
        GridPSbind();//重新绑定配送表
    }
    protected void GridZT_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)   //自提(修改)
    {
        //取值 用e.NewValues[索引]
        string lngopUseraddressId = Convert.ToString(e.Keys[0]);
        string strIdCard = e.NewValues["strIdCard"].ToString().Trim();
        string strDriverTel = e.NewValues["strDriverTel"].ToString().Trim();
        string strDriverName = e.NewValues["strDriverName"].ToString().Trim();
        string strCarplateNumber = e.NewValues["strCarplateNumber"].ToString().Trim();
        //更新
        BasicInfo bi = new BasicInfo(lngopUseraddressId, strCarplateNumber, strDriverName, strDriverTel, strIdCard);
        bool c = new BasicInfoManager().Update_UserAddressZT(bi);
        GridPS.CancelEdit();//结束编辑状态
        e.Cancel = true;
        GridZTbind();//重新绑定自提表
    }
    protected void GridZTXZQ_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)   //自提(修改)
    {

    }

    protected void GridPSbind()//绑定配送表
    {
        DataTable dt = new DataTable();
        dt = new BasicInfoManager().DLproc_UserAddressPSBySel(Session["lngopUserId"].ToString());
        GridPS.DataSource = dt;
        GridPS.DataBind();
    }

    protected void GridZTbind()//绑定自提表
    {
        DataTable dt = new DataTable();
        dt = new BasicInfoManager().DLproc_UserAddressZTBySel(Session["lngopUserId"].ToString());
        GridZT.DataSource = dt;
        GridZT.DataBind();
    }

    protected void GridZTXZQbind()//绑定自提行政区表
    {
        DataTable dt = new DataTable();
        dt = new BasicInfoManager().DL_UserAddressZTXZQBySel(Session["ConstcCusCode"].ToString());
        GridZTXZQ.DataSource = dt;
        GridZTXZQ.DataBind();
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
    protected void GridPS_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e) //配送(新增)
    {
        //取值 用e.NewValues[索引]
        string strConsigneeName = e.NewValues["strConsigneeName"].ToString().Trim();
        string strConsigneeTel = e.NewValues["strConsigneeTel"].ToString().Trim();
        //string strReceivingAddress = e.NewValues[2].ToString();
        //string strDistrict = e.NewValues[3].ToString();
        GridViewDataColumn columnHobbies1 = GridPS.Columns["strReceivingAddress"] as GridViewDataColumn; //取出GridView的Column
        GridViewDataColumn columnHobbies2 = GridPS.Columns["strDistrict"] as GridViewDataColumn; //取出GridView的Column
        //通过ASPxGridView1.FindEditRowCellTemplateControl找出自定义的CheckBox
        ASPxTextBox cbH1 = (GridPS.FindEditRowCellTemplateControl(columnHobbies1, "txtName") as ASPxTextBox);
        ASPxDropDownEdit cbH2 = (GridPS.FindEditRowCellTemplateControl(columnHobbies2, "DropDownEdit") as ASPxDropDownEdit);
        string strReceivingAddress = cbH1.Text.ToString().Trim();
        string strDistrict = cbH2.Text.ToString().Trim();
        string strDistributionType = "配送";
        //新增
        strConsigneeName = strConsigneeName.Replace(" ", "");
        strReceivingAddress = strReceivingAddress.Replace(",", ";").Replace("，", ";");  //2016-04-27修改,过滤地址中的逗号
        bool c = new BasicInfoManager().Insert_UserAddressPS(Session["lngopUserId"].ToString(), strDistributionType, strConsigneeName, strConsigneeTel, strReceivingAddress, strDistrict);
        GridPS.CancelEdit();//结束编辑状态
        e.Cancel = true;
        GridPSbind();//重新绑定配送表
    }
    protected void GridZT_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e) //自提新增
    {
        string strIdCard = e.NewValues["strIdCard"].ToString().Trim();       //身份证
        string strDriverTel = e.NewValues["strDriverTel"].ToString().Trim();    //司机电话
        string strDriverName = e.NewValues["strDriverName"].ToString().Trim();   //司机姓名
        string strCarplateNumber = e.NewValues["strCarplateNumber"].ToString().Trim();//车牌号
        string strDistributionType = "自提";
        //新增
        bool c = new BasicInfoManager().Insert_UserAddressZT(Session["lngopUserId"].ToString(), strDistributionType, strCarplateNumber, strDriverName, strDriverTel, strIdCard, Session["lngopUserId"].ToString());
        GridZT.CancelEdit();//结束编辑状态
        e.Cancel = true;
        GridZTbind();//重新绑定自提表
    }
    protected void GridZTXZQ_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e) //自提新增行政区域
    {
        GridViewDataColumn columnHobbies2 = GridZTXZQ.Columns["xzq"] as GridViewDataColumn; //取出GridView的Column
        //通过ASPxGridView1.FindEditRowCellTemplateControl找出自定义的CheckBox
        ASPxDropDownEdit cbH2 = (GridZTXZQ.FindEditRowCellTemplateControl(columnHobbies2, "DropDownEditxzq") as ASPxDropDownEdit);
        string xzq = cbH2.Text.ToString();
        ////新增
        bool c = new BasicInfoManager().Insert_UserAddressZTXZQ(Session["ConstcCusCode"].ToString(), xzq, Session["ConstcCusCode"].ToString() + xzq);
        GridZTXZQ.CancelEdit();//结束编辑状态
        e.Cancel = true;
        GridZTXZQbind();//重新绑定自提表
    }

    protected void GridZT_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
    {
        if (e.NewValues["strIdCard"] == null)
        {
            e.Errors.Add(GridZT.Columns["strIdCard"], "NoNone!");//check
            e.RowError = "请填写身份证!";
        }
        if (e.NewValues["strDriverTel"] == null)
        {
            e.Errors.Add(GridZT.Columns["strDriverTel"], "NoNone!");//check
            e.RowError = "请填写司机电话!";
        }
        if (e.NewValues["strDriverName"] == null)
        {
            e.Errors.Add(GridZT.Columns["strDriverName"], "NoNone!");//check
            e.RowError = "请填写司机姓名!";
        }
        if (e.NewValues["strCarplateNumber"] == null)
        {
            e.Errors.Add(GridZT.Columns["strCarplateNumber"], "NoNone!");//check
            e.RowError = "请填写车牌号!";
        }
    }
    protected void GridPS_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
    {
        //string strConsigneeName = e.NewValues["strConsigneeName"].ToString();
        //string strConsigneeTel = e.NewValues["strConsigneeTel"].ToString();
        GridViewDataColumn columnHobbies1 = GridPS.Columns["strReceivingAddress"] as GridViewDataColumn; //取出GridView的Column
        GridViewDataColumn columnHobbies2 = GridPS.Columns["strDistrict"] as GridViewDataColumn; //取出GridView的Column
        //通过ASPxGridView1.FindEditRowCellTemplateControl找出自定义的CheckBox
        ASPxTextBox cbH1 = (GridPS.FindEditRowCellTemplateControl(columnHobbies1, "txtName") as ASPxTextBox);
        ASPxDropDownEdit cbH2 = (GridPS.FindEditRowCellTemplateControl(columnHobbies2, "DropDownEdit") as ASPxDropDownEdit);
        string strReceivingAddress = cbH1.Text.ToString();
        string strDistrict = cbH2.Text.ToString();

        if (strReceivingAddress == null || strReceivingAddress == "")
        {
            e.Errors.Add(GridPS.Columns["strReceivingAddress"], "NoNone!");//check
            e.RowError = "请填写收货地址!";
            return;
        }
        if (e.NewValues["strConsigneeName"] == null)
        {
            e.Errors.Add(GridPS.Columns["strConsigneeName"], "NoNone!");//check
            e.RowError = "请填写收货人!";
            return;
        }
        if (e.NewValues["strConsigneeTel"] == null)
        {
            e.Errors.Add(GridPS.Columns["strConsigneeTel"], "NoNone!");//check
            e.RowError = "请填写联系电话!";
            return;
        }
        if (strDistrict == null || strDistrict == "")
        {
            e.Errors.Add(GridPS.Columns["strDistrict"], "NoNone!");//check
            e.RowError = "请选择行政区域!";
            return;
        }
        else//判断行政区是否是末级节点
        {
            bool c = new BasicInfoManager().IsExists_strDistrict(strDistrict);
            if (c)
            {

            }
            else
            {
                e.Errors.Add(GridPS.Columns["strDistrict"], "NoNone!");//check
                e.RowError = "请选择行政区域末级节点!";
                return;
            }
        }
    }
    protected void GridZTXZQ_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
    {


        GridViewDataColumn columnHobbies2 = GridZTXZQ.Columns["xzq"] as GridViewDataColumn; //取出GridView的Column
        //通过ASPxGridView1.FindEditRowCellTemplateControl找出自定义的CheckBox
        ASPxDropDownEdit cbH2 = (GridZTXZQ.FindEditRowCellTemplateControl(columnHobbies2, "DropDownEditxzq") as ASPxDropDownEdit);
        string xzq = cbH2.Text.ToString().Trim();


        if (xzq == null || xzq == "")
        {
            e.Errors.Add(GridZTXZQ.Columns["xzq"], "NoNone!");//check
            e.RowError = "请选择行政区域!";
            return;
        }
        else//判断行政区是否是末级节点
        {
            bool c = new BasicInfoManager().IsExists_strDistrict(xzq);
            if (c)
            {

            }
            else
            {
                e.Errors.Add(GridZTXZQ.Columns["xzq"], "NoNone!");//check
                e.RowError = "请选择行政区域末级节点!";
                return;
            }
        }
    }


    protected void GridPS_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
    {
        string id = e.Values["lngopUseraddressId"].ToString();
        //更新地址信息状态为1
        bool c = new BasicInfoManager().DL_UserAddressByDel(id);
        if (c)
        {
            GridPSbind();//重新绑定自提表
        }
        else
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('删除失败,请联系管理员！');</script>");
            GridPSbind();//重新绑定自提表
        }
        e.Cancel = true;
    }
    protected void GridZT_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
    {
        string id = e.Values["lngopUseraddressId"].ToString();
        //更新地址信息状态为1
        bool c = new BasicInfoManager().DL_UserAddressByDel(id);
        if (c)
        {
            GridZTbind();//重新绑定自提表
        }
        else
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('删除失败,请联系管理员！');</script>");
            GridZTbind();//重新绑定自提表
        }
        e.Cancel = true;
    }

    protected void GridZTXZQ_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
    {
        string id = e.Values["lngopUseraddress_exId"].ToString();
        //更新地址信息状态为1
        bool c = new BasicInfoManager().DL_UserAddress_exByDel(id);
        if (c)
        {
            GridZTXZQbind();//重新绑定自提表
        }
        else
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('删除失败,请联系管理员！');</script>");
            GridZTXZQbind();//重新绑定自提表
        }
        e.Cancel = true;
    }

}