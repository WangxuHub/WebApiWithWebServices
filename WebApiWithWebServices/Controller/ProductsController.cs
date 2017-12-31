using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWithWebServices.Controller
{
    public class ProductsController : System.Web.Http.ApiController
    {
        public IEnumerable<string> Get()
        {
            return new string[] { "1111", "2222" };
        }

        [System.Web.Http.HttpPost,System.Web.Http.HttpGet]
        public string GetString(DysoftParams location123)
        {

            var v1 = Request.Content.ReadAsStringAsync().Result;
            var v2 = Request.Content.ReadAsStringAsync().Result;
            return "123";
        }
    }
}
