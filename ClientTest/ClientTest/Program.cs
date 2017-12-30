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
            var server1 = new ServiceReference1.UserAuthSoapClient();
            var res = server1.Auth("asdasd", "asdasd");

            var res2 = server1.Auth("csy", "123456");


            var server2 = new ServiceReference2.UserAuthSoapClient();
            var res1 = server2.Auth("asdasd", "asdasd");

            var res12 = server2.Auth("csy", "123456");

            Console.Read();
        }
    }
}
