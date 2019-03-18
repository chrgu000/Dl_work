var SpecialList = [];
var orders = {};
var element = layui.element();
var form = layui.form();
$(document).ready(function() {

  var columns = [{
      title: "操作",

      formatter: function(value, row, index, field) {
        return '<div style="text-align:center"><button type="button" class="btn btn-success  btn-sm b_info">查看审核</button><button type="button" class="  btn btn-info  btn-sm b_show">预约详情</button><button type="button" class="  btn btn-danger  btn-sm b_audit">  审核预约 </button></div>';

      }
    }, {
      field: 'cMAACode',
      title: '预约号'
    },
    //{
    //  field: 'iType',
    //  title: '预约类型',
    //  formatter: function(value, row, index, field) {
    //    if (value == 1) {
    //      return "普通预约"
    //    } else {
    //      return '<div style="color:red">特殊预约</div>';
    //    }
    //  }
    //}, {
    //  field: 'bytStatus',
    //  title: '预约状态',
    //  formatter: function(value, row, index, field) {
    //    if (value == 1) {
    //      return '<div style="color:green">等待审核</div>'
    //    } else {
    //      return '<div style="color:red">异常</div>'
    //    }
    //  }
    //},
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
        width:'300'

    }, {
      field: null,
      title: '预约装车时间',
      formatter: function(value, row, index, field) {
        return row.datDate.split("T")[0] + " " + row.datStartTime + "~" + row.datEndTime
      }
    }

  ]

  // 初始化Table
  var oTable = new TableInit(columns, 'tb');
  oTable.Init();
  // })

  get_maaSpeciallist();

  /**
   * @查看该条预约所包含的订单
   */
  $(document).on("click", ".b_show", function() {
    $("#tb").find("tr").removeClass("select")
    $(this).parents("tr").addClass("select");
    var index = $("#tb").find("tr.select").data("index");
    var list = $("#tb").bootstrapTable('getData')[index];
    orders = {};
    console.log(SpecialList)
    var MAAOrderID = list.MAAOrderID;
    $.each(SpecialList, function(i, v) {
      if (v["MAAOrderID"] == MAAOrderID) {
        if (!orders[v["strBillNo"]]) {
          orders[v["strBillNo"]] = [];
        }
        orders[v["strBillNo"]].push(v);
      }
    })
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
      title: list.cMAACode+"预约详情",
      success: function() {
        element.init();
      }
    })
  })


  /*
  //点击折叠面板，加载订单详情
   */
  element.on('collapse(test)', function(data) {
    console.log(data)
    console.log($(this))
    var strbillno = $(this).attr("strbillno");
    var html = "";
    var html = '<div style="width:95%;margin:0 auto"><table class="layui-table" style="max-height:500px;"><thead><th>序号</th><th>产品名称</th><th>产品规格</th><th>基本数量</th><th>包装结果</th></thead><tbody>';
    $.each(orders[strbillno], function(i, v) {
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


function get_maaSpeciallist() {
  $.ajax({
    type: "Post",
    url: "/Handler/AdminHandler.ashx",
    dataType: "Json",
    async: false,
    data: {
      "Action": "Get_SpecialAuditList"
    },
    success: function(data) {
      if (data.flag != 1) {
        layer.alert(data.message, {
          icon: 2
        });
        return false;
      } else {
        SpecialList = data.specialList;
        if (SpecialList.length > 0) {
          var MAAList = [],
            MAACode_arr = [];
          $.each(SpecialList, function(i, v) {
            if (MAACode_arr.indexOf(v["cMAACode"]) == -1) {
              MAACode_arr.push(v["cMAACode"])

            }
          })

          $.each(MAACode_arr, function(i, v) {
            $.each(SpecialList, function(m, n) {

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
    error: function(err) {
      layer.alert("获取数据失败,请重试或联系管理员!", {
        icon: 2
      });
      console.log(err);
    }
  });
}



$(document).on("click", ".b_audit", function() {
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
      success: function() {
        form.render();
      },
      btn: ['确认', '关闭'],
      btn1: function() {
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
            "id":list.Id,
            "opdata":$("input[name=opdata]:checked").val(),
            "cMemo":$("#cMemo").val().trim()
          },
          success: function(res) {
            if (res.flag == 1) {
              layer.alert(res.message, {
                icon: 1,
                closeBtn: 0
              }, function() {
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



  $(document).on("click",".b_info",function(){
  $("#tb").find("tr").removeClass("select")
  $(this).parents("tr").addClass("select");
  var index = $("#tb").find("tr.select").data("index");
  var list = $("#tb").bootstrapTable('getData')[index];
  var MAAOrderID=list.MAAOrderID;
  
  $.ajax({
        type: "Post",
          url: "/Handler/AdminHandler.ashx",
          dataType: "Json",
          async: false,
          data: {
            "Action": "AuditSpecialMAAInfo",
            "MAAOrderID": MAAOrderID
          },
          success: function(res) {
            if (res.flag!=1) {
              layer.alert(res.message,{icon:2})
              return false;
            }
            else {
              if (res.info.length==0) {
                  layer.alert("该预约还没有人审核！",{icon:2})
              return false;
              }
              else{
                var html="";
                html+='<div style="width:90%"><br><form class="layui-form layui-form-pane">';
                $.each(res.info,function(i,v){
                   console.log(v.iwfStatus)
                  html+='  <div class="layui-form-item">\
    <label class="layui-form-label">审核人</label>\
    <div class="layui-input-inline">\
      <input disabled value="'+v.cOPName+'" class="layui-input"></div></div>'
      var iwfStatus="";
      if (v.iwfStatus==4) {
          iwfStatus='审核通过';
      }else{
        iwfStatus='审核不通过';
      }
      html+='  <div class="layui-form-item">\
    <label class="layui-form-label">审核结果</label>\
    <div class="layui-input-inline">\
      <input disabled value='+iwfStatus+' class="layui-input"></div></div>'

           html+='  <div class="layui-form-item">\
    <label class="layui-form-label">审核意见</label>\
    <div class="layui-input-inline">\
    <textarea disabled   class="layui-textarea">'+v.cMemo+'</textarea></div></div>'
                })
                html+='</form></div>';
                layer.open({
                  type:0,
                  title:list.cMAACode+"审核结果",
                  area:["500px"],
                  shadeClose:1,
                  content:html
                })

              }
            }
          }
  })
  })