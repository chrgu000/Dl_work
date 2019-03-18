//mui.ajaxSettings({
////	beforeSend: function() {
////					if (1) {
////						mui.showWaiting("ttt", "options");
////					}
////				},
////				complete: function() {
////					if (loding) {
////						plus.nativeUI.closeWaiting();
////					}
////				},
//	error: function(xhr, type, errorThrown) {
//					//异常处理；
//					mui.alert(errorThrown+"1");
//					console.log(type);
//					return false;
//				}
//})

//转换日期格式，将格式为“/Date(-2036476800000+0800)/”转换为“2016-09-09”
function return_date(date) {
    if (date != "" && date != null) {
        if (date.indexOf("T") > -1) {
            var arr = date.split("T");
            return arr[0];

        } else {
            var new_date = date.slice(6, 19);
            var time = new Date(Number(new_date));
            var t = time.getFullYear() + "-";
            t = t + ((time.getMonth() + 1).toString().length == 2 ? (time.getMonth() + 1) : ("0" + (time.getMonth() + 1).toString())) + "-";
            t = t + ((time.getDate()).toString().length == 2 ? (time.getDate()) : ("0" + (time.getDate()).toString()));
            return t;
        }

        //return time.toLocaleDateString().replace(/\//g, "-");
    }
    else {
        return "";
    }
}
//转换日期格式，将格式为“/Date(-2036476800000+0800)/”转换为“2016-6-25 17:14:38”
function return_datetime(date) {
    if (date != "" && date != null) {
        if (date.indexOf("T") > -1) {
            var arr = date.split("T");
            var t = arr[0] + " " + arr[1].substr(0, 8);
            return t;
        } else {
            var new_date = date.slice(6, 19);
            var time = new Date(Number(new_date));
            var t = time.getFullYear() + "-";
            t = t + ((time.getMonth() + 1).toString().length == 2 ? (time.getMonth() + 1) : ("0" + (time.getMonth() + 1).toString())) + "-";
            t = t + ((time.getDate()).toString().length == 2 ? (time.getDate()) : ("0" + (time.getDate()).toString())) + " ";
            t = t + ((time.getHours()).toString().length == 2 ? (time.getHours()) : ("0" + (time.getHours()).toString())) + ":";
            t = t + ((time.getMinutes()).toString().length == 2 ? (time.getMinutes()) : ("0" + (time.getMinutes()).toString())) + ":";
            t = t + ((time.getSeconds()).toString().length == 2 ? (time.getSeconds()) : ("0" + (time.getSeconds()).toString()));
            // return time.getFullYear() + "-" +  (time.getMonth() + 1)+ "-" + time.getDate() + " " + time.getHours() + ":" + time.getMinutes() + ":" + time.getSeconds();
            return t;
        }

    }
    else {
        return "";
    }
}



//采用正则表达式获取地址栏参数 
function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]);
    return null;
}

//var mask = mui.createMask(function () {
//    mui(".mui-backdrop")[0].innerHTML = '<div style="margin:200px auto;text-align:center;color:white"><h1>加载中</h1></div>';
//    return false;
//});
 
////设置全局beforeSend
//mui.ajaxSettings.beforeSend = function (xhr, setting) {
//    //beforeSend演示,也可在$.ajax({beforeSend:function(){}})中设置单个Ajax的beforeSend
//    //                  console.log('beforeSend:::'  );
//    //   loading();
//    mask.show();
//};
////设置全局complete
//mui.ajaxSettings.complete = function (xhr, status) {
//    //                  console.log('complete:::' + status);
//    //removeLoading('loading');
//    mask.close();
//}


//function loading() {
//    alert("sadf")
//    $('body').loading({
//        loadingWidth: 240,
//        title: '请稍等',
//        name: 'loading',
//        discription: '数据查询中',
//        direction: 'column',
//        type: 'origin',
//        // originBg:'#71EA71',
//        originDivWidth: 40,
//        originDivHeight: 40,
//        originWidth: 6,
//        originHeight: 6,
//        smallLoading: false,
//        loadingMaskBg: 'rgba(0,0,0,0.6)'
//    });
//}

//mui(document)[0].ajaxComplete(function () {　 //mui.alert("complete")
//    removeLoading('loading');
//}).ajaxStart(function () {
//    loading();
//});



//window.onload = startLoading();
//监听加载状态改变  
 //document.onreadystatechange = setTimeout(completeLoading, 2000);

//  加载loading状态
function startLoading() {
    //获取浏览器页面可见高度和宽度  
    var _PageHeight = document.documentElement.clientHeight,
        _PageWidth = document.documentElement.clientWidth;
    //计算loading框距离顶部和左部的距离（loading框的宽度为215px，高度为61px）  
    var _LoadingTop = _PageHeight > 61 ? (_PageHeight - 61) / 2 : 0,
        _LoadingLeft = _PageWidth > 215 ? (_PageWidth - 215) / 2 : 0;
    //在页面未加载完毕之前显示的loading Html自定义内容  
  //  var _LoadingHtml = '<div id="loadingDiv" style="position:absolute;left:0;width:100%;height:' + _PageHeight + 'px;top:0;background:#a9a9a9;opacity:0.9;filter:alpha(opacity=100);z-index:10000;"><div style="position: absolute; cursor: wait; left: ' + _LoadingLeft + 'px; top:' + _LoadingTop + 'px; width: auto; height: 50px; line-height: 50px; padding-left: 70px; padding-right: 5px; background: #a9a9a9 url(../images/loading.gif) no-repeat scroll 5px 10px;  "> </div></div>';
    // document.write(_LoadingHtml);
    var _LoadingHtml = '<div id="loadingDiv" style="position:absolute;left:0;width:100%;height:' + _PageHeight + 'px;top:0;background:#505050;opacity:0.9;filter:alpha(opacity=100);z-index:10000;"><div id="cssload-pgloading"><div class="cssload-loadingwrap"><ul class="cssload-bokeh"><li></li><li></li><li></li><li></li></ul></div></div></div>';
    if (!document.getElementById('loadingDiv')) {
        var d = document.createElement("div");
        d.innerHTML = _LoadingHtml;
        document.getElementsByTagName('body')[0].appendChild(d);
       
    }
 

}

//加载状态为complete时移除loading效果  
function completeLoading() {
    if (document.getElementById('loadingDiv')) {
        var loadingMask = document.getElementById('loadingDiv');
        loadingMask.parentNode.removeChild(loadingMask);
    }

    
}

//设置全局beforeSend
mui.ajaxSettings.beforeSend = function (xhr, setting) {
    //beforeSend演示,也可在$.ajax({beforeSend:function(){}})中设置单个Ajax的beforeSend
    //                  console.log('beforeSend:::'  );
    //   loading();
   startLoading();
   
};
//设置全局complete
mui.ajaxSettings.complete = function (xhr, status) {
    //                  console.log('complete:::' + status);
    //removeLoading('loading');
     completeLoading();
 //setTimeout(completeLoading, 3000);
}