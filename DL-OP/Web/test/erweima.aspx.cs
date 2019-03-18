using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ThoughtWorks.QRCode.Codec;

public partial class test_erweima : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
        //qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
        //qrCodeEncoder.QRCodeScale = 4;
        //qrCodeEncoder.QRCodeVersion = 7;
        //qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;

 



    }

    /// <summary>
    /// 生成二维码，如果有Logo，则在二维码中添加Logo
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    //public Bitmap CreateQRCode(string content)
    //{
    //    QRCodeEncoder qrEncoder = new QRCodeEncoder();
    //    qrEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;        //编码方式:BYTE 能支持中文，ALPHA_NUMERIC 扫描出来的都是数字
    //    qrEncoder.QRCodeScale = Convert.ToInt32(100);              //大小：值越大生成的二维码图片像素越高
    //    qrEncoder.QRCodeVersion = Convert.ToInt32(cboVersion.SelectedValue);//版本
    //    qrEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;    //错误校验、更正等级
    //    try
    //    {
    //        Bitmap qrcode = qrEncoder.Encode(content, Encoding.UTF8);
    //        if (!logoImagepath.Equals(string.Empty))
    //        {
    //            Graphics g = Graphics.FromImage(qrcode);
    //            Bitmap bitmapLogo = new Bitmap(logoImagepath);
    //            int logoSize = Convert.ToInt32(txtLogoSize.Text);
    //            bitmapLogo = new Bitmap(bitmapLogo, new System.Drawing.Size(logoSize, logoSize));
    //            PointF point = new PointF(qrcode.Width / 2 - logoSize / 2, qrcode.Height / 2 - logoSize / 2);
    //            g.DrawImage(bitmapLogo, point);
    //        }
    //        return qrcode;
    //    }
    //    catch (IndexOutOfRangeException ex)
    //    {
    //        MessageBox.Show("超出当前二维码版本的容量上限，请选择更高的二维码版本！", "系统提示");
    //        return new Bitmap(100, 100);
    //    }
    //    catch (Exception ex)
    //    {
    //        MessageBox.Show("生成二维码出错！", "系统提示");
    //        return new Bitmap(100, 100);
    //    }
    //}


}