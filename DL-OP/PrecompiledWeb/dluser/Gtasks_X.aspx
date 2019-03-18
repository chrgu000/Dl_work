<%@ page language="C#" autoeventwireup="true" inherits="dluser_Gtasks_X, dlopwebdll" enableviewstate="false" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>待办工作-特殊订单</title>
<style type="text/css">
        .auto-style2 {
            width: 200px;
            height: 23px;
        }

        .auto-style3 {
            width: 60px;
            height: 23px;
            font-size: small;
        }

        .auto-style4 {
            width: auto;
            height: 23px;
        }

        .auto-style5 {
            height: 25px;
        }

        .auto-style6 {
            height: 20px;
            font-size: small;
        }

        .auto-style7 {
            width: 200px;
            height: 20px;
        }

        .dxeBase {
            font: 12px Tahoma, Geneva, sans-serif;
        }

        .auto-style13 {
            height: 10px;
        }

        .auto-style15 {
            width: 70px;
            height: 23px;
            font-size: small;
        }

        .auto-style16 {
            font-size: small;
            height: 20px;
        }

        .auto-style18 {
            height: 25px;
            font-size: small;
        }

        .auto-style19 {
            height: 10px;
            font-size: small;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <dx:ASPxButton ID="BtnRefresh" runat="server" Text="刷新" OnClick="BtnRefresh_Click"></dx:ASPxButton>

            <dx:ASPxGridView ID="GridOrder" ClientInstanceName="GridOrder" runat="server" AutoGenerateColumns="False" OnCustomButtonCallback="GridOrder_CustomButtonCallback"
                EnableTheming="True" Theme="Office2010Blue" KeyFieldName="strBillNo">
                <Columns>
                    <dx:GridViewDataTextColumn Caption="客户名称" FieldName="strUserName" VisibleIndex="6" Width="160px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="开票单位名称" FieldName="ccusname" VisibleIndex="7" Width="160px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataHyperLinkColumn Caption="审核" FieldName="strBillNo" VisibleIndex="3" Width="60px">
                        <PropertiesHyperLinkEdit NavigateUrlFormatString="Gtasks_X.aspx?billno={0}" Text="审核">
                            <Style ForeColor="#009933">
                            </Style>
                        </PropertiesHyperLinkEdit>
                    </dx:GridViewDataHyperLinkColumn>
                    <dx:GridViewDataHyperLinkColumn Caption="驳回" FieldName="strBillNo" VisibleIndex="2" Width="60px" Visible="False">
                        <PropertiesHyperLinkEdit NavigateUrlFormatString="Gtasks_X.aspx?dbillno={0}" Text="驳回">
                            <Style ForeColor="Red">
                            </Style>
                        </PropertiesHyperLinkEdit>
                    </dx:GridViewDataHyperLinkColumn>
                    <dx:GridViewDataHyperLinkColumn Caption="网单号" FieldName="strBillNo" VisibleIndex="4" Width="160px">
                        <PropertiesHyperLinkEdit NavigateUrlFormatString="Gtasks_X.aspx?vbillno={0}">
                            <Style ForeColor="#0066FF">
                            </Style>
                        </PropertiesHyperLinkEdit>
                    </dx:GridViewDataHyperLinkColumn>
                    <dx:GridViewDataTextColumn Caption="单据日期" FieldName="ddate" VisibleIndex="5" Width="150px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataHyperLinkColumn Caption="修改" FieldName="strBillNo" VisibleIndex="1" Visible="False" Width="0px">
                        <PropertiesHyperLinkEdit NavigateUrlFormatString="OrderFrameDlUser.aspx?billno={0}" Text="修改">
                                                       <Style ForeColor="blue">
                            </Style>
                        </PropertiesHyperLinkEdit>
                    </dx:GridViewDataHyperLinkColumn>
                    <dx:GridViewDataHyperLinkColumn Caption="作废" FieldName="strBillNo" VisibleIndex="0" Width="60px">
                        <PropertiesHyperLinkEdit NavigateUrlFormatString="Gtasks_X.aspx?Ibillno={0}" Text="作废">
                            <Style ForeColor="Red">
                            </Style>
                        </PropertiesHyperLinkEdit>
                    </dx:GridViewDataHyperLinkColumn>
                </Columns>
                <SettingsBehavior AllowSort="False" />
                <SettingsPager Mode="ShowAllRecords" Visible="False">
                </SettingsPager>
                <Settings VerticalScrollableHeight="260" VerticalScrollBarMode="Visible" />
            </dx:ASPxGridView>
            <br />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>

                    <table align="left" style="width: 100%; float: none;">
                <tr>
                    <td class="auto-style15">网单号</td>
                    <td class="auto-style2">
                        <dx:ASPxTextBox ID="TxtOrderBillNo" runat="server" Width="145px" Height="18px" ReadOnly="True" Theme="Office2010Blue"  CssClass="auto-style16" BackColor="#3366FF">
                            <ReadOnlyStyle BackColor="White">
                            </ReadOnlyStyle>
                        </dx:ASPxTextBox>
                    </td>
                    <td class="auto-style3">下单时间</td>
                    <td class="auto-style2">
                        <dx:ASPxTextBox ID="TxtBillTime" runat="server" BackColor="#3366FF" CssClass="auto-style16" Height="18px" ReadOnly="True" Theme="Office2010Blue" Width="145px">
                            <ReadOnlyStyle BackColor="White">
                            </ReadOnlyStyle>
                        </dx:ASPxTextBox>
                    </td>
                    <td class="auto-style15">订单日期</td>
                    <td class="auto-style2">
                        <dx:ASPxTextBox ID="TxtBillDate" runat="server" Width="145px" Height="18px" ReadOnly="True" Theme="Office2010Blue" CssClass="auto-style16" BackColor="#3366FF">
                            <ReadOnlyStyle BackColor="White">
                            </ReadOnlyStyle>
                        </dx:ASPxTextBox>
                    </td>
                    <td class="auto-style4"></td>
                </tr>
                <tr>
                    <td class="auto-style19">开票单位</td>
                    <td class="auto-style13">
                        <div style="float: left">
                            <dx:ASPxTextBox ID="TxtCustomer" runat="server" Width="125px" ReadOnly="True" Theme="Office2010Blue" Text="" Height="18px" EnableViewState="true" CssClass="auto-style16" BackColor="#3366FF">
                                <ReadOnlyStyle BackColor="White">
                                </ReadOnlyStyle>
                            </dx:ASPxTextBox>
                        </div>
                        <div style="float: left; margin-left: 4px">
                        </div>


                    </td>
                    <td class="auto-style19">&nbsp;</td>
                    <td class="auto-style13">
                        &nbsp;</td>
                    <td class="auto-style19">&nbsp;</td>
                    <td class="auto-style13">
                        &nbsp;</td>
                    <td class="auto-style13"></td>
                </tr>
            </table>
                    <dx:ASPxGridView ID="ViewOrderGrid" ClientInstanceName="ViewOrderGrid" runat="server" AutoGenerateColumns="False" EnableTheming="True" 
                        KeyFieldName="irowno" Theme="Office2010Blue">
                        <SettingsPager Visible="False" Mode="ShowAllRecords">
                        </SettingsPager>
                        <Settings ShowTitlePanel="true" ShowFooter="True" VerticalScrollableHeight="280" VerticalScrollBarMode="Auto" />
                        <SettingsBehavior AllowSort="false" />
                        <SettingsText Title="订单明细表" />
                        <TotalSummary>
                            <dx:ASPxSummaryItem FieldName="irowno" SummaryType="Count" />
                            <dx:ASPxSummaryItem FieldName="cComUnitAmount" SummaryType="Sum" />
                            <dx:ASPxSummaryItem FieldName="isum" SummaryType="Sum" />
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
                            <dx:GridViewDataTextColumn Caption="执行价格" FieldName="itaxunitprice" ReadOnly="True" VisibleIndex="7" Width="65px" Visible="False">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="执行金额" FieldName="isum" ReadOnly="True" VisibleIndex="8" Width="120px" Visible="False">
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
