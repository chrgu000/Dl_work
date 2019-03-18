var $ = layui.jquery;
//JQ ajax全局事件
    $(document).ajaxStart(function () {
        layer.load();
    }).ajaxComplete(function (request, status) {
        layer.closeAll('loading')
        //  layer.closeAll('loading');
    }).ajaxError(function () {
        layer.alert('程序出现错误,请重试或联系管理员!', {
            icon: 2
        })
        return false;
    });

//按钮点击后去除焦点，防止Enter重复点击
$(document).on('click','.layui-btn',function(){
    $(this).blur();
})

//错误提示
function errMsg(msg) {
    layer.alert(msg, {
        icon: 2,
        closeBtn: 0,
        anim: get_msgIndex()
    })
}

//获取动画随机数
function get_msgIndex() {
    return Math.floor((Math.random() * 7));
}

function t() {
    layer.closeAll('loading')
}


//采用正则表达式获取地址栏参数 
function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]);
    return null;
}

