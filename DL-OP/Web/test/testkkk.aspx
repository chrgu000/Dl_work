<%@ Page Language="C#" AutoEventWireup="true" CodeFile="testkkk.aspx.cs" Inherits="testkkk" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        .auto-style1 {
            height: 25px;
        }
    </style>
</head>
<script type="text/javascript">
    var text = "";
    function Grid_BatchEditStartEditing(s, e) {
        //var strDriverTelColumn = s.GetColumnByField("strDriverTel");
        //if(!e.rowValues.hasOwnProperty(productNameColumn.index))
        //    return;
        //var cellInfo = e.rowValues[productNameColumn.index];
        //productNameTextBox.SetValue(cellInfo.value);
        //if(e.focusedColumn === productNameColumn)
        //    productNameTextBox.SetFocus();
        //alert("1");
        //var strDriverTelColumn = s.GetColumnByField("strDriverTel");
        //var text = e.rowValues[strDriverTelColumn.index];
        //strIdCard
        //s.GetRowKey(e.visibleIndex)//取得行号

    }
    function Grid_BatchEditEndEditing(s, e) {
        //var productNameColumn = s.GetColumnByField("ProductName");
        //if(!e.rowValues.hasOwnProperty(productNameColumn.index))
        //    return;
        //var cellInfo = e.rowValues[productNameColumn.index];
        //cellInfo.value = productNameTextBox.GetValue();
        //cellInfo.text = encodeHtml(productNameTextBox.GetText());
        //productNameTextBox.SetValue(null);
        //var strDriverTelColumn = s.GetColumnByField("strDriverTel");
        //var strIdCardColumn = s.GetColumnByField("strIdCard");
        //var cellInfo = e.rowValues[strDriverTelColumn.index];
        //cellInfo.value = strDriverNameColumn.GetValue();
        //strIdCardColumn.rowValues.index.value = "11";
        //strCarplateNumberTextBox.SetValue(cellInfo.value);

        //alert(cellInfo.value);
        //alert(s.GetFocusedRowIndex());
        var strCarplateNumberColumn = s.GetColumnByField("strCarplateNumber");
        var strDriverNameColumn = s.GetColumnByField("strDriverName");
        var strDriverTelColumn = s.GetColumnByField("strDriverTel");
        var cellInfoQTYa = e.rowValues[strCarplateNumberColumn.index];
        var cellInfoQTYb = e.rowValues[strDriverNameColumn.index];
        var cellInfoPrice =e.rowValues[strDriverTelColumn.index];
        var QTYPrice = (parseInt(cellInfoQTYa.value) + parseInt(cellInfoQTYb.value)) * parseInt(cellInfoPrice.value);
        //alert(cellInfoQTYa.value);
        if (cellInfoQTYa.value % 5 != 0) {
            //alert("请输入5的整倍数");
            //var rows1 = document.getElementById("gridzt_DXDataRow0");  //根据id找到这行
            //var cell1 = rows1.cells[1];//获取某行下面的某个td元素
            //var a1 = cell1.innerHTML;
            ////alert (a1);
            //var index11 = a1.indexOf(">");
            //var index21 = a1.indexOf("</div");
            //var b1 = a1.substring(index11 + 1, index21);
            //var c1 = a1.replace(b1, cellInfoQTYa.value);
            ////alert("style=\"display: none\"");
            ////alert(c1);
            //c1 = c1.replace("v style=\"display: none;\"", "v style=\"color:red\"");
            ////alert(a1); alert(b1); alert(c1); alert(cellInfoQTYa.value);
            //cell1.innerHTML = c1;
            ////alert(c1);
        }
        var rowsindex = "gridzt_DXDataRow" + s.GetFocusedRowIndex();
        var rows = document.getElementById("gridzt_DXDataRow0");  //根据id找到这行
        //for (var j = 0; j < rows.cells.length; j++)//取得第几行下面的td个数，再次循环遍历该行下面的td元素
        //{
        //    var cell = rows.cells[j];//获取某行下面的某个td元素
        //    //alert("第0行第" + (j + 1) + "格的数字是" + cell.innerHTML);//cell.innerHTML获取元素里头的值 
        //    var a =cell.innerHTML;
        //    var index1=a.indexOf(">");
        //    var index2 = a.indexOf("</div");
        //    var b = a.substring(index1 + 1, index2);
        //    var c = a.replace(b, QTYPrice);
        //    //alert(a.substring(index1+1,index2));
        //    //alert(index1 + "&" + index2+"&"+a);
        //}
        var cell = rows.cells[4];//获取某行下面的某个td元素
        //alert( cell.innerHTML);//cell.innerHTML获取元素里头的值 
        var a = cell.innerHTML;
        var index1 = a.indexOf(">");
        var index2 = a.indexOf("</div");
        var b = a.substring(index1 + 1, index2);
        var c = a.replace(b, QTYPrice);
        //alert(c);
        cell.innerHTML = c;

    }
    function Grid_NumberChanged(s, e) {
        //var strDriverNameColumn = s.GetColumnByField("strDriverTel");        
        //var cellInfo = e.rowValues[strDriverNameColumn.index];
        //alert(cellInfo.value);
    }


    function gettd() {
        var tb = document.getElementById("gridzt");  //根据id找到这个表格
        var rows = tb.rows;               //取得这个table下的所有行
        for (var i = 0; i < rows.length; i++)//循环遍历所有的tr行
        {
            for (var j = 0; j < rows[i].cells.length; j++)//取得第几行下面的td个数，再次循环遍历该行下面的td元素
            {
                var cell = rows[i].cells[j];//获取某行下面的某个td元素
                alert("第" + (i + 1) + "行第" + (j + 1) + "格的数字是" + cell.innerHTML);//cell.innerHTML获取元素里头的值      
            }
        }
    }
    function test() {
        alert("test");
    }
</script>
<body>
    <form id="form1" runat="server">
        <div>

            <br />

            <dx:ASPxGridView ID="gridzt" ClientInstanceName="gridzt" runat="server" AutoGenerateColumns="False" ClientIDMode="Static"
                KeyFieldName="lngopUseraddressId" Theme="Office2010Blue" OnRowUpdating="gridzt_RowUpdating" OnFocusedRowChanged="gridzt_FocusedRowChanged">
                <ClientSideEvents BatchEditStartEditing="Grid_BatchEditStartEditing" BatchEditEndEditing="Grid_BatchEditEndEditing" />
                <Columns>
                    <dx:GridViewCommandColumn ShowNewButtonInHeader="true" ShowEditButton="True" VisibleIndex="0" ShowUpdateButton="True">
                    </dx:GridViewCommandColumn>
                    <dx:GridViewDataTextColumn FieldName="strCarplateNumber" VisibleIndex="1" Caption="车牌号&数量1" >
                        <EditFormSettings VisibleIndex="0" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="strDriverName" VisibleIndex="2" Caption="司机姓名&数量2">
                        <PropertiesTextEdit>
                            <ValidationSettings CausesValidation="True" EnableCustomValidation="True" ErrorText="请输入正整数!" Display="Dynamic" SetFocusOnError="True">
                                <RegularExpression ValidationExpression="^[1-9]\d*$" ErrorText="请输入正整数!!" />
                            </ValidationSettings>
                        </PropertiesTextEdit>
                        <EditFormSettings VisibleIndex="1" />
                    </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataSpinEditColumn Caption="司机电话&单价" FieldName="strDriverTel" VisibleIndex="3" ReadOnly="true">
                            <propertiesspinedit allownull="False" displayformatstring="g" maxvalue="999999999999" numbertype="Integer">
                        </propertiesspinedit>
                            <editformsettings visibleindex="2" />
                    </dx:GridViewDataSpinEditColumn>
                    <dx:GridViewDataTextColumn Caption="司机身份证&总量" FieldName="strIdCard" VisibleIndex="4">
                        <EditFormSettings VisibleIndex="3" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataCheckColumn VisibleIndex="5" Caption="是否默认" Visible="false">
                        <EditFormSettings VisibleIndex="4" />
                    </dx:GridViewDataCheckColumn>
                </Columns>
                <SettingsEditing EditFormColumnCount="4" Mode="Batch" />
                <SettingsText CommandCancel="取消" CommandUpdate="确定" />
                <SettingsBehavior AllowSort="False" AllowFocusedRow="True" />
                <SettingsPager Mode="ShowAllRecords" />
                <Settings ShowTitlePanel="true" />
                <SettingsText Title="用户自提信息编辑" />
            </dx:ASPxGridView>
<table id="tb1" width="200" border="1" cellpadding="4" cellspacing="0">
        <tr>
                <td >第一行</td>
        </tr>
        <tr>
                <td  >第二行</td>
        </tr>
        <tr>
                <td ">第三行</td>
        </tr>
        <tr>
                <td >第四行</td>
        </tr>
		 <tr>
                <td class="auto-style1">
                    <input type="button" name="getTableContent" value="获得表格内容" onclick="gettd()" /></td>
        </tr>
</table>
        </div>
    </form>
</body>
</html>
