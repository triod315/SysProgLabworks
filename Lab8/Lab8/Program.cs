using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab8
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Input URL:");
            string url = Console.ReadLine();
            if (url == "") url = @"http://91.202.128.107:55555/sysprog/";
            using (ServiceReference1.SomeSoapServiceClient W = new ServiceReference1.SomeSoapServiceClient()) {
                ServiceReference1.checkFile T = new ServiceReference1.checkFile();
                T.url = url;
                string res = W.checkFile(T).checkFileResult;
                Console.WriteLine($"URL: {T.url}\nResult: {res}");
            }
        }
    }
}
