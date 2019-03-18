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
using System.Configuration;

public partial class test_shiwu : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        //SqlConnection sqlConnection = new SqlConnection();
        //string connStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        //sqlConnection = new SqlConnection(connStr);
        //System.Data.SqlClient.SqlCommand cm = new System.Data.SqlClient.SqlCommand();
        //cm.Connection = sqlConnection;
        //sqlConnection.Open();
        //System.Data.SqlClient.SqlTransaction trans = sqlConnection.BeginTransaction();

        System.Data.SqlClient.SqlConnection cnn = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString);
        System.Data.SqlClient.SqlCommand cm = new System.Data.SqlClient.SqlCommand();
        cm.Connection = cnn;
        cnn.Open();
        System.Data.SqlClient.SqlTransaction trans = cnn.BeginTransaction(); 

        try
        {
            cm.Transaction = trans;
            cm.CommandText = "DLproc_NewArrearByIns";

            //cm.Parameters.Add("@amount", SqlDbType.Int);
            //cm.Parameters["@amount"].Value = Convert.ToInt32(dr["amount"]);
            //cm.Parameters.Add("@productID", SqlDbType.VarChar);
            //cm.Parameters["@productID"].Value = dr["productID"].ToString();
            cm.ExecuteNonQuery();
            cm.CommandText = "DLproc_NewArrearByIns_test";
            cm.ExecuteNonQuery();
            //cm.ExecuteScalar

            trans.Commit();
        }
        catch
        {
            trans.Rollback();
        }
        finally
        {
            cnn.Close();
            trans.Dispose();
            cnn.Dispose();
        }
    }
}