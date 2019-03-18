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
				field: 'lngSystemTimeControlId',
				title: 'ID',
				width: 70,
				align: 'center',
			}, {
				field: 'datBeginTime',
				title: '开始时间',
				width: 180,
				align: 'center',
				templet:'#startTimeTpl'
			}, {
				field: 'datEndTime',
				title: '结束时间',
				width: 180,
				align: 'center',
				templet: '#endTimeTpl'
			}


		]
	]
};



$(document).ready(function() {

	Get_ArrearList();


	laydate.render({
		elem: '.datepicker',
		type: 'datetime',
		range: '~',
		format: 'yyyy-M-d HH:mm:ss'
	});

	table.on("tool(tb)", function(obj) {
		console.log(obj);
		if (obj.event === 'del') {
			layer.confirm("确认要删除该条记录？", {
				icon: 3
			}, function(result) {

				var id = obj.data["lngSystemTimeControlId"];
				console.log(id)
				 
				$.ajax({
					type: "Post",
					url: "/Handler/AdminHandler.ashx",
					dataType: "Json",
					data: {
						"Action": "DelTimeControlList",
						"id": id
					},
					success: function(res) {
						if (res.flag != 1) {
							errMsg(res.message)
							return false;
						}
						obj.del();
						layer.alert("删除成功！", {
							icon: 1
						})
						Get_ArrearList();

					}
				});


			})
		}
	});



})



function Get_ArrearList() {
	$.ajax({
		type: "Post",
		url: "/Handler/AdminHandler.ashx",
		dataType: "Json",
		data: {
			"Action": "GetTimeControlList"
		},
		success: function(res) {
			if (res.flag != "1") {
				errMsg(res.message);
				return false;
			}
			tb_options.data = res.TimeTable;
			table.render(tb_options);
		}
	});
}

$('#add').click(function() {
	var html = '<div class="layui-form" style="margin:10px auto"><div class="layui-form-item">';
	html += '   <div class="layui-inline"><label class="layui-form-label">开始时间</label><div class="layui-input-inline">\
       <div id="startTime" style="height: 38px; line-height: 38px; cursor: pointer; border-bottom: 1px solid #e2e2e2;"></div></div></div>';
	html += '   <div class="layui-inline"><label class="layui-form-label">结束时间</label><div class="layui-input-inline">\
       <div id="endTime" style="height: 38px; line-height: 38px; cursor: pointer; border-bottom: 1px solid #e2e2e2;"></div></div></div>'

	html += '</div></div>';
	layer.open({
		type: 1,
		area: ['500px', '250px'],
		title: '新增控制时间段',
		content: html,
		success: function() {
			laydate.render({
				elem: '#startTime',
				type: 'datetime',
				format: 'yyyy-M-d HH:mm:ss',
				theme: 'molv'

			});
			laydate.render({
				elem: '#endTime',
				type: 'datetime',
				format: 'yyyy-M-d HH:mm:ss',
				theme: 'molv'

			});
		},
		shade: 0.6,
		btn: ["保存", "取消"],
		btn1: function() {
			if ($('#startTime').text() == '') {
				errMsg('开始时间不能为空！');
				return false;
			}
			if ($('#endTime').text() == '') {
				errMsg('结束时间不能为空！');
				return false;
			}
			if (Date.parse($('#startTime').text()) > Date.parse($('#endTime').text())) {
				errMsg('开始时间不能大于结束时间！');
				return false;
			}

 AddTimeControl();
		}
	})
})

 

function AddTimeControl() {
	$.ajax({
		type: "Post",
		url: "/Handler/AdminHandler.ashx",
		dataType: "Json",
		data: {
			"Action": "AddTimeControl",
			"start_date": $("#startTime").text(),
			"end_date": $("#endTime").text()
		},
		success: function(res) {
			if (res.flag != "1") {
				errMsg(res.message);
				return false;
			} else {
				layer.alert( "添加成功！",{icon:1,closeBtn:0},function(){
					Get_ArrearList();
					layer.closeAll();
				})
				
			}

		}
	});

}