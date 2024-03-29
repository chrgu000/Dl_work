﻿//layui.use(['layer','laydate' ], function () {

//var layer=layui.layer;

//$.ajax({
//	type: "Post",
//	url: "/../Handler/ProductHandler.ashx",
//	dataType: "Json",
//	data: { "Action": "DL_PreviousOrderBySel"},
//	success:function(data){
//		  vm.orders=data.dt;
//	},
//	error:function(err){
//		console.log(err);
//		 layui.layer.alert("获取数据失败,请重试或联系管理员!",{icon:2});
//		console.log(err);
//	}


//});
//})
 



layui.use(['util', 'laydate', 'form'], function () {
    var form = layui.form(),
        layer = layui.layer,
      laydate = layui.laydate;
    //  element = layui.element(); //Tab的切换功能，切换事件监听等，需要依赖element模块

    var start = {
        min: '2016-01-01 00:00:00'
               , max: '2099-06-16 23:59:59'
             , format: 'YYYY-MM-DD' //日期格式
        //  , istime: true     //是否开启时间选择
               , istoday: false
               , choose: function (datas) {
                   end.min = datas; //开始日选好后，重置结束日的最小日期
                   end.start = datas //将结束日的初始值设定为开始日
               }
    };

    var end = {
        min: laydate.now()
      , max: '2099-06-16 23:59:59'
         , format: 'YYYY-MM-DD' //日期格式
        //  , istime: true     //是否开启时间选择
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
  //JQ ajax全局事件
$(document).ajaxStart(function () {
    layer.load();
}).ajaxComplete(function (request, status) {
    layer.closeAll('loading');
});

})

//查看订单列表
$("#btn").click(function () {
 
    		    $.ajax({
    		        type: "Post",
    		        url: "/../Handler/ProductHandler.ashx",
    		        dataType: "Json",
    		        data: { "Action": "DL_PreviousOrderBySel", "start_date": $("#start_date").val(), "end_date": $("#end_date").val(),"type":$("#type option:selected").val() },
    		        success: function (data) {
    		            if (data.flag==0) {
    		                layer.alert(data.message, { icon: 2 });
    		            }
    		            else if (data.flag==1) {
    		                if (data.dt.length > 0) {
    		                    $("#tb").bootstrapTable("load", data.dt);
    		                }
    		                }  
    		        },
    		        error: function (err) {
    		             
    		            layui.layer.alert("获取数据失败,请重试或联系管理员!", { icon: 2 });
    		            console.log(err);
    		        }


    		    });
})


//查看订单详情
$(document).on("click", ".btn-info", function () {
    var strBillNo = $(this).parents("tr").find("td:eq(2)").text();
    $.ajax({
        type: "Post",
        url: "/../Handler/ProductHandler.ashx",
        dataType: "Json",
        async: false,
        data: { "Action": "DL_OrderU8BillBySel", "strBillNo": strBillNo },
        success: function (data) {
            
            if (data.dt.length > 0) {
                $("#detail").bootstrapTable("load", data.dt);
                var t = $("#detail").bootstrapTable("getData");
                var money = 0;
                $.each(t, function (i, v) {
                    money += v.iSum;
                })
                $("#strBillNo").text(strBillNo)
                $("#cSCCode").text(t[0].cSCCode)
                $("#datBillTime").text(return_datetime(t[0].datBillTime))
                $("#datAuditordTime").text(return_datetime(t[0].datAuditordTime))
                $("#cdefine11").text(t[0].cdefine11)
                $("#money").text(money.toFixed(2))
            }
           

        },
        error: function (err) {
            console.log(err);
            layer.alert("获取数据失败,请重试或联系管理员!", { icon: 2 });
        }
    });
})


$(function () {

    //1.初始化Table
    var oTable = new TableInit();
    oTable.Init();
    var oTableDetail = new TableDetailInit();
    oTableDetail.Init();
});


var TableInit = function () {
    var oTableInit = new Object();
    var d = []
    //初始化Table
    oTableInit.Init = function () {
        $('#tb').bootstrapTable({
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
            pageSize: 50,                       //每页的记录行数（*）
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
            // detailView: true,                       //详细页面模式
            height: 400,
            search: true,
            columns: [
             {
                 title:"操作",
                 formatter: function (value, row, index, field) {
                     if ((row.cSOCode).trim() != "") {
                         return '<div style="text-align:center"><button type="button" class="  btn btn-info  btn-sm"  >查看详情</button></div>';
                     }
                     else {
                         return '';
                     }
                 }
             },
            {
                field: 'strBillNo',
                title: 'DL网单号'
            }, {
                field: 'cSOCode',
                title: '正式订单号'

            } ,
              
              {
                  field: 'chdefine2',
                  title: '样品订单号'
              },
              {
                  field: 'dDate',
                  title: '单据日期'
              },
              {
                  field: 'cCusName',
                  title: '开票单位名称'
              },
              {
                  field: 'billtype',
                  title: '销售类型'
              },
              {
                  field: 'XDFS',
                  title: '下单方式'
              },
              {
                  field: 'cDefine11',
                  title: '送货地址',
                  width:200
              },
              {
                  field: 'cMemo',
                  title: '备注',
                  width:200
              }
              ]
        });
    };

 

    oTableInit.response = function (res) {
        return res.rows;

    }
    return oTableInit;
};

var TableDetailInit = function () {
    var oTableInit = new Object();
    var d = []
    //初始化Table
    oTableInit.Init = function () {
        $('#detail').bootstrapTable({
            data: d,         //请求后台的URL（*）
          //  toolbar: '#toolbar',                //工具按钮用哪个容器
            striped: true,                      //是否显示行间隔色
            cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            pagination: true,                   //是否显示分页（*）
            sortable: false,                     //是否启用排序
            sortOrder: "asc",                   //排序方式
            queryParams: oTableInit.queryParams,//传递参数（*）
            sidePagination: "client",           //分页方式：client客户端分页，server服务端分页（*）
            pageNumber: 1,                       //初始化加载第一页，默认第一页
            pageSize: 50,                       //每页的记录行数（*）
            pageList: [50, 100, 200, 'All'],        //可供选择的每页的行数（*）
            clickToSelect: true,
            showExport: false,                     //是否显示导出
            exportDataType: "all",              //basic', 'all', 'selected'.
            exportTypes: ["excel"],
            showColumns: false,                  //是否显示所有的列
            minimumCountColumns: 2,             //最少允许的列数
            undefinedText: "",                   //当数据为 undefined 时显示的字符
            clickToSelect: true,                 //点击选中行
            // showRefresh:true,                    //显示刷新按钮
            //cardView:true,
            // showToggle: true,                          //切换卡模式
            // detailView: true,                       //详细页面模式
            height: 400,
            search: false,
            columns: [
             {
                 title: "序号",
                 formatter: function (value, row, index, field) {
                     return '<span>'+(Number(index)+1)+'</span>';
                 }
             },
            {
                field: 'cInvName',
                title: '名称'

            },
              {
                  field: 'cInvStd',
                  title: '规格'
              },
              {
                  field: 'iQuantity',
                  title: '数量'
              },
              {
                  field: 'cDefine22',
                  title: '包装结果'
              }, {
                  field: 'iTaxUnitPrice',
                  title: '单价'
              }, {
                  field: 'iSum',
                  title: '合计'
              }
            ]
        });
    };



    oTableInit.response = function (res) {
        return res.rows;

    }
    return oTableInit;
};



//var vm=new Vue({
//	el:"#vm",
//	data:{
//		orders:null,
//		products:null,
//		type: "0",
//		seen_orders: true,
//		csccode: ".",
//		datBillTime: " .",
//		datAuditordTime: ". ",
//	    cdefine11:" "
//	},
//	methods:{
//		show:function(event){
//			var strBillNo=$(event.target).parents("tr").find(".strBillNo").text();
//			$(event.target).parents("tbody").find("tr").removeClass("blue");
//			$(event.target).parents("tr").addClass("blue");
//				$.ajax({
//					type: "Post",
//					url: "/../Handler/ProductHandler.ashx",
//					dataType: "Json",
//					async:false,
//					data: { "Action": "DL_OrderU8BillBySel",strBillNo:strBillNo},
//					success:function(data){
//					    vm.products = data.dt;
//					    vm.csccode = data.dt[0].cSCCode;
//					    vm.datBillTime =return_datetime(data.dt[0].datBillTime);
//					    vm.datAuditordTime =return_datetime(data.dt[0].datAuditordTime);
//					    vm.cdefine11 = data.dt[0].cdefine11;
//					    $("#strBillNo_span").text(strBillNo);
					 
//					},
//					error:function(err){
//						console.log(err);
//						layer.alert("获取数据失败,请重试或联系管理员!",{icon:2});
//					}
//					});
//		},
//		money:function(){
//			if (this.products==null) {
//				return 0; 
//			}else {
//				var sum=0;
//				for (var i = 0; i < this.products.length; i++) {
//					sum+=this.products[i].iSum;
//				};
//				return sum.toFixed(2);
//			}
//		},
//		count:function(){
//			if (this.products==null) {
//				return 0;
//			}else {
//				return this.products.length;
//			}
//		},
//		seen_products:function(){
//				if (this.products==null) {
//					return true;
//				}else {
//					return false;
//				}
//		},
//		get_data: function () {
//		    $.ajax({
//		        type: "Post",
//		        url: "/../Handler/ProductHandler.ashx",
//		        dataType: "Json",
//		        data: { "Action": "DL_PreviousOrderBySel", "start_date": $("#start_date").val(), "end_date": $("#end_date").val(),"type":$("#type option:selected").val() },
//		        success: function (data) {
//		            vm.orders = data.dt;
//		            if (vm.orders.length == 0) {
//		                vm.seen_orders = true;
//		            } else {
//		                vm.seen_orders = false;
//		            }
//		        },
//		        error: function (err) {
//		            console.log(err);
//		            layui.layer.alert("获取数据失败,请重试或联系管理员!", { icon: 2 });
//		            console.log(err);
//		        }


//		    });
//		}
//	},
//	//computed:{
//	//		seen_orders:function(){
//	//			if (this.orders.length==0) {
//	//				return true;
//	//			}else {
//	//				return false;
//	//			}
//	//		}
//	//},
//	created: function () {
//	    layui.use(['layer', 'laydate','form'], function () {

//	        var layer = layui.layer;
//	        var form = layui.form();
//	        var start = {
//	            min: '2016-01-01 00:00:00'
//               , max: '2099-06-16 23:59:59'
//             , format: 'YYYY-MM-DD' //日期格式
//            //  , istime: true     //是否开启时间选择
//               , istoday: false
//               , choose: function (datas) {
//                   end.min = datas; //开始日选好后，重置结束日的最小日期
//                   end.start = datas //将结束日的初始值设定为开始日
//               }
//	        };

//	        var end = {
//	            min: laydate.now()
//              , max: '2099-06-16 23:59:59'
//                 , format: 'YYYY-MM-DD' //日期格式
//              //  , istime: true     //是否开启时间选择
//              , istoday: false
//              , choose: function (datas) {
//                  start.max = datas; //结束日选好后，重置开始日的最大日期
//              }
//	        };

//	        $("#start_date").click(function () {
//	            start.elem = this;
//	            laydate(start);
//	        });

//	        $("#end_date").click(function () {
//	            end.elem = this;
//	            laydate(end);
//	        });
	         
//	        form.render();
	         
//	    })
//	}
//})

 



