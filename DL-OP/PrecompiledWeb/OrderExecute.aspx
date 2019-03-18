<%@ page language="C#" autoeventwireup="true" inherits="OrderExecute, dlopwebdll" enableviewstate="false" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        .auto-style1 {
            color: #FF3300;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <div style="border-color: red; border-bottom-style: dotted; height:80px">
                <div style="height: 25px">
                                        <div style="float: left">
                    <dx:ASPxComboBox ID="ComboTimeType" runat="server" ClientInstanceName="ComboTimeType" SelectedIndex="0" Width="100px" AllowMouseWheel="False">
                        <Items>
                            <dx:ListEditItem Selected="True" Text="下单时间" Value="XDtime" />
                            <dx:ListEditItem Text="审核时间" Value="SHtime" />
                        </Items>
                    </dx:ASPxComboBox></div>
                    <a style="float: left">
                    开始时间</a><div style="float: left; margin-right: 5px">
                        <dx:ASPxDateEdit ID="DateBegin" ClientInstanceName="DateBegin" runat="server" NullText="选择开始日期" Width="160px" 
                            EditFormat="Custom" EditFormatString="yyyy-MM-dd HH:mm" MinDate="2016-01-01" >
                            <CalendarProperties FirstDayOfWeek="Monday">
                            </CalendarProperties>
                            <TimeSectionProperties Visible="True">
                                <TimeEditProperties AllowNull="False" DisplayFormatString="" EditFormatString="HH:mm">
                                </TimeEditProperties>
                            </TimeSectionProperties>
                        </dx:ASPxDateEdit>  </div>

                    <a style="float: left">结束时间</a><div style="float: left; margin-right: 20px">
                        <dx:ASPxDateEdit ID="DateEnd" ClientInstanceName="DateEnd" runat="server" NullText="选择结束日期" Width="160px" 
                            EditFormat="Custom" EditFormatString="yyyy-MM-dd HH:mm" MinDate="2016-01-01" >
                            <CalendarProperties FirstDayOfWeek="Monday">
                            </CalendarProperties>
                            <TimeSectionProperties Visible="True">
                                <TimeEditProperties DisplayFormatString="" EditFormatString="HH:mm">
                                    <ButtonEditEllipsisImage AlternateText="23:59">
                                    </ButtonEditEllipsisImage>
                                </TimeEditProperties>
                            </TimeSectionProperties>
                        </dx:ASPxDateEdit>
                    </div>
                    <a style="float: left">DL网单号</a><div style="float: left">
                        <dx:ASPxTextBox ID="TxtstrBillNo" ClientInstanceName="TxtstrBillNo" runat="server" Width="170px" NullText="输入想要查询的DL号,如639"></dx:ASPxTextBox>
                    </div>
                      <a style="float: left">操作帐号：</a><div style="float: left">
                         <dx:ASPxComboBox ID="ComboUSer" runat="server" ClientInstanceName="ComboUSer" Width="100px" AllowMouseWheel="False" OnInit="ComboUSer_Init" >
                    </dx:ASPxComboBox>
                    </div>

                </div>
                <div style="height: 25px">
                    <a style="float: left">是否显示明细</a><div style="float: left; margin-right: 20px">
                        <dx:ASPxCheckBox ID="showtype" ClientInstanceName="showtype" runat="server" Checked="True" CheckState="Checked" ValueChecked="1" ValueType="System.Int32" ValueUnchecked="0"></dx:ASPxCheckBox>
                    </div>
                    <a style="float: left">是否包含未发货订单</a><div style="float: left; margin-right: 20px">
                        <dx:ASPxCheckBox ID="FHStatus" ClientInstanceName="FHStatus" runat="server" Checked="True" CheckState="Checked" ValueChecked="1" ValueType="System.Int32" ValueUnchecked="0"></dx:ASPxCheckBox>
                    </div>
                    <a style="float: left">开票单位</a>
                    <div style="float: left; margin-right: 10px">
                        <dx:ASPxComboBox ID="ComboBoxccuscode" ClientInstanceName="ComboBoxccuscode" runat="server" ValueType="System.String" Width="170px" OnInit="ComboBoxccuscode_Init"></dx:ASPxComboBox>
                    </div>
                    <dx:ASPxButton ID="BtnS" ClientInstanceName="BtnS" runat="server" Text="查询" Width="100px" OnClick="BtnS_Click">
                    </dx:ASPxButton>
                    <a style="margin-right: 20px"></a>
                    <dx:ASPxButton ID="BtnSXLS" ClientInstanceName="BtnSXLS" runat="server" Text="查询并导出EXCEL" Width="150px" OnClick="BtnSXLS_Click" UseSubmitBehavior="false">
                    </dx:ASPxButton>
                </div>
                <div style="height: 25px">
                    <div style="float: left; margin-right: 10px">
                        <dx:ASPxSpinEdit ID="ASPxSpinEdit1" runat="server" MaxValue="3000" MinValue="100" NullText="输入100~3000之间,默认400高度" NumberType="Integer" Width="200px" />
                    </div>
                    <div style="float: left; margin-right: 10px">
                        <dx:ASPxButton ID="ASPxButton1" runat="server" OnClick="ASPxButton1_Click" Text="设置订单执行情况表高度">
                        </dx:ASPxButton>
                    </div>
                    <div style="float: left; margin-right: 10px">
                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="单笔订单合计" BackColor="Thistle"></dx:ASPxLabel>
                    </div>
                    <div style="float: left; margin-right: 10px">
                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="未发货" BackColor="MediumPurple"></dx:ASPxLabel>
                    </div>
                    <div style="float: left; margin-right: 10px">
                        <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="未发完,无退货" BackColor="RoyalBlue"></dx:ASPxLabel>
                    </div>
                                        <div style="float: left; margin-right: 10px">
                        <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="未发完,有退货" BackColor="DarkKhaki"></dx:ASPxLabel>
                    </div>
                                        <div style="float: left; margin-right: 10px">
                        <dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="已发完,有退货" BackColor="Orange"></dx:ASPxLabel>
                    </div>
                                                            <div style="float: left; margin-right: 10px">
                        <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="无提货清单退货" BackColor="Crimson"></dx:ASPxLabel>
                    </div>
                </div>

            </div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div>
                        <dx:ASPxGridView ID="OrderExcute" ClientInstanceName="OrderExcute" runat="server" AutoGenerateColumns="False" EnableTheming="True" Theme="Default"
                            Width="1260px" OnHtmlRowPrepared="OrderExcute_HtmlRowPrepared" Font-Size="10pt">
                            <TotalSummary>
                                <dx:ASPxSummaryItem DisplayFormat="{0:F}" FieldName="isum" ShowInColumn="订单金额" SummaryType="Sum" />
                                <dx:ASPxSummaryItem DisplayFormat="{0:F}" FieldName="U8iFHMoney" ShowInColumn="发货金额" SummaryType="Sum" />
                                <dx:ASPxSummaryItem DisplayFormat="{0:F}" FieldName="U8iTHMoney" ShowInColumn="退货金额" SummaryType="Sum" />
                            </TotalSummary>
                            <Columns>
                                <dx:GridViewDataTextColumn Caption="DL网单号" FieldName="strBillNo" ReadOnly="True" VisibleIndex="0" Width="130px">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="正式订单号" FieldName="cSOCode" ReadOnly="True" VisibleIndex="1" Width="130px">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="开票单位" FieldName="ccusname" ReadOnly="True" VisibleIndex="4" Width="120px">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="商品名称" FieldName="cinvname" ReadOnly="True" VisibleIndex="5" Width="180px">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="商品规格" FieldName="cInvStd" ReadOnly="True" VisibleIndex="6" Width="100px">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="订单数量" FieldName="iquantity" ReadOnly="True" VisibleIndex="7" Width="80px">
                                    <PropertiesTextEdit DisplayFormatString="{0:F}">
                                    </PropertiesTextEdit>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="订单金额" FieldName="isum" VisibleIndex="8" Width="100px" ReadOnly="True">
                                    <PropertiesTextEdit DisplayFormatString="{0:F}">
                                    </PropertiesTextEdit>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="发货数量" FieldName="U8iFHQuantity" ReadOnly="True" VisibleIndex="9" Width="100px">
                                    <PropertiesTextEdit DisplayFormatString="{0:F}">
                                    </PropertiesTextEdit>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="发货金额" FieldName="U8iFHMoney" VisibleIndex="10" Width="100px" ReadOnly="True">
                                    <PropertiesTextEdit DisplayFormatString="{0:F}">
                                    </PropertiesTextEdit>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="下单时间" FieldName="datBillTime" ReadOnly="True" VisibleIndex="2" Width="80px">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="审核时间" FieldName="datAuditordTime" ReadOnly="True" VisibleIndex="3" Width="80px">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="地址信息" FieldName="cdefine11" ReadOnly="True" VisibleIndex="14" Width="450px">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="备注" FieldName="strRemarks" ReadOnly="True" VisibleIndex="15" Width="200px">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="状态" FieldName="BillStatus" VisibleIndex="13" Width="60px" ReadOnly="True">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="退货数量" FieldName="U8iTHQuantity" ReadOnly="True" VisibleIndex="11" Width="100px">
                                    <PropertiesTextEdit DisplayFormatString="{0:F}">
                                    </PropertiesTextEdit>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="退货金额" FieldName="U8iTHMoney" ReadOnly="True" VisibleIndex="12" Width="100px">
                                    <PropertiesTextEdit DisplayFormatString="{0:F}">
                                    </PropertiesTextEdit>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="BillStatusName" FieldName="BillStatusName" ReadOnly="True" VisibleIndex="18" Width="0px">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="电话号码" FieldName="cCusPhone" ReadOnly="True" VisibleIndex="17" Width="100px">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="操作帐号" FieldName="strAllAcount" ReadOnly="True" VisibleIndex="16" Width="70px">
                                </dx:GridViewDataTextColumn>
                            </Columns>
                            <SettingsBehavior AllowSort="False" />
                            <SettingsPager Mode="ShowAllRecords" Visible="False">
                            </SettingsPager>
                            <Settings VerticalScrollableHeight="400" VerticalScrollBarMode="Visible" HorizontalScrollBarMode="Visible" ShowFooter="True" />
                            <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
                        </dx:ASPxGridView>
                        <dx:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="OrderExcute" FileName="订单执行情况表" ExportEmptyDetailGrid="True"></dx:ASPxGridViewExporter>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="BtnS" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="ASPxButton1" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
