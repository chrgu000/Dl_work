var cstcode = '00';
var iShowType = 2;
layui.use(['form', 'util', 'laydate', 'layer'], function () {
    var form = layui.form(),
    util = layui.util,
    laydate = layui.laydate,
    layer = layui.layer;


    $.ajax({
        url: "../Handler/ProductHandler.ashx",
        dataType: "Json",
        type: "Post",
        data: { "Action": "Get_BaseInfo" },
        success: function (data) {
            if (data.flag == "5") {
                layer.open({
                    title: '信息'
                    , content: data.message
                    , closeBtn: false
                    , fixed: false
                    , resize: false
                    , icon: 2
                    , btn: ["关闭"]
                    , yes: function (index, layero) {
                        layer.close(index);
                    }
                });
                return false;
            }
           // console.log(data);
            var time = new Date();
            data["msg"] = time.toLocaleDateString().replace(/\//g, "-");
            $("#datCreateTime").text(data.msg);          //订单日期
            $("#cdiscountname").text(data.CusCredit_dt[0].cdiscountname);  //酬宾类型
            //开票单位
            $.each(data.kpdw_dt, function (i, v) {
                $("#TxtCustomer").append('<option cCusPPerson=' + v.cCusPPerson + ' value="' + v.cCusCode + '">' + v.cCusName + '</option>')
            })
            form.render();
        }
    });



    //弹出产品分类选择页面
    $(document).on("click", "#select_product_ts", function () {
        var kpdw = $("#TxtCustomer option:selected").val();
        if (kpdw == 0 || kpdw == "" || kpdw == 'undefined' || kpdw == null) {
            layer.alert("请先选择开票单位!", { icon: 2 });
            return false;
        }
        product_codes = get_codes("buy_list");
        codes = get_codes("buy_list");
        layer.open({
            type: 2
            , offset:"10px"
              , area: ["1020px", "490px"]
              , title: false
              , content: "select_product.html?cSTCode=" + cstcode + "&kpdw=" + kpdw + "&iShowType=" + iShowType
              , success: function (layero, index) {
              }
              , btn: ['确定']
              , btn1: function (index, layero) {
                  // var product_codes = get_codes("buy_list"); //获取购物清单里所有的产品ID
                  // codes= product_codes;
                  var add_codes = [];
                  var tds = $("#buy_list tr").find("td:eq(1)");                      //不在清单里的产品才重新添加
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
                      $.ajax({
                          traditional: true,
                          type: "Post",
                          url: "../Handler/ProductHandler.ashx",
                          dataType: "Json",
                          data: { "Action": "DLproc_QuasiOrderDetailBySel_new", "codes": add_codes, "kpdw": kpdw,"areaid":'0' },
                          success: function (data) {
                              if (data.flag != "1") {
                                  layer.alert(data.message, { icon: 2 });
                                  return false;
                              };
                              var html = "";
                               console.log(data.dt);
                              $.each(data.dt, function (i, v) {
                                  var unit_b = parseFloat(v.cinvdefine13).toString();
                                  var unit_m = parseFloat(v.cinvdefine14).toString();
                                  html += "<tr>";
                                  html += "<td class='ui-widget-content'><a href='javascript:void(0)'  class='del_this '>删除</a><a href='javascript:void(0)'  class='up_this'>上移</a><a href='javascript:void(0)'  class='down_this'>下移</a></td>";
                                  html += "<td class='SN' code=" + v.cInvCode + ">" + (i + 1) + "</td>";
                                  if (v.cSRPolicy=='LP') {
                                       html += "<td class='cInvName' cinvdefine13=" + unit_b + " cinvdefine14=" + unit_m + " cunitid=" + v.cComUnitCode + "  kl=" + v.Rate + " iTaxRate=" + v.iTaxRate + ">" + v.cInvName + " <i class='layui-icon' style='  margin-left:5px; color: red;'>&#xe63b;</i> </td>";
                                  }else{
                                  html += "<td class='cInvName' cinvdefine13=" + unit_b + " cinvdefine14=" + unit_m + " cunitid=" + v.cComUnitCode + "  kl=" + v.Rate + " iTaxRate=" + v.iTaxRate + ">" + v.cInvName + "</td>";

                                  }
                                  html += "<td>" + v.cInvStd + "</td>";
                                  html += "<td class='unitGroup'>" + unit_b + v.cComUnitName + "=" + unit_b.div(unit_m) + v.cInvDefine2 + "=1" + v.cInvDefine1 + "</td>";
                                  html += "<td class='in num_s'></td>";
                                  html += "<td class='cComUnitName'>" + v.cComUnitName + "</td>";
                                  html += "<td class='in num_m'></td>";
                                  html += "<td class='cInvDefine2'>" + v.cInvDefine2 + "</td>";
                                  html += "<td class='in num_b'></td>";
                                  html += "<td class='cInvDefine1'>" + v.cInvDefine1 + "</td>";
                                  html += "<td class='num'></td>";
                                  html += "<td class='MinOrderQTY'>" + v.MinOrderQTY + "</td>";
                                  html += "<td class='fAvailQtty'>" + v.fAvailQtty + "</td>";
                                  html += "<td class='pack'>" + (typeof (v.cDefine22) == 'undefined' ? "" : v.cDefine22) + "</td>";
                                  html += "<td class='price'>" + (v.ExercisePrice).toFixed(6) + "</td>";
                                  html += "<td class='sum'></td>";
                                  html += "<td class='ex_price' style='display:none'>" + (v.ExercisePrice).toFixed(6) + "</td><td style='display:none' class='ex_sum'></td>";
                                  html += '<td class="code" style="display:none">' + v.cInvCode + '</td>';
                                  html += '<td style="display:none" class="unit_m">' + unit_m + '</td>';
                                  html += '<td style="display:none" class="unit_b">' + unit_b + '</td>';
                                  html += "</tr>";
                              })
                              $("#buy_list tbody").append(html);
                              layer.msg("添加商品成功！");
                              layer.close(index);
                              set_table_num("buy_list");
                            //  set_table_color("buy_list");
                              var trs = $("#buy_list tr");
                          }
                      })
                  } else {
                      set_table_num("buy_list");
                      layer.closeAll();
                  }
              }

        })

    });

    //选择开票单位，刷新信用额度 
    form.on('select(kpdw_ts)', function (data) {
     //   layer.confirm("切换开票单位将清空产品列表里的所有商品,<br />你确定要切换?", { icon: 3 }, function () {
            $("#buy_list tbody").html("");
            $("#money").text("0.00");
            layer.closeAll();
            $.ajax({
                url: "../Handler/ProductHandler.ashx",
                dataType: "Json",
                type: "Post",
                data: { "Action": "DLproc_getCusCreditInfo", "kpdw": data.value },
                aysnc: false,
                success: function (data) {
                    if (data.flag == "0") {
                        layer.alert(data.message, { icon: 2 });

                    } else {
                        $("#cdiscountname").text(data.CusCredit_dt[0].cdiscountname);  //酬宾类型
                        $("#moeny").text("0.00");
                    }

                }
            });

       // });
    })


    $(document).on("click", "#submit_buy_list", function () {
        if ($("#TxtCustomer").val() == 0 || $("#TxtCustomer").val() == "") {
            layer.msg("请选择开票单位!", { icon: 2 });
            return false;
        }
        if ($("#datDeliveryDate").val() == 0 || $("datDeliveryDate").val() == "") {
            layer.msg("请输入提货时间!", { icon: 2 });
            return false;
        }
        if ($("#buy_list tbody").find("tr").length == 0) {
            layer.msg("你还未选择商品", { icon: 2 });
            return false;
        }
        if ($("#buy_list tbody").find("input").length > 0) {
            layer.msg("购物清单里还有不正确的输入，请检查后重新输入", { icon: 2 });
            return false;
        }
        var flag = true;
        var trs = $("#buy_list tbody tr");
        $.each(trs, function (i, v) {
            $(v).removeClass("red").removeClass("yellow");
            if (Number($(v).find(".num").text()) == 0 || $(v).find(".num").text() == "") {
                $(v).addClass("red");
                flag = false;
            }

            if ($(v).find(".MinOrderQTY").text() > 0) {
                if (Number($(v).find(".num").text()) < Number($(v).find(".MinOrderQTY").text())) {
                    $(v).addClass("yellow");
                    flag = false;
                }
            }

        })
        if (!flag) {
            layer.msg("购物清单里有未输入数量商品或小于起订量！", { icon: 2 });
            return false;
        }

        $.ajax({
            url: "../Handler/ProductHandler.ashx",
            dataType: "Json",
            type: "Post",
            traditional: true,
            data: {
                "Action": "DLproc_NewYOrderByIns",
                "date": $("#datCreateTime").text(),
                "ccuscode": $("#TxtCustomer option:selected").val(),
                "ccusname": $("#TxtCustomer option:selected").text(),
                "strRemarks": $("#strRemarks").text(),
                "datDeliveryDate": $("#datDeliveryDate").val(),
                "buy_list": JSON.stringify(get_listData("buy_list"))
            },
            success: function (data) {
                if (data.flag == 0) {
                    layer.alert(data.message, { icon: 2 });
                    var trs = $("#buy_list tbody tr");
                    $.each(trs, function (i, v) {
                        $(v).removeClass("red").removeClass("yellow");
                        if (Number($(v).find(".num").text()) == 0 || $(v).find(".num").text() == "") {
                            $(v).addClass("red");
                        }

                        if ($(v).find(".MinOrderQTY").text() > 0) {
                            if (Number($(v).find(".num").text()) < Number($(v).find(".MinOrderQTY").text())) {
                                $(v).addClass("yellow");
                            }
                        }

                    })
                } else if (data.flag == 1) {
                    layer.alert("订单提交成功！订单编号为:\n" + data.message, { icon: 6 }, function () {
                        location.reload();
                    });
                }
            },
            error: function (err) {
                layer.alert("出现错误，请重试或联系管理员!", { icon: 2 });
            }
        })
    })

})


//一键清除零库存
$(document).on("click", "#del_none", function () {
    layer.confirm("你确定要删除无库存的商品？", function () {
        del_none("buy_list", "code");

    })
});


$("#btn1").click(function(){
  layer.tips('表示订单该产品有完工入库微信消息提醒.<br />消息推送规则为:该产品在全部完工入库之前没有发生过提货业务.', '#btn1', {
  tips: [1, '#3595CC'],
  time: 0
});
})