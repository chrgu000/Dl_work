 $(document).ready(function() {

     var oTable = new TableInit(columns, 'table');
     oTable.Init();
 



 })

 function GetOrderWeight() {
     if ($("#order").val().trim()=="") {
         bootbox.alert({ message: "请输入正式订单号" })
         return false;
     }
 	$.ajax({
 		type: "Post",
 		url: "/Handler/AdminHandler.ashx",
 		dataType: "Json",
 		data: {
 		    "Action": "GetOrderWeight",
 		    "order": $("#order").val().trim()
 		},
 		success: function(data) {
 			if (data.flag != "1") {
 				ErrorMsgAlert(data.message);
 				return false;
 			}
 			console.log(data.table)
 		
 			if (data.table.length > 0) {
 			    $("#table").bootstrapTable("load", data.table);
 			    $("#weight").val(sumweight());
 			} else {
 			    $("#table").bootstrapTable("removeAll");
 			    ErrorMsgAlert("没有查询到该订单，请核实订单号！");
 			    $("#weight").val("0");
 			}
 		}
 	});
 }

 $(document).on("click", "#search", function () {
     GetOrderWeight();
 })

 function sumweight() {
     var t = $("#table").bootstrapTable('getData');
     if (t.length==0) {
         return 0;
     } else {
         var w = 0;
         $.each(t, function (i,v) {
             w+=Number(v.weight)
         })
         return w.toFixed(6);
     }
 }

 var columns = [
     {
         field: null,
         title: "序号",
         formatter:  function(value, row, index, field){
             return index + 1;
            
         }
     },

 	{
 	    field: 'weight',
 	    title: '重量',
 	    formatter: function (value, row, index, field) {
 	        return value.toFixed(6);
 	    }
 	}, {
 	    field: 'cinvname',
 		title: '产品名称'
 	}, {
 	    field: 'cInvStd',
 		title: '产品规格'
 	 
 	}, {
 	    field: 'iquantity',
 		title: '基本数量'
 	 
 	},
    {
        field: 'cdefine22',
        title: '包装结果'

    },
 	{
 	    field: 'ccusname',
 		title: '客户名称'
 		 
 	}

 ]


 