using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace jewellery
{
    public class OnlySymbols
    {
        public void GetOnlySymbols(char ch, KeyPressEventArgs e)
        {
            ch = e.KeyChar;

            if ((ch < 'A' || ch > 'я') && ch != 8)
            {
                e.Handled = true;
            }
        }

    }
}
