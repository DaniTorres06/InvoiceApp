using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceModel
{
    public class InvoiceBody
    {
        public int ItemId { get; set; }
		public int ItemQuantity { get; set; }
        public decimal Price { get; set; }
        public decimal GrossValue { get; set; }
        public decimal ValueDiscount { get; set; }
        public decimal IvaAmout { get; set; }
        public decimal TotalValue { get; set; }
        public int IdInvoice { get; set; }

        public InvoiceBody()
        {
            ItemId = 0;
            ItemQuantity = 0;
            Price = 0;
            GrossValue = 0;
            ValueDiscount = 0;
            IvaAmout = 0;
            TotalValue = 0;
            IdInvoice = 0;
        }
    }
}
