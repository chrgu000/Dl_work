var MAA = [];
layui.use(['layer'], function() {
  var columns = [

  {
      title: "操作",

      formatter: function(value, row, index, field) {
        if (row.buttonStatus!=1) {
        return '<div style="text-align:center"><button type="button" disabled class="  btn btn-danger  btn-sm b_audit" >不能审核</button></div>';

        }else {
        return '<div style="text-align:center"><button type="button" class="  btn btn-info  btn-sm b_audit">审核并生成发货单</button></div>';

        }

      }
    }, {
      field: 'cMAACode',
      title: '预约号',
      width:'90'
    }, {
      field: 'iType',
      title: '预约类型',
      formatter: function(value, row, index, field) {
        if (value == 1) {
          return "普通预约"
        } else {
          return '<div style="color:red">特殊预约</div>';
        }
      }
    },
    // {
    //   field: 'bytStatus',
    //   title: '预约状态',
    //   formatter:function(value, row, index, field) {
    //     if (index%2==0) {
    //       return '<div style="color:red">不能审核</div>'
    //     }else{
    //       return '<div style="color:green">正常</div>'
    //     }
    //   }
    // }, 
 
     {
      field: 'cCarNumber',
      title: '预约车牌',
      width:'90'
    }, {
      field: 'cCarType',
      title: '预约车型'
    }, {
      field: 'cDriver',
      title: '司机姓名'

    }, {
      field: 'cDriverPhone',
      title: '司机电话',
      width:'100'

    } , {
      field: 'cIdentity',
      title: '司机身份证',
      width:'150'

    }, {
      field: 'cMemo',
      title: '备注',
      width:'150'

    }, {
      field: null,
      title: '预约装车时间',
      formatter: function(value, row, index, field) {
        return  row.datDate.split("T")[0]+" "+ row.datStartTime+"~"+row.datEndTime 
      }
    } 

  ]

  // 初始化Table
  var oTable = new TableInit_detailView(columns, 'tb');
  oTable.Init();
  // })

  get_maalist();



})

function get_maalist() {
  $.ajax({
    type: "Post",
    url: "/Handler/AdminHandler.ashx",
    dataType: "Json",
    async: false,
    data: {
      "Action": "Get_ToU8AuditList"
    },
    success: function(data) {
      if (data.flag !=1) {
        layer.alert(data.message, {
          icon: 2
        });
        return false;
      } else {
        MAA = data.maalist;
        console.log(data.maalist)
        if (data.maalist.length > 0) {
          var MAAList = [],
            MAACode_arr = [];
          $.each(data.maalist, function(i, v) {
            if (MAACode_arr.indexOf(v["cMAACode"]) == -1) {
              MAACode_arr.push(v["cMAACode"])

            }
          })
          console.log(MAACode_arr)
          $.each(MAACode_arr, function(i, v) {
            $.each(data.maalist, function(m, n) {

              if (n.cMAACode == v) {
                MAAList.push(n);
                return false;
              }
            })
          })
          console.log(MAAList)

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


$(document).on("click", ".b_modify", function() {
  $("#tb").find("tr").removeClass("select")
  $(this).parents("tr").addClass("select");
  var index = $("#tb").find("tr.select").data("index");
  var list = $("#tb").bootstrapTable('getData')[index];
  console.log(list)
  if (list.iType == 1) {

    $("#myModal .modal-title").html('<span> 普通预约 ' + list.cName + '</span>')
  } else {

    $("#myModal .modal-title").html('<span style="color:red"> 特殊预约 ' + list.cName + '</span>')
  }
  $("#cCode").val(list.cCode);
  $("#iType").val(list.iType);
  $("#iQty").val(list.iQty);
  $("#dAheadTime").val(list.dAheadTime);
  $("#datStartTime").val(list.datStartTime);
  $("#datEndTime").val(list.datEndTime);
  $("#datValidStartTime").val(return_datetime(list.datValidStartTime));
  $("#datValidEndTime").val(return_datetime(list.datValidEndTime));
  $('#myModal').modal();
})

$(document).on("click", "#sub", function() {
  var d = {};
  d.cCode = $("#cCode").val();
  d.iType = $("#iType").val();
  d.iQty = $("#iQty").val();
  d.dAheadTime = $("#dAheadTime").val();
  d.datStartTime = $("#datStartTime").val();
  d.datEndTime = $("#datEndTime").val();
  d.datValidStartTime = $("#datValidStartTime").val();
  d.datValidEndTime = $("#datValidEndTime").val();
  console.log(d)
  $.ajax({
    type: "Post",
    url: "/Handler/AdminHandler.ashx",
    dataType: "Json",
    async: false,
    data: {
      "Action": "Save_MAASetting",
      "data": JSON.stringify(d)
    },
    success: function(res) {
      if (res.flag == 1) {
        layer.alert("更新成功！", {
          icon: 1,
          closeBtn: 0
        }, function() {
          window.location.reload();
        })
      } else {
        layer.alert("更新失败，请重试或联系管理员！", {
          icon: 2
        })
      }
    }
  })
})

$(document).on("click",".b_audit",function(){
  alert("sfasdf")
})



//Bootstrap-Table 初始化参数（带详情列表）
var TableInit_detailView = function(columns, tableId) {
  console.log('TableInit_detailView')
  var oTableInit_detailView = new Object();

  var d = []
    //初始化Table
  oTableInit_detailView.Init = function() {
    $('#' + tableId).bootstrapTable({
      data: d, //请求后台的URL（*）
      toolbar: '#toolbar', //工具按钮用哪个容器
      striped: true, //是否显示行间隔色
      cache: false, //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
      pagination: true, //是否显示分页（*）
      sortable: false, //是否启用排序
      sortOrder: "asc", //排序方式
      queryParams: oTableInit_detailView.queryParams, //传递参数（*）
      sidePagination: "client", //分页方式：client客户端分页，server服务端分页（*）
      pageNumber: 1, //初始化加载第一页，默认第一页
      pageSize: 50, //每页的记录行数（*）
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
      detailView: true, //详细页面模式
      height: 600,
      search: true,
      columns: columns,
      detailFormatter: function(index, row) {
        console.log(row)
        var MAAOrderID = row.MAAOrderID;
        
      var  html = "";
        html+='<div style="width:50%">'
        html+='<table class="layui-table"><thead><th>订单号</th><th>网单审核状态</th><th>U8审核状态</th></thead><tbody>'
        $.each(MAA, function(i, v) {
          if (row.MAAOrderID == v.MAAOrderID) {
        html+='<tr><td>'+v.strBillNo+'</td>' 
          if (v.opStatus!='已审核') {
            html+='<td style="color:red">'+v.opStatus+'</td>';
          }
          else {
            html+=' <td>'+v.opStatus+'</td>';
          }
          if (v.U8Status!='U8订单已审核') {
             html+='<td style="color:red">'+v.U8Status+'</td>';
          }
           else {
            html+=' <td>'+v.U8Status+'</td>';
          }
          html+='</tr>';
          }
        })
        html+='</tbody></table></div>'
        return html;
      }

    });
  };


  return oTableInit_detailView;
};