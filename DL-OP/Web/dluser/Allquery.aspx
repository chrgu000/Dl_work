<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Allquery.aspx.cs" Inherits="dluser_Allquery" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>

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

                <div style="float: left;">
<dx:ASPxTreeList ID="treeList" runat="server" AutoGenerateColumns="False" Caption="选择物料大类 ↓"
                                        ClientInstanceName="treeList" EnableTheming="True" KeyFieldName="KeyFieldName"
                                        OnCustomDataCallback="treeList_CustomDataCallback"
                                        ParentFieldName="ParentFieldName" Theme="Office2010Blue" Width="335px" Font-Size="10pt">
                                        <Columns>
                                            <dx:TreeListTextColumn Caption="产品分类" FieldName="NodeName" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="0">
                                            </dx:TreeListTextColumn>
                                        </Columns>
                                        <Settings ScrollableHeight="400" VerticalScrollBarMode="Auto" />
                                        <SettingsBehavior AllowFocusedNode="True" AllowSort="False" ProcessFocusedNodeChangedOnServer="True" ProcessSelectionChangedOnServer="True"
                                            AllowDragDrop="False" ExpandCollapseAction="NodeDblClick" FocusNodeOnExpandButtonClick="False" FocusNodeOnLoad="False" />
                                        <SettingsCookies CookiesID="NewOrder" Enabled="True" />
                                        <SettingsLoadingPanel ShowOnPostBacks="True" Text="数据加载中,请稍候..." />
                                        <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
                                        <SettingsText LoadingPanelText="数据加载中,请稍候..." />
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
}"
                                            FocusedNodeChanged="function(s, e) {
            var key = treeList.GetFocusedNodeKey();
            treeList.PerformCustomDataCallback(key);
}" />
                                    </dx:ASPxTreeList>
</div>
                <div style="float: left;">
                    <asp:Button ID="btn" runat="server" OnClick="btn_Click" Text="Button" Style="display: none;" />
                 <dx:ASPxGridView ID="TreeDetail" ClientInstanceName="TreeDetail" runat="server" AutoGenerateColumns="False" Theme="Office2010Blue"
                                        ShowVerticalScrollBar="true" KeyFieldName="cInvCode" PreviewFieldName="cInvName" Caption="选择物料明细 ↓" Width="515px" Font-Size="9pt">
                                        <ClientSideEvents BeginCallback="function(s, e) {
}" />
                                        <Columns>
                                            <dx:GridViewDataTextColumn FieldName="cInvName" VisibleIndex="4" Width="150px" Caption="名称" ReadOnly="True">
                                                <Settings AllowAutoFilterTextInputTimer="True" AutoFilterCondition="Contains" AllowAutoFilter="True" />
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="cInvStd" ReadOnly="True" VisibleIndex="5" Width="100px" Caption="规格">
                                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="cComUnitName" VisibleIndex="6" Caption="单位" Width="30px" ReadOnly="True">
                                                <Settings AllowAutoFilter="False" />
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewCommandColumn Caption="选择" VisibleIndex="0" Width="35px" ShowClearFilterButton="True">
                                            </dx:GridViewCommandColumn>
                                            <dx:GridViewDataTextColumn Caption="编码" FieldName="cInvCode" VisibleIndex="2" Width="0px">
                                                <Settings AllowAutoFilter="False" />
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="首字拼音" FieldName="PY" ReadOnly="True" VisibleIndex="3" Width="100px">
                                                <Settings AllowAutoFilter="True" AllowAutoFilterTextInputTimer="True" AllowSort="False" AutoFilterCondition="Contains" />
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataImageColumn FieldName="img" VisibleIndex="1" Width="25px" Caption=" ">
                                            </dx:GridViewDataImageColumn>
                                            <dx:GridViewDataTextColumn Caption="可用量" FieldName="iqty" VisibleIndex="7" Width="80px">
                                            </dx:GridViewDataTextColumn>
                                        </Columns>
                                        <SettingsBehavior AllowSort="False" AllowFocusedRow="True" AllowDragDrop="False" />
                                        <SettingsPager Visible="False" Mode="ShowAllRecords" NumericButtonCount="999">
                                        </SettingsPager>
                                        <SettingsEditing EditFormColumnCount="99">
                                        </SettingsEditing>
                                        <Settings VerticalScrollBarMode="Visible" ShowFooter="False" ShowTitlePanel="True" VerticalScrollableHeight="350" ShowFilterRow="True" ShowFilterRowMenu="True" />
                                        <SettingsText CommandApplySearchPanelFilter="查询" CommandCancel="取消" CommandClearSearchPanelFilter="清除" ConfirmOnLosingBatchChanges="未保存的数据将丢失,是否离开?" SearchPanelEditorNullText="请输入查找内容..." />
                                        <SettingsLoadingPanel Text="数据加载中,请稍候..." />
                                        <SettingsDataSecurity AllowDelete="False" AllowInsert="False" AllowEdit="False" />
                                        <SettingsSearchPanel AllowTextInputTimer="False" ColumnNames="名称;规格" ShowApplyButton="True" ShowClearButton="True" Visible="True" />
                                    </dx:ASPxGridView>
                    </div>

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