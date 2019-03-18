<%@ Page Language="C#" AutoEventWireup="true" CodeFile="warning.aspx.cs" Inherits="test_warning" %>

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

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
        <asp:Timer ID="Timer1" runat="server" Interval="5000" OnTick="Timer1_Tick">
        </asp:Timer>
                <asp:Label ID="xx" runat="server" Text="Label"></asp:Label>

                <br />
                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" />

            </ContentTemplate>
        </asp:UpdatePanel>
    
    </div>
    </form>
</body>
</html>
