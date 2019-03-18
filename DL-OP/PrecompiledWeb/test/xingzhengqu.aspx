<%@ page language="C#" autoeventwireup="true" inherits="test_xingzhengqu, dlopwebdll" enableviewstate="false" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Data.Linq" tagprefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script type="text/javascript">
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
</head>
<body>
    <form id="form1" runat="server">
    <div>
   <dx:ASPxDropDownEdit ID="DropDownEditxzq" runat="server" ClientInstanceName="DropDownEditxzq"
        Width="170px" AllowUserInput="False" AnimationType="None">
        <DropDownWindowStyle>
            <Border BorderWidth="0px" />
        </DropDownWindowStyle>
        <ClientSideEvents Init="UpdateSelection_xzq" DropDown="OnDropDown_xzq" />
        <DropDownWindowTemplate>
            <div>
                <dx:ASPxTreeList ID="TreeListxzq" ClientInstanceName="TreeListxzq" runat="server"
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
                        <dx:ASPxButton ID="clearButton" ClientEnabled="false" ClientInstanceName="clearButton"
                            runat="server" AutoPostBack="false" Text="清除">
                            <ClientSideEvents Click="ClearSelection_xzq" />
                        </dx:ASPxButton>
                    </td>
                    <td style="text-align: right; padding: 10px;">
                        <dx:ASPxButton ID="selectButton" ClientEnabled="false" ClientInstanceName="selectButton"
                            runat="server" AutoPostBack="false" Text="选择">
                            <ClientSideEvents Click="UpdateSelection_xzq" />
                        </dx:ASPxButton>
                        <dx:ASPxButton ID="closeButton" runat="server" AutoPostBack="false" Text="关闭">
                            <ClientSideEvents Click="function(s,e) { DropDownEdit.HideDropDown(); }" />
                        </dx:ASPxButton>
                    </td>
                </tr>
            </table>
        </DropDownWindowTemplate>
    </dx:ASPxDropDownEdit> 
    </div>
          <asp:ObjectDataSource ID="ods" runat="server"
                    TypeName="EmployeeSessionProvider" SelectMethod="Select"></asp:ObjectDataSource>
    </form>
</body>
</html>
