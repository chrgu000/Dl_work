/*
*	Null转换为空字符串
*/
function sxToString(obj) {
    if (obj == null) {
        return '';
    }
    return obj;
}

/*
*	JSON返回数据库的时间格式为/Date(1332919782070)/，转换为标准格式
*/
function sxDateToString(val) {
    if (val == null) {
        return '';
    } else {
        var date = new Date(parseInt(val.replace("/Date(", "").replace(")/", ""), 10));
        //月份为0-11，所以+1，月份小于10时补个0
        var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
        return date.getFullYear() + "-" + month + "-" + currentDate;
    }
}

/*
*	删除数组某个元素（IE浏览器不支持Array.remove()方法）
*/
function removeArrayItem(array, val) {
    var index = array.indexOf(val.toString());
    if (index > -1) {
        array.splice(index, 1);
    }
    return array;
};