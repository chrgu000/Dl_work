<%@ WebHandler Language="C#" Class="test" %>

using System;
using System.Web;

public class test : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        string datagridjson = "1234";
        datagridjson = "[{text: \"常用商品\", children: [{text: \"常用(系统),购买次数\"  },{";
        datagridjson = datagridjson + "text: \"常用(系统),名称,规格\", children: [{text: \"购物\" }]}]}]";

        datagridjson = "[{text:\"常用商品\",attributes:{cInvCode:\"00\"},children[{text:\"常用(系统),购买次数\",attributes:{cInvCode:\"0001\"},text:\"常用(系统),名称,规格\",attributes:{cInvCode:\"0002\"}}],text:\"多联牌\",attributes:{cInvCode:\"01\"},children[{text:\"阻燃PVC电线套管系列\",attributes:{cInvCode:\"0101\"},children[{text:\"白色阻燃PVC电线套管系列\",attributes:{cInvCode:\"010101\"}}]}]}]";

        datagridjson = "[{text:\"常用商品\",attributes:{cInvCode:\"00\"}},{text:\"多联牌\",attributes:{cInvCode:\"01\"}}]";
        
        //输出 json 字符串    
        context.Response.ContentType = "text/plain";
        context.Response.Write(datagridjson.ToString());
        context.Response.End();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}