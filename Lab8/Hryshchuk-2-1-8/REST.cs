using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace REST
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Input URL:");
            string url = Console.ReadLine();
            if (url == "") url = @"http://91.202.128.107:55555/sysprog/";
            Console.WriteLine(url);
            WebClient D = new WebClient();
            D.Headers[HttpRequestHeader.ContentType] = "application/json";
            string response = D.UploadString("http://192.168.159.129:5003/users/checkFile", "{ \"url\": \""+url+"\"}");
            Console.WriteLine($"URL: {url}\nResult: {response}");
        }
    }
}
