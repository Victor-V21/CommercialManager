using CommercialManager.API.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CommercialManager.API.Dtos.Products
{
    public class ProductsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public int Stock { get; set; }
        public decimal? Discount { get; set; }
        public Guid CategoryId { get; set; }
    }

}
