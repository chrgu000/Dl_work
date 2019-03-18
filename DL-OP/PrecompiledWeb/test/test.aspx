<%@ page language="C#" autoeventwireup="true" inherits="test, dlopwebdll" enableviewstate="false" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>

<body>
    <form id="form1" runat="server">
        <div>
            <dx:ASPxButton ID="btShowModal" runat="server" Text="修改密码" AutoPostBack="False" UseSubmitBehavior="false" Width="100%">
                <ClientSideEvents Click="function(s, e) { ShowPWDWindow(); }" />
            </dx:ASPxButton>
            <dx:ASPxPopupControl ID="setPWD" runat="server" CloseAction="CloseButton" CloseOnEscape="True" Modal="True"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="setPWD"
                HeaderText="修改密码" AllowDragging="True" PopupAnimationType="None" EnableViewState="False" OnWindowCallback="setPWD_WindowCallback">
                <ClientSideEvents PopUp="function(s, e) { ASPxClientEdit.ClearGroup('entryGroup'); tbPWD.Focus(); }" />
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                        <dx:ASPxPanel ID="Panel1" runat="server" DefaultButton="btOK">
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <table>
                                        <tr>
                                            <td rowspan="4">
                                                <div class="pcmSideSpacer">
                                                </div>
                                            </td>
                                            <td class="pcmCellCaption">
                                                <dx:ASPxLabel ID="lblUsername1" runat="server" Text="新密码:" AssociatedControlID="tbPWD" Width="60px">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td class="pcmCellText">
                                                <dx:ASPxTextBox ID="tbPWD" runat="server" Width="150px" ClientInstanceName="tbPWD" Password="True" ClientIDMode="Static" tt="tbPWD">
                                                    <ValidationSettings EnableCustomValidation="True" ValidationGroup="entryGroup" SetFocusOnError="True"
                                                        ErrorDisplayMode="Text" ErrorTextPosition="Bottom" CausesValidation="True">
                                                        <RequiredField ErrorText="请输入新密码" IsRequired="True" />
                                                        <RegularExpression ErrorText="Login required" />
                                                        <ErrorFrameStyle Font-Size="10px">
                                                            <ErrorTextPaddings PaddingLeft="0px" />
                                                        </ErrorFrameStyle>
                                                    </ValidationSettings>

                                                </dx:ASPxTextBox>
                                            </td>
                                            <td rowspan="4">
                                                <div class="pcmSideSpacer">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="pcmCellCaption">
                                                <dx:ASPxLabel ID="lblPass1" runat="server" Text="确认密码:" AssociatedControlID="tbPassword">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td class="pcmCellText">
                                                <dx:ASPxTextBox ID="tbPassword" runat="server" Width="150px" Password="True" ClientIDMode="Static" tt="tbPassword">
                                                    <ValidationSettings EnableCustomValidation="True" ValidationGroup="entryGroup" SetFocusOnError="True"
                                                        ErrorDisplayMode="Text" ErrorTextPosition="Bottom">
                                                        <RequiredField ErrorText="请再次输入新密码" IsRequired="True" />
                                                        <ErrorFrameStyle Font-Size="10px">
                                                            <ErrorTextPaddings PaddingLeft="0px" />
                                                        </ErrorFrameStyle>
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td class="pcmCheckBox">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div class="pcmButton">
                                                    <dx:ASPxButton ID="btOK" runat="server" Text="确定" Width="80px" AutoPostBack="False" Style="float: left; margin-right: 8px" OnClick="btOK_Click">
                                                        <ClientSideEvents Click="function(s, e) {
 if(ASPxClientEdit.ValidateGroup('entryGroup')) 
                                                    {
                                                            PWD();
setPWD.Hide();

                                                    }
 }" />
                                                    </dx:ASPxButton>
                                                    <dx:ASPxButton ID="btCancel" runat="server" Text="取消" Width="80px" AutoPostBack="False" Style="float: left; margin-right: 8px">
                                                        <ClientSideEvents Click="function(s, e) { setPWD.Hide(); }" />
                                                    </dx:ASPxButton>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxPanel>
                        <div>
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
                <ContentStyle>
                    <Paddings PaddingBottom="5px" />
                </ContentStyle>
            </dx:ASPxPopupControl>

            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:UFDATA_001_2015ConnectionString %>" 
                DeleteCommand="DELETE FROM [Dl_opUser] WHERE [lngopUserId] = @lngopUserId" 
                InsertCommand="INSERT INTO [Dl_opUser] ([strUserPwd]) VALUES (@strUserPwd)" 
                SelectCommand="SELECT [lngopUserId], [strUserPwd] FROM [Dl_opUser] WHERE ([lngopUserId] = @lngopUserId)" 
                UpdateCommand="UPDATE [Dl_opUser] SET [strUserPwd] = @strUserPwd WHERE [lngopUserId] = @lngopUserId">
                <DeleteParameters>
                    <asp:Parameter Name="lngopUserId" Type="Int32" />
                </DeleteParameters>
                <InsertParameters>
                    <asp:Parameter Name="strUserPwd" Type="String" />
                </InsertParameters>
                <SelectParameters>
                    <asp:Parameter DefaultValue="2" Name="lngopUserId" Type="Int32" />
                </SelectParameters>
                <UpdateParameters>
                    <asp:Parameter Name="strUserPwd" Type="String" />
                    <asp:Parameter Name="lngopUserId" Type="Int32" />
                </UpdateParameters>
            </asp:SqlDataSource>

        </div>
        <dx:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="False" OnClick="ASPxButton1_Click" Text="ASPxButton">
            <ClientSideEvents Click="function(s, e) {
}" />
        </dx:ASPxButton>
                        <dx:ASPxDateEdit ID="DeliveryDate" runat="server" AllowMouseWheel="False" AllowNull="False" AllowUserInput="False"
                            DateOnError="Today" EnableTheming="True" Theme="Office2010Blue" Height="18px" EditFormatString="yyyy-MM-dd HH"    EditFormat="Custom"  >
                            <CalendarProperties DayNameFormat="Short" FirstDayOfWeek="Monday">
                            </CalendarProperties>
                            <TimeSectionProperties Visible="True" ShowMinuteHand="False">
                                <TimeEditProperties AllowNull="False" DisplayFormatString="HH" EditFormatString="HH">
                                </TimeEditProperties>
                            </TimeSectionProperties>
</dx:ASPxDateEdit>
        <br />
        <dx:ASPxTabControl ID="ASPxTabControl1" runat="server">
        </dx:ASPxTabControl>
    </form>
</body>
</html>
<script type="text/javascript">
    function ShowPWDWindow() {
        document.getElementById("<%=ASPxButton1.ClientID%>").click();
        setPWD.Show();
    }
    function PWD() {
        //获取ID为tbPWD下面的input为password的ID和value，如果是页面所有的input则将document.getElementById("tbPWD")换为document即可
        var lista = document.getElementById("tbPWD").getElementsByTagName("input");
        var listb = document.getElementById("tbPassword").getElementsByTagName("input");
        //var list = document.getElementsByTagName("input");
        //var strData = "";
        var tbPWD = "";
        var tbPassword = "";
        //对表单中所有的input进行遍历
        for (var i = 0; i < lista.length && lista[i]; i++) {
            //判断是否为文本框
            if (lista[i].type == "password") {
                //strData += list[i].id + ":" + list[i].value + "--";
                tbPWD = lista[i].value
            }
        }
        for (var j = 0; j < listb.length && listb[j]; j++) {
            //判断是否为文本框
            if (listb[j].type == "password") {
                tbPassword = listb[j].value
            }
        }
        if (tbPWD == "'+tbPWD+'") {
            return;
        } else {
            if (tbPWD == tbPassword) {
                var c = '<%=newPWD("'+tbPWD+'") %>';//这里一定注意单引号和双引号的使用!!!!! 
 
                alert(c);
            } else {
                alert("密码不一致!");
                setPWD.show();
            }
        }


    }
</script>
