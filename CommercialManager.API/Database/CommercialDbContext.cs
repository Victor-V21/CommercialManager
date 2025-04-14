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

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ShoppingCartEntity> ShoppingCarts { get; set; }
        public DbSet<ShoppingCartDetailEntity> ShoppingCartDetails { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<SalesEntity> Sales { get; set; }
        public DbSet<SalesDetailEntity> SalesDetails { get; set; }
        DbSet<CategoryEntity> Categories { get; set; }
    }
}
