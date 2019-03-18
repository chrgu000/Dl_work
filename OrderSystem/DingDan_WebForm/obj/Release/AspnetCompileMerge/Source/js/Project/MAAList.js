var layer = layui.layer,
    canSelect_orders = {},
    MAAOrderId = "", //追加时需要的ID
    MAAList = {}, //所有预约信息，按状态存为OBJ
    MAALists = []; //页面加载时获取的所有已预约信息
var form = layui.form();
var element = layui.element();

$(document).ready(function() {

    $.ajax({
        type: "Post",
        async: false,
        url: "/Handler/ProductHandler.ashx",
        dataType: "Json",
        data: {
            "Action": "Get_MAAList","iType":1
        },
        success: function(res) {
            if (res["flag"] != 1) {
                layer.alert(res.message, {
                    icon: 2
                });
                return false;
            }

            MAALists = res["MAAList"]
            sort_MAALists();
            render_MAAList();
            //  render_MAAList();
            // var len = MAALists.length;
            // var arr = [],
            //  html = "",
            //  n = 0;
            // console.log(MAALists)
            // var MAAlist=[];


            // if (MAALists.length > 0) {
            //  for (var i = 0; i < len; i++) {
            //      if (arr.length == 0) {
            //          arr.push(MAALists[i])
            //      } else {
            //          if (arr[0]["lngopOrderId"] == MAALists[i]["lngopOrderId"]) {
            //              arr.push(MAALists[i]);
            //          } else {
            //              orders[arr[0]["lngopOrderId"]] = arr;
            //              arr = [];
            //              arr.push(MAALists[i]);
            //          }
            //      }
            //      if (i == len - 1) {
            //          orders[arr[0]["lngopOrderId"]] = arr
            //      }
            //  }



            //  // $.each(MAAList, function(i, v) {
            //  //  html += "<tr>";
            //  //  html += '<td><input type="button" value="查看" class="layui-btn   layui-btn-mini show"><input type="button" value="追加" class="layui-btn   layui-btn-mini add"><input type="button" value="作废" class="layui-btn   layui-btn-mini layui-btn-danger cancel"></td>';
            //  //  html += '<td> ' + '000000' + '</td>';
            //  //  html += '<td> 2017-07-28 <br>08:00:00~12:00:00</td>';
            //  //  html += '<td> ' + v["cCarNumber"] + '</td>';
            //  //  html += '<td> ' +v["cDriver"] + '水豆腐</td>';
            //  //  html += '<td> ' + v["cDriverPhone"]+ '3111111111</td>';
            //  //  html += '<td class="strBillNo">' + v["cIdentity"] + '510123198007111111</td>';
            //  //  html += '<td title="今天下午不上班">' + '今天下...' + '</td>';
            //  //  html += '<td class=" lngopOrderId">已过期</td>';
            //  //  html += "</tr>";
            //  // })
            // } else {
            //  //html = '<tr><td colspan=5 style="text-align:center "><span >没有可预约的订单</span></td></tr>'
            // }
            //$("#MAAList  tbody").html(html);

        }
    })



})


//对获取的预约数据进行排序，MAAList按预约号状态进行分类的OBJ，orders是按lngopOrderId进行分类的OBJ,MAAList和orders为全局变量
function sort_MAALists() {
    var arr = [];
    var len = MAALists.length;
    $.each(MAALists, function(i, v) {

        if (!MAAList[v["status"]]) {
            MAAList[v["status"]] = {};
            MAAList[v["status"]][v["cMAACode"]] = v;
        } else {
            if (!MAAList[v["status"]][v["cMAACode"]]) {
                MAAList[v["status"]][v["cMAACode"]] = v;
            }
        }

        // if (arr.length == 0) {
        //  arr.push(MAALists[i])
        // } else {
        //  if (arr[0]["lngopOrderId"] == MAALists[i]["lngopOrderId"]) {
        //      arr.push(MAALists[i]);
        //  } else {
        //      orders[arr[0]["lngopOrderId"]] = arr;
        //      arr = [];
        //      arr.push(MAALists[i]);
        //  }
        // }
        // if(i==len-1){
        //  orders[arr[0]["lngopOrderId"]] = arr
        // }

    })


    console.log(MAAList)
}


//渲染已预约列表
function render_MAAList() {
    var html = "";
    var status = $("#MAAType option:selected").val();
    if (isEmptyObject(MAAList) || isEmptyObject(MAAList[status])) {
        html += "<tr>";
        html += '<td colspan="9" style="text-align:center ">未查询到数据</td>'
        html += "</tr>";
    } else {

        $.each(MAAList[status], function(i, v) {
            html += '<tr>';
            html += '<td><input type="button" value="查看" class="layui-btn   layui-btn-mini show"><input type="button" value="追加" class="layui-btn   layui-btn-mini add"><input type="button" value="作废" class="layui-btn   layui-btn-mini layui-btn-danger cancel"></td>';
            html += '<td  >' + v["cMAACode"] + '</td>';
            html += '<td>' + v["datDate"] + '</td>';
            html += '<td>' + v["cCarNumber"] + '</td>';
            html += '<td>' + v["cDriver"] + '</td>';
            html += '<td>' + v["cDriverPhone"] + '</td>';
            html += '<td>' + v["cIdentity"] + '</td>';
            html += '<td>' + v["cMemo"] + '</td>';
            html += '<td>' + v["status"] + '</td>';
            html += '<td class="layui-hide cMAACode">' + v["cMAACode"] + '</td>';
            html += '<td class="layui-hide MAAOrderId">' + v["MAAOrderID"] + '</td>';
            html += '</tr>';
        })
    }
    $("#MAAList tbody").html(html);
}


form.on('select(status)', function(data) {
    render_MAAList();
})


//查看详情按钮
$(document).on("click", ".show", function() {
    var cMAACode = $(this).parents("tr").find(".cMAACode").text();
    orders = {};
    MAA_arr = [], arr = [];
    console.log(cMAACode)
    $.each(MAALists, function(i, v) {

        if (v["cMAACode"] == cMAACode) {
            if (!orders[v["lngopOrderId"]]) {

                orders[v["lngopOrderId"]] = [];

            }
            orders[v["lngopOrderId"]].push(v);
        }
    })

    var html = "";

    html += ' <div class="layui-collapse"   lay-filter="test">';
    $.each(orders, function(i, v) {
        html += '  <div class="layui-colla-item" ><h2 class="layui-colla-title" lngopOrderId="' + v[0]["lngopOrderId"] + '">' + v[0]["strBillNo"] + '</h2>';
        html += '<div class="layui-colla-content">' + v[0]["strBillNo"];
        html += '</div></div>';
    })
    html += '</div>'

    layer.open({
        area: ['800px', '600px'],
        shadeClose: true,
        type: 0,
        btn: [],
        closeBtn: false,
        title: cMAACode,
        content: html,
        success: function() {
            element.init();
        }
    })

})

//点击折叠面板，加载订单详情
element.on('collapse(test)', function(data) {
    var lngopOrderId = $(this).attr("lngopOrderId");
    var html = "";
    var html = '<div style="width:95%;margin:0 auto"><table class="layui-table" style="max-height:500px;"><thead><th>序号</th><th>产品名称</th><th>产品规格</th><th>基本数量</th><th>包装结果</th></thead><tbody>';
    $.each(orders[lngopOrderId], function(i, v) {
        html += '<tr>';
        html += '<td>' + (i + 1) + '</td>'
        html += '<td>' + v["cinvname"] + '</td>'
        html += '<td>' + v["cInvStd"] + '</td>'
        html += '<td>' + v["iquantity"] + '</td>'
        html += '<td>' + v["cdefine22"] + '</td>'
        html += '</tr>';
    })
    html += '</tbody></table></div>';
    $(this).parent().find(".layui-colla-content").html(html);
});


//追加订单
$(document).on("click", ".add", function() {
    var cMAACode = $(this).parents("tr").find(".cMAACode").text();
    MAAOrderId = $(this).parents("tr").find(".MAAOrderId").text();
    $.ajax({
        type: "Post",
        async: false,
        url: "/Handler/ProductHandler.ashx",
        dataType: "Json",
        data: {
            "Action": "Get_MAAOrders"
        },
        success: function(res) {
            var t = res["ordersTable"];
            var o = {};
            if (t.length == 0) {
                layer.alert("没有可预约的订单", {
                    icon: 2
                })
                return false;
            }
            var html = '';
            html += '<form  class="layui-form" style="margin:10px 10px 0 10px; "><div class="layui-form-item  layui-form-pane"><div class="layui-inline">\
      <label class="layui-form-label">已选订单数</label>\
      <div class="layui-input-inline">\
        <label class="layui-form-label" style="width:190px"> <strong id="select_order_num">0</strong>\
        </label>\
      </div>\
    </div>\
    <div class="layui-inline">\
      <label class="layui-form-label">已选订单重</label>\
      <div class="layui-input-inline">\
        <label class="layui-form-label" style="width:190px"> <strong id="select_order_weight">0吨</strong>\
        </label>\
      </div>\
    </div>\
</div>\
<div style="max-height: 300px;overflow-y: scroll;overflow-x:hidden">\
  <table class="layui-table layui-form" id="order_table">\
    <thead>\
      <tr>\
        <th width="60px">\
           选择</th>\
        <th width="60px">查看明细</th>\
        <th width="100px">订单号</th>\
        <th width="150px">下单时间</th>\
        <th  >开票单位</th>\
        <th class="layui-hide">订单ID</th>\
      </tr>\
    </thead>\
    <tbody>';

            //拼接已预约订单列表html
            var a = {}
            $.each(MAALists, function(i, v) {
                if (v["cMAACode"] == cMAACode) {
                    if (!a[v["lngopOrderId"]]) {
                        a[v["lngopOrderId"]] = []
                    }
                    a[v["lngopOrderId"]].push(v)
                }
            })

            var select_order_num = 0; //已预约订单数
            var select_order_weight = 0; //已预约订单重量

            $.each(a, function(i, v) {
                $.each(v, function(m, n) {
                    select_order_weight += n.iInvWeight * n.iquantity;
                })
                select_order_num++;
            })



            $.each(a, function(i, v) {
                html += '<tr class="no_selected">';
                html += '<td> 已预约</td>';
                html += '<td><input type="button" value="查看" class="layui-btn   layui-btn-mini show_detail_selected"></td>';
                html += '<td class="strBillNo">' + v[0]["strBillNo"] + '</td>';
                html += '<td>' + v[0]["datCreateTime"].replace("T", " ") + '</td>';
                html += '<td>' + v[0]["cCusName"] + '</td>';
                html += '<td class="layui-hide lngopOrderId">' + v[0]["lngopOrderId"] + '</td>';
                html += '</tr>';

            })

            //拼接可预约订单列表html
            $.each(t, function(i, v) {
                if (!o[v["lngopOrderId"]]) {
                    o[v["lngopOrderId"]] = [];
                }
                o[v["lngopOrderId"]].push(v)
            })
            canSelect_orders = o;


            $.each(o, function(i, v) {
                html += "<tr>";
                html += '<td> <input type="checkbox" lay-filter="order"   lay-skin="primary"></td>';
                html += '<td><input type="button" value="查看" class="layui-btn   layui-btn-mini show_detail"></td>';
                html += '<td class="strBillNo">' + v[0]["strBillNo"] + '</td>';
                html += '<td>' + v[0]["datCreateTime"].replace("T", " ") + '</td>';
                html += '<td>' + v[0]["ccusname"] + '</td>';
                html += '<td class="layui-hide lngopOrderId">' + v[0]["lngopOrderId"] + '</td>';
                html += "</tr>";
            })
            html += '</tbody></table></div></form>';

            layer.open({
                area: ['800px', '500px'],
                type: 1,
                title: "可预约订单列表",
                shadeClose: true,
                content: html,
                btn: ["确认", "取消"],
                success: function(layero, index) {

                    select_order_weight = (select_order_weight / 1000000).toFixed(4);
                    layero.find("#select_order_num").text(select_order_num)
                    layero.find("#select_order_weight").attr("weight", select_order_weight);
                    layero.find("#select_order_weight").text(Math.floor(select_order_weight) + " ~ " + Math.ceil(select_order_weight) + " 吨");
                },
                btn1: function(index, layero) {
                    layer.confirm("确认要追加勾选的订单？", function() {
                        var ck = layero.find('input[type=checkbox]:checked');
                        var arr = [];
                        $.each(ck, function(i, v) {
                            arr.push($(v).parents("tr").find(".lngopOrderId").text());
                        })

                        if (arr.length == 0) {
                            layer.alert("你还未选择需要追加的订单!", {
                                icon: 2
                            });
                            return false;
                        }


                        $.ajax({
                            type: "Post",
                            async: false,
                            url: "/Handler/ProductHandler.ashx",
                            dataType: "Json",
                            traditional: true,
                            data: {
                                "Action": "AddToMAAOrder",
                                "MAAOrderId": MAAOrderId,
                                "arr_orderId": arr
                            },
                            success: function(res) {
                                if (res.flag == '0') {
                                    layer.alert(res.message, {
                                        icon: 2
                                    })
                                    return false;
                                } else {
                                    layer.alert(res.message, {
                                        icon: 1,
                                        closeBtn: false
                                    }, function() {
                                        window.location.reload();
                                    })
                                }
                            }
                        })
                    })
                }
            })
            form.render();

        }
    })



})


//查看已预约订单明细
$(document).on("click", ".show_detail_selected", function() {
    var lngopOrderId = $(this).parents("tr").find(".lngopOrderId").text();
    var order_arr = canSelect_orders[lngopOrderId];
    var title = $(this).parents("tr").find(".strBillNo").text() + "  详情"
    var html = '<div style="width:95%;margin:0 auto"><table class="layui-table" style="max-height:500px;"><thead><th>序号</th><th>产品名称</th><th>产品规格</th><th>基本数量</th><th>包装结果</th></thead><tbody>';
    var n = 1;
    $.each(MAALists, function(i, v) {
        if (lngopOrderId == v["lngopOrderId"]) {
            html += '<tr>';
            html += '<td>' + n + '</td>'
            html += '<td>' + v["cinvname"] + '</td>'
            html += '<td>' + v["cInvStd"] + '</td>'
            html += '<td>' + v["iquantity"] + '</td>'
            html += '<td>' + v["cdefine22"] + '</td>'
            html += '</tr>';
            n++;
        }


    })
    html += '</tbody></table></div>';
    layer.open({
        area: ['670px', '400px'],
        shadeClose: true,
        type: 1,
        closeBtn: false,
        title: title,
        content: html
    })
})


//查看未预约订单明细
$(document).on("click", ".show_detail", function() {
    var lngopOrderId = $(this).parents("tr").find(".lngopOrderId").text();
    var order_arr = canSelect_orders[lngopOrderId];
    var title = $(this).parents("tr").find(".strBillNo").text() + "  详情";
    var html = '<div style="width:95%;margin:0 auto"><table class="layui-table" style="max-height:500px;"><thead><th>序号</th><th>产品名称</th><th>产品规格</th><th>基本数量</th><th>包装结果</th></thead><tbody>';
    $.each(order_arr, function(i, v) {
        html += '<tr>';
        html += '<td>' + (i + 1) + '</td>'
        html += '<td>' + v["cinvname"] + '</td>'
        html += '<td>' + v["cInvStd"] + '</td>'
        html += '<td>' + v["iquantity"] + '</td>'
        html += '<td>' + v["cdefine22"] + '</td>'
        html += '</tr>';
    })
    html += '</tbody></table></div>';
    layer.open({
        area: ['670px', '400px'],
        shadeClose: true,
        type: 1,
        closeBtn: false,
        title: title,
        content: html
    })
})


form.on('checkbox(order)', function(data) {
    $this = $(this)
    var weight = 0;
    var arr = [];
    var a = $this.parents(".layui-layer-content").find(".no_selected");
    var b = $this.parents(".layui-layer-content").find("input[type=checkbox]:checked");
    $.each(a, function(i, v) {
        arr.push($(v).find(".lngopOrderId").text())
    })
    $.each(b.parents("tr"), function(i, v) {
        arr.push($(v).find(".lngopOrderId").text())
    })

    $.each(arr, function(i, v) {
        if (canSelect_orders[v]) {
            $.each(canSelect_orders[v], function(m, n) {
                weight += n.iInvWeight * n.iquantity;
            })
        }
    })
    weight=Number((weight/1000000).toFixed(2))+Number($this.parents(".layui-layer-content").find("#select_order_weight").attr("weight"));
   console.log(weight)
   $this.parents(".layui-layer-content").find("#select_order_num").text(a.length+b.length)
  // $this.parents(".layui-layer-content").find("#select_order_weight").attr("weight",weight)
    $this.parents(".layui-layer-content").find("#select_order_weight").text(Math.floor(weight) + " ~ " + Math.ceil(weight) + " 吨");
     
})

//判断对像是否为空
function isEmptyObject(e) {
    var t;
    for (t in e)
        return !1;
    return !0
}


$(document).ajaxStart(function() {
    layer.load();
}).ajaxComplete(function(request, status) {
    layer.closeAll('loading');
}).ajaxError(function(err) {
    console.log(err)
    layer.alert("加载页面出错，请联系管理员！", {
        icon: 2
    });
    layer.closeAll('loading');
});