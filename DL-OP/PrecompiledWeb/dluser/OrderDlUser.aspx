<%@ page language="C#" autoeventwireup="true" inherits="dluser_OrderDlUser, dlopwebdll" enableviewstate="false" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>订单修改</title>

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
            height: 19px;
        }

        .dxeBase {
            font: 12px Tahoma, Geneva, sans-serif;
        }

        .auto-style8 {
            height: 18px;
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
          height: 19px;
          font-size: small;
      }
    </style>

</head>
<script type="text/javascript">
    //编辑修改数量
    function OrderGrid_BatchEditEndEditing(s, e) {
        //通过单位组分解换算率
        var UnitGroupColumn = s.GetColumnByField("UnitGroup");  //单位组换算率
        //alert(e.rowValues[s.GetColumnByField("UnitGroup").index].value);
        var ch = new Array();    //获取换算率
        ch = e.rowValues[UnitGroupColumn.index].value.split("=");
        for (i = 0; i < ch.length; i++) {
            ch[i] = ch[i].replace(/[^0-9]/ig, "");
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
        var BasicUnitAmount = (parseFloat(cellInfoQTYa.value) + parseFloat(cellInfoQTYb.value) * (parseFloat(ch[0]) / parseFloat(ch[1])) + parseFloat(cellInfoQTYc.value) * parseFloat(ch[0]));//基本单位数量汇总
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
        cell13.innerHTML = parseInt(BasicUnitAmount / parseFloat(ch[0])) + chUnit[2] + parseInt(parseInt(BasicUnitAmount % parseFloat(ch[0])) / (parseFloat(ch[0]) / parseFloat(ch[1]))) + chUnit[1] + parseInt(BasicUnitAmount * 100 % ((parseFloat(ch[0]) / parseFloat(ch[1])) * 100)) / 100 + chUnit[0];

    }

    function EndCallBack(s, e) {
        if (s.cpAlertMsg != "" && s.cpAlertMsg != null) {
            alert(s.cpAlertMsg);
            s.cpAlertMsg = null;
        }
    }

</script>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager2" runat="server"></asp:ScriptManager>
        <div>
            <table align="left" style="width: 100%; float: none;">
                <tr>
                    <td class="auto-style15">网单号</td>
                    <td class="auto-style2">
                        <dx:ASPxTextBox ID="TxtOrderBillNo" runat="server" Width="145px" Height="18px" ReadOnly="True" Theme="Office2010Blue"  CssClass="auto-style16">
                        </dx:ASPxTextBox>
                    </td>
                    <td class="auto-style3">制单人</td>
                    <td class="auto-style2">
                        <dx:ASPxTextBox ID="TxtBiller" runat="server" Width="145px" Height="18px" EnableTheming="True" ReadOnly="True" Theme="Office2010Blue" CssClass="auto-style16">
                        </dx:ASPxTextBox>
                    </td>
                    <td class="auto-style15">订单日期</td>
                    <td class="auto-style2">
                        <dx:ASPxTextBox ID="TxtBillDate" runat="server" Width="145px" Height="18px" ReadOnly="True" Theme="Office2010Blue" CssClass="auto-style16">
                        </dx:ASPxTextBox>
                    </td>
                    <td class="auto-style4"></td>
                </tr>
                <tr>
                    <td class="auto-style19">开票单位</td>
                    <td class="auto-style13">
                        <div style="float: left">
                            <dx:ASPxTextBox ID="TxtCustomer" runat="server" Width="125px" ReadOnly="True" Theme="Office2010Blue" Text="" Height="18px" EnableViewState="true" CssClass="auto-style16">
                            </dx:ASPxTextBox>
                        </div>
                        <div style="float: left; margin-left: 4px">
                            <dx:ASPxButton ID="ASPxButton2" runat="server" Text="选择" Width="40px" AutoPostBack="false" Theme="Office2010Blue" Height="16px">
                                <ClientSideEvents Click="function(s, e) { Customer.Show(); }" />
                            </dx:ASPxButton>
                        </div>


                    </td>
                    <td class="auto-style19">业务员</td>
                    <td class="auto-style13">
                        <dx:ASPxTextBox ID="TxtSalesman" ClientInstanceName="TxtSalesman" runat="server" Width="145px" Height="18px" EnableTheming="True" Theme="Office2010Blue" Text="1249" CssClass="auto-style16">
                            <ReadOnlyStyle BackColor="Gray" />
                        </dx:ASPxTextBox>


                    </td>
                    <td class="auto-style19">信用额</td>
                    <td class="auto-style13">
                        <dx:ASPxTextBox ID="TxtCusCredit" ClientInstanceName="TxtCusCredit" runat="server" Width="145px" Height="18px" EnableTheming="True" ReadOnly="True" Theme="Office2010Blue" CssClass="auto-style16">
                        </dx:ASPxTextBox>


                    </td>
                    <td class="auto-style13"><a style="float:left">酬宾类型</a><dx:ASPxTextBox ID="TxtCBLX" ClientInstanceName="TxtCBLX" runat="server" Width="135px" Height="18px" EnableTheming="True" ReadOnly="True" style="margin-bottom: 0px">
                            </dx:ASPxTextBox></td>
                </tr>
                <tr>
                    <td class="auto-style18">送货方式</td>
                    <td class="auto-style5" colspan="3">
                        <div style="float: left" class="auto-style16">
                            <dx:ASPxTextBox ID="TxtOrderShippingMethod" runat="server" Width="390px" Height="18px" Theme="Office2010Blue" ReadOnly="true" CssClass="auto-style16">
                            </dx:ASPxTextBox>
                        </div>
                        <div style="float: left; margin-left: 5px">
                            <dx:ASPxButton ID="ASPxButton1" runat="server" Text="选择" Width="50px" Height="18px" AutoPostBack="false" Theme="Office2010Blue">
                                <ClientSideEvents Click="function(s, e) { ShowWindow(); }" />
                            </dx:ASPxButton>
                        </div>


                    </td>
                    <td class="auto-style18">发运方式</td>
                    <td class="auto-style5">
                        <dx:ASPxTextBox ID="TxtcSCCode" ClientInstanceName="TxtcSCCode" runat="server" Width="145px" Height="18px" EnableTheming="True" ReadOnly="True" Theme="Office2010Blue" CssClass="auto-style16">
                        </dx:ASPxTextBox>


                    </td>
                    <td class="auto-style5"></td>
                </tr>
                <tr>
                    <td class="auto-style6">备注</td>
                    <td class="auto-style6">
                        <dx:ASPxTextBox ID="TxtOrderMark" runat="server" Width="185px" Theme="Office2010Blue" CssClass="auto-style16">
                            <ClientSideEvents LostFocus="function(s, e) {
	SetCookieText(s.GetText());
}" />
                        </dx:ASPxTextBox>
                    </td>
                    <td class="auto-style6">车型</td>
                    <td class="auto-style6">
                        <div style="float: left;">
                            <dx:ASPxTextBox ID="Txtcdefine3" runat="server" Width="115px" ReadOnly="True" Theme="Office2010Blue" Text="" Height="18px" EnableViewState="true" CssClass="auto-style16">
                            </dx:ASPxTextBox>
                        </div>
                        <div style="float: left; margin-left: 5px">
                            <dx:ASPxButton ID="ASPxButton3" runat="server" Text="选择" Width="40px" AutoPostBack="false" Theme="Office2010Blue" Height="16px">
                                <ClientSideEvents Click="function(s, e) { cdefine3.Show(); }" />
                            </dx:ASPxButton>
                        </div>
                    </td>
                    <td class="auto-style6">装车方式</td>
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
                    <td class="auto-style20">下单时间</td>
                    <td class="auto-style20">
                        <dx:ASPxTextBox ID="TxtBillTime" runat="server" Width="185px" Height="16px" ReadOnly="True" Theme="Office2010Blue" CssClass="auto-style16">
                        </dx:ASPxTextBox>
                    </td>
                    <td class="auto-style20">提货时间</td>
                    <td class="auto-style20">
                        <dx:ASPxDateEdit ID="DeliveryDate" runat="server" AllowMouseWheel="False" AllowNull="False" AllowUserInput="False"
                            DateOnError="Today" EnableTheming="True" Theme="Office2010Blue" Height="18px" EditFormatString="yyyy-MM-dd HH" EditFormat="Custom"                            
                             >
                            <%--DisplayFormatString="G" UseMaskBehavior="True"--%> 
                            <CalendarProperties DayNameFormat="Short" FirstDayOfWeek="Monday">
                            </CalendarProperties>
                            <TimeSectionProperties Visible="True" ShowMinuteHand="False">
                                <TimeEditProperties AllowNull="False" DisplayFormatString="HH" EditFormatString="HH">
                                </TimeEditProperties>
                            </TimeSectionProperties>
                            <ClientSideEvents ValueChanged="function(s, e) {
	SetCookieTextDeliveryDate(s.GetText());
}" />
                        </dx:ASPxDateEdit>
                    </td>
                    <td class="auto-style20">销售类型</td>
                    <td class="auto-style7">
                        <dx:ASPxTextBox ID="TxtcSTCode" runat="server" ReadOnly="true" Width="145px" Height="18px" Theme="Office2010Blue" CssClass="auto-style16">
                        </dx:ASPxTextBox>
                    </td>
                    <td class="auto-style20"></td>
                </tr>
            </table>
        </div>
        <div>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server"><ContentTemplate>
            <dx:ASPxGridView ID="OrderGrid" ClientInstanceName="OrderGrid" runat="server" AutoGenerateColumns="False" EnableTheming="True" KeyFieldName="cInvCode"
                OnInit="OrderGrid_Init"
                ClientIDMode="Static" ShowVerticalScrollBar="true" Theme="Office2010Blue" OnRowValidating="OrderGrid_RowValidating"  OnRowDeleting="OrderGrid_RowDeleting"
                OnRowUpdating="OrderGrid_RowUpdating" OnCustomUnboundColumnData="GridViewShippingMethod_CustomUnboundColumnData" Width="1230px">
                <Settings ShowTitlePanel="true" ShowFooter="True" VerticalScrollableHeight="240" VerticalScrollBarMode="Auto" />
                <SettingsText CommandCancel="取消" CommandUpdate="确定" />
                <ClientSideEvents BatchEditEndEditing="OrderGrid_BatchEditEndEditing" EndCallback="function(s, e) {EndCallBack(s,e);}" />
                <TotalSummary>
                    <dx:ASPxSummaryItem FieldName="cInvName" SummaryType="Count" />
                    <dx:ASPxSummaryItem FieldName="cComUnitAmount" SummaryType="Sum" />
                </TotalSummary>
                <Columns>
                    <dx:GridViewCommandColumn ButtonType="Button" Caption="删除" ShowDeleteButton="True" VisibleIndex="0" Width="70px">
                                                <HeaderTemplate>
                            <dx:ASPxButton ID="Choose" runat="server" Text="选择商品"  AutoPostBack="False" EnableTheming="True" Theme="Office2010Blue" ForeColor="Red" Width="55px">
                                <ClientSideEvents Click="function(s, e) {
Inventory.Show()	
}" /></dx:ASPxButton>
                        </HeaderTemplate>
                    </dx:GridViewCommandColumn>
                    <dx:GridViewDataTextColumn Caption="编码" FieldName="cInvCode" Visible="False" VisibleIndex="2">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="名称" FieldName="cInvName" VisibleIndex="4" ReadOnly="True" Width="150px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="规格" FieldName="cInvStd" VisibleIndex="5" ReadOnly="True" Width="100px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="基本单位" FieldName="cComUnitName" VisibleIndex="8" ReadOnly="True" Width="50px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="小包装单位" FieldName="cInvDefine2" VisibleIndex="10" ReadOnly="True" Width="55px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="大包装单位" FieldName="cInvDefine1" VisibleIndex="12" ReadOnly="True" Width="55px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="单位组" FieldName="UnitGroup" VisibleIndex="6" ReadOnly="True" Width="120px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="单价" VisibleIndex="16" FieldName="cComUnitPrice" ReadOnly="True" Width="50px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="金额" VisibleIndex="17" FieldName="cComUnitAmount" ReadOnly="True" Width="100px">
                        <EditFormSettings Visible="False" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataHyperLinkColumn Caption="删除" FieldName="cInvCode" VisibleIndex="1" ReadOnly="True" Width="38px" Visible="False">
                        <PropertiesHyperLinkEdit NavigateUrlFormatString="OrderDlUser.aspx?code={0}" Text="删除">
                        </PropertiesHyperLinkEdit>
                    </dx:GridViewDataHyperLinkColumn>
                    <dx:GridViewDataTextColumn Caption="基本单位汇总" FieldName="cInvDefineQTY" VisibleIndex="13" ReadOnly="True" Width="80px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="小包装换算率" FieldName="cInvDefine14" VisibleIndex="18" Width="0px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="大包装换算率" FieldName="cInvDefine13" VisibleIndex="19" Width="0px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="序号" FieldName="hh" ReadOnly="True" UnboundType="Integer" VisibleIndex="3" Width="30px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="包装结果" FieldName="pack" ReadOnly="True" VisibleIndex="15" Width="100px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataSpinEditColumn Caption="基本单位数量" FieldName="cComUnitQTY" VisibleIndex="7" EditFormSettings-VisibleIndex="0" Width="70px">
                        <PropertiesSpinEdit DisplayFormatString="g" MaxValue="99999">
                        </PropertiesSpinEdit>
                        <EditFormSettings VisibleIndex="0"></EditFormSettings>
                        <CellStyle BackColor="#FF66FF">
                        </CellStyle>
                    </dx:GridViewDataSpinEditColumn>
                    <dx:GridViewDataSpinEditColumn Caption="小包装单位数量" FieldName="cInvDefine2QTY" VisibleIndex="9" Width="80px">
                        <PropertiesSpinEdit DisplayFormatString="g" MaxValue="99999" NumberType="Integer">
                        </PropertiesSpinEdit>
                        <CellStyle BackColor="#FF66FF">
                        </CellStyle>
                    </dx:GridViewDataSpinEditColumn>
                    <dx:GridViewDataSpinEditColumn Caption="大包装单位数量" FieldName="cInvDefine1QTY" VisibleIndex="11" Width="80px">
                        <PropertiesSpinEdit DisplayFormatString="g" MaxValue="99999" NumberType="Integer">
                        </PropertiesSpinEdit>
                        <CellStyle BackColor="#FF66FF">
                        </CellStyle>
                    </dx:GridViewDataSpinEditColumn>
                    <dx:GridViewDataTextColumn Caption="可用库存量" FieldName="Stock" ReadOnly="True" VisibleIndex="14" Width="60px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="执行单价" FieldName="ExercisePrice" ReadOnly="True" VisibleIndex="20" Width="0px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="执行金额" FieldName="xx" ReadOnly="True" UnboundType="Decimal" VisibleIndex="21" Width="0px">
                    </dx:GridViewDataTextColumn>
                </Columns>
                <SettingsBehavior AllowSort="False" AllowFocusedRow="True" />
                <SettingsPager Mode="ShowAllRecords" />
                <SettingsEditing EditFormColumnCount="99" Mode="Batch" />
                <SettingsText Title="订单明细表" />

                <SettingsCommandButton>
                    <CancelButton Text="取消">
                        <Styles>
                            <Style Font-Size="Medium" ForeColor="Red">
                                <Paddings PaddingRight="1080px" />
                            </Style>
                        </Styles>
                    </CancelButton>
                     <UpdateButton Text="保存">
                        <Styles>
                            <Style Font-Size="Medium" ForeColor="Red">
                                 <Paddings PaddingRight="20px" />
                            </Style>
                        </Styles>
                    </UpdateButton>
                    <DeleteButton Text="删除">
                    </DeleteButton>
                </SettingsCommandButton>

                <Styles>
                    <Footer BackColor="#C2D491">
                    </Footer>
                </Styles>
            </dx:ASPxGridView>
</ContentTemplate></asp:UpdatePanel>
            <table class="dxflInternalEditorTable">
                <tr>
                    <td class="auto-style14">
                        <div style="float:left;width:200px;">
                        <dx:ASPxButton ID="BtnSaveOrder" runat="server" Text="保存修改" OnClick="BtnSaveOrder_Click" Height="40px" Width="130px"
                            BackColor="#0066FF" ClientInstanceName="BtnSaveOrder" EnableTheming="True" Font-Bold="False" Font-Overline="False" Font-Size="Large"
                            Font-Strikeout="False" ForeColor="Red">
                            <FocusRectBorder BorderColor="#FF9900" />
                            <Border BorderColor="#0066FF" />
                        </dx:ASPxButton>
                             </div>
                        <div style="float:left;width:200px;">
                        <dx:ASPxHyperLink ID="HyBack" runat="server" Text="返回" ClientInstanceName="HyBack" EnableTheming="True" Font-Names="微软雅黑" 
                            Font-Size="13pt" ForeColor="#3333CC" Height="33px" Target="_self" Theme="Office2010Blue" Width="90px" BackColor="#FF6600" BorderLeft-BorderWidth="10" >
                            <ClientSideEvents Click="function(s, e) {
 if (confirm('确定要返回吗？')){
         window.parent.location ='Gtasks.aspx';
         }	
}" />
                            <BorderLeft BorderColor="#FF6600" BorderStyle="Solid" BorderWidth="50px" />
                            <BorderTop BorderColor="#FF6600" BorderStyle="Solid" BorderWidth="10px" />
                        </dx:ASPxHyperLink>
                   </div>        
                        <dx:ASPxButton ID="BtnInvalidOrder" runat="server" Text="作废订单" OnClick="BtnInvalidOrder_Click" Height="40px" Width="130px"
                            ClientInstanceName="BtnInvalidOrder" EnableTheming="True" Theme="RedWine"  >
                            <ClientSideEvents Click="function(s, e) {
e.processOnServer=confirm('确认作废该订单?'); 	
}" />
                        </dx:ASPxButton>
                    </td>
                    <td class="auto-style14"></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
            </table>

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
                                            <td class="auto-style12">
                                                <dx:ASPxComboBox ID="ComboShippingMethod" runat="server" Height="20px" Width="194px" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ComboShippingMethod_SelectedIndexChanged" EnableTheming="True" Theme="Office2010Blue">
                                                    <Items>
                                                        <dx:ListEditItem Text="配送" Value="配送" />
                                                        <dx:ListEditItem Text="自提" Value="自提" />
                                                    </Items>
                                                </dx:ASPxComboBox>
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
                                                    OnFocusedRowChanged="GridViewShippingMethod_FocusedRowChanged"
                                                    OnCustomUnboundColumnData="GridViewShippingMethod_CustomUnboundColumnData">
                                                    <ClientSideEvents RowDblClick="function(s, e) {
ShippingMethod.Hide();
dbclick();	
}" />
                                                    <Columns>
                                                        <dx:GridViewDataTextColumn ShowInCustomizationForm="True" VisibleIndex="0" Caption="lngopUseraddressId" FieldName="lngopUseraddressId" ReadOnly="True" Visible="False">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="序号" FieldName="hh" UnboundType="Integer" ShowInCustomizationForm="True" VisibleIndex="1" Width="30px">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn ShowInCustomizationForm="True" VisibleIndex="2" Caption="联系信息" FieldName="ShippingInformation" Width="200px">
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

                                                    <dx:ASPxButton ID="btOK" ClientInstanceName="btOK" runat="server" Text="确定" Width="80px" AutoPostBack="true"
                                                        Style="float: left; margin-right: 8px" OnClick="btOK_Click">
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
                                                        <dx:GridViewDataTextColumn Caption="序号" FieldName="hh" UnboundType="Integer" ShowInCustomizationForm="True" VisibleIndex="1" ReadOnly="True" Width="30px">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn ShowInCustomizationForm="True" VisibleIndex="2" Caption="单位名称" FieldName="cCusName" ReadOnly="True" Width="200px">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="业务员" FieldName="cCusPPerson" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="3" Width="0px">
                                                        </dx:GridViewDataTextColumn>
                                                    </Columns>
                                                    <SettingsBehavior AllowFocusedRow="True" AllowSort="False" />
                                                    <SettingsPager  NumericButtonCount="15" PageSize="15"  Mode="ShowAllRecords">
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
<dx:ASPxPopupControl ID="cdefine3" runat="server" CloseAction="CloseButton" CloseOnEscape="True" Modal="True" Target="_blank"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="cdefine3"
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
                                                        <dx:GridViewDataTextColumn ShowInCustomizationForm="True" VisibleIndex="1" Caption="序号" FieldName="hh" ReadOnly="True" UnboundType="Integer" Width="20px">
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
                                                        AutoPostBack="true" Style="float: left; margin-right: 8px; height: 23px;" OnClick="Btncdefine3Ok_Click">
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
        
        <div><dx:ASPxPopupControl ID="Inventory" runat="server" CloseAction="CloseButton" LoadContentViaCallback="OnFirstShow"
            PopupElementID="Inventory" PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter"
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
                            <Settings ScrollableHeight="400" VerticalScrollBarMode="Auto" />
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
                                        <dx:GridViewDataTextColumn FieldName="cInvName" VisibleIndex="2" Width="150px" Caption="名称" ReadOnly="True">
                                            <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="cInvStd" ReadOnly="True" VisibleIndex="3" Width="100px" Caption="规格">
                                            <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="cComUnitName" VisibleIndex="4" Caption="单位" Width="30px" ReadOnly="True">
                                            <Settings AllowAutoFilter="False" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewCommandColumn Caption="选择" ShowSelectCheckbox="True" VisibleIndex="0" Width="35px" SelectAllCheckboxMode="AllPages" ShowClearFilterButton="True">
                                        </dx:GridViewCommandColumn>
                                        <dx:GridViewDataTextColumn Caption="编码" FieldName="cInvCode" VisibleIndex="1" Width="0px">
                                            <Settings AllowAutoFilter="False" />
                                        </dx:gridviewdatatextcolumn>
                                    </Columns>
                                    <SettingsBehavior AllowSort="False" AllowFocusedRow="True" AllowDragDrop="False" FilterRowMode="OnClick" />
                                    <SettingsPager Visible="False" Mode="EndlessPaging" NumericButtonCount="999">
                                    </SettingsPager>
                                    <SettingsEditing EditFormColumnCount="99">
                                    </SettingsEditing>
                                    <Settings VerticalScrollBarMode="Visible" ShowFooter="False" ShowTitlePanel="True" VerticalScrollableHeight="390" ShowFilterRow="True" ShowFilterRowMenu="True" />
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
        </dx:ASPxPopupControl></div>

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
            c_start = document.cookie.indexOf("ModifyTxtOrderMark=")
            if (c_start != -1) {
                //写入cookie
                document.cookie = "ModifyTxtOrderMark=" + encodeURIComponent(text) + ";path=/";
            } else {
                //创建cookie,并写入值
                var expdate = new Date();   //初始化时间
                expdate.setTime(expdate.getTime() + 30 * 60 * 1000);   //时间30分钟
                document.cookie = "ModifyTxtOrderMark=" + encodeURIComponent(text) + ";expires=" + expdate.toGMTString() + ";path=/";
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
            c_start = document.cookie.indexOf("ModifyTxtLoadingWays=")
            if (c_start != -1) {
                //写入cookie
                document.cookie = "ModifyTxtLoadingWays=" + encodeURIComponent(text) + ";path=/";
            } else {
                //创建cookie,并写入值
                var expdate = new Date();   //初始化时间
                expdate.setTime(expdate.getTime() + 30 * 60 * 1000);   //时间30分钟
                document.cookie = "ModifyTxtLoadingWays=" + encodeURIComponent(text) + ";expires=" + expdate.toGMTString() + ";path=/";
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
            c_start = document.cookie.indexOf("ModifyDeliveryDate=")
            if (c_start != -1) {
                //写入cookie
                document.cookie = "ModifyDeliveryDate=" + encodeURIComponent(text) + ";path=/";
            } else {
                //创建cookie,并写入值
                var expdate = new Date();   //初始化时间
                expdate.setTime(expdate.getTime() + 30 * 60 * 1000);   //时间30分钟
                document.cookie = "ModifyDeliveryDate=" + encodeURIComponent(text) + ";expires=" + expdate.toGMTString() + ";path=/";
                //即document.cookie= name+"="+value+";path=/";   时间可以不要，但路径(path)必须要填写，因为JS的默认路径是当前页，如果不填，此cookie只在当前页面生效！~
            }
            //}
        }
    }

    //调用btOK按钮事件
    function dbclick() {
        document.getElementById("<%=btOK.ClientID%>").click();
    }
    //调用btOK按钮事件
    function Customer_dbclick() {
        document.getElementById("<%=BtnCustomerOk.ClientID%>").click();
    }
    //调用btOK按钮事件
    function cdefine3_dbclick() {
        document.getElementById("<%=Btncdefine3Ok.ClientID%>").click();
    }

    var cscode = function () {
        document.getElementById("<%=btn.ClientID%>").click();
        }

</script>