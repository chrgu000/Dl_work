using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
/// UU 的摘要说明
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
// [System.Web.Script.Services.ScriptService]
public class UU : System.Web.Services.WebService
{

    public UU()
    {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }
    #region 发送UU普通消息
    /// <summary>
    /// 
    /// </summary>
    /// <param name="toUser">接收消息的用户编码，用"|"隔开</param>
    /// <param name="title">消息标题</param>
    /// <param name="content">消息内容</param>
    [WebMethod(Description = "发送UU普通消息")]
    public void SendMsg(string toUser, string title, string content)
    {
        UUSendMessage.UUSendMessage UU = new UUSendMessage.UUSendMessage();
        UU.SendMsg(toUser, title, content);
    }
    #endregion

}
