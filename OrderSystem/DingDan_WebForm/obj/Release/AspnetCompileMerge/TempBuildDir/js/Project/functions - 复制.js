var datatable_options={
                        "columns":[
        {"data":null,"title":"操作","class":"center","width":"80",
            render:function(data, type, row, meta){
                return '<a>删除</a><a>上移</a><a>下移</a>';
            }
        },
        {"data":null,"title":"序号","class":"center","width":"30" },
        {"data":"cInvName","title":"名称","class":"center","width":"200"},
        {"data":"cInvStd","title":"规格","class":"center","width":"100"},
        {"data":null,"title":"单位组","class":"center","width":"80",
            render:function(){
                return "";
            }
        },
        {"data":null,"title":"基本数量","class":"center in","width":"60",
             render:function(){
                return "";
            }
        },
        {"data":"cComUnitName","title":"基本单位","class":"center","width":"60"},
        {"data":null,"title":"小包装数量","class":"center in", "width":"75",
             render:function(){
                return "";
            }
        },
        {"data":"cInvDefine2","title":"小包装单位","class":"center","width":"75",},
        {"data":null,"title":"大包装数量","class":"center in","width":"75",
             render:function(){
                return "";
            }
        },
        {"data":"cInvDefine1","title":"大包装单位","class":"center","width":"75"},
        {"data":null,"title":"基本单位汇总","class":"center","width":"90",
             render:function(){
                return "";
            }
        },
        {"data":"fAvailQtty","title":"可用库存量","class":"center","width":"100"},
        {"data":null,"title":"包装结果","class":"center","width":"120",
             render:function(){
                return "";
            }
        },
        {"data":"Quote","title":"单价","class":"center","width":"60"},
        {"data":null,"title":"金额","class":"center","width":"60",
             render:function(data, type, row, meta){
                return "";
            }
        },
     
          {"data":"ExercisePrice","title":"执行价格","class":"center","width":"60","visible":true,
              render:function(data, type, row, meta){
                return   data.toFixed(6) ;
            }
        },
          {"data":null,"title":"执行金额","class":"center","width":"60",
                render:function(data, type, row, meta){
                return   "";
            }
        },
             {"data":"cInvCode","title":"ID","class":"center","width":"60"},
             {"data":"cinvdefine13","title":"cinvdefine13","class":"center","width":"60"},
             {"data":"cinvdefine14","title":"cinvdefine14","class":"center","width":"60"}
        ],
        "language":{
                "lengthMenu": "每页 _MENU_ 条记录",
                "zeroRecords": "没有找到记录",
                "info": "第 _PAGE_ 页 ( 总共 _PAGES_ 页 )",
                "infoEmpty": "无记录",
                "infoFiltered": "(从 _MAX_ 条记录过滤)"
        }
                        };

function draw_table(table_id,data){
    $("#"+table_id).DataTable({
        dom:"",
        paging:false,
          destroy: true,
          data:data,
          "columns":datatable_options.columns,
          "language":datatable_options.language

    })
}

//操作购物清单，输入数值获取结果
$(document).on("click", "#buy_list tbody .in", function () {

    var td = $(this);
    var tds = td.parent().find("td");
    if (td.parents("tbody").find("input").length > 0) {
        layer.msg("上次输入不正确，请重新输入", {
            icon: 2
        });
        return false;
    };
    var table=$("#buy_list").DataTable();
    var unit_s = 1,
        unit_m=table.row(this).data().cinvdefine13;
        unit_b=table.row(this).data().cinvdefine14;
        table.row(this).data().cInvCode=34234234;
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


//执行金额,保留两位小数
function get_money(table_id) {
    var money = 0;
    var trs = $("#" + table_id + " tbody tr");
    $.each(trs, function (i, v) {
        money = money + Number($(v).find("td:eq(17)").text());
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
        c.push($(v).attr("code"));
    })
    return c;
};

//转换日期格式，将格式为“/Date(-2036476800000+0800)/”转换为“2016-09-09”
function return_date(date) {
     if(date!=""&&date!=null){
    var new_date = date.slice(6, 19);
    var time = new Date(Number(new_date));
    return time.toLocaleDateString().replace(/\//g, "-");
}
}
//转换日期格式，将格式为“/Date(-2036476800000+0800)/”转换为“2016-6-25 17:14:38”
function return_datetime(date){
    if(date!=""&&date!=null){
        var new_date = date.slice(6, 19);
   
    var time = new Date(Number(new_date));
    return time.getFullYear()+"-"+(time.getMonth()+1)+"-"+time.getDate()+" "+time.getHours()+":"+time.getMinutes()+":"+time.getSeconds();
     }
}

//获取产品列表里已选择的产品，拼接为数组
function get_codes(table_id) {
    var c = [];
    var table=$("#"+table_id).DataTable();
    $.each(table.rows().data(),function(i,v){
        c.push(v.cInvCode);
    });
    // var tds = $("#" + table_id + " tr").find("td:eq(1)");
    // $.each(tds, function (i, v) {
    //     c.push($(v).attr("code"));
    // })
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

 

        layui.use(['form', 'util', 'laytpl', 'laydate', 'layer'], function () {
    var form = layui.form(),layer=layui.layer;
   


 //通过radio点击，ajax获取送货地址及行政区
       
            form.on('radio(shfs)', function (data) {
                var d = data.value;
                get_address(d);
            });


function get_address(d){
            $.ajax({
                    type: "Post",
                    url: "Handler/BaseHandler.ashx",
                    data: {"Action":"DLproc_UserAddressZTBySelGroup","shfs":d},
                   dataType:"Json",
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
                                $(v).find("td:eq(12)").text(data.messages[i][1]);
                                $(v).find("td:eq(14)").text(data.messages[i][2]);
                                $(v).find("td:eq(16)").text(data.messages[i][3]);
                                $(v).find("td:eq(15)").text(($(v).find("td:eq(11)").text() * $(v).find("td:eq(14)").text()).toFixed(2));
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
              $(document).on("click","#strRemarks",function () {
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
$(document).on("click", "#select_product1", function () {
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
              var add_codes = [];
              var tds = $("#buy_list tr").find("td:eq(1)");                      //不在清单里的产品才重新添加
              //遍历清单数组，如果数组中的元素不在全局变量codes中，则删除此行
              var table=$("#buy_list").DataTable();
              $.each(product_codes, function (i, v) {
                  if ($.inArray(v, codes) == -1) {
                      $.each(table.rows().data(), function (m, n) {
                          if (n.cInvCode == v) {
                           //   $(n).parents("tr").remove();
                           console.log(n);

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
      console.log("codes");
    console.log( codes);
    console.log("product_codes");
    console.log(product_codes);
    console.log("add_codes");
    console.log(add_codes);
              if (add_codes.length > 0) {
                  $.ajax({
                      traditional: true,
                      type: "Post",
                      url: "/Handler/BaseHandler.ashx",
                        dataType: "Json",
                      data: { "Action": "DLproc_QuasiOrderDetailBySel_new", "codes": add_codes,"kpdw":$("#TxtCustomer option:selected").val()},
                      success: function (data) {
                        console.log(data);
                        draw_table("buy_list",data);
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

