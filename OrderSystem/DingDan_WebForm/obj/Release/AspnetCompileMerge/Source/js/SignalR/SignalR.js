var signalRInit = function () {
    // 声明一个代理引用该集线器,记得$.connection.后面的方法首字母必须要小写,这也是我为什么使用别名的原因

    var chat = $.connection.getMessage;


     

    $.connection.hub.start().done(function () {
        signalR_login(chat.connection.id);

        $("#getAllClient").click(function () {
            chat.server.getAllClient();
        });

        $('#send').click(function () {

            chat.server.sendToOneClient($("#message").val().trim());
        });

        $('#send1').click(function () {
            chat.server.send1();
        });


    });

    chat.client.receviceMsg = function (msg) {

        var t = JSON.parse(msg);

        console.log(flagToMsg(t["flag"]))
      //  exit();
       layer.alert(flagToMsg(t["flag"]))

    }


    chat.client.sendSignalRId = function () {
        var SignalRId = chat.connection.id;
        chat.server.checkOnlineUsers(SignalRId);
    }


    $("#SignalR_Login").click(function () {
        chat.server.signalR_Login(chat.connection.id);
    })


    chat.client.sendMsg = function (msg) {
        msg = JSON.parse(msg);
       
        layer.open({
            title: msg["title"],
            content:msg["content"]
        })
        //  var str = "<script>location.href ='" + msg + "'<\/script>";
        //  console.log(str);
        //  document.write(str)

    }

    chat.client.exit=function(msg){
        msg = JSON.parse(msg);
        exit_login();
        console.log(msg)
        layer.alert(flagToMsg(msg["flag"]), {icon:2, closeBtn: 0 }, function () {
            top.window.location.reload();
        })
    }

    chat.client.sendSignalRId = function () {
        chat.server.signalR_Login(chat.connection.id);
    }



    function get_signalRId() {
        return chat.connection.id;
    }

    function signalR_login(id) {
        $.ajax({
            //要用post方式      
            type: "Post",
            //方法所在页面和方法名      
            url: "/Handler/SignalRHandler.ashx",
            //  contentType: "application/json; charset=utf-8",
            dataType: "Json",
            data: { "Action": "client_Login", "signalRId": id },
            success: function (data) {
                //console.log(data)
            },
            error: function (err) {
                console.log("err");
                console.log(err)
            }
        });
    }
    //}
    //var receviceMsg = {
    //    "1": "正常",
    //    "99": "重复登录！"
    //}

    function flagToMsg(flag) {
        switch (flag) {
            case "1":
                return "正常";
                break;
            case "99":
                return "你的账号已在其它地点登录!<br />如非本人操作,请立即修改密码!";
                break;

        }

    }
}