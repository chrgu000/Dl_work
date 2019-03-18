<%@ page language="C#" autoeventwireup="true" inherits="dluser_UpdateCusCodeClass, dlopwebdll" enableviewstate="false" %>

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

            <div style="border: medium double #999966">
                <h3>更新顾客的允限销分类:</h3>
                <a style="float: left">①单个顾客分类更新,请输入需要更新的顾客编码:</a><div style="float: left">
                    <dx:ASPxTextBox ID="TxtCusClassText" runat="server" Width="170px">
                    </dx:ASPxTextBox>
                </div>
                <dx:ASPxButton ID="BtnUpdateSinCustomerClass" runat="server" Text="更新" Width="150px" OnClick="BtnUpdateSinCustomerClass_Click">
                    <ClientSideEvents Click="function(s, e) {
e.processOnServer=confirm('确认执行此操作吗?'); 	
}" />
                </dx:ASPxButton>
                <p></p>
                <a style="float: left">②更新所有顾客的分类(此操作耗时较长,建议在订单系统空闲时间进行此操作,勿在顾客下单频繁时使用!)</a><div style="float: left">
                </div>
                <dx:ASPxButton ID="BtnUpdateAllCustomerClass" runat="server" Text="更新" Width="150px" OnClick="BtnUpdateAllCustomerClass_Click">
                    <ClientSideEvents Click="function(s, e) {
e.processOnServer=confirm('此操作会更新全部顾客产品分类,并且耗时较长,确认执行此操作吗?'); 	
}" />
                </dx:ASPxButton>



            </div>

            <div style="border: medium double #999966">
                <h3>新增顾客:(从U8中生成该顾客的网上订单账户,需要先完成U8中的顾客信息的新增.)</h3>
                <a style="float: left">①单个顾客分类更新,请输入需要更新的顾客编码:</a>
                <div style="float: left">
                    <dx:ASPxTextBox ID="TxtCusAdd" runat="server" Width="170px">
                    </dx:ASPxTextBox>
                </div >
                <dx:ASPxButton ID="BtnCusAdd" runat="server" Text="新增顾客账户" Width="150px" OnClick="BtnCusAdd_Click">
                    <ClientSideEvents Click="function(s, e) {
e.processOnServer=confirm('确认执行此操作吗?'); 	
}" />
                </dx:ASPxButton>
            </div>

                        

        </div>
    </form>
</body>
</html>
