using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using BLL;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

public partial class test_cuscode : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //通过手机号,密码,查询出对应的账户,然后在查询出该操作账户设置的客户代码信息
        string phone = Request.QueryString["phone"].ToString();
        string pwd = Request.QueryString["pwd"].ToString();
        string username=Request.QueryString["username"].ToString();
        pwd = FormsAuthentication.HashPasswordForStoringInConfigFile(pwd.Trim(), "MD5");// 把密码转为MD5码的形式
        DataTable login = new LoginManager().SubLogin(username,phone,pwd);
        if (login.Rows.Count>0)
        {
            logincheck.Text = "ok";
            DataTable dt = new SearchManager().DL_CustomerCodeBySel(phone, pwd);
            jsondata.Text = json(dt);
        }
    
        //GV1.DataSource = dt;
        //GV1.DataBind();


        //定义一个JSON字符串   
        //string jsonText = "[{'a':'aaa','b':'bbb','c':'ccc'},{'a':'aaa2','b':'bbb2','c':'ccc2'}]";
        string jsonText = jsondata.Text;
        //反序列化JSON字符串  
        JArray ja = (JArray)JsonConvert.DeserializeObject(jsonText);
        //将反序列化的JSON字符串转换成对象  
        JObject o = (JObject)ja[0];
        //读取对象中的各项值   
        //Response.Write(o["cInvCode"]+";");
        //Response.Write(ja[2]["cInvCode"]);

    }

    public string json(DataTable dt)
    {
        string result = "";
        result = JsonConvert.SerializeObject(dt, new DataTableConverter());
        return result;
    }
}