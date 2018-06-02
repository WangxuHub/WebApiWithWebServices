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
using System.Dynamic;
using System.Collections.Specialized;

namespace WebApiWithWebServices
{
    class Program
    {
        static void Main(string[] args)
        {
            //使用Owin启动 支持主机名*
            //Microsoft.Owin.Hosting
            //Microsoft.Owin.Host.HttpListener
            var options = new Microsoft.Owin.Hosting.StartOptions();
            options.Urls.Add("https://*:9443/");
            options.Urls.Add("https://*:9444/");
            options.Urls.Add("http://*:8090/");

            var basicManageApp = Microsoft.Owin.Hosting.WebApp.Start<Startup>(options);
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

          //  apiconfig.MessageHandlers.Add(new RequireHttpsHandler());

            app.UseWebApi(apiconfig);
        }
    }

    [ModelBinder(typeof(DysoftParamsModelBinder))]
    public class DysoftParams
    {
        public DysoftParams()
        {
            Body = new BodyDynamic(this);
            Query = new QueryDynamic(this);
            Param = new QueryOrBodyDynamic(this);
        }

        public string BodyString { get; internal set; } = "";

        public dynamic Query { get; internal set; } 

        public dynamic Body { get; internal set; }

        public dynamic Param { get; internal set; }


        public System.Collections.Specialized.NameValueCollection queryKeyValues { get; internal set; }
        public Dictionary<string, string> urlEncodeKeyValues { get; internal set; }
        public Dictionary<string, object> jsonKeyValues { get; internal set; }

        public string this[string key]
        {
            get
            {
                var value = GetValue(key);
                return value?.ToString() ?? "";
            }
        }

        internal object GetValue(string key)
        {
            object value = null;

            if (urlEncodeKeyValues != null && urlEncodeKeyValues.ContainsKey(key))
            {
                value = System.Web.HttpUtility.UrlDecode(urlEncodeKeyValues[key]);
            }
            else if (jsonKeyValues != null && jsonKeyValues.ContainsKey(key))
            {
                value = jsonKeyValues[key];
            }
            else if (queryKeyValues != null && queryKeyValues.Count > 0)
            {
                value = queryKeyValues[key];
            }
            return value;
        }
        
    }
    
    public class QueryOrBodyDynamic : System.Dynamic.DynamicObject
    {
        private DysoftParams data;
        public QueryOrBodyDynamic(DysoftParams _data)
        {
            data = _data;
        }
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            object value = null;
            var name = binder.Name;

            value = data.GetValue(name);
            result = new AutoConvertSupport(value);
            return true;
        }
    }

    public class QueryDynamic : System.Dynamic.DynamicObject
    {
        private DysoftParams data;
        public QueryDynamic(DysoftParams _data)
        {
            data = _data;
        }
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            object value = null;
            var name = binder.Name;

            value = data.queryKeyValues[name];
            result = new AutoConvertSupport(value);
            return true;
        }
    }

    public class BodyDynamic : System.Dynamic.DynamicObject
    {
        private DysoftParams data;
        public BodyDynamic(DysoftParams _data)
        {
            data = _data;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            object value = null;
            var name = binder.Name;

            if (data.urlEncodeKeyValues != null && data.urlEncodeKeyValues.ContainsKey(name))
            {
                value = System.Web.HttpUtility.UrlDecode(data.urlEncodeKeyValues[name]);
            }
            else if (data.jsonKeyValues != null && data.jsonKeyValues.ContainsKey(name))
            {
                value = data.jsonKeyValues[name];
            }

            result = new AutoConvertSupport(value);
            return true;
        }
    }

    internal class AutoConvertSupport : DynamicObject
    {
        private readonly object _value;

        public AutoConvertSupport(object value)
        {
            _value = value;
        }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            var type = binder.Type;
            result = ChangeType(_value, binder.Type);
            return true;
        }

        static public object ChangeType(object value, Type type)
        {
            if (value == null && type.IsGenericType) return Activator.CreateInstance(type);
            if (value == null) return null;
            if (type == value.GetType()) return value;
            if (type.IsEnum)
            {
                if (value is string)
                    return Enum.Parse(type, value as string);
                else
                    return Enum.ToObject(type, value);
            }
            if (!type.IsInterface && type.IsGenericType)
            {
                Type innerType = type.GetGenericArguments()[0];
                object innerValue = ChangeType(value, innerType);
                return Activator.CreateInstance(type, new object[] { innerValue });
            }
            if (value is string && type == typeof(Guid)) return new Guid(value as string);
            if (value is string && type == typeof(Version)) return new Version(value as string);
            if (!(value is IConvertible)) return value;
            return Convert.ChangeType(value, type);
        }
    }

    public class DysoftParamsModelBinder : IModelBinder
    {
        static DysoftParamsModelBinder()
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
                bindingContext.Model = new DysoftParams();
                return true;
            }
            #endregion

            DysoftParams result = new DysoftParams();
            result.BodyString = actionContext.Request.Content.ReadAsStringAsync().Result;

            result.queryKeyValues = actionContext.Request.RequestUri.ParseQueryString();
          
            try
            {
                var bodyStr = actionContext.Request.Content.ReadAsStringAsync().Result.Trim();
                if (bodyStr.StartsWith("{") && bodyStr.EndsWith("}"))
                {
                    result.jsonKeyValues = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(bodyStr);
                }
                else
                {
                    result.urlEncodeKeyValues = bodyStr.Split(new char[] { '&' },StringSplitOptions.RemoveEmptyEntries).Select(a => {
                        var arr = a.Split('=');

                        var key = arr[0];
                        var value = "";
                        if (arr.Length > 1) {
                            value = arr[1];
                        }
                        return new
                        {
                            key,
                            value
                        };
                    }).ToDictionary(a=>a.key,a=>a.value);
                }
            }
            catch
            {
            }

            bindingContext.Model = result;
            return true;
        }
    }
}
