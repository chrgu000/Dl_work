<%@ page language="C#" autoeventwireup="true" inherits="dluser_ALLOP, dlopwebdll" enableviewstate="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Label ID="Label1" runat="server" Text="订单号"></asp:Label>
    
        <asp:TextBox ID="TxtBillNo" runat="server"></asp:TextBox>
        <asp:Button ID="BtnSHL" runat="server" Text="更新税率差额" OnClick="BtnSHL_Click" />
    
    </div>
    </form>
</body>
</html>
