﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace DingDan_WebForm.SMSSend9001 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://dl.duolian.com/", ConfigurationName="SMSSend9001.SendSMS2CustomerSoap")]
    public interface SendSMS2CustomerSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://dl.duolian.com/SendFHSMS2Customer", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string SendFHSMS2Customer(string phoneno, string orderno, string content);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://dl.duolian.com/SendFHSMS2Customer", ReplyAction="*")]
        System.Threading.Tasks.Task<string> SendFHSMS2CustomerAsync(string phoneno, string orderno, string content);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://dl.duolian.com/SendTest", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string SendTest(string phoneno, string orderno, string content);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://dl.duolian.com/SendTest", ReplyAction="*")]
        System.Threading.Tasks.Task<string> SendTestAsync(string phoneno, string orderno, string content);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://dl.duolian.com/SendQY_Message_Text", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string SendQY_Message_Text(string touser, string toparty, string totag, string agentid, string strWxMessage);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://dl.duolian.com/SendQY_Message_Text", ReplyAction="*")]
        System.Threading.Tasks.Task<string> SendQY_Message_TextAsync(string touser, string toparty, string totag, string agentid, string strWxMessage);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://dl.duolian.com/SendSMS", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        bool SendSMS(string phoneno, string content);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://dl.duolian.com/SendSMS", ReplyAction="*")]
        System.Threading.Tasks.Task<bool> SendSMSAsync(string phoneno, string content);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface SendSMS2CustomerSoapChannel : DingDan_WebForm.SMSSend9001.SendSMS2CustomerSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SendSMS2CustomerSoapClient : System.ServiceModel.ClientBase<DingDan_WebForm.SMSSend9001.SendSMS2CustomerSoap>, DingDan_WebForm.SMSSend9001.SendSMS2CustomerSoap {
        
        public SendSMS2CustomerSoapClient() {
        }
        
        public SendSMS2CustomerSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SendSMS2CustomerSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SendSMS2CustomerSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SendSMS2CustomerSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string SendFHSMS2Customer(string phoneno, string orderno, string content) {
            return base.Channel.SendFHSMS2Customer(phoneno, orderno, content);
        }
        
        public System.Threading.Tasks.Task<string> SendFHSMS2CustomerAsync(string phoneno, string orderno, string content) {
            return base.Channel.SendFHSMS2CustomerAsync(phoneno, orderno, content);
        }
        
        public string SendTest(string phoneno, string orderno, string content) {
            return base.Channel.SendTest(phoneno, orderno, content);
        }
        
        public System.Threading.Tasks.Task<string> SendTestAsync(string phoneno, string orderno, string content) {
            return base.Channel.SendTestAsync(phoneno, orderno, content);
        }
        
        public string SendQY_Message_Text(string touser, string toparty, string totag, string agentid, string strWxMessage) {
            return base.Channel.SendQY_Message_Text(touser, toparty, totag, agentid, strWxMessage);
        }
        
        public System.Threading.Tasks.Task<string> SendQY_Message_TextAsync(string touser, string toparty, string totag, string agentid, string strWxMessage) {
            return base.Channel.SendQY_Message_TextAsync(touser, toparty, totag, agentid, strWxMessage);
        }
        
        public bool SendSMS(string phoneno, string content) {
            return base.Channel.SendSMS(phoneno, content);
        }
        
        public System.Threading.Tasks.Task<bool> SendSMSAsync(string phoneno, string content) {
            return base.Channel.SendSMSAsync(phoneno, content);
        }
    }
}
