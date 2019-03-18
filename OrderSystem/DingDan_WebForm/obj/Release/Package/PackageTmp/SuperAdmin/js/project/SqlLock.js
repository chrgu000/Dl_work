var laydate = layui.laydate,
    $ = layui.jquery,
    table = layui.table;
//表格配置
var tb_options = {
    elem: "#tb",
    limit: 1000,
    size: 'lg',
    data: [],
    cols: [
        [{
                title: "操作",
                fixed: 'left',
                width: 80,
                align: 'center',
                toolbar: '#ToolBar'
            }, {
                field: 'BlockingSessesionId',
                title: 'blockingid',
                width: 120,
                align: 'center',
            }, {
                field: 'BlockedSessionId',
                title: 'blockedid',
                width: 120,
                align: 'center',
            }, {
                field: 'BlockedSQLText',
                title: 'BlockedText',
                width: 120,
                align: 'center',
            }
, {
                field: 'BlockingSQLText',
                title: 'BlockingText',
                width: 120,
                align: 'center',
            }, {
                field: 'ProgramName',
                title: 'ProgramName',
                width: 140,
                align: 'center',
            }, {
                field: 'HostName',
                title: 'HostName',
                width: 140,
                align: 'center',
            }, {
                field: 'ClientIpAddress',
                title: 'ClientIpAddress',
                width: 140,
                align: 'center',
            }, {
                field: 'DatabaseName',
                title: 'DatabaseName',
                width: 140,
                align: 'center',
            }

        ]
    ]
};



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
 		success: function(res) {
 			if (res.flag != "1") {
 				errMsg(res.message);
 				return false;
 			}
 		  
            tb_options.data = res.LockTable;
            table.render(tb_options);
            layer.msg('加载成功')
 		}
 	});
 }

 

 table.on('tool(tb)',function(obj){
    if (obj.event==='del') {
        layer.confirm('确认要杀死它？',{icon:3},function(){
            KillLock(obj.data.BlockingSessesionId);
        })
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
 		success: function(res) {
 			if (res.flag != "1") {
 				errMsg(data.message);
 				return false;
 			}
 			 layer.alert('OK!',{icon:1,closeBtn:0},function(){
                 GetSqlLockList();
             });
            
 			
 		}
 	});
}

 

 