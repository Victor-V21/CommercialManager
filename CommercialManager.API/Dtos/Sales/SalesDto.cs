using CommercialManager.API.Dtos.Sales.Details;

namespace CommercialManager.API.Dtos.Sales
{
    public class SalesDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public double Total { get; set; }
        public List<SaleDetailDto> saleDetails { get; set; }
    }
}
