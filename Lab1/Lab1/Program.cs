using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;


namespace Lab1
{
    class Program
    {

        /// <summary>
        /// Check if string conatains word
        /// </summary>
        /// <param name="text">text for chaking</param>
        /// <param name="sample">sample</param>
        /// <returns>true if contains or false in other case</returns>
        static bool checkStringUpper(string[] text, string sample) {
            for (int i = 0; i < text.Length; i++) {
                if (text[i].ToUpper() == sample.ToUpper()) return true;
            }
            return false;
        }

        static bool checkString(string[] text, string sample) => (Array.IndexOf(text, sample) != -1);


        static void Main(string[] args)
        {

            Console.WriteLine($"string to remove: 'chip' or '{args[0]}'\nstring to replace by MODIFIED: '{args[1]}'");

            //path to file from web
            string path = @"manual.txt";
            string text;
            using (WebClient W = new WebClient())
            {
                text = W.DownloadString("http://mail.univ.net.ua/manual.txt");
            }

            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(text);
            }

            string lightPath = @"Manual-LIGHT.txt";
            string[] lightText = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            string[] tmpStringArr;
            using (StreamWriter sw = new StreamWriter(lightPath))
            {
                for (int i = 0; i < lightText.Length; i++)
                {

                    tmpStringArr = lightText[i].Split(new char[] { ' ', '!', '?', ',' });

                    if (checkStringUpper(tmpStringArr, "chip") || checkStringUpper(tmpStringArr, args[0]))
                        continue;

                    if (checkString(tmpStringArr,args[1]))
                        lightText[i] = "MODIFIED";
                    sw.WriteLine(lightText[i]);
                }
            }
           

            Console.ReadKey();
        }
    }
}
