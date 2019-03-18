var codes = [];  //全局变量，本页面添加和删除产品均会在此变量里反映
var product_codes = [];
var cSTCode; //全局变量，销售类型，用于传入选择商品页面

//JQ ajax全局事件
$(document).ajaxStart(function () {
    layer.load();
}).ajaxComplete(function (request, status) {
    layer.closeAll('loading');
});

layui.use(['element', 'util', 'laydate', 'form'], function () {
    var form = layui.form(),
        util = layui.util,
        laydate = layui.laydate,
        element = layui.element();



    $.ajax({
        url: "Handler/BaseHandler.ashx",
        type: "Post",
        dataType: "Json",
        data: { "Action": "DLproc_OrderDetailModifyBySel", "strBillNo": GetQueryString("strBillNo") },
        success: function (data) {
            console.log(data);

            $("#strBillNo").text(GetQueryString("strBillNo"));     //主订单号
            $("#strUserName").text(data.dt[0].ccusname); //制单人 
            $("#strRemarks").val(data.dt[0].strRemarks);//备注
            if (data.dt[0].cSCCode == 00) {
                $("#shfs").text("自提"); //送货方式
                $("#shfs").attr("cSCCode", "00");  //送货方式编码
            }
            if (data.dt[0].cSCCode == 01) {
                $("#shfs").text("配送"); //送货方式
                $("#shfs").attr("cSCCode", "01");  //送货方式编码
            }
             
            $("#txtAddress").text(data.dt[0].cdefine11); //送货地址
            $("#txtAddress").attr("lngopUseraddressId", data.dt[0].lngopUseraddressId);  //送货地址代号
            var datBillTime = (data.dt[0].datBillTime).slice(6, 19);
            var time = new Date(Number(datBillTime));
            $("#datCreateTime").text(time.toLocaleDateString()); //下单日期
            $("#TxtCustomer").text(data.dt[0].ccusname);//开票单位
            $("#TxtCustomer").attr("cpersoncode", data.dt[0].cpersoncode); //业务员
            $("#strLoadingWays").val(data.dt[0].strLoadingWays); //装车方式
            $("#cdefine3").text(data.dt[0].cdefine3);  //车型
            if (data.dt[0].lngBillType == 0) {
                $("#cSTCode").text("普通订单");
                $("#cSTCode").attr("cSTCode", data.dt[0].cSTCode);
            }
            if (data.dt[0].lngBillType == 1) {
                $("#cSTCode").text("酬宾订单");
                $("#cSTCode").attr("cSTCode", data.dt[0].cSTCode);
            }
            if (data.dt[0].lngBillType == 2) {
                $("#cSTCode").text("特殊订单");
                $("#cSTCode").attr("cSTCode", data.dt[0].cSTCode);
            }
            form.render();

            var html = "";
            $.each(data.datatable, function (i, v) {
                html += "<tr>";
                html += "<td class='ui-widget-content'><a href='javascript:void(0)'  class='del_this '>删除</a><a href='javascript:void(0)'  class='up_this'>上移</a><a href='javascript:void(0)'  class='down_this'>下移</a></td>";
                html += "<td code=" + v.cInvCode + ">" + (i + 1) + "</td>";
                html += "<td cinvdefine13=" + v.cInvDefine13 + " cinvdefine14=" + v.cInvDefine14 + " cunitid=" + v.cComUnitCode + " >" + v.cInvName + "</td>";
                html += "<td>" + v.cInvStd + "</td>";
                html += "<td>" + v.UnitGroup + "</td>";
                html += "<td class='in'>" + v.cComUnitQTY + "</td>";
                html += "<td>" + v.cComUnitName + "</td>";
                html += "<td class='in'>" + v.cInvDefine2QTY + "</td>";
                html += "<td>" + v.cInvDefine2 + "</td>";
                html += "<td class='in'>" + v.cInvDefine1QTY + "</td>";
                html += "<td>" + v.cInvDefine1 + "</td>";
                html += "<td>" + v.cInvDefineQTY + "</td>";
                html += "<td>" + v.Stock + "</td>";
                html += "<td>" + v.pack + "</td>";
                html += "<td>" + v.cComUnitPrice + "</td>";
                html += "<td>" + (v.cComUnitAmount).toFixed(2) + "</td>";
                html += "<td style='display:none'>" + v.ExercisePrice + "</td><td style='display:none'>" + ((v.cInvDefineQTY) * (v.ExercisePrice)).toFixed(2) + "</td>";
                html += "</tr>";
            })
            $("#buy_list tbody").html(html);
            $("#money").text(get_money("buy_list"));
            set_table_color("buy_list");
            if ((data.msg).length != 0) {
                var s = "有" + (data.msg).length + "件商品未找到： <br/>", count = 1;
                $.each(data.msg, function (i, v) {
                    s = s + count + "、" + v + "<br/>";
                    count++;
                })
                s = s + "剩余商品信息提取完成！"
                layer.alert(s);
            }
        },
        error: function (err) {
            console.log(err);
        }

    })

    //JQ ajax全局事件
    $(document).ajaxStart(function () {
        layer.load();
    }).ajaxComplete(function (request, status) {
        layer.closeAll('loading');
    });

    //选择开票单位，刷新信用额度及购物清单
    form.on('select(kpdw)', function (data) {
        //  alert($("#TxtCusCredit").text());
        if (data.value == 0) {
            return false;
        }
        var tds = $("#buy_list tbody tr").find("td:eq(1)"),
            c = [],
            trs = $("#buy_list tbody tr");
        $.each(trs, function (i, v) {
            c.push($(v).find("td:eq(1)").attr("code"));
        });
        $.ajax({
            traditional: true,
            type: "Post",
            url: "Handler/BaseHandler.ashx",
            data: {
                "Action": "Change_KPDW",
                "kpdw": data.value,
                "codes": c
            },
            success: function (data) {
                data = eval('(' + data + ')');
                $("#TxtCusCredit").text(data.msg[0]);
                $("#TxtCusCredit span").text(data.msg[1]);
                $.each(trs, function (i, v) {
                    if ($(v).find("td:eq(1)").attr("code") == data.messages[i][0]) {
                        $(v).find("td:eq(12)").text(data.messages[i][1]);
                        $(v).find("td:eq(14)").text(data.messages[i][2]);
                        $(v).find("td:eq(16)").text(data.messages[i][3]);
                        $(v).find("td:eq(15)").text(($(v).find("td:eq(11)").text() * $(v).find("td:eq(14)").text()).toFixed(
                            2));
                    }
                })
                set_table_num("buy_list");
                $("#money").text(get_money("buy_list"));
                set_table_color("buy_list");

            },
            error: function (e) {
                layer.msg("数据刷新失败，请重试！", {
                    icon: 2
                });
            }
        })
    });


    //通过radio点击，ajax获取送货地址及行政区
    form.on('radio(shfs)', function (data) {
        var d = data.value;
        $.ajax({
            //要用post方式     
            type: "Post",
            //方法所在页面和方法名     
            url: "../Buy.aspx/Get_UserAddress",
            // contentType: "application/json; charset=utf-8",
            //   dataType: "text",
            data: "{'da':'" + d + "'}",
            contentType: "application/json; charset=utf-8",

            // dataType: "json",
            success: function (data) {
                var address = eval('(' + (data.d)[1] + ')');
                var area = eval('(' + (data.d)[0] + ')');
                $("#txtAddress").empty(); //清空地址下拉
                $("#txtArea").empty(); //清空行政区下拉
                $("#txtAddress").append("<option value='0'>请选择送货地址</option>");
                var option = "";
                $.each(address, function (i, v) {
                    // option += $("<option>").val(address[i].lngopUseraddressId).text(address[i].ShippingInformation);
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

                $("#txtArea").append("<option value='0'>请选择行政区</option>");
                $.each(area, function (i, v) {
                    // option += $("<option>").val(address[i].lngopUseraddressId).text(address[i].ShippingInformation);
                    $("#txtArea").append("<option value='" + area[i].lngopUseraddress_exId + "'>" + area[i].xzq +
                        "</option>");
                })

                form.render('select');

            },
            error: function (err) {
                alert("error");
            }
        });
    });


    //右下功能图标
    util.fixbar({
        bar1: 'ဇ',
        bar2: '',
        click: function (type) {
            if (type === 'bar1') {
                if ($("#jbxx").css("display") == "none") {
                    layer.msg("显示基本信息");
                    $("#jbxx").show();
                } else {
                    $("#jbxx").hide();
                    layer.msg("隐藏基本信息");
                }
            }

            if (type === 'bar2') {
                layer.msg('意见反馈')
            }

            if (type === 'top') {
                layer.msg('返回顶部')
            }
        }
    });


    //一键清除零库存
    $("#del_none").click(function () {
        layer.confirm("你确定要删除无库存的商品？", function () {
            var trs = $("#buy_list tbody tr");
            $.each(trs, function (i, v) {
                if ($(v).find("td:eq(12)").text() == 0) {
                    codes.splice($.inArray($(v).find("td:eq(1)").attr("code"), codes), 1);
                    $(v).remove();
                    set_table_num("buy_list");
                    $("#money").text(get_money("buy_list"));
                    layer.closeAll();
                }

            })
        })


    })






    //加载商品清单后，判断清单里是否有零库存或者库存小于购买数量的商品，并标注相应行

    function set_table_color(table_id) {
        var trs = $("#" + table_id + " tbody tr");
        $.each(trs, function (i, v) {
            $(v).removeClass("red").removeClass("yellow");
        })
        $.each(trs, function (i, v) {
            if ($(v).find("td:eq(12)").text() == 0) {
                $(v).addClass("red");
            } else if (($(v).find("td:eq(11)").text() - $(v).find("td:eq(12)").text()) > 0) {
                $(v).addClass("yellow");
            }
        })
    }


    //对table重新排序

    function set_table_num(table_id) {
        var tds = $("#" + table_id + " tr").find("td:eq(1)");
        $.each(tds, function (i, v) {
            v.innerText = (i + 1);
        })
    };

    //备注信息弹窗
    $("#strRemarks").click(function () {
        layer.open({
            type: 1,
            title: "备注",
            area: ['300px', '300px'],
            btn: ['确定', '取消'],
            content: "<textarea style='width:296px;height:182px' id='Remarks'>" + $("#strRemarks").val() + "</textarea>",
            btn1: function () {
                $("#strRemarks").val($("#Remarks").val());
                layer.closeAll();
            },
        })
    });


    //执行金额,保留两位小数

    function get_money(table_id) {
        var money = 0;
        var trs = $("#" + table_id + " tbody tr");
        $.each(trs, function (i, v) {
            money = money + Number($(v).find("td:eq(17)").text());
        })
        return money.toFixed(2);
    }

    //操作购物清单，输入数值获取结果
    $(document).on("click", ".in", function () {

        var td = $(this);
        var tds = td.parent().find("td");

        if (td.parents("tbody").find("input").length > 0) {
            layer.msg("上次输入不正确，请重新输入", {
                icon: 2
            });
            return false;
        }
        //  var unit_s = tds.eq(1).attr("unit_s"), unit_m = tds.eq(1).attr("unit_m"), unit_b = tds.eq(1).attr("unit_b");
        var unit_s = 1,
            unit_m = tds.eq(2).attr("cinvdefine13"),
            unit_b = tds.eq(2).attr("cinvdefine14");
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
                    return false;
                } else if (td.next().text() != "米" && !(/^[0-9]\d*$/).test(val) && val != "") {
                    layer.msg("输入数值不合法请重新输入");
                    return false;
                }
            } else {
                if (td.next().text() != "米" && !(/^[0-9]\d*$/).test(val) && val != "") {
                    layer.msg("输入数值不合法请重新输入");
                    return false;
                }

            }
            var input_blur = $(this);
            var newText = input_blur.val();
            td.html(newText);
            //基本单位汇总
            tds.eq(11).text(tds.eq(5).text().mul(unit_s) + tds.eq(7).text().mul(unit_m) + tds.eq(9).text().mul(unit_b));
            //总金额
            tds.eq(15).text((tds.eq(11).text().mul(tds.eq(14).text())).toFixed(2));
            //折扣金额
            tds.eq(17).text(tds.eq(11).text().mul(tds.eq(16).text()));
            //包装结果
            // tds.eq(13).text(tds.eq(5).text()+tds.eq(6).text());
            if (((tds.eq(11).text().mul(100)) % ((unit_b).mul(100)) == 0)) {
                tds.eq(13).text(parseInt((tds.eq(11).text()).div(unit_b)) + tds.eq(10).text());
            } else {
                if ((((tds.eq(11).text().mul(100)) % (unit_b.mul(100))) % (unit_m.mul(100))) / 100 == 0) {
                    tds.eq(13).text(
                        parseInt(tds.eq(11).text().div(unit_b)) + tds.eq(10).text() + parseInt(((tds.eq(11).text().mul(100)) %
                        (unit_b.mul(100))) / (unit_m.mul(100))) + tds.eq(8).text());
                } else {
                    tds.eq(13).text(
                        parseInt(tds.eq(11).text().div(unit_b)) + tds.eq(10).text() + parseInt(((tds.eq(11).text().mul(100)) %
                        (unit_b.mul(100))) / (unit_m.mul(100))) + tds.eq(8).text() +
                        parseInt(((tds.eq(11).text() * 100) % (unit_b * 100)) % (unit_m * 100)) / 100 + tds.eq(6).text());
                }
            }

            //如果库存不为0，则移除样式
            if ($(td).parents("tr").find("td:eq(12)").text() != 0) {
                $(td).parent().removeClass("red");
            }

            //执行金额
            $("#money").text(get_money("buy_list"));
        });
    }).on("click", ".del_this", function () { //绑定行删除事件
        var data_tr = $(this).parent().parent(); //获取到触发的tr  
        //if (confirm("确定要删除当前商品?")) {
        //    var c = $(data_tr).find("td:eq(1)").attr("code");
        //    if ($.inArray(c, codes) != -1) {
        //        codes.splice($.inArray(c, codes), 1);
        //    }
        //    data_tr.remove();
        //    set_table_num("buy_list");
        //    $("#money").text(get_money("buy_list"));
        //}
        layer.confirm("确定要删除当前商品?", {
            icon: 3,
            title: '提示'
        }, function (index) {
            var c = $(data_tr).find("td:eq(1)").attr("code");
            if ($.inArray(c, codes) != -1) {
                codes.splice($.inArray(c, codes), 1);
            }
            data_tr.remove();
            set_table_num("buy_list");
            $("#money").text(get_money("buy_list"));
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
    })

    //弹出产品分类选择页面
    $("#select_product").click(function () {
        product_codes = get_codes("buy_list");
        codes = get_codes("buy_list");
        layer.open({
            type: 2
              , area: ["1020px", "630px"]
              , title: "选择产品"
              , content: "select_product.html?cSTCode=01"
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
                          url: "/Handler/BaseHandler.ashx",
                          // dataType: "Json",
                          data: { "Action": "DLproc_QuasiOrderDetailBySel", "codes": add_codes },
                          success: function (data) {
                              $("#buy_list tbody").append(data);
                              layer.msg("添加商品成功！");
                              layer.close(index);
                              set_table_num("buy_list");
                              var trs = $("#buy_list tr");
                              // $.each(trs, function (i, v) {
                              //     if ($(v).find("td:eq(12)").text() == 0) {
                              //         $(v).addClass("red");
                              //     }
                              //      $(v).find("td:eq(7)").removeClass("in");
                              //     $(v).find("td:eq(9)").removeClass("in");
                              // })
                          }
                      })
                  } else {
                      set_table_num("buy_list");
                      layer.closeAll();
                  }
              }

        })

    });
});

//获取产品列表里已选择的产品，拼接为数组
function get_codes(table_id) {
    var c = [];
    var tds = $("#" + table_id + " tr").find("td:eq(1)");
    $.each(tds, function (i, v) {
        c.push($(v).attr("code"));
    })
    return c;
};


//提交正式订单
$("#submit_buy_list").click(function () {

    var strBillNo = $("#strBillNo").text().trim();
    if (strBillNo == "" || strBillNo == null) {
        layer.alert("数据出现问题，请重试或联系管理员！", { icon: 2 });
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
    $.each($("#buy_list tr").find("td:eq(11)"), function (i, v) {
        if ($(v).text() == 0 || $(v).text() == "") {
            $(v).parent().addClass("red");
            flag = false;
        }
    })
    if (!flag) {
        layer.msg("购物清单里有未输入数量商品！", { icon: 2 });
        return false;
    }


    //拼接ajax发送的表体数据
    var trs = $("#buy_list tbody tr"), buy_list = [];
    $.each(trs, function (i, v) {
        var product = {};
        product.irowno = $(v).find("td:eq(1)").text();         //行号
        product.cinvcode = $(v).find("td:eq(1)").attr("code"); //存货编码
        product.cinvname = $(v).find("td:eq(2)").text();      //存货名称
        product.iquantity = $(v).find("td:eq(11)").text() != "" ? $(v).find("td:eq(11)").text() : "0";    //汇总数量
        product.iquotedprice = $(v).find("td:eq(14)").text();  //报价,保留5位小数,四舍五入
        product.itaxrate = $(v).find("td:eq(2)").attr("itaxrate");//税率
        product.kl = $(v).find("td:eq(2)").attr("kl");          //扣率
        product.cComUnitName = $(v).find("td:eq(6)").text();  //基本单位名称
        product.cInvDefine1 = $(v).find("td:eq(10)").text();  //大包装单位名称     
        product.cInvDefine2 = $(v).find("td:eq(8)").text();   //小包装单位名称 
        product.cInvDefine13 = $(v).find("td:eq(2)").attr("cInvDefine13");//大包装换算率
        product.cInvDefine14 = $(v).find("td:eq(2)").attr("cInvDefine14");//小包装换算率
        product.unitGroup = $(v).find("td:eq(4)").text();    //单位换算率组
        product.cComUnitQTY = $(v).find("td:eq(5)").text() != "" ? $(v).find("td:eq(5)").text() : "0";  //基本单位数量
        product.cInvDefine2QTY = $(v).find("td:eq(7)").text() != "" ? $(v).find("td:eq(7)").text() : "0"; //小包装单位数量
        product.cInvDefine1QTY = $(v).find("td:eq(9)").text() != "" ? $(v).find("td:eq(9)").text() : "0";//大包装单位数量
        product.itaxunitprice = $(v).find("td:eq(16)").text();//原币含税单价,即执行价格dt.Columns.Add("ExercisePrice"); //基本单位单价(执行价格)   16,保留5位
        product.cunitid = $(v).find("td:eq(2)").attr("cunitid");////基本计量单位编码
        product.cDefine22 = $(v).find("td:eq(13)").text();//表体自定义项22,包装量
        buy_list.push(product);
    });

    $.ajax({
        timeout: 10000,
        type: "Post",
        url: "Handler/BaseHandler.ashx",
        dataType: "Json",
        data: {
            "Action": "DLproc_SampleOrderByUpd",
            "strBillNo": GetQueryString("strBillNo"),
            "strRemarks": $("#strRemarks").val(),
            "strLoadingWays": $("#strLoadingWays").val(),
            "buy_list": JSON.stringify(buy_list)
        },
        success: function (data) {
            console.log(data.message);
            if (data.flag == 1) {
                layer.alert(data.message, { icon: 2 });
                layer.closeAll('loading');
                //set_table_color("buy_list");
            }
            else if (data.flag == 0) {
                //layer.msg("订单提交成功！", { icon: 1 });
                layer.alert("订单提交成功！订单编号为:\n" + data.message, { icon: 6 }, function () {
                    window.location.href = "Reject_Order.aspx";
                });
            }
        },
        err: function (err) {
            layer.alert(err);
        }
    })

})


//加载商品清单后，判断清单里是否有零库存或者库存小于购买数量的商品，并标注相应行
function set_table_color(table_id) {
    var trs = $("#" + table_id + " tbody tr");
    $.each(trs, function (i, v) {
        $(v).removeClass("red").removeClass("yellow");
    })
    $.each(trs, function (i, v) {
        if ($(v).find("td:eq(12)").text() == 0) {
            $(v).addClass("red");
        } else if (($(v).find("td:eq(11)").text() - $(v).find("td:eq(12)").text()) > 0) {
            $(v).addClass("yellow");
        }
    })
}


//采用正则表达式获取地址栏参数 
function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}

function accDiv(arg1, arg2) {
    var t1 = 0, t2 = 0, r1, r2;
    try { t1 = arg1.toString().split(".")[1].length } catch (e) { }
    try { t2 = arg2.toString().split(".")[1].length } catch (e) { }
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
//乘法函数，用来得到精确的乘法结果
//说明：javascript的乘法结果会有误差，在两个浮点数相乘的时候会比较明显。这个函数返回较为精确的乘法结果。
//调用：accMul(arg1,arg2)
//返回值：arg1乘以arg2的精确结果
function accMul(arg1, arg2) {
    var m = 0, s1 = arg1.toString(), s2 = arg2.toString();
    try { m += s1.split(".")[1].length } catch (e) { }
    try { m += s2.split(".")[1].length } catch (e) { }
    return Number(s1.replace(".", "")) * Number(s2.replace(".", "")) / Math.pow(10, m);
}
//给Number类型增加一个mul方法，调用起来更加方便。
String.prototype.mul = function (arg) {
    return accMul(arg, this);
};
//加法函数，用来得到精确的加法结果
//说明：javascript的加法结果会有误差，在两个浮点数相加的时候会比较明显。这个函数返回较为精确的加法结果。
//调用：accAdd(arg1,arg2)
//返回值：arg1加上arg2的精确结果
function accAdd(arg1, arg2) {
    var r1, r2, m;
    try { r1 = arg1.toString().split(".")[1].length } catch (e) { r1 = 0 }
    try { r2 = arg2.toString().split(".")[1].length } catch (e) { r2 = 0 }
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
    try { r1 = arg1.toString().split(".")[1].length } catch (e) { r1 = 0 }
    try { r2 = arg2.toString().split(".")[1].length } catch (e) { r2 = 0 }
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