<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wx.aspx.cs" Inherits="DingDan_WebForm.test.wx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <%--<h1>WXtest</h1>--%>
        </div>
    </form>
        <script src="../SuperAdmin/js/jquery-2.2.4.min.js"></script>

    <script src="http://res.wx.qq.com/open/js/jweixin-1.2.0.js"></script>
    <script>
        $(function () {
            wx.config({
                beta: true,// 必须这么写，否则在微信插件有些jsapi会有问题
                debug: 1, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
                appId: 'wx85ee38394e42f0b7', // 必填，企业微信的cropID
                timestamp: 1507598027, // 必填，生成签名的时间戳
                nonceStr: 'ABC', // 必填，生成签名的随机串
                signature: 'kgt8ON7yVITDhtdwci0qeSl6AsXXpNFg1lns-L42OyPQ7GERLdHdefl90X3AWvfaRqbpkuF8eU7VIIToQCxeOw',// 必填，签名，见[附录1](#11974)
                jsApiList: [
                 'checkJsApi',
         'onMenuShareTimeline',
         'onMenuShareAppMessage',
         'onMenuShareQQ',
         'onMenuShareWeibo',
         'hideMenuItems',
         'showMenuItems',
         'hideAllNonBaseMenuItem',
         'showAllNonBaseMenuItem',
         'translateVoice',
         'startRecord',
         'stopRecord',
         'onRecordEnd',
         'playVoice',
         'pauseVoice',
         'stopVoice',
         'uploadVoice'
                ] // 必填，需要使用的JS接口列表，所有JS接口列表见附录2
            });
            wx.ready(function () {
                console.log("1")
            })


            wx.error(function (res) {
                console.log(res)
            })

            wx.checkJsApi({
                jsApiList: ['chooseImage'], // 需要检测的JS接口列表，所有JS接口列表见附录2,
                success: function (res) {
                    console.log(res)
                    // 以键值对的形式返回，可用的api值true，不可用为false
                    // 如：{"checkResult":{"chooseImage":true},"errMsg":"checkJsApi:ok"}
                }
            });

        })




    </script>
</body>
</html>
