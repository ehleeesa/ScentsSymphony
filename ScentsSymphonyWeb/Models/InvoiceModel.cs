namespace ScentsSymphonyWeb.Models
{
    public class InvoiceModel
    {
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }

        public List<InvoiceItem> Items { get; set; } = new();
        public decimal TotalAmount => Items.Sum(item => item.TotalPrice);
    }

    public class InvoiceItem
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => Quantity * UnitPrice;
    }
}