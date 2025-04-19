namespace CommercialManager.API.Dtos.Sales.Details
{
    public class SaleDetailDto
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
    }
}
