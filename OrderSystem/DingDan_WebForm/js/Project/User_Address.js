 var useraddress = [],
 	zt_address = [],
 	ps_address = [],
 	ztps_address = [],
 	ct007 = [];



 var form = layui.form,
 	table = layui.table,
 	layer = layui.layer,
 	$ = layui.jquery;


 //自提table配置
 var zt_options = {
 	elem: "#tb",
 	page: true,
 	limit: 10,
 	even: true,
 	data: [],
 	cols: [
 		[{
 				fixed: 'left',
 				width: 150,
 				align: 'center',
 				toolbar: '#ToolBar',
 				title: "操作"
 			}, {
 				field: 'strDistributionType',
 				title: '送货方式',
 				width: 120,
 				align: 'center'
 			}, {
 				field: 'strCarplateNumber',
 				title: '车牌号码',
 				width: 120,
 				align: 'center',
 				sort: true
 			}, {
 				field: 'strDriverName',
 				title: '司机姓名',
 				width: 220,
 				align: 'center',
 				sort: true
 			}, {
 				field: 'strDriverTel',
 				title: '司机电话',
 				width: 220,
 				align: 'center',
 				sort: true
 			}, {
 				field: 'strIdCard',
 				title: '司机身份证',
 				width: 220,
 				align: 'center',
 				sort: true
 			}

 		]
 	]
 };


 //配送table配置
 var ps_options = {
 	elem: "#tb",
 	page: true,
 	limit: 10,
 	even: true,
 	data: [],
 	cols: [
 		[{
 				fixed: 'left',
 				width: 150,
 				align: 'center',
 				toolbar: '#ToolBar',
 				title: "操作"
 			},

 			{
 				field: 'strDistributionType',
 				title: '送货方式',
 				width: 120,
 				align: 'center'
 			}, {
 				field: 'strConsigneeName',
 				title: '收货人姓名',
 				width: 120,
 				align: 'center',
 				sort: true
 			}, {
 				field: 'strConsigneeTel',
 				title: '收货人电话',
 				width: 150,
 				align: 'center',
 				sort: true
 			}, {
 				field: 'strDistrict',
 				title: '收货行政区',
 				width: 300,
 				align: 'center',
 				sort: true
 			}, {
 				field: 'strReceivingAddress',
 				title: '收货地址',
 				width: 300,
 				align: 'center',
 				sort: true
 			}

 		]
 	]
 };

 //自提配送table配置
 var ztps_options = {
 	elem: "#tb",
 	page: true,
 	limit: 10,
 	even: true,
 	data: [],
 	cols: [
 		[{
 				fixed: 'left',
 				width: 150,
 				align: 'center',
 				toolbar: '#ToolBar',
 				title: "操作"
 			}, {
 				field: 'strDistributionType',
 				title: '送货方式',
 				width: 120,
 				align: 'center'
 			}, {
 				field: 'strConsigneeName',
 				title: '收货人姓名',
 				width: 120,
 				align: 'center',
 				sort: true
 			}, {
 				field: 'strConsigneeTel',
 				title: '收货人电话',
 				width: 150,
 				align: 'center',
 				sort: true
 			}, {
 				field: 'strDistrict',
 				title: '收货行政区',
 				width: 300,
 				align: 'center',
 				sort: true
 			}, {
 				field: 'strReceivingAddress',
 				title: '收货地址',
 				width: 300,
 				align: 'center',
 				sort: true
 			}

 		]
 	]
 };


 get_address();
 RenderTable();

 //切换送货方式按钮，重新渲染table
 form.on('radio(shfs)', function(data) {
 	RenderTable()

 })


 //获取所有信息
 function get_address() {
 	$.ajax({
 		url: "../Handler/ProductHandler.ashx",
 		data: {
 			"Action": "Get_User_Address"
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
 			useraddress = res.DataSet.opUserAddress;
 			ct007 = res.DataSet.CT007;
 			address_class();



 		}
 	})
 }

 //将获取所有的地址按iAddressType分类，1为自提，2为配送，3为自提配送 
 function address_class() {
 	zt_address.length = ps_address.length = ztps_address.length = 0;
 	$.each(useraddress, function(i, v) {
 		if (v.iAddressType == 1) {
 			zt_address.push(v);
 		} else if (v.iAddressType == 2) {
 			ps_address.push(v);
 		} else if (v.iAddressType == 3) {
 			ztps_address.push(v);
 		}
 	})

 	zt_options.data = zt_address;
 	ps_options.data = ps_address;
 	ztps_options.data = ztps_address;

 }



 function RenderTable() {
 	var value = $("input[name=shfs]:checked").val();
 	if (value == 1) {
 		table.render(zt_options);
 	} else if (value == 2) {
 		table.render(ps_options);
 	} else if (value == 3) {
 		table.render(ztps_options);
 	}
 }

  

//table操作，编辑和删除
 table.on('tool(tb)', function(obj) {
 	if (obj.event === 'edit') {
 		//－－－－－－－－－－－－－－－－－－－－－－－－－－－编辑自提－－－－－－－－－－－－－－－－－－－－－－－－
 		if ($("input[name=shfs]:checked").val() == 1) {
 
 	layer.open({
 		type: 1,
 		title: '修改自提信息',
 		shade: 0.6,
 		resize: false,
 		area: ['600px', '350px'],
 		btn: ["保存", "取消"],
 		content: $("#change_zt_div").html(),
 		success: function(layero, index) {
 			layero.find("#strCarplateNumber").val(obj.data.strCarplateNumber);
 			layero.find("#strDriverName").val(obj.data.strDriverName);
 			layero.find("#strDriverTel").val(obj.data.strDriverTel);
 			layero.find("#strIdCard").val(obj.data.strIdCard);
 		},
 		btn1: function(index, layero) {
 			var strCarplateNumber = layero.find("#strCarplateNumber").val().trim();
 			var strDriverName = layero.find("#strDriverName").val().trim();
 			var strDriverTel = layero.find("#strDriverTel").val().trim();
 			var strIdCard = layero.find("#strIdCard").val().trim();

 			if (strCarplateNumber == "") {
 				layer.alert("车牌号不能为空!", {
 					icon: 2
 				});
 				return false;
 			}
 			if (strDriverName == "") {
 				layer.alert("司机姓名不能为空!", {
 					icon: 2
 				});
 				return false;
 			}
 			if (strDriverTel == "") {
 				layer.alert("司机电话不能为空!", {
 					icon: 2
 				});
 				return false;
 			}
 			if (strDriverTel.trim().length>13) {
 			    layer.alert("司机电话位数过长!", {
 			        icon: 2
 			    });
 			    return false;
 			}
 			if (strIdCard.trim().length>18) {
 				layer.alert("司机身份证位数过长!", {
 					icon: 2
 				});
 				return false;
 			}
 			$.ajax({
 				url: "../Handler/ProductHandler.ashx",
 				dataType: "Json",
 				type: "Post",
 				data: {
 					"Action": "Update_UserAddressZT",
 					"lngopUseraddressId": obj.data.lngopUseraddressId,
 					"strCarplateNumber": strCarplateNumber,
 					"strDriverName": strDriverName,
 					"strDriverTel": strDriverTel.trim(),
 					"strIdCard": strIdCard.trim()
 				},
 				success: function(data) {
 					if (data.flag != 1) {
 						layer.alert(data.message, {
 							icon: 2
 						});
 						return false;
 					}
 				 
 						get_address();
 						RenderTable();
 						layer.alert(data.message, {
 							icon: 1
 						}, function() {
 							layer.closeAll();

 						})
 					 
 				},
 				error: function(err) {
 					layer.alert("更新失败,请重试或联系管理员!", {
 						icon: 2
 					});
 					return false;
 				}
 			});
 		}

 	})
 		}
//－－－－－－－－－－－－－－－－－－－－－－－－－－－编辑配送－－－－－－－－－－－－－－－－－－－－－－－－
 		else if ($("input[name=shfs]:checked").val() == 2) {
 			layer.open({
 				type: 1,
 				title: '修改配送信息',
 				shade: 0.6,
 				resize: false,
 				area: ['750px', '500px'],
 				btn: ["保存", "取消"],
 				content: $("#change_ps_div").html(),
 				success: function(layero, index) {
 					layero.find("#strConsigneeName").val(obj.data.strConsigneeName);
 					layero.find("#strConsigneeTel").val(obj.data.strConsigneeTel);
 					layero.find("#strReceivingAddress").val(obj.data.strReceivingAddress);
 					load_province(layero);
 				},
 				btn1: function(index, layero) {
 					var strConsigneeName = layero.find("#strConsigneeName").val().trim();
 					var strConsigneeTel = layero.find("#strConsigneeTel").val().trim();
 					var strReceivingAddress = layero.find("#strReceivingAddress").val().trim();

 					if (strConsigneeName == "") {
 						layer.alert("收货人姓名不能为空!", {
 							icon: 2
 						});
 						return false;
 					}
 					if (strConsigneeTel == "") {
 						layer.alert("收货人电话不能为空!", {
 							icon: 2
 						});
 						return false;
 					}
 					if (strConsigneeTel.trim().length>13) {
 					    layer.alert("收货人电话位数过长!", {
 					        icon: 2
 					    });
 					    return false;
 					}
 					if (
 						layero.find("#province").val() == "" ||
 						(layero.find("#city").val() == "" && !layero.find("#city").parent().hasClass('layui-hide')) ||
 						(layero.find("#area").val() == "" && !layero.find("#area").parent().hasClass('layui-hide'))
 					) {
 						layer.alert("行政区选择不正确!", {
 							icon: 2
 						});
 						return false;
 					}
 					if (strReceivingAddress == "") {
 						layer.alert("收货地址不能为空!", {
 							icon: 2
 						});
 						return false;
 					}
 					var b = {};
 					if (!layero.find("#area").parent().hasClass('layui-hide')) {
 						b = layero.find("#area option:selected").data();
 					} else {
 						if (!layero.find("#city").parent().hasClass('layui-hide')) {
 							b = layero.find("#city option:selected").data();
 						} else {
 							b = layero.find("#province option:selected").data();
 						}
 					}

 					$.ajax({
 						url: "../Handler/ProductHandler.ashx",
 						dataType: "Json",
 						type: "Post",
 						data: {
 							"Action": "Update_UserAddressPS",
 							"lngopUseraddressId": obj.data.lngopUseraddressId,
 							"strConsigneeName": strConsigneeName,
 							"strConsigneeTel": strConsigneeTel.trim(),
 							"strReceivingAddress": strReceivingAddress,
 							"area": b.vdescription
 						},
 						success: function(data) {
 							if (data.flag != 1) {
 								layer.alert(data.message, {
 									icon: 2
 								});
 								return false;
 							}

 							get_address();
 							RenderTable();
 							layer.alert(data.message, {
 								icon: 1
 							}, function() {
 								layer.closeAll();

 							})

 						},
 						error: function(err) {
 							layer.alert("更新失败,请重试或联系管理员!", {
 								icon: 2
 							});
 							return false;
 						}
 					});
 				}

 			})

 		}

//－－－－－－－－－－－－－－－－－－－－－－－－－－－编辑自提托运－－－－－－－－－－－－－－－－－－－－－－－－
 		else if ($("input[name=shfs]:checked").val() == 3) {

 			layer.open({
 				type: 1,
 				title: '修改自提托运信息',
 				shade: 0.6,
 				resize: false,
 				area: ['750px', '500px'],
 				btn: ["保存", "取消"],
 				content: $("#change_ps_div").html(),
 				success: function(layero, index) {
 					layero.find("#strConsigneeName").val(obj.data.strConsigneeName);
 					layero.find("#strConsigneeTel").val(obj.data.strConsigneeTel);
 					layero.find("#strReceivingAddress").val(obj.data.strReceivingAddress);
 					load_province(layero);
 				},
 				btn1: function(index, layero) {
 					var strConsigneeName = layero.find("#strConsigneeName").val().trim();
 					var strConsigneeTel = layero.find("#strConsigneeTel").val().trim();
 					var strReceivingAddress = layero.find("#strReceivingAddress").val().trim();

 					if (strConsigneeName == "") {
 						layer.alert("收货人姓名不能为空!", {
 							icon: 2
 						});
 						return false;
 					}
 					if (strConsigneeTel == "") {
 						layer.alert("收货人电话不能为空!", {
 							icon: 2
 						});
 						return false;
 					}
 					if (strConsigneeTel.trim().length>13) {
 					    layer.alert("收货人电话位数过长!", {
 					        icon: 2
 					    });
 					    return false;
 					}
 					if (
 						layero.find("#province").val() == "" ||
 						(layero.find("#city").val() == "" && !layero.find("#city").parent().hasClass('layui-hide')) ||
 						(layero.find("#area").val() == "" && !layero.find("#area").parent().hasClass('layui-hide'))
 					) {
 						layer.alert("行政区选择不正确!", {
 							icon: 2
 						});
 						return false;
 					}
 					if (strReceivingAddress == "") {
 						layer.alert("收货地址不能为空!", {
 							icon: 2
 						});
 						return false;
 					}
 		 
					var b = {};
 					if (!layero.find("#area").parent().hasClass('layui-hide')) {
 						b = layero.find("#area option:selected").data();
 					} else {
 						if (!layero.find("#city").parent().hasClass('layui-hide')) {
 							b = layero.find("#city option:selected").data();
 						} else {
 							b = layero.find("#province option:selected").data();
 						}
 					}
 					$.ajax({
 						url: "../Handler/ProductHandler.ashx",
 						dataType: "Json",
 						type: "Post",
 						data: {
 							"Action": "Update_UserAddressZTPS",
 							"lngopUseraddressId": obj.data.lngopUseraddressId,
 							"strConsigneeName": strConsigneeName,
 							"strConsigneeTel": strConsigneeTel.trim(),
 							"strReceivingAddress": strReceivingAddress,
 							"area": b.vdescription,
 							"ccodeid":b.ccodeid

 						},
 						success: function(data) {
 							if (data.flag != 1) {
 								layer.alert(data.message, {
 									icon: 2
 								});
 								return false;
 							}

 							get_address();
 							RenderTable();
 							layer.alert(data.message, {
 								icon: 1
 							}, function() {
 								layer.closeAll();

 							})

 						},
 						error: function(err) {
 							layer.alert("更新失败,请重试或联系管理员!", {
 								icon: 2
 							});
 							return false;
 						}
 					});
 				}

 			})
 		}
 	}
 	else if(obj.event==='del'){
 		layer.confirm('确认要删除该地址？',{icon:3},function(){
 					$.ajax({
 			url: "../Handler/ProductHandler.ashx",
 			dataType: "Json",
 			type: "Post",
 			data: {
 				"Action": "Del_UserAddress",
 				"lngopUseraddressId": obj.data.lngopUseraddressId
 			},
 			success: function(data) {
 				if (data.flag !=1) {
 					layer.alert(data.message, {
 						icon: 2
 					});
 					return false;
 				}
 				 
 				 	//obj.del();
 				 	get_address();
 				 	RenderTable();
 					layer.alert(data.message, {
 						icon: 1
 					}, function() {
 						layer.closeAll();

 					})
 			 
 			},
 			error: function(err) {
 				layer.alert("更新失败,请重试或联系管理员!", {
 					icon: 2
 				});
 				return false;
 			}
 		});
 		})
 	}
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

//新增自提信息
 $("#insert_zt").click(function() {
 	layer.open({
 		type: 1,
 		title: '新增自提信息',
 		shade: 0.6,
 		resize: false,
 		area: ['600px', '350px'],
 		btn: ["保存", "取消"],
 		content: $("#change_zt_div").html(),
 		btn1: function(index, layero) {
 			var strCarplateNumber = layero.find("#strCarplateNumber").val().trim();
 			var strDriverName = layero.find("#strDriverName").val().trim();
 			var strDriverTel = layero.find("#strDriverTel").val().trim();
 			var strIdCard = layero.find("#strIdCard").val().trim();

 			if (strCarplateNumber == "") {
 				layer.alert("车牌号不能为空!", {
 					icon: 2
 				});
 				return false;
 			}
 			if (strDriverName == "") {
 				layer.alert("司机姓名不能为空!", {
 					icon: 2
 				});
 				return false;
 			}
 			if (strDriverTel == "") {
 				layer.alert("司机电话不能为空!", {
 					icon: 2
 				});
 				return false;
 			}
 			if (strDriverTel.trim().length > 13) {
 			    layer.alert("司机电话位数过长!", {
 			        icon: 2
 			    });
 			    return false;
 			}
 			if (strIdCard == "") {
 				layer.alert("司机身份证不能为空!", {
 					icon: 2
 				});
 				return false;
 			}
 			if (strIdCard.trim().length > 18) {
 			    layer.alert("司机身份证位数过长!", {
 			        icon: 2
 			    });
 			    return false;
 			}
 			$.ajax({
 				url: "../Handler/ProductHandler.ashx",
 				dataType: "Json",
 				type: "Post",
 				data: {
 					"Action": "Insert_UserAddressZT",
 					"strCarplateNumber": strCarplateNumber,
 					"strDriverName": strDriverName,
 					"strDriverTel": strDriverTel.trim(),
 					"strIdCard": strIdCard.trim()
 				},
 				success: function(data) {
 					if (data.flag != 1) {
 						layer.alert(data.message, {
 							icon: 2
 						});
 						return false;
 					}
 					 
 						get_address();
 						RenderTable();
 						layer.alert(data.message, {
 							icon: 1
 						}, function() {
 							layer.closeAll();

 						})
 				 
 				},
 				error: function(err) {
 					layer.alert("更新失败,请重试或联系管理员!", {
 						icon: 2
 					});
 					return false;
 				}
 			});
 		}

 	})
 })

//新增配送信息
 $("#insert_ps").click(function() {
 	layer.open({
 		type: 1,
 		title: '新增配送信息',
 		shade: 0.6,
 		resize: false,
 		area: ['750px', '500px'],
 		btn: ["保存", "取消"],
 		content: $("#change_ps_div").html(),
 		success: function(layero, index) {
 			load_province(layero);
 		},
 		btn1: function(index, layero) {
 			var strConsigneeName = layero.find("#strConsigneeName").val().trim();
 			var strConsigneeTel = layero.find("#strConsigneeTel").val().trim();
 			var strReceivingAddress = layero.find("#strReceivingAddress").val().trim();

 			if (strConsigneeName == "") {
 				layer.alert("收货人姓名不能为空!", {
 					icon: 2
 				});
 				return false;
 			}
 			if (strConsigneeTel == "") {
 				layer.alert("收货人电话不能为空!", {
 					icon: 2
 				});
 				return false;
 			}
 			if (strConsigneeTel.trim().length>13) {
 			    layer.alert("收货人电话位数过长!", {
 			        icon: 2
 			    });
 			    return false;
 			}
 			if (
 				layero.find("#province").val() == "" ||
 				(layero.find("#city").val() == "" && !layero.find("#city").parent().hasClass('layui-hide')) ||
 				(layero.find("#area").val() == "" && !layero.find("#area").parent().hasClass('layui-hide'))
 			) {
 				layer.alert("行政区选择不正确!", {
 					icon: 2
 				});
 				return false;
 			}
 			if (strReceivingAddress == "") {
 				layer.alert("收货地址不能为空!", {
 					icon: 2
 				});
 				return false;
 			}
 			var b = {};
 			if (!layero.find("#area").parent().hasClass('layui-hide')) {
 				b = layero.find("#area option:selected").data();
 			} else {
 				if (!layero.find("#city").parent().hasClass('layui-hide')) {
 					b = layero.find("#city option:selected").data();
 				} else {
 					b = layero.find("#province option:selected").data();
 				}
 			}
 			$.ajax({
 				url: "../Handler/ProductHandler.ashx",
 				dataType: "Json",
 				type: "Post",
 				data: {
 					"Action": "Insert_UserAddressPS",
 					"strConsigneeName": strConsigneeName,
 					"strConsigneeTel": strConsigneeTel.trim(),
 					"strReceivingAddress": strReceivingAddress,
 					"area": b.vdescription,
 					"ccodeid": b.ccodeid
 				},
 				success: function(data) {
 					if (data.flag !=1) {
 						layer.alert(data.message, {
 							icon: 2
 						});
 						return false;
 					}
 					 
 						get_address();
 						RenderTable();
 						layer.alert(data.message, {
 							icon: 1
 						}, function() {
 							layer.closeAll();

 						})
 					 
 				},
 				error: function(err) {
 					layer.alert("新增失败,请重试或联系管理员!", {
 						icon: 2
 					});
 					return false;
 				}
 			});
 		}

 	})
 })

//新增自提配送信息
 $("#insert_ztps").click(function() {
 	layer.open({
 		type: 1,
 		title: '新增自提(托运)信息',
 		shade: 0.6,
 		resize: false,
 		area: ['750px', '500px'],
 		btn: ["保存", "取消"],
 		content: $("#change_ps_div").html(),
 		success: function(layero, index) {
 			load_province(layero);
 		},
 		btn1: function(index, layero) {
 			var strConsigneeName = layero.find("#strConsigneeName").val().trim();
 			var strConsigneeTel = layero.find("#strConsigneeTel").val().trim();
 			var strReceivingAddress = layero.find("#strReceivingAddress").val().trim();

 			if (strConsigneeName == "") {
 				layer.alert("收货人姓名不能为空!", {
 					icon: 2
 				});
 				return false;
 			}
 			if (strConsigneeTel == "") {
 				layer.alert("收货人电话不能为空!", {
 					icon: 2
 				});
 				return false;
 			}
 			if (strConsigneeTel.trim().length >13) {
 			    layer.alert("收货人电话位数过长!", {
 			        icon: 2
 			    });
 			    return false;
 			}
 			if (
 				layero.find("#province").val() == "" ||
 				(layero.find("#city").val() == "" && !layero.find("#city").parent().hasClass('layui-hide')) ||
 				(layero.find("#area").val() == "" && !layero.find("#area").parent().hasClass('layui-hide'))
 			) {
 				layer.alert("行政区选择不正确!", {
 					icon: 2
 				});
 				return false;
 			}
 			if (strReceivingAddress == "") {
 				layer.alert("收货地址不能为空!", {
 					icon: 2
 				});
 				return false;
 			}
 			var b = {};
 			if (!layero.find("#area").parent().hasClass('layui-hide')) {
 				b = layero.find("#area option:selected").data();
 			} else {
 				if (!layero.find("#city").parent().hasClass('layui-hide')) {
 					b = layero.find("#city option:selected").data();
 				} else {
 					b = layero.find("#province option:selected").data();
 				}
 			}
 			$.ajax({
 				url: "../Handler/ProductHandler.ashx",
 				dataType: "Json",
 				type: "Post",
 				data: {
 					"Action": "Insert_UserAddressZTPS",
 					"strConsigneeName": strConsigneeName,
 					"strConsigneeTel": strConsigneeTel.trim(),
 					"strReceivingAddress": strReceivingAddress,
 					"area": b.vdescription,
 					"ccodeid": b.ccodeid
 				},
 				success: function(data) {
 					if (data.flag !=1) {
 						layer.alert(data.message, {
 							icon: 2
 						});
 						return false;
 					}
 					 
 						get_address();
 						RenderTable();
 						layer.alert(data.message, {
 							icon: 1
 						}, function() {
 							layer.closeAll();

 						})
 					 
 				},
 				error: function(err) {
 					layer.alert("新增失败,请重试或联系管理员!", {
 						icon: 2
 					});
 					return false;
 				}
 			});
 		}

 	})
 })


 $("#area_detail").click(function() {
 	layer.open({
 		type: 1,
 		title: '行政区信息',
 		shade: 0.6,
 		resize: false,
 		area: ['400px', '500px'],
 		btn: ["关闭"],
 		content: $("#area_div").html(),
 		success: function(layero, index) {
 			Get_User_Area(layero);
 		},
 		btn1: function() {
 			layer.closeAll();
 		}
 	})
 })

 $(document).on("click", ".del_area", function() {
 	$this = $(this);
 	layer.confirm("你确定要删除该条数据?", function() {

 		$.ajax({
 			url: "../Handler/ProductHandler.ashx",
 			dataType: "Json",
 			type: "Post",
 			data: {
 				"Action": "DL_UserAddress_exByDel",
 				"area_id": $this.parents("tr").find(".lngopUseraddress_exId").text()
 			},
 			success: function(res) {
 				if (res.flag == '0') {
 					layer.alert(res.message, {
 						icon: 2
 					});
 					return false;
 				} else if (res.flag == '1') {
 					layer.alert(res.message, {
 						icon: 1
 					});
 					$this.parents("tr").remove();
 				}
 			}
 		})
 	})

 })

 $(document).on("click", "#insert_area", function() {
 	layer.open({
 		type: 1,
 		title: '新增行政区',
 		shade: 0.6,
 		resize: false,
 		area: ['750px', '350px'],
 		btn: ["保存", "取消"],
 		content: $("#new_area").html(),
 		success: function(layero, index) {
 			load_province(layero);
 		},
 		btn1: function(index, layero) {
 			if (
 				layero.find("#province").val() == "" ||
 				(layero.find("#city").val() == "" && !layero.find("#city").parent().hasClass('layui-hide')) ||
 				(layero.find("#area").val() == "" && !layero.find("#area").parent().hasClass('layui-hide'))
 			) {
 				layer.alert("行政区选择不正确!", {
 					icon: 2
 				});
 				return false;
 			}

 			var b = {};
 			if (!layero.find("#area").parent().hasClass('layui-hide')) {
 				b = layero.find("#area option:selected").data();
 			} else {
 				if (!layero.find("#city").parent().hasClass('layui-hide')) {
 					b = layero.find("#city option:selected").data();
 				} else {
 					b = layero.find("#province option:selected").data();
 				}
 			}
 			$.ajax({
 				url: "../Handler/ProductHandler.ashx",
 				dataType: "Json",
 				type: "Post",
 				data: {
 					"Action": "Insert_UserArea",
 					"area": b.vdescription,
 					"ccodeid": b.ccodeid
 				},
 				success: function(res) {
 					if (res.flag != 1) {
 						layer.alert(res.message, {
 							icon: 2
 						});
 						return false;
 					}
 					if (res.flag == '1') {
 						layer.alert("新增成功!", {
 							icon: 1
 						}, function() {
 							layer.closeAll();
 						});
 					}
 				},
 				error: function(err) {
 					console.log(err);
 					layer.alert("新增失败,请重试或联系管理员!", {
 						icon: 2
 					});
 					return false;
 				}
 			})
 		}
 	})
 })

 function Get_User_Area(layero) {
 	$.ajax({
 		url: "../Handler/ProductHandler.ashx",
 		dataType: "Json",
 		type: "Post",
 		data: {
 			"Action": "Get_User_Area"
 		},
 		success: function(res) {
 			console.log(res);

 			var areaHtml = "";
 			$.each(res, function(i, v) {
 				areaHtml += '<tr><td><input type="button" class="layui-btn layui-btn-danger layui-btn-mini del_area" value="删除"></td>';
 				areaHtml += '<td style="display:none" class="lngopUseraddress_exId">' + v.lngopUseraddress_exId + '</td>';
 				areaHtml += '<td>' + v.xzq + '</td></tr>';
 			})
 			layero.find('table tbody').html(areaHtml);
 		},
 		error: function(err) {
 			layer.alert("获取行政区失败,请重试或联系管理员!", {
 				icon: 2
 			});
 			return false;
 		}
 	})
 }

 function load_province(layero) {

 	var pHtml = "<option value>请选择</option>";
 	$.each(ct007, function(i, v) {
 		if (v.ilevels == 0) {
 			pHtml += '<option data-bChildFlag=' + v.bChildFlag + ' value=' + v.ccodeID + ' data-ccodeID=' + v.ccodeID + ' data-vdescription=' + v.vdescription + ' data-vsimpleName=' + v.vsimpleName + '>' + v.vsimpleName + '</option>'
 		}
 		//pHtml += '<option value=' + v.provinceCode + '_' + v.mallCityList.length + '_' + i + '>' + v.provinceName + '</option>';
 	});

 	layero.find("#province").html(pHtml);
 	form.render();


 	form.on('select(province)', function(data) {
 		if ($(data.elem).find("option:selected").data().bchildflag) {
 			var cHtml = "<option value>请选择</option>"
 			var d = data.value;
 			$.each(ct007, function(i, v) {
 				if (v.cpCodeID == d) {
 					cHtml += '<option data-bChildFlag=' + v.bChildFlag + ' value=' + v.ccodeID + ' data-ccodeID=' + v.ccodeID + ' data-vdescription=' + v.vdescription + ' data-vsimpleName=' + v.vsimpleName + '>' + v.vsimpleName + '</option>'
 				}
 			})

 			layero.find("#city").parent().removeClass('layui-hide');
 			layero.find("#area").parent().addClass('layui-hide');
 			layero.find('#city').html(cHtml);
 			form.render('select');
 		} else {
 			layero.find("#city").html();
 			layero.find("#area").html();
 			layero.find("#city").parent().addClass('layui-hide');
 			layero.find("#area").parent().addClass('layui-hide');
 		}
 	})

 	form.on('select(city)', function(data) {
 		if ($(data.elem).find("option:selected").data().bchildflag) {
 			var aHtml = " <option value>请选择</option>";
 			var d = data.value;
 			$.each(ct007, function(i, v) {
 				if (v.cpCodeID == d) {
 					aHtml += '<option data-bChildFlag=' + v.bChildFlag + ' value=' + v.ccodeID + ' data-ccodeID=' + v.ccodeID + ' data-vdescription=' + v.vdescription + ' data-vsimpleName=' + v.vsimpleName + '>' + v.vsimpleName + '</option>'
 				}
 			})
 			layero.find("#area").html(aHtml);
 			layero.find("#area").parent().removeClass('layui-hide');
 			form.render('select')
 		}

 	})
 }