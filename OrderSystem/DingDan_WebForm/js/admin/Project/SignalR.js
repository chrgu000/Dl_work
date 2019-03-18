//var form;

layui.use(['layer', 'form'], function () {

    var layer = layui.layer;
    var form = layui.form();



    var vm = new Vue({
        el: "#vm",
        data: {
            msg: "123",
            users: []


        },
        methods: {
            //获取所有客户
            getAllUsers: function () {
                $.ajax({
                    type: "Post",
                    url: "/Handler/SignalRHandler.ashx",
                    dataType: "Json",
                    async: false,
                    data: { "Action": "getAllClient" },
                    success: function (data) {
                        console.log(data)
                        var o = [];
                        $.each(data, function (i, v) {
                            o.push(v);
                        })
                        vm.users = o;
                        console.log(vm)
                    },
                    error: function (err) {
                        layer.alert("获取数据失败,请重试或联系管理员!", { icon: 2 });
                        console.log(err);
                    }
                });
            },

            sendMsg: function (event) {
                var strAllAcount = $(event.target).parents("tr").find(".strAllAcount").text();
                var signalRId = $(event.target).parents("tr").find(".signalRId").text();
                layer.open({
                    type: 1,
                    title: "发送信息给" + strAllAcount,
                    content: $("#sendMsg_div").html(),
                    area: ['600px', '400px'],
                    btn: ["发送", "取消"],
                    success: function () {
                        form.render();
                    },
                    btn1: function (index, layero) {
                        var title = layero.find("#msg_title").val().trim(), content = layero.find("#msg_content").val().trim();
                        if (title=="") {
                            layer.alert("标题不能为空！", { icon: 2 })
                            return false;
                        }
                        if (content == "") {
                            layer.alert("内容不能为空！", { icon: 2 })
                            return false;
                        }
                        $.ajax({
                            type: "Post",
                            url: "/Handler/SignalRHandler.ashx",
                            dataType: "Json",
                            async: false,
                            data: { "Action": "sendMsg", "title": title, "content": content, "signalRId": signalRId },
                            success: function (data) {
                                layer.alert("消息已发送", { icon: 1 ,closeBtn:0}, function () {
                                    layer.closeAll();
                                });
                            },
                            error: function (err) {
                                console.log(err)
                            }
                        });
                    }
                })


            },
            userExit: function () {

            },
            sendAll: function () {
                layer.confirm("你确定要发送全体消息？", { icon: 3 }, function () {
                    layer.open({
                        type: 1,
                        title: "发送全体消息",
                        content: $("#sendMsg_div").html(),
                        area: ['600px', '400px'],
                        btn: ["发送", "取消"],
                        success: function () {
                            form.render();
                        },
                        btn1: function (index, layero) {
                            var title = layero.find("#msg_title").val().trim(), content = layero.find("#msg_content").val().trim();
                            if (title == "") {
                                layer.alert("标题不能为空！", {icon:2})
                                return false;
                            }
                            if (content == "") {
                                layer.alert("内容不能为空！", { icon: 2 })
                                return false;
                            }
                            $.ajax({
                                type: "Post",
                                url: "/Handler/SignalRHandler.ashx",
                                dataType: "Json",
                                async: false,
                                data: { "Action": "sendMsg", title: title, "content": content},
                                success: function (data) {
                                    layer.alert("消息已发送", { icon: 1, closeBtn: 0 }, function () {
                                        layer.closeAll();
                                    });
                                    
                                },
                                error: function (err) {
                                    console.log(err)
                                }
                            });
                        }
                    })
                })
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
                console.log("1")
                if (this.orders != null && this.orders.length != 0) {
                    return false;
                } else {
                    return true;
                }
            }
        }
    });

    vm.getAllUsers();




    $(document).on("click", ".btn_sendmsg", function () {
        $("#tb").find("tr").removeClass("select")
        $(this).parents("tr").addClass("select");
        //var code = $(this).parents("tr").find("td:eq(1)").text();
        var index = $("#tb").find("tr.select").data("index");
        var list = $("#tb").bootstrapTable('getData')[index];
        console.log(list)
    })

    form.on('checkbox', function (data) {

        if (data.value == 'on') {
            console.log($(this))
        }
    })




















});



