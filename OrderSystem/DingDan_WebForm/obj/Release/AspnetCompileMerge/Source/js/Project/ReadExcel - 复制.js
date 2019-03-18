var X = XLSX;
var ExcelData;
var XW = {};



// 弹出上传窗口
$(document).on("click", "#Dao", function() {

    if ($("#TxtCustomer option:selected").val() == 0) {
        layer.msg("请选择开票单位！");
        return false;
    }


    var xlf = document.getElementById('uploadExcel');
     if (!xlf.addEventListener) return;
     function handleFile(e) {
         read_Excel(e.target.files);
         //xlf.value = '';
     }
     xlf.addEventListener('change', handleFile, false);
     var html = '<div style="width:80%;margin:10px auto"><div>\
    <a href="javascript:;" class="file layui-btn">导入Excel<input type="file" name="" id="uploadExcel"></a></div></div>';
     layer.open({
         type: 1,
         title: '导入Excel',
         area: ['500px', '400px'],
         content: html
     })
})

$(function() {

	$(document).on('click', '#uploadExcel', function() {
		if ($("#TxtCustomer option:selected").val() == 0) {
			layer.msg("请选择开票单位！");
			return false;
		}
	})

	$(document).on('change', '#uploadExcel', function(event) {

		console.log(event.target.files[0].type)
		var f = event.target.files[0];
		var FileExt = f.name.substr(f.name.lastIndexOf('.')).toLowerCase()
		if (FileExt != '.xls' && FileExt != '.xlsx') {
			layer.alert('只能上传类型为xls或xlsx的文件！', {
				icon: 2
			});
			return false;
		}

		var reader = new FileReader();
		reader.onload = function(e) {
			var data = e.target.result;
			var wb = X.read(data, {
				type: 'binary'
			});
			var result = {};
			var FristSheetName = wb.SheetNames[0]
			result['Sheet1'] = X.utils.sheet_to_json(wb.Sheets[FristSheetName]);
			ExcelData = result;
			$('#buy_list tbody').html('');
			var h = '';
			//检测模板列名是否正确
			if (!('code' in ExcelData['Sheet1'][0])) {
				h += '<p>请将第一列列名设置为:code,表示自定义产品编码列。 </p>';
			}
			// if (!('num_s' in o)) {
			// 	h += '<p>请将第二列列名设置为:num_s,表示基本单位数量。</p>';
			// }
			// if (!('num_m' in o)) {
			// 	h += '<p>请将第三列列名设置为:num_m,表示小包装单位数量。 </p>';
			// }
			// if (!('num_b' in o)) {
			// 	h += '<p>请将第四列列名设置为:num_b,表示大包装单位数量。</p>';
			// }

			if (h != '') {
				h += '或者下载模板后重新录入数据。'
				layer.open({
					title: '模板错误',
					type: 1,
					area: ['500px', '300px'],
					content: '<div style="  padding:20px"> ' + h + ' </div>',
					btn: ['下载模板', '关闭'],
					btn1: function() {
						window.open('/Tpl/ExcelTpl.xls');
					}
				})
				event.target.value = '';
				return false;
			}

			 


			//检测Excel中产品编码是否为空，是否有重复，以及基本单位数量是否为数字，小包装单位、大包装单位数量是否为正整数
			var SheetOjb = {}; //每个code的数量
			var EmptyCode = ''; //Code为空的信息
			var ErrNum = ''; //数量错误的信息
			var ReCode = ''; //重复的编码
			$.each(ExcelData['Sheet1'], function(i, v) {
				if (!v['code']) {
					EmptyCode += '<p>第 <strong>' + i + ' </strong>行编码为空</p>'
				} else {
					if (!SheetOjb[v['code']]) {
						SheetOjb[v['code']] = 1;
					} else {
						SheetOjb[v['code']] += 1;
					}
				}

				//检测基本单位数量
				if (v['num_s'] == undefined) {
					ExcelData['Sheet1'][i]['num_s'] = 0;
				} else {
					if (!/^[0-9]+.?[0-9]*$/.test(Number(v['num_s']))) {
						ErrNum += '<p>第 <strong>' + (i + 2) + '</strong>行 编码为：<strong>' + v['code'] + '</strong> 的基本单位数量不为数字</p>';

					} else {
						ExcelData['Sheet1'][i]['num_s'] = Number(v['num_s']);
					}
				}

				//检测小包装单位数量
				if (v['num_m'] == undefined) {
					ExcelData['Sheet1'][i]['num_m'] = 0;
				} else {
					if (!(/^[0-9]\d*$/).test(Number(v['num_m']))) {
						ErrNum += '<p>第 <strong>' + (i + 2) + '</strong>行 编码为：<strong>' + v['code'] + ' </strong>的小包装单位数量不为整数</p>'
					} else {
						ExcelData['Sheet1'][i]['num_m'] = Number(v['num_m']);
					}
				}

				//检测大包装单位数量
				if (v['num_b'] == undefined) {
					ExcelData['Sheet1'][i]['num_b'] = 0;
				} else {
					if (!(/^[0-9]\d*$/).test(Number(v['num_b']))) {
						ErrNum += '<p>第 <strong>' + (i + 2) + ' </strong>行 编码为：<strong>' + v['code'] + ' </strong>的大包装单位数量不为整数</p>'
					} else {
						ExcelData['Sheet1'][i]['num_b'] = Number(v['num_b']);
					}
				}
			})



			// if ($.inArray(undefined, codes) > -1) {
			// 	layer.alert('导入的Excel中有客户编码为空的产品，请修改后重新上传', {
			// 		icon: 2
			// 	});
			// 	event.target.value = '';
			// 	return false;
			// }

			 
			$.each(SheetOjb, function(i, v) {
				if (v > 1) {
					ReCode += '<p>编码：<strong>' + i + '   </strong>重复次数：<strong> ' + v + '</strong></p>'
				}
			})

			var html = '';

			if (EmptyCode != '' || ErrNum != '' || ReCode != '') {

				if (EmptyCode != '') {
					html += '<blockquote class="layui-elem-quote" style="border-left:5px solid #79C48C" ><div style="text-align: center;font-size:16px;font-weight:bold">有编码为空的行</div>' + EmptyCode + '</blockquote>';
				}
				if (ErrNum != '') {
					html += '<blockquote class="layui-elem-quote" style="border-left:5px solid #ff8c8c"><div style="text-align: center;font-size:16px;font-weight:bold">有数量错误的行</div>' + ErrNum + '</blockquote>';
				}
				if (ReCode != '') {
					html += '<blockquote class="layui-elem-quote" style="border-left:5px solid #839eff"><div style="text-align: center;font-size:16px;font-weight:bold">有编码重复的行</div>' + ReCode + '</blockquote>';
				}

				var layerIndex = layer.open({
					title: '导入的Excel中有以下问题，请修复后重新导入',
					type: 1,
					area: ['400px', '500px'],
					content: '<div style="  padding:20px"> ' + html + ' </div>',
					btn: ['关闭'],
					btn1: function() {
						layer.close(layerIndex);
					}
				})
				event.target.value = '';
				console.log(ExcelData['Sheet1']);
				return false;
			}

			// if (!isEmptyObject(c)) {
			//     var h = '导入的Excel中有以下客户编码重复,请删除后重新导入：<br>';
			//     $.each(c, function (i, v) {
			//         h += '编码：' + i + '   重复次数：' + v + '<br/>';
			//     });
			//     layer.alert(h, {
			//         icon: 2
			//     });
			//     event.target.value = '';
			//     return false;
			// }



			// return false;

			var kpdw = $("#TxtCustomer option:selected").val();
			var areaid = "0";
			if ($("#zt").prop("checked") && $("#txtArea").val() != '' && $("#txtArea").val() != undefined) {
				areaid = $("#txtArea").val();
			}
			$.ajax({
				traditional: true,
				type: "Post",
				url: "../Handler/ProductHandler.ashx",
				dataType: "Json",
				data: {
					"Action": "GetcCusInvCodes",
					"codes": JSON.stringify(ExcelData['Sheet1']),
					"kpdw": kpdw,
					"areaid": areaid
				},
				success: function(res) {
					console.log(res)
					if (res.flag != 1) {
						layer.alert(res.message, {
							icon: 2
						});
						return false;
					}
					event.target.value = '';
					if (res.table.length > 0) {
						//添加buy_list 里的产品
						$("#buy_list tbody").html(get_html(res.table));
						$("#buy_list tbody tr").find(".realqty").hide();

						var $trs = $('#buy_list tbody tr');
						//根据正常返回的产品代码生成对象，并添加大中小三个单位的数量，用于填充至产品列表中
						var oja = {};
						$.each(res.ja, function(i, v) {
							oja[v.cInvCode] = {};
							oja[v.cInvCode]["num_s"] = v.num_s == isNaN(Number(v.num_s)) ? 0 : Number(v.num_s);
							oja[v.cInvCode]["num_m"] = v.num_m == isNaN(Number(v.num_m)) ? 0 : Number(v.num_m);
							oja[v.cInvCode]["num_b"] = v.num_b == isNaN(Number(v.num_b)) ? 0 : Number(v.num_b);
						})


						//循环添加产品数量
						var codeconfig_select = $('#codeconfig_select').val();

						$.each($trs, function(i, v) {
							var unit_m = $(v).find(".unit_m").text();
							var c = $(v).find('.code').text();
							var o = oja[c];
							if ($(v).find('.cComUnitName').text() == "米") {

								if ((o.num_s * 100) % (unit_m * 100) != 0 && o.num_s != "") {
									if (codeconfig_select == 1) {
										o.num_s = Math.ceil(o.num_s / unit_m).mul(unit_m);
									} else if (codeconfig_select == 2) {
										o.num_s = Math.floor(o.num_s / unit_m).mul(unit_m);
									} else {
										// eMsg = "产品添加成功,但基本数量已重置为0,请重新在下方列表中输入！";
										o.num_s = 0;
									}
								}
							} else {
								if (!(/^[0-9]\d*$/).test(o.num_s)) {
									if (codeconfig_select == 1) {
										o.num_s = Math.ceil(o.num_s);
									} else if (codeconfig_select == 2) {
										o.num_s = Math.floor(o.num_s);
									} else {
										// eMsg = "产品添加成功,但基本数量已重置为0,请重新在下方列表中输入！";
										o.num_s = 0;
									}

								}
							}
							$(v).find('.num_s').text(o.num_s);
							$(v).find('.num_m').text(o.num_m);
							$(v).find('.num_b').text(o.num_b);
							get_pack($(v).find('.num_s'));
						})

						var listInfo = get_listInfo("buy_list");
						$("#money").text(listInfo.money);
						$("#pro_weight").text(listInfo.weight)

						set_table_num("buy_list");
						set_table_color("buy_list");

					}


					var html = '<blockquote class="layui-elem-quote" style="border-left:5px solid #79C48C" ><div style="text-align: center;">成功导入' + res.ja.length + '个编码</div></blockquote>';
					if (res.Errcodes && res.Errcodes.length > 0) {
						var h = '';
						$.each(res.Errcodes, function(i, v) {
							h += '<p><strong>' + v + '</strong></p>';
						})
						html += '<blockquote class="layui-elem-quote" style="border-left:5px solid #ff8c8c" ><div style="text-align: center;">你有' + res.Errcodes.length + '个编码没有查到，请核实:</div>' + h + '</blockquote>';

					}
					if (res.LimitCodes && res.LimitCodes.length > 0) {
						var h = '';
						$.each(res.LimitCodes, function(i, v) {
							h += '<p><strong>' + v + '</strong></p>';
						})
						html += '<blockquote class="layui-elem-quote" style="border-left:5px solid #839eff" ><div style="text-align: center">你有' + res.LimitCodes.length + '个编码被限销，请核实:</div>' + h + '</blockquote>';
					}
					layer.alert(html);


				}
			});

		};
		reader.readAsBinaryString(f);

	})

 
})