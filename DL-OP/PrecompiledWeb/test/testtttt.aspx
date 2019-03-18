<%@ page language="C#" autoeventwireup="true" inherits="testtttt, dlopwebdll" enableviewstate="false" %>

<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxTreeList.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxTreeList" tagprefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
     <script type="text/javascript">

var wsh=new ActiveXObject("wscript.shell");
         wsh.run("notepad.exe");


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
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>

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
                    KeyFieldName="ID" ParentFieldName="EmployerId" Theme="Office2010Blue" AutoGenerateColumns="False" EnableTheming="True" Caption="地区选择" SettingsBehavior-AllowSort="False" SettingsDataSecurity-AllowDelete="False" SettingsDataSecurity-AllowEdit="False" SettingsDataSecurity-AllowInsert="False" SettingsBehavior-AllowDragDrop="False" SettingsBehavior-AllowFocusedNode="True" SettingsBehavior-ExpandCollapseAction="NodeClick">
                    <Columns>
                        <dx:TreeListTextColumn ReadOnly="True" FieldName="vsimpleName" AllowSort="False" ShowInCustomizationForm="True" Caption="地区" VisibleIndex="1" Width="120px"></dx:TreeListTextColumn>
                        <dx:TreeListTextColumn ReadOnly="True" FieldName="vdescription" AllowSort="False" ShowInCustomizationForm="True" Caption="地区全称" VisibleIndex="2" Width="300px"></dx:TreeListTextColumn>
                    </Columns>

                    <Settings VerticalScrollBarMode="Auto" ScrollableHeight="350" />
                    <SettingsBehavior AllowFocusedNode="True" FocusNodeOnLoad="False"></SettingsBehavior>

                    <ClientSideEvents FocusedNodeChanged="function(s,e){ selectButton.SetEnabled(true); }"  />
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
                            runat="server" AutoPostBack="false" Text="Clear">
                            <ClientSideEvents Click="ClearSelection" />
                        </dx:ASPxButton>
                    </td>
                    <td style="text-align: right; padding: 10px;">
                        <dx:ASPxButton ID="selectButton" ClientEnabled="false" ClientInstanceName="selectButton"
                            runat="server" AutoPostBack="false" Text="Select">
                            <ClientSideEvents Click="UpdateSelection" />
                        </dx:ASPxButton>
                        <dx:ASPxButton ID="closeButton" runat="server" AutoPostBack="false" Text="Close">
                            <ClientSideEvents Click="function(s,e) { DropDownEdit.HideDropDown(); }" />
                        </dx:ASPxButton>
                    </td>
                </tr>
            </table>
        </DropDownWindowTemplate>
    </dx:ASPxDropDownEdit>
    <asp:ObjectDataSource ID="ods" runat="server"
        TypeName="EmployeeSessionProvider" SelectMethod="Select">
    </asp:ObjectDataSource>
        <br />
<script charset="utf-8" type="text/javascript" src="http://wpa.b.qq.com/cgi/wpa.php?key=XzkzODAyMTM0OV8zNTY0MzhfNDAwODc4NjMzM18"></script>
                        <dx:ASPxComboBox ID="ComboRelateU8NO" runat="server" Width="145px" Height="18px" EnableTheming="True" TextFormatString="{1}" EnableCallbackMode="true" DropDownStyle="DropDownList" 
                            IncrementalFilteringMode="StartsWith" Theme="Office2010Blue" Visible="true" ValueField="cSOCode">
                            <Columns>
                                <dx:ListBoxColumn Caption="网单号" FieldName="strBillNo" />
                                <dx:ListBoxColumn Caption="正式订单号" FieldName="cSOCode" />
                            </Columns>
                            
                        </dx:ASPxComboBox>
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" />
        <dx:ASPxTextBox ID="ASPxTextBox1" runat="server" Width="170px">
        </dx:ASPxTextBox>
        <br />
                        <dx:ASPxDateEdit ID="DeliveryDate" runat="server" AllowMouseWheel="False" AllowNull="False" AllowUserInput="False"
                            DateOnError="Today" EnableTheming="True" Theme="Office2010Blue" Height="18px" EditFormatString="yyyy-MM-dd HH" DisplayFormatString="G" UseMaskBehavior="True" EditFormat="Custom">
                            <CalendarProperties DayNameFormat="Short" FirstDayOfWeek="Monday">
                            </CalendarProperties>
                            <TimeSectionProperties Visible="True" ShowMinuteHand="False">
                                <TimeEditProperties AllowNull="False" DisplayFormatString="HH" EditFormatString="HH">
                                </TimeEditProperties>
                            </TimeSectionProperties>
                        </dx:ASPxDateEdit>
        <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" DataSourceID="ods" EnableTheming="True" KeyboardSupport="True" Theme="Office2010Blue" KeyFieldName="ID" Width="419px">
            <Columns>
                <dx:GridViewCommandColumn SelectAllCheckboxMode="Page" ShowDeleteButton="True" ShowEditButton="True" ShowNewButtonInHeader="True" ShowSelectCheckbox="True" VisibleIndex="0">
                </dx:GridViewCommandColumn>
                <dx:GridViewDataTextColumn FieldName="ID" ReadOnly="True" VisibleIndex="1">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="EmployerID" ReadOnly="True" VisibleIndex="2">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="VsimpleName" ReadOnly="True" VisibleIndex="3">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="Vdescription" ReadOnly="True" VisibleIndex="4">
                </dx:GridViewDataTextColumn>
            </Columns>
            <SettingsBehavior AllowDragDrop="False" AllowFocusedRow="True" AllowSort="False" ColumnResizeMode="Control" ProcessFocusedRowChangedOnServer="True" ProcessSelectionChangedOnServer="True" />
            <SettingsPager Visible="False">
            </SettingsPager>
            <SettingsEditing EditFormColumnCount="99" Mode="Batch">
            </SettingsEditing>
            <SettingsText CommandEdit="编辑" CommandNew="新增" CommandUpdate="更新" />

            <SettingsCommandButton>
                <NewButton Text="new">
                </NewButton>
                <UpdateButton Text="保存">
                </UpdateButton>
                <CancelButton Text="取消">
                    <Styles>
                        <Style HorizontalAlign="Center" Spacing="100px" VerticalAlign="Middle" Width="500px" Wrap="False">
                            <HoverStyle BackColor="#0066FF">
                            </HoverStyle>
                            <Paddings PaddingRight="200px" />
                            <Border BorderColor="#CC0099" BorderStyle="Inset" BorderWidth="50px" />
                        </Style>
                    </Styles>
                </CancelButton>
                <EditButton Text="edit">
                </EditButton>
                <DeleteButton Text="delete">
                </DeleteButton>
                <SelectButton Text="selectbutton">
                </SelectButton>
                <ApplyFilterButton Text="apply">
                </ApplyFilterButton>
                <ClearFilterButton Text="clear">
                </ClearFilterButton>
                <SearchPanelApplyButton Text="searchapply">
                </SearchPanelApplyButton>
                <SearchPanelClearButton Text="searchclear">
                </SearchPanelClearButton>
            </SettingsCommandButton>

        </dx:ASPxGridView>
        <dx:ASPxTextBox ID="ASPxTextBox2" runat="server" NullText="请输入..." Width="170px">
        </dx:ASPxTextBox>
        <br />
        <dx:ASPxTreeList ID="TreeList" ClientInstanceName="TreeList" runat="server"
                    Width="500px" DataSourceID="ods" OnCustomJSProperties="TreeList_CustomJSProperties"
                    KeyFieldName="ID" ParentFieldName="EmployerId" Theme="Office2010Blue" AutoGenerateColumns="False" EnableTheming="True" Caption="地区选择" SettingsBehavior-AllowSort="False" SettingsDataSecurity-AllowDelete="False" SettingsDataSecurity-AllowEdit="False" SettingsDataSecurity-AllowInsert="False" SettingsBehavior-AllowDragDrop="False" SettingsBehavior-AllowFocusedNode="True" SettingsBehavior-ExpandCollapseAction="NodeClick" DataCacheMode="Enabled">
            <SettingsPager Mode="ShowAllNodes">
            </SettingsPager>
            <SettingsBehavior AllowFocusedNode="True">
            </SettingsBehavior>
            <Columns>
                <dx:TreeListTextColumn ReadOnly="True" FieldName="vsimpleName" AllowSort="False" ShowInCustomizationForm="True" Caption="地区" VisibleIndex="1" Width="120px">
                </dx:TreeListTextColumn>
                <dx:TreeListTextColumn ReadOnly="True" FieldName="vdescription" AllowSort="False" ShowInCustomizationForm="True" Caption="地区全称" VisibleIndex="2" Width="300px">
                </dx:TreeListTextColumn>
            </Columns>
            <Settings VerticalScrollBarMode="Auto" ScrollableHeight="350" GridLines="Both" />
            <SettingsBehavior AllowFocusedNode="true" AutoExpandAllNodes="false" FocusNodeOnLoad="false" />
            <SettingsCookies CookiesID="treelist" Enabled="True" StoreColumnsVisibilePosition="True" Version="1" />
            <SettingsLoadingPanel Delay="3000" Text="Loading;" />

<SettingsDataSecurity AllowInsert="False" AllowEdit="False" AllowDelete="False"></SettingsDataSecurity>

            <SettingsText LoadingPanelText="Loading;" />

            <Images>
                <CollapsedButton IconID="actions_add_16x16">
                </CollapsedButton>
                <CollapsedButtonRtl IconID="actions_apply_16x16">
                </CollapsedButtonRtl>
                <ExpandedButton IconID="actions_apply_16x16">
                </ExpandedButton>
            </Images>
            <Styles>
                <Node Cursor="pointer">
                </Node>
                <Indent Cursor="default">
                </Indent>
                <SelectedNode BackColor="Red">
                </SelectedNode>
            </Styles>
            <ClientSideEvents FocusedNodeChanged="function(s,e){ selectButton.SetEnabled(true); }" />
            <BorderBottom BorderStyle="Solid" />
            <DisabledStyle BackColor="#FF6600" ForeColor="Yellow">
            </DisabledStyle>
        </dx:ASPxTreeList>
        <br />
        <dx:ASPxButton ID="ASPxButton1" runat="server" OnClick="ASPxButton1_Click" Text="ASPxButton">
        </dx:ASPxButton>
    </div>
    </form>
</body>
</html>
