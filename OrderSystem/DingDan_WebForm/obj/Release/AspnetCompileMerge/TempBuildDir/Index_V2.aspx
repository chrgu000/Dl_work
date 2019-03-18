<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index_V2.aspx.cs" Inherits="DingDan_WebForm.Index" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <title>多联网上订单系统</title>
    <meta name="renderer" content="webkit">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <%--<meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">--%>
    <meta name="apple-mobile-web-app-status-bar-style" content="black">
    <meta name="apple-mobile-web-app-capable" content="yes">
    <meta name="format-detection" content="telephone=no">
    <!-- HTTP 1.1 -->  
<meta http-equiv="pragma" content="no-cache">  
<!-- HTTP 1.0 -->  
<meta http-equiv="cache-control" content="no-cache"> 
    <link rel="stylesheet" href="../js/plugins/layui/css/layui.css" media="all" />
    <link rel="stylesheet" href="../css/global.css" media="all">
    <link rel="stylesheet" href="../js/plugins/font-awesome/css/font-awesome.min.css">
    <link rel="stylesheet" href="../css/site.css">
    <link href="css/fhuaui.css" rel="stylesheet" />
    <style>
        .layui-this {
            background-color: none;
        }
    </style>
</head>

<body>
    <div class="layui-layout layui-layout-admin">
        <div class="layui-header header header-demo">
            <div class="layui-main">
                <div class="admin-login-box" style="float: left">

                    <img src="/images/dl_logo2.png" alt="" style="height: 90%;">
                </div>
               
                <div style="float: left; margin-top: 15px" class="logo-fo">
                    <p style="font-size: 18px; color: yellow;">
                        塑胶管道&nbsp;&nbsp;|&nbsp;&nbsp;开关插座&nbsp;&nbsp;|&nbsp;&nbsp;电线&nbsp;&nbsp;|&nbsp;&nbsp;卫浴洁具
                    <p>
                    <p style="font-size: 10px;">
                        品质水电建材 集成服务商
                        <a style="color: #fff" href="http://www.duolian.com">&nbsp;&nbsp; <i class="fa fa-clone" aria-hidden="true"></i>
                            &nbsp;
                            www.duolian.com
                        </a>
                        &nbsp;
                        <i class="fa fa-phone" aria-hidden="true"></i>
                        &nbsp;
                        400-8786-333
                    </p>
                </div>
                 <div class="admin-side-toggle" id="ddd">
                  <i class="fa  fa-hand-o-left fa-lg" aria-hidden="true"> </i> 
                      <p>隐藏菜单</p>	
				 </div>
                <div style="float:left;margin:10px auto auto  200px;">
                    <p> <asp:Label   runat="server" id="type"/></p>
                 <p> <asp:Label  runat="server" id="server"/></p>
                      <p> <asp:Label   runat="server" id="database"/></p>
                 
                </div>
                <ul class="layui-nav admin-header-item">
                    <li class="layui-nav-item">
                        <div style="padding-top: 20px;">
                            <%-- <script charset="utf-8" type="text/javascript" src="http://wpa.b.qq.com/cgi/wpa.php?key=XzkzODAyMTM0OV8zNTY4OTdfNDAwODc4NjMzM18"></script>--%>
                            <script charset="utf-8" type="text/javascript" src="../js/wpaqq.js"></script>
                        </div>
                    </li>
                    <li class="layui-nav-item">
                        <a href="javascript:;" class="admin-header-user">
                            <img src="/images/0.jpg" />
                            <asp:Label Text="text" runat="server" ID="Login_Name" />
                            <%--<span>beginner</span>--%>
                        </a>
                        <dl class="layui-nav-child">
                            <dd>
                                <a href="javascript:;">
                                    <i class="fa fa-user-circle" aria-hidden="true"></i>
                                    个人信息
                                </a>
                            </dd>
                            <dd>
                                <a href="javascript:;" class="change_pwd">
                                    <i class="fa fa-gear" aria-hidden="true"></i>
                                    修改密码
                                </a>
                            </dd>
                            <dd id="lock1" class="lock-screen">
                                <a href="javascript:;">
                                    <i class="fa fa-lock" aria-hidden="true" style="padding-right: 3px; padding-left: 1px;"></i>
                                    锁屏 (Alt+L)
                                </a>
                            </dd>
                            <dd>
                                <a href="javascript:;" onclick="Logout();">
                                    <i class="fa fa-sign-out" aria-hidden="true"></i>
                                    注销
                                </a>
                            </dd>
                        </dl>
                    </li>
                </ul>
                <ul class="layui-nav admin-header-item-mobile">
                    <li class="layui-nav-item">
                        <a href="javascript:;" class="admin-header-user">
                            <img src="/images/0.jpg" />
                            <asp:Label Text="text" runat="server" ID="Login_Name_Mobile" />
                        </a>
                        <dl class="layui-nav-child">
                            <dd>
                                <a href="javascript:;">
                                    <i class="fa fa-user-circle" aria-hidden="true"></i>
                                    个人信息
                                </a>
                            </dd>
                            <dd>
                                <a href="javascript:;" class="change_pwd">
                                    <i class="fa fa-gear" aria-hidden="true"></i>
                                    修改密码
                                </a>
                            </dd>
                            <dd id="lock" class="lock-screen">
                                <a href="javascript:;">
                                    <i class="fa fa-lock" aria-hidden="true" style="padding-right: 3px; padding-left: 1px;"></i>
                                    锁屏 (Alt+L)
                                </a>
                            </dd>
                            <dd>
                                <a href="javascript:;" onclick="Logout();">
                                    <i class="fa fa-sign-out" aria-hidden="true"></i>
                                    注销
                                </a>
                            </dd>
                        </dl>
                    </li>
                </ul>
            </div>
        </div>
        <div class="layui-side layui-bg-black" id="admin-side">
           
            <div class="layui-side-scroll" id="admin-navbar-side" lay-filter="side">
            </div>
        </div>
     
        <div class="layui-body" style="bottom: 0; border-left: solid 4px #0170BA; top: 65px" id="admin-body">
     
            <div class="layui-tab admin-nav-card layui-tab-brief" lay-filter="admin-tab">
                <ul class="layui-tab-title" style="border: 1px solid #eee; border-bottom: 5px solid #2C95D3;">
                    <li class="layui-this">
                        <i class="fa fa-dashboard" aria-hidden="true"></i>
                        <cite>系统主页</cite>
                    </li>
                </ul>
                <div class="layui-tab-content" style="overflow-y: no-content; padding: 0 0 0 0;">
                    <div class="layui-tab-item layui-show">
                        <iframe src="../html/Center.html?v=223"></iframe>

                        <%--<iframe src="http://192.168.0.249:8002/other/SingleLoginCheck.aspx?<%=str %>" ></iframe>--%>
                    </div>
                </div>
            </div>
        </div>
        <div class="layui-footer footer footer-demo" id="admin-footer">
            <div class="layui-main">
                <p>
                    Copyright 2016 Powered by
                    <a href="http://www.duolian.com">DuoLian</a>
                </p>
            </div>
        </div>
        <div class="site-tree-mobile layui-hide">
            <i class="layui-icon">&#xe602;</i>
        </div>
        <div class="site-mobile-shade"></div>

        <!--锁屏模板 start-->
        <script type="text/template" id="lock-temp">
            <div class="admin-header-lock" id="lock-box">
                <div class="admin-header-lock-img">
                    <img src="/images/0.jpg" />
                </div>
                <div class="admin-header-lock-name" id="lockUserName">
                    <asp:Label Text="text" runat="server" ID="Lock_Name" />
                </div>
                <input type="text" class="admin-header-lock-input" value="输入密码解锁.." name="lockPwd" id="lockPwd" />
                <button class="layui-btn layui-btn-small" id="unlock">解锁</button>
            </div>
        </script>
        <!--锁屏模板 end -->
        <script src="/js/jquery-1.11.0.min.js"></script>
        <script type="text/javascript" src="/js/plugins/layui/layui.js"></script>
                <script type="text/javascript">
                    var a = Date.parse(new Date());
                    document.write(' <script type="text/javascript"  charset="utf-8" src="\/js\/datas\/nav.js?v=' + a + '"><\/script>');
</script>
        <%--<script type="text/javascript" src="/js/datas/nav.js"></script>--%>
        <script src="/js/index.js?v=12"></script>
        <script src="/js/Scripts/jquery.signalR-2.2.2.min.js" type="text/javascript"></script>
        <script src="/Signalr/Hubs"></script>
        <script src="/js/SignalR/SignalR.js?v=12"></script>

        <script>
            layui.use('layer', function () {

                var layer = layui.layer;
                signalRInit();

            });
            function exit() {
                $.ajax({
                    type: "Post",
                    url: "Handler/LoginHandler.ashx",
                    data: { "Action": "Logout" },
                    success: function (data) {
                        if (data == "ok") {
                            top.window.location = "login_V2.html";
                        }
                    }
                })
            }

            function exit_login() {
                $.ajax({
                    type: "Post",
                    url: "Handler/LoginHandler.ashx",
                    data: { "Action": "Logout" }
                   
                })
            }

            function Logout() {
                layer.confirm("确定要退出？", function () {
                    exit();
                })
            }

            $(".change_pwd").click(function () {
                layer.open({
                    type: 1
                   , title: '修改密码'
                   , shade: 0.9
                   , resize: false
                   , area: ['300px', '300px']
                   , content: $("#change_div").html()

                })
            })

            $(document).on("click", "#change_btn", function () {
                var pwd_ini = $(this).parents("form").find("#pwd_ini").val().trim();
                var pwd_fir = $(this).parents("form").find("#pwd_fir").val().trim();
                var pwd_sen = $(this).parents("form").find("#pwd_sen").val().trim();
                if (pwd_ini.length == 0) {
                    layer.msg("初始密码不能为空!", { icon: 2, time: 1000 });
                    return false;
                }
                if (pwd_fir.length == 0) {
                    layer.msg("修改密码不能为空!", { icon: 2, time: 1000 });
                    return false;
                }
                if (pwd_sen.length == 0) {
                    layer.msg("确认密码不能为空!", { icon: 2, time: 1000 });
                    return false;
                }
                if (pwd_fir !== pwd_sen) {
                    layer.msg("两次密码输入不一致,请重新输入!", { icon: 2, time: 1000 });
                    return false;
                }
                $.ajax({
                    url: "Handler/LoginHandler.ashx",
                    dataType: "Json",
                    type: "Post",
                    data: { "Action": "ChangePwd", "pwd_ini": pwd_ini, "pwd_fir": pwd_fir, "pwd_sen": pwd_sen },
                    success: function (data) {
                        if (data.flag == '0') {
                            layer.alert(data.message, { icon: 5 });
                            return false;
                        }
                        if (data.flag == '1') {
                            layer.alert(data.message, { icon: 6 }, function () {
                                layer.closeAll();
                            });
                        }
                    }

                })
            })
        </script>

    </div>
</body>
<div id="change_div" style="display: none;">
    <form class="layui-form layui-form-pane" style="margin: 20px 10px auto 10px">
        <div class="layui-form-item">
            <label class="layui-form-label">原始密码</label>
            <div class="layui-input-block">
                <input type="password" id="pwd_ini" name="pwd_ini" autocomplete="off" placeholder="请输入原始密码" class="layui-input">
            </div>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">修改密码</label>
            <div class="layui-input-block">
                <input type="password" id="pwd_fir" autocomplete="off" placeholder="请输入修改密码" class="layui-input">
            </div>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">确认密码</label>
            <div class="layui-input-block">
                <input type="password" id="pwd_sen" autocomplete="off" placeholder="请输入确认密码" class="layui-input">
            </div>
        </div>
        <br>
        <div class="layui-form-item" style="text-align: center;">
            <input class="layui-btn layui-btn-normal" type="button" value="修改密码" style="width: 40%" id="change_btn" />
            <!-- <input class="layui-btn-primary layui-btn" type="button" value="关闭"  style="width:40%" id="btn2"/> -->
        </div>
    </form>
</div>
</html>
