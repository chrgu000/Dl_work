<%@ Page Language="C#" AutoEventWireup="true" CodeFile="testhyperlink.aspx.cs" Inherits="test_testhyperlink" %>

<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
          <style type="text/css">
hp a:link {color: #FF0000}	/* 未访问的链接 */
hp a:visited {color: #00FF00}	/* 已访问的链接 */
hp a:hover {color: #FF00FF}	/* 鼠标移动到链接上 */
hp a:active {color: #0000FF}	/* 选定的链接 */
          </style>
</head>

<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                
                
                <dx:ASPxGridView ID="TreeDetail" runat="server" AutoGenerateColumns="False">
                    <Columns>
                        <dx:GridViewDataTextColumn FieldName="cInvCode" VisibleIndex="0">
                            <DataItemTemplate>
                                <dx:ASPxHyperLink ID="gridhp" ClientInstanceName="gridhp" CssClass="hp" runat="server" NavigateUrl='<%# "news.aspx?ca_id="+Eval("cInvCode") %>' Text="添加" Target="_blank"  >
                                                    <ClientSideEvents Click="function(s, e) {
e.htmlElement.innerHTML='已添加';
}" />
                </dx:ASPxHyperLink>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="cInvName" VisibleIndex="1">
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <SettingsPager Visible="False">
                    </SettingsPager>
                </dx:ASPxGridView>
                
                
                <br />
                <dx:ASPxHyperLink ID="texthp" ClientInstanceName="texthp" runat="server" Text="测试" NavigateUrl="new.aspx"  >
                    <ClientSideEvents Click="function(s, e) {
alert('11111111111111111111')	
}" />
                </dx:ASPxHyperLink>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:UFDATA_001_2015ConnectionString %>" SelectCommand="SELECT [strLoginName], [strUserName] FROM [Dl_opUser]"></asp:SqlDataSource>
            </ContentTemplate>
        </asp:UpdatePanel>
    
    </div>
    </form>
</body>
</html>
<script type="text/javascript">
    var xxxcode = function (s, e) {
        alert('okkkkkkkk');
        }
</script>