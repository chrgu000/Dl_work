<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test空格回车table.aspx.cs" Inherits="test_test空格回车table" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <dx:ASPxTextBox ID="ASPxTextBox1" runat="server" Width="170px"></dx:ASPxTextBox>  
        <dx:ASPxMemo ID="ASPxMemo1" runat="server" Height="71px" Width="170px"></dx:ASPxMemo>
        <dx:ASPxTextBox ID="ASPxTextBox2" runat="server" Width="170px"></dx:ASPxTextBox>
        <dx:ASPxTextBox ID="ASPxTextBox3" runat="server" Width="170px"></dx:ASPxTextBox>
        <dx:ASPxButton ID="ASPxButton1" runat="server" OnClick="ASPxButton1_Click" Text="ASPxButton">
        </dx:ASPxButton>
        <br />
    </div>
    </form>
</body>
</html>
