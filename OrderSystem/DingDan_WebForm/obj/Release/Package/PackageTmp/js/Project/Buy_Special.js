var cstcode = '10';
//var codes=[{itemid:'02020100210Y20170100067'},{itemid:'01170200102Y20170100063'}];
var codes = [];
var product_codes = [];
var kpdw = "";
var isModify = 0;
layui.use(['form', 'util', 'laytpl', 'laydate', 'layer'], function () {
    var form = layui.form(),
        util = layui.util,
        laydate = layui.laydate,
        laytpl = layui.laytpl,
        layer = layui.layer;


//页面加载时初始化数据
    $.ajax({
        url: "../Handler/ProductHandler.ashx",
        dataType: "Json",
        type: "Post",
        data: { "Action": "Get_BaseInfo" },
        aysnc: false,
        success: function (data) {
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
            var time = new Date();
            data["msg"] = time.toLocaleDateString().replace(/\//g, "-");
            $("#view").load("../tpl/buy_orderTpl.html?v=" + new Date().getTime(), function (da, status, XHR) {
                if (status == 'success') {
                    $("#select_product").hide();
                    $("#select_history_order").hide();
                    $("#select_temp_order").hide();
                    $("#alert_buy_temp").hide();
                    $("#TxtCustomer").attr("lay-filter","TS_kpdw");
                    $("#del_none").attr("id", "del_none_TS");
                    $("#refresh").attr("page", "buy_special");
                    $("#txtArea").attr("lay-filter","txtArea_special");
                    $("#txtAddress").attr("lay-filter","txtAddress_special");

                    $("#datCreateTime").text(data.msg);          //订单日期
                  //  $("#TxtCusCredit").text(data.CusCredit_dt[0].iCusCreLine); //信用 
                 //   $("#cdiscountname").text(data.CusCredit_dt[0].cdiscountname);  //酬宾类型
              
                    $.each(data.kpdw_dt, function (i, v) {
                        $("#TxtCustomer").append('<option cCusPPerson=' + v.cCusPPerson + ' value="' + v.cCusCode + '">' + v.cCusName + '</option>')
                    });
                    var html = "";
                    $.each(data.CarType_dt, function (i, v) {
                        html += '<option value=' + v.cValue + '>' + v.cValue + '</option>';
                    });
                    $("#cdefine3").append(html);
                    form.render();

                }
            });

        }
    })


   



    $(document).on("click", "#select_SpecialProduct", function () {      //点开产品选择页面
        kpdw = $("#TxtCustomer option:selected").val();
        if (kpdw == 0 || kpdw == "" || kpdw == 'undefined' || kpdw == null) {
            layer.alert("请先选择开票单位!", { icon: 2 });
            return false;
        };
        codes = get_TSCodes('itemid', 'buy_list');
        product_codes = get_TSCodes('itemid', 'buy_list');
              var areaid=0;
        if ($("input[name=shfs]:checked").val()!=undefined&&$("input[name=shfs]:checked").val()=='自提'&&$("#txtArea").val()!=""&&$("#txtArea").val()!=undefined) {
            areaid=$("#txtArea").val();
        }
        layer.open({
            type: 2
            , offset:"10px"
            , area: ["1050px", "490px"]
            , title: "选择产品"
            , content: "select_specialProduct.html"
            , success: function (layero, index) {
            }
            , btn: ['确定']
            , btn1: function (index, layero) {
                var add_codes = [];
                var tds = $("#buy_list tr").find(".itemid");                      //不在清单里的产品才重新添加
                //遍历清单数组，如果数组中的元素不在全局变量codes中，则删除此行
                $.each(tds, function (i, v) {
                    if ($.inArray($(v).text(), codes) == -1) {
                        $(v).parents("tr").remove();
                    }
                })
                //遍历全局数组，如果数组中的元素不在清单数组product_codes中，则需要添加此产品
                $.each(codes, function (i, v) {
                    if ($.inArray(v, product_codes) == -1) {
                        add_codes.push(v);
                    }
                })
                if (add_codes.length > 0) {
  

                    $.ajax({
                        traditional: true,
                        type: "Post",
                        url: "../Handler/ProductHandler.ashx",
                        dataType: "Json",
                        data: { "Action": "DLproc_QuasiYOrderDetail_TSBySel", "itemids": add_codes, "isModify": isModify,"areaid":areaid },
                        success: function (data) {
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
    }).on("click", "#del_none_TS", function () {                               //删除无库存产品
        layer.confirm("你确定要删除无库存的商品？", function () {
            del_none("buy_list", "itemid");
        })
    }).on("click", "#submit_buy_list", function () {                            //提交表单
        if (check_form()) {              //验证表单数据是否完整
        // var areaid=0;
        // if ($("input[name=shfs]:checked").val()!=undefined&&$("input[name=shfs]:checked").val()=='自提'&&$("#txtArea").val()!=""&&$("#txtArea").val()!=undefined) {
        //     areaid=$("#txtArea").val();
        // }
            $.ajax({
                traditional: true,
                type: "Post",
                url: "../Handler/ProductHandler.ashx",
                dataType: "Json",
                data: {
                    "Action": "DLproc_NewYYOrderByIns",
                    "formData": JSON.stringify(get_formData()),
                    "listData": JSON.stringify(get_listData("buy_list"))
                
                },
                success: function (data) {
                    if (data.flag == '0') {
                        var errMsg = "你的订单存在以下问题:<br />";
                        for (var i = 0; i < data.list_msg.length; i++) {
                            errMsg += ((i + 1) + "、" + data.list_msg[i] + "<br />");
                        }
                        set_table_color("buy_list");
                      //  set_Table_Colors("buy_list", 3);

                        layer.alert(errMsg, { icon: 2 });
                    }else if(data.flag=='1'){
                        layer.alert("你的订单已提交成功,订单号为:<br />"+data.message,{icon:1},function(){
                            window.location.reload();
                        });
                    }
                },
                error: function (err) {

                }

            })


        };

    });




    function get_TSCodes(css, table_id) {
        var arr = [];
        $tds = $("#" + table_id).find("." + css);
        $.each($tds, function (i, v) {
            arr.push($(v).text());
        });
        return arr;
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
            //html += "<td class='price'>" + v[0].iquotedprice + "</td>";
            html += "<td class='price'>" + (v[0].itaxunitprice).toFixed(6) + "</td>";
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


    //选择开票单位，刷新信用额度 
    form.on('select(TS_kpdw)', function (data) {
      //  layer.confirm("切换开票单位将清空产品列表里的所有商品,<br />你确定要切换?",{icon:3},function(){
            $("#buy_list tbody").html("");
            $("#money").text("0.00");
            layer.closeAll();
            $.ajax({
            url: "../Handler/ProductHandler.ashx",
            dataType: "Json",
            type: "Post",
            data: { "Action": "DLproc_getCusCreditInfo","kpdw":data.value },
            aysnc: false,
            success: function (data) {
                if (data.flag=="0") {
                    layer.alert(data.message,{icon:2});
                }else{
                    if (data.CusCredit_dt[0].iCusCreLine != '-99999999.000000') {
                        $("#TxtCusCredit").text(data.CusCredit_dt[0].iCusCreLine);
                    } else {
                        $("#TxtCusCredit").text("现金用户")
                    }
                    $("#cdiscountname").text(data.CusCredit_dt[0].cdiscountname);  //酬宾类型
                }
                   
                }
            });
 
   // })

        // var iteids = [];
        // itemids = get_TSCodes("itemid", "buy_list");
        // $.ajax({
        //     traditional: true,
        //     type: "Post",
        //     url: "/../Handler/ProductHandler.ashx",
        //     dataType: "Json",
        //     data: { "Action": "DLproc_QuasiYOrderDetail_TSBySel", "itemids": itemids, "kpdw": data.value },
        //     success: function (data) {
        //         $("#TxtCusCredit").text((data.dt)[0].iCusCreLine);
        //         $("#cdiscountname").text((data.dt)[0].cdiscountname);
        //         if (itemids.length != 0) {
        //             var $trs = $("#buy_list tbody tr");
        //             $.each($trs, function (i, v) {
        //                 if ($(v).find(".itemid").text() == data.list_dt[i][0].itemid) {
        //                     $(v).find(".realqty").text(data.list_dt[i][0].realqty);
        //                     $(v).find(".fAvailQtty").text(data.list_dt[i][0].fAvailQtty);
        //                 }
        //             })
        //             set_table_num("buy_list");
        //             var trs = $("#buy_list tr");
        //         }
        //     },
        //     error: function (err) {
        //         layer.alert("出现异常，请重试或联系管理员！", { icon: 2 });
        //     }
        // })
    });


 //选择配送和自提托运，刷新价格以及自动填写行政区域
 form.on('select(txtAddress_special)',function(obj){
     
     if ($('#ps').prop('checked')||$('#shipping_check').prop('checked')) {
        var html='<option value="'+$('#txtAddress option:selected').attr('ccodeid')+'">'+$('#txtAddress option:selected').attr('strdistrict')+'</option>';
        $('#txtArea').html(html);
        form.render();
     }

        if ($("#buy_list tbody tr").length == 0 ||  $('#TxtCustomer').val() == 0 ||  $('#TxtCustomer').val() == "" ||  $('#TxtCustomer').val() == undefined ||
            $("#TxtCustomer option:selected").val() == "" ||($('#zt').prop('checked')&&!$('#shipping_check').prop('checked'))) {
            return false;
        }
     Change_Area();

 })


  //选择自提行政区，刷新信用额度及购物清单
form.on('select(txtArea_special)',function(data){
  if ($("#buy_list tbody tr").length==0||data.value == 0 || data.value == ""|| data.value==undefined ||$("#TxtCustomer option:selected").val()==""||$("input[name=shfs]:checked").val()=='配送'||$('#shipping_check').prop('checked')) {
            return false;
        }
     Change_Area();
})


function Change_Area(){
       var tds = $("#buy_list tbody tr").find(".code"),
            codes = [],
            $trs = $("#buy_list tbody tr");
       $.each($trs, function (i, v) {
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
      
      var page=$("#refresh").attr("page");
            $.ajax({
                traditional: true,
                type: "Post",
                dataType: "Json",
                url: "../Handler/ProductHandler.ashx",
                data: {
                    "Action": "Refresh",
                    "codes": codes,
                    "page": page,
                    "areaid":areaid
                },
            success: function (data) {
                 console.log(data)
                if (data.dt.length > 0) {
                   
                    $.each($("#buy_list tbody tr"), function (i, v) {
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
            error: function (e) {
                layer.msg("数据刷新失败，请重试！", {
                    icon: 2
                });
            }
        });
}

 


    // 设置表格颜色
    // type=1时，只判断表格中未输入数量的产品，添加red
    // type=2时，判断库存为0的产品，添加red;判断购买数量大天库存的产品，添加yellow
    // type=3时，判断库存为0的产品，添加red;判断购买数量大天库存的产品，添加yellow;判断购买量大天可用量的，添加orange

    function set_Table_Colors(table_id, type) {
        $trs = $("#" + table_id + " tbody tr");
        $.each($trs, function (i, v) {
            $(v).removeClass("red").removeClass("yellow").removeClass("orange");
        })
        switch (type) {
            case 1:
                $.each($trs, function (i, v) {
                    if ($(v).find(".num").text() == 0 || $(v).find(".num").text() == "") {
                        $(v).addClass("red");
                    }
                });
                break;

            case 2:
                $.each($trs, function (i, v) {
                    if ($(v).find(".num").text() == 0 || $(v).find(".num").text() == "") {
                        $(v).addClass("red");
                    } else if (Number($(v).find(".num").text()) > Number($(v).find(".fAvailQtty"))) {
                        $(v).addClass("yellow");
                    }
                });
                break;

            case 3:
                $.each($trs, function (i, v) {
                    if ($(v).find(".num").text() == 0 || $(v).find(".num").text() == "") {
                        $(v).addClass("red");
                    }
                    if (Number($(v).find(".num").text()) > Number($(v).find(".fAvailQtty"))) {
                        $(v).addClass("yellow");
                    }
                    if (Number($(v).find(".num").text()) > Number($(v).find(".realqty"))) {
                        $(v).addClass("orange");
                    }

                });
                break;

        }
    }


})




