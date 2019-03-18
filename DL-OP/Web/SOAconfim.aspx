<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SOAconfim.aspx.cs" Inherits="SOAconfim" %>

<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">

     
        .auto-style1 {
            font-size: x-large;
        }
        .auto-style2 {
            height: 25px;
        }

     
        .auto-style3 {
            text-align: right;
        }

     
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div runat="server" id="SOADiv">
            <table  id="TabSOA" style="font-family:'Microsoft YaHei';border:thin;border-color:orange">
                <tr>
                    <td class="auto-style1" style="text-align: center;width:610px"  >对账单</td>
                </tr>
                <tr  >
                    <td  ><dx:ASPxLabel ID="Lbcccusname" runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Size="Medium" Font-Underline="True" Text="xxx">
                        </dx:ASPxLabel>,您好:</td>
                </tr>
                <tr >
                    <td class="auto-style2" ><a style="padding-left:25px">在过去的业务合作中，感谢您对我公司的支持。经核对，截至<dx:ASPxLabel ID="Lbdate" runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Size="Medium" Font-Underline="True" Text="xxxx年xx月xx日">
                        </dx:ASPxLabel>
                        止：</td>
                </tr>
                <tr  >
                    <td ><a>您尚欠我公司货款金额￥<dx:ASPxLabel ID="Lbmoney" runat="server" Font-Size="Medium" Font-Underline="True" Text="YYYYY.YY">
                        </dx:ASPxLabel>
                        </a>元，大写<a><dx:ASPxLabel ID="Lbmoneyup" runat="server" Font-Size="Medium" Font-Underline="True" Text="YYYYY.YY">
                        </dx:ASPxLabel>
                        </a><a>。</a></td>
                    
                </tr>
                <tr><td class="auto-style3" ><a text style="text-align: right">四川多联实业有限公司</a></td></tr>
                <tr  >
                    <td ><a style="padding-left:25px"><strong>如无异议，请确认。</strong></a></td>
                </tr>
                <tr>
                    <td >
                        <dx:ASPxButton ID="BtnComf" ClientInstanceName="BtnComf" runat="server" Text="确认账单" Theme="SoftOrange" OnClick="BtnComf_Click" >
                            <ClientSideEvents Click="function(s, e) {
e.processOnServer=confirm('确定执行此操作吗?'); 	
}" />
                        </dx:ASPxButton>
                        <dx:ASPxCheckBox ID="CheckBoxOk" runat="server" Checked="True" CheckState="Checked" ClientInstanceName="CheckBoxOk" EnableTheming="True" ForeColor="Red" Height="30px" ReadOnly="True" Text="已确认" Theme="SoftOrange">
                            <ReadOnlyStyle BackColor="#FF6600">
                                <Border BorderStyle="Solid" BorderWidth="2px" />
                            </ReadOnlyStyle>
                        </dx:ASPxCheckBox>
                    </td>
                </tr>
            </table>
    </div>
    </div>
    </form>
</body>
</html>
