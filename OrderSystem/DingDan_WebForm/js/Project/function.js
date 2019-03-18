var datatable_options = {
    "columns": [{
        "data": null,
        "title": "操作",
        "class": "center",
        "width": "80",
        render: function (data, type, row, meta) {
            return '<a>删除</a><a>上移</a><a>下移</a>';
        }
    }, {
        "data": null,
        "title": "序号",
        "class": "center",
        "width": "30"
    }, {
        "data": "cInvName",
        "title": "名称",
        "class": "center",
        "width": "200"
    }, {
        "data": "cInvStd",
        "title": "规格",
        "class": "center",
        "width": "100"
    }, {
        "data": null,
        "title": "单位组",
        "class": "center",
        "width": "80",
        render: function (data, type, row, meta) {
            return "";
        }
    }, {
        "data": null,
        "title": "基本数量",
        "class": "center in",
        "width": "60",
        render: function () {
            return "";
        }
    }, {
        "data": "cComUnitName",
        "title": "基本单位",
        "class": "center",
        "width": "60"
    }, {
        "data": null,
        "title": "小包装数量",
        "class": "center in",
        "width": "75",
        render: function () {
            return "";
        }
    }, {
        "data": "cInvDefine2",
        "title": "小包装单位",
        "class": "center",
        "width": "75",
    }, {
        "data": null,
        "title": "大包装数量",
        "class": "center in",
        "width": "75",
        render: function () {
            return "";
        }
    }, {
        "data": "cInvDefine1",
        "title": "大包装单位",
        "class": "center",
        "width": "75"
    }, {
        "data": null,
        "title": "基本单位汇总",
        "class": "center",
        "width": "90",
        render: function () {
            return "";
        }
    }, {
        "data": "fAvailQtty",
        "title": "可用库存量",
        "class": "center",
        "width": "100"
    }, {
        "data": null,
        "title": "包装结果",
        "class": "center",
        "width": "120",
        render: function () {
            return "";
        }
    }, {
        "data": "Quote",
        "title": "单价",
        "class": "center",
        "width": "60"
    }, {
        "data": null,
        "title": "金额",
        "class": "center",
        "width": "60",
        render: function () {
            return "";
        }
    }],
    "language": {
        "lengthMenu": "每页 _MENU_ 条记录",
        "zeroRecords": "没有找到记录",
        "info": "第 _PAGE_ 页 ( 总共 _PAGES_ 页 )",
        "infoEmpty": "无记录",
        "infoFiltered": "(从 _MAX_ 条记录过滤)"
    }
};

//点击按钮后去除焦点，避免按回车键时会重复触发该按钮
$(document).on("click", ".layui-btn", function () {
    $(this).blur();
})

function draw_table(table_id, data) {
    $("#" + table_id).DataTable({
        dom: "",
        paging: false,
        destroy: true,
        //scrollX: "1700",
        data: data,
        "columns": datatable_options.columns,
        "language": datatable_options.language

    })
}



//操作购物清单，输入数值获取结果
$(document).on("click", ".in", function () {

    var $this = $(this);
    var td = $(this);
    var tr = td.parents("tr");
    var tds = td.parent().find("td");
    if (td.parents("tbody").find("input").length > 0) {
        layer.msg("上次输入不正确，请重新输入", {
            icon: 2
        });
        return false;
    }
    var unit_s = 1,
        unit_m = tr.find(".unit_m").text(),
        unit_b = tr.find(".unit_b").text();
    var oldText = td.text();
    var input = $("<input type='text' value='" + oldText + "'/>");
    td.html(input);
    input.click(function () {
        return false;
    });
    input.css("border-width", "1px");
    input.css("font-size", "12px");
    input.css("text-align", "center");
    input.css("width", "20px");
    input.css("height", "20px");
    input.width((td.width() - 10));
    input.trigger("focus").trigger("select");
    input.blur(function () {
        val = input.val();
        if (td.index() == 5) {
            if ((val * 100) % (unit_m * 100) != 0 && td.next().text() == "米" && val != "") {
                layer.msg("请输入" + unit_m + "的整倍数！");
                td.text("");
                get_pack(td);
                var listInfo = get_listInfo("buy_list");
                $("#money").text(listInfo.money);
                $("#pro_weight").text(listInfo.weight)
                return false;
            } else if (td.next().text() != "米" && !(/^[0-9]\d*$/).test(val) && val != "") {
                layer.msg("输入数值不合法请重新输入");
                td.text("");
                get_pack(td);
                var listInfo = get_listInfo("buy_list");
                $("#money").text(listInfo.money);
                $("#pro_weight").text(listInfo.weight)
                return false;
            }
        } else {
            if (td.next().text() != "米" && !(/^[0-9]\d*$/).test(val) && val != "") {
                layer.msg("输入数值不合法请重新输入");
                td.text("");
                get_pack(td);
                var listInfo = get_listInfo("buy_list");
                $("#money").text(listInfo.money);
                $("#pro_weight").text(listInfo.weight)
                return false;
            }

        }
        var input_blur = $(this);

        var newText = input_blur.val();
        td.html(newText);

        get_pack(td);
        var listInfo = get_listInfo("buy_list");
        $("#money").text(listInfo.money);
        $("#pro_weight").text(listInfo.weight)

        if (parseFloat($this.parents("tr").find(".num").text()) > parseFloat($this.parents("tr").find(".fAvailQtty").text())) {
            $this.parents("tr").addClass("yellow");
        } else {
            $this.parents("tr").removeClass("yellow");
        }

        //var num_s = tr.find(".num_s").text(),
        //    num_m = tr.find(".num_m").text(),
        //    num_b = tr.find(".num_b").text();

        ////基本单位汇总
        //tr.find(".num").text((Number(num_s) + Number(num_m.mul(unit_m)) + Number(num_b.mul(unit_b))).toFixed(2));
        ////总金额
        //tr.find(".sum").text((tr.find(".num").text().mul(tr.find(".price").text())).toFixed(2));
        ////折扣金额
        //tr.find(".ex_sum").text(tr.find(".num").text().mul(tr.find(".ex_price").text()));
        ////包装结果
        //// tds.eq(13).text(tds.eq(5).text()+tds.eq(6).text());
        //if (((tr.find(".num").text().mul(100)) % ((unit_b).mul(100)) == 0)) {
        //    tr.find(".pack").text(parseInt((tr.find(".num").text()).div(unit_b)) + tr.find(".cInvDefine1").text());
        //} else {
        //    if ((((tr.find(".num").text().mul(100)) % (unit_b.mul(100))) % (unit_m.mul(100))) / 100 == 0) {
        //        tr.find(".pack").text(
        //            parseInt(tr.find(".num").text().div(unit_b)) + tr.find(".cInvDefine1").text() + parseInt(((tr.find(".num").text().mul(100)) %
        //                (unit_b.mul(100))) / (unit_m.mul(100))) + tr.find(".cInvDefine2").text());
        //    } else {
        //        tr.find(".pack").text(
        //            parseInt(tr.find(".num").text().div(unit_b)) + tr.find(".cInvDefine1").text() + parseInt(((tr.find(".num").text().mul(100)) %
        //                (unit_b.mul(100))) / (unit_m.mul(100))) + tr.find(".cInvDefine2").text() +
        //            parseInt(((tr.find(".num").text() * 100) % (unit_b * 100)) % (unit_m * 100)) / 100 + tr.find(".cComUnitName").text());
        //    }
        //}


        //执行金额
        // $("#money").text(get_money("buy_list"));
    });
}).on("click", ".del_this", function () { //绑定行删除事件
    var data_tr = $(this).parents("tr"); //获取到触发的tr  
    layer.confirm("确定要删除当前商品?", {
        icon: 3,
        title: '提示'
    }, function (index) {
        // var c = $(data_tr).find("td:eq(1)").attr("code");
        // if ($.inArray(c, codes) != -1) {
        //     codes.splice($.inArray(c, codes), 1);
        // }
        data_tr.remove();
        set_table_num("buy_list");
        var listInfo = get_listInfo("buy_list");
        $("#money").text(listInfo.money);
        $("#pro_weight").text(listInfo.weight)
        layer.close(index);
    })
}).on("click", ".up_this", function () { //绑定行上移事件
    var data_tr = $(this).parent().parent(); //获取到触发的tr  
    if ($(data_tr).prev().html() == null) { //获取tr的前一个相同等级的元素是否为空  
        alert("已经是最顶部了!");
        return;
    } {
        $(data_tr).insertBefore($(data_tr).prev()).fadeOut().fadeIn();
        set_table_num("buy_list"); //将本身插入到目标tr的前面   
    }
}).on("click", ".down_this", function () { //绑定行下移事件
    var data_tr = $(this).parent().parent();
    if ($(data_tr).next().html() == null) {
        alert("已经是最低部了!");
        return;
    } {
        $(data_tr).insertAfter($(data_tr).next()).fadeOut().fadeIn();
        set_table_num("buy_list"); //将本身插入到目标tr的后面   
    }
});


function get_pack(td) {

    var tr = $(td).parents("tr");
    var num_s = tr.find(".num_s").text(),
        num_m = tr.find(".num_m").text(),
        num_b = tr.find(".num_b").text();

    var unit_s = 1,
        unit_m = tr.find(".unit_m").text(),
        unit_b = tr.find(".unit_b").text();

    //基本单位汇总
    tr.find(".num").text((Number(num_s) + Number(num_m.mul(unit_m)) + Number(num_b.mul(unit_b))).toFixed(2));
    //总金额
    tr.find(".sum").text((tr.find(".num").text().mul(tr.find(".price").text())).toFixed(2));
    //折扣金额
    tr.find(".ex_sum").text(tr.find(".num").text().mul(tr.find(".ex_price").text()));
    //包装结果
    // tds.eq(13).text(tds.eq(5).text()+tds.eq(6).text());
    if (((tr.find(".num").text().mul(100)) % ((unit_b).mul(100)) == 0)) {
        tr.find(".pack").text(parseInt((tr.find(".num").text()).div(unit_b)) + tr.find(".cInvDefine1").text());
    } else {
        if ((((tr.find(".num").text().mul(100)) % (unit_b.mul(100))) % (unit_m.mul(100))) / 100 == 0) {
            tr.find(".pack").text(
                parseInt(tr.find(".num").text().div(unit_b)) + tr.find(".cInvDefine1").text() + parseInt(((tr.find(".num").text().mul(100)) %
                    (unit_b.mul(100))) / (unit_m.mul(100))) + tr.find(".cInvDefine2").text());
        } else {
            tr.find(".pack").text(
                parseInt(tr.find(".num").text().div(unit_b)) + tr.find(".cInvDefine1").text() + parseInt(((tr.find(".num").text().mul(100)) %
                    (unit_b.mul(100))) / (unit_m.mul(100))) + tr.find(".cInvDefine2").text() +
                parseInt(((tr.find(".num").text() * 100) % (unit_b * 100)) % (unit_m * 100)) / 100 + tr.find(".cComUnitName").text());
        }
    }


}



////执行金额,保留两位小数
//function get_money(table_id) {
//    var money = 0;
//    var trs = $("#" + table_id + " tbody tr");
//    $.each(trs, function (i, v) {
//        money = money + Number($(v).find(".ex_sum").text());
//    })
//    return money.toFixed(2);
//}



//执行金额,保留两位小数
function get_listInfo(table_id) {
    var money = 0;
    var weight = 0;
    var list_info = {};
    var trs = $("#" + table_id + " tbody tr");
    $.each(trs, function (i, v) {
        money = money + Number($(v).find(".ex_sum").text());
        weight = weight + ($(v).find(".weight").text() * $(v).find(".num").text())
    })
    list_info.money = money.toFixed(2);

    list_info.weight = (weight / 1000000).toFixed(4);
    if (list_info.weight == 0) {
        list_info.weight = "0吨"
    }

        //else if (0<list_info.weight<1) {
        //     list_info.weight="小于1吨"
        // } 
    else {
        list_info.weight = Math.floor(list_info.weight) + " ~ " + Math.ceil(list_info.weight) + " 吨";
    }
    return list_info;
}

//对table重新排序
function set_table_num(table_id) {
    var tds = $("#" + table_id + " tr").find("td:eq(1)");
    $.each(tds, function (i, v) {
        v.innerText = (i + 1);
    })
};

//获取产品列表里已选择的产品，拼接为数组
function get_codes(table_id) {
    var c = [];
    var tds = $("#" + table_id + " tr").find(".code");
    $.each(tds, function (i, v) {
        c.push($(v).text());
    })
    return c;
};

////转换日期格式，将格式为“/Date(-2036476800000+0800)/”转换为“2016-09-09”
//function return_date(date) {
//    if (date != "" && date != null) {
//        var new_date = date.slice(6, 19);
//        var time = new Date(Number(new_date));
//        return time.toLocaleDateString().replace(/\//g, "-");
//    }
//}
////转换日期格式，将格式为“/Date(-2036476800000+0800)/”转换为“2016-6-25 17:14:38”
//function return_datetime(date) {
//    if (date != "" && date != null) {
//        var new_date = date.slice(6, 19);

//        var time = new Date(Number(new_date));
//    return time.getFullYear() + "-" + (time.getMonth() + 1) + "-" + time.getDate() + " " + time.getHours() + ":" + time.getMinutes() + ":" + time.getSeconds();
//    }
//}
//转换日期格式，将格式为“/Date(-2036476800000+0800)/”转换为“2016-09-09”
function return_date(date) {

    if (date != "" && date != null) {
        if (date.indexOf("T") > -1) {

            var arr = date.split("T");
            var t = arr[0];

        } else {

            var new_date = date.slice(6, 19);
            var time = new Date(Number(new_date));
            var t = time.getFullYear() + "-";
            t = t + ((time.getMonth() + 1).toString().length == 2 ? (time.getMonth() + 1) : ("0" + (time.getMonth() + 1).toString())) + "-";
            t = t + ((time.getDate()).toString().length == 2 ? (time.getDate()) : ("0" + (time.getDate()).toString()));

        }
        return t;
        //return time.toLocaleDateString().replace(/\//g, "-");
    } else {
        return "";
    }
}


//转换日期格式，将格式为“/Date(-2036476800000+0800)/”转换为“2016-6-25 17:14:38”
function return_datetime(date) {
    if (date != "" && date != null) {
        if (date.indexOf("T") > -1) {
            var arr = date.split("T");
            var t = arr[0] + " " + arr[1].substr(0, 8);
            return t;
        } else {
            var new_date = date.slice(6, 19);
            var time = new Date(Number(new_date));
            var t = time.getFullYear() + "-";
            t = t + ((time.getMonth() + 1).toString().length == 2 ? (time.getMonth() + 1) : ("0" + (time.getMonth() + 1).toString())) + "-";
            t = t + ((time.getDate()).toString().length == 2 ? (time.getDate()) : ("0" + (time.getDate()).toString())) + " ";
            t = t + ((time.getHours()).toString().length == 2 ? (time.getHours()) : ("0" + (time.getHours()).toString())) + ":";
            t = t + ((time.getMinutes()).toString().length == 2 ? (time.getMinutes()) : ("0" + (time.getMinutes()).toString())) + ":";
            t = t + ((time.getSeconds()).toString().length == 2 ? (time.getSeconds()) : ("0" + (time.getSeconds()).toString()));
            // return time.getFullYear() + "-" +  (time.getMonth() + 1)+ "-" + time.getDate() + " " + time.getHours() + ":" + time.getMinutes() + ":" + time.getSeconds();
            return t;
        }

    } else {
        return "";
    }
}



//获取模板html
function get_tpl(Tpl) {
    $.ajax({
        url: "Tpl/" + Tpl + ".html",
        dataType: "html",
        async: false,
        success: function (h) {
            html_tpl = h;
        },
        error: function (err) {
            console.log(err);
        }
    })
}

layui.use(['form', 'util', 'laytpl', 'laydate', 'layer'], function () {
    var form = layui.form(),
        layer = layui.layer,
        util = layui.util,
        laydate = layui.laydate;


    //JQ ajax全局事件
    $(document).ajaxStart(function () {
        layer.load();
    }).ajaxComplete(function (request, status) {
        setTimeout(t, 50)
        //  layer.closeAll('loading');
    }).ajaxError(function () {
        layer.alert('程序出现错误,请重试或联系管理员!', {
            icon: 2
        })
        return false;
    });


    function t() {
        layer.closeAll('loading')
    }

    ////右下功能图标
    //util.fixbar({
    //    bar1: '&#x1007;',
    //    bar2: '&#xe613;',
    //    click: function (type) {
    //        if (type === 'bar1') {
    //            if ($("#jbxx").css("display") == "none") {
    //                layer.msg("显示基本信息");
    //                $("#jbxx").show();
    //            } else {
    //                $("#jbxx").hide();
    //                layer.msg("隐藏基本信息");
    //            }
    //        }

    //        if (type === 'bar2') {
    //            layer.msg('意见反馈')
    //        }

    //        if (type === 'top') {
    //            layer.msg('返回顶部')
    //        }
    //    }
    //});


    //$("#txtAddress_div").find(".layui-unselect").click(function () {
    //   layer.msg("请先选择送货方式后才可以选择地址")
    //})

    //用户自定义编码添加产品
    $(document).on('click', '#GetcCusInvCode', function () {
        GetcCusInvCode();
    })

    $(document).on('keyup', '.cusInvCode', function (e) {
        var keynum;
        // var keychar;
        keynum = window.event ? e.keyCode : e.which;
        // keychar = String.fromCharCode(keynum);
        if (keynum == 13) {
            //GetcCusInvCode();
            GetcCusInvCode();
        }
    })

    //用户自定义编码添加产品
    function GetcCusInvCode() {
        var kpdw = $("#TxtCustomer option:selected").val();
        if (kpdw == 0 || kpdw == "" || kpdw == 'undefined' || kpdw == null) {
            layer.msg("请先选择开票单位!", {
                icon: 2
            });
            $('.cusInvCode').blur();
            return false;
        }
        var code = $('#cCusInvCode').val().trim();
        if (code == '') {
            layer.msg('用户编码不能为空！', { icon: 2 });
            $('#cCusInvCode').focus();
            return false;
        }

        var areaid = "0";
        if ($("#zt").prop("checked") && $("#txtArea").val() != '' && $("#txtArea").val() != undefined) {
            areaid = $("#txtArea").val();
        }
        var hasCodes = [];
        var $hasCodes = $('#buy_list tr').find('.code');
        $.each($hasCodes, function (i, v) {
            hasCodes.push(v.innerText)
        })

        $.ajax({
            type: "Post",
            url: "../Handler/ProductHandler.ashx",
            data: {
                "Action": "GetcCusInvCode",
                "code": code,
                "kpdw": kpdw,
                "areaid": areaid
            },
            dataType: "Json",
            success: function (res) {
                if (res.flag != 1) {
                    layer.msg(res.message, { icon: 2 });
                    // $('.cusInvCode').blur();
                    $('.cusInvCode').val('');
                    $('#cCusInvCode').focus();
                    return false;
                }
                if ($.inArray(res.table[0].cInvCode, hasCodes) != -1) {
                    var s = '名称:' + res.table[0].cInvName + '<br/>规格:' + res.table[0].cInvStd + '<br/>已在列表中，请勿重复添加！'
                    layer.msg(s, { icon: 2 });
                    $('.cusInvCode').val('');
                    $('#cCusInvCode').focus();
                    return false;
                }
                $('#buy_list tbody').append(get_html(res.table));
                $("#buy_list tbody tr").find(".realqty").hide();

                //写入数量
                var num_s = $('#cusNum_s').val().trim() == '' ? 0 : $('#cusNum_s').val().trim();
                var num_m = $('#cusNum_m').val().trim() == '' ? 0 : $('#cusNum_m').val().trim();
                var num_b = $('#cusNum_b').val().trim() == '' ? 0 : $('#cusNum_b').val().trim();

                var $tr = $('#buy_list tbody tr').last();
                var unit_m = $tr.find(".unit_m").text();
                var eMsg = '';
                var codeconfig_select = $('#codeconfig_select').val();
                if ($tr.find('.cComUnitName').text() == "米") {
                    if ((num_s * 100) % (unit_m * 100) != 0 && num_s != "") {
                        if (codeconfig_select == 1) {
                            num_s = Math.ceil(num_s / unit_m).mul(unit_m);
                        } else if (codeconfig_select == 2) {
                            num_s = Math.floor(num_s / unit_m).mul(unit_m);
                        } else {
                            eMsg = "产品添加成功,但基本数量已重置为0,请重新在下方列表中输入！";
                            num_s = 0;
                        }

                    }
                } else {

                    if (!(/^[0-9]\d*$/).test(num_s)) {
                        if (codeconfig_select == 1) {
                            num_s = Math.ceil(num_s);
                        } else if (codeconfig_select == 2) {
                            num_s = Math.floor(num_s);
                        } else {
                            eMsg = "产品添加成功,但基本数量已重置为0,请重新在下方列表中输入！";
                            num_s = 0;
                        }

                    }
                }

                $tr.find('.num_s').text(num_s);
                $tr.find('.num_m').text(num_m);
                $tr.find('.num_b').text(num_b);

                $td = $tr.find('.num_s');
                get_pack($td);
                var listInfo = get_listInfo("buy_list");
                $("#money").text(listInfo.money);
                $("#pro_weight").text(listInfo.weight)

                set_table_num("buy_list");
                set_table_color("buy_list");
                $('#cCusInvCode').val('').focus();
                $('#cusNum_s').val('');
                $('#cusNum_m').val('');
                $('#cusNum_b').val('');
                if (eMsg != '') {
                    layer.msg(eMsg, { icon: 5 });
                } else {
                    layer.msg('添加商品成功！', { icon: 1 });
                }
            }
        });
    }


    //用户自定义编码输入数量时判断
    $(document).on('keyup', '#cusNum_s', function () {
        var val = $(this).val().trim();
        if (val != '' && isNaN(Number(val))) {
            layer.msg('请输入数字', { icon: 2 });
            $(this).val('');
        }
    }).on('keyup', '#cusNum_m', function () {
        var val = $(this).val().trim();
        if (val != '' && !(/^[0-9]\d*$/).test(val)) {
            layer.msg('请输入整数', { icon: 2 });
            $(this).val('');
        }
    }).on('keyup', '#cusNum_b', function () {
        var val = $(this).val().trim();
        if (val != '' && !(/^[0-9]\d*$/).test(val)) {
            layer.msg('请输入整数', { icon: 2 });
            $(this).val('');
        }
    }).on('focus', '.cusInvCode', function () {
        $('.cusInvCode').removeClass('inputFocus');
        $(this).addClass('inputFocus');
    })



    //通过radio点击， 获取送货地址及行政区
    form.on('radio(shfs)', function (data) {
        if (data.value == '自提') {
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



    function get_address(d) {
        $.ajax({
            type: "Post",
            url: "../Handler/ProductHandler.ashx",
            data: {
                "Action": "DLproc_UserAddressZTBySelGroup",
                "shfs": d
            },
            dataType: "Json",
            success: function (data) {
                var address = data.list_dt[0];
                var area = data.list_dt[1];
                $("#txtAddress").empty(); //清空地址下拉
                $("#txtArea").empty(); //清空行政区下拉
                if (d == '自提') {
                    $("#txtAddress").append("<option value>请选择自提地址</option>");
                } else {
                    $("#txtAddress").append("<option value>请选择配送地址</option>");
                }
                var option = "";
                $.each(address, function (i, v) {
                    option = "<option value='" + address[i].lngopUseraddressId;
                    option += "' lngopUseraddressId='" + address[i].lngopUseraddressId;
                    option += "' strConsigneeTel='" + address[i].strConsigneeTel;
                    option += "' strReceivingAddress='" + address[i].strReceivingAddress;
                    option += "' strCarplateNumber='" + address[i].strCarplateNumber;
                    option += "' strDriverName='" + address[i].strDriverName;
                    option += "' strDriverTel='" + address[i].strDriverTel;
                    option += "' strIdCard='" + address[i].strIdCard;
                    option += "' strDistrict='" + address[i].strDistrict;
                    option += "' strConsigneeName='" + address[i].strConsigneeName + "'>";
                    option += address[i].ShippingInformation + "</option>";
                    $("#txtAddress").append(option);
                });
                $("#txtArea").append("<option value>自提必须选择行政区</option>");
                $.each(area, function (i, v) {
                    // $("#txtArea").append("<option value='" + area[i].lngopUseraddress_exId + "'>" + area[i].xzq + "</option>");
                    $("#txtArea").append("<option value='" + area[i].ccodeID + "'>" + area[i].xzq + "</option>");

                })

                form.render('select');
                if (d == '自提') {
                    $("#shipping_check").parent().removeClass('layui-hide');
                } else if (d == '配送') {
                    $("#shipping_check").parent().addClass('layui-hide');
                    $("#shipping_check").prop('checked', false);
                    $("#shipping_info").addClass('layui-hide');
                    form.render();
                }

            },
            error: function (err) {
                console.log(err);
                layer.alert("请重试或联系管理员！", {
                    icon: 2
                });
                return false;
            }
        });
    }


    var count = 60; //倒计时时间
    var myCountDown;
    //点击获取验证码后，按钮倒记时
    function countDown() {

        $("#refresh").addClass("layui-btn-disabled");
        $("#refresh").attr("disabled", true);
        $("#refresh").val(count + " 秒后重新获取");

        count--;

        if (count == 0) {

            $("#refresh").val("刷新库存量").removeClass(" layui-btn-disabled").attr("disabled", false);

            clearInterval(myCountDown);

            count = 60;
        }
    }

    //刷新库存按键
    $(document).on("click", "#refresh", function () {
        $this = $(this);
        // Change_KPDW();
        //  myCountDown = setInterval(countDown, 1000);
        // return false;
        var page = $this.attr("page");
        $trs = $("#buy_list tbody").find("tr");
        if ($trs.length == 0) {
            layer.alert("你还没有选择商品!", {
                icon: 2
            })
            return false;
        }
        var areaid = "0";
        if ($("#zt").prop("checked") && $("#txtArea").val() != '' && $("#txtArea").val() != undefined) {
            areaid = $("#txtArea").val();
        }

        //更新普通订单库存
        if (page == 'buy') {
            var codes = [];
            $.each($trs, function (i, v) {
                codes.push($(v).find(".code").text());
            })
            $.ajax({
                traditional: true,
                type: "Post",
                dataType: "Json",
                async: false,
                url: "../Handler/ProductHandler.ashx",
                data: {
                    "Action": "Refresh",
                    "kpdw": $("#TxtCustomer option:selected").val(),
                    "codes": codes,
                    "page": page,
                    "areaid": areaid,
                    "cWhCode":$("#txtWareHouse").val()
                },
                success: function (data) {
                    if (data.flag == "1") {
                        count = 60;
                        $.each($trs, function (i, v) {
                            if ($(v).find(".code").text() == data.dt[i].cInvCode) {
                                $(v).find(".fAvailQtty").text(data.dt[i].fAvailQtty);
                                $(v).find(".price").text(data.dt[i].ExercisePrice);
                                $(v).find(".ex_price").text(data.dt[i].ExercisePrice);
                                $(v).find(".sum").text(($(v).find(".num").text() * data.dt[i].ExercisePrice).toFixed(2));
                                $(v).find(".ex_sum").text(($(v).find(".num").text() * data.dt[i].ExercisePrice).toFixed(2));
                            }
                        })

                        layer.msg("库存更新成功!", {
                            icon: 1,
                            time: 1000
                        })
                    } else {
                        count = 10
                        layer.alert(data.message, {
                            icon: 2
                        })
                    }
                    myCountDown = setInterval(countDown, 1000);
                },
                error: function (err) {
                    layer.alert(err, {
                        icon: 2
                    })
                }
            })
            set_table_color("buy_list");
            // $("#money").text(get_money("buy_list"));
            var listInfo = get_listInfo("buy_list");
            $("#money").text(listInfo.money);
            $("#pro_weight").text(listInfo.weight)
            return;
        }

        //修改被驳回普通订单 更新库存
        if (page == 'modify_buy') {
            var kpdw = $("#TxtCustomer option:selected").val();
            if (kpdw == '') {
                layer.alert('开票单位不能为空！', {
                    icon: 2
                })
                return false;
            }
            var codes = [];
            $.each($trs, function (i, v) {
                codes.push($(v).find(".code").text());
            })
            $.ajax({
                traditional: true,
                type: "Post",
                dataType: "Json",
                async: false,

                url: "../Handler/ProductHandler.ashx",
                data: {
                    "Action": "Refresh",
                    "kpdw": kpdw,
                    "codes": codes,
                    "page": page,
                    "strBillNo": GetQueryString("strBillNo"),
                    "areaid": areaid,
                    "cWhCode":$("#txtWareHouse").val()
                },
                success: function (data) {
                    if (data.flag == "1") {
                        count = 60;
                        $.each($trs, function (i, v) {
                            if ($(v).find(".code").text() == data.dt[i].cInvCode) {
                                $(v).find(".fAvailQtty").text(data.dt[i].fAvailQtty);
                                $(v).find(".price").text(data.dt[i].ExercisePrice);
                                $(v).find(".ex_price").text(data.dt[i].ExercisePrice);
                                $(v).find(".sum").text(($(v).find(".num").text() * data.dt[i].ExercisePrice).toFixed(2));
                                $(v).find(".ex_sum").text(($(v).find(".num").text() * data.dt[i].ExercisePrice).toFixed(2));
                            }
                        })

                        layer.msg("库存更新成功!", {
                            icon: 1,
                            time: 1000
                        })
                    } else {
                        count = 10
                        layer.alert(data.message, {
                            icon: 2
                        })
                    }
                    myCountDown = setInterval(countDown, 1000);
                },
                error: function (err) {
                    layer.alert(err, {
                        icon: 2
                    })
                }
            })
            set_table_color("buy_list");
            //   $("#money").text(get_money("buy_list"));
            var listInfo = get_listInfo("buy_list");
            $("#money").text(listInfo.money);
            $("#pro_weight").text(listInfo.weight)
            return;
        }

        //参照特殊订单 更新库存及可用量
        if (page == 'buy_special') {
            var codes = [];
            $.each($trs, function (i, v) {
                codes.push($(v).find(".itemid").text());
            })
            $.ajax({
                traditional: true,
                type: "Post",
                dataType: "Json",
                async: false,

                url: "../Handler/ProductHandler.ashx",
                data: {
                    "Action": "Refresh",
                    "codes": codes,
                    "page": page,
                    "areaid": areaid
                },
                success: function (data) {
                    if (data.flag == "1") {
                        count = 60;
                        $.each($trs, function (i, v) {
                         
                            if ($(v).find(".itemid").text() == data.dt[i].code) {
                                $(v).find(".realqty").text(data.dt[i].realqty);
                                $(v).find(".fAvailQtty").text(data.dt[i].fAvailQtty);
                                $(v).find(".price").text(data.dt[i].ExercisePrice);
                                $(v).find(".ex_price").text(data.dt[i].ExercisePrice);
                                $(v).find(".sum").text(($(v).find(".num").text() * data.dt[i].ExercisePrice).toFixed(2));
                                $(v).find(".ex_sum").text(($(v).find(".num").text() * data.dt[i].ExercisePrice).toFixed(2));
                            }
                        })
                        layer.msg("库存更新成功!", {
                            icon: 1,
                            time: 1000
                        })
                    } else {
                        count = 10;
                        layer.alert(data.message, {
                            icon: 2
                        })
                    }
                    myCountDown = setInterval(countDown, 1000);
                },
                error: function (err) {

                }
            })
            set_TS_table_color("buy_list");
            // $("#money").text(get_money("buy_list"));
            var listInfo = get_listInfo("buy_list");
            $("#money").text(listInfo.money);
            $("#pro_weight").text(listInfo.weight)
            return;
        }

        //修改被驳回的参照特殊订单 更新库存及可用量
        if (page == 'modify_special') {
            var codes = [];
            $.each($trs, function (i, v) {
                codes.push($(v).find(".itemid").text());
            })
            $.ajax({
                traditional: true,
                type: "Post",
                dataType: "Json",
                async: false,

                url: "../Handler/ProductHandler.ashx",
                data: {
                    "Action": "Refresh",
                    "codes": codes,
                    "page": page,
                    "strBillNo": GetQueryString("strBillNo"),
                    "areaid": areaid
                },
                success: function (data) {
                    if (data.flag == "1") {
                        count = 60;
                        $.each($trs, function (i, v) {
                            if ($(v).find(".itemid").text() == data.dt[i].code) {
                                $(v).find(".realqty").text(data.dt[i].realqty);
                                $(v).find(".fAvailQtty").text(data.dt[i].fAvailQtty);
                                $(v).find(".price").text(data.dt[i].nOriginalPrice);
                                $(v).find(".ex_price").text(data.dt[i].ExercisePrice);
                                $(v).find(".sum").text(($(v).find(".num").text() * data.dt[i].nOriginalPrice).toFixed(2));
                                $(v).find(".ex_sum").text(($(v).find(".num").text() * data.dt[i].ExercisePrice).toFixed(2));
                            }
                        })
                        layer.msg("库存更新成功!", {
                            icon: 1,
                            time: 1000
                        })
                    } else {
                        count = 10;
                        layer.alert(data.message, {
                            icon: 2
                        })
                    }
                    myCountDown = setInterval(countDown, 1000);
                },
                error: function (err) {

                }
            })
            set_TS_table_color("buy_list");
            //    $("#money").text(get_money("buy_list"));
            var listInfo = get_listInfo("buy_list");
            $("#money").text(listInfo.money);
            $("#pro_weight").text(listInfo.weight)
            return;
        }



    })



    function set_TS_table_color(table_id) {
        var $trs = $("#" + table_id + " tbody tr");
        $trs.each(function (i, v) {
            $(v).removeClass("red").removeClass("yellow");
            if ($(v).find(".fAvailQtty").text() == 0 || $(v).find(".realqty").text() == 0) {
                $(v).addClass("red");
            } else {
                if (($(v).find(".num").text() - $(v).find(".fAvailQtty").text()) > 0 || ($(v).find(".num").text() - $(v).find(".realqty").text()) > 0) {
            
                    $(v).addClass("yellow");
                }
            }
        })
    }

    //选择开票单位，刷新信用额度及购物清单
    form.on('select(kpdw)', function (data) {
         
        if (data.value == 0 || data.value == "") {
            return false;
        }
        Change_KPDW();
        // var tds = $("#buy_list tbody tr").find(".code"),
        //     c = [],
        //     trs = $("#buy_list tbody tr");
        // $.each(trs, function(i, v) {
        //     c.push($(v).find(".code").text());
        // });
        // var areaid = 0;
        // if ($("input[name=shfs]:checked").val() != undefined && $("input[name=shfs]:checked").val() == '自提' && $("#txtArea").val() != "" && $("#txtArea").val() != undefined) {
        //     areaid = $("#txtArea").val();
        // }
        // $.ajax({
        //     traditional: true,
        //     type: "Post",
        //     dataType: "Json",
        //     url: "../Handler/ProductHandler.ashx",
        //     data: {
        //         "Action": "Change_KPDW",
        //         "kpdw": data.value,
        //         "codes": c,
        //         "areaid": areaid
        //     },
        //     success: function(data) {

        //         if (data.CusCredit_dt[0].iCusCreLine != '-99999999.000000') {
        //             $("#TxtCusCredit").text(data.CusCredit_dt[0].iCusCreLine);
        //         } else {
        //             $("#TxtCusCredit").text("现金用户 余额:" + Number(data.message).toFixed(2));
        //         }
        //         $("#cdiscountname").text(data.CusCredit_dt[0].cdiscountname);
        //         if (data.list_msg != null && data.list_msg.length > 0) {
        //             var h = "你有" + data.list_msg.length + "件商品未找到<br />";
        //             $.each(trs, function(i, v) {
        //                 if ($.inArray($(v).find(".code").text(), data.list_msg) != -1) {
        //                     h += ((i + 1) + "、" + $(v).find(".cInvName").text() + "<br />");
        //                     $(v).remove();
        //                 }
        //             });
        //             h += '系统自动从购物单里删除';
        //             layer.alert(h, {
        //                 icon: 7
        //             });
        //         };

        //         if (data.list_dt.length > 0) {
        //             $.each($("#buy_list tbody tr"), function(i, v) {
        //                 if ($(v).find(".code").text() == data.list_dt[i][0].cInvCode) {
        //                     $(v).find(".fAvailQtty").text(data.list_dt[i][0].fAvailQtty);
        //                     $(v).find(".price").text(data.list_dt[i][0].ExercisePrice.toFixed(6));
        //                     $(v).find(".ex_price").text(data.list_dt[i][0].ExercisePrice.toFixed(6));
        //                     $(v).find(".sum").text(($(v).find(".num").text() * $(v).find(".ex_price").text()).toFixed(2));
        //                     $(v).find(".ex_sum").text(($(v).find(".num").text() * $(v).find(".ex_price").text()).toFixed(2));
        //                 }
        //             });
        //         }

        //         set_table_num("buy_list");
        //         $("#money").text(get_listInfo("buy_list").money);
        //         set_table_color("buy_list");

        //     },
        //     error: function(e) {
        //         layer.msg("数据刷新失败，请重试！", {
        //             icon: 2
        //         });
        //     }
        // })
    });


    //选择自提行政区，刷新信用额度及购物清单
    form.on('select(txtArea)', function (data) {
        if ($("#buy_list tbody tr").length == 0 || data.value == 0 || data.value == "" || data.value == undefined || $("#TxtCustomer option:selected").val() == "" || $("input[name=shfs]:checked").val() == '配送' || $('#shipping_check').prop('checked')) {
            return false;
        }

        Change_KPDW();
        // var tds = $("#buy_list tbody tr").find(".code"),
        //     c = [],
        //     trs = $("#buy_list tbody tr");
        // $.each(trs, function(i, v) {
        //     c.push($(v).find(".code").text());
        // });
        // var areaid = 0;
        // if ($("input[name=shfs]:checked").val() == '自提' && $("#txtArea").val() != "" && $("#txtArea").val() != undefined) {
        //     areaid = $("#txtArea").val();
        // }
        // $.ajax({
        //     traditional: true,
        //     type: "Post",
        //     dataType: "Json",
        //     url: "../Handler/ProductHandler.ashx",
        //     data: {
        //         "Action": "Change_KPDW",
        //         "kpdw": $("#TxtCustomer option:selected").val(),
        //         "codes": c,
        //         "areaid": areaid
        //     },
        //     success: function(data) {
        //         if (data.CusCredit_dt[0].iCusCreLine != '-99999999.000000') {
        //             $("#TxtCusCredit").text(data.CusCredit_dt[0].iCusCreLine);
        //         } else {
        //             $("#TxtCusCredit").text("现金用户 余额:" + Number(data.message).toFixed(2));
        //         }
        //         $("#cdiscountname").text(data.CusCredit_dt[0].cdiscountname);
        //         if (data.list_msg != null && data.list_msg.length > 0) {
        //             var h = "你有" + data.list_msg.length + "件商品未找到<br />";
        //             $.each(trs, function(i, v) {
        //                 if ($.inArray($(v).find(".code").text(), data.list_msg) != -1) {
        //                     h += ((i + 1) + "、" + $(v).find(".cInvName").text() + "<br />");
        //                     $(v).remove();
        //                 }
        //             });
        //             h += '系统自动从购物单里删除';
        //             layer.alert(h, {
        //                 icon: 7
        //             });
        //         };

        //         if (data.list_dt.length > 0) {
        //            $.each($("#buy_list tbody tr"), function(i, v) {
        //                 if ($(v).find(".code").text() == data.list_dt[i][0].cInvCode) {
        //                     $(v).find(".fAvailQtty").text(data.list_dt[i][0].fAvailQtty);
        //                     $(v).find(".price").text(data.list_dt[i][0].ExercisePrice.toFixed(6));
        //                     $(v).find(".ex_price").text(data.list_dt[i][0].ExercisePrice.toFixed(6));
        //                     $(v).find(".sum").text(($(v).find(".num").text() * $(v).find(".ex_price").text()).toFixed(2));
        //                     $(v).find(".ex_sum").text(($(v).find(".num").text() * $(v).find(".ex_price").text()).toFixed(2));
        //                 }
        //             });
        //         }

        //         set_table_num("buy_list");
        //         $("#money").text(get_listInfo("buy_list").money);
        //         set_table_color("buy_list");

        //     },
        //     error: function(e) {
        //         layer.msg("数据刷新失败，请重试！", {
        //             icon: 2
        //         });
        //     }
        // })
    })



    //选择自提（托运），加载送货地址类型为3的地址
    form.on('checkbox(shipping_check)', function (data) {
        if (this.checked) {
            get_ztpsAddress(3);
            $("#shipping_info").removeClass('layui-hide')
        } else {
            get_ztpsAddress(1);
            $("#shipping_info").addClass('layui-hide')
        }
        form.render();
    })



    //填写物流信息
    $(document).on('click', '#shipping_info', function () {
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
            success: function () {
                var info = $("#shipping_info").attr('info');
                if (info != "") {
                    var info_arr = info.split('|');
                    $("#companyName").val(info_arr[0]);
                    $("#contactName").val(info_arr[1]);
                    $("#contactPhone").val(info_arr[2]);

                }
            },
            btn1: function () {
                var i = $("#companyName").val().trim() + '|' + $("#contactName").val().trim() + '|' + $("#contactPhone").val().trim();
                if ($("#companyName").val().trim() != '' && $("#contactName").val().trim() != '' && $("#contactPhone").val().trim() != '') {
                    $("#shipping_info").attr('info', i).removeClass('layui-btn-danger').text('已填写完');
                } else {
                    layer.alert("信息未填写完整！", {
                        icon: 2
                    })
                    return false;
                }
                layer.closeAll();
            }
        });
    })


    //选择配送和自提托运，刷新价格以及自动填写行政区域
    form.on('select(txtAddress)', function (obj) {

        if ($('#ps').prop('checked') || $('#shipping_check').prop('checked')) {
            var html = '<option value="' + $('#txtAddress option:selected').attr('ccodeid') + '">' + $('#txtAddress option:selected').attr('strdistrict') + '</option>';
            $('#txtArea').html(html);
            form.render();
        }

        if ($("#buy_list tbody tr").length == 0 || $('#TxtCustomer').val() == 0 || $('#TxtCustomer').val() == "" || $('#TxtCustomer').val() == undefined ||
            $("#TxtCustomer option:selected").val() == "" || ($('#zt').prop('checked') && !$('#shipping_check').prop('checked'))) {
            return false;
        }
        Change_KPDW();

    })

    /*切换仓库时，刷新库存*/
    form.on('select(txtWareHouse)', function (data) {
          if (data.value==$("#txtWareHouse").attr("oldWhCode")) {
            return false;
          }
         $("#txtWareHouse").attr("oldWhCode", data.value);
            if ($("#buy_list tbody tr").length == 0 || $('#TxtCustomer').val() == 0 || $('#TxtCustomer').val() == "" || $('#TxtCustomer').val() == undefined ||
            $("#TxtCustomer option:selected").val() == "" ) {
            return false;
        }
        Change_KPDW();
    })

    //切换开票单位或行政区时，重新获取单价等信息
    function Change_KPDW() {
      var  oldWhCode=  $("#txtWareHouse").attr("oldWhCode");
        var tds = $("#buy_list tbody tr").find(".code"),
          c = [],
          trs = $("#buy_list tbody tr");
        $.each(trs, function (i, v) {
            c.push($(v).find(".code").text());
        });
        var areaid = 0;
        if ($('#zt').prop('checked')) {

            areaid = $("#txtArea").val();

        }
        var cWhCode = "";
        //if ($("#txtWareHouse").val() == "") {
        //    cWhCode = "CD01"
        //} else {
        //    cWhCode = $("#txtWareHouse").val()
        //}
        cWhCode = $("#txtWareHouse").val() == "" ? "CD01" : $("#txtWareHouse").val();
        $.ajax({
            traditional: true,
            type: "Post",
            dataType: "Json",
            url: "../Handler/ProductHandler.ashx",
            data: {
                "Action": "Change_KPDW",
                "kpdw": $('#TxtCustomer').val(),
                "codes": c,
                "areaid": areaid,
                "cWhCode": cWhCode,
                "isSpecial": isSpecial,
                "isModify":isModify,
                "strBillNo":GetQueryString("strBillNo")
            },
            success: function (data) {
                // if (data.WareHouse_dt.length > 0) {
                //     var html="";
                //     $.each(data.WareHouse_dt, function (i, v) {
                //         html += '<option value=' + v["cWhCode"] + '>' + v["cWhName"] + '</option>';
                //     })
                //     $("#txtWareHouse").html(html);
                // }  
                // $("#txtWareHouse").val(oldWhCode);
              
                if (data.CusCredit_dt[0].iCusCreLine != '-99999999.000000') {
                    $("#TxtCusCredit").text(data.CusCredit_dt[0].iCusCreLine);
                } else {
                    $("#TxtCusCredit").text("现金用户 余额:" + Number(data.message).toFixed(2));
                }
                $("#cdiscountname").text(data.CusCredit_dt[0].cdiscountname);
                if (data.list_msg != null && data.list_msg.length > 0) {
                    var h = "你有" + data.list_msg.length + "件商品未找到<br />";
                    $.each(trs, function (i, v) {
                        if ($.inArray($(v).find(".code").text(), data.list_msg) != -1) {
                            h += ((i + 1) + "、" + $(v).find(".cInvName").text() + "<br />");
                            $(v).remove();
                        }
                    });
                    h += '系统自动从购物单里删除';
                    layer.alert(h, {
                        icon: 7
                    });
                };
                 
                /*更新仓库*/

                if   (data.WareHouse_dt!=null&&data.WareHouse_dt.length > 0) {
               
                    var html="";
                    $.each(data.WareHouse_dt,function(i,v){
                        html+="<option value="+v.cWhCode+">"+v.cWhName+"</option>";
                    })
                    $("#txtWareHouse").html(html);
                    $("#txtWareHouse").val(data.cWhCode);
                    $("#txtWareHouse").attr("oldwhcode",data.cWhCode);
                   
                }
                  form.render();
                /*更新库存及价格*/
                if (data.datatable!=null&&data.datatable.length > 0) {
                    $.each($("#buy_list tbody tr"), function (i, v) {
                        $.each(data.datatable, function (m,n) {
                            if ($(v).find(".code").text() == n.cInvCode) {
                                $(v).find(".fAvailQtty").text(n.fAvailQtty);
                                $(v).find(".price").text(n.ExercisePrice.toFixed(6));
                                $(v).find(".ex_price").text(n.ExercisePrice.toFixed(6));
                                $(v).find(".sum").text(($(v).find(".num").text() * $(v).find(".ex_price").text()).toFixed(2));
                                $(v).find(".ex_sum").text(($(v).find(".num").text() * $(v).find(".ex_price").text()).toFixed(2));
                            }
                            return;
                        })
                       
                        //if ($(v).find(".code").text() == data.list_dt[i][0].cInvCode) {
                        //    $(v).find(".fAvailQtty").text(data.list_dt[i][0].fAvailQtty);
                        //    $(v).find(".price").text(data.list_dt[i][0].ExercisePrice.toFixed(6));
                        //    $(v).find(".ex_price").text(data.list_dt[i][0].ExercisePrice.toFixed(6));
                        //    $(v).find(".sum").text(($(v).find(".num").text() * $(v).find(".ex_price").text()).toFixed(2));
                        //    $(v).find(".ex_sum").text(($(v).find(".num").text() * $(v).find(".ex_price").text()).toFixed(2));
                        //}
                    });
                }

                set_table_num("buy_list");
                $("#money").text(get_listInfo("buy_list").money);
                set_table_color("buy_list");

            },
            error: function (e) {
                layer.msg("数据刷新失败，请重试！", {
                    icon: 2
                });
            }
        })
    }

    //备注信息弹窗
    $(document).on("click", "#strRemarks", function () {
        layer.open({
            type: 1,
            title: "备注",
            area: ['400px', '400px'],
            btn: ['确定', '取消'],
            // content: "<textarea style='width:296px;height:182px' id='Remarks'>" + $("#strRemarks").val() + "</textarea>",
            content: "<div style='width:95%;margin:10px auto'><form class='layui-form layui-form-pane'><div class='layui-form-item'><label  class='layui-form-label'>字数统计</label><label id='Remarks_WordsCount' class='layui-form-label'>23/200</label></div><textarea class='layui-textarea' style='height:220px' id='Remarks'></textarea></form></div>",
            success: function (layero, index) {

                $("#Remarks").focus().val($("#strRemarks").val());
                $("#Remarks_WordsCount").text($("#Remarks").val().trim().length + '/200')
            },
            btn1: function () {
                if ($("#Remarks").val().trim().length > 200) {
                    layer.alert('备注不能超过200个字', {
                        icon: 2
                    })
                    return false;
                }
                $("#strRemarks").val($("#Remarks").val());
                layer.closeAll();
            },
        })
    })

})


//备注字数统计
$(document).on('keyup', '#Remarks', function () {
    var l = $("#Remarks").val().trim().length;
    $("#Remarks_WordsCount").text(l + '/200')

    if (l > 200) {
        $("#Remarks_WordsCount").css('color', "red");
    } else {
        $("#Remarks_WordsCount").css('color', "#000");
    }

})



//弹出产品分类选择页面
$(document).on("click", "#select_product", function () {
    var kpdw = $("#TxtCustomer option:selected").val();
    if (kpdw == 0 || kpdw == "" || kpdw == 'undefined' || kpdw == null) {
        layer.alert("请先选择开票单位!", {
            icon: 2
        });
        return false;
    }
    product_codes = get_codes("buy_list");
    codes = get_codes("buy_list");
    layer.open({
        type: 2,
        offset: '10px',
        area: ["1020px", "490px"],
        title: false,
        content: "select_product.html?cSTCode=" + cstcode + "&kpdw=" + kpdw + "&iShowType=" + iShowType,
        success: function (layero, index) { },
        btn: ['确定'],
        btn1: function (index, layero) {
            // var product_codes = get_codes("buy_list"); //获取购物清单里所有的产品ID
            // codes= product_codes;
            var add_codes = [];
            var tds = $("#buy_list tr").find("td:eq(1)"); //不在清单里的产品才重新添加
            //遍历清单数组，如果数组中的元素不在全局变量codes中，则删除此行
            $.each(product_codes, function (i, v) {
                if ($.inArray(v, codes) == -1) {
                    $.each(tds, function (m, n) {
                        if ($(n).attr("code") == v) {
                            $(n).parents("tr").remove();

                        }
                    })
                }
            })

            //遍历全局数组，如果数组中的元素不在清单数组product_codes中，则需要添加此产品
            $.each(codes, function (i, v) {
                if ($.inArray(v, product_codes) == -1) {
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
                    url: "../Handler/ProductHandler.ashx",
                    dataType: "Json",
                    data: {
                        //"Action": "DLproc_QuasiOrderDetailBySel_new",
                        "Action":"DLproc_QuasiOrderDetail_All_Warehouse_BySel",
                        "codes": add_codes,
                        "kpdw": kpdw,
                        "isModify": isModify,
                        "isSpecial":isSpecial,
                        "strBillNo": $("#strBillNo").text(),
                        "areaid": areaid,
                        "cWhCode": $("#txtWareHouse").val() == "" ? "CD01" : $("#txtWareHouse").val()
                    },
                    success: function (data) {
                        if (data.flag !="1") {
                            layer.alert(data.message, {
                                icon: 2
                            });
                            return false;
                        }
                   
                        $("#buy_list tbody").append(get_html(data.dt));
                        $("#buy_list tbody tr").find(".realqty").hide();
                        layer.msg("添加商品成功！");
                        layer.close(index);
                        set_table_num("buy_list");
                        set_table_color("buy_list");

                        //自动保存临时订单
                        if (typeof (autoSave) == 'number') {

                            $.ajax({
                                traditional: true,
                                type: "Post",
                                url: "../Handler/ProductHandler.ashx",
                                dataType: "Json",
                                data: {
                                    "Action": "DLproc_AddOrderBackByIns_auto",
                                    "temp_name": "系统自动保存",
                                    "formData": JSON.stringify(get_formData()),
                                    "listData": JSON.stringify(get_listData("buy_list"))
                                },
                                success: function (data) {
                                    layer.closeAll();
                                    if (data.flag == '0') {
                                        layer.alert(data.message, {
                                            icon: 2
                                        });
                                    } else if (data.flag == '1') {
                                        //layer.alert(data.message, {
                                        //    icon: 1
                                        //}, function () {
                                        //    layer.closeAll();
                                        //    // window.location.reload();
                                        //});
                                        $("#autosave_msg").text(data.message);
                                    }
                                },
                                error: function (err) {

                                }

                            });
                        }


                    }
                })
            } else {
                set_table_num("buy_list");
                layer.closeAll();
            }
        }

    })
    //layer.full(index);

});

//传入后台产品数据，拼接为html返回给页面上显示
function get_html(data) {
    var html = "";
    $.each(data, function (i, v) {
        var unit_b = parseFloat(v.cInvDefine13).toString();
        var unit_m = parseFloat(v.cInvDefine14).toString();
        html += "<tr>";
        html += "<td class='ui-widget-content'><a href='javascript:void(0)'  class='del_this '>删除</a><a href='javascript:void(0)'  class='up_this'>上移</a><a href='javascript:void(0)'  class='down_this'>下移</a></td>";
        html += "<td class='SN' code=" + v.cInvCode + ">" + (i + 1) + "</td>";
        html += "<td class='cInvName' cinvdefine13=" + unit_b + " cinvdefine14=" + unit_m + " cunitid=" + v.cComUnitCode + "  kl=" + v.Rate + " iTaxRate=" + v.iTaxRate + ">" + v.cInvName + "</td>";
        html += "<td>" + v.cInvStd + "</td>";
        html += "<td class='unitGroup'>" + unit_b + v.cComUnitName + "=" + unit_b.div(unit_m) + v.cInvDefine2 + "=1" + v.cInvDefine1 + "</td>";
        html += "<td class='in num_s'></td>";
        html += "<td class='cComUnitName'>" + v.cComUnitName + "</td>";
        html += "<td class='in num_m'></td>";
        html += "<td class='cInvDefine2'>" + v.cInvDefine2 + "</td>";
        html += "<td class='in num_b'></td>";
        html += "<td class='cInvDefine1'>" + v.cInvDefine1 + "</td>";
        html += "<td class='num'></td>";
        html += "<td class='realqty'>" + v.iquantity + "</td>";
        html += "<td class='fAvailQtty'>" + v.fAvailQtty + "</td>";
        html += "<td class='pack'>" + (typeof (v.cDefine22) == 'undefined' ? "" : v.cDefine22) + "</td>";
        //html += "<td class='price'>" + v.Quote + "</td>";
        html += "<td class='price'>" + (v.ExercisePrice).toFixed(6) + "</td>";
        html += "<td class='sum'></td>";
        html += "<td class='ex_price' style='display:none'>" + (v.ExercisePrice).toFixed(6) + "</td><td style='display:none' class='ex_sum'></td>";
        html += '<td class="code" style="display:none">' + v.cInvCode + '</td>';
        html += '<td style="display:none" class="unit_m">' + unit_m + '</td>';
        html += '<td style="display:none" class="unit_b">' + unit_b + '</td>';
        html += '<td style="display:none" class="weight">' + v.iInvWeight + '</td>';
        html += "</tr>";
    })
    return html;
};

//传入后台产品数据，拼接为html返回给页面上显示
function get_html_num(data) {
    var html = "";
    $.each(data, function (i, v) {
        var unit_b = parseFloat(v.cInvDefine13).toString();
        var unit_m = parseFloat(v.cInvDefine14).toString();
        html += "<tr>";
        html += "<td class='ui-widget-content'><a href='javascript:void(0)'  class='del_this '>删除</a><a href='javascript:void(0)'  class='up_this'>上移</a><a href='javascript:void(0)'  class='down_this'>下移</a></td>";
        html += "<td class='SN' code=" + v.cInvCode + ">" + (i + 1) + "</td>";
        html += "<td class='cInvName' cinvdefine13=" + unit_b + " cinvdefine14=" + unit_m + " cunitid=" + v.cComUnitCode + "  kl=" + v.Rate + " iTaxRate=" + v.iTaxRate + ">" + v.cInvName + "</td>";
        html += "<td>" + (v.cInvStd == null ? "-" : v.cInvStd) + "</td>";
         html += "<td class='unitGroup'>" + v.UnitGroup + "</td>";
         // html += "<td class='unitGroup'>" + unit_b + v.cComUnitName + "=" + unit_b.div(unit_m) + v.cInvDefine2 + "=1" + v.cInvDefine1 + "</td>";
        html += "<td class='in num_s'>" + (typeof (v.cComUnitQTY) == 'undefined' ? "" : v.cComUnitQTY) + "</td>";
        html += "<td class='cComUnitName'>" + v.cComUnitName + "</td>";
        html += "<td class='in num_m'>" + (typeof (v.cInvDefine2QTY) == 'undefined' ? "" : v.cInvDefine2QTY) + "</td>";
        html += "<td class='cInvDefine2'>" + v.cInvDefine2 + "</td>";
        html += "<td class='in num_b'>" + (typeof (v.cInvDefine1QTY) == 'undefined' ? "" : v.cInvDefine1QTY) + "</td>";
        html += "<td class='cInvDefine1'>" + v.cInvDefine1 + "</td>";
        html += "<td class='num'>" + (typeof (v.cInvDefineQTY) == 'undefined' ? "" : v.cInvDefineQTY) + "</td>";
        html += "<td class='realqty'>" + v.iquantity + "</td>";
        html += "<td class='fAvailQtty'>" + (v.fAvailQtty == null ? 0 : v.fAvailQtty) + "</td>";
        html += "<td class='pack'>" + (typeof (v.cDefine22) == 'undefined' ? "" : v.cDefine22) + "</td>";
        // html += "<td class='price'>" + v.cComUnitPrice + "</td>";
        html += "<td class='price'>" + (v.ExercisePrice).toFixed(6) + "</td>";
        // html += "<td class='sum'>" + (typeof (v.cComUnitAmount) == 'undefined' ? "" : (v.cComUnitAmount).toFixed(2)) + "</td>";
        html += "<td class='sum'>" + (v.cInvDefineQTY * v.ExercisePrice).toFixed(2) + "</td>";
        html += "<td class='ex_price' style='display:none'>" + (v.ExercisePrice).toFixed(6) + "</td><td style='display:none' class='ex_sum'>" + (v.cInvDefineQTY * v.ExercisePrice).toFixed(2) + "</td>";
        html += '<td class="code" style="display:none">' + v.cInvCode + '</td>';
        html += '<td style="display:none" class="unit_m">' + unit_m + '</td>';
        html += '<td style="display:none" class="unit_b">' + unit_b + '</td>';
        html += '<td style="display:none" class="weight">' + v.iInvWeight + '</td>';
        html += "</tr>";
    })
    return html;
}


//传入后台产品数据，拼接为html返回给页面上显示
function get_TShtml(data) {
    var html = "";

    $.each(data, function (i, v) {

        var unit_b = (v[0].cInvDefine13).toString();
        var unit_m = (v[0].cInvDefine14).toString();

        html += "<tr>";
        html += "<td class='ui-widget-content'><a href='javascript:void(0)'  class='del_this '>删除</a><a href='javascript:void(0)'  class='up_this'>上移</a><a href='javascript:void(0)'  class='down_this'>下移</a></td>";
        html += "<td class='SN' code=" + v[0].cinvcode + ">" + (i + 1) + "</td>";
        html += "<td class='cInvName' cinvdefine13=" + unit_m + " cinvdefine14=" + unit_b + " cunitid=" + v[0].cComUnitCode + "  kl=" + v[0].Rate + " iTaxRate=" + v[0].iTaxRate + ">" + v[0].cinvname + "</td>";
        html += "<td>" + v[0].cInvStd + "</td>";
        html += "<td class='unitGroup'>" + v[0].UnitGroup + "</td>";
        html += "<td class='in num_s'></td>";
        html += "<td class='cComUnitName'>" + v[0].cComUnitName + "</td>";
        html += "<td class='in num_m'></td>";
        html += "<td class='cInvDefine2'>" + v[0].cInvDefine2 + "</td>";
        html += "<td class='in num_b'></td>";
        html += "<td class='cInvDefine1'>" + v[0].cInvDefine1 + "</td>";
        html += "<td class='num'></td>";
        html += "<td class='realqty'>" + v[0].realqty + "</td>";
        html += "<td class='fAvailQtty'>" + v[0].fAvailQtty + "</td>";
        html += "<td class='pack'></td>";
        html += "<td class='price'>" + v[0].itaxunitprice + "</td>";
        html += "<td class='sum'></td>";
        html += "<td class='ex_price' style='display:none'>" + (v[0].itaxunitprice).toFixed(6) + "</td><td style='display:none' class='ex_sum'></td>";
        html += '<td class="itemid" style="display:none">' + v[0].itemid + '</td>';
        html += '<td style="display:none" class="unit_m">' + unit_m + '</td>';
        html += '<td style="display:none" class="unit_b">' + unit_b + '</td>';
        html += '<td style="display:none" class="weight">' + v[0].iInvWeight + '</td>';
        html += "</tr>";
    })
    return html;
}

//提交表单时表头数据拼接为OBJ
function get_formData() {
    var $TxtCustomer = $("#TxtCustomer option:selected");
    var $txtAddress = $("#txtAddress option:selected");
    var $txtArea = $("#txtArea option:selected");
    var $cdefine3 = $("#cdefine3 option:selected");
    var formData = {};
    formData.ccuscode = $TxtCustomer.val(),
        formData.ccuspperson = $TxtCustomer.attr("ccuspperson"),
        formData.ccusname = $TxtCustomer.text(),
    // formData.cdiscountname= $("#TxtCusCredit").attr("cdiscountname"),
    // formData.cdefine11=$txtAddress.text(),
    // formData.cdefine12= $txtAddress.attr("strconsigneetel"),
    // formData.strreceivingaddress= $txtAddress.attr("strreceivingaddress"),
    // formData.cdefine10=$txtAddress.attr("strcarplatenumber");
    // formData.lngopUseraddressId=$txtAddress.attr("lngopUseraddressId");
    // formData.cdefine1=$txtAddress.attr("strdrivername");
    // formData.cdefine13=$txtAddress.attr("strdrivertel");
    // formData.cdefine2=$txtAddress.attr("stridcard");
    // formData.strdistrict=$txtAddress.attr("strdistrict");
    // formData.cdefine9=$txtAddress.attr("strconsigneename");
    formData.txtAddress = $txtAddress.text();
    formData.lngopUseraddressId = $txtAddress.val();
    formData.txtArea = $txtArea.text();
    formData.strRemarks = $("#strRemarks").val();
    formData.datDeliveryDate = $("#datDeliveryDate").val();
    formData.strLoadingWays = $("#strLoadingWays").val();
    formData.carType = $cdefine3.val();
    formData.cWhCode = $("#txtWareHouse option:selected").val();
    // formData.cSCCode = $("#ps").prop("checked") ? "01" : "00";
    if ($("#ps").prop("checked")) {
        formData.cSCCode = '01';
        formData.iAddressType = 2;
    } else {
        formData.cSCCode = '00';
        if ($("#shipping_check").prop('checked')) {
            formData.iAddressType = 3;
        } else {
            formData.iAddressType = 1;
        }
    };
    formData.chdefine21 = $('#shipping_info').attr('info')
    formData.areaid = $("#ps").prop("checked") ? '0' : $("#txtArea").val()
    return formData;
}


//提交表单时拼接表体数据为数组
function get_listData(table_id) {
    var trs = $("#" + table_id + " tbody tr"),
        buy_list = [];
    $.each(trs, function (i, v) {
        var product = {};
        product.irowno = $(v).find(".SN").text(); //行号
        product.cinvcode = $(v).find(".code").text(); //产品编码
        product.cinvname = $(v).find(".cInvName").text(); //存货名称
        product.iquantity = $(v).find(".num").text() != "" ? $(v).find(".num").text() : "0"; //汇总数量
        //product.iquotedprice = $(v).find(".price").text();  //报价,保留5位小数,四舍五入
        //product.itaxrate = $(v).find("td:eq(2)").attr("itaxrate");//税率
        //product.kl = $(v).find("td:eq(2)").attr("kl");          //扣率
        //product.cComUnitName = $(v).find("td:eq(6)").text();  //基本单位名称
        //product.cInvDefine1 = $(v).find("td:eq(10)").text();  //大包装单位名称     
        //product.cInvDefine2 = $(v).find("td:eq(8)").text();   //小包装单位名称 
        product.unitGroup = $(v).find(".unitGroup").text(); //单位换算率组
        product.cInvDefine13 = $(v).find(".unit_b").text(); //大包装换算率
        product.cInvDefine14 = $(v).find(".unit_m").text(); //小包装换算率
        //product.unitGroup = $(v).find("td:eq(4)").text();    //单位换算率组
        product.cComUnitQTY = $(v).find(".num_s").text() != "" ? $(v).find(".num_s").text() : "0"; //基本单位数量
        product.cInvDefine2QTY = $(v).find(".num_m").text() != "" ? $(v).find(".num_m").text() : "0"; //小包装单位数量
        product.cInvDefine1QTY = $(v).find(".num_b").text() != "" ? $(v).find(".num_b").text() : "0"; //大包装单位数量
        // product.itaxunitprice = $(v).find("td:eq(16)").text();//原币含税单价,即执行价格dt.Columns.Add("ExercisePrice"); //基本单位单价(执行价格)   16,保留5位
        product.cunitid = $(v).find("td:eq(2)").attr("cunitid"); ////基本计量单位编码
        product.cDefine22 = $(v).find(".pack").text(); //表体自定义项22,包装量
        product.itemid = $(v).find(".itemid").text();
        buy_list.push(product);
    });
    return buy_list;
}

//商品列表里有限销商品时,弹窗提示,并且删除列表里的商品
function Product_limit(data, table_id) {
    var trs = $("#" + table_id + " tbody tr");
    $.each(trs, function (i, v) {
        if ($.inArray($(v).find(".code").text(), data.limit_code) != -1) {
            $(v).remove();
        }
    });
    var errMsg = "列表里有以下商品未找到:<br />";
    $.each(data.limit_name, function (i, v) {
        errMsg += (i + 1 + "、 " + v + "<br />");
    });
    errMsg += "现已自动删除,请核实后重新提交表单!"
    layer.alert(errMsg, {
        icon: 7
    });
}


//加载商品清单后，判断清单里是否有零库存或者库存小于购买数量的商品，并标注相应行
// function set_table_color(table_id) {
//     var trs = $("#" + table_id + " tbody tr");
//     $.each(trs, function (i, v) {
//          $(v).removeClass("red").removeClass("yellow");
//         if ($(v).find("td:eq(12)").text() == 0) {
//             $(v).addClass("red");
//         } else if (($(v).find("td:eq(11)").text() - $(v).find("td:eq(12)").text()) > 0) {
//             $(v).addClass("yellow");
//         }
//     })
// };

function set_table_color(table_id) {
    var trs = $("#" + table_id + " tbody tr");
    $.each(trs, function (i, v) {

        $(v).removeClass("red").removeClass("yellow");
        if ($(v).find(".fAvailQtty").text() == 0) {
            $(v).addClass("red");
        } else if (($(v).find(".num").text() - $(v).find(".fAvailQtty").text()) > 0) {
            $(v).addClass("yellow");
        }
    })
};

//提交表单时先验证数据是否合法
function check_form() {
    var $TxtCustomer = $("#TxtCustomer option:selected");
    var $txtAddress = $("#txtAddress option:selected");
    var $txtArea = $("#txtArea");
    var $cdefine3 = $("#cdefine3 option:selected");

    if ($("#TxtCustomer option:selected").val() == 0) {
        layer.msg("请选择开票单位！");
        return false;
    }
    if ($("#txtAddress option:selected").val() == 0) {
        layer.msg("请选择送货地址！");
        return false;
    }
    if ($('#shipping_check').prop('checked') && $('#shipping_info').attr('info') == '') {
        layer.msg('你选择了自提托运，但是没有填写物流信息！')
        return false;
    }
    if ($("#txtArea option:selected").val() == 0 && $('body input[name="shfs"]:checked').val() == "自提") {
        layer.msg("请选择行政区！");
        return false;
    }
    if ($("#datDeliveryDate").val() == "") {
        layer.msg("请选择提货时间！");
        return false;
    }
    if ($("#cdefine3 option:selected").val() == 0) {
        layer.msg("请选择车型！");
        return false;
    }
    if ($("#strLoadingWays").val().length > 100) {
        layer.msg("装车方式不能超过100个字!");
        return false;
    }
    if ($("#strRemarks").val().length > 200) {
        layer.msg("备注不能超过200个字!");
        return false;
    }
    if ($("#buy_list tbody").find("tr").length == 0) {
        layer.msg("你还未选择商品", {
            icon: 2
        });
        return false;
    }
    if ($("#buy_list tbody").find("input").length > 0) {
        layer.msg("购物清单里还有不正确的输入，请检查后重新输入", {
            icon: 2
        });
        return false;
    }
    var flag = true;
    $.each($("#buy_list tr").find(".num"), function (i, v) {
        $(v).parent().removeClass("red");
        if ($(v).text() == 0 || $(v).text() == "") {
            $(v).parent().addClass("red");
            flag = false;
        }
    })
    if (!flag) {
        layer.msg("购物清单里有未输入数量商品！", {
            icon: 2
        });
        return false;
    }
    return true;
};


// //返回信息判断类
// //ReInfo flag返回值:
// //0:一般为数据有问题,客户需要重新修改
// //1:数据正确,提交成功
// //4:try catch 里程序报错,写入日志,并返回提示
// //7:有限销商品,返回限销的商品编码及名称,前台自动删除限销商品并给出提示
// function ReInfo_Check(){
//     var len=arguments.length;
//     if (len==1) {
//         switch (arguments[0]){
//             case '0':
//                 layer.alert(arguments[0].message,{icon:2});
//                 break;
//             case '1':
//                 layer.alert("你的订单已提交成功,订单号为:<br />" + arguments[0].message, { icon: 1 }, function () {
//                     window.location="Reject_Order.html";
//                 };
//                 break;
//             case '4':
//                 layer.alert(arguments[0].message,{icon:2});
//                 break;
//             case '7':
//                 if (arguments[0].list_msg != null&&arguments[0].list_msg.length>0) {
//                     var h = "你有" + data.list_msg.length + "件商品未找到,已自动从购物单里删除<br />";
//                     $.each(trs, function (i, v) {
//                         if ($.inArray($(v).find(".code").text(), arguments[0].list_msg) != -1) {
//                             h += ($(v).find(".cInvName").text() + "<br />");
//                             $(v).remove();
//                         }
//                     });
//                     layer.alert(h, { icon: 7 });
//                 };
//         }
//     }

// }

//判断时间管理或未确认
function check_time(data) {
    if (data.flag == "5") {
        layer.open({
            title: '信息',
            content: data.message,
            closeBtn: false,
            fixed: false,
            resize: false,
            icon: 2,
            btn: ["关闭"],
            yes: function (index, layero) {
                layer.close(index);
            }
        });
        return false;
    }
}

//一键清除零库存
function del_none(table_id, code_id) {
    var trs = $("#" + table_id + " tbody tr");
    $.each(trs, function (i, v) {
        if ($(v).find(".fAvailQtty").text() == 0) {
            $(v).remove();
            set_table_num("buy_list");
            // $("#money").text(get_money("buy_list"));
            var listInfo = get_listInfo("buy_list");
            $("#money").text(listInfo.money);
            $("#pro_weight").text(listInfo.weight)
        }
    })
    layer.closeAll();
}

//用于根据提交表单后，后台返回的数据，对清单里不合法的行添加颜色标注
function set_Table_Colors(table_id, data) {
    $trs = $("#" + table_id + " tbody tr");
    $.each($trs, function (i, v) {
        $(v).removeClass("red").removeClass("yellow").removeClass(".blue");
        if (data[i]["rowType"] != 0) {
            switch (data[i]["rowType"]) {
                case 1:
                    $(v).addClass("red");
                    break;

                case 2:
                    $(v).addClass("yellow");
                    break;

                case 3:
                    $(v).addClass("blue");
                    break;
            }
        }
    })
}



//获取地址信息
function get_ztpsAddress(AddressType) {
    $.ajax({
        type: "Post",
        dataType: "Json",
        async: false,
        url: "../Handler/ProductHandler.ashx",
        data: {
            "Action": "GetAddressByType",
            "AddressType": AddressType
        },
        success: function (res) {
            if (res.flag != 1) {
                layer.alert(res.message, {
                    icon: 2
                })
                return false;
            }
            var html = "";
            if (AddressType == 1) {
                html = "<option value>请选择自提地址</option>";
                $.each(res.DataSet.address_dt, function (i, v) {
                    html += '<option   value="' + v.lngopUseraddressId + '">' + v.strDistributionType + ',' + v.strCarplateNumber + ',' + v.strDriverName + ',' + v.strDriverTel + v.strIdCard + '</option>'
                })
            } else if (AddressType == 2) {
                html = "<option value>请选择配送地址</option>";
                $.each(res.DataSet.address_dt, function (i, v) {
                    html += '<option ccodeid="' + v.ccodeID + '" strDistrict="' + v.strDistrict + '" value="' + v.lngopUseraddressId + '">' + v.strDistributionType + ',' + v.strConsigneeName + ',' + v.strConsigneeTel + ',' + v.strDistrict + v.strReceivingAddress + '</option>'
                })
            } else if (AddressType == 3) {
                html = "<option value>请选择自提(托运)地址</option>";
                $.each(res.DataSet.address_dt, function (i, v) {
                    html += '<option ccodeid="' + v.ccodeID + '" strDistrict="' + v.strDistrict + '" value="' + v.lngopUseraddressId + '">' + v.strDistributionType + ',' + v.strConsigneeName + ',' + v.strConsigneeTel + ',' + v.strDistrict + v.strReceivingAddress + '</option>'
                })
            }

            $("#txtAddress").html(html);
            if (AddressType == 1) {
                html = '<option value>自提必须选择行政区</option>';
                $.each(res.DataSet.area_dt, function (i, v) {
                    html += '<option value="' + v.ccodeID + '">' + v.xzq + '</option>'
                })
            }
            else {
                html = '<option value>选择地址信息后自动更新</option>';
            }

            $('#txtArea').html(html);
        }
    });
}



////转换日期格式，将格式为“/Date(-2036476800000+0800)/”转换为“2016-09-09”
//function return_date(date) {
//    if (date != "" && date != null) {
//        var new_date = date.slice(6, 19);
//        var time = new Date(Number(new_date));
//        return time.toLocaleDateString().replace(/\//g, "-");
//    }
//}
////转换日期格式，将格式为“/Date(-2036476800000+0800)/”转换为“2016-6-25 17:14:38”
//function return_datetime(date) {
//    if (date != "" && date != null) {
//        var new_date = date.slice(6, 19);

//        var time = new Date(Number(new_date));
//        return time.getFullYear() + "-" + (time.getMonth() + 1) + "-" + time.getDate() + " " + time.getHours() + ":" + time.getMinutes() + ":" + time.getSeconds();
//    }
//}



//采用正则表达式获取地址栏参数 
function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]);
    return null;
}

function accDiv(arg1, arg2) {
    var t1 = 0,
        t2 = 0,
        r1, r2;
    try {
        t1 = arg1.toString().split(".")[1].length
    } catch (e) { }
    try {
        t2 = arg2.toString().split(".")[1].length
    } catch (e) { }
    with (Math) {
        r1 = Number(arg1.toString().replace(".", ""));
        r2 = Number(arg2.toString().replace(".", ""));
        return (r1 / r2) * pow(10, t2 - t1);
    }
}
//给Number类型增加一个div方法，调用起来更加方便。
String.prototype.div = function (arg) {
    return accDiv(this, arg);
};
Number.prototype.div = function (arg) {
    return accDiv(this, arg);
}
//乘法函数，用来得到精确的乘法结果
//说明：javascript的乘法结果会有误差，在两个浮点数相乘的时候会比较明显。这个函数返回较为精确的乘法结果。
//调用：accMul(arg1,arg2)
//返回值：arg1乘以arg2的精确结果
function accMul(arg1, arg2) {
    var m = 0,
        s1 = arg1.toString(),
        s2 = arg2.toString();
    try {
        m += s1.split(".")[1].length
    } catch (e) { }
    try {
        m += s2.split(".")[1].length
    } catch (e) { }
    return Number(s1.replace(".", "")) * Number(s2.replace(".", "")) / Math.pow(10, m);
}
//给Number类型增加一个mul方法，调用起来更加方便。
String.prototype.mul = function (arg) {
    return accMul(arg, this);
};
Number.prototype.mul = function (arg) {
    return accMul(arg, this);
};
//加法函数，用来得到精确的加法结果
//说明：javascript的加法结果会有误差，在两个浮点数相加的时候会比较明显。这个函数返回较为精确的加法结果。
//调用：accAdd(arg1,arg2)
//返回值：arg1加上arg2的精确结果
function accAdd(arg1, arg2) {
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
//给Number类型增加一个add方法，调用起来更加方便。
Number.prototype.add = function (arg) {
    return accAdd(arg, this);
}
//减法函数
function accSub(arg1, arg2) {
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
    //last modify by deeka
    //动态控制精度长度
    n = (r1 >= r2) ? r1 : r2;
    return ((arg2 * m - arg1 * m) / m).toFixed(n);
}
///给number类增加一个sub方法，调用起来更加方便
Number.prototype.sub = function (arg) {
    return accSub(arg, this);
}



//判断对像是否为空
function isEmptyObject(e) {
    var t;
    for (t in e)
        return !1;
    return !0
}