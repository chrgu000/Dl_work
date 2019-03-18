var AllcCuscode = {};
var Manager = 0;


$(document).ready(function () {

    //layui.use(['util', 'laydate', 'form'], function () {

    //    var form = layui.form(),
    //        layer = layui.layer,
    //      laydate = layui.laydate;
    //    //  element = layui.element(); //Tab的切换功能，切换事件监听等，需要依赖element模块

    //    var start = {

    //        min: '2016-01-01 00:00:00'
    //                , max: '2099-06-16 23:59:59'
    //                , format: 'YYYY-MM-DD' //日期格式
    //        //   , istime: true     //是否开启时间选择
    //                , istoday: false
    //                , choose: function (datas) {
    //                    end.min = datas; //开始日选好后，重置结束日的最小日期
    //                    end.start = datas //将结束日的初始值设定为开始日
    //                }
    //    };

    //    var end = {
    //        min: laydate.now()
    //      , max: '2099-06-16 23:59:59'
    //         , format: 'YYYY-MM-DD' //日期格式
    //        //  , istime: true     //是否开启时间选择
    //      , istoday: false
    //      , choose: function (datas) {
    //          start.max = datas; //结束日选好后，重置开始日的最大日期
    //      }
    //    };



    //    $("#start_date").click(function () {
    //        start.elem = this;
    //        laydate(start);
    //    });

    //    $("#end_date").click(function () {
    //        end.elem = this;
    //        laydate(end);
    //    });



    var columns = [
             {
                 title: "操作",
                 width: 300,
                 formatter: function (value, row, index, field) {
                     if (row.bytStatus == 11) {
                         if (row.cMake == Manager) {
                             return '<div style="text-align:center"><button type="button" class="  btn btn-info  btn-sm b_show">查看</button>\
                                                                    <button type="button" class="  btn    btn-sm b_back" disabled>取回</button>\
                                                                    <button type="button" class="  btn btn-warning  btn-sm b_modify">修改</button>\
                                                                    <button type="button" class="  btn btn-danger  btn-sm b_cancel">作废</button>\</div>';
                         } else {
                             return '<div style="text-align:center"><button type="button" class="  btn btn-info  btn-sm b_show">查看</button>\
                                                                    <button type="button" class="  btn btn-primary  btn-sm b_back">取回</button>\
                                                                    <button type="button" class="  btn  btn-sm b_modify" disabled>修改</button>\
                                                                    <button type="button" class="  btn btn-danger  btn-sm b_cancel">作废</button>\</div>';
                         }


                     }
                     else if (row.bytStatus == 21 || row.bytStatus == 22) {
                         return '<div style="text-align:center"><button type="button" class="  btn btn-info  btn-sm b_show">查看</button>\
                                                                    <button type="button" class="  btn btn-primary  btn-sm b_back">取回</button>\
                                                                    <button type="button" class="  btn  btn-sm b_modify" disabled>修改</button>\
                                                                    <button type="button" class="  btn btn-danger  btn-sm b_cancel">作废</button>\</div>';

                     } else {
                         return '<div style="text-align:center"><button type="button" class="  btn btn-info  btn-sm b_show">查看</button>\
                                                                    <button type="button" class="  btn    btn-sm b_back" disabled>取回</button>\
                                                                    <button type="button" class="  btn   btn-sm b_modify" disabled>修改</button>\
                                                                    <button type="button" class="  btn   btn-sm b_cancel" disabled>作废</button>\</div>';
                     }
                 }
             },
            {
                field: 'cCode',
                title: '通知单编号'
            }, {
                field: 'cCusCode',
                title: '客户编号'
            },

              {
                  field: 'cCusName',
                  title: '客户名称'
              },

              {
                  field: 'bytStatus',
                  title: '单据状态',
                  formatter: function (value, row, index, field) {
                      switch (value) {
                          case 11:
                              return "待复核"
                              break;
                          case 21:
                              return "待客户确认（未逾期三个月）"
                              break;
                          case 22:
                              return "待客户确认（逾期三个月）"
                              break;
                          case 31:
                              return "客户已确认（未逾期三个月）"
                              break;
                          case 32:
                              return "客户已确认（逾期三个月）"
                              break;
                          case 41:
                              return "销售经理已确认（未逾期三个月）"
                              break;
                          case 42:
                              return "销售经理已确认（逾期三个月）（不停止发货）"
                              break;
                          case 43:
                              return "销售经理已确认（逾期三个月）（停止发货）"
                              break;
                          case 51:
                              return "停止发货后手工开通网单"
                              break;
                          case 99:
                              return "已作废"
                              break;
                      }
                  }
              },
              {
                  field: 'cMake',
                  title: '单据申请人'
              },
                   {
                       field: 'dDate',
                       title: '提交时间',
                       formatter: function (value, row, index, field) {
                           return return_datetime(value)
                       }
                   },
              {
                  field: 'cReview',
                  title: '单据复核人'
              },
              {
                  field: 'dReviewDate',
                  title: '单据复核时间',
                  formatter: function (value, row, index, field) {
                      return return_datetime(value)
                  }
              },

    ]



    //获取所有客户
    $.ajax({
        type: "Post",
        url: "/Handler/AdminHandler.ashx",
        dataType: "Json",
        async: false,
        data: { "Action": "Get_AllcCuscode" },
        success: function (data) {
            if (data.flag != "1") {
                alert(data.message, { icon: 2 });
                return false;
            }
            else {
                Manager = data.message;
                $.each(data.dt, function (i, v) {
                    AllcCuscode[v.cCusCode] = v.cCusName;
                })
            }
        },
        error: function (err) {
            layer.alert("获取数据失败,请重试或联系管理员!", { icon: 2 });
            console.log(err);
        }
    });

    //1.初始化Table
    var oTable = new TableInit(columns, 'tb');
    oTable.Init();
    // })


})




$(document).on("click", "#ccus_add", function () {                      //增加客户
    this.rowSpan += 1;
    //   $(this).parents("tr").after('<tr class="ccus_tr"> <td class="ccuscode"> <div class="cSubCusCode_div"> <input type="text" value="" placeholder="客户编码" class="form-control cSubCusCode" /></div> </td><td colspan="2" class="cSubCusName"></td><td>欠款金额</td><td> <div class="row"><label class="col-sm-2  control-label"> ￥:</label><div class="col-sm-9 iSumSubCusCode_div"> <input type="text" value="" placeholder="小写" class="form-control money  iSumSubCusCode" /></div> </div></td></tr>')
    $("#detail .ccus_tr:last").after('<tr class="ccus_tr"> <td class="ccuscode"> <div class="cSubCusCode_div"> <input type="text" value="" placeholder="客户编码" class="form-control cSubCusCode" /></div> </td><td colspan="2" class="cSubCusName"></td><td>欠款金额</td><td> <div class="row"><label class="col-sm-2  control-label"> ￥:</label><div class="col-sm-9 iSumSubCusCode_div"> <input type="text" value="0" placeholder="小写" class="form-control money  iSumSubCusCode" /></div> </div></td></tr>')
}).on("click", ".cSubCusName", function () {                                 //删除客户
    var $this = $(this);
    if ($this.hasClass("no_del")) {
        layer.alert("此行不允许删除！", { icon: 2 })
    }
    else {
        layer.confirm("确认删除？", { icon: 3 }, function () {

            $this.parents("tr").remove();
            var add_tr = document.getElementById('ccus_add')
            add_tr.rowSpan -= 1;
            layer.closeAll();
            SumMoney();
        })
    }
}).on("click", "#print", function () {                              //打印
    //  $(".modal-body").printArea();
    $(".modal-body").jqprint();
}).on("blur", "#iSumArrears", function () {                    //大小写金额转换
    $("#iSumArrearsCapital").text(AmountLtoU($(this).val()));
}).on("blur", "#iSumPreviousLiquidatedDamages", function () {
    $("#iSumPreviousLiquidatedDamagesCapital").text(AmountLtoU($(this).val()));
}).on("blur", "#iSumLiquidatedDamages", function () {
    $("#iSumLiquidatedDamagesCapital").text(AmountLtoU($(this).val()));
}).on("blur", "#cCusName", function () {                      //表头客户编码转换
    var $this = $(this);
    var a = $this.val().trim();
    if (AllcCuscode[a] != undefined) {
        $this.val(AllcCuscode[a]);
        $("#cCusCode").val(a);
    }
    else {
        $(this).val("未查询到该客户");
        $("#cCusCode").val("");
    }
}).on("blur", ".cSubCusCode", function () {                //表体客户编码转换
    var $this = $(this);
    var a = $this.val().trim();
    if (AllcCuscode[a] != undefined) {
        $this.parents("tr").find(".cSubCusName").text(AllcCuscode[a]);
    }
    else {
        $this.parents("tr").find(".cSubCusName").text("未查询到该客户");
    }
}).on("blur", ".money", function () {                         //判断金额输入是否正确
    var $this = $(this);
    var val = $this.val().trim();
    if (isNaN(val)) {
        layer.msg("输入数值不合法，请重新输入！");
        $(this).val("");
    }
}).on("blur", ".iSumSubCusCode", function () {               //表体金额合计
    SumMoney();
}).on("blur", "#iSumPreviousLiquidatedDamages", function () {
    var iSumDamages = (Number($("#iSumPreviousLiquidatedDamages").val()) + Number($("#iSumLiquidatedDamages").val())).toFixed(2)
    $("#iSumDamages").text(iSumDamages);
    $("#iSumDamagesCapital").text(AmountLtoU(iSumDamages));
}).on("blur", "#iSumLiquidatedDamages", function () {
    var iSumDamages = (Number($("#iSumPreviousLiquidatedDamages").val()) + Number($("#iSumLiquidatedDamages").val())).toFixed(2)
    $("#iSumDamages").text(iSumDamages);
    $("#iSumDamagesCapital").text(AmountLtoU(iSumDamages));
})
//    .on("blur", ".p_m", function () {                          //去年计算合计
//    var sum = 0;
//    var b = $("#detail").find(".p_m");
//    $.each(b, function (i, v) {
//        sum += Number($(v).val().trim());
//    })
//    sum = sum.toFixed(2);
//    $("#iSumPreviousLiquidatedDamages").text(sum);
//    $("#iSumPreviousLiquidatedDamagesCapital").text(AmountLtoU(sum));

//    var iSumDamages = (Number($("#iSumPreviousLiquidatedDamages").text()) + Number($("#iSumLiquidatedDamages").text())).toFixed(2)
//    $("#iSumDamages").text(iSumDamages);
//    $("#iSumDamagesCapital").text(AmountLtoU(iSumDamages));
//}).on("blur", ".n_m", function () {                        //本年计算合计
//    var sum = 0;
//    var b = $("#detail").find(".n_m");
//    $.each(b, function (i, v) {
//        sum += Number($(v).val().trim());
//    })
//    sum = sum.toFixed(2);
//    $("#iSumLiquidatedDamages").text(sum);
//    $("#iSumLiquidatedDamagesCapital").text(AmountLtoU(sum));

//    var iSumDamages = (Number($("#iSumPreviousLiquidatedDamages").text()) + Number($("#iSumLiquidatedDamages").text())).toFixed(2)
//    $("#iSumDamages").text(iSumDamages);
//    $("#iSumDamagesCapital").text(AmountLtoU(iSumDamages));
//})


//查看通知单
$(document).on("click", ".b_show", function () {
    $("#tb").find("tr").removeClass("select")
    $(this).parents("tr").addClass("select");
    //var code = $(this).parents("tr").find("td:eq(1)").text();
    var index = $("#tb").find("tr.select").data("index");
    var list = $("#tb").bootstrapTable('getData')[index];
    var code = list.cCode;
    $.ajax({
        type: "Post",
        url: "/Handler/AdminHandler.ashx",
        dataType: "Json",
        data: { "Action": "Get_ArrearDetail", "code": code },
        success: function (data) {
            if (data.flag != '1') {
                layer.alert(data.message, { icon: 2 });
                return false;
            }
            else if (data.flag == '1') {
                $('#myModal').modal({
                    backdrop: "static",
                    keyboard: "true"
                });
                $(".modal-content").load("/tpl/ArrearTable.html", function (da, status) {
                    if (status == 'success') {
                        //$("#detail").find(".c_write").remove();
                        //$("#detail").find(".m_write").remove();
                        //$("#print").remove();
                        var ob = {};
                        ob = data.dt[0];
                        if (ob.bytStatus == 11 && ob.cMake != Manager) {

                            $("#print").remove();
                            $("#submit").remove();
                        }
                        else if (ob.bytStatus == 43) {
                            $("#print").remove();
                            $("#submit").remove();
                            $("#confirm").attr("id", "renew").text("开通网单");

                        } else {
                            $("#submit").remove();
                            $("#confirm").remove();
                        }

                        $("label").css("padding-top", "0")

                        var t_head = $(".modal-body").find(".t_head");
                        //填充表头数据
                        $.each(t_head, function (i, v) {
                            if ($(v).prop("id") == 'dAccountDay' || $(v).prop("id") == 'dArrearsCycleStart' || $(v).prop("id") == 'dArrearsCycleEnd' || $(v).prop("id") == 'dExtensionDateStart' || $(v).prop("id") == 'dExtensionDateEnd') {
                                var a = ob[$(v).prop("id")];
                                $(v).parent().text(return_date(a));
                            }
                            else if ($(v).prop("tagName") == 'INPUT') {
                                $(v).parent().text(ob[$(v).prop("id")])
                            } else if ($(v).prop("tagName") == 'U') {
                                $(v).text(ob[$(v).prop("id")]);
                            }
                        })



                        //填充表体数据
                        $.each(data.dt, function (i, v) {
                            if (i > 0) {
                                var t = document.getElementById("ccus_add");
                                t.rowSpan += 1;
                                $("#detail .ccus_tr:last").after('<tr class="ccus_tr"> <td class="ccuscode"> <div class="cSubCusCode_div"> <input type="text" value="" placeholder="客户编码" class="form-control cSubCusCode" /></div> </td><td colspan="2" class="cSubCusName">  <div class=" cSubCusName_div"></div></td><td>欠款金额</td><td> <div class="row"><label class="col-sm-2  control-label"> ￥:</label><div class="col-sm-9 iSumSubCusCode_div"> <input type="text" value="" placeholder="小写" class="form-control money  iSumSubCusCode" /></div> </div></td></tr>')
                            }
                            $("#detail").find(".ccus_tr:eq(" + i + ")").find(".cSubCusCode_div").text(v.cSubCusCode)
                            $("#detail").find(".ccus_tr:eq(" + i + ")").find(".cSubCusName_div").text(v.cSubCusName)
                            $("#detail").find(".ccus_tr:eq(" + i + ")").find(".iSumSubCusCode_div").text(v.iSumSubCusCode)

                        })

                        //填充客户数据
                        if (ob.cCusConfirm == null || ob.cCusConfirm == "") {
                            $("#dExtensionDateStart_div").text("");
                            $("#dExtensionDateEnd_div").text("");
                            $("#cCusConfirm_div").html('<strong style="color:red">客户未确认</strong>')
                        } else {
                            $("#dExtensionDateStart_div").text(return_date(ob.dExtensionDateStart));
                            $("#dExtensionDateEnd_div").text(return_date(ob.dExtensionDateEnd));
                            $("#cCusConfirm_div").text(ob.ccusperson + " 确认日期：" + return_datetime(ob.dCusConfirmDate));
                        }

                        //填充销售经理数据


                        if (ob.bStopShipment != null && ob.bStopShipment != "") {
                            if (ob.bStopShipment == '1') {

                                $("#bStopShipment_div").text(return_datetime(ob.dStopShipmentDate) + " 起停止发货")
                            } else if (ob.bStopShipment == '0') {
                                $("#bStopShipment_div").text("不停止发货")
                            }
                        } else {
                            $("#cSaleConfirm_div").html('<strong style="color:red">销售经理未确认</strong>')
                        }

                        if (ob.cMemo != null && ob.cMemo != "") {
                            $("#cMemo_div").text(ob.cMemo);
                        }

                        if (ob.cSaleConfirm != null && ob.cSaleConfirm != "") {
                            $("#cSaleConfirm_div").text(ob.strUserName + " 确认日期：" + return_datetime(ob.dSaleConfirm));
                        }
                    }
                });

            }
        }

    });

})




function SumMoney() {
    var sum = 0;
    var b = $("#detail").find(".iSumSubCusCode");
    $.each(b, function (i, v) {
        sum += Number($(v).val().trim());
    })
    sum = sum.toFixed(2);
    $("#iSumArrears").text(sum);
    $("#iSumArrearsCapital").text(AmountLtoU(sum));
}


//查看订单列表
$("#search").click(function () {
    Get_ArrearList();
})

//新增表
$(document).on("click", "#add", function () {
    $('#myModal').modal({
        backdrop: "static",
        keyboard: "true"
    });
    $(".modal-content").load("/tpl/ArrearTable.html", function (da, status) {
        if (status == 'success') {
            $("#detail").find(".c_write").remove();
            $("#detail").find(".m_write").remove();
            $("#print").remove();
            $("#confirm").remove();
        }
    });


})

//提交表单
$(document).off("click", "#submit").on("click", "#submit", function () {
    if (!Check_Arrear()) {
        return false;
    }
    var arrHead = {};
    $.each($(".modal-body").find(".t_head"), function (i, v) {
        if ($(v).prop("tagName") == 'INPUT') {
            arrHead[$(v).prop("id")] = $(v).val().trim();
        } else if ($(v).prop("tagName") == 'U') {
            arrHead[$(v).prop("id")] = $(v).text().trim();
        }
    })
    var arrBody = Get_Arrear_Body();
    console.log(arrHead);
    console.log(arrBody);
    $.ajax({
        type: "Post",
        traditional: true,
        url: "/../Handler/AdminHandler.ashx",
        dataType: "Json",
        data: { "Action": "DLproc_NewArrearByIns", "arrHead": JSON.stringify(arrHead), "arrBody": JSON.stringify(arrBody) },
        success: function (data) {
            console.log(data);
            if (data.flag != '1') {
                layer.alert(data.message, { icon: 2 });
                return false;
            } else if (data.flag == '1') {
                layer.alert(data.message, { icon: 1 }, function () {
                    Get_ArrearList();
                    layer.closeAll();
                    $('#myModal').modal("hide");
                });
            }
        }
    });

})

//取回通知单
$(document).on("click", ".b_back", function () {
    $("#tb").find("tr").removeClass("select")
    $(this).parents("tr").addClass("select");
    layer.confirm("确认要取回该通知单？", { icon: 3 }, function () {
        var index = $("#tb").find("tr.select").data("index");
        var list = $("#tb").bootstrapTable('getData')[index];
        var code = list.cCode;
        // if (list.cMake == Manager) {
        //  layer.alert("不需要取回自己创建的通知单！", { icon: 2 });
        //return false;

        $.ajax({
            type: "Post",
            traditional: true,
            url: "/../Handler/AdminHandler.ashx",
            dataType: "Json",
            data: { "Action": "BackArrear", "code": code },
            success: function (data) {
                console.log(data);
                if (data.flag != '1') {
                    layer.alert(data.message, { icon: 2 });
                    return false;
                } else if (data.flag == '1') {
                    layer.alert(data.message, { icon: 1 }, function () {
                        //  location.reload();
                        //$("#tb").find("tr.select").find(".b_modify").addClass("btn-warning").removeAttr("disabled");
                        //$("#tb").find("tr.select").find(".b_back").removeClass("btn-primary").attr("disabled", true);
                        //layer.closeAll();
                        Get_ArrearList();
                        layer.closeAll();
                        $('#myModal').modal("hide");
                    });
                }
            }
        });


    })

})

//作废通知单
$(document).on("click", ".b_cancel", function () {
    $("#tb").find("tr").removeClass("select")
    $(this).parents("tr").addClass("select");
    layer.confirm("确认要作废该通知单？", { icon: 3 }, function () {
        var index = $("#tb").find("tr.select").data("index");
        var list = $("#tb").bootstrapTable('getData')[index];
        var bytstatus = list.bytstatus;
        if (bytstatus == '31' || bytstatus == '11' || bytstatus == '99') {
            layer.alert("不能作废该通知单！", { icon: 2, closeBtn: 0 });
            return false;
        } else {
            var code = list.cCode;
            $.ajax({
                type: "Post",
                traditional: true,
                url: "/../Handler/AdminHandler.ashx",
                dataType: "Json",
                data: { "Action": "CancelArrear", "code": code },
                success: function (data) {
                    console.log(data);
                    if (data.flag != '1') {
                        layer.alert(data.message, { icon: 2, closeBtn: 0 });
                        return false;
                    } else if (data.flag == '1') {
                        layer.alert(data.message, { icon: 1, closeBtn: 0 }, function () {
                            Get_ArrearList();
                            layer.closeAll();
                            $('#myModal').modal("hide");
                        });
                    }
                }
            });
        }

    })
})


//复核通知单
$(document).on("click", "#confirm", function () {
    layer.confirm("确认要复核该通知单？", { icon: 3 }, function () {
        var index = $("#tb").find("tr.select").data("index");
        var list = $("#tb").bootstrapTable('getData')[index];
        var bytstatus = list.bytstatus;
        var code = list.cCode;
        $.ajax({
            type: "Post",
            traditional: true,
            url: "/../Handler/AdminHandler.ashx",
            dataType: "Json",
            data: { "Action": "ConfirmArrear", "code": code },
            success: function (data) {
                console.log(data);
                if (data.flag != '1') {
                    layer.alert(data.message, { icon: 2, closeBtn: 0 });
                    return false;
                } else if (data.flag == '1') {
                    layer.alert(data.message, { icon: 1, closeBtn: 0 }, function () {
                        Get_ArrearList();
                        layer.closeAll();
                        $('#myModal').modal("hide");
                    });
                }
            }
        });
    })
})

//获取修改的延期通知单
$(document).on("click", ".b_modify", function () {
    $("#tb").find("tr").removeClass("select")
    $(this).parents("tr").addClass("select");

    //var code = $(this).parents("tr").find("td:eq(1)").text();
    var index = $("#tb").find("tr.select").data("index");
    var list = $("#tb").bootstrapTable('getData')[index];
    var code = list.cCode;
    $.ajax({
        type: "Post",
        url: "/Handler/AdminHandler.ashx",
        dataType: "Json",
        data: { "Action": "Get_ArrearDetail", "code": code },
        success: function (data) {
            if (data.flag != '1') {
                layer.alert(data.message, { icon: 2 });
                return false;
            }
            else if (data.flag == '1') {
                if (data.dt[0].cMake != Manager) {
                    layer.alert("你不能修改该通知单！", { icon: 2, closeBtn: 0 })
                    return false;
                }
                $('#myModal').modal({
                    backdrop: "static",
                    keyboard: "true"
                });
                $(".modal-content").load("/tpl/ArrearTable.html", function (da, status) {
                    if (status == 'success') {
                        $("#detail").find(".c_write").remove();
                        $("#detail").find(".m_write").remove();
                        $("#print").remove();
                        var ob = {};
                        ob = data.dt[0];

                        // $("#submit").remove();
                        $("#confirm").remove();


                        $("label").css("padding-top", "0")

                        var t_head = $(".modal-body").find(".t_head");
                        //填充表头数据
                        $.each(t_head, function (i, v) {
                            if ($(v).prop("id") == 'dAccountDay' || $(v).prop("id") == 'dArrearsCycleStart' || $(v).prop("id") == 'dArrearsCycleEnd') {
                                var a = ob[$(v).prop("id")];
                                $(v).val(return_date(a));
                            }
                            else if ($(v).prop("tagName") == 'INPUT') {
                                $(v).val(ob[$(v).prop("id")])
                            } else if ($(v).prop("tagName") == 'U') {
                                $(v).text(ob[$(v).prop("id")]);
                            }
                        })

                        //填充表体数据
                        $.each(data.dt, function (i, v) {
                            if (i > 0) {
                                var t = document.getElementById("ccus_add");
                                t.rowSpan += 1;
                                $("#detail .ccus_tr:last").after('<tr class="ccus_tr"> <td class="ccuscode"> <div class="cSubCusCode_div"> <input type="text" value="" placeholder="客户编码" class="form-control cSubCusCode" /></div> </td><td colspan="2" class="cSubCusName">  <div class=" cSubCusName_div"></div></td><td>欠款金额</td><td> <div class="row"><label class="col-sm-2  control-label"> ￥:</label><div class="col-sm-9 iSumSubCusCode_div"> <input type="text" value="" placeholder="小写" class="form-control money  iSumSubCusCode" /></div> </div></td></tr>')
                            }
                            $("#detail").find(".ccus_tr:eq(" + i + ")").find(".cSubCusCode").val(v.cSubCusCode)
                            $("#detail").find(".ccus_tr:eq(" + i + ")").find(".cSubCusName_div").text(v.cSubCusName)
                            $("#detail").find(".ccus_tr:eq(" + i + ")").find(".iSumSubCusCode").val(v.iSumSubCusCode)

                        })
                        $("#submit").attr("id", "b_modify_submit")
                        //  $(".modal-footer").append(' <button type="button" class="btn btn-info " id="b_modify_primary">提交</button>')

                    }
                });

            }
        }

    });
})

//提交修改延期通知单
$(document).on("click", "#b_modify_submit", function () {
    if (!Check_Arrear()) {
        return false;
    }
    var index = $("#tb").find("tr.select").data("index");
    var list = $("#tb").bootstrapTable('getData')[index];
    var code = list.cCode;
    var arrHead = {};
    $.each($(".modal-body").find(".t_head"), function (i, v) {
        if ($(v).prop("tagName") == 'INPUT') {
            arrHead[$(v).prop("id")] = $(v).val().trim();
        } else if ($(v).prop("tagName") == 'U') {
            arrHead[$(v).prop("id")] = $(v).text().trim();
        }
    })
    var arrBody = Get_Arrear_Body();

    $.ajax({
        type: "Post",
        traditional: true,
        url: "/../Handler/AdminHandler.ashx",
        dataType: "Json",
        data: { "Action": "DLproc_NewArrearByUpd", "code": code, "arrHead": JSON.stringify(arrHead), "arrBody": JSON.stringify(arrBody) },
        success: function (data) {
            if (data.flag != '1') {
                layer.alert(data.message, { icon: 2 });
                return false;
            } else if (data.flag == '1') {
                layer.alert(data.message, { icon: 1 }, function () {
                    Get_ArrearList();
                    layer.closeAll();
                    $('#myModal').modal("hide");
                });
            }
        }
    });

})



//恢复网上下单
$(document).on("click", "#renew", function () {
    layer.confirm("你确定要开通此账号的网上下单功能？", { icon: 3 }, function () {
        var index = $("#tb").find("tr.select").data("index");
        var list = $("#tb").bootstrapTable('getData')[index];
        var code = list.cCode;
        $.ajax({
            type: "Post",
            url: "/../Handler/AdminHandler.ashx",
            dataType: "Json",
            data: { "Action": "RenewArrear", "code": code },
            success: function (data) {
                if (data.flag != "1") {
                    layer.alert(data.message, { icon: 2, closeBtn: 0 }, function () {
                        layer.closeAll();
                    })
                }
                else {
                    layer.alert(data.message, { icon: 1, closeBtn: 0 }, function () {
                        layer.closeAll();
                        $('#myModal').modal("hide");
                        Get_ArrearList();
                    })

                }
            }
        });
    })
})



//获取通知单列表
function Get_ArrearList() {
    $.ajax({
        type: "Post",
        url: "/../Handler/AdminHandler.ashx",
        dataType: "Json",
        data: { "Action": "Get_ArrearList", "start_date": $("#start_date").val(), "end_date": $("#end_date").val(), "bytstatus": $("#bytstatus option:selected").val() },
        success: function (data) {
            if (data.flag!="1") {
                layer.alert(data.message, { icon: 2, closeBtn: 0 })
                return false;
            }
            if (data.dt.length > 0) {
                $("#tb").bootstrapTable("load", data.dt);
            } else {
                $("#tb").bootstrapTable("removeAll");
            }
        }
    });
}

$(document).on("focus", ".money", function () {
    this.select();
})




















