<%@ Page Language="C#" AutoEventWireup="true" CodeFile="testgridmod.aspx.cs" Inherits="test_testgridmod" %>

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
 
        <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" KeyFieldName="strUserName">
            <Columns>
                <dx:GridViewDataHyperLinkColumn VisibleIndex="0" Caption="link" FieldName="strUserName">
 
                    <PropertiesHyperLinkEdit NavigateUrlFormatString="testgridmod.aspx?id={0}" Target="_self" Text="xxxxx">
                    </PropertiesHyperLinkEdit>
 
                </dx:GridViewDataHyperLinkColumn >
                <dx:GridViewDataTextColumn FieldName="strUserName" VisibleIndex="1">
                </dx:GridViewDataTextColumn>
            </Columns>
        </dx:ASPxGridView>
        <br />
 
        <dx:ASPxButton ID="ASPxButton1" runat="server" OnClick="ASPxButton1_Click" Text="ASPxButton">
        </dx:ASPxButton>
 
    </div>
    </form>
</body>
</html>
