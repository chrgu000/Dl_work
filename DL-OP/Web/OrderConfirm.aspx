<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrderConfirm.aspx.cs" Inherits="OrderConfirm" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>待确认订单</title>
    <style type="text/css">
        .auto-style4 {
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
              <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server"><ContentTemplate>--%>
            <dx:ASPxGridView ID="GridOrder" ClientInstanceName="GridOrder" runat="server" AutoGenerateColumns="False" OnCustomButtonCallback="GridOrder_CustomButtonCallback"
                EnableTheming="True" Theme="Office2010Blue" KeyFieldName="strBillNo" Font-Size="10pt" Width="1200px" >
                <Columns>
                    <dx:GridViewDataTextColumn Caption="客户名称" FieldName="strUserName" VisibleIndex="5" Width="150px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="开票单位名称" FieldName="ccusname" VisibleIndex="6" Width="150px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="地址信息" FieldName="cDefine11" VisibleIndex="7" Width="200px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="备注" FieldName="strRemarks" VisibleIndex="8" Width="200px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataHyperLinkColumn Caption="操作" FieldName="strBillNo" VisibleIndex="2" Width="80px" ToolTip="对修改后的订单进行确认,确认后将直接生成正式订单.">
                        <PropertiesHyperLinkEdit NavigateUrlFormatString="OrderConfirm.aspx?billno={0}" Text="确认订单">
                            <Style ForeColor="#009933">
                            </Style>
                        </PropertiesHyperLinkEdit>
                    </dx:GridViewDataHyperLinkColumn>
                    <dx:GridViewDataHyperLinkColumn Caption="网单号" FieldName="strBillNo" VisibleIndex="3" Width="140px" ToolTip="点击后可以查看该订单信息,并且可以修改该订单状态!">
                        <PropertiesHyperLinkEdit NavigateUrlFormatString="OrderConfirm.aspx?vbillno={0}">
                            <Style ForeColor="#0066FF">
                            </Style>
                        </PropertiesHyperLinkEdit>
                    </dx:GridViewDataHyperLinkColumn>
                    <dx:GridViewDataTextColumn Caption="单据日期" FieldName="datCreateTime" VisibleIndex="4" Width="120px">
                    </dx:GridViewDataTextColumn>
                </Columns>
                <SettingsBehavior AllowSort="False" />
                <SettingsPager Visible="False" Mode="ShowAllRecords">
                </SettingsPager>
                <Settings VerticalScrollableHeight="190" VerticalScrollBarMode="Visible" VerticalScrollBarStyle="Virtual" />
            </dx:ASPxGridView>
                  <br />
                  <dx:ASPxButton ID="BtnModifyOrder" runat="server" ClientInstanceName="BtnModifyOrder" EnableTheming="True" Height="40px" OnClick="BtnModifyOrder_Click" Text="修改订单" Theme="SoftOrange" Width="130px" ToolTip="将该订单的状态变更为'被驳回'状态,可以在被驳回订单中找到该订单并且编辑后提交审核或者作废该订单.">
                      <ClientSideEvents Click="function(s, e) {
e.processOnServer=confirm('确定要修改该订单?确定修改后该订单状态为被驳回状态!'); 	
}" />
                  </dx:ASPxButton>
<%--                  </ContentTemplate></asp:UpdatePanel>--%>
            <br />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>

                    <table align="left" style="width: 100%; float: none;">
                        <tr>
                            <td style="width: 80px;" class="auto-style4">网单号</td>
                            <td style="width: 150px;">
                                <dx:ASPxTextBox ID="TxtOrderBillNo" runat="server" Width="145px" ReadOnly="True" Theme="Office2010Blue">
                                    <ReadOnlyStyle BackColor="White">
                                    </ReadOnlyStyle>
                                </dx:ASPxTextBox>
                            </td>
                            <td style="width: 60px;" class="auto-style4">制单人</td>
                            <td style="width: 150px;">
                                <dx:ASPxTextBox ID="TxtBiller" runat="server" Width="145px" EnableTheming="True" ReadOnly="True" Theme="Office2010Blue">
                                    <ReadOnlyStyle BackColor="White" />
                                </dx:ASPxTextBox>
                            </td>
                            <td style="width: 80px;" class="auto-style4">订单日期</td>
                            <td style="width: 150px;">
                                <dx:ASPxTextBox ID="TxtBillDate" runat="server" Width="145px" ReadOnly="True" Theme="Office2010Blue">
                                    <ReadOnlyStyle BackColor="White" />
                                </dx:ASPxTextBox>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td class="auto-style4">开票单位</td>
                            <td>
                                <div style="float: left">
                                    <dx:ASPxTextBox ID="TxtCustomer" runat="server" Width="145px" ReadOnly="True" Theme="Office2010Blue" Text="" Height="16px" EnableViewState="true">
                                        <ReadOnlyStyle BackColor="White">
                                        </ReadOnlyStyle>
                                    </dx:ASPxTextBox>
                                </div>
                                <div style="float: left; margin-left: 4px">
                                </div>


                            </td>
                            <td class="auto-style4">业务员</td>
                            <td>
                                <dx:ASPxTextBox ID="TxtSalesman" ClientInstanceName="TxtSalesman" ReadOnly="True" runat="server" Width="145px" EnableTheming="True" Theme="Office2010Blue">
                                    <ReadOnlyStyle BackColor="White" />
                                </dx:ASPxTextBox>


                            </td>
                            <td class="auto-style4">&nbsp;</td>
                            <td class="auto-style4">&nbsp;</td>
                            <td></td>
                        </tr>
                        <tr>
                            <td class="auto-style4">送货方式</td>
                            <td colspan="5">
                                <div style="float: left">
                                    <dx:ASPxTextBox ID="TxtOrderShippingMethod" ReadOnlyStyle-BackColor="White" runat="server" Width="650px" Theme="Office2010Blue" ReadOnly="true">
                                    </dx:ASPxTextBox>
                                </div>


                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td class="auto-style4">备注</td>
                            <td colspan="5">
                                <dx:ASPxTextBox ID="TxtOrderMark" ReadOnlyStyle-BackColor="White" runat="server" Width="650px" Theme="Office2010Blue" ReadOnly="true">
                                </dx:ASPxTextBox>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td class="auto-style4">发运方式</td>
                            <td>
                                <dx:ASPxTextBox ID="TxtcSCCode" ClientInstanceName="TxtcSCCode" runat="server" Width="145px" EnableTheming="True" ReadOnly="True" Theme="Office2010Blue">
                                    <ReadOnlyStyle BackColor="White" />
                                </dx:ASPxTextBox>


                            </td>
                            <td class="auto-style4">QQ客服</td>
                            <td>
                                <dx:ASPxTextBox ID="TxtQQ" runat="server" ClientInstanceName="TxtQQ" EnableTheming="True" ReadOnly="True" Theme="Office2010Blue" Width="145px">
                                    <ReadOnlyStyle BackColor="white" />
                                </dx:ASPxTextBox>
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>
                    <dx:ASPxGridView ID="ViewOrderGrid" ClientInstanceName="ViewOrderGrid" runat="server" AutoGenerateColumns="False" EnableTheming="True" 
                        KeyFieldName="irowno" Theme="Office2010Blue" Font-Size="9pt">
                        <SettingsPager Visible="False" Mode="ShowAllRecords">
                        </SettingsPager>
                        <Settings ShowTitlePanel="true" ShowFooter="True" VerticalScrollableHeight="170" VerticalScrollBarMode="Auto" />
                        <SettingsBehavior AllowSort="false" />
                        <SettingsText Title="订单明细表" />
                        <TotalSummary>
                            <dx:ASPxSummaryItem FieldName="irowno" SummaryType="Count" />
                            <dx:ASPxSummaryItem FieldName="isum" SummaryType="Sum" DisplayFormat="合计:{0:F}" ShowInColumn="金额" />
                        </TotalSummary>
                        <Columns>
                            <dx:GridViewDataTextColumn Caption="序号" FieldName="irowno" ReadOnly="True" VisibleIndex="0" UnboundType="Integer">
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
                            <dx:GridViewDataTextColumn Caption="执行金额" FieldName="isum" ReadOnly="True" VisibleIndex="7" Width="0px">
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