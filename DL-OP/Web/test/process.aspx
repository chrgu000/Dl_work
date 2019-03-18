<%@ Page Language="C#" AutoEventWireup="true" CodeFile="process.aspx.cs" Inherits="test_process" %>
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
                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div>


                    <div style="float: left">
                        <dx:ASPxTreeList ID="treeList" runat="server" AutoGenerateColumns="False" Caption="选择物料大类 ↓"
                            ClientInstanceName="treeList" EnableTheming="True" KeyFieldName="KeyFieldName"
                            OnCustomDataCallback="treeList_CustomDataCallback" 
                            ParentFieldName="ParentFieldName" Theme="Office2010Blue" Width="335px">
                            <Columns>
                                <dx:TreeListTextColumn Caption="产品分类" FieldName="NodeName" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="0">
                                </dx:TreeListTextColumn>
                            </Columns>
                            <Settings ScrollableHeight="400" VerticalScrollBarMode="Auto" />
                            <SettingsBehavior AllowFocusedNode="True" AllowSort="False"  AllowDragDrop="False" />
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
                                ASPxLoadingPanel1.Show();
}" />
                        </dx:ASPxTreeList>
                    </div>
                    <div style="float: left;">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server"><ContentTemplate>
                                <asp:Button ID="btn" runat="server" OnClick="btn_Click" Text="Button" Style="display: none;" />
                                <%--<input id='ctl' type='button' onclick='cscode();' style="display: none;" />--%>
                                <dx:ASPxGridView ID="TreeDetail" ClientInstanceName="TreeDetail" runat="server" AutoGenerateColumns="False" Theme="Office2010Blue"
                                    ShowVerticalScrollBar="true" KeyFieldName="cInvCode" PreviewFieldName="cInvName" Caption="选择物料明细 ↓" Width="515px">
                                    <ClientSideEvents BeginCallback="function(s, e) {
}" />
                                    <Columns>
                                        <dx:GridViewDataTextColumn FieldName="cInvName" VisibleIndex="2" Width="150px" Caption="名称" ReadOnly="True">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="cInvStd" ReadOnly="True" VisibleIndex="3" Width="100px" Caption="规格">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="cComUnitName" VisibleIndex="4" Caption="单位" Width="30px" ReadOnly="True">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewCommandColumn Caption="选择" ShowSelectCheckbox="True" VisibleIndex="0" Width="35px" SelectAllCheckboxMode="AllPages">
                                        </dx:GridViewCommandColumn>
                                        <dx:GridViewDataTextColumn Caption="编码" FieldName="cInvCode" VisibleIndex="1" Width="0px">
                                        </dx:gridviewdatatextcolumn>
                                    </Columns>
                                    <SettingsBehavior AllowSort="False" AllowFocusedRow="True" AllowDragDrop="False" />
                                    <SettingsPager Visible="False" Mode="ShowAllRecords" NumericButtonCount="999">
                                    </SettingsPager>
                                    <SettingsEditing EditFormColumnCount="99">
                                    </SettingsEditing>
                                    <Settings VerticalScrollBarMode="Visible" ShowFooter="False" ShowTitlePanel="True" VerticalScrollableHeight="390" />
                                    <SettingsDataSecurity AllowDelete="False" AllowInsert="False" AllowEdit="False" />
                                </dx:ASPxGridView>        
                            </ContentTemplate></asp:UpdatePanel>                                 
                                    </div>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server"><ProgressTemplate>load....</ProgressTemplate></asp:UpdateProgress>  
        <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server"></dx:ASPxLoadingPanel>        

    </div>
    </form>
</body>
    <script type="text/javascript">
        var cscode = function () {
            document.getElementById("<%=btn.ClientID%>").click();
            
            }

    </script>
</html>
