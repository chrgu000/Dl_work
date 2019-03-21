using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using ThoughtWorks.QRCode.Codec;
using System.Drawing;



namespace BLL
{
    public class QRCode
    {
        public string QR(string content, string MAACode)
        {
            if (content == "" || MAACode == "")
            {
                return "0";
            }
            QRCodeEncoder encoder = new QRCodeEncoder();
            encoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;  //编码方式(注意：BYTE能支持中文，ALPHA_NUMERIC扫描出来的都是数字)
            encoder.QRCodeScale = 10;    //大小(值越大生成的二维码图片像素越高)
            encoder.QRCodeVersion = 0;  //版本(注意：设置为0主要是防止编码的字符串太长时发生错误)
            encoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;  //错误效验、错误更正(有4个等级)
            

            string localFilePath = System.Environment.CurrentDirectory;

            System.Drawing.Bitmap bp = encoder.Encode(content.ToString(), Encoding.GetEncoding("GB2312"));
            Image image = bp;
            bp.Save(localFilePath + "\\QRimages\\" + MAACode + ".jpg");
            //pictureBox1.Image = bp;
            //pictureBox1.Image.Save(localFilePath + "\\" + qrdata.Replace("|","_") + ".jpg");
            //pictureBox1.Image.Save("123213.jpg");
            return localFilePath + "\\QRimages\\" + MAACode + ".jpg";

        }
    }
}


//QRCodeEncoder encoder = new QRCodeEncoder();
//           encoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;  //编码方式(注意：BYTE能支持中文，ALPHA_NUMERIC扫描出来的都是数字)
//           encoder.QRCodeScale = 4;    //大小(值越大生成的二维码图片像素越高)
//           encoder.QRCodeVersion = 0;  //版本(注意：设置为0主要是防止编码的字符串太长时发生错误)
//           encoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;  //错误效验、错误更正(有4个等级)

//           string localFilePath = System.Environment.CurrentDirectory;

//           string[] strTxt = content.Split("\r\n".ToCharArray());
//           //int i = strTxt.Length;
//           foreach (string s in strTxt)
//           {
//               //MessageBox.Show(s);

//               String qrdata = s;

//               if (s != "")
//               {
//                   System.Drawing.Bitmap bp = encoder.Encode(qrdata.ToString(), Encoding.GetEncoding("GB2312"));
//                   Image image = bp;
//                   bp.Save(localFilePath + "\\QRimages\\" + qrdata.Replace("|", "_") + ".jpg");
//                   //pictureBox1.Image = bp;
//                   //pictureBox1.Image.Save(localFilePath + "\\" + qrdata.Replace("|","_") + ".jpg");
//                   //pictureBox1.Image.Save("123213.jpg");
//               }
//           }