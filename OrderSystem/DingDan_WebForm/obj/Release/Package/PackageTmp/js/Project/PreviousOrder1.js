//layui.use(['layer','laydate' ], function () {

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
 

 //JQ ajax全局事件
$(document).ajaxStart(function () {
    layer.load();
}).ajaxComplete(function (request, status) {
    layer.closeAll('loading');
});

var vm=new Vue({
	el:"#vm",
	data:{
		orders:null,
		products:null,
		type: "0",
		seen_orders: true,
		csccode: ".",
		datBillTime: " .",
		datAuditordTime: ". ",
	    cdefine11:" "
	},
	methods:{
		show:function(event){
			var strBillNo=$(event.target).parents("tr").find(".strBillNo").text();
			$(event.target).parents("tbody").find("tr").removeClass("blue");
			$(event.target).parents("tr").addClass("blue");
				$.ajax({
					type: "Post",
					url: "/../Handler/ProductHandler.ashx",
					dataType: "Json",
					async:false,
					data: { "Action": "DL_OrderU8BillBySel",strBillNo:strBillNo},
					success:function(data){
					    vm.products = data.dt;
					    vm.csccode = data.dt[0].cSCCode;
					    vm.datBillTime =return_datetime(data.dt[0].datBillTime);
					    vm.datAuditordTime =return_datetime(data.dt[0].datAuditordTime);
					    vm.cdefine11 = data.dt[0].cdefine11;
					    $("#strBillNo_span").text(strBillNo);
					 
					},
					error:function(err){
						console.log(err);
						layer.alert("获取数据失败,请重试或联系管理员!",{icon:2});
					}
					});
		},
		money:function(){
			if (this.products==null) {
				return 0; 
			}else {
				var sum=0;
				for (var i = 0; i < this.products.length; i++) {
					sum+=this.products[i].iSum;
				};
				return sum.toFixed(2);
			}
		},
		count:function(){
			if (this.products==null) {
				return 0;
			}else {
				return this.products.length;
			}
		},
		seen_products:function(){
				if (this.products==null) {
					return true;
				}else {
					return false;
				}
		},
		get_data: function () {
		    $.ajax({
		        type: "Post",
		        url: "/../Handler/ProductHandler.ashx",
		        dataType: "Json",
		        data: { "Action": "DL_PreviousOrderBySel", "start_date": $("#start_date").val(), "end_date": $("#end_date").val(),"type":$("#type option:selected").val() },
		        success: function (data) {
		            vm.orders = data.dt;
		            if (vm.orders.length == 0) {
		                vm.seen_orders = true;
		            } else {
		                vm.seen_orders = false;
		            }
		        },
		        error: function (err) {
		            console.log(err);
		            layui.layer.alert("获取数据失败,请重试或联系管理员!", { icon: 2 });
		            console.log(err);
		        }


		    });
		}
	},
	//computed:{
	//		seen_orders:function(){
	//			if (this.orders.length==0) {
	//				return true;
	//			}else {
	//				return false;
	//			}
	//		}
	//},
	created: function () {
	    layui.use(['layer', 'laydate','form'], function () {

	        var layer = layui.layer;
	        var form = layui.form();
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
	         
	        form.render();
	         
	    })
	}
})

$("#search").keyup(function () {
   // console.log($(this).val());
  //  var $trs = $("#Previous_Order tbody").find("tr");
   // console.log($trs)
    $this = $("#search");
    var txt = $this.val().trim().toUpperCase();//
    console.log(txt);
    $("#Previous_Order   tr:gt(0)").hide();
    var $d = $("#Previous_Order tbody tr").filter(":contains('" + txt + "')");
    $d.show();
})



