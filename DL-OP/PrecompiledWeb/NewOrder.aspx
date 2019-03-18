<%@ page language="C#" autoeventwireup="true" inherits="NewOrder, dlopwebdll" enableviewstate="false" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>新增订单</title>
    <link href="../js/themes/icon.css" rel="stylesheet" />
    <link href="../js/themes/metro/easyui.css" rel="stylesheet" />
    <script src="../js/jquery.min.js"></script>
    <script src="../js/jquery.easyui.min.js"></script>
    <script src="../js/jquery.easyui.mobile.js"></script>
    <script src="../js/easyui-lang-zh_CN.js"></script>

    <style type="text/css">
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

        .auto-style14 {
            height: 48px;
        }
    </style>

</head>
<script type="text/javascript">
    $.messager.progress({
        title: '请稍后',
        msg: '页面加载中...'
    });

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
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>

                <div id="main" runat="server">

                    <div style="margin-bottom: 8px; width: 2500px">
                        <a style="float: left">订单日期：</a><div style="float: left">
                            <dx:ASPxTextBox ID="TxtBillDate" runat="server" Width="130px" Height="18px" ReadOnly="True">
                            </dx:ASPxTextBox>
                        </div>

                        <a style="float: left; margin-left: 15px">开票单位：</a><div style="float: left">
                            <dx:ASPxTextBox ID="TxtCustomer" ClientInstanceName="TxtCustomer" runat="server" Width="125px" ReadOnly="True" Text="" Height="18px" EnableViewState="true">
                            </dx:ASPxTextBox>
                        </div>
                        <div style="float: left; margin-left: 4px">
                            <dx:ASPxButton ID="ASPxButton2" runat="server" Text="选择" Width="40px" AutoPostBack="false" Height="16px" Theme="Office2010Blue">
                                <ClientSideEvents Click="function(s, e) { Customer.Show(); }" />
                            </dx:ASPxButton>
                        </div>

                        <a style="float: left; margin-left: 15px">信用额：</a><div style="float: left">
                            <dx:ASPxTextBox ID="TxtCusCredit1" ClientInstanceName="TxtCusCredit1" runat="server" Width="135px" Height="18px" EnableTheming="True" ReadOnly="True">
                            </dx:ASPxTextBox>
                        </div>
                         <div >
                             <asp:Label ID="LabHDNR" runat="server" Text="￥"></asp:Label>
                        </div>
                        <a style="float: left; margin-left: 15px; display: none;">酬宾类型：</a><div style="display: none;">
                            <dx:ASPxTextBox ID="TxtCBLX" ClientInstanceName="TxtCBLX" runat="server" Width="135px" Height="18px" EnableTheming="True" ReadOnly="True" Style="margin-bottom: 0px">
                            </dx:ASPxTextBox>
                            <dx:ASPxTextBox ID="TxtCusCredit" ClientInstanceName="TxtCusCredit" runat="server" Width="135px" Height="18px" EnableTheming="True" ReadOnly="True">
                            </dx:ASPxTextBox>
                        </div>

                    </div>

                    <div style="margin-bottom: 8px; width: 2500px">
                        <a style="float: left;">送货方式：</a><div style="float: left">
                            <dx:ASPxTextBox ID="TxtOrderShippingMethod" ClientInstanceName="TxtOrderShippingMethod" runat="server" Width="550px" Height="18px" EnableTheming="True" ReadOnly="True">
                            </dx:ASPxTextBox>
                        </div>
                        <div style="float: left;">
                            <dx:ASPxButton ID="ASPxButton1" runat="server" Text="选择" Width="50px" Height="18px" AutoPostBack="false" Theme="Office2010Blue">
                                <ClientSideEvents Click="function(s, e) { ShowWindow(); }" />
                            </dx:ASPxButton>
                            </div>
                        <div style="float: left;">
                            <dx:ASPxTextBox ID="Txtztadd" runat="server" ClientInstanceName="Txtztadd" EnableTheming="True" Height="18px" ReadOnly="True" Width="140px" NullText="自提必须选择行政区！">
                            </dx:ASPxTextBox>
                             </div >   <div >                        <dx:ASPxButton ID="ASPxButton4" runat="server" Text="选择" Width="50px" Height="18px" AutoPostBack="false" Theme="Office2010Blue">
                                <ClientSideEvents Click="function(s, e) { ShowXZQ(); }" />
                            </dx:ASPxButton>
                        </div>
                    </div>

                    <div style="margin-bottom: 8px; width: 2500px">
                        <a style="float: left">备注：</a>
                        <div>
                            <dx:ASPxTextBox runat="server" Width="700px" ID="TxtOrderMark" ClientInstanceName="TxtOrderMark">
                                <ClientSideEvents LostFocus="function(s, e) {
	SetCookieText(s.GetText());
}" />
                            </dx:ASPxTextBox>
                        </div>
                    </div>

                    <div style="margin-bottom: 8px; width: 2500px">
                        <a style="float: left">装车方式：</a>
                        <dx:ASPxTextBox runat="server" Width="680px" ID="TxtLoadingWays" ClientInstanceName="TxtLoadingWays">
                            <ClientSideEvents LostFocus="function(s, e) {
	SetCookieTextTxtLoadingWays(s.GetText());
}" />
                        </dx:ASPxTextBox>
                    </div>

                    <div style="margin-bottom: 10px; float: left; width: 2500px">
                        <a style="float: left">车型：</a>
                        <div style="float: left;">
                            <dx:ASPxTextBox ID="Txtcdefine3" ClientInstanceName="Txtcdefine3" runat="server" Width="175px" Height="18px" EnableTheming="True" ReadOnly="True">
                            </dx:ASPxTextBox>
                        </div>
                        <div style="float: left">
                            <dx:ASPxButton ID="ASPxButton3" runat="server" Text="选择" Width="40px" AutoPostBack="false" Height="16px" Theme="Office2010Blue">
                                <ClientSideEvents Click="function(s, e) { cdefine3.Show(); }" />
                            </dx:ASPxButton>
                        </div>
                        <a style="float: left">提货时间：</a>
                        <div style="float: left">
                            <dx:ASPxDateEdit ID="DeliveryDate" runat="server" AllowMouseWheel="False" AllowNull="False" AllowUserInput="False"
                                DateOnError="Today" EnableTheming="True" Theme="Office2010Blue" Height="18px" EditFormatString="yyyy-MM-dd HH" DisplayFormatString="G" UseMaskBehavior="True" EditFormat="Custom">
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
                        </div>
                    </div>

                    <div style="display: none;">
                        <dx:ASPxTextBox ID="TxtSalesman" ClientInstanceName="TxtSalesman" runat="server" ReadOnly="True">
                        </dx:ASPxTextBox>
                        <dx:ASPxTextBox ID="TxtcSCCode" ClientInstanceName="TxtcSCCode" runat="server" ReadOnly="True">
                        </dx:ASPxTextBox>
                        <dx:ASPxTextBox ID="TxtcSTCode" ClientInstanceName="TxtcSTCode" runat="server" ReadOnly="True">
                        </dx:ASPxTextBox>
                    </div>

                </div>

                <div style="margin-bottom: 8px; width: 2500px;">
                    <div style="float: left; margin-right: 10px">
                        <dx:ASPxButton ID="PreviousOrder" ClientInstanceName="PreviousOrder" runat="server" Text="参照历史订单" Width="100px" AutoPostBack="false" Height="16px" Theme="Youthful">
                            <ClientSideEvents Click="function(s, e) { PreviousOrderAgainPOP.Show(); }" />
                        </dx:ASPxButton>
                    </div>
                    <div style="float: left; margin-right: 10px;">
                        <dx:ASPxButton ID="BackData" ClientInstanceName="BackData" runat="server" Text="临时订单" Width="100px" AutoPostBack="false" Height="16px" Theme="Youthful" ToolTip="提取临时订单的信息,提交正式订单后该数据会被清除!">
                            <ClientSideEvents Click="function(s, e){
BackOrderPOP.Show();
}" />
                        </dx:ASPxButton>
                    </div>
                    <div style="float: left; margin-right: 10px">
                        <dx:ASPxButton ID="BtnMain" ClientInstanceName="BtnMain" runat="server" Text="收起↑" OnClick="BtnMain_Click" Theme="Office2010Blue"></dx:ASPxButton>
                    </div>
                    <div style="float: left; margin-right: 10px">
                        <dx:ASPxSpinEdit ID="GridViewHeigh" ClientInstanceName="GridViewHeigh" runat="server" MaxValue="3000" MinValue="200" NullText="输入200-2000之间,默认200高度" NumberType="Integer" Width="200px" Increment="25"></dx:ASPxSpinEdit>
                    </div>
                    <div style="float: left; margin-right: 10px">
                        <dx:ASPxSpinEdit ID="GridViewFontSize" ClientInstanceName="GridViewFontSize" runat="server" MaxValue="14" MinValue="8" NullText="输入8-14之间,默认字体9大小" NumberType="Integer" Width="200px"></dx:ASPxSpinEdit>
                    </div>
                    <div style="float: left;margin-right: 1200px">
                        <dx:ASPxButton ID="BtnOrderGridHeigh" runat="server" Text="设置订单明细表高度和字体大小" OnClick="BtnOrderGridHeigh_Click" Theme="Office2010Blue"></dx:ASPxButton>
                        <asp:HiddenField ID="HFMain" runat="server" Value="0" />
                    </div>
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>

        <div style="margin-top:5px">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <dx:ASPxGridView ID="OrderGrid" ClientInstanceName="OrderGrid" runat="server" AutoGenerateColumns="False" EnableTheming="True" KeyFieldName="cInvCode" OnRowDeleting="OrderGrid_RowDeleting"
                        ClientIDMode="Static" ShowVerticalScrollBar="true" Theme="Office2010Blue" OnRowValidating="OrderGrid_RowValidating" OnHtmlRowPrepared="OrderGrid_HtmlRowPrepared"
                        OnInit="OrderGrid_Init" OnCustomButtonCallback="OrderGrid_CustomButtonCallback"
                        OnRowUpdating="OrderGrid_RowUpdating" OnCustomUnboundColumnData="GridViewShippingMethod_CustomUnboundColumnData" Width="1260px" KeyboardSupport="True" Font-Size="10pt">
                        <SettingsText Title="订单明细表" ContextMenuSummarySum="合计" ConfirmOnLosingBatchChanges="订单明细未保存,点击'取消'返回,如不需要保存点击'确定'提交订单." />

                        <ClientSideEvents BatchEditEndEditing="OrderGrid_BatchEditEndEditing" EndCallback="function(s, e) {EndCallBack(s,e);}" />
                        <TotalSummary>
                            <dx:ASPxSummaryItem FieldName="cInvName" SummaryType="Count" ShowInGroupFooterColumn="名称" />
                            <dx:ASPxSummaryItem FieldName="cComUnitAmount" SummaryType="Sum" DisplayFormat="总金额:" ShowInColumn="单价" />
                            <dx:ASPxSummaryItem DisplayFormat="{0:F}" FieldName="cComUnitAmount" ShowInColumn="金额" SummaryType="Sum" />
                            <dx:ASPxSummaryItem DisplayFormat="{0:F}" FieldName="xx" ShowInColumn="规格" SummaryType="Sum" Visible="False" />
                        </TotalSummary>
                        <Columns>
                            <dx:GridViewDataTextColumn Caption="编码" FieldName="cInvCode" VisibleIndex="20" Width="0px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="名称" VisibleIndex="2" FieldName="cInvName" ReadOnly="True" Width="150px">
                                <PropertiesTextEdit DisplayFormatString="{0:F}">
                                    <Style Font-Size="9pt">
                                    </Style>
                                </PropertiesTextEdit>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="规格" VisibleIndex="3" FieldName="cInvStd" ReadOnly="True" Width="100px">
                                <PropertiesTextEdit DisplayFormatString="0.0000">
                                </PropertiesTextEdit>
                                <EditFormSettings Visible="False" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="基本单位" FieldName="cComUnitName" VisibleIndex="6" ReadOnly="True" Width="50px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="小包装单位" FieldName="cInvDefine2" VisibleIndex="8" ReadOnly="True" Width="55px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="大包装单位" FieldName="cInvDefine1" VisibleIndex="10" ReadOnly="True" Width="55px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="单位组" FieldName="UnitGroup" VisibleIndex="4" ReadOnly="True" Width="120px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="单价" FieldName="cComUnitPrice" VisibleIndex="14" ReadOnly="True" Width="50px">
                                <PropertiesTextEdit DisplayFormatString="{0:F}">
                                </PropertiesTextEdit>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="金额" FieldName="cComUnitAmount" VisibleIndex="15" ReadOnly="True" Width="100px">
                                <PropertiesTextEdit DisplayFormatString="0.0000">
                                </PropertiesTextEdit>
                                <EditFormSettings Visible="False" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="基本单位汇总" FieldName="cInvDefineQTY" VisibleIndex="11" ReadOnly="True" Width="80px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="小包装换算率" FieldName="cInvDefine14" VisibleIndex="16" Width="0px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="大包装换算率" FieldName="cInvDefine13" VisibleIndex="17" Width="0px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="序号" FieldName="irowno" ReadOnly="True" UnboundType="Integer" VisibleIndex="1" Width="36px">
                                <HeaderStyle Font-Size="9pt" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="包装结果" FieldName="pack" ReadOnly="True" VisibleIndex="13" Width="100px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataSpinEditColumn Caption="基本单位数量" FieldName="cComUnitQTY" VisibleIndex="5" EditFormSettings-VisibleIndex="0" Width="70px">
                                <PropertiesSpinEdit DisplayFormatString="g" MaxValue="99999" AllowMouseWheel="False" AllowNull="False" NullText="0">
                                </PropertiesSpinEdit>
                                <EditFormSettings VisibleIndex="0"></EditFormSettings>
                                <CellStyle BackColor="#FF66FF">
                                </CellStyle>
                            </dx:GridViewDataSpinEditColumn>
                            <dx:GridViewDataSpinEditColumn Caption="小包装单位数量" FieldName="cInvDefine2QTY" VisibleIndex="7" Width="80px">
                                <PropertiesSpinEdit DisplayFormatString="g" MaxValue="99999" NumberType="Integer" AllowMouseWheel="False" AllowNull="False" NullText="0">
                                </PropertiesSpinEdit>
                                <CellStyle BackColor="#FF66FF">
                                </CellStyle>
                            </dx:GridViewDataSpinEditColumn>
                            <dx:GridViewDataSpinEditColumn Caption="大包装单位数量" FieldName="cInvDefine1QTY" VisibleIndex="9" Width="80px">
                                <PropertiesSpinEdit DisplayFormatString="g" MaxValue="99999" NumberType="Integer" AllowMouseWheel="False" AllowNull="False" NullText="0">
                                </PropertiesSpinEdit>
                                <CellStyle BackColor="#FF66FF">
                                </CellStyle>
                            </dx:GridViewDataSpinEditColumn>
                            <dx:GridViewDataTextColumn Caption="可用库存量" FieldName="Stock" ReadOnly="True" VisibleIndex="12" Width="60px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewCommandColumn Caption="删除" ShowDeleteButton="True" VisibleIndex="0" Width="100px">
                                <CustomButtons>
                                    <dx:GridViewCommandColumnCustomButton ID="RowsNoBtnUp" Text=" ">
                                        <Image Height="15px" Url="~/images/up.jpg" Width="8px">
                                        </Image>
                                        <Styles>
                                            <Style>
                                                <Paddings PaddingLeft="14px" />
                                            </Style>
                                        </Styles>
                                    </dx:GridViewCommandColumnCustomButton>
                                    <dx:GridViewCommandColumnCustomButton ID="RowsNoBtnDown" Text=" ">
                                        <Image Height="15px" Url="~/images/down.jpg" Width="8px">
                                        </Image>
                                        <Styles>
                                            <Style>
                                                <Paddings PaddingLeft="8px" />
                                            </Style>
                                        </Styles>
                                    </dx:GridViewCommandColumnCustomButton>
                                </CustomButtons>
                                <HeaderTemplate>
                                    <dx:ASPxButton ID="Choose" runat="server" Text="选择商品" AutoPostBack="False" EnableTheming="True" Theme="Office2010Blue" ForeColor="Red" Width="55px">
                                        <ClientSideEvents Click="function(s, e) {
 Inventory.Show()	
}" />
                                    </dx:ASPxButton>
                                </HeaderTemplate>
                            </dx:GridViewCommandColumn>
                            <dx:GridViewDataTextColumn Caption="执行单价" FieldName="ExercisePrice" ReadOnly="True" VisibleIndex="18" Width="0px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="执行金额" FieldName="xx" ReadOnly="True" UnboundType="Decimal" VisibleIndex="19" Width="0px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="iLowSumRel" FieldName="iLowSumRel" VisibleIndex="21" Width="0px">
                            </dx:GridViewDataTextColumn>
                        </Columns>
                        <SettingsBehavior AllowSort="False" AllowFocusedRow="True" />
                        <SettingsPager Mode="ShowAllRecords" />
                        <SettingsEditing EditFormColumnCount="99" Mode="Batch" />
                        <Settings ShowTitlePanel="true" ShowFooter="True" VerticalScrollableHeight="200" VerticalScrollBarMode="Auto" HorizontalScrollBarMode="Visible" />

                        <SettingsCommandButton>
                            <UpdateButton Text="保存购物信息" ButtonType="Button">
                                <Styles>
                                    <Style Font-Size="Medium" ForeColor="Red" HorizontalAlign="Left" VerticalAlign="Top" Paddings-PaddingRight="1000px">
                                        
                                        <Paddings PaddingRight="1000px" />
                                        
                                    </Style>
                                </Styles>
                            </UpdateButton>
                            <CancelButton Text="取消修改" ButtonType="Button">
                                <Styles>
                                    <Style Font-Size="Medium" ForeColor="Red" HorizontalAlign="Center" VerticalAlign="Middle">
                                        
                                    </Style>
                                </Styles>
                            </CancelButton>
                            <DeleteButton ButtonType="Image">
                                <Image AlternateText="删除该项" Url="~/images/del.jpg" Width="18px">
                                </Image>
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
                        <div style="float: left; margin-left: 10px">
                            <dx:ASPxButton ID="BtnSaveOrder" runat="server" Text="保存购物信息后提交生成正式订单" OnClick="BtnSaveOrder_Click" Height="35px" Width="160px"
                                ClientInstanceName="BtnSaveOrder" EnableTheming="True" Font-Size="12pt"
                                ForeColor="White" Font-Names="微软雅黑" Theme="SoftOrange">
                            </dx:ASPxButton>
                        </div>
                        <div style="float: left; margin-left: 10px">
                            <dx:ASPxButton ID="BtnSaveBackOrder" runat="server" Text="保存为临时订单" Height="35px" Width="160px"
                                ClientInstanceName="BtnSaveBackOrder" EnableTheming="True" Font-Size="12pt"
                                ForeColor="White" Font-Names="微软雅黑" Theme="RedWine" AutoPostBack="False">
                                <ClientSideEvents Click="function(s, e) {
SaveBackOrderNamePOP.Show();	
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

            <dx:ASPxPopupControl ID="ShippingMethod" runat="server" CloseAction="CloseButton" CloseOnEscape="True" Modal="True" Target="_blank"
                PopupHorizontalAlign="LeftSides" PopupVerticalAlign="TopSides" ClientInstanceName="ShippingMethod"
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
                                                    <dx:ASPxComboBox ID="ComboShippingMethod" runat="server" Height="20px" Width="194px" SelectedIndex="0" AutoPostBack="true"
                                                        OnSelectedIndexChanged="ComboShippingMethod_SelectedIndexChanged">
                                                        <Items>
                                                            <dx:ListEditItem Selected="True" Text="配送" Value="配送" />
                                                            <dx:ListEditItem Text="自提" Value="自提" />
                                                        </Items>
                                                    </dx:ASPxComboBox>
                                                </div>
                                                <div style="float: left; margin-left: 20px; margin-bottom: 10px">
                                                    <dx:ASPxButton ID="ReAddr" runat="server" Text="刷新地址列表信息" AutoPostBack="true" Theme="Office2010Blue" OnClick="ComboShippingMethod_SelectedIndexChanged" Font-Size="9pt">
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
                                                    OnFocusedRowChanged="GridViewShippingMethod_FocusedRowChanged"
                                                    OnCustomUnboundColumnData="GridViewShippingMethod_CustomUnboundColumnData" Font-Size="10pt">
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
                                                        <dx:GridViewDataTextColumn Caption="cDefine8" FieldName="strDistrict" ShowInCustomizationForm="True" Visible="False" VisibleIndex="10">
                                                        </dx:GridViewDataTextColumn>
                                                    </Columns>
                                                    <SettingsBehavior AllowFocusedRow="True" AllowSort="False" />
                                                    <SettingsPager NumericButtonCount="15" PageSize="15" Mode="ShowAllRecords">
                                                    </SettingsPager>
                                                    <Settings VerticalScrollableHeight="200" VerticalScrollBarMode="Visible" ShowFilterRow="True" ShowFilterRowMenu="True" />
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
            <dx:ASPxPopupControl ID="apc_xzq" runat="server" CloseAction="CloseButton" CloseOnEscape="True" Modal="True" Target="_blank"
                PopupHorizontalAlign="LeftSides" PopupVerticalAlign="TopSides" ClientInstanceName="apc_xzq"
                HeaderText="选择行政区" AllowDragging="True" PopupAnimationType="None" EnableViewState="true" ClientIDMode="Static" Height="203px" Width="447px">
                <ClientSideEvents PopUp="function(s, e) { ComboShippingMethod.Focus(); }" />
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl7" runat="server">
                        <dx:ASPxPanel ID="ASPxPanel6" runat="server" DefaultButton="btOK">

                            <PanelCollection>

                                <dx:PanelContent ID="PanelContent7" runat="server">


                                    <table>
                                        <tr>
                                            <td class="auto-style8">
                                                &nbsp;</td>
                                            <td class="auto-style12" style="margin-bottom: 10px">
                                                <div style="float: left; margin-left: 20px; margin-bottom: 10px">
                                                    <dx:ASPxButton ID="ASPxButton5" runat="server" Text="刷新行政区列表信息" AutoPostBack="true" Theme="Office2010Blue" 
                                                        OnClick="xzq_SelectedIndexChanged" Font-Size="9pt">
                                                    </dx:ASPxButton>
                                                    (行政区信息在‘收货地址’中添加)</div>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td class="auto-style8">
                                                <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="行政区:" AssociatedControlID="xzq">
                                                </dx:ASPxLabel>
                                            </td>

                                            <td class="auto-style12">

                                                <dx:ASPxGridView ID="xzq_GV" runat="server" AutoGenerateColumns="False" EnableTheming="True"
                                                    Theme="Office2010Blue" KeyFieldName="lngopUseraddress_exId" Width="352px"
                                                    OnInit="xzq_GV_Init"
                                                     Font-Size="10pt" ClientInstanceName="xzq_GV">
                                                    <ClientSideEvents RowDblClick="function(s, e) {
apc_xzq.Hide();
xzqdbclick();	
}" />
                                                    <Columns>
                                                        <dx:GridViewDataTextColumn ShowInCustomizationForm="True" VisibleIndex="0" Caption="lngopUseraddress_exId" FieldName="lngopUseraddress_exId" ReadOnly="True" Visible="False">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="行政区信息" FieldName="xzq" ShowInCustomizationForm="True" VisibleIndex="1">
                                                            <Settings AutoFilterCondition="Contains" />
                                                        </dx:GridViewDataTextColumn>
                                                    </Columns>
                                                    <SettingsBehavior AllowFocusedRow="True" AllowSort="False" />
                                                    <SettingsPager NumericButtonCount="15" PageSize="15" Mode="ShowAllRecords">
                                                    </SettingsPager>
                                                    <Settings VerticalScrollableHeight="200" VerticalScrollBarMode="Visible" ShowFilterRowMenu="True" />
                                                    <SettingsCommandButton>
                                                        <SearchPanelApplyButton Text="查询">
                                                        </SearchPanelApplyButton>
                                                        <SearchPanelClearButton Text="清除">
                                                        </SearchPanelClearButton>
                                                    </SettingsCommandButton>
                                                    <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
                                                    <SettingsSearchPanel ColumnNames="行政区信息" ShowApplyButton="True" ShowClearButton="True" />
                                                </dx:ASPxGridView>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td class="auto-style10"></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div style="text-align: center">

                                                    <dx:ASPxButton ID="btxzqOK" ClientInstanceName="btxzqOK" runat="server" Text="确定" Width="80px" AutoPostBack="true"
                                                        Style="float: left; margin-right: 8px" OnClick="btxzqOK_Click">
                                                        <ClientSideEvents Click="function(s, e) {
apc_xzq.Hide();
 }" />
                                                    </dx:ASPxButton>

                                                    <dx:ASPxButton ID="ASPxButton7" runat="server" Text="取消" Width="80px" AutoPostBack="False" Style="float: left; margin-right: 8px">
                                                        <ClientSideEvents Click="function(s, e) { apc_xzq.Hide(); }" />
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
                PopupHorizontalAlign="LeftSides" PopupVerticalAlign="TopSides" ClientInstanceName="Customer"
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
                                                    OnCustomUnboundColumnData="GridViewShippingMethod_CustomUnboundColumnData" Font-Size="10pt">
                                                    <ClientSideEvents RowDblClick="function(s, e) {
Customer.Hide();
Customer_dbclick();	
}" />
                                                    <Columns>
                                                        <dx:GridViewDataTextColumn ShowInCustomizationForm="True" VisibleIndex="0" Caption="客户编码" FieldName="cCusCode" ReadOnly="True" Visible="False">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="序号" FieldName="hh" Width="50px" UnboundType="Integer" ShowInCustomizationForm="True" VisibleIndex="1" ReadOnly="True">
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
            <dx:ASPxPopupControl ID="cdefine3" runat="server" CloseAction="CloseButton" CloseOnEscape="True" Modal="True" Target="_blank"
                PopupHorizontalAlign="LeftSides" PopupVerticalAlign="TopSides" ClientInstanceName="cdefine3"
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
                                                    OnCustomUnboundColumnData="GridViewShippingMethod_CustomUnboundColumnData" Font-Size="10pt">
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
        </div>

        <div>
            <dx:ASPxPopupControl ID="PreviousOrderAgainPOP" runat="server" CloseAction="CloseButton" CloseOnEscape="True" Modal="True" Target="_blank"
                PopupHorizontalAlign="LeftSides" PopupVerticalAlign="TopSides" ClientInstanceName="PreviousOrderAgainPOP"
                HeaderText="选择DL订单" AllowDragging="True" PopupAnimationType="None" EnableViewState="true" ClientIDMode="Static" Height="350px" Width="500px">
                <ClientSideEvents PopUp="function(s, e) { PreviousOrderAgainGridView.Focus(); }" />
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">
                        <dx:ASPxPanel ID="ASPxPanel4" runat="server" DefaultButton="BtnPreviousOrderAgainOk">
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent5" runat="server">
                                    <table>
                                        <tr>
                                            <td>
                                                <a style="color: red">注意：参照历史订单会提取对应的历史订单的所有信息并将当前在制的订单信息覆盖掉，请谨慎操作。如不需要使用该功能请点击'取消'返回。</a>
                                                <dx:ASPxGridView ID="PreviousOrderAgainGridView" runat="server" AutoGenerateColumns="False" EnableTheming="True"
                                                    ClientInstanceName="PreviousOrderAgainGridView" Theme="Office2010Blue" KeyFieldName="strBillNo" Width="409px" Font-Size="9pt">
                                                    <ClientSideEvents RowDblClick="function(s, e) {
PreviousOrderAgainPOP.Hide();
PreviousOrderAgainPOP_dbclick();	
}" />
                                                    <Columns>
                                                        <dx:GridViewCommandColumn ShowClearFilterButton="True" ShowInCustomizationForm="True" VisibleIndex="0">
                                                        </dx:GridViewCommandColumn>
                                                        <dx:GridViewDataTextColumn Caption="网单号" FieldName="strBillNo" ShowInCustomizationForm="True" VisibleIndex="1" ReadOnly="True" Width="160px"
                                                            SortIndex="0">
                                                            <Settings AllowSort="False" AutoFilterCondition="Contains" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="正式订单" FieldName="cSOCode" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="2" Width="160px">
                                                            <Settings AutoFilterCondition="Contains" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="下单时间" FieldName="datBillTime" ShowInCustomizationForm="True" VisibleIndex="3" Width="200px" ReadOnly="True">
                                                            <Settings AllowAutoFilter="False" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="lngopOrderId" FieldName="lngopOrderId" Name="lngopOrderId" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="4" Width="0px">
                                                        </dx:GridViewDataTextColumn>
                                                    </Columns>
                                                    <SettingsBehavior AllowFocusedRow="True" AllowSort="False" AllowSelectByRowClick="True" AllowSelectSingleRowOnly="True" />
                                                    <SettingsPager Mode="ShowAllRecords">
                                                    </SettingsPager>
                                                    <Settings VerticalScrollBarMode="Auto" ShowFilterRow="True" ShowFilterRowMenu="True" VerticalScrollableHeight="350" />
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

                                                    <dx:ASPxButton ID="BtnPreviousOrderAgainOk" ClientInstanceName="BtnPreviousOrderAgainOk" runat="server" Text="确定" Width="80px"
                                                        AutoPostBack="true" Style="float: left; margin-right: 8px" OnClick="BtnPreviousOrderAgainOk_Click">
                                                        <ClientSideEvents Click="function(s, e) {
PreviousOrderAgainPOP.Hide();
 }" />
                                                    </dx:ASPxButton>

                                                    <dx:ASPxButton ID="Btn_PreviousOrderAgain_Cancel" runat="server" Text="取消" Width="80px" AutoPostBack="False"
                                                        ClientInstanceName="Btn_PreviousOrderAgain_Cancel" Style="float: left; margin-right: 8px">
                                                        <ClientSideEvents Click="function(s, e) { PreviousOrderAgainPOP.Hide(); }" />
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
            <dx:ASPxPopupControl ID="BackOrderPOP" runat="server" CloseAction="CloseButton" CloseOnEscape="True" Modal="True" Target="_blank"
                PopupHorizontalAlign="LeftSides" PopupVerticalAlign="TopSides" ClientInstanceName="BackOrderPOP"
                HeaderText="选择临时订单" AllowDragging="True" PopupAnimationType="None" EnableViewState="true" ClientIDMode="Static" Height="350px" Width="500px">
                <ClientSideEvents PopUp="function(s, e) { BtnBackOrderGridView.Focus(); }" />
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                        <dx:ASPxPanel ID="ASPxPanel3" runat="server" DefaultButton="BtnBackOrderOK">
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent4" runat="server">
                                    <table>
                                        <tr>
                                            <td>
                                                <a style="color: red">注意：参照临时订单会提取对应临时订单的所有信息并将当前在制的订单信息覆盖掉，请谨慎操作。如不需要使用该功能请点击&#39;取消&#39;返回。</a>
                                                <dx:ASPxGridView ID="BackOrderGridView" runat="server" AutoGenerateColumns="False" EnableTheming="True"
                                                    OnLoad="BackOrderGridView_Load" OnRowDeleting="BackOrderGridView_RowDeleting"
                                                    ClientInstanceName="BackOrderGridView" Theme="Office2010Blue" KeyFieldName="lngopOrderBackId" Width="409px" Font-Size="9pt">
                                                    <ClientSideEvents RowDblClick="function(s, e) {
BackOrderPOP.Hide();
BackOrderPOP_dbclick();	
}" />
                                                    <Columns>
                                                        <dx:GridViewCommandColumn ShowClearFilterButton="True" ShowInCustomizationForm="True" VisibleIndex="0" Width="40px" ShowDeleteButton="True">
                                                        </dx:GridViewCommandColumn>
                                                        <dx:GridViewDataTextColumn Caption="名称" FieldName="strBillName" ShowInCustomizationForm="True" VisibleIndex="1" ReadOnly="True" Width="160px"
                                                            SortIndex="0">
                                                            <Settings AllowSort="False" AutoFilterCondition="Contains" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="保存时间" FieldName="datBillTime" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="2" Width="200px">
                                                            <Settings AllowAutoFilter="False" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="订单id" ShowInCustomizationForm="True" VisibleIndex="3" Width="0px" FieldName="lngopOrderBackId">
                                                        </dx:GridViewDataTextColumn>
                                                    </Columns>
                                                    <SettingsBehavior AllowFocusedRow="True" AllowSort="False" AllowSelectByRowClick="True" AllowSelectSingleRowOnly="True" />
                                                    <SettingsPager Mode="ShowAllRecords">
                                                    </SettingsPager>
                                                    <Settings VerticalScrollBarMode="Auto" ShowFilterRow="True" ShowFilterRowMenu="True" VerticalScrollableHeight="350" />
                                                    <SettingsText ContextMenuDeleteRow="确认删除该临时订单?" ConfirmDelete="确认删除该临时订单?" />
                                                    <SettingsCommandButton>
                                                        <DeleteButton Text="删除">
                                                        </DeleteButton>
                                                    </SettingsCommandButton>
                                                    <SettingsDataSecurity AllowEdit="False" AllowInsert="False" />
                                                </dx:ASPxGridView>
                                            </td>
                                        </tr>
                                        <tr>

                                            <td class="auto-style10">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div style="text-align: center">

                                                    <dx:ASPxButton ID="BtnBackOrderOK" ClientInstanceName="BtnBackOrderOK" runat="server" Text="确定" Width="80px"
                                                        AutoPostBack="true" Style="float: left; margin-right: 8px" OnClick="BtnBackOrderOK_Click">
                                                        <ClientSideEvents Click="function(s, e) {
e.processOnServer=confirm('确定要恢复该临时订单信息吗?'); 	
BackOrderPOP.Hide();
 }" />
                                                    </dx:ASPxButton>

                                                    <dx:ASPxButton ID="Btn_BackOrder_Cancel" runat="server" Text="取消" Width="80px" AutoPostBack="False"
                                                        ClientInstanceName="Btn_BackOrder_Cancel" Style="float: left; margin-right: 8px">
                                                        <ClientSideEvents Click="function(s, e) { BackOrderPOP.Hide(); }" />
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
            <dx:ASPxPopupControl ID="SaveBackOrderNamePOP" runat="server" CloseAction="CloseButton" CloseOnEscape="True" Modal="True" Target="_blank"
                PopupHorizontalAlign="LeftSides" PopupVerticalAlign="WindowCenter" ClientInstanceName="SaveBackOrderNamePOP"
                HeaderText="保存临时订单" AllowDragging="True" PopupAnimationType="None" EnableViewState="true" ClientIDMode="Static" Height="125px" Width="500px">
                <ClientSideEvents PopUp="function(s, e) { TxtSaveBackOrderName.Focus(); }" />
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl6" runat="server">
                        <dx:ASPxPanel ID="ASPxPanel5" runat="server" DefaultButton="BtnSaveBackOrderNameOK">
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent6" runat="server">
                                    <table>
                                        <tr>
                                            <td>
                                                <a style="color: red">请输入保存临时订单的名称,如不输入则默认为"临时订单"</a>
                                            </td>
                                        </tr>
                                        <tr>

                                            <td>
                                                <dx:ASPxTextBox ID="TxtSaveBackOrderName" ClientInstanceName="TxtSaveBackOrderName" runat="server" Width="300px" MaxLength="60" NullText="请输入保存临时订单的名称,30汉字以内">
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div style="text-align: center; margin-top: 10px">

                                                    <dx:ASPxButton ID="BtnSaveBackOrderNameOK" ClientInstanceName="BtnSaveBackOrderNameOK" runat="server" Text="确定" Width="80px"
                                                        AutoPostBack="true" Style="float: left; margin-right: 8px" OnClick="BtnSaveBackOrderNameOK_Click">
                                                        <ClientSideEvents Click="function(s, e) {
SaveBackOrderNamePOP.Hide();
 }" />
                                                    </dx:ASPxButton>

                                                    <dx:ASPxButton ID="Btn_SaveBackOrderName_Cancel" runat="server" Text="取消" Width="80px" AutoPostBack="False"
                                                        ClientInstanceName="Btn_SaveBackOrderName_Cancel" Style="float: left; margin-right: 8px">
                                                        <ClientSideEvents Click="function(s, e) { SaveBackOrderNamePOP.Hide(); }" />
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
            <dx:ASPxPopupControl ID="Inventory" runat="server" CloseAction="CloseButton"
                PopupElementID="Inventory" PopupVerticalAlign="TopSides" PopupHorizontalAlign="LeftSides"
                ShowFooter="True" Width="890px" Height="450px" HeaderText="选择商品" ClientInstanceName="Inventory" CloseOnEscape="True" Modal="True">
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
                                <div style="float: left; margin: 6px 100px 6px 0px; width: 50px;">
                                    <dx:ASPxButton ID="BtnInv_Reset" runat="server" AutoPostBack="true" ClientInstanceName="BtnInv_Reset" OnClick="BtnInv_Reset_Click"
                                        Style="float: left; margin-right: 8px" Text="清除所有选择项" Width="120px">
                                        <ClientSideEvents Click="function(s, e) { Inventory.Show(); }" />
                                    </dx:ASPxButton>

                                </div>
                                <div style="float: left; margin: 6px 0px 20px 0px"><a style="float: left">请勿在切换分类数据没有刷新完毕时再切换分类,否则可能造成已选数据丢失.</a></div>
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
                                        <SettingsBehavior AllowFocusedNode="True" AllowSort="False" ProcessFocusedNodeChangedOnServer="True" ProcessSelectionChangedOnServer="True"
                                            AllowDragDrop="False" ExpandCollapseAction="NodeDblClick" FocusNodeOnExpandButtonClick="False" FocusNodeOnLoad="False" />
                                        <SettingsCookies CookiesID="NewOrder" Enabled="True" />
                                        <SettingsLoadingPanel ShowOnPostBacks="True" Text="数据加载中,请稍候..." />
                                        <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
                                        <SettingsText LoadingPanelText="数据加载中,请稍候..." />
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
}"
                                            FocusedNodeChanged="function(s, e) {
            var key = treeList.GetFocusedNodeKey();
            treeList.PerformCustomDataCallback(key);
}" />
                                    </dx:ASPxTreeList>
                                </div>
                                <div style="float: left;">

                                    <asp:Button ID="btn" runat="server" OnClick="btn_Click" Text="Button" Style="display: none;" />
                                    <%--<input id='ctl' type='button' onclick='cscode();' style="display: none;" />--%>
                                    <dx:ASPxGridView ID="TreeDetail" ClientInstanceName="TreeDetail" runat="server" AutoGenerateColumns="False" Theme="Office2010Blue"
                                        ShowVerticalScrollBar="true" KeyFieldName="cInvCode" PreviewFieldName="cInvName" Caption="选择物料明细 ↓" Width="515px" Font-Size="9pt">
                                        <ClientSideEvents BeginCallback="function(s, e) {
}" />
                                        <Columns>
                                            <dx:GridViewDataTextColumn FieldName="cInvName" VisibleIndex="4" Width="150px" Caption="名称" ReadOnly="True">
                                                <Settings AllowAutoFilterTextInputTimer="True" AutoFilterCondition="Contains" AllowAutoFilter="True" />
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="cInvStd" ReadOnly="True" VisibleIndex="5" Width="100px" Caption="规格">
                                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="cComUnitName" VisibleIndex="6" Caption="单位" Width="30px" ReadOnly="True">
                                                <Settings AllowAutoFilter="False" />
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewCommandColumn Caption="选择" ShowSelectCheckbox="True" VisibleIndex="0" Width="35px" SelectAllCheckboxMode="AllPages" ShowClearFilterButton="True">
                                            </dx:GridViewCommandColumn>
                                            <dx:GridViewDataTextColumn Caption="编码" FieldName="cInvCode" VisibleIndex="2" Width="0px">
                                                <Settings AllowAutoFilter="False" />
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="首字拼音" FieldName="PY" ReadOnly="True" VisibleIndex="3" Width="100px">
                                                <Settings AllowAutoFilter="True" AllowAutoFilterTextInputTimer="True" AllowSort="False" AutoFilterCondition="Contains" />
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataImageColumn FieldName="img" VisibleIndex="1" Width="25px" Caption=" ">
                                            </dx:GridViewDataImageColumn>
                                        </Columns>
                                        <SettingsBehavior AllowSort="False" AllowFocusedRow="True" AllowDragDrop="False" />
                                        <SettingsPager Visible="False" Mode="EndlessPaging" NumericButtonCount="999">
                                        </SettingsPager>
                                        <SettingsEditing EditFormColumnCount="99">
                                        </SettingsEditing>
                                        <Settings VerticalScrollBarMode="Visible" ShowFooter="False" ShowTitlePanel="True" VerticalScrollableHeight="390" ShowFilterRow="True" ShowFilterRowMenu="True" />
                                        <SettingsText CommandApplySearchPanelFilter="查询" CommandCancel="取消" CommandClearSearchPanelFilter="清除" ConfirmOnLosingBatchChanges="未保存的数据将丢失,是否离开?" SearchPanelEditorNullText="请输入查找内容..." />
                                        <SettingsLoadingPanel Text="数据加载中,请稍候..." />
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
                
                        <asp:ObjectDataSource ID="ods" runat="server"
                    TypeName="EmployeeSessionProvider" SelectMethod="Select"></asp:ObjectDataSource>
    </form>
</body>
</html>
<script type="text/javascript">
    //显示送货方式窗口,
      function ShowWindow() {
          ShippingMethod.Show();
      }

      //显示行政区窗口,
      function ShowXZQ() {
          apc_xzq.Show();
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
    //调用btOK按钮事件
    function PreviousOrderAgainPOP_dbclick() {
        document.getElementById("<%=BtnPreviousOrderAgainOk.ClientID%>").click();
    }
    //调用btOK按钮事件
    function BackOrderPOP_dbclick() {
        document.getElementById("<%=BtnBackOrderOK.ClientID%>").click();
    }
          //调用btOK按钮事件
      function xzqdbclick() {
          document.getElementById("<%=btxzqOK.ClientID%>").click();
      }

    var cscode = function () {
        document.getElementById("<%=btn.ClientID%>").click();
    }
</script>
