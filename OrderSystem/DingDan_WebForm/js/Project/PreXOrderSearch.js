var detailorder = [];
var html = "";
 
layui.use(['element', 'layer', 'util', 'laydate', 'form'], function () {
    var oTable = new TableInit();
 
    oTable.Init();
  //  get_list();
            //  var $ = layui.jquery,
       var form = layui.form(),
           layer=layui.layer,
           //util = layui.util,
         laydate = layui.laydate;
              //element = layui.element(); //Tab的切换功能，切换事件监听等，需要依赖element模块

            var start = {
                min: '2016-01-01 00:00:00'
 , max: '2099-06-16 23:59:59'
 , istoday: false
 , choose: function (datas) {
     end.min = datas; //开始日选好后，重置结束日的最小日期
     end.start = datas //将结束日的初始值设定为开始日
 }
            };

            var end = {
                min: laydate.now()
              , max: '2099-06-16 23:59:59'
              , istoday: false
              , choose: function (datas) {
                  start.max = datas; //结束日选好后，重置开始日的最大日期
              }
            };

            $("#start_date").click(function () {
                start.elem = this;
                laydate(start);
            });

            $("#end_date").click(function () {
                end.elem = this;
                laydate(end);
            });

 
        
            $("#btn").click(function () {
                get_list();
            })



})

function get_list() {
    $.ajax({
        url: "../Handler/ProductHandler.ashx",
        type: "Post",
        dataType: "Json",
        data: {
            "Action": "DLproc_MyWorkPreYOrderForCustomerBySel",
            "start_date": $("#start_date").val(),
            "end_date": $("#end_date").val(),
            "strbillno": $("#strbillno").val(),
            "orderstatus": $("#orderStatus  option:selected").val()
        },
        success: function (data) {
            if (data.flag == 0) {
                layer.alert(data.message, { icon: 2 });
            } else {
                $("#PreXOrder_list").bootstrapTable('load', data.dt);

            }
        }
    })


}

function get_detail(strBillNo) {
    $.ajax({
        url: "../Handler/ProductHandler.ashx",
        dataType: "Json",
        type: "Post",
        async:false,
        data: { "Action": "DL_XOrderBillDetailBySel", "strBillNo": strBillNo },
        success: function (data) {
            detailorder = data.dt;
            html = "";
            var money = 0;
            html += '<div style="width:80%;text-align:center">'
            html += '<table class="layui-table" style="margin:0 auto"><thead><th>名称</th><th>规格</th><th>单位组</th><th>基本数量汇总</th><th>包装结果</th><th>单价</th><th>金额</th></thead><tbody>'
            $.each(data.dt, function (i, v) {
                html += '<tr><td>' + v.cinvname + '</td>';
                html += '<td>' + v.cInvStd + '</td>';
                html += '<td>' + v.UnitGroup + '</td>';
                html += '<td>' + v.iquantity + '</td>';
                html += '<td>' + v.cDefine22 + '</td>';
                html += '<td>' + v.itaxunitprice + '</td>';
                html += '<td>' + v.xx + '</td></tr>';
                money += v.xx;
            })
            html += '<tr><td style="font-weight:bold;font-size:18px" colspan=6>合计</td> <td style="color:red;font-weight:bold;font-size:18px">' + money + '</td></tr>';
            html += '</tbody></table></div>'
            
          
            $.each(data.dt, function (i, v) {
              
            })
            $("#money").text(money.toFixed(2));
          
        }
    })
}

        //function draw_tb(data) {
        //    $("#PreXOrder_list").bootstrapTable({
        //        scrollY: "300px",
        //        info: false,
        //        autoWidth: false,
        //        paging: false,
        //        searching: false,
        //        destroy: true,
        //        data: data.dt,
        //        "language": {
        //            "lengthMenu": "每页 _MENU_ 条记录",
        //            "zeroRecords": "没有找到记录",
        //            "info": "第 _PAGE_ 页 ( 总共 _PAGES_ 页 )",
        //            "infoEmpty": "无记录",
        //            "infoFiltered": "(从 _MAX_ 条记录过滤)"
        //        },
        //        "columns": [
        //      {"title": "操作", "data": null,class:"center",orderable:false,
        //        formatter:function(){
        //          return '<input type="button" class="layui-btn layui-btn-small show_detail" value="查看订单"/>';
        //        }
        //      },
        //      {"title": "网单号", "data": "strBillNo",class:"center"},
        //      {"title": "正式订单号", "data": "cSOCode",class:"center"},
        //      {"title": "订单状态", "data": "bytStatus",class:"center"},
        //      {"title": "提交日期", "data": "xdsj",class:"center"},
        //      {"title": "开票单位", "data": "ccusname",class:"center"}
        //        ]
        //    });
        //}

        //  function draw_detail(data) {
        //      $("#PreXOrder_detail").bootstrapTable({
        //        scrollY: "200px",
        //        info: false,
        //        autoWidth: false,
        //        paging: false,
        //        searching: false,
        //        destroy: true,
        //        data: data.dt,
        //        "language": {
        //            "lengthMenu": "每页 _MENU_ 条记录",
        //            "zeroRecords": "没有找到记录",
        //            "info": "第 _PAGE_ 页 ( 总共 _PAGES_ 页 )",
        //            "infoEmpty": "无记录",
        //            "infoFiltered": "(从 _MAX_ 条记录过滤)"
        //        },
        //        "columns": [
        //      {"title": "名称", "data": "cinvname",class:"center"},
        //      {"title": "规格", "data": "cInvStd",class:"center"},
        //      {"title": "单位组", "data": "UnitGroup",class:"center"},
        //      {"title": "基本数量汇总", "data": "iquantity",class:"center"},
        //      {"title": "包装结果", "data": "cDefine22",class:"center"},
        //      {"title": "单价", "data": "iquotedprice",class:"center"},
        //      {"title": "金额", "data": "cComUnitAmount",class:"center"},
        //      {"title": "执行单价", "data": "itaxunitprice",class:"center",visible:false},
        //      {"title": "执行金额", "data": "xx",class:"center xx",visible:false},
        //        ]
        //    });
        //}


     
        //$(document).on("click",".show_detail",function(){
        //  var strBillNo=$(this).parents("tr").find("td:eq(1)").text();
        //  $.ajax({
        //    url:"../Handler/ProductHandler.ashx",
        //    dataType:"Json",
        //    type:"Post",
        //    data:{"Action":"DL_XOrderBillDetailBySel","strBillNo":strBillNo},
        //    success:function(data){

        //      draw_detail(data);
        //      $("#info").html('<strong>订单号 :</strong>'+strBillNo+'       <strong>下单时间 :</strong>'+return_datetime(data.dt[0].datBillTime)+'         <strong>开票单位 :</strong>'+data.dt[0].ccusname);
        //      var money=0;
        //      var table=$("#PreXOrder_detail").DataTable();
        //      $.each(table.columns(".xx").data()[0],function(i,v){
        //        money+=v;
        //      })
        //      $("#money").text(money.toFixed(2));
        //    },
        //    error:function(err){
        //      layer.alert("出现错误，请重试或联系管理员!",{icon:2});
        //    }
        //  })
        //})
    
        //$("#btn1").click(function () {
        //  $('#detail').tableExport({type:'excel',escape:'false'});

        //})
 


        var TableInit = function () {
            var oTableInit = new Object();
            var d = []
            //初始化Table
            oTableInit.Init = function () {
                $('#PreXOrder_list').bootstrapTable({
                    data: d,         //请求后台的URL（*）
                    //  method: 'get',                      //请求方式（*）
                    toolbar: '#toolbar',                //工具按钮用哪个容器
                    striped: true,                      //是否显示行间隔色
                    cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
                    pagination: true,                   //是否显示分页（*）
                    sortable: false,                     //是否启用排序
                    sortOrder: "asc",                   //排序方式
                    queryParams: oTableInit.queryParams,//传递参数（*）
                    sidePagination: "client",           //分页方式：client客户端分页，server服务端分页（*）
                    pageNumber: 1,                       //初始化加载第一页，默认第一页
                    pageSize: 10,                       //每页的记录行数（*）
                    pageList: [50, 100, 200, 'All'],        //可供选择的每页的行数（*）
                    clickToSelect: true,
                    showExport: true,                     //是否显示导出
                    exportDataType: "all",              //basic', 'all', 'selected'.
                    exportTypes: ["excel"],
                    showColumns: true,                  //是否显示所有的列
                    minimumCountColumns: 2,             //最少允许的列数
                    undefinedText: "",                   //当数据为 undefined 时显示的字符
                    clickToSelect: true,                 //点击选中行
                    // showRefresh:true,                    //显示刷新按钮
                    //cardView:true,
                    // showToggle: true,                          //切换卡模式
                     detailView: true,                       //详细页面模式
                  //  height: 400,
                    search: true,
                    columns: [
                     //{
                     //    title: "操作",
                     //    formatter: function () {
                     //            return '<input type="button" class="layui-btn layui-btn-small show_detail" value="查看订单"/>';
                     //    }
                     //},
              { "title": "网单号", "field": "strBillNo", "align": "center" },
              { "title": "正式订单号", "field": "cSOCode", "align": "center" },
              { "title": "订单状态", "field": "bytStatus", "align": "center" },
              { "title": "提交日期", "field": "xdsj", "align": "center" },
              { "title": "开票单位", "field": "ccusname", "align": "center" }
                    ],
                    detailFormatter: function (index, row) {
                        get_detail(row.strBillNo);
                        return html;
                
                    }
                });
            };

  

            oTableInit.response = function (res) {
                return res.rows;

            }
            return oTableInit;
        };