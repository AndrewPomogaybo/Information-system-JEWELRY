using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace jewellery
{
    public class Load_Data
    {
        ConnectionString con = new ConnectionString();
        ComboBox comboBox1 = new ComboBox();
        public void SQLConnection(string conStr, DataGridView dvg, string query)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(con.ConString(conStr));
                connection.Open();

                MySqlCommand command = new MySqlCommand(query, connection);

                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);

                DataTable table = new DataTable();

                dvg.DataSource = table;

                dvg.AllowUserToAddRows = false;
                dvg.AllowUserToResizeRows = false;
                dvg.ReadOnly = true;

                dataAdapter.Fill(table);

                dvg.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Ошибка подключения", "Внимание", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                MessageBox.Show(ex.ToString());
            }
        }

        public void LoadData(string conStr, string TableName, DataGridView dvg, string query)
        {

            SQLConnection(conStr, dvg, query);
        }

        public void LoadDataProducts(string conStr, DataGridView dvg)
        {
            string query = "SELECT id_product, name_category as 'Категория товара', name_weaving as 'Тип плетения', name_product as 'Наименование', weight as 'Вес', price as 'Цена', count as 'Количество' FROM products JOIN categories ON products.id_category = categories.id_category JOIN type_weaving ON products.id_type_weaving = type_weaving.id_type_weaving";

            SQLConnection(conStr, dvg, query);
        }

        public void LoadDataBasket(string conStr, DataGridView dvg)
        {
            string query = "SELECT id, name_product as 'Наименование', name_category as 'Категория', price as 'Цена' FROM basket LEFT JOIN products ON basket.id_product = products.id_product";
            SQLConnection(conStr, dvg, query);
        }


    }
}
