﻿var layer = layui.layer,
	orders = {};
	ztInfo=[];
var form = layui.form();
var element = layui.element();


//监听折叠


$(document).ready(function() {

	$.ajax({
		type: "Post",
		async: false,
		url: "/Handler/ProductHandler.ashx",
		dataType: "Json",
		data: {
			"Action": "Get_MAA","iType":1
		},
		success: function(res) {
			if (res["flag"] != 1) {
				layer.alert(res.message, {
					icon: 2
				});
				return false;
			}
			 
			renderOrderTable(res.DataSet["ordersTable"]);
			renderTimeTable(res.DataSet["timeTable"]);
			renderCarType(res.DataSet["carTypes"]);
			ztInfo=res.DataSet["ztInfo"];
			console.log(ztInfo);
		}
	})

})

	$("#get_orders").click(function() {
		get_orders();
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


	//订单列表全选按钮
	form.on("checkbox(all)", function(data) {

		if (data.elem.checked === true) {
			$("#order_table tbody tr").find("input[type=checkbox]").prop('checked', true)
		} else {
			$("#order_table tbody tr").find("input[type=checkbox]").prop('checked', false)
		}
		form.render();
		get_num_weight();
		$("#select_order_num").text($("#order_table tbody").find("input[type=checkbox]:checked").length)
	})


	//订单列表复选框
	form.on("checkbox(order)", function(data) {
		get_num_weight()
		if ($("#order_table tbody").find("input[type=checkbox]:checked").length === $("#order_table tbody tr").length) {
			$("#checkAll").prop("checked", true);

		} else {
			$("#checkAll").prop("checked", false);
		}
		form.render();

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

		if ($("#order_table tbody").find("input[type=checkbox]:checked").length === 0) {
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
		$.each($("#order_table tbody").find("input[type=checkbox]:checked"), function(i, v) {
			os.push($(v).parents("tr").find(".lngopOrderId").text());
		})
		data.field["orders"] = os;
		console.log(data)
		
		$.ajax({
			type: "Post",
			async: false,
			url: "/Handler/ProductHandler.ashx",
			dataType: "Json",
			data: {
				"Action": "DLproc_NewMAAOrderByIns","info":JSON.stringify(data.field),"iType":1
			},
			success:function(res){
				if (res.flag=="0") {
					layer.alert(res.message,{icon:2})
					return false;
				}
				else if(res.flag=="1"){
						layer.alert("预约成功<br>预约号："+res.code,{icon:1,closeBtn:0},function(){
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



	//获取订单列表
	function get_orders() {
		$.ajax({
			type: "Post",
			async: false,
			url: "/Handler/ProductHandler.ashx",
			dataType: "Json",
			data: {
			    "Action": "Get_MAAOrders" 
			},
			success: function(res) {
				if (res["flag"]!= 1) {
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
		var len=orders_table.length;
		if (orders_table.length > 0) {
			//for (var i = 0; i < len; i++) {
				//if (arr.length == 0) {
				//	arr.push(orders_table[i])
				//} else {
				//	if (arr[0]["lngopOrderId"] == orders_table[i]["lngopOrderId"]) {
				//		arr.push(orders_table[i]);
				//	} else {
				//		orders[arr[0]["lngopOrderId"]] = arr;
				//		arr = [];
				//		arr.push(orders_table[i]);
				//	}
				//}
				//if(i==len-1){
				//	orders[arr[0]["lngopOrderId"]] = arr
			    //}
			   
			//}
				
		    orders = {};
		    $.each(orders_table, function (i,v) {
		        if (!orders[v["lngopOrderId"]]) {
		            orders[v["lngopOrderId"]] = [];
		        }
		        orders[v["lngopOrderId"]].push(v)
		    })

			$.each(orders, function(i, v) {
				html += "<tr>";
				html += '<td> <input type="checkbox" lay-filter="order"   lay-skin="primary"></td>';
				html += '<td><input type="button" value="查看" class="layui-btn   layui-btn-mini show_detail"></td>';
				html += '<td class="strBillNo">' + v[0]["strBillNo"] + '</td>';
				html += '<td>' + v[0]["datCreateTime"].replace("T", " ") + '</td>';
				html += '<td>' + v[0]["ccusname"] + '</td>';
				html += '<td class="layui-hide lngopOrderId">' + v[0]["lngopOrderId"] + '</td>';
				html += "</tr>";
			})
		} else {
			html = '<tr><td colspan=5 style="text-align:center "><span >没有可预约的订单</span></td></tr>'
		}
		$("#order_table tbody").html(html);
		form.render();
	}

	//获取时间段列表,并写入time_table
	function get_times() {

		$.ajax({
			type: "Post",
			async: false,
			url: "/Handler/ProductHandler.ashx",
			dataType: "Json",
			data: {
				"Action": "Get_MAATimes","iType":1
			},
			success: function(res) {
				if (res["flag"] !=1) {
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
	function renderCarType(cartypes){
		var html="";
		$.each(cartypes,function(i,v){
			html+='<option value="'+v.cValue+'">'+v.cValue+'</option>'
		})
		$("#cartype").append(html);
		form.render();
	}

	//渲染可预约时段列表
	function renderTimeTable(times_table) {
		var count1 = 0,count2 = 0,count3 = 0,count4 = 0,count5 = 0,count6 = 0,count7 = 0,count8 = 0,html = "";
		html += '<thead><tr><th>日期</th>';
		for(var i=1;i<9;i++){
			html += '<th>' + times_table[0]["datStartTime"+i] + '~' + times_table[0]["datEndTime"+i] + '</th>';
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
			for(var i=1;i<9;i++){
					if (v['iQty'+i] > 0) {
				html += '<td class="en_time" cCode="' + v["cCode"+i] + '" datStartTime="' + v["datStartTime"+i] + '" datEndTime="' + v["datEndTime"+i] + '" cName="' + v["cName"+i] + '">可选';
				html += '</td>'
			} else {
				html += '<td class="dis_time">不可选';
				html += '</td>'
			}
			}
 
			html += "</tr></tbody>";
		})

		$("#time_table").html(html)
		var count = [count1, count2, count3, count4, count5,count6,count7,count8];
		var len = 0 - times_table.length;
		for (var i = 8; i > 0; i--) {
			if (count[i - 1] == len) {
				delcol(i);
			}
		}


		$("#time_table").removeClass('layui-hide')
		form.render();
	}


	//订单列表查看详细
	$(document).on("click", ".show_detail", function() {
		var id = $(this).parents("tr").find(".lngopOrderId").text()
		var order_arr = orders[id];
		var title = order_arr[0]["strBillNo"] + "详情"
		var html = '<div style="width:95%;margin:0 auto"><table class="layui-table" style="max-height:500px;"><thead><th>序号</th><th>产品名称</th><th>产品规格</th><th>基本数量</th><th>包装结果</th></thead><tbody>';
		console.log(order_arr)
		console.log(orders)
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

	//获取已选订单的数量和总重量，并写入orders_table
	function get_num_weight() {
		$("#select_order_num").text($("#order_table tbody").find("input[type=checkbox]:checked").length);

		var select_orders = $("#order_table tbody").find("input[type=checkbox]:checked");
		var weight = 0;
		$.each(select_orders, function(i, v) {
			$.each(orders[$(v).parents("tr").find(".lngopOrderId").text()], function(m, n) {
				var w = n.iInvWeight == null ? 0 : n.iInvWeight;
				weight += n.iInvWeight * n.iquantity;
			})
		})
		weight = (weight / 1000000).toFixed(4)
		$("#select_order_weight").text(Math.floor(weight) + " ~ " + Math.ceil(weight) + " 吨");
	}

	//查看已选择的订单弹窗
	$("#show_select_order").click(function() {
		var html = "";
		var select_orders = $("#order_table tbody").find("input[type=checkbox]:checked");
		if (select_orders.length == 0) {
			layer.msg("你还未选择订单", {
				icon: 2
			})
			return false;
		}
		html += ' <div class="layui-collapse" lay-accordion="" lay-filter="test">';
		$.each(select_orders, function(i, v) {
			html += '  <div class="layui-colla-item" ><h2 class="layui-colla-title" lngopOrderId="' + $(v).parents("tr").find(".lngopOrderId").text() + '">' + $(v).parents("tr").find(".strBillNo").text() + '</h2>';
			html += '<div class="layui-colla-content">' + $(v).parents("tr").find(".strBillNo").text();
			html += '</div></div>';
		})
		html += '</div>'

		layer.open({
			area: ['700px', '600px'],
			shadeClose: true,
			type: 0,
			btn: [],
			closeBtn: false,
			title: "已选择的订单",
			content: html,
			success: function() {
				element.init();
			}
		})
	})


	//点击折叠面板，加载订单详情
	element.on('collapse(test)', function(data) {
		var lngopOrderId = $(this).attr("lngopOrderId");
		var html = "";
		var html = '<div style="width:95%;margin:0 auto"><table class="layui-table" style="max-height:500px;"><thead><th>序号</th><th>产品名称</th><th>产品规格</th><th>基本数量</th><th>包装结果</th></thead><tbody>';
		$.each(orders[lngopOrderId], function(i, v) {
			html += '<tr>';
			html += '<td>' + (i + 1) + '</td>'
			html += '<td>' + v["cinvname"] + '</td>'
			html += '<td>' + v["cInvStd"] + '</td>'
			html += '<td>' + v["iquantity"] + '</td>'
			html += '<td>' + v["cdefine22"] + '</td>'
			html += '</tr>';
		})
		html += '</tbody></table></div>';
		$(this).parent().find(".layui-colla-content").html(html);
	});


	$("#get_ztinfo").click(function(){
		
		if (ztInfo.length==0) {
			layer.alert("收货地址里没有保存自提信息！",{icon:2})
			return false;
		}else{
			var html="";
			html+='<div style="width:95%;margin:15px auto">';
			html+='<div class="layui-inline"> \
                    <input type="text" id="search"  placeholder="输入搜索关键字" class="layui-input"></div>'
			html+='<table class="layui-table" id="ztInfo_table"><thead><tr><th>序号</th><th>自提车牌</th><th>司机姓名</th><th>司机电话</th><th>司机身份证</th></tr></thead><tbody>'
				$.each(ztInfo,function(i,v){
					html+='<tr><td>'+(i+1)+'</td>';
					html+='<td>'+v.strCarplateNumber+'</td>';
					html+='<td>'+v.strDriverName+'</td>';
					html+='<td>'+v.strDriverTel+'</td>';
					html+='<td>'+v.strIdCard+'</td></tr>';
		})
				html+="</tbody></table></div>";

		layer.open({
			area: ['700px', '400px'],
			shadeClose: true,
			type:1,
			btn: ["选择","关闭"],
			title: "自提信息",
			content: html,
			success: function() {
				 
			},
			btn1:function(){
				if ($("#ztInfo_table").find(".ztInfo_selected").length==0) {
					layer.alert("你还未选择自提信息！",{icon:2})
					return false;
				}else{
					var $tr=$("#ztInfo_table").find(".ztInfo_selected");
					$("#step2").find("input[name=cCarNumber]").val($tr.find("td:eq(1)").text());
					$("#step2").find("input[name=cDriver]").val($tr.find("td:eq(2)").text());
					$("#step2").find("input[name=cDriverPhone]").val($tr.find("td:eq(3)").text());
					$("#step2").find("input[name=cIdentity]").val($tr.find("td:eq(4)").text());
					layer.closeAll();
					layer.msg("自提信息已成功提取");
				}
			}
		})
		}
	
	
	})


//自提信息点击行时添加Class
$(document).on("click","#ztInfo_table tr",function(){
	$(this).siblings().removeClass('ztInfo_selected');
	$(this).addClass('ztInfo_selected');
})

	//预约时段tips
	$("#step1 .step-title span").hover(function () {
	    layer.tips('<strong style="font-size:20px;text-align:center">预约时段说明:</strong><p><strong>【可选】</strong>：表明该单元格所对应的时段可以预约，点击选中你需要预约的时段。</p>\
			<p><strong>【不可选】</strong>：表明该时段预约已满或者未开启预约，不可选择。</p>\
			<p><strong>【更新预约时段】</strong>：点击可实时刷新时段可预约状态。</p>', $(this), {
			    tips: [2, '#3595CC'],
			    time: 1000000
			});
	},
		function () {
		    layer.closeAll('tips');
		});


    //自提信息tips
	$("#step2 .step-title span").hover(function () {
	    layer.tips('<strong style="font-size:20px;text-align:center">自提信息说明:</strong>\
            <p><strong>【注意】</strong>：<strong style="color:#ff4545;font-size:16px">成功预约的订单以此处填写的自提信息为准</strong>，订单上填写的自提信息会被视为无效。以后会逐步取消在新增订单时填写自提信息。</p>\
			<p>为了您的车辆能顺利进厂提货，此处请务必填写 <strong style="color:#ff4545;font-size:16px">真实、准确</strong>的信息。</p>', $(this), {
			    tips: [2, '#3595CC'],
			    time: 1000000
			});
	},
    function () {
        layer.closeAll('tips');
    });

    //自提订单tips
	$("#step3 .step-title span").hover(function () {
	    layer.tips('<strong style="font-size:20px;text-align:center">自提订单说明:</strong>\
<p>多张订单可合并到一个预约号里。</p>\
			<p>订单勾选时会有合计重量显示，请注意不要超载。</p>', $(this), {
			    tips: [2, '#3595CC'],
			    time: 1000000
			});
	},
    function () {
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


//自提信息搜索
$(document).on("keyup","#search",function(){
	var val=$(this).val();
	$("#ztInfo_table tbody tr").addClass('layui-hide').removeClass('ztInfo_selected');
	$("#ztInfo_table tbody tr:contains("+val+")").removeClass('layui-hide');
})




    $(document).ajaxStart(function () {
        layer.load();
    }).ajaxComplete(function (request, status) {
        layer.closeAll('loading');
    }).ajaxError(function (err) {
        console.log(err)
        layer.alert("加载页面出错，请联系管理员！", { icon: 2 });
        layer.closeAll('loading');
    });
