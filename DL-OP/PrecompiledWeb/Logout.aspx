﻿<%@ page language="C#" autoeventwireup="true" inherits="Logout, dlopwebdll" enableviewstate="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<%
    Response.Buffer = true;
    Response.ExpiresAbsolute = System.DateTime.Now.AddSeconds(-1);
    Response.Expires = 0;
    Response.CacheControl = "no-cache";
    Response.AddHeader("Pragma", "No-Cache");
%>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
    </form>
</body>
</html>
