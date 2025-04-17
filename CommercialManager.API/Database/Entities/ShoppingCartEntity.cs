using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommercialManager.API.Database.Entities
{
    [Table("ShoppingCarts")]
    public class ShoppingCartEntity
    {
        //[Key]
        //[Required]
        //[Column("id")]
        //public Guid Id { get; set; }

        [Key]
        [Required]
        [Column("user_id")]
        public Guid UserId { get; set;}
        [Column("create_date")]
        public DateTime CreateDate { get; set; }

        [Column("total_items")]
        public int TotalItems { get; set; }

        [Column ("total_amount")]
        public decimal TotalAmount { get; set; }

        // Llaves Foraneas
        [Required]
        [ForeignKey(nameof(UserId))]
        public virtual UserEntity User { get; set; }

        // Relaciones
        public virtual IEnumerable<ShoppingCartDetailEntity> Details { get; set; }
    }
}
