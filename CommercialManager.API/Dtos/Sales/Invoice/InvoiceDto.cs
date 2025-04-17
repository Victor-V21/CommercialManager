namespace CommercialManager.API.Dtos.Sales.Invoice
{
    public class InvoiceDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public double Total { get; set; }
        public List<InvoiceItemDto> Items { get; set; }
    }
}
