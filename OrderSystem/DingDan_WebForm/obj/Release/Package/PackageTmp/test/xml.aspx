<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="xml.aspx.cs" Inherits="DingDan_WebForm.test.xml" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <br />
    <div>
        <h1 id="title"></h1>
        <form runat="server">
            <input type="text" name="name" value=" " class="tt" id="bbb" />
            <input type="text" name="name" value=" " class="tt" id="aaa" />
            <asp:Label Text="bbbbbbbbbbb" runat="server" ID="aa" />
            <asp:HiddenField ID="HiddenField1" runat="server" value=""/>
            <asp:Button Text="text" runat="server" id="btn" OnClick="btn_Click"/>
        </form>

    </div>
    <script src="../SuperAdmin/js/jquery-2.2.4.min.js"></script>
    <script>
        $(function () {
            $(".tt").keyup(function () {
                $("#HiddenField1").val($(this).val())
                var that = this;
                $("#aa").text($(this).val());
                if ($(this).val().trim().length == 4) {
                    console.log($(this).val().trim());
                    var aaa = "<%=abc()%>";
                    $("#btn").click(function () {
                      
                        $(that).val("<%=aa.Text%>");
                    });
                }





                //$.ajax({
                //    url: "xml.aspx/GetName?code="+$(this).val().trim(),
                //    type: "Post",
                //    contentType: "application/json; charset=utf-8",
                //    success: function (res) {
                //        console.log(res);
                //    },
                //    error: function (err) {
                //        console.log(err);
                //    }
                //})

            })
        })
    </script>
</body>
</html>
