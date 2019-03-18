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
        async: false,
        data: {
            "Action": "CheckXOrderPageOpen",
            "PageType": 1
        },
        success: function(res) {
            console.log(res);
            if (res.flag == 0) {
                layer.alert(res.message, { icon: 2 });
                return false;
            }
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
                    $("#view").load("../tpl/buy_orderTpl.html?v=" + new Date().getTime(), function (da, status, XHR) {
                        if (status == 'success') {
                            $("#select_SpecialProduct").hide();
                            $("#buy_list thead tr th:eq(12)").hide();
                            $("#datCreateTime").text(data.msg); //订单日期
                            //开票单位
                            $("#select_history_order").remove();
                            $("#select_temp_order").remove();
                            $("#alert_buy_temp").remove();
                            $("#del_none").remove();
                            $("#refresh").remove();
                            $("#select_product").attr("id", "select_product_xq");
                            //$(".btns").append('<button  class="layui-btn layui-btn-small" id="showTips">下单说明</button>')
                            $(".btns").append("<span style='color:red;font-weight:bold;padding-left:15px;'>一张需求订单只能提交一种产品</span style='color:red;font-weight:bold'>")
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

        },
        error: function(err) {
            console.log(err);
        }

    })

    // $.ajax({
    //     url: "../Handler/ProductHandler.ashx",
    //     dataType: "Json",
    //     type: "Post",
    //     data: {
    //         //  "Action": "Get_BaseInfo",
    //         "Action": "GetAllBaseInfo",
    //         "PageType": 1
    //     },
    //     aysnc: false,
    //     success: function(data) {
    //         if (data.flag != 1) {
    //             layer.alert(data.message, {
    //                 icon: 2
    //             })
    //             return false;
    //         }

    //         var time = new Date();
    //         data["msg"] = time.toLocaleDateString().replace(/\//g, "-");
    //         $("#view").load("../tpl/buy_orderTpl.html", function(da, status, XHR) {
    //             if (status == 'success') {
    //                 $("#select_SpecialProduct").hide();
    //                 $("#buy_list thead tr th:eq(12)").hide();
    //                 $("#datCreateTime").text(data.msg); //订单日期
    //                 //开票单位
    //                 $("#select_history_order").remove();
    //                 $("#select_temp_order").remove();
    //                 $("#alert_buy_temp").remove();
    //                 $("#del_none").remove();
    //                 $("#refresh").remove();
    //                 $.each(data.DataSet.Kpdw_dt, function(i, v) {
    //                     $("#TxtCustomer").append('<option cCusPPerson=' + v.cCusPPerson + ' value="' + v.cCusCode + '">' + v.cCusName + '</option>')
    //                 });
    //                 var html = "";
    //                 $.each(data.DataSet.CarType_dt, function(i, v) {
    //                     html += '<option value=' + v.cValue + '>' + v.cValue + '</option>';
    //                 });
    //                 $("#cdefine3").append(html);
    //                 form.render();
    //             }
    //         });

    //     }
    // });





    //提交正式订单
    $(document).on("click", "#submit_buy_list", function() {
        $("#submit_buy_list").blur();

        if ($("#buy_list tbody").find("tr").length > 1) {
            layer.msg('一张需求订单只能提交一种产品', { icon: 2 });
            return false;
        }
        if (check_form()) { //验证表单数据是否完整

            $.ajax({
                traditional: true,
                type: "Post",
                url: "../Handler/ProductHandler.ashx",
                dataType: "Json",
                data: {
                    "Action": "DLproc_NewOrder_X_ByIns",
                    "formData": JSON.stringify(get_formData()),
                    "listData": JSON.stringify(get_listData("buy_list"))
                },
                success: function(data) {
                    if (data.flag != '1') {
                        layer.alert(data.message, {
                            icon: 2
                        });
                        return false;
                    }
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


                },
                error: function(err) {

                }

            });

        }

    })

    //弹出产品分类选择页面
    $(document).on("click", "#select_product_xq", function () {
        var kpdw = $("#TxtCustomer option:selected").val();
        if (kpdw == 0 || kpdw == "" || kpdw == 'undefined' || kpdw == null) {
            layer.alert("请先选择开票单位!", {
                icon: 2
            });
            return false;
        }
        product_codes = get_codes("buy_list");
        codes = get_codes("buy_list");
        if (codes.length==1) {
            layer.alert("一张需求订单只能选择一种产品", { icon: 2 });
            return false;
        }
        layer.open({
            type: 2,
            offset: '10px',
            area: ["1020px", "490px"],
            title: false,
            content: "select_product_xq.html?cSTCode=" + cstcode + "&kpdw=" + kpdw + "&iShowType=" + iShowType,
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
                            "Action": "DLproc_QuasiOrderDetail_All_Warehouse_BySel",
                            "codes": add_codes,
                            "kpdw": kpdw,
                            "isModify": isModify,
                            "isSpecial": isSpecial,
                            "strBillNo": $("#strBillNo").text(),
                            "areaid": areaid,
                            "cWhCode": $("#txtWareHouse").val() == "" ? "CD01" : $("#txtWareHouse").val()
                        },
                        success: function (data) {
                            if (data.flag != "1") {
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


    $(document).on("click", "#showTips", function () {
        var html = '<p>1.一张产品需求订单只能提交一种产品</p>';
        html += '<p>2.asdfasdf</p>';
        layer.tips(html, '#showTips', {
            tips: [1, '#3595CC'],
            time: 0
        });
    })


})