 


'组件生效    
    Set obj = CreateObject("ASIM.IM")
    Set Msg = CreateObject("ASIM.Msg")

'设置服务器，发送者，接受者等信息
    Amserver="192.168.0.254"
    Sender="网上订单消息"
    Sender_password="test"
    Receive="804"

'设置消息的相关内容
    Msg.ContentType = "Text/Text"
    Msg.Subject = "Start"
    Msg.Body = "顾客编码:"
    obj.port=5001
    obj.Init Amserver, Sender, Sender_password

'消息发送
    obj.SendMsgEx Msg, Receive
 