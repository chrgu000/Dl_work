/**
 * fhuaui
 * Autor: Fhua
 * Date: 17-1-20
 */

var tab;
layui.use(['layer', 'element', 'util', 'common', 'navbar', 'tab'], function () {
    var $ = layui.jquery
    , element = layui.element()
    , util = layui.util
    , common = layui.common
    , navbar = layui.navbar()
    , tab = layui.tab({
        elem: '.layui-tab-card' //设置选项卡容器
    });

    //iframe自适应
    $(window).on('resize', function () {
        var $content = $('.admin-nav-card .layui-tab-content');
        $content.height($(this).height() - 192);
        $content.find('iframe').each(function () {
            $(this).height($content.height());
        });

        var width = $(".y-role").width();
        $('#gridList,#gbox_gridList,#gview_gridList,#gridPager,.ui-jqgrid-hdiv,#ui-jqgrid-hbox').width(width - 2);
    }).resize();

    //绑定导航数据
    var $menu = $('#menu');
    $menu.find('li.layui-nav-item').each(function () {
        var $this = $(this);
        //绑定一级导航的点击事件
        $this.on('click', function () {
            //获取设置的模块ID
            var id = $this.find('a').data('fid');

            //设置数据源有两个方式。
            //第一：在此页面通过ajax读取设置  举个栗子：
            /*$.getJSON('/api/xxx',{moduleId:id},function(data){
				navbar.set({
					elem: '#side',

					data: data
				});
				navbar.render();
				navbar.on('click(side)', function(data) {
					tab.tabAdd(data.field);
				});
			});*/

            //第二：设置url
            /*navbar.set({
				elem: '#side',
				url: '/api/xxx?moduleId='+id
			});
			navbar.render();
			navbar.on('click(side)', function(data) {
				tab.tabAdd(data.field);
			});*/

            $.getJSON('js/datas/99.json', { fid: id }, function (result) {
                if (result.state == 1) {
                    //设置navbar
                    navbar.set({
                        elem: '#admin-navbar-side', //存在navbar数据的容器ID
                        data: result.data
                    });
                    //渲染navbar
                    navbar.render();

                    //添加tips		                  
                    var li = $("#sidebar-side").find("li");
                    var dd = $("#sidebar-side").find("dd");
                    li.hover(function () {
                        var minitips = $("#sidebar-side").hasClass("sidebar-mini");
                        if (minitips) {
                            var title = $(this).find("a").first().find("cite").text();
                            layer.tips(title, this, {
                                tips: 2,
                                time: 1500
                            });
                        }
                    });
                    dd.hover(function () {
                        var minitips = $("#sidebar-side").hasClass("sidebar-mini");
                        if (minitips) {
                            var title = $(this).find("a").find("cite").text();
                            layer.tips(title, this, {
                                tips: 2,
                                time: 1500
                            });
                        }
                    });
                    //监听点击事件
                    navbar.on('click(side)', function (data) {
                        tab.tabAdd(data.field);
                    });
                } else {
                    common.layerAlertE(result.message, '错误');
                }
            });
        });

    });
    //模拟点击内容管理
   $('#menu').find('a[data-fid=5]').click();

    //固定Bar
    util.fixbar({
        bar1: true
      , click: function (type) {
          if (type === 'bar1') {
              location.href = 'http://nnfhua.com';
          }
      }
    });

    //退出系统
    var adminActive = {
        doLoginOut: function () {
            var url = $(this).data('href');
            var rturl = $(this).data('rturl');
            if (url) {
                if (!rturl) {
                    rturl = '/Login/Login';
                }
                common.signOut('确认退出系统？', '请再次确认是否要退出系统！', url, rturl, 'post', 'json', {});
            }
            else {
                common.layerAlertE('链接错误！', '提示');
            }
        }
    };

    $('.do-admin').on('click', function (event) {
        var type = $(this).data('type');
        adminActive[type] ? adminActive[type].call(this) : '';
        return false;
    });

    //左侧菜单收缩
    var foldNode = $('#sidebar');
    var sidebarNode = $('#sidebar-side');
    var headerNode = $('.header-admin');
    if (foldNode) {
        $(document).on("click", '#sidebar', function () {
            var toType = sidebarNode.hasClass("sidebar-mini") ? "full" : "mini";
            var sideWidth = sidebarNode.width();
            if (sideWidth === 200) {
                $('#admin-body').animate({
                    left: '70px'
                }); //admin-footer
                $('.admin-footer').animate({
                    left: '70px'
                });
                sidebarNode.addClass('sidebar-mini');
                headerNode.addClass('header-mini');
                $('#sidebar').find('i').removeClass('fa-bars').addClass('fa-th-large');
            } else {
                $('#admin-body').animate({
                    left: '200px'
                });
                $('.admin-footer').animate({
                    left: '200px'
                });
                sidebarNode.removeClass('sidebar-mini');
                headerNode.removeClass('header-mini');
                $('#sidebar').find('i').removeClass('fa-th-large').addClass('fa-bars');
            }
        });
    }

    //open-tab
    function opentab(t) {
        $this = $(t);
        var data = {
            field: {
                href: $this.data('href'),
                icon: $this.data('icon'),
                title: $this.data('title')
            }
        }
        tab.tabAdd(data.field);
        layui.stope(e);//阻止冒泡事件
    };
})
