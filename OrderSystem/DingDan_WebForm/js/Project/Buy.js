var html_tpl = ""; //全局模板变量
var cstcode = '00';
var iShowType = 1;
var autoSave = 1;
var isModify = 0;
var isSpecial = 0; //是否是特殊订单，0为否，1为是；用于Change_KPDW方法


layui.use(['form', 'util', 'laytpl', 'laydate', 'layer', 'element', 'upload'], function() {
    var form = layui.form(),
        util = layui.util,
        laydate = layui.laydate,
        laytpl = layui.laytpl,
        element = layui.element(),
        upload = layui.upload;
    layer = layui.layer;

    //页面加载时初始化数据
    $.ajax({
        url: "../Handler/ProductHandler.ashx",
        dataType: "Json",
        type: "Post",
        data: {
            //  "Action": "Get_BaseInfo",
            "Action": "GetAllBaseInfo",
            "PageType": 1
        },
        aysnc: false,
        success: function(data) {
            if (data.flag != 1) {
                layer.alert(data.message, {
                    icon: 2
                })
                return false;
            }

            var time = new Date();
            data["msg"] = time.toLocaleDateString().replace(/\//g, "-");
            $("#view").load("../tpl/buy_orderTpl.html?v=" + new Date().getTime(), function(da, status, XHR) {
                if (status == 'success') {
                    $("#select_SpecialProduct").hide();
                    $("#buy_list thead tr th:eq(12)").hide();
                    $("#datCreateTime").text(data.msg); //订单日期
                    //开票单位

                    $("#autosave_msg").removeClass('layui-hide');

                    //$('#upload').removeClass('layui-hide');
                    // var Upload='<button class="layui-btn layui-btn-small"  type="button" id="uploadExcel">导入Excel</button>';
                    //$('.btns').append('<a href="javascript:;" class="file layui-btn">导入Excel<input type="file" name="" id="uploadExcel"></a>');
                    $('.btns').append('<button class="layui-btn layui-btn-small"  type="button" id="fastInput">客户代码输入</button>');
                    $('.btns').append('<button class="layui-btn layui-btn-small"  type="button" id="Dao">导入Excel</button>');
                    // $('.btns').append('<button class="layui-btn layui-btn-small layui-btn-disabled" disabled  type="button" id="Print">打印订单</button>');
                    // $('.btns').append('<button class="layui-btn layui-btn-small"  type="button" onclick="javascript:location.reload()">刷新</button>');
                    $.each(data.DataSet.Kpdw_dt, function(i, v) {
                        $("#TxtCustomer").append('<option cCusPPerson=' + v.cCusPPerson + ' value="' + v.cCusCode + '">' + v.cCusName + '</option>')
                    });
                    var html = "";
                    $.each(data.DataSet.CarType_dt, function(i, v) {
                        html += '<option value=' + v.cValue + '>' + v.cValue + '</option>';
                    });
                    $("#cdefine3").append(html);
                    form.render();


                }
            });

        }
    });

    //一键清除零库存
    $(document).on("click", "#del_none", function() {
        layer.confirm("你确定要删除无库存的商品？", function() {
            del_none("buy_list", "code");
        })
    });

    /**
    2018-09-20修改，添加提交时检测是否有需求订单
    */

    //提交正式订单
    $(document).on("click", "#submit_buy_list", function() {
        $("#submit_buy_list").blur();
        if (check_form()) { //验证表单数据是否完整
            //submitOrder();
            var codes = [];
            $.each($("#buy_list tbody tr"), function(i, v) {
                codes.push($(v).find(".code").text());
            })

            $.ajax({
                type: "post",
                url: "../Handler/ProductHandler.ashx",
                dataType: "Json",
                //async: false,
                data: {
                    "Action": "CheckCodeInXOrder",
                    "codes": codes.join('|'),
                },
                success: function(data) {
                    console.table(data)
                    if (data.flag != '1') {
                        layer.alert(data.message, { icon: 2 })
                        return false;
                    }
                    if (data.data.length == 0) {
                        submitOrder();
                    } else {
                        var html = '';
                        html = '<table  style="text-align:center"    border="1" cellspacing="0" cellpadding="0">';
                        html += '<thead  style="background-color:#eee"><th  width="110" height="40">需求订单号</th><th width="200">产品名称</th><th width="80">产品规格</th><th width="80">产品数量</th><th width="90">包装数量</th><th width="140">开票单位</th></thead>'
                        html += '<tbody>';
                        $.each(data.data, function(i, v) {
                            html += '<tr style="height:30px;font-size:12px;">';
                            html += '<td>' + v.strBillNo + '</td><td>' + v.cInvName + '</td><td>' + v.cInvStd + '</td><td>' + v.iQuantity + '</td><td>' + v.cdefine22 + '</td><td>' + v.cCusName + '</td>';
                            html += '<tr>';
                        })
                        html += '</tbody>';
                        html += '</table>';
                        var sns = $("#buy_list tbody tr").find('.SN');
                        $('#buy_list tbody').find('i').remove();
                        $.each(data.data, function(i, v) {
                            $.each(sns, function(m, n) {
                                if ($(n).attr('code') == v.cInvCode) {
                                    if ($(n).parents('tr').find('.cInvName i').length == 0) {
                                        $(n).parents('tr').find('.cInvName').prepend('<i class="layui-icon"   xOrderNo="'+v.strBillNo+'" xOrderNum="'+v.iQuantity+'" cCusName="'+v.cCusName+'" style="font-size: 20px; color: #1E9FFF;">&#xe60a;</i>');
                                        return;
                                    }
                                } 
                            })
                        });
                        layer.open({
                            type: 1,
                            title: '提示：以下产品的需求订单还未完成,是否要继续提交普通订单',
                            area: ['720px', '400px'], //宽高
                            content: html,
                            btn: ['确认继续提交', '返回修改订单'],
                            yes: function(index, layero) {
                                layer.close(index);
                                submitOrder();
                            },
                            btn2: function(index, layero) {

                            }

                        });
                    }


                }
            })

        }

    })

  

    function submitOrder() {
        $.ajax({
            traditional: true,
            type: "Post",
            url: "../Handler/ProductHandler.ashx",
            dataType: "Json",
            data: {
                "Action": "DLproc_NewOrderByIns_new",
                "formData": JSON.stringify(get_formData()),
                "listData": JSON.stringify(get_listData("buy_list"))
            },
            success: function(data) {
                if (data.flag == '5') {
                    layer.alert(data.message, { icon: 2 })
                    return false;
                }
                if (data.flag == '7') {
                    Product_limit(data, "buy_list");
                    return false;
                }
                if (data.flag == '0') {
                    var errMsg = "你的订单存在以下问题:<br />";
                    if (data.list_msg != null && data.list_msg.length > 0) {
                        for (var i = 0; i < data.list_msg.length; i++) {
                            errMsg += ((i + 1) + "、" + data.list_msg[i] + "<br />");
                        };
                    }
                    if (data.dt != null && data.dt.length > 0) {
                        $.each($("#buy_list tbody tr"), function(i, v) {
                            if ($(v).find(".code").text() == data.dt[i]["cInvCode"]) {
                                $(v).find(".fAvailQtty").text(data.dt[i]["fAvailQtty"]);
                                $(v).find(".ex_price").text(data.dt[i]["ExercisePrice"]);
                            }
                        })
                        var listInfo = get_listInfo("buy_list");
                        $("#money").text(listInfo.money);
                        $("#pro_weight").text(listInfo.weight)
                        set_Table_Colors("buy_list", data.dt);
                    }


                    layer.alert(errMsg, {
                        icon: 2
                    });
                } else if (data.flag == '1') {
                    $('#submit_buy_list').addClass('layui-btn-disabled').prop('disabled', true);
                    layer.open({
                        title: ['提交成功', 'background:#009E94;color:#fff'],
                        content: "你的订单已提交成功,订单号为:<br />" + data.message,
                        cancel: function(index, layero) {
                            window.location.reload();
                        },
                        btn: ['打印'],
                        btn1: function() {
                            layer.open({
                                type: 2,
                                title: '网上订单--' + data.message,
                                shadeClose: true,
                                shade: 0.8,
                                area: ['800px', '80%'],
                                content: '/Tpl/PrintOrderTpl.html?strBillNo=' + data.message
                            });
                        }
                    });

                }
            },
            error: function(err) {

            }

        });
    }

//     $(document).on("click", "i", function() {
//         console.log($(this))
//         layer.tips("需求订单:"+$(this).attr('xOrderNo')+"<br>"+"需求数量:"+$(this).attr('xOrderNum')+"<br>开票单位:"+$(this).attr('cCusName'), $(this), {
//   tips: [1, '#3595CC'],
//   time: 4000
// });
//     })
    ///////////////////////////////////////////////////////////////////////////////////////////////////////
    // //提交正式订单
    // $(document).on("click", "#submit_buy_list", function() {
    //     $("#submit_buy_list").blur();

    //     if (check_form()) { //验证表单数据是否完整

    //         $.ajax({
    //             traditional: true,
    //             type: "Post",
    //             url: "../Handler/ProductHandler.ashx",
    //             dataType: "Json",
    //             data: {
    //                 "Action": "DLproc_NewOrderByIns_new",
    //                 "formData": JSON.stringify(get_formData()),
    //                 "listData": JSON.stringify(get_listData("buy_list"))
    //             },
    //             success: function (data) {
    //                 if (data.flag == '5') {
    //                     layer.alert(data.message, {icon:2})
    //                     return false;
    //                 }
    //                 if (data.flag == '7') {
    //                     Product_limit(data, "buy_list");
    //                     return false;
    //                 }
    //                 if (data.flag == '0') {
    //                     var errMsg = "你的订单存在以下问题:<br />";
    //                     if (data.list_msg != null && data.list_msg.length > 0) {
    //                         for (var i = 0; i < data.list_msg.length; i++) {
    //                             errMsg += ((i + 1) + "、" + data.list_msg[i] + "<br />");
    //                         };
    //                     }
    //                     if (data.dt != null && data.dt.length > 0) {
    //                         $.each($("#buy_list tbody tr"), function(i, v) {
    //                             if ($(v).find(".code").text() == data.dt[i]["cInvCode"]) {
    //                                 $(v).find(".fAvailQtty").text(data.dt[i]["fAvailQtty"]);
    //                                 $(v).find(".ex_price").text(data.dt[i]["ExercisePrice"]);
    //                             }
    //                         })
    //                         var listInfo = get_listInfo("buy_list");
    //                         $("#money").text(listInfo.money);
    //                         $("#pro_weight").text(listInfo.weight)
    //                         set_Table_Colors("buy_list", data.dt);
    //                     }


    //                     layer.alert(errMsg, {
    //                         icon: 2
    //                     });
    //                 } else if (data.flag == '1') {
    //                     $('#submit_buy_list').addClass('layui-btn-disabled').prop('disabled', true);
    //                     // $('#Print').removeClass('layui-btn-disabled').prop('disabled', false).attr('strBillNo', data.message);
    //                     // layer.alert("你的订单已提交成功,订单号为:<br />" + data.message, {
    //                     //     icon: 1,
    //                     //     closeBtn: 1
    //                     // }, function() {
    //                     //    // window.location.reload();
    //                     //    layer.closeAll();

    //                     // });
    //                     layer.open({
    //                         title: ['提交成功', 'background:#009E94;color:#fff'],
    //                         content: "你的订单已提交成功,订单号为:<br />" + data.message,
    //                         cancel: function(index, layero) {
    //                             window.location.reload();
    //                         },
    //                         btn: ['打印'],
    //                         btn1: function() {
    //                             layer.open({
    //                                 type: 2,
    //                                 title: '网上订单--' + data.message,
    //                                 shadeClose: true,
    //                                 shade: 0.8,
    //                                 area: ['800px', '80%'],
    //                                 content: '/Tpl/PrintOrderTpl.html?strBillNo=' + data.message
    //                             });
    //                         }
    //                     });

    //                 }
    //             },
    //             error: function(err) {

    //             }

    //         });

    //     }

    // })

    //提取历史订单
    $(document).on("click", "#select_history_order", function() {
        // var kpdw = $("#TxtCustomer option:selected").val();
        //if (kpdw == 0 || kpdw == "" || kpdw == 'undefined' || kpdw == null) {
        //    layer.alert("请先选择开票单位!", {
        //        icon: 2
        //    });
        //    return false;
        //};
        layer.open({
            type: 2,
            area: ["550px", "520px"],
            title: "选择历史订单",
            content: "select_historyorder.html",
            success: function(layero, index) {},
            btn: ['确定'],
            btn1: function(index, layero) {
                var historyorder = layer.getChildFrame('#historyorder', index);
                var lngoporderid = $(historyorder).find(".select td:eq(3)").text();
                if (lngoporderid == "") {
                    layer.msg("你还未选择历史订单！");
                } else {
                    $.ajax({
                        url: "../Handler/ProductHandler.ashx",
                        data: {
                            "Action": "DLproc_BackOrderandPrvOrdercInvCodeIsBeLimited_All_Warehouse_BySel",
                            "id": lngoporderid,
                            "iBillType": 2

                        },
                        type: "Post",
                        dataType: "Json",
                        success: function(data) {
                            if (data.flag != 1) {
                                layer.alert(data.message, {
                                    icon: 2
                                });
                                return false;
                            }
                            if (data.dt != null) {
                                $("#buy_list tbody").html(get_html(data.datatable));
                                $("#buy_list tbody tr").find(".realqty").hide();
                            }

                            $("#strRemarks").val(data.msg[0]); //备注
                            $("#TxtCustomer").val(data.msg[5]) //开票单位
                            if (data.CusCredit_dt != null) {
                                if (data.CusCredit_dt[0].iCusCreLine > -99999998) { //信用
                                    $("#TxtCusCredit").text(data.CusCredit_dt[0].iCusCreLine);
                                } else {
                                    $("#TxtCusCredit").text("现金用户")
                                }
                                $("#cdiscountname").text(data.CusCredit_dt[0].cdiscountname); //酬宾类型
                            }
                            // 送货方式
                            if (data.msg[1] != "") {
                                var d;

                                if (data.msg[1] == "00") {
                                    //  $("input[name=shfs]:eq(1)").prop("checked", 'checked');
                                    $("#zt").prop("checked", true);
                                    d = "自提";
                                } else if (data.msg[1] == "01") {
                                    d = "配送";
                                    $("#ps").prop("checked", true);
                                    // $("input[name=shfs]:eq(0)").prop("checked", 'checked');
                                }
                                form.render()
                                $.ajax({
                                    type: "Post",
                                    url: "../Handler/ProductHandler.ashx",
                                    data: {
                                        "Action": "DLproc_UserAddressZTBySelGroup",
                                        "shfs": d
                                    },
                                    dataType: "Json",
                                    success: function(res) {
                                        if (data.dt[0].iAddressType == '1') {
                                            get_ztpsAddress(1);
                                            $("#shipping_check").parent().removeClass('layui-hide');
                                            $('#zt').prop('checked', true);
                                            $('#txtAddress').val(data.dt[0].lngopUseraddressId);
                                            $('#txtArea').val(data.dt[0].chdefine49);
                                            form.render();

                                        } else if (data.dt[0].iAddressType == '3') {
                                            $("#shipping_info").removeClass('layui-hide').removeClass('layui-btn-danger').text('已填写完').attr('info', data.dt[0].chdefine21)
                                            $("#shipping_check").parent().removeClass('layui-hide')
                                            $("#shipping_check").prop('checked', true)
                                            $("input[name=shfs]:eq(1)").attr("checked", 'checked');

                                            get_ztpsAddress(3);
                                            $('#txtAddress').val(data.dt[0].lngopUseraddressId);
                                            html = '<option value="' + data.dt[0].chdefine49 + '">' + data.dt[0].cdefine8 + '</option>';
                                            $('#txtArea').html(html);
                                            form.render();

                                        } else if (data.dt[0].iAddressType == '2') {
                                            $('#ps').prop('checked', true);
                                            get_ztpsAddress(2);
                                            $('#txtAddress').val(data.dt[0].lngopUseraddressId);
                                            html = '<option value="' + data.dt[0].chdefine49 + '">' + data.dt[0].cdefine8 + '</option>';
                                            $('#txtArea').html(html);
                                            form.render();
                                        }

                                        /*仓库*/
                                        if (data.WareHouse_dt != null && data.WareHouse_dt.length > 0) {
                                            var html = "";
                                            $.each(data.WareHouse_dt, function(i, v) {
                                                html += '<option value=' + v["cWhCode"] + '>' + v["cWhName"] + '</option>';
                                            })
                                            $("#txtWareHouse").html(html);

                                            if (data.dt[0].chdefine51 == null || data.dt[0].chdefine51 == "") {
                                                $("#txtWareHouse").val("CD01").attr("oldwhcode", "CD01")
                                            } else {
                                                $("#txtWareHouse").val(data.dt[0].chdefine51).attr("oldwhcode", data.dt[0].chdefine51)
                                            }
                                        }
                                        $("#txtAddress").val(data.dt[0].lngopUseraddressId); //送货地址
                                        $("#txtArea").val(data.dt[0].chdefine49); //送货行政区
                                        form.render('select');

                                    },
                                    error: function(err) {
                                        alert("error");
                                    }
                                });
                            }

                            $("#strLoadingWays").val(data.msg[2]); //装车方式
                            $("#cdefine3").val(data.msg[3]); //车型
                            form.render();
                            layer.closeAll();
                            if (data.limit_name != null && data.limit_name.length > 0) {
                                var s = "有" + data.limit_name.length + "件商品未找到： <br/>",
                                    count = 1;
                                $.each(data.limit_name, function(i, v) {
                                    s = s + count + "、" + v + "<br/>";
                                    count++;
                                })
                                s = s + "剩余商品信息提取完成！"
                                layer.alert(s, {
                                    icon: 7
                                });
                            };
                            set_table_color("buy_list");
                            //   $("#money").text(get_money("buy_list"));
                            var listInfo = get_listInfo("buy_list");
                            $("#money").text(listInfo.money);
                            $("#pro_weight").text(listInfo.weight)
                            set_table_num("buy_list");

                        },
                        error: function(e) {
                            console.log(e);
                            alert("error-----" + e);
                        }
                    })
                }
            }

        })
    })


    //提取临时订单 
    $(document).on("click", "#select_temp_order", function() {
        // var kpdw = $("#TxtCustomer option:selected").val();
        //if (kpdw == 0 || kpdw == "" || kpdw == 'undefined' || kpdw == null) {
        //    layer.alert("请先选择开票单位!", {
        //        icon: 2
        //    });
        //    return false;
        //}
        layer.open({
            type: 2,
            area: ["550px", "520px"],
            title: "选择临时订单",
            content: "select_backorder.html",
            success: function(layero, index) {},
            btn: ['确定'],
            btn1: function(index, layero) {
                var backorder = layer.getChildFrame('#backorder', index);
                var backorderid = $(backorder).find(".select td:eq(3)").text();
                if (backorderid == "") {
                    layer.msg("你还未选择临时订单！");
                } else {
                    $.ajax({
                        url: "../Handler/ProductHandler.ashx",
                        data: {
                            "Action": "DLproc_BackOrderandPrvOrdercInvCodeIsBeLimited_All_Warehouse_BySel",
                            "id": backorderid,
                            "iBillType": 1
                        },
                        type: "Post",
                        dataType: "Json",
                        success: function(data) {
                            console.log(data);
                            layer.close(index);
                            if (data.flag != 1) {
                                layer.alert(data.message, {
                                    icon: 2
                                })
                                return false;
                            }

                            if (data.dt != null && data.dt.length > 0) {
                                $("#buy_list tbody").html(get_html_backOrder(data.dt));

                            } else {
                                $("#buy_list tbody").html("");
                            }

                            $("#strRemarks").val(data.msg[0]); //备注
                            $("#TxtCustomer").val(data.msg[5]) //开票单位
                            if (data.CusCredit_dt != null && data.CusCredit_dt.length > 0) {
                                if (data.CusCredit_dt[0].iCusCreLine > -99999998) { //信用
                                    $("#TxtCusCredit").text(data.CusCredit_dt[0].iCusCreLine);
                                } else {
                                    $("#TxtCusCredit").text("现金用户")
                                }
                                $("#cdiscountname").text(data.CusCredit_dt[0].cdiscountname); //酬宾类型
                            }

                            $("#zt").prop("checked", false);
                            $("#ps").prop("checked", false);
                            $("#txtArea").html('<option value>请先选择送货方式</option>');
                            $("#txtAddress").html('<option value>自提必须选择行政区</option>');
                            $("#shipping_check").prop("checked", false);
                            $("#shipping_check").parent().addClass("layui-hide");
                            $("#shipping_info").attr('info', '').addClass('layui-btn-danger').text("托运信息");
                            if (data.limit_name != null && data.limit_name.length > 0) {
                                var s = "有" + data.limit_name.length + "件商品未找到： <br/>",
                                    count = 1;
                                $.each(data.limit_name, function(i, v) {
                                    s = s + count + "、" + v + "<br/>";
                                    count++;
                                })
                                s = s + "剩余商品信息提取完成！"
                                layer.alert(s, {
                                    icon: 7
                                });
                            };

                            form.render('select');


                            $("#strLoadingWays").val(data.msg[2]); //装车方式
                            $("#cdefine3").val(data.msg[3]); //车型
                            form.render();


                            // $("#money").text(get_money("buy_list"));
                            var listInfo = get_listInfo("buy_list");
                            $("#money").text(listInfo.money);
                            $("#pro_weight").text(listInfo.weight)
                            set_table_num("buy_list");
                            set_table_color("buy_list");
                        },
                        error: function(e) {
                            alert("error-----" + e);
                        }
                    })
                }
            }

        })
    });



    //保存临时订单
    $(document).on("click", "#alert_buy_temp", function() {
        layer.prompt(function(val, index) {

            if ($("#buy_list tbody").find("input").length > 0) {
                layer.msg("购物清单里还有不正确的输入，请检查后重新输入", {
                    icon: 2
                });
                return false;
            };
            $.ajax({
                traditional: true,
                type: "Post",
                url: "../Handler/ProductHandler.ashx",
                dataType: "Json",
                data: {
                    "Action": "DLproc_AddOrderBackByIns_new",
                    "temp_name": val,
                    "formData": JSON.stringify(get_formData()),
                    "listData": JSON.stringify(get_listData("buy_list"))
                },
                success: function(data) {
                    layer.closeAll();
                    if (data.flag == '0') {
                        layer.alert(data.message, {
                            icon: 2
                        });
                    } else if (data.flag == '1') {
                        layer.alert("临时订单保存成功", {
                            icon: 1
                        }, function() {
                            layer.closeAll();
                            // window.location.reload();
                        });
                    }
                },
                error: function(err) {

                }

            });
        });
    })

    $(document).on('click', '#fastInput', function() { //用户自定义编码输入
        $('#cCusInvCode_div').toggleClass('layui-hide');
    })
    // .on('click', '#Print', function () {                         //打印页面
    //     if ($(this).attr('strBillNo') == undefined) {
    //         return false;
    //     }
    //     layer.open({
    //         type: 2,
    //         title: '网上订单--' + $(this).attr('strBillNo'),
    //         shadeClose: true,
    //         shade: 0.8,
    //         area: ['780px', '80%'],
    //         content: '/Tpl/PrintOrderTpl.html?strBillNo=' + $(this).attr('strBillNo'),
    //         btn: ["关闭"]

    //     });
    //     // window.open('http://localhost:19471/Tpl/PrintOrderTpl.html?strBillNo='+$(this).attr('strBillNo'))
    // })



})

// var X = XLSX;
// var ExcelData;
// var XW = {};
// //弹出上传窗口
// $(document).on("click", "#uploadExcel", function() {

//     if ($("#TxtCustomer option:selected").val() == 0) {
//         layer.msg("请选择开票单位！");
//         return false;
//     }


//     // var xlf = document.getElementById('uploadExcel');
//     //  if (!xlf.addEventListener) return;
//     //  function handleFile(e) {
//     //      read_Excel(e.target.files);
//     //      //xlf.value = '';
//     //  }
//     //  xlf.addEventListener('change', handleFile, false);
//     //  var html = '<div style="width:80%;margin:10px auto"><div>\
//     // <button type="button" class="layui-btn" id="uploadExcel"><i class="layui-icon"></i>上传文件</button><button class="layui-btn layui-btn-small" type="file" id="xlf">下载模板</button></div>\
//     //  <div style="color:red;font-size:16px"><i class="layui-icon" style="font-size: 20px;">&#xe607;</i> <strong>注意事项</strong></div></div>';
//     //  layer.open({
//     //      type: 1,
//     //      title: '导入Excel',
//     //      area: ['500px', '300px'],
//     //      content: html
//     //  })
// })

// $(document).on('change', '#uploadExcel', function(e) {

//     console.log(e.target.files[0].type)
//     var f = e.target.files[0];
//     var FileExt = f.name.substr(f.name.lastIndexOf('.')).toLowerCase()
//     if (FileExt != '.xls' && FileExt != '.xlsx') {
//         layer.alert('只能上传类型为xls或xlsx的文件！', {
//             icon: 2
//         });
//         return false;
//     }

//     var reader = new FileReader();
//     reader.onload = function(e) {
//         var data = e.target.result;
//         var wb = X.read(data, {
//             type: 'binary'
//         });
//         var result = {};
//         var FristSheetName = wb.SheetNames[0]
//         result['Sheet1'] = X.utils.sheet_to_json(wb.Sheets[FristSheetName]);
//         ExcelData = result;
//         console.log(ExcelData)

//         if (!ExcelData['Sheet1'][0].code) {
//             layer.alert('请将表格编码的列名设置为:code', {
//                 icon: 2
//             });
//             return false;
//             }

//         var codes=[];
//         $.each(ExcelData['Sheet1'],function(i,v){
//             codes.push(v.code)
//         })
//          console.log(codes)

//         $.each(codes,function(i,v){

//         })

//        var a= Check_Codes(codes);
//        console.log(a)
//         return false;

//         var kpdw = $("#TxtCustomer option:selected").val();
//         var areaid = "0";
//         if ($("#zt").prop("checked") && $("#txtArea").val() != '' && $("#txtArea").val() != undefined) {
//             areaid = $("#txtArea").val();
//         }
//          $.ajax({
//                 traditional: true,
//                 type: "Post",
//                 url: "../Handler/ProductHandler.ashx",
//                 dataType: "Json",
//                 data: {
//                     "Action": "GetcCusInvCodes",
//                     "codes": JSON.stringify(ExcelData['Sheet1']),
//                     "kpdw":kpdw,
//                     "areaid":areaid
//                 },
//                 success: function(res) {
//                     console.log(res)
//                 }
//             });

//     };
//     reader.readAsBinaryString(f);

// })

//  var Check_Codes=(function(){
//     var o={};
//     return function Check_Codes(codes){
//         $.each(codes,function(i,v){
//             if (!o[v]) {
//                 o[v]=1;
//             }else{
//                 o[v]+=1;
//             }
//         })
//         return o;
//     }
//  })();


/*传入后台产品数据，拼接为html返回给页面上显示,临时订单*/
function get_html_backOrder(data) {
    var html = "";
    $.each(data, function(i, v) {
        var unit_b = parseFloat(v.cInvDefine13).toString();
        var unit_m = parseFloat(v.cInvDefine14).toString();
        html += "<tr>";
        html += "<td class='ui-widget-content'><a href='javascript:void(0)'  class='del_this '>删除</a><a href='javascript:void(0)'  class='up_this'>上移</a><a href='javascript:void(0)'  class='down_this'>下移</a></td>";
        html += "<td class='SN' code=" + v.cInvCode + ">" + (i + 1) + "</td>";
        html += "<td class='cInvName' cinvdefine13=" + unit_b + " cinvdefine14=" + unit_m + " cunitid=" + v.cComUnitCode + "  kl=" + v.Rate + " iTaxRate=" + v.iTaxRate + ">" + v.cInvName + "</td>";
        html += "<td>" + (v.cInvStd == null ? "-" : v.cInvStd) + "</td>";
        html += "<td class='unitGroup'>" + unit_b + v.cComUnitName + "=" + unit_b.div(unit_m) + v.cInvDefine2 + "=1" + v.cInvDefine1 + "</td>";
        html += "<td class='in num_s'>" + (typeof(v.cComUnitQTY) == 'undefined' ? "" : v.cComUnitQTY) + "</td>";
        html += "<td class='cComUnitName'>" + v.cComUnitName + "</td>";
        html += "<td class='in num_m'>" + (typeof(v.cInvDefine2QTY) == 'undefined' ? "" : v.cInvDefine2QTY) + "</td>";
        html += "<td class='cInvDefine2'>" + v.cInvDefine2 + "</td>";
        html += "<td class='in num_b'>" + (typeof(v.cInvDefine1QTY) == 'undefined' ? "" : v.cInvDefine1QTY) + "</td>";
        html += "<td class='cInvDefine1'>" + v.cInvDefine1 + "</td>";
        html += "<td class='num'>" + (typeof(v.iquantity) == 'undefined' ? "" : v.iquantity) + "</td>";
        html += "<td class='fAvailQtty'>" + (v.fAvailQtty == null ? 0 : v.fAvailQtty) + "</td>";
        html += "<td class='pack'>" + (typeof(v.cDefine22) == 'undefined' ? "" : v.cDefine22) + "</td>";
        // html += "<td class='price'>" + v.cComUnitPrice + "</td>";
        html += "<td class='price'>" + (v.ExercisePrice).toFixed(6) + "</td>";
        // html += "<td class='sum'>" + (typeof (v.cComUnitAmount) == 'undefined' ? "" : (v.cComUnitAmount).toFixed(2)) + "</td>";
        html += "<td class='sum'>" + (v.iquantity * v.ExercisePrice).toFixed(2) + "</td>";
        html += "<td class='ex_price' style='display:none'>" + (v.ExercisePrice).toFixed(6) + "</td><td style='display:none' class='ex_sum'>" + (v.iquantity * v.ExercisePrice).toFixed(2) + "</td>";
        html += '<td class="code" style="display:none">' + v.cInvCode + '</td>';
        html += '<td style="display:none" class="unit_m">' + unit_m + '</td>';
        html += '<td style="display:none" class="unit_b">' + unit_b + '</td>';
        html += '<td style="display:none" class="weight">' + v.iInvWeight + '</td>';
        html += "</tr>";
    })
    return html;
}

/*传入后台产品数据，拼接为html返回给页面上显示,历史订单*/
function get_html_historyOrder(data) {
    var html = "";
    $.each(data, function(i, v) {
        var unit_b = parseFloat(v.cInvDefine13).toString();
        var unit_m = parseFloat(v.cInvDefine14).toString();
        html += "<tr>";
        html += "<td class='ui-widget-content'><a href='javascript:void(0)'  class='del_this '>删除</a><a href='javascript:void(0)'  class='up_this'>上移</a><a href='javascript:void(0)'  class='down_this'>下移</a></td>";
        html += "<td class='SN' code=" + v.cInvCode + ">" + (i + 1) + "</td>";
        html += "<td class='cInvName' cinvdefine13=" + unit_b + " cinvdefine14=" + unit_m + " cunitid=" + v.cComUnitCode + "  kl=" + v.Rate + " iTaxRate=" + v.iTaxRate + ">" + v.cInvName + "</td>";
        html += "<td>" + (v.cInvStd == null ? "-" : v.cInvStd) + "</td>";
        html += "<td class='unitGroup'>" + unit_b + v.cComUnitName + "=" + unit_b.div(unit_m) + v.cInvDefine2 + "=1" + v.cInvDefine1 + "</td>";
        html += "<td class='in num_s'>" + (typeof(v.cComUnitQTY) == 'undefined' ? "" : v.cComUnitQTY) + "</td>";
        html += "<td class='cComUnitName'>" + v.cComUnitName + "</td>";
        html += "<td class='in num_m'>" + (typeof(v.cInvDefine2QTY) == 'undefined' ? "" : v.cInvDefine2QTY) + "</td>";
        html += "<td class='cInvDefine2'>" + v.cInvDefine2 + "</td>";
        html += "<td class='in num_b'>" + (typeof(v.cInvDefine1QTY) == 'undefined' ? "" : v.cInvDefine1QTY) + "</td>";
        html += "<td class='cInvDefine1'>" + v.cInvDefine1 + "</td>";
        html += "<td class='num'>" + (typeof(v.iquantity) == 'undefined' ? "" : v.iquantity) + "</td>";
        html += "<td class='fAvailQtty'>" + (v.fAvailQtty == null ? 0 : v.fAvailQtty) + "</td>";
        html += "<td class='pack'>" + (typeof(v.cDefine22) == 'undefined' ? "" : v.cDefine22) + "</td>";
        // html += "<td class='price'>" + v.cComUnitPrice + "</td>";
        html += "<td class='price'>" + (v.ExercisePrice).toFixed(6) + "</td>";
        // html += "<td class='sum'>" + (typeof (v.cComUnitAmount) == 'undefined' ? "" : (v.cComUnitAmount).toFixed(2)) + "</td>";
        html += "<td class='sum'>" + (v.iquantity * v.ExercisePrice).toFixed(2) + "</td>";
        html += "<td class='ex_price' style='display:none'>" + (v.ExercisePrice).toFixed(6) + "</td><td style='display:none' class='ex_sum'>" + (v.iquantity * v.ExercisePrice).toFixed(2) + "</td>";
        html += '<td class="code" style="display:none">' + v.cInvCode + '</td>';
        html += '<td style="display:none" class="unit_m">' + unit_m + '</td>';
        html += '<td style="display:none" class="unit_b">' + unit_b + '</td>';
        html += '<td style="display:none" class="weight">' + v.iInvWeight + '</td>';
        html += "</tr>";
    })
    return html;
}