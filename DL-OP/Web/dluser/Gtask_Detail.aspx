<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Gtask_Detail.aspx.cs" Inherits="dluser_Gtask_Detail" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>

</head>
<body>
    <form id="form1" runat="server">
        <div>

            <table style="width: 100%; float: left;display:none">
                <tr>
                    <td>
                        <div style="margin-bottom:5px">
                        <div style="float:left">
                            <a style="float:left">网单号</a><dx:ASPxTextBox ID="TxtOrderBillNo" runat="server" Width="145px"  ReadOnly="True"    >
                                <ReadOnlyStyle BackColor="White">
                                </ReadOnlyStyle>
                            </dx:ASPxTextBox>
                        </div>
                        <div style="float:left">
                            <a style="float:left">制单人</a><dx:ASPxTextBox ID="TxtBiller" runat="server" Width="145px"  EnableTheming="True" ReadOnly="True"    >
                                <ReadOnlyStyle BackColor="White">
                                </ReadOnlyStyle>
                            </dx:ASPxTextBox>
                        </div>
                        <div style="float:left">
                            <a style="float:left">订单日期</a><dx:ASPxTextBox ID="TxtBillDate" runat="server" Width="145px"  ReadOnly="True"    >
                                <ReadOnlyStyle BackColor="White">
                                </ReadOnlyStyle>
                            </dx:ASPxTextBox>
                        </div>
                        <div style="float:left; height: 17px;">
                            <a style="float:left">开票单位</a><div style="float: left">
                                <dx:ASPxTextBox ID="TxtCustomer" runat="server" Width="125px" ReadOnly="True"  Text=""  EnableViewState="true"  >
                                    <ReadOnlyStyle BackColor="White">
                                    </ReadOnlyStyle>
                                </dx:ASPxTextBox>
                            </div>

                        </div>
                        <div style="float:left">
                            <a style="float:left">业务员</a><dx:ASPxTextBox ID="TxtSalesman" ClientInstanceName="TxtSalesman" runat="server" Width="145px"  EnableTheming="True"    ReadOnly="true">
                            </dx:ASPxTextBox>

                        </div>
                        <div style="float:left">
                           <a style="float:left">信用额</a><dx:ASPxTextBox ID="TxtCusCredit" ClientInstanceName="TxtCusCredit" runat="server" Width="145px"  EnableTheming="True" ReadOnly="True"    >
                                <ReadOnlyStyle BackColor="White">
                                </ReadOnlyStyle>
                            </dx:ASPxTextBox>
                            </div>
     </div>
                        <div style="margin-bottom:5px">
                        <div >
                            <a style="float:left">送货方式</a> 
                                <dx:ASPxTextBox ID="TxtOrderShippingMethod" runat="server" Width="600px"   ReadOnly="true"  >
                                </dx:ASPxTextBox>
                            </div>
                            <div style="float:left">
                                <a style="float:left">发运方式</a>
                                <dx:ASPxTextBox ID="TxtcSCCode" ClientInstanceName="TxtcSCCode" runat="server" Width="145px"  EnableTheming="True" ReadOnly="True"    >
                                    <ReadOnlyStyle BackColor="White">
                                    </ReadOnlyStyle>
                                </dx:ASPxTextBox>
                            </div>
                           </div> 
                        <div>
                            <div  style="float:left">
                                <a style="float:left">备注</a><dx:ASPxTextBox ID="TxtOrderMark" runat="server" Width="185px"   >
                                    <ClientSideEvents LostFocus="function(s, e) {
	SetCookieText(s.GetText());
}" />
                                </dx:ASPxTextBox>
                            </div>
                            <div style="float:left">
                               <a style="float:left">车型</a> 
                                    <dx:ASPxTextBox ID="Txtcdefine3" runat="server" Width="115px" ReadOnly="True"  Text=""  EnableViewState="true"   >
                                        <ReadOnlyStyle BackColor="White">
                                        </ReadOnlyStyle>
                                    </dx:ASPxTextBox>
                                </div>
                                <div style="float:left">
                                    <a style="float:left">装车方式</a><dx:ASPxTextBox ID="TxtLoadingWays" runat="server" Width="145px"    >
                                        <ClientSideEvents LostFocus="function(s, e) {
	SetCookieTextTxtLoadingWays(s.GetText());
}" />
                                    </dx:ASPxTextBox>
                                </div>
                                <div style="float:left">
                                    <a style="float:left">下单时间</a><dx:ASPxTextBox ID="TxtBillTime" runat="server" Width="145px"  ReadOnly="True"    >
                                        <ReadOnlyStyle BackColor="White">
                                        </ReadOnlyStyle>
                                    </dx:ASPxTextBox>
                                </div>
                                <div style="float:left">
                                    <a style="float:left">提货时间</a><dx:ASPxTextBox ID="TxtDeliveryDate" runat="server"  ClientInstanceName="TxtDeliveryDate"   EnableTheming="True"  ReadOnly="True"  Width="145px">
                                        <ReadOnlyStyle BackColor="White">
                                        </ReadOnlyStyle>
                                    </dx:ASPxTextBox>
                                </div>
                                <div style="float:left">
                                    <a style="float:left">销售类型</a><dx:ASPxTextBox ID="TxtcSTCode" runat="server"  ClientInstanceName="TxtcSTCode"   EnableTheming="True"  ReadOnly="True"  Width="145px">
                                        <ReadOnlyStyle BackColor="White">
                                        </ReadOnlyStyle>
                                    </dx:ASPxTextBox>
                                </div>
                            <br />
                            </div>
                    </td>
                </tr>
            </table>

            <table>
                <tr>
                    <td>    <dx:ASPxMemo ID="ASPxMemo1" runat="server" Height="85px" Width="1150px" Font-Size="12" ReadOnly="true">
                            </dx:ASPxMemo>
                            </td>
                </tr>
            </table>

            <dx:ASPxGridView ID="ViewOrderGrid" ClientInstanceName="ViewOrderGrid" runat="server" AutoGenerateColumns="False" EnableTheming="True"
                KeyFieldName="irowno"  Font-Size="9pt">
                <SettingsPager Visible="False" Mode="ShowAllRecords">
                </SettingsPager>
                <Settings ShowTitlePanel="true" ShowFooter="True" VerticalScrollableHeight="200" VerticalScrollBarMode="Auto" />
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
                    <dx:GridViewDataTextColumn Caption="包装结果" FieldName="cdefine22" ReadOnly="True" VisibleIndex="5" Width="150px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="金额" FieldName="cComUnitAmount" ReadOnly="True" VisibleIndex="7" Width="100px" Visible="False">
                        <PropertiesTextEdit DisplayFormatString="{0:F}">
                        </PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="单价" FieldName="iquotedprice" VisibleIndex="6" Width="65px" Visible="False">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="执行价格" FieldName="itaxunitprice" ReadOnly="True" VisibleIndex="8" Width="65px" Visible="true">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="执行金额" FieldName="isum" ReadOnly="True" VisibleIndex="9" Width="120px" Visible="true">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="包装规格" FieldName="cPackingType" VisibleIndex="4" Width="160px">
                    </dx:GridViewDataTextColumn>
                </Columns>
                <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
            </dx:ASPxGridView>

        </div>
    </form>
</body>
</html>
