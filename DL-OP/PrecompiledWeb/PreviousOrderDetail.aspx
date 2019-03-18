<%@ page language="C#" autoeventwireup="true" inherits="PreviousOrderDetail, dlopwebdll" enableviewstate="false" %>

<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    
    <style type="text/css">
        .auto-style6 {
            width: 600px;
        }
        .auto-style17 {
            width: 51px;
        }
    </style>
    
</head>
<body>
    <form id="form1" runat="server" style="font-size: 12pt">
 <a style="float:left">接单员</a>
        <div style="float:left">
                                <dx:ASPxTextBox ID="TxtBiller" runat="server" EnableTheming="True" ReadOnly="True" Theme="Office2010Blue" Width="100px" Font-Size="11pt">
                                    <ReadOnlyStyle BackColor="White" />
                                </dx:ASPxTextBox></div>
 <a style="float:left">网单号</a>
                <div style="float:left">
                                <dx:ASPxTextBox ID="TxtBillNo" runat="server" ClientInstanceName="TxtBillNo" EnableTheming="True" ReadOnly="True" Theme="Office2010Blue" Width="145px" Font-Size="11pt">
                                    <ReadOnlyStyle BackColor="White" />
                                </dx:ASPxTextBox></div>
         <a style="float:left">发运方式</a>
        <div style="float:left">
                                <dx:ASPxTextBox ID="TxtcSCCode" ClientInstanceName="TxtcSCCode" runat="server" Width="145px" EnableTheming="True" ReadOnly="True" Theme="Office2010Blue" Font-Size="11pt">
                                    <ReadOnlyStyle BackColor="White" />
                                </dx:ASPxTextBox></div>
         <a style="float:left">下单时间</a>
        <div style="float:left">
                                <dx:ASPxTextBox ID="TxtBillTime" ClientInstanceName="TxtBillTime" runat="server" Width="145px" EnableTheming="True" ReadOnly="True" Theme="Office2010Blue" Font-Size="11pt">
                                    <ReadOnlyStyle BackColor="White" />
                                </dx:ASPxTextBox></div>
                 <a style="float:left">审核时间</a>
        <div style="float:left">
                                <dx:ASPxTextBox ID="TxtAuthTime" ClientInstanceName="TxtAuthTime" runat="server" Width="145px" EnableTheming="True" ReadOnly="True" Theme="Office2010Blue" Font-Size="11pt">
                                    <ReadOnlyStyle BackColor="White" />
                                </dx:ASPxTextBox></div>

        <div style="float:left;width:100%;height:1px"></div>
 <a style="float:left">送货方式</a>
         <div style="float:left">
                                    <dx:ASPxTextBox ID="TxtOrderShippingMethod"  runat="server" Width="600px" Theme="Office2010Blue" ReadOnly="True" Font-Size="11pt">
                                        <ReadOnlyStyle BackColor="White">
                                        </ReadOnlyStyle>
                                    </dx:ASPxTextBox></div>
                  <div style="float:left;width:100%;height:1px"></div>
 <a style="float:left">QQ客服</a>
                        <div style="float:left">
                                <dx:ASPxTextBox ID="TxtQQ" runat="server" ClientInstanceName="TxtQQ" EnableTheming="True" ReadOnly="True" Theme="Office2010Blue" Width="80px" Font-Size="11pt">
                                    <ReadOnlyStyle BackColor="white" />
                                </dx:ASPxTextBox></div>

 <a style="float:left">备注</a>
         <div style="float:left">
                                <dx:ASPxTextBox ID="TxtOrderMark" runat="server" Width="500px" Theme="Office2010Blue" ReadOnly="true" Font-Size="11pt">
                                    <ReadOnlyStyle BackColor="White">
                                    </ReadOnlyStyle>
                                </dx:ASPxTextBox></div>

        <div style="float:left;width:100%;height:1px"></div>
        <div  >
                    <dx:ASPxGridView ID="ViewOrderGrid" ClientInstanceName="ViewOrderGrid" runat="server" AutoGenerateColumns="False" EnableTheming="True" 
                        KeyFieldName="irowno" Theme="Office2010Blue" Font-Size="11pt" Width="800px">
                        <SettingsPager Visible="False" Mode="ShowAllRecords">
                        </SettingsPager>
                        <Settings ShowTitlePanel="true" ShowFooter="True" VerticalScrollableHeight="140" VerticalScrollBarMode="Auto" />
                        <SettingsBehavior AllowSort="false" />
                        <SettingsText Title="订单明细表" />
                        <TotalSummary>
                            <dx:ASPxSummaryItem FieldName="irowno" SummaryType="Count" DisplayFormat="总计:{0}条" ShowInColumn="包装结果" />
                            <dx:ASPxSummaryItem FieldName="iSum" SummaryType="Sum" DisplayFormat="合计金额={0:F}" ShowInColumn="名称" />
                        </TotalSummary>
                        <Columns>
                            <dx:GridViewDataTextColumn Caption="名称" FieldName="cInvName" ReadOnly="True" VisibleIndex="0" Width="160px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="规格" FieldName="cInvStd" ReadOnly="True" VisibleIndex="1" Width="100px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="数量" FieldName="iQuantity" ReadOnly="True" VisibleIndex="2" Width="60px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="包装结果" FieldName="cDefine22" ReadOnly="True" VisibleIndex="3" Width="150px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="执行金额" FieldName="iSum" Visible="False" VisibleIndex="4" Width="0px">
                            </dx:GridViewDataTextColumn>
                        </Columns>
                        <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
                    </dx:ASPxGridView>
            </div>
    </form>
</body>
</html>
