<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SelectItem.aspx.cs" Inherits="SelectItem" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    
    <style type="text/css">

    </style>
    
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div style="float:left;">
                <dx:ASPxTreeList ID="treeList" runat="server" AutoGenerateColumns="False" Caption="选择物料大类 ↓" ClientInstanceName="treeList" EnableTheming="True" KeyFieldName="KeyFieldName" OnCustomDataCallback="treeList_CustomDataCallback" ParentFieldName="ParentFieldName" Theme="Office2010Blue" Width="300px" Font-Size="10pt">
                    <Columns>
                        <dx:TreeListTextColumn Caption="产品分类" FieldName="NodeName" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="0">
                        </dx:TreeListTextColumn>
                    </Columns>
                    <Settings ScrollableHeight="400" VerticalScrollBarMode="Auto" />
                    <SettingsBehavior AllowDragDrop="False" AllowFocusedNode="True" AllowSort="False" ExpandCollapseAction="NodeDblClick" FocusNodeOnExpandButtonClick="False" FocusNodeOnLoad="False" ProcessFocusedNodeChangedOnServer="True" ProcessSelectionChangedOnServer="True" />
                    <SettingsCookies CookiesID="NewOrder" Enabled="True" />
                    <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
                    <Images>
                        <CollapsedButton IconID="actions_addfile_16x16">
                        </CollapsedButton>
                        <ExpandedButton IconID="actions_apply_16x16">
                        </ExpandedButton>
                    </Images>
                    <Styles>
                        <FocusedNode BackColor="#9966FF">
                        </FocusedNode>
                    </Styles>
                    <ClientSideEvents CustomDataCallback="function(s, e) {cscode();
}" FocusedNodeChanged="function(s, e) {
            var key = treeList.GetFocusedNodeKey();
            treeList.PerformCustomDataCallback(key);
}" />
                </dx:ASPxTreeList></div>
                <asp:Button ID="btn" runat="server" OnClick="btn_Click" Text="Button" Style="display: none;" />
                <div style="float:left;"> <dx:ASPxGridView ID="TreeDetail" runat="server" AutoGenerateColumns="False" Caption="选择物料明细 ↓" ClientInstanceName="TreeDetail" KeyFieldName="cInvCode" PreviewFieldName="cInvName" ShowVerticalScrollBar="true" Theme="Office2010Blue" Width="500px" Font-Size="10pt">
                    <ClientSideEvents BeginCallback="function(s, e) {
}" />
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="名称" FieldName="cInvName" ReadOnly="True" VisibleIndex="4" Width="150px">
                            <Settings AllowAutoFilter="True" AllowAutoFilterTextInputTimer="True" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="规格" FieldName="cInvStd" ReadOnly="True" VisibleIndex="5" Width="100px">
                            <Settings AllowAutoFilterTextInputTimer="True" AutoFilterCondition="Contains" AllowAutoFilter="True" AllowSort="False" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="单位" FieldName="cComUnitName" ReadOnly="True" VisibleIndex="6" Width="30px">
                            <Settings AllowAutoFilter="False" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewCommandColumn ShowClearFilterButton="True" VisibleIndex="0" Width="30px">
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataTextColumn Caption="编码" FieldName="cInvCode" VisibleIndex="2" Width="0px">
                            <Settings AllowAutoFilter="False" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="首字拼音" FieldName="PY" VisibleIndex="3" Width="100px">
                            <Settings AllowAutoFilter="True" AllowAutoFilterTextInputTimer="True" AllowSort="False" AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataHyperLinkColumn Caption="添加" FieldName="cInvCode" VisibleIndex="1" Width="40px">
                            <PropertiesHyperLinkEdit NavigateUrlFormatString="SelectItem.aspx?add={0}" Target="Iframe2" Text="添加">
                            </PropertiesHyperLinkEdit>
                            <Settings AllowAutoFilter="False" AllowAutoFilterTextInputTimer="False" AllowSort="False" />
                        </dx:GridViewDataHyperLinkColumn>
                    </Columns>
                    <SettingsBehavior AllowDragDrop="False" AllowFocusedRow="True" AllowSort="False" />
                    <SettingsPager Mode="EndlessPaging" NumericButtonCount="999" Visible="False">
                    </SettingsPager>
                    <SettingsEditing EditFormColumnCount="99">
                    </SettingsEditing>
                    <Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowFooter="False" ShowTitlePanel="True" VerticalScrollableHeight="340" VerticalScrollBarMode="Visible" />
                    <SettingsText CommandApplySearchPanelFilter="查询" CommandCancel="取消" CommandClearSearchPanelFilter="清除" ConfirmOnLosingBatchChanges="未保存的数据将丢失,是否离开?" SearchPanelEditorNullText="请输入查找内容..." />
                    <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
                    <SettingsSearchPanel AllowTextInputTimer="False" ColumnNames="名称;规格;首字拼音" ShowApplyButton="True" ShowClearButton="True" Visible="True" />
                </dx:ASPxGridView></div>
            </ContentTemplate>

        </asp:UpdatePanel>
    
    </div>
    </form>
</body>
</html>
<script type="text/javascript">
    var cscode = function () {
        document.getElementById("<%=btn.ClientID%>").click();
        }
</script>
