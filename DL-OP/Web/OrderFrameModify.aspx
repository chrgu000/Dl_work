<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrderFrameModify.aspx.cs" Inherits="OrderFrameModify" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>订单处理框架</title>
</head>
<script type="text/javascript">
    function switchSysBar() {
        var ssrc = document.all("frmTitle").style.display;
        if (ssrc == "none") {
            document.all("frmTitle").style.display = "";
            var aa = document.getElementById("switchPoint");
            aa.innerHTML = "<label style='cursor:pointer;color: #66FF66;' onclick='switchSysBar()'>←收起</label>";
        }
        else {
            document.all("frmTitle").style.display = "none"
            var aa = document.getElementById("switchPoint");
            aa.innerHTML = "<label style='cursor:pointer;color: #66FF66;' onclick='switchSysBar()'>→展开</label>";
        }
    }

</script>
<body>
    <form id="form1" runat="server">
        <div>
            <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
<%--                <tr>
                    <td colspan="3" style="height: 30px;">
                        <dx:ASPxMenu ID="ASPxMenu1" runat="server" EnableTheming="True" OnItemClick="ASPxMenu1_ItemClick" Theme="Office2010Blue">
                            <Items>
                                <dx:MenuItem NavigateUrl="~/OrderModifyMaterial.aspx" Target="OrderLeft" Text="刷新档案">
                                </dx:MenuItem>
                            </Items>
                        </dx:ASPxMenu>
                    </td>
                </tr>--%>
                <tr>
<%--                    <td width="350" id="frmTitle" nowrap="noWrap" name="fmTitle" valign="top" height="600">
                        <iframe id="OrderLeft" height="100%" width="100%" frameborder="0" src="OrderModifyMaterial.aspx"
                            name="OrderLeft"></iframe>
                    </td>
                    <td width="6" style="width: 6px; background: #35527E" valign="middle"
                        height="500">
                        <span id="switchPoint" title="关闭/打开侧边栏">
                            <label onclick="switchSysBar()" style="cursor: pointer; color: #66FF66;">←收起</label>
                        </span>
                    </td>--%>
                    <td valign="top" width="*%" height="600">
                        <iframe id="OrderCenter" height="100%" width="100%" frameborder="0" name="OrderCenter" src="OrderModify.aspx"></iframe>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
