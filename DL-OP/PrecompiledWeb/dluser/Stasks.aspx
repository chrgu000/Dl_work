<%@ page language="C#" autoeventwireup="true" inherits="dluser_Stasks, dlopwebdll" enableviewstate="false" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>已办工作</title>
</head>
<body>
    <form id="form1" runat="server">
                <div style="float: left;margin-right:5px">
            <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="系统编号">
            </dx:ASPxLabel>
        </div>
        <div style="float: left">
        <dx:ASPxTextBox ID="TxtBillNo" ClientInstanceName="TxtBillNo" runat="server" Width="120px">
</dx:ASPxTextBox></div>
        <div style="float: left;margin-right:5px;margin-left: 20px;">
            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="开始日期">
            </dx:ASPxLabel>
        </div>
        <div style="float: left">
            <dx:ASPxDateEdit ID="DatBeginDate" ClientInstanceName="DatBeginDate" runat="server" Width="120">
            </dx:ASPxDateEdit>
        </div>
        <div style="float: left; margin-left: 20px;margin-right:5px">
            <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="截至日期">
            </dx:ASPxLabel>
        </div>
        <div style="float: left">
            <dx:ASPxDateEdit ID="DatEndDate" ClientInstanceName="DatEndDate" runat="server" Width="120">
            </dx:ASPxDateEdit>
        </div>
        <div style="float: left; margin-left: 20px;margin-right:5px">
            <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="订单类型">
            </dx:ASPxLabel>
        </div>
        <div style="float: left">
            <dx:ASPxComboBox ID="CombocSTCode" ClientInstanceName="CombocSTCode" runat="server" SelectedIndex="0" Width="80px">
                <Items>
                    <dx:ListEditItem Selected="True" Text="全部" Value="-1" />
                    <dx:ListEditItem Text="普通销售" Value="00" />
                    <dx:ListEditItem Text="样品资料" Value="01" />
                    <dx:ListEditItem Text="参照酬宾订单" Value="1" />
                    <dx:ListEditItem Text="参照特殊订单" Value="2" />
                </Items>
            </dx:ASPxComboBox>
        </div>
        <div style="float: left; margin-left: 20px;margin-right:5px">
            <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="订单状态 ">
            </dx:ASPxLabel>
        </div>
        <div style="float: left">
            <dx:ASPxComboBox ID="ComboOrderStatus" runat="server" SelectedIndex="0" Width="80px" ClientInstanceName="ComboOrderStatus">
                <Items>
                    <dx:ListEditItem Selected="True" Text="全部" Value="0" />
                    <dx:ListEditItem Text="已审核" Value="4" />
                    <dx:ListEditItem Text="未审核" Value="1" />
                    <dx:ListEditItem Text="已作废" Value="99" />
                </Items>
            </dx:ASPxComboBox>
        </div>
                <div style="float: left;margin-left:20px">
                <dx:ASPxButton ID="BtnOk" ClientInstanceName="BtnOk" runat="server" Text="查询" Width="80" OnClick="BtnOk_Click">
                </dx:ASPxButton>
                            </div>
        <div>
                <dx:ASPxGridView ID="Grid" runat="server" AutoGenerateColumns="False" EnableTheming="True" KeyFieldName="strBillNo" Theme="Office2010Blue"
                  OnCustomButtonCallback="Grid_CustomButtonCallback"  >
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="系统编号" FieldName="strBillNo" VisibleIndex="1" Width="140px" ReadOnly="True">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="正式订单号" FieldName="cSOCode" VisibleIndex="2" Width="140px" ReadOnly="True">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="状态" FieldName="bytStatus" VisibleIndex="3" Width="70px" ReadOnly="True">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="开票单位" FieldName="ccusname" VisibleIndex="7" Width="160px" ReadOnly="True">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="地址" FieldName="cdefine11" VisibleIndex="8" Width="320px" ReadOnly="True">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="备注" FieldName="strRemarks" VisibleIndex="9" Width="150px" ReadOnly="True">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="装车方式" FieldName="strLoadingWays" VisibleIndex="10" Width="120px" ReadOnly="True">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="提交日期" FieldName="datCreateTime" VisibleIndex="5" Width="85px" ReadOnly="True">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="下单时间" FieldName="datBillTime" ReadOnly="True" VisibleIndex="6" Width="150px">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="审核/作废/驳回时间" ReadOnly="True" VisibleIndex="4" Width="150px" FieldName="stime">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewCommandColumn ButtonType="Button" Caption="关闭订单" Name="关闭订单" VisibleIndex="0" Width="70px">
                            <CustomButtons>
                                <dx:GridViewCommandColumnCustomButton ID="closeBtn" Text="关闭">
                                </dx:GridViewCommandColumnCustomButton>
                            </CustomButtons>
                        </dx:GridViewCommandColumn>
                    </Columns>
                    <SettingsPager Visible="False" Mode="ShowAllRecords">
                    </SettingsPager>
                    <Settings VerticalScrollableHeight="350" VerticalScrollBarMode="Auto" />
                    <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
                </dx:ASPxGridView>
            </div>
    </form>
</body>
</html>
