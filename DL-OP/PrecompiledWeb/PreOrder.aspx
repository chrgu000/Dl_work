<%@ page language="C#" autoeventwireup="true" inherits="PreOrder, dlopwebdll" enableviewstate="false" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>新增预订单-酬宾</title>

    <style type="text/css">
        .auto-style2 {
            width: 200px;
            height: 23px;
        }

        .auto-style4 {
            width: auto;
            height: 23px;
        }

        .dxeBase {
            font: 12px Tahoma, Geneva, sans-serif;
        }

        .auto-style10 {
            width: 359px;
        }

        .auto-style12 {
            height: 18px;
            width: 359px;
        }

        .auto-style13 {
            height: 10px;
        }

        .auto-style14 {
            height: 48px;
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

        .auto-style19 {
            height: 10px;
            font-size: small;
        }

        </style>

</head>
<script type="text/javascript">
    Math.formatFloat = function (f, digit) {
        var m = Math.pow(10, digit);
        return parseInt(f * m, 10) / m;
    }
    //编辑修改数量
    function OrderGrid_BatchEditEndEditing(s, e) {
        //通过单位组分解换算率
        var UnitGroupColumn = s.GetColumnByField("UnitGroup");  //单位组换算率
        //alert(e.rowValues[s.GetColumnByField("UnitGroup").index].value);
        var ch = new Array();    //获取换算率
        ch = e.rowValues[UnitGroupColumn.index].value.split("=");
        for (i = 0; i < ch.length; i++) {
            ch[i] = ch[i].replace(/[^0-9,.]/ig, "");
            //alert(ch[i]);
        }
        //获取单位中文名称
        var chUnit = new Array();
        chUnit = e.rowValues[UnitGroupColumn.index].value.split("=");
        for (i = 0; i < chUnit.length; i++) {
            chUnit[i] = chUnit[i].replace(/[^\u4e00-\u9fa5]/gi, "");
            //alert(chUnit[i]);
        }

        //计算基本计量单位数量是否满足整数
        var cComUnitQTYColumn = s.GetColumnByField("cComUnitQTY");  //定义基本计量单位数量字段,//数量1,基本单位数量,FieldName="cComUnitQTY"
        var cellInfoQTYa = e.rowValues[cComUnitQTYColumn.index];  //获取基本计量单位数量
        //if (cellInfoQTYa.value % (ch[0] / ch[1]) != 0) {
        //    //var rowsindex = "OrderGrid_DXDataRow" + s.GetFocusedRowIndex();
        //    //var rows = document.getElementById(rowsindex);  //根据id找到这行
        //    //var cell = rows.cells[5];//获取某行下面的某个td元素
        //    //var a = cell.innerHTML;
        //    alert("请输入正确的基本单位数量,为小包装的整倍数,否则系统无法保存订单!")
        //}
        //计算金额
        var cInvDefine2QTYColumn = s.GetColumnByField("cInvDefine2QTY");//数量2,小包装单位数量,FieldName="cInvDefine2QTY"
        var cInvDefine1QTYColumn = s.GetColumnByField("cInvDefine1QTY");//数量3,大包装单位数量,FieldName="cInvDefine1QTY"
        var cComUnitPriceColumn = s.GetColumnByField("cComUnitPrice"); //单价,FieldName="cComUnitPrice"
        //var cellInfoQTYa = e.rowValues[strCarplateNumberColumn.index];  //数量1,基本单位数量,FieldName="cComUnitQTY"
        var cellInfoQTYb = e.rowValues[cInvDefine2QTYColumn.index];      //数量2,小包装单位数量,FieldName="cInvDefine2QTY"
        var cellInfoQTYc = e.rowValues[cInvDefine1QTYColumn.index];      //数量3,大包装单位数量,FieldName="cInvDefine1QTY"
        var cellInfoPrice = e.rowValues[cComUnitPriceColumn.index];      //单价,FieldName="cComUnitPrice"
        //var BasicUnitAmount = (parseFloat(cellInfoQTYa.value) + parseFloat(parseFloat(cellInfoQTYb.value * 10*10) * parseFloat((parseFloat(ch[0]) / parseFloat(ch[1])) * 10*10)) / 10000 + parseFloat(parseFloat(cellInfoQTYc.value * 10*10) * (parseFloat(ch[0]) * 10*10)/10000));//基本单位数量汇总
        var BasicUnitAmount = (parseFloat(cellInfoQTYa.value) + parseFloat(parseFloat(cellInfoQTYb.value * 10 * 10) * parseFloat((parseFloat(ch[0]) / parseFloat(ch[1])) * 10 * 10)) / 10000 + parseFloat(parseFloat(cellInfoQTYc.value * 10 * 10) * (parseFloat(ch[0]) * 10 * 10) / 10000));//基本单位数量汇总
        var QTYPrice = BasicUnitAmount * parseFloat(cellInfoPrice.value);
        var rowsindex = "OrderGrid_DXDataRow" + s.GetFocusedRowIndex();
        var rows = document.getElementById(rowsindex);  //根据id找到这行
        var cell = rows.cells[15];//获取某行下面的某个td元素,金额
        //var a = cell.innerHTML;
        //var index1 = a.indexOf(">");
        //var index2 = a.indexOf("</div");
        //var b = a.substring(index1 + 1, index2);
        //var c = a.replace(b, QTYPrice);
        //cell.innerHTML = c;
        cell.innerHTML = QTYPrice.toFixed(2);
        var cell11 = rows.cells[11];//获取某行下面的某个td元素,基本单位汇总
        cell11.innerHTML = BasicUnitAmount + chUnit[0];
        var cell13 = rows.cells[13];//获取某行下面的某个td元素,包装结果
        cell13.innerHTML = parseInt((BasicUnitAmount * 10 * 10) / (parseFloat(ch[0]) * 10 * 10)) + chUnit[2] + parseInt((parseInt((BasicUnitAmount * 10 * 10)) % parseInt(parseFloat(ch[0]) * 10 * 10)) / ((parseFloat(ch[0]) / parseFloat(ch[1])) * 10 * 10)) + chUnit[1] + parseFloat(parseFloat((parseInt(BasicUnitAmount * 10 * 10) % (parseInt(parseFloat(ch[0]) * 10 * 10 / parseFloat(ch[1]))))) / (100)) + chUnit[0];

    }

    //OrderGrid修改提示功能
    function EndCallBack(s, e) {
        if (s.cpAlertMsg != "" && s.cpAlertMsg != null) {
            alert(s.cpAlertMsg);
            s.cpAlertMsg = null;
        }
    }

</script>
<body>
    <form id="form1" runat="server">
       
        <div>
            <table align="left" style="width: 100%; float: none;">
                <tr>
                    <td class="auto-style15">
                        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                        网单号</td>
                    <td class="auto-style2">
                        <dx:ASPxTextBox ID="TxtOrderBillNo" runat="server" Width="185px" Height="18px" ReadOnly="True" Theme="Office2010Blue" Text="保存后自动生成" CssClass="auto-style16" BackColor="#3366FF">
                            <ReadOnlyStyle BackColor="White">
                            </ReadOnlyStyle>
                        </dx:ASPxTextBox>
                    </td>
                    <td class="auto-style15">订单日期</td>
                    <td class="auto-style2">
                        <dx:ASPxTextBox ID="TxtBillDate" runat="server" Width="175px" Height="18px" ReadOnly="True" Theme="Office2010Blue" CssClass="auto-style16" BackColor="#3366FF">
                            <ReadOnlyStyle BackColor="White">
                            </ReadOnlyStyle>
                        </dx:ASPxTextBox>
                    </td>
                    <td ><a style="float: left; margin-left: 15px;display:none;">酬宾类型：</a><div style="display:none;">
                            <dx:ASPxTextBox ID="TxtCBLX" ClientInstanceName="TxtCBLX" runat="server" Width="135px" Height="18px" EnableTheming="True" ReadOnly="True" style="margin-bottom: 0px">
                            </dx:ASPxTextBox></div></td>
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
                            <dx:ASPxButton ID="ASPxButton2" runat="server" Text="选择" Width="40px" AutoPostBack="false" Theme="Office2010Blue" Height="16px" >
                                <ClientSideEvents Click="function(s, e) { Customer.Show(); }" />
                            </dx:ASPxButton>
                        </div>


                    </td>
                    <td class="auto-style19">下单时间</td>
                    <td class="auto-style13">
                        <dx:ASPxTextBox ID="TxtBillTime" runat="server" Width="185px" Height="18px" ReadOnly="True" Theme="Office2010Blue" Text="保存后自动生成" CssClass="auto-style16" BackColor="#3366FF">
                            <ReadOnlyStyle BackColor="White">
                            </ReadOnlyStyle>
                        </dx:ASPxTextBox>
                    </td>
                    <td class="auto-style19"><div ><a style="float:left;margin-right:10px">信用额</a>
                        <dx:ASPxTextBox ID="TxtCusCredit1" ClientInstanceName="TxtCusCredit1" runat="server" Width="175px" Height="18px" EnableTheming="True" ReadOnly="True" Theme="Office2010Blue" >
                        </dx:ASPxTextBox>
                        </div><div style="display:none"><dx:ASPxTextBox ID="TxtCusCredit" ClientInstanceName="TxtCusCredit" runat="server" Width="175px" Height="18px" EnableTheming="True" 
    ReadOnly="True" Theme="Office2010Blue" Visible="false">
                        </dx:ASPxTextBox></div>

                    </td>
                    <td class="auto-style13">
                        &nbsp;</td>
                    <td class="auto-style13"></td>
                </tr>
                </table>
        </div>
        <div>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server"><ContentTemplate>
            <dx:ASPxGridView ID="OrderGrid" ClientInstanceName="OrderGrid" runat="server" AutoGenerateColumns="False" EnableTheming="True" KeyFieldName="cInvCode" 
                OnRowDeleting="OrderGrid_RowDeleting" ClientIDMode="Static" ShowVerticalScrollBar="true" Theme="Office2010Blue" OnRowValidating="OrderGrid_RowValidating" 
                OnInit="OrderGrid_Init"
                OnRowUpdating="OrderGrid_RowUpdating" OnCustomUnboundColumnData="GridViewShippingMethod_CustomUnboundColumnData" Width="1230px" KeyboardSupport="True" Font-Size="9pt">
                <SettingsText Title="订单明细表" ContextMenuSummarySum="合计" />

                <ClientSideEvents BatchEditEndEditing="OrderGrid_BatchEditEndEditing" EndCallback="function(s, e) {EndCallBack(s,e);}" />
                <TotalSummary>
                    <dx:ASPxSummaryItem FieldName="cInvName" SummaryType="Count" />
                    <dx:ASPxSummaryItem FieldName="hh" SummaryType="Sum" DisplayFormat="{0:F}" ShowInColumn="金额" />
                </TotalSummary>
                <Columns>
                    <dx:GridViewDataTextColumn Caption="编码" FieldName="cInvCode" Visible="False" VisibleIndex="1">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="名称" FieldName="cInvName" VisibleIndex="3" ReadOnly="True" Width="150px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="规格" FieldName="cInvStd" VisibleIndex="4" ReadOnly="True" Width="100px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="基本单位" FieldName="cComUnitName" VisibleIndex="7" ReadOnly="True" Width="50px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="小包装单位" FieldName="cInvDefine2" VisibleIndex="9" ReadOnly="True" Width="55px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="大包装单位" FieldName="cInvDefine1" VisibleIndex="11" ReadOnly="True" Width="55px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="单位组" FieldName="UnitGroup" VisibleIndex="5" ReadOnly="True" Width="120px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="单价" VisibleIndex="15" FieldName="cComUnitPrice" ReadOnly="True" Width="50px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="金额" VisibleIndex="16" FieldName="cComUnitAmount" ReadOnly="True" Width="100px">
                        <EditFormSettings Visible="False" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="基本单位汇总" FieldName="cInvDefineQTY" VisibleIndex="12" ReadOnly="True" Width="80px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="小包装换算率" FieldName="cInvDefine14" VisibleIndex="17" Width="0px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="大包装换算率" FieldName="cInvDefine13" VisibleIndex="18" Width="0px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="序号" FieldName="hh" ReadOnly="True" UnboundType="Integer" VisibleIndex="2" Width="30px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="包装结果" FieldName="pack" ReadOnly="True" VisibleIndex="14" Width="100px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataSpinEditColumn Caption="基本单位数量" FieldName="cComUnitQTY" VisibleIndex="6" EditFormSettings-VisibleIndex="0" Width="70px">
                        <PropertiesSpinEdit DisplayFormatString="g" MaxValue="99999" AllowMouseWheel="False" AllowNull="False" NullText="0">
                        </PropertiesSpinEdit>
                        <EditFormSettings VisibleIndex="0"></EditFormSettings>
                        <CellStyle BackColor="#FF66FF">
                        </CellStyle>
                    </dx:GridViewDataSpinEditColumn>
                    <dx:GridViewDataSpinEditColumn Caption="小包装单位数量" FieldName="cInvDefine2QTY" VisibleIndex="8" Width="80px">
                        <PropertiesSpinEdit DisplayFormatString="g" MaxValue="99999" NumberType="Integer" AllowMouseWheel="False" AllowNull="False" NullText="0">
                        </PropertiesSpinEdit>
                        <CellStyle BackColor="#FF66FF">
                        </CellStyle>
                    </dx:GridViewDataSpinEditColumn>
                    <dx:GridViewDataSpinEditColumn Caption="大包装单位数量" FieldName="cInvDefine1QTY" VisibleIndex="10" Width="80px">
                        <PropertiesSpinEdit DisplayFormatString="g" MaxValue="99999" NumberType="Integer" AllowMouseWheel="False" AllowNull="False" NullText="0">
                        </PropertiesSpinEdit>
                        <CellStyle BackColor="#FF66FF">
                        </CellStyle>
                    </dx:GridViewDataSpinEditColumn>
                    <dx:GridViewDataTextColumn Caption="可用库存量" FieldName="Stock" ReadOnly="True" VisibleIndex="13" Width="60px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewCommandColumn Caption="删除" ShowDeleteButton="True" VisibleIndex="0" Width="60px">
                        <HeaderTemplate>
                            <dx:ASPxButton ID="Choose" runat="server" Text="选择商品"  AutoPostBack="False" EnableTheming="True" Theme="Office2010Blue" ForeColor="Red" Width="55px">
                                <ClientSideEvents Click="function(s, e) {
Inventory.Show()	
}" /></dx:ASPxButton>
                        </HeaderTemplate>
                    </dx:GridViewCommandColumn>
                    <dx:GridViewDataTextColumn Caption="执行单价" FieldName="ExercisePrice" ReadOnly="True" VisibleIndex="19" Width="0px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="执行金额" FieldName="xx" ReadOnly="True" VisibleIndex="20" Width="0px" UnboundType="Decimal">
                    </dx:GridViewDataTextColumn>
                </Columns>
                <SettingsBehavior AllowSort="False" AllowFocusedRow="True" ColumnResizeMode="Control" />
                <SettingsPager Mode="ShowAllRecords" />
                <SettingsEditing EditFormColumnCount="99" Mode="Batch" />
                <Settings ShowTitlePanel="true" ShowFooter="True" VerticalScrollableHeight="200" VerticalScrollBarMode="Auto" HorizontalScrollBarMode="Visible" />
                <SettingsText CommandCancel="取消" CommandUpdate="确定" CommandDelete="删除" />
                <SettingsCommandButton>
                    <UpdateButton Text="保存购物信息" ButtonType="Link">
                        <Styles>
                            <Style Font-Size="Medium" ForeColor="Red"  HorizontalAlign="Center" VerticalAlign="Middle">
                                <HoverStyle BackColor="#FFFF99">
                                </HoverStyle>
                                <Paddings PaddingRight="30px" />
                            </Style>
                        </Styles>
                    </UpdateButton>
                    <CancelButton Text="取消修改" ButtonType="Link">
                        <Styles>
                            <Style Font-Size="Medium" ForeColor="Red" HorizontalAlign="Center" VerticalAlign="Middle">
                                <HoverStyle BackColor="#FFFF99">
                                </HoverStyle>
                                <Paddings PaddingRight="1000px" />
                            </Style>
                        </Styles>
                    </CancelButton>
                    <DeleteButton ButtonType="Button" Text="删除">
                        <Styles>
                            <Style Width="50px">
                            </Style>
                        </Styles>
                    </DeleteButton>
                </SettingsCommandButton>
                <SettingsDataSecurity AllowInsert="False" />
                <Styles>
                    <Footer BackColor="#C2D491">
                    </Footer>
                </Styles>
            </dx:ASPxGridView>
</ContentTemplate></asp:UpdatePanel>
            <table class="dxflInternalEditorTable">
                <tr>
                    <td class="auto-style14">
                        <div style="float: left; margin-left: 10px">
                            <dx:ASPxButton ID="BtnSaveOrder" runat="server" Text="保存购物信息后提交生成正式订单" OnClick="BtnSaveOrder_Click" Height="35px" Width="160px"
                                 ClientInstanceName="BtnSaveOrder" EnableTheming="True" Font-Size="12pt"
                                 Font-Names="微软雅黑" Theme="SoftOrange" >
                            </dx:ASPxButton>
                        </div>
                    </td>
                    <td class="auto-style14"></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </div>
        <div>
            <dx:ASPxPopupControl ID="Customer" runat="server" CloseAction="CloseButton" CloseOnEscape="True" Modal="True" Target="_blank"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="Customer"
                HeaderText="选择开票单位" AllowDragging="True" PopupAnimationType="None" EnableViewState="true" ClientIDMode="Static" Height="203px" Width="447px">
                <ClientSideEvents PopUp="function(s, e) { CustomerGrid.Focus(); }" />
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                        <dx:ASPxPanel ID="ASPxPanel1" runat="server" DefaultButton="BtnCustomerOk">
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent2" runat="server">
                                    <table>
                                        <tr>
                                            <td class="auto-style12">
                                                <dx:ASPxGridView ID="CustomerGrid" runat="server" AutoGenerateColumns="False" EnableTheming="True" ClientInstanceName="CustomerGrid"
                                                    Theme="Office2010Blue" KeyFieldName="cCusCode" Width="409px"
                                                    OnCustomUnboundColumnData="GridViewShippingMethod_CustomUnboundColumnData">
                                                    <ClientSideEvents RowDblClick="function(s, e) {
Customer.Hide();
Customer_dbclick();	
}" />
                                                    <Columns>
                                                        <dx:GridViewDataTextColumn ShowInCustomizationForm="True" VisibleIndex="0" Caption="客户编码" FieldName="cCusCode" ReadOnly="True" Visible="False">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="序号" FieldName="hh"  Width="50px" UnboundType="Integer" ShowInCustomizationForm="True" VisibleIndex="1" ReadOnly="True">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn ShowInCustomizationForm="True" VisibleIndex="2" Caption="单位名称" FieldName="cCusName" ReadOnly="True">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="业务员" FieldName="cCusPPerson" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="3">
                                                        </dx:GridViewDataTextColumn>
                                                    </Columns>
                                                    <SettingsBehavior AllowFocusedRow="True" AllowSort="False" />
                                                    <SettingsPager NumericButtonCount="15" PageSize="15" Mode="ShowAllRecords">
                                                    </SettingsPager>
                                                    <Settings VerticalScrollBarMode="Visible" />
                                                    <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
                                                </dx:ASPxGridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td class="auto-style10">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div style="text-align: center">

                                                    <dx:ASPxButton ID="BtnCustomerOk" ClientInstanceName="BtnCustomerOk" runat="server" Text="确定" Width="80px"
                                                        AutoPostBack="true" Style="float: left; margin-right: 8px" OnClick="BtnCustomerOk_Click">
                                                        <ClientSideEvents Click="function(s, e) {
Customer.Hide();
 }" />
                                                    </dx:ASPxButton>
                                                    <dx:ASPxButton ID="Btn_Customer_Cancel" runat="server" Text="取消" Width="80px" AutoPostBack="False"
                                                        ClientInstanceName="Btn_Customer_Cancel" Style="float: left; margin-right: 8px">
                                                        <ClientSideEvents Click="function(s, e) { Customer.Hide(); }" />
                                                    </dx:ASPxButton>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxPanel>
                    </dx:PopupControlContentControl>
                </ContentCollection>
                <ContentStyle>
                    <Paddings PaddingBottom="5px" />
                </ContentStyle>
            </dx:ASPxPopupControl>
        </div>
        <div>
        <dx:ASPxPopupControl ID="Inventory" runat="server" CloseAction="CloseButton" LoadContentViaCallback="OnFirstShow"
            PopupElementID="Inventory" PopupVerticalAlign="TopSides" PopupHorizontalAlign="WindowCenter"
            ShowFooter="True" Width="888px" Height="550px" HeaderText="选择商品" ClientInstanceName="Inventory" CloseOnEscape="True" Modal="True">
            <ContentCollection>

                <dx:PopupControlContentControl ID="PopupControlContentControl" runat="server">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
   <ContentTemplate> 

                    <div style="float: left">
                        <dx:ASPxButton ID="RefreshTree" runat="server" AutoPostBack="False" Style="margin: 6px 10px 6px 10px" Text="刷新目录">
                            <ClientSideEvents Click="function(s, e) { Inventory.PerformCallback(); }" />
                        </dx:ASPxButton>
                    </div>
                    <div style="float: left; margin: 6px 0px 6px 0px">
                        <dx:ASPxButton ID="BtnInvOK" runat="server" ClientInstanceName="BtnInvOK" OnClick="BtnInvOK_Click" Style="float: left; margin-right: 8px" Text="确定" Width="80px">
                            <ClientSideEvents Click="function(s, e) {
Inventory.Hide();
 }" />
                        </dx:ASPxButton>
                    </div>
                    <div style="float: left; margin: 6px 0px 6px 0px">
                        <dx:ASPxButton ID="BtnInvOK_Cancel" runat="server" AutoPostBack="False" ClientInstanceName="BtnInvOK_Cancel" Style="float: left; margin-right: 8px" Text="取消" Width="80px">
                            <ClientSideEvents Click="function(s, e) { Inventory.Hide(); }" />
                        </dx:ASPxButton>

                    </div>
                    <div style="float: left; margin: 6px 300px 6px 0px">
                        <dx:ASPxButton ID="BtnInv_Reset" runat="server" AutoPostBack="true" ClientInstanceName="BtnInv_Reset" OnClick="BtnInv_Reset_Click"
                            Style="float: left; margin-right: 8px" Text="清除所有选择项" Width="120px">
                            <ClientSideEvents Click="function(s, e) { Inventory.Show(); }" />
                        </dx:ASPxButton>

                    </div>
                    <div style="float: left">
                        <dx:ASPxTreeList ID="treeList" runat="server" AutoGenerateColumns="False" Caption="选择物料大类 ↓"
                            ClientInstanceName="treeList" EnableTheming="True" KeyFieldName="KeyFieldName"
                            OnCustomDataCallback="treeList_CustomDataCallback" 
                            ParentFieldName="ParentFieldName" Theme="Office2010Blue" Width="335px">
                            <Columns>
                                <dx:TreeListTextColumn Caption="产品分类" FieldName="NodeName" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="0">
                                </dx:TreeListTextColumn>
                            </Columns>
                            <Settings ScrollableHeight="290" VerticalScrollBarMode="Auto" />
                            <SettingsBehavior AllowFocusedNode="True" AllowSort="False" ProcessFocusedNodeChangedOnServer="True" ProcessSelectionChangedOnServer="True" 
                                AllowDragDrop="False" ExpandCollapseAction="NodeDblClick" FocusNodeOnExpandButtonClick="False" FocusNodeOnLoad="False" />
                            <SettingsCookies CookiesID="NewOrder" Enabled="True" />
                            <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
                            <Images>
                                <CollapsedButton IconID="actions_addfile_16x16">
                                </CollapsedButton>
                                <ExpandedButton IconID="actions_apply_16x16">
                                </ExpandedButton>
                            </Images>
                            <Styles>
                                <FocusedNode BackColor="#9966FF">
                                </FocusedNode>
                            </Styles>
                            <ClientSideEvents CustomDataCallback="function(s, e) {cscode();
}" FocusedNodeChanged="function(s, e) {
            var key = treeList.GetFocusedNodeKey();
            treeList.PerformCustomDataCallback(key);
}" />
                        </dx:ASPxTreeList>
                    </div>
                    <div style="float: left;">

                                <asp:Button ID="btn" runat="server" OnClick="btn_Click" Text="Button" Style="display: none;" />
                                <%--<input id='ctl' type='button' onclick='cscode();' style="display: none;" />--%>
                                <dx:ASPxGridView ID="TreeDetail" ClientInstanceName="TreeDetail" runat="server" AutoGenerateColumns="False" Theme="Office2010Blue"
                                    ShowVerticalScrollBar="true" KeyFieldName="cInvCode" PreviewFieldName="cInvName" Caption="选择物料明细 ↓" Width="515px">
                                    <ClientSideEvents BeginCallback="function(s, e) {
}" />
                                    <Columns>
                                        <dx:GridViewDataTextColumn FieldName="cInvName" VisibleIndex="3" Width="150px" Caption="名称" ReadOnly="True">
                                            <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="cInvStd" ReadOnly="True" VisibleIndex="4" Width="100px" Caption="规格">
                                            <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="cComUnitName" VisibleIndex="5" Caption="单位" Width="30px" ReadOnly="True">
                                            <Settings AllowAutoFilter="False" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewCommandColumn Caption="选择" ShowSelectCheckbox="True" VisibleIndex="0" Width="35px" SelectAllCheckboxMode="AllPages">
                                        </dx:GridViewCommandColumn>
                                        <dx:GridViewDataTextColumn Caption="编码" FieldName="cInvCode" VisibleIndex="2" Width="0px">
                                            <Settings AllowAutoFilter="False" />
                                        </dx:gridviewdatatextcolumn>
                                        <dx:GridViewDataImageColumn Caption=" " FieldName="img" VisibleIndex="1" Width="25px">
                                        </dx:GridViewDataImageColumn>
                                    </Columns>
                                    <SettingsBehavior AllowSort="False" AllowFocusedRow="True" AllowDragDrop="False" FilterRowMode="OnClick" />
                                    <SettingsPager Visible="False" Mode="EndlessPaging" NumericButtonCount="999">
                                    </SettingsPager>
                                    <SettingsEditing EditFormColumnCount="99">
                                    </SettingsEditing>
                                    <Settings VerticalScrollBarMode="Visible" ShowFooter="False" ShowTitlePanel="True" VerticalScrollableHeight="220" ShowFilterRow="True" ShowFilterRowMenu="True" />
                                    <SettingsText CommandApplySearchPanelFilter="查询" CommandCancel="取消" CommandClearSearchPanelFilter="清除" ConfirmOnLosingBatchChanges="未保存的数据将丢失,是否离开?" SearchPanelEditorNullText="请输入查找内容..." />
                                    <SettingsDataSecurity AllowDelete="False" AllowInsert="False" AllowEdit="False" />
                                    <SettingsSearchPanel AllowTextInputTimer="False" ColumnNames="名称;规格" ShowApplyButton="True" ShowClearButton="True" Visible="True" />
                                </dx:ASPxGridView>                                         
                                    </div>
                    </div>
               
       </ContentTemplate>                               
</asp:UpdatePanel> 

       </dx:PopupControlContentControl>

            </ContentCollection>
            <FooterTemplate>
            </FooterTemplate>
        </dx:ASPxPopupControl>

        </div>
             
    </form>
</body>
</html>
<script type="text/javascript">

    //调用btOK按钮事件
    function Customer_dbclick() {
        document.getElementById("<%=BtnCustomerOk.ClientID%>").click();
    }

    var cscode = function () {
        document.getElementById("<%=btn.ClientID%>").click();
    }
</script>
