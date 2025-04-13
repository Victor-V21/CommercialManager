using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommercialManager.API.Database.Entities
{

    [Table("Sales")]
    public class SalesEntity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("user_id")]
        public Guid UserId { get; set; }

        [Column("date")]
        public DateTime Date { get; set; }

        [Column("total")]
        public double Total { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual UserEntity User { get; set; }

        // Relaciones
        public IEnumerable<SalesDetailEntity> SalesDetail { get; set; }
    }
}
