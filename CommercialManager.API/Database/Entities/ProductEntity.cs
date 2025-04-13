using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CommercialManager.API.Database.Entities
{
    [Table("Products")]
    public class ProductEntity
    {
        [Key]
        [Required]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }
        [Column("price")]
        public decimal? Price { get; set; }

        [Column("stock")]
        public int Stock { get; set; }
        [Column("discount")]
        public decimal? Discount { get; set; }

        [Column("category_id")]
        public Guid CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public virtual CategoryEntity category { get; set; }

        //Relaciones que salen de esta tabla
        public virtual IEnumerable<ShoppingCartDetailEntity> ShoppingCartDetail { get; set; }
        public virtual IEnumerable<SalesDetailEntity> SalesDetail { get; set; }

    }
}
