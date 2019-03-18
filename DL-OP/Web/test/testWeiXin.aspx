<%@ Page Language="C#" AutoEventWireup="true" CodeFile="testWeiXin.aspx.cs" Inherits="test_testWeiXin" %>

<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        &nbsp;微信账号<dx:ASPxTextBox ID="ASPxTextBox7" runat="server" Width="170px">
        </dx:ASPxTextBox>
        应用ID<dx:ASPxTextBox ID="ASPxTextBox12" runat="server" Width="170px">
        </dx:ASPxTextBox>
        消息内容<dx:ASPxTextBox ID="ASPxTextBox8" runat="server" Width="170px">
        </dx:ASPxTextBox>
        <br />
    
        <dx:ASPxButton ID="ASPxButton1" runat="server" OnClick="ASPxButton1_Click" Text="发送微信消息">
        </dx:ASPxButton>
        <br />
        <dx:ASPxTextBox ID="ASPxTextBox1" runat="server" Width="825px" Height="24px">
        </dx:ASPxTextBox>
    
        <br />
        <br />
        <br />
        a:<dx:ASPxTextBox ID="ASPxTextBox2" runat="server" Width="170px">
        </dx:ASPxTextBox>
        b:<dx:ASPxTextBox ID="ASPxTextBox3" runat="server" Width="170px">
        </dx:ASPxTextBox>
        sum:<dx:ASPxTextBox ID="ASPxTextBox4" runat="server" Width="170px">
        </dx:ASPxTextBox>
        <br />
        <dx:ASPxButton ID="ASPxButton2" runat="server" OnClick="ASPxButton2_Click" Text="获取a+b">
        </dx:ASPxButton>
    
        <br />
        <br />
        <dx:ASPxTextBox ID="ASPxTextBox5" runat="server" Width="873px" Height="16px">
        </dx:ASPxTextBox>
        <dx:ASPxButton ID="ASPxButton3" runat="server" OnClick="ASPxButton3_Click" Text="调用webservices获取微信token1234">
        </dx:ASPxButton>
        <br />
        <dx:ASPxButton ID="ASPxButton6" runat="server" OnClick="ASPxButton6_Click" Text="调用webservices获取微信token8001">
        </dx:ASPxButton>
        <br />
        <dx:ASPxTextBox ID="ASPxTextBox6" runat="server" Width="862px" Height="16px">
        </dx:ASPxTextBox>
        <dx:ASPxTextBox ID="ASPxTextBox9" runat="server" Height="16px" Width="880px">
        </dx:ASPxTextBox>
        <br />
        <dx:ASPxButton ID="ASPxButton4" runat="server" Text="获取application9001" OnClick="ASPxButton4_Click">
        </dx:ASPxButton>
        <br />
        <asp:TextBox ID="TextBox1" runat="server" Height="45px" TextMode="MultiLine" Width="837px"></asp:TextBox>
        <br />
        <dx:ASPxTextBox ID="ASPxTextBox10" runat="server" Width="862px" Height="16px">
        </dx:ASPxTextBox>
        <dx:ASPxTextBox ID="ASPxTextBox11" runat="server" Width="862px" Height="16px">
        </dx:ASPxTextBox>
        <dx:ASPxButton ID="ASPxButton5" runat="server" Text="获取application9001" OnClick="ASPxButton5_Click">
        </dx:ASPxButton>
        <br />
        <br />
        <dx:ASPxButton ID="ASPxButton7" runat="server" Text="清除application9001" OnClick="ASPxButton7_Click" >
        </dx:ASPxButton>
        <br />
    
    </div>
    </form>
</body>
</html>
