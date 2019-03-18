using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Configuration;
using System.IO;
using System.Text;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Services;

namespace DingDan_WebForm.test
{
    public partial class xml : System.Web.UI.Page
    {
        public static string thisTime="";
        protected void Page_Load(object sender, EventArgs e)
        {
            //string configPath = "test.config";
            //string path = System.AppDomain.CurrentDomain.BaseDirectory;
            //ExeConfigurationFileMap map = new ExeConfigurationFileMap();
            //map.ExeConfigFilename =path+ configPath;
            //Configuration config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            //config.AppSettings.Settings["isCode"].Value = "123123";
            //config.Sections.Get(0);
            //config.Save(ConfigurationSaveMode.Modified);
            //ConfigurationManager.RefreshSection("appsettings");

            //string configPath = "test/test.xml";
            //string systemPath = System.AppDomain.CurrentDomain.BaseDirectory;
            //string path = systemPath + configPath;
            //XmlDocument doc = new XmlDocument();
            //doc.Load(path);
             
            //string loginname = Session["strLoginName"].ToString();

            //DataSet ds = new DataSet();
            //ds.ReadXml(path);
            
            //DataView dv = ds.Tables["group"].DefaultView;
            //dv.RowFilter = "groupid=2";
            //Response.Write(dv);

            
            //    Response.Write("没有权限！");
            //    return;
         

            //dv = ds.Tables["group"].DefaultView;
            //string a="3";
            //dv.RowFilter = "groupid=" + a;
            //Response.Write(dv.ToTable().Rows[0]["methodids"]);
            
            //string action = "ConfirmArrear";

            //XmlNodeList method_nodes = doc.SelectNodes("//method");
            //string methodid = string.Empty;
            //foreach (XmlNode item in method_nodes)
            //{
            //    if (item.InnerText==action)
            //    {
            //        methodid = item.Attributes["methodid"].Value;
            //    }
            //}

            //if (string.IsNullOrEmpty(methodid))
            //{
            //    Response.Write("错误的方法！");
            //    return;
            //}


         
             
        
        }

        [WebMethod]
        public static string GetName() {
            string code = HttpContext.Current.Request.QueryString["code"];
            return code;
        }

        public string abc()
        {
            var time = DateTime.Now.ToString();
            //Response.Write("<script>alert(time  )</script>");
            if (aa.Text=="")
            {
                return "nullll";
            }
          return HiddenField1.Value;

        }

        protected void btn_Click(object sender, EventArgs e)
        {
            aa.Text = DateTime.Now.ToString();
        }

         
 
    }
}