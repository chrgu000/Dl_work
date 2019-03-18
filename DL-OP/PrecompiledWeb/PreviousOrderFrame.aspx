<%@ page language="C#" autoeventwireup="true" inherits="PreviousOrderFrame, dlopwebdll" enableviewstate="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>历史订单查询框架</title>
</head>
<body style="width: 100%; height: 100%">
    <form id="form1" runat="server">
        <div>
            <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="*%" id="frmTitle" nowrap="noWrap" name="fmTitle" valign="top" height="310">
                        <iframe id="OrderLeft" height="100%" width="100%" frameborder="0" src="PreviousOrder.aspx"
                            name="OrderLeft"></iframe>
                    </td>
                </tr>
                <tr>
                  <td valign="top" width="*%" height="600">
                        <iframe id="OrderCenter" height="100%" width="100%" frameborder="0" name="OrderCenter" src="PreviousOrderDetail.aspx"></iframe>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
