<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WXRegister.aspx.cs" Inherits="WXRegister" %>

<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        .auto-style1 {
            height: 24px;
            bor
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>

    <div>
    <div>
        <dx:ASPxLabel ID="dw" ClientInstanceName="dw" runat="server" Text="ASPxLabel" Font-Size="11pt">
        </dx:ASPxLabel>
        </div>
        <dx:ASPxGridView ID="WXGrid" ClientInstanceName="WXGrid" runat="server" AutoGenerateColumns="False" Width="600px">
            <Columns>
                <dx:GridViewDataTextColumn Caption="姓名" FieldName="strWXName" VisibleIndex="1" Width="120px">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="手机号码" FieldName="strWXPhoneNum" VisibleIndex="2" Width="160px">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="微信号" FieldName="strWXAcount" VisibleIndex="3" Width="180px">
                </dx:GridViewDataTextColumn>
            </Columns>
            <SettingsPager Mode="ShowAllRecords">
            </SettingsPager>
            <Settings ShowTitlePanel="True" />
            <SettingsText Title="已登记号码" EmptyDataRow="您还没有登记微信号码，赶紧登记一个吧" />
            <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
        </dx:ASPxGridView>
    
        <br />
    
    </div>
     <div id="wxtable" runat="server" style="border: thin double #008080;width:850px">

         <table>
             <tr>
                 <td style="width:150px">姓名(必填)</td>
                 <td style="width:200px">手机号码(必填)</td>
                 <td style="width:250px">微信号(选填)</td>
                 <td style="width:250px" ></td>
             </tr>
                          <tr>
                 <td class="auto-style1">
                     <dx:ASPxTextBox ID="TxtWXName" runat="server" Width="170px">
                         <ValidationSettings ErrorTextPosition="Bottom">
                             <RequiredField IsRequired="True" />
                         </ValidationSettings>
                     </dx:ASPxTextBox>
                              </td>
                 <td class="auto-style1">
                     <dx:ASPxTextBox ID="TxtWXPhoneNum" runat="server" Width="170px">
                         <ValidationSettings ErrorTextPosition="Bottom">
                             <RegularExpression ErrorText="请输入正确的手机号码!" ValidationExpression="^1[35678][0-9]{9}$" />
                             <RequiredField IsRequired="True" />
                         </ValidationSettings>
                     </dx:ASPxTextBox>
                              </td>
                 <td class="auto-style1">
                     <dx:ASPxTextBox ID="TxtWXAcount" runat="server" Width="170px">
                     </dx:ASPxTextBox>
                              </td>
                               <td ><dx:ASPxButton ID="BtnOk" runat="server" Text="登记号码" Theme="SoftOrange" OnClick="BtnOk_Click"></dx:ASPxButton></td>
             </tr>

         </table>
     </div>           
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
