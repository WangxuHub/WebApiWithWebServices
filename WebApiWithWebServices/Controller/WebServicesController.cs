using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace WebApiWithWebServices.Controller
{
    public class WebServicesController : System.Web.Http.ApiController
    {
        [HttpGet]
        public HttpResponseMessage UserAuth(string wsdl )
        {
            var response = new HttpResponseMessage();

            #region xml 内容
            var sr = new System.IO.StreamReader("Content/UserAuthWsdl.xml");
            var xmlContent = sr.ReadToEnd();
            var host = Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority;
            xmlContent = xmlContent.Replace("{webservices}", host);
            #endregion
            response.Content = new StringContent(xmlContent, Encoding.UTF8, "text/xml");
            return response;
        }
        
        [HttpPost]
        public HttpResponseMessage UserAuth()
        {
            var xml = Request.Content.ReadAsStringAsync().Result;

            var xmlNode = new System.Xml.XmlDocument();
            xmlNode.LoadXml(xml);


            var username = xmlNode.GetElementsByTagName("userName")[0].InnerText;
            var password = xmlNode.GetElementsByTagName("password")[0].InnerText;

            var res = username == "csy" && password == "123456";
        //    var res = true;
            var response = new HttpResponseMessage();

            #region xml 内容
            var xmlContent = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
  <soap:Body>
    <AuthResponse xmlns=""http://tempuri.org/"">
      <AuthResult>{res.ToString().ToLower()}</AuthResult>
    </AuthResponse>
  </soap:Body>
</soap:Envelope>";
            #endregion
            response.Content = new StringContent(xmlContent, Encoding.UTF8, "text/xml");
            return response;
        }
    }
}
