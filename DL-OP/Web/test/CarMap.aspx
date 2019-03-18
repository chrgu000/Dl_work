<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CarMap.aspx.cs" Inherits="test_CarMap" %>

<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
            <link href="../js/themes/icon.css" rel="stylesheet" />
    <link href="../js/themes/metro/easyui.css" rel="stylesheet" />
    <link href="../js/themes/mobile.css" rel="stylesheet" />
    <script src="../js/jquery.min.js"></script>
    <script src="../js/jquery.easyui.min.js"></script>
    <script src="../js/jquery.easyui.mobile.js"></script>
    <script src="../js/easyui-lang-zh_CN.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        订单号：<dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="ASPxLabel">
        </dx:ASPxLabel>
        <br />
        状态：<dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="ASPxLabel">
        </dx:ASPxLabel>
        <br />
        <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False">
            <Columns>
                <dx:GridViewDataTextColumn Caption="时间" FieldName="datDatetime" VisibleIndex="0" Width="140px">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataHyperLinkColumn Caption="查看" FieldName="lbs" VisibleIndex="2" Width="80px">
                    
                    <PropertiesHyperLinkEdit NavigateUrlFormatString="../CarMapView.aspx?id={0}" Target="_blank" Text="点击查看">
                    </PropertiesHyperLinkEdit>
                </dx:GridViewDataHyperLinkColumn>
                <dx:GridViewDataTextColumn Caption="位置" FieldName="local" VisibleIndex="1" Width="350px">
                </dx:GridViewDataTextColumn>
            </Columns>
            <SettingsPager Visible="False">
            </SettingsPager>
            <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
        </dx:ASPxGridView>
    </form>
</body>
</html>
