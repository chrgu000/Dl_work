<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SysSetting.aspx.cs" Inherits="SysSetting" %>

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
        <a style="float:left;">请先选择顾客:</a>
                        <div style="float:left;margin-right:20px">
                <dx:ASPxComboBox ID="ComboBoxPhone" ClientInstanceName="ComboBoxPhone" runat="server"   EnableTheming="True" Theme="SoftOrange">
                </dx:ASPxComboBox></div>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <dx:ASPxButton ID="BtnCustomerPhone" ClientInstanceName="BtnCustomerPhone" runat="server" Text="查 询" Theme="Youthful" OnClick="BtnCustomerPhone_Click">
                </dx:ASPxButton>
                <dx:ASPxGridView ID="PhoneGrid" ClientInstanceName="PhoneGrid" runat="server" AutoGenerateColumns="False" EnableTheming="True" KeyFieldName="Id" 
                    OnCustomUnboundColumnData="PhoneGrid_CustomUnboundColumnData" OnRowDeleting="PhoneGrid_RowDeleting" OnRowInserting="PhoneGrid_RowInserting" OnRowUpdating="PhoneGrid_RowUpdating" Theme="SoftOrange" style="margin-bottom: 2px">
                    <Columns>
                        <dx:GridViewCommandColumn ShowDeleteButton="True" ShowEditButton="True" ShowNewButtonInHeader="True" VisibleIndex="0" Width="100px">
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataTextColumn Caption="手机号码" FieldName="PhoneNo" VisibleIndex="2" Width="300px">
                            <PropertiesTextEdit>
                                <ValidationSettings Display="Dynamic" SetFocusOnError="True" ErrorDisplayMode="ImageWithText">
                                    <RegularExpression ErrorText="请输入有效的手机号码!" ValidationExpression="^1[34578][0-9]{9}$" />
                                    <RequiredField IsRequired="True" />
                                </ValidationSettings>
                            </PropertiesTextEdit>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="序号" FieldName="hh" ReadOnly="True" UnboundType="Integer" VisibleIndex="1">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="Id" ReadOnly="True" VisibleIndex="3" Width="0px">
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <SettingsBehavior AllowSort="False" />
                    <SettingsPager Visible="False">
                    </SettingsPager>
                    <SettingsEditing EditFormColumnCount="1" Mode="PopupEditForm">
                    </SettingsEditing>
                    <SettingsText EmptyDataRow="还没有手机号码,添加一个!" />
                </dx:ASPxGridView>
                <asp:HiddenField ID="HF" runat="server" Value="0" />
                <asp:HiddenField ID="HFcCusCode" runat="server" Value="0" />
                <br />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="BtnCustomerPhone" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>

        <div>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server"><ContentTemplate>
    <a>★账单日期范围为0~31,账单日期的凌晨进行生成账单并且发送! </a>
                <br>
                <br></br>
                <a>当设置为0时,则取消自动发送账单,如选择31,遇到当月没有31号时,则在当月最后一天凌晨发送账单!</a>
                <dx:ASPxGridView ID="SOAGrid" runat="server" AutoGenerateColumns="False" ClientInstanceName="SOAGrid" EnableTheming="True" KeyFieldName="cCusCode" OnHtmlDataCellPrepared="SOAGrid_HtmlDataCellPrepared" OnRowUpdating="SOAGrid_RowUpdating" Theme="SoftOrange">
                    <Columns>
                        <dx:GridViewCommandColumn ShowClearFilterButton="True" VisibleIndex="0" Width="60px">
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataTextColumn Caption="U8顾客编号" FieldName="cCusCode" ReadOnly="True" VisibleIndex="2" Width="80px">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="U8顾客名称" FieldName="cCusName" ReadOnly="True" VisibleIndex="3" Width="220px">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="当前账单生成发送日期" FieldName="SOASendTime" VisibleIndex="4" Width="150px">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataSpinEditColumn Caption="新账单日期" FieldName="NewSendDate" UnboundType="Integer" VisibleIndex="5" Width="80px">
                            <PropertiesSpinEdit DisplayFormatString="g" MaxValue="31" NullDisplayText="0" NullText="0" NumberType="Integer">
                            </PropertiesSpinEdit>
                            <CellStyle BackColor="#FF9900">
                            </CellStyle>
                        </dx:GridViewDataSpinEditColumn>
                        <dx:GridViewDataTextColumn Caption="系统登录用户" FieldName="strLoginName" ReadOnly="True" VisibleIndex="1" Width="95px">
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <SettingsBehavior FilterRowMode="OnClick" />
                    <SettingsPager Mode="ShowAllRecords" Visible="False">
                    </SettingsPager>
                    <SettingsEditing Mode="Batch">
                    </SettingsEditing>
                    <Settings ShowFilterRow="True" ShowFilterRowMenu="True" VerticalScrollableHeight="350" VerticalScrollBarMode="Auto" VerticalScrollBarStyle="Virtual" />
                </dx:ASPxGridView>
                </br>
    </ContentTemplate>
            </asp:UpdatePanel>
        </div>

    
    
    </div>
    </form>
</body>
</html>
