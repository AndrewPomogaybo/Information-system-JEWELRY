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
using Word = Microsoft.Office.Interop.Word;

namespace jewellery
{
    public partial class Offers : Form
    {
        Connector con = new Connector();
        Load_Data load = new Load_Data();
        ConnectionString str = new ConnectionString();
        KeyPressNums onlynums = new KeyPressNums();
        DataGridView dvg = new DataGridView();
        public Offers()
        {
            InitializeComponent();
            LoadDataOffers("", dataGridView1);
            load.LoadData("","order_create", dataGridView2, "SELECT * FROM order_create");
            textBox10.Visible = false;
            label8.Visible = false;
            textBox6.Visible = false;
        }

       
        

        void LoadDataOffers(string conStr, DataGridView dvg)
        {
            string query = "SELECT id_order_buy, StatusName as 'Статус', ProductName as 'Наименование', ProductCategory as 'Категория', Count as 'Количество', Price as 'Цена' FROM order_buy JOIN Status ON order_buy.Status = Status.StatusId; ";     //JOIN Status ON order_buy.StatusId = Status.StatusId
            load.SQLConnection(conStr, dvg, query);
        }
       
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы действительно хотите удалить?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                string conStr = "";

                MySqlConnection connection = new MySqlConnection(str.ConString(conStr)); //строка подключения

                connection.Open();

                MySqlCommand command = new MySqlCommand("DELETE FROM order_buy WHERE id_order_buy = @pId;", connection); //удаление

                command.Parameters.Add(new MySqlParameter("@pId", this.dataGridView1.CurrentRow.Cells["id_order_buy"].Value));
                command.ExecuteNonQuery();
                DataTable table = new DataTable();
                dataGridView1.DataSource = table;
                command.Connection = new MySqlConnection(str.ConString(conStr));
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
                dataAdapter.Fill(table);
                dataGridView1.DataSource = table;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                connection.Close();
                LoadDataOffers("", dataGridView1);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы действительно хотите удалить?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                string conStr = "";

                MySqlConnection connection = new MySqlConnection(str.ConString(conStr)); //строка подключения

                connection.Open();

                MySqlCommand command = new MySqlCommand("DELETE FROM order_create WHERE id_order_create = @pId;", connection); //удаление

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
                LoadDataOffers("", dataGridView1);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string conStr = "";
            string query = "UPDATE order_buy SET Status = '" + comboBox2.Text + "', ProductName = '" + textBox2.Text + "', ProductCategory = '"+ comboBox1.Text + "', Count = " + textBox1.Text + ", Price = " + textBox5.Text + " WHERE id_order_buy = '" + textBox6.Text + "';";

            con.c(dvg, query, conStr); //редактирование данных
            LoadDataOffers("", dataGridView1);
            textBox1.Clear();
            textBox2.Clear();
            textBox5.Clear();
            textBox6.Clear();
            
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                textBox1.Text = row.Cells[4].Value.ToString();
                comboBox1.Text = row.Cells[3].Value.ToString();
                comboBox3.Text = row.Cells[1].Value.ToString();
                textBox2.Text = row.Cells[2].Value.ToString();
                textBox5.Text = row.Cells[5].Value.ToString();
                textBox6.Text = row.Cells[0].Value.ToString();
            }
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = '\0';
            onlynums.OnlyNums(ch, e);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = '\0';
            onlynums.OnlyNums(ch, e);
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = '\0';
            onlynums.OnlyNums(ch, e);
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = '\0';
            onlynums.OnlyNums(ch, e);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView2.SelectedRows)
            {
                textBox10.Text = row.Cells[0].Value.ToString();
                textBox8.Text = row.Cells[1].Value.ToString();
                textBox7.Text = row.Cells[2].Value.ToString();
                textBox3.Text = row.Cells[3].Value.ToString();
                textBox4.Text = row.Cells[4].Value.ToString();
                textBox9.Text = row.Cells[5].Value.ToString();
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this.dataGridView1.Columns[e.ColumnIndex].Name == "StatusId")
            {
                if(e.ColumnIndex == 2)
                {
                    e.CellStyle.BackColor = Color.Green;
                }
            }
        }

        

        private void button6_Click(object sender, EventArgs e)
        {
            Word.Application wdApp = new Word.Application();
            Word.Document wdDoc = null;
            Object wdMiss = System.Reflection.Missing.Value;

            wdDoc = wdApp.Documents.Add(ref wdMiss, ref wdMiss, ref wdMiss, ref wdMiss);

            // устанавливаем ориентацию (вид) документа
            wdDoc.PageSetup.Orientation = Word.WdOrientation.wdOrientPortrait;

            // устанавливаем размеры полей
            wdDoc.PageSetup.TopMargin = wdApp.InchesToPoints(0.60f);    //0.67 = 1.7 см
            wdDoc.PageSetup.BottomMargin = wdApp.InchesToPoints(0.60f);
            wdDoc.PageSetup.LeftMargin = wdApp.InchesToPoints(0.80f);
            wdDoc.PageSetup.RightMargin = wdApp.InchesToPoints(0.59f);  //0.59 = 1.5 см

            // выводим документ на экран
            wdApp.Visible = true;

            // устанваливаем интервал между строками
            wdApp.ActiveWindow.Selection.ParagraphFormat.LineSpacingRule = Word.WdLineSpacing.wdLineSpaceSingle;
            wdApp.ActiveWindow.Selection.ParagraphFormat.SpaceAfter = 0.0F;

            // вставляем новый параграф
            // имя параграфа
            Word.Paragraph oPara5b;
            oPara5b = wdDoc.Content.Paragraphs.Add(ref wdMiss);
            //текст в параграфе
            oPara5b.Range.Text = "Чек";
            //выравнивание в документе
            oPara5b.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            //размер шрифта
            oPara5b.Range.Font.Size = Convert.ToInt32(26);
            oPara5b.Range.InsertParagraphAfter();
            // закрываем параграф
            oPara5b.CloseUp();

            // и так можно вставлять параграфв неограниченное количество
           

            string count = textBox1.Text;
            string name = textBox2.Text;
            string price = textBox5.Text;
            string category = comboBox1.Text;
            string status = comboBox2.Text;

            // текст на новой странице
            Word.Paragraph Par1;
            Par1 = wdDoc.Content.Paragraphs.Add(ref wdMiss);
            Par1.Range.Text = "Количество: " + count;
            Par1.Alignment = Word.WdParagraphAlignment.wdAlignParagraphJustify;
            Par1.Range.Font.Size = Convert.ToInt32(14);
            Par1.Range.Font.Bold = 0;
            wdApp.ActiveWindow.Selection.ParagraphFormat.LineSpacingRule = Word.WdLineSpacing.wdLineSpaceSingle;
            wdApp.ActiveWindow.Selection.ParagraphFormat.SpaceAfter = 0.0F;
            Par1.Range.InsertParagraphAfter();
            Par1.CloseUp();

            Word.Paragraph Par2;
            Par2 = wdDoc.Content.Paragraphs.Add(ref wdMiss);
            Par2.Range.Text = "Наименование: " + name;
            Par2.Alignment = Word.WdParagraphAlignment.wdAlignParagraphJustify;
            Par2.Range.Font.Size = Convert.ToInt32(14);
            Par2.Range.Font.Bold = 0;
            wdApp.ActiveWindow.Selection.ParagraphFormat.LineSpacingRule = Word.WdLineSpacing.wdLineSpaceSingle;
            wdApp.ActiveWindow.Selection.ParagraphFormat.SpaceAfter = 0.0F;
            Par2.Range.InsertParagraphAfter();
            Par2.CloseUp();

            Word.Paragraph Par3;
            Par3 = wdDoc.Content.Paragraphs.Add(ref wdMiss);
            Par3.Range.Text = "Цена: " + price;
            Par3.Alignment = Word.WdParagraphAlignment.wdAlignParagraphJustify;
            Par3.Range.Font.Size = Convert.ToInt32(14);
            Par3.Range.Font.Bold = 0;
            wdApp.ActiveWindow.Selection.ParagraphFormat.LineSpacingRule = Word.WdLineSpacing.wdLineSpaceSingle;
            wdApp.ActiveWindow.Selection.ParagraphFormat.SpaceAfter = 0.0F;
            Par3.Range.InsertParagraphAfter();
            Par3.CloseUp();

            Word.Paragraph Par4;
            Par4 = wdDoc.Content.Paragraphs.Add(ref wdMiss);
            if(category == "1") 
            {
                Par4.Range.Text = "Категория: Кресты" ;
                Par4.Alignment = Word.WdParagraphAlignment.wdAlignParagraphJustify;
                Par4.Range.Font.Size = Convert.ToInt32(14);
                Par4.Range.Font.Bold = 0;
                wdApp.ActiveWindow.Selection.ParagraphFormat.LineSpacingRule = Word.WdLineSpacing.wdLineSpaceSingle;
                wdApp.ActiveWindow.Selection.ParagraphFormat.SpaceAfter = 0.0F;
                Par4.Range.InsertParagraphAfter();
                Par4.CloseUp();
            }
            else
            {
                Par4.Range.Text = "Категория: Цепи";
                Par4.Alignment = Word.WdParagraphAlignment.wdAlignParagraphJustify;
                Par4.Range.Font.Size = Convert.ToInt32(14);
                Par4.Range.Font.Bold = 0;
                wdApp.ActiveWindow.Selection.ParagraphFormat.LineSpacingRule = Word.WdLineSpacing.wdLineSpaceSingle;
                wdApp.ActiveWindow.Selection.ParagraphFormat.SpaceAfter = 0.0F;
                Par4.Range.InsertParagraphAfter();
                Par4.CloseUp();
            }
            
            
            Word.Paragraph Par5;
            Par5 = wdDoc.Content.Paragraphs.Add(ref wdMiss);
            if(status == "1")
            {
                Par5.Range.Text = "Статус: Не оплачен";
                Par5.Alignment = Word.WdParagraphAlignment.wdAlignParagraphJustify;
                Par5.Range.Font.Size = Convert.ToInt32(14);
                Par5.Range.Font.Bold = 0;
                wdApp.ActiveWindow.Selection.ParagraphFormat.LineSpacingRule = Word.WdLineSpacing.wdLineSpaceSingle;
                wdApp.ActiveWindow.Selection.ParagraphFormat.SpaceAfter = 0.0F;
                Par5.Range.InsertParagraphAfter();
                Par5.CloseUp();
            }
            else
            {
                Par5.Range.Text = "Статус: Оплачен";
                Par5.Alignment = Word.WdParagraphAlignment.wdAlignParagraphJustify;
                Par5.Range.Font.Size = Convert.ToInt32(14);
                Par5.Range.Font.Bold = 0;
                wdApp.ActiveWindow.Selection.ParagraphFormat.LineSpacingRule = Word.WdLineSpacing.wdLineSpaceSingle;
                wdApp.ActiveWindow.Selection.ParagraphFormat.SpaceAfter = 0.0F;
                Par5.Range.InsertParagraphAfter();
                Par5.CloseUp();
            }
            

            // Сохранение документа в файл
            try
            {
                // можно прописать полный путь сохранения к файлу
                // по-умолчанию, файл сохраняется в мои документы
                object filename = @"_example_file_ms-word" + ".doc";

                //сохраняем документ на диске
                wdDoc.SaveAs(ref filename);

                // Закрываем текущий документ
                
            }
            catch (Exception y)
            {
                Console.WriteLine("Ошибка сохранения документа", y.ToString());
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string conStr = "";
            string query = "SELECT id_order_buy, StatusName as 'Статус', ProductName as 'Наименование', ProductCategory as 'Категория', Count as 'Количество', Price as 'Цена' FROM order_buy JOIN Status ON order_buy.Status = Status.StatusId WHERE StatusName = 'Оплачен';";
            con.c(dataGridView1, query, conStr);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            LoadDataOffers("", dataGridView1);
            textBox1.Clear();
            textBox2.Clear();
            textBox5.Clear();
            textBox6.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            string conStr = "";
            string query = "SELECT id_order_buy, StatusName as 'Статус', ProductName as 'Наименование', ProductCategory as 'Категория', Count as 'Количество', Price as 'Цена' FROM order_buy JOIN Status ON order_buy.Status = Status.StatusId ORDER BY Status"; //сортировка клиентов по фамилии
            con.c(dataGridView1, query, conStr);
        }

        private void Offers_Load(object sender, EventArgs e)
        {
            string conStr = "";
            MySqlConnection con = new MySqlConnection(str.ConString(conStr));
            MySqlCommand cmd = new MySqlCommand("SELECT id_order_buy, StatusName as 'Статус', ProductName as 'Наименование', ProductCategory as 'Категория', Count as 'Количество', Price as 'Цена' FROM order_buy JOIN Status ON order_buy.Status = Status.StatusId; ", con);     //JOIN Status ON order_buy.StatusId = Status.StatusId
            con.Open();
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                comboBox3.Items.Add(id);
            }

        }
    }
}
