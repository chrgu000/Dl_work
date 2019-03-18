
//ajax分页扩展模块
//Autor:mastes Beginner / Fhua
//Date: 16-11-25
layui.define(['layer', 'laypage', 'laytpl', 'icheck'], function (exports) {
    "use strict";
    var $ = layui.jquery,
		layer = layui.layer,
		laytpl = layui.laytpl;

    var Paging = function () {
        this.config = {
            url: undefined,  //数据远程地址
            type: 'get',     //数据的获取方式  get or post
            elem: undefined, //内容容器
            params: null,
            tempElem: undefined, //模板容器
            paged: true,
            pageConfig: { //参数应该为object类型
                elem: undefined,
                pageSize: 15 //分页大小
            },
            success: undefined, //type:function
            fail: undefined, //type:function
            complate: undefined, //type:function
            serverError: function (xhr, status, error) {  //ajax的服务错误
                throwError("错误提示： " + xhr.status + " " + xhr.statusText);
            }
        };
    };
    /**
	 * 设置
	 * @param {Object} options
	 */
    Paging.prototype.set = function (options) {
        var that = this;
        $.extend(true, that.config, options);
        return that;
    };
    /**
	 * 初始化
	 * @param {Object} options
	 */
    Paging.prototype.init = function (options) {
        var that = this;
        $.extend(true, that.config, options);
        var _config = that.config;
        //var parent = $(_config.elem).parent();
        //if (parent === undefined) {
        //    throwError('Paging Error:请配置table父容器!');
        //}
        //else {
        //    //赋值远程URL;
        //    _config.url = parent.data("pageurl");
        //    _config.type = parent.data("type");
        //    _config.pageConfig.pageSize = parent.data("pagesize");
        //}
        if (_config.elem === undefined) {
            throwError('Paging Error:请配置参数elem!');
        }
        if ($(_config.elem).length === 0) {
            throwError('Paging Error:找不到配置的容器elem!');
        }
        if (_config.tempElem === undefined) {
            throwError('Paging Error:请配置参数tempElem!');
        }
        if ($(_config.tempElem).length === 0) {
            throwError('Paging Error:找不到配置的容器tempElem!');
        }
        if (_config.paged) {
            var _pageConfig = _config.pageConfig;
            if (_pageConfig.elem === undefined) {
                throwError('Paging Error:请配置参数pageConfig.elem!');
            }
        }
        if (_config.url === undefined) {
            throwError('Paging Error:请配置远程URL!');
        }
        if (_config.type.toUpperCase() !== 'GET' && _config.type.toUpperCase() !== 'POST') {
            throwError('Paging Error:type参数配置出错，只支持GET或都POST');
        }
        that.get({
            page: 1,
            pageSize: _config.pageConfig.pageSize
        });

        return that;
    };
    /**
	 * 获取数据
	 * @param {Object} options
	 */
    Paging.prototype.get = function (options) {
        var that = this;
        var _config = that.config;
        //默认参数
        var uparams = {
            page: 1,
            pageSize: _config.pageConfig.pageSize
        };
        $.extend(true, uparams, options, _config.params);
        $.ajax({
            type: _config.type,
            url: _config.url,
            data: uparams,
            dataType: 'json',
            success: function (result, status, xhr) {
                if (result.code === 1) {
                    //获取模板
                    var tpl = $(_config.tempElem).html();
                    //渲染数据
                    laytpl(tpl).render(eval(result.list), function (html) {
                        $(_config.elem).html(html);
                        //加载单选框样式
                        $('input').iCheck({
                            checkboxClass: 'icheckbox_square-green',
                            radioClass: 'iradio_square-green'
                        });

                        //表格行点击勾选
                        $('.layui-table tbody tr').on('click', function () {
                            var $this = $(this);
                            var $input = $this.children('td').eq(0).find('input');
                            $input.on('ifChecked', function (e) {
                                $this.css('background-color', '#EEEEEE');
                            });
                            $input.on('ifUnchecked', function (e) {
                                $this.removeAttr('style');
                            });
                            $input.iCheck('toggle');
                        }).find('input').each(function () {
                            var $this = $(this);
                            $this.on('ifChecked', function (e) {
                                $this.parents('tr').css('background-color', '#EEEEEE');
                            });
                            $this.on('ifUnchecked', function (e) {
                                $this.parents('tr').removeAttr('style');
                            });
                        });
                        //全选
                        $('#selected-all').on('ifChanged', function (event) {
                            var $input = $('.layui-table tbody tr td').find('input');
                            $input.iCheck(event.currentTarget.checked ? 'check' : 'uncheck');
                        });
                    });
                    if (_config.paged) {
                        if (result.count === null || result.count === 0) {
                            throwError('Paging Error:请返回数据总数！');
                            return;
                        }
                        var _pageConfig = _config.pageConfig;
                        var pageSize = _pageConfig.pageSize;
                        var pages = result.count % pageSize == 0 ?
							(result.count / pageSize) : (result.count / pageSize + 1);

                        var defaults = {
                            cont: $(_pageConfig.elem),
                            curr: uparams.page,
                            pages: pages,
                            jump: function (obj, first) {
                                //得到了当前页，用于向服务端请求对应数据
                                var curr = obj.curr;
                                if (!first) {
                                    that.get({
                                        page: curr,
                                        pageSize: pageSize
                                    });
                                }
                            }
                        };
                        $.extend(defaults, _pageConfig);
                        layui.laypage(defaults);
                    }
                    _config.success(); //渲染成功
                } else {
                    _config.fail(result.msg);//获取数据失败
                }
                _config.complate();//渲染完成
            },
            error: function (xhr, status, error) {
                _config.serverError(xhr, status, error);//服务器错误
            }
        });
    };
    /**
	 * 抛出一个异常错误信息
	 * @param {String} msg
	 */
    function throwError(msg) {
        throw new Error(msg);
        return;
    };

    var paging = new Paging();
    exports('paging', function (options) {
        return paging.set(options);
    });
});