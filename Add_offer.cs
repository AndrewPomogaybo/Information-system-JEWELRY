using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace jewellery
{
    public partial class Add_offer : Form
    {
        Connector con = new Connector();
        Load_Data load = new Load_Data();
        ConnectionString str = new ConnectionString();
        public Add_offer()
        {
            InitializeComponent(); // загрузка формы
            load.LoadData("","order_create", dataGridView1, "SELECT * FROM order_create");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBoxName.Text == "" || textBoxSurname.Text == "" || textBoxPrice.Text == "" || textBox1.Text == "" || textBoxWeight.Text == "" )
            {
                MessageBox.Show("Ошибка ввода", "Внимание", MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
            }
            else 
            {
                string conStr = "";
                string name = textBoxName.Text;
                string surname = textBoxSurname.Text;
                int price = Convert.ToInt32(textBoxPrice.Text);
                string description = textBox1.Text;
                int weight = Convert.ToInt32(textBoxWeight.Text);
                string query = string.Format("INSERT INTO order_create (name,surname,weight, price, description) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')", name, surname, weight, price, description); //запрос на добавление данных

                con.Connection(conStr, query);
                MessageBox.Show("Данные успешно добавлены", "Оповещение", MessageBoxButtons.OK,
                               MessageBoxIcon.Information);
                load.LoadData("", "order_create", dataGridView1, "SELECT * FROM order_create"); //загрузка таблицы
                textBox1.Clear();
                textBoxName.Clear();
                textBoxSurname.Clear();
                textBoxPrice.Clear();
                textBoxWeight.Clear();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы действительно хотите удалить?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                string conStr = "";

                MySqlConnection connection = new MySqlConnection(str.ConString(conStr));

                connection.Open();

                MySqlCommand command = new MySqlCommand("DELETE FROM order_create WHERE id_order_create = @pId;", connection); // запрос на удаление данных

                command.Parameters.Add(new MySqlParameter("@pId", this.dataGridView1.CurrentRow.Cells["id_order_create"].Value));
                command.ExecuteNonQuery();
                DataTable table = new DataTable();
                dataGridView1.DataSource = table;
                command.Connection = new MySqlConnection(str.ConString(conStr));
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
                dataAdapter.Fill(table);
                dataGridView1.DataSource = table;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                connection.Close();
                load.LoadData("", "order_create", dataGridView1, "SELECT * FROM order_create"); //загрузка таблицы
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
           /* string conStr = "";
            int price = Convert.ToInt32(textBox6.Text);
            int weight = Convert.ToInt32(textBox5.Text);
            int id_client = Convert.ToInt32(textBox7.Text);
            string name = textBox4.Text;
            string query = string.Format("INSERT INTO order_create (id_client,weight, price, name) VALUES ('{0}', '{1}', '{2}', '{3}')", id_client, weight, price, name);

            con.Connection(conStr, query);
 
            textBox6.Clear();
            textBox5.Clear();
            textBox7.Clear();
            textBox4.Clear();*/
        }

        private void button4_Click(object sender, EventArgs e)
        {
            /*string conStr = "";

            MySqlConnection connection = new MySqlConnection(str.ConString(conStr));

            connection.Open();

            MySqlCommand command = new MySqlCommand("DELETE FROM order_create WHERE id_order_create = @pId;", connection);

            command.Parameters.Add(new MySqlParameter("@pId", this.dataGridView2.CurrentRow.Cells["id_order_create"].Value));
            command.ExecuteNonQuery();
            DataTable table = new DataTable();
            dataGridView2.DataSource = table;
            command.Connection = new MySqlConnection(str.ConString(conStr));
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
            dataAdapter.Fill(table);
            dataGridView2.DataSource = table;
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            connection.Close();
            load.LoadData("","order_create", dataGridView2);*/
        }

        private void Add_offer_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            this.Hide(); //кнопка выхода
        }
    }
}
