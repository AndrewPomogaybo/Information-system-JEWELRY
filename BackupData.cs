using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jewellery
{
    public class BackupData
    {

        ConnectionString str = new ConnectionString();

        public void Backup()
        {
            String date = DateTime.Now.ToString("yyyy.MM.dd");
            string conStr = "";
            string file = Directory.GetCurrentDirectory() + $"backup-{date}.sql";


            using (MySqlConnection con = new MySqlConnection(str.ConString(conStr)))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    using (MySqlBackup backup = new MySqlBackup(cmd))
                    {
                        cmd.Connection = con;
                        con.Open();
                        backup.ExportToFile(file);
                        con.Close();
                    }
                }
            }
        }
    }
}
