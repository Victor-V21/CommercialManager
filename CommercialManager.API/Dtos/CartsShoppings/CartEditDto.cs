using CommercialManager.API.Dtos.CartsShoppings.Details;

namespace CommercialManager.API.Dtos.CartsShoppings
{
    public class CartEditDto
    {
        public DateTime CreateDate { get; set; }
        public List<CartDetailDto> Items { get; set; }
    }
}
