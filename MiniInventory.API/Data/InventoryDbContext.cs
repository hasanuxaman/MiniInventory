using Microsoft.EntityFrameworkCore;
using MiniInventory.API.Models;


namespace MiniInventory.API.Data
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        
        public DbSet<ProductStock> ProductStocks { get; set; } 
        public DbSet<StockTransaction> StockTransactions { get; set; }
    }
}
