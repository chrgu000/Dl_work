var navs = [
 
	{
	    "title": "系统主页",
	    "icon": "fa-stop-circle",
	    "href": " ",
	    "spread": false
	},
    {
        "title": "购物",
        "icon": "fa-github",
        "spread": true,
        "children": [{
            "title": "新增普通订单",
            "icon": "&#xe641;",
            "href": "html/buy.html?v=3"
        }, {
            "title": "新增样品资料订单",
            "icon": "&#xe63c;",
            "href": "html/buy_sample.html?v=3"
        },   {
            "title": " 参照特殊订单",
            "icon": "&#xe609;",
            "href": "html/Buy_Special.html?v=3"
        }]
    },
	{
	    "title": "订单管理",
	    "icon": "fa-cubes",
	    "spread": true,
	    "children": [ {
	        "title": "订单查询",
	        "icon": "&#xe62c;",
	        "href": "html/AllOrders.html"
	    }, {
	        "title": "订单重量查询",
	        "icon": "fa-hand-peace-o",
	        "href": "html/orderweightnew.html"
	    }, {
	        "title": "待审核订单",
	        "icon": "&#xe638;",
	        "href": "html/CheckPendingOrder.html"
	    }, {
	        "title": "被驳回订单",
	        "icon": "&#xe622;",
	        "href": "html/Reject_Order.html"
	    }
        ,
         {
             "title": "更改收货方式",
             "icon": "&#xe630;",
             "href": "html/Modify_shippingMethod.html?v=1"
         }, {
	        "title": "待确认订单",
	        "icon": "&#xe64c;",
	        "href": "html/OrderConfirm.html"
	    }]
	}, {
	    "title": "产品需求",
	    "icon": "fa-calendar-plus-o",
	    "spread": false,
	    "children": [{
	        "title": "新增需求订单",
	        "icon": "fa-heartbeat",
	        "href": "html/buy_XOrder.html"
	    },
	    {
	        "title": "需求订单列表",
	        "icon": "fa-list-alt",
	        "href": "html/XOrderList.html"
	    }]
	},
    {
	    "title": "预订订单",
	    "icon": "&#x1002;",
	    "spread": false,
	    "children": [{
	        "title": "新增预订订单",
	        "icon": "&#xe62a",
	        "href": "html/XPreOrder.html?v=1"
	    },
	    {
		"title": "预订订单查询",
		"icon": "&#xe628",
		"href": "html/PreXOrderSearch.html"
	}]
	},
{
    "title": "报表",
    "icon": "fa-address-book",
    "href": "",
    "spread": false,
    "children": [{
        "title": "我的对账单",
        "icon": "fa-github",
        "href": "html/SOA.html"
    },
    {
        "title": "延期通知单",
        "icon": "fa-window-restore",
        "href": "html/arrear.html"
    },
    {
        "title": "账单明细",
        "icon": "fa-hand-lizard-o",
        "href": "html/SOADetail.html"
    }, {
        "title": "订单执行情况",
        "icon": "fa-hand-peace-o",
        "href": "html/OrderExecute.html"
    }
    //  , {
    //    "title": "客户反馈",
    //    "icon": "&#xe609;",
    //    "href": "html/feedback.html"
    //}
    //, {
    //    "title": "预约",
    //    "icon": "&#xe609;",
    //    "href": "html/maa.html"
    //}
    // , {
    //     "title": "预约列表",
    //     "icon": "&#xe609;",
    //     "href": "html/maalist.html"
    // }
    //  , {
    //      "title": "特殊预约",
    //      "icon": "&#xe609;",
    //      "href": "html/maa_special.html"
    //  }
    // , {
    //     "title": "特殊预约列表",
    //     "icon": "&#xe609;",
    //     "href": "html/maalist_special.html"
    // }
    ]
},{
    "title": "我的账户",
    "icon": "fa-user-o",
	"href": "",
	"spread": false,
	"children": [ {
	    "title": "收货地址",
	    "icon": "fa-weibo",
	    "href": "html/User_Address.html?v=3"
	}
    , {
       "title": "客户编码",
        "icon": "fa-cubes",
        "href": "html/CodeConfig.html"  
    }, {
        "title": "信用列表",
        "icon": "fa-bug",
        "href": "html/XOrderBehaviorList.html"
    }
	]
}

];