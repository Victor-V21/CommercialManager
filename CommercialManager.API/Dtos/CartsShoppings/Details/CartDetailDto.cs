using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CommercialManager.API.Dtos.CartsShoppings.Details
{
    public class CartDetailDto
    {
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }
        public string ProductName { get; set; }
        public Guid ProductId { get; set; }
    }
}
