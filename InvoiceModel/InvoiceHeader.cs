using InvoiceModel.DTO;

namespace InvoiceModel
{
    public class InvoiceHeader
    {
        public DateTime DateInv { get; set; }
		public Int64 DocumentClient { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? AddressClient { get; set; }
        public string? PhoneClient { get; set; }
        public List<InvoiceBody> InvoiceBodys { get; set; }

        public InvoiceHeader()
        {
            DateInv = DateTime.MinValue;
            DocumentClient = 0;
            Name = string.Empty;
            LastName = string.Empty;
            AddressClient = string.Empty;
            PhoneClient = string.Empty;
            InvoiceBodys = new List<InvoiceBody>();
        }
    }
}
