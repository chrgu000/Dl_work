<%@ page language="C#" autoeventwireup="true" inherits="PreXOrderSearch, dlopwebdll" enableviewstate="false" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="float: left;margin-right:5px">
            <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="系统编号">
            </dx:ASPxLabel>
        </div>
        <div style="float: left">
        <dx:ASPxTextBox ID="TxtBillNo" ClientInstanceName="TxtBillNo" runat="server" Width="120px">
</dx:ASPxTextBox></div>
        <div style="float: left;margin-right:5px;margin-left: 20px;">
            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="开始日期">
            </dx:ASPxLabel>
        </div>
        <div style="float: left">
            <dx:ASPxDateEdit ID="DatBeginDate" ClientInstanceName="DatBeginDate" runat="server" Width="120">
            </dx:ASPxDateEdit>
        </div>
        <div style="float: left; margin-left: 20px;margin-right:5px">
            <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="截至日期">
            </dx:ASPxLabel>
        </div>
        <div style="float: left">
            <dx:ASPxDateEdit ID="DatEndDate" ClientInstanceName="DatEndDate" runat="server" Width="120">
            </dx:ASPxDateEdit>
        </div>
        <div style="float: left; margin-left: 20px;margin-right:5px">
            <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="订单状态 ">
            </dx:ASPxLabel>
        </div>
        <div style="float: left;margin-right:20px">
            <dx:ASPxComboBox ID="ComboOrderStatus" runat="server" SelectedIndex="0" Width="80" AutoResizeWithContainer="False" ClientInstanceName="ComboOrderStatus">
                <Items>
                    <dx:ListEditItem Selected="True" Text="全部" Value="0" />
                    <dx:ListEditItem Text="已审核" Value="4" />
                    <dx:ListEditItem Text="未审核" Value="1" />
                </Items>
            </dx:ASPxComboBox>
        </div>
        <div style="margin-left:20px">
                <dx:ASPxButton ID="BtnOk" ClientInstanceName="BtnOk" runat="server" Text="查询" Width="80" OnClick="BtnOk_Click">
                </dx:ASPxButton>
                            </div>
        <div>
                <dx:ASPxGridView ID="Grid" runat="server" AutoGenerateColumns="False" EnableTheming="True" KeyFieldName="strBillNo" Theme="Office2010Blue" Font-Size="10pt">
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="正式订单号" FieldName="cSOCode" VisibleIndex="1" Width="130px">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="状态" FieldName="bytStatus" VisibleIndex="2" Width="70px">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="开票单位" FieldName="ccusname" VisibleIndex="4" Width="200px">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="提交日期" FieldName="xdsj" VisibleIndex="3" Width="120px">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataHyperLinkColumn Caption="网单号" FieldName="strBillNo" VisibleIndex="0" Width="130px">
                            <PropertiesHyperLinkEdit NavigateUrlFormatString="PreXOrderDetail.aspx?id={0}" Target="OrderCenter">
                            </PropertiesHyperLinkEdit>
                        </dx:GridViewDataHyperLinkColumn>
                    </Columns>
                    <SettingsBehavior AllowSort="False" />
                    <SettingsPager Visible="False" Mode="ShowAllRecords">
                    </SettingsPager>
                    <Settings VerticalScrollableHeight="150" VerticalScrollBarMode="Auto" />
                    <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
                </dx:ASPxGridView>
            </div>
        <div style="width:100%;height:600px">
                        <iframe id="OrderCenter" height="100%" width="100%" frameborder="0" name="OrderCenter" src="PreXOrderDetail.aspx"></iframe>
        </div>
    </form>
</body>
</html>
