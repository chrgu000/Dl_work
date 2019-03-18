<%@ page language="C#" autoeventwireup="true" inherits="PendingOrder, dlopwebdll" enableviewstate="false" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>待处理订单(被驳回)</title>
    <style type="text/css">
        .auto-style1 {
            width: 129px;
        }
        .auto-style2 {
            width: 82px;
        }
        .auto-style3 {
            width: 168px;
        }
        .auto-style4 {
            width: 75px;
        }
        .auto-style5 {
            width: 65px;
        }
        .auto-style6 {
            width: 82px;
            height: 10px;
        }
        .auto-style7 {
            width: 129px;
            height: 10px;
        }
        .auto-style8 {
            width: 65px;
            height: 10px;
        }
        .auto-style9 {
            width: 168px;
            height: 10px;
        }
        .auto-style10 {
            width: 75px;
            height: 10px;
        }
        .auto-style11 {
            height: 10px;
        }
    </style>
    </head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <dx:ASPxButton ID="BtnRefresh" runat="server" Text="刷   新" OnClick="BtnRefresh_Click" Height="30px" Width="100px"></dx:ASPxButton>

            <dx:ASPxGridView ID="GridOrder" ClientInstanceName="GridOrder" runat="server" AutoGenerateColumns="False" OnCustomButtonCallback="GridOrder_CustomButtonCallback"
                EnableTheming="True" Theme="Office2010Blue" KeyFieldName="strBillNo" Font-Size="11pt" Width="1300px">
                <Columns>
                    <dx:GridViewDataTextColumn Caption="客户名称" FieldName="strUserName" VisibleIndex="5" Width="150px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="开票单位名称" FieldName="ccusname" VisibleIndex="6" Width="150px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="地址信息" FieldName="cDefine11" VisibleIndex="7" Width="300px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="备注" FieldName="strRemarks" VisibleIndex="8" Width="200px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataHyperLinkColumn Caption="网单号" FieldName="strBillNo" VisibleIndex="0" Width="160px">
                        <PropertiesHyperLinkEdit NavigateUrlFormatString="PendingOrder.aspx?vbillno={0}" TextField="strBillNo">
                            <Style ForeColor="#0066FF">
                            </Style>
                        </PropertiesHyperLinkEdit>
                    </dx:GridViewDataHyperLinkColumn>
                    <dx:GridViewDataTextColumn Caption="单据日期" FieldName="datCreateTime" VisibleIndex="4" Width="120px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="订单类型" FieldName="cSTCode" ReadOnly="True" VisibleIndex="3" Width="80px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataMemoColumn Caption="驳回说明" FieldName="strRejectRemarks" ReadOnly="True" VisibleIndex="1" Width="160px">
                    </dx:GridViewDataMemoColumn>
              <%--      <dx:GridViewDataTextColumn Caption="过期时间" FieldName="datExpTime" ReadOnly="True" VisibleIndex="2" Width="140px">
                    </dx:GridViewDataTextColumn>--%>
                    <dx:GridViewDataTextColumn Caption="到期时间(自动作废)" FieldName="datExpTime" ReadOnly="True" VisibleIndex="2" Width="140px">
                    </dx:GridViewDataTextColumn>
                </Columns>
                <SettingsBehavior AllowSort="False" />
                <SettingsPager Mode="ShowAllRecords">
                </SettingsPager>
            </dx:ASPxGridView>
            <br />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <dx:ASPxHyperLink ID="EditOrder" runat="server" NavigateUrl="~/OrderFrameModify.aspx" Text="编辑订单" BackColor="#FF6600" ClientInstanceName="EditOrder" 
                        EnableTheming="True" Font-Names="微软雅黑" Font-Size="15pt" ForeColor="#3333CC" Height="30px" Target="_self" Theme="Office2010Blue" Width="150px" 
                        Visible="false" />
                    <table align="left" style="width: 1000px; float: none; font-size: small;">
                        <tr>
                            <td style="font-size: 12pt;" class="auto-style2">网单号</td>
                            <td style="font-size: 12pt;" class="auto-style1">
                                <dx:ASPxTextBox ID="TxtOrderBillNo" runat="server" Width="145px" ReadOnly="True" Theme="Office2010Blue" Font-Size="11pt">
                                    <ReadOnlyStyle BackColor="white">
                                    </ReadOnlyStyle>
                                </dx:ASPxTextBox>
                            </td>
                            <td style="font-size: 12pt;" class="auto-style5">制单人</td>
                            <td style="font-size: 12pt;" class="auto-style3">
                                <dx:ASPxTextBox ID="TxtBiller" runat="server" Width="145px" EnableTheming="True" ReadOnly="True" Theme="Office2010Blue" Font-Size="11pt">
                                    <ReadOnlyStyle BackColor="white" />
                                </dx:ASPxTextBox>
                            </td>
                            <td style="font-size: 12pt;" class="auto-style4">订单日期</td>
                            <td style="font-size: 12pt;">
                                <dx:ASPxTextBox ID="TxtBillDate" runat="server" Width="145px" ReadOnly="True" Theme="Office2010Blue" Font-Size="11pt">
                                    <ReadOnlyStyle BackColor="white" />
                                </dx:ASPxTextBox>
                            </td>
                            <td style="font-size: 12pt">
                                <div style="display:none;">
                                <dx:ASPxTextBox ID="TxtcSCCode" runat="server" ClientInstanceName="TxtcSCCode" EnableTheming="True" Font-Size="11pt" ReadOnly="True" Theme="Office2010Blue" Width="145px">
                                    <ReadOnlyStyle BackColor="white" />
                                </dx:ASPxTextBox></div>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style6" style="font-size: 12pt">开票单位</td>
                            <td class="auto-style7" style="font-size: 12pt">
                                <div style="float: left">
                                    <dx:ASPxTextBox ID="TxtCustomer" runat="server" Width="145px" ReadOnly="True" Theme="Office2010Blue" Text="" Height="16px" EnableViewState="true" Font-Size="11pt">
                                        <ReadOnlyStyle BackColor="white">
                                        </ReadOnlyStyle>
                                    </dx:ASPxTextBox>
                                </div>
                                <div style="float: left; margin-left: 4px">
                                </div>


                            </td>
                            <td class="auto-style8" style="font-size: 12pt">业务员</td>
                            <td class="auto-style9" style="font-size: 12pt">
                                <dx:ASPxTextBox ID="TxtSalesman" ClientInstanceName="TxtSalesman" ReadOnly="True" runat="server" Width="145px" EnableTheming="True" Theme="Office2010Blue" Font-Size="11pt">
                                    <ReadOnlyStyle BackColor="white" />
                                </dx:ASPxTextBox>


                            </td>
                            <td class="auto-style10" style="font-size: 12pt">
                                客服QQ</td>
                            <td style="font-size: 12pt" class="auto-style11">
                                <dx:ASPxTextBox ID="TxtQQ" runat="server" ClientInstanceName="TxtQQ" EnableTheming="True" Font-Size="11pt" ReadOnly="True" Theme="Office2010Blue" Width="145px">
                                    <ReadOnlyStyle BackColor="white" />
                                </dx:ASPxTextBox>
                            </td>
                            <td style="font-size: 12pt" class="auto-style11"></td>
                        </tr>
                        <tr>
                            <td class="auto-style2" style="font-size: 12pt">送货方式</td>
                            <td colspan="5" style="font-size: 12pt">
                                <div style="float: left">
                                    <dx:ASPxTextBox ID="TxtOrderShippingMethod" ReadOnlyStyle-BackColor="white" runat="server" Width="650px" Theme="Office2010Blue" ReadOnly="true" Font-Size="11pt">
                                    </dx:ASPxTextBox>
                                </div>


                            </td>
                            <td style="font-size: 12pt"></td>
                        </tr>
                        <tr>
                            <td class="auto-style2" style="font-size: 12pt">备注</td>
                            <td colspan="5" style="font-size: 12pt">
                                <dx:ASPxTextBox ID="TxtOrderMark" ReadOnlyStyle-BackColor="white" runat="server" Width="650px" Theme="Office2010Blue" ReadOnly="true" Font-Size="11pt">
                                </dx:ASPxTextBox>
                            </td>
                            <td style="font-size: 12pt"></td>
                        </tr>
                    </table>
                    <dx:ASPxGridView ID="ViewOrderGrid" ClientInstanceName="ViewOrderGrid" runat="server" AutoGenerateColumns="False" EnableTheming="True" KeyFieldName="irowno" Theme="Office2010Blue" Font-Size="11pt" Width="1000px">
                        <SettingsPager Visible="False" Mode="ShowAllRecords">
                        </SettingsPager>
                        <Settings ShowTitlePanel="true" ShowFooter="True" VerticalScrollBarMode="Auto" />
                        <SettingsBehavior AllowSort="false" />
                        <SettingsText Title="订单明细表" />
                        <TotalSummary>
                            <dx:ASPxSummaryItem FieldName="irowno" SummaryType="Count" />
                            <dx:ASPxSummaryItem FieldName="cComUnitAmount" SummaryType="Sum" DisplayFormat="合计:{0:F}" ShowInColumn="金额" />
                        </TotalSummary>
                        <Columns>
                            <dx:GridViewDataTextColumn Caption="序号" FieldName="irowno" ReadOnly="True" VisibleIndex="0" UnboundType="Integer" Width="30px">
                                <Settings AllowSort="True" SortMode="Value" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="名称" FieldName="cinvname" ReadOnly="True" VisibleIndex="1" Width="180px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="规格" FieldName="cInvStd" ReadOnly="True" VisibleIndex="2" Width="100px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="数量" FieldName="iquantity" ReadOnly="True" VisibleIndex="3" Width="60px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="包装结果" FieldName="cdefine22" ReadOnly="True" VisibleIndex="4" Width="150px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="金额" FieldName="cComUnitAmount" ReadOnly="True" VisibleIndex="6" Width="100px">
                                <PropertiesTextEdit DisplayFormatString="{0:F}">
                                </PropertiesTextEdit>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="单价" FieldName="iquotedprice" VisibleIndex="5" Width="65px">
                            </dx:GridViewDataTextColumn>
                        </Columns>
                        <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
                    </dx:ASPxGridView>
                    <br />

                    <asp:HiddenField ID="HFlngBillType" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>