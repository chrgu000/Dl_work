 $(document).ready(function() {


 	GetSqlLockList();



 })

 function GetSqlLockList() {
 	$.ajax({
 		type: "Post",
 		url: "/Handler/AdminHandler.ashx",
 		dataType: "Json",
 		data: {
 			"Action": "GetSqlLockList"
 		},
 		success: function(data) {
 			if (data.flag != "1") {
 				ErrorMsgAlert(data.message);
 				return false;
 			}
 			console.log(data.LockTable)
 			var oTable_detail = new TableInit_Detail(columns, 'SqlLockList');
 			oTable_detail.Init();
 			if (data.LockTable.length > 0) {
 				$("#SqlLockList").bootstrapTable("load", data.LockTable);
 			} else {
 				$("#SqlLockList").bootstrapTable("removeAll");
 			}
 		}
 	});
 }

 $(document).on("click", "#refresh", function() {
 	GetSqlLockList();
 })

 $(document).on("click", ".showblockedsqltext", function() {
 	$("#SqlLockList").find("tr").removeClass("select")
 	$(this).parents("tr").addClass("select");
 	var index = $("#SqlLockList").find("tr.select").data("index");
 	var list = $("#SqlLockList").bootstrapTable('getData')[index];
 	bootbox.alert({message:list.BlockedSQLText,title:"BlockedSQLText",closeButton:false})

 })

  $(document).on("click", ".showblockingsqltext", function() {
 	$("#SqlLockList").find("tr").removeClass("select")
 	$(this).parents("tr").addClass("select");
 	var index = $("#SqlLockList").find("tr.select").data("index");
 	var list = $("#SqlLockList").bootstrapTable('getData')[index];
bootbox.alert({message:list.BlockingSQLText,title:"BlockingSQLText",closeButton:false})

 })

$(document).on("click",".kill",function(){
	if (confirm("确认要杀死它?")) {
	$("#SqlLockList").find("tr").removeClass("select")
 	$(this).parents("tr").addClass("select");
 	var index = $("#SqlLockList").find("tr.select").data("index");
 	var list = $("#SqlLockList").bootstrapTable('getData')[index];
 	var id=list.BlockingSessesionId;
 	KillLock(id);
	}

})


function KillLock(id){
	 	$.ajax({
 		type: "Post",
 		url: "/Handler/AdminHandler.ashx",
 		dataType: "Json",
 		data: {
 			"Action": "KillLock","id":id
 		},
 		success: function(data) {
 			if (data.flag != "1") {
 				ErrorMsgAlert(data.message);
 				return false;
 			}
 			else{
 				bootbox.alert({message:"我也不知道成功没有,自己刷新列表看看!",title:"成功",closeButton:false,
 				callback:function(){
 						GetSqlLockList();
 				}})
 			
 			}
 			
 		}
 	});
}

 var columns = [

 	{
 		field: 'BlockingSessesionId',
 		title: 'blockingid'
 	}, {
 		field: 'BlockedSessionId',
 		title: 'blockedid'
 	}, {
 		field: 'BlockedSQLText',
 		title: 'BlockedSQL',
 		formatter: function() {
 			return '<button class="btn showblockedsqltext" type="button">show</button>'
 		}
 	}, {
 		field: 'BlockingSQLText',
 		title: 'BlockingSQL',
 		formatter: function() {
 			return '<button class="btn  showblockingsqltext" type="button">show</button>'
 		}
 	},
 	{
 		field: null,
 		title: '操作',
 		formatter: function() {
 			return '<button class="btn btn-danger  kill" type="button">Kill</button>'
 		}
 	}

 ]


 //Bootstrap-Table 初始化参数
 var TableInit_Detail = function (columns, tableId) {
     var oTableInit_Detail = new Object();
     var d = []
     //初始化Table
     oTableInit_Detail.Init = function () {
         $('#' + tableId).bootstrapTable({
             data: d, //请求后台的URL（*）
             //  method: 'get',                      //请求方式（*）
             toolbar: '#toolbar', //工具按钮用哪个容器
             striped: true, //是否显示行间隔色
             cache: false, //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
             pagination: true, //是否显示分页（*）
             sortable: false, //是否启用排序
             sortOrder: "asc", //排序方式
             queryParams: oTableInit_Detail.queryParams, //传递参数（*）
             sidePagination: "client", //分页方式：client客户端分页，server服务端分页（*）
             pageNumber: 1, //初始化加载第一页，默认第一页
             pageSize: 30, //每页的记录行数（*）
             pageList: [30, 100, 200, 'All'], //可供选择的每页的行数（*）
             showExport: true, //是否显示导出
             exportDataType: "all", //basic', 'all', 'selected'.
             exportTypes: ["excel"],
             //  checkboxHeader: true,
             showColumns: true, //是否显示所有的列
             minimumCountColumns: 2, //最少允许的列数
             undefinedText: "", //当数据为 undefined 时显示的字符
             clickToSelect: true, //点击选中行
             //showRefresh: true,                    //显示刷新按钮
             //cardView:true,
             // showToggle: true,                          //切换卡模式
             detailView: true,                       //详细页面模式
             //height: 400,
             search: true,
             columns: columns,
             detailFormatter: function (index, row) {
                 var html = "";
                 html += "<p>ClientIpAddress:<strong>" + row.ClientIpAddress + "</strong></p>";
                 html += "<p>DatabaseName:<strong>" + row.DatabaseName + "</strong></p>";
                 html += "<p>HostName:<strong>" + row.HostName + "</strong></p>";
                 html += "<p>WaitDuration:<strong>" + row.WaitDuration + "</strong></p>";
                 html += "<p>WaitType:<strong>" + row.WaitType + "</strong></p>";
                 html += "<p>BlockingStartTime:<strong>" + row.BlockingStartTime.replace("T", " ") + "</strong></p>";
                 return html;
             }

         });
     };
     return oTableInit_Detail;
 };