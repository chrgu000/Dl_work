var $ = layui.jquery,
	table = layui.table,
	form = layui.form,
	laydate = layui.laydate;
var DataSet = {},
	codes = [];
var cstcode = '00';
var iShowType = 1;
var isModify = 0;



//表格配置
var tb_options = {
	elem: "#tb",
	id: "tb",
	page: 0,
	limit: 3000,
	height: 600,
	size: 'lg',
	//even: true,
	data: [],
	cols: [
		[{
				type: 'numbers',
				fixed: 'left',
				title: "序号"
			}, {
				type: 'checkbox',
				fixed: 'left'
			}, {
				field: 'cInvCode',
				title: 'ID',
				//width: 0,
				align: 'center',
				fixed: 'left'
			}, {
				field: 'cInvName',
				title: '名称',
				width: 170,
				align: 'center',
				fixed: 'left'
			}, {
				field: 'cInvStd',
				title: '规格',
				width: 120,
				align: 'center',
				fixed: 'left'
			}, {
				field: 'UnitGroup',
				title: '单位组',
				width: 120,
				align: 'center',
				templet: '#unitGroupTpl'
			},

			{
				field: 'cComUnitQTY',
				title: '基本数量',
				width: 100,
				align: 'center',
				edit: true
			}, {
				field: 'cComUnitName',
				title: '基本单位',
				width: 100,
				align: 'center',
			}, {
				field: 'cInvDefine2QTY',
				title: '小包装数量',
				width: 100,
				align: 'center',
				edit: true
			}, {
				field: 'cInvDefine2',
				title: '小包装单位',
				width: 100,
				align: 'center',
			}, {
				field: 'cInvDefine1QTY',
				title: '大包装数量',
				width: 100,
				align: 'center',
				edit: true
			}, {
				field: 'cInvDefine1',
				title: '大包装单位',
				width: 100,
				align: 'center',
			}, {
				field: 'cInvDefineQTY',
				title: '基本单位汇总',
				width: 110,
				align: 'center',
			}, {
				field: 'fAvailQtty',
				title: '库存量',
				width: 100,
				align: 'center',
			}, {
				field: 'pack',
				title: '包装结果',
				width: 150,
				align: 'center',
			}

			, {
				field: 'sum',
				title: '合计',
				width: 150,
				align: 'center',
			}
			// 	, {
			// 	field: 'ExercisePrice',
			// 	title: '单价',
			// 	width: 150,
			// 	align: 'center',
			// }
			// , {
			// 	field: 'iInvWeight',
			// 	title: '重量',
			// 	width: 150,
			// 	align: 'center',
			// }

		]
	]
};


$(function() {

	LoadBaseInfo();

	$(document).on("click", "#add", function() {
		console.log(tb_options.data)

	})


	table.on('edit(tb)', function(obj) {
		$this = $(this)
		if (isNaN(Number(obj.value))) {
			obj.data[obj.field] = '';
			$this.val('')
			layer.msg('请输入数字');
			//get_pack(obj);
			get_money();
			return false;
		}

		if (obj.field == 'cComUnitQTY') {
			if (obj.data.cComUnitName == '米') {
				if ((obj.value % obj.data.cinvdefine14).toFixed(2) != 0 && (obj.value % obj.data.cinvdefine14).toFixed(2) != obj.data.cinvdefine14) {
					// obj.data.cComUnitQTY = '';
					// $this.val('')
					// layer.msg('请输入' + obj.data.cinvdefine14 + '的整倍数');
					// return false;
					layer.msg('输入的不是' + obj.data.cinvdefine14 + '的整倍数');
				}

			} else {
				if (String(obj.value).indexOf(".") > -1) {
					obj.data.cComUnitQTY = '';
					$this.val('')
					layer.msg('请输入整数');
					// return false;
				}
			}
		} else if (obj.field == 'cInvDefine2QTY') {
			if (String(obj.value).indexOf(".") > -1) {
				obj.data.cInvDefine2QTY = '';
				$this.val('')
				layer.msg('请输入整数');
				// return false;
			}
		} else if (obj.field == 'cInvDefine1QTY') {
			if (String(obj.value).indexOf(".") > -1) {
				obj.data.cInvDefine1QTY = '';
				$this.val('')
				layer.msg('请输入整数');
				// return false;
			}
		}

		var a = get_pack(obj);
		$this.parents('tr').find('td[data-field="pack"]').html('<div class="layui-table-cell">' + a.data.pack + '</div>');
		$this.parents('tr').find('td[data-field="sum"]').html('<div class="layui-table-cell">' + a.data.sum + '</div>');
		$this.parents('tr').find('td[data-field="cInvDefineQTY"]').html('<div class="layui-table-cell">' + a.data.cInvDefineQTY + '</div>');
		get_money();
		table.render(tb_options);

	})



	//弹出产品分类选择页面
	$(document).on("click", "#select_product", function() {
		var kpdw = $("#TxtCustomer option:selected").val();
		if (kpdw == 0 || kpdw == "" || kpdw == 'undefined' || kpdw == null) {

			errMsg("请先选择开票单位!");
			return false;
		}

		codes.length = 0;
		$.each(tb_options.data, function(i, v) {
			codes.push(v.cInvCode)
		})
		layer.open({
				type: 2,
				offset: '10px',
				area: ["1020px", "490px"],
				title: false,
				content: "/html/select_product.html?cSTCode=" + cstcode + "&kpdw=" + kpdw + "&iShowType=" + iShowType,
				success: function(layero, index) {},
				btn: ['确定'],
				btn1: function(index, layero) {
					//var product_codes = tb_options.data; //获取购物清单里所有的产品ID
					var add_codes = [],
						c = [],
						d = [];
					//遍历清单数组，如果数组中的元素不在全局变量codes中，则删除此行
					$.each(tb_options.data, function(i, v) {
						c.push(v.cInvCode);
						if ($.inArray(v.cInvCode, codes) != -1) {
							d.push(v);
						}
					})
					tb_options.data = d;
					//遍历全局数组，如果数组中的元素不在清单数组product_codes中，则需要添加此产品
					$.each(codes, function(i, v) {
						if ($.inArray(v, c) == -1) {
							add_codes.push(v);
						}


					})
					if (add_codes.length > 0) {
						var areaid = "0";
						if ($("#zt").prop("checked") && $("#txtArea").val() != '' && $("#txtArea").val() != undefined) {
							areaid = $("#txtArea").val();
						}
						$.ajax({
							traditional: true,
							type: "Post",
							url: "/Handler/ProductHandler.ashx",
							dataType: "Json",
							async: false,
							data: {
								"Action": "DLproc_QuasiOrderDetailBySel_new",
								"codes": add_codes,
								"kpdw": kpdw,
								"isModify": isModify,
								"strBillNo": $("#strBillNo").text(),
								"areaid": areaid
							},
							success: function(res) {
								if (res.flag != 1) {
									errMsg(res.message);
									return false;
								}
								console.log(res)
								$.each(res.dt, function(i, v) {
									tb_options.data.push(v)
								})
								table.render(tb_options);
								// $("#buy_list tbody").append(get_html(data.dt));
								//   $("#buy_list tbody tr").find(".realqty").hide();
								layer.msg("添加商品成功！");
								layer.close(index);
								//   set_table_num("buy_list");
								//  set_table_color("buy_list");



							}
						})
					} else {
						table.render(tb_options);
						layer.closeAll();
					}
				}

			})
			//layer.full(index);

	});

	// //送货方式按钮切换
	// form.on('radio(shfs)', function(obj) {
	// 	if (obj.value == 2) {
	// 		var html = "<option value>请选择配送信息</option>";
	// 		$.each(DataSet.UserAddress_dt, function(i, v) {
	// 			if (v.iAddressType == 2) {
	// 				html += '<option value="' + v.lngopUseraddressId + '">' + v.strDistributionType + "," + v.strConsigneeName + "," + v.strConsigneeTel + "," + v.strDistrict + "," + v.strReceivingAddress + '</option>'
	// 			}
	// 		})
	// 		$("#txtAddress").html(html);
	// 		$("#shipping_check").parent().addClass('layui-hide');
	// 		$('#shipping_check').prop('checked', false);
	// 		$("#shipping_info").addClass('layui-hide');
	// 	} else {
	// 		var html = "<option value>请选择自提信息</option>";
	// 		$.each(DataSet.UserAddress_dt, function(i, v) {
	// 			if (v.iAddressType == 1) {
	// 				html += '<option value="' + v.lngopUseraddressId + '">' + v.strDistributionType + "," + v.strCarplateNumber + "," + v.strDriverName + "," + v.strDriverTel + "," + v.strIdCard + '</option>'
	// 			}
	// 		})
	// 		$("#txtAddress").html(html);
	// 		$("#shipping_check").parent().removeClass('layui-hide')
	// 		$('#shipping_check').prop('checked', false);
	// 		$("#shipping_info").addClass('layui-hide');

	// 	}
	// 	var html = "<option value>自提必须选择行政区</option>";
	// 	$.each(DataSet.Area_dt, function(i, v) {
	// 		html += '<option value="' + v.ccodeID + '">' + v.xzq + '</option>'
	// 	})
	// 	$("#txtArea").html(html)
	// 	form.render();
	// })

	//通过radio点击， 获取送货地址及行政区
	form.on('radio(shfs)', function(data) {
		if (data.value == '1') {
			$('#shipping_check').prop('checked', false);
			$('#shipping_info').addClass('layui-hide');
			$("#shipping_check").parent().removeClass('layui-hide');
			get_ztpsAddress(1);
		} else {
			$("#shipping_check").parent().addClass('layui-hide');
			$("#shipping_check").prop('checked', false);
			$("#shipping_info").addClass('layui-hide');
			get_ztpsAddress(2)
		}
		form.render();
	});


	//选择自提（托运），加载送货地址类型为3的地址
	form.on('checkbox(shipping_check)', function(data) {
		if (this.checked) {
			get_ztpsAddress(3);

		} else {
			var html = "<option value>请选择自提信息</option>";
			$.each(DataSet.UserAddress_dt, function(i, v) {
				if (v.iAddressType == 1) {
					html += '<option value="' + v.lngopUseraddressId + '">' + v.strDistributionType + "," + v.strCarplateNumber + "," + v.strDriverName + "," + v.strDriverTel + "," + v.strIdCard + '</option>'
				}
			})
			$("#txtAddress").html(html);
			$("#shipping_info").addClass('layui-hide')
		}
		form.render();
	})


	//获取地址信息
	function get_ztpsAddress(AddressType) {
		$.ajax({
			type: "Post",
			dataType: "Json",
			async: false,
			url: "/Handler/ProductHandler.ashx",
			data: {
				"Action": "GetAddressByType",
				"AddressType": AddressType
			},
			success: function(res) {
				if (res.flag != 1) {
					layer.alert(res.message, {
						icon: 2
					})
					return false;
				}
				var html = "";
				if (AddressType == 1) {
					html = "<option value>请选择自提地址</option>";
					$.each(res.DataSet.address_dt, function(i, v) {
						html += '<option   value="' + v.lngopUseraddressId + '">' + v.strDistributionType + ',' + v.strCarplateNumber + ',' + v.strDriverName + ',' + v.strDriverTel + v.strIdCard + '</option>'
					})
				} else if (AddressType == 2) {
					html = "<option value>请选择配送地址</option>";
					$.each(res.DataSet.address_dt, function(i, v) {
						html += '<option ccodeid="' + v.ccodeID + '" strDistrict="' + v.strDistrict + '" value="' + v.lngopUseraddressId + '">' + v.strDistributionType + ',' + v.strConsigneeName + ',' + v.strConsigneeTel + ',' + v.strDistrict + v.strReceivingAddress + '</option>'
					})
				} else if (AddressType == 3) {
					html = "<option value>请选择自提(托运)地址</option>";
					$.each(res.DataSet.address_dt, function(i, v) {
						html += '<option ccodeid="' + v.ccodeID + '" strDistrict="' + v.strDistrict + '" value="' + v.lngopUseraddressId + '">' + v.strDistributionType + ',' + v.strConsigneeName + ',' + v.strConsigneeTel + ',' + v.strDistrict + v.strReceivingAddress + '</option>'
					})
				}

				$("#txtAddress").html(html);
				if (AddressType == 1) {
					html = '<option value>自提必须选择行政区</option>';
					$.each(res.DataSet.area_dt, function(i, v) {
						html += '<option value="' + v.ccodeID + '">' + v.xzq + '</option>'
					})
				} else {
					html = '<option value>选择地址信息后自动更新</option>';
				}

				$('#txtArea').html(html);
			}
		});
	}



	//填写物流信息
	$(document).on('click', '#shipping_info', function() {
		layer.open({
			title: "填写托运信息",
			type: 1,
			area: ['500px', '290px'],
			content: '<div style="margin:20px auto;width:95%" ><form class="layui-form layui-form-pane"  ><div class="layui-form-item">\
                    <label class="layui-form-label">物流名称</label><div class="layui-input-block">\
                    <input type="text" id="companyName"   autocomplete="off" placeholder="请输入物流公司名称" class="layui-input"></div></div>\
                    <div class="layui-form-item"><label class="layui-form-label">收货人姓名</label>\
                    <div class="layui-input-block"><input type="text" id="contactName" autocomplete="off" placeholder="请输入收货人姓名" class="layui-input"></div></div>\
                    <div class="layui-form-item"><label class="layui-form-label">收货人电话</label>\
                    <div class="layui-input-block"><input type="text" id="contactPhone" autocomplete="off" placeholder="请输入收货人电话" class="layui-input"></div>\
                    </div></form></div>',
			btn: ['确定', '关闭'],
			success: function() {
				var info = $("#shipping_info").attr('info');
				if (info != "") {
					var info_arr = info.split('|');
					$("#companyName").val(info_arr[0]);
					$("#contactName").val(info_arr[1]);
					$("#contactPhone").val(info_arr[2]);

				}
			},
			btn1: function() {
				var i = $("#companyName").val().trim() + '|' + $("#contactName").val().trim() + '|' + $("#contactPhone").val().trim();
				if ($("#companyName").val().trim() != '' && $("#contactName").val().trim() != '' && $("#contactPhone").val().trim() != '') {
					$("#shipping_info").attr('info', i).removeClass('layui-btn-danger').text('已填写完');
				} else {
					errMsg('信息未填写完整');
					return false;
				}
				layer.closeAll();
			}
		});

	})

	//备注信息弹窗
	$(document).on("click", "#strRemarks", function() {
		layer.open({
			type: 1,
			title: "备注",
			area: ['400px', '400px'],
			btn: ['确定', '取消'],
			// content: "<textarea style='width:296px;height:182px' id='Remarks'>" + $("#strRemarks").val() + "</textarea>",
			content: "<div style='width:95%;margin:10px auto'><form class='layui-form layui-form-pane'><div class='layui-form-item'><label  class='layui-form-label'>字数统计</label><label id='Remarks_WordsCount' class='layui-form-label'>23/200</label></div><textarea class='layui-textarea' style='height:220px' id='Remarks'></textarea></form></div>",
			success: function(layero, index) {

				$("#Remarks").focus().val($("#strRemarks").val());
				$("#Remarks_WordsCount").text($("#Remarks").val().trim().length + '/200')
			},
			btn1: function() {
				if ($("#Remarks").val().trim().length > 200) {
					errMsg('备注不能超过200个字');
					return false;
				}
				$("#strRemarks").val($("#Remarks").val());
				layer.closeAll();
			},
		})
	})

})


//备注字数统计
$(document).on('keyup', '#Remarks', function() {
	var l = $("#Remarks").val().trim().length;
	$("#Remarks_WordsCount").text(l + '/200')

	if (l > 200) {
		$("#Remarks_WordsCount").css('color', "red");
	} else {
		$("#Remarks_WordsCount").css('color', "#000");
	}

})

//切换开票单位，更新数据
form.on('select(kpdw)', function(obj) {
	if (tb_options.data.length > 0) {
		Change_Kpdw();
	}
})

//切换行政区，更新数据
form.on('select(txtArea)', function(obj) {
	if (tb_options.data.length > 0 && $("#zt").prop("checked") && !$('#shipping_check').prop('checked') && obj.value != '' && obj.value != 0) {
		Change_Kpdw();
	}
})

//选择配送和自提托运，刷新价格以及自动填写行政区域
form.on('select(txtAddress)', function(obj) {

	if ($('#ps').prop('checked') || $('#shipping_check').prop('checked')) {
		var html = '<option value="' + $('#txtAddress option:selected').attr('ccodeid') + '">' + $('#txtAddress option:selected').attr('strdistrict') + '</option>';
		$('#txtArea').html(html);
		form.render();
	}

	if (tb_options.data.length == 0 || $('#TxtCustomer').val() == 0 || $('#TxtCustomer').val() == "" || $('#TxtCustomer').val() == undefined ||
		$("#TxtCustomer option:selected").val() == "" || ($('#zt').prop('checked') && !$('#shipping_check').prop('checked'))) {
		return false;
	}
	Change_Kpdw();

})



//切换开票单位或行政区，刷新价格、库存
function Change_Kpdw() {
	var c = []
	$.each(tb_options.data, function(i, v) {
		c.push(v.cInvCode);
	})
	var areaid = 0;
	if ($('#zt').prop('checked')) {
		if ($('#shipping_check').prop('checked')) {
			areaid = $('#txtAddress option:selected').attr('ccodeid');
		} else {
			areaid = $("#txtArea").val();
		}
	}
	$.ajax({
		traditional: true,
		type: "Post",
		dataType: "Json",
		url: "/Handler/ProductHandler.ashx",
		data: {
			"Action": "Change_KPDW_new",
			"kpdw": $("#TxtCustomer").val(),
			"codes": c,
			"areaid": areaid
		},
		success: function(res) {
			if (res.flag != 1) {
				errMsg(res.message);
				return false;
			}
			if (res.list_msg.length > 0) {
				var h = "你有" + res.list_msg.length + "件商品未找到<br />";
				var d = [];
				$.each(tb_options.data, function(i, v) {
					if ($.inArray(v.cInvCode, res.list_msg) != -1) {
						h += ((i + 1) + "、" + v.cInvName + "<br />");
					} else {
						d.push(v)
					}
				});
				h += '系统自动从购物单里删除';
				tb_options.data = d;
				layer.alert(h, {
					icon: 7
				});
			}

			$.each(tb_options.data, function(i, v) {
				$.each(res.list_dt, function(m, n) {
					if (v.cInvCode == n.code) {
						v.fAvailQtty = n.fAvailQtty;
						v.ExercisePrice = n.ExercisePrice;
						return;
					}
				})
			})
			get_money();
			table.render(tb_options);
		}
	});
}

$(document).on('click', '#submit_buy_list', function () {
	if ($('#txtAddress').val() == '' || $('#txtAddress').val() == 0) {
		errMsg('还未选择送货地址！');
		return false;
	}
	if ($('input[name=shfs]:checked').val() == undefined) {
		errMsg('送货方式不能为空！');
		return false;
	}
	if ($('input[name=shfs]:checked').val() == 1 && ($("#txtArea").val() == 0 || $("#txtArea").val() == '')) {
		errMsg('自提必须选择行政区！');
		return false;
	}
	if ($('#shipping_check').prop('checked') && $('#shipping_info').attr('info') == '') {
		errMsg('你选择了托运，但是没有输入物流信息！')
		return false;
	}
	if ($('#datDeliveryDate').val().trim() == '') {
		errMsg('提货时间不能为空！');
		return false;
	}
	if ($('#cdefine3').val() == '' || $('#cdefine3').val() == 0) {
		errMsg('还未选择车型！');
		return false;
	}
	if ($("#strLoadingWays").val().length > 100) {
		errMsg("装车方式不能超过100个字!");
		return false;
	}
	if ($("#strRemarks").val().length > 200) {
		errMsg("备注不能超过200个字!");
		return false;
	}
	if (tb_options.data.length == 0) {
		errMsg("你还未选择产品!");
		return false;
	}

	var flag = true;
	var c = [];
	$.each(tb_options.data, function(i, v) {
		if (v.cInvDefineQTY == 0 || v.cInvDefineQTY == undefined) {
			flag = false;
			c.push(v.cInvCode);
		}
	})
	console.log(c)
	if (!flag) {
		$.each($('.layui-table-body td'), function(i, v) {
			if ($(v).data('field') == 'cInvCode' && $.inArray(v.innerText, c) != -1) {
				$(v).parents('tr').addClass('NumberError');
			}
		})
		errMsg("订单里还有未输入数量的产品!");
		return false;
	}

	var HeadData = {},
		BodyData = [];

	HeadData.strRemarks = $("#strRemarks").val().trim();
	HeadData.ccuspperson = $('#TxtCustomer option:selected').attr('ccuspperson')
	HeadData.ccuscode = $('#TxtCustomer option:selected').val();
	HeadData.ccusname = $('#TxtCustomer option:selected').text();
	HeadData.cdefine3 = $("#cdefine3 option:selected").val();
	HeadData.lngopUseraddressId = $("#txtAddress option:selected").val();
	HeadData.strLoadingWays = $("#strLoadingWays").val().trim();
	HeadData.cdefine8 = $("#txtArea option:selected").text();
	HeadData.chdefine49 = $("#txtArea option:selected").val();
	HeadData.cdefine11 = $("#txtAddress option:selected").text();
	HeadData.datDeliveryDate = $('#datDeliveryDate').val();
	if ($("#zt").prop('checked')) {
		HeadData.cSCCode = '00';
		if ($('#shipping_check').prop('checked')) {
			HeadData.iAddressType = 3
			HeadData.chdefine21 = $('#shipping_info').attr('info');
		} else {
			HeadData.iAddressType = 1;
			HeadData.chdefine21 = ''
		}
	} else {
		HeadData.cSCCode = '01';
		HeadData.iAddressType = 2;
		HeadData.chdefine21 = '';
	}
	HeadData.cSTCode = '00';
	$.each(tb_options.data, function(i, v) {
		v.cComUnitQTY = (v.cComUnitQTY == undefined || v.cComUnitQTY == '') ? 0 : v.cComUnitQTY;
		v.cInvDefine1QTY = (v.cInvDefine1QTY == undefined || v.cInvDefine1QTY == '') ? 0 : v.cInvDefine1QTY;
		v.cInvDefine2QTY = (v.cInvDefine2QTY == undefined || v.cInvDefine2QTY == '') ? 0 : v.cInvDefine2QTY;
		v.UnitGroup = (v.cinvdefine13).toString() + v.cComUnitName + '=' + Math.round(v.cinvdefine13 / v.cinvdefine14).toString() + v.cInvDefine2 + '=1' + v.cInvDefine1;
	})
	HeadData.BodyData = tb_options.data;

	$.ajax({
		traditional: true,
		type: "Post",
		url: "/Handler/AdminHandler.ashx",
		dataType: "Json",
		data: {
			"Action": "DLproc_NewOrderByIns_Admin",
			"HeadData": JSON.stringify(HeadData)
				// "BodyData": JSON.stringify(BodyData)
		},
		success: function(res) {
			if (res.flag != 1) {
				errMsg(res.message);
				return false;
			}
			layer.alert("您的订单已提交成功,订单号为:<br />" + res.code, {
				icon: 1,
				closeBtn: 0
			}, function() {
				window.location.reload();

			})
		}
	});

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
			get_money();
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



//包装、计算基本单位汇总
function get_pack(obj) {
	//基本单位汇总
	obj.data.cInvDefineQTY = (Number(obj.data.cComUnitQTY == undefined ? 0 : obj.data.cComUnitQTY) + Number((obj.data.cInvDefine2QTY == undefined ? 0 : obj.data.cInvDefine2QTY) * obj.data.cinvdefine14) + Number((obj.data.cInvDefine1QTY == undefined ? 0 : obj.data.cInvDefine1QTY) * obj.data.cinvdefine13)).toFixed(2);
	//包装
	obj.data.pack = parseInt(obj.data.cInvDefineQTY / obj.data.cinvdefine13) + obj.data.cInvDefine1 +
		parseInt(floatDiv((floatMul(obj.data.cInvDefineQTY, 100) % floatMul(obj.data.cinvdefine13, 100)), floatMul(obj.data.cinvdefine14, 100))) + obj.data.cInvDefine2 +
		floatDiv((floatMul(obj.data.cInvDefineQTY, 100) % floatMul(obj.data.cinvdefine13, 100)) % floatMul(obj.data.cinvdefine14, 100), 100) + obj.data.cComUnitName;
	//合计
	obj.data.sum = (obj.data.cInvDefineQTY * obj.data.ExercisePrice).toFixed(2);
	// obj.update({sum:obj.data.sum,pack:obj.data.pack});
	return obj;
	//table.render(tb_options);
}
//计算总重量、计算总金额
function get_money() {

	var weight = money = 0;
	$.each(tb_options.data, function(i, v) {
		weight += v.cInvDefineQTY == undefined ? 0 : v.cInvDefineQTY * v.iInvWeight;
		money += v.cInvDefineQTY == undefined ? 0 : v.cInvDefineQTY * v.ExercisePrice;
		v.sum = (v.cInvDefineQTY == undefined ? 0 : v.cInvDefineQTY * v.ExercisePrice).toFixed(2);
	})
	$("#weight").text((weight / 1000000).toFixed(2) + "吨");
	$("#money").text(money.toFixed(2));
	//table.render(tb_options);

}

//加载普通订单页面
function LoadBaseInfo() {

	$.ajax({
		type: "Post",
		url: "/Handler/ProductHandler.ashx",
		dataType: "Json",
		async: false,
		data: {
			"Action": "GetAllBaseInfo"
		},
		success: function(res) {
			if (res.flag != "1") {

				errMsg(res.message);

				return false;
			} else {

				DataSet = res.DataSet;

				//加载产品列表
				table.render(tb_options)
					//加载开票单位
				var html = "";
				$.each(DataSet.Kpdw_dt, function(i, v) {
					html += '<option value="' + v.cCusCode + '" cCusPPerson="' + v.cCusPPerson + '">' + v.cCusName + '</option>'
				})
				$("#TxtCustomer").append(html);
				// $('#TxtCustomer').val('01010101');
				//加载车型
				html = "<option value>请选择车型</option>";
				$.each(DataSet.CarType_dt, function(i, v) {
					html += '<option value="' + v.cValue + '"  >' + v.cValue + '</option>'
				})
				$("#cdefine3").html(html);
				form.render();
				//日期控件
				laydate.render({
					elem: '#datDeliveryDate',
					istime: true,
					type: 'datetime',
					theme: 'molv',
					istoday: true
				});

			}
		},
		error: function(err) {
			layer.alert("获取数据失败,请重试或联系管理员!", {
				icon: 2
			});
			console.log(err);
		}
	});
}


//加    
function floatAdd(arg1, arg2) {
	var r1, r2, m;
	try {
		r1 = arg1.toString().split(".")[1].length
	} catch (e) {
		r1 = 0
	}
	try {
		r2 = arg2.toString().split(".")[1].length
	} catch (e) {
		r2 = 0
	}
	m = Math.pow(10, Math.max(r1, r2));
	return (arg1 * m + arg2 * m) / m;
}

//减    
function floatSub(arg1, arg2) {
	var r1, r2, m, n;
	try {
		r1 = arg1.toString().split(".")[1].length
	} catch (e) {
		r1 = 0
	}
	try {
		r2 = arg2.toString().split(".")[1].length
	} catch (e) {
		r2 = 0
	}
	m = Math.pow(10, Math.max(r1, r2));
	//动态控制精度长度    
	n = (r1 >= r2) ? r1 : r2;
	return ((arg1 * m - arg2 * m) / m).toFixed(n);
}

//乘    
function floatMul(arg1, arg2) {
	var m = 0,
		s1 = arg1.toString(),
		s2 = arg2.toString();
	try {
		m += s1.split(".")[1].length
	} catch (e) {}
	try {
		m += s2.split(".")[1].length
	} catch (e) {}
	return Number(s1.replace(".", "")) * Number(s2.replace(".", "")) / Math.pow(10, m);
}


//除   
function floatDiv(arg1, arg2) {
	var t1 = 0,
		t2 = 0,
		r1, r2;
	try {
		t1 = arg1.toString().split(".")[1].length
	} catch (e) {}
	try {
		t2 = arg2.toString().split(".")[1].length
	} catch (e) {}

	r1 = Number(arg1.toString().replace(".", ""));

	r2 = Number(arg2.toString().replace(".", ""));
	return (r1 / r2) * Math.pow(10, t2 - t1);
}