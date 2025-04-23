using CommercialManager.API.Dtos.CartsShoppings.Details;

namespace CommercialManager.API.Dtos.CartsShoppings
{
    public class CartCreateDto
    {
        public DateTime CreateDate { get; set; }
        public virtual List<CartDetailActionResponse> Items { get; set; }
    }
}
