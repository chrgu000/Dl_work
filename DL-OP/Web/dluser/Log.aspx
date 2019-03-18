<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Log.aspx.cs" Inherits="dluser_Log" %>

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
        <asp:Button ID="Button2" runat="server" Text="Button" OnClick="Button2_Click" />
        <asp:Button ID="Button1" runat="server" Text="刷新API日志" OnClick="Button1_Click" />
        <dx:ASPxGridView ID="DVGLog" runat="server" AutoGenerateColumns="False">
            <Columns>
                <dx:GridViewDataTextColumn Caption="编号" FieldName="strBillNo" VisibleIndex="0" Width="120px">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="内容" FieldName="Err" VisibleIndex="1" Width="600px">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="时间" FieldName="datdate" VisibleIndex="2" Width="180px">
                </dx:GridViewDataTextColumn>
            </Columns>
        </dx:ASPxGridView>
    </div>
    </form>
</body>
</html>
