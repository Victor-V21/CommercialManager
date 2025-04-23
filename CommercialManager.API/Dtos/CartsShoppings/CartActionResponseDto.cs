using CommercialManager.API.Dtos.CartsShoppings.Details;

namespace CommercialManager.API.Dtos.CartsShoppings
{
    public class CartActionResponseDto
    {
        public Guid UserId { get; set; }
        public DateTime CreateDate { get; set; }
        public int TotalItems { get; set; }
        public decimal TotalAmount { get; set; }
        public List<CartDetailActionResponse> Items { get; set; }
    }
}
