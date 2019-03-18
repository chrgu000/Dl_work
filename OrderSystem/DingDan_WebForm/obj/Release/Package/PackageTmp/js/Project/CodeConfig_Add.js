var $ = layui.jquery,
	table = layui.table,
	form = layui.form,
	laydate = layui.laydate;
var DataSet = {},
	hasCodes = [];

var cstcode = '00';
var iShowType = 1;
var isModify = 0;
var type;


//表格配置
var tb_options = {
	elem: "#tb",
	id: "tb",
	page: 0,
	limit: 2000,
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
	  type=GetQueryString('type')
	if (type === '1') {
		 
		 $('#select_product').remove();
		 $('#del').remove();
		 tb_options.page=1;
		 tb_options.limit=50;
		 tb_options.cols[0].shift();
		 

		 console.log(tb_options.cols)
		 
		 GetAllCodeConfigProducts();
	} else if (type === '2') {
		GetCodeConfig();
	}


	table.on('edit(tb)', function(obj) {
		$this = $(this);
		obj.data.cCusInvCode = obj.value.toLowerCase().trim();
		$this.val(obj.value.toLowerCase().trim());
		// if (isNaN(Number(obj.value))) {
		// 	obj.data[obj.field] = '';
		// 	$this.val('')
		// 	layer.msg('请输入数字');
		// 	return false;
		// }
		if (obj.value.toLowerCase().trim().length > 50) {
			errMsg('代码长度不能超过50');
			obj.data.cCusInvCode = '';
			$this.val('');
			return false;
		}
		$.each(hasCodes, function(i, v) {
			if (v.cCusInvCode.toLowerCase() == obj.value.toLowerCase()) {
				var s = '该用户编码已被以下产品占用:<br/>产品名称:' + v.cInvName + '<br/>规格:' + v.cInvStd + '<br/>单位组:' + v.UnitGroup + '<br/>请重新设置编码！'
				errMsg(s);
				obj.data.cCusInvCode = '';
				$this.val('');
				return false;
			}
		})


		$.each(tb_options.data, function(i, v) {
			if (v.cCusInvCode != null && obj.data.cInvCode != v.cInvCode && v.cCusInvCode.toLowerCase() == obj.value.toLowerCase()) {
				var s = '该用户编码已被本表中序号为' + (v.LAY_TABLE_INDEX + 1) + '的产品占用<br/>请重新设置编码！'
				errMsg(s);
				obj.data.cCusInvCode = '';
				$this.val('');
				return false;
			}
		})

	})



	//弹出产品分类选择页面
	$(document).on("click", "#select_product", function() {
		layer.open({
				type: 2,
				offset: '10px',
				area: ["1020px", "490px"],
				title: false,
				content: "/html/selectallproduct.html",
				success: function(layero, index) {},
				btn: ['确定'],
				btn1: function(index, layero) {

					table.render(tb_options);
					layer.closeAll();

				}

			})
			//layer.full(index);

	});



})


//获取已自定义编码的信息
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
			console.log(res)
			if (res.flag != 1) {
				layer.alert(res.message, {
					icon: 2
				})
				return false;
			}
			// $.each(res.table,function(i,v){
			// 	hasCodes.push(v.cCusInvCode);
			// })
			hasCodes = res.table;
		table.render(tb_options);

		}
	})
}

function GetAllCodeConfigProducts(){
	$.ajax({
	        type: "Post",
	        url: "../Handler/ProductHandler.ashx",
	        dataType: "Json",
	        data: { "Action": "GetCodeConfigProducts", "code": '02' },
	        success: function (res) {
	            if (res.flag != 1) {
	                layer.alert(res.message, { icon: 2 });
	                return false;
	            }
	            tb_options.data=res.Products;
	           table.render(tb_options);

	        }
	});
}


//保存列表
$(document).on('click', '#save', function() {
	if (tb_options.data.length == 0) {
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
	$.each(tb_options.data, function(i, v) {
		v.cCusInvCode = v.cCusInvCode.toLowerCase().trim();
	})
	$.ajax({
		traditional: true,
		type: "Post",
		url: "/Handler/ProductHandler.ashx",
		dataType: "Json",
		async: false,
		data: {
			"Action": "SaveCodeConfig",
			"type": type,
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
				location.href = 'CodeConfig.html'
			});



		}
	})
})



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
					c.push(v.cInvCode)
				});
				$.each(tb_options.data, function(i, v) {
					if ($.inArray(v.cInvCode, c) == -1) {
						d.push(v)
					}
				});
				tb_options.data = d;
			}
			table.render(tb_options);

			layer.closeAll();
		})
	}
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



$(document).on('click', '#selectProduct', function() {
	var cstcode = '00',
		kpdw = '999999',
		iShowType = '1';
	layer.open({
		type: 2,
		offset: '10px',
		area: ["1020px", "490px"],
		title: false,
		content: "selectAllproduct.html?cSTCode=" + cstcode + "&kpdw=" + kpdw + "&iShowType=" + iShowType,
		success: function(layero, index) {},
		btn: ['确定'],
		btn1: function(index, layero) {}
	})
})

//采用正则表达式获取地址栏参数 
function GetQueryString(name) {
	var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
	var r = window.location.search.substr(1).match(reg);
	if (r != null) return unescape(r[2]);
	return null;
}