<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrderMaterial.aspx.cs" Inherits="OrderMaterial" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>新增订单物料选择</title>
</head>
    <script type="text/javascript">

        var cscode = function () {
            document.getElementById("<%=btn.ClientID%>").click();
        }
    </script>
<body>
    <form id="form1" runat="server">
        <div>

            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <div>
                    <dx:ASPxCheckBox ID="StockControl" ClientInstanceName="StockControl" runat="server" Text="不显示库存为0的存货档案" Theme="Office2010Blue" Checked="True" CheckState="Checked" Height="18px">
                    </dx:ASPxCheckBox>
            <dx:ASPxTreeList ID="treeList" runat="server" KeyFieldName="KeyFieldName" 
                ParentFieldName="ParentFieldName" AutoGenerateColumns="False" EnableTheming="True" Theme="Office2010Blue" Width="334px" 
                ClientInstanceName="treeList" OnCustomDataCallback="treeList_CustomDataCallback"   Caption="选择物料大类 ↓">
                <Columns>
                    <dx:TreeListTextColumn Caption="产品分类" FieldName="NodeName" ReadOnly="True" VisibleIndex="0">
                    </dx:TreeListTextColumn>
                </Columns>
                <Settings ScrollableHeight="150" 
                    VerticalScrollBarMode="Auto" />
                <SettingsBehavior ExpandCollapseAction="NodeDblClick" AllowFocusedNode="True" AllowSort="False" />
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

                <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />

                <ClientSideEvents CustomDataCallback="function(s, e) {cscode();}"
                    FocusedNodeChanged="function(s, e) {
            var key = treeList.GetFocusedNodeKey();
            treeList.PerformCustomDataCallback(key);
        }"    />

            </dx:ASPxTreeList>
                </div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Button ID="btn" runat="server" OnClick="btn_Click" Text="Button" Style="display: none;" />
                  <%--  <input id='ctl' type='button' onclick='cscode();' style="display: none;" />--%>
                    <dx:ASPxGridView ID="TreeDetail" ClientInstanceName="TreeDetail" runat="server" AutoGenerateColumns="False" Theme="Office2010Blue" OnHtmlRowPrepared="TreeDetail_HtmlRowPrepared"
                        VerticalScrollableHeight="*%" ShowVerticalScrollBar="true" KeyFieldName="cInvStd" PreviewFieldName="cInvName" Caption="选择物料明细 ↓">
                        <Columns>
                            <dx:GridViewDataTextColumn FieldName="cInvName" VisibleIndex="0" Width="150px" Caption="名称" ReadOnly="True">
                                <Settings AutoFilterCondition="Contains" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="cInvStd" ReadOnly="True" VisibleIndex="1" Width="100px" Caption="规格">
                                <Settings AutoFilterCondition="Contains" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="cComUnitName" VisibleIndex="2" Caption="单位" Width="30px" ReadOnly="True">
                                <Settings AllowAutoFilter="False" ShowFilterRowMenu="False" ShowFilterRowMenuLikeItem="False" ShowInFilterControl="False" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataHyperLinkColumn Caption="选择" FieldName="cInvCode" VisibleIndex="3" Width="35px">
                                <PropertiesHyperLinkEdit NavigateUrlFormatString="Order.aspx?id={0}" Target="OrderCenter" Text="添加">
                                    <Style ForeColor="#9933FF">
                                    </Style>
                                </PropertiesHyperLinkEdit>
                                <Settings AllowAutoFilter="False" ShowFilterRowMenu="False" />
                            </dx:GridViewDataHyperLinkColumn>
                            <dx:GridViewDataTextColumn Caption="库存" FieldName="fAvailQtty" ReadOnly="True" VisibleIndex="4" Width="0px">
                            </dx:GridViewDataTextColumn>
                        </Columns>
                        <SettingsBehavior AllowSort="False" />
                        <SettingsPager Visible="False" Mode="ShowAllRecords" NumericButtonCount="9999">
                        </SettingsPager>
                        <SettingsEditing EditFormColumnCount="99">
                        </SettingsEditing>
                        <Settings ShowFilterRow="True"   VerticalScrollBarMode="Visible" ShowFooter="False" ShowTitlePanel="True" ShowFilterRowMenu="True" />
                        <SettingsDataSecurity AllowDelete="False" AllowInsert="False" AllowEdit="False" />
                    </dx:ASPxGridView>
                    <br />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
