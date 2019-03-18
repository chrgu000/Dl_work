layui.use(['layer'], function() {
  var columns = [{
      title: "操作",

      formatter: function(value, row, index, field) {
        return '<div style="text-align:center"><button type="button" class="  btn btn-info  btn-sm b_modify">编辑</button></div>';

      }
    }, {
      field: 'cName',
      title: '时间段名称'
    }, {
      field: 'iType',
      title: '时间段类型',
      formatter: function(value, row, index, field) {
        if (value == 1) {
          return "普通预约"
        } else {
          return '<div style="color:red">特殊预约</div>';
        }
      }
    }, {
      field: 'iQty',
      title: '可预约量'

    }, {
      field: 'dAheadTime',
      title: '提前预约时间(分钟)'

    }, {
      field: 'datStartTime',
      title: '时段开始时间'

    }, {
      field: 'datEndTime',
      title: '时段结束时间'

    }, {
      field: 'datValidStartTime',
      title: '时段生效日期',
      formatter: function(value, row, index, field) {
        return return_datetime(value)
      }
    }, {
      field: 'datValidEndTime',
      title: '时段结束日期',
      formatter: function(value, row, index, field) {
        return return_datetime(value)
      }
    }, {
      field: 'cMaker',
      title: '修改人'
    }, {
      field: 'datModifyTime',
      title: '修改时间',
      formatter: function(value, row, index, field) {
        return return_datetime(value)
      }
    }

  ]

  // 初始化Table
  var oTable = new TableInit(columns, 'tb');
  oTable.Init();
  // })

  get_setting();



})

function get_setting() {
  $.ajax({
    type: "Post",
    url: "/Handler/AdminHandler.ashx",
    dataType: "Json",
    async: false,
    data: {
      "Action": "Get_MAASetiing"
    },
    success: function(data) {
      if (data.flag != 1) {
        layer.alert(data.message, {
          icon: 2
        });
        return false;
      } else {
        console.log(data.setting_table)
        if (data.setting_table.length > 0) {
          $("#tb").bootstrapTable("load", data.setting_table);
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
  if (list.iType==1) {
   
    $("#myModal .modal-title").html('<span> 普通预约 '+list.cName+'</span>')
  }else{
   
    $("#myModal .modal-title").html('<span style="color:red"> 特殊预约 '+list.cName+'</span>')
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
  var d={};
  d.cCode=$("#cCode").val();
  d.iType=$("#iType").val();
  d.iQty=$("#iQty").val();
  d.dAheadTime=$("#dAheadTime").val();
  d.datStartTime=$("#datStartTime").val();
  d.datEndTime=$("#datEndTime").val();
  d.datValidStartTime=$("#datValidStartTime").val();
  d.datValidEndTime=$("#datValidEndTime").val();
  console.log(d)
    $.ajax({
    type: "Post",
    url: "/Handler/AdminHandler.ashx",
    dataType: "Json",
    async: false,
    data: {
      "Action": "Save_MAASetting","data":JSON.stringify(d)
    },
    success:function(res){
      if (res.flag!=1) {
    layer.alert(res.message,{icon:2})
    return false;
      }else{
            $('#myModal').modal('hide');
        layer.alert("更新成功！",{icon:1,closeBtn:0},function(){
          get_setting();
          layer.closeAll();
        })
      }
    }
  })
})