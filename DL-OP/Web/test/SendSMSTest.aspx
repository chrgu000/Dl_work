<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SendSMSTest.aspx.cs" Inherits="test_SendSMSTest" %>

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
    
        <dx:ASPxTextBox ID="cuscode" runat="server" Width="170px">
        </dx:ASPxTextBox>
        <dx:ASPxButton ID="BtnSend" runat="server" OnClick="BtnSend_Click" Text="发送">
        </dx:ASPxButton>
    
    </div>
    </form>
</body>
</html>
