var $ = layui.jquery, layer = layui.layer;
$(function () {
	    $.ajax({
         url:"/handler/AdminHandler.ashx",
            type:"Post",
            dataType:"Json",
            data:{"Action":"SuperAdminIndex"},
            success:function(res){
                if (res.flag!=1) {
                    layer.alert(res.message, { icon: 2 ,closeBtn:0}, function () {
                        window.parent.location='/login_v2.html'
                    })
                	return false;
                }
                $("#feedback span").text(res.unreplay);

               $("#sqllock span").text(res.sqllock);
            }
    })
})