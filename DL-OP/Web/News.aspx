<%@ Page Language="C#" AutoEventWireup="true" CodeFile="News.aspx.cs" Inherits="News" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>公司公告通知</title>
    <style type="text/css">
        .auto-style1 {
            font-size: large;
            color: #FF0000;
        }

        .auto-style2 {
            color: #FF0000;
        }

        .auto-style3 {
            color: #000000;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <dx:ASPxGridView ID="NewsGrid" runat="server" AutoGenerateColumns="False" EnableTheming="True" Theme="Aqua" OnCustomUnboundColumnData="GridViewShippingMethod_CustomUnboundColumnData">
            <Columns>
                <dx:GridViewDataTextColumn Caption="序号" ReadOnly="True" VisibleIndex="0" Width="60px" FieldName="hh" UnboundType="Integer">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataMemoColumn Caption="内容" FieldName="srtNewsContent" ReadOnly="True" VisibleIndex="1" Width="600px">
                </dx:GridViewDataMemoColumn>
                <dx:GridViewDataTextColumn Caption="发布时间" FieldName="datCreateTime" ReadOnly="True" VisibleIndex="2" Width="160px">
                </dx:GridViewDataTextColumn>
            </Columns>
            <SettingsPager Mode="ShowAllRecords" Visible="False">
            </SettingsPager>
            <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
        </dx:ASPxGridView>
        <h3 class="auto-style2">★多账户操作说明：</h3>
<p>1：一个手机号码只能绑定一个账号（主账户或者子账户）</p>
<p>2：登陆时，需要输入手机号码，验证码只推送给登陆界面输入的手机号码（会先验证该账户是否绑定此手机号码，如果绑定，则推送验证码）。</p>
<p>3：单据审核、驳回短信推送给当前操作的账户绑定的手机号码和主账户绑定的手机号码。</p>
<p>4：账户之前的基础信息是共享的，如地址信息，开票账户，信用额。</p>
<p>5：账户之间的单据是独立的，比如账户A做的订单或者预订单，只能被A账户取回/修改、参照提交，其他账户无法看到，但参照历史订单、临时订单、样品资料和查看历史订单以及报表的内容可以共享。</p>

        <h3 class="auto-style2">★关于 对账单 的操作流程</h3>
        <p>1.进入我的对账单:左侧导航菜单中"报表"→"我的对账单",如下图</p>
        <p>
            <img alt="" src="images/对账单1.jpg" />
        </p>
        <p>2.查询对账单,并点击查看,如下图</p>
        <p>
            <img alt="" src="images/对账单2.jpg" />
        </p>
        <p>3.对'对账单'进行确认,如下图</p>
        <p>
            <img alt="" src="images/对账单3.jpg" />
        </p>
        <p>PS:如超过30天未对'对账单'进行确认,将不能使用系统的购物和下单功能,请及时进行账单确认,如有任何疑问请联系多联客服.</p>
        <a style="color:red">----------------------------------------------------------------------------------------------------------------------------------------</a>
        <p>
            关于信用额说明:酬宾订单的信用额=普通购物信用额-已下单酬宾订单金额；参照酬宾订单信用额=普通购物信用额</br>
        普通购物信用额=信用总额-已下单金额（普通订单、参照酬宾订单、参照特殊订单）
        </p>
        <h3 class="auto-style2">★关于 预订单(酬宾订单) 的下单操作流程</h3>
        <p>当顾客在销售活动期间，因为库存不足或者其他原因无法建立普通订单，需要享受当前销售活动的优惠政策时，可使用该类型订单。该类型为需求预订单，</br>不做实际订单处理（不发货），以后在酬宾订单未关闭前可使用‘普通订单’→‘购物’中的‘参照酬宾订单’来建立实际的购买订单并享受该活动的相关优惠政策。</br>简单流程：下预订单(酬宾订单)→预订单(酬宾订单)审核之后→参照酬宾订单→参照酬宾订单审核之后(发货)</p>
        <p>
            关于信用额说明:酬宾订单的信用额=普通购物信用额-已下单酬宾订单金额；参照酬宾订单信用额=普通购物信用额</br>
        普通购物信用额=信用总额-已下单金额（普通订单、参照酬宾订单、参照特殊订单）
        </p>
        <p>1.1 选择酬宾订单</p>
        <p>
            <img alt="" src="HelpSop/特殊订单和酬宾订单.png" />
        </p>
        <p>1.2 酬宾订单制单</p>
        <p>
            <img alt="" src="HelpSop/酬宾订单表体.png" />
        </p>
        <p>1.3 填写订单表头信息，选择开票单位</p>
        <p>
            <img alt="" src="HelpSop/开票单位.png" />
        </p>
        <p>1.4 选择商品</p>
        <p>
            <img alt="" src="HelpSop/选择商品.png" />
        </p>
        <p>▶1.4.1	刷新目录：此按钮功能是解决在选择商品分类的时候由于意外情况（网络延迟等）造成的界面卡住，点击后将刷新 4选择商品大类的选择界面。</p>
        <p>▶1.4.2	确定：选择好商品之后点击 确定 提交。</p>
        <p>▶1.4.3	取消：退出商品选择界面，返回订单界面。</p>
        <p>▶1.4.4	清除所有选择项：清除所有已经选择的商品。</p>
        <p>▶1.4.5	选择商品大类：商品的分类信息，点击选择后，右侧的（7.商品栏目）将显示该分类下的所有商品信息。</p>
        <p>▶1.4.6   查询：该功能是对 7.商品栏目 中的所有商品进行筛选(名称和规格)。</p>
        <p>▶1.4.7   商品信息</p>
        <p>1.5 输入商品数量信息，并保存商品信息。</p>
        <p>
            <img alt="" src="HelpSop/酬宾订单表体操作.png" />
        </p>
        <p>1.6 保存订单信息（提交订单），等待订单员审核。(可在酬宾订单查询中查看)</p>
        <br />
        <h1>2.参照酬宾订单</h1>
        <p>参照酬宾订单流程图</p>
        <p>
            <img alt="" src="HelpSop/参照酬宾订单流程.png" />
        </p>

        <p>2.1	选择酬宾订单。<a style="color: red">注意：此订单模版的前提是已经通过了'酬宾订单'的审核！</a></p>
        <p>
            <img alt="" src="HelpSop/购物.png" />
        </p>
        <p>2.2 酬宾订单模版</p>
        <p>
            <img alt="" src="HelpSop/普通订单.png" />
        </p>
        <p>2.3 填写订单模版表头信息，选择开票单位等信息。</p>
        <p>
            <img alt="" src="HelpSop/开票单位.png" />
        </p>
        <p>2.4 选择商品。<a style="color: red">注意：'参照酬宾订单'模版只能选择'酬宾订单'中的订单信息</a></p>
        <p>
            <img alt="" src="HelpSop/参照订单.png" />
        </p>
        <p>▶2.4.1	刷新目录：此按钮功能是解决在选择商品分类的时候由于意外情况（网络延迟等）造成的界面卡住，点击后将刷新 5选择商品大类的选择界面。</p>
        <p>▶2.4.2	确定：选择好商品之后点击 确定 提交。</p>
        <p>▶2.4.3	取消：退出商品选择界面，返回订单界面。</p>
        <p>▶2.4.4	清除所有选择项：清除所有已经选择的商品。</p>
        <p>2.5 输入商品数量信息，并保存商品信息。</p>
        <p>
            <img alt="" src="HelpSop/酬宾订单表体操作.png" />
        </p>
        <p>2.6 保存订单信息（提交订单），等待订单员审核。(可在待审核订单中查看)</p>
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        更新日志：<br />
        2016-3-5日,更新:历史订单明细中可查询合计金额<br />
        2016-3-6日,更新:添加报表功能,&quot;订单执行情况表&quot;<br />
        2016-3-8日,更新:优化历史订单的查询(修复点击订单号查看详细信息时会重置查询的条件和表单数据)<br />
        2016-3-9日,更新:添加检测功能(顾客提交订单时,检测开票单位名称与编码是否一致)<br />
        2016-3-10日,更新:优化订单执行情况表(添加总合计,单个订单的合计颜色,合计排序,添加开票单位筛选)<br />
        2016-3-16日,更新:更新选择商品搜索方式,增加常用商品,价格检测,库存颜色标识等<br />
        2016-3-17日,更新:增加'参照历史订单'的重复购买功能<br />
        <br />
        <strong><span>♠如出现<a style="color: red">'网页正在维护中'</a>,可以使用回车键上面的回退键 <a style="color: red">←</a>来返回购物界面,并且任意添加一个商品,之前保存的所有的选择的商品将刷新出来.</span></strong><br />
        <strong><span class="auto-style1">♠因为网速等原因,在选择商品分类的时候出现比较慢的情况,可以通过查询功能来快速选择商品。具体操作如下：</span></strong><br />
        如我想购买以下两样商品： 
        <br />
        1) <span class="auto-style2">环保排水管（B型）规格：110x2.8/3.4</span> （该商品位于：多联牌→PVC-U环保排水管系列→白色PVC-U环保排水管材系列→环保排水管（B型）这个产品分类下） 
        <br />
        2) <span class="auto-style2">白色PP-R环保冷水管S4 规格：110x12.3</span> （该商品位于：多联牌→PP-R环保冷热给水管系列产品→白色PP-R环保冷热给水管材系列产品→白色PP-R环保冷水管S4这个产品分类下）<span class="auto-style2"><br />
            ★</span><span class="auto-style3">搜索商品操作步骤</span>： 
        <br />
        A． 使用<span class="auto-style2"><strong>查询功能</strong></span>查找商品1：环保排水管（B型）规格：110x2.8/3.4
        <br />
        1. 先选择产品分类（确定要在那个分类下查找商品） 
        <br />
        2. 使用查询功能进行查找,输入要查找的商品的关键字，这里输入的是 &#39;<span class="auto-style2">排水管 2.8</span>&#39;，查询结果如下<br />
        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/查询A1.jpg" />
        <br />
        3. 使用过滤功能，在过滤栏上输入需要查找的信息，输入完毕后按回车显示结果，在结果中找到需要的商品并勾选,完成商品的选择。<br />
        （该功能可以和第2步的查询结合使用，过滤查询的结果，使查询结果更精确，也可以单独使用。）<br />
        <asp:Image ID="Image2" runat="server" ImageUrl="~/images/查询A2.jpg" />
        <br />
        B． 使用过滤功能查找商品2：白色PP-R环保冷水管S4 规格：110x12.3<br />
        1. 先选择产品分类（确定要在那个分类下查找商品） 
        <br />
        2. 使用过滤功能进行查找。输入关键字，按回车后，显示结果，找到并勾选需要的商品。<br />
        <asp:Image ID="Image3" runat="server" ImageUrl="~/images/查询B1.jpg" />
        <br />
        3. 过滤功能的过滤关系选择。默认为&#39;包含&#39;，如下图。<br />
        <asp:Image ID="Image4" runat="server" ImageUrl="~/images/查询B2.jpg" />
        <br />
        <br />
    </form>
</body>
</html>
