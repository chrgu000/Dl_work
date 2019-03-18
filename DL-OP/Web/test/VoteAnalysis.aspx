<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VoteAnalysis.aspx.cs" Inherits="test_VoteAnalysis" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1">
            <Columns>
                <asp:BoundField DataField="votetime" HeaderText="时间">
                <ItemStyle Width="230px" />
                </asp:BoundField>
                <asp:BoundField DataField="no1" HeaderText="no1">
                <ItemStyle Width="80px" />
                </asp:BoundField>
                <asp:BoundField DataField="dl" HeaderText="多联">
                <ItemStyle Width="80px" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:UFDATA_001_2015ConnectionString %>" SelectCommand="DlProc_VoteAnalysis" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
    
        <br />
    
    </div>
    </form>
</body>
</html>
