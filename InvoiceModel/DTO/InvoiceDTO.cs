using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceModel.DTO
{
    public class InvoiceDTO
    {
        public DateTime DateInv { get; set; }
        public Int64 DocumentClient { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? AddressClient { get; set; }
        public string? PhoneClient { get; set; }
        public List<InvoiceBodyDTO> InvoiceBodys { get; set; }

        public InvoiceDTO()
        {
            DateInv = DateTime.Now;
            DocumentClient = 0;
            Name = string.Empty;
            LastName = string.Empty;
            AddressClient = string.Empty;
            PhoneClient = string.Empty;
            InvoiceBodys = new List<InvoiceBodyDTO>();
        }
    }
}
