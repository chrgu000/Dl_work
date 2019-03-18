

$(document).ready(function () {
    var strAllAcount = "";
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
                 width: 150,
                 formatter: function (value, row, index, field) {
                     return '<div style="text-align:center"><button type="button" class="  btn btn-info  btn-sm b_show">查看通知单</button> ';
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
                          case 21:
                              return "需确认"
                              break;
                          case 22:
                              return "需确认（逾期三个月）"
                              break;
                          case 31:
                          case 41:
                          case 42:
                          case 51:
                              return "已确认"
                              break;
                          case 32:
                              return "已确认，等待审核"
                              break;
                          case 43:
                              return "已确认，停止下单"
                              break;
                      
                      }
                  }
              },
              {
                  field: 'iSumArrears',
                  title: '欠款合计'
              },
               {
                   field: 'iSumDamages',
                   title: '违约金合计'
               },
                   {
                       field: 'dDate',
                       title: '单据时间',
                       formatter: function (value, row, index, field) {
                           return return_datetime(value)
                       }
                   }

    ]
    //1.初始化Table
    var oTable = new TableInit(columns, 'tb');
    oTable.Init();
    // })


})







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
        url: "/Handler/ProductHandler.ashx",
        dataType: "Json",
        data: { "Action": "Get_ArrearDetail", "code": code },
        success: function (data) {
            if (data.flag == '0') {
                layer.alert(data.message, { icon: 2 });
                return false;
            }
            else if (data.flag == '1') {
                $('#myModal').modal({
                    backdrop: "true",
                    keyboard: "true"
                });
                $(".modal-content").load("/tpl/ArrearTable.html", function (da, status) {
                    if (status == 'success') {
                        //$("#detail").find(".m_write").remove();
                        $("#print").remove();
                        $("#submit").remove();
                        $("#confirm").remove();
                        var ob = {};
                        ob = data.dt[0];
                      
                        if (ob.bytStatus == 32 ) {
                            layer.alert("您的延期申请已提交，请耐心等待审核。<br>如有疑问，请咨询收款员！", { icon: 4, closeBtn: 0 })
                        }
                        if ( ob.bytStatus == 43) {
                            layer.alert("您的货款已连续三月未结清，系统已自动控制下单。<br>如有疑问，请咨询收款员！", { icon: 4, closeBtn: 0 })
                        }
                        $("label").css("padding-top", "0")

                        var t_head = $(".modal-body").find(".t_head");
                        //填充表头数据
                        $.each(t_head, function (i, v) {
                            if ($(v).prop("id") == 'dAccountDay' || $(v).prop("id") == 'dArrearsCycleStart' || $(v).prop("id") == 'dArrearsCycleEnd') {
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
                        var $ccus_tr = $(".ccus_tr");
                        $.each(data.dt, function (i, v) {

                            if (i > 0) {
                                var t = document.getElementById("ccus_add");
                                t.rowSpan += 1;
                                $("#detail .ccus_tr:last").after('<tr class="ccus_tr"> <td class="ccuscode"> <div class="cSubCusCode_div"> <input type="text" value="" placeholder="客户编码" class="form-control cSubCusCode" /></div> </td><td colspan="2" class="cSubCusName">  <div class=" cSubCusName_div"></div></td><td>欠款金额</td><td> <div class="row"><label class="col-sm-2  control-label"> ￥:</label><div class="col-sm-9 iSumSubCusCode_div"> <input type="text" value="" placeholder="小写" class="form-control money  iSumSubCusCode" /></div> </div></td></tr>')

                                // $("#detail .ccus_tr:last").after('<tr class="ccus_tr">  <td colspan="3" class="cSubCusName">  <div class=" cSubCusName_div"></div></td><td>欠款金额</td><td> <div class="row"><label class="col-sm-2  control-label"> ￥:</label><div class="col-sm-9 iSumSubCusCode_div"> <input type="text" value="" placeholder="小写" class="form-control money  iSumSubCusCode" /></div> </div></td></tr>')
                            }
                            $("#detail").find(".ccus_tr:eq(" + i + ")").find(".cSubCusCode_div").text(v.cSubCusCode)
                            $("#detail").find(".ccus_tr:eq(" + i + ")").find(".cSubCusName_div").text(v.cSubCusName)
                            $("#detail").find(".ccus_tr:eq(" + i + ")").find(".iSumSubCusCode_div").text(v.iSumSubCusCode)

                        })


                        //var a = $(".cSubCusCode_div")
                        //$.each($(".cSubCusCode_div"), function (i, v) {
                        //    $(v).parent().hide()
                        //    var s = $(v).parent().next();
                        //    console.log(s)
                        //    $(s).prop("colspan", 3);
                        //})

                        if (strAllAcount != ob.cCusCode) {
                            $("#cCusConfirm_div").html('<strong style="color:red">请在主账号确认该通知单！</strong>')
                        }

                        //填充销售经理数据
                        if (ob.bStopShipment != null && ob.bStopShipment.trim() != "") {
                            if (ob.bStopShipment == '1') {
                                $("#bStopShipment_div").text(return_datetime(ob.dStopShipmentDate) + " 起停止发货")
                            } else if (ob.bStopShipment == '0') {
                                $("#bStopShipment_div").text("不停止发货")
                            }
                            $("#cSaleConfirm_div").text(ob.strUserName + " 确认日期：" + return_datetime(ob.dSaleConfirm));
                            if (ob.cMemo != null && ob.cMemo != "") {
                                $("#cMemo_div").text(ob.cMemo);
                            }
                        }
                        else {
                            $("#detail").find(".m_write").remove();
                        }

                        if (ob.bytStatus == 41 || ob.bytStatus == 42 || ob.bytStatus == 43 || ob.bytStatus == 31 || ob.bytStatus == 32||ob.bytStatus==51) {
                           
                            $("#dExtensionDateStart_div").text(return_date(ob.dExtensionDateStart));
                            $("#dExtensionDateEnd_div").text(return_date(ob.dExtensionDateEnd));
                            $("#cCusConfirm_div").text(ob.ccusperson + " 确认日期：" + return_datetime(ob.dCusConfirmDate));
                        }
                    }
                });

            }
        }

    });

})





//查看订单列表
$("#search").click(function () {
    Get_ArrearList();
})





//确认通知单
$(document).on("click", "#cCusConfirm", function () {
    layer.confirm("是否要确认该通知单？", { icon: 3 }, function () {
        if ($("#dExtensionDateStart").val().trim() == "" || $("#dExtensionDateEnd").val().trim() == "") {
            layer.alert("申请日期不能为空！", { icon: 2 });
            return false;
        }
        if ($("#dExtensionDateStart").val().trim() > $("#dExtensionDateEnd").val().trim()) {
            layer.alert("开始日期不能大于结束日期！", { icon: 2 });
            return false;
        }
        if ($("#dExtensionDateStart").val().trim() < $("#dAccountDay_div").text()) {
            layer.alert("开始日期不能小于结款日！", { icon: 2 });
            return false;
        }
        //var d = new Date($("#dAccountDay_div").text().replace(/-/g, "/"));
        //d = DateAdd("M", 1, d);
        //console.log(d)
        //console.log(d.Format("yyyy-MM-dd"))
        if ($("#dExtensionDateEnd").val().trim() > getNextMonth($("#dAccountDay_div").text().trim())) {
            layer.alert("结束日期不能大于下月结款日！", { icon: 2 });
            return false;
        }
        var index = $("#tb").find("tr.select").data("index");
        var list = $("#tb").bootstrapTable('getData')[index];
        var code = list.cCode;
        if (list.cCusCode != strAllAcount) {
            layer.alert("子账号不能确认该通知单！", { icon: 2 });
            return false;
        }

        $.ajax({
            type: "Post",
            traditional: true,
            url: "/../Handler/ProductHandler.ashx",
            dataType: "Json",
            data: { "Action": "CusConfirmArrear", "code": code, "dExtensionDateStart": $("#dExtensionDateStart").val(), "dExtensionDateEnd": $("#dExtensionDateEnd").val() },
            success: function (data) {
                console.log(data);
                if (data.flag == '0') {
                    layer.alert(data.message, { icon: 2 });
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








function Get_ArrearList() {
    $.ajax({
        type: "Post",
        url: "/../Handler/ProductHandler.ashx",
        dataType: "Json",
        async: false,
        data: { "Action": "Get_ArrearList", "statstart_date": $("#start_date").val(), "end_date": $("#end_date").val(), "bytstatus": $("#bytstatus option:selected").val() },
        success: function (data) {
            strAllAcount = data.message;
            if (data.dt.length > 0) {
                $("#tb").bootstrapTable("load", data.dt);
            } else {
                $("#tb").bootstrapTable("removeAll");
            }
        }
    });
}



















