<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ActivityContent.aspx.cs" Inherits="dluser_ActivityContent" %>
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
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div style="border: medium double #999966">
                <h3>维护170511夏季酬宾</h3>

                <div style="float:left">
                    <dx:ASPxGridView ID="GV_170511" ClientInstanceName="GV_170511" runat="server" AutoGenerateColumns="False" KeyFieldName="cInvCode"  OnRowUpdating="GV_170511_RowUpdating" >
                        <Columns>
                            <dx:GridViewCommandColumn ShowClearFilterButton="True" ShowEditButton="True" VisibleIndex="0">
                            </dx:GridViewCommandColumn>
                            <dx:GridViewDataTextColumn Caption="存货编码" FieldName="cInvCode" ReadOnly="True" VisibleIndex="1" Width="150px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="数量" FieldName="iQuantity" VisibleIndex="4" Width="100px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="客户" FieldName="cCusCode" VisibleIndex="6" Width="80px" ReadOnly="True">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="到期日" FieldName="datEndDate" VisibleIndex="7" Width="150px" ReadOnly="True">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="规格" FieldName="cInvStd" ReadOnly="True" VisibleIndex="3" Width="100px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="存货名称" FieldName="cInvName" ReadOnly="True" VisibleIndex="2" Width="180px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="修改数量" FieldName="NewiQuantity" VisibleIndex="5" Width="100px">
                                <EditCellStyle BackColor="#FF6600">
                                </EditCellStyle>
                                <CellStyle BackColor="#99CCFF">
                                </CellStyle>
                            </dx:GridViewDataTextColumn>
                        </Columns>
                        <SettingsBehavior FilterRowMode="OnClick" />
            <SettingsPager Visible="False" Mode="ShowAllRecords">
            </SettingsPager>
            <SettingsEditing Mode="Batch">
            </SettingsEditing>
            <Settings VerticalScrollableHeight="350" VerticalScrollBarMode="Auto" VerticalScrollBarStyle="Virtual" ShowFilterRow="True" ShowFilterRowMenu="True" />
                        <SettingsSearchPanel Visible="True" />
                    </dx:ASPxGridView>
                </div >
                <dx:ASPxButton ID="BtnHDNR" runat="server" Text="查询" Width="150px" OnClick="BtnHDNR_Click"  >
                </dx:ASPxButton>
                            <dx:ASPxButton ID="BtnSoaExp" ClientInstanceName="BtnSoaExp" runat="server" Text="导出所有设置数据" Theme="Youthful" Width="300px" OnClick="BtnSoaExp_Click">
                            </dx:ASPxButton>
            </div>
    </div>
                <dx:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="GV_170511" FileName="活动基础数据设置" ExportEmptyDetailGrid="True"></dx:ASPxGridViewExporter>
    </form>
</body>
</html>
