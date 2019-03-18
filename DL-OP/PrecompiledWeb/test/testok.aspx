<%@ page language="C#" autoeventwireup="true" inherits="test_testok, dlopwebdll" enableviewstate="false" %>

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
<SPAN>div>
<SPAN><asp:TextBox ID="TextBox1" runat="server" Text="双实线边框" BorderStyle="Double" BorderWidth="3"/></SPAN>
<SPAN><asp:TextBox ID="TextBox2" runat="server" Text="凹槽状边框" BorderStyle="Groove" BorderWidth="3"/></SPAN>
<SPAN><asp:TextBox ID="TextBox3" runat="server" Text="突起边框" BorderStyle="Ridge" BorderWidth="3"/></SPAN>
<SPAN><asp:TextBox ID="TextBox4" runat="server" Text="内嵌边框" BorderStyle="Inset" BorderWidth="3"/></SPAN>
<SPAN><asp:TextBox ID="TextBox5" runat="server" Text="外嵌边框" BorderStyle="Outset" BorderWidth="3"/></SPAN>
<SPAN><asp:TextBox ID="TextBox6" runat="server" Text="空样式边框"/></SPAN>
<SPAN><asp:TextBox ID="TextBox7" runat="server" Text="无样式边框" BorderStyle="None" BorderWidth="3"/></SPAN>
<SPAN><asp:TextBox ID="TextBox8" runat="server" Text="虚线边框" BorderStyle="Dotted" BorderWidth="3"/></SPAN>
<SPAN><asp:TextBox ID="TextBox9" runat="server" Text="点划线边框" BorderStyle="Dashed" BorderWidth="3"/></SPAN>
<SPAN><asp:TextBox ID="TextBox10" runat="server" Text="实线边框" BorderStyle="Solid" BorderWidth="3"/></SPAN>
<SPAN><asp:TextBox ID="TextBox11" runat="server" Text="边框颜色" BorderColor="Blue" BorderWidth="3"/></SPAN>
<SPAN><asp:TextBox ID="TextBox12" runat="server" Text="边框宽度" BorderColor="Red" BorderWidth="8" ForeColor="Red"/></SPAN>
</SPAN>div>
    </div>
        <dx:ASPxTextBox ID="ASPxTextBox1" runat="server" Width="170px" Border-BorderStyle="None" BorderBottom-BorderWidth="3" BorderBottom-BorderStyle="Solid" BorderBottom-BorderColor="Orange" Text="wubiankuang">
            <Border BorderStyle="None" />
        </dx:ASPxTextBox>
    </form>
</body>
</html>
