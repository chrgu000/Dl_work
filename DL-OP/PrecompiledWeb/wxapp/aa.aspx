<%@ page language="C#" autoeventwireup="true" inherits="wxapp_aa, dlopwebdll" enableviewstate="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta name="viewport" content="initial-scale=1.0, maximum-scale=1.0, user-scalable=no">
    <title></title>
    <link href="../js/themes/icon.css" rel="stylesheet" />
    <link href="../js/themes/metro/easyui.css" rel="stylesheet" />
    <link href="../js/themes/mobile.css" rel="stylesheet" />
    <script src="../js/jquery.min.js"></script>
    <script src="../js/jquery.easyui.min.js"></script>
    <script src="../js/jquery.easyui.mobile.js"></script>
    <script src="../js/easyui-lang-zh_CN.js"></script>

      <style type="text/css">
        .subtotal { font-weight: bold; }/*合计单元格样式*/
    </style>

    <script type="text/javascript">

        $(document).ready(function () {
            $("#ComboBoxDateTime").combobox({
                onChange: function (n, o) {
                    //alert("我是老大!");
                    if ($('#ComboBoxDateTime').combobox('getText') == '自定义') {
                        document.getElementById('DateTime').style.display = 'block';
                    } else {
                        document.getElementById('DateTime').style.display = 'none';
                    }
                }
            });
            //$('#CusDateTimeBegin').datebox({
            //    formatter: function (date) { return date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate(); },
            //    parser: function (date) { return new Date(Date.parse(date.replace(/-/g, "/"))); }
            //});
            //$('#CusDateTimeEnd').datebox({
            //    formatter: function (date) { return date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate(); },
            //    parser: function (date) { return new Date(Date.parse(date.replace(/-/g, "/"))); }
            //});
        });
        function HideDateTime() {
            document.getElementById('DateTime').style.display = 'none';
        }

        function btn_click() {
            //创建XMLHttpRequest对象
            var xmlHttp = new XMLHttpRequest();
            //获取值
            var ccuscode = $('#ComboBoxccuscode').combobox('getValue');
            var datetype = $('#ComboBoxDateTime').combobox('getValue');
            var strFHStatus = $('#ComboBoxOrderStatus').combobox('getValue');
            var begin = $('#CusDateTimeBegin').combobox('getValue');
            var end = $('#CusDateTimeEnd').combobox('getValue');
            //配置XMLHttpRequest对象
            xmlHttp.open("get", "getorder.ashx?ccuscode=" + ccuscode + "&datetype=" + datetype + "&strFHStatus=" + strFHStatus + "&begin=" + begin + "&end=" + end);
            //设置回调函数
            xmlHttp.onreadystatechange = function () {
                if (xmlHttp.readyState == 4 && xmlHttp.status == 200) {
                    document.getElementById("code").innerHTML = ccuscode + ";" + datetype + ";" + strFHStatus + ";" + begin + ";" + end;
                    document.getElementById("result").innerHTML = xmlHttp.responseText;
                    document.getElementById("MyHiddenField").value = xmlHttp.responseText;
                    var json = eval('(' + xmlHttp.responseText + ')');
                    $('#dg').datagrid({
                        data: json
                    });
                }
            }
            //发送请求
            xmlHttp.send(null);
        }


        function onLoadSuccess() {
            //添加“合计”列
            $('#dg').datagrid('appendRow', {
                strBillNo: '<span class="subtotal">合计</span>'
            });
        }
        //指定列求和
        function compute(colName) {
            var rows = $('#dg').datagrid('getRows');
            var total = 0;
            for (var i = 0; i < rows.length; i++) {
                total += parseFloat(rows[i][colName]);
            }
            return total;
        }

    </script>

</head>
<body onload="HideDateTime()">

    <div class="easyui-navpanel" style="position: relative; padding: 20px">
        <header>
            <div class="m-toolbar">
                <div class="m-title">我的订单</div>
                <div class="m-right">
                    <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-search'" style="width: 80px" onclick="document.getElementById('searchcont').style.display = 'block';document.getElementById('searchcontBtn').style.display = 'block';">显示查询</a>
                </div>
                <div class="m-left">
                    <a href="javascript:void(0)" class="easyui-linkbutton" onclick="$('#ff').form('reset')" style="width: 60px">重置</a>
                </div>
            </div>
        </header>
        <form id="ff" runat="server">
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <asp:HiddenField ID="MyHiddenField" runat="server" />
            <asp:HiddenField ID="HFccuscode" runat="server" />
            <asp:HiddenField ID="HFUserId" runat="server" />

            <div id="searchcont" runat="server">
                <div>
                    <select class="easyui-combobox" runat="server" style="width: 100%;" editable="false"
                        name="ComboBoxccuscode" id="ComboBoxccuscode"
                        datatextfield="cCusName" datavaluefield="cCusCode">
                        <option value="010101" selected>010101</option>
                        <option value="010102">010102</option>
                        <option value="010103">010103</option>
                    </select>
                </div>

                <div>
                    <select class="easyui-combobox" runat="server" id="ComboBoxOrderStatus" name="ComboBoxOrderStatus" style="width: 100%;" editable="false">
                        <option value="1" selected>全部订单</option>
                        <option value="0">不包含未发货订单</option>

                    </select>
                </div>

                <div>
                    <%--                <label>日期</label>--%>
                    <select class="easyui-combobox" id="ComboBoxDateTime" name="ComboBoxDateTime" style="width: 100%;" editable="false" required="true"
                        onchange="upperCase(this.id)">
                        <option value="1" selected>今天</option>
                        <option value="2">昨天</option>
                        <option value="3">本周</option>
                        <option value="4">本月</option>
                        <option value="5">自定义</option>
                    </select>
                </div>

                <div id="DateTime">
                    <input class="easyui-datebox" runat="server" id="CusDateTimeBegin" name="CusDateTimeBegin" prompt="开始日期" data-options="editable:false,panelWidth:220,panelHeight:240,iconWidth:30" style="width: 100%">
                    <input class="easyui-datebox" runat="server" id="CusDateTimeEnd" name="CusDateTimeEnd" prompt="结束日期" data-options="editable:false,panelWidth:220,panelHeight:240,iconWidth:30" style="width: 100%">
                </div>
            </div>




            <div id="searchcontBtn" runat="server">

                <input type="button" value="查询" id="btn" onclick="btn_click();" />

            </div>
            <table id="dg" data-options="header:'#hh',singleSelect:false,border:false,fit:true,fitColumns:true,scrollbarSize:0,
                				singleSelect: false,
                fitColumns: true,
				rownumbers: true,
				showFooter: true"
                style="height: 200px">
                <thead>
                    <tr>
                        <th data-options="field:'ck',checkbox:true"></th>
                        <th data-options="field:'strBillNo',width:80" class="auto-style1">网单号</th>
                        <th data-options="field:'datBillTime',width:80" class="auto-style1">下单时间</th>
                        <%--<th data-options="field:'cdefine11',width:180,align:'right'" class="auto-style1">送货信息</th>--%>
                        <th data-options="field:'isum',width:80,align:'right'" class="auto-style1">订单金额</th>
                        <th data-options="field:'U8iFHMoney',width:80,align:'right'" class="auto-style1">发货金额</th>
                        <th data-options="field:'U8iTHMoney',width:80,align:'right'" class="auto-style1">退货金额</th>
                    </tr>
                </thead>
            </table>


            <script>
                //var data = [
                //    { "strBillNo": "FI-SW-01", "datBillTime": "Koi", "cdefine11": 10.00, "isum": "P", "U8iFHMoney": 36.50, "U8iTHMoney": "Large", "duoliana": "Large" }
                //];      
                <%--                function strToJson(str) {
                    var json = eval('(' + str + ')');
                    return json;
                }
                data = strToJson(document.getElementById("<%= MyHiddenField.ClientID %>").value)
                                $(function () {
                                    $('#dg').datagrid({
                                        data: data
                                    });
                                });
                                document.getElementById("Label2").innerHTML = data;--%>
            </script>
            <div id="code"></div>
            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
            <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
            <div id="result"></div>
        </form>
    </div>

    <iframe id="OrderCenter" height="0" width="0" frameborder="0" name="OrderCenter" src="OrderManagerDetails.aspx"></iframe>

    <style scoped>
        form label {
            display: block;
            margin: 10px .auto-style1;

        {

        {
            height: 20px;
        }
    </style>

</body>
</html>
