   layui.use(['element', 'util', 'laydate', 'form'], function () {
            //  var $ = layui.jquery,
            var form = layui.form(),
                util = layui.util,
              laydate = layui.laydate,
              element = layui.element(); //Tab的切换功能，切换事件监听等，需要依赖element模块

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

            var data="";
            draw_tb(data);
             draw_detail(data);

            $("#btn").click(function () {
                $.ajax({
                    url: "../Handler/ProductHandler.ashx",
                    type: "Post",
                    dataType: "Json",
                    data: {
                        "Action": "DLproc_MyWorkPreYOrderForCustomerBySel",
                        "start_date":$("#start_date").val(),
                        "end_date": $("#end_date").val(),
                        "strbillno":$("#strbillno").val(),
                        "orderstatus": $("#orderStatus  option:selected").val()
                    },
                    success: function (data) {
                        if (data.flag == 0) {
                            layer.alert(data.message, { icon: 2 });
                        } else {
                         draw_tb(data);
                  
                        }
                    }
                })
            })



        })

        function draw_tb(data) {
            $("#PreXOrder_list").DataTable({
                scrollY: "300px",
                info: false,
                autoWidth: false,
                paging: false,
                searching: false,
                destroy: true,
                data: data.dt,
                "language": {
                    "lengthMenu": "每页 _MENU_ 条记录",
                    "zeroRecords": "没有找到记录",
                    "info": "第 _PAGE_ 页 ( 总共 _PAGES_ 页 )",
                    "infoEmpty": "无记录",
                    "infoFiltered": "(从 _MAX_ 条记录过滤)"
                },
                "columns": [
              {"title": "操作", "data": null,class:"center",orderable:false,
                render:function(){
                  return '<input type="button" class="layui-btn layui-btn-small show_detail" value="查看订单"/>';
                }
              },
              {"title": "网单号", "data": "strBillNo",class:"center"},
              {"title": "正式订单号", "data": "cSOCode",class:"center"},
              {"title": "订单状态", "data": "bytStatus",class:"center"},
              {"title": "提交日期", "data": "xdsj",class:"center"},
              {"title": "开票单位", "data": "ccusname",class:"center"}
                ]
            });
        }

          function draw_detail(data) {
            $("#PreXOrder_detail").DataTable({
                scrollY: "200px",
                info: false,
                autoWidth: false,
                paging: false,
                searching: false,
                destroy: true,
                data: data.dt,
                "language": {
                    "lengthMenu": "每页 _MENU_ 条记录",
                    "zeroRecords": "没有找到记录",
                    "info": "第 _PAGE_ 页 ( 总共 _PAGES_ 页 )",
                    "infoEmpty": "无记录",
                    "infoFiltered": "(从 _MAX_ 条记录过滤)"
                },
                "columns": [
              {"title": "名称", "data": "cinvname",class:"center"},
              {"title": "规格", "data": "cInvStd",class:"center"},
              {"title": "单位组", "data": "UnitGroup",class:"center"},
              {"title": "基本数量汇总", "data": "iquantity",class:"center"},
              {"title": "包装结果", "data": "cDefine22",class:"center"},
              {"title": "单价", "data": "iquotedprice",class:"center"},
              {"title": "金额", "data": "cComUnitAmount",class:"center"},
              {"title": "执行单价", "data": "itaxunitprice",class:"center",visible:false},
              {"title": "执行金额", "data": "xx",class:"center xx",visible:false},
                ]
            });
        }


     
        $(document).on("click",".show_detail",function(){
          var strBillNo=$(this).parents("tr").find("td:eq(1)").text();
          $.ajax({
            url:"../Handler/ProductHandler.ashx",
            dataType:"Json",
            type:"Post",
            data:{"Action":"DL_XOrderBillDetailBySel","strBillNo":strBillNo},
            success:function(data){

              draw_detail(data);
              $("#info").html('<strong>订单号 :</strong>'+strBillNo+'       <strong>下单时间 :</strong>'+return_datetime(data.dt[0].datBillTime)+'         <strong>开票单位 :</strong>'+data.dt[0].ccusname);
              var money=0;
              var table=$("#PreXOrder_detail").DataTable();
              $.each(table.columns(".xx").data()[0],function(i,v){
                money+=v;
              })
              $("#money").text(money.toFixed(2));
            },
            error:function(err){
              layer.alert("出现错误，请重试或联系管理员!",{icon:2});
            }
          })
        })
    
        $("#btn1").click(function () {
          $('#detail').tableExport({type:'excel',escape:'false'});

        })
        //转换日期格式，将格式为“/Date(-2036476800000+0800)/”转换为“2016-6-25 17:14:38”
        function return_datetime(date) {
            if (date != "" && date != null) {
                var new_date = date.slice(6, 19);

                var time = new Date(Number(new_date));
                return time.getFullYear() + "-" + (time.getMonth() + 1) + "-" + time.getDate() + " " + time.getHours() + ":" + time.getMinutes() + ":" + time.getSeconds();
            }
        }