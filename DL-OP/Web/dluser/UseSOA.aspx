<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UseSOA.aspx.cs" Inherits="dluser_UseSOA" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../css/superTables.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../js/superTables.js"></script>
    <script type="text/javascript" src="../js/jquery.superTable.js"></script>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>对账单</title>
    <script type="text/javascript">
        $(function () {
            $("#searchBtn").click(function () {
                var search = $("#SearchWord").val();
                var row = $('#GridView1 >tbody >tr');
                var rowcolumn = $('#GridView1 >tbody >tr >td');
                row.removeClass('hightlite');
                rowcolumn.each(function (column) {
                    var match = $(this).html();
                    if (match == search) {
                        $(this).parent().addClass('hightlite');
                    }
                });
            });
        });
    </script>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }

        .auto-style2 {
            height: 37px;
        }

        .auto-style3 {
            width: 655px;
            border: 2px solid #F4921B;
        }

        .auto-style4 {
            text-align: center;
            width: 650px;
            font-family: 微软雅黑;
            font-size: x-large;
        }

        .auto-style5 {
            width: 650px;
        }

        .auto-style6 {
            width: 650px;
            height: 20px;
        }

        .auto-style7 {
            width: 650px;
            height: 36px;
        }

        .新建样式1 {
            font-family: 微软雅黑;
        }

        .新建样式2 {
            font-family: 微软雅黑;
        }
    </style>
</head>
<body>
    <input type="text" id="SearchWord" /><input type="button" id="searchBtn" value="快速定位行" />
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div>
            <table class="auto-style1">
                <tr>
                    <td class="auto-style2">
                        <h2>☞查询顾客账单信息</h2>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div style="float: left; margin-bottom: 10px; width: 1000px">
                            <a style="float: left">顾客编码</a><div style="float: left">
                                <dx:ASPxTextBox ID="Txtccuscode" ClientInstanceName="Txtccuscode" runat="server" Width="130px" Theme="Office2010Blue" NullText="不输入则为全部客户">
                                </dx:ASPxTextBox>
                            </div>
                            <a style="float: left">账单年度</a><div style="float: left">
                                <dx:ASPxComboBox ID="ComboPeriod0" ClientInstanceName="ComboPeriod" runat="server" EnableTheming="True" Theme="SoftOrange" Width="60px" SelectedIndex="0">
                                    <Items>
                                        <dx:ListEditItem Selected="True" Text="2016" Value="2016" />
                                        <dx:ListEditItem Text="2017" Value="2017" />
                                        <dx:ListEditItem Text="2018" Value="2018" />
                                        <dx:ListEditItem Text="2019" Value="2019" />
                                        <dx:ListEditItem Text="2020" Value="2020" />
                                        <dx:ListEditItem Text="2021" Value="2021" />
                                        <dx:ListEditItem Text="2022" Value="2022" />
                                    </Items>
                                    <ClearButton Visibility="False">
                                    </ClearButton>
                                </dx:ASPxComboBox>
                            </div>
                            <a style="float: left">账单期间</a><div style="float: left">
                                <dx:ASPxComboBox ID="ComboPeriod" ClientInstanceName="ComboPeriod" runat="server" EnableTheming="True" Theme="SoftOrange" Width="60px" SelectedIndex="0">
                                    <Items>
                                        <dx:ListEditItem Selected="True" Text="全部" Value="0" />
                                        <dx:ListEditItem Text="1" Value="1" />
                                        <dx:ListEditItem Text="2" Value="2" />
                                        <dx:ListEditItem Text="3" Value="3" />
                                        <dx:ListEditItem Text="4" Value="4" />
                                        <dx:ListEditItem Text="5" Value="5" />
                                        <dx:ListEditItem Text="6" Value="6" />
                                        <dx:ListEditItem Text="7" Value="7" />
                                        <dx:ListEditItem Text="8" Value="8" />
                                        <dx:ListEditItem Text="9" Value="9" />
                                        <dx:ListEditItem Text="10" Value="10" />
                                        <dx:ListEditItem Text="11" Value="11" />
                                        <dx:ListEditItem Text="12" Value="12" />
                                    </Items>
                                    <ClearButton Visibility="False">
                                    </ClearButton>
                                </dx:ASPxComboBox>
                            </div>
                            <div style="float: left;">
                                <dx:ASPxRadioButton ID="Radio1" ClientInstanceName="Radio1" runat="server" EnableTheming="True" GroupName="RadioType" Text="全部账单" Theme="PlasticBlue" Checked="True">
                                </dx:ASPxRadioButton>
                            </div>
                            <div style="float: left;">
                                <dx:ASPxRadioButton ID="Radio2" ClientInstanceName="Radio2" runat="server" EnableTheming="True" GroupName="RadioType" Text="已发账单" Theme="PlasticBlue">
                                </dx:ASPxRadioButton>
                            </div>
                            <div style="float: left;">
                                <dx:ASPxRadioButton ID="Radio3" ClientInstanceName="Radio3" runat="server" EnableTheming="True" GroupName="RadioType" Text="未发账单" Theme="PlasticBlue">
                                </dx:ASPxRadioButton>
                            </div>

                            <dx:ASPxButton ID="BtnOk" ClientInstanceName="BtnOk" runat="server" Text="查询" Theme="Youthful" Width="80px" OnClick="BtnOk_Click">
                            </dx:ASPxButton>
                            <br />

                            <dx:ASPxButton ID="BtnSoaExp" ClientInstanceName="BtnSoaExp" runat="server" Text="导出所有账单数据" Theme="Youthful" Width="300px" OnClick="BtnSoaExp_Click">
                            </dx:ASPxButton>
                        </div>
                    </td>

                </tr>
            </table>
        </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div>
                    <dx:ASPxGridView ID="SOAGrid" ClientInstanceName="SOAGrid" runat="server" AutoGenerateColumns="False" EnableTheming="True" Theme="Metropolis"
                        KeyFieldName="keyid">
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
                            <dx:GridViewDataTextColumn Caption="操作员" FieldName="strOper" VisibleIndex="5" Width="60px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="确认时间" FieldName="datCheckTime1" VisibleIndex="7" Width="180px">
                                <Settings AllowAutoFilter="True" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="账单期间" FieldName="intPeriod" VisibleIndex="8" Width="60px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="是否确认" FieldName="bytCheck" VisibleIndex="6" Width="60px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="keyid" FieldName="keyid" VisibleIndex="9" Width="80px">
                            </dx:GridViewDataTextColumn>
                        </Columns>
                        <SettingsBehavior AllowFocusedRow="True" FilterRowMode="OnClick" />
                        <SettingsPager Visible="false" Mode="ShowAllRecords">
                        </SettingsPager>
                        <Settings VerticalScrollableHeight="280" VerticalScrollBarMode="Visible" ShowFilterRowMenu="True" ShowFilterRow="True" />
                        <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
                    </dx:ASPxGridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div style="display: none">
            <div>
                <h2>☞生成、发送顾客账单</h2>
                <div style="float: left">顾客编码：</div>
                <div style="float: left; margin-right: 10px">
                    <dx:ASPxTextBox ID="Txtccuscode1" ClientInstanceName="Txtccuscode1" runat="server" Width="130px" Theme="Office2010Blue">
                    </dx:ASPxTextBox>
                </div>
            </div>
            <div style="float: left">选择账单截止日期：</div>
            <div style="float: left; margin-right: 10px">
                <dx:ASPxDateEdit ID="DateEdit1" ClientInstanceName="DateEdit1" runat="server" Width="120px" EnableTheming="True" Theme="SoftOrange" NullText="请选择日期...">
                </dx:ASPxDateEdit>
            </div>
            <div>
                <dx:ASPxButton ID="BtnSOA" ClientInstanceName="BtnSOA" runat="server" Text="生成账单" Width="80px" EnableTheming="True" Theme="Office2010Blue" OnClick="BtnSOA_Click"></dx:ASPxButton>
            </div>
            <div>
                <table class="auto-style3">
                    <tr>
                        <td class="auto-style4">对账单</td>
                    </tr>
                    <tr class="新建样式1">
                        <td class="auto-style6">您好:</td>
                    </tr>
                    <tr class="新建样式1">
                        <td class="auto-style7"><a style="padding-left: 25px">在过去的业务合作中，感谢您对我公司的支持。经核对，截至<dx:ASPxLabel ID="Lbdate" runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Size="Medium" Font-Underline="True" Text="xxxx年xx月xx日">
                        </dx:ASPxLabel>
                            止：您尚欠我公司货款金额￥<a><dx:ASPxLabel ID="Lbmoney" runat="server" Font-Size="Medium" Font-Underline="True" Text="YYYYY.YY">
                            </dx:ASPxLabel>
                            </a>元，大写<a><dx:ASPxLabel ID="Lbmoneyup" runat="server" Font-Size="Medium" Font-Underline="True" Text="YYYYY.YY">
                            </dx:ASPxLabel>
                            </a>。</a></td>
                    </tr>
                    <tr class="新建样式1">
                        <td class="auto-style6"><a style="padding-left: 25px">如无异议，请确认。</a></td>
                    </tr>
                    <tr>
                        <td class="auto-style5">
                            <dx:ASPxButton ID="BtnSend" ClientInstanceName="BtnSend" runat="server" Text="发送对账单" OnClick="BtnSend_Click" Theme="SoftOrange">
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <a>
            <asp:HiddenField ID="HFccusname" runat="server" />
        </a><a>
            <asp:HiddenField ID="HFccuscode" runat="server" />
            <asp:HiddenField ID="HFdblAmount" runat="server" />
            <asp:HiddenField ID="HFstrUper" runat="server" />
            <asp:HiddenField ID="HFstrEndDate" runat="server" />
        </a>
        <dx:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="SOAGrid" FileName="未确认账单的客户" ExportEmptyDetailGrid="True"></dx:ASPxGridViewExporter>
    </form>
</body>
</html>
