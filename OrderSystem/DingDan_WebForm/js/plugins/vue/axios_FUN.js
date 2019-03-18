axios.interceptors.request.use(

    config => {
        // vm.handleSpinShow();
        startLoading();
        let ret = '';
        for (let it in config.data) {
            ret += encodeURIComponent(it) + '=' + encodeURIComponent(config.data[it]) + '&'
        }
        config.data = ret;

        return config;
    },
    err => {
        return Promise.reject(err);
    });
axios.interceptors.response.use(

    data => {
       
        let res = false;
        if (data.data.flag ==0) {
            vm.$Message.error({
                content: data.data.message,
                duration: 15,
                closable: true
            });

        } else {
            res =data.data;
        }

        completeLoading();
        return res;
    }, error => {
        completeLoading();
        alert(error)
        return Promise.reject(error);
    }

)


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