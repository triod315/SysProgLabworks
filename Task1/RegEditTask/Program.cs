using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.IO;
using System.Net;

namespace RegEditTask
{
    class Program
    {
        static void Main(string[] args)
        {
            //   string x = Console.ReadLine();
            //int y = Convert.ToInt32(Console.ReadLine());
            //int z = Convert.ToInt32(args[0]);
            //int n=0;  
            //int x;
            //string[] S;
            //using (RegistryKey v = Registry.LocalMachine.OpenSubKey("Software\\BOSS", true))
            //{
            //    v.SetValue("PARAM-1", "Work!");
            //    v.SetValue("PARAM-2", 256);
            //    v.SetValue("PARAM-3", 256, RegistryValueKind.DWord);
            //    x = (int)v.GetValue("PARAM-7", 333);

            //    S = (string[])v.GetValue("PARAM-4");
            //    //v.GetValue("PARAM-4");

            //}
            //Console.WriteLine(S[2]);   

            //Text files;

            string t;


            //using (StreamWriter F = new StreamWriter("C:\\KI3\\write.txt", false))
            //{
            //    F.WriteLine("123456"); F.WriteLine("87654");
            //}

            //using (StreamReader F = new StreamReader(@"C:\KI3\ggg.txt", false))
            //{
            //    t = F.ReadToEnd(); 
            //    Console.WriteLine(t);
            //}

            using (StreamWriter F = new StreamWriter("C:\\files\\write.txt", false))
            {
                F.WriteLine("123456"); F.WriteLine("87654");
            }
            using (StreamReader F = new StreamReader("C:\\files\\write.txt", true))
            {
                t = F.ReadToEnd();
                Console.WriteLine(t);
            }

            // HTTP Server:

            //using (WebClient W = new WebClient())
            //{
            //    t = W.DownloadString("http://mail.univ.net.ua/apple.txt");
            //    W.UploadString("http://mail.univ.net.ua/apple.txt", "777777777777");
            //}

            //// Binary files:

            //int FSize;
            //using (FileStream F = new FileStream("C:\\Work\\a.zip", FileMode.Open, FileAccess.Read))
            //{
            //    byte[] B = new byte[F.Length];
            //    FSize = (int)F.Length;
            //    F.Read(B, 0, FSize);
            //    using (FileStream G = new FileStream("C:\\Work\\b.zip", FileMode.Create, FileAccess.Write))
            //    {
            //        G.Write(B, 0, FSize);
            //        byte[] B2 = new byte[] { 0x41, 0x42, 0x43 };
            //        G.Write(B2, 0, B2.Length);
            //    }

            //}

            //Console.WriteLine(t);






            //Console.WriteLine(t + "          " + y.ToString() + "          " + n.ToString());
            //Console.WriteLine(t);   
        }
    }
}
