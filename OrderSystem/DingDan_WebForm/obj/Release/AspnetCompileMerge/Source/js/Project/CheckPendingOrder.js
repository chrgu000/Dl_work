layui.use(['layer'], function () {

    var layer = layui.layer;
    var vm = new Vue({
        el: "#vm",
        data: {
            orders: [],
            products: [],
            type: "1"
        },
        methods: {

        },
        computed: {
            money: function () {
                console.log(this)
                var m = 0;
                if ($("#detail tbody tr").length > 0) {
                    if (type == "0") {
                        m += Number($("#detail tbody").find("tr.sum").text());
                    } else {
                        m += Number($("#detail tbody tr").find(".isum").text());
                    }
                }

                return m;
            }
        }
    })


    $.ajax({
        type: "Post",
        url: "../Handler/ProductHandler.ashx",
        dataType: "Json",
        data: { "Action": "DL_UnauditedOrder_SubBySel" },
        success: function (data) {
            vm.orders = data.dt;
            vm.type = "1";
            layer.closeAll();
        },
        error: function (err) {
            console.log(err);
            layer.alert("获取数据失败,请重试或联系管理员!", { icon: 2 });
        }



    });

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

    $(document).on("click", ".recaption", function () {
        var lngoporderid = $(this).parents("tr").find(".lngoporderid").text();
        var strBillNo = $(this).parents("tr").find(".strBillNo").text();
        $(this).parents("tbody").find("tr").removeClass("blue");
        $(this).parents("tr").addClass("blue");
        layer.confirm("取回的订单请在30分钟内重新提交，否则将被自动关闭！", { icon: 3 }, function () {
            $.ajax({
                type: "Post",
                url: "../Handler/ProductHandler.ashx",
                dataType: "Json",
                data: { "Action": "RecaptionOrder", "strBillNo": strBillNo, "lngoporderid": lngoporderid },
                success: function (data) {
                    if (data.flag == '0') {
                        layer.alert(data.message, { icon: 2, closeBtn: 0 }, function () {
                            layer.closeAll();
                        });
                        return false;
                    } else if(data.flag=='1'){
                        layer.alert(data.message, { icon: 1, closeBtn: 0 }, function () {
                            layer.closeAll();
                        });
                        $("#Reject_Order tbody").find(".blue").remove();
                    }
                },
                error: function (err) {
                    console.log(err);
                    layer.alert("获取数据失败,请重试或联系管理员!", { icon: 2 });
                }
            });
        })
    })


})

