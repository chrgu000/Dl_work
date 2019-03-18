<%@ page language="C#" autoeventwireup="true" inherits="wxapp_scan, dlopwebdll" enableviewstate="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="http://rescdn.qqmail.com/node/ww/wwopenmng/js/sso/wwLogin-1.0.0.js"></script>
</head>

<body>
    <form id="form1" runat="server">
        <div>
    <span id="wx_reg"></span>  
                <script type="text/javascript">
                    var obj = window.WwLogin({
                        "id": "wx_reg",
                        "appid": "wx85ee38394e42f0b7",
                        "agentid": "1000002",
                        "redirect_uri": encodeURIComponent("http://dl.duolian.com:8002/test/scan.aspx"),
                        "state": "RPG5G9paWp8MwDvme2rRxEdkuKHoHXUrlu-EsQgH7UrrajPvga17PAA4cMjocPu5qP09E6q4I-rrAWq0U-eTu_JZVgoZf1vnETSJ479xWTMHB6Uwoz18fJvasDW99ygvvIHFmc650SNO907PCI592IjIF1CvpxSl90bRLzXAaNKjSDjymWLSgHhV-4QsaJPWqSJDANyhOaG3xFzl73JALNG0E-Vb_bck2Z5sIUaVH3pRTZaQCL6vekT6V-dQiip5puukcC8VxeDbhi6NHOZO2AFlzpFy1l07l0ZuJfapP3c",
                        "href": "",
                    });
</script> 
            <asp:Label ID="Label2" runat="server" Text="Labe2"></asp:Label>
        </div>
    </form>
</body>

</html>
