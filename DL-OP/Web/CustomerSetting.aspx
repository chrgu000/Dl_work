<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CustomerSetting.aspx.cs" Inherits="CustomerSetting" %>

<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>

                <div style="margin-bottom:10px">
                <dx:ASPxButton ID="BtnRef" runat="server" ClientInstanceName="BtnRef" Text="刷新设置" Theme="SoftOrange">
                </dx:ASPxButton></div>
                 <div style="float:left;margin-right:20px">
                <dx:ASPxGridView ID="PhoneGrid" runat="server" AutoGenerateColumns="False" EnableTheming="True" KeyFieldName="Id" OnCustomUnboundColumnData="PhoneGrid_CustomUnboundColumnData"  Theme="Office2010Black" Caption="绑定的手机号码" ClientInstanceName="PhoneGrid" style="margin-top: 0px">
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="手机号码" FieldName="PhoneNo" ReadOnly="True" VisibleIndex="2" Width="300px">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="序号" FieldName="hh" ReadOnly="True" UnboundType="Integer" VisibleIndex="1">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="Id" ReadOnly="True" Visible="False" VisibleIndex="3" Width="0px">
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <SettingsBehavior AllowSort="False" />
                    <SettingsPager Visible="False">
                    </SettingsPager>
                    <SettingsEditing EditFormColumnCount="1" Mode="PopupEditForm">
                    </SettingsEditing>
                    <SettingsText EmptyDataRow="还没有手机号码,添加一个!" Title="已绑定的手机号码" />
                </dx:ASPxGridView>
                    </div>
                <div>
                <dx:ASPxGridView ID="SOADateGrid" runat="server" AutoGenerateColumns="False" Caption="账单日期" ClientInstanceName="SOADateGrid" 
                    EnableTheming="True" KeyFieldName="Id" OnCustomUnboundColumnData="PhoneGrid_CustomUnboundColumnData" style="margin-top: 0px" Theme="Office2010Black">
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="账单日期" FieldName="ccdefine1" ReadOnly="True" VisibleIndex="3" Width="100px">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="序号" FieldName="hh" ReadOnly="True" UnboundType="Integer" VisibleIndex="1">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="cCusName" ReadOnly="True" VisibleIndex="2" Width="180px" Caption="开票单位">
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <SettingsBehavior AllowSort="False" />
                    <SettingsPager Visible="False">
                    </SettingsPager>
                    <SettingsEditing EditFormColumnCount="1" Mode="PopupEditForm">
                    </SettingsEditing>
                    <SettingsText EmptyDataRow="没有数据!" Title="账单日期" />
                </dx:ASPxGridView>
                    </div>
                <dx:ASPxTabControl ID="ASPxTabControl1" runat="server">
                </dx:ASPxTabControl>
                <br />
            </ContentTemplate>
        </asp:UpdatePanel>
    
    
    </div>
    </form>
</body>
</html>
