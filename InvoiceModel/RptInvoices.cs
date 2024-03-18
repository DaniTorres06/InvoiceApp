namespace InvoiceModel
{
    public class RptInvoices
    {
        public int IdInvoice { get; set; }
        public string? DateInv { get; set; }
        public Int64 DocumentClient { get; set; }
        public string? NameClient { get; set; }
        public string? AddressClient { get; set; }
        public string? PhoneClient { get; set; }
        public string? DescItem { get; set; }
        public int ItemQuantity { get; set; } 
        public decimal Price { get; set; }
        public decimal GrossValue { get; set; }
        public decimal ValueDiscount { get; set; }
        public decimal IvaAmout { get; set; }
        public decimal TotalValue { get; set; }

        public RptInvoices()
        {
            IdInvoice = 0;
            DateInv = string.Empty;
            DocumentClient = 0;
            NameClient = string.Empty;
            AddressClient = string.Empty;
            PhoneClient = string.Empty;
            DescItem = string.Empty;
            ItemQuantity = 0;
            Price = 0;
            GrossValue = 0;
            ValueDiscount = 0;
            IvaAmout = 0;
            TotalValue = 0;

        }

    }
}
