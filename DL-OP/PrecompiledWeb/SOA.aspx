<%@ page language="C#" autoeventwireup="true" inherits="SOA, dlopwebdll" enableviewstate="false" %>

<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">


.dxeButtonEditSys 
{
    border-collapse: separate;
    border-spacing: 1px;
}

.dxeTextBoxSys,
.dxeButtonEditSys 
{
    width: 170px;
}

.dxeButtonEditSys td.dxic 
{
    padding: 2px 2px 1px 2px;
    overflow: hidden;
}

.dxeButtonEditSys .dxeEditAreaSys,
.dxeButtonEditSys td.dxic,
.dxeTextBoxSys td.dxic,
.dxeMemoSys td,
.dxeEditAreaSys
{
	width: 100%;
}

.dxeMemoEditAreaSys, /*Bootstrap correction*/
input[type="text"].dxeEditAreaSys, /*Bootstrap correction*/
input[type="password"].dxeEditAreaSys /*Bootstrap correction*/
{
    display: block;
    -webkit-box-shadow: none;
    -moz-box-shadow: none;
    box-shadow: none;
    -webkit-transition: none;
    -moz-transition: none;
    -o-transition: none;
    transition: none;
	-webkit-border-radius: 0px;
    -moz-border-radius: 0px;
    border-radius: 0px;
}
.dxeEditAreaSys,
.dxeMemoEditAreaSys, /*Bootstrap correction*/
input[type="text"].dxeEditAreaSys, /*Bootstrap correction*/
input[type="password"].dxeEditAreaSys /*Bootstrap correction*/
{
    font: inherit;
    line-height: normal;
    outline: 0;
}

input[type="text"].dxeEditAreaSys, /*Bootstrap correction*/
input[type="password"].dxeEditAreaSys /*Bootstrap correction*/
{
    margin-top: 0;
    margin-bottom: 0;
}
.dxeEditAreaSys,
input[type="text"].dxeEditAreaSys, /*Bootstrap correction*/
input[type="password"].dxeEditAreaSys /*Bootstrap correction*/
{
    padding: 0px 1px 0px 0px; /* B146658 */
}
.dxic .dxeEditAreaSys
{
	padding: 0px 1px 0px 0px;
}
.dxeEditAreaSys 
{
    border: 0px!important;
    background-position: 0 0; /* iOS Safari */
    -webkit-box-sizing: content-box; /*Bootstrap correction*/
    -moz-box-sizing: content-box; /*Bootstrap correction*/
    box-sizing: content-box; /*Bootstrap correction*/
}

.dxeButtonEditSys .dxeButton
{
    line-height: 100%;
}

a.dxbButtonSys
{
    border: 0;
    background: none;
    padding: 0;
}
.dxbButtonSys /*Bootstrap correction*/
{
    -webkit-box-sizing: content-box;
    -moz-box-sizing: content-box;
    box-sizing: content-box;
}
.dxbButtonSys
{
	cursor: pointer;
	display: inline-block;
	text-align: center;
	white-space: nowrap;
}
a.dxbButtonSys > span
{
    text-decoration: inherit;
}
        .新建样式1 {
            font-family: 微软雅黑;
        }
        </style>
</head>
<body>
    <form id="form1" runat="server">
                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server"><ContentTemplate>
        <div>
 <div><a style="float:left;"> 账单年:</a>
     <div style="float:left;">
         <dx:ASPxComboBox ID="ComboPeriodYear" ClientInstanceName="ComboPeriodYear" runat="server" EnableTheming="True" Theme="SoftOrange" Width="60px" SelectedIndex="0">
             <Items>
                 <dx:ListEditItem Text="2016" Value="2016" Selected="True" />
                 <dx:ListEditItem Text="2017" Value="2017" />
                 <dx:ListEditItem Text="2018" Value="2018" />
                 <dx:ListEditItem Text="2019" Value="2019" />
                 <dx:ListEditItem Text="2020" Value="2020" />
             </Items>
             <ClearButton Visibility="False">
             </ClearButton>
         </dx:ASPxComboBox>
            </div>
           <a style="float:left;"> 账单期间:</a>
     <div style="float:left;">
         <dx:ASPxComboBox ID="ComboPeriod" ClientInstanceName="ComboPeriod" runat="server" EnableTheming="True" Theme="SoftOrange" Width="60px" SelectedIndex="0">
             <Items>
                 <dx:ListEditItem Selected="True" Text="全部" Value="0" />
                 <dx:ListEditItem Text="1" Value="1" />
                 <dx:ListEditItem Text="2" Value="2" />
                 <dx:ListEditItem Text="3" Value="3" />
                 <dx:ListEditItem Text="4" Value="4" />
                 <dx:ListEditItem Text="5" Value="5" />
                 <dx:ListEditItem Text="6" Value="6" />
                 <dx:ListEditItem Text="7" Value="7" />
                 <dx:ListEditItem Text="8" Value="8" />
                 <dx:ListEditItem Text="9" Value="9" />
                 <dx:ListEditItem Text="10" Value="10" />
                 <dx:ListEditItem Text="11" Value="11" />
                 <dx:ListEditItem Text="12" Value="12" />
             </Items>
             <ClearButton Visibility="False">
             </ClearButton>
         </dx:ASPxComboBox>
            </div>
            <a style="float:left;">单位:</a>     <div style="float:left;">
                <dx:ASPxComboBox ID="Comboccusname" ClientInstanceName="Comboccusname" runat="server" EnableTheming="True" 
                    Theme="SoftOrange" Width="160px"   >
                    <ClearButton Visibility="False">
                    </ClearButton>
                </dx:ASPxComboBox>
            </div>
             <a style="float:left;">是否确认:</a>     <div style="float:left;padding-right:15px">
                <dx:ASPxComboBox ID="ComboCheck" ClientInstanceName="ComboCheck" runat="server" EnableTheming="True" Theme="SoftOrange" Width="60px" SelectedIndex="0">
                    <Items>
                        <dx:ListEditItem Selected="True" Text="全部" Value="-1" />
                        <dx:ListEditItem Text="未确认" Value="0" />
                        <dx:ListEditItem Text="已确认" Value="1" />
                    </Items>
                    <ClearButton Visibility="False">
                    </ClearButton>
                </dx:ASPxComboBox>
            </div>
           <div style="padding-left:15px">
               <dx:ASPxButton ID="BtnOk" ClientInstanceName="BtnOk" runat="server" Text="查    询" Theme="SoftOrange" Width="80px" OnClick="BtnOk_Click">
               </dx:ASPxButton>
            </div></div>
        </div>
    <div>   <dx:ASPxGridView ID="SOAGrid" ClientInstanceName="SOAGrid" runat="server" AutoGenerateColumns="False" EnableTheming="True" Theme="SoftOrange"
         KeyFieldName="lngSOAid"  >
                <Columns>
                    <dx:GridViewDataTextColumn Caption="顾客编号" FieldName="ccuscode" VisibleIndex="1" Width="80px" Visible="False">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="顾客名称" FieldName="ccusname" VisibleIndex="3" Width="170px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="账单截至日期" FieldName="strEndDate" VisibleIndex="2" Width="90px">
                        <PropertiesTextEdit DisplayFormatString="yyyy-MM-dd">
                        </PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="账单接收时间" FieldName="datSendTime" VisibleIndex="4" Width="140px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="账单金额" FieldName="dblAmount" VisibleIndex="5" Width="90px">
                        <PropertiesTextEdit DisplayFormatString="{0:F}">
                        </PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="发送人" FieldName="strOper" VisibleIndex="6" Width="60px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="确认时间" FieldName="datCheckTime1" VisibleIndex="7" Width="140px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="期间" FieldName="intPeriod" VisibleIndex="9" Width="40px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataHyperLinkColumn Caption=" " FieldName="lngSOAid" VisibleIndex="0" Width="65px">
                        <PropertiesHyperLinkEdit NavigateUrlFormatString="SOAConfim.aspx?id={0}" Target="OrderCenter" Text="查看账单">
                            <Style ForeColor="#66FF33">
                            </Style>
                        </PropertiesHyperLinkEdit>
                    </dx:GridViewDataHyperLinkColumn>
                    <dx:GridViewDataTextColumn Caption="年度" ReadOnly="True" VisibleIndex="8" FieldName="intPeriodYear" Width="50px">
                    </dx:GridViewDataTextColumn>
                </Columns>
                <SettingsBehavior AllowFocusedRow="True" FilterRowMode="OnClick" AllowSort="False" />
                <SettingsPager Visible="False" Mode="ShowAllRecords">
                </SettingsPager>
                <Settings VerticalScrollableHeight="180" VerticalScrollBarMode="Visible" ShowFilterRowMenu="True" />
                <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
            </dx:ASPxGridView>
    
    </div>

        <div id="DivSOA" runat="server" >

        </div>
            </ContentTemplate>
                </asp:UpdatePanel>
                       <asp:HiddenField ID="HFComboccusname"  runat="server" />
    </form>
</body>
</html>
