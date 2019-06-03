using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Task1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
           
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ServiceReference1.Service1Client client = new ServiceReference1.Service1Client();
            label1.Text = client.MyIPAddress();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ServiceReference1.Service1Client client = new ServiceReference1.Service1Client();

            if (client.IsLoginFree(textBox1.Text))
            {
                label1.Text = $"Login '{textBox1.Text}' is free";
            }
            else
            {
                label1.Text = $"Login '{textBox1.Text}' is used";
            }
        }
    }
}
