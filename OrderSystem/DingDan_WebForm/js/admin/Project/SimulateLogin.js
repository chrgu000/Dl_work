var $ = layui.jquery,
	form = layui.form,
	MainCode, SubCode;

$(function() {

	//获取所有客户编码
	$.ajax({
		type: "Post",
		url: "/Handler/AdminHandler.ashx",
		dataType: "Json",
		async: false,
		data: {
			"Action": "GetAllUserCode",
		},
		success: function(res) {
			console.log(res)
			if (res.flag != "1") {
				errMsg(res.message);
				return false;
			}
			MainCode = res.MainCode;
			SubCode = res.SubCode;
			var html = [];
			html.push('<option value="">直接选择或搜索选择</option>');
			$.each(res.MainCode, function(i, v) {
				html.push('<option value="' + v.cCusCode + '" phone="' + v.ccusphone + '">' + v.cCusCode + '---' + v.cCusName + '</option>');
			})
			$('#select_MainCode').html(html.join(''));
			form.render();

		}
	});

	//主账号选择
	form.on('select(MainCode)', function(data) {
		var d = data.value;
		if (d != '') {
		console.log(data);
			var c = [];
			$.each(SubCode, function(i, v) {
				if (d == v.ccuscode) {
					c.push(v);
				}
			});
			if (c.length != 0) {
				var html = [];
				html.push('<option value="">必须选择一个子账号</option>');
				
				html.push('<option value="' + d + '" phone="'+$(data.elem).find('option:selected').attr('phone')+'">' + d + '</option>');
				$.each(c, function(i, v) {
					html.push('<option value="' + v.strAllAccount + '" phone="'+v.strSubPhone+'">' + v.strAllAccount + '</option>');
				})
				$('#select_SubCode').html(html.join(''));
				form.render();
				$('#SubCode_div').removeClass('layui-hide');
				$('#login').attr('disabled', true).addClass('layui-btn-disabled');
			} else {
				$('#SubCode_div').addClass('layui-hide');
				$('#login').attr('disabled', false).removeClass('layui-btn-disabled');
			}
		}else{
			$('#login').attr('disabled', true).addClass('layui-btn-disabled');
			$('#SubCode_div').addClass('layui-hide');
		}


	})

	//子账号选择
	form.on('select(SubCode)', function(data) {
		var d = data.value;
		if (d != '') {
			$('#login').attr('disabled', false).removeClass('layui-btn-disabled');
		}else{
			$('#login').attr('disabled', true).addClass('layui-btn-disabled');
		}
	})


	//登录按钮
	$("#login").click(function() {
		if (!$('#SubCode_div').hasClass('layui-hide')&&$('#select_SubCode').val()=="") {
			errMsg('该账号有多个子账号，必须选择其中一个！');
		}
		var ccuscode, phone;
		ccuscode = $("#select_MainCode").val();
		if ($('#SubCode_div').hasClass('layui-hide')) {
		  
		    phone = $("#select_MainCode").find("option:selected").attr("phone");
		} else {
		  //  ccuscode = $("#select_SubCode").val();
		    phone = $("#select_SubCode").find("option:selected").attr("phone");
		}
		console.log(ccuscode);
		console.log(phone)
		$.ajax({
		    type: "Post",
		    url: "/Handler/AdminHandler.ashx",
		    dataType: "Json",
		    async: false,
		    data: {
		        "Action": "SimulateLogin",
		        "phone": phone,
		        'ccuscode': ccuscode
		    },
		    success: function (res) {
		        if (res.flag != 1) {
		            errMsg(res.message);
		            return false;
		        } else if (res.flag == '1') {
		            layer.msg("登录成功");
		            window.location.href = 'adminbuyorder.html'
		        }


		    },
		    error: function (err) {
		        layer.alert("获取数据失败,请重试或联系管理员!", {
		            icon: 2
		        });
		        console.log(err);
		    }
		});
		//$.ajax({
		//	type: "Post",
		//	url: "/Handler/AdminHandler.ashx",
		//	dataType: "Json",
		//	async: false,
		//	data: {
		//		"Action": "SimulateLogin",
		//		"phone": phone,
		//		//'type': type,
		//		'ccuscode': ccuscode
		//	},
		//	success: function(res) {
		//		if (res.flag == "0") {
		//			errMsg(res.message);
		//			return false;
		//		} else if (res.flag == '2') {
		//			console.log(res);
		//			var html = "<option value lay-search>请选择</option>";
		//			$.each(res.users, function(i, v) {
		//				html += '<option value="' + v.strLoginName + '">' + v.strLoginName + '  ' + v.strUserName + '</option>';
		//			})
		//			$('#select_user').html(html);
		//			form.render();
		//			$('#select_user_div').removeClass('layui-hide');
		//			$('#login').attr('user', 'more');
		//		} else if (res.flag == '1') {
		//			window.location.href = 'adminbuyorder.html'
		//		}


		//	},
		//	error: function(err) {
		//		layer.alert("获取数据失败,请重试或联系管理员!", {
		//			icon: 2
		//		});
		//		console.log(err);
		//	}
		//});
		// var phone = $("#phone").val().trim();
		// if (!(/^1[34578]\d{9}$/.test(phone))) {
		// 	errMsg("手机号码有误，请重填");
		// 	return false;
		// }

		// if ($('#login').attr('user') == 'more' && $('#select_user').val() == '') {
		// 	errMsg('该手机号码绑定有多个账户，必须先选择一个再登录！');
		// 	return false;
		// }
		// var type = 1,
		// 	ccuscode = '';
		// if ($('#login').attr('user') == 'more') {
		// 	type = 2;
		// 	ccuscode = $('#select_user').val();
		// }

		// $.ajax({
		// 	type: "Post",
		// 	url: "/Handler/AdminHandler.ashx",
		// 	dataType: "Json",
		// 	async: false,
		// 	data: {
		// 		"Action": "SimulateLogin",
		// 		"phone": phone,
		// 		'type': type,
		// 		'ccuscode': ccuscode
		// 	},
		// 	success: function(res) {
		// 		if (res.flag == "0") {
		// 			errMsg(res.message);
		// 			return false;
		// 		} else if (res.flag == '2') {
		// 			console.log(res);
		// 			var html = "<option value lay-search>请选择</option>";
		// 			$.each(res.users, function(i, v) {
		// 				html += '<option value="' + v.strLoginName + '">' + v.strLoginName + '  ' + v.strUserName + '</option>';
		// 			})
		// 			$('#select_user').html(html);
		// 			form.render();
		// 			$('#select_user_div').removeClass('layui-hide');
		// 			$('#login').attr('user', 'more');
		// 		} else if (res.flag == '1') {
		// 			window.location.href = 'adminbuyorder.html'
		// 		}


		// 	},
		// 	error: function(err) {
		// 		layer.alert("获取数据失败,请重试或联系管理员!", {
		// 			icon: 2
		// 		});
		// 		console.log(err);
		// 	}
		// });
	})
})


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