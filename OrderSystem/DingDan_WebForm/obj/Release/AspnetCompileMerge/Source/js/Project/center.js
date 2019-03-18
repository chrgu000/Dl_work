var form;
layui.use(['layer', 'form'], function () {
    var $ = layui.jquery,
        layer = layui.layer;
    form = layui.form();
    //新版功能
    $($('.logck')).on("click", function() {
        $(".layui-field-box").find(".admin-log-content").css("display", "none");
        var $this = $(this);
        $($this).parent().parent(".admin-log-title").find(".admin-log-content").fadeToggle("slow");
    });
 


 
    setTimeout(LoginInfo, 5000);





})

$("#wxinfo").click(function () {

    layer.open({
        title: "多联企业微信关注说明",
        type: 1,
        anim:Math.floor(Math.random()*7),
        area: ['530px', '350px'],
        btn: [],
        shadeClose:true,
        content:' <div style="width:90%;margin:10px auto">\
            <h4>为了能正确接收我公司发送的相关微信信息，您需要按如下步骤操作：</h4>\
                    <p>一、关注公司 微信企业号 <img src="../images/wx.jpg"   style=" width:150px;height:150px" /></p>\
                    <p>二、点击按钮测试是否能接收公司发送的微信信息 <input type="button" id="wx"   value="发送测试信息" class="layui-btn " /></p>\
                    <p>三、如不能接收测试信息，请联系公司客服处理</p></div>'
    })
})

$(document).on("click","#wx",function () {
    $.ajax({
        url: "/Handler/LoginHandler.ashx",
        type: "Post",
        data: { "Action": "send_wx", "phone":"25434523645645" },
        success: function (data) {
            console.log(data);
            if (data == 'True') {
                layer.msg("发送成功，请查看多联企业微信！")
                wx_count = 60;
                wx_myCountDown = setInterval(wx_countDown, 1000);
            }
            else {
                layer.msg("发送失败，请重试或联系管理员！")
                wx_count = 10;
                wx_myCountDown = setInterval(wx_countDown, 1000);
            }
        },
        error: function (e) {
            alert("出现错误，请联系管理员！");
        }
    });
})
var wx_count;
var wx_myCountDown;
//点击获取验证码后，按钮倒记时
function countDown() {

    $("#get_code").attr("disabled", true);

    $("#get_code").val(count + " 秒后重新获取");

    count--;

    if (count == 0) {

        $("#get_code").val("获取验证码").removeAttr("disabled");

        clearInterval(myCountDown);

        count = 60;
    }
}

//点击获取微信后，按钮倒记时
function wx_countDown() {

    $("#wx").attr("disabled", true);
    
    $("#wx").addClass("layui-btn-disabled");

    $("#wx").val(wx_count + " 秒后重新获取");

    wx_count--;

    if (wx_count == 0) {

        $("#wx").val("发送测试信息").removeAttr("disabled");
        $("#wx").removeClass("layui-btn-disabled");
        clearInterval(wx_myCountDown);
        wx_count = 60;

    }
}

function LoginInfo() {
    $.ajax({
        url: "../Handler/ProductHandler.ashx",
        dataType: "Json",
        type: "Post",
        aysnc: false,
        data: {
            "Action": "LoginInfo", "info": JSON.stringify(Login_Info())
        },
        success: function (res) {

        },
        error: function (err) {
            console.log(err);
        }
    })

}

function Login_Info() {
    var info = {};
    info["screenHeight"] = window.screen.height;
    info["screenWidth"] = window.screen.width;
    info["clientIp"] = returnCitySN["cip"];
    info["cityId"] = returnCitySN["cid"];
    info["cityName"] = returnCitySN["cname"];
    var explorer = window.navigator.userAgent ;
    info["ua"]=explorer;

//ie 
if (explorer.indexOf("MSIE") >= 0) {
info["browser"]="ie";
}
//firefox 
else if (explorer.indexOf("Firefox") >= 0) {
info["browser"]="Firefox";
}
//Chrome
else if(explorer.indexOf("Chrome") >= 0){
info["browser"]="Chrome";
}
//Opera
else if(explorer.indexOf("Opera") >= 0){
info["browser"]="Opera";
}
//Safari
else if(explorer.indexOf("Safari") >= 0){
info["browser"]="Safari";
}
return info;
}



$("#report").click(function () {
    var html = "";
    html += '<div style="width:90%;margin:0 auto"><form class="layui-form layui-form-pane"  ><fieldset class="layui-elem-field layui-field-title"><legend>您对2017年07月28日“财税相关知识培训会”是否满意？</legend>\
        <div class="layui-field-box"> <input type="radio" name="satisfy" value="1" title="满意"><input type="radio" name="satisfy" value="0" title="不满意"></div>\
<legend>您是否有其他意见及建议？</legend><textarea id="cMemo" class="layui-textarea"></textarea>\
        </fieldset></form></div>';
    layer.open({
        area: ["600px", "400px"],
        type: 1,
        title: "财务培训回访",
//        content: '<div style="width:90%"><form class="layui-form layui-form-pane">\
//                <div class="layui-form-item" pane>\
//                <label class="layui-form-label">您对2017年07月28日“财税相关知识培训会”是否满意？</label>\
//                <input type="radio" name="satisfy" value="1" title="满意"><input type="radio" name="satisfy" value="0" title="不满意" checked> </div>\
//                <div class="layui-form-item" pane>\
//<label class="layui-form-label">您对2017年07月28日“财税相关知识培训会”是否满意？</label>\
// <div class="layui-input-inline">\
//<textarea class="layui-textarea"></textarea>\
        //              </div> </div> </form></div>',
        content:html,
        success: function () {
            form.render();
        },
        btn: ["提交", '关闭'],
        btn1: function () {
            if ($("input[name=satisfy]:checked").length == 0) {
                layer.alert("你还未选择是否满意！", {
                    icon: 2
                })
                return false;
            }
            if ($("#cMemo").val().trim() == "") {
                layer.alert("提点建议吧！", {
                    icon: 2
                })
                return false;
            }

                $.ajax({
                    url: "../Handler/ProductHandler.ashx",
                    dataType: "Json",
                    type: "Post",
                    data: {
                        "Action": "SubmitOrderQuestion",
                        "satisfy": $("input[name=satisfy]:checked").val(),
                        "cMemo":$("#cMemo").val()
                    },
                    success: function (res) {
                        if (res.flag != 1) {
                            layer.alert(res.message, { icon: 2 })
                            return false;
                        } else {
                            layer.alert("提交成功，感谢参与！", { icon: 1, closeBtn: 0 }, function () {
                                layer.closeAll();
                            })
                        }
                    },
                    error: function(err) {
                        console.log(err);
                    }
                })
        }
    })
})

//$("#href_old").click(function() {
//    $.ajax({
//        url: "../Handler/LoginHandler.ashx",
//        dataType: "Json",
//        type: "Post",
//        data: {
//            "Action": "Get_Token"
//        },
//        success: function(res) {
//            window.top.location.href = "http://dl.duolian.com:1235/login.aspx?token=" + res.message;
//        },
//        error: function(err) {
//            console.log(err);
//        }
//    })
//})