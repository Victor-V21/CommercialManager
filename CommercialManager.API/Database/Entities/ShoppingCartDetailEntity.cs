using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommercialManager.API.Database.Entities
{

    [Table("CartDetails")]
    public class ShoppingCartDetailEntity
    {
        [Key]
        [Required]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("cart_id")]
        public Guid ShoppingCartId { get; set; }
        [Required]
        [Column ("price")]
        public decimal Price { get; set; }
        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("subtotal")]
        public decimal Subtotal { get; set; }

        [Column("product_name")]
        public string ProductName { get; set; }

        [Column("product_id")]
        public Guid ProductId { get; set; }

        // Laves foraneas
        [ForeignKey(nameof(ProductId))]
        public virtual ProductEntity Product { get; set; }
        
        [ForeignKey(nameof(ShoppingCartId))]
        public virtual ShoppingCartEntity ShoppingCart { get; set; }

    }
}
