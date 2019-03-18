using DAL;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using BLL;
using System.Drawing;
using System.IO;
using System.Net;


public partial class VoteYanZhengMa : System.Web.UI.Page
{
    private readonly XmlDocument doc;
    private const string ApiKey = "f35e56921091f0c58dlpkIXooHbC9JTHCFrPtNUWMdUqVLUtNzo86dCX6cE2MrhynDnXDWKMtAE7Kd5w";

    protected void Page_Load(object sender, EventArgs e)
    {
        this.txturl.Text = "";
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        #region 读取文件名，生成链接，识别验证码后，并清空文件
        string path = @"D:/test/yzm";
        DirectoryInfo dinfo = new DirectoryInfo(path);
        string[] temp = null;
        string dd = "";
        if (dinfo.Exists)
        {
            FileInfo[] finfo = dinfo.GetFiles();
            temp = new string[finfo.Length];
            for (int i = 0; i < finfo.Length; i++)
            {
                temp[i] = finfo[i].Name;
                dd = finfo[i].Name;
            }
        }
        #endregion

        #region 识别验证码
        var ocrKing = new OcrKing(ApiKey)
        {
            Language = Language.Eng,
            Service = Service.OcrKingForNumber,
            Charset = Charset.Digit,
            //FileUrl = this.tbNetFile.Text.Trim()
            FileUrl = "http://dl.duolian.com:8003/yzm/" + dd.ToString()
        };
        string x = "http://dl.duolian.com:8003/yzm/" + dd.ToString();
        txturl.Text = x;
        // 网络文件识别时FileUrl传图片url  此时type可以省略
        // 服务端根据url进行匹配
        ocrKing.DoService();
        this.ParseResult(ocrKing.OcrResult, ocrKing.ProcessStatus);
        #endregion



    }

    private void ParseResult(string result, bool processStats)
    {
        if (processStats)
        {
            // 解析结果
            //this.doc.LoadXml(result);

            // 识别结果
            //this.tbResult.Text = this.doc.SelectSingleNode("//Results/ResultList/Item/Result").InnerText;
            this.tbResult.Text = result.Substring(result.IndexOf("</Result>\r\n") - 4, 4).ToString();
            hfyzm.Value = result.Substring(result.IndexOf("</Result>\r\n") - 4, 4).ToString();
            // 原始图片
            //this.pbSrcFile.Image =
            //    GetBitmap(this.doc.SelectSingleNode("//Results/ResultList/Item/SrcFile").InnerText);

            // 处理后图片
            //this.pbDesFile.Image =
            //    GetBitmap(this.doc.SelectSingleNode("//Results/ResultList/Item/DesFile").InnerText);
        }
        else
        {
            // 识别结果
            this.tbResult.Text = "未处理";
            hfyzm.Value = "未处理";
        }

        //this.txturl.Text = "";
        #region 读取文件名，生成链接，识别验证码后，并清空文件
        string path = @"D:/test/yzm";
        DirectoryInfo dinfo = new DirectoryInfo(path);
        string[] temp = null;
        if (dinfo.Exists)
        {
            FileInfo[] finfo = dinfo.GetFiles();
            temp = new string[finfo.Length];
            foreach (FileInfo f in finfo)
            {
                File.Delete(f.FullName);
            }

        }
        #endregion
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        this.txturl.Text = "";
        #region 读取文件名，生成链接，识别验证码后，并清空文件
        string path = @"D:/test/yzm";
        DirectoryInfo dinfo = new DirectoryInfo(path);
        string[] temp = null;
        if (dinfo.Exists)
        {
            FileInfo[] finfo = dinfo.GetFiles();
            temp = new string[finfo.Length];
            foreach (FileInfo f in finfo)
            {
                File.Delete(f.FullName);
            }

        }
        #endregion

    }
}