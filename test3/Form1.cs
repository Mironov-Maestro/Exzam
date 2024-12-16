using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadDgv();
            Items();
        }

        public void LoadDgv()
        {
            string SqlText = "Select * From Tab";
            SqlDataAdapter adapter = new SqlDataAdapter(SqlText, DataBase.SqlConnect);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            Dgv.DataSource = dataTable;
        }

        private void Dgv_DoubleClick(object sender, EventArgs e)
        {
            textBox1.Text = Dgv.CurrentRow.Cells[0].Value.ToString();
            textBox2.Text = Dgv.CurrentRow.Cells[1].Value.ToString();
            textBox3.Text = Dgv.CurrentRow.Cells[2].Value.ToString();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string SqlText = $"Insert into Tab values ('{textBox2.Text}', '{textBox3.Text}')";
                SqlConnection connection = new SqlConnection(DataBase.SqlConnect);
                connection.Open();
                SqlCommand cmd = new SqlCommand(SqlText, connection);
                int kol = cmd.ExecuteNonQuery();
                connection.Close();

                MessageBox.Show("Запись добавлена", "Сообщение", MessageBoxButtons.OK);

                LoadDgv();
                flowLayoutPanel1.Controls.Clear();
                Items();
            }
            catch
            {
                MessageBox.Show("Ошибка", "Ошибка", MessageBoxButtons.OK);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string SqlText = $"Update Tab set stroka = '{textBox2.Text}', stroka2 = '{textBox3.Text}' where kod = '{Convert.ToInt32(textBox1.Text)}'";

                SqlConnection connection = new SqlConnection(DataBase.SqlConnect);
                connection.Open();
                SqlCommand cmd = new SqlCommand(SqlText, connection);
                int kol = cmd.ExecuteNonQuery();
                connection.Close();

                MessageBox.Show("Запись изменена", "Сообщение", MessageBoxButtons.OK);
                LoadDgv();
                flowLayoutPanel1.Controls.Clear();
                Items();
            }
            catch
            {
                MessageBox.Show("Ошибка", "Ошибка", MessageBoxButtons.OK);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string SqlText = $"Delete From Tab Where kod = '{Convert.ToInt32(textBox1.Text)}'";
                SqlConnection connection = new SqlConnection(DataBase.SqlConnect);
                connection.Open();
                SqlCommand cmd = new SqlCommand(SqlText, connection);
                int kol = cmd.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Запись удалена", "Сообщение", MessageBoxButtons.OK);
                LoadDgv();
                flowLayoutPanel1.Controls.Clear();
                Items();
            }
            catch
            {
                MessageBox.Show("Ошибка", "Ошибка", MessageBoxButtons.OK);
            }
        }

        public void Items()
        {
            SqlConnection connection = new SqlConnection(DataBase.SqlConnect);
            connection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "Select count(kod) From Tab";

            int Count = (int)cmd.ExecuteScalar();

            cmd.CommandText = "Select kod From Tab";
            SqlDataReader reader = cmd.ExecuteReader();

            List<int> Massive = new List<int>();
            while(reader.Read())
            {
                Massive.Add((int)reader.GetValue(0));
            }
            reader.Close();

            ListItem[] listItems = new ListItem[Count];

            for (int i = 0; i < Count; i++)
            {
                listItems[i] = new ListItem();

                cmd.CommandText = $"Select stroka from Tab where kod = {Massive[i]}";
                listItems[i].Stroka = (string)cmd.ExecuteScalar();

                cmd.CommandText = $"Select stroka2 from Tab where kod = {Massive[i]}";
                listItems[i].Stroka2 = (string)cmd.ExecuteScalar();

                if (flowLayoutPanel1.Controls.Count < 0)
                {
                    flowLayoutPanel1.Controls.Clear();
                }
                else
                {
                    flowLayoutPanel1.Controls.Add(listItems[i]);
                }
            }          
        }
    }
}
