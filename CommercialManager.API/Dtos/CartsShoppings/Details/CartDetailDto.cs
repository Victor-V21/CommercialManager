using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CommercialManager.API.Dtos.CartsShoppings.Details
{
    public class CartDetailDto
    {
        public int Quantity { get; set; }
        public Guid ProductId { get; set; }
    }
}
