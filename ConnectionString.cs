using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jewellery
{
    public class ConnectionString
    {
        public string ConString(string conStr)
        {
            conStr = "host = localhost; database = jewellery; uid = root; pwd =12345;";
            return conStr;
        }
    }
}
