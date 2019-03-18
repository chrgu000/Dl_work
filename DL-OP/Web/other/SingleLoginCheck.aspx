<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SingleLoginCheck.aspx.cs" Inherits="other_SingleLoginCheck" %>

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
        <asp:Timer ID="SessionTimer" runat="server" OnInit="SessionTimer_Init" OnTick="SessionTimer_Tick"></asp:Timer>
    </div>
    </form>
</body>
</html>
