using Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Web.Http.SelfHost;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Globalization;
using System.Web.Http.ModelBinding;
using System.Collections.Concurrent;
using System.Web.Http.Controllers;
using System.Web.Http.ValueProviders;

namespace WebApiWithWebServices
{
    class Program
    {
        static void Main(string[] args)
        {
            //使用Owin启动 支持主机名*
            //Microsoft.Owin.Host
            //Microsoft.Owin.Host.HttpListener
            var basicManageApp = Microsoft.Owin.Hosting.WebApp.Start<Startup>("http://*:8090");
            Console.WriteLine("基础管理服务已启动");



            //Microsoft.AspNet.WebApi.SelfHost
            //使用WebApi 自宿主启动，不支持主机名*，但是不过滤主机名
            var config = new System.Web.Http.SelfHost.HttpSelfHostConfiguration("http://localhost:8080");

            config.Routes.MapHttpRoute(
                "Api Default", "api/{controller}/{id}",
                new { id = RouteParameter.Optional });


            using (var server = new HttpSelfHostServer(config))
            {
                server.OpenAsync().Wait();
                Console.WriteLine("Press Enter to quit.");
                Console.ReadLine();
            }

        }
    }


    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var apiconfig = new HttpConfiguration();
            
            #region 配置路由
            apiconfig.MapHttpAttributeRoutes();

            apiconfig.Routes.MapHttpRoute(
                name: "Default",
                routeTemplate: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = RouteParameter.Optional });

            #endregion
            
            app.UseWebApi(apiconfig);
        }
    }

    [ModelBinder(typeof(GeoPointModelBinder))]
    public class DysoftParams
    {
        public string BodyString { get; internal set; } = "";

        public object Query { get; internal set; }

        public object Body { get; internal set; }
    }


    public class GeoPointModelBinder : IModelBinder
    {
        static GeoPointModelBinder()
        {

        }

        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            #region 数据有效性判断
            if (bindingContext.ModelType != typeof(DysoftParams))
            {
                return false;
            }

            //不支持文件上传的数据流 multipart/form-data
            if (actionContext.Request.Content.IsMimeMultipartContent())
            {
                return false;
            }
            #endregion

            var queryKeyValues = actionContext.Request.RequestUri.ParseQueryString();

            var urlEncodeKeyValues = actionContext.Request.Content.ReadAsFormDataAsync().Result;

            var jsonKeyValues = actionContext.Request.Content.ReadAsStringAsync().Result;


            DysoftParams result = new DysoftParams();
            result.BodyString = actionContext.Request.Content.ReadAsStringAsync().Result;
            
            var v2 = actionContext.Request.Content.ReadAsStringAsync().Result;

            bindingContext.Model = result;
            return true;
        }
    }
}
