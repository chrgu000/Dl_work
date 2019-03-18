<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="Index" %>

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
    <script charset="utf-8" type="text/javascript" src="http://wpa.b.qq.com/cgi/wpa.php?key=XzkzODAyMTM0OV8zNTY4OTNfNDAwODc4NjMzM18"></script>
    <script type="text/javascript">
        function logout() {
            var msg = '您确定要退出吗?';
            if (confirm(msg) == true) {
                parent.location.href = 'logout.aspx';
            } else {
                return false;
            }
        }

    </script>
</head>
<body class="easyui-layout">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>



        <div region="north" split="true" border="false" style="overflow: hidden; height: 30px; background: url(images/layout-browser-hd-bg.gif) #7f99be repeat-x center 50%; line-height: 20px; color: #fff; font-family: Verdana, 微软雅黑,黑体">
            <span style="float: right; padding-right: 30px; font-size: 11pt" class="head"><%=Session["strUserName"].ToString() %><a>|</a> <%if(Session["strAllAcount"].ToString().Length==6){%> 主账户 <%}else{%> 子账户<%=Session["strAllAcount"].ToString().Substring(Session["strAllAcount"].ToString().Length-3,3) %> <%}%><a>|</a> <%=Session["strLoginPhone"].ToString() %> <a style="padding-left: 15px; color: salmon" href="HelpSop.html" target="_blank">使用帮助</a>
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
                &nbsp;</div>
        <div region="south" split="true" style="height: 30px; background: #D2E0F2;">

            <div class="footer" style="text-align: center">
                <span>
                    <script charset="utf-8" type="text/javascript" src="http://wpa.b.qq.com/cgi/wpa.php?key=XzkzODAyMTM0OV8zNTY4OTdfNDAwODc4NjMzM18"></script>
                </span>
                <a>Copyright © 2015 Powered by DuoLian：IMD</a><dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="ASPxLabel" Visible="false"></dx:ASPxLabel>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:Timer ID="SessionTimer" runat="server" OnInit="SessionTimer_Init" OnTick="SessionTimer_Tick"></asp:Timer>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>

        <div region="center" title="工作区" id="work">
            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
                    <div style="width: 1600px">
                        <asp:HyperLink ID="MainNewNo1" runat="server" ForeColor="#FF3300" NavigateUrl="~/News.aspx" Target="_blank" Font-Size="11pt">*</asp:HyperLink>
                    </div>
                    <div style="width: 1600px">
                        <asp:HyperLink ID="MainNewNo2" runat="server" ForeColor="#000000" NavigateUrl="~/News.aspx" Target="_blank" Font-Size="11pt">*</asp:HyperLink>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="SessionTimer" EventName="Tick" />
                </Triggers>
            </asp:UpdatePanel>

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
                <div title="首页" iconcls="icon-dl_zy" style="overflow: auto">
                    <%--<iframe src="https://www.wenjuan.com/s/i2aQrq/" style="height:600px;width:1000px;border:none"></iframe>--%>
                    <br />
<%--                    <h3 style="color: orangered; font-size: 14pt">2016年度客户满意度调查,<a href="../other/2016DiaoCha.aspx" style="color: green; font-size: 14pt" target="blank">参与调查>>>>>></a>
                    </h3>--%>
                    <h3 style="color: orangered; font-size: 14pt"><a href="../other/RedirectToNewOPV2.aspx" style="color: green; font-size: 14pt" target="_self">使用新版网上订单</a>
                    </h3>
                    <h3 style="color: red; font-size: 20pt"><a href="http://dl.duolian.com:1234/html/help/help.html" style="color: red; font-size: 30pt" target="_blank">新版网上订单操作说明</a>
                    </h3>
                    <br />
                    <br />
                    <%--<h3 style="color: orangered; font-size: 14pt">我要开通微信提醒,<a href="../WXRegister.aspx" style="color: green; font-size: 14pt" target="_blank">登记入口>>>>>> </a></h3>
                    <a>【主要用于接收网上订单系统登录验证码,订单状态消息,以及移动端应用】</a>
                    <br />--%>
                    <br />
                    <%--<h3>2016-04-06更新</h3>
                    <a style="font-size: 11pt;">★系统调整.</a><br />
                    <a style="font-size: 11pt;">★调整 '订单执行情况表'的取数逻辑,并增加退货信息,所有数据均来源已完成发货或退货操作的单据信息.完善状态分类显示区别.</a><br />
                    <h3>2016-04-05更新</h3>
                    <a style="font-size: 11pt;">★调整所有订单的数量输入方式,取消滑鼠的上下滚轮调整数量功能,不输入数量系统默认数量为0.</a><br />
                    <a style="font-size: 11pt;">★调整 '账单明细' 的显示高度,减小表体高度;历史订单增加时间显示(下单时间,审核时间).</a><br />
                    <h3>2016-04-01更新</h3>
                    <a style="font-size: 11pt;">★统一网上订单与正式订单的编号.</a><br />
                    <h3>2016-03-29更新</h3>
                    <a style="font-size: 11pt;">★添加用户取回功能(订单提交后在待审核订单中,如果该订单没有被接单员接收处理,则用户可以自行取回再进行编辑后提交,取回后可在被驳回订单中找到该订单,并进行编辑).</a><br />
                    <a style="font-size: 11pt;">★修复临时订单自动保存会被其他用户的临时订单自动保存的信息覆盖的问题.</a><br />
                    <a style="font-size: 11pt;">★订单执行情况表由下单日期查询变更为下单时间查询,增加审核时间查询.</a><br />
                    <a style="font-size: 11pt;">★增加特殊订单明细显示.</a><br />
                    <a style="font-size: 11pt;">★调整待审核订单明细表的合计金额显示.</a><br />
                    <a style="font-size: 11pt;">★修复修改订单后再次提交订单时发货地址信息保存缺失.</a>
                    <h3>2016-03-28更新</h3>
                    <a style="font-size: 11pt;">★普通订单增加保存临时订单功能,使用该功能,可以将在制的订单保存为临时订单,保存后可以在任意时候通过按钮"临时订单",在弹出界面中选择对应的订单,点击确定或者双击即可完成临时订单信息提取.<br />
                        &nbsp;&nbsp;&nbsp;&nbsp;保存的临时订单不会自动删除,请不需要使用该订单后手工删除,避免存储太多临时订单造成混乱.</a><br />
                    <a style="font-size: 11pt;">★普通订单增加自动保存临时订单功能,在添加完商品后会自动保存临时订单,名称为"系统自动保存".</a><br />
                    <br />
                    <a style="font-size: 11pt;">★样品资料订单可以关联任意订单(不再需要关联通过审核的订单),请各位关联准确的主订单号.</a><br />--%>
                </div>

            </div>

        </div>
        <div region="west" class="west" title="导航菜单" style="width: 160px">
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
            text: "我的会员", iconCls: "icon-dl_hy",
            children: [{
                text: "首页", iconCls: "icon-dl_zy",
                attributes: {
                    url: "../DlDefault.aspx"
                }, url: "../DlDefault.aspx"
            }, {
                text: "普通订单", iconCls: "icon-dl_hy",
                children: [{
                    text: "购物", iconCls: "icon-dl_gwc",
                    attributes: {
                        url: "../OrderFrame.aspx"
                    }, url: "../OrderFrame.aspx"
                },
    {
        text: "待审核订单", iconCls: "icon-dl_ybc",
        attributes: {
            url: "../CheckPendingOrder.aspx"
        }, url: "../CheckPendingOrder.aspx"
    }, {
        text: "被驳回订单", iconCls: "icon-dl_dsh",
        attributes: {
            url: "../PendingOrder.aspx"
        }, url: "../PendingOrder.aspx"
    }, {
        text: "待确认订单", iconCls: "icon-dl_dqr",
        attributes: {
            url: "../OrderConfirm.aspx"
        }, url: "../OrderConfirm.aspx"
    }, {
        text: "历史订单", iconCls: "icon-dl_lsdd",
        attributes: {
            url: "../PreviousOrderFrame.aspx"
        }, url: "../PreviousOrderFrame.aspx"
    }
                ]
            },
            {
                text: "酬宾订单", iconCls: "icon-dl_hy",
                children: [{

                    text: "酬宾订单", iconCls: "icon-dl_ddzx",
                    attributes: {
                        url: "../PreOrder.aspx"
                    }, url: "../PreOrder.aspx"
                }, {

                    text: "酬宾订单查询", iconCls: "icon-dl_lsdd",
                    attributes: {
                        url: "../PreYOrderSearch.aspx"
                    }, url: "../PreYOrderSearch.aspx"
                }]
            },
            {
                text: "特殊订单", iconCls: "icon-dl_hy",
                children: [{

                    text: "特殊订单", iconCls: "icon-dl_gw",
                    attributes: {
                        url: "../XPreOrder.aspx"
                    }, url: "../XPreOrder.aspx"
                }, {

                    text: "特殊订单查询", iconCls: "icon-dl_lsdd",
                    attributes: {
                        url: "../PreXOrderSearch.aspx"
                    }, url: "../PreXOrderSearch.aspx"
                }]
            },

                        {
                            text: "我的账户", iconCls: "icon-dl_user",
                            state: "closed",
                            children: [{
                                text: "个人信息", iconCls: "icon-dl_grxx",
                                attributes: {
                                    url: "../Blank.aspx"
                                }, url: "../Blank.aspx"
                            }, {
                                text: "账户安全", iconCls: "icon-dl_zhaq",
                                attributes: {
                                    url: "../Blank.aspx"
                                }, url: "../Blank.aspx"
                            }, {
                                text: "收货地址", iconCls: "icon-dl_shdz",
                                attributes: {
                                    url: "../UserAddress.aspx"
                                }, url: "../UserAddress.aspx"
                            }
                            ]
                        },

            {
                text: "报表", iconCls: "icon-dl_bb", state: "closed",
                children: [{
                    text: "我的对账单", iconCls: "icon-dl_dzd",
                    attributes: {
                        url: "../SOAFrame.aspx"
                    }, url: "../SOAFrame.aspx"
                }, {
                    text: "账单明细", iconCls: "icon-dl_zdmx",
                    attributes: {
                        url: "../SOADetail.aspx"
                    }, url: "../SOADetail.aspx"
                }, {
                    text: "订单执行情况", iconCls: "icon-dl_shdz",
                    attributes: {
                        url: "../OrderExecute.aspx"
                    }, url: "../OrderExecute.aspx"
                }]
            },
                {
                    text: "系统设置", iconCls: "icon-dl_pz",
                    attributes: {
                        url: "../CustomerSetting.aspx"
                    }, url: "../CustomerSetting.aspx"
                },
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
