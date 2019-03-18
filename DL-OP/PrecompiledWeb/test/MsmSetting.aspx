<%@ page language="C#" autoeventwireup="true" inherits="test_MsmSetting, dlopwebdll" enableviewstate="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../js/themes/icon.css" rel="stylesheet" />
    <link href="../js/themes/metro/easyui.css" rel="stylesheet" />
    <link href="../js/themes/mobile.css" rel="stylesheet" />
    <script src="../js/jquery.min.js"></script>
    <script src="../js/jquery.easyui.min.js"></script>
    <script src="../js/jquery.easyui.mobile.js"></script>
    <script src="../js/easyui-lang-zh_CN.js"></script>
    <style type="text/css">
        .auto-style1 {
            width: 246px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div style="float:left">
            <div class="easyui-panel" style="padding: 5px; width: 210px">
                <ul class="easyui-tree">
                    <li>
                        <span>消息设置</span>
                        <ul>
                            <li data-options="state:'opened'">
                                <span>功能模块</span>
                                <ul>
                                    <li>
                                        <span>登录验证码</span>
                                    </li>
                                    <li>
                                        <span>订单审核</span>
                                    </li>
                                    <li>
                                        <span>订单驳回</span>
                                    </li>
                                    <li>
                                        <span>订单发货</span>
                                    </li>

                                </ul>
                            </li>
                            <li data-options="state:'closed'">
                                <span>其他</span>
                                <ul>
                                    <li>1</li>
                                    <li>2</li>
                                    <li>3</li>
                                    <li>4</li>
                                </ul>
                            </li>
                        </ul>
                    </li>
                </ul>
            </div>
            </div>

            <div style="float:left">
           
            
</div>

            	<div class="easyui-tabs" style="width:700px;height:250px">
		<div title="About" style="padding:10px">
			 <div>
                <table id="dg" class="easyui-datagrid" title="选择手机号码" style="width: 300px; height: 300px"
                    data-options="rownumbers:true,singleSelect:true,url:'datagrid_data1.json',method:'get'">
                    <thead>
                        <tr>
                            <th data-options="field:'ck',checkbox:true"></th>
                            <th data-options="field:'PhoneNo',width:80" class="auto-style1">手机号码</th>
                        </tr>
                    </thead>
                </table>
            </div>
		</div>
		<div title="My Documents" style="padding:10px">
			<ul class="easyui-tree" data-options="url:'tree_data1.json',method:'get',animate:true"></ul>
		</div>
		
	</div>

        </div>
    </form>
</body>
</html>
