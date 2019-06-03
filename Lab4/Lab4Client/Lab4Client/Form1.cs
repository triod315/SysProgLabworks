using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Lab4Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        bool flag;

        private void button1_Click(object sender, EventArgs e)
        {
            string res = getUDPResult(textBox3.Text,"request");
            int code=AnalResp(res);
            switch (code)
            {
                case 300:
                    MessageBox.Show("Даний ключ вже існує");
                    break;
                case 404:
                    MessageBox.Show($"Гілки '{textBox1.Text}' не існує");
                    break;
                case 400:
                    DialogResult result = MessageBox.Show($"Сворити підключ {textBox2.Text}?", "OK",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (result==DialogResult.Yes )
                    {
                        res = getUDPResult(textBox3.Text, "create");
                        if (AnalResp(res) == 200)
                            MessageBox.Show($"Ключ {textBox2.Text} було створено успішно");
                        else
                            MessageBox.Show("Щось пішло не так (");
                    }
                    
                    break;
            }
            writeProgLog(res);
        }

        void AcceptNewKey() { }

        void writeProgLog(string z)
        {
            using (StreamWriter F = new StreamWriter(@"Demon.txt", true, Encoding.UTF8))
            {
                F.WriteLine(z);
            }
        }

        public string getUDPResult(string ip, string type)
        {
            string res = "";

            string Answer;

            string filePath = "file1.xml";

            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            doc.DocumentElement.ParentNode.RemoveAll();

            XmlElement xRoot = doc.DocumentElement;

            XmlElement element = doc.CreateElement(type);
            XmlElement currentKey = doc.CreateElement("currentkey");
            XmlElement newSubKey = doc.CreateElement("newkey");

            XmlText currentKeyText = doc.CreateTextNode(textBox1.Text);
            XmlText newSubKeyText = doc.CreateTextNode(textBox2.Text);

            
            currentKey.AppendChild(currentKeyText);
            newSubKey.AppendChild(newSubKeyText);

            element.AppendChild(currentKey);
            element.AppendChild(newSubKey);

            doc.AppendChild(element);
            doc.Save(filePath);

            string F = File.ReadAllText(filePath, Encoding.UTF8);
            byte[] M = Encoding.UTF8.GetBytes(F);
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(textBox3.Text), Convert.ToInt32(textBox4.Text));
            byte[] bytes = new byte[1000000];
            using (Socket S = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            {
                S.Connect(ipEndPoint);
                S.Send(M);
                int bytesRec = S.Receive(bytes);
                Answer = Encoding.UTF8.GetString(bytes, 0, bytesRec);
                S.Shutdown(SocketShutdown.Both);
                return Answer;
            }

            return res;

        }

        private int AnalResp(string response)
        {
            int code = 500;

            try
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(response);

                XmlElement xRoot = xdoc.DocumentElement;
                XmlNodeList xmlNodeList = xRoot.ChildNodes;
                //MessageBox.Show(xmlNodeList[0].InnerText, "Response");
                return int.Parse(xmlNodeList[1].InnerText);
            }
            catch (Exception e)
            {
                writeProgLog(e.Message);
            }

            return code;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
