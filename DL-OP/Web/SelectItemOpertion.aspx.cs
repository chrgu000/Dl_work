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

public partial class SelectItemOpertion : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["code"] != null)
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
            
                string acode = Request.QueryString["code"].ToString();

                //判断是否已经在gridselect(选择项目)中存在
                if (Session["gridselect"] != null)
                {
                    DataTable dtgridselect = (DataTable)Session["gridselect"];  //获取选中行的值,保存 
                    for (int i = 0; i < dtgridselect.Rows.Count; i++)
                    {
                        if (acode == dtgridselect.Rows[i]["cInvCode"].ToString())
                        {
                            return;
                        }
                    }
                }

                //判断是否已经在ordergrid(订单明细表)中存在
                if (Session["ordergrid"]!=null)
                {
                    DataTable dtordergrid = (DataTable)Session["ordergrid"];  //获取选中行的值,保存 
                    for (int i = 0; i < dtordergrid.Rows.Count; i++)
                    {
                        if (acode == dtordergrid.Rows[i]["cInvCode"].ToString())
                        {
                            return;
                        }
                    }
                }

                //add data
                DataTable dtst = (DataTable)Session["gridselect"];  //获取选中行的值,保存
                dtst.Rows.Add(new object[] { acode });
                Session["gridselect"] = dtst;
            }
        }
    }


}