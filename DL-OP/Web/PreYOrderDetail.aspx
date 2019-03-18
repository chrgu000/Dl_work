<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreYOrderDetail.aspx.cs" Inherits="PreYOrderDetail" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
 <div>
   <a style="float:left;">网单号：</a> <div style="float:left;"><dx:ASPxTextBox ID="TxtBillNo" runat="server" Width="170px"></dx:ASPxTextBox>
    </div>
           <a style="float:left;">开票单位：</a> <div style="float:left;"><dx:ASPxTextBox ID="TxtKPDW" runat="server" Width="170px"></dx:ASPxTextBox>
    </div>
                   <a style="float:left;">&nbsp;下单时间：</a> <div ><dx:ASPxTextBox ID="TxtDDate" runat="server" Width="170px"></dx:ASPxTextBox>
    </div>
        <div style="margin-top:10px">
        <dx:ASPxGridView ID="XOrderGrid" ClientInstanceName="XOrderGrid" runat="server" AutoGenerateColumns="False" EnableTheming="True" Font-Size="10pt" Theme="Office2010Blue">
            <TotalSummary>
                <dx:ASPxSummaryItem FieldName="cinvcode" ShowInColumn="名称" ShowInGroupFooterColumn="名称" SummaryType="Count" />
                <dx:ASPxSummaryItem DisplayFormat="合计:{0:F}" FieldName="xx" ShowInColumn="金额" ShowInGroupFooterColumn="金额" SummaryType="Sum" />
            </TotalSummary>
            <Columns>
                <dx:GridViewDataTextColumn Caption="名称" VisibleIndex="1" Width="150px" FieldName="cinvname">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="规格" VisibleIndex="2" Width="120px" FieldName="cInvStd">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="单位组" VisibleIndex="3" Width="120px" FieldName="UnitGroup">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="基本数量汇总" VisibleIndex="4" Width="100px" FieldName="iquantity">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="包装结果" VisibleIndex="5" Width="150px" FieldName="cDefine22">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="单价" VisibleIndex="6" Width="80px" FieldName="iquotedprice">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="金额" VisibleIndex="7" Width="140px" FieldName="cComUnitAmount">
                    <PropertiesTextEdit DisplayFormatString="{0:F}">
                    </PropertiesTextEdit>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="执行单价" VisibleIndex="8" Width="0px" FieldName="itaxunitprice">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="执行金额" VisibleIndex="9" Width="0px" FieldName="xx">
                    <PropertiesTextEdit DisplayFormatString="{0:F}">
                    </PropertiesTextEdit>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="编码" VisibleIndex="10" Width="0px" FieldName="cinvcode">
                </dx:GridViewDataTextColumn>
            </Columns>
            <SettingsPager Mode="ShowAllRecords" Visible="False">
            </SettingsPager>
            <Settings VerticalScrollableHeight="300" VerticalScrollBarMode="Visible" VerticalScrollBarStyle="Virtual" ShowFooter="True" />
            <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
        </dx:ASPxGridView></div>
    </div>
    </form>
</body>
</html>
