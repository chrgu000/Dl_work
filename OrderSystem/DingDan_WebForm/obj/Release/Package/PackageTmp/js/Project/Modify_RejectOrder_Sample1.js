var cstcode = 01;
var codes = [];  //全局变量，本页面添加和删除产品均会在此变量里反映
var product_codes = [];
var kpdw;
var iShowType = 1;
layui.use(['form', 'util', 'laytpl', 'laydate', 'layer'], function () {
    var form = layui.form(),
     util = layui.util,
      laydate = layui.laydate,
      laytpl = layui.laytpl,
        layer = layui.layer;



    $.ajax({
        url: "../Handler/ProductHandler.ashx",
        dataType: "Json",
        type: "Post",
        data: { "Action": "DLproc_OrderDetailModifyBySel", "strBillNo": GetQueryString("strBillNo"),"iShowType":1 },
        aysnc: false,
        success: function (data) {
            $("#view").load("../tpl/buy_orderTpl.html?v=" + new Date().getTime(), function (da, status, XHR) {
                 if (status == 'success') 
                 {
                    $(".btns").append('  <input type="button" class="layui-btn layui-btn-small " id="btn" value="返回列表" />');
                    $("#select_product").attr("id","select_product_YP");
                    $("#select_SpecialProduct").hide();
                    $("#select_history_order").hide();
                    $("#select_temp_order").hide();
                    $("#alert_buy_temp").hide();
                    $("#info_div").removeClass("layui-hide");
                    $("#strBillNo").text(data.dt[0].strBillNo);     //主订单号
                    $("#strUserName").text(data.dt[0].ccusname); //制单人 
                       if (data.dt[0].lngBillType == 0) {
                        $("#cSTCode").text("普通订单");
                        $("#cSTCode").attr("cSTCode", data.dt[0].cSTCode);
                    };
                    if (data.dt[0].lngBillType == 1) {
                        $("#cSTCode").text("酬宾订单");
                        $("#cSTCode").attr("cSTCode", data.dt[0].cSTCode);
                    };
                    if (data.dt[0].lngBillType == 2) {
                        $("#cSTCode").text("特殊订单");
                        $("#cSTCode").attr("cSTCode", data.dt[0].cSTCode);
                    };
                    $("#TxtCusCredit").text((data.CusCredit_dt[0].iCusCreLine).toFixed(2));  //信用额
                    $("#datCreateTime").text(return_date(data.dt[0].datCreateTime)); //下单日期
                    $("#TxtCustomer_div").html(data.dt[0].ccusname);//开票单位名称
                    kpdw=data.dt[0].ccuscode; //开票单位编码
                    $("#strRemarks").val(data.dt[0].strRemarks);//备注
                    if (data.dt[0].cSCCode == 00) {
                        $("#shfs_div").text("自提"); //送货方式
                        $("#shfs_div").attr("cSCCode", "00");  //送货方式编码
                    }
                    if (data.dt[0].cSCCode == 01) {
                        $("#shfs_div").text("配送"); //送货方式
                        $("#shfs_div").attr("cSCCode", "01");  //送货方式编码
                    }
                    $("#txtAddress_div").html(data.dt[0].cdefine11); //送货地址
                    $("#txtAddress_div").attr("lngopUseraddressId", data.dt[0].lngopUseraddressId);  //送货地址代号
                    $("#TxtCustomer").attr("cpersoncode", data.dt[0].cpersoncode); //业务员
                    $("#strLoadingWays").val(data.dt[0].strLoadingWays); //装车方式
                    $("#cdefine3_div").text(data.dt[0].cdefine3);  //车型
                    $("#datDeliveryDate_div").html(return_datetime(data.dt[0].datDeliveryDate));//提货时间
                    $("#txtArea_div").html("");
                    form.render();
                  
                    $("#buy_list tbody").html(get_html_num(data.datatable));
                    $("#buy_list thead tr th:eq(12)").hide();
                    $("#buy_list tbody tr").find(".realqty").hide();
                     // $("#money").text(get_money("buy_list"));
                    var listInfo = get_listInfo("buy_list");
                    $("#money").text(listInfo.money);
                    $("#pro_weight").text(listInfo.weight)
                   
                    
                    if (data.msg.length > 0) {
                        var s = "有" + data.msg.length + "件商品未找到： <br/>", count = 1;
                        $.each(data.msg, function (i, v) {
                        s = s + count + "、" + v + "<br/>";
                        count++;
                    })
                        s = s + "剩余商品信息提取完成！"
                    layer.alert(s,{icon:7});
                    };

                   //  set_table_color("buy_list");
                    // $("#money").text(get_money("buy_list"));
                     
                 }
            });
          
        }
    })

//弹出产品分类选择页面
$(document).on("click", "#select_product_YP", function () {
     
    if (kpdw == 0 || kpdw == "" || kpdw == 'undefined' || kpdw == null) {
        layer.alert("请先选择开票单位!", { icon: 2 });
        return false;
    }
    product_codes = get_codes("buy_list");
    codes = get_codes("buy_list");
    layer.open({
        type: 2
          , area: ["1020px", "630px"]
          , title: "选择产品"
          , content: "select_product.html?cSTCode=" + cstcode + "&kpdw=" + kpdw + "&iShowType=1" 
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
                      data: { "Action": "DLproc_QuasiOrderDetailBySel_new", "codes": add_codes,"kpdw":kpdw ,"areaid":'0'},
                      success: function (data) {
                          if (data.flag=="0") {
                              layer.alert("请先选择开票单位!", { icon: 2 });
                              return false;
                          }
                          $("#buy_list tbody").append(get_html(data.dt));
                          $("#buy_list tbody tr").find(".realqty").hide();
                          layer.msg("添加商品成功！");
                          layer.close(index);
                          set_table_num("buy_list");
                          set_table_color("buy_list");
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

    //提交正式订单
    $(document).on("click", "#submit_buy_list", function () {

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

        $.ajax({
            
            traditional: true,
            type: "Post",
            url: "../Handler/ProductHandler.ashx",
            dataType: "Json",
            data: {
                "Action": "DLproc_SampleOrderByUpd",
                "strBillNo": strBillNo,
                "strRemarks": $("#strRemarks").val(),
                "strLoadingWays": $("#strLoadingWays").val(),
                "kpdw":kpdw,
                "listData": JSON.stringify(get_listData("buy_list"))
            },
            success: function (data) {
                if (data.flag=="7") {
                    Product_limit(data,"buy_list");
                    set_table_num("buy_list");
                    return false;
                };
                if (data.flag=="0") {
                    layer.alert(data.message,{icon:2});
                    return false;
                };
                if (data.flag=="1") {
                     layer.alert("订单提交成功,订单编号为:<br />"+data.message,{icon:1},function(){
                        window.location="Reject_Order.html";
                     });
                }

            },
            err: function (err) {
                layer.alert(err);
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

$(document).on("click","#btn",function(){
    window.location="Reject_Order.html";
   })

//采用正则表达式获取地址栏参数 
function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]);
    return null;
}