using CommercialManager.API.Dtos.Sales.Details;

namespace CommercialManager.API.Dtos.Sales
{
    public class SaleActionResponseDto
    {
        public Guid Id { get; set; }          
        public Guid UserId { get; set; }     
        public DateTime Date { get; set; }    
        public double Total { get; set; }     
        public bool IsSuccess { get; set; }   
        public string Message { get; set; }

        public List<SaleDetailDto> Items { get; set; } = new();
    }
}
