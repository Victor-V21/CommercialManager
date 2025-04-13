using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommercialManager.API.Database.Entities
{

    [Table("Categories")]
    public class CategoryEntity
    {
        [Key]
        [Required]
        [Column("id")]
        public Guid Id { get; set; }
        [Required]
        [Column("name")]
        public string Name { get; set; }
        [Required]
        [Column("description")]
        public string Description { get; set; }

        //Relaciones que nacen de esta tabla
        public virtual IEnumerable<ProductEntity> Products { get; set; }
    }
}
