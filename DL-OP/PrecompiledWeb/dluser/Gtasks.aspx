<%@ page language="C#" autoeventwireup="true" inherits="dluser_Gtasks, dlopwebdll" viewstatemode="Enabled" enableviewstate="false" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>待办工作</title>
    <style type="text/css">
        .auto-style2 {
            width: 200px;
            height: 23px;
        }

        .auto-style3 {
            width: 60px;
            height: 23px;
            font-size: small;
        }

        .auto-style4 {
            width: auto;
            height: 23px;
        }

        .auto-style5 {
            height: 25px;
        }

        .auto-style6 {
            height: 20px;
            font-size: small;
        }

        .auto-style7 {
            width: 200px;
            height: 20px;
        }

        .dxeBase {
            font: 12px Tahoma, Geneva, sans-serif;
        }

        .auto-style13 {
            height: 10px;
        }

        .auto-style15 {
            width: 70px;
            height: 23px;
            font-size: small;
        }

        .auto-style16 {
            font-size: small;
            height: 20px;
        }

        .auto-style18 {
            height: 25px;
            font-size: small;
        }

        .auto-style19 {
            height: 10px;
            font-size: small;
        }
    </style>
    <script type="text/javascript">
        //window.onload = function () {
        //    grid_SelectionChanged(s, e);
        //}
        //编辑修改数量
        function GetSum(s, e) {
            //通过单位组分解换算率
            var UnitGroupColumn = s.GetColumnByField("strBillNo");  //单位组换算率
            //alert(e.rowValues[s.GetColumnByField("UnitGroup").index].value);
            //var ch = new Array();    //获取换算率
            //ch = e.rowValues[UnitGroupColumn.index].value.split("=");
        }
        function grid_SelectionChanged(s, e) {
            s.GetSelectedFieldValues("isum", GetSelectedFieldValuesCallback);
        }
        function GetSelectedFieldValuesCallback(values) {
            var maxsum = 0;
            for (var i = 0; i < values.length; i++) {
                maxsum = maxsum + values[i];
            }
            //document.getElementById("maxsum").innerHTML = grid.GetSelectedRowCount();
            document.getElementById("maxsum").innerHTML = maxsum;
            if (maxsum >= 30000) {
                document.getElementById('maxsum').style.backgroundColor = "red ";//背景色
            } else {
                document.getElementById('maxsum').style.backgroundColor = "white ";//背景色
            }
            //alert(maxsum);
        }
        function CheckSum() {
            if ((document.getElementById('maxsum').innerText.trim()) < 30000)
                var r = confirm("当前金额小于30000,是否继续审核?");
            return r;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <div style="float: left; margin-left: 15px">
                <dx:ASPxButton ID="BtnRefresh" runat="server" Text="刷  新" OnClick="BtnRefresh_Click" Height="28px" Width="80px"></dx:ASPxButton>
                <asp:Button ID="BatchAudit" runat="server" OnClick="BatchAudit_Click" Text="批量审核" OnClientClick="return CheckSum()" />
            </div>
            <div style="float: left; margin-left: 25px; margin-top: 5px; margin-bottom: 10px">
                <asp:RadioButton ID="RadioButton1" runat="server" GroupName="rbtn1" Text="自提   " />
                <asp:RadioButton ID="RadioButton2" runat="server" GroupName="rbtn1" Text="配送   " />
                <asp:RadioButton ID="RadioButton3" runat="server" GroupName="rbtn1" Text="全部   " Checked="true" />
            </div>
            <dx:ASPxGridView ID="GridOrder" ClientInstanceName="GridOrder" runat="server" AutoGenerateColumns="False"
                OnCustomButtonCallback="GridOrder_CustomButtonCallback" OnRowUpdating="GridOrder_RowUpdating"
                EnableTheming="True" Theme="Office2010Blue" KeyFieldName="strBillNo" Font-Size="9pt" OnHtmlRowPrepared="GridOrder_HtmlRowPrepared">
                <ClientSideEvents SelectionChanged="grid_SelectionChanged" />
                <Columns>
                    <dx:GridViewCommandColumn Caption="功能区域" VisibleIndex="0" Width="60px"
                        ShowClearFilterButton="True" SelectAllCheckboxMode="Page" ShowSelectCheckbox="True">
                    </dx:GridViewCommandColumn>
                    <dx:GridViewDataTextColumn Caption="客户名称" FieldName="strUserName" VisibleIndex="10" Width="150px" ReadOnly="True">
                        <Settings AutoFilterCondition="Contains" />
                        <EditFormSettings Visible="False" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="开票单位名称" FieldName="ccusname" VisibleIndex="12" Width="150px" ReadOnly="True">
                        <Settings AutoFilterCondition="Contains" />
                        <EditFormSettings Visible="False" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="地址信息" FieldName="cdefine11" VisibleIndex="13" Width="300px" ReadOnly="True">
                        <PropertiesTextEdit>
                            <Style Wrap="False">
                            </Style>
                        </PropertiesTextEdit>
                        <Settings AutoFilterCondition="Contains" />
                        <EditFormSettings Visible="False" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="备注" FieldName="strRemarks" VisibleIndex="14" Width="160px" ReadOnly="True">
                        <Settings AutoFilterCondition="Contains" />
                        <EditFormSettings Visible="False" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataHyperLinkColumn Caption="审核" FieldName="strBillNo" VisibleIndex="3" Width="40px" ReadOnly="True">
                        <PropertiesHyperLinkEdit NavigateUrlFormatString="Gtasks.aspx?billno={0}" Text="审核">
                            <Style ForeColor="#009933">
                            </Style>
                        </PropertiesHyperLinkEdit>
                        <Settings AllowAutoFilter="False" />
                        <EditFormSettings Visible="False" />
                    </dx:GridViewDataHyperLinkColumn>
                    <dx:GridViewDataHyperLinkColumn Caption="驳回" FieldName="strBillNo" VisibleIndex="2" Width="40px" ReadOnly="True">
                        <PropertiesHyperLinkEdit NavigateUrlFormatString="Gtasks.aspx?dbillno={0}" Text="驳回">
                            <Style ForeColor="Red">
                            </Style>
                        </PropertiesHyperLinkEdit>
                        <Settings AllowAutoFilter="False" />
                        <EditFormSettings Visible="False" />
                    </dx:GridViewDataHyperLinkColumn>
                    <dx:GridViewDataHyperLinkColumn Caption="网单号" FieldName="strBillNo" VisibleIndex="6" Width="160px" ReadOnly="True">
                        <PropertiesHyperLinkEdit NavigateUrlFormatString="Gtask_Detail.aspx?vbillno={0}" Target="OrderDetail">
                            <Style ForeColor="#0066FF">
                            </Style>
                        </PropertiesHyperLinkEdit>
                        <Settings AutoFilterCondition="Contains" />
                        <EditFormSettings Visible="False" />
                    </dx:GridViewDataHyperLinkColumn>
                    <dx:GridViewDataTextColumn Caption="单据日期" FieldName="datCreateTime" VisibleIndex="9" Width="100px" ReadOnly="True">
                        <Settings AutoFilterCondition="Contains" />
                        <EditFormSettings Visible="False" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataHyperLinkColumn Caption="修改" FieldName="strBillNo" VisibleIndex="1" Width="40px">
                        <PropertiesHyperLinkEdit NavigateUrlFormatString="OrderFrameDlUser.aspx?billno={0}" Text="修改">
                            <Style ForeColor="blue">
                            </Style>
                        </PropertiesHyperLinkEdit>
                        <Settings AllowAutoFilter="False" />
                        <EditFormSettings Visible="False" />
                    </dx:GridViewDataHyperLinkColumn>
                    <dx:GridViewDataTextColumn Caption="单据类型" FieldName="lngBillType" VisibleIndex="15" Width="70px">
                        <Settings AutoFilterCondition="Contains" />
                        <EditFormSettings Visible="False" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataMemoColumn Caption="驳回备注" FieldName="strRejectRemarks" VisibleIndex="7" Width="120px">
                        <PropertiesMemoEdit DisplayFormatString="{0}">
                        </PropertiesMemoEdit>
                        <Settings AutoFilterCondition="Contains" />
                        <EditFormSettings Caption="备注说明" Visible="True" />
                    </dx:GridViewDataMemoColumn>
                    <dx:GridViewDataHyperLinkColumn Caption="限时驳回" FieldName="strBillNo" VisibleIndex="5" Width="70px">
                        <PropertiesHyperLinkEdit NavigateUrlFormatString="Gtasks.aspx?xdbillno={0}" Text="限时驳回">
                            <Style ForeColor="#FF3300">
                            </Style>
                        </PropertiesHyperLinkEdit>
                    </dx:GridViewDataHyperLinkColumn>
                    <dx:GridViewDataHyperLinkColumn Caption="czts审核" FieldName="strBillNo" ReadOnly="True" VisibleIndex="4" Width="65px">
                        <PropertiesHyperLinkEdit NavigateUrlFormatString="Gtasks.aspx?cztsbillno={0}" Text="审核czts">
                        </PropertiesHyperLinkEdit>
                    </dx:GridViewDataHyperLinkColumn>

                    <dx:GridViewDataTextColumn Caption="金额" FieldName="isum" VisibleIndex="11" Width="80px">
                        <PropertiesTextEdit DisplayFormatString="0.0000">
                        </PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="补贴行政区Code" FieldName="ccodeID" ReadOnly="True" VisibleIndex="16" Width="0px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="客户自提" FieldName="iAddressType" ReadOnly="True" VisibleIndex="17" Width="0px">
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="行政区" FieldName="vsimpleName" ReadOnly="True" VisibleIndex="8" Width="85px">
                    </dx:GridViewDataTextColumn>

                </Columns>
                <SettingsBehavior AllowSort="False" />
                <SettingsPager Mode="ShowAllRecords" PageSize="5" Visible="False">
                </SettingsPager>
                <SettingsEditing Mode="EditFormAndDisplayRow" EditFormColumnCount="1">
                </SettingsEditing>
                <Settings VerticalScrollBarMode="Visible" VerticalScrollableHeight="250" ShowFilterRow="True" />
                <SettingsCommandButton>
                    <EditButton Text="编辑">
                    </EditButton>
                    <ClearFilterButton ButtonType="Link" Text="清除">
                    </ClearFilterButton>
                    <SearchPanelApplyButton ButtonType="Button" Text="查询">
                    </SearchPanelApplyButton>
                    <SearchPanelClearButton ButtonType="Button" Text="清除">
                    </SearchPanelClearButton>
                </SettingsCommandButton>
                <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
                <SettingsSearchPanel ShowApplyButton="True" ShowClearButton="True" Visible="True" />
            </dx:ASPxGridView>
            已选合计金额:<asp:Label ID="maxsum" runat="server" Text="0"></asp:Label><br />
            <div>
                <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="*%" id="frmTitle" nowrap="noWrap" name="fmTitle" valign="top" height="800">
                            <iframe id="OrderDetail" height="100%" width="100%" frameborder="0" src="Gtask_Detail.aspx?vbillno=1"
                                name="OrderDetail"></iframe>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>

                    <table align="left" style="width: 100%; float: none;">
                        <tr>
                            <td class="auto-style15">网单号</td>
                            <td class="auto-style2">
                                <dx:ASPxTextBox ID="TxtOrderBillNo" runat="server" Width="145px" Height="18px" ReadOnly="True" Theme="Office2010Blue" CssClass="auto-style16" BackColor="#3366FF">
                                    <ReadOnlyStyle BackColor="White">
                                    </ReadOnlyStyle>
                                </dx:ASPxTextBox>
                            </td>
                            <td class="auto-style3">制单人</td>
                            <td class="auto-style2">
                                <dx:ASPxTextBox ID="TxtBiller" runat="server" Width="145px" Height="18px" EnableTheming="True" ReadOnly="True" Theme="Office2010Blue" CssClass="auto-style16" BackColor="#3366FF">
                                    <ReadOnlyStyle BackColor="White">
                                    </ReadOnlyStyle>
                                </dx:ASPxTextBox>
                            </td>
                            <td class="auto-style15">订单日期</td>
                            <td class="auto-style2">
                                <dx:ASPxTextBox ID="TxtBillDate" runat="server" Width="145px" Height="18px" ReadOnly="True" Theme="Office2010Blue" CssClass="auto-style16" BackColor="#3366FF">
                                    <ReadOnlyStyle BackColor="White">
                                    </ReadOnlyStyle>
                                </dx:ASPxTextBox>
                            </td>
                            <td class="auto-style4"></td>
                        </tr>
                        <tr>
                            <td class="auto-style19">开票单位</td>
                            <td class="auto-style13">
                                <div style="float: left">
                                    <dx:ASPxTextBox ID="TxtCustomer" runat="server" Width="125px" ReadOnly="True" Theme="Office2010Blue" Text="" Height="18px" EnableViewState="true" CssClass="auto-style16" BackColor="#3366FF">
                                        <ReadOnlyStyle BackColor="White">
                                        </ReadOnlyStyle>
                                    </dx:ASPxTextBox>
                                </div>
                                <div style="float: left; margin-left: 4px">
                                </div>


                            </td>
                            <td class="auto-style19">业务员</td>
                            <td class="auto-style13">
                                <dx:ASPxTextBox ID="TxtSalesman" ClientInstanceName="TxtSalesman" runat="server" Width="145px" Height="18px" EnableTheming="True" Theme="Office2010Blue" CssClass="auto-style16" ReadOnly="true">
                                </dx:ASPxTextBox>


                            </td>
                            <td class="auto-style19">信用额</td>
                            <td class="auto-style13">
                                <dx:ASPxTextBox ID="TxtCusCredit" ClientInstanceName="TxtCusCredit" runat="server" Width="145px" Height="18px" EnableTheming="True" ReadOnly="True" Theme="Office2010Blue" CssClass="auto-style16" BackColor="#3366FF">
                                    <ReadOnlyStyle BackColor="White">
                                    </ReadOnlyStyle>
                                </dx:ASPxTextBox>


                            </td>
                            <td class="auto-style13"></td>
                        </tr>
                        <tr>
                            <td class="auto-style18">送货方式</td>
                            <td class="auto-style5" colspan="3">
                                <div style="float: left" class="auto-style16">
                                    <dx:ASPxTextBox ID="TxtOrderShippingMethod" runat="server" Width="390px" Height="18px" Theme="Office2010Blue" ReadOnly="true" CssClass="auto-style16">
                                    </dx:ASPxTextBox>
                                </div>
                                <div style="float: left; margin-left: 5px">
                                </div>


                            </td>
                            <td class="auto-style18">发运方式</td>
                            <td class="auto-style5">
                                <dx:ASPxTextBox ID="TxtcSCCode" ClientInstanceName="TxtcSCCode" runat="server" Width="145px" Height="18px" EnableTheming="True" ReadOnly="True" Theme="Office2010Blue" CssClass="auto-style16" BackColor="#3366FF">
                                    <ReadOnlyStyle BackColor="White">
                                    </ReadOnlyStyle>
                                </dx:ASPxTextBox>


                            </td>
                            <td class="auto-style5"></td>
                        </tr>
                        <tr>
                            <td class="auto-style16">备注</td>
                            <td class="auto-style6">
                                <dx:ASPxTextBox ID="TxtOrderMark" runat="server" Width="185px" Theme="Office2010Blue" CssClass="auto-style16">
                                    <ClientSideEvents LostFocus="function(s, e) {
	SetCookieText(s.GetText());
}" />
                                </dx:ASPxTextBox>
                            </td>
                            <td class="auto-style16">车型</td>
                            <td class="auto-style6">
                                <div style="float: left;">
                                    <dx:ASPxTextBox ID="Txtcdefine3" runat="server" Width="115px" ReadOnly="True" Theme="Office2010Blue" Text="" Height="18px" EnableViewState="true" CssClass="auto-style16" BackColor="#3366FF">
                                        <ReadOnlyStyle BackColor="White">
                                        </ReadOnlyStyle>
                                    </dx:ASPxTextBox>
                                </div>
                                <div style="float: left; margin-left: 5px">
                                </div>
                            </td>
                            <td class="auto-style16">装车方式</td>
                            <td class="auto-style6">
                                <dx:ASPxTextBox ID="TxtLoadingWays" runat="server" Width="145px" Height="18px" Theme="Office2010Blue" CssClass="auto-style16">
                                    <ClientSideEvents LostFocus="function(s, e) {
	SetCookieTextTxtLoadingWays(s.GetText());
}" />
                                </dx:ASPxTextBox>
                            </td>
                            <td class="auto-style6"></td>
                        </tr>
                        <tr>
                            <td class="auto-style6">下单时间</td>
                            <td class="auto-style6">
                                <dx:ASPxTextBox ID="TxtBillTime" runat="server" Width="145px" Height="18px" ReadOnly="True" Theme="Office2010Blue" CssClass="auto-style16" BackColor="#3366FF">
                                    <ReadOnlyStyle BackColor="White">
                                    </ReadOnlyStyle>
                                </dx:ASPxTextBox>
                            </td>
                            <td class="auto-style6">提货时间</td>
                            <td class="auto-style6">

                                <dx:ASPxTextBox ID="TxtDeliveryDate" runat="server" BackColor="#3366FF" ClientInstanceName="TxtDeliveryDate" CssClass="auto-style16" EnableTheming="True" Height="18px" ReadOnly="True" Theme="Office2010Blue" Width="145px">
                                    <ReadOnlyStyle BackColor="White">
                                    </ReadOnlyStyle>
                                </dx:ASPxTextBox>

                            </td>
                            <td class="auto-style6">销售类型</td>
                            <td class="auto-style7">
                                <dx:ASPxTextBox ID="TxtcSTCode" runat="server" BackColor="#3366FF" ClientInstanceName="TxtcSTCode" CssClass="auto-style16" EnableTheming="True" Height="18px" ReadOnly="True" Theme="Office2010Blue" Width="145px">
                                    <ReadOnlyStyle BackColor="White">
                                    </ReadOnlyStyle>
                                </dx:ASPxTextBox>
                            </td>
                            <td class="auto-style6"></td>
                        </tr>
                    </table>



                    <dx:ASPxGridView ID="ViewOrderGrid" ClientInstanceName="ViewOrderGrid" runat="server" AutoGenerateColumns="False" EnableTheming="True"
                        KeyFieldName="irowno" Theme="Office2010Blue" Font-Size="9pt">
                        <SettingsPager Visible="False" Mode="ShowAllRecords">
                        </SettingsPager>
                        <Settings ShowTitlePanel="true" ShowFooter="True" VerticalScrollableHeight="200" VerticalScrollBarMode="Auto" />
                        <SettingsBehavior AllowSort="false" />
                        <SettingsText Title="订单明细表" />
                        <TotalSummary>
                            <dx:ASPxSummaryItem FieldName="irowno" SummaryType="Count" />
                            <dx:ASPxSummaryItem FieldName="cComUnitAmount" SummaryType="Sum" />
                            <dx:ASPxSummaryItem FieldName="isum" SummaryType="Sum" />
                        </TotalSummary>
                        <Columns>
                            <dx:GridViewDataTextColumn Caption="序号" FieldName="irowno" ReadOnly="True" VisibleIndex="0" SortIndex="0" SortOrder="Ascending" UnboundType="Integer">
                                <Settings AllowSort="True" SortMode="Value" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="名称" FieldName="cinvname" ReadOnly="True" VisibleIndex="1" Width="160px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="规格" FieldName="cInvStd" ReadOnly="True" VisibleIndex="2" Width="100px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="数量" FieldName="iquantity" ReadOnly="True" VisibleIndex="3" Width="60px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="包装结果" FieldName="cdefine22" ReadOnly="True" VisibleIndex="5" Width="150px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="金额" FieldName="cComUnitAmount" ReadOnly="True" VisibleIndex="7" Width="100px">
                                <PropertiesTextEdit DisplayFormatString="{0:F}">
                                </PropertiesTextEdit>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="单价" FieldName="iquotedprice" VisibleIndex="6" Width="65px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="执行价格" FieldName="itaxunitprice" ReadOnly="True" VisibleIndex="8" Width="65px" Visible="False">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="执行金额" FieldName="isum" ReadOnly="True" VisibleIndex="9" Width="120px" Visible="False">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="包装规格" FieldName="cPackingType" VisibleIndex="4" Width="160px">
                            </dx:GridViewDataTextColumn>
                        </Columns>
                        <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
                    </dx:ASPxGridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
