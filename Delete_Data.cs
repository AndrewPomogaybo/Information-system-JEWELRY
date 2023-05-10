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
    public class Delete_Data
    {
        Connector con = new Connector();
        DataGridView dataGridView1 = new DataGridView();
        ConnectionString str = new ConnectionString();

        public void DeleteProduct(string conStr, string query, DataGridView dataGridView1)
        {
            using (MySqlConnection connection = new MySqlConnection(str.ConString(conStr)))
            {
                query = "DELETE FROM products WHERE id_product = @pId;";
                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.Add(new MySqlParameter("@pId", this.dataGridView1.CurrentRow.Cells["id_product"].Value));
                command.ExecuteNonQuery();
                DataTable table = new DataTable();
                dataGridView1.DataSource = table;
                command.Connection = new MySqlConnection(str.ConString(conStr));
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
                dataAdapter.Fill(table);
                dataGridView1.DataSource = table;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
        }
    }
}
