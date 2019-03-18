 
 

 var Starred={
 	 
	alert1:function(){
		mdui.snackbar({
  message: 'Starred alert1'
});
	},
	load1:function(){
		$('#Starred_div li')[1].innerText=new Date();
	},
		alert2:function(){
		mdui.snackbar({
  message: 'Starred alert2'
});
	},
	init:function(){
		this.alert1();
		$('#Starred_div li').click(function(){
				$(this).addClass('mdui-color-red').siblings().removeClass('mdui-color-red');
			console.log(this)
		});
		$('#Starred_btn').click(function(){
			Starred.load1();
			console.log(this)
		})
 
	}
}
 
 if (!pj['Starred'] ) {
 	pj['Starred']=Starred;
 	console.log(new Date())
 }