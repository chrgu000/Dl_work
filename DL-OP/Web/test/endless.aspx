<%@ Page Language="C#" AutoEventWireup="true" CodeFile="endless.aspx.cs" Inherits="test_endless" %>

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
                        <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" KeyFieldName="cInvCode" Width="538px" Theme="Office2010Blue">
                            <Columns>
                                <dx:GridViewDataTextColumn Caption="cInvCode" FieldName="cInvCode" VisibleIndex="0">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="cInvName" FieldName="cInvName" VisibleIndex="1">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="cInvStd" FieldName="cInvStd" VisibleIndex="2">
                                </dx:GridViewDataTextColumn>
                            </Columns>
                                    <SettingsBehavior AllowSort="False" AllowFocusedRow="True" AllowDragDrop="False" />
                                    <SettingsPager Visible="False" Mode="EndlessPaging" NumericButtonCount="50" PageSize="50">
                                    </SettingsPager>
                                    <SettingsEditing EditFormColumnCount="99">
                                    </SettingsEditing>
                                    <Settings VerticalScrollBarMode="Visible" ShowFooter="False" ShowTitlePanel="True" VerticalScrollableHeight="470" />
                                    <SettingsLoadingPanel Delay="500" Mode="ShowAsPopup" />
                                    <SettingsDataSecurity AllowDelete="False" AllowInsert="False" AllowEdit="False" />
                        </dx:ASPxGridView>
                        <dx:ASPxButton ID="ASPxButton1" runat="server" OnClick="ASPxButton1_Click" Text="ASPxButton">
                        </dx:ASPxButton>
</ContentTemplate>
</asp:UpdatePanel>
    </div>
        <div>  
        <asp:UpdatePanel ID="update" runat="server">  
            <ContentTemplate>  
                <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>  
                <asp:Button ID="Button1" runat="server" Text="Button1" OnClick="Button1_Click" />  


                <asp:UpdateProgress ID="progress" runat="server" AssociatedUpdatePanelID="update">  
                    <ProgressTemplate>  
                        <span style="color: Red;" mce_style="color: Red;"><strong>数据加载中....</strong> 
                        <br />
                        </span>  
                    </ProgressTemplate>  
                </asp:UpdateProgress>  

            </ContentTemplate>  
        </asp:UpdatePanel>  

    </form>
</body>
</html>
