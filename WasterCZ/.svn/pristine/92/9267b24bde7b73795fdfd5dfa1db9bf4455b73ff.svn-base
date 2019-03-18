/*勾选提交*/
function disable()
  {
  document.getElementById("accept").disabled=true
  }
function enable()
  {
  document.getElementById("accept").disabled=false
  }
  
  /*城市三级联动*/
  
  var Gid  = document.getElementById ;
var showArea = function(){
	Gid('show').innerHTML = "<h3>省" + Gid('s_province').value + " - 市" + 	
	Gid('s_city').value + " - 县/区" + 
	Gid('s_county').value + "</h3>"
							}
Gid('s_county').setAttribute('onchange','showArea()');

/*div在最底部*/

$(document).ready(function(){
	    var screenWidth = $(window).width();//获取屏幕可视区域的宽度。
		$(".admin-footer").width(screenWidth);//将宽度赋值给bottomDiv使其可以贯穿整个屏幕。
		var screenHeight = $(window).height();//获取屏幕可视区域的高度。
		var divHeight = $(".admin-footer").height() + 1;//bottomDiv的高度再加上它一像素的边框。
		
		$(window).scroll(function(){
			var scrollHeight = $(document).scrollTop();//获取滚动条滚动的高度。
			if(!window.XMLHttpRequest){
				$(".admin-footer").css("top",screenHeight + scrollHeight - divHeight);	
			}//判断是否为IE6，如果是，执行大括号中内容
		})
	})