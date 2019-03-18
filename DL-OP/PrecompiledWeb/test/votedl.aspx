<%@ page language="C#" autoeventwireup="true" inherits="test_votedl, dlopwebdll" enableviewstate="false" %>

<!DOCTYPE html>
<html class="html">
<head>
    <meta http-equiv="Content-type" content="text/html;charset=UTF-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes">
    <title>请为四川多联实业有限公司投票-2016-2017年度500强开发商首选品牌评选</title>
	<link href="http://voteimg.dichan.com/Css/style.css" rel="stylesheet" type="text/css" />
	<script src="http://voteimg.dichan.com/Scripts/jquery-1.8.3.min.js" type="text/javascript"></script>
	<script>
	    $(function () {
	        $(window).scroll(function () {
	            var scollTop = $(window).scrollTop();
	            if (scollTop > 10) {
	                $("#_logo").css("display", "inline-block");
	            }
	            else {
	                $("#_logo").css("display", "none");
	            }
	            if (scollTop > 160) {
	                $("#__pageTitle").css("opacity", 0.8);
	            }
	            else {
	                $("#__pageTitle").css("opacity", scollTop / 200);
	            }
	        });
	    });
	    function showMenuList() {
	        $("#__menu_List").css("display", "block");
	        $("#__menu_List_Info").css("margin-top", $(window).scrollTop());
	    }
	    function closeMenuList() {
	        $("#__menu_List").css("display", "none");
	    }
    </script>
</head>
<body>
	<div id="__menu_List" style="z-index:99999; filter: Alpha(opacity=80); -moz-opacity: 0.8;
    -khtml-opacity: 0.8; opacity: 0.8; background-color: #000; width: 100%; height: 4000px;
	 position: absolute; left: 0; top: 0;display:none">
	 <p id="__menu_List_Info"></p>
	 <p></p>
	 <p><a href="http://vote.dichan.com/Review/TestFlow.aspx">评选流程</a></p>
	 <p><a href="http://vote.dichan.com/Review/PrePrizeList.aspx">往届榜单</a></p>
	 <p><a href="http://vote.dichan.com/Review/Syndicate.aspx">评审团</a></p>
	 <p><a href="http://vote.dichan.com/index.html">返回首页</a></p>
	 <p class="_cursor" onclick="closeMenuList()" style="cursor:pointer;">关闭</p>
	</div>
	<div class="__logo">
		<a class="__logo_a" href="http://vote.dichan.com/index.html">
			<img id="_logo" src="http://voteimg.dichan.com/Images/menuLogo.png" />
		</a>
	</div>
    <div class="_main"> 
<script src="http://voteimg.dichan.com/Scripts/CompanyList.js" type="text/javascript"></script>
<script>var pageId = 1</script>
<div class="_main_Box">
    <div class="_main_companyInfo">
        <div class="_main_companyInfo_head">
            <div id="voteCount1752" class="_main_companyInfo_head_companyRecode">
                454票</div>
            <input id="company1752" type="button" class="_main_companyList_itemBottom_btn _main_companyInfo_btn" />
            </div>
    </div>
</div>
<script>$(function () { $("._main_companyList_itemBottom_btn").click(function () { var companyId = $(this).attr("id"); if (companyId) { companyId = companyId.substring(7, companyId.length); if (__a) { $.ajax({ async: false, url: "http://vote1.dichan.com/Votes/MaterialsVotes.aspx", type: "GET", dataType: 'jsonp', jsonp: 'votecallback', data: { "CompanyId": companyId, subId: pageId, "p": "F7C7C0898A8B8B4BDB51A915E5BA1346", CallBack: "voteFinishCallBack" }, timeout: 5000 }); } } }) })</script>
<!--笼罩层-->
<div id="mask" style="z-index: 800; filter: Alpha(opacity=30); -moz-opacity: 0.3;
    -khtml-opacity: 0.3; opacity: 0.3; background-color: #000; width: 100%; height: 4000px;
    position: absolute; left: 0; top: 0; display:none">
</div>
<!--笼罩层-->
<!--每日5票限制-->
<div class="__showWindow" id="toupiao" style="z-index:999;left:0px; display:none">
    <div class="__showWindow_head __showWindowAutoWidth">
        <img src="http://voteimg.dichan.com/Images/closeWindow.gif" onclick="closeWindow('toupiao')" />
    </div>
	 <div class="__showWindow_Info __showWindowAutoWidth">
		每天一个IP只限投5票!
    </div>
	<div class="__showWindow_Info __showWindowAutoWidth">
		&nbsp;
    </div>
	<div class="__showWindow_Info  __showWindowAutoWidth __height55">
		<div class="__showWindow_Info_btn" onclick="closeWindow('toupiao')">关闭</div>
    </div>
</div>
<script>
    function showWindow() {
        $("#mask").css("display", "block");
        $("#toupiao").css("display", "block");
        var _scrollHeight = $(document).scrollTop(); //获取当前窗口距离页面顶部高度 
        var _windowHeight = $(window).height(); //获取当前窗口高度 
        var height = (_scrollHeight + _windowHeight / 2 - 200);
        $("#toupiao").css('top', height + 'px');
    }
    function closeWindow(name) {
        $("#" + name).css("display", "none");
        $("#mask").css("display", "none");
    }
</script></div></body></html>