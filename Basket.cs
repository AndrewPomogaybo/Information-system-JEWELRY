using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySql.Data;

namespace jewellery
{
    public partial class Basket : Form
    {
        private System.Data.DataSet dataSet;
        Connector con = new Connector();
        KeyPressNums onlynums = new KeyPressNums();
        Load_Data load = new Load_Data();
        ConnectionString str = new ConnectionString();

        public Basket()
        {
            InitializeComponent();
            load.LoadDataProducts("", dataGridView1);
            LoadD("", dataGridView2);
            textBox1.Visible = true;
            textBox3.Visible = true;
            textBox4.Visible = true;
            textBox6.Visible = true;
            textBox5.Visible = false;
            textBox11.Visible = false;
        }

        

        public void LoadD(string conStr, DataGridView dvg)
        {
            string query = "SELECT id, name_prod as 'Наименование', name_category as 'Категория', amount as 'Количество', cost as 'Цена' FROM basket;";
            load.SQLConnection(conStr, dvg, query);
        }

        public void Sale(string  name_category, int amount, int cost, int id_product, string name_product)
        {

            string conStr = "";
            MySqlConnection conn = new MySqlConnection(str.ConString(conStr));
            conn.Open();

            MySqlTransaction trans = conn.BeginTransaction();

            MySqlCommand sql1 = new MySqlCommand(@"UPDATE Products SET count = count - " + amount.ToString() + " WHERE id_product = " + id_product.ToString() + "; ", conn);
            sql1.Transaction = trans;

            MySqlCommand sql2 = conn.CreateCommand();
            sql2.CommandText = "INSERT INTO basket(id, name_prod, name_category, amount, cost) VALUES ('" + id_product + "', '"+name_product +"', '" + name_category + "'," + amount.ToString() + ", " + cost.ToString() + " * amount" + ");";
            sql2.Transaction = trans;

            
                sql1.ExecuteNonQuery();
                sql2.ExecuteNonQuery();

                trans.Commit();
            
            
                
                
            

            conn.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            foreach (DataGridViewRow row in dataGridView1.SelectedRows) //заполнение ячеек данными
            {
                textBox3.Text = row.Cells[5].Value.ToString();
                textBox10.Text = row.Cells[0].Value.ToString();
                textBox1.Text = row.Cells[1].Value.ToString();
                textBox9.Text = row.Cells[2].Value.ToString();
                textBox12.Text = row.Cells[3].Value.ToString();

            }
            try
            {
                int amount = Convert.ToInt32(textBox2.Text);
                string name_category = textBox1.Text;
                int price = Convert.ToInt32(textBox3.Text) * Convert.ToInt32(textBox2.Text);
                int id_product = Convert.ToInt32(textBox10.Text);
                 string name_product = textBox12.Text;

                Sale(textBox1.Text, Convert.ToInt32(textBox2.Text), Convert.ToInt32(textBox3.Text), Convert.ToInt32(textBox10.Text), textBox12.Text); //метод продажи
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка ввода", "Внимание", MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
            }
            load.LoadDataProducts("", dataGridView1); ; //загрузка таблицы
            LoadD("", dataGridView2);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            foreach (DataGridViewRow row in dataGridView2.SelectedRows) //заполнение ячеек данными
            {
                textBox6.Text = row.Cells[4].Value.ToString();
                textBox11.Text = row.Cells[1].Value.ToString();
                textBox7.Text = row.Cells[2].Value.ToString();
                textBox8.Text = row.Cells[3].Value.ToString();
            }

            string conStr = "";
            try
            {
                string id_basket = textBox4.Text;
                string name_category = textBox7.Text;
                string name_product = textBox11.Text;
                int amount = Convert.ToInt32(textBox8.Text);
                int cost = Convert.ToInt32(textBox6.Text);

                try
                {
                    string query1 = string.Format("INSERT INTO order_buy (ProductName, ProductCategory, count, price) VALUES ('" + name_product + "', '" + name_category + "', " + amount + ", " + cost + ");");
                    string query2 = string.Format("DELETE FROM basket WHERE id = @pId;");

                    using (MySqlConnection connection = new MySqlConnection(str.ConString(conStr)))
                    {

                        using (MySqlCommand command = new MySqlCommand(query1, connection))
                        {
                            connection.Open();
                            int numm = command.ExecuteNonQuery();
                        }

                        using (MySqlCommand command = new MySqlCommand(query2, connection))
                        {
                            command.Parameters.Add(new MySqlParameter("@pId", this.dataGridView2.CurrentRow.Cells["id"].Value));
                            int numm = command.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка запроса", "Внимание", MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
            }
            MessageBox.Show("Заказ оформлен", "Оповещение", MessageBoxButtons.OK,
                               MessageBoxIcon.Information);
            load.LoadData("", "basket", dataGridView2, "SELECT * FROM basket");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Products products = new Products(); //выход из корзины
            products.ShowDialog();
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = '\0';
            onlynums.OnlyNums(ch, e); //ограничение ввода
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string conStr = "";
            string query2 = string.Format("DELETE FROM basket WHERE id = @pId;");
            
            MySqlConnection connection = new MySqlConnection(str.ConString(conStr));
            connection.Open();
            MySqlTransaction trans = connection.BeginTransaction();

            MySqlCommand command = new MySqlCommand(query2, connection);
            command.Parameters.Add(new MySqlParameter("@pId", this.dataGridView2.CurrentRow.Cells["id"].Value));
            int numm = command.ExecuteNonQuery();
            connection.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            foreach (DataGridViewRow row in dataGridView2.SelectedRows)
            {
                textBox6.Text = row.Cells[0].Value.ToString();
                textBox11.Text = row.Cells[1].Value.ToString();
                textBox7.Text = row.Cells[2].Value.ToString();
                textBox8.Text = row.Cells[3].Value.ToString();
            }

            string conStr = "";

            string name_category = textBox11.Text;
            string name_product = textBox6.Text;
            //int amount = Convert.ToInt32(textBox7.Text);
            int cost = Convert.ToInt32(textBox8.Text);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Basket_Load(object sender, EventArgs e)
        {
            
        }
    }
}
