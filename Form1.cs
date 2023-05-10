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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string Userlogin = textBox1.Text;
            string Userpassword = textBox2.Text;

            string connection_str = "host=localhost; database = jewellery; uid = root; pwd =12345;";

            MySqlConnection connection = new MySqlConnection(connection_str);

            connection.Open();

            MySqlCommand command = new MySqlCommand("SELECT * FROM user WHERE login = @uL AND pwd = @uP;", connection);

            command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = Userlogin;
            command.Parameters.Add("@uP", MySqlDbType.VarChar).Value = Userpassword;

            DataTable table = new DataTable();

            command.Connection = new MySqlConnection(connection_str);
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);

            dataAdapter.Fill(table);

            if (Userlogin == "admin" && Userpassword == "admin") //разграничение доступа
            {
                this.Hide();
                Admin Admin = new Admin();
                Admin.ShowDialog();
            }
            else if (Userlogin == "master" && Userpassword == "master")
            {
                this.Hide();
                Offers offers = new Offers();
                offers.ShowDialog();
            }
            else if (table.Rows.Count > 0)
            {
                this.Hide();
                User User = new User();
                User.ShowDialog();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль", "Внимание", MessageBoxButtons.OK,    //сообщение об ошибке
                               MessageBoxIcon.Error);
            }

        }
    }
}
