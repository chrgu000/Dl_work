<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SOASetting.aspx.cs" Inherits="dluser_SOASetting" %>

<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div style="float:left;width:2500px;margin-bottom:15px;display:none;"><a style="float:left"">设置账单日期:对勾选的用户批量设置账单日期:</a>
            <div style="float:left">
            <dx:ASPxSpinEdit ID="BatchUpdate" runat="server" MaxValue="31" Number="0" NumberType="Integer" Width="80px" />
                  </div>
            <dx:ASPxButton ID="BtnBatchUpdateOk" runat="server" Text="批量更新">
            </dx:ASPxButton>
        </div>
        <div style="float:left;width:2500px;margin-bottom:15px"><a style="float:left"">发送账单:对勾选的用户批量发送账单:</a>
            <div style="float:left">
                  </div>
            <dx:ASPxButton ID="BtnSendSOA" runat="server" Text="发送账单" OnClick="BtnSendSOA_Click">
            </dx:ASPxButton>
            </div><div>
            <a  style="float:left">导出账单：年</a><div style="float:left"><dx:ASPxComboBox ID="ComboPeriodYear_exp" ClientInstanceName="ComboPeriodYear_exp" runat="server" EnableTheming="True" Theme="SoftOrange" Width="60px" SelectedIndex="0">
                                    <Items>
                                        <dx:ListEditItem Selected="True" Text="2016" Value="2016" />
                                        <dx:ListEditItem Text="2017" Value="2017" />
                                        <dx:ListEditItem Text="2018" Value="2018" />
                                        <dx:ListEditItem Text="2019" Value="2019" />
                                        <dx:ListEditItem Text="2020" Value="2020" />
                                        <dx:ListEditItem Text="2021" Value="2021" />
                                        <dx:ListEditItem Text="2022" Value="2022" />
                                        <dx:ListEditItem Text="2023" Value="2023" />
                                        <dx:ListEditItem Text="2024" Value="2024" />
                                        <dx:ListEditItem Text="2025" Value="2025" />
                                        <dx:ListEditItem Text="2026" Value="2026" />
                                        <dx:ListEditItem Text="2027" Value="2027" />
                                        <dx:ListEditItem Text="2028" Value="2028" />
                                    </Items>
                                    <ClearButton Visibility="False">
                                    </ClearButton>
                                </dx:ASPxComboBox></div><a style="float:left">月</a>
                <div style="float:left"><dx:ASPxComboBox ID="ComboPeriodMon_exp" ClientInstanceName="ComboPeriodMon_exp" runat="server" EnableTheming="True" Theme="SoftOrange" Width="60px" SelectedIndex="0">
                                    <Items>
                                        <dx:ListEditItem Selected="True" Text="01" Value="01" />
                                        <dx:ListEditItem Text="02" Value="02" />
                                        <dx:ListEditItem Text="03" Value="03" />
                                        <dx:ListEditItem Text="04" Value="04" />
                                        <dx:ListEditItem Text="05" Value="05" />
                                        <dx:ListEditItem Text="06" Value="06" />
                                        <dx:ListEditItem Text="07" Value="07" />
                                        <dx:ListEditItem Text="08" Value="08" />
                                        <dx:ListEditItem Text="09" Value="09" />
                                        <dx:ListEditItem Text="10" Value="10" />
                                        <dx:ListEditItem Text="11" Value="11" />
                                        <dx:ListEditItem Text="12" Value="12" />
                                    </Items>
                                    <ClearButton Visibility="False">
                                    </ClearButton>
                                </dx:ASPxComboBox></div>
            <dx:ASPxButton ID="BtnSoaExp" ClientInstanceName="BtnSoaExp" runat="server" Text="导出账单数据" Theme="Youthful" Width="300px" OnClick="BtnSoaExp_Click">
                            </dx:ASPxButton></div>
        <dx:ASPxGridView ID="SOAGrid" ClientInstanceName="SOAGrid" runat="server" EnableTheming="True" Theme="SoftOrange" AutoGenerateColumns="False"
              OnRowUpdating="SOAGrid_RowUpdating"   KeyFieldName="cCusCode"
            OnHtmlDataCellPrepared="SOAGrid_HtmlDataCellPrepared">
            <Columns>
                <dx:GridViewCommandColumn ShowClearFilterButton="True" VisibleIndex="0" Width="60px" SelectAllCheckboxMode="Page" ShowSelectCheckbox="True">
                </dx:GridViewCommandColumn>
                <dx:GridViewDataTextColumn Caption="U8顾客编号" FieldName="cCusCode" ReadOnly="True" VisibleIndex="2" Width="80px">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="U8顾客名称" FieldName="cCusName" VisibleIndex="3" Width="220px" ReadOnly="True">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="当前账单生成发送日期" FieldName="SOASendTime" VisibleIndex="4" Width="150px">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataSpinEditColumn Caption="新账单日期" FieldName="NewSendDate" UnboundType="Integer" VisibleIndex="5" Width="80px">
                    <PropertiesSpinEdit DisplayFormatString="g" MaxValue="31" NullDisplayText="0" NullText="0" NumberType="Integer">
                    </PropertiesSpinEdit>
                    <CellStyle BackColor="#FF9900">
                    </CellStyle>
                </dx:GridViewDataSpinEditColumn>
                <dx:GridViewDataTextColumn Caption="系统登录用户" FieldName="strLoginName" ReadOnly="True" VisibleIndex="1" Width="95px">
                </dx:GridViewDataTextColumn>
            </Columns>
            <SettingsBehavior FilterRowMode="OnClick" />
            <SettingsPager Visible="False" Mode="ShowAllRecords">
            </SettingsPager>
            <SettingsEditing Mode="Batch">
            </SettingsEditing>
            <Settings VerticalScrollableHeight="350" VerticalScrollBarMode="Auto" VerticalScrollBarStyle="Virtual" ShowFilterRow="True" ShowFilterRowMenu="True" />
        </dx:ASPxGridView>
    
                    <br />
    
                    <dx:ASPxGridView ID="SOAGrid_exp" ClientInstanceName="SOAGrid_exp" runat="server" AutoGenerateColumns="False" EnableTheming="True" Theme="Metropolis"
                        KeyFieldName="ccuscode">
                        <Columns>
                            <dx:GridViewDataTextColumn Caption="顾客编号" FieldName="ccuscode" VisibleIndex="0" Width="80px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="顾客名称" FieldName="ccusname" VisibleIndex="1" Width="180px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="账单截至日期" FieldName="strEndDate" VisibleIndex="2" Width="120px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="账单发送时间" FieldName="datSendTime" VisibleIndex="3" Width="180px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="账单金额" FieldName="dblAmount" VisibleIndex="4" Width="80px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="账单期间" FieldName="intPeriod" VisibleIndex="8" Width="60px">
                            </dx:GridViewDataTextColumn>

                        </Columns>
                        <SettingsBehavior AllowFocusedRow="True" FilterRowMode="OnClick" />
                        <SettingsPager Visible="false" Mode="ShowAllRecords">
                        </SettingsPager>
                        <Settings VerticalScrollableHeight="280" VerticalScrollBarMode="Visible" ShowFilterRowMenu="True" ShowFilterRow="True" />
                        <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
                    </dx:ASPxGridView>
    
        <br />
 <dx:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="SOAGrid_exp" FileName="网上订单账单明细" ExportEmptyDetailGrid="True"></dx:ASPxGridViewExporter>   
    </div>
    </form>
</body>
</html>
