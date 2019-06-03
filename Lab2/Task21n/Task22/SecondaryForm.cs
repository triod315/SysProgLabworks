using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace Task22
{
    public partial class SecondaryForm : Form
    {

        string DB = "Data Source=DESKTOP-5582JAK\\SQLEXPRESS;Initial Catalog=TradeDB;Integrated Security=True";

        public SecondaryForm()
        {
            InitializeComponent();
        }

        public Form1 parent;

        void fillColums(string[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                dataGridView1.Columns.Add(arr[i], arr[i]);
                dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }
        }


        void LoadAmount()
        {
            dataGridView1.Columns.Clear();
            string query = "use TradeDB select * from Amount_of_trade";
            using (SqlConnection connection = new SqlConnection(DB))
            {
                connection.Open();
                SqlCommand sqlCommand = new SqlCommand(query, connection);
                SqlDataReader R = sqlCommand.ExecuteReader();
                fillColums(new string[] { "ID", "Money", "Description" });

                while (R.Read())
                {
                    dataGridView1.Rows.Add(R[0].ToString(), R[1].ToString(), R[2].ToString());
                }
                connection.Close();

            
            }

        }


        void InsertAm(float money, string description)
        {
            string query = $"use TradeDB insert into Amount_of_trade(Money, Description) values({money}, N'{description}')";

            using (SqlConnection connection = new SqlConnection(DB))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            MessageBox.Show("Рядок було додано успішно");
        }

        void updateRecord(int id,float money, string description)
        {
            string query = $"use TradeDB update Amount_of_trade set Money = {money}, Description = N'{description}' where ID={id}";
            using (SqlConnection connection = new SqlConnection(DB))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            MessageBox.Show("Рядок було змінено успішно");
        }

        void deleteRecord(int id)
        {
            string query = $"use TradeDB delete Amount_of_trade where ID = {id}";
            using (SqlConnection connection = new SqlConnection(DB))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            MessageBox.Show("Рядок було видалено успішно");
        }

        bool checkMoney(string price)
        {
            float tmp;
            bool res = float.TryParse(price, out tmp);
            if (!res) MessageBox.Show("Невірна сума");
            return res;
        }

        bool checkDescription(string org)
        {
            bool res = org != "";
            if (!res) MessageBox.Show("Невірно введено опис");
            return res;
        }

        bool checkID(string id)
        {
            bool res;
            int tmp;
            res = int.TryParse(id, out tmp);
            if (!res) MessageBox.Show("Невірний ID");
            return res;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            parent.Show();
            this.Close();
        }

        bool checkID(int id)
        {
            bool res = false;
            string query = $"select * from Amount_of_trade where ID={id}";

            using (SqlConnection connection = new SqlConnection(DB))
            {
                connection.Open();
                SqlCommand sqlCommand = new SqlCommand(query, connection);
                SqlDataReader R = sqlCommand.ExecuteReader();
                res = R.Read();
                connection.Close();
            }
            return res;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                LoadAmount();
            if (radioButton2.Checked)
                if (checkMoney(textBox2.Text) && checkDescription(textBox3.Text))
                    InsertAm((float)Convert.ToDouble(textBox2.Text),textBox3.Text);

            if (radioButton3.Checked)
                if (checkMoney(textBox2.Text) && checkDescription(textBox3.Text))
                    if (checkID(Convert.ToInt32(textBox1.Text)))
                    {
                        updateRecord(Convert.ToInt32(textBox1.Text), (float)Convert.ToDouble(textBox2.Text), textBox3.Text);
                    }
                    else
                        MessageBox.Show($"Запис з ID={textBox1.Text} не існує", "Помилка");

            if (radioButton4.Checked)
                if (checkID(textBox1.Text))
                {
                    if (checkID(Convert.ToInt32(textBox1.Text)))
                        deleteRecord(Convert.ToInt32(textBox1.Text));
                    else
                        MessageBox.Show($"Запис з ID={textBox1.Text} не існує", "Помилка");
                }

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                textBox1.Enabled = false;
                textBox2.Enabled = true;
                textBox3.Enabled = true;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                textBox3.Enabled = true;
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
            {
                textBox1.Enabled = true;
                textBox2.Enabled = false;
                textBox3.Enabled = false;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                textBox3.Enabled = false;
            }
        }

        private void SecondaryForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
    }
}
