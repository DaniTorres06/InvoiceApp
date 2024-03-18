using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceModel
{
    public class Items
    {
        public int IdItem { get; set; }
        public string? DescItem { get; set; }

        public Items() 
        { 
            IdItem = 0;
            DescItem = string.Empty;
        }
    }
}
