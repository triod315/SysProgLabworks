using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.Threading;

namespace Task3
{
    class Program
    {
        static void Main(string[] args)
        {

            if (args.Length == 2)
            {
                SmtpClient C = new SmtpClient("smtp.gmail.com");
                C.Port = 587;
                C.Credentials = new NetworkCredential("spammailbox98", "c15ef1f9660731187b01435bda747686dbf0e530c3b3a626e7d7c389e56c6ac1");
                C.EnableSsl = true;
                string body = "Date: " + DateTime.Now.ToString() + "\nSender: Oleksandr Hryshchuk";
                C.Send(new MailMessage("spammailbox98@gmail.com", args[0], args[1], body));
            }
            else if (args.Length == 3)
            {
                if (args[2] == "-t")
                {
                    TelegramSender ts = new TelegramSender();
                    string body = "Date: " + DateTime.Now.ToString() + "\nSender: Oleksandr Hryshchuk";

                    ts.SendMessage(args[0], $"Subject: {args[1]}\n{body}");

                    Console.WriteLine("\tMessage was sent via telegram bot");
                    //magic, don't touch :)
                    //Console.ReadKey();
                    Thread.Sleep(1500);
                    /*realy, don't touch, becouse telegram async method SendTextMessageAsync need time to send your message*/
                }
            }
            else
            {
                Console.WriteLine("SYNTAX: Task3 <ToAddr> <Subject>");
            }

        }
    }
}
