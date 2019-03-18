using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using BLL;

public partial class wxapp_MM : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        #region 获取微信userid
        //获取userid
        string getAccessTokenUrl = "https://qyapi.weixin.qq.com/cgi-bin/user/getuserinfo?access_token={0}&code={1}";
        string UserId = "";
        string respText = "";
        //获取josn数据
        //获取accesstoken
        string access_token = "";
        //if (Application["WXAccessToken"] != null)
        //{
        //    string[] asx = Application["WXAccessToken"].ToString().Split(',');
        //    access_token = asx[0].ToString();
        //}
        DataTable dtToekn = new BasicInfoManager().DL_GetWXTokenBySel();
        //access_token = dtToekn.Rows[0][0].ToString();
        access_token = "dZTkFdm8YCYCtfzk34egS05F5_zRGS_omOQv_P8RcX_EBO3o30WtngaVb7aYAbkQ1b1BQPF5KnH9dHrinYZamwnnC2Imrq14a1WuABxVRnkiguJcDejIVS7BK_6Qxc3mMOuX8iNJTYmrwF16vbIOZuS91rD3dwzrVYZlX6ExcMW2qgAr4duJ2D4vCfYWu_jIbOeRX-9WjRQ3n4-j1z1MnzMG5drJGVX5XJHafTanRCfES2PxHRRycOQP_M3KSTKWAzLPBGTfcHr3tOnOZdGGZ-o7VpzUHiiFHFEb4k5LvtM";

        string code = Request.QueryString["code"].ToString();
        string url = string.Format(getAccessTokenUrl, access_token, code);
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        using (Stream resStream = response.GetResponseStream())
        {
            StreamReader reader = new StreamReader(resStream, Encoding.Default);
            respText = reader.ReadToEnd();
            resStream.Close();
        }
        JavaScriptSerializer Jss = new JavaScriptSerializer();
        Dictionary<string, object> respDic = (Dictionary<string, object>)Jss.DeserializeObject(respText);
        //通过键UserId获取值
        UserId = respDic["UserId"].ToString();
        Label2.Text = "你当前的企业应用账户为：" + UserId;
        #endregion

        #region 根据userid获取对应绑定的账户信息
        DataTable dt = new BasicInfoManager().DL_GetWXOpBindInfoBySel(UserId);
        string strConstcCusCode = "没有对应的网上订单账户！";
        if (dt.Rows.Count>0)
        {
            strConstcCusCode = dt.Rows[0]["strOpUserCode"].ToString();         
        }
        string rel = "您没有评价权限！";
        if (UserId == "15308078836")
        {
            rel = "可以提交评价！";
        }
        Label1.Text = "你当前绑定的网上订单账户：" + strConstcCusCode + ";" + rel;
        #endregion

    }
}