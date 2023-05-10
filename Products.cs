using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace jewellery
{
    public partial class Products : Form
    {
        Connector con = new Connector();
        KeyPressNums onlynum = new KeyPressNums();
        OnlySymbols onlysym = new OnlySymbols();
        Load_Data load = new Load_Data();
        ConnectionString str = new ConnectionString();
        DataGridView dvg = new DataGridView();
        Delete_Data delete = new Delete_Data();
        ExportCSV export = new ExportCSV();
        public Products()
        {
            InitializeComponent();
            load.LoadDataProducts("", dataGridView1);
            textBox6.Visible = false;
            next(dataGridView1);
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox4.Text == "" || textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox3.Text == "" || textBox7.Text == "" || textBox5.Text == "")
            {
                MessageBox.Show("Данные не введены", "Внимание", MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    string conStr = "";
                    string id_category = textBox4.Text;
                    string name_product = textBox1.Text;
                    int weight = Convert.ToInt32(textBox2.Text);
                    int price = Convert.ToInt32(textBox3.Text);
                    int count = Convert.ToInt32(textBox7.Text);
                    int id_typew = Convert.ToInt32(textBox5.Text);

                    string query = string.Format("INSERT INTO products (id_category, id_type_weaving, name_product, weight, price, count) VALUES ('{0}', '{1}', '{2}','{3}', '{4}', '{5}')", id_category, id_typew, name_product, weight, price, count);
                    con.Connection(conStr, query);
                    load.LoadDataProducts("", dataGridView1); //добавление товаров
                    next(dataGridView1);
                    MessageBox.Show("Данные успешно добавлены", "Оповещение", MessageBoxButtons.OK,
                               MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                textBox4.Clear();
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox7.Clear();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы действительно хотите удалить?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                string conStr = "";

                MySqlConnection connection = new MySqlConnection(str.ConString(conStr));//строка подключения

                connection.Open();

                MySqlCommand command = new MySqlCommand("DELETE FROM products WHERE id_product = @pId;", connection); //удаление данных

                command.Parameters.Add(new MySqlParameter("@pId", this.dataGridView1.CurrentRow.Cells["id_product"].Value));
                command.ExecuteNonQuery();
                DataTable table = new DataTable();
                dataGridView1.DataSource = table;
                command.Connection = new MySqlConnection(str.ConString(conStr));
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
                dataAdapter.Fill(table);
                dataGridView1.DataSource = table;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                connection.Close();

                load.LoadDataProducts("", dataGridView1);
                next(dataGridView1);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string conStr = "";
            string query = "UPDATE products SET id_category= " + textBox4.Text +  ", name_product = '" + textBox1.Text + "', weight = " + textBox2.Text + ", price = " + textBox3.Text + ", count = " + textBox7.Text + " WHERE id_product = " + textBox6.Text + ";";

            con.c(dvg, query, conStr); //редактирование данных
            load.LoadDataProducts("", dataGridView1);
            next(dataGridView1);
            textBox4.Clear();
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox6.Clear();
            textBox7.Clear();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                textBox4.Text = row.Cells[1].Value.ToString();
                textBox2.Text = row.Cells[4].Value.ToString();
                textBox1.Text = row.Cells[3].Value.ToString();
                textBox5.Text = row.Cells[2].Value.ToString();
                textBox6.Text = row.Cells[0].Value.ToString();
                textBox3.Text = row.Cells[5].Value.ToString();
                textBox7.Text = row.Cells[6].Value.ToString();
            }
        }

        double count;
        int NextPage = 1;
        int NumInPage = 5;

        public void next(DataGridView dvg)
        {
            string conStr = "";
            try
            {
                MySqlConnection connection = new MySqlConnection(str.ConString(conStr));
                connection.Open();

                string queryLimit = "select id_product, name_category as 'Категория товара', name_weaving as 'Тип плетения', name_product as 'Наименование', weight as 'Вес', price as 'Цена', count as 'Количество' from products JOIN categories ON products.id_category = categories.id_category JOIN type_weaving ON products.id_type_weaving = type_weaving.id_type_weaving limit " + NumInPage + " offset " + (NumInPage * (NextPage - 1));
                MySqlCommand commandOut = new MySqlCommand(queryLimit, connection);
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(commandOut);
                DataTable table = new DataTable();
                dataAdapter.Fill(table);
                dvg.DataSource = table;

                string queryCount = "select count(*) from products;";
                MySqlCommand commandCount = new MySqlCommand(queryCount, connection);
                int MaxCount = Convert.ToInt32(commandCount.ExecuteScalar());
                count = Math.Ceiling(Convert.ToDouble(MaxCount) / NumInPage); //Возвращает наименьшее
                                                                              //целое число, которое больше
                                                                              //или равно указанному числу.
                connection.Close();
 
                label13.Text = count.ToString();
                label14.Text = NextPage.ToString();

                if (NextPage == 1)
                {
                    button_back.Visible = false;
                }
                if (NextPage == count)
                {
                    button_next.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }



        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = '\0';
            onlysym.GetOnlySymbols(ch, e);
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = '\0';
            onlynum.OnlyNums(ch, e);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = '\0';
            onlynum.OnlyNums(ch, e);
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = '\0';
            onlynum.OnlyNums(ch, e);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Basket basket = new Basket();
            basket.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide(); //выход
        }

        private void button_next_Click(object sender, EventArgs e)
        {
            NextPage++;
            next(dataGridView1);//листание пагинации вперед
            button_back.Visible = true;
        }
        
        private void button_back_Click(object sender, EventArgs e)
        {
            NextPage--;
            next(dataGridView1);//листание пагинации назад
            button_next.Visible = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string conStr = "";
            string query = "SELECT id_product, name_category as 'Категория товара', name_weaving as 'Тип плетения', name_product as 'Наименование', weight as 'Вес', price as 'Цена', count as 'Количество' FROM products JOIN categories ON products.id_category = categories.id_category JOIN type_weaving ON products.id_type_weaving = type_weaving.id_type_weaving WHERE name_category = 'Цепи';";
            con.c(dataGridView1, query, conStr); 
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string conStr = "";
            string query = "SELECT id_product, name_category as 'Категория товара', name_weaving as 'Тип плетения', name_product as 'Наименование', weight as 'Вес', price as 'Цена', count as 'Количество' FROM products JOIN categories ON products.id_category = categories.id_category JOIN type_weaving ON products.id_type_weaving = type_weaving.id_type_weaving ORDER BY name_product;";
            con.c(dataGridView1, query, conStr);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            try
            {
                export.ToCSV(dataGridView1, "products.csv");
                MessageBox.Show("Данные успешно экспортированы", "Оповещение", MessageBoxButtons.OK,
                               MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка ввода", "Внимание", MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
            }
        }

    }
}
