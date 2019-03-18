var $ = layui.jquery,
	table = layui.table,
	form = layui.form,
	laydate = layui.laydate;


var form = layui.form,
	table = layui.table,
	layer = layui.layer,
	$ = layui.jquery;


//表格配置
var tb_options = {
	elem: "#tb",
	id: "tb",
	page: 0,
	limit: 5000,
	height: 600,
	size: 'lg',
	//even: true,
	data: [],
	cols: [
		[{
				checkbox: true,
				LAY_CHECKED: false,
				fixed: 'left'
			}, {
				field: '',
				title: '序号',
				width: 60,
				align: 'center',
				templet: '#idTpl',
			}, {
				field: 'cInvName',
				title: '名称',
				width: 300,
				align: 'center',
			}, {
				field: 'cInvStd',
				title: '规格',
				width: 200,
				align: 'center',
			}, {
				field: 'UnitGroup',
				title: '单位组',
				width: 150,
				align: 'center',

			},

			{
				field: 'cCusInvCode',
				title: '客户代码',
				width: 300,
				align: 'center',
				edit: true
			}


		]
	]
};


$(function() {

	GetCodeConfig();
})



//获取所有信息
function GetCodeConfig() {
	$.ajax({
		url: "../Handler/ProductHandler.ashx",
		data: {
			"Action": "GetCodeConfig"
		},
		dataType: "Json",
		type: "Post",
		async: false,
		success: function(res) {


			if (res.flag != 1) {
				layer.alert(res.message, {
					icon: 2
				})
				return false;
			}
			tb_options.data = res.table;
			table.render(tb_options);
		}
	})


	table.on('edit(tb)', function(obj) {
		$this = $(this);
	obj.data.cCusInvCode = obj.value.toLowerCase().trim();
			$this.val(obj.value.toLowerCase().trim());

			if (obj.value.toLowerCase().trim().length>50) {
				errMsg('代码长度不能超过50');
					obj.data.cCusInvCode = '';
				$this.val('');
				return false;
			}

		$.each(tb_options.data, function(i, v) {

			if (obj.data.cInvCode != v.cInvCode && v.cCusInvCode.toLowerCase() == obj.value.toLowerCase()) {
				var s = '该用户编码已被本表中序号为' + (v.LAY_TABLE_INDEX + 1) + '的产品占用<br/>请重新设置编码！'
				errMsg(s);
				$this.val('');
				obj.data.cCusInvCode = '';

				return false;
			}
		})
	})

}



//删除勾选项
$(document).on('click', '#del', function() {
	var ck = table.checkStatus('tb');
	if (ck.data.length > 0) {
		layer.confirm('确定要删除勾选项？', {
			icon: 3
		}, function() {
			if (ck.isAll) {
				tb_options.data = [];
			} else {
				var c = [];
				var d = [];
				$.each(ck.data, function(i, v) {
					c.push(v.ID)
				});
				$.each(tb_options.data, function(i, v) {
					if ($.inArray(v.ID, c) == -1) {
						d.push(v)
					}
				});
				tb_options.data = d;
			}
			table.render(tb_options);

			layer.closeAll();
			layer.alert('删除成功，请点击全部保存', {
				icon: 1
			})
		})
	}
})


//保存列表
$(document).on('click', '#save', function() {
	if (tb_options.data.length==0) {
		errMsg('列表中没有需要保存的数据!');
		return false;
	}
	b = true;
	$.each(tb_options.data, function(i, v) {


		if (v.cCusInvCode.trim() == '') {
			errMsg('列表中序号为' + (v.LAY_TABLE_INDEX + 1) + '的产品还未输入客户自定义编码！');
			b = false;
			return false;
		}
	})
	if (!b) {
		return false;
	}
	$.each(tb_options.data,function(i,v){
		v.cCusInvCode=v.cCusInvCode.toLowerCase().trim();
	})
	$.ajax({
		traditional: true,
		type: "Post",
		url: "/Handler/ProductHandler.ashx",
		dataType: "Json",
		async: false,
		data: {
			"Action": "SaveCodeConfig",
			"type": 1,
			"data": JSON.stringify(tb_options.data)
		},
		success: function(res) {
			console.log(res)
			if (res.flag != 1) {
				errMsg(res.message);
				return false;
			}


			//table.render(tb_options);
			layer.alert("自定义编码保存成功！", {
				icon: 1,
				closeBtn: 0
			}, function() {
				location.reload();
			});



		}
	})
})



//产品全局搜索功能
function searchall() {
	$this = $("#search_all");
	var txt = $this.val().trim(); //

	if ($('input:radio[name="shfs"]:checked').val() == '自提') {
		var $d = $("#tb tbody tr").filter(":contains('" + txt + "')");
	} else {
		var $d = $("#tb tbody tr").filter(":contains('" + txt + "')");
	}
	$d.show();
	$("#count").text($d.length);
}


$("#search_all").keyup(function(event) {
	searchall();
})



//JQ ajax全局事件
$(document).ajaxStart(function() {
	layer.load();
}).ajaxComplete(function(request, status) {
	setTimeout(t, 50)
		//  layer.closeAll('loading');
}).ajaxError(function() {
	layer.alert('程序出现错误,请重试或联系管理员!', {
		icon: 2
	})
	return false;
});



//错误提示
function errMsg(msg) {
	layer.alert(msg, {
		icon: 2,
		closeBtn: 0,
		anim: get_msgIndex()
	})
}

//获取动画随机数
function get_msgIndex() {
	return Math.floor((Math.random() * 7));
}

function t() {
	layer.closeAll('loading')
}