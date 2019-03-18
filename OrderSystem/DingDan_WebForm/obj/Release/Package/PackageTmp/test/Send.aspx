<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Send.aspx.cs" Inherits="DingDan_WebForm.test.Send" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        .auto-style1 {
            height: 20px;
        }
        .auto-style2 {
            width: 540px;
        }
        .auto-style3 {
            height: 20px;
            width: 540px;
        }
        .auto-style4 {
            width: 344px;
        }
        .auto-style5 {
            height: 20px;
            width: 344px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <table style="width:100%;">
                        <tr>
                <td class="auto-style4">接口地址：</td>
                <td class="auto-style2">
                    <asp:TextBox ID="txt_url" runat="server">http://112.35.1.155:1992/sms/norsubmit</asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style4">企业名称：</td>
                <td class="auto-style2">
                    <asp:TextBox ID="txt_ecname" runat="server">四川多联实业有限公司</asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style4">用户名：</td>
                <td class="auto-style2">
                    <asp:TextBox ID="txt_username" runat="server">duo</asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style5">密码：</td>
                <td class="auto-style3">
                    <asp:TextBox ID="txt_password" runat="server">duolian</asp:TextBox>
                </td>
                <td class="auto-style1"></td>
            </tr>
            <tr>
                <td class="auto-style4">签名编码：</td>
                <td class="auto-style2">
                    <asp:TextBox ID="txt_sgin" runat="server">eb2Lji2Uz</asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
                 <tr>
                <td class="auto-style4">扩展码</td>
                <td class="auto-style2">
                    <asp:TextBox ID="txt_addSerial" runat="server"></asp:TextBox>
                     </td>
                <td>&nbsp;</td>
            </tr>
                             <tr>
                <td class="auto-style4">发送号码</td>
                <td class="auto-style2">
                    <asp:TextBox ID="txt_mobiles" runat="server">13438904933</asp:TextBox>
                     </td>
                <td>&nbsp;</td>
            </tr>
                        </tr>
                 <tr>
                <td class="auto-style4">短信内容</td>
                <td class="auto-style2" colspan ="2">
                    <asp:TextBox ID="txt_content" runat="server" Height="103px" TextMode="MultiLine" Width="1067px">今天天气不错哟</asp:TextBox>
                     </td>
            </tr>
                        </tr>
                 <tr>
                <td class="auto-style4">&nbsp;</td>
                <td class="auto-style2">
                    <asp:Button ID="btn_send" runat="server" Text="发送" Height="30px" Width="89px" OnClick="btn_send_Click" />
                     </td>
                <td>&nbsp;</td>
                                 </tr>
                        </tr>
                 <tr>
                <td class="auto-style4">返回值</td>
                <td class="auto-style2 "  colspan ="2">
                    <asp:Label ID="lbl_result" runat="server" Height="50px" Width="100%"></asp:Label>
                     </td>

            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
