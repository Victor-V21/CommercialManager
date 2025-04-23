namespace CommercialManager.API.Dtos.Sales.Invoice
{
    public class InvoiceDto
    {
        public Guid SaleId { get; set; }
        public DateTime Date { get; set; }
        public string ClientName { get; set; } 
        public string ClientDNI { get; set; }
        public decimal TotalAmount { get; set; }
        public List<InvoiceItemDto> Items { get; set; } = new();

    }
}
