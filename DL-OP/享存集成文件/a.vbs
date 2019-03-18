Set   objArgs   =   WScript.Arguments  
dim a1,a2,a3,a4,a5
a1=objArgs(0)
a2=objArgs(1)
a3=objArgs(2)
a4=objArgs(3)
a5=objArgs(4)


'组件生效    
    Set obj = CreateObject("ASIM.IM")
    Set Msg = CreateObject("ASIM.Msg")

'设置服务器，发送者，接受者等信息
    Amserver="192.168.0.254"
    Sender="网上订单消息"
    Sender_password="test"
    Receive="635"

'设置消息的相关内容
    Msg.ContentType = "Text/Text"
    Msg.Subject = "Start"
    Msg.Body = "顾客编码:"+a2+",名称:"+a3+","+a5+":"+a4+".请尽快处理!"
    obj.port=5001
    obj.Init Amserver, Sender, Sender_password

'消息发送
    obj.SendMsgEx Msg, Receive
