  var Inbox={
	alert1:function(){
		mdui.snackbar({
  message: 'Inbox alert1'
});
	},
		alert2:function(){
		mdui.snackbar({
  message: 'Inbox alert2'
});
	},

	init:function(){
		this.alert1();
		 var inst = new mdui.Collapse('#Inbox_collapse',{accordion:true});

		$('#Inbox_panel p').click(function(){
		 
			$(this).addClass('mdui-text-color-theme-accent').siblings().removeClass('mdui-text-color-theme-accent')
		}) 
	}

}
 
 if (!pj['Inbox'] ) {
 	pj['Inbox']=Inbox;

 }



// (function(){
// 	var Inbox={
// 	alert1:function(){
// 		mdui.snackbar({
//   message: 'Inbox alert1'
// });
// 	},
// 		alert2:function(){
// 		mdui.snackbar({
//   message: 'Inbox alert2'
// });
// 	},
// 	init:function(){
// 		this.alert1();
// 	}
// }
// })();