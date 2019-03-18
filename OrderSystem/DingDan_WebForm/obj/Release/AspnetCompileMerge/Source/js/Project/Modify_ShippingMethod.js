 
layui.use(['layer','form'] ,function(){
    var form = layui.form();
     var layer=layui.layer;
    var psAddr, ztAddr, xzqAddr;
    var psOptions, ztOptions, xzqOptions,carOptions;
    var vm = new Vue({
        el: "#vm",
        data: {
            orders: [],
            products: [],
            type: "0"
        },
        methods: {

        },
        computed: {
            money: function () {
                var m = 0;
                if ($("#detail tbody tr").length > 0) {
                    if (type == "0") {
                        m += Number($("#detail tbody").find("tr.sum").text());
                    } else {
                        m += Number($("#detail tbody tr").find(".ex_sum").text());
                    }
                }

                return m;
            }
        }
    })
 
   Get_info();
    
  
    function Get_info() {
        $.ajax({
            type: "Post",
            url: "../Handler/ProductHandler.ashx",
            dataType: "Json",
            data: { "Action": "Get_ModifyShippingMethod_list" },
            success: function (data) {
                vm.orders = data.dt;
                vm.type = data.message;
                ztOptions = "";
                $.each(data.list_dt[0], function (i, v) {
                    ztOptions += '<option value=' + data.list_dt[0][i].lngopUseraddressId + '>' + data.list_dt[0][i].ShippingInformation + '</option>';
                });
                psOptions = "";
                $.each(data.list_dt[1], function (i, v) {
                    psOptions += '<option value=' + data.list_dt[1][i].lngopUseraddressId + '>' + data.list_dt[1][i].ShippingInformation + '</option>';
                });
                xzqOptions = "";
                $.each(data.list_dt[2], function (i, v) {
                    xzqOptions += '<option value=' + data.list_dt[2][i].lngopUseraddress_exId + '>' + data.list_dt[2][i].xzq + '</option>';
                });
                carOptions = "";
                $.each(data.list_dt[3], function (i, v) {
                    carOptions += '<option value=' + data.list_dt[3][i].cValue + '>' + data.list_dt[3][i].cValue + '</option>';
                });

              
            },
            error: function (err) {
                console.log(err);
                layer.alert("获取数据失败,请重试或联系管理员!", { icon: 2 });
            }
        });

    }

    //查看
    $(document).on("click", ".show", function () {
        var strBillNo = $(this).parents("tr").find(".strBillNo").text();
        $(this).parents("tbody").find("tr").removeClass("blue");
        $(this).parents("tr").addClass("blue");
        $.ajax({
            type: "Post",
            url: "../Handler/ProductHandler.ashx",
            dataType: "Json",
            data: { "Action": "DL_OrderBillBySel", strBillNo: strBillNo },
            success: function (data) {
                vm.products = data.dt;
                $("#strBillNo_span").text('订单号: ' + strBillNo);
                console.log(vm)
                var m = 0;
                if (vm.type == "1") {
                    $.each(vm.products, function (i, v) {
                        m += v.isum;
                    });
                } else {
                    $.each(vm.products, function (i, v) {
                        m += v.cComUnitAmount;
                    });
                }
                $("#money").text(m.toFixed(2));
                $("#count").text(vm.products.length);
            },
            error: function (err) {
                console.log(err);
                layer.alert("获取数据失败,请重试或联系管理员!", { icon: 2 });
            }
        });

    })

    //修改
    $(document).on("click", ".recaption", function () {
        var lngoporderid = $(this).parents("tr").find(".lngoporderid").text();
        var strBillNo = $(this).parents("tr").find(".strBillNo").text();
        $(this).parents("tbody").find("tr").removeClass("blue");
        $(this).parents("tr").addClass("blue");
        var cSCCode = $("#list tbody .blue .cSCCode").text().trim();
        //  console.log($(this).parents("tr").find(".cSCCode").text())
        layer.open({
            type: 1 //Page层类型
   , area: ['900px', '500px']
   , title: "订单编号：" + strBillNo
   , shadeClose: true
   , shade: 0.8 //遮罩透明度
            //, maxmin: true //允许全屏最小化
   , anim: 2 //0-6的动画形式，-1不开启
   , content: '<form class="layui-form layui-form-pane"  style="padding:30px;">\
<div class="layui-form-item">\
<label class="layui-form-label">送货方式</label>  \
<input name="shfs" id="ps" value="配送" title="配送" type="radio" class="radio" disabled  lay-filter="method" csccode="01">\
<input name="shfs" id="zt" value="自提" title="自提" type="radio" class="radio" disabled lay-filter="method"  csccode="00">\
</div>\
<div class="layui-form-item" >\
<label class="layui-form-label">送货地址</label>\
<div class="layui-inline" id="txtAddress_div"  style="width:80%"><select id="txtAddress" lay-search lay-filter="txtAddress"><option value>请选择送货方式</option></select>\
</div></div>\
<div class="layui-form-item" >\
<label class="layui-form-label">车型</label>\
<div class="layui-inline"   style="width:80%"><select id="carType" lay-search lay-filter="carType"><option value>请选择装车方式</option></select>\
</div></div>\
<div class="layui-form-item" >\
<label class="layui-form-label">备注</label>\
<div class="layui-inline"><textarea style="width:670px;height:150px"  placeholder="点击输入备注内容" id="remark" class="layui-input"></textarea>\
</div></div>\
</form>'
            //<div class="layui-form-item" >\
            //<label class="layui-form-label">送货地址</label>\
            //<div class="layui-inline" id="txtAddress_div"  style="width:80%"><select id="txtArea" lay-search lay-filter="txtAddress"><option value>自提必须选择行政区</option></select>\
            //</div></div>\
    , success: function () {
        $("#remark").text($("#list tbody .blue .remark").text().trim())
        $("#carType").html(carOptions);
        $("#carType").val($("#list tbody .blue .carType").text().trim());
        if ($("#list tbody .blue .cSCCode").text().trim() == "配送") {
            $("#ps").attr("checked", true);
            $("#txtAddress").html(psOptions);
            $("#txtAddress").val($("#list tbody .blue .lngopUseraddressId").text().trim())
        } else {
            $("#zt").attr("checked", true);
           // $("#txtAddress").html('<option value>选择送货地址</option>')
            $("#txtAddress").html(ztOptions);
            $("#txtAddress").val($("#list tbody .blue .lngopUseraddressId").text().trim())
        }
        form.render();
    }
   , btn: ['确定', '关闭']
   , btn1: function (index, layero) {
       if ($("#list tbody .blue .lngoporderid").text().trim()=="") {
           layer.alert("出现异常，请联系技术人员！", { icon: 2 }, function () {
               layer.closeAll();
           });
           return false;
       }
       if (!$("#ps").prop("checked") && !$("#zt").prop("checked")) {
           layer.alert("出现异常，请联系技术人员！", { icon: 2 }, function () {
               layer.closeAll();
           });
           return false;
       }
       if ($("#txtAddress option:selected").val()=="") {
           layer.alert("你还未选择送货地址！", { icon: 2 } );
           return false;
       }
       $.ajax({
           type: "Post",
           url: "../Handler/ProductHandler.ashx",
           dataType: "Json",
           data: {
               "Action": "ModifyShippingMethod", "lngoporderid": $("#list tbody .blue .lngoporderid").text().trim(),
               "cCSCode": $("#ps").prop("checked") ? "01" : "00", "addressId": $("#txtAddress option:selected").val(),
               "strRemark":$("#remark").val().trim(),"carType":$("#carType option:selected").val()
           },
           success: function (data) {
               if (data.flag == '0') {
                   layer.alert(data.message, { icon: 2, closeBtn: 0 }, function () {
                       layer.closeAll();
                   });
                   return false;
               } else if (data.flag == '1') {
                   layer.alert(data.message, { icon: 1, closeBtn: 0 }, function () {
                       layer.closeAll();
                   });
                   Get_info();
               }
           },
           error: function (err) {
               console.log(err);
               layer.alert("获取数据失败,请重试或联系管理员!", { icon: 2 });
           }
       });
   }

        });

    })





    form.on('radio(method)', function (data) {
        $("#txtAddress").html('<option value>选择送货地址</option>')
        if (data.value == "配送") {
            $("#txtAddress").append(psOptions);
        } else {
            $("#txtAddress").append(ztOptions);
        }
        form.render();
    })







})







