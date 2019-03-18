<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CheckPendingOrder_Y.aspx.cs" Inherits="CheckPendingOrder_Y" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>待审核订单-酬宾</title>
    </head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <dx:ASPxButton ID="BtnRefresh" runat="server" Text="刷新" OnClick="BtnRefresh_Click"></dx:ASPxButton>

            <dx:ASPxGridView ID="GridOrder" ClientInstanceName="GridOrder" runat="server" AutoGenerateColumns="False" OnCustomButtonCallback="GridOrder_CustomButtonCallback"
                EnableTheming="True" Theme="Office2010Blue" KeyFieldName="strBillNo" Font-Size="11pt">
                <Columns>
                    <dx:GridViewDataTextColumn Caption="客户名称" FieldName="strUserName" VisibleIndex="4" Width="150px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="开票单位名称" FieldName="ccusname" VisibleIndex="5" Width="160px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataHyperLinkColumn Caption="网单号" FieldName="strBillNo" VisibleIndex="2" Width="160px">
                        <PropertiesHyperLinkEdit NavigateUrlFormatString="CheckPendingOrder.aspx?vbillno={0}">
                            <Style ForeColor="#0066FF">
                            </Style>
                        </PropertiesHyperLinkEdit>
                    </dx:GridViewDataHyperLinkColumn>
                    <dx:GridViewDataTextColumn Caption="单据日期" FieldName="datCreateTime" VisibleIndex="3" Width="120px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataHyperLinkColumn Caption="操作" VisibleIndex="1" FieldName="strBillNo" Width="50px">
                        <PropertiesHyperLinkEdit NavigateUrlFormatString="CheckPendingOrder.aspx?cbillno={0}" Text="催办">
                            <Style ForeColor="#ff00ff">
                            </Style>
                        </PropertiesHyperLinkEdit>
                    </dx:GridViewDataHyperLinkColumn>
                </Columns>
                <SettingsBehavior AllowSort="False" />
                <SettingsPager Mode="ShowAllRecords" Visible="False">
                </SettingsPager>
                <Settings VerticalScrollableHeight="150" VerticalScrollBarMode="Auto" />
            </dx:ASPxGridView>
            <br />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>

                    <table align="left" style="width: 100%; float: none;">
                        <tr>
                            <td style="width: 80px;">网单号</td>
                            <td style="width: 150px;">
                                <dx:ASPxTextBox ID="TxtOrderBillNo" runat="server" Width="145px" ReadOnly="True" Theme="Office2010Blue">
                                    <ReadOnlyStyle BackColor="white">
                                    </ReadOnlyStyle>
                                </dx:ASPxTextBox>
                            </td>
                            <td style="width: 80px;">开票单位</td>
                            <td style="width: 150px;">
                                <dx:ASPxTextBox ID="TxtCustomer" runat="server" EnableViewState="true" Height="16px" ReadOnly="True" Text="" Theme="Office2010Blue" Width="145px">
                                    <ReadOnlyStyle BackColor="white">
                                    </ReadOnlyStyle>
                                </dx:ASPxTextBox>
                            </td>
                            <td style="width: 80px;">订单日期</td>
                            <td style="width: 150px;">
                                <dx:ASPxTextBox ID="TxtBillDate" runat="server" Width="145px" ReadOnly="True" Theme="Office2010Blue">
                                    <ReadOnlyStyle BackColor="white" />
                                </dx:ASPxTextBox>
                            </td>
                            <td></td>
                        </tr>
                    </table>
                    <dx:ASPxGridView ID="ViewOrderGrid" ClientInstanceName="ViewOrderGrid" runat="server" AutoGenerateColumns="False" EnableTheming="True" KeyFieldName="irowno" Theme="Office2010Blue" Font-Size="11pt">
                        <SettingsPager Visible="False" Mode="ShowAllRecords">
                        </SettingsPager>
                        <Settings ShowTitlePanel="true" ShowFooter="True" VerticalScrollableHeight="280" VerticalScrollBarMode="Auto" />
                        <SettingsBehavior AllowSort="false" />
                        <SettingsText Title="订单明细表" />
                        <TotalSummary>
                            <dx:ASPxSummaryItem FieldName="irowno" SummaryType="Count" />
                            <dx:ASPxSummaryItem FieldName="cComUnitAmount" SummaryType="Sum" />
                        </TotalSummary>
                        <Columns>
                            <dx:GridViewDataTextColumn Caption="序号" FieldName="irowno" ReadOnly="True" VisibleIndex="0" SortIndex="0" SortOrder="Ascending" UnboundType="Integer">
                                <Settings AllowSort="True" SortMode="Value" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="名称" FieldName="cinvname" ReadOnly="True" VisibleIndex="1" Width="160px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="规格" FieldName="cInvStd" ReadOnly="True" VisibleIndex="2" Width="100px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="数量" FieldName="iquantity" ReadOnly="True" VisibleIndex="3" Width="60px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="包装结果" FieldName="cdefine22" ReadOnly="True" VisibleIndex="4" Width="150px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="金额" FieldName="cComUnitAmount" ReadOnly="True" VisibleIndex="6" Width="100px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="单价" FieldName="iquotedprice" VisibleIndex="5" Width="65px">
                            </dx:GridViewDataTextColumn>
                        </Columns>
                        <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
                    </dx:ASPxGridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
