using System.ComponentModel.DataAnnotations.Schema;

namespace CommercialManager.API.Dtos.Sales.Details
{
    public class SaleDetailDto
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal? Discount { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
