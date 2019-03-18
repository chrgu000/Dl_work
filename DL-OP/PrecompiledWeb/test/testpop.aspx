<%@ page language="C#" autoeventwireup="true" inherits="test_testpop, dlopwebdll" enableviewstate="false" %>

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

            <dx:ASPxButton ID="ShowButton" runat="server" Text="Show Popup Window" AutoPostBack="False" />
    </div>
    <dx:ASPxPopupControl ID="PopupControl" runat="server" CloseAction="OuterMouseClick" LoadContentViaCallback="OnFirstShow"
                         PopupElementID="ShowButton" PopupVerticalAlign="Below" PopupHorizontalAlign="LeftSides" AllowDragging="True"
                         ShowFooter="True" Width="310px" Height="160px" HeaderText="Updatable content" ClientInstanceName="ClientPopupControl">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl" runat="server">
                <div style="vertical-align:middle">
                    The content of this popup control was updated at <%= DateTime.Now.ToLongTimeString() %><br /> <b> </b>
                </div>
                                <dx:ASPxButton ID="UpdateButton" runat="server" Text="Update Content" AutoPostBack="False" style="margin: 6px 6px 6px 210px"
                               ClientSideEvents-Click="function(s, e) { ClientPopupControl.PerformCallback(); }"/>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <FooterTemplate>

        </FooterTemplate>
    </dx:ASPxPopupControl>
    </div>
    </form>
</body>
</html>
