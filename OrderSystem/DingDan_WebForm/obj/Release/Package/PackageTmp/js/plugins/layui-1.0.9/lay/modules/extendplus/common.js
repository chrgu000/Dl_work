/**
 * layui 扩展公共模块
 * Autor: Fhua
 * Date: 16-11-25
 */

layui.define(['layer'], function (exports) {
    var $ = layui.jquery;

    var obj = {
        ajax: function (url, type, dataType, data, callback) {
            $.ajax({
                url: url,
                type: type,
                dataType: dataType,
                data: data,
                success: function (data, startic) {
                    if (data.state == 1) {
                        location.href = location.href;
                        obj.layerAlertS(data.message, '提示');
                    }
                    else {
                        obj.layerAlertE(data.message, '提示');
                    }
                },
                error: function () {

                }
            });
        },
        layerDel: function (title, text, url, type, dataType, data, callback) {
            parent.layer.confirm(text, {
                title: title,
                btnAlign: 'c',
                resize: false,
                icon: 3,
                btn: ['确定删除', '容我想想'],
                yes: function () {
                    obj.ajax(url, type, dataType, data, callback);
                }
            });
        },
        /**
		 * 抛出一个异常错误信息
		 * @param {String} msg
		 */
        throwError: function (msg) {
            throw new Error(msg);
            return;
        },
        //成功提示
        layerAlertS: function (text, title) {
            parent.layer.alert(text, { title: title, icon: 1, time: 5000, resize: false, zIndex: layer.zIndex, anim: Math.ceil(Math.random() * 6) });
        },
        //错误提示
        layerAlertE: function (text, title) {
            parent.layer.alert(text, { title: title, icon: 2, time: 5000, resize: false, zIndex: layer.zIndex, anim: Math.ceil(Math.random() * 6) });
        },
        //信息提示
        layerAlertI: function (text) {
            parent.layer.alert(text, { time: 5000, resize: false, zIndex: layer.zIndex, anim: Math.ceil(Math.random() * 6) });
            return;
        },
        layerPrompt: function () {
        },
        //询问层
        layerConfirm: function () {
        },
        //退出系统
        signOut: function (title, text, url,rturl,type, dataType, data, callback) {
            parent.layer.confirm(text, {
                title: title,
                resize: false,
                btn: ['确定退出', '容我想想'],
                btnAlign: 'c',
                icon: 3

            }, function () {
                $.ajax({
                    url: url,
                    type: type,
                    dataType: dataType,
                    data: data,
                    success: function (data, startic) {
                        if (data.state == 47) {                           
                            location.href = rturl;
                            obj.layerAlertS(data.message, '提示');
                        }
                        else {
                            obj.layerAlertE(data.message, '提示');
                        }
                    },
                    error: function () {

                    }
                });
            }, function () {
                layer.msg('也可以这样！', {
                    time: 20000, //20s后自动关闭
                    btnAlign: 'c',
                    btn: ['明白了']
                });
            });
        }

    }

    exports("common", obj);
});
