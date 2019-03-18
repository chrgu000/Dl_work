<%@ page language="C#" autoeventwireup="true" inherits="PreOrderTimeSetting, dlopwebdll" enableviewstate="false" %>

<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        当前设置:<dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="ASPxLabel">
        </dx:ASPxLabel>
        <br />
        开始时间:<dx:ASPxDateEdit ID="ASPxDateEdit1" runat="server" AllowNull="False" EditFormat="DateTime" EnableTheming="True" Theme="SoftOrange" DateOnError="Today">
             <CalendarProperties DayNameFormat="Short" FirstDayOfWeek="Monday">
                            </CalendarProperties>
                            <TimeSectionProperties Visible="True" ShowMinuteHand="False">
                                <TimeEditProperties AllowNull="False" >
                                </TimeEditProperties>
                            </TimeSectionProperties>
        </dx:ASPxDateEdit>
        <br />
        结束时间:<dx:ASPxDateEdit ID="ASPxDateEdit2" runat="server" AllowNull="False" EditFormat="DateTime" EnableTheming="True" Theme="SoftOrange" DateOnError="Today">
             <CalendarProperties DayNameFormat="Short" FirstDayOfWeek="Monday">
                            </CalendarProperties>
                            <TimeSectionProperties Visible="True" ShowMinuteHand="False">
                                <TimeEditProperties AllowNull="False" >
                                </TimeEditProperties>
                            </TimeSectionProperties>
        </dx:ASPxDateEdit>
        <br />
        <br />
        <dx:ASPxButton ID="ASPxButton1" runat="server" OnClick="ASPxButton1_Click" Text="保存" Theme="Youthful">
        </dx:ASPxButton>
    
    </div>
    </form>
</body>
</html>
