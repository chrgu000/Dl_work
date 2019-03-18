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


public partial class wxapp_OrderManager : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            #region 获取微信userid
            //获取userid
            string getAccessTokenUrl = "https://qyapi.weixin.qq.com/cgi-bin/user/getuserinfo?access_token={0}&code={1}";
            string UserId = "";
            string respText = "";
            //获取josn数据
            //获取accesstoken
            string access_token = "";
            if (Application["WXAccessToken"] != null)
            {
                string[] asx = Application["WXAccessToken"].ToString().Split(',');
                access_token = asx[0].ToString();
            }
            //string access_token = "KKg64swan96oQsPD9dAXLUDPDJ6hWwpqTtTSpSb2NjXTtPBkfrLMeIfRojm-kqom";
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
            HFUserId.Value = UserId;
            #endregion
        }
        if (IsPostBack)
        {
            //this.searchcontBtn.Style.Add("display", "none");   //隐藏Div,显示this.DivSOA.Style.Add("display", "block");
            //this.searchcont.Style.Add("display", "none");   //隐藏Div,显示this.DivSOA.Style.Add("display", "block");
        }


        #region 根据userid获取对应绑定的账户信息
        DataTable dt = new BasicInfoManager().DL_GetWXOpBindInfoBySel(HFUserId.Value);
        string strConstcCusCode = dt.Rows[0]["strOpUserCode"].ToString();
        #endregion

        DataTable DtComboCustomer = new DataTable();
        DtComboCustomer = new SearchManager().DL_ComboCustomerBySel(strConstcCusCode + "%");
        //ComboBoxccuscode.Items.Add("全部", strConstcCusCode + "%");
        //ComboBoxccuscode.SelectedIndex = 0;
        //for (int i = 0; i < DtComboCustomer.Rows.Count; i++)
        //{
        //    ComboBoxccuscode.Items.Add(DtComboCustomer.Rows[i]["cCusName"].ToString(), DtComboCustomer.Rows[i]["cCusCode"].ToString());
        //}

        //排序
        DataTable dtNew = DtComboCustomer.Copy();  //复制dt表数据结构
        dtNew.Clear();  //清楚数据
        DataRow row;
        row = dtNew.NewRow();
        row["cCusName"] = " 全部单位";
        row["cCusCode"] = strConstcCusCode + "%";
        dtNew.Rows.Add(row);
        for (int i = 0; i < DtComboCustomer.Rows.Count; i++)
        {
            row = dtNew.NewRow();
            row["cCusName"] = DtComboCustomer.Rows[i]["cCusName"].ToString();
            row["cCusCode"] = DtComboCustomer.Rows[i]["cCusCode"].ToString();
            dtNew.Rows.Add(row);
        }
        ComboBoxccuscode.DataSource = dtNew;
        ComboBoxccuscode.DataTextField = "cCusName";
        ComboBoxccuscode.DataValueField = "cCusCode";
        ComboBoxccuscode.DataBind();

        //MyHiddenField.Value = " { \"strBillNo\": \"FI-SW-01\", \"datBillTime\": \"Koi\", \"cdefine11\": 10.00, \"isum\": \"P\", \"U8iFHMoney\": 36.50, \"U8iTHMoney\":\"Large\" },{ \"strBillNo\": \"K9-DL-01\", \"datBillTime\": \"Dalmation\", \"cdefine11\": 12.00, \"isum\": \"P\", \"U8iFHMoney\": 18.50, \"U8iTHMoney\": \"Spotted Adult Female\" }";
        //MyHiddenField.Value = " [{ \"strBillNo\": \"FI-SW-01\", \"datBillTime\": \"Koi\", \"cdefine11\": 10.00, \"isum\": \"P\", \"U8iFHMoney\": 36.50, \"U8iTHMoney\":\"Large\" }]";
        ////查询数据
        //string strBillNo = "";
        //string ccuscode = ComboBoxccuscode.DataValueField[0].ToString();
        //string begin = System.DateTime.Now.ToString("d") + " 0:00:00";
        //string end = System.DateTime.Now.AddDays(+1).ToString("d") + " 0:00:00";
        //string strshowtype = "0";
        //string strFHStatus = "1";
        //DataTable dtOrderExecute = new SearchManager().DLproc_OrderExecuteBySel(strBillNo, ccuscode, begin, end, strshowtype, strFHStatus);
        //string datagridjson = " [";
        //if (dtOrderExecute.Rows.Count > 0)
        //{
        //    for (int i = 0; i < dtOrderExecute.Rows.Count; i++)
        //    {
        //        string datagridjsonstrBillNo = "\"strBillNo" + "\":\"" + dtOrderExecute.Rows[i]["strBillNo"].ToString() + "\",";
        //        string datagridjsondatBillTime = "\"datBillTime" + "\":\"" + dtOrderExecute.Rows[i]["datBillTime"].ToString() + "\",";
        //        string datagridjsoncdefine11 = "\"cdefine11" + "\":\"" + dtOrderExecute.Rows[i]["cdefine11"].ToString() + "\",";
        //        string datagridjsonisum = "\"isum" + "\":\"" + dtOrderExecute.Rows[i]["isum"].ToString() + "\",";
        //        string datagridjsonU8iFHMoney = "\"U8iFHMoney" + "\":\"" + dtOrderExecute.Rows[i]["U8iFHMoney"].ToString() + "\",";
        //        string datagridjsonU8iTHMoney = "\"U8iTHMoney" + "\":\"" + dtOrderExecute.Rows[i]["U8iTHMoney"].ToString() + "\"";
        //        datagridjson = datagridjson + "{" + datagridjsonstrBillNo + datagridjsondatBillTime + datagridjsoncdefine11 + datagridjsonisum + datagridjsonU8iFHMoney + datagridjsonU8iTHMoney + "},";
        //    }
        //}
        //datagridjson = datagridjson.TrimEnd(',');
        //datagridjson = datagridjson + "]";
        //MyHiddenField.Value = datagridjson;
        //查询数据
        string strBillNo = "";
        //string ccuscode = ComboBoxccuscode.DataValueField[0].ToString();
        string ccuscode = HFccuscode.Value;
        //string ccuscode = Request.QueryString["id"].ToString();
        string begin = System.DateTime.Now.ToString("d") + " 0:00:00";
        string end = System.DateTime.Now.AddDays(+1).ToString("d") + " 0:00:00";
        string strshowtype = "0";
        string strFHStatus = "1";
        DataTable dtOrderExecute = new SearchManager().DLproc_OrderExecuteBySel(strBillNo, ccuscode, begin, end, strshowtype, strFHStatus,"");
        string datagridjson = " [";
        if (dtOrderExecute.Rows.Count > 0)
        {
            for (int i = 0; i < dtOrderExecute.Rows.Count; i++)
            {
                string datagridjsonstrBillNo = "\"strBillNo" + "\":\"" + dtOrderExecute.Rows[i]["strBillNo"].ToString() + "\",";
                string datagridjsondatBillTime = "\"datBillTime" + "\":\"" + dtOrderExecute.Rows[i]["datBillTime"].ToString() + "\",";
                string datagridjsoncdefine11 = "\"cdefine11" + "\":\"" + dtOrderExecute.Rows[i]["cdefine11"].ToString() + "\",";
                string datagridjsonisum = "\"isum" + "\":\"" + dtOrderExecute.Rows[i]["isum"].ToString() + "\",";
                string datagridjsonU8iFHMoney = "\"U8iFHMoney" + "\":\"" + dtOrderExecute.Rows[i]["U8iFHMoney"].ToString() + "\",";
                string datagridjsonU8iTHMoney = "\"U8iTHMoney" + "\":\"" + dtOrderExecute.Rows[i]["U8iTHMoney"].ToString() + "\"";
                datagridjson = datagridjson + "{" + datagridjsonstrBillNo + datagridjsondatBillTime + datagridjsoncdefine11 + datagridjsonisum + datagridjsonU8iFHMoney + datagridjsonU8iTHMoney + "},";
            }
        }
        datagridjson = datagridjson.TrimEnd(',');
        datagridjson = datagridjson + "]";
        MyHiddenField.Value = datagridjson;
        Label1.Text = datagridjson;
        Label2.Text = ccuscode;


    }
    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        //HFccuscode.Value = "010103";
        HFccuscode.Value = ComboBoxccuscode.Items[ComboBoxOrderStatus.SelectedIndex].Value.ToString();
        //HFccuscode.Value = ComboBoxOrderStatus.Items[ComboBoxOrderStatus.SelectedIndex].Value.ToString();
    }

}