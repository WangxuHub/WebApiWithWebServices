using Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Web.Http.SelfHost;

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
}
