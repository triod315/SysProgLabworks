using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;


namespace SMTPClient
{
    class Program
    {
        static void Main(string[] args)
        {
            
             SmtpClient C = new SmtpClient("smtp.gmail.com");
             C.Port = 587;
             C.Credentials = new NetworkCredential("spammailbox98", "passoword");
             C.EnableSsl = true;
             C.Send(new MailMessage("spammailbox98@gmail.com", "380961842146@sms.kyivstar.net", "ANONIM", "BODY"));
            
        }
    }
}
