<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VoteYanZhengMa.aspx.cs" Inherits="VoteYanZhengMa" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:HiddenField ID="hfyzm" runat="server"  />
        <asp:Button ID="Button1" runat="server" Text="识别验证码" OnClientClick="yzm()" OnClick="Button1_Click" /> 
        <asp:Button ID="Button2" runat="server" Text="del pic" OnClientClick="cltxt()" OnClick="Button2_Click" /> 
        <asp:TextBox ID="tbResult" runat="server" Width="101px"></asp:TextBox>
        <br />
        <asp:TextBox ID="txturl" runat="server" Width="275px" ></asp:TextBox>
    </div>
    </form>
</body>
            <script type="text/javascript">                
                function yzm() {
                    //parent[0].document.getElementById("yanzhengma").value = "5678";
                    parent[0].document.getElementById("yanzhengma").value = document.getElementById("hfyzm").value;
                }
                function cltxt() {                   
                    document.getElementById("txturl").value = "";
                }
        </script> 
</html>
