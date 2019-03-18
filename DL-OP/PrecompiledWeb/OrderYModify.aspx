<%@ page language="C#" autoeventwireup="true" inherits="OrderYModify, dlopwebdll" enableviewstate="false" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>修改参照酬宾订单</title>
    <style type="text/css">
        .auto-style12 {
            height: 18px;
            width: 359px;
        }

        .auto-style10 {
            width: 359px;
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
        var cell = rows.cells[16];//获取某行下面的某个td元素,金额
        //var a = cell.innerHTML;
        //var index1 = a.indexOf(">");
        //var index2 = a.indexOf("</div");
        //var b = a.substring(index1 + 1, index2);
        //var c = a.replace(b, QTYPrice);
        //cell.innerHTML = c;
        cell.innerHTML = QTYPrice.toFixed(2);
        var cell11 = rows.cells[11];//获取某行下面的某个td元素,基本单位汇总
        cell11.innerHTML = BasicUnitAmount + chUnit[0];
        var cell14 = rows.cells[14];//获取某行下面的某个td元素,包装结果
        cell14.innerHTML = parseInt((BasicUnitAmount * 10 * 10) / (parseFloat(ch[0]) * 10 * 10)) + chUnit[2] + parseInt((parseInt((BasicUnitAmount * 10 * 10)) % parseInt(parseFloat(ch[0]) * 10 * 10)) / ((parseFloat(ch[0]) / parseFloat(ch[1])) * 10 * 10)) + chUnit[1] + parseFloat(parseFloat((parseInt(BasicUnitAmount * 10 * 10) % (parseInt(parseFloat(ch[0]) * 10 * 10 / parseFloat(ch[1]))))) / (100)) + chUnit[0];
    }

    //OrderGrid修改提示功能
    function EndCallBack(s, e) {
        if (s.cpAlertMsg != "" && s.cpAlertMsg != null) {
            alert(s.cpAlertMsg);
            s.cpAlertMsg = null;
        }
    }

    function fun1() {
        if (window.top == window.self) {
            alert('请在系统框架中运行!');
            window.location.href = "Error.html";
        }
    }
</script>

<body onload="">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div>
                        <div style="float: left; width: 1000px; margin-bottom: 5px">
                            <a style="float: left; padding-right: 16px">网单号:</a><dx:ASPxTextBox ID="TxtBillNo" ClientInstanceName="TxtBillNo" runat="server" Width="180px" Style="float: left; border-style: none;" ReadOnly="true"></dx:ASPxTextBox>
                            <a style="float: left; margin-left: 20px">制单人:</a><dx:ASPxTextBox ID="TxtBiller" ClientInstanceName="TxtBiller" runat="server" Width="140px" Style="float: left; border-style: none;" ReadOnly="true"></dx:ASPxTextBox>
                            <a style="float: left; margin-left: 35px">订单日期:</a><dx:ASPxTextBox ID="TxtBillDate" ClientInstanceName="TxtBillDate" runat="server" Width="140px" Style="float: left; border-style: none;" ReadOnly="true"></dx:ASPxTextBox>
                        </div>
                        <div style="float: left; width: 1000px; margin-bottom: 5px">
                            <a style="float: left;">开票单位:</a><dx:ASPxTextBox ID="TxtKPWDName" ClientInstanceName="TxtKPWDName" runat="server" Width="180px" Style="float: left; border-style: none;" ReadOnly="true"></dx:ASPxTextBox>
                            <a style="float: left; margin-left: 20px">业务员:</a><dx:ASPxTextBox ID="TxtSalesman" ClientInstanceName="TxtSalesman" runat="server" Width="140px" Style="float: left; border-style: none;" ReadOnly="true"></dx:ASPxTextBox>
                            <a style="float: left; margin-left: 35px">信用额:</a><dx:ASPxTextBox ID="TxtCusCredit" ClientInstanceName="TxtCusCredit" runat="server" Width="140px" Style="float: left; border-style: none;" ReadOnly="true"></dx:ASPxTextBox>
                        </div>
                        <div style="float: left; width: 1000px; margin-bottom: 5px">
                            <a style="float: left">送货方式:</a><dx:ASPxTextBox ID="TxtOrderShippingMethod" ClientInstanceName="TxtOrderShippingMethod" runat="server" Width="350px" Style="float: left" ReadOnly="true"></dx:ASPxTextBox>
                            <dx:ASPxButton ID="ASPxButton1" runat="server" Text="选择" Style="float: left" AutoPostBack="false" Theme="SoftOrange">
                                <ClientSideEvents Click="function(s, e) { ShippingMethod.Show(); }" />
                            </dx:ASPxButton>
                            <a style="float: left; margin-left: 22px; height: 16px;">发运方式:</a><dx:ASPxTextBox ID="TxtcSCCode" ClientInstanceName="TxtcSCCode" runat="server" Width="140px" Style="float: left; border-style: none;" ReadOnly="true"></dx:ASPxTextBox>

                        </div>
                        <div style="float: left; width: 1000px; margin-bottom: 5px">
                            <a style="float: left">下单时间:</a><dx:ASPxTextBox ID="TxtBillTime" ClientInstanceName="TxtBillTime" runat="server" Width="180px" Style="float: left; border-style: none;" ReadOnly="true"></dx:ASPxTextBox>
                            <a style="float: left; margin-left: 20px">车型:</a><dx:ASPxTextBox ID="Txtcdefine3" ClientInstanceName="Txtcdefine3" runat="server" Width="120px" Style="float: left" ReadOnly="true"></dx:ASPxTextBox>
                            <dx:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Style="float: left" Text="选择" Theme="SoftOrange">
                                <ClientSideEvents Click="function(s, e) { cdefine3.Show(); }" />
                            </dx:ASPxButton>
                            <a style="float: left; margin-left: 18px">提货时间:</a>
                            <dx:ASPxDateEdit ID="DeliveryDate" ClientInstanceName="DeliveryDate" runat="server" AllowMouseWheel="False" AllowNull="False" AllowUserInput="False" DateOnError="Today"
                                DisplayFormatString="G" EditFormat="Custom" EditFormatString="yyyy-MM-dd HH" EnableTheming="True" Height="18px" Theme="SoftOrange" UseMaskBehavior="True">
                                <CalendarProperties DayNameFormat="Short" FirstDayOfWeek="Monday"></CalendarProperties>
                                <TimeSectionProperties ShowMinuteHand="False" Visible="True">
                                    <TimeEditProperties AllowNull="False" DisplayFormatString="HH" EditFormatString="HH"></TimeEditProperties>
                                </TimeSectionProperties>
                                <ClientSideEvents ValueChanged="function(s, e) {
	SetCookieTextDeliveryDate(s.GetText());
}" />
                            </dx:ASPxDateEdit>
                        </div>
                        <div style="float: left; width: 1000px; margin-bottom: 5px">
                            <a style="float: left; padding-right: 32px">备注:</a><dx:ASPxMemo ID="TxtOrderMark" ClientInstanceName="TxtOrderMark" runat="server" Height="50px" Width="260px" Style="float: left" MaxLength="999"></dx:ASPxMemo>
                            <a style="float: left; margin-left: 20px">装车方式:</a><dx:ASPxMemo ID="TxtLoadingWays" ClientInstanceName="TxtLoadingWays" runat="server" Height="50px" Width="260px" Style="float: left" MaxLength="999"></dx:ASPxMemo>
                        </div>
                    </div>
                    <div>
                        <dx:ASPxGridView ID="OrderGrid" runat="server" AutoGenerateColumns="False" ClientInstanceName="OrderGrid" EnableTheming="True" KeyFieldName="itemid" 
                            OnCustomUnboundColumnData="GridViewShippingMethod_CustomUnboundColumnData" OnRowDeleting="OrderGrid_RowDeleting"  OnInit="OrderGrid_Init"
                            OnRowUpdating="OrderGrid_RowUpdating" OnRowValidating="OrderGrid_RowValidating" ShowVerticalScrollBar="true" Theme="Office2010Blue" Width="1350px" Font-Size="9pt">
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                            
                        <SettingsText CommandCancel="取消" CommandDelete="删除" CommandUpdate="确定" /><ClientSideEvents BatchEditEndEditing="OrderGrid_BatchEditEndEditing" EndCallback="function(s, e) {EndCallBack(s,e);}" /><TotalSummary>
                                
                                
                            <dx:ASPxSummaryItem FieldName="cInvName" SummaryType="Count" /><dx:ASPxSummaryItem FieldName="cComUnitAmount" SummaryType="Sum" /></TotalSummary><Columns>
                                
                                
                                
                                
                                
                                
                                
                                
                                
                                
                                
                                
                                
                                
                                
                                
                                
                                
                                
                                
                                
                                
                                
                                
                                
                            <dx:GridViewCommandColumn ShowDeleteButton="True" VisibleIndex="0" Width="80px">
                                    
                                <HeaderTemplate>
                                        
                                    <dx:ASPxButton ID="Choose" runat="server" AutoPostBack="False" EnableTheming="True" ForeColor="Red" Text="选择商品" Theme="Office2010Blue" Width="55px">
                                            
                                        <ClientSideEvents Click="function(s, e) {
Inventory.Show()	
}" /></dx:ASPxButton></HeaderTemplate></dx:GridViewCommandColumn><dx:GridViewDataTextColumn Caption="编码" FieldName="cInvCode" ReadOnly="True" Visible="False" VisibleIndex="1">
                                </dx:GridViewDataTextColumn><dx:GridViewDataTextColumn Caption="名称" FieldName="cInvName" ReadOnly="True" VisibleIndex="3" Width="150px">
                                </dx:GridViewDataTextColumn><dx:GridViewDataTextColumn Caption="规格" FieldName="cInvStd" ReadOnly="True" VisibleIndex="4" Width="100px">
                                </dx:GridViewDataTextColumn><dx:GridViewDataTextColumn Caption="基本单位" FieldName="cComUnitName" ReadOnly="True" VisibleIndex="7" Width="50px">
                                </dx:GridViewDataTextColumn><dx:GridViewDataTextColumn Caption="小包装单位" FieldName="cInvDefine2" ReadOnly="True" VisibleIndex="9" Width="55px">
                                </dx:GridViewDataTextColumn><dx:GridViewDataTextColumn Caption="大包装单位" FieldName="cInvDefine1" ReadOnly="True" VisibleIndex="11" Width="55px">
                                </dx:GridViewDataTextColumn><dx:GridViewDataTextColumn Caption="单位组" FieldName="UnitGroup" ReadOnly="True" VisibleIndex="5" Width="120px">
                                </dx:GridViewDataTextColumn><dx:GridViewDataTextColumn Caption="单价" FieldName="cComUnitPrice" ReadOnly="True" VisibleIndex="16" Width="50px">
                                </dx:GridViewDataTextColumn><dx:GridViewDataTextColumn Caption="金额" FieldName="cComUnitAmount" ReadOnly="True" VisibleIndex="17" Width="100px">
                                    
                                <EditFormSettings Visible="False" /></dx:GridViewDataTextColumn><dx:GridViewDataTextColumn Caption="基本单位汇总" FieldName="cInvDefineQTY" ReadOnly="True" VisibleIndex="12" Width="80px">
                                </dx:GridViewDataTextColumn><dx:GridViewDataTextColumn Caption="小包装换算率" FieldName="cInvDefine14" VisibleIndex="18" Width="0px">
                                </dx:GridViewDataTextColumn><dx:GridViewDataTextColumn Caption="大包装换算率" FieldName="cInvDefine13" VisibleIndex="19" Width="0px">
                                </dx:GridViewDataTextColumn><dx:GridViewDataTextColumn Caption="序号" FieldName="hh" ReadOnly="True" UnboundType="Integer" VisibleIndex="2" Width="30px">
                                </dx:GridViewDataTextColumn><dx:GridViewDataTextColumn Caption="包装结果" FieldName="pack" ReadOnly="True" VisibleIndex="15" Width="100px">
                                </dx:GridViewDataTextColumn><dx:GridViewDataSpinEditColumn Caption="基本单位数量" EditFormSettings-VisibleIndex="0" FieldName="cComUnitQTY" VisibleIndex="6" Width="70px">
                                    
                                    
                                    
                                <PropertiesSpinEdit DisplayFormatString="g" MaxValue="99999" AllowMouseWheel="False" AllowNull="False" NullText="0"></PropertiesSpinEdit><EditFormSettings VisibleIndex="0" /><CellStyle BackColor="#FF66FF">
                                    </CellStyle></dx:GridViewDataSpinEditColumn><dx:GridViewDataSpinEditColumn Caption="小包装单位数量" FieldName="cInvDefine2QTY" VisibleIndex="8" Width="80px">
                                    
                                    
                                <PropertiesSpinEdit DisplayFormatString="g" MaxValue="99999" NumberType="Integer" AllowMouseWheel="False" AllowNull="False" NullText="0"></PropertiesSpinEdit><CellStyle BackColor="#FF66FF">
                                    </CellStyle></dx:GridViewDataSpinEditColumn><dx:GridViewDataSpinEditColumn Caption="大包装单位数量" FieldName="cInvDefine1QTY" VisibleIndex="10" Width="80px">
                                    
                                    
                                <PropertiesSpinEdit DisplayFormatString="g" MaxValue="99999" NumberType="Integer" AllowMouseWheel="False" AllowNull="False" NullText="0"></PropertiesSpinEdit><CellStyle BackColor="#FF66FF">
                                    </CellStyle></dx:GridViewDataSpinEditColumn><dx:GridViewDataTextColumn Caption="可用量" FieldName="realqty" ReadOnly="True" VisibleIndex="13" Width="60px">
                                </dx:GridViewDataTextColumn><dx:GridViewDataTextColumn Caption="预订单号" FieldName="ccode" ReadOnly="True" VisibleIndex="20" Width="0px">
                                </dx:GridViewDataTextColumn><dx:GridViewDataTextColumn Caption="物料编码+预订单号" FieldName="itemid" ReadOnly="True" VisibleIndex="21" Width="0px">
                                </dx:GridViewDataTextColumn><dx:GridViewDataTextColumn Caption="库存量" FieldName="Stock" ReadOnly="True" VisibleIndex="14" Width="60px">
                                </dx:GridViewDataTextColumn><dx:GridViewDataTextColumn Caption="iaoids" FieldName="iaoids" ReadOnly="True" VisibleIndex="22" Width="0px">
                                </dx:GridViewDataTextColumn><dx:GridViewDataTextColumn Caption="执行单价" FieldName="ExercisePrice" ReadOnly="True" VisibleIndex="23" Width="0px">
                                </dx:GridViewDataTextColumn><dx:GridViewDataTextColumn Caption="执行金额" FieldName="xx" ReadOnly="True" UnboundType="Decimal" VisibleIndex="24" Width="0px">
                                </dx:GridViewDataTextColumn></Columns><SettingsBehavior AllowFocusedRow="True" AllowSort="False" /><SettingsPager Mode="ShowAllRecords" /><SettingsEditing EditFormColumnCount="99" Mode="Batch" /><Settings HorizontalScrollBarMode="Auto" ShowFooter="True" ShowTitlePanel="true" VerticalScrollableHeight="200" VerticalScrollBarMode="Auto" /><SettingsText Title="订单明细表" /><SettingsCommandButton>
                                <UpdateButton ButtonType="Link" Text="保存购物信息">
                                    <Styles>
                                        <Style Font-Size="Medium" ForeColor="Red" HorizontalAlign="Center" VerticalAlign="Middle" Width="1200px">
 
                                            <HoverStyle BackColor="#FFFF99">
                                            </HoverStyle>
                                            <Paddings PaddingRight="1180px" />
 
                                        </Style>
                                    </Styles>
                                </UpdateButton>
                                <CancelButton ButtonType="Link" Text="取消修改">
                                    <Styles>
                                        <Style Font-Size="Medium" ForeColor="Red" HorizontalAlign="Center" VerticalAlign="Middle" Width="1200px">
   
                                            <HoverStyle BackColor="#FFFF99">
                                            </HoverStyle>
                                            <Paddings PaddingRight="1210px" />
   
                                        </Style>
                                    </Styles>
                                </CancelButton>
                                <DeleteButton ButtonType="Button" Text="删除">
                                </DeleteButton>
                            </SettingsCommandButton><SettingsDataSecurity AllowInsert="False" /><Styles>
                                <Footer BackColor="#C2D491">
                                </Footer>
                            </Styles></dx:ASPxGridView>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div style="float: left; width: 300px;">
                <dx:ASPxButton ID="BtnSaveOrder" runat="server" Text="修改后提交订单员审核" OnClick="BtnSaveOrder_Click" Height="40px" Width="130px"
                    ClientInstanceName="BtnSaveOrder" EnableTheming="True" Font-Size="Large"
                    Theme="SoftOrange">
                </dx:ASPxButton>
            </div>
                        <div style="float: left; width: 200px;">
            <dx:ASPxButton ID="BtnCancel" runat="server" Text="返回" OnClick="BtnCancel_Click" Height="40px" Width="130px"
                ClientInstanceName="BtnCancel" EnableTheming="True" Theme="Office2010Black">
                <ClientSideEvents Click="function(s, e) {
e.processOnServer=confirm('确认放弃编辑吗?'); 	
}" />
            </dx:ASPxButton>
                            </div>
            <div style="float: left; width: 200px;">
                <dx:ASPxButton ID="BtnInvalidOrder" runat="server" Text="作废订单" OnClick="BtnInvalidOrder_Click" Height="40px" Width="130px"
                    ClientInstanceName="BtnInvalidOrder" EnableTheming="True" Theme="RedWine">
                    <ClientSideEvents Click="function(s, e) {
e.processOnServer=confirm('确认作废该订单?'); 	
}" />
                </dx:ASPxButton>
            </div>
            <div>
                <dx:ASPxPopupControl ID="ShippingMethod" runat="server" CloseAction="CloseButton" CloseOnEscape="True" Modal="True" Target="_blank"
                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="ShippingMethod"
                    HeaderText="选择送货方式" AllowDragging="True" PopupAnimationType="None" EnableViewState="true" ClientIDMode="Static" Height="203px" Width="447px">
                    <ClientSideEvents PopUp="function(s, e) { ComboShippingMethod.Focus(); }" />
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                            <dx:ASPxPanel ID="Panel1" runat="server" DefaultButton="btOK">

                                <PanelCollection>

                                    <dx:PanelContent ID="PanelContent1" runat="server">
                                        <table>
                                            <tr>
                                                <td class="auto-style8">
                                                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="送货方式:" AssociatedControlID="shfs">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td class="auto-style12" style="margin-bottom: 10px">
                                                    <div style="float: left">
                                                        <dx:ASPxComboBox ID="ComboShippingMethod" runat="server" Height="20px" Width="194px" AutoPostBack="true"
                                                            OnSelectedIndexChanged="ComboShippingMethod_SelectedIndexChanged">
                                                            <Items>
                                                                <dx:ListEditItem Text="配送" Value="配送" />
                                                                <dx:ListEditItem Text="自提" Value="自提" />
                                                            </Items>
                                                        </dx:ASPxComboBox>
                                                    </div>
                                                    <div style="float: left; margin-left: 20px; margin-bottom: 10px">
                                                        <dx:ASPxButton ID="ReAddr" runat="server" Text="刷新地址列表信息" AutoPostBack="true" Theme="Office2010Blue" OnClick="ComboShippingMethod_SelectedIndexChanged">
                                                        </dx:ASPxButton>
                                                    </div>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td class="auto-style8">
                                                    <dx:ASPxLabel ID="lblPass1" runat="server" Text="送货信息:" AssociatedControlID="dz">
                                                    </dx:ASPxLabel>
                                                </td>

                                                <td class="auto-style12">

                                                    <dx:ASPxGridView ID="GridViewShippingMethod" runat="server" AutoGenerateColumns="False" EnableTheming="True"
                                                        Theme="Office2010Blue" KeyFieldName="lngopUseraddressId" Width="352px"
                                                        OnCustomUnboundColumnData="GridViewShippingMethod_CustomUnboundColumnData">
                                                        <ClientSideEvents RowDblClick="function(s, e) {
ShippingMethod.Hide();
dbclick();	
}" />
                                                        <Columns>
                                                            <dx:GridViewDataTextColumn ShowInCustomizationForm="True" VisibleIndex="0" Caption="lngopUseraddressId" FieldName="lngopUseraddressId" ReadOnly="True" Visible="False">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="序号" FieldName="hh" UnboundType="Integer" ShowInCustomizationForm="True" VisibleIndex="1" Width="50px" ReadOnly="True">
                                                                <Settings AllowAutoFilter="False" ShowFilterRowMenu="False" ShowFilterRowMenuLikeItem="False" ShowInFilterControl="False" />
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn ShowInCustomizationForm="True" VisibleIndex="2" Caption="联系信息" FieldName="ShippingInformation">
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="cDefine9" FieldName="strConsigneeName" ReadOnly="True" ShowInCustomizationForm="True" Visible="False" VisibleIndex="3">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="cDefine12" FieldName="strConsigneeTel" ReadOnly="True" ShowInCustomizationForm="True" Visible="False" VisibleIndex="4">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="cDefine11" FieldName="strReceivingAddress" ReadOnly="True" ShowInCustomizationForm="True" Visible="False" VisibleIndex="5">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="cDefine10" FieldName="strCarplateNumber" ReadOnly="True" ShowInCustomizationForm="True" Visible="False" VisibleIndex="6">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="cDefine1" FieldName="strDriverName" ReadOnly="True" ShowInCustomizationForm="True" Visible="False" VisibleIndex="7">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="cDefine13" FieldName="strDriverTel" ReadOnly="True" ShowInCustomizationForm="True" Visible="False" VisibleIndex="8">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="cDefine2" FieldName="strIdCard" ReadOnly="True" ShowInCustomizationForm="True" Visible="False" VisibleIndex="9">
                                                            </dx:GridViewDataTextColumn>
                                                        </Columns>
                                                        <SettingsBehavior AllowFocusedRow="True" AllowSort="False" />
                                                        <SettingsPager NumericButtonCount="15" PageSize="15" Mode="ShowAllRecords">
                                                        </SettingsPager>
                                                        <Settings VerticalScrollableHeight="200" VerticalScrollBarMode="Visible" ShowFilterRow="True" ShowFilterRowMenu="True" />
                                                        <SettingsText SearchPanelEditorNullText="请输入查询内容" />
                                                        <SettingsCommandButton>
                                                            <SearchPanelApplyButton Text="查询">
                                                            </SearchPanelApplyButton>
                                                            <SearchPanelClearButton Text="清除">
                                                            </SearchPanelClearButton>
                                                        </SettingsCommandButton>
                                                        <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
                                                        <SettingsSearchPanel ColumnNames="联系信息" ShowApplyButton="True" ShowClearButton="True" Visible="True" />
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

                                                        <dx:ASPxButton ID="btOK" ClientInstanceName="btOK" runat="server" Text="确定" Width="80px" AutoPostBack="true"
                                                            Style="float: left; margin-right: 8px" OnClick="btOK_Click" Enabled="false">
                                                            <ClientSideEvents Click="function(s, e) {
ShippingMethod.Hide();
 }" />
                                                        </dx:ASPxButton>

                                                        <dx:ASPxButton ID="btCancel" runat="server" Text="取消" Width="80px" AutoPostBack="False" Style="float: left; margin-right: 8px">
                                                            <ClientSideEvents Click="function(s, e) { ShippingMethod.Hide(); }" />
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
                <dx:ASPxPopupControl ID="cdefine3" runat="server" CloseAction="CloseButton" CloseOnEscape="True" Modal="True" Target="_blank"
                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="cdefine3" OnLoad="cdefine3_Load"
                    HeaderText="选择车型" AllowDragging="True" PopupAnimationType="None" EnableViewState="true" ClientIDMode="Static" Height="203px" Width="447px">
                    <ClientSideEvents PopUp="function(s, e) { cdefine3Grid.Focus(); }" />
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                            <dx:ASPxPanel ID="ASPxPanel2" runat="server" DefaultButton="Btncdefine3Ok">

                                <PanelCollection>

                                    <dx:PanelContent ID="PanelContent3" runat="server">


                                        <table>

                                            <tr>

                                                <td class="auto-style12">

                                                    <dx:ASPxGridView ID="cdefine3Grid" runat="server" AutoGenerateColumns="False" EnableTheming="True" ClientInstanceName="cdefine3Grid"
                                                        Theme="Office2010Blue" KeyFieldName="cValue" Width="409px"
                                                        OnCustomUnboundColumnData="GridViewShippingMethod_CustomUnboundColumnData">
                                                        <ClientSideEvents RowDblClick="function(s, e) {
cdefine3.Hide();
cdefine3_dbclick();	
}" />
                                                        <Columns>
                                                            <dx:GridViewDataTextColumn ShowInCustomizationForm="True" VisibleIndex="1" Caption="序号" FieldName="hh" ReadOnly="True" UnboundType="Integer" Width="50px">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="车型名称" FieldName="cValue" ShowInCustomizationForm="True" VisibleIndex="2" ReadOnly="True" Width="300px">
                                                            </dx:GridViewDataTextColumn>
                                                        </Columns>
                                                        <SettingsBehavior AllowFocusedRow="True" AllowSort="False" />
                                                        <SettingsPager NumericButtonCount="15" PageSize="5" Mode="ShowAllRecords">
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

                                                        <dx:ASPxButton ID="Btncdefine3Ok" ClientInstanceName="Btncdefine3rOk" runat="server" Text="确定" Width="80px"
                                                            AutoPostBack="true" Style="float: left; margin-right: 8px" OnClick="Btncdefine3Ok_Click">
                                                            <ClientSideEvents Click="function(s, e) {
cdefine3.Hide();
 }" />
                                                        </dx:ASPxButton>

                                                        <dx:ASPxButton ID="Btn_cdefine3_Cancel" runat="server" Text="取消" Width="80px" AutoPostBack="False"
                                                            ClientInstanceName="Btn_cdefine3_Cancel" Style="float: left; margin-right: 8px">
                                                            <ClientSideEvents Click="function(s, e) { cdefine3.Hide(); }" />
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
                    PopupElementID="Inventory" PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter"
                    ShowFooter="True" Width="888px" Height="550px" HeaderText="选择商品" ClientInstanceName="Inventory" CloseOnEscape="True" Modal="True" Theme="Office2010Blue">
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl" runat="server">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
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
                                        <dx:ASPxTreeList ID="treeList" runat="server" AutoGenerateColumns="False" Caption="选择酬宾订单 ↓"
                                            ClientInstanceName="treeList" EnableTheming="True" KeyFieldName="strBillNo"
                                            OnCustomDataCallback="treeList_CustomDataCallback"
                                            ParentFieldName="strBillNo" Theme="Office2010Blue" Width="335px" Font-Size="10pt">
                                            <Columns>
                                                <dx:TreeListTextColumn Caption="酬宾订单" FieldName="strBillNo" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="0">
                                                </dx:TreeListTextColumn>
                                            </Columns>
                                            <Settings ScrollableHeight="350" VerticalScrollBarMode="Auto" />
                                            <SettingsBehavior AllowFocusedNode="True" AllowSort="False" ExpandCollapseAction="NodeDblClick" ProcessFocusedNodeChangedOnServer="True" ProcessSelectionChangedOnServer="True" AllowDragDrop="False" />
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
                                            <ClientSideEvents CustomDataCallback="function(s, e) {cscode();}" FocusedNodeChanged="function(s, e) {
            var key = treeList.GetFocusedNodeKey();
            treeList.PerformCustomDataCallback(key);
        }" />
                                        </dx:ASPxTreeList>
                                    </div>
                                    <div style="float: left">
                                        <asp:Button ID="btn" runat="server" OnClick="btn_Click" Text="Button" Style="display: none;" />
                                        <%--<input id='ctl' type='button' onclick='cscode();' style="display: none;" />--%>
                                        <dx:ASPxGridView ID="TreeDetail" ClientInstanceName="TreeDetail" runat="server" AutoGenerateColumns="False" Theme="Office2010Blue"
                                            ShowVerticalScrollBar="true" KeyFieldName="itemid" PreviewFieldName="itemid" Caption="选择物料明细 ↓" Width="515px" Font-Size="10pt">
                                            <Columns>
                                                <dx:GridViewDataTextColumn FieldName="cInvName" VisibleIndex="2" Width="150px" Caption="名称" ReadOnly="True">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="cInvStd" ReadOnly="True" VisibleIndex="3" Width="100px" Caption="规格">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewCommandColumn Caption="选择" ShowSelectCheckbox="True" VisibleIndex="0" Width="35px" SelectAllCheckboxMode="AllPages">
                                                </dx:GridViewCommandColumn>
                                                <dx:GridViewDataTextColumn Caption="编码" FieldName="cInvCode" VisibleIndex="1" Width="0px">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="可用量" VisibleIndex="4" Width="60px" FieldName="realqty">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="预订单号" VisibleIndex="5" Width="0px" FieldName="cCode">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="物料编码+预订单号" FieldName="itemid" ReadOnly="True" VisibleIndex="6" Width="0px">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="iaoids" FieldName="iaoids" ReadOnly="True" VisibleIndex="7" Width="0px">
                                                </dx:GridViewDataTextColumn>
                                            </Columns>
                                            <SettingsBehavior AllowSort="False" AllowFocusedRow="True" AllowDragDrop="False" />
                                            <SettingsPager Visible="False" Mode="EndlessPaging" NumericButtonCount="50" PageSize="50">
                                            </SettingsPager>
                                            <SettingsEditing EditFormColumnCount="50">
                                            </SettingsEditing>
                                            <Settings VerticalScrollBarMode="Visible" ShowFooter="False" ShowTitlePanel="True" VerticalScrollableHeight="320" />
                                            <SettingsLoadingPanel Delay="500" Mode="ShowAsPopup" />
                                            <SettingsDataSecurity AllowDelete="False" AllowInsert="False" AllowEdit="False" />
                                            <SettingsSearchPanel AllowTextInputTimer="False" />
                                        </dx:ASPxGridView>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="BtnInv_Reset" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                    <FooterTemplate>
                    </FooterTemplate>

                </dx:ASPxPopupControl>

            </div>




        </div>
    </form>
</body>
</html>

<script type="text/javascript">
    //调用btOK按钮事件
    function dbclick() {
        document.getElementById("<%=btOK.ClientID%>").click();
    }

    //调用btOK按钮事件
    function cdefine3_dbclick() {
        document.getElementById("<%=Btncdefine3Ok.ClientID%>").click();
    }
    var cscode = function () {
        document.getElementById("<%=btn.ClientID%>").click();
        }
</script>
