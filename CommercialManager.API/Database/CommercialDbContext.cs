using CommercialManager.API.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace CommercialManager.API.Database
{
    public class CommercialDbContext : DbContext
    {
        public CommercialDbContext(DbContextOptions options) : base(options)
        {
            
        }

        // Tablas de la base de datos

        DbSet<UserEntity> Users { get; set; }
        DbSet<ShoppingCartEntity> ShoppingCarts { get; set; }
        DbSet<ShoppingCartDetailEntity> ShoppingCartDetails { get; set; }
        DbSet<ProductEntity> Products { get; set; }
        DbSet<SalesEntity> Sales { get; set; }
        DbSet<SalesDetailEntity> SalesDetails { get; set; }
        DbSet<CategoryEntity> Categories { get; set; }
    }
}
