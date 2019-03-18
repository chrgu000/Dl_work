<%@ page language="C#" autoeventwireup="true" inherits="Blank, dlopwebdll" enableviewstate="false" %>

<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
        <script class="jsbin" src="../js/jquery-1.7.2.min.js"></script>
    <link rel="stylesheet" type="text/css" href="../js/themes/default/easyui.css">
    <script type="text/javascript" src="../js/jquery.easyui.min.js"></script>
    <link rel="stylesheet" type="text/css" href="../js/themes/icon.css" />
    <style type="text/css">

.dxbButtonSys /*Bootstrap correction*/
{
    -webkit-box-sizing: content-box;
    -moz-box-sizing: content-box;
    box-sizing: content-box;
}
.dxbButtonSys
{
	cursor: pointer;
	display: inline-block;
	text-align: center;
	white-space: nowrap;
}

.dx-vam, .dx-valm { vertical-align: middle; }
.dx-vam, .dx-vat, .dx-vab { display: inline-block!important; }
        .auto-style1 {
            line-height: 100%;
            text-decoration: inherit;
            padding: 2px 0;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <span><script charset="utf-8" type="text/javascript" src="http://wpa.b.qq.com/cgi/wpa.php?key=XzkzODAyMTM0OV8zNTY4OTdfNDAwODc4NjMzM18"></script></span>
        该功能正在建设!<br />
<%--            <dx:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="False" Text="ASPxButton">
                <ClientSideEvents Click="function(s, e) {
	addTab('购物','icon-dl_gwc','../OrderFrame.aspx','../OrderFrame.aspx');
}" />
            </dx:ASPxButton>--%>
        </div>

    </form>
</body>
    <script type="text/javascript">
        function addTab(text, icon, url, urll) {
            if ($('#tabs').tabs('exists', text)) {
                $('#tabs').tabs('select', text);
            } else {
                var content = '<iframe scrolling="auto" frameborder="0"  src="' + url + '" style="width:100%;height:100%;"></iframe>';
                $('#tabs').tabs('add', {
                    title: text,
                    iconCls: icon,
                    closable: true,
                    content: content
                });
            }
        }
    </script>
</html>
