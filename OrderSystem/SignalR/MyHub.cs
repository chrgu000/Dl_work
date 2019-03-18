using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.SessionState;
using System.Data;



namespace SignalR
{

    [HubName("getMessage")]
    public class MyHub : Hub
    {
        public static JObject UserLists = new JObject();
        public void Hello()
        {
            //Clients.All.hello();
            Clients.All.tt();
        }
        public static void send(string msg)
        {
            if (string.IsNullOrEmpty(msg))
            {
                msg = "http://www.163.com";
            }
            //  Clients.Others.sendmsg(msg);
            JObject jo = new JObject();
            jo["signalr_id"] = 11;
            jo["dd"] = "dfssdffffff";
            jo["name"] = UserLists["id"];
            jo["id"] = System.Web.HttpContext.Current.Session["lngopUserId"].ToString();
            string strLoginName = string.Empty;
            if (System.Web.HttpContext.Current.Session["strLoginName"] == null)
            {
                // jo["strLoginName"] = "null";
                strLoginName = "null";
            }
            else
            {
                strLoginName = System.Web.HttpContext.Current.Session["strLoginName"].ToString();
                //jo["strLoginName"] = System.Web.HttpContext.Current.Session["strLoginName"].ToString();

            }
            var context = GlobalHost.ConnectionManager.GetHubContext<MyHub>();

            context.Clients.All.receviceMsg(jo);
            //   Clients.All.receviceMsg(jo);
        }

        public void send1()
        {
            JObject jo = new JObject();
            jo["signalr_id"] = 11;
            jo["dd"] = "dddddddddddddddddd";
            jo["name"] = "namenamenamenamenamenamename";
            jo["time"] = DateTime.Now.ToString();
            //string strLoginName = "strLoginNamestrLoginNamestrLoginNamestrLoginNamestrLoginName";
            //if (System.Web.HttpContext.Current.Session["strLoginName"] == null)
            //{
            //    // jo["strLoginName"] = "null";
            //    strLoginName = "null";
            //}
            //else
            //{
            //    strLoginName = System.Web.HttpContext.Current.Session["strLoginName"].ToString();
            //    //jo["strLoginName"] = System.Web.HttpContext.Current.Session["strLoginName"].ToString();

            //}
            //   jo["strLoginName"] = get_session() ;
            jo["flag"] = "1";
            Clients.All.receviceMsg(JsonConvert.SerializeObject(jo));
            //var context = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
            //context.Clients.All.receviceMsg(jo);

        }

        public string get_session()
        {

            return HttpContext.Current.Session["strLoginName"].ToString();
        }

        #region 用户登录成进index后，记录SignalR_ID和Session里的UserID
        public void signalR_Login(JObject info)
        {
            if (UserLists[info["strAllAcount"].ToString()] != null)
            {
                JToken jo = UserLists[info["strAllAcount"].ToString()];
                string sessionid = jo["sessionId"].ToString();
                if (sessionid != (string)info["sessionId"])
                {
                    string touser = jo["signalRId"].ToString();
                    JObject j = new JObject();
                    j["flag"] = "99";
                    var context = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
                    context.Clients.Client(touser).exit(JsonConvert.SerializeObject(j));

                    // sendToOneClient(touser,j);
                    //  Clients.Client(touser).sendMsg("重复登录!");
                    // var context = GlobalHost.ConnectionManager.GetHubContext<MyHub>();

                    // context.Clients.User(UserLists[info["lngopUserId"].ToString()].ToString()).receviceMsg("重复登录！");
                }
            }

            UserLists[info["strAllAcount"].ToString()] = info;


        }
        #endregion

        public void getSignalRId(string SignalRId)
        {
            Clients.Caller.getSignalRId();
        }

        #region 对一个客户端发送消息
        public void sendToOneClient(string signalRId, JObject jo)
        {
            //Clients.Client(signalRId).sendMsg("ccccccccccccc");
            //  Clients.User(signalRId).sendMsg("vvvvvvvvvvvvvvvvvv");
            var context = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
            context.Clients.Client(signalRId).sendMsg(JsonConvert.SerializeObject(jo));
        }
        #endregion

        #region 发送消息，给指定客户


        public void sendMsg(string signalRId, JObject jo)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
            context.Clients.Client(signalRId).sendMsg(JsonConvert.SerializeObject(jo));
        }
        #endregion

        #region 发送消息，给全部客户
        public void sendMsg(JObject jo)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
            context.Clients.All.sendMsg(JsonConvert.SerializeObject(jo));
        }
        #endregion


        //public void getAllClient() {
        //    Clients.Caller.sendMsg(JsonConvert.SerializeObject(UserLists));
        //}

        public void sendMsgForCheck()
        {
            JObject jo = new JObject();
            jo["title"] = "asfd";
            jo["content"] = DateTime.Now.ToString();

            //  sendMsg(jo);
            if (UserLists != null)
            {
                JObject NewUserLists = new JObject();

                foreach (var item in UserLists)
                {
                    int times = (int)item.Value["checkTimes"];
                    times += 1;
                    if (times < 5)
                    {
                        item.Value["checkTimes"] = times;
                        NewUserLists[item.Key]=item.Value;
                    }
                }
                UserLists = NewUserLists;
            }
            var context = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
            context.Clients.All.sendSignalRId();
        }

        #region 接收前台发回来的SignalRId，检测UserLists里的用户是否在线，checkTimes超过5次则删除该用户
        public void checkOnlineUsers(string SignalRId)
        {
            if (UserLists != null)
            {
                foreach (var item in UserLists)
                {
                    if (item.Value["signalRId"].ToString()==SignalRId)
                    {
                        item.Value["checkTimes"] = 0;
                    }
                }
            }
        }
        #endregion

        public JObject getAllClient()
        {
            return UserLists;
        }
    }
}