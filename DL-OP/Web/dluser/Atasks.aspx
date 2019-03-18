<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Atasks.aspx.cs" Inherits="dluser_Atasks" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>全部待办工作</title>
    </head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
          

            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
  <dx:ASPxButton ID="BtnRefresh" runat="server" Text="刷新" OnClick="BtnRefresh_Click"></dx:ASPxButton>

            <dx:ASPxGridView ID="GridOrder" ClientInstanceName="GridOrder" runat="server" AutoGenerateColumns="False"              
                EnableTheming="True" Theme="Office2010Blue" KeyFieldName="strBillNo" Font-Size="10pt">
                <Columns>
                    <dx:GridViewCommandColumn ShowClearFilterButton="True" VisibleIndex="0" Width="50px">
                    </dx:GridViewCommandColumn>
                    <dx:GridViewDataTextColumn Caption="客户名称" FieldName="strUserName" VisibleIndex="5" Width="150px">
                        <Settings AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="开票单位名称" FieldName="ccusname" VisibleIndex="6" Width="150px">
                        <Settings AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="地址信息" FieldName="cdefine11" VisibleIndex="7" Width="300px">
                        <Settings AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="备注" FieldName="strRemarks" VisibleIndex="8" Width="200px">
                        <Settings AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataHyperLinkColumn Caption="处理订单" FieldName="strBillNo" VisibleIndex="3" Width="80px">
                        <PropertiesHyperLinkEdit NavigateUrlFormatString="Atasks.aspx?gbillno={0}" Text="接收处理">
                            <Style ForeColor="#0066FF">
                            </Style>
                        </PropertiesHyperLinkEdit>
                        <Settings AllowAutoFilter="False" AllowHeaderFilter="False" />
                    </dx:GridViewDataHyperLinkColumn>
                    <dx:GridViewDataTextColumn Caption="单据日期" FieldName="datCreateTime" VisibleIndex="4" Width="100px">
                        <Settings AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="网单号" FieldName="strBillNo" ReadOnly="True" VisibleIndex="2" Width="160px">
                        <Settings AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>
                </Columns>
                <SettingsBehavior AllowSort="False" />
                <SettingsPager Visible="False" Mode="ShowAllRecords">
                </SettingsPager>
                <Settings VerticalScrollableHeight="330" VerticalScrollBarMode="Visible" ShowFilterRow="True" />
                <SettingsCommandButton>
                    <ClearFilterButton ButtonType="Link" Text="清除">
                    </ClearFilterButton>
                    <SearchPanelApplyButton ButtonType="Button" Text="查询">
                    </SearchPanelApplyButton>
                    <SearchPanelClearButton ButtonType="Button" Text="清除">
                    </SearchPanelClearButton>
                </SettingsCommandButton>
                <SettingsSearchPanel ShowApplyButton="True" ShowClearButton="True" Visible="True" />
            </dx:ASPxGridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
         <div style="width:0;height:0">
                        <iframe id="OrderCenter" height="0" width="0" frameborder="0" name="OrderCenter" src="AtasksSubHandle.aspx"></iframe>
        </div>
    </form>
</body>
</html>

