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
    }
}
