<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserAddress.aspx.cs" Inherits="UserAddress" EnableViewState="true" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Data.Linq" tagprefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script type="text/javascript">
        function ClearSelection() {
            TreeList.SetFocusedNodeKey("");
            UpdateControls(null, "");
        }
        function UpdateSelection() {
            var employeeName = "";
            var focusedNodeKey = TreeList.GetFocusedNodeKey();
            if (focusedNodeKey != "")
                employeeName = TreeList.cpvsimpleName[focusedNodeKey];
            UpdateControls(focusedNodeKey, employeeName);
        }
        function UpdateControls(key, text) {
            DropDownEdit.SetText(text);
            DropDownEdit.SetKeyValue(key);
            DropDownEdit.HideDropDown();
            UpdateButtons();
        }
        function UpdateButtons() {
            clearButton.SetEnabled(DropDownEdit.GetText() != "");
            selectButton.SetEnabled(TreeList.GetFocusedNodeKey() != "");
        }
        function OnDropDown() {
            TreeList.SetFocusedNodeKey(DropDownEdit.GetKeyValue());
            TreeList.MakeNodeVisible(TreeList.GetFocusedNodeKey());
        }
        function ClearSelection_xzq() {
            TreeListxzq.SetFocusedNodeKey("");
            UpdateControls_xzq(null, "");
        }
        function UpdateSelection_xzq() {
            var employeeName = "";
            var focusedNodeKey = TreeListxzq.GetFocusedNodeKey();
            if (focusedNodeKey != "")
                employeeName = TreeListxzq.cpvsimpleName[focusedNodeKey];
            UpdateControls_xzq(focusedNodeKey, employeeName);
        }
        function UpdateControls_xzq(key, text) {
            DropDownEditxzq.SetText(text);
            DropDownEditxzq.SetKeyValue(key);
            DropDownEditxzq.HideDropDown();
            UpdateButtons_xzq();
        }
        function UpdateButtons_xzq() {
            clearButton.SetEnabled(DropDownEditxzq.GetText() != "");
            selectButton.SetEnabled(TreeListxzq.GetFocusedNodeKey() != "");
        }
        function OnDropDown_xzq() {
            TreeListxzq.SetFocusedNodeKey(DropDownEditxzq.GetKeyValue());
            TreeListxzq.MakeNodeVisible(TreeListxzq.GetFocusedNodeKey());
        }
    </script>
    <style type="text/css">
        .auto-style1 {
            background-color: #FF0066;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div><h4>如需添加新的信息,请点击对应 配送/自提 信息<span class="auto-style1">表格左上角</span>的'新增信息',在弹出的编辑窗口中,输入信息后保存,完成新信息添加.</h4></div>
        <div>

            <dx:ASPxGridView ID="GridPS" ClientInstanceName="GridPS" runat="server" AutoGenerateColumns="False" OnRowValidating="GridPS_RowValidating" OnRowDeleting="GridPS_RowDeleting"
                KeyFieldName="lngopUseraddressId" Theme="Office2010Blue" OnRowUpdating="GridPS_RowUpdating" OnRowInserting="GridPS_RowInserting" Width="950px" Font-Size="11pt">
                <Columns>
                    <dx:GridViewCommandColumn ShowNewButtonInHeader="true" ShowEditButton="True" VisibleIndex="0" ShowUpdateButton="True" Width="80px" ShowClearFilterButton="True" ShowDeleteButton="True">
                    </dx:GridViewCommandColumn>
                    <dx:GridViewDataTextColumn FieldName="strConsigneeName" VisibleIndex="1" Caption="收货人" Width="90px">
                        <PropertiesTextEdit MaxLength="6" NullDisplayText="输入收货人">
                        </PropertiesTextEdit>
                        <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                        <EditFormSettings VisibleIndex="0" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataCheckColumn VisibleIndex="7" Caption="是否默认" Visible="False">
                    </dx:GridViewDataCheckColumn>
                    <dx:GridViewDataTextColumn FieldName="strConsigneeTel" VisibleIndex="2" Caption="联系电话" Width="110px">
                        <PropertiesTextEdit MaxLength="13" NullDisplayText="输入联系电话">
                        </PropertiesTextEdit>
                        <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                        <EditFormSettings VisibleIndex="1" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="行政区域" VisibleIndex="4" FieldName="strDistrict" Width="250px">
                        <PropertiesTextEdit MaxLength="120" NullDisplayText="选择行政区域">
                        </PropertiesTextEdit>
                        <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                        <EditItemTemplate>
 <dx:ASPxDropDownEdit ID="DropDownEdit" runat="server" ClientInstanceName="DropDownEdit"
        Width="170px" AllowUserInput="False" AnimationType="None">
        <DropDownWindowStyle>
            <Border BorderWidth="0px" />
        </DropDownWindowStyle>
        <ClientSideEvents Init="UpdateSelection" DropDown="OnDropDown" />
        <DropDownWindowTemplate>
            <div>
                <dx:ASPxTreeList ID="TreeList" ClientInstanceName="TreeList" runat="server"
                    Width="500px" DataSourceID="ods" OnCustomJSProperties="TreeList_CustomJSProperties"
                    KeyFieldName="ID" ParentFieldName="EmployerId" Theme="Office2010Blue" AutoGenerateColumns="False" EnableTheming="True" Caption="地区选择" 
                    SettingsBehavior-AllowSort="False" SettingsDataSecurity-AllowDelete="False" SettingsDataSecurity-AllowEdit="False" SettingsDataSecurity-AllowInsert="False" 
                    SettingsBehavior-AllowDragDrop="False" SettingsBehavior-AllowFocusedNode="True" SettingsBehavior-ExpandCollapseAction="NodeClick">
                    <Columns>
                        <dx:TreeListTextColumn ReadOnly="True" FieldName="vsimpleName" AllowSort="False" ShowInCustomizationForm="True" Caption="地区" VisibleIndex="1" Width="120px"></dx:TreeListTextColumn>
                        <dx:TreeListTextColumn ReadOnly="True" FieldName="vdescription" AllowSort="False" ShowInCustomizationForm="True" Caption="地区全称" VisibleIndex="2" Width="300px"></dx:TreeListTextColumn>
                    </Columns>

                    <Settings VerticalScrollBarMode="Auto" ScrollableHeight="250" />
                    <SettingsBehavior AllowFocusedNode="True" FocusNodeOnLoad="False"></SettingsBehavior>
                    <ClientSideEvents FocusedNodeChanged="function(s,e){ selectButton.SetEnabled(true); }" NodeDblClick="UpdateSelection" />
                    <BorderBottom BorderStyle="Solid" />
                    <SettingsBehavior AllowFocusedNode="true" AutoExpandAllNodes="false" FocusNodeOnLoad="false" />
                    <SettingsPager Mode="ShowAllNodes">
                    </SettingsPager>
                    <Styles>
                        <Node Cursor="pointer">
                        </Node>
                        <Indent Cursor="default">
                        </Indent>
                    </Styles>
                </dx:ASPxTreeList>
            </div>
            <table style="background-color: White; width: 100%;">
                <tr>
                    <td style="padding: 10px;">
                        <dx:ASPxButton ID="clearButton" ClientEnabled="false" ClientInstanceName="clearButton"
                            runat="server" AutoPostBack="false" Text="清除">
                            <ClientSideEvents Click="ClearSelection" />
                        </dx:ASPxButton>
                    </td>
                    <td style="text-align: right; padding: 10px;">
                        <dx:ASPxButton ID="selectButton" ClientEnabled="false" ClientInstanceName="selectButton"
                            runat="server" AutoPostBack="false" Text="选择">
                            <ClientSideEvents Click="UpdateSelection" />
                        </dx:ASPxButton>
                        <dx:ASPxButton ID="closeButton" runat="server" AutoPostBack="false" Text="关闭">
                            <ClientSideEvents Click="function(s,e) { DropDownEdit.HideDropDown(); }" />
                        </dx:ASPxButton>
                    </td>
                </tr>
            </table>
        </DropDownWindowTemplate>
    </dx:ASPxDropDownEdit>
                        </EditItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="收货地址" VisibleIndex="6" FieldName="strReceivingAddress" Width="300px">
                        <PropertiesTextEdit MaxLength="200" NullDisplayText="输入详细街道地址">
                        </PropertiesTextEdit>
                        <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
<EditItemTemplate>
    <dx:ASPxTextBox ID="txtName" runat="server" Width="170px" ></dx:ASPxTextBox>
</EditItemTemplate>
                                            </dx:GridViewDataTextColumn>
                </Columns>
                <SettingsEditing EditFormColumnCount="3" Mode="EditForm" />
                <SettingsText CommandCancel="取消" CommandUpdate="确定" CommandDelete="删除" CommandEdit="编辑" ContextMenuDeleteRow="是否确认删除?" ConfirmDelete="是否确定删除该地址信息?" ConfirmOnLosingBatchChanges="有未保存的数据,是否离开?" EmptyDataRow="没有数据" />
                <SettingsPopup>
                    <EditForm Width="600" Modal="true"/>
                </SettingsPopup>
                 <SettingsCommandButton>
                    <NewButton Text="新增信息">
                        <Styles>
                            <Style ForeColor="#6600FF">
                            </Style>
                        </Styles>
                    </NewButton>
                    <UpdateButton Text="保存" ButtonType="Button">
                        <Styles>
                            <Style>
                                <Paddings PaddingRight="850px" />
                            </Style>
                        </Styles>
                    </UpdateButton>
                    <CancelButton Text="取消" ButtonType="Button">
                        <Styles>
                            <Style>
                                <Paddings PaddingRight="850px" />
                            </Style>
                        </Styles>
                    </CancelButton>
                    <EditButton Text="编辑">
                        <Styles>
                            <Style ForeColor="#FF5050">
                            </Style>
                        </Styles>
                    </EditButton>
                     <DeleteButton Text="删除">
                         <Styles>
                             <Style ForeColor="#000066">
                             </Style>
                         </Styles>
                     </DeleteButton>
                </SettingsCommandButton>
                <SettingsBehavior AllowSort="False" FilterRowMode="OnClick" ConfirmDelete="True" />
                <SettingsPager Mode="ShowAllRecords" />
                <Settings ShowTitlePanel="true" VerticalScrollableHeight="180" VerticalScrollBarMode="Auto" ShowFilterRow="True" ShowFilterRowMenu="True" />
                <SettingsText Title="用户配送地址编辑" />
            </dx:ASPxGridView>

            <br />

            <dx:ASPxGridView ID="GridZT" ClientInstanceName="GridZT" runat="server" AutoGenerateColumns="False" OnRowValidating="GridZT_RowValidating" OnRowDeleting="GridZT_RowDeleting"
                KeyFieldName="lngopUseraddressId" Theme="Office2010Blue" OnRowUpdating="GridZT_RowUpdating" OnRowInserting="GridZT_RowInserting" Width="900px" Font-Size="11pt">
                <SettingsText Title="用户自提信息编辑" CommandDelete="删除" ConfirmDelete="是否确认删除该自提信息?" />
                <Columns>
                    <dx:GridViewCommandColumn ShowNewButtonInHeader="true" ShowEditButton="True" VisibleIndex="0" ShowUpdateButton="True" Width="100px" ShowClearFilterButton="True" ShowDeleteButton="True">
                    </dx:GridViewCommandColumn>
                    <dx:GridViewDataTextColumn FieldName="strCarplateNumber" VisibleIndex="1" Caption="车牌号">
                        <PropertiesTextEdit MaxLength="14">
                        </PropertiesTextEdit>
                        <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                        <EditFormSettings VisibleIndex="0" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="strDriverName" VisibleIndex="2" Caption="司机姓名">
                        <PropertiesTextEdit MaxLength="8">
                        </PropertiesTextEdit>
                        <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                        <EditFormSettings VisibleIndex="1" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="strDriverTel" VisibleIndex="3" Caption="司机电话">
                        <PropertiesTextEdit MaxLength="13">
                        </PropertiesTextEdit>
                        <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                        <EditFormSettings VisibleIndex="2" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="司机身份证" FieldName="strIdCard" VisibleIndex="4">
                        <PropertiesTextEdit MaxLength="20">
                        </PropertiesTextEdit>
                        <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                        <EditFormSettings VisibleIndex="3" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataCheckColumn VisibleIndex="5" Caption="是否默认" Visible="False">
                    </dx:GridViewDataCheckColumn>
                </Columns>
                <SettingsBehavior AllowSort="False" ConfirmDelete="True" />
                <SettingsPager Mode="ShowAllRecords" />
                <SettingsEditing EditFormColumnCount="4" Mode="EditFormAndDisplayRow" />
                <Settings ShowTitlePanel="true" VerticalScrollableHeight="180" VerticalScrollBarMode="Auto" ShowFilterRow="True" ShowFilterRowMenu="True" />
                <SettingsText CommandCancel="取消" CommandUpdate="确定" />
                <SettingsPopup>
                    <EditForm Width="600" />
                </SettingsPopup>
                <SettingsCommandButton>
                    <NewButton Text="新增信息">
                        <Styles>
                            <Style ForeColor="#6600FF">
                            </Style>
                        </Styles>
                    </NewButton>
                    <UpdateButton Text="保存" ButtonType="Button">
                        <Styles>
                            <Style>
                                <Paddings PaddingRight="800px" />
                            </Style>
                        </Styles>
                    </UpdateButton>
                    <CancelButton Text="取消" ButtonType="Button">
                        <Styles>
                            <Style>
                                <Paddings PaddingRight="800px" />
                            </Style>
                        </Styles>
                    </CancelButton>
                    <EditButton Text="编辑">
                        <Styles>
                            <Style ForeColor="#FF5050">
                            </Style>
                        </Styles>
                    </EditButton>
                    <DeleteButton Text="删除">
                        <Styles>
                            <Style ForeColor="#000066">
                            </Style>
                        </Styles>
                    </DeleteButton>
                </SettingsCommandButton>
            </dx:ASPxGridView>
  <br />

            <dx:ASPxGridView ID="GridZTXZQ" ClientInstanceName="GridZTXZQ" runat="server" AutoGenerateColumns="False" OnRowValidating="GridZTXZQ_RowValidating" 
                OnRowDeleting="GridZTXZQ_RowDeleting" KeyFieldName="lngopUseraddress_exId" Theme="Office2010Blue" OnRowUpdating="GridZTXZQ_RowUpdating" 
                OnRowInserting="GridZTXZQ_RowInserting" Width="800px" Font-Size="11pt">
                <Columns>
                    <dx:GridViewCommandColumn ShowNewButtonInHeader="true" VisibleIndex="0" ShowUpdateButton="True" Width="80px" ShowDeleteButton="True">
                    </dx:GridViewCommandColumn>
                    <dx:GridViewDataTextColumn Caption="id" FieldName="lngopUseraddress_exId" VisibleIndex="1" ReadOnly="True" Width="100px">
                        <PropertiesTextEdit NullText="自动生成！不需要填写">
                            <NullTextStyle ForeColor="Red">
                            </NullTextStyle>
                        </PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="xzq" FieldName="xzq" VisibleIndex="2" Width="400px">
                        <EditItemTemplate>
                            <dx:ASPxDropDownEdit ID="DropDownEditxzq" runat="server" AllowUserInput="False" AnimationType="None" ClientInstanceName="DropDownEditxzq" Width="170px">
                                <DropDownWindowStyle>
                                    <Border BorderWidth="0px" />
                                </DropDownWindowStyle>
                                <ClientSideEvents DropDown="OnDropDown_xzq" Init="UpdateSelection_xzq" />
                                <DropDownWindowTemplate>
                                    <div>
                                        <dx:ASPxTreeList ID="TreeListxzq" runat="server" AutoGenerateColumns="False" Caption="地区选择" ClientInstanceName="TreeListxzq" DataSourceID="ods" EnableTheming="True" KeyFieldName="ID" OnCustomJSProperties="TreeList_CustomJSProperties" ParentFieldName="EmployerId" SettingsBehavior-AllowDragDrop="False" SettingsBehavior-AllowFocusedNode="True" SettingsBehavior-AllowSort="False" SettingsBehavior-ExpandCollapseAction="NodeClick" SettingsDataSecurity-AllowDelete="False" SettingsDataSecurity-AllowEdit="False" SettingsDataSecurity-AllowInsert="False" Theme="Office2010Blue" Width="500px">
                                            <Columns>
                                                <dx:TreeListTextColumn AllowSort="False" Caption="地区" FieldName="vsimpleName" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="1" Width="120px">
                                                </dx:TreeListTextColumn>
                                                <dx:TreeListTextColumn AllowSort="False" Caption="地区全称" FieldName="vdescription" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="2" Width="300px">
                                                </dx:TreeListTextColumn>
                                            </Columns>
                                            <Settings ScrollableHeight="250" VerticalScrollBarMode="Auto" />
                                            <SettingsBehavior AllowFocusedNode="True" FocusNodeOnLoad="False" />
                                            <ClientSideEvents FocusedNodeChanged="function(s,e){ selectButton.SetEnabled(true); }" NodeDblClick="UpdateSelection_xzq" />
                                            <BorderBottom BorderStyle="Solid" />
                                            <SettingsBehavior AllowFocusedNode="true" AutoExpandAllNodes="false" FocusNodeOnLoad="false" />
                                            <SettingsPager Mode="ShowAllNodes">
                                            </SettingsPager>
                                            <Styles>
                                                <Node Cursor="pointer">
                                                </Node>
                                                <Indent Cursor="default">
                                                </Indent>
                                            </Styles>
                                        </dx:ASPxTreeList>
                                    </div>
                                    <table style="background-color: White; width: 100%;">
                                        <tr>
                                            <td style="padding: 10px;">
                                                <dx:ASPxButton ID="clearButton" runat="server" AutoPostBack="false" ClientEnabled="false" ClientInstanceName="clearButton" Text="清除">
                                                    <ClientSideEvents Click="ClearSelection_xzq" />
                                                </dx:ASPxButton>
                                            </td>
                                            <td style="text-align: right; padding: 10px;">
                                                <dx:ASPxButton ID="selectButton" runat="server" AutoPostBack="false" ClientEnabled="false" ClientInstanceName="selectButton" Text="选择">
                                                    <ClientSideEvents Click="UpdateSelection_xzq" />
                                                </dx:ASPxButton>
                                                <dx:ASPxButton ID="closeButton" runat="server" AutoPostBack="false" Text="关闭">
                                                    <ClientSideEvents Click="function(s,e) { DropDownEditxzq.HideDropDown(); }" />
                                                </dx:ASPxButton>
                                            </td>
                                        </tr>
                                    </table>
                                </DropDownWindowTemplate>
                            </dx:ASPxDropDownEdit>
                        </EditItemTemplate>
                    </dx:GridViewDataTextColumn>
                </Columns>
                <SettingsEditing EditFormColumnCount="3" Mode="EditForm" />
                <SettingsText CommandCancel="取消" CommandUpdate="确定" CommandDelete="删除" CommandEdit="编辑" ContextMenuDeleteRow="是否确认删除?" ConfirmDelete="是否确定删除该地址信息?" ConfirmOnLosingBatchChanges="有未保存的数据,是否离开?" EmptyDataRow="没有数据" />
                <SettingsPopup>
                    <EditForm Width="600" Modal="true"/>
                </SettingsPopup>
                 <SettingsCommandButton>
                    <NewButton Text="新增信息">
                        <Styles>
                            <Style ForeColor="#6600FF">
                            </Style>
                        </Styles>
                    </NewButton>
                    <UpdateButton Text="保存" ButtonType="Button">
                        <Styles>
                            <Style>
                                <Paddings PaddingRight="850px" />
                            </Style>
                        </Styles>
                    </UpdateButton>
                    <CancelButton Text="取消" ButtonType="Button">
                        <Styles>
                            <Style>
                                <Paddings PaddingRight="850px" />
                            </Style>
                        </Styles>
                    </CancelButton>
                    <EditButton Text="编辑">
                        <Styles>
                            <Style ForeColor="#FF5050">
                            </Style>
                        </Styles>
                    </EditButton>
                     <DeleteButton Text="删除">
                         <Styles>
                             <Style ForeColor="#000066">
                             </Style>
                         </Styles>
                     </DeleteButton>
                </SettingsCommandButton>
                <SettingsBehavior AllowSort="False" FilterRowMode="OnClick" ConfirmDelete="True" />
                <SettingsPager Mode="ShowAllRecords" />
                <Settings ShowTitlePanel="true" VerticalScrollableHeight="180" VerticalScrollBarMode="Auto" ShowFilterRowMenu="True" />
                <SettingsText Title="用户自提行政区信息编辑(行政区不要重复！)" />
            </dx:ASPxGridView>

            <br />

                <asp:SqlDataSource ID="PsData" runat="server" ConnectionString="<%$ ConnectionStrings:UFDATA_001_2015ConnectionString %>" SelectCommand="DLproc_UserAddressPSBySel" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:CookieParameter CookieName="lngopUserId" DefaultValue="0" Name="lngopUserId" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>

            <br />
            <div>
                <asp:ObjectDataSource ID="ods" runat="server"
                    TypeName="EmployeeSessionProvider" SelectMethod="Select"></asp:ObjectDataSource>
            </div>
        </div>

        <%--<ef:entitydatasource runat="server" ID="CustomersDataSource"
        ContextTypeName="NorthwindContext"
        EnableDelete="True" EnableInsert="True" EnableUpdate="True"
        EntitySetName="Customers">
    </ef:entitydatasource>--%>
    </form>

</body>
</html>
