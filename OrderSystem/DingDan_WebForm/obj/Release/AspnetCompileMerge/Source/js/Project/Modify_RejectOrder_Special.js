var codes = []; //全局变量，本页面添加和删除产品均会在此变量里反映
var product_codes = [];
var cstcode = 00; //全局变量，销售类型，用于传入选择商品页面
var isModify = 1;
var strBillNo = '';

////JQ ajax全局事件
//$(document).ajaxStart(function () {
//    layer.load();
//}).ajaxComplete(function (request, status) {
//    layer.closeAll('loading');
//});

layui.use(['element', 'util', 'laydate', 'form'], function() {
    var form = layui.form(),
        util = layui.util,
        laydate = layui.laydate,
        element = layui.element();
    strBillNo = GetQueryString('strBillNo');
    $("#view").load("../tpl/buy_orderTpl.html?v=" + new Date().getTime(), function (da, status, XHR) {
        if (status == 'success') {
            $("#select_product").hide();
            $("#select_history_order").hide();
            $("#select_temp_order").hide();
            $("#alert_buy_temp").hide();
            $("#refresh").attr("page", "modify_special");
            $("#del_none").attr("id", "del_none_TS");
            $("#txtArea").attr("lay-filter", "txtArea_special");
            $("#txtAddress").attr("lay-filter", "txtAddress_special");
            $(".btns").append('  <input type="button" class="layui-btn layui-btn-small " id="btn" value="返回列表" />');
            $("#info_div").removeClass("layui-hide");
            $.ajax({
                url: "../Handler/ProductHandler.ashx",
                dataType: "Json",
                type: "Post",
                data: {
                    "Action": "Get_BaseInfo"
                },
                aysnc: false,
                success: function(data) {
                    var time = new Date();
                    data["msg"] = time.toLocaleDateString().replace(/\//g, "-");

                    //   $("#buy_list thead tr th:eq(12)").hide();
                    $("#datCreateTime").text(data.msg); //订单日期

                    //开票单位
                    //$.each(data.kpdw_dt, function (i, v) {
                    //    $("#TxtCustomer").append('<option cCusPPerson=' + v.cCusPPerson + ' value="' + v.cCusCode + '">' + v.cCusName + '</option>')
                    //});

                    var html = "";
                    $.each(data.CarType_dt, function(i, v) {
                        html += '<option value=' + v.cValue + '>' + v.cValue + '</option>';
                    });
                    $("#cdefine3").append(html);
                }
            });



            $.ajax({
                url: "../Handler/ProductHandler.ashx",
                dataType: "Json",
                type: "Post",
                data: {
                    "Action": "DLproc_QuasiModifyOrderDetail_CZTSBySel",
                    "strBillNo": strBillNo
                },
                aysnc: false,
                success: function(data) {
                    // $("#TxtCustomer").val(data.dt[0].ccuscode); 
                    $("#TxtCustomer_div").html('<span id="TxtCustomer" cpersoncode=' + data.dt[0].cpersoncode + ' ccuscode=' + data.dt[0].ccuscode + '>' + data.dt[0].ccusname + '</span>') //开票单位
                    if (data.CusCredit_dt[0].iCusCreLine > -99999998) { //信用
                        $("#TxtCusCredit").text(data.CusCredit_dt[0].iCusCreLine);
                    } else {
                        $("#TxtCusCredit").text("现金用户")
                    }
                    $("#cdiscountname").text(data.CusCredit_dt[0].cdiscountname); //酬宾类型
                    kpdw = data.dt[0].ccuscode;
                    // 送货方式
                    var d;
                    var html = ''
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
                    // $.ajax({
                    //     type: "Post",
                    //     aysnc: false,
                    //     url: "../Handler/ProductHandler.ashx",
                    //     data: {
                    //         "Action": "DLproc_UserAddressZTBySelGroup",
                    //         "shfs": d
                    //     },
                    //     dataType: "Json",
                    //     success: function(res) {
                    //         var address = res.list_dt[0];
                    //         var area = res.list_dt[1];
                    //         $("#txtAddress").empty(); //清空地址下拉
                    //         $("#txtArea").empty(); //清空行政区下拉
                    //         $("#txtAddress").append("<option value>请选择送货地址</option>");
                    //         var option = "";
                    //         $.each(address, function(i, v) {
                    //             option = "<option value='" + address[i].lngopUseraddressId;
                    //             option += "' lngopUseraddressId='" + address[i].lngopUseraddressId;
                    //             option += "' strConsigneeTel='" + address[i].strConsigneeTel;
                    //             option += "' strReceivingAddress='" + address[i].strReceivingAddress;
                    //             option += "' strCarplateNumber='" + address[i].strCarplateNumber;
                    //             option += "' strDriverName='" + address[i].strDriverName;
                    //             option += "' strDriverTel='" + address[i].strDriverTel;
                    //             option += "' strIdCard='" + address[i].strIdCard;
                    //             option += "' strDistrict='" + address[i].strDistrict;
                    //             option += "' strConsigneeName='" + address[i].strConsigneeName + "'>";
                    //             option += address[i].ShippingInformation + "</option>";
                    //             $("#txtAddress").append(option);
                    //         });
                    //         $("#txtArea").append("<option value>自提必须选择行政区</option>");
                    //         $.each(area, function(i, v) {
                    //             $("#txtArea").append("<option value='" + area[i].ccodeID + "'>" + area[i].xzq + "</option>");
                    //         })


                    //         form.render('select');

                    //     },
                    //     error: function(err) {
                    //         alert("error");
                    //     }
                    // });



                    $("#cSTCode").text("特殊订单");
                    $("#strBillNo").text(data.dt[0].strBillNo); //订单号
                    $("#strUserName").text(data.dt[0].ccusname); //制单人
                    // $("#txtAddress").val(data.dt[0].lngopUseraddressId); //送货地址
                    // $("#txtArea").val(data.dt[0].chdefine49); //送货行政区
                    $("#strLoadingWays").val(data.dt[0].strLoadingWays); //装车方式
                    $("#cdefine3").val(data.dt[0].cdefine3); //车型
                    $("#strRemarks").val(data.dt[0].strRemarks); //备注
                    form.render();

                    $("#buy_list tbody").html(get_LoadTShtml(data.dt));
                    var listInfo = get_listInfo("buy_list");
                    $("#money").text(listInfo.money);
                    $("#pro_weight").text(listInfo.weight)


                }
            });
        }
    })



    //选择自提行政区，刷新信用额度及购物清单
    form.on('select(txtArea_special)', function(data) {
        if (data.value == 0 || data.value == "" || data.value == undefined || $("#TxtCustomer option:selected").val() == "" || $("input[name=shfs]:checked").val() == '配送') {
            return false;
        }
        var tds = $("#buy_list tbody tr").find(".code"),
            codes = [],
            $trs = $("#buy_list tbody tr");
        $.each($trs, function(i, v) {
            codes.push($(v).find(".itemid").text());
        })

        var areaid = 0;
        if ($("input[name=shfs]:checked").val() == '自提' && $("#txtArea").val() != "" && $("#txtArea").val() != undefined) {
            areaid = $("#txtArea").val();
        }

        var page = $("#refresh").attr("page");
        $.ajax({
            traditional: true,
            type: "Post",
            dataType: "Json",
            url: "../Handler/ProductHandler.ashx",
            data: {
                "Action": "Refresh",
                "codes": codes,
                "page": page,
                "areaid": areaid,
                "strBillNo": strBillNo
            },
            success: function(data) {
                if (data.dt.length > 0) {

                    $.each($("#buy_list tbody tr"), function(i, v) {
                        if ($(v).find(".itemid").text() == data.dt[i].code) {
                            $(v).find(".fAvailQtty").text(data.dt[i].fAvailQtty);
                            $(v).find(".price").text(data.dt[i].ExercisePrice);
                            $(v).find(".ex_price").text(data.dt[i].ExercisePrice);
                            $(v).find(".sum").text(($(v).find(".num").text() * data.dt[i].ExercisePrice).toFixed(2));
                            $(v).find(".ex_sum").text(($(v).find(".num").text() * data.dt[i].ExercisePrice).toFixed(2));
                        }
                    });
                }

                set_table_num("buy_list");
                $("#money").text(get_listInfo("buy_list").money);
                set_table_color("buy_list");

            },
            error: function(e) {
                layer.msg("数据刷新失败，请重试！", {
                    icon: 2
                });
            }
        })
    })


    //选择配送和自提托运，刷新价格以及自动填写行政区域
    form.on('select(txtAddress_special)', function(obj) {
        if ($('#ps').prop('checked') || $('#shipping_check').prop('checked')) {
            var html = '<option value="' + $('#txtAddress option:selected').attr('ccodeid') + '">' + $('#txtAddress option:selected').attr('strdistrict') + '</option>';
            $('#txtArea').html(html);
            form.render();
        }

        if ($("#buy_list tbody tr").length == 0  ||($('#zt').prop('checked') && !$('#shipping_check').prop('checked'))
            ) {
        console.log(obj)

            return false;
        }
        console.log('false')
        Change_Area();

    })


    //选择自提行政区，刷新信用额度及购物清单
    form.on('select(txtArea_special)', function(data) {

        if ($("#buy_list tbody tr").length == 0 || data.value == 0 || data.value == "" || data.value == undefined || $("#TxtCustomer option:selected").val() == "" || $("input[name=shfs]:checked").val() == '配送'||$('#shipping_check').prop('checked')) {

            return false;
        }
        Change_Area();
    })


    function Change_Area() {
        var tds = $("#buy_list tbody tr").find(".code"),
            codes = [],
            $trs = $("#buy_list tbody tr");
        $.each($trs, function(i, v) {
            codes.push($(v).find(".itemid").text());
        })

        var areaid = 0;
        if ($('#zt').prop('checked')) {
            if ($('#shipping_check').prop('checked')) {
                areaid=$('#txtAddress option:selected').attr('ccodeid');
            }else{
                areaid = $("#txtArea").val();
            }
        } 

        var page = $("#refresh").attr("page");
        $.ajax({
            traditional: true,
            type: "Post",
            dataType: "Json",
            url: "../Handler/ProductHandler.ashx",
            data: {
                "Action": "Refresh",
                "codes": codes,
                "page": page,
                "areaid": areaid,
                "strBillNo": strBillNo
            },
            success: function(data) {
                console.log(data)
                if (data.dt.length > 0) {

                    $.each($("#buy_list tbody tr"), function(i, v) {
                        if ($(v).find(".itemid").text() == data.dt[i].code) {
                            $(v).find(".fAvailQtty").text(data.dt[i].fAvailQtty);
                            $(v).find(".price").text(data.dt[i].ExercisePrice);
                            $(v).find(".ex_price").text(data.dt[i].ExercisePrice);
                            $(v).find(".sum").text(($(v).find(".num").text() * data.dt[i].ExercisePrice).toFixed(2));
                            $(v).find(".ex_sum").text(($(v).find(".num").text() * data.dt[i].ExercisePrice).toFixed(2));
                        }
                    });
                }

                set_table_num("buy_list");
                $("#money").text(get_listInfo("buy_list").money);
                set_table_color("buy_list");

            },
            error: function(e) {
                layer.msg("数据刷新失败，请重试！", {
                    icon: 2
                });
            }
        });
    }



})



$(document).on("click", "#select_SpecialProduct", function() { //点开产品选择页面
    kpdw = $("#TxtCustomer").attr("ccuscode");

    if (kpdw == 0 || kpdw == "" || kpdw == 'undefined' || kpdw == null) {
        layer.alert(" 开票单位不能为空!", {
            icon: 2
        });
        return false;
    };
    codes = get_TSCodes('itemid', 'buy_list');
    product_codes = get_TSCodes('itemid', 'buy_list');
    // console.log(codes);
    layer.open({
        type: 2,
        area: ["1050px", "600px"],
        title: "选择产品",
        content: "select_specialProduct.html",
        success: function(layero, index) {},
        btn: ['确定'],
        btn1: function(index, layero) {
            var add_codes = [];
            var tds = $("#buy_list tr").find(".itemid"); //不在清单里的产品才重新添加
            //遍历清单数组，如果数组中的元素不在全局变量codes中，则删除此行
            $.each(tds, function(i, v) {
                    if ($.inArray($(v).text(), codes) == -1) {
                        $(v).parents("tr").remove();
                    }
                })
                //遍历全局数组，如果数组中的元素不在清单数组product_codes中，则需要添加此产品
            $.each(codes, function(i, v) {
                if ($.inArray(v, product_codes) == -1) {
                    add_codes.push(v);
                }
            })
            if (add_codes.length > 0) {
                var areaid = 0;
                if ($("input[name=shfs]:checked").val() == '自提' && $("#txtArea").val() != "" && $("#txtArea").val() != undefined) {
                    areaid = $("#txtArea").val();
                }
                $.ajax({
                    traditional: true,
                    type: "Post",
                    url: "../Handler/ProductHandler.ashx",
                    dataType: "Json",
                    data: {
                        "Action": "DLproc_QuasiYOrderDetail_TSBySel",
                        "itemids": add_codes,
                        "isModify": isModify,
                        "strBillNo": strBillNo,
                        "areaid": areaid
                    },
                    success: function(data) {
                        console.log(data)
                        $("#buy_list tbody").append(get_TShtml(data.list_dt));
                        layer.msg("添加商品成功！");
                        layer.close(index);
                        set_table_num("buy_list");
                        set_table_color("buy_list");
                    }
                })
            } else {
                set_table_num("buy_list");
                layer.closeAll();
            }
        }

    })
}).on("click", "#del_none_TS", function() { //删除无库存产品
    layer.confirm("你确定要删除无库存的商品？", function() {
        del_none("buy_list", "itemid");
    })
}).on("click", "#submit_buy_list", function() { //提交表单
    if (check_form()) { //验证表单数据是否完整

        $.ajax({
            traditional: true,
            type: "Post",
            url: "../Handler/ProductHandler.ashx",
            dataType: "Json",
            data: {
                "Action": "DLproc_NewYOrderByUpd",
                "strBillNo": GetQueryString("strBillNo"),
                "kpdw": $("#TxtCustomer").attr("ccuscode"),
                "cpersoncode": $("#TxtCustomer").attr("cpersoncode"),
                "ccusname": $("#TxtCustomer").text(),
                "formData": JSON.stringify(get_formData()),
                "listData": JSON.stringify(get_listData("buy_list"))
            },
            success: function(data) {
                console.log(data);
                if (data.flag != 1) {
                    if (data.dt==null) {
                           layer.alert(data.message, {
                        icon: 2
                    });
                    }else {

                   
                    var errMsg = "你的订单存在以下问题:<br />";
                    for (var i = 0; i < data.list_msg.length; i++) {
                        errMsg += ((i + 1) + "、" + data.list_msg[i] + "<br />");
                    }
                    //更新可用量及库存量
                    var $trs = $("#buy_list tbody").find("tr");
                    $trs.each(function(i, v) {
                            for (var j = 0; j < data.dt.length; j++) {
                                if (data.dt[j].itemid == $(v).find(".itemid").text()) {
                                    $(v).find(".realqty").text(data.dt[j].realqty);
                                    $(v).find(".fAvailQtty").text(data.dt[j].fAvailQtty);
                                }
                            }
                        })
                        // set_table_color("buy_list");
                        //  set_TS_table_color("buy_list");

                    layer.alert(errMsg, {
                        icon: 2
                    });
                     }
                } else   {
                    // layer.alert("你的订单已提交成功,订单号为:<br />" + data.message, {
                    //     icon: 1
                    // }, function() {
                    //     window.location.href = "Reject_Order.html";
                    // });
                         layer.open({
                            title: ['提交成功', 'background:#009E94;color:#fff'],
                            content: "你的订单已提交成功,订单号为:<br />" + data.message,
                            cancel: function (index, layero) {
                                window.location = "Reject_Order.html";
                            },
                            btn: ['打印'],
                            btn1: function () {
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
                console.log(err);
            }

        })


    };

});



function get_TSCodes(css, table_id) {
    var arr = [];
    $tds = $("#" + table_id).find("." + css);
    $.each($tds, function(i, v) {
        arr.push($(v).text());
    });
    return arr;
}

//传入后台产品数据，拼接为html返回给页面上显示
function get_LoadTShtml(data) {
    var html = "";

    $.each(data, function(i, v) {

        var unit_b = (v.cInvDefine13).toString();
        var unit_m = (v.cInvDefine14).toString();

        html += "<tr>";
        html += "<td class='ui-widget-content'><a href='javascript:void(0)'  class='del_this '>删除</a><a href='javascript:void(0)'  class='up_this'>上移</a><a href='javascript:void(0)'  class='down_this'>下移</a></td>";
        html += "<td class='SN' code=" + v.cinvcode + ">" + (i + 1) + "</td>";
        html += "<td class='cInvName' cinvdefine13=" + unit_m + " cinvdefine14=" + unit_b + " cunitid=" + v.cComUnitCode + "  >" + v.cinvname + "</td>";
        html += "<td>" + v.cInvStd + "</td>";
        html += "<td class='unitGroup'>" + v.UnitGroup + "</td>";
        html += "<td class='in num_s'>" + v.cComUnitQTY + "</td>";
        html += "<td class='cComUnitName'>" + v.cComUnitName + "</td>";
        html += "<td class='in num_m'>" + v.cInvDefine2QTY + "</td>";
        html += "<td class='cInvDefine2'>" + v.cInvDefine2 + "</td>";
        html += "<td class='in num_b'>" + v.cInvDefine1QTY + "</td>";
        html += "<td class='cInvDefine1'>" + v.cInvDefine1 + "</td>";
        html += "<td class='num'>" + v.iquantity + "</td>";
        html += "<td class='realqty'>" + v.realqty + "</td>";
        html += "<td class='fAvailQtty'>" + v.dStckQty + "</td>";
        html += "<td class='pack'>" + v.cdefine22 + "</td>";
        //html += "<td class='price'>" + (v.iquotedprice) + "</td>";
        //html += "<td class='sum'>" + ((v.iquotedprice) * v.iquantity).toFixed(2) + "</td>";
        html += "<td class='price'>" + (v.itaxunitprice).toFixed(6) + "</td>";
        html += "<td class='sum'>" + ((v.itaxunitprice).toFixed(6) * v.iquantity).toFixed(2) + "</td>";
        html += "<td class='ex_price' style='display:none'>" + (v.itaxunitprice).toFixed(6) + "</td><td style='display:none' class='ex_sum'>" + ((v.itaxunitprice).toFixed(6) * v.iquantity).toFixed(2) + "</td>";
        html += '<td class="itemid" style="display:none">' + v.itemid + '</td>';
        html += '<td style="display:none" class="unit_m">' + unit_m + '</td>';
        html += '<td style="display:none" class="unit_b">' + unit_b + '</td>';
        html += '<td style="display:none" class="weight">' + v.iInvWeight + '</td>';
        html += "</tr>";
    })
    return html;
}

$(document).on("click", "#btn", function() {
    window.location = "Reject_Order.html";
})