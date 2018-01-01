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
        public string GetString(DysoftParams data)
        {
            var getValue = data["getValue"];
            var getValue2 = data["getValue2"];

            int intValue = data.Body.intValue;
            string strValue = data.Body.strValue;
            Guid guidValue = data.Param.guidValue;

            DateTime? dateValue = data.Param.dateValue;
            return "123";
        }
    }
}
