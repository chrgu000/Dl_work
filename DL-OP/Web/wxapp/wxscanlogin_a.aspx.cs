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


public partial class wxapp_wxscanlogin_a : System.Web.UI.Page
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
        //DataTable dtToekn = new BasicInfoManager().DL_GetWXTokenBySel();
        //access_token = dtToekn.Rows[0][0].ToString();

        QYH9003.WeiXinSoapClient qyh=new QYH9003.WeiXinSoapClient();
        access_token = qyh.Get_access_token();
        //access_token = "YSQx_Exxk7PfI3jJuYSe-tQv1SGnPLu-Ew7dS1liOBMOQ1rK0eh1bj1BB56VfpoD5sAId1kA8-2tt4BaMJpAGCzpkg2f6bkCcthyrZ351CAfbyjsaxupNGiWBamVlY-zRnNJB8Tf_tOqQvjjJiTol4gWvsnrkDfcFKGN5ukKthZPL13VlirNfB7ENC-uimbtXaVcYgGbqBPm9opfGNT9XpVhhEefvz6YW-VDysz2dzj65KctaIhmKp1fTZuk9WiQvTd2sAxHvVimLf49BzMRTsb_8CfiajboK70MF0tXEEw";
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
        if (string.IsNullOrEmpty(respDic["UserId"].ToString()) )
        {
            Label1.Text = respDic.ToString();
            return;
        }
        else
        {
            UserId = respDic["UserId"].ToString();
        }
        //Label1.Text = "你当前的企业应用账户为：" + UserId;
        //Label1.Text = respText;
        #endregion

        #region 根据userid获取对应绑定的账户信息
        DataTable dt = new BasicInfoManager().DL_GetWXOpBindInfoBySel(UserId);
        string strConstcCusCode = "没有对应的网上订单账户！";
        if (dt.Rows.Count > 0)
        {
            #region 更新表QrCodeLogin的flag标签
            bool c = new BasicInfoManager().Update_QrCodeLogin(Request.QueryString["state"].ToString(), "1",dt.Rows[0]["strOpUserCode"].ToString());
            if (c)
            {
                Label1.Text = "登录成功!";

            }
            else
            {
                Label1.Text = "登录失败!";
            }
            #endregion
        }
        else
        {
            Label1.Text = "请先开通网上订单账户！";
        }
        string rel = "您没有评价权限！";
        if (UserId == "15308078836")
        {
            rel = "可以提交评价！";
        }
        //Label1.Text = "你当前绑定的网上订单账户：" + strConstcCusCode + ";" + rel;
        #endregion

        
    }
}