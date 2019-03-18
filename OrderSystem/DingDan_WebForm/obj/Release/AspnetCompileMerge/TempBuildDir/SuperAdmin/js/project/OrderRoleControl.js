var $ = layui.jquery,
	layer = layui.layer,
	table = layui.table,
	form = layui.form;
var alldata = [];
//用户表配置
var tb_user_options = {

	initSort: {
		field: 'loginname' //排序字段，对应 cols 设定的各字段名
			,
		type: 'asc' //排序方式  asc: 升序、desc: 降序、null: 默认排序
	},
	elem: "#tb_user",
	page: true,
	limit: 10,
	size: 'lg',
	even: true,
	data: [],
	cols: [
		[{
				field: 'loginname',
				title: '登录账号',
				width: 120,
				align: 'center'
			}, {
				field: 'groupid',
				title: '用户组ID',
				width: 100,
				edit: true,
				align: 'center'
			}, {
				field: 'user_Text',
				title: '姓名',
				width: 220,
				edit: true,
				align: 'center'
			}, {
				fixed: 'right',
				width: 150,
				align: 'center',
				toolbar: '#ToolBar'
			}

		]
	]
};

//用户组配置
var tb_group_options = {

	initSort: {
		field: 'groupid' //排序字段，对应 cols 设定的各字段名
			,
		type: 'asc' //排序方式  asc: 升序、desc: 降序、null: 默认排序
	},
	elem: "#tb_group",
	page: true,
	limit: 10,
	size: 'lg',
	even: true,
	data: [],
	cols: [
		[{
				field: 'groupid',
				title: '用户组ID',
				width: 100,
				align: 'center'
			}, {
				field: 'descraption',
				title: '用户组描述',
				width: 220,
				align: 'center'
			}, {
				field: 'methodids',
				title: '允许方法ID',
				width: 420,
				align: 'center'
			}, {
				fixed: 'right',
				width: 150,
				align: 'center',
				toolbar: '#ToolBar',
				align: 'center'
			}

		]
	]
};



//方法配置
var tb_method_options = {

	initSort: {
		field: 'methodgroupid' //排序字段，对应 cols 设定的各字段名
			,
		type: 'asc' //排序方式  asc: 升序、desc: 降序、null: 默认排序
	},
	elem: "#tb_method",
	//page: true,
	//limit: 15,
	height:600,
	size: 'lg',
	even: true,
	data: [],
	cols: [
		[{
				field: 'methodid',
				title: '方法ID',
				width: 120,
				align: 'center',
				sort:true
			}, {
				field: 'methodgroupid',
				title: '方法组ID',
				width: 120,
				templet: "#methodTpl",
				align: 'center',
				sort:true

			}, {
				field: 'methodname',
				title: '方法名',
				width: 220,
				 
				align: 'center'
			}, {
				field: 'descraption',
				title: '方法描述',
				width: 220,
				edit: true,
				align: 'center'
			}, {
				fixed: 'right',
				width: 150,
				align: 'center',
				toolbar: '#ToolBar',
				align: 'center'
			}



		]
	]
};
$(function() {

	GetAllRole()



})

//-----------------------------------------------------对user表操作开始-----------------------------------------------
//修改和删除
table.on('tool(tb_user)', function(obj) {
	if (obj.event == 'edit') {
		layer.confirm("确认要更新该条数据？", {
			icon: 3
		}, function() {
			layer.closeAll();
			$.ajax({
				url: "/handler/AdminHandler.ashx",
				type: "Post",
				dataType: "Json",
				data: {
					"Action": "ModifyRol",
					"tablename": "user",
					"event": "edit",
					"loginname": obj.data.loginname,
					"user_Text": obj.data.user_Text,
					"groupid": obj.data.groupid
				},
				success: function(res) {
					console.log(res)
					if (res.flag != 1) {
						layer.alert(res.message, {
							icon: 2
						})
						return false;
					};
					layer.alert("更新成功！", {
						icon: 1
					})
				}

			});
		})
	} else if (obj.event == 'del') {
		layer.confirm("确认要删除该条数据？", {
			icon: 3
		}, function() {
			layer.closeAll();

			$.ajax({
				url: "/handler/AdminHandler.ashx",
				type: "Post",
				dataType: "Json",
				data: {
					"Action": "ModifyRol",
					"tablename": "user",
					"event": "del",
					"loginname": obj.data.loginname

				},
				success: function(res) {
					console.log(res)
					if (res.flag != 1) {
						layer.alert(res.message, {
							icon: 2
						})
						return false;
					};
					layer.alert("删除成功！", {
						icon: 1
					})
					alldata = res.DataSet;
					tb_user_options.data = res.DataSet.user;
					table.render(tb_user_options);
				}

			});
		})
	}


})



//对user表操作,新增
$('#addUser').click(function() {
	var html = "";
	html += '<div style="width:95%;margin:10px auto"><form class="layui-form layui-form-pane">';
	html += '  <div class="layui-form-item"><label class="layui-form-label">登录账号</label><div class="layui-input-block">\
      <input type="text" name="loginname" lay-verify="title" autocomplete="off" placeholder="请输入系统登录账号" class="layui-input"></div></div>'
	html += '  <div class="layui-form-item"><label class="layui-form-label">用户姓名</label><div class="layui-input-block">\
      <input type="text" name="username" lay-verify="title" autocomplete="off" placeholder="请输入用户姓名" class="layui-input"></div></div>'
		// html+='  <div class="layui-form-item"  ><label class="layui-form-label">超级管理员</label><div class="layui-input-block"><input type="checkbox"     lay-skin="switch" lay-filter="switchTest" title="开|关"></div></div>'
	html += '  <div class="layui-form-item"><label class="layui-form-label">选择用户组</label><div class="layui-input-block">';
	html += '<select name="group" lay-filter="group"><option value  >选择用户组</option>'
	$.each(alldata.group, function(i, v) {
		html += ' <option value="' + v.groupid + '">' + v.descraption + '</option>'
	})
	html += '</select></div></div>'

	html += '</form></div>';

	layer.open({
		type: 1,
		title: '新增用户',
		area: ['500px', '350px'],
		content: html,
		btn: ["确定", '关闭'],
		success: function() {
			form.render();
		},
		btn1: function(index, layero) {
			if ($(layero).find("input[name=loginname]").val().trim() == "") {
				layer.alert("登录账号不能为空！", {
					icon: 2
				})
				return false;
			}
			if ($(layero).find("input[name=username]").val().trim() == "") {
				layer.alert("用户姓名不能为空！", {
					icon: 2
				})
				return false;
			}
			if ($(layero).find("select").val() == "") {
				layer.alert("还未选取权限！", {
					icon: 2
				})
				return false;
			}

			$.ajax({
				url: "/handler/AdminHandler.ashx",
				type: "Post",
				dataType: "Json",
				data: {
					"Action": "ModifyRol",
					"event": "add",
					'tablename': "user",
					'loginname': $(layero).find("input[name=loginname]").val().trim(),
					'username': $(layero).find("input[name=username]").val().trim(),
					'groupid': $(layero).find("select").val(),
				},
				success: function(res) {
					if (res.flag != 1) {
						layer.alert(res.message, {
							icon: 2
						})
						return false;
					} else {
						layer.alert('新增成功！', {
							icon: 1,
							closeBtn: 0
						}, function() {
							layer.closeAll();
						})
						alldata = res.DataSet;
						tb_user_options.data = res.DataSet.user;
						table.render(tb_user_options);

					}
				}
			});


		}
	})
});

//-----------------------------------------------------对user表操作结束-----------------------------------------------



//-----------------------------------------------------对group表操作开始-----------------------------------------------

//修改和删除
table.on('tool(tb_group)', function(obj) {
	if (obj.data.groupid==99) {
			layer.alert("不能操作超级管理员！", {
						icon: 2
					})
					return false;
	}
	if (obj.event == 'edit') {
		var d = {};
		$.each(alldata.method, function(i, v) {
			if (!d[v.methodgroupid]) {
				d[v.methodgroupid] = []
			}
			d[v.methodgroupid].push(v)

		})
		var html = "";
		html += '<div style="width:95%;margin:10px auto"><form class="layui-form layui-form-pane">';
		html += '  <div class="layui-form-item"><label class="layui-form-label">用户组ID</label><div class="layui-input-block">\
      <input type="text" name="groupid" lay-verify="title" autocomplete="off" placeholder="请输入用户组ID" class="layui-input"></div></div>'
		html += '  <div class="layui-form-item"><label class="layui-form-label">用户组描述</label><div class="layui-input-block">\
      <input type="text" name="descraption" lay-verify="title" autocomplete="off" placeholder="请输入用户组描述" class="layui-input"></div></div>'
		for (var n in d) {
			html += ' <div class="methods"><div class="layui-row ck">  <div class="layui-col-xs1">';
			html += '  <div style="text-align:center;margin:5px auto" class="ck_left"> <input type="checkbox" lay-filter="methodgroupcheck" name="methodgroupcheck" methodgroupid="' + d[n][0].methodgroupid + '" title="全选" lay-skin="primary"/></div></div>';
			html += '<div  class="layui-col-xs11 ck_right"><div class="layui-row" style="margin:5px auto">';
			$.each(d[n], function(i, v) {
				//html+='<div class="layui-col-xs4"><input type="checkbox" class="method"  methodid="'+v.methodid+'" methodgroupid="'+v.methodgroupid+'" title="'+v.descraption+'"></div>'
				html += '<div class="layui-col-xs4"><input lay-filter="methodcheck" type="checkbox" class="method"  methodid="' + v.methodid + '" methodgroupid="' + v.methodgroupid + '" title="' + v.descraption + '" lay-skin="primary"/></div>'

			})
			html += '</div></div></div></div>';
		}
		html += '</form></div>';

		layer.open({
			type: 1,
			title: '编辑用户组',
			area: ['1100px', '700px'],
			content: html,
			btn: ["确定", '关闭'],
			success: function(layero, index) {
				console.log(layero)
				console.log(obj.data.methodids)
				var methodids=obj.data.methodids.split('|');
			  layero.find('input[name=groupid]').val(obj.data.groupid);
			  layero.find('input[name=descraption]').val(obj.data.descraption);
				$.each($('.ck input[type=checkbox]'),function(i,v){

					if (methodids.indexOf($(v).attr('methodid'))>-1) {
						$(v).prop('checked',true);
					}
				})
				$.each(layero.find('.ck_left input[type=checkbox]'), function(i, v) {
					if (Number($(v).attr('methodgroupid')) >= 80) {
						$(v).parents('.ck_left').parent().css('background-color', 'pink');
					}
				})
				form.render();
			},
			btn1: function(index, layero) {
				if ($(layero).find("input[name=groupid]").val().trim() == "") {
					layer.alert("用户组ID不能为空！", {
						icon: 2
					})
					return false;
				}
				if ($(layero).find("input[name=descraption]").val().trim() == "") {
					layer.alert("用户组描述不能为空！", {
						icon: 2
					})
					return false;
				}
				if (layero.find('.method:checked').length == 0) {
					layer.alert("还未选取权限！", {
						icon: 2
					})
					return false;
				}
				var cks = layero.find('.method:checked');
				var methods = []
				$.each(cks, function(i, v) {
					methods.push($(v).attr('methodid'))
				});
				var methodids = methods.join('|')
				$.ajax({
					url: "/handler/AdminHandler.ashx",
					type: "Post",
					dataType: "Json",
					data: {
						"Action": "ModifyRol",
						"event": "edit",
						'tablename': "group",
						'groupid': $(layero).find("input[name=groupid]").val().trim(),
						'methodids': methodids,
						'descraption': $(layero).find("input[name=descraption]").val().trim()

					},
					success: function(res) {
						if (res.flag != 1) {
							layer.alert(res.message, {
								icon: 2
							})
							return false;
						} else {
							layer.alert('编辑成功！', {
								icon: 1,
								closeBtn: 0
							}, function() {
								layer.closeAll();
							})
							alldata = res.DataSet;
							tb_group_options.data = res.DataSet.group;
							table.render(tb_group_options);

						}
					}
				});


			}
		})
	} else if (obj.event == 'del') {
		layer.confirm("确认要删除该条数据？", {
			icon: 3
		}, function() {
			layer.closeAll();
			console.log(obj)

			$.ajax({
				url: "/handler/AdminHandler.ashx",
				type: "Post",
				dataType: "Json",
				data: {
					"Action": "ModifyRol",
					"tablename": "group",
					"event": "del",
					"groupid": obj.data.groupid
				},
				success: function(res) {
					console.log(res)
					if (res.flag != 1) {
						layer.alert(res.message, {
							icon: 2
						})
						return false;
					};
					layer.alert("删除成功！", {
						icon: 1
					})
					alldata = res.DataSet;
					tb_group_options.data = res.DataSet.group;
					table.render(tb_group_options);
				}

			});
		})
	}


})



//新增
$('#addGroup').click(function() {
	var d = {};
	$.each(alldata.method, function(i, v) {
		if (!d[v.methodgroupid]) {
			d[v.methodgroupid] = []
		}
		d[v.methodgroupid].push(v)

	})
	var html = "";
	html += '<div style="width:95%;margin:10px auto"><form class="layui-form layui-form-pane">';
	html += '  <div class="layui-form-item"><label class="layui-form-label">用户组ID</label><div class="layui-input-block">\
      <input type="text" name="groupid" lay-verify="title" autocomplete="off" placeholder="请输入用户组ID" class="layui-input"></div></div>'
	html += '  <div class="layui-form-item"><label class="layui-form-label">用户组描述</label><div class="layui-input-block">\
      <input type="text" name="descraption" lay-verify="title" autocomplete="off" placeholder="请输入用户组描述" class="layui-input"></div></div>'
	for (var n in d) {
		html += ' <div class="methods"><div class="layui-row ck">  <div class="layui-col-xs1">';
		html += '  <div style="text-align:center;margin:5px auto" class="ck_left"> <input type="checkbox" lay-filter="methodgroupcheck" name="methodgroupcheck" methodgroupid="' + d[n][0].methodgroupid + '" title="全选" lay-skin="primary"/></div></div>';
		html += '<div  class="layui-col-xs11 ck_right"><div class="layui-row" style="margin:5px auto">';
		$.each(d[n], function(i, v) {
			//html+='<div class="layui-col-xs4"><input type="checkbox" class="method"  methodid="'+v.methodid+'" methodgroupid="'+v.methodgroupid+'" title="'+v.descraption+'"></div>'
			html += '<div class="layui-col-xs4"><input lay-filter="methodcheck" type="checkbox" class="method"  methodid="' + v.methodid + '" methodgroupid="' + v.methodgroupid + '" title="' + v.descraption + '" lay-skin="primary"/></div>'

		})
		html += '</div></div></div></div>';
	}
	html += '</form></div>';

	layer.open({
		type: 1,
		title: '新增用户组',
		area: ['1100px', '700px'],
		content: html,
		btn: ["确定", '关闭'],
		success: function(layero, index) {
			console.log(layero)
			$.each(layero.find('.ck_left input[type=checkbox]'), function(i, v) {
				console.log($(v).attr('methodgroupid'))
				if (Number($(v).attr('methodgroupid')) >= 80) {
					$(v).parents('.ck_left').parent().css('background-color', 'pink');
				}
			})
			form.render();
		},
		btn1: function(index, layero) {
			if ($(layero).find("input[name=groupid]").val().trim() == "") {
				layer.alert("用户组ID不能为空！", {
					icon: 2
				})
				return false;
			}
			if ($(layero).find("input[name=descraption]").val().trim() == "") {
				layer.alert("用户组描述不能为空！", {
					icon: 2
				})
				return false;
			}
			if (layero.find('.method:checked').length == 0) {
				layer.alert("还未选取权限！", {
					icon: 2
				})
				return false;
			}

			console.log(layero.find('.method:checked').length)
			var cks = layero.find('.method:checked');
			var methods = []
			$.each(cks, function(i, v) {
				methods.push($(v).attr('methodid'))
			});
			var methodids = methods.join('|')
			$.ajax({
				url: "/handler/AdminHandler.ashx",
				type: "Post",
				dataType: "Json",
				data: {
					"Action": "ModifyRol",
					"event": "add",
					'tablename': "group",
					'groupid': $(layero).find("input[name=groupid]").val().trim(),
					'methodids': methodids,
					'descraption': $(layero).find("input[name=descraption]").val().trim()
				},
				success: function(res) {
					if (res.flag != 1) {
						layer.alert(res.message, {
							icon: 2
						})
						return false;
					} else {
						layer.alert('新增成功！', {
							icon: 1,
							closeBtn: 0
						}, function() {
							layer.closeAll();
						})
						alldata = res.DataSet;
						tb_group_options.data = res.DataSet.group;
						table.render(tb_group_options);

					}
				}
			});


		}
	})
});


//-----------------------------------------------------对group表操作结束-----------------------------------------------



//-----------------------------------------------------对method表操作开始-----------------------------------------------

//修改和删除
table.on('tool(tb_method)', function(obj) {
	if (obj.event == 'edit') {
		layer.confirm("确认要更新该条数据？", {
			icon: 3
		}, function() {
			layer.closeAll();
			$.ajax({
				url: "/handler/AdminHandler.ashx",
				type: "Post",
				dataType: "Json",
				data: {
					"Action": "ModifyRol",
					"tablename": "method",
					"event": "edit",
					"methodid": obj.data.methodid,
					"methodname": obj.data.methodname,
					"descraption": obj.data.descraption
				},
				success: function(res) {
					console.log(res)
					if (res.flag != 1) {
						layer.alert(res.message, {
							icon: 2
						})
						return false;
					};
					layer.alert("更新成功！", {
						icon: 1
					})
				}

			});
		})
	} else if (obj.event == 'del') {
		layer.confirm("确认要删除该条数据？", {
			icon: 3
		}, function() {
			layer.closeAll();
			console.log(obj)

			$.ajax({
				url: "/handler/AdminHandler.ashx",
				type: "Post",
				dataType: "Json",
				data: {
					"Action": "ModifyRol",
					"tablename": "method",
					"event": "del",
					"methodid": obj.data.methodid,
					"methodname": obj.data.methodname,
					"descraption": obj.data.descraption

				},
				success: function(res) {
					console.log(res)
					if (res.flag != 1) {
						layer.alert(res.message, {
							icon: 2
						})
						return false;
					};
					layer.alert("删除成功！", {
						icon: 1
					})
					alldata = res.DataSet;
					tb_method_options.data = res.DataSet.method;
					table.render(tb_method_options);
				}

			});
		})
	}


})



//新增
$('#addMethod').click(function() {
	var html = "";
	html += '<div style="width:95%;margin:10px auto"><form class="layui-form layui-form-pane">';
	html += '  <div class="layui-form-item"><label class="layui-form-label">方法ID</label><div class="layui-input-block">\
      <input type="text" name="methodid" lay-verify="title" autocomplete="off" placeholder="请输入方法ID" class="layui-input"></div></div>'
	html += '  <div class="layui-form-item"><label class="layui-form-label">方法组ID</label><div class="layui-input-block">\
      <input type="text" name="methodgroupid" lay-verify="title" autocomplete="off" disabled placeholder="自动生成" class="layui-input"></div></div>'
	html += '  <div class="layui-form-item"><label class="layui-form-label">方法名称</label><div class="layui-input-block">\
      <input type="text" name="methodname" lay-verify="title" autocomplete="off" placeholder="请输入方法名称" class="layui-input"></div></div>'
	html += '  <div class="layui-form-item"><label class="layui-form-label">方法描述</label><div class="layui-input-block">\
      <input type="text" name="descraption" lay-verify="title" autocomplete="off" placeholder="方法描述" class="layui-input"></div></div>'

	html += '</form></div>';

	layer.open({
		type: 1,
		title: '新增用户',
		area: ['500px', '350px'],
		content: html,
		btn: ["确定", '关闭'],
		success: function() {
			form.render();
		},
		btn1: function(index, layero) {
			if ($(layero).find("input[name=methodid]").val().trim() == "") {
				layer.alert("方法ID不能为空！", {
					icon: 2
				})
				return false;
			}
			if ($(layero).find("input[name=methodname]").val().trim() == "") {
				layer.alert("方法名称不能为空！", {
					icon: 2
				})
				return false;
			}
			if ($(layero).find("input[name=descraption]").val().trim() == "") {
				layer.alert("方法描述不能为空！", {
					icon: 2
				})
				return false;
			}


			$.ajax({
				url: "/handler/AdminHandler.ashx",
				type: "Post",
				dataType: "Json",
				data: {
					"Action": "ModifyRol",
					"event": "add",
					'tablename': "method",
					'methodid': $(layero).find("input[name=methodid]").val().trim(),
					'methodgroupid': $(layero).find("input[name=methodgroupid]").val().trim(),
					'methodname': $(layero).find("input[name=methodname]").val().trim(),
					'descraption': $(layero).find("input[name=descraption]").val().trim()

				},
				success: function(res) {
					if (res.flag != 1) {
						layer.alert(res.message, {
							icon: 2
						})
						return false;
					} else {
						layer.alert('新增成功！', {
							icon: 1,
							closeBtn: 0
						}, function() {
							layer.closeAll();
						})
						alldata = res.DataSet;
						tb_method_options.data = res.DataSet.method;
						table.render(tb_method_options);

					}
				}
			});


		}
	})
});



//-----------------------------------------------------对method表操作结束-----------------------------------------------



//获取所有权限列表
function GetAllRole() {
	$.ajax({
		url: "/handler/AdminHandler.ashx",
		type: "Post",
		dataType: "Json",
		data: {
			"Action": "GetAllRole"
		},
		success: function(res) {
			console.log(res)
			if (res.flag != 1) {
				layer.alert(res.message, {
					icon: 2
				})
				return false;
			};
			alldata = res.DataSet;
			tb_user_options.data = res.DataSet.user;
			tb_group_options.data = res.DataSet.group;
			tb_method_options.data = res.DataSet.method;

			table.render(tb_user_options);
			table.render(tb_group_options);
			table.render(tb_method_options);
		}
	});
}


//方法ID失去焦点时自动取前两位填写方法组ID
$(document).on("blur", "input[name=methodid]", function() {
	$("input[name=methodgroupid]").val($(this).val().trim().substr(0, 2))
})


//方法组全选时自动勾选当前方法组下面的所有方法
form.on('checkbox(methodgroupcheck)', function(data) {
	var methodgroupid = $(this).attr('methodgroupid');
	if (this.checked) {
		$(this).parents('.ck').find('.ck_right input[type=checkbox]').prop('checked', true);
	} else {
		$(this).parents('.ck').find('.ck_right input[type=checkbox]').prop('checked', false);
	}
	form.render();
})


//操作方法选择时，对全选按钮进行自动勾选
form.on('checkbox(methodcheck)', function(data) {
	var checked_l = $(this).parents(".ck_right").find("input[type=checkbox]:checked").length;
	var checkbox_l = $(this).parents(".ck_right").find("input[type=checkbox]").length;
	if (this.checked) {
		if (checkbox_l == checked_l) {
			$(this).parents('.ck').find('.ck_left input[type=checkbox]').prop('checked', true);
		}
	} else {
		$(this).parents('.ck').find('.ck_left input[type=checkbox]').prop('checked', false);
	}
	form.render();
})