


var vm = new Vue({
    el: "#vm",
    data: {
        orders: [],
        products: [],
        type: "0",
        cCusName: "",
        CreateDate: "",
        cSOCode: "",
        cMemo: "",
        cMemo_sub: "",
        cSCCode: "",
        address: ""

    },
    methods: {
        show: function (event) {
            var strBillNo = $(event.target).parents("tr").find(".strBillNo").text();
            $(event.target).parents("tbody").find("tr").removeClass("blue");
            $(event.target).parents("tr").addClass("blue");
            $(event.target).parents("tbody").find("tr").find(".confirm").addClass("layui-btn-disabled");
            $(event.target).parents(".blue").find(".confirm").removeClass("layui-btn-disabled");
            $.ajax({
                type: "Post",
                url: "../Handler/ProductHandler.ashx",
                dataType: "Json",
                async: false,
                data: { "Action": "DL_OrderU8BillBySel_new", strBillNo: strBillNo },
                success: function (data) {
                    vm.products = data.dt;
                    $("#strBillNo_span").text('订单号: ' + strBillNo);
                },
                error: function (err) {
                    console.log(err);
                    layer.alert("获取数据失败,请重试或联系管理员!", { icon: 2 });
                }
            });

        },
        money: function () {
            if (this.products == null) {
                return 0;
            } else {
                var sum = 0;
                for (var i = 0; i < this.products.length; i++) {
                    sum += this.products[i].iSum;
                };
                return sum.toFixed(2);
            }
        },
        count: function () {
            if (this.products == null) {
                return 0;
            } else {
                return this.products.length;
            }
        },
        seen_products: function () {
            if (this.products != null && this.products.length != 0) {
                vm.cCusName = this.products[0].cCusName;
                vm.CreateDate = this.products[0].CreateDate;
                vm.cSOCode = this.products[0].cSOCode;
                if (this.products[0].cMemo != null) {
                    vm.cMemo = this.products[0].cMemo;
                } else {
                    vm.cMemo = "";
                }
                //vm.cMemo=this.products[0].cMemo!=null?this.products[0].cMemo:"";
                if (vm.cMemo.length > 25) {
                    vm.cMemo_sub = vm.cMemo.substring(0, 20) + "......";
                } else {
                    vm.cMemo_sub = vm.cMemo;
                }
                if (this.products[0].cSCCode === "00") {
                    vm.cSCCode = "自提";
                    vm.address = "车牌号:" + this.products[0].cDefine10 == null ? "" : this.products[0].cDefine10 + ",司机姓名:" + this.products[0].cDefine1 == null ? "" : this.products[0].cDefine1 + ",司机电话:" + this.products[0].cDefine13 == null ? "" : this.products[0].cDefine13 + ",司机身份证:" + this.products[0].cDefine2 == null ? "" : this.products[0].cDefine2;

                } else if (this.products[0].cSCCode === "01") {
                    vm.cSCCode = "配送";
                    vm.address = "收货人电话:" + this.products[0].cDefine12 == null ? "" : this.products[0].cDefine12 + ",收货人地址:" + this.products[0].cDefine11 == null ? "" : this.products[0].cDefine11;
                }
                return false;
            } else {
                return true;
            }
        },
        confirm: function (event) {
            if ($(event.target).parents("tr").find(".confirm").hasClass("layui-btn-disabled")) {
                return false;
            }
            layer.confirm("你是否已仔细核实了订单信息?", { icon: 3 }, function () {
                var strBillNo = $(event.target).parents("tr").find(".strBillNo").text();
                $.ajax({
                    type: "Post",
                    url: "../Handler/ProductHandler.ashx",
                    dataType: "Json",
                    data: { "Action": "DL_U8OrderBillConfirmByUpd", "strBillNo": strBillNo },
                    success: function (data) {
                        if (data.flag == "0") {
                            layer.alert(data.message, { icon: 2 });
                        } else if (data.flag == "1") {
                            layer.alert(data.message, { icon: 1 });
                            $(event.target).parents("tr").remove();
                            vm.products = [];
                            vm.cMemo = "";
                            vm.cMemo_sub = "";
                            vm.address = "";
                            vm.CreateDate = "";
                            vm.cSCCode = "";
                            vm.cSOCode = "";
                            vm.cCusName = "";


                        }
                    },
                    error: function (err) {
                        console.log(err);
                        layer.alert("操作失败,请重试或联系管理员!", { icon: 2 });
                    }
                });
            })

        }

    },
    computed: {
        seen_orders: function () {
            if (this.orders != null && this.orders.length != 0) {
                return false;
            } else {
                return true;
            }
        }
    }
});

    layui.use(['layer'], function () {

        var layer = layui.layer;
        $.ajax({
            type: "Post",
            url: "../Handler/ProductHandler.ashx",
            dataType: "Json",
            async: false,
            data: { "Action": "DL_UnauditedOrderBySel" },
            success: function (data) {
                console.log(data.dt.length);
                if (data.dt.length > 0) {
                    vm.orders = data.dt;
                }

            },
            error: function (err) {

                layer.alert("获取数据失败,请重试或联系管理员!", { icon: 2 });
                console.log(err);
            }
        });

    })


//function load_orders() {
//    $.ajax({
//        type: "Post",
//        url: "/../Handler/ProductHandler.ashx",
//        dataType: "Json",
//        async:false,
//        data: { "Action": "DL_UnauditedOrderBySel" },
//        success: function (data) {
//            console.log(data.dt.length);
//            if (data.dt.length>0) {
//                vm.orders = data.dt;
//            }
            
//        },
//        error: function (err) {

//            layer.alert("获取数据失败,请重试或联系管理员!", { icon: 2 });
//            console.log(err);
//        }
//    });
//}


////JQ ajax全局事件
//$(document).ajaxStart(function () {
//    layer.load();
//}).ajaxComplete(function (request, status) {
//    layer.closeAll('loading');
//});
