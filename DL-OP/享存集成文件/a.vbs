Set   objArgs   =   WScript.Arguments  
dim a1,a2,a3,a4,a5
a1=objArgs(0)
a2=objArgs(1)
a3=objArgs(2)
a4=objArgs(3)
a5=objArgs(4)


'�����Ч    
    Set obj = CreateObject("ASIM.IM")
    Set Msg = CreateObject("ASIM.Msg")

'���÷������������ߣ������ߵ���Ϣ
    Amserver="192.168.0.254"
    Sender="���϶�����Ϣ"
    Sender_password="test"
    Receive="635"

'������Ϣ���������
    Msg.ContentType = "Text/Text"
    Msg.Subject = "Start"
    Msg.Body = "�˿ͱ���:"+a2+",����:"+a3+","+a5+":"+a4+".�뾡�촦��!"
    obj.port=5001
    obj.Init Amserver, Sender, Sender_password

'��Ϣ����
    obj.SendMsgEx Msg, Receive
