using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;

public partial class test_WapUpLoad : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int count = Request.Files.Count;
        if (count > 0)
        {
            try
            {
                HttpPostedFile File1 = Request.Files[0];
                string suffix = Path.GetExtension(File1.FileName);


                string path = "/upload/temp/" + DateTime.Now.ToString("yyyyMMddHHmmss") + suffix;


                File1.SaveAs(Server.MapPath(path));


            }
            catch (Exception ex)
            {
            }
        }
    }
}