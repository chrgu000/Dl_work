var MAALists = [];
var orders = {};
var element = layui.element;
var form = layui.form;

$(document).ready(function () {
    var a = ' <div class="btn-group">\
  <button type="button" class="btn btn-success btn-sm dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">\
   <span class="glyphicon glyphicon-eye-open"></span> 看信息 <span class="caret"></span>\
  </button>\
  <ul class="dropdown-menu">\
    <li><a href="#"  onclick=test(this)>Action</a></li>\
    <li><a href="#">Another action</a></li>\
    <li><a href="#">Something else here</a></li>\
    <li role="separator" class="divider"></li>\
    <li><a href="#" class="bb">Separated link</a></li>\
  </ul>\
</div>';

    var columns = [

    // {
    //     title: "操作",

    //     formatter: function(value, row, index, field) {
    //       return '<div style="text-align:center">'+a+a+'<button type="button" class="btn btn-success  btn-sm b_info"> <span class="glyphicon glyphicon-eye-open"></span> 看信息</button><button type="button" class="  btn btn-info  btn-sm b_add"><span class="glyphicon glyphicon-ok"></span> 改订单</button><button type="button" class="  btn btn-danger  btn-sm b_modify"><span class="glyphicon glyphicon-th"></span>   改信息 </button></div>';

    //     },
    //     align:"center",
    //     width:250
    //   },
       {
           field: 'cMAACode',
           title: '预约号'
       },

       {
           field: 'cCusCode',
           title: '客户编码'
       },
      {
          field: 'cCarNumber',
          title: '预约车牌'
      }, {
          field: 'cCarType',
          title: '预约车型'
      }, {
          field: 'cDriver',
          title: '司机姓名'

      }, {
          field: 'cDriverPhone',
          title: '司机电话'

      }, {
          field: 'cIdentity',
          title: '司机身份证'

      }, {
          field: 'cMemo',
          title: '申请理由',
          width: '300'

      }, {
          field: null,
          title: '预约装车时间',
          formatter: function (value, row, index, field) {
              return row.datDate.split("T")[0] + " " + row.datStartTime + "~" + row.datEndTime
          }
      }

    ]

    // 初始化Table
    var oTable = new TableInit(columns, 'tb');
    oTable.Init();
    // })

    Get_MAAList();

    /**
     * @查看该条预约所包含的订单
     */
    $(document).on("click", ".b_info", function () {
        if ($("#tb .tr_selected").length == 0) {
            layer.msg("请先选择一行记录！", { icon: 2 });
            return false;
        }
        var index = $("#tb .tr_selected").index();
        var list = $("#tb").bootstrapTable('getData')[index];
        orders = {};

        var MAAOrderID = list.MAAOrderID;
        $.each(MAALists, function (i, v) {
            if (v["MAAOrderID"] == MAAOrderID) {
                if (!orders[v["strBillNo"]]) {
                    orders[v["strBillNo"]] = [];
                }
                orders[v["strBillNo"]].push(v);
            }
        })

        console.log(orders)
        var html = '';
        html += ' <div class="layui-collapse" lay-accordion="" lay-filter="test">';
        for (var o in orders) {
            html += '<div class="layui-colla-item"><h2 class="layui-colla-title" strBillNo="' + o + '">' + o + '</h2>';
            html += '<div class="layui-colla-content">' + o;
            html += '</div></div>';
        }
        html += '</div>'

        layer.open({
            type: 0,
            shadeClose: true,
            area: ['800px', '600px'],
            content: html,
            title: list.cMAACode + "预约详情",
            success: function () {
                element.init();
            }
        })
    })


    /*
    //点击折叠面板，加载订单详情
     */
    element.on('collapse(test)', function (data) {
        var strbillno = $(this).attr("strbillno");
        var html = "";
        var html = '<div style="width:95%;margin:0 auto"><table class="layui-table" style="max-height:500px;"><thead><th>序号</th><th>产品名称</th><th>产品规格</th><th>基本数量</th><th>包装结果</th></thead><tbody>';
        $.each(orders[strbillno], function (i, v) {
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


})


function Get_MAAList() {
    $.ajax({
        type: "Post",
        url: "/Handler/AdminHandler.ashx",
        dataType: "Json",
        async: false,
        data: {
            "Action": "Get_MAAList",
            "iType": 1
        },
        success: function (data) {
            if (data.flag != 1) {
                layer.alert(data.message, {
                    icon: 2
                });
                return false;
            } else {
                MAALists = data.MAAList;
                if (MAALists.length > 0) {
                    var MAAList = [], MAACode_arr = [];
                    $.each(MAALists, function (i, v) {
                        if (MAACode_arr.indexOf(v["cMAACode"]) == -1) {
                            MAACode_arr.push(v["cMAACode"])

                        }
                    })

                    $.each(MAACode_arr, function (i, v) {
                        $.each(MAALists, function (m, n) {

                            if (n.cMAACode == v) {
                                MAAList.push(n);
                                return false;
                            }
                        })
                    })


                    $("#tb").bootstrapTable("load", MAAList);
                    var t = $("#tb").bootstrapTable()


                } else {
                    $("#tb").bootstrapTable("removeAll");
                }

            }
        },
        error: function (err) {
            layer.alert("获取数据失败,请重试或联系管理员!", {
                icon: 2
            });
            console.log(err);
        }
    });
}



$(document).on("click", ".b_audit", function () {
    $("#tb").find("tr").removeClass("select")
    $(this).parents("tr").addClass("select");
    var index = $("#tb").find("tr.select").data("index");
    var list = $("#tb").bootstrapTable('getData')[index];
    layer.open({
        title: "审核" + list.cMAACode,
        type: 1,
        area: ["500px", "250px"],
        content: '<div style="width:90%;margin:0 auto"><br><form class="layui-form layui-form-pane"   >   <div class="layui-form-item" pane><label class="layui-form-label">审核</label>\
                <div class="layui-input-block"  >\
                <input type="radio" name="opdata" value="4" title="通过">\
                <input type="radio" name="opdata" value="99" title="不通过">\
                </div></div><div class="layui-form-item">\
                <label class="layui-form-label">意见</label>\
                <div class="layui-input-block">\
                <input type="text"  id="cMemo"   placeholder="请输入审核意见"   class="layui-input">\
                </div></div> </form></div>',
        success: function () {
            form.render();
        },
        btn: ['确认', '关闭'],
        btn1: function () {
            if ($("input[name=opdata]:checked").length == 0) {
                layer.alert("你还未选择是否同意通过审核！", {
                    icon: 2
                })
                return false;
            }
            if ($("#cMemo").val().trim() == "") {
                layer.alert("审核意见为必填项！", {
                    icon: 2
                })
                return false;
            }


            $.ajax({
                type: "Post",
                url: "/Handler/AdminHandler.ashx",
                dataType: "Json",
                async: false,
                data: {
                    "Action": "AuditSpecialMAA",
                    "MAAID": list.MAAOrderID,
                    "id": list.Id,
                    "opdata": $("input[name=opdata]:checked").val(),
                    "cMemo": $("#cMemo").val().trim()
                },
                success: function (res) {
                    if (res.flag == 1) {
                        layer.alert(res.message, {
                            icon: 1,
                            closeBtn: 0
                        }, function () {
                            layer.closeAll();
                            get_maaSpeciallist();

                        })
                    } else {
                        layer.alert(res.message, {
                            icon: 2
                        })
                    }
                }
            });

        }
    })

})


$(document).on("click", ".b_modify", function () {
    $.ajax({
        type: "get",
        url: "AdminMAA.html",
        dataType: "html",
        async: false,
        success: function (res) {
            // console.log(res)
            layer.open({
                type: 1,
                area: ['1280px', '960px'],
                maxmin: true,
                content: res,
                shadeClose: true,
                success: function (layero, index) {
                    //   layer.full();


                    console.log(layero)

                }
            })
        }
    })
})



//Bootstrap-Table 初始化参数
var TableInit = function (columns, tableId) {
    console.log('TableInit')
    var oTableInit = new Object();
    var d = []
    //初始化Table
    oTableInit.Init = function () {
        $('#' + tableId).bootstrapTable({
            data: d, //请求后台的URL（*）
            //  method: 'get',                      //请求方式（*）
            toolbar: '#toolbar', //工具按钮用哪个容器
            striped: false, //是否显示行间隔色
            cache: false, //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            pagination: true, //是否显示分页（*）
            sortable: false, //是否启用排序
            sortOrder: "asc", //排序方式
            queryParams: oTableInit.queryParams, //传递参数（*）
            sidePagination: "client", //分页方式：client客户端分页，server服务端分页（*）
            pageNumber: 1, //初始化加载第一页，默认第一页
            pageSize: 10, //每页的记录行数（*）
            pageList: [50, 100, 200, 'All'], //可供选择的每页的行数（*）
            showExport: true, //是否显示导出
            exportDataType: "all", //basic', 'all', 'selected'.
            exportTypes: ["excel"],
            //  checkboxHeader: true,
            showColumns: true, //是否显示所有的列
            minimumCountColumns: 2, //最少允许的列数
            undefinedText: "", //当数据为 undefined 时显示的字符
            clickToSelect: true, //点击选中行
            // showRefresh:true,                    //显示刷新按钮
            //cardView:true,
            // showToggle: true,                          //切换卡模式
            // detailView: true,                       //详细页面模式
            height: 600,
            search: true,
            columns: columns


        });
    };


    return oTableInit;
}


//选中行是添加样式
$(document).on("click", "#tb tbody tr", function () {

    $(this).siblings().removeClass('tr_selected')
    $(this).addClass('tr_selected');

})

//加载修改订单
$(document).on("click", ".b_modify_order", function () {
       if ($("#tb .tr_selected").length == 0) {
            layer.msg("请先选择一行记录！", { icon: 2 });
            return false;
        }
        var index = $("#tb .tr_selected").index();
        var list = $("#tb").bootstrapTable('getData')[index];
        var o;
      $.each(MAALists,function(i,v){
        if (v["MAAOrderID"]==list.MAAOrderID) {
          o=v;
          return false;
        }
      });
      console.log(o)
     if (typeof o!='object') {
       layer.msg("获取记录出错！", { icon: 2 });
      return false;
     }
 
       var index = layer.open({
                        title:list.cMAACode,
                        type: 1,
                        content: $('#modify_order_div'),
                        area: ['1350px', '600px'],
                        btn: ["确定", "关闭"],
                        shadeClose:true,
                        maxmin: true,
                        success: function (layero,index) {
                            form.render();
                           layero.find("input[name=cCarNumber]").val(o.cCarNumber)
                           layero.find("input[name=cDriver]").val(o.cDriver)
                           layero.find("input[name=cDriverPhone]").val(o.cDriverPhone)
                           layero.find("input[name=cIdentity]").val(o.cIdentity)
                           layero.find("input[name=cMemo]").val(o.cMemo)
                            
                        }
                    });

    //layer.full(index)
})