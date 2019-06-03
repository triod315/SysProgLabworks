using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Task6
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string concatenateString(string[] str)
        {
            string tmp = "";
            for (int i = 0; i < str.Length; i++)
            {
                tmp += str[i] + "\n";
            }
            return tmp;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] S;
            using (RegistryKey v = Registry.LocalMachine.OpenSubKey("Software\\Hryshchuk", true))
            {

                S = (string[])v.GetValue("P5");
                MessageBox.Show(concatenateString(S), "result");

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
  
            using (RegistryKey v = Registry.LocalMachine.OpenSubKey("Software\\Hryshchuk", true))
            {
                v.SetValue("P6", new string[] { "Я - студент", "кафедри КІ!"}, RegistryValueKind.MultiString);
                MessageBox.Show("data was writen succesfully");
            }
        }
    }
}
