var codes = []; //全局变量，本页面添加和删除产品均会在此变量里反映
var product_codes = [];
var cstcode = 00; //全局变量，销售类型，用于传入选择商品页面
var iShowType = 1;
var isModify = 1;
var isSpecial = 0; //是否是特殊订单，0为否，1为是；用于Change_KPDW方法


//JQ ajax全局事件
$(document).ajaxStart(function() {
    layer.load();
}).ajaxComplete(function(request, status) {
    layer.closeAll('loading');
});

layui.use(['element', 'util', 'laydate', 'form'], function() {
    var form = layui.form(),
        util = layui.util,
        laydate = layui.laydate,
        element = layui.element();

    $("#view").load("../tpl/buy_orderTpl.html?v=" + new Date().getTime(), function (da, status, XHR) {
        if (status == 'success') {
            $("#select_SpecialProduct").hide();
            $("#select_history_order").hide();
            $("#select_temp_order").hide();
            $("#alert_buy_temp").hide();
            $("#refresh").attr("page", "modify_buy");
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
                    if (data.flag == "5") {
                        layer.alert(data.message, {
                            icon: 2,
                            closeBtn: 0
                        }, function() {
                            window.location = "Reject_Order.html";
                        })
                        return false;
                    }
                    //开票单位
                    $.each(data.kpdw_dt, function(i, v) {
                        $("#TxtCustomer").append('<option cCusPPerson=' + v.cCusPPerson + ' value="' + v.cCusCode + '">' + v.cCusName + '</option>')
                    });
                    form.render();
                    var time = new Date();
                    data["msg"] = time.toLocaleDateString().replace(/\//g, "-");
                    $("#select_SpecialProduct").hide();
                    $("#buy_list thead tr th:eq(12)").hide();
                    $("#datCreateTime").text(data.msg); //订单日期
                    // $("#TxtCusCredit").text(data.CusCredit_dt[0].iCusCreLine); //信用 
                    //$("#cdiscountname").text(data.CusCredit_dt[0].cdiscountname);  //酬宾类型

                    var html = "";
                    $.each(data.CarType_dt, function(i, v) {
                        html += '<option value=' + v.cValue + '>' + v.cValue + '</option>';
                    });
                    $("#cdefine3").append(html);

                    $.ajax({
                        url: "../Handler/ProductHandler.ashx",
                        dataType: "Json",
                        type: "Post",
                        data: {
                            "Action": "GetModifyOrderDetail",
                            "strBillNo": GetQueryString("strBillNo"),
                            "iShowType": 1
                        },
                        aysnc: false,
                        success: function(data) {
                            console.log(data);
                            $("#TxtCustomer").val(data.dt[0].ccuscode); //开票单位
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
                            $("#datDeliveryDate").val(data.dt[0].datDeliveryDate.replace("T", " ")) //提货时间


                            $("#txtAddress").val(data.dt[0].lngopUseraddressId); //送货地址
                            $("#txtArea").val(data.dt[0].chdefine49); //送货行政区

                            cSTCode = data.dt[0].cSTCode;
                            if (cSTCode == '00') {
                                $("#cSTCode").text("普通销售");
                            }
                            if (cSTCode == '01') {
                                $("#cSTCode").text("样品资料");
                            }
                            $("#strBillNo").text(data.dt[0].strBillNo); //订单号
                            $("#strUserName").text(data.dt[0].ccusname); //制单人

                            $("#strLoadingWays").val(data.dt[0].strLoadingWays); //装车方式
                            $("#cdefine3").val(data.dt[0].cdefine3); //车型
                            $("#strRemarks").val(data.dt[0].strRemarks); //备注
                            form.render();

                            $("#buy_list tbody").html(get_html_ModifyOrder(data.datatable));
                            $("#buy_list tbody tr td.realqty").hide();
                            // $("#money").text(get_money("buy_list"));
                            var listInfo = get_listInfo("buy_list");
                            $("#money").text(listInfo.money);
                            $("#pro_weight").text(listInfo.weight)
                            if (data.limit_name != null && data.limit_name.length > 0) {
                                var errMsg = "列表里有以下商品未找到:<br />";
                                $.each(data.limit_name, function(i, v) {
                                    errMsg += (i + 1 + "、 " + v + "<br />");
                                });
                                errMsg += "其余商品已全部提取"
                                layer.alert(errMsg, {
                                    icon: 7
                                });
                            }

                        }
                    });
                }
            });



        }
    })

    //    //提交正式订单
    //$("#submit_buy_list").click(function () {

    //    var $TxtCustomer = $("#TxtCustomer option:selected");
    //    var $txtAddress = $("#txtAddress option:selected");
    //    var $txtArea = $("#txtArea");
    //    var $cdefine3 = $("#cdefine3 option:selected");

    //    if ($("#TxtCustomer option:selected").val() == 0) {
    //        layer.msg("请选择开票单位！");
    //        return false;
    //    }
    //    if ($("#txtAddress option:selected").val() == 0) {
    //        layer.msg("请选择送货地址！");
    //        return false;
    //    }
    //    if ($("#txtArea option:selected").val() == 0 && $('body input[name="shfs"]:checked').val() == "自提") {
    //        layer.msg("请选择行政区！");
    //        return false;
    //    }
    //    if ($("#datDeliveryDate").val() == "") {
    //        layer.msg("请选择提货时间！");
    //        return false;
    //    }
    //    if ($("#cdefine3 option:selected").val() == 0) {
    //        layer.msg("请选择车型！");
    //        return false;
    //    }
    //    if ($("#buy_list tbody").find("tr").length == 0) {
    //        layer.msg("你还未选择商品", { icon: 2 });
    //        return false;
    //    }
    //    if ($("#buy_list tbody").find("input").length > 0) {
    //        layer.msg("购物清单里还有不正确的输入，请检查后重新输入", { icon: 2 });
    //        return false;
    //    }
    //    var flag = true;
    //    $.each($("#buy_list tr").find("td:eq(11)"), function (i, v) {
    //        if ($(v).text() == 0 || $(v).text() == "") {
    //            $(v).parent().addClass("red");
    //            flag = false;
    //        }
    //    })
    //    if (!flag) {
    //        layer.msg("购物清单里有未输入数量商品！", { icon: 2 });
    //        return false;
    //    }


    //    //拼接ajax发送的表体数据
    //    var trs = $("#buy_list tbody tr"), buy_list = [];
    //    $.each(trs, function (i, v) {
    //        var product = {};
    //        product.irowno = $(v).find("td:eq(1)").text();         //行号
    //        product.cinvcode = $(v).find("td:eq(1)").attr("code"); //存货编码
    //        product.cinvname = $(v).find("td:eq(2)").text();      //存货名称
    //        product.iquantity = $(v).find("td:eq(11)").text() != "" ? $(v).find("td:eq(11)").text() : "0";    //汇总数量
    //        product.iquotedprice = $(v).find("td:eq(14)").text();  //报价,保留5位小数,四舍五入
    //        product.itaxrate = $(v).find("td:eq(2)").attr("itaxrate");//税率
    //        product.kl = $(v).find("td:eq(2)").attr("kl");          //扣率
    //        product.cComUnitName = $(v).find("td:eq(6)").text();  //基本单位名称
    //        product.cInvDefine1 = $(v).find("td:eq(10)").text();  //大包装单位名称     
    //        product.cInvDefine2 = $(v).find("td:eq(8)").text();   //小包装单位名称 
    //        product.cInvDefine13 = $(v).find("td:eq(2)").attr("cInvDefine13");//大包装换算率
    //        product.cInvDefine14 = $(v).find("td:eq(2)").attr("cInvDefine14");//小包装换算率
    //        product.unitGroup = $(v).find("td:eq(4)").text();    //单位换算率组
    //        product.cComUnitQTY = $(v).find("td:eq(5)").text() != "" ? $(v).find("td:eq(5)").text() : "0";  //基本单位数量
    //        product.cInvDefine2QTY = $(v).find("td:eq(7)").text() != "" ? $(v).find("td:eq(7)").text() : "0"; //小包装单位数量
    //        product.cInvDefine1QTY = $(v).find("td:eq(9)").text() != "" ? $(v).find("td:eq(9)").text() : "0";//大包装单位数量
    //        product.itaxunitprice = $(v).find("td:eq(16)").text();//原币含税单价,即执行价格dt.Columns.Add("ExercisePrice"); //基本单位单价(执行价格)   16,保留5位
    //        product.cunitid = $(v).find("td:eq(2)").attr("cunitid");////基本计量单位编码
    //        product.cDefine22 = $(v).find("td:eq(13)").text();//表体自定义项22,包装量
    //        buy_list.push(product);
    //    });

    //    $.ajax({
    //        timeout: 10000,
    //        type: "Post",
    //        url: "Handler/BaseHandler.ashx",
    //        data: {
    //            "Action": "DLproc_NewOrderByUpd",
    //            "strBillNo": GetQueryString("strBillNo"),
    //            "ccuscode": $TxtCustomer.attr("value"),
    //            "ccusname": $TxtCustomer.text(),
    //            "cdiscountname": $("#cdiscountname").text(),
    //            "cdefine11": $txtAddress.text(),
    //            "cdefine12": $txtAddress.attr("strconsigneetel"),
    //            "strreceivingaddress": $txtAddress.attr("strreceivingaddress"),
    //            "cdefine10": $txtAddress.attr("strcarplatenumber"),
    //            "lngopUseraddressId": $txtAddress.attr("lngopUseraddressId"),
    //            "cdefine1": $txtAddress.attr("strdrivername"),
    //            "cdefine13": $txtAddress.attr("strdrivertel"),
    //            "cdefine2": $txtAddress.attr("stridcard"),
    //            "strdistrict": $txtAddress.attr("strdistrict"),
    //            "cdefine9": $txtAddress.attr("strconsigneename"),
    //            "txtArea": $txtArea.val(),
    //            "strRemarks": $("#strRemarks").val(),
    //            "datDeliveryDate": $("#datDeliveryDate").val(),
    //            "strLoadingWays": $("#strLoadingWays").val(),
    //            "cdefine3": $cdefine3.val(),
    //            "cSCCode": $("#ps").prop("checked") ? "01" : "00",
    //            "cpersoncode": $("#TxtCustomer option:selected").attr("ccuspperson"),
    //            "buy_list": JSON.stringify(buy_list)
    //        },
    //        success: function (data) {
    //            console.log(data);
    //            data = eval('(' + data + ')');
    //            console.log(data.messages);
    //            if (data.flag == 1) {
    //                layer.alert("有产品库存不足", { icon: 2 });
    //                var trs = $("#buy_list tbody tr");
    //                for (var i = 0; i < data.messages.length; i++) {
    //                    $.each(trs, function (n, v) {
    //                        if ($(v).find("td:eq(1)").attr("code") == data.messages[i][0]) {
    //                            $(v).find("td:eq(12)").text(data.messages[i][1]);
    //                        }
    //                    })
    //                }
    //                layer.closeAll('loading');
    //                set_table_color("buy_list");
    //            }
    //            else if (data.flag == 2) {
    //                layer.alert("账户信用不足", { icon: 3 });
    //            } else if (data.flag == 0) {
    //                //layer.msg("订单提交成功！", { icon: 1 });
    //                layer.alert("订单提交成功！订单编号为:\n" + data.message, { icon: 6 }, function () {
    //                    window.location.href = "Reject_Order.aspx";
    //                });
    //            }
    //        },
    //        err: function (err) {
    //            layer.alert(err);
    //        }
    //    })

    //})

})

//一键清除零库存
$(document).on("click", "#del_none", function() {
    layer.confirm("你确定要删除无库存的商品？", function() {
        del_none("buy_list", "code");

    })
});

//提交数据
$(document).on("click", "#submit_buy_list", function() {

    if (check_form()) { //验证表单数据是否完整

        $.ajax({
            traditional: true,
            type: "Post",
            url: "../Handler/ProductHandler.ashx",
            dataType: "Json",
            data: {
                "Action": "DLproc_NewOrderByUpd",
                "strBillNo": $("#strBillNo").text(),
                "formData": JSON.stringify(get_formData()),
                "listData": JSON.stringify(get_listData("buy_list")),
                "isModify": isModify,
                "strBillNo": $("#strBillNo").text(),
                "cWhCode": $("#txtWareHouse").val()
            },
            success: function(data) {

                if (data.flag == '2') {
                    layer.alert(data.message, {
                        icon: 2,
                        closeBtn: 0
                    }, function() {
                        window.location = "Reject_Order.html";
                    });
                    return false;
                } else if (data.flag == '3') {
                    layer.alert(data.message, {
                        icon: 2,
                        closeBtn: 0
                    }, function() {});
                    return false;
                } else if (data.flag == '7') {
                    Product_limit(data, "buy_list");
                    return false;
                } else if (data.flag == '0') {
                    if (data.dt == null) {
                        layer.alert(data.message, {
                            icon: 2
                        });
                    } else {
                        var errMsg = "你的订单存在以下问题:<br />";
                        for (var i = 0; i < data.list_msg.length; i++) {
                            errMsg += ((i + 1) + "、" + data.list_msg[i] + "<br />");
                        };
                        $.each($("#buy_list tbody tr"), function(i, v) {
                            if ($(v).find(".code").text() == data.dt[i]["cInvCode"]) {
                                $(v).find(".fAvailQtty").text(data.dt[i]["fAvailQtty"]);
                                $(v).find(".ex_price").text(data.dt[i]["ExercisePrice"]);
                            }
                        })
                        // $("#money").text(get_money("buy_list"));
                        var listInfo = get_listInfo("buy_list");
                        $("#money").text(listInfo.money);
                        $("#pro_weight").text(listInfo.weight)
                        set_Table_Colors("buy_list", data.dt);

                        layer.alert(errMsg, {
                            icon: 2
                        });
                    }
                } else if (data.flag == '1') {
                    // layer.alert("您的订单已提交成功,订单号为:<br />" + data.message, {
                    //     icon: 1,
                    //     closeBtn: 0
                    // }, function() {
                    //     window.location = "Reject_Order.html";
                    // });
                    layer.open({
                        title: ['提交成功', 'background:#009E94;color:#fff'],
                        content: "你的订单已提交成功,订单号为:<br />" + data.message,
                        cancel: function(index, layero) {
                            window.location = "Reject_Order.html";
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

})

$(document).on("click", "#btn", function() {
    window.location = "Reject_Order.html";
})

//传入后台产品数据，拼接为html返回给页面上显示
function get_html_ModifyOrder(data) {

    var html = "";
    $.each(data, function(i, v) {
        var unit_b = parseFloat(v.cInvDefine13).toString();
        var unit_m = parseFloat(v.cInvDefine14).toString();
        html += "<tr>";
        html += "<td class='ui-widget-content'><a href='javascript:void(0)'  class='del_this '>删除</a><a href='javascript:void(0)'  class='up_this'>上移</a><a href='javascript:void(0)'  class='down_this'>下移</a></td>";
        html += "<td class='SN' code=" + v.cInvCode + ">" + (i + 1) + "</td>";
        html += "<td class='cInvName' cinvdefine13=" + unit_b + " cinvdefine14=" + unit_m + " cunitid=" + v.cComUnitCode + "  kl=" + v.Rate + " iTaxRate=" + v.iTaxRate + ">" + v.cInvName + "</td>";
        html += "<td>" + (v.cInvStd == null ? "-" : v.cInvStd) + "</td>";
        html += "<td class='unitGroup'>" + v.UnitGroup + "</td>";
        // html += "<td class='unitGroup'>" + unit_b + v.cComUnitName + "=" + unit_b.div(unit_m) + v.cInvDefine2 + "=1" + v.cInvDefine1 + "</td>";
        html += "<td class='in num_s'>" + (typeof(v.cComUnitQTY) == 'undefined' ? "" : v.cComUnitQTY) + "</td>";
        html += "<td class='cComUnitName'>" + v.cComUnitName + "</td>";
        html += "<td class='in num_m'>" + (typeof(v.cInvDefine2QTY) == 'undefined' ? "" : v.cInvDefine2QTY) + "</td>";
        html += "<td class='cInvDefine2'>" + v.cInvDefine2 + "</td>";
        html += "<td class='in num_b'>" + (typeof(v.cInvDefine1QTY) == 'undefined' ? "" : v.cInvDefine1QTY) + "</td>";
        html += "<td class='cInvDefine1'>" + v.cInvDefine1 + "</td>";
        html += "<td class='num'>" + (typeof(v.iquantity) == 'undefined' ? "" : v.iquantity) + "</td>";
        html += "<td class='realqty'>" + v.iquantity + "</td>";
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