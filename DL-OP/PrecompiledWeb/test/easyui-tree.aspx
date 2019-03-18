<%@ page language="C#" autoeventwireup="true" inherits="test_easyui_tree, dlopwebdll" enableviewstate="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
        <link href="../js/themes/icon.css" rel="stylesheet" />
    <link href="../js/themes/metro/easyui.css" rel="stylesheet" />
    <link href="../js/themes/mobile.css" rel="stylesheet" />
    <script src="../js/jquery.min.js"></script>
    <script src="../js/jquery.easyui.min.js"></script>
    <script src="../js/jquery.easyui.mobile.js"></script>
    <script src="../js/easyui-lang-zh_CN.js"></script>
</head>
    <script type="text/javascript">
        $(function () {
            var treeData = [{
                text: "我的会员", iconCls: "icon-dl_hy",
                children: [
                    {
                        text: "首页", iconCls: "icon-dl_zy",
                        attributes: {
                            url: "../DlDefault.aspx"
                        }
                    }, 
                {
                    text: "普通订单", iconCls: "icon-dl_hy",
                    children: [
                        {
                            text: "购物", iconCls: "icon-dl_gwc",
                            attributes: {
                                url: "../OrderFrame.aspx"
                            }
                        }]
                }
                ]}];
            document.getElementById("first").innerHTML = treeData.toString;
            //创建XMLHttpRequest对象
            var xmlHttp = new XMLHttpRequest();

            //配置XMLHttpRequest对象
            xmlHttp.open("get", "../json/test.ashx");
            //设置回调函数
            xmlHttp.onreadystatechange = function () {
                if (xmlHttp.readyState == 4 && xmlHttp.status == 200) {
                    document.getElementById("result").innerHTML = xmlHttp.responseText;

                    var json = eval('(' + xmlHttp.responseText + ')');
                    $('#tree').tree({
                        data: json
                    });
                }
            }
            //发送请求
            xmlHttp.send(null);


            $("#tree-2").tree({
                data: treeData,
                lines: true,
                onClick: function (node) {
                    if (node.attributes) {
                        Open(node.text, node.iconCls, node.attributes.url, node.url);
                    }
                }
            });



        });
    </script>
<body>
    <form id="form1" runat="server">
    <div>

    <div class="easyui-panel" style="padding:5px">
<%--        <ul id="tree" class="easyui-tree" ></ul>--%>
        		<ul class="easyui-tree" data-options="
					url:'../json/test.ashx',
					method:'get',animate:true,lines:true""></ul>
        <ul id="tree" class="easyui-tree" title="商品树" data-options="rownumbers:true,fitColumns:true,pagination:true,onClickRow:ClickRow,striped:true,singleSelect:true"></ul>
        <ul id="tree-2"></ul>
	</div>
<div></div>
    </div>
        <div id="result"></div>
        <div id="first"></div>
        <div class="easyui-datagrid" ></div>
    </form>
</body>
</html>
