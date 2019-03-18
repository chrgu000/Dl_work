<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SOAFrame.aspx.cs" Inherits="SOAFrame" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>订单处理框架</title>
</head>

<body style="width: 100%; height: 100%">
    <form id="form1" runat="server">
        <div>
            <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="*%" id="frmTitle" nowrap="noWrap" name="fmTitle" valign="top" height="310">
                        <iframe id="OrderLeft" height="100%" width="100%" frameborder="0" src="SOA.aspx"
                            name="OrderLeft"></iframe>
                    </td>
                </tr>
                <tr>
                  <td valign="top" width="*%" height="650">
                        <iframe id="OrderCenter" height="100%" width="100%" frameborder="0" name="OrderCenter" src="SOAConfim.aspx"></iframe>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
