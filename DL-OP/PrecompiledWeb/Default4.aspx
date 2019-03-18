<%@ page language="C#" autoeventwireup="true" inherits="Default4, dlopwebdll" enableviewstate="false" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>
<html>
<%
    Response.Buffer = true;
    Response.ExpiresAbsolute = System.DateTime.Now.AddSeconds(-1);
    Response.Expires = 0;
    Response.CacheControl = "no-cache";
    Response.AddHeader("Pragma", "No-Cache");
%>
<head>
    <meta charset="utf-8" />
    <title>多联实业有限公司</title>

    <script class="jsbin" src="../js/jquery-1.7.2.min.js"></script>
    <link rel="stylesheet" type="text/css" href="../js/themes/default/easyui.css">
    <script type="text/javascript" src="../js/jquery.easyui.min.js"></script>
    <link rel="stylesheet" type="text/css" href="../js/themes/icon.css" />

    <style>
        article, aside, figure, footer, header, hgroup,
        menu, nav, section {
            display: block;
        }

        .west {
            width: 200px;
            padding: 10px;
        }

        .north {
            height: 100px;
        }
    </style>
    <script type="text/javascript">
        function logout() {
            var msg = '您确定要退出吗?';
            if (confirm(msg) == true) {
                parent.location.href = '../logout.aspx';
            } else {
                return false;
            }
        }

    </script>
</head>
<body class="easyui-layout">
    <form id="form1" runat="server">
        <div region="north" split="true" border="false" style="overflow: hidden; height: 66px; background: url(images/layout-browser-hd-bg.gif) #7f99be repeat-x center 50%; line-height: 20px; color: #fff; font-family: Verdana, 微软雅黑,黑体">
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <span style="float: right; padding-right: 30px;" class="head"><%=Session["strUserName"].ToString() %>
                <dx:ASPxButton ID="btShowModal" runat="server" Text="修改密码" AutoPostBack="False" UseSubmitBehavior="False" Width="70px"
                    Border-BorderStyle="None" Theme="Default" Height="19px" RenderMode="Link" EnableTheming="True" Font-Size="Small" ForeColor="#FF9900">
                    <ClientSideEvents Click="function(s, e) { CallBtn(); }" />
                    <Border BorderStyle="None" />
                    <BorderLeft BorderStyle="None" />
                    <BorderTop BorderStyle="None" />
                    <BorderRight BorderStyle="None" />
                    <BorderBottom BorderStyle="None" />
                </dx:ASPxButton>
                <a href="#" id="logout" onclick="logout()" style="color: #FF9900; font-size: small">安全退出</a></span>
            <span style="padding-left: 10px; font-size: 16px;">
                <img src="images/logo60.png" width="96" height="60" /></span>

        </div>
        <div region="south" split="true" style="height: 30px; background: #D2E0F2;">
            <div class="footer" style="text-align: center">Copyright © 2015 Powered by DuoLian：IMD</div>
        </div>

        <div region="center" title="工作区" id="work">
            <div style="display: none;">
                <dx:ASPxButton ID="btSetPWD" runat="server" Border-BorderStyle="None" AutoPostBack="False" UseSubmitBehavior="False"
                    ClientIDMode="Static" tt="btSetPWD">
                    <ClientSideEvents Click="function(s, e) { ShowPWDWindow(); }" />
                </dx:ASPxButton>
            </div>

            <dx:ASPxPopupControl ID="setPWD" runat="server" CloseAction="CloseButton" CloseOnEscape="True" Modal="True" Target="_blank"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="setPWD"
                HeaderText="修改密码" AllowDragging="True" PopupAnimationType="None" EnableViewState="False" OnWindowCallback="setPWD_WindowCallback" ClientIDMode="Static">
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
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                        <ContentTemplate>
                                                            <dx:ASPxButton ID="btOK" runat="server" Text="确定" Width="80px" AutoPostBack="False" Style="float: left; margin-right: 8px" OnClick="btOK_Click">
                                                                <ClientSideEvents Click="function(s, e) {
 if(ASPxClientEdit.ValidateGroup('entryGroup')) 
                                                    {
PWD();
setPWD.Hide();
                                                    }
 }" />
                                                            </dx:ASPxButton>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
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
            <div class="easyui-tabs" fit="true" border="false" id="tabs">
                <div title="首页" iconcls="icon-dl_zy">
                    
                </div>
            </div>
        </div>
        <div region="west" class="west" title="导航菜单" style="width: 180px">
            <ul id="tree"></ul>
        </div>

        <div id="tabsMenu" class="easyui-menu" style="width: 120px;">
            <div name="close">关闭</div>
            <div name="Other">关闭其他</div>
            <div name="All">关闭所有</div>
        </div>
    </form>
</body>
<script type="text/javascript">
    $(function () {
        //动态菜单数据
        var treeData = [{
            text: "我的多联", iconCls: "icon-dl_hy",
            children: [{
                text: "首页", iconCls: "icon-dl_zy",
                attributes: {
                    url: "../dluser/Default.aspx"
                }, url: "../dluser/Default.aspx"
            }
            ,

            {
                text: "我的工作", iconCls: "icon-dl_ddzx",
                state: "opened",
                children: [
                    {
                          text: "库存查询", iconCls: "icon-dl_redo",
                          attributes: { url: "../dluser/Allquery.aspx" }
                            }
                        ]
                    }
                    //,
 //                   {
 //                       text: "预订单参照订单处理", iconCls: "icon-dl_redo",
 //                       state: "closed",
 //                       children: [
 //{
 //    text: "待处理订单", iconCls: "icon-dl_gwc",
 //    attributes: {
 //        url: "../dluser/Atasks.aspx"
 //    }, url: "../dluser/Atasks.aspx"
 //}, {
 //    text: "待办工作", iconCls: "icon-dl_ybc",
 //    attributes: {
 //        url: "../dluser/Gtasks.aspx"
 //    }, url: "../dluser/Gtasks.aspx"
 //}, {
 //    text: "已办工作", iconCls: "icon-dl_ybc",
 //    attributes: {
 //        url: "../dluser/Stasks.aspx"
 //    }, url: "../dluser/Stasks.aspx"
 //}, {
 //    text: "办结工作", iconCls: "icon-dl_dsh",
 //    attributes: {
 //        url: "../dluser/Etasks.aspx"
 //    }, url: "../dluser/Etasks.aspx"
 //}
 //                       ]
 //                   },
                                        //{
                                        //    text: "订单中心", iconCls: "icon-dl_tip",
                                        //    state: "opend",
                                        //    children: [
                                        //         {
                                        //             text: "待处理订单", iconCls: "icon-dl_gwc",
                                        //             attributes: {
                                        //                 url: "../dluser/Atasks.aspx"
                                        //             }, url: "../dluser/Atasks.aspx"
                                        //         }, {
                                        //             text: "待办工作(普通订单)", iconCls: "icon-dl_ybc",
                                        //             attributes: {
                                        //                 url: "../dluser/Gtasks.aspx"
                                        //             }, url: "../dluser/Gtasks.aspx"
                                        //         },
                                                 //{
                                                 //    text: "待办工作(酬宾订单)", iconCls: "icon-dl_ybc",
                                                 //    attributes: {
                                                 //        url: "../dluser/Gtasks_Y.aspx"
                                                 //    }, url: "../dluser/Gtasks_Y.aspx"
                                                 //}, {
                                                 //    text: "待办工作(特殊订单)", iconCls: "icon-dl_ybc",
                                                 //    attributes: {
                                                 //        url: "../dluser/Gtasks_X.aspx"
                                                 //    }, url: "../dluser/Gtasks_X.aspx"
                                                 //},
        //                                         {
        //                                             text: "已办工作", iconCls: "icon-dl_ybc",
        //                                             attributes: {
        //                                                 url: "../dluser/Stasks.aspx"
        //                                             }, url: "../dluser/Stasks.aspx"
        //                                         }, {
        //                                             text: "办结工作", iconCls: "icon-dl_dsh",
        //                                             attributes: {
        //                                                 url: "../dluser/Etasks.aspx"
        //                                             }, url: "../dluser/Etasks.aspx"
        //                                         }
        //                                    ]
        //                                },
        //        ]
        //    },
        //    {
        //        text: "我的信息", iconCls: "icon-dl_user",
        //        state: "closed",
        //        children: [{
        //            text: "个人信息", iconCls: "icon-dl_grxx",
        //            attributes: {
        //                url: "../dluser/Default.aspx"
        //            }, url: "../dluser/Default.aspx"
        //        },

        //        {
        //            text: "账户安全", iconCls: "icon-dl_zhaq",
        //            attributes: {
        //                url: "../dluser/Default.aspx"
        //            }, url: "../dluser/Default.aspx"
        //        }
        //        ]
        //    },

        //    {
        //        text: "报表系统", iconCls: "icon-dl_bb",
        //        state: "closed", children: [{
        //            text: "对账单", iconCls: "icon-dl_grxx",
        //            attributes: {
        //                url: "../dluser/UseSOA.aspx"
        //            }, url: "../dluser/UseSOA.aspx"
        //        },

        //        {
        //            text: "业务报表", iconCls: "icon-dl_zhaq",
        //            attributes: {
        //                url: "#"
        //            }, url: "#"
        //        }
        //        ]
        //    },

        //    {
        //        text: "系统设置", iconCls: "icon-dl_pz",
        //        attributes: {
        //            url: "../dluser/UpdateCusCodeClass.aspx"
        //        }, url: "../dluser/UpdateCusCodeClass.aspx"
        //    }, {
        //        text: "账单设置", iconCls: "icon-dl_pz",
        //        attributes: {
        //            url: "../dluser/SOASetting.aspx"
        //        }, url: "../dluser/SOASetting.aspx"
        //    },

            ]
        }
        ];

        //实例化树形菜单
        $("#tree").tree({
            data: treeData,
            lines: true,
            onClick: function (node) {
                if (node.attributes) {
                    Open(node.text, node.iconCls, node.attributes.url, node.url);
                }
            }
        });
        //在右边center区域打开菜单，新增tab
        function Open(text, icon, url, urll) {
            if ($("#tabs").tabs('exists', text)) {
                $('#tabs').tabs('select', text);
            } else {
                var content = '<iframe scrolling="auto" frameborder="0"  src="' + url + '" style="width:100%;height:100%;"></iframe>';
                $('#tabs').tabs('add', {
                    title: text,
                    iconCls: icon,
                    closable: true,
                    content: content
                });
            }
        }
        //绑定tabs的右键菜单
        $("#tabs").tabs({
            onContextMenu: function (e, title) {
                e.preventDefault();
                $('#tabsMenu').menu('show', {
                    left: e.pageX,
                    top: e.pageY
                }).data("tabTitle", title);
            }
        });
        //实例化menu的onClick事件
        $("#tabsMenu").menu({
            onClick: function (item) {
                CloseTab(this, item.name);
            }
        });
        //几个关闭事件的实现
        function CloseTab(menu, type) {
            var curTabTitle = $(menu).data("tabTitle");
            var tabs = $("#tabs");

            if (type === "close") {
                tabs.tabs("close", curTabTitle);
                return;
            }
            var allTabs = tabs.tabs("tabs");
            var closeTabsTitle = [];
            $.each(allTabs, function () {
                var opt = $(this).panel("options");
                if (opt.closable && opt.title != curTabTitle && type === "Other") {
                    closeTabsTitle.push(opt.title);
                } else if (opt.closable && type === "All") {
                    closeTabsTitle.push(opt.title);
                }
            });
            for (var i = 0; i < closeTabsTitle.length; i++) {
                tabs.tabs("close", closeTabsTitle[i]);
            }
        }
    });

</script>
</html>

<script type="text/javascript">
    //调用center中的显示窗口按钮
    function CallBtn() {
        //document.getElementById("<%=btSetPWD.ClientID%>").click();
        //document.getElementById("btSetPWD_CD").click();
        var list = document.getElementById("btSetPWD").getElementsByTagName("input");
        var btSetPWD = "";
        for (var i = 0; i < list.length && list[i]; i++) {
            //判断是否为按钮
            if (list[i].type == "button") {
                //strData += list[i].id + ":" + list[i].value + "--";
                btSetPWD = list[i].name
                document.getElementById(btSetPWD).click();
                //alert(list[i].name);
            }
        }
    }
    //显示修改密码窗口,
    function ShowPWDWindow() {
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
            //判断是否为密码框
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
        if (tbPWD == tbPassword && tbPWD !== "") {

        } else {
            alert("密码不一致!");
            setPWD.show();
        }

    }
</script>
