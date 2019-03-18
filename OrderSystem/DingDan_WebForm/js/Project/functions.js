

//操作购物清单，输入数值获取结果
$(document).on("click", ".in", function () {

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
        var num_s = tr.find(".num_s").text(),
            num_m = tr.find(".num_m").text(),
            num_b = tr.find(".num_b").text();

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
                     parseInt(tr.find(".num").text().div(unit_b)) + tr.find(".cInvDefine1").text() + parseInt(((tr.find(".num")*100) %
                     (unit_b.mul(100))) / (unit_m.mul(100))) + tr.find(".cInvDefine2").text() +
                     parseInt(((tr.find(".num").text() * 100) % (unit_b * 100)) % (unit_m * 100)) / 100 + tr.find(".cComUnitName").text());
            }
        }

   
        //执行金额
        $("#money").text(get_money("buy_list"));
    });
}).on("click", ".del_this", function () { //绑定行删除事件
    var data_tr = $(this).parent().parent(); //获取到触发的tr  
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
});



//执行金额,保留两位小数
function get_money(table_id) {
    var money = 0;
    var tds = $("#" + table_id + " tbody").find(".ex_sum");
    $.each(tds, function (i, v) {
        money = money + Number($(v).text());
    })
    return money.toFixed(2);
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
    var tds = $("#" + table_id + " tr").find("td:eq(1)");
    $.each(tds, function (i, v) {
        c.push(tr.attr("code"));
    })
    return c;
};

//转换日期格式，将格式为“/Date(-2036476800000+0800)/”转换为“2016-09-09”
function return_date(date) {
    if (date != "" && date != null) {
        var new_date = date.slice(6, 19);
        var time = new Date(Number(new_date));
        return time.toLocaleDateString().replace(/\//g, "-");
    }
}
//转换日期格式，将格式为“/Date(-2036476800000+0800)/”转换为“2016-6-25 17:14:38”
function return_datetime(date) {
    if (date != "" && date != null) {
        var new_date = date.slice(6, 19);

        var time = new Date(Number(new_date));
        return time.getFullYear() + "-" + (time.getMonth() + 1) + "-" + time.getDate() + " " + time.getHours() + ":" + time.getMinutes() + ":" + time.getSeconds();
    }
}

//获取产品列表里已选择的产品，拼接为数组
function get_codes(table_id) {
    var c = [];
    var tds = $("#" + table_id + " tr").find("td:eq(1)");
    $.each(tds, function (i, v) {
        c.push($(v).attr("code"));
    })
    return c;
};

//JQ ajax全局事件
$(document).ajaxStart(function () {
    layer.load();
}).ajaxComplete(function (request, status) {
    layer.closeAll('loading');
});

//一键清除零库存
$("#del_none").click(function () {
    layer.confirm("你确定要删除无库存的商品？", function () {
        var trs = $("#buy_list tbody tr");
        $.each(trs, function (i, v) {
            if (tr.find("td:eq(12)").text() == 0) {
                codes.splice($.inArray(tr.find("td:eq(1)").attr("code"), codes), 1);
                tr.remove();
                set_table_num("buy_list");
                $("#money").text(get_money("buy_list"));
                layer.closeAll();
            }

        })
    })
});

//把order详细列表里的数据拼接为数组
function get_tableData(table_id){
                var trs = $("#"+table_id+" tbody tr"), buy_list = [];
                $.each(trs, function (i, v) {
                    var tr=$(v);
                    var product = {};
                    product.irowno = tr.find("td:eq(1)").text();         //行号
                    product.cinvcode = tr.find("td:eq(1)").attr("code"); //存货编码
                    product.cinvname = tr.find(".cInvName").text();      //存货名称
                    product.iquantity = tr.find(".num").text() != "" ? tr.find(".num").text() : "0";    //汇总数量
                    product.iquotedprice = tr.find(".sum").text();  //报价,保留5位小数,四舍五入
                    product.itaxrate = tr.find(".cInvName").attr("itaxrate");//税率
                    product.kl = tr.find(".cInvName").attr("kl");          //扣率
                    product.cComUnitName = tr.find(".cComUnitName").text();  //基本单位名称
                    product.cInvDefine1 = tr.find(".cInvDefine1").text();  //大包装单位名称     
                    product.cInvDefine2 = tr.find(".cInvDefine2").text();   //小包装单位名称 
                    product.cInvDefine13 = tr.find(".cInvName").attr("cinvdefine13");//大包装换算率
                    product.cInvDefine14 = tr.find(".cInvName").attr("cinvdefine14");//小包装换算率
                    product.unitGroup = tr.find(".unitGroup").text();    //单位换算率组
                    product.MinOrderQTY=tr.find(".MinOrderQTY").text();  //起订量
                    product.cComUnitQTY = tr.find(".num_s").text() != "" ? tr.find(".num_s").text() : "0";  //基本单位数量
                    product.cInvDefine2QTY = tr.find(".num_m").text() != "" ? tr.find(".num_m").text() : "0"; //小包装单位数量
                    product.cInvDefine1QTY = tr.find(".num_b").text() != "" ? tr.find(".num_b").text() : "0";//大包装单位数量
                    product.itaxunitprice = tr.find(".ex_price").text();//原币含税单价,即执行价格dt.Columns.Add("ExercisePrice"); //基本单位单价(执行价格)   16,保留5位
                    product.cunitid = tr.find(".cInvName").attr("cunitid");////基本计量单位编码
                    product.cDefine22 = tr.find(".pack").text();//表体自定义项22,包装量
                    buy_list.push(product);
                });
                return buy_list;
}



layui.use(['form', 'util', 'laytpl', 'laydate', 'layer'], function () {
    var form = layui.form(), layer = layui.layer;



    //通过radio点击，ajax获取送货地址及行政区

    form.on('radio(shfs)', function (data) {
        var d = data.value;
        get_address(d);
    });


    function get_address(d) {
        $.ajax({
            type: "Post",
            url: "Handler/BaseHandler.ashx",
            data: { "Action": "DLproc_UserAddressZTBySelGroup", "shfs": d },
            dataType: "Json",
            success: function (data) {
                var address = data.list_dt[0];
                var area = data.list_dt[1];
                $("#txtAddress").empty(); //清空地址下拉
                $("#txtArea").empty();    //清空行政区下拉
                $("#txtAddress").append("<option value='0'>请选择送货地址</option>");
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
                $("#txtArea").append("<option value='0'>自提必须选择行政区</option>");
                $.each(area, function (i, v) {
                    $("#txtArea").append("<option value='" + area[i].lngopUseraddress_exId + "'>" + area[i].xzq + "</option>");
                })

                form.render('select');

            },
            error: function (err) {
                alert("error");
            }
        });
    }

    //选择开票单位，刷新信用额度及购物清单
    form.on('select(kpdw)', function (data) {
        if (data.value == 0) {
            return false;
        }
        var tds = $("#buy_list tbody tr").find("td:eq(1)"), c = [], trs = $("#buy_list tbody tr");
        $.each(trs, function (i, v) {
            c.push($(v).find("td:eq(1)").attr("code"));
        });
        $.ajax({
            traditional: true,
            type: "Post",
            url: "Handler/BaseHandler.ashx",
            data: { "Action": "Change_KPDW", "kpdw": data.value, "codes": c },
            success: function (data) {
                data = eval('(' + data + ')');
                $("#TxtCusCredit").text(data.msg[0]);
                $("#TxtCusCredit span").text(data.msg[1]);
                $.each(trs, function (i, v) {
                    if ($(v).find("td:eq(1)").attr("code") == data.messages[i][0]) {
                        $(v).find(".fAvailQtty").text(data.messages[i][1]);
                        $(v).find(".price").text(data.messages[i][2]);
                        $(v).find(".ex_price").text(data.messages[i][3]);
                        $(v).find(".ex_sum").text(($(v).find(".num").text() * $(v).find(".ex_price").text()).toFixed(2));
                    }
                })
                set_table_num("buy_list");
                $("#money").text(get_money("buy_list"));
                set_table_color("buy_list");

            },
            error: function (e) {
                layer.msg("数据刷新失败，请重试！", { icon: 2 });
            }
        })
    });

    //备注信息弹窗
    // $("#strRemarks").click(function () {
    $(document).on("click", "#strRemarks", function () {
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
    })

})




//弹出产品分类选择页面
$(document).on("click", "#select_product", function () {
    product_codes = get_codes("buy_list");
    codes = get_codes("buy_list");
    layer.open({
        type: 2
          , area: ["1020px", "630px"]
          , title: "选择产品"
          , content: "select_product_new.html?cSTCode=" + cstcode
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
                      dataType: "Json",
                      data: { "Action": "DLproc_QuasiOrderDetailBySel_new", "codes": add_codes,"kpdw":$("#TxtCustomer").val() },
                      success: function (data) {
                          $("#buy_list tbody").append(get_html(data));
                          layer.msg("添加商品成功！");
                          layer.close(index);
                          set_table_num("buy_list");
                      }
                  })
              } else {
                  set_table_num("buy_list");
                  layer.closeAll();
              }
          }

    })

});

//传入产品数据，拼接为html返回
function get_html(data) {
    var html = "";
    $.each(data, function (i, v) {
        var unit_b = (v.cinvdefine13).toString();
        var unit_m = (v.cinvdefine14).toString();
        html += "<tr>";
        html += "<td class='ui-widget-content'><a href='javascript:void(0)'  class='del_this '>删除</a><a href='javascript:void(0)'  class='up_this'>上移</a><a href='javascript:void(0)'  class='down_this'>下移</a></td>";
        html += "<td code=" + v.cInvCode + ">" + (i + 1) + "</td>";
        html += "<td class='cInvName' cinvdefine13=" + unit_b + " cinvdefine14=" + unit_m + " cunitid=" + v.cComUnitCode + "  kl=" + v.Rate + " iTaxRate=" + v.iTaxRate + ">" + v.cInvName + "</td>";
        html += "<td>" + v.cInvStd + "</td>";
        html += "<td class='unitGroup'>" + unit_b + v.cComUnitName + "=" + (unit_b.div(unit_m)) + v.cInvDefine2 + "=1" + v.cInvDefine1 + "</td>";
        html += "<td class='in num_s'></td>";
        html += "<td class='cComUnitName'>" + v.cComUnitName + "</td>";
        html += "<td class='in num_m'></td>";
        html += "<td class='cInvDefine2'>" + v.cInvDefine2 + "</td>";
        html += "<td class='in num_b'></td>";
        html += "<td class='cInvDefine1'>" + v.cInvDefine1 + "</td>";
        html += "<td class='num'></td>";
        html += "<td class='MinOrderQTY'>" + v.MinOrderQTY + "</td>";
        html += "<td class='fAvailQtty'>" + v.fAvailQtty + "</td>";
        html += "<td class='pack'></td>";
        html += "<td class='price'>" + v.nOriginalPrice + "</td>";
        html += "<td class='sum'></td>";
        html += "<td class='ex_price' style='display:none'>" + (v.ExercisePrice).toFixed(6) + "</td><td style='display:none' class='ex_sum'></td>";
        html += '<td style="display:none" class="unit_m">' + v.cinvdefine14+ '</td>';
        html += '<td style="display:none" class="unit_b">' + v.cinvdefine13+ '</td>';
        html += "</tr>";
    })
    return html;
}


//加载商品清单后，判断清单里是否有零库存或者库存小于购买数量的商品，并标注相应行
function set_table_color(table_id) {
 var trs=$("#buy_list tbody tr");
        $.each(trs, function (i, v) {
            $(v).removeClass("red").removeClass("yellow");
            if (Number($(v).find(".num").text()) == 0 || $(v).find(".num").text() == "") {
                $(v).addClass("red");
            }
            if ($(v).find(".MinOrderQTY").text()>0) {
                   if (Number($(v).find(".num").text())<Number($(v).find(".MinOrderQTY").text())) {
                $(v).addClass("yellow");
            }
            }
         
        })
 
};

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

