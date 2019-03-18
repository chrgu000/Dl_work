<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MM.aspx.cs" Inherits="wxapp_MM" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" name="viewport" content="width=device-width;initial-scale=1.0;maximum-scale=2.0;minimum-scale=1.0;user-scalable=no;target-densitydpi=device-dpi" />
    <title></title>
    <script type="text/javascript">
        $(window).bind('resize load', function () {
            $("body").css("zoom", $(window).width() / 640);
            $("body").css("display", "block");
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="width: 640px">
            <asp:Label ID="Label2" runat="server" Text="企业应用帐号"></asp:Label></br>
            <asp:Label ID="Label1" runat="server" Text="当前无绑定帐号!请联系关联员进行绑定"></asp:Label>
 
        </div>
    </form>
</body>
</html>
