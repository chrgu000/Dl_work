 


'�����Ч    
    Set obj = CreateObject("ASIM.IM")
    Set Msg = CreateObject("ASIM.Msg")

'���÷������������ߣ������ߵ���Ϣ
    Amserver="192.168.0.254"
    Sender="���϶�����Ϣ"
    Sender_password="test"
    Receive="804"

'������Ϣ���������
    Msg.ContentType = "Text/Text"
    Msg.Subject = "Start"
    Msg.Body = "�˿ͱ���:"
    obj.port=5001
    obj.Init Amserver, Sender, Sender_password

'��Ϣ����
    obj.SendMsgEx Msg, Receive
 