<%@ page language="C#" autoeventwireup="true" inherits="SOADetail, dlopwebdll" enableviewstate="false" %>

<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div>

        <a style="float:left">单位：</a>
        <div style="float:left">
        <dx:ASPxComboBox ID="ComboBoxccuscode" ClientInstanceName="ComboBoxccuscode" runat="server" ValueType="System.String" Width="160px">
        </dx:ASPxComboBox> </div>
        <a style="float:left">开始日期：</a>
        <div style="float:left"><dx:ASPxDateEdit ID="DateEditbegin" ClientInstanceName="DateEditbegin" runat="server" Width="100px">
        </dx:ASPxDateEdit></div>
        <a style="float:left">截止日期：</a>
        <div style="float:left;padding-right:20px"><dx:ASPxDateEdit ID="DateEditend" ClientInstanceName="DateEditend" runat="server" Width="100px">
        </dx:ASPxDateEdit></div><div >
        <dx:ASPxButton ID="BtnOk" ClientInstanceName="BtnOk" runat="server" Text="查询" Theme="SoftOrange" Width="80px" OnClick="BtnOk_Click">
        </dx:ASPxButton></div>
        <div >
        <dx:ASPxButton ID="BtnToXlsx" ClientInstanceName="BtnToXlsx" runat="server" Text="查询并导出Excel文件" OnClick="BtnToXlsx_Click" style="margin-top: 2px"  UseSubmitBehavior="false" Theme="Youthful">
                </dx:ASPxButton>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server"><ContentTemplate>
            <dx:ASPxGridView ID="SOAGrid" runat="server" AutoGenerateColumns="False" EnableTheming="True" Theme="SoftOrange" OnCustomSummaryCalculate="SOAGrid_CustomSummaryCalculate"
                ClientInstanceName="SOAGrid"  OnHtmlDataCellPrepared="SAOGrid_HtmlDataCellPrepared" OnHtmlRowPrepared="SOAGrid_HtmlRowPrepared" Font-Size="10pt" Width="1200px">
                <TotalSummary>
                    <dx:ASPxSummaryItem FieldName="cDwName" ShowInGroupFooterColumn="单位名称" SummaryType="Count" DisplayFormat="总计={0}条记录" />
                    <dx:ASPxSummaryItem FieldName="jf" ShowInGroupFooterColumn="发货金额" SummaryType="Sum" DisplayFormat="提货金额合计={0:F}" />
                    <dx:ASPxSummaryItem FieldName="df" ShowInGroupFooterColumn="结算金额" SummaryType="Sum" DisplayFormat="结算金额合计={0:F}" />
                    <dx:ASPxSummaryItem DisplayFormat="应付余额={0:F}" FieldName="ye" SummaryType="Custom" />
                </TotalSummary>
                <Columns>
                    <dx:GridViewDataTextColumn Caption="单位名称" FieldName="cDwName" VisibleIndex="0" Width="160px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="订单号" FieldName="cOrderNo" VisibleIndex="4" Width="130px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="日" FieldName="iDay" VisibleIndex="3" Width="40px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="月" FieldName="iMonth" VisibleIndex="2" Width="40px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="年" FieldName="iYear" VisibleIndex="1" Width="50px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="提货金额" FieldName="jf" VisibleIndex="6" Width="100px">
                        <PropertiesTextEdit DisplayFormatString="{0:F}">
                        </PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="提货清单号" FieldName="cDLCode" VisibleIndex="5" Width="130px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="结算金额" FieldName="df" VisibleIndex="9" Width="100px">
                        <PropertiesTextEdit DisplayFormatString="{0:F}">
                        </PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="结算方式" VisibleIndex="8" Width="80px" FieldName="cSSName">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="备注" FieldName="dgst" VisibleIndex="7" Width="140px" Visible="False">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="应付余额" FieldName="ye" VisibleIndex="11" Width="140px">
                        <PropertiesTextEdit DisplayFormatString="{0:F}">
                        </PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="付款备注" FieldName="skdBZ" VisibleIndex="10" Width="140px" >
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="单据类型" FieldName="vtype" Visible="False" VisibleIndex="13" >
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="订单备注" FieldName="cMemo" ReadOnly="True" VisibleIndex="12" Width="450px">
                    </dx:GridViewDataTextColumn>
                </Columns>
                <SettingsBehavior AllowSort="False" AllowDragDrop="False" />
                <SettingsPager Mode="ShowAllRecords" Visible="False">
                </SettingsPager>
                <Settings ShowFooter="True" VerticalScrollableHeight="350" VerticalScrollBarMode="Visible" UseFixedTableLayout="True" HorizontalScrollBarMode="Visible" />
                <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
            </dx:ASPxGridView>
                <dx:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="SOAGrid" FileName="账单明细" ExportEmptyDetailGrid="True"></dx:ASPxGridViewExporter>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="BtnOk" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>

    </div>
    </form>
</body>
</html>
