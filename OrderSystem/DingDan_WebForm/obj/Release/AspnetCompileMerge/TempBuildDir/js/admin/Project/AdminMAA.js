var orders = {};
var layer = layui.layer,
	form = layui.form,
	element = layui.element,
	$ = layui.jquery,
	table = layui.table;


 
$(document).ready(function() {

	$.ajax({
		type: "Post",
		async: false,
		url: "/Handler/AdminHandler.ashx",
		dataType: "Json",
		data: {
			"Action": "GetAdminMAA",
			"iType": 1
		},
		success: function(res) {
			if (res["flag"] != 1) {
				layer.alert(res.message, {
					icon: 2
				});
				return false;
			}
		
			renderOrderTable(res["ordersTable"]);
			renderTimeTable(res["timeTable"]);
			renderCarType(res["carTypes"]);
			renderShippingMethod(res["shipping"]);
			form.render();
		}
	})

})

	$("#get_orders_ps").click(function() {
		get_orders("01");
		layer.msg("更新订单信息成功！", {
			icon: 1
		})
		$("#select_order_num").text('0');
		$("#select_order_weight").text('0吨');
	});


	$("#get_orders_zt").click(function() {
		get_orders("00");
		layer.msg("更新订单信息成功！", {
			icon: 1
		})
		$("#select_order_num").text('0');
		$("#select_order_weight").text('0吨');
	});


	$("#get_times").click(function() {

		get_times();
		layer.msg("更新预约时段成功！", {
			icon: 1
		})
	})



	//预约时间段删除列
	function delcol(col) {
		var l = $("#time_table tr").length;
		for (var i = 0; i < l; i++) {
			$("#time_table tbody tr").eq(i).find("td").eq(col).remove();
			$("#time_table").eq(i).find("th").eq(col).remove();
		}
	}



	//选择时间段
	$(document).on("click", ".en_time", function() {
		$(".en_time").removeClass("time_selected").text("可选");
		$(this).addClass('time_selected');
		$(this).text("已选")
	})



	//提交预约
	form.on('submit(submit)', function(data) {
		if ($("#time_table tbody").find(".time_selected").length == 0) {
			layer.msg("还未选择预约时段！", {
				icon: 2,
				anim: 6
			})
			return false;
		}

		if ($("#time_table tbody").find(".time_selected").length > 1) {
			layer.msg("你为什么能同时选择多个预约时段？？？", {
				icon: 2,
				anim: 6
			})
			return false;
		}
		var checkStatus = table.checkStatus('orderTable')

		if (checkStatus.data.length === 0) {
			layer.msg("还未选择预约订单！", {
				icon: 2,
				anim: 6
			})
			return false;
		}

		var $select_time = $("#time_table tbody").find(".time_selected");
		data.field["datDate"] = $select_time.parents("tr").find("td:eq(0)").text();
		data.field["datStartTime"] = $select_time.attr("datStartTime");
		data.field["datEndTime"] = $select_time.attr("datEndTime");
		data.field["cCode"] = $select_time.attr("ccode");
		data.field["cName"] = $select_time.attr("cname");
		var os = [];
		$.each(checkStatus.data , function(i, v) {
			os.push(v.lngopOrderId);
		})
		data.field["orders"] = os;
	 
			
		$.ajax({
				type: "Post",
				async: false,
				url: "/Handler/AdminHandler.ashx",
				dataType: "Json",
				data: {
					"Action": "DLproc_NewMAAOrderByIns",
					"info": JSON.stringify(data.field),
					"iType": 1
				},
				success: function(res) {
					if (res.flag != 1) {
						layer.alert(res.message, {
							icon: 2
						})
						return false;
					} else if (res.flag == "1") {
						layer.alert("预约成功<br>预约号：" + res.code, {
							icon: 1,
							closeBtn: 0
						}, function() {
							window.location.reload();
						})
					}
				}
			})
			// layer.alert(JSON.stringify(data.field), {
			// 	title: '最终的提交信息'
			// })
		return false;
	});



	//获取配送订单列表
	function get_orders(cSCCode) {
		$.ajax({
			type: "Post",
			async: false,
			url: "/Handler/AdminHandler.ashx",
			dataType: "Json",
			data: {
				"Action": "GetPSOrders","cSCCode":cSCCode
			},
			success: function(res) {
				if (res["flag"] != 1) {
					layer.alert(res.message, {
						icon: 2
					});
					return false;
				}
				renderOrderTable(res["ordersTable"]);
			}
		})

	}
 
	//渲染订单列表
	function renderOrderTable(orders_table) {
		var html = "";
		var arr = [];
		orders = {};
		$.each(orders_table, function(i, v) {
			if (!orders[v["lngopOrderId"]]) {
				orders[v["lngopOrderId"]] = []
				arr.push(v);
			}
			orders[v["lngopOrderId"]].push(v);
		})

		var cols = [
			[{
					checkbox: true,
					fixed: "left"
				},

				{
					field: "strBillNo",
					title: "订单编号",
					align: 'center',
					width: 140,
					sort:true
				}, {
					field: "ccuscode",
					title: "客户编码",
					align: 'center',
					width: 100,
					sort:true
				}, {
					field: "ccusname",
					title: "客户名称",
					align: 'center',
					width: 170,
					sort:true
				}, {
					field: "cdefine11",
					title: "配送地址",
					 width: 760
				}, {
					fixed: 'right',
					width: 100,
					align: 'center',
					toolbar: '#toolBar',
					title: "操作"
				},
			]
		]
		table.render({
			id: 'orderTable',
			elem: "#orderTable",
			page: false,
			checkbox: true,
			height: 500,
			limit: 15,
			//width: auto,
			size: 'lg',
			even: true,
			data: arr,
			cols: cols
		})



	}

	//查看订单详情
	table.on('tool(orderTable)', function(obj) {
		var lngopOrderId = obj.data.lngopOrderId
		var order_arr = orders[lngopOrderId];
		var title = order_arr[0]["strBillNo"] + "详情"
		var html = '<div style="width:95%;margin:0 auto"><table class="layui-table" style="max-height:500px;"><thead><th>序号</th><th>产品名称</th><th>产品规格</th><th>基本数量</th><th>包装结果</th></thead><tbody>';
		$.each(order_arr, function(i, v) {
			html += '<tr>';
			html += '<td>' + (i + 1) + '</td>'
			html += '<td>' + v["cinvname"] + '</td>'
			html += '<td>' + v["cInvStd"] + '</td>'
			html += '<td>' + v["iquantity"] + '</td>'
			html += '<td>' + v["cdefine22"] + '</td>'
			html += '</tr>';
		})
		html += '</tbody></table></div>';
		layer.open({
			area: ['670px', '400px'],
			shadeClose: true,
			type: 1,
			closeBtn: false,
			title: title,
			content: html
		})
	})

	//选取订单时，计算重量
	table.on('checkbox(orderTable)', function(obj) {
		var checkStatus = table.checkStatus('orderTable')
		var weight=0;
		$("#select_order_num").text(checkStatus.data.length)
		$.each(checkStatus.data, function(i, v) {
			$.each(orders[v["lngopOrderId"]], function(m, n) {
				var w = n.iInvWeight == null ? 0 : n.iInvWeight;
				weight += n.iInvWeight * n.iquantity;
			})
		})
	 	weight = (weight / 1000000).toFixed(4)
	 	$("#select_order_weight").text(Math.floor(weight) + " ~ " + Math.ceil(weight) + " 吨");
	})

	//获取时间段列表,并写入time_table
	function get_times() {

		$.ajax({
			type: "Post",
			async: false,
			url: "/Handler/ProductHandler.ashx",
			dataType: "Json",
			data: {
				"Action": "Get_MAATimes",
				"iType": 1
			},
			success: function(res) {
				if (res["flag"] != 1) {
					layer.alert(res.message, {
						icon: 2
					});
					return false;
				}
				renderTimeTable(res["timeTable"])
			}
		});
	}

	//渲染车型
	function renderCarType(cartypes) {
		var html = "<option value>请选择车型</option>";
		$.each(cartypes, function(i, v) {
			html += '<option value="' + v.cValue + '">' + v.cValue + '</option>'
		})
		$("#cartype").html(html);
		 
	}

	//渲染可预约时段列表
	function renderTimeTable(times_table) {
		var count1 = 0,
			count2 = 0,
			count3 = 0,
			count4 = 0,
			count5 = 0,
			count6 = 0,
			count7 = 0,
			count8 = 0,
			html = "";
		html += '<thead><tr><th>日期</th>';
		for (var i = 1; i < 9; i++) {
			html += '<th>' + times_table[0]["datStartTime" + i] + '~' + times_table[0]["datEndTime" + i] + '</th>';
		}

		html += '</tr></thead><tbody>';
		$.each(times_table, function(i, v) {
			count1 += v["iQty1"];
			count2 += v["iQty2"];
			count3 += v["iQty3"];
			count4 += v["iQty4"];
			count5 += v["iQty5"];
			count6 += v["iQty6"];
			count7 += v["iQty7"];
			count8 += v["iQty8"];

			html += "<tr>";
			html += '<td>' + v["datDate"] + '</td>'
			for (var i = 1; i < 9; i++) {
				html += '<td class="en_time" cCode="' + v["cCode" + i] + '" datStartTime="' + v["datStartTime" + i] + '" datEndTime="' + v["datEndTime" + i] + '" cName="' + v["cName" + i] + '">可选';
				html += '</td>'
			}

			html += "</tr></tbody>";
		})

		$("#time_table").html(html)
		var count = [count1, count2, count3, count4, count5, count6, count7, count8];
		var len = 0 - times_table.length;
		for (var i = 8; i > 0; i--) {
			if (count[i - 1] == len) {
				delcol(i);
			}
		}
		$("#time_table").removeClass('layui-hide')
		 
	}

function renderShippingMethod(shipping){
	var html = "<option value>请选择送货方式</option>";
		$.each(shipping, function(i, v) {
			html += '<option value="' + v.cSCCode + '">' + v.cSCName + '</option>'
		})
		$("#cSCCode").html(html);
		 
	 
}
 
 

 


	//预约时段tips
	$("#step1 .step-title span").hover(function() {
			layer.tips('<strong style="font-size:20px;text-align:center">预约时段说明:</strong><p><strong>【可选】</strong>：表明该单元格所对应的时段可以预约，点击选中你需要预约的时段。</p>\
			<p><strong>【不可选】</strong>：表明该时段预约已满或者未开启预约，不可选择。</p>\
			<p><strong>【更新预约时段】</strong>：点击可实时刷新时段可预约状态。</p>', $(this), {
				tips: [2, '#3595CC'],
				time: 1000000
			});
		},
		function() {
			layer.closeAll('tips');
		});


	//自提信息tips
	$("#step2 .step-title span").hover(function() {
			layer.tips('<strong style="font-size:20px;text-align:center">自提信息说明:</strong>\
            <p><strong>【注意】</strong>：<strong style="color:#ff4545;font-size:16px">成功预约的订单以此处填写的自提信息为准</strong>，订单上填写的自提信息会被视为无效。以后会逐步取消在新增订单时填写自提信息。</p>\
			<p>为了您的车辆能顺利进厂提货，此处请务必填写 <strong style="color:#ff4545;font-size:16px">真实、准确</strong>的信息。</p>', $(this), {
				tips: [2, '#3595CC'],
				time: 1000000
			});
		},
		function() {
			layer.closeAll('tips');
		});

	//自提订单tips
	$("#step3 .step-title span").hover(function() {
			layer.tips('<strong style="font-size:20px;text-align:center">自提订单说明:</strong>\
<p>多张订单可合并到一个预约号里。</p>\
			<p>订单勾选时会有合计重量显示，请注意不要超载。</p>', $(this), {
				tips: [2, '#3595CC'],
				time: 1000000
			});
		},
		function() {
			layer.closeAll('tips');
		});

	//车牌改大写
	$("input[name=cCarNumber]").blur(function(event) {
		$(this).val($(this).val().toUpperCase())
	});


	//点击按钮后去除焦点，避免按回车键时会重复触发该按钮
	$(document).on("click", ".layui-btn", function() {
		$(this).blur();
	})






$(document).ajaxStart(function() {
	layer.load();
}).ajaxComplete(function(request, status) {
	layer.closeAll('loading');
}).ajaxError(function(err) {
	console.log(err)
	layer.alert("加载页面出错，请联系管理员！", {
		icon: 2
	});
	layer.closeAll('loading');
});