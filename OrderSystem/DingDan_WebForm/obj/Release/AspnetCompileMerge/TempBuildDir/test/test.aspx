<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="DingDan_WebForm.test.test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
        <script src="/js/Scripts/jquery-1.6.4.min.js" type="text/javascript"></script>
    <script src="/js/Scripts/jquery.signalR-2.2.2.min.js" type="text/javascript"></script>
    <script src="/Signalr/Hubs"></script>
   <script src="/js/SignalR/SignalR.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
  <button type="button" id="btn">readjson</button>
  <button type="button" id="btn1">writejson</button>
        <br />
        <asp:Button Text="加密" runat="server" ID="Button1"  OnClick="btn1_Click"/>
        <br />

   <asp:TextBox ID="text1" runat="server"></asp:TextBox>
        <asp:Button Text="解密" runat="server" ID="Button"  OnClick="btn_Click"/>
    </div>
    </form>
         <script>
             $(function () {
                 $("#btn").click(function () {
                  
                     $.ajax({
                         url: "test.aspx/SendSms",
                         type: "post",
                          contentType: "application/json; charset=utf-8",
                      //   dataType: "json",
                        // data: JSON.stringify(a),
                         success: function (res) {
                            console.log(JSON.parse(res.d))
                         },
                         error: function (err) {
                             console.log(err)
                         }
                     })

                 })
           
             })
     </script>
</body>
</html>
