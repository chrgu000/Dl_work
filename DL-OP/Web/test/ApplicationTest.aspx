<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ApplicationTest.aspx.cs" Inherits="test_ApplicationTest" %>

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
    
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    
        <dx:ASPxButton ID="ASPxButton1" runat="server" OnClick="ASPxButton1_Click" Text="ASPxButton">
        </dx:ASPxButton>
        <br />
        <dx:ASPxGridView ID="ASPxGridView1" runat="server">
        </dx:ASPxGridView>
     <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
       
        <br />

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
    <div>        <asp:Label ID="xx" runat="server" Text="Label"></asp:Label>    </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    
        <asp:Timer ID="Timer1" runat="server" Interval="1000" OnTick="Timer1_Tick">
        </asp:Timer>
    
        <br />


    </div>
    </form>
</body>
</html>
