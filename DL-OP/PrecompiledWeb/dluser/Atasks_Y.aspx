<%@ page language="C#" autoeventwireup="true" inherits="dluser_Atasks_Y, dlopwebdll" enableviewstate="false" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>全部待办工作-酬宾订单</title>
    </head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <dx:ASPxButton ID="BtnRefresh" runat="server" Text="刷新" OnClick="BtnRefresh_Click"></dx:ASPxButton>

            <dx:ASPxGridView ID="GridOrder" ClientInstanceName="GridOrder" runat="server" AutoGenerateColumns="False" 
                
                EnableTheming="True" Theme="Office2010Blue" KeyFieldName="strBillNo" Font-Size="10pt">
                <Columns>
                    <dx:GridViewDataTextColumn Caption="客户名称" FieldName="strUserName" VisibleIndex="5" Width="180px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="开票单位名称" FieldName="ccusname" VisibleIndex="6" Width="180px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataHyperLinkColumn Caption="处理订单" FieldName="strBillNo" VisibleIndex="3" Width="90px">
                        <PropertiesHyperLinkEdit NavigateUrlFormatString="Atasks_Y.aspx?gbillno={0}" Text="接收处理">
                            <Style ForeColor="#0066FF">
                            </Style>
                        </PropertiesHyperLinkEdit>
                    </dx:GridViewDataHyperLinkColumn>
                    <dx:GridViewDataTextColumn Caption="单据日期" FieldName="datCreateTime" VisibleIndex="4" Width="120px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="网单号" FieldName="strBillNo" ReadOnly="True" VisibleIndex="2" Width="160px">
                    </dx:GridViewDataTextColumn>
                </Columns>
                <SettingsBehavior AllowSort="False" />
                <SettingsPager Mode="ShowAllRecords" Visible="False">
                </SettingsPager>
                <Settings VerticalScrollableHeight="300" VerticalScrollBarMode="Visible" />
            </dx:ASPxGridView>
            <br />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
