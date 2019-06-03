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

namespace Task7Canon
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DGV.DataSource = null;
            DGV.Rows.Clear();
            DGV.Columns.Clear();


            DGV.Columns.Add("ID", "ID");
            DGV.Columns.Add("name", "name");
            DGV.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            DGV.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            string DB = "Data Source=DESKTOP-5582JAK\\SQLEXPRESS;Initial Catalog=TestDatabes;Integrated Security=True";
            string Query = "select * from Animals";
            using (SqlConnection Conn = new SqlConnection(DB))
            {
                Conn.Open();
                SqlCommand Comm = new SqlCommand(Query, Conn);
                SqlDataReader R = Comm.ExecuteReader();
                while (R.Read())
                {
                    DGV.Rows.Add(R[0].ToString(), R[1].ToString());
                }
                Conn.Close();
            }

        }
    }
}
