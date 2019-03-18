<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SampleOrderModify.aspx.cs" Inherits="dluser_SampleOrderModify" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        .auto-style1 {
            height: 25px;
        }
    
.dxpcLite .dxpc-header,
.dxdpLite .dxpc-header 
{
	color: #404040;
	background-color: #DCDCDC;
	border-bottom: 1px solid #C9C9C9;
	padding: 2px 2px 2px 12px;
}

.dxpcLite .dxpc-footer,
.dxdpLite .dxpc-footer
{
	color: #858585;
	background-color: #F3F3F3;
	border-top: 1px solid #E0E0E0;
}

        .auto-style2 {
            height: 23px;
        }

        .auto-style3 {
            height: 25px;
            width: 161px;
        }
        .auto-style4 {
            height: 23px;
            width: 161px;
        }
        .auto-style5 {
            width: 375px;
        }
        .auto-style6 {
            width: 215px;
        }
        .auto-style7 {
            width: 165px;
        }
        .auto-style8 {
            width: 161px;
        }
        .auto-style9 {
            height: 25px;
            width: 186px;
        }
        .auto-style10 {
            height: 23px;
            width: 186px;
        }
        .auto-style11 {
            width: 186px;
        }
        .auto-style14 {
            width: 214px;
        }
        .auto-style16 {
            height: 23px;
            width: 155px;
        }
        .auto-style18 {
            height: 25px;
            width: 106px;
        }
        .auto-style19 {
            height: 23px;
            width: 92px;
        }
        .auto-style20 {
            height: 25px;
            width: 144px;
        }
        .auto-style21 {
            width: 92px;
        }
        .auto-style22 {
            height: 25px;
            width: 92px;
        }
        .auto-style23 {
            height: 23px;
            width: 106px;
        }
        .auto-style24 {
            width: 106px;
        }

    </style>

    <script type="text/javascript">
        Math.formatFloat = function (f, digit) {
            var m = Math.pow(10, digit);
            return parseInt(f * m, 10) / m;
        }
        //编辑修改数量,并且禁止编辑没有单位的数量
        function OrderGrid_BatchEditEndEditing(s, e) {
            //通过单位组分解换算率
            var UnitGroupColumn = s.GetColumnByField("UnitGroup");  //单位组换算率
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
            //计算金额
            var cInvDefine2QTYColumn = s.GetColumnByField("cInvDefine2QTY");//数量2,小包装单位数量,FieldName="cInvDefine2QTY"
            var cInvDefine1QTYColumn = s.GetColumnByField("cInvDefine1QTY");//数量3,大包装单位数量,FieldName="cInvDefine1QTY"
            var cComUnitPriceColumn = s.GetColumnByField("cComUnitPrice"); //单价,FieldName="cComUnitPrice"
            //var cellInfoQTYa = e.rowValues[strCarplateNumberColumn.index];  //数量1,基本单位数量,FieldName="cComUnitQTY"
            var cellInfoQTYb = e.rowValues[cInvDefine2QTYColumn.index];      //数量2,小包装单位数量,FieldName="cInvDefine2QTY"
            var cellInfoQTYc = e.rowValues[cInvDefine1QTYColumn.index];      //数量3,大包装单位数量,FieldName="cInvDefine1QTY"
            var cellInfoPrice = e.rowValues[cComUnitPriceColumn.index];      //单价,FieldName="cComUnitPrice"
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
                baozhuangjieguo = parseInt((BasicUnitAmount * 10 * 10) / (parseFloat(ch[0]) * 10 * 10)) + chUnit[1] + parseFloat(parseFloat((parseInt(BasicUnitAmount * 10 * 10) % (parseInt(parseFloat(ch[0]) * 10 * 10 / parseFloat(ch[1]))))) / (100)) + chUnit[1];
                qtybb = -1;
                qtycc = 0;
            }
            if (ch.length >= 3) {
                //BasicUnitAmount = parseFloat(cellInfoQTYa.value);//基本单位数量汇总
                BasicUnitAmount = (parseFloat(cellInfoQTYa.value) + parseFloat(parseFloat(cellInfoQTYb.value * 10 * 10) * parseFloat((parseFloat(ch[0]) / parseFloat(ch[1])) * 10 * 10)) / 10000 + parseFloat(parseFloat(cellInfoQTYc.value * 10 * 10) * (parseFloat(ch[0]) * 10 * 10) / 10000));//基本单位数量汇总
                baozhuangjieguo = parseInt((BasicUnitAmount * 10 * 10) / (parseFloat(ch[0]) * 10 * 10)) + chUnit[2] + parseInt((parseInt((BasicUnitAmount * 10 * 10)) % parseInt(parseFloat(ch[0]) * 10 * 10)) / ((parseFloat(ch[0]) / parseFloat(ch[1])) * 10 * 10)) + chUnit[1] + parseFloat(parseFloat((parseInt(BasicUnitAmount * 10 * 10) % (parseInt(parseFloat(ch[0]) * 10 * 10 / parseFloat(ch[1]))))) / (100)) + chUnit[0];
                qtybb = -1;
                qtycc = -1;
            }
            var QTYPrice = BasicUnitAmount * parseFloat(cellInfoPrice.value);
            var rowsindex = "OrderGrid_DXDataRow" + s.GetFocusedRowIndex();
            var rows = document.getElementById(rowsindex);  //根据id找到这行
            var cell = rows.cells[14];//获取某行下面的某个td元素,金额
            cell.innerHTML = QTYPrice.toFixed(2);   //金额赋值
            var cell11 = rows.cells[11];//获取某行下面的某个td元素,基本单位汇总
            cell11.innerHTML = BasicUnitAmount + chUnit[0]; //数量赋值
            var cell12 = rows.cells[12];//获取某行下面的某个td元素,包装结果
            cell12.innerHTML = baozhuangjieguo;
            var cell7 = rows.cells[7];//获取某行下面的某个td元素,小包装数量
            if (qtybb != -1) {
                cell7.innerHTML = 0;
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
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <asp:ScriptManager ID="ScriptManager2" runat="server"></asp:ScriptManager>
        <div>
            <table align="left" style="width: 100%; float: none;">
                <tr>
                    <td class="auto-style18">网单号</td>
                    <td class="auto-style3">
                        <dx:ASPxTextBox ID="TxtOrderBillNo" runat="server" Width="145px" Height="18px" ReadOnly="True" Theme="Office2010Blue"  CssClass="auto-style16" Border-BorderStyle="None">
                        </dx:ASPxTextBox>
                    </td>
                    <td class="auto-style22">制单人</td>
                    <td class="auto-style9">
                        <dx:ASPxTextBox ID="TxtBiller" runat="server" Width="145px" Height="18px" EnableTheming="True" ReadOnly="True" Theme="Office2010Blue" CssClass="auto-style16" Border-BorderStyle="None">
                        </dx:ASPxTextBox>
                    </td>
                    <td class="auto-style22">订单日期</td>
                    <td class="auto-style1">
                        <dx:ASPxTextBox ID="TxtBillDate" runat="server" Width="145px" Height="18px" ReadOnly="True" Theme="Office2010Blue" CssClass="auto-style16" Border-BorderStyle="None">
                        </dx:ASPxTextBox>
                    </td>
                    <td class="auto-style1"></td>
                </tr>
                <tr>
                    <td class="auto-style23">开票单位</td>
                    <td class="auto-style4">
                        <div style="float: left">
                            <dx:ASPxTextBox ID="TxtCustomer" runat="server" Width="125px" ReadOnly="True" Theme="Office2010Blue" Text="" Height="18px" EnableViewState="true" CssClass="auto-style16" Border-BorderStyle="None">
                            </dx:ASPxTextBox>
                        </div>


                    </td>
                    <td class="auto-style19">业务员</td>
                    <td class="auto-style10">
                        <dx:ASPxTextBox ID="TxtSalesman" ClientInstanceName="TxtSalesman" runat="server" Width="145px" Height="18px" EnableTheming="True" Theme="Office2010Blue" Text="1249" CssClass="auto-style16" Border-BorderStyle="None">
                            <ReadOnlyStyle BackColor="Gray" />
                        </dx:ASPxTextBox>


                    </td>
                    <td class="auto-style19">信用额</td>
                    <td class="auto-style2">
                        <dx:ASPxTextBox ID="TxtCusCredit" ClientInstanceName="TxtCusCredit" runat="server" Width="145px" Height="18px" EnableTheming="True" ReadOnly="True" Theme="Office2010Blue" CssClass="auto-style16" Border-BorderStyle="None">
                        </dx:ASPxTextBox>


                    </td>
                    <td class="auto-style2"></td>
                </tr>
                <tr>
                    <td class="auto-style18">送货方式</td>
                    <td class="auto-style5" colspan="3">
                        <div style="float: left" class="auto-style16">
                            <dx:ASPxTextBox ID="TxtOrderShippingMethod" runat="server" Width="390px" Height="18px" Theme="Office2010Blue" ReadOnly="true" CssClass="auto-style16" Border-BorderStyle="None">
                            </dx:ASPxTextBox>
                        </div>


                    </td>
                    <td class="auto-style21">发运方式</td>
                    <td class="auto-style5">
                        <dx:ASPxTextBox ID="TxtcSCCode" ClientInstanceName="TxtcSCCode" runat="server" Width="145px" Height="18px" EnableTheming="True" ReadOnly="True" Theme="Office2010Blue" CssClass="auto-style16" Border-BorderStyle="None">
                        </dx:ASPxTextBox>


                    </td>
                    <td class="auto-style5"></td>
                </tr>
                <tr>
                    <td class="auto-style24">备注</td>
                    <td class="auto-style8">
                        <dx:ASPxTextBox ID="TxtOrderMark" runat="server" Width="185px" Theme="Office2010Blue" CssClass="auto-style16">
                        </dx:ASPxTextBox>
                    </td>
                    <td class="auto-style21">车型</td>
                    <td class="auto-style11">
                        <div style="float: left;">
                            <dx:ASPxTextBox ID="Txtcdefine3" runat="server" Width="115px" ReadOnly="True" Theme="Office2010Blue" Text="" Height="18px" EnableViewState="true" CssClass="auto-style16" Border-BorderStyle="None">
                            </dx:ASPxTextBox>
                        </div>
                    </td>
                    <td class="auto-style21">装车方式</td>
                    <td class="auto-style6">
                        <dx:ASPxTextBox ID="TxtLoadingWays" runat="server" Width="145px" Height="18px" Theme="Office2010Blue" CssClass="auto-style16">
                        </dx:ASPxTextBox>
                    </td>
                    <td class="auto-style6"></td>
                </tr>
                <tr>
                    <td class="auto-style18">下单时间</td>
                    <td class="auto-style8">
                        <dx:ASPxTextBox ID="TxtBillTime" runat="server" Width="185px" Height="16px" ReadOnly="True" Theme="Office2010Blue" CssClass="auto-style16" Border-BorderStyle="None">
                        </dx:ASPxTextBox>
                    </td>
                    <td class="auto-style21">提货时间</td>
                    <td class="auto-style11">
                        <dx:ASPxTextBox ID="TxtDate" runat="server" Width="145px" Height="18px" Theme="Office2010Blue" CssClass="auto-style16" ReadOnly="true" Border-BorderStyle="None">
                        </dx:ASPxTextBox>
                    </td>
                    <td class="auto-style21">销售类型</td>
                    <td class="auto-style7">
                        <dx:ASPxTextBox ID="TxtcSTCode" Text="样品资料" runat="server" ReadOnly="true" Width="145px" Height="18px" Theme="Office2010Blue" CssClass="auto-style16" Border-BorderStyle="None">
                        </dx:ASPxTextBox>
                    </td>
                    <td class="auto-style20"></td>
                </tr>
            </table>
        </div>
        <div>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server"><ContentTemplate>
            <dx:ASPxGridView ID="OrderGrid" ClientInstanceName="OrderGrid" runat="server" AutoGenerateColumns="False" EnableTheming="True" KeyFieldName="cInvCode" OnRowDeleting="OrderGrid_RowDeleting"
                ClientIDMode="Static" ShowVerticalScrollBar="true" Theme="Office2010Blue" OnRowValidating="OrderGrid_RowValidating" 
                OnHtmlDataCellPrepared="OrderGrid_HtmlDataCellPrepared"
                OnRowUpdating="OrderGrid_RowUpdating" OnCustomUnboundColumnData="GridViewShippingMethod_CustomUnboundColumnData" 
                Width="1230px" KeyboardSupport="True">
                <SettingsText Title="订单明细表" ContextMenuSummarySum="合计" />

                <ClientSideEvents BatchEditEndEditing="OrderGrid_BatchEditEndEditing" EndCallback="function(s, e) {EndCallBack(s,e);}" />
                <TotalSummary>
                    <dx:ASPxSummaryItem FieldName="cInvName" SummaryType="Count" ShowInGroupFooterColumn="名称" />
                    <dx:ASPxSummaryItem FieldName="cComUnitAmount" SummaryType="Sum" ShowInGroupFooterColumn="规格" />
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
                        <PropertiesTextEdit DisplayFormatString="{0:F}">
                        </PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="金额" VisibleIndex="16" FieldName="cComUnitAmount" ReadOnly="True" Width="100px">
                        <EditFormSettings Visible="False" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="基本单位汇总" FieldName="cInvDefineQTY" VisibleIndex="12" ReadOnly="True" Width="80px">
                        <PropertiesTextEdit DisplayFormatString="{0:F}">
                        </PropertiesTextEdit>
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
                        <PropertiesSpinEdit DisplayFormatString="g" MaxValue="99999">
                        </PropertiesSpinEdit>
                        <EditFormSettings VisibleIndex="0"></EditFormSettings>
                        <CellStyle BackColor="#FF66FF">
                        </CellStyle>
                    </dx:GridViewDataSpinEditColumn>
                    <dx:GridViewDataSpinEditColumn Caption="小包装单位数量" FieldName="cInvDefine2QTY" VisibleIndex="8" Width="80px">
                        <PropertiesSpinEdit DisplayFormatString="g" MaxValue="99999" NumberType="Integer">
                        </PropertiesSpinEdit>
                        <CellStyle BackColor="#FF66FF">
                        </CellStyle>
                    </dx:GridViewDataSpinEditColumn>
                    <dx:GridViewDataSpinEditColumn Caption="大包装单位数量" FieldName="cInvDefine1QTY" VisibleIndex="10" Width="80px">
                        <PropertiesSpinEdit DisplayFormatString="g" MaxValue="99999" NumberType="Integer">
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
                </ContentTemplate>
</asp:UpdatePanel>
            <table class="dxflInternalEditorTable">
                <tr>
                    <td class="auto-style14">
                        <div style="float:left;width:200px;">
                        <dx:ASPxButton ID="BtnSaveOrder" runat="server" Text="保存修改" OnClick="BtnSaveOrder_Click" Height="40px" Width="130px"
                            ClientInstanceName="BtnSaveOrder" EnableTheming="True" Theme="SoftOrange"  >
                        </dx:ASPxButton>
                             </div>
                        <div style="float:left;width:200px;"> 
                                               <dx:ASPxButton ID="BtnCancel" runat="server" Text="返回" OnClick="BtnCancel_Click" Height="40px" Width="130px"
                            ClientInstanceName="BtnCancel" EnableTheming="True" Theme="Office2010Black"  >
                            <ClientSideEvents Click="function(s, e) {
e.processOnServer=confirm('确认放弃编辑吗?'); 	
}" />
                        </dx:ASPxButton>
                   </div>      
                        <div style="float:left;width:200px;">
                        <dx:ASPxButton ID="BtnInvalidOrder" runat="server" Text="作废订单" OnClick="BtnInvalidOrder_Click" Height="40px" Width="130px"
                            ClientInstanceName="BtnInvalidOrder" EnableTheming="True" Theme="RedWine"  >
                            <ClientSideEvents Click="function(s, e) {
e.processOnServer=confirm('确认作废该订单?'); 	
}" />
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


        <dx:ASPxPopupControl ID="Inventory" runat="server" CloseAction="CloseButton" LoadContentViaCallback="OnFirstShow"
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
        </dx:ASPxPopupControl>
    </div>
    </form>
</body>
</html>
<script type="text/javascript">
    var cscode = function () {
        document.getElementById("<%=btn.ClientID%>").click();
    }
</script>
