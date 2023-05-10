using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace jewellery
{
    class Connector
    {
        ConnectionString str = new ConnectionString();
        public void Connection(string conStr, string query)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(str.ConString(conStr)))
                {
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        connection.Open();
                        MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
                        DataTable table = new DataTable();
                        int numm = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                MessageBox.Show("Ошибка запроса", "Внимание", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        public void c(DataGridView dvg, string query, string conStr)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(str.ConString(conStr)))
                {
                    MySqlCommand command = new MySqlCommand(query, connection);

                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);

                    DataTable table = new DataTable();

                    dataAdapter.Fill(table);

                    dvg.DataSource = table;
                    dvg.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка запроса", "Внимание", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                MessageBox.Show(e.ToString());
            }
        }

        





        
    }
}
