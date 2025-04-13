using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace CommercialManager.API.Database.Entities
{
    [Table("Users")]
    public class UserEntity
    {
        [Key]
        [Required]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("dni")]
        public int DNI { get; set; }

        [Required]
        [Column("firstname")]
        public string FirstName { get; set; }

        [Required]
        [Column("lastname")]
        public string LastName { get; set; }

        [Required]
        [Column("email")]
        public string Email { get; set; }

        [Required]
        [Column("age")]
        public int Age { get; set; }

        // Relaciones
        public virtual ShoppingCartEntity ShoppingCart { get; set; } // De uno a uno
        public virtual IEnumerable<SalesEntity> Sales { get; set; } // De uno a muchos
    }
}
