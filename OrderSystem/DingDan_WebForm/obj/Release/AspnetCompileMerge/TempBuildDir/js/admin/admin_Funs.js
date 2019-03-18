layui.use(['util', 'laydate', 'form'], function() {

    var form = layui.form,
        layer = layui.layer,
        laydate = layui.laydate;

    $(document).ajaxStart(function() {
        layer.load();
    }).ajaxComplete(function(request, status) {
        layer.closeAll('loading');
    }).ajaxError(function(err) {
        console.log(err)
        layer.alert("加载页面出错，请联系管理员！", {
            icon: 2
        });
        layer.closeAll('loading');
    });


 

})



//Bootstrap-Table 初始化参数
var TableInit = function(columns, tableId) {
    console.log('TableInit')
    var oTableInit = new Object();
    var d = []
        //初始化Table
    oTableInit.Init = function() {
        $('#' + tableId).bootstrapTable({
            data: d, //请求后台的URL（*）
            //  method: 'get',                      //请求方式（*）
            toolbar: '#toolbar', //工具按钮用哪个容器
            striped: true, //是否显示行间隔色
            cache: false, //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            pagination: true, //是否显示分页（*）
            sortable: false, //是否启用排序
            sortOrder: "asc", //排序方式
            queryParams: oTableInit.queryParams, //传递参数（*）
            sidePagination: "client", //分页方式：client客户端分页，server服务端分页（*）
            pageNumber: 1, //初始化加载第一页，默认第一页
            pageSize: 50, //每页的记录行数（*）
            pageList: [50, 100, 200, 'All'], //可供选择的每页的行数（*）
            showExport: true, //是否显示导出
            exportDataType: "all", //basic', 'all', 'selected'.
            exportTypes: ["excel"],
            //  checkboxHeader: true,
            showColumns: true, //是否显示所有的列
            minimumCountColumns: 2, //最少允许的列数
            undefinedText: "", //当数据为 undefined 时显示的字符
            clickToSelect: true, //点击选中行
            // showRefresh:true,                    //显示刷新按钮
            //cardView:true,
            // showToggle: true,                          //切换卡模式
            // detailView: true,                       //详细页面模式
            height: 600,
            search: true,
            columns: columns

        });
    };


    return oTableInit;
};



//小写金额转化大写金额
function AmountLtoU(num) {
    if (isNaN(num)) return "无效数值！";
    var strPrefix = "";
    if (num < 0) strPrefix = "(负)";
    num = Math.abs(num);
    if (num >= 1000000000000) return "无效数值！";
    var strOutput = "";
    var strUnit = '仟佰拾亿仟佰拾万仟佰拾元角分';
    var strCapDgt = '零壹贰叁肆伍陆柒捌玖';
    num += "00";
    var intPos = num.indexOf('.');
    if (intPos >= 0) {
        num = num.substring(0, intPos) + num.substr(intPos + 1, 2);
    }
    strUnit = strUnit.substr(strUnit.length - num.length);
    for (var i = 0; i < num.length; i++) {
        strOutput += strCapDgt.substr(num.substr(i, 1), 1) + strUnit.substr(i, 1);
    }
    return strPrefix + strOutput.replace(/零角零分$/, '整').replace(/零[仟佰拾]/g, '零').replace(/零{2,}/g, '零').replace(/零([亿|万])/g, '$1').replace(/零+元/, '元').replace(/亿零{0,3}万/, '亿').replace(/^元/, "零元");
};


//提交延期结款情况通知单时验证
function Check_Arrear() {
    var $detail = $("#detail");
    if ($("#cCusName").val().trim() == "") {
        layer.alert("主客户不能为空！", {
            icon: 2
        })
        return false;
    }
    if ($("#cCusCode").val().trim() == "") {
        layer.alert("主客户不正确！", {
            icon: 2
        })
        return false;
    }
       if ($("#cCusCode").val().trim().length != 6) {
        layer.alert("主客户只能为六位账号！", {
            icon: 2
        })
        return false;
    }
    if ($("#dAccountDay").val().trim() == "") {
        layer.alert("结款日期不能为空！", {
            icon: 2
        })
        return false;
    }

    if ($("#dArrearsCycleStart").val().trim() == "" || $("#dArrearsCycleEnd").val().trim() == "") {
        layer.alert("欠款周期不能为空！", {
            icon: 2
        })
        return false;
    }
    if ($("#dArrearsCycleStart").val().trim() > $("#dArrearsCycleEnd").val().trim()) {
        layer.alert("欠款周期结束日期不能大于开始日期！", {
            icon: 2
        })
        return false;
    }

    var b = true;
    $.each($detail.find(".cSubCusCode"), function(i, v) {

        if ($(v).val().trim() == "") {
            layer.alert("有子账户账单不正确！", {
                icon: 2
            })
            b = false;
            return false;
        }
    })
    if (!b) {
        return false;
    }
    $.each($detail.find(".cSubCusName"), function(i, v) {
        if ($(v).text() == "未查询到该客户") {
            layer.alert("有子账户账单不正确！", {
                icon: 2
            })
            b = false;
            return false;
        }
    })
    if (!b) {
        return false;
    }
    $.each($detail.find(".iSumSubCusCode"), function(i, v) {
        if ($(v).val().trim() == "" || $(v).val().trim() == 0) {
            layer.alert("有子账户账单不正确！", {
                icon: 2
            })
            b = false;
            return false;
        }
    })
    if (!b) {
        return false;
    }

    if ((Number($("#iSumArrears").text().trim()) - Number($("#iSumPreviousUncleared").val().trim()) - Number($("#iSumUncleared").val().trim())).toFixed(2) != 0) {
        //console.log(Number($("#iSumArrears").text().trim()).toFixed(2) - Number($("#iSumPreviousUncleared").val().trim()).toFixed(2) - Number($("#iSumUncleared").val().trim()).toFixed(2));
        layer.alert("总合计金额不正确！", {
            icon: 2
        })
        return false;
    }

    if ((Number($("#iSumPreviousUncleared").val().trim()) - Number($("#iPreviousOneMonthMoney").val().trim()) - Number($("#iPreviousTwoMonthMoney").val().trim()) - Number($("#iPreviousThreeMonthMoney").val().trim())).toFixed(2) != 0) {
        layer.alert("以前年度合计金额不正确！", {
            icon: 2
        })
        return false;
    }
    if ((Number($("#iSumUncleared").val().trim()) - Number($("#iOneMonthMoney").val().trim()) - Number($("#iTwoMonthMoney").val().trim()) - Number($("#iThreeMonthMoney").val().trim())).toFixed(2) != 0) {
        layer.alert("本年度合计金额不正确！", {
            icon: 2
        })
        return false;
    }

    return true;
}

////提交前拼接表头数据
//function Get_Arrear_Head() {
//    var o = {};
//    o.cCusCode = $("#cCusCode").val().trim() ;
//    o.cCusName = $("#cCusName").val().trim();
//    o.dAccountDay = $("#dAccountDay").val().trim();
//    o.dArrearsCycleStart = $("#dArrearsCycleStart").val().trim();
//    o.dArrearsCycleEnd = $("#dArrearsCycleEnd").val().trim();
//    o.iSumArrears = $("#iSumArrears").text().trim();
//    o.iSumArrearsCapital = $("#iSumArrearsCapital").text().trim();
//    o.iSumPreviousUncleared = $("#iSumPreviousUncleared").val().trim();
//    o.iSumUncleared = $("#iSumUncleared").text();
//    o.iPreviousOneMonthMoney = $("#iPreviousOneMonthMoney").val().trim();
//    o.iPreviousTwoMonthMoney = $("#iPreviousTwoMonthMoney").val().trim();
//    o.iPreviousThreeMonthMoney = $("#iPreviousThreeMonthMoney").val().trim();
//    o.iPreviousOneMonthLiquidatedDamages = $("#iPreviousOneMonthLiquidatedDamages").val().trim();
//    o.iPreviousTwoMonthLiquidatedDamages = $("#iPreviousTwoMonthLiquidatedDamages").val().trim();
//    o.iPreviousThreeMonthLiquidatedDamages = $("#iPreviousThreeMonthLiquidatedDamages").val().trim();
//    o.iSumPreviousLiquidatedDamages = $("#iSumPreviousLiquidatedDamages").text();
//    o.iSumPreviousLiquidatedDamagesCapital = $("#iSumPreviousLiquidatedDamagesCapital").text().trim();
//    o.iOneMonthMoney = $("#iOneMonthMoney").val().trim();
//    o.iTwoMonthMoney = $("#iTwoMonthMoney").val().trim();
//    o.iThreeMonthMoney = $("#iThreeMonthMoney").val().trim();
//    o.iOneMonthLiquidatedDamages = $("#iOneMonthLiquidatedDamages").val().trim();
//    o.iTwoMonthLiquidatedDamages = $("#iTwoMonthLiquidatedDamages").val().trim();
//    o.iThreeMonthLiquidatedDamages = $("#iThreeMonthLiquidatedDamages").val().trim();
//    o.iSumLiquidatedDamages = $("#iSumLiquidatedDamages").val().trim();
//    o.iSumLiquidatedDamagesCapital = $("#iSumLiquidatedDamagesCapital").text().trim();
//    return o;
//}


//提交前拼接延期通知单表体数据
function Get_Arrear_Body() {
    var $detail = $("#detail");
    var arr = [];
    $.each($detail.find(".ccus_tr"), function(i, v) {
        var arrearbody = new ArrearBody($(v).find(".cSubCusCode").val().trim(), $(v).find(".cSubCusName").text().trim(), $(v).find(".iSumSubCusCode").val().trim());
        arr.push(arrearbody);
    })
    return arr;
}

//延期通知单表体构造函数
var ArrearBody = function(cSubCusCode, cSubCusName, iSumSubCusCode) {
    this.cSubCusCode = cSubCusCode;
    this.cSubCusName = cSubCusName;
    this.iSumSubCusCode = iSumSubCusCode;
}


//转换日期格式，将格式为“/Date(-2036476800000+0800)/”转换为“2016-09-09”
function return_date(date) {
    if (date != "" && date != null) {
        if (date.indexOf("T") > -1) {
            var arr = date.split("T");
            return arr[0];

        } else {
            var new_date = date.slice(6, 19);
            var time = new Date(Number(new_date));
            var t = time.getFullYear() + "-";
            t = t + ((time.getMonth() + 1).toString().length == 2 ? (time.getMonth() + 1) : ("0" + (time.getMonth() + 1).toString())) + "-";
            t = t + ((time.getDate()).toString().length == 2 ? (time.getDate()) : ("0" + (time.getDate()).toString()));
            return t;
        }

        //return time.toLocaleDateString().replace(/\//g, "-");
    } else {
        return "";
    }
}
//转换日期格式，将格式为“/Date(-2036476800000+0800)/”转换为“2016-6-25 17:14:38”
function return_datetime(date) {
    if (date != "" && date != null) {
        if (date.indexOf("T") > -1) {
            var arr = date.split("T");
            var t = arr[0] + " " + arr[1].substr(0, 8);
            return t;
        } else {
            var new_date = date.slice(6, 19);
            var time = new Date(Number(new_date));
            var t = time.getFullYear() + "-";
            t = t + ((time.getMonth() + 1).toString().length == 2 ? (time.getMonth() + 1) : ("0" + (time.getMonth() + 1).toString())) + "-";
            t = t + ((time.getDate()).toString().length == 2 ? (time.getDate()) : ("0" + (time.getDate()).toString())) + " ";
            t = t + ((time.getHours()).toString().length == 2 ? (time.getHours()) : ("0" + (time.getHours()).toString())) + ":";
            t = t + ((time.getMinutes()).toString().length == 2 ? (time.getMinutes()) : ("0" + (time.getMinutes()).toString())) + ":";
            t = t + ((time.getSeconds()).toString().length == 2 ? (time.getSeconds()) : ("0" + (time.getSeconds()).toString()));
            // return time.getFullYear() + "-" +  (time.getMonth() + 1)+ "-" + time.getDate() + " " + time.getHours() + ":" + time.getMinutes() + ":" + time.getSeconds();
            return t;
        }

    } else {
        return "";
    }
}


/*
 *   功能:实现VBScript的DateAdd功能.
 *   参数:interval,字符串表达式，表示要添加的时间间隔.
 *   参数:number,数值表达式，表示要添加的时间间隔的个数.
 *   参数:date,时间对象.
 *   返回:新的时间对象.
 *   var now = new Date();
 *   var newDate = DateAdd( "d", 5, now);
 *---------------   DateAdd(interval,number,date)   -----------------
 */
function DateAdd(interval, number, date) {
    switch (interval) {
        case "y":
            {
                date.setFullYear(date.getFullYear() + number);
                return date;
                break;
            }
        case "q":
            {
                date.setMonth(date.getMonth() + number * 3);
                return date;
                break;
            }
        case "M":
            {
                date.setMonth(date.getMonth() + 1 + number);
                return date;
                break;
            }
        case "w":
            {
                date.setDate(date.getDate() + number * 7);
                return date;
                break;
            }
        case "d":
            {
                date.setDate(date.getDate() + number);
                return date;
                break;
            }
        case "h":
            {
                date.setHours(date.getHours() + number);
                return date;
                break;
            }
        case "m":
            {
                date.setMinutes(date.getMinutes() + number);
                return date;
                break;
            }
        case "s":
            {
                date.setSeconds(date.getSeconds() + number);
                return date;
                break;
            }
        default:
            {
                date.setDate(date.getDate() + number);
                return date;
                break;
            }
    }
}


// 对Date的扩展，将 Date 转化为指定格式的String   
// 月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符，   
// 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字)   
// 例子：   
// (new Date()).Format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423   
// (new Date()).Format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18   
Date.prototype.Format = function(fmt) { //author: meizz   
    var o = {
        "M+": this.getMonth() + 1, //月份   
        "d+": this.getDate(), //日   
        "h+": this.getHours(), //小时   
        "m+": this.getMinutes(), //分   
        "s+": this.getSeconds(), //秒   
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度   
        "S": this.getMilliseconds() //毫秒   
    };
    if (/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt))
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}



/** 
 * 获取上一个月 
 * 
 * @date 格式为yyyy-mm-dd的日期，如：2014-01-25 
 */

function getPreMonth(date) {
    var arr = date.split('-');
    var year = arr[0]; //获取当前日期的年份  
    var month = arr[1]; //获取当前日期的月份  
    var day = arr[2]; //获取当前日期的日  
    var days = new Date(year, month, 0);
    days = days.getDate(); //获取当前日期中月的天数  
    var year2 = year;
    var month2 = parseInt(month) - 1;
    if (month2 == 0) {
        year2 = parseInt(year2) - 1;
        month2 = 12;
    }
    var day2 = day;
    var days2 = new Date(year2, month2, 0);
    days2 = days2.getDate();
    if (day2 > days2) {
        day2 = days2;
    }
    if (month2 < 10) {
        month2 = '0' + month2;
    }
    var t2 = year2 + '-' + month2 + '-' + day2;
    return t2;
}

/** 
 * 获取下一个月 
 * 
 * @date 格式为yyyy-mm-dd的日期，如：2014-01-25 
 */
function getNextMonth(date) {
    var arr = date.split('-');
    var year = arr[0]; //获取当前日期的年份  
    var month = arr[1]; //获取当前日期的月份  
    var day = arr[2]; //获取当前日期的日  
    var days = new Date(year, month, 0);
    days = days.getDate(); //获取当前日期中的月的天数  
    var year2 = year;
    var month2 = parseInt(month) + 1;
    if (month2 == 13) {
        year2 = parseInt(year2) + 1;
        month2 = 1;
    }
    var day2 = day;
    var days2 = new Date(year2, month2, 0);
    days2 = days2.getDate();
    if (day2 > days2) {
        day2 = days2;
    }
    if (month2 < 10) {
        month2 = '0' + month2;
    }

    var t2 = year2 + '-' + month2 + '-' + day2;
    return t2;
}