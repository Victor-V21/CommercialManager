namespace CommercialManager.API.Dtos.Sales.Invoice
{
    public class InvoiceItemDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
    }
}
