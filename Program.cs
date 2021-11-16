using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AutoMessEMail
{
    class Program
    {
        static byte ProvOutAd(string address)
        {
            byte j = 0;
            string s = "";
            for(int i=address.Length-1;i>-1;i--)
            {
                if(address[i]!='@')
                {
                    s += address[i];
                }
                else
                {
                    break;
                }
            }
            char[] chAr = s.ToCharArray();
            Array.Reverse(chAr);
            s = new string(chAr);
            switch (s)
            {
                case "mail.ru":
                    j = 1;
                    break;
                case "yandex.ru":
                    j = 2;
                    break;
                default:
                    j = 0;
                    break;
            }
            return j;
        }
        static void Main(string[] args)
        {
            int Errors = 0;
            string subject = "";
            string message = "";
            string login = "";
            string password = "";
            string[] Addresses = new string[0];
            //считываем адреса в массив
            string pathAd = "Addresses";
            if (File.Exists(pathAd))
            {
                Addresses = new string[File.ReadAllLines(pathAd).Length];
                for( int i=0; i< File.ReadAllLines(pathAd).Length;i++)
                {
                    Addresses[i] = File.ReadLines(pathAd).Skip(i).First();
                }
            }
            else
            {
                Errors++;
            }
            //считываем тему письма
            string pathS = "Subject";
            if (File.Exists(pathS))
            {
                subject = File.ReadLines(pathS).Skip(0).First();
            }
            else
            {
                Errors++;
            }
            //считываем текст
            string pathMes = "Message.html";
            if (File.Exists(pathMes))
            {
                for (int i = 0; i < File.ReadAllLines(pathMes).Length; i++)
                {
                    message += " ";
                    message += File.ReadLines(pathMes).Skip(i).First();
                }
            }
            else
            {
                Errors++;
            }
            //считываем данные адреса отправителя
            string pathMA = "MyAddress";
            if (File.Exists(pathS))
            {
                login = File.ReadLines(pathMA).Skip(0).First();
                password = File.ReadLines(pathMA).Skip(1).First();
            }
            else
            {
                Errors++;
            }
            if(Errors==0)
            {
                //
                MailAddress from = new MailAddress(login);
                MailAddress to; MailMessage m;
                SmtpClient smtp;
                switch(ProvOutAd(login))
                {
                    case 1:
                        smtp = new SmtpClient("smtp.mail.ru", 2525);
                        break;
                    case 2:
                        smtp = new SmtpClient("smtp.yandex.ru", 465);
                        break;
                    default:
                        Errors++;
                        smtp = new SmtpClient("smtp.mail.ru", 2525);
                        break;
                }
                smtp.Credentials = new NetworkCredential(login, password);
                smtp.EnableSsl = true;
                for(int i=0;i< Addresses.Length;i++)
                {
                    //кому отправляем
                    to = new MailAddress(Addresses[i]);
                    //создаём объект сообщения
                    m = new MailMessage(from, to);
                    m.Subject = subject;
                    m.Body = message;
                    m.IsBodyHtml = true;
                    smtp.Send(m);
                }
            }
            Console.WriteLine(Errors);
            Console.WriteLine("Press any key...");
            Console.Read();
        }
    }
}
