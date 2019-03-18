<%@ Page Language="C#" AutoEventWireup="true" CodeFile="testjsshuzi.aspx.cs" Inherits="test_testjsshuzi" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>

<script type="text/javascript">
    function getNum(text) {
        var aa = new Array();
        aa = text.split("=");
        for (i = 0; i < aa.length; i++) {
            aa[i] = aa[i].replace(/[^0-9]/ig, "");
            alert(aa[i]);
        }     
    }
</script>
</head>


<body>
<input type="text" id="btn_getNum"/>
<input type="button" value="得到数字" onclick="getNum(btn_getNum.value);"/>
</body>
</html>
