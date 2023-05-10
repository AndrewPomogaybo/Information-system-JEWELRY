using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace jewellery
{
    public class ImportCSV
    {
        ConnectionString str = new ConnectionString();
        
        public void FromCSV(string conStr)
        {
            OpenFileDialog fd = new OpenFileDialog();

            fd.DefaultExt = "*.csv";
            fd.Filter = "CSV files (*.csv)|*.csv";
            fd.ShowDialog();
            string file = fd.FileName;
            var lineNumber = 0;
            using (MySqlConnection conn = new MySqlConnection(str.ConString(conStr)))
            {
                conn.Open();

                //Put your file location here:
                using (StreamReader reader = new StreamReader(file, Encoding.GetEncoding("windows-1251")))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
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
                conn.Close();
            }
        }
    }
}
