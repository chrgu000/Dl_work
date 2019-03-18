<%@ page language="C#" autoeventwireup="true" inherits="CheckPendingOrder, dlopwebdll" enableviewstate="false" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>待审核订单</title>
    <style type="text/css">
        
        .auto-style3 {
            height: 25px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <dx:ASPxButton ID="BtnRefresh" runat="server" Text="刷新" OnClick="BtnRefresh_Click" Width="132px"></dx:ASPxButton>
            
            <dx:ASPxGridView ID="GridOrder" ClientInstanceName="GridOrder" runat="server" AutoGenerateColumns="False" OnCustomButtonCallback="GridOrder_CustomButtonCallback"
                EnableTheming="True" Theme="Office2010Blue" KeyFieldName="strBillNo" Font-Size="11pt" Width="1200px">
                <Columns>
                    <dx:GridViewDataTextColumn Caption="客户名称" FieldName="strUserName" VisibleIndex="4" Width="100px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="开票单位名称" FieldName="ccusname" VisibleIndex="5" Width="150px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="地址信息" FieldName="cDefine11" VisibleIndex="6" Width="350px" /> 
                    <dx:GridViewDataTextColumn Caption="备注" FieldName="strRemarks" VisibleIndex="7" Width="180px">
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
                        <CellStyle Font-Size="9pt">
                        </CellStyle>
                    </dx:GridViewDataHyperLinkColumn>
<%--                    <dx:GridViewDataHyperLinkColumn Caption="取回" FieldName="strBillNo" ToolTip="未被接受处理的待审核订单，可以进行取回，取回后的订单，将出现在被驳回订单中" VisibleIndex="0" Width="55px">
                        <PropertiesHyperLinkEdit Text="取回" NavigateUrlFormatString="CheckPendingOrder.aspx?qhid={0}">
                            <ClientSideEvents Click="function(s, e) {

}" />
                            <Style ForeColor="#009900">
                            </Style>
                        </PropertiesHyperLinkEdit>
                    </dx:GridViewDataHyperLinkColumn>--%>
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
                            <td style="width: 60px;">制单人</td>
                            <td style="width: 150px;">
                                <dx:ASPxTextBox ID="TxtBiller" runat="server" Width="145px" EnableTheming="True" ReadOnly="True" Theme="Office2010Blue">
                                    <ReadOnlyStyle BackColor="white" />
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
                        <tr>
                            <td>开票单位</td>
                            <td>
                                <div style="float: left">
                                    <dx:ASPxTextBox ID="TxtCustomer" runat="server" Width="145px" ReadOnly="True" Theme="Office2010Blue" Text="" Height="16px" EnableViewState="true">
                                        <ReadOnlyStyle BackColor="white">
                                        </ReadOnlyStyle>
                                    </dx:ASPxTextBox>
                                </div>
                                <div style="float: left; margin-left: 4px">
                                </div>


                            </td>
                            <td>业务员</td>
                            <td>
                                <dx:ASPxTextBox ID="TxtSalesman" ClientInstanceName="TxtSalesman" ReadOnly="True" runat="server" Width="145px" EnableTheming="True" Theme="Office2010Blue">
                                    <ReadOnlyStyle BackColor="white" />
                                </dx:ASPxTextBox>


                            </td>
                            <td>发运方式</td>
                            <td>
                                <dx:ASPxTextBox ID="TxtcSCCode" runat="server" ClientInstanceName="TxtcSCCode" EnableTheming="True" ReadOnly="True" style="margin-bottom: 0px" Theme="Office2010Blue" Width="145px">
                                    <ReadOnlyStyle BackColor="white" />
                                </dx:ASPxTextBox>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>送货方式</td>
                            <td colspan="6">
                                <div style="float: left">
                                    <dx:ASPxTextBox ID="TxtOrderShippingMethod" ReadOnlyStyle-BackColor="white" runat="server" Width="650px" Theme="Office2010Blue" ReadOnly="true">
                                    </dx:ASPxTextBox>
                                </div>


                            </td>
                        </tr>
                        <tr>
                            <td>备注</td>
                            <td colspan="6">
                                <dx:ASPxTextBox ID="TxtOrderMark" ReadOnlyStyle-BackColor="white" runat="server" Width="650px" Theme="Office2010Blue" ReadOnly="true">
                                </dx:ASPxTextBox>
                            </td>
                        </tr>
                    </table>
                    <dx:ASPxGridView ID="ViewOrderGrid" ClientInstanceName="ViewOrderGrid" runat="server" AutoGenerateColumns="False" EnableTheming="True" KeyFieldName="irowno" Theme="Office2010Blue" Font-Size="11pt" Width="900px">
                        <SettingsPager Visible="False" Mode="ShowAllRecords">
                        </SettingsPager>
                        <Settings ShowTitlePanel="true" ShowFooter="True" VerticalScrollableHeight="210" VerticalScrollBarMode="Auto" />
                        <SettingsBehavior AllowSort="false" />
                        <SettingsText Title="订单明细表" />
                        <TotalSummary>
                            <dx:ASPxSummaryItem FieldName="irowno" SummaryType="Count" />
                            <dx:ASPxSummaryItem FieldName="xx" SummaryType="Sum" DisplayFormat="合计:{0:F}" ShowInColumn="金额" ShowInGroupFooterColumn="金额" />
                        </TotalSummary>
                        <Columns>
                            <dx:GridViewDataTextColumn Caption="序号" FieldName="irowno" ReadOnly="True" VisibleIndex="0" SortIndex="0" SortOrder="Ascending" UnboundType="Integer" Width="40px">
                                <Settings AllowSort="False" SortMode="Value" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="名称" FieldName="cinvname" ReadOnly="True" VisibleIndex="1" Width="160px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="规格" FieldName="cInvStd" ReadOnly="True" VisibleIndex="2" Width="130px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="数量" FieldName="iquantity" ReadOnly="True" VisibleIndex="3" Width="80px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="包装结果" FieldName="cdefine22" ReadOnly="True" VisibleIndex="4" Width="170px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="金额" FieldName="cComUnitAmount" ReadOnly="True" VisibleIndex="6" Width="120px">
                                <PropertiesTextEdit DisplayFormatString="{0:F}">
                                </PropertiesTextEdit>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="单价" FieldName="iquotedprice" VisibleIndex="5" Width="80px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="执行单价" FieldName="itaxunitprice" VisibleIndex="7" Width="0px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="执行金额" FieldName="xx" VisibleIndex="8" Width="0px">
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
