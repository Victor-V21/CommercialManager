﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommercialManager.API.Database.Entities
{
    [Table("SalesDetails")]
    public class SalesDetailEntity
    {
        [Key]
        [Required]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("sale_id")]
        public Guid SalesId { get; set; }

        [Required]
        [Column("product_id")]
        public Guid ProductId { get; set; }

        [Required]
        [Column("quantity")]
        public int Quantity { get; set; }

        [Required]
        [Column("unit_price")]
        public double UnitPrice { get; set; }

        [ForeignKey(nameof(SalesId))]
        public virtual SalesEntity Sales { get; set; }

        [ForeignKey(nameof(ProductId))]
        public virtual ProductEntity Product { get; set; }
    }
}
