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
    public partial class Admin : Form
    {
        ConnectionString str = new ConnectionString();
        BackupData backup = new BackupData();
        public Admin()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Offers Offers = new Offers(); //переход на форму просмотр заказов
            Offers.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Products Products = new Products(); //переход на форму просмотр товаров
            Products.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DB DB = new DB(); //переход на форму просмотр БД
            DB.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Add_offer Add_offer = new Add_offer();
            Add_offer.ShowDialog(); //кнопка перехода на форму добавить заказ
        }

       

        private void button3_Click(object sender, EventArgs e)
        {
            backup.Backup();
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            DataCopy dc = new DataCopy(); //кнопка резервного копирования
            dc.ShowDialog();
        }
    }
}
