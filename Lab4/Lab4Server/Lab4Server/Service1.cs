using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Xml;

namespace Lab4Server
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }


        Thread T;
        bool mustStop;



        protected override void OnStart(string[] args)
        {
            T = new Thread(WorkerThread);
            T.Start();
        }

        protected override void OnStop()
        {
            if ((T != null) && (T.IsAlive))
            {
                mustStop = true;
            }
        }

        void WorkerThread()
        {
            Debugger.Launch();
            string ip = "192.168.0.101";
            
            WriteLog($"started\nmustStop={mustStop}");

            IPAddress IP = IPAddress.Parse(ip);

            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 54000);
            WriteLog($"IP={ip}\n");
            Socket S = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            S.Bind(ipEndPoint);
            while (!mustStop)
            {
                
                try
                {
                   
                    //S.Listen(10);
                    //WriteLog($"listening on {ip}\n");
                    while (true)
                    {
                        //using (Socket H = S.Accept())
                        //{
                            IPEndPoint L = new IPEndPoint(IPAddress.Any, 0);
                            EndPoint R = (EndPoint)(L);
                            byte[] D = new byte[10000];
                            int Receive = S.ReceiveFrom(D, ref R);
                            string Request = Encoding.UTF8.GetString(D, 0, Receive);
                            WriteLog($"Request recived from {R.ToString()}:\n{Request}");
                            CheckRequest(Request);
                            string W = File.ReadAllText(@"C:\Windows\Demon\response.xml", Encoding.UTF8);
                            byte[] M = Encoding.UTF8.GetBytes(W);
                            S.SendTo(M, R);
                        //  H.Send(M); 
                        //  H.Shutdown(SocketShutdown.Both);

                        //}
                    }
                    
                }
                catch (Exception e)
                {
                    WriteLog(e.Message);
                }
                S.Close();
            }
        }

        private static void CheckRequest(string request)
        {
            try
            {
                string reqFile = @"C:\Windows\Demon\request.xml";

                using (StreamWriter F = new StreamWriter(reqFile, false, Encoding.UTF8))
                {
                    F.WriteLine(request);
                }

                XmlDocument doc = new XmlDocument();
                doc.Load(reqFile);

                XmlElement xRoot = doc.DocumentElement;

                string curKey;
                string newKey;

                curKey = xRoot.ChildNodes[0].InnerText;
                newKey = xRoot.ChildNodes[1].InnerText;

                WriteLog($"type {xRoot.Name}");
                WriteLog($"Get current key{curKey}");
                WriteLog($"Get new key{newKey}");

                if (xRoot.Name == "request") 
                    using (RegistryKey v = Registry.LocalMachine.OpenSubKey(curKey, true))
                    {
                        if (v == null)
                        {
                            writeResponse(404, $"Key '{curKey}' DOESN'T EXIST :(");
                            WriteLog($"\n404\t{curKey} DOESN'T EXIST \n");
                        }
                        else
                        if (v.GetSubKeyNames().Contains(newKey))
                        {
                            writeResponse(300, $"Key '{newKey}' EXIST");
                            WriteLog($"\n\n300\t{newKey} EXIST\n\n");
                        }
                        else
                        {
                            writeResponse(400, $"Key '{newKey}' DOESN'T EXIST :(");
                            WriteLog($"\n400\t{newKey} DOESN'T EXIST \n");
                        }

                    }
                if (xRoot.Name=="create")
                    using (RegistryKey v = Registry.LocalMachine.OpenSubKey(curKey, true))
                    {
                        if (v == null)
                        {
                            writeResponse(204, $"Key '{curKey}' DOESN'T EXIST :(");
                            WriteLog($"\n204\t{curKey} DOESN'T EXIST \n");
                        }
                        else
                        if (v.GetSubKeyNames().Contains(newKey))
                        {
                            writeResponse(203, $"Key '{newKey}' EXIST");
                            WriteLog($"\n\n203\t{newKey} EXIST\n\n");
                        }
                        else
                        {
                            v.CreateSubKey(newKey);
                            writeResponse(200, $"Key '{newKey}' WAS CREATED");
                            WriteLog($"\n200\t{newKey} WAS CREATED \n");
                        }

                    }

            }
            catch (Exception e)
            {
                WriteLog(e.Message);
            }

        }

        private static void WriteLog(string z)
        {
            using (StreamWriter F = new StreamWriter(@"C:\Windows\Logs\Demon.txt", true, Encoding.UTF8))
            {
                F.WriteLine(z);
            }
        }

        static void writeResponse(int code,string description)
        {
            Debugger.Launch();
            string filePath = @"C:\Windows\Demon\response.xml";

            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            doc.DocumentElement.ParentNode.RemoveAll();

            XmlElement xRoot = doc.DocumentElement;

            XmlElement response = doc.CreateElement("response");

            XmlElement xcode = doc.CreateElement("code");
            XmlElement xdesc = doc.CreateElement("description");
            
            XmlText responseText = doc.CreateTextNode(description);
            XmlText respCode = doc.CreateTextNode($"{code}");

            xdesc.AppendChild(responseText);
            xcode.AppendChild(respCode);

            response.AppendChild(xdesc);
            response.AppendChild(xcode);
            
            doc.AppendChild(response);
            doc.Save(filePath);
        }

    }
}
