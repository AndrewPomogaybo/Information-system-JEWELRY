using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace jewellery
{
    

    public partial class DB : Form
    {
        Connector con = new Connector();
        KeyPressNums onlynum = new KeyPressNums();
        OnlySymbols onlysym = new OnlySymbols();
        Load_Data load = new Load_Data();
        ConnectionString str = new ConnectionString();
        DataGridView dvg = new DataGridView();
        ExportCSV export = new ExportCSV();
        ImportCSV import = new ImportCSV();

        double count;
        int NextPage = 1;
        int NumInPage = 5;

        public DB()
        {
            InitializeComponent();
            load.LoadData("", "user", dataGridView2, "SELECT id_user, login as 'Логин', pwd as 'Пароль', role as 'Роль' FROM user;");
            load.LoadData("", "clients", dataGridView3, "SELECT id_client, surname as 'Фамилия', name as 'Имя', patronymic as 'Отчество', phone_number as 'Номер телефона' FROM clients;");
            load.LoadData("", "categories", dataGridView4, "SELECT * FROM categories");
            load.LoadData("", "type_weaving", dataGridView5, "SELECT * FROM type_weaving");
            textBox_id.Visible = false;
            label4.Visible = false;
            textBox2.Visible = false;
            textBox3.Visible = false;
            next("clients", dataGridView3);
            
        }



        private void button13_Click(object sender, EventArgs e)
        {
           string conStr = "";
           string login = textBox1.Text;
           string pwd = textBox4.Text;
           string role = comboBox1.Text;

            string query = string.Format("INSERT INTO user (login, pwd, role) VALUES ('{0}', '{1}', '{2}')", login, pwd, role);//запрос на добавление данных
            
            con.Connection(conStr, query);
            MessageBox.Show("Данные успешно добавлены", "Оповещение", MessageBoxButtons.OK,    
                               MessageBoxIcon.Information); // сообщение об успешном добавление
            load.LoadData("", "user", dataGridView2, "SELECT id_user, login as 'Логин', pwd as 'Пароль', role as 'Роль' FROM user;"); //загрузка таблицы
            textBox1.Clear();
            textBox4.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы действительно хотите удалить?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Warning); //Оповещение

            if (result == DialogResult.Yes)
            {
                string conStr = "";
                MySqlConnection connection = new MySqlConnection(str.ConString(conStr));

                connection.Open();
                string query = "DELETE FROM user WHERE id_user = @pId;";  //запрос на удаление
                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.Add(new MySqlParameter("@pId", this.dataGridView2.CurrentRow.Cells["id_user"].Value));
                command.ExecuteNonQuery();
                DataTable table = new DataTable();
                dataGridView2.DataSource = table;
                command.Connection = new MySqlConnection(str.ConString(conStr));
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
                dataAdapter.Fill(table);
                dataGridView2.DataSource = table;
                dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                connection.Close();

                load.LoadData("", "user", dataGridView2, "SELECT id_user, login as 'Логин', pwd as 'Пароль', role as 'Роль' FROM user;"); // загрузка таблицы
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string conStr = "";
            string query = "UPDATE users SET login= '" + textBox1.Text + "', pwd = '" + textBox4.Text + ", role = '" + comboBox1.Text + "'  WHERE id_user = @pId;"; //запрос на редактирование

            con.c(dvg, query, conStr);
            load.LoadData("", "user", dataGridView2, "SELECT id_user, login as 'Логин', pwd as 'Пароль', role as 'Роль' FROM user;"); //закгрузка таблицы
            textBox4.Clear();
            textBox_name.Clear();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (textBox8.Text == "")
            {
                MessageBox.Show("Ошибка ввода", "Внимание", MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
            }
            else
            {
                string conStr = "";
                string name = textBox8.Text;

                string query = string.Format("INSERT INTO categories (name_category) VALUES ('{0}')", name); //запрос на добавление

                con.Connection(conStr, query);
                MessageBox.Show("Данные успешно добавлены", "Оповещение", MessageBoxButtons.OK,
                               MessageBoxIcon.Information);
                load.LoadData("", "categories", dataGridView4, "SELECT * FROM categories");
                textBox8.Clear();
            }
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            string search = textBox11.Text;

            string conStr = "";

            MySqlConnection connection = new MySqlConnection(str.ConString(conStr));

            connection.Open();

            string query = "SELECT * FROM user WHERE concat(login) LIKE '" + textBox11.Text + "%';"; //поиск пользователей
            if (search.Length == 0)
            {
                query = "SELECT * FROM user";
            }

            MySqlCommand command = new MySqlCommand(query, connection);

            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);

            DataTable table = new DataTable();

            dataGridView2.DataSource = table;

            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.AllowUserToResizeRows = false;
            dataGridView2.ReadOnly = true;

            dataAdapter.Fill(table);

            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            connection.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (textBox_surname.Text == "" || textBox_name.Text == "" || textBox_pat.Text == "" || textBox_phone.Text == "")
            {
                MessageBox.Show("Ошибка ввода", "Внимание", MessageBoxButtons.OK,
                               MessageBoxIcon.Error); //сообщение об ошшибке
            }
            else
            {
                string name = textBox_name.Text;
                string surname = textBox_surname.Text;
                string patronymic = textBox_pat.Text;
                string phone_number = textBox_phone.Text;
                string conStr = "";

                try
                {
                    string query = string.Format("INSERT INTO clients (name, surname, patronymic, phone_number) VALUES ('{0}', '{1}', '{2}', '{3}')", name, surname, patronymic, phone_number); //запрос на добавление клиентов

                    con.Connection(conStr, query);
                    MessageBox.Show("Данные успешно добавлены", "Оповещение", MessageBoxButtons.OK,
                               MessageBoxIcon.Information); //оповещение что данные добавлены
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                load.LoadData("", "clients", dataGridView3, "SELECT * FROM clients"); //загрузка таблицы
                textBox_name.Clear();
                textBox_surname.Clear();
                textBox_pat.Clear();
                textBox_phone.Clear();
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы действительно хотите удалить?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                string conStr = "";

                MySqlConnection connection = new MySqlConnection(str.ConString(conStr));

                connection.Open();

                MySqlCommand command = new MySqlCommand("DELETE FROM clients WHERE id_client = @pId;", connection); //запрос на удаление данных

                command.Parameters.Add(new MySqlParameter("@pId", this.dataGridView3.CurrentRow.Cells["id_client"].Value));
                command.ExecuteNonQuery();
                DataTable table = new DataTable();
                dataGridView2.DataSource = table;
                command.Connection = new MySqlConnection(str.ConString(conStr));
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
                dataAdapter.Fill(table);
                dataGridView2.DataSource = table;
                dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                connection.Close();

                load.LoadData("", "clients", dataGridView3, "SELECT * FROM clients"); //загрузка таблицы
            }
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            string search = textBox12.Text;

            string conStr = "";

            MySqlConnection connection = new MySqlConnection(str.ConString(conStr));

            connection.Open();

            string query = "SELECT * FROM clients WHERE concat(name, surname, patronymic, phone_number) LIKE '" + textBox12.Text + "%';"; // поиск клиентов по имени
            if (search.Length == 0)
            {
                query = "SELECT * FROM clients";
            }
            MySqlCommand command = new MySqlCommand(query, connection);

            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);

            DataTable table = new DataTable();

            dataGridView3.DataSource = table;

            dataGridView3.AllowUserToAddRows = false;
            dataGridView3.AllowUserToResizeRows = false;
            dataGridView3.ReadOnly = true;

            dataAdapter.Fill(table);

            dataGridView3.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            connection.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы действительно хотите удалить?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                string conStr = "";

                MySqlConnection connection = new MySqlConnection(str.ConString(conStr));

                connection.Open();

                MySqlCommand command = new MySqlCommand("DELETE FROM categories WHERE id_category = @pId;", connection); //запрос на удаление

                command.Parameters.Add(new MySqlParameter("@pId", this.dataGridView4.CurrentRow.Cells["id_category"].Value));
                command.ExecuteNonQuery();
                DataTable table = new DataTable();
                dataGridView4.DataSource = table;
                command.Connection = new MySqlConnection(str.ConString(conStr));
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
                dataAdapter.Fill(table);
                dataGridView4.DataSource = table;
                dataGridView4.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                connection.Close();

                load.LoadData("", "categories", dataGridView4, "SELECT * FROM categories"); //загрузка таблицы
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (textBox9.Text == "")
            {
                MessageBox.Show("Ошибка ввода", "Внимание", MessageBoxButtons.OK,
                               MessageBoxIcon.Error); // сообщение об ошибке
            }
            else
            {
                string name = textBox9.Text;
                string conStr = "";

                string query = string.Format("INSERT INTO type_weaving (name) VALUES ('{0}')", name); //запрос на добавление

                con.Connection(conStr, query);
                MessageBox.Show("Данные успешно добавлены", "Оповещение", MessageBoxButtons.OK,
                               MessageBoxIcon.Information);
                load.LoadData("", "type_weaving", dataGridView5, "SELECT * FROM type_weaving");
                textBox9.Clear();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы действительно хотите удалить?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                string conStr = "";

                MySqlConnection connection = new MySqlConnection(str.ConString(conStr));

                connection.Open();

                MySqlCommand command = new MySqlCommand("DELETE FROM type_weaving WHERE id_type_weaving = @pId;", connection); //удаление данных

                command.Parameters.Add(new MySqlParameter("@pId", this.dataGridView5.CurrentRow.Cells["id_type_weaving"].Value));
                command.ExecuteNonQuery();
                DataTable table = new DataTable();
                dataGridView5.DataSource = table;
                command.Connection = new MySqlConnection(str.ConString(conStr));
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
                dataAdapter.Fill(table);
                dataGridView5.DataSource = table;
                dataGridView5.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                connection.Close();

                load.LoadData("", "type_weaving", dataGridView5, "SELECT * FROM type_weaving"); //загрузка таблицы
            }
        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            string search = textBox13.Text;

            string conStr = "";

            MySqlConnection connection = new MySqlConnection(str.ConString(conStr));

            connection.Open();

            string query = "SELECT * FROM categories WHERE concat(name) LIKE '" + textBox13.Text + "%';"; //поиск по категориям
            if (search.Length == 0)
            {
                query = "SELECT * FROM categories";
            }
            MySqlCommand command = new MySqlCommand(query, connection);

            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);

            DataTable table = new DataTable();

            dataGridView4.DataSource = table;

            dataGridView4.AllowUserToAddRows = false;
            dataGridView4.AllowUserToResizeRows = false;
            dataGridView4.ReadOnly = true;

            dataAdapter.Fill(table);

            dataGridView4.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            connection.Close();
        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {
            string search = textBox14.Text;

            string conStr = "";

            MySqlConnection connection = new MySqlConnection(str.ConString(conStr)); //подключение к БД

            connection.Open();

            string query = "SELECT * FROM type_weaving WHERE concat(name) LIKE '" + textBox14.Text + "%';"; // поиск по типам плетения
            if (search.Length == 0)
            {
                query = "SELECT * FROM type_weaving";
            }
            MySqlCommand command = new MySqlCommand(query, connection);

            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);

            DataTable table = new DataTable();

            dataGridView5.DataSource = table;

            dataGridView5.AllowUserToAddRows = false;
            dataGridView5.AllowUserToResizeRows = false;
            dataGridView5.ReadOnly = true;



            dataAdapter.Fill(table); //заполнение таблички

            dataGridView5.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            connection.Close();
        }

        
        private void button3_Click(object sender, EventArgs e)
        {
            string conStr = "";
            string query = "UPDATE clients SET name= '" + textBox_name.Text + "', surname = '" + textBox_surname.Text + "', patronymic = '" + textBox_pat.Text + "', phone_number = '" + textBox_phone.Text + "'  WHERE id_client = " + textBox_id.Text + ";"; //запрос на редактирование

            con.c(dataGridView3, query, conStr);
            load.LoadData("","clients", dataGridView3, "SELECT * FROM clients"); //загрузка таблицы
            textBox_name.Clear();
            textBox_surname.Clear();
            textBox_pat.Clear();
            textBox_phone.Clear();
            textBox_id.Clear();
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView3.SelectedRows)
            {
                textBox_name.Text = row.Cells[1].Value.ToString();
                textBox_surname.Text = row.Cells[2].Value.ToString(); //заполнение текстбокса по индексу
                textBox_pat.Text = row.Cells[3].Value.ToString();
                textBox_phone.Text = row.Cells[4].Value.ToString();
                textBox_id.Text = row.Cells[0].Value.ToString();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string conStr = "";
            string query = "UPDATE categories SET name= '" + textBox8.Text +  "'  WHERE id_category = " + textBox2.Text + ";"; //запрос на редактирование

            con.c(dvg, query, conStr);
            load.LoadData("","categories", dataGridView4, "SELECT * FROM categories"); //загрузка таблицы
            textBox8.Clear();
            textBox2.Clear();
        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView4.SelectedRows)
            {
                textBox8.Text = row.Cells[1].Value.ToString();
                textBox2.Text = row.Cells[0].Value.ToString();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string conStr = "";
            string query = "UPDATE type_weaving SET name= '" + textBox9.Text + "'  WHERE id_type_weaving = " + textBox3.Text + ";"; //запрос на редактирование

            con.c(dvg, query, conStr);
            load.LoadData("","type_weaving", dataGridView5, "SELECT * FROM type_weaving"); // загрузка таблицы
            textBox9.Clear();
            textBox3.Clear();
        }

        public void next(string TableName, DataGridView dvg)
        {
            string conStr = "";
            try
            {
                MySqlConnection connection = new MySqlConnection(str.ConString(conStr));// строка подключения
                connection.Open();

                string queryLimit = "select * from " + TableName + " limit " + NumInPage + " offset " + (NumInPage * (NextPage - 1));
                MySqlCommand commandOut = new MySqlCommand(queryLimit, connection);
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(commandOut);
                DataTable table = new DataTable();
                dataAdapter.Fill(table);
                dvg.DataSource = table;

                string queryCount = "select count(*) from " + TableName + ";";
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

        private void dataGridView5_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView5.SelectedRows)
            {
                textBox9.Text = row.Cells[1].Value.ToString();
                textBox3.Text = row.Cells[0].Value.ToString();
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string conStr = "";
            string query = "SELECT * FROM clients ORDER BY Surname"; //сортировка клиентов по фамилии
            con.c(dataGridView3, query, conStr);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            string conStr = "";
            string query = "SELECT * FROM type_weaving ORDER BY name"; //сортировка типов плетения по имени
            con.c(dataGridView5, query, conStr);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            string conStr = "";
            string query = "SELECT * FROM clients WHERE surname = '" + textBox5.Text + "';";//фильтрация клиентотв по фамилии
            con.c(dataGridView3, query, conStr);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            string conStr = "";
            string query = "SELECT * FROM type_weaving WHERE name = '" + textBox6.Text + "';"; //фильтрация типов плетения по названию
            con.c(dataGridView5, query, conStr);
        }

        private void textBox_phone_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = '\0';
            onlynum.OnlyNums(ch, e);                    //ограничение ввода по числам
        }

        private void textBox_name_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = '\0';
            onlysym.GetOnlySymbols(ch, e);      //ограничение ввода только символы
        }

        private void textBox_surname_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = '\0';
            onlysym.GetOnlySymbols(ch, e);
        }

        private void textBox_pat_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = '\0';
            onlysym.GetOnlySymbols(ch, e);
        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = '\0';
            onlysym.GetOnlySymbols(ch, e);
        }

        private void textBox9_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = '\0';
            onlysym.GetOnlySymbols(ch, e);
        }


        private void textBox_surname_TextChanged(object sender, EventArgs e)
        {

        }

        private void button17_Click(object sender, EventArgs e)
        {
            this.Hide(); //выход
        }

        private void button_next_Click(object sender, EventArgs e)
        {
            NextPage++;
            next("clients", dataGridView3); //перелистывание пагинации вперед
            button_back.Visible = true;
        }

        private void button_back_Click(object sender, EventArgs e)
        {
            NextPage--;//перелиистывании пагинации назад
            next("clients", dataGridView3);
            button_next.Visible = true;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            try
            {
                export.ToCSV(dataGridView3, Directory.GetCurrentDirectory() + "clients.csv"); //экспорт в CSV
                MessageBox.Show("Данные успешно экспортированы", "Оповещение", MessageBoxButtons.OK,
                               MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка ввода", "Внимание", MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            try
            {
                export.ToCSV(dataGridView2, Directory.GetCurrentDirectory() + "users.csv");
                MessageBox.Show("Данные успешно экспортированы", "Оповещение", MessageBoxButtons.OK,
                               MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка ввода", "Внимание", MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            try
            {
                export.ToCSV(dataGridView4, Directory.GetCurrentDirectory() + "categories.csv");
                MessageBox.Show("Данные успешно экспортированы", "Оповещение", MessageBoxButtons.OK,
                               MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка ввода", "Внимание", MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            try
            {
                export.ToCSV(dataGridView5, Directory.GetCurrentDirectory() + "type_weaving.csv");
                MessageBox.Show("Данные успешно экспортированы", "Оповещение", MessageBoxButtons.OK,
                               MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка ввода", "Внимание", MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
            }
        }

        private void DB_Load(object sender, EventArgs e)
        {
            string conStr = "";
            string Sql = "select role from user";
            string query = "select name_category from categories";
            MySqlConnection conn = new MySqlConnection(str.ConString(conStr));
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(Sql, conn);
           // MySqlCommand cmd2 = new MySqlCommand(query, conn);

            MySqlDataReader DR = cmd.ExecuteReader();
           // MySqlDataReader DR2 = cmd2.ExecuteReader();


            while (DR.Read())                   //заполнение комбобокса данными из БД
            {
                comboBox1.Items.Add(DR[0]);

            }
        }

        

        private void button19_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog fd = new OpenFileDialog();

                fd.DefaultExt = "*.csv";
                fd.Filter = "CSV files (*.csv)|*.csv";
                fd.ShowDialog();
                string conStr = "";
                var lineNumber = 0;
                using (MySqlConnection conn = new MySqlConnection(str.ConString(conStr)))
                {
                    conn.Open();
                    string file = fd.FileName;

                    using (StreamReader reader = new StreamReader(file, Encoding.GetEncoding("windows-1251")))
                    {
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine(); //импорт данных из CSV
                            if (lineNumber != 0)
                            {
                                var values = line.Split(';');

                                var sql = "INSERT INTO clients VALUES (" + values[0] + ",'" + values[1] + "','" + values[2] + "','" + values[3] + "','" + values[4] + "')";

                                var cmd = new MySqlCommand();
                                cmd.CommandText = sql;
                                cmd.CommandType = System.Data.CommandType.Text;
                                cmd.Connection = conn;
                                cmd.ExecuteNonQuery();
                            }
                            lineNumber++;
                        }
                    }
                    MessageBox.Show("Данные успешно импортированы", "Оповещение", MessageBoxButtons.OK,
                              MessageBoxIcon.Information);
                    conn.Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка импорта", "Внимание", MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
            }


            load.LoadData("", "clients", dataGridView3, "SELECT * FROM clients");
            next("clients", dataGridView3);
        }

        private void button18_Click_1(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog fd = new OpenFileDialog();

                fd.DefaultExt = "*.csv";
                fd.Filter = "CSV files (*.csv)|*.csv";
                fd.ShowDialog();
                string conStr = "";
                var lineNumber = 0;
                using (MySqlConnection conn = new MySqlConnection(str.ConString(conStr)))
                {
                    conn.Open();//импорт данных из CSV
                    string file = fd.FileName;

                    using (StreamReader reader = new StreamReader(file, Encoding.GetEncoding("windows-1251")))
                    {
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            if (lineNumber != 0)
                            {
                                var values = line.Split(';');

                                var sql = "INSERT INTO user VALUES (" + values[0] + ",'" + values[1] + "','" + values[2] + "','" + values[3] + "')";

                                var cmd = new MySqlCommand();
                                cmd.CommandText = sql;
                                cmd.CommandType = System.Data.CommandType.Text;
                                cmd.Connection = conn;
                                cmd.ExecuteNonQuery();
                            }
                            lineNumber++;
                        }
                    }
                    MessageBox.Show("Данные успешно импортированы", "Оповещение", MessageBoxButtons.OK,
                              MessageBoxIcon.Information);
                    conn.Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка импорта", "Внимание", MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
            }
            load.LoadData("", "user", dataGridView2, "SELECT id_user, login as 'Логин', pwd as 'Пароль', role as 'Роль' FROM user;");
        }

        private void button20_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog fd = new OpenFileDialog();

                fd.DefaultExt = "*.csv";
                fd.Filter = "CSV files (*.csv)|*.csv";
                fd.ShowDialog();
                string conStr = "";
                var lineNumber = 0;
                using (MySqlConnection conn = new MySqlConnection(str.ConString(conStr)))
                {//импорт данных из CSV
                    conn.Open();
                    string file = fd.FileName;

                    using (StreamReader reader = new StreamReader(file, Encoding.GetEncoding("windows-1251")))
                    {
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            if (lineNumber != 0)
                            {
                                var values = line.Split(';');

                                var sql = "INSERT INTO categories VALUES (" + values[0] + ",'" + values[1] + "')";

                                var cmd = new MySqlCommand();
                                cmd.CommandText = sql;
                                cmd.CommandType = System.Data.CommandType.Text;
                                cmd.Connection = conn;
                                cmd.ExecuteNonQuery();
                            }
                            lineNumber++;
                        }
                    }
                    MessageBox.Show("Данные успешно импортированы", "Оповещение", MessageBoxButtons.OK,
                              MessageBoxIcon.Information);
                    conn.Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка импорта", "Внимание", MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
            }

            load.LoadData("", "categories", dataGridView4, "SELECT * FROM categories");
        }

        private void button21_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog fd = new OpenFileDialog();

                fd.DefaultExt = "*.csv";
                fd.Filter = "CSV files (*.csv)|*.csv";
                fd.ShowDialog();
                string conStr = "";//импорт данных из CSV
                var lineNumber = 0;
                using (MySqlConnection conn = new MySqlConnection(str.ConString(conStr)))
                {
                    conn.Open();
                    string file = fd.FileName;

                    using (StreamReader reader = new StreamReader(file, Encoding.GetEncoding("windows-1251")))
                    {
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            if (lineNumber != 0)
                            {
                                var values = line.Split(';');

                                var sql = "INSERT INTO type_weaving VALUES (" + values[0] + ",'" + values[1] + "')";

                                var cmd = new MySqlCommand();
                                cmd.CommandText = sql;
                                cmd.CommandType = System.Data.CommandType.Text;
                                cmd.Connection = conn;
                                cmd.ExecuteNonQuery();
                            }
                            lineNumber++;
                        }
                    }
                    MessageBox.Show("Данные успешно импортированы", "Оповещение", MessageBoxButtons.OK,
                              MessageBoxIcon.Information);
                    conn.Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка импорта", "Внимание", MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
            }

            load.LoadData("", "type_weaving", dataGridView5, "SELECT * FROM type_weaving");
        }
    }
}
