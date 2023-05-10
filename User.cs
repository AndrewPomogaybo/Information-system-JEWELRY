using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace jewellery
{
    public partial class User : Form
    {
        BackupData backup = new BackupData();
        public User()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Offers offers = new Offers(); //открытие формы заказов
            offers.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Products products = new Products();  //открытие формы товаров
            products.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Add_offer AO = new Add_offer();  //открытие формы добавления заказов
            AO.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            backup.Backup(); //выход/ создание резервной копии
            this.Close();
        }
    }
}
