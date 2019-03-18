<%@ Page Language="C#" AutoEventWireup="true" CodeFile="vote.aspx.cs" Inherits="test_vote" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="gb2312" />
    <meta name="Generator" content="EditPlus?" />
    <meta name="Author" content="" />
    <meta name="Keywords" content="" />
    <meta name="Description" content="" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black-translucent" />
    <title></title>
</head>
<body onload="reload_yanzhengma()">
    <form id="form_main" runat="server">
        <div>
            <input id="mycheckbox" type="checkbox" value="147614" name="mycheckbox"  checked="checked"  /> 
             <input id="yanzhengma" type="text" maxlength="4" size="6" name="yanzhengma" /> 
        </div>
        <div class="button"> 
      <img onclick="form_votes('mycheckbox')"  src="http://www.paihang360.com/zt/slgd2016/images/toupiao.png" style="margin-right:30px;" /> 
      <img onclick="seeResult()" src="http://www.paihang360.com/zt/slgd2016/images/jieguo.png" /> 
     </div> 
         <img id="imgYanZhengma" border="0"  /> <br>
	       <a href="javascript:reload_yanzhengma();">看不清，换一张！</a> 
<a id="qqq" >sdas</a> 
    </form>
     <script type="text/javascript">
         function two_char(n) {
             return n >= 10 ? n : "0" + n;
         }
         function time_fun() {
             var sec = 0;
             setInterval(function () {
                 sec++;
                 var date = new Date(0, 0)
                 date.setSeconds(sec);
                 var h = date.getHours(), m = date.getMinutes(), s = date.getSeconds();
                 document.getElementById("mytime").innerText = two_char(h) + ":" + two_char(m) + ":" + two_char(s);
             }, 1000);
         }
         //投票
         function form_votes(mycheckbox) {
             document.form_main.action = 'http://www.paihang360.com/zt/slgd2016/company.jsp?record_ids=147614';
             document.form_main.op.value = 'op_submit';
             document.form_main.submit();
         }
         //刷新验证码
         function reload_yanzhengma() {
             document.getElementById("yanzhengma").value = '';
             var m = Math.random();
             document.getElementById("imgYanZhengma").src = "http://www.paihang360.com/zt/slgd2016/Num.jsp?" + m;
             document.getElementById("qqq").innerHTML = m;
         }



         //查看投票结果
         function seeResult() {

             document.form_main.action = 'http://www.paihang360.com/zt/slgd2016/finally.jsp';
             document.form_main.submit();
         }

</script>

</body>
</html>
