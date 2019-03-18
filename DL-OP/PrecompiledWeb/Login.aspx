<%@ page language="C#" autoeventwireup="true" inherits="Login, dlopwebdll" enableviewstate="true" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<%
    Response.Buffer = true;
    Response.ExpiresAbsolute = System.DateTime.Now.AddSeconds(-1);
    Response.Expires = 0;
    Response.CacheControl = "no-cache";
    Response.AddHeader("Pragma", "No-Cache");
%>
<script type="text/javascript" src="../js/jquery-1.11.0.min.js"></script>
<script type="text/javascript" src="../js/jquery.qrcode.min.js"></script>
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script type="text/javascript">

        //得到焦点时触发事件
        function onFocusFun(element, elementValue) {
            if (element.value == elementValue) {
                element.value = "";
                //                element.style.color = "";
            }
        }
        //离开输入框时触发事件
        function onblurFun(element, elementValue) {
            if (element.value == '') {
                //                element.style.color = "#808080";
                element.value = elementValue;
            }
        }

        function changeImg(btn) //鼠标移入，更换图片
        {
            btn.src = "images/login80.png";
        }
        function changeback(btn)  //鼠标移出，换回原来的图片
        {
            btn.src = "images/login80black.png";
        }

        function openLink() {
            window.open("http://www.wenjuan.com/s/qyquUj/");
        }

        function newWinUrl(url) {
            var f = document.createElement("form");
            f.setAttribute("action", url);
            f.setAttribute("method", 'get');
            f.setAttribute("target", '_black');
            document.body.appendChild(f)
            f.submit();
        }
        function qr() {
            $('#code').qrcode("http://www.helloweba.com"); //任意字符串 
        }
        $(document).ready(function () {
            //$('#code').qrcode("http://www.helloweba.com"); //任意字符串 
            //var path="http://127.1.1.1:8080/pc/dowmload";
            //var realpath=toUtf8(path);
            var sid = $('#HidFSessionID').attr('value')
            var realpath = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx85ee38394e42f0b7&redirect_uri=http://dl.duolian.com:8003/wxapp/wxscanlogin_a.aspx&response_type=code&scope=snsapi_base&state=" + sid + "#wechat_redirect";
            $("#code").qrcode({
                text: realpath,  //设置二维码内容  
                //render : "table",//设置渲染方式  
                width: 200,     //设置宽度,默认生成的二维码大小是 256×256
                height: 200,     //设置高度  
                //typeNumber : -1,      //计算模式  
                //correctLevel : QRErrorCorrectLevel.H,//纠错等级  
                //background : "#ffffff",//背景颜色  
                //foreground : "#000000" //前景颜色  
            });
        })

    </script>


    <style type="text/css">
        .auto-style1 {
            width: 770px;
        }

        .auto-style3 {
            width: 100%;
            height: 100%;
            background-image: url('images/blue.jpg');
        }

        .auto-style4 {
            height: 150px;
        }

        .auto-style5 {
            width: 335px;
            font-size: small;
            color: #FF0000;
        }

        .auto-style6 {
            width: 255px;
            height: 36px;
        }

        .auto-style7 {
            color: #FF0000;
        }

        .auto-style8 {
            color: #FFFFFF;
        }
    </style>
</head>
<body style="height: 100%; padding: 0; margin: 0">
    <form id="form1" runat="server">
        <div id="mp" style="width: 100%; height: 100%; position: absolute; top: -1px; left: 0px;">
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <asp:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick" Enabled="False" Interval="1000" ViewStateMode="Enabled">
            </asp:Timer>
            <asp:HiddenField ID="HidFSessionID" runat="server" />
            <table align="center" cellpadding="0" cellspacing="0" class="auto-style3">
                <tr>
                    <td class="auto-style3">

                        <table align="center" class="auto-style1">
                            <tr>
                                <td colspan="4" style="text-align: center" class="auto-style4">
                                    <h1 style="margin-left: 300; font-family: 'Microsoft YaHei'" class="auto-style8">多联公司网上订单系统</h1>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 335px; height: 18px; text-align: right;">
                                    <dx:ASPxLabel ID="Warm" ClientInstanceName="Warm" runat="server" Style="text-align: left; color: #FF0000; font-weight: 700; text-decoration: underline;" Text="">
                                    </dx:ASPxLabel>
                                </td>
                                <td rowspan="2" colspan="2" class="auto-style6">
                                    <div style="float: left">
                                        <dx:ASPxTextBox ID="TxtUserName" runat="server" Width="240px" EnableTheming="True" Height="30px" HorizontalAlign="Left" NullText="请输入用户名" Theme="Office2010Blue" Font-Size="Medium" TabIndex="1">
                                            <ValidationSettings CausesValidation="True" ErrorText="请输入用户名" SetFocusOnError="True" ValidationGroup="imgBtn">
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </div>
                                    <div style="float: left">
                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="请输入登录名" ControlToValidate="TxtUserName"
                                            ValueToCompare="请输入登录名" Operator="NotEqual" ForeColor="Red" ValidationGroup="imgBtn">*</asp:CompareValidator>
                                    </div>
                                </td>
                                <td rowspan="6" style="width: 180px;">
                                    <asp:ImageButton ID="IbtnLogin" runat="server" ImageUrl="~/images/login80black.png" OnClick="IbtnLogin_Click" onmouseout="changeback(this)" onmouseover="changeImg(this)" ValidationGroup="imgBtn" TabIndex="6" />

                                </td>
                            </tr>
                            <tr>
                                <td rowspan="4" style="height: 72px; background-image: url('../images/logo72.png'); background-repeat: no-repeat; background-position: right">
                                    <asp:Image ID="qiyehao" runat="server" ImageUrl="~/images/qiyehao.jpg" Height="82px" Width="87px" />
                                    <a style="color: #99ff99">(扫描关注企业号)</a></td>
                            </tr>
                            <tr>
                                <td colspan="2" class="auto-style6">

                                    <div style="float: left">
                                        <dx:ASPxTextBox ID="TxtPWD" runat="server" Width="240px" Height="30px" NullText="请输入密码" Theme="Office2010Blue" TabIndex="2" Font-Size="Medium" Password="True">
                                            <ValidationSettings CausesValidation="True" ErrorText="请输入密码" SetFocusOnError="True" ValidationGroup="imgBtn">
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </div>
                                    <div style="float: left">
                                        <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="请输入登录密码" ControlToValidate="TxtPWD"
                                            ValueToCompare="请输入登录密码" Operator="NotEqual" ForeColor="Red" ValidationGroup="imgBtn">*</asp:CompareValidator>
                                    </div>
        </div>
        </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="auto-style6">
                                    <div style="float: left">
                                        <dx:ASPxTextBox ID="TxtUserSubPhone" runat="server" Width="240px" EnableTheming="True" Height="30px" HorizontalAlign="Left" NullText="请输入已绑定的手机号码" Theme="Office2010Blue" Font-Size="Medium" TabIndex="3">
                                            <ValidationSettings CausesValidation="True" ErrorText="请输入已绑定的手机号码" SetFocusOnError="True" ValidationGroup="imgBtn" ErrorTextPosition="Bottom">
                                                <RegularExpression ErrorText="请输入正确手机号码" ValidationExpression="^(13[0-9]|14[5|7]|15[0|1|2|3|5|6|7|8|9]|18[0|1|2|3|5|6|7|8|9])\d{8}$" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </div>
                                    <div style="float: left">
                                        <asp:CompareValidator ID="CompareValidator4" runat="server" ErrorMessage="请输入已绑定的手机号码" ControlToValidate="TxtUserSubPhone"
                                            ValueToCompare="请输入已绑定的手机号码" Operator="NotEqual" ForeColor="Red" ValidationGroup="imgBtn">*</asp:CompareValidator>
                                    </div>
                                </td>
                            </tr>
        <tr>
            <td rowspan="2" style="width: 135px; text-align: left;">
                <div style="float: left">
                    <dx:ASPxTextBox ID="TxtAuth" runat="server" Width="120px" Height="30px" NullText="请输入短信验证码" Theme="Office2010Blue" TabIndex="4" Font-Size="10pt">
                        <ValidationSettings CausesValidation="True" ErrorText="请输入验证码" ValidationGroup="imgBtn">
                        </ValidationSettings>
                    </dx:ASPxTextBox>
                </div>
                <div style="float: left">
                    <asp:CompareValidator ID="CompareValidator3" runat="server" ErrorMessage="请输入短信验证码" ControlToValidate="TxtAuth"
                        ValueToCompare="请输入短信验证码" Operator="NotEqual" ForeColor="Red" ValidationGroup="imgBtn">*</asp:CompareValidator>
                </div>
            </td>
            <td rowspan="2" style="width: 120px; text-align: left;">

                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="btn" runat="server" Style="width: 85px; height: 33px;" Text="获取验证码" OnClick="btn_Click" TabIndex="5" />
                        <asp:Label ID="Label1" runat="server" Style="width: 5px; height: 33px;" ForeColor="Red"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td class="auto-style5" rowspan="3"></td>
        </tr>
        <%-- <tr> <td style="height: 10px; text-align: center;">   </td>
             <td colspan="2" style="height: 10px; text-align: left;"><asp:CheckBox ID="CheckBox1" runat="server" Text="微信企业号消息" ForeColor="#99ff99" Font-Size="9" Checked="true" />   
            <td></td> </tr>--%>
        <tr>
            <td colspan="2" style="height: 10px; text-align: left;">
                <asp:RadioButton ID="RadioButton1" Text="短信通道1" runat="server" Checked="true" GroupName="sms" />
                <asp:RadioButton ID="RadioButton2" Text="短信通道2" runat="server" Checked="false" GroupName="sms" />
                <td></td>
        </tr>
        <tr>
            <td colspan="2" style="height: 10px; text-align: left;">
                <asp:CheckBox ID="CheckBox1" runat="server" Text="微信企业号消息(需关注多联企业号,并联系客服添加手机号码！如不需要微信信息，请取消勾选！)" ForeColor="#99ff99" Font-Size="9" Checked="true" />
                <td></td>
        </tr>
        <tr>

            <td colspan="4" style="height: 150px; text-align: center;">
                <br />
                <span class="auto-style7">
                    <div id="Div1">使用微信扫码登录👇</div>
                    <div id="code"></div>
                    <div id="Div2">使用微信扫码登录👆</div>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="QRRefresh" runat="server" OnClick="QRRefresh_Click" Text="刷新二维码" />
                            <asp:Timer ID="Timer2" runat="server" OnTick="Timer2_Tick"></asp:Timer>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <dvi>为了获取更好的浏览效果,请使用IE8以上的浏览器!</dvi>
                </span>
                <br class="auto-style7" />
                <span class="auto-style7">
                    <dvi>推荐使用火狐浏览器!</dvi>
                </span>

                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                Copyright © 2015 Powered by DuoLian：IMD </td>
        </tr>
        </table>

                    </td>
                </tr>
            </table>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="true" ShowSummary="false" />
        </div>

    </form>

</body>
</html>
