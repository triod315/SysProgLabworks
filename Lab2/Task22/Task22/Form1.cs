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
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace Task22
{
    public partial class Form1 : Form
    {

        string DB= "server=192.168.254.129; port=3306; database=TradeDB; user=root; password=123456;charset=utf8";

        public Form1()
        {
            InitializeComponent();
            checkDB(50000, "food", 458, "weapoons");
            loadAmountofTrade();       

        }

        void checkDB(float money1,string description1, float money2, string description2)
        {
            string query1 = $"select * from Amount_of_trade where Amount_of_trade.Money={money1} and Description='{description1}'";
            string query2 = $"select * from Amount_of_trade where Amount_of_trade.Money={money2} and Description='{description2}'";


            using (MySqlConnection conn = new MySqlConnection(DB))
            {
                conn.Open();
                MySqlCommand command = new MySqlCommand(query1, conn);
                MySqlDataReader R = command.ExecuteReader();
                if (R.Read() == false)
                {
                    string insertQuery1 = $" insert into Amount_of_trade(Description, Money) values(n'{description1}', {money1});";
                    MySqlCommand command1 = new MySqlCommand(insertQuery1, conn);
                    R.Close();
                    command1.ExecuteNonQuery();

                }
                R.Close();
                MySqlCommand commandn = new MySqlCommand(query2, conn);
                R = commandn.ExecuteReader();
                if (R.Read() == false)
                {
                    string insertQuery2 = $"use TradeDB; insert into Amount_of_trade(Description, Money) values(n'{description2}', {money2})";
                    MySqlCommand command2 = new MySqlCommand(insertQuery2, conn);
                    R.Close();
                    command2.ExecuteNonQuery();
                }
                conn.Close();
            }
        }

        void fillColums(string[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                dataGridView1.Columns.Add(arr[i],arr[i]);
                dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }
        }

        void loadAmountofTrade()
        {
            comboBox1.Items.Clear();

            string query1 = $"select Description from Amount_of_trade ";

            using (MySqlConnection conn = new MySqlConnection(DB))
            {
                conn.Open();
                MySqlCommand command = new MySqlCommand(query1, conn);
                MySqlDataReader R = command.ExecuteReader();
                while (R.Read())
                {
                    comboBox1.Items.Add(R[0].ToString());
                }
                conn.Close();
            }
            
        }


        void LoadOrgs(bool idCheck,bool amountCheck)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            string query;
            if (!idCheck && !amountCheck)
            {
                query = "use TradeDB; select * from Trading_enterprisessUkraine inner join Amount_of_trade on Amount_of_trade.ID_am = Trading_enterprisessUkraine.AmountOfTrade_ID";

                using (MySqlConnection connection = new MySqlConnection(DB))
                {
                    connection.Open();
                    MySqlCommand sqlCommand = new MySqlCommand(query, connection);
                    MySqlDataReader R = sqlCommand.ExecuteReader();

                    fillColums(new string[] { "ID", "Enterprise Name", "Enterprise Owner", "Type", "Price", "Amount_of_trade_ID", "Money", "Descrition" });

                    while (R.Read())
                    {
                        dataGridView1.Rows.Add(R[0].ToString(), R[1].ToString(), R[2].ToString(), R[3].ToString(), R[4].ToString(), R[5].ToString(), R[7].ToString(), R[8].ToString());
                    }
                    connection.Close();
                }
            }
            else
            {

                query = "use TradeDB; select * from Trading_enterprisessUkraine inner join Amount_of_trade on Amount_of_trade.ID_am = Trading_enterprisessUkraine.AmountOfTrade_ID where ";
                if (idCheck && amountCheck)
                {
                    if (checkID(textBox1.Text))
                        query += $"Trading_enterprisessUkraine.ID={Convert.ToInt32(textBox1.Text)} and Amount_of_trade.Description='{comboBox1.Text}'";
                    else
                    {
                        MessageBox.Show("Невірний ID");
                        return;
                    }
                }
                else
                    if (idCheck)
                    if (checkID(textBox1.Text))
                        query += $"Trading_enterprisessUkraine.ID={Convert.ToInt32(textBox1.Text)}";
                    else
                    {
                        MessageBox.Show("Невірний ID");
                        return;
                    }
                else
                    query += $"Amount_of_trade.Description='{comboBox1.Text}'";

                using (MySqlConnection connection = new MySqlConnection(DB))
                {
                    connection.Open();
                    MySqlCommand sqlCommand = new MySqlCommand(query, connection);
                    MySqlDataReader R = sqlCommand.ExecuteReader();
                    fillColums(new string[] { "ID", "Enterprise Name", "Enterprise Owner", "Type", "Price","Amount_of_trade_ID" });

                    while (R.Read())
                    {
                        dataGridView1.Rows.Add(R[0].ToString(), R[1].ToString(), R[2].ToString(), R[3].ToString(), R[4].ToString(), R[5].ToString());
                    }
                    connection.Close();

                }
            }
            
        }

        bool checkExit(string ename,string oname, string type, float price, int aid)
        {
            bool res = false;

            string query = $" SELECT * FROM Trading_enterprisessUkraine where EnterpriseName='{ename}' and EnterpriseOwner='{oname}' and Type='{type}' and Price={price} and AmountOfTrade_ID={aid}";

            using (MySqlConnection connection = new MySqlConnection(DB))
            {
                connection.Open();
                MySqlCommand sqlCommand = new MySqlCommand(query, connection);
                MySqlDataReader R = sqlCommand.ExecuteReader();
                res = R.Read();
                connection.Close();
            }
            return res;
        }

        bool checkID(int id)
        {
            bool res = false;
            string query = $"select * from Trading_enterprisessUkraine where ID={id}";

            using (MySqlConnection connection = new MySqlConnection(DB))
            {
                connection.Open();
                MySqlCommand sqlCommand = new MySqlCommand(query, connection);
                MySqlDataReader R = sqlCommand.ExecuteReader();
                res = R.Read();
                connection.Close();
            }
            return res;
        }

        int getAlountID(string description)
        {
            int res;
            string query1 = $"select ID_am from Amount_of_trade where Description='{description}'";

            using (MySqlConnection conn = new MySqlConnection(DB))
            {
                conn.Open();
                MySqlCommand command = new MySqlCommand(query1, conn);
                MySqlDataReader R = command.ExecuteReader();
                if (R.Read())
                {
                    res = R.GetInt32(0);
                }
                else
                    res = -1;
                conn.Close();
         
            }
            return res;
        }

        void InsertOrg(string orgName, string orgOwner, string orgType, float price, int amountOfTradeID)
        {
            string query = $"use TradeDB; insert into Trading_enterprisessUkraine(EnterpriseName, EnterpriseOwner, Type, Price, AmountOfTrade_ID) values(n'{orgName}', n'{orgOwner}', n'{orgType}', {price}, {amountOfTradeID})";

            using (MySqlConnection connection =new MySqlConnection(DB))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(query, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            MessageBox.Show("Рядок було додано успішно");
        }

        void updateRecord(int recordId,string orgName, string orgOwner, string orgType, float price, int amountOfTradeID)
        {
            string query = $"use TradeDB; update Trading_enterprisessUkraine set EnterpriseName = n'{orgName}', EnterpriseOwner = n'{orgOwner}', Type = n'{orgType}', Price = {price}, AmountOfTrade_ID = {amountOfTradeID} where ID = {recordId}";
            using (MySqlConnection connection = new MySqlConnection(DB))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(query, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            MessageBox.Show("Рядок було змінено успішно");
        }

        void deleteRecord(int id)
        {
            string query = $"use TradeDB; delete from Trading_enterprisessUkraine where ID = {id}";
            using (MySqlConnection connection = new MySqlConnection(DB))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(query, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            MessageBox.Show("Рядок було видалено успішно");
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //get records
            if (radioButton1.Checked)
                LoadOrgs(checkBox2.Checked,checkBox1.Checked);

            //insert record
            if (radioButton2.Checked)
            {
                if(checkOrgName(textBox2.Text)&& chekOwner(textBox3.Text) && checkOrgType(textBox4.Text) && checkPrice(textBox5.Text))
                    if ( !checkExit(textBox2.Text, textBox3.Text, textBox4.Text, (float)Convert.ToDouble(textBox5.Text), getAlountID(comboBox1.Text)))
                        InsertOrg(textBox2.Text, textBox3.Text, textBox4.Text, (float)Convert.ToDouble(textBox5.Text), getAlountID(comboBox1.Text));
                    else
                        MessageBox.Show("Даний запис вже існує");
            }

            //update record
            if (radioButton3.Checked)
            {
                if (checkID(textBox1.Text) && checkOrgName(textBox2.Text) && chekOwner(textBox3.Text) && checkOrgType(textBox4.Text) && checkPrice(textBox5.Text))
                    if (checkID(Convert.ToInt32(textBox1.Text)))
                    {
                        if (chekOwner(textBox3.Text))
                            updateRecord(Convert.ToInt32(textBox1.Text), textBox2.Text, textBox3.Text, textBox4.Text, (float)Convert.ToDouble(textBox5.Text), getAlountID(comboBox1.Text));
                    }
                    else
                        MessageBox.Show($"Запис з ID={textBox1.Text} не існує", "Помилка");
            }

            //delete record
            if (radioButton4.Checked)
            {
                if (checkID(textBox1.Text))
                {
                    if (checkID(Convert.ToInt32(textBox1.Text)))
                        deleteRecord(Convert.ToInt32(textBox1.Text));
                    else
                        MessageBox.Show($"Запис з ID={textBox1.Text} не існує", "Помилка");
                }
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                textBox3.Enabled = false;
                textBox4.Enabled = false;
                textBox5.Enabled = false;
                comboBox1.Enabled = false;
                if (checkBox1.Checked)
                    comboBox1.Enabled = true;
                if (checkBox2.Checked)
                    textBox1.Enabled = true;
            }
            

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                textBox1.Enabled = false;
                textBox2.Enabled = true;
                textBox3.Enabled = true;
                textBox4.Enabled = true;
                textBox5.Enabled = true;
                comboBox1.Enabled = true;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                textBox3.Enabled = true;
                textBox4.Enabled = true;
                textBox5.Enabled = true;
                comboBox1.Enabled = true;
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            comboBox1.Enabled = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked && checkBox1.Checked)
                comboBox1.Enabled = true;
            else if (radioButton1.Checked) comboBox1.Enabled = false;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked && checkBox2.Checked)
                textBox1.Enabled = true;
            else if (radioButton1.Checked) textBox1.Enabled = false;
        }

        bool chekOwner(string str)
        {
            Regex regex = new Regex("^[a-zA-Zа-яА-Я]+$");
            Match M = regex.Match(str);
            if (!M.Success)
            {
                MessageBox.Show("Власник введений не вірно");
                return false;
            }
            return true;
        }

        bool checkOrgName(string org)
        {
            bool res = org != "";
            if (!res) MessageBox.Show("Невірно введено ім'я організації");
            return res;
        }

        bool checkOrgType(string org)
        {
            bool res = org != "";
            if (!res) MessageBox.Show("Невірно введеий тип");
            return res;
        }

        bool checkPrice(string price)
        {
            float tmp;
            bool res = float.TryParse(price, out tmp);
            if (!res) MessageBox.Show("Невірна ціна");
            return res;
        }

        bool checkID(string id)
        {
            bool res;
            int tmp;
            res=int.TryParse(id, out tmp);
            if (!res) MessageBox.Show("Невірний ID");
            return res;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SecondaryForm secondaryForm = new SecondaryForm();
            secondaryForm.parent = this;
            secondaryForm.Show();
            this.Hide();
       
        }

        private void Form1_VisibleChanged(object sender, EventArgs e)
        {
            loadAmountofTrade();
        }
    }
}
