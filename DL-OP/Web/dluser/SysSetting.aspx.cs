using BLL;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SysSetting : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Convert.ToInt32(HF.Value) < 1)
        {
            DataTable dt = new BasicInfoManager().DL_AllCustomerInfoBySel();
            ComboBoxPhone.Columns.Add("strUserName", "顾客名称", 120);
            ComboBoxPhone.Columns.Add("strLoginName", "顾客代码", 120);
            //ComboBoxPhone.DisplayFormatString = "{1}";
            ComboBoxPhone.DataSource = dt;
            ComboBoxPhone.DataBind();
        }
        HF.Value = (Convert.ToInt32(HF.Value) + 1).ToString();


        DataTable dtsoa = new BasicInfoManager().DL_SOAAutoSendBySel();
        SOAGrid.DataSource = dtsoa;
        SOAGrid.DataBind();

    }

    protected void PhoneGrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
    {
        //string username = Session["ConstcCusCode"].ToString();
        string username = HFcCusCode.Value;
        string phone = "";
        string id = e.Values["Id"].ToString();
        if (PhoneGrid.VisibleRowCount > 0)
        {
            for (int i = 0; i < PhoneGrid.VisibleRowCount; i++)
            {
                if (e.Values["Id"].ToString() != PhoneGrid.GetRowValues(i, "Id").ToString() || e.Values["PhoneNo"].ToString() != PhoneGrid.GetRowValues(i, "PhoneNo").ToString())
                {
                    phone = phone + PhoneGrid.GetRowValues(i, "PhoneNo").ToString().Trim() + ";";
                }
            }
            phone = phone.Substring(0, phone.Length - 1);
        }
        //插入数据
        bool c = new BasicInfoManager().DL_NewCustomerPhoneNoByIns(phone, username);
        if (!c)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('保存失败,请联系管理员！');</script>");
        }
        else
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('保存成功！');</script>");
        }
        PhoneGrid.CancelEdit();//结束编辑状态
        e.Cancel = true;

        DataTable dt = new GetPhoneNo().GetCustomerPhoneNo(username);
        if (dt.Rows.Count < 1)
        {
            PhoneGrid.SettingsText.EmptyDataRow = "还没有手机号码,赶紧添加一个!";
        }
        PhoneGrid.DataSource = dt;
        PhoneGrid.DataBind();
    }

    protected void PhoneGrid_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
    {
        //string username = Session["ConstcCusCode"].ToString();
        string username = HFcCusCode.Value;
        string phone = "";
        if (PhoneGrid.VisibleRowCount > 0)
        {
            for (int i = 0; i < PhoneGrid.VisibleRowCount; i++)
            {
                phone = phone + PhoneGrid.GetRowValues(i, "PhoneNo").ToString().Trim() + ";";
            }
        }
        //添加新的电话号码
        phone = phone + e.NewValues["PhoneNo"].ToString();
        //插入数据
        bool c = new BasicInfoManager().DL_NewCustomerPhoneNoByIns(phone, username);
        if (!c)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('保存失败,请联系管理员！');</script>");
        }
        else
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('保存成功！');</script>");
        }
        PhoneGrid.CancelEdit();//结束编辑状态
        e.Cancel = true;

        DataTable dt = new GetPhoneNo().GetCustomerPhoneNo(username);
        if (dt.Rows.Count < 1)
        {
            PhoneGrid.SettingsText.EmptyDataRow = "还没有手机号码,赶紧添加一个!";
        }
        PhoneGrid.DataSource = dt;
        PhoneGrid.DataBind();
    }

    protected void PhoneGrid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
    {
        //string username = Session["ConstcCusCode"].ToString();
        string username = HFcCusCode.Value;
        string phone = "";
        string id = e.NewValues["Id"].ToString();
        if (PhoneGrid.VisibleRowCount > 0)
        {
            for (int i = 0; i < PhoneGrid.VisibleRowCount; i++)
            {
                if (e.OldValues["Id"].ToString() != PhoneGrid.GetRowValues(i, "Id").ToString() || e.OldValues["PhoneNo"].ToString() != PhoneGrid.GetRowValues(i, "PhoneNo").ToString())
                {
                    phone = phone + PhoneGrid.GetRowValues(i, "PhoneNo").ToString().Trim() + ";";
                }
                else
                {
                    phone = phone + e.NewValues["PhoneNo"].ToString().Trim() + ";";
                }
            }
            phone = phone.Substring(0, phone.Length - 1);
        }
        //插入数据
        bool c = new BasicInfoManager().DL_NewCustomerPhoneNoByIns(phone, username);
        if (!c)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('保存失败,请联系管理员！');</script>");
        }
        else
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('保存成功！');</script>");
        }
        PhoneGrid.CancelEdit();//结束编辑状态
        e.Cancel = true;

        DataTable dt = new GetPhoneNo().GetCustomerPhoneNo(username);
        if (dt.Rows.Count < 1)
        {
            PhoneGrid.SettingsText.EmptyDataRow = "还没有手机号码,赶紧添加一个!";
        }
        PhoneGrid.DataSource = dt;
        PhoneGrid.DataBind();
    }

    protected void PhoneGrid_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e) //生成Grid的序号
    {
        if (e.Column.Caption == "序号" && e.IsGetData)
        {
            e.Value = (e.ListSourceRowIndex + 1).ToString();
        }
    }

    protected void BtnCustomerPhone_Click(object sender, EventArgs e)
    {
        string username = ComboBoxPhone.Text;
        if (username != "")
        {
            string[] sArray = username.Split(';');
            username = sArray[1].ToString();
            string cCusCode = Convert.ToString(username).Trim();
            HFcCusCode.Value = cCusCode;
        }
        if (HFcCusCode.Value != "0")
        {
            DataTable dtphone = new GetPhoneNo().GetCustomerPhoneNo(HFcCusCode.Value);
            if (dtphone.Rows.Count < 1)
            {
                PhoneGrid.SettingsText.EmptyDataRow = "还没有手机号码,赶紧添加一个!";
            }
            PhoneGrid.DataSource = dtphone;
            PhoneGrid.DataBind();
        }
    }

    protected void SOAGrid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
    {
        string ccdefine1 = e.NewValues["NewSendDate"].ToString(); //获取新值
        //if (NewSendDate=="0")
        //{
        //    NewSendDate = "null";
        //}
        string cCusCode = e.NewValues["cCusCode"].ToString();
        bool c = new BasicInfoManager().DL_SOAAutoSendByUpd(cCusCode, ccdefine1);

        e.Cancel = true;
        //重新绑定Grid
        DataTable dt = new BasicInfoManager().DL_SOAAutoSendBySel();
        SOAGrid.DataSource = dt;
        SOAGrid.DataBind();
    }

    protected void SOAGrid_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
    {
        if (e.DataColumn.FieldName.ToString() == "NewSendDate" && e.VisibleIndex >= 0)
        {
            e.Cell.Text = SOAGrid.GetRowValues(e.VisibleIndex, "SOASendTime").ToString();
        }

    }



}