using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Data;
using System.Data.SqlClient;

public partial class dluser_UpdateCusCodeClass : System.Web.UI.Page
{
    //public System.Web.UI.WebControls.Button BtnUpdateAllCustomerClass;

    protected void Page_Load(object sender, EventArgs e)
    {
        //BtnUpdateAllCustomerClass.Attributes.Add("onclick", "return confirm('确定要删吗?');");
        if (Session["strLoginName"].ToString() == "0109" || Session["strLoginName"].ToString() == "0960" || Session["strLoginName"].ToString() == "1303" ||  Convert.ToInt16(Session["strUserLevel"].ToString()) < 2)
        {

        }
        else
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('权限不够!');</script>");
            Response.Redirect("../dluser/Default.aspx");
        }
    }
    protected void BtnUpdateAllCustomerClass_Click(object sender, EventArgs e)
    {
        //更新所有顾客的允限销分类表
        bool c = new OrderManager().DL_CodeClassByUp();
        Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('更新允限销分类表成功!');</script>");
    }
    protected void BtnUpdateSinCustomerClass_Click(object sender, EventArgs e)
    {
        //更新单个顾客允限销分类表
        string ccuscode = TxtCusClassText.Text.Trim().ToString();
        bool c = new OrderManager().DL_CusCodeClassByUp(ccuscode);
        Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('更新所有允限销分类表成功!');</script>");
    }
    protected void BtnCusAdd_Click(object sender, EventArgs e)
    {
        //增加顾客登录帐号
        //判断是否该顾客,通过获取顾客抬头来判断
        DataTable dt = new SearchManager().DL_IsExistCustomerBySel(TxtCusAdd.Text.Trim().ToString());
        if (dt.Rows.Count > 0)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('已经存在该用户!');</script>");
            //return;
        }
        else
        {
            //新增用户
            bool c = new BasicInfoManager().DL_AddNewCustomerByIns(TxtCusAdd.Text.Trim().ToString());
            if (c)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('新用户增加成功!');</script>");
                //return;
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('增加失败,请联系管理员!');</script>");
                //return;
            }
        }
    }

}