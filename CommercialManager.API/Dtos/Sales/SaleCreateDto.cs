using CommercialManager.API.Dtos.Sales.Details;

namespace CommercialManager.API.Dtos.Sales
{
    public class SaleCreateDto
    {
        public Guid UserId { get; set; }
        public List<SaleDetailCreateDto> saleDetails{ get; set; }
    }
}
