using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClientTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var server2 = new ServiceReference1.UserAuthSoapClient();
            var res = server2.Auth("asdasd", "asdasd");

            var res2 = server2.Auth("csy", "123456");

            Console.Read();
        }
    }
}
