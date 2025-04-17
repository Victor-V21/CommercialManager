namespace CommercialManager.API.Dtos.Sales.Details
{
    public class SaleDetailDto
    {
        public Guid Id { get; set; }

        public Guid SaleId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
    }
}
