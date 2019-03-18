using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Model;
using System.Data;
using System.Data.SqlClient;
using DevExpress.Web;
using DevExpress.Web.ASPxTreeList;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public partial class testtttt : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DataTable RelateU8NO = new SearchManager().DL_ComboCustomerU8NOBySel("010101");
        ComboRelateU8NO.DataSource = RelateU8NO;
        ComboRelateU8NO.DataBind();
    }


    protected void TreeList_CustomJSProperties(object sender, TreeListCustomJSPropertiesEventArgs e)
    {
        ASPxTreeList treeList = sender as ASPxTreeList;
        Hashtable nameTable = new Hashtable();
        foreach (TreeListNode node in treeList.GetVisibleNodes())
            //nameTable.Add(node.Key, string.Format("{0} {1}", node["vsimpleName"], node["vdescription"]));
            nameTable.Add(node.Key, string.Format("{0}", node["vdescription"]));
        e.Properties["cpvsimpleName"] = nameTable;
        treeList.ExpandToLevel(0);
    }


    protected void Button1_Click(object sender, EventArgs e)
    {
        System.DateTime currentTime = new System.DateTime();
        ASPxTextBox1.Text = ComboRelateU8NO.Value.ToString();

        //if (Convert.ToDateTime(DeliveryDate.Text))
        //{
            
        //}
    }
    protected void ASPxButton1_Click(object sender, EventArgs e)
    {
        byte[] data = new byte[1024];
        string input, stringData;

        //构建TCP 服务器

        //Console.WriteLine("This is a Client, host name is {0}", Dns.GetHostName());

        //设置服务IP，设置TCP端口号
        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("192.168.2.178"), 9001);

        //定义网络类型，数据连接类型和网络协议UDP
        Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        //string welcome = "1231";
        string welcome = "1;顾客编码01;顾客名称aa;订单编号123;新增订单";
        //data = Encoding.ASCII.GetBytes(welcome);
        data = Encoding.UTF8.GetBytes(welcome);
        server.SendTo(data, data.Length, SocketFlags.None, ipep);
        //IPEndPoint senderx = new IPEndPoint(IPAddress.Any, 0);
        //EndPoint Remote = (EndPoint)senderx;

        //data = new byte[1024];
        ////对于不存在的IP地址，加入此行代码后，可以在指定时间内解除阻塞模式限制
        ////server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 100);
        //int recv = server.ReceiveFrom(data, ref Remote);
        //Console.WriteLine("Message received from {0}: ", Remote.ToString());
        //Console.WriteLine(Encoding.ASCII.GetString(data, 0, recv));
        //while (true)
        //{
        //    input = Console.ReadLine();
        //    if (input == "exit")
        //        break;
        //    server.SendTo(Encoding.ASCII.GetBytes(input), Remote);
        //    data = new byte[1024];
        //    recv = server.ReceiveFrom(data, ref Remote);
        //    stringData = Encoding.ASCII.GetString(data, 0, recv);
        //    Console.WriteLine(stringData);
        //}
        //Console.WriteLine("Stopping Client.");
        //server.Close();    
    }
}