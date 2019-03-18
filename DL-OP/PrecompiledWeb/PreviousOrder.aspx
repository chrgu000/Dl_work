<%@ page language="C#" autoeventwireup="true" inherits="PreviousOrder, dlopwebdll" enableviewstate="false" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>历史订单</title>
    <style type="text/css">


.dxpcLite .dxpc-header,
.dxdpLite .dxpc-header 
{
	color: #404040;
	background-color: #DCDCDC;
	border-bottom: 1px solid #C9C9C9;
	padding: 2px 2px 2px 12px;
}

.dxpnlControl
{
    font: 12px Tahoma, Geneva, sans-serif;
    border: 0px solid #8b8b8b;
}

        </style>
    </head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>

                <a style="float:left">开始日期</a><div style="float:left"><dx:ASPxDateEdit ID="DateEditbegin" ClientInstanceName="DateEditbegin" runat="server" EnableTheming="True" NullText="默认为结束日期前90天" Theme="SoftOrange"></dx:ASPxDateEdit></div>
            <a style="float:left">结束日期</a><div style="float:left;padding-right:30px; margin-bottom: 0px;"><dx:ASPxDateEdit ID="DateEditend" ClientInstanceName="DateEditend" runat="server" NullText="默认为今天" Theme="SoftOrange"></dx:ASPxDateEdit></div>
             <a style="float:left">单据类型:</a><div style="float:left;margin-right:20px">
                 <dx:ASPxComboBox ID="ComBoType" ClientInstanceName="ComBoType" runat="server" Width="80px" EnableTheming="True" SelectedIndex="0" Theme="SoftOrange">
            <Items>
                <dx:ListEditItem Selected="True" Text="已审核" Value="0" />
                <dx:ListEditItem Text="已作废" Value="99" />
            </Items>
                </dx:ASPxComboBox></div><div >
 <dx:ASPxButton ID="BtnRefresh" runat="server" Text="查询" OnClick="BtnRefresh_Click" Theme="SoftOrange" Width="80px"  ></dx:ASPxButton>
        &nbsp;【点击正式订单号查看详情。开始日期与结束日期不能超过90天】</div>
<div></div>
        <div>
            <dx:ASPxGridView ID="GridOrder" ClientInstanceName="GridOrder" runat="server" AutoGenerateColumns="False" 
                EnableTheming="True" Theme="Office2010Blue" KeyFieldName="cSOCode" Font-Size="11pt" Width="1250px">
                <TotalSummary>
                    <dx:ASPxSummaryItem FieldName="strBillNo" ShowInGroupFooterColumn="网单号" SummaryType="Count" />
                </TotalSummary>
                <Columns>
                    <dx:GridViewDataTextColumn Caption="开票单位名称" FieldName="cCusName" VisibleIndex="7" Width="180px">
                        <Settings AllowAutoFilter="True" AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains"  />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="单据日期" FieldName="dDate" VisibleIndex="6" Width="120px">
                        <Settings AllowAutoFilter="False" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="网单号" FieldName="strBillNo" VisibleIndex="3" Width="160px">
                        <PropertiesTextEdit DisplayFormatString="{0}">
                            <Style ForeColor="#0066FF">
                            </Style>
                        </PropertiesTextEdit>
                        <Settings AllowAutoFilter="True" AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains"  />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="销售类型" FieldName="billtype" VisibleIndex="1" Width="90px" ReadOnly="True">
                        <Settings AllowAutoFilter="True" AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="下单方式" FieldName="XDFS" ReadOnly="True" VisibleIndex="2" Width="90px">
                        <Settings AllowAutoFilter="True" AllowAutoFilterTextInputTimer="False"  AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataHyperLinkColumn Caption="正式订单号" FieldName="cSOCode" VisibleIndex="4" Width="140px">
                        <PropertiesHyperLinkEdit NavigateUrlFormatString="PreviousOrderDetail.aspx?ubillno={0}" Target="OrderCenter">
                        </PropertiesHyperLinkEdit>
                        <Settings AllowAutoFilter="True" AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains"  />
                        <CellStyle ForeColor="Fuchsia">
                        </CellStyle>
                    </dx:GridViewDataHyperLinkColumn>
                    <dx:GridViewDataHyperLinkColumn Caption="样品资料关联正式订单号" FieldName="chdefine2" ReadOnly="True" VisibleIndex="5" Width="140px">
                        <PropertiesHyperLinkEdit NavigateUrlFormatString="PreviousOrderDetail.aspx?ubillno={0}" Target="OrderCenter">
                        </PropertiesHyperLinkEdit>
                        <Settings AllowAutoFilter="True" AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains"  />
                        <CellStyle ForeColor="Fuchsia">
                        </CellStyle>
                    </dx:GridViewDataHyperLinkColumn>
                    <dx:GridViewDataTextColumn Caption="备注" FieldName="cMemo" VisibleIndex="8" Width="200px">
                        <Settings AllowAutoFilter="True" AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains"  />
                    </dx:GridViewDataTextColumn>
                </Columns>
                <SettingsBehavior AllowSort="False" SortMode="DisplayText" />
                <SettingsPager Visible="False" Mode="ShowAllRecords">
                </SettingsPager>
                <Settings VerticalScrollableHeight="180" VerticalScrollBarMode="Visible" ShowFilterRow="True" ShowFilterRowMenu="True" ShowFooter="True" />
                <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
            </dx:ASPxGridView>


        </div>
    </form>
</body>
 
</html>