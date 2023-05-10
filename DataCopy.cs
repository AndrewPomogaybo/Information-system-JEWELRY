using MySql.Data.MySqlClient;
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

namespace jewellery
{
    public partial class DataCopy : Form
    {
        ConnectionString str = new ConnectionString();
        public DataCopy()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close(); //выход
        }

        public void Import()
        {
            string conStr = "";
            string file = "D:\\" + comboBox1.Text;
            using (MySqlConnection conn = new MySqlConnection(str.ConString(conStr)))
            {
                using (MySqlCommand cmd = new MySqlCommand()) //импорт резервной копии
                {
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        cmd.Connection = conn;
                        conn.Open();
                        mb.ImportFromFile(file);
                        conn.Close();
                    }
                }
            }
        }

        private void DataCopy_Load(object sender, EventArgs e)
        {
            /*DirectoryInfo dinfo = new DirectoryInfo(@"D:\");
            FileInfo[] files = dinfo.GetFiles();
            foreach (FileInfo filenames in files)
            {
                comboBox1.Items.Add(filenames); //запонение комбобоксов данными из папки
            }*/
            try
            {
                string[] files = Directory.GetFiles("D:\\", "*.sql");
                string file = String.Join("", files);
                string f = file.Substring(3);
                string[] arr = f.Split(' ');
                comboBox1.Items.AddRange(arr);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Нет сохраненных копий", "Оповещение", MessageBoxButtons.OK,
                               MessageBoxIcon.Information);
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Import();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
    }
}
