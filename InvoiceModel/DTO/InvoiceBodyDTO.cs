using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceModel.DTO
{
    public class InvoiceBodyDTO
    {
        public int ItemId { get; set; }
        public int ItemQuantity { get; set; }
        public decimal Price { get; set; }
        

        public InvoiceBodyDTO()
        {
            ItemId = 0;
            ItemQuantity = 0;
            Price = 0;            
        }

    }
}
