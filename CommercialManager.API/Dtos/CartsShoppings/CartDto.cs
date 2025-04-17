using CommercialManager.API.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CommercialManager.API.Dtos.CartsShoppings.Details;

namespace CommercialManager.API.Dtos.CartsShoppings
{
    public class CartDto
    {
        public Guid UserId { get; set; }
        public DateTime CreateDate { get; set; }
        public int TotalItems { get; set; }
        public decimal TotalAmount { get; set; }
        public List<CartDetailDto> Items { get; set; }
    }
}
