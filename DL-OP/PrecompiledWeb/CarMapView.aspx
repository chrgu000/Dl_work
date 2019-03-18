<%@ page language="C#" autoeventwireup="true" inherits="CarMapView, dlopwebdll" enableviewstate="false" %>

<!DOCTYPE html>

<html>
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="initial-scale=1.0, user-scalable=no, width=device-width">
    <title>车辆位置</title>
    <link rel="stylesheet" href="http://cache.amap.com/lbs/static/main1119.css"/>
    <script type="text/javascript" src="http://webapi.amap.com/maps?v=1.3&key=e3c3f60c1ceb5de69b94e06d0afdda45&plugin=AMap.Geocoder"></script>
    <script type="text/javascript" src="http://cache.amap.com/lbs/static/addToolbar.js"></script>
</head>
<body onload="regeocoder()">
<div id="container"></div>
<div id="tip">
 <%--   <b>经纬度 116.396574, 39.992706 的地理编码结果:</b>
    <span id="result"></span>--%>
</div>
<script type="text/javascript">
    var map = new AMap.Map("container", {
        resizeEnable: true,
        zoom: 18
    });
    var querys = getQueryParameters();
    var strs= new Array(); //定义一数组 
    strs = querys["id"].split(","); //字符分割 
    var lnglatXY = [strs[0],strs[1]];
    //lnglatXY = ["103.91926975","30.5636038"]; //已知点坐标
    function regeocoder() {  //逆地理编码
        var geocoder = new AMap.Geocoder({
            radius: 1000,
            extensions: "all"
        });
        geocoder.getAddress(lnglatXY, function (status, result) {
            if (status === 'complete' && result.info === 'OK') {
                geocoder_CallBack(result);
            }
        });
        var marker = new AMap.Marker({  //加点
            map: map,
            position: lnglatXY
        });
        map.setFitView();
    }
    function geocoder_CallBack(data) {
        var address = data.regeocode.formattedAddress; //返回地址描述
        document.getElementById("result").innerHTML = address;
    }
    function getQueryParameters() {
        var url = window.location.search;
        var parameters = new Object();
        if (url.indexOf("?") != -1) {
            var query = url.substr(1);
            var ary = query.split("&");
            for (var i = 0; i < ary.length; i++) {
                var keyValuePair = ary[i].split("=");
                parameters[keyValuePair[0]] = unescape(keyValuePair[1]);
            }
        }
        return parameters;
    }
</script>
</body>
</html>				