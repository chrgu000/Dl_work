<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Ptasks.aspx.cs" Inherits="dluser_Ptasks" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>预订单-已办工作</title>
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
        <div >
            <dx:ASPxComboBox ID="ComboOrderStatus" runat="server" SelectedIndex="0" Width="80" AutoResizeWithContainer="False" ClientInstanceName="ComboOrderStatus">
                <Items>
                    <dx:ListEditItem Selected="True" Text="全部" Value="0" />
                    <dx:ListEditItem Text="已审核" Value="4" />
                    <dx:ListEditItem Text="未审核" Value="1" />
                </Items>
            </dx:ASPxComboBox>

                <dx:ASPxButton ID="BtnOk" ClientInstanceName="BtnOk" runat="server" Text="查询" Width="80" OnClick="BtnOk_Click">
                </dx:ASPxButton>
                            </div>
        <div>
                <dx:ASPxGridView ID="Grid" runat="server" AutoGenerateColumns="False" EnableTheming="True" KeyFieldName="strBillNo" Theme="Office2010Blue">
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="系统编号" FieldName="strBillNo" VisibleIndex="0" Width="150px">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="正式订单号" FieldName="cSOCode" VisibleIndex="1" Width="150px">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="状态" FieldName="bytStatus" VisibleIndex="2" Width="60px">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="开票单位" FieldName="ccusname" VisibleIndex="5" Width="180px">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="提交日期" FieldName="ddate" VisibleIndex="4" Width="120px">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="单据类型" FieldName="lngBillType" VisibleIndex="3" Width="80px">
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <SettingsPager Visible="False" Mode="ShowAllRecords">
                    </SettingsPager>
                    <Settings VerticalScrollableHeight="350" VerticalScrollBarMode="Auto" />
                    <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
                </dx:ASPxGridView>
            </div>
    </form>
</body>
</html>
