<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SampleOrder.aspx.cs" Inherits="SampleOrder" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>新增订单</title>

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
            width: 222px;
            height: 20px;
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

        .auto-style18 {
            height: 25px;
            font-size: small;
        }

        .auto-style19 {
            height: 10px;
            font-size: small;
        }

        .auto-style20 {
            width: 222px;
            height: 23px;
        }
        .auto-style21 {
            height: 10px;
            width: 222px;
        }
        .auto-style22 {
            height: 25px;
            width: 222px;
        }
        .auto-style23 {
            height: 20px;
            font-size: small;
            width: 222px;
        }

        </style>

</head>
<script type="text/javascript">
    Math.formatFloat = function (f, digit) {
        var m = Math.pow(10, digit);
        return parseInt(f * m, 10) / m;
    }
    //编辑修改数量,并且禁止编辑没有单位的数量
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
        var cellInfoQTYa = e.rowValues[cComUnitQTYColumn.index];  //数量1,获取基本计量单位数量
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
        //var BasicUnitAmount = (parseFloat(cellInfoQTYa.value) + parseFloat(parseFloat(cellInfoQTYb.value * 10 * 10) * parseFloat((parseFloat(ch[0]) / parseFloat(ch[1])) * 10 * 10)) / 10000 + parseFloat(parseFloat(cellInfoQTYc.value * 10 * 10) * (parseFloat(ch[0]) * 10 * 10) / 10000));//基本单位数量汇总
        var BasicUnitAmount = 0;
        var baozhuangjieguo = "";
        var qtybb = -1;
        var qtycc = -1;
        if (ch.length == 1) {
            BasicUnitAmount = parseFloat(cellInfoQTYa.value);//基本单位数量汇总
            baozhuangjieguo = BasicUnitAmount + chUnit[0];//包装结果
            qtybb = 0;
            qtycc = 0;
        }
        if (ch.length == 2) {
            //BasicUnitAmount = parseFloat(cellInfoQTYa.value);//基本单位数量汇总
            BasicUnitAmount = (parseFloat(cellInfoQTYa.value) + parseFloat(parseFloat(cellInfoQTYb.value * 10 * 10) * parseFloat((parseFloat(ch[0]) / parseFloat(ch[1])) * 10 * 10)) / 10000);//基本单位数量汇总
            baozhuangjieguo= parseInt((BasicUnitAmount * 10 * 10) / (parseFloat(ch[0]) * 10 * 10)) + chUnit[1] + parseFloat(parseFloat((parseInt(BasicUnitAmount * 10 * 10) % (parseInt(parseFloat(ch[0]) * 10 * 10 / parseFloat(ch[1]))))) / (100)) + chUnit[1];
            qtybb = -1;
            qtycc = 0;
        }
        if (ch.length >= 3) {
            //BasicUnitAmount = parseFloat(cellInfoQTYa.value);//基本单位数量汇总
            BasicUnitAmount = (parseFloat(cellInfoQTYa.value) + parseFloat(parseFloat(cellInfoQTYb.value * 10 * 10) * parseFloat((parseFloat(ch[0]) / parseFloat(ch[1])) * 10 * 10)) / 10000 + parseFloat(parseFloat(cellInfoQTYc.value * 10 * 10) * (parseFloat(ch[0]) * 10 * 10) / 10000));//基本单位数量汇总
            baozhuangjieguo=parseInt((BasicUnitAmount * 10 * 10) / (parseFloat(ch[0]) * 10 * 10)) + chUnit[2] + parseInt((parseInt((BasicUnitAmount * 10 * 10)) % parseInt(parseFloat(ch[0]) * 10 * 10)) / ((parseFloat(ch[0]) / parseFloat(ch[1])) * 10 * 10)) + chUnit[1] + parseFloat(parseFloat((parseInt(BasicUnitAmount * 10 * 10) % (parseInt(parseFloat(ch[0]) * 10 * 10 / parseFloat(ch[1]))))) / (100)) + chUnit[0];
            qtybb = -1;
            qtycc = -1;
        }
        var QTYPrice = BasicUnitAmount * parseFloat(cellInfoPrice.value);
        var rowsindex = "OrderGrid_DXDataRow" + s.GetFocusedRowIndex();
        var rows = document.getElementById(rowsindex);  //根据id找到这行
        var cell = rows.cells[14];//获取某行下面的某个td元素,金额
        //var a = cell.innerHTML;
        //var index1 = a.indexOf(">");
        //var index2 = a.indexOf("</div");
        //var b = a.substring(index1 + 1, index2);
        //var c = a.replace(b, QTYPrice);
        //cell.innerHTML = c;
        cell.innerHTML = QTYPrice.toFixed(2);   //金额赋值
        var cell11 = rows.cells[11];//获取某行下面的某个td元素,基本单位汇总
        cell11.innerHTML = BasicUnitAmount + chUnit[0]; //数量赋值
        var cell12 = rows.cells[12];//获取某行下面的某个td元素,包装结果
        cell12.innerHTML = baozhuangjieguo;
        var cell7 = rows.cells[7];//获取某行下面的某个td元素,小包装数量
        if (qtybb != -1) {
            cell7.innerHTML =0;
        }
        var cell9 = rows.cells[9];//获取某行下面的某个td元素,大包装数量
        if (qtycc != -1) {
            cell9.innerHTML = 0;
        }
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
                          <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>     
                <div>                        
                    <dx:ASPxMenu ID="ASPxMenu2" ClientInstanceName="ASPxMenu2" runat="server" EnableTheming="True"  Theme="Office2010Blue" OnItemClick="ASPxMenu2_ItemClick">
                            <Items>
                                <dx:MenuItem Text="新增普通订单"  >
                                </dx:MenuItem>
                                 <dx:MenuItem Text="新增样品资料订单"  >
                                                                         <ItemStyle BackColor="#FF9966" />
                                </dx:MenuItem>
                                <dx:MenuItem  Text="参照酬宾订单">
                                </dx:MenuItem>
                                <dx:MenuItem  Text="参照特殊订单">
                                </dx:MenuItem>
                            </Items>
                        </dx:ASPxMenu>
                </div>
        <div>
            <table align="left" style="width: 100%; float: none;">
                <tr>
                    <td class="auto-style15">

                        网单号</td>

                    <td class="auto-style2">
                        <dx:ASPxTextBox ID="TxtOrderBillNo" runat="server" Width="185px" Height="18px" ReadOnly="True" Theme="Office2010Blue" Text="保存后自动生成" CssClass="auto-style16" BackColor="#3366FF">
                            <ReadOnlyStyle BackColor="White">
                            </ReadOnlyStyle>
                        </dx:ASPxTextBox>
                    </td>
                    <td class="auto-style3">制单人</td>
                    <td class="auto-style2">
                        <dx:ASPxTextBox ID="TxtBiller" runat="server" Width="185px" Height="18px" EnableTheming="True" ReadOnly="True" Theme="Office2010Blue" CssClass="auto-style16" BackColor="#3366FF">
                            <ReadOnlyStyle BackColor="White">
                            </ReadOnlyStyle>
                        </dx:ASPxTextBox>
                    </td>
                    <td class="auto-style15">订单日期</td>
                    <td class="auto-style20">
                        <dx:ASPxTextBox ID="TxtBillDate" runat="server" Width="175px" Height="18px" ReadOnly="True" Theme="Office2010Blue" CssClass="auto-style16" BackColor="#3366FF">
                            <ReadOnlyStyle BackColor="White">
                            </ReadOnlyStyle>
                        </dx:ASPxTextBox>
                    </td>
                    <td class="auto-style4">
                        <dx:ASPxTextBox ID="TxtCusCredit" ClientInstanceName="TxtCusCredit" runat="server" Width="175px" Height="18px" EnableTheming="True" ReadOnly="True" Theme="Office2010Blue" CssClass="auto-style16" BackColor="#3366FF" Visible="False">
                            <ReadOnlyStyle BackColor="White">
                            </ReadOnlyStyle>
                        </dx:ASPxTextBox>


                    </td>
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


                    </td>
                    <td class="auto-style19">业务员</td>
                    <td class="auto-style13">
                        <dx:ASPxTextBox ID="TxtSalesman"  ClientInstanceName="TxtSalesman" runat="server" Width="185px" Height="18px" EnableTheming="True" Theme="Office2010Blue"  CssClass="auto-style16" ReadOnly="True">
                            <ReadOnlyStyle BackColor="White" />
                        </dx:ASPxTextBox>


                    </td>
                    <td class="auto-style19">  
                            关联主订单
</td>
                    <td class="auto-style21">
                         <div style="float: left; margin-left: 5px">
                            <dx:ASPxTextBox ID="TxtRelateU8NO" runat="server" Width="150px" Height="18px" EnableTheming="True" ReadOnly="True" Theme="Office2010Blue" CssClass="auto-style16" BackColor="#3366FF">
                                <ReadOnlyStyle BackColor="White">
                                </ReadOnlyStyle>
                            </dx:ASPxTextBox>
</div><div style="float: left; margin-left: 5px">
                            <dx:ASPxButton ID="BtnTxtRelateU8NOChose" runat="server" Text="选择" Width="40px" AutoPostBack="false" Theme="Office2010Blue" Height="16px">
                                <ClientSideEvents Click="function(s, e) { TxtRelateU8NOPOP.Show(); }" />
                            </dx:ASPxButton>
                        </div></td>
                    <td class="auto-style13">【请准确关联主订单号，我们将根据该订单号打包资料】</td>
                </tr>
                <tr>
                    <td class="auto-style18">送货方式</td>
                    <td class="auto-style5" colspan="3">
                        <div style="float: left" class="auto-style16">
                            <dx:ASPxTextBox ID="TxtOrderShippingMethod" runat="server" Width="390px" Height="18px" Theme="Office2010Blue" ReadOnly="true" CssClass="auto-style16">
                            </dx:ASPxTextBox>
                        </div>


                    </td>
                    <td class="auto-style18">发运方式</td>
                    <td class="auto-style22">
                        <dx:ASPxTextBox ID="TxtcSCCode" ClientInstanceName="TxtcSCCode" runat="server" Width="175px" Height="18px" EnableTheming="True" ReadOnly="True" Theme="Office2010Blue" CssClass="auto-style16" BackColor="#3366FF">
                            <ReadOnlyStyle BackColor="White">
                            </ReadOnlyStyle>
                        </dx:ASPxTextBox>


                    </td>
                    <td class="auto-style5"></td>
                </tr>
                <tr>
                    <td class="auto-style6">备注</td>
                    <td class="auto-style6">
                        <dx:ASPxTextBox ID="TxtOrderMark" runat="server" Width="185px" Theme="Office2010Blue" CssClass="auto-style16" MaxLength="300">
                            <ClientSideEvents LostFocus="function(s, e) {
	SetCookieText(s.GetText());
}" />
                        </dx:ASPxTextBox>
                    </td>
                    <td class="auto-style6">车型</td>
                    <td class="auto-style6">
                        <div style="float: left;">
                            <dx:ASPxTextBox ID="Txtcdefine3" runat="server" Width="122px" ReadOnly="True" Theme="Office2010Blue" Text="" Height="18px" EnableViewState="true" CssClass="auto-style16" BackColor="#3366FF">
                                <ReadOnlyStyle BackColor="White">
                                </ReadOnlyStyle>
                            </dx:ASPxTextBox>
                        </div>
                    </td>
                    <td class="auto-style6">装车方式</td>
                    <td class="auto-style23">
                        <dx:ASPxTextBox ID="TxtLoadingWays" runat="server" Width="175px" Height="18px" Theme="Office2010Blue" CssClass="auto-style16">
                            <ClientSideEvents LostFocus="function(s, e) {
	SetCookieTextTxtLoadingWays(s.GetText());
}" />
                        </dx:ASPxTextBox>
                    </td>
                    <td class="auto-style6">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style6">下单时间</td>
                    <td class="auto-style6">
                        <dx:ASPxTextBox ID="TxtBillTime" runat="server" Width="185px" Height="18px" ReadOnly="True" Theme="Office2010Blue" Text="保存后自动生成" CssClass="auto-style16" BackColor="#3366FF">
                            <ReadOnlyStyle BackColor="White">
                            </ReadOnlyStyle>
                        </dx:ASPxTextBox>
                    </td>
                    <td class="auto-style6">&nbsp;</td>
                    <td class="auto-style6">
                        <div style="display:none">
                        <dx:ASPxDateEdit ID="DeliveryDate" runat="server" AllowMouseWheel="False" AllowNull="False" AllowUserInput="False"
                            DateOnError="Today" EnableTheming="True" Theme="Office2010Blue" Height="18px" EditFormatString="yyyy-MM-dd HH" DisplayFormatString="G" UseMaskBehavior="True" EditFormat="Custom" Visible="False">
                            <CalendarProperties DayNameFormat="Short" FirstDayOfWeek="Monday">
                            </CalendarProperties>
                            <TimeSectionProperties Visible="True" ShowMinuteHand="False">
                                <TimeEditProperties AllowNull="False" DisplayFormatString="HH" EditFormatString="HH">
                                </TimeEditProperties>
                            </TimeSectionProperties>
                            <ClientSideEvents ValueChanged="function(s, e) {
	SetCookieTextDeliveryDate(s.GetText());
}" />
                        </dx:ASPxDateEdit></div>
                    </td>
                    <td class="auto-style6">销售类型</td>
                    <td class="auto-style7">
                        <div style="float: left; margin-left: 5px">
                            <dx:ASPxTextBox ID="TxtcSTCode" runat="server" Width="108px" Height="18px" EnableTheming="True" ReadOnly="True" Theme="Office2010Blue" CssClass="auto-style16" BackColor="#3366FF">
                                <ReadOnlyStyle BackColor="White">
                                </ReadOnlyStyle>
                            </dx:ASPxTextBox>

                        </div>
                        <div style="float: left; margin-left: 5px">
   <%--                         <dx:ASPxButton ID="ASPxButton4" runat="server" Text="选择" Width="40px" AutoPostBack="false" Theme="Office2010Blue" Height="16px">
                                <ClientSideEvents Click="function(s, e) { CombocSTCodePOP.Show(); }" />
                            </dx:ASPxButton>--%>
                        </div>
                    </td>
                    <td class="auto-style6">

                       
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server"><ContentTemplate>
            <dx:ASPxGridView ID="OrderGrid" ClientInstanceName="OrderGrid" runat="server" AutoGenerateColumns="False" EnableTheming="True" KeyFieldName="cInvCode" OnRowDeleting="OrderGrid_RowDeleting"
                ClientIDMode="Static" ShowVerticalScrollBar="true" Theme="Office2010Blue" OnRowValidating="OrderGrid_RowValidating" 
                 OnCellEditorInitialize="OrderGrid_CellEditorInitialize" OnHtmlDataCellPrepared="OrderGrid_HtmlDataCellPrepared"
                OnRowUpdating="OrderGrid_RowUpdating" OnCustomUnboundColumnData="GridViewShippingMethod_CustomUnboundColumnData" 
                Width="1320px" KeyboardSupport="True" Font-Size="10pt">
                <SettingsText Title="订单明细表" ContextMenuSummarySum="合计" />

                <ClientSideEvents BatchEditEndEditing="OrderGrid_BatchEditEndEditing" EndCallback="function(s, e) {EndCallBack(s,e);}" />
                <TotalSummary>
                    <dx:ASPxSummaryItem FieldName="cInvName" SummaryType="Count" ShowInGroupFooterColumn="名称" />
                    <dx:ASPxSummaryItem FieldName="cComUnitAmount" SummaryType="Sum" ShowInGroupFooterColumn="规格" />
                </TotalSummary>
                <Columns>
                    <dx:GridViewDataTextColumn Caption="编码" FieldName="cInvCode" Visible="False" VisibleIndex="1">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="名称" FieldName="cInvName" VisibleIndex="3" ReadOnly="True" Width="170px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="规格" FieldName="cInvStd" VisibleIndex="4" ReadOnly="True" Width="120px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="基本单位" FieldName="cComUnitName" VisibleIndex="7" ReadOnly="True" Width="50px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="小包装单位" FieldName="cInvDefine2" VisibleIndex="9" ReadOnly="True" Width="55px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="大包装单位" FieldName="cInvDefine1" VisibleIndex="11" ReadOnly="True" Width="55px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="单位组" FieldName="UnitGroup" VisibleIndex="5" ReadOnly="True" Width="140px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="单价" VisibleIndex="15" FieldName="cComUnitPrice" ReadOnly="True" Width="60px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="金额" VisibleIndex="16" FieldName="cComUnitAmount" ReadOnly="True" Width="120px">
                        <EditFormSettings Visible="False" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="基本单位汇总" FieldName="cInvDefineQTY" VisibleIndex="12" ReadOnly="True" Width="80px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="小包装换算率" FieldName="cInvDefine14" VisibleIndex="17" Width="0px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="大包装换算率" FieldName="cInvDefine13" VisibleIndex="18" Width="0px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="序号" FieldName="hh" ReadOnly="True" UnboundType="Integer" VisibleIndex="2" Width="40px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="包装结果" FieldName="pack" ReadOnly="True" VisibleIndex="14" Width="130px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataSpinEditColumn Caption="基本单位数量" FieldName="cComUnitQTY" VisibleIndex="6" EditFormSettings-VisibleIndex="0" Width="80px">
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
                    <dx:GridViewDataTextColumn Caption="可用库存量" FieldName="Stock" ReadOnly="True" VisibleIndex="13" Width="60px" Visible="False">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewCommandColumn Caption="删除" ShowDeleteButton="True" VisibleIndex="0" Width="60px">
                        <HeaderTemplate>
                            <dx:ASPxButton ID="Choose" runat="server" Text="选择商品"  AutoPostBack="False" EnableTheming="True" Theme="Office2010Blue" ForeColor="Red" Width="55px">
                                <ClientSideEvents Click="function(s, e) {
Inventory.Show()	
}" /></dx:ASPxButton>
                        </HeaderTemplate>
                    </dx:GridViewCommandColumn>
                </Columns>
                <SettingsBehavior AllowSort="False" AllowFocusedRow="True" ColumnResizeMode="Control" />
                <SettingsPager Mode="ShowAllRecords" />
                <SettingsEditing EditFormColumnCount="99" Mode="Batch" />
                <Settings ShowTitlePanel="true" ShowFooter="True" VerticalScrollableHeight="200" VerticalScrollBarMode="Auto" HorizontalScrollBarMode="Visible" />

                <SettingsCommandButton>
                    <UpdateButton Text="保存购物信息" ButtonType="Link">
                        <Styles>
                            <Style Font-Size="Medium" ForeColor="Red"  HorizontalAlign="Center" VerticalAlign="Middle">
                                <HoverStyle BackColor="#FFFF99">
                                </HoverStyle>
                                <Paddings PaddingRight="1111px" />
                            </Style>
                        </Styles>
                    </UpdateButton>
                    <CancelButton Text="取消修改" ButtonType="Link">
                        <Styles>
                            <Style Font-Size="Medium" ForeColor="Red" HorizontalAlign="Center" VerticalAlign="Middle">
                                <HoverStyle BackColor="#FFFF99">
                                </HoverStyle>
                                <Paddings PaddingRight="1111px" />
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
                                 ClientInstanceName="BtnSaveOrder" EnableTheming="True"  Font-Size="12pt"
                                 Font-Names="微软雅黑" Theme="SoftOrange"  >
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
        </div>

        <div>
        </div>

        <div>
            <dx:ASPxPopupControl ID="TxtRelateU8NOPOP" runat="server" CloseAction="CloseButton" CloseOnEscape="True" Modal="True" Target="_blank"
                PopupHorizontalAlign="LeftSides" PopupVerticalAlign="TopSides" ClientInstanceName="TxtRelateU8NOPOP"
                HeaderText="选择关联正式订单" AllowDragging="True" PopupAnimationType="None" EnableViewState="true" ClientIDMode="Static" Height="203px" Width="447px">
                <ClientSideEvents PopUp="function(s, e) { TxtRelateU8NOGridView.Focus(); }" />
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">
                        <dx:ASPxPanel ID="ASPxPanel4" runat="server" DefaultButton="BtnTxtRelateU8NOOk">
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent5" runat="server">
                                    <table>
                                        <tr>
                                            <td class="auto-style12">
                                                <dx:ASPxGridView ID="TxtRelateU8NOGridView" runat="server" AutoGenerateColumns="False" EnableTheming="True" ClientInstanceName="TxtRelateU8NOGridView"
                                                    Theme="Office2010Blue" KeyFieldName="strBillNo" Width="409px" Font-Size="9pt">
                                                    <ClientSideEvents RowDblClick="function(s, e) {
TxtRelateU8NOPOP.Hide();
TxtRelateU8NOPOP_dbclick();	
}" />
                                                    <Columns>
                                                        <dx:GridViewDataTextColumn Caption="网单号" FieldName="strBillNo" ShowInCustomizationForm="True" VisibleIndex="1" ReadOnly="True" Width="160px" SortIndex="0" SortOrder="Descending">
                                                            <Settings AllowSort="True" SortMode="DisplayText" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="正式订单" FieldName="cSOCode" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="2" Width="160px">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="下单时间" FieldName="datBillTime" ShowInCustomizationForm="True" VisibleIndex="3" Width="200px" ReadOnly="True">
                                                        </dx:GridViewDataTextColumn>
                                                    </Columns>
                                                    <SettingsBehavior AllowFocusedRow="True" AllowSort="False" />
                                                    <SettingsPager NumericButtonCount="15" PageSize="5" Mode="ShowAllRecords">
                                                    </SettingsPager>
                                                    <Settings VerticalScrollBarMode="Auto" />
                                                    <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
                                                </dx:ASPxGridView>
                                            </td>
                                        </tr>
                                        <tr>

                                            <td class="auto-style10">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div style="text-align: center">

                                                    <dx:ASPxButton ID="BtnTxtRelateU8NOOk" ClientInstanceName="BtnTxtRelateU8NOOk" runat="server" Text="确定" Width="80px"
                                                        AutoPostBack="true" Style="float: left; margin-right: 8px" OnClick="BtnTxtRelateU8NOOk_Click">
                                                        <ClientSideEvents Click="function(s, e) {
TxtRelateU8NOPOP.Hide();
 }" />
                                                    </dx:ASPxButton>

                                                    <dx:ASPxButton ID="Btn_TxtRelateU8NO_Cancel" runat="server" Text="取消" Width="80px" AutoPostBack="False"
                                                        ClientInstanceName="Btn_TxtRelateU8NO_Cancel" Style="float: left; margin-right: 8px">
                                                        <ClientSideEvents Click="function(s, e) { TxtRelateU8NOPOP.Hide(); }" />
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
            PopupElementID="Inventory" PopupVerticalAlign="TopSides" PopupHorizontalAlign="LeftSides"
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
                            ParentFieldName="ParentFieldName" Theme="Office2010Blue" Width="335px" Font-Size="10pt">
                            <Columns>
                                <dx:TreeListTextColumn Caption="产品分类" FieldName="NodeName" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="0">
                                </dx:TreeListTextColumn>
                            </Columns>
                            <Settings ScrollableHeight="400" VerticalScrollBarMode="Auto" />
                            <SettingsBehavior AllowFocusedNode="True" AllowSort="False" ProcessFocusedNodeChangedOnServer="True" ProcessSelectionChangedOnServer="True" AllowDragDrop="False" ExpandCollapseAction="NodeDblClick" FocusNodeOnExpandButtonClick="False" FocusNodeOnLoad="False" />
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
                                    ShowVerticalScrollBar="true" KeyFieldName="cInvCode" PreviewFieldName="cInvName" Caption="选择物料明细 ↓" Width="515px" Font-Size="10pt">
                                    <ClientSideEvents BeginCallback="function(s, e) {
}" />
                                    <Columns>
                                        <dx:GridViewDataTextColumn FieldName="cInvName" VisibleIndex="2" Width="150px" Caption="名称" ReadOnly="True">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="cInvStd" ReadOnly="True" VisibleIndex="3" Width="100px" Caption="规格">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="cComUnitName" VisibleIndex="4" Caption="单位" Width="30px" ReadOnly="True">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewCommandColumn Caption="选择" ShowSelectCheckbox="True" VisibleIndex="0" Width="35px" SelectAllCheckboxMode="AllPages">
                                        </dx:GridViewCommandColumn>
                                        <dx:GridViewDataTextColumn Caption="编码" FieldName="cInvCode" VisibleIndex="1" Width="0px">
                                        </dx:gridviewdatatextcolumn>
                                    </Columns>
                                    <SettingsBehavior AllowSort="False" AllowFocusedRow="True" AllowDragDrop="False" />
                                    <SettingsPager Visible="False" Mode="ShowAllRecords" NumericButtonCount="999">
                                    </SettingsPager>
                                    <SettingsEditing EditFormColumnCount="99">
                                    </SettingsEditing>
                                    <Settings VerticalScrollBarMode="Visible" ShowFooter="False" ShowTitlePanel="True" VerticalScrollableHeight="390" />
                                    <SettingsDataSecurity AllowDelete="False" AllowInsert="False" AllowEdit="False" />
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
    //显示送货方式窗口,
    function ShowWindow() {
        ShippingMethod.Show();
    }

    function SetCookieText(text) {
        //alert(text);
        //text是否有输入
        if (text != "") {
            //检查是否已存在cookie,否则创建cookie
            //if (document.cookie.length > 0) {
            c_start = document.cookie.indexOf("TxtOrderMark=")
            if (c_start != -1) {
                //写入cookie
                document.cookie = "TxtOrderMark=" + encodeURIComponent(text) + ";path=/";
            } else {
                //创建cookie,并写入值
                var expdate = new Date();   //初始化时间
                expdate.setTime(expdate.getTime() + 30 * 60 * 1000);   //时间30分钟
                document.cookie = "TxtOrderMark=" + encodeURIComponent(text) + ";expires=" + expdate.toGMTString() + ";path=/";
                //即document.cookie= name+"="+value+";path=/";   时间可以不要，但路径(path)必须要填写，因为JS的默认路径是当前页，如果不填，此cookie只在当前页面生效！~
            }
            //}
        }
    }

    function SetCookieTextTxtLoadingWays(text) {
        //alert(text);
        //text是否有输入
        if (text != "") {
            //检查是否已存在cookie,否则创建cookie
            //if (document.cookie.length > 0) {
            c_start = document.cookie.indexOf("TxtLoadingWays=")
            if (c_start != -1) {
                //写入cookie
                document.cookie = "TxtLoadingWays=" + encodeURIComponent(text) + ";path=/";
            } else {
                //创建cookie,并写入值
                var expdate = new Date();   //初始化时间
                expdate.setTime(expdate.getTime() + 30 * 60 * 1000);   //时间30分钟
                document.cookie = "TxtLoadingWays=" + encodeURIComponent(text) + ";expires=" + expdate.toGMTString() + ";path=/";
                //即document.cookie= name+"="+value+";path=/";   时间可以不要，但路径(path)必须要填写，因为JS的默认路径是当前页，如果不填，此cookie只在当前页面生效！~
            }
            //}
        }
    }

    function SetCookieTextDeliveryDate(text) {
        //alert(text);
        //text是否有输入
        if (text != "") {
            //检查是否已存在cookie,否则创建cookie
            //if (document.cookie.length > 0) {
            c_start = document.cookie.indexOf("DeliveryDate=")
            if (c_start != -1) {
                //写入cookie
                document.cookie = "DeliveryDate=" + encodeURIComponent(text) + ";path=/";
            } else {
                //创建cookie,并写入值
                var expdate = new Date();   //初始化时间
                expdate.setTime(expdate.getTime() + 30 * 60 * 1000);   //时间30分钟
                document.cookie = "DeliveryDate=" + encodeURIComponent(text) + ";expires=" + expdate.toGMTString() + ";path=/";
                //即document.cookie= name+"="+value+";path=/";   时间可以不要，但路径(path)必须要填写，因为JS的默认路径是当前页，如果不填，此cookie只在当前页面生效！~
            }
            //}
        }
    }

   
    //调用btOK按钮事件
    function TxtRelateU8NOPOP_dbclick() {
        document.getElementById("<%=BtnTxtRelateU8NOOk.ClientID%>").click();
    }
    var cscode = function () {
        document.getElementById("<%=btn.ClientID%>").click();
    }
</script>
