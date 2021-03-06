﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClientTest.ServiceReference2 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference2.UserAuthSoap")]
    public interface UserAuthSoap {
        
        // CODEGEN: 命名空间 http://tempuri.org/ 的元素名称 userName 以后生成的消息协定未标记为 nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Auth", ReplyAction="*")]
        ClientTest.ServiceReference2.AuthResponse Auth(ClientTest.ServiceReference2.AuthRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class AuthRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="Auth", Namespace="http://tempuri.org/", Order=0)]
        public ClientTest.ServiceReference2.AuthRequestBody Body;
        
        public AuthRequest() {
        }
        
        public AuthRequest(ClientTest.ServiceReference2.AuthRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class AuthRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string userName;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string password;
        
        public AuthRequestBody() {
        }
        
        public AuthRequestBody(string userName, string password) {
            this.userName = userName;
            this.password = password;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class AuthResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="AuthResponse", Namespace="http://tempuri.org/", Order=0)]
        public ClientTest.ServiceReference2.AuthResponseBody Body;
        
        public AuthResponse() {
        }
        
        public AuthResponse(ClientTest.ServiceReference2.AuthResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class AuthResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public bool AuthResult;
        
        public AuthResponseBody() {
        }
        
        public AuthResponseBody(bool AuthResult) {
            this.AuthResult = AuthResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface UserAuthSoapChannel : ClientTest.ServiceReference2.UserAuthSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class UserAuthSoapClient : System.ServiceModel.ClientBase<ClientTest.ServiceReference2.UserAuthSoap>, ClientTest.ServiceReference2.UserAuthSoap {
        
        public UserAuthSoapClient() {
        }
        
        public UserAuthSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public UserAuthSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public UserAuthSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public UserAuthSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        ClientTest.ServiceReference2.AuthResponse ClientTest.ServiceReference2.UserAuthSoap.Auth(ClientTest.ServiceReference2.AuthRequest request) {
            return base.Channel.Auth(request);
        }
        
        public bool Auth(string userName, string password) {
            ClientTest.ServiceReference2.AuthRequest inValue = new ClientTest.ServiceReference2.AuthRequest();
            inValue.Body = new ClientTest.ServiceReference2.AuthRequestBody();
            inValue.Body.userName = userName;
            inValue.Body.password = password;
            ClientTest.ServiceReference2.AuthResponse retVal = ((ClientTest.ServiceReference2.UserAuthSoap)(this)).Auth(inValue);
            return retVal.Body.AuthResult;
        }
    }
}
