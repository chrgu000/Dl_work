

layui.config({
    base: '/js/'
}).use(['element', 'layer', 'navbar', 'tab'], function () {
    var element = layui.element(),
		$ = layui.jquery,
		layer = layui.layer,
		navbar = layui.navbar(),
		tab = layui.tab({
		    elem: '.admin-nav-card' //设置选项卡容器
		});

    //         var getting = {

    //         url:'Handler/ProductHandler.ashx',

    //         dataType:'json',

    //         data:{"Action":"check_one"},

    //         type:"Post",

    //         success:function(res) {

    //          console.log(res);

    // }

    // };
    var check_one = {



        url: 'Handler/ProductHandler.ashx',

        dataType: 'json',

        data: { "Action": "check_one" },

        type: "Post",

        async: false,

        success: function (data) {

            console.log(data);

            if (data.flag == '0') {

                $.ajax({
                    type: "Post",
                    url: "Handler/LoginHandler.ashx",
                    data: { "Action": "Logout" },
                    success: function (data) {
                        if (data == "ok") {
                            layer.open({

                                title: '信息',
                                content: '你的账号已在其它地点登录!<br />如非本人操作,请立即修改密码!',
                                shade: 0.95,
                                closeBtn: 0,
                                btn: ["关闭"],
                                icon: 2,
                                btn1: function () {
                                    top.window.location = "login_V2.aspx";
                                }
                            });


                        }
                    }

                })

            }

        }

    }


    //  window.setInterval(function () { $.ajax(check_one) }, 5000);

    //iframe自适应
    $(window).on('resize', function () {
        var $content = $('.admin-nav-card .layui-tab-content');
        $content.height($(this).height() - 147);
        $content.find('iframe').each(function () {
            $(this).height($content.height());
        });
    }).resize();

    //设置navbar
    navbar.set({
        spreadOne: true,
        elem: '#admin-navbar-side',
        cached: true,
        data: navs
        /*cached:true,
        url: 'datas/nav.json'*/
    });
    //渲染navbar
    navbar.render();
    //监听点击事件
    navbar.on('click(side)', function (data) {
        //console.log(data.field);
        //console.log(data);
        tab.tabAdd(data.field);
    });

    $('#ddd').on('click', function () {
        $(this).parents("li").removeClass("layui-this");
        var sideWidth = $('#admin-side').width();
        if (sideWidth === 200) {
            $("#ddd i").removeClass("fa-hand-o-left").addClass("fa-hand-o-right");
            $("#ddd p").text("显示菜单");
            $('#admin-body').animate({
                left: '0'
            }); //admin-footer
            $('#admin-footer').animate({
                left: '0'
            });
            $('#admin-side').animate({
                width: '0'
            });

        } else {
            $("#ddd i").removeClass("fa-hand-o-right").addClass("fa-hand-o-left");
            $("#ddd p").text("隐藏菜单");
            $('#admin-body').animate({
                left: '200px'
            });
            $('#admin-footer').animate({
                left: '200px'
            });
            $('#admin-side').animate({
                width: '200px'
            });
        }
    });

    $('#aaa').on('click', function () {
        if (!$("#admin-side").hasClass("sidebar-mini")) {
            $("#admin-side").addClass("sidebar-mini")
            $("#admin-body").animate({
                left: '70px'
            })
            $("#sidebar").find('i').removeClass('fa-hand-o-left')
            $("#sidebar").find('i').addClass('fa-hand-o-right')
        } else {
            $("#admin-side").removeClass("sidebar-mini")
            $("#admin-body").animate({
                left: '200px'
            })
            $("#sidebar").find('i').removeClass('fa-hand-o-right')
            $("#sidebar").find('i').addClass('fa-hand-o-left')
        }

    });

    //锁屏
    $(document).on('keydown', function () {
        var e = window.event;
        if (e.keyCode === 76 && e.altKey) {
            //alert("你按下了alt+l");
            lock($, layer);
        }
    });
    //$('#lock').on('click', function() {
    //	lock($, layer);
    //});
    $('.lock-screen').on('click', function () {
        lock($, layer);
    });

    //手机设备的简单适配
    var treeMobile = $('.site-tree-mobile'),
		shadeMobile = $('.site-mobile-shade');
    treeMobile.on('click', function () {
        $('body').addClass('site-mobile');
    });
    shadeMobile.on('click', function () {
        $('body').removeClass('site-mobile');
    });
});

function lock($, layer) {
    //自定页
    layer.open({
        title: false,
        type: 1,
        closeBtn: 0,
        anim: 6,
        content: $('#lock-temp').html(),
        shade: [0.98, '#393D49'],
        success: function (layero, lockIndex) {

            //给显示用户名赋值
            //layero.find('div#lockUserName').text('admin');
            layero.find('input[name=lockPwd]').on('focus', function () {
                var $this = $(this);
                if ($this.val() === '输入密码解锁..') {
                    $this.val('').attr('type', 'password');
                }
            })
				.on('blur', function () {
				    var $this = $(this);
				    if ($this.val() === '' || $this.length === 0) {
				        $this.attr('type', 'text').val('输入密码解锁..');
				    }
				});
            //在此处可以写一个请求到服务端删除相关身份认证，因为考虑到如果浏览器被强制刷新的时候，身份验证还存在的情况			
            //do something...
            //e.g. 
            /*
			 $.post(url,params,callback,'json');
			 */
            //绑定解锁按钮的点击事件
            layero.find('button#unlock').on('click', function () {
                var $lockBox = $('div#lock-box');

                var userName = $lockBox.find('div#lockUserName').text();
                var pwd = $lockBox.find('input[name=lockPwd]').val();
                if (pwd === '输入密码解锁..' || pwd.length === 0) {
                    layer.msg('请输入密码..', {
                        icon: 2,
                        time: 1000
                    });
                    return;
                }
                unlock(userName, pwd);
            });
            /**
			 * 解锁操作方法
			 * @param {String} 用户名
			 * @param {String} 密码
			 */
            var unlock = function (un, pwd) {
                //这里可以使用ajax方法解锁
                /*$.post('api/xx',{username:un,password:pwd}},function(data){
				 	//验证成功
					if(data.success){
						//关闭锁屏层
						layer.close(lockIndex);
					}else{
						layer.msg('密码输入错误..',{icon:2,time:1000});
					}					
				},'json');
				*/

                //演示：默认输入密码都算成功
                //关闭锁屏层
                $.ajax({
                    url: "Handler/LoginHandler.ashx",
                    type: "Post",
                    data: { "Action": "Unlock", "pass": pwd },
                    success: function (data) {
                        if (data != "ok") {
                            layer.msg('密码输入错误..', { icon: 2, time: 1000 });
                        } else {
                            layer.close(lockIndex);
                        }
                    }
                })
                //if (pwd!="test") {
                //    layer.msg('密码输入错误..', { icon: 2, time: 1000 });
                //    return false;
                //}
                //layer.close(lockIndex);
            };
        }
    });
};