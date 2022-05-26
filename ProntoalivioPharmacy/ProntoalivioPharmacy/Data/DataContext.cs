using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProntoalivioPharmacy.Data.Entities;

namespace ProntoalivioPharmacy.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<City> Cities { get; set; }
        public DbSet<Laboratory> Laboratories { get; set; }
        public DbSet<MedicineType> MedicineTypes { get; set; }
        public DbSet<Neighborhood> Neighborhoods { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<City>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<MedicineType>().HasIndex(m => m.Name).IsUnique();
            modelBuilder.Entity<Neighborhood>().HasIndex("Name", "CityId").IsUnique();
            modelBuilder.Entity<Laboratory>().HasIndex("Name", "NeighborhoodId").IsUnique();
            modelBuilder.Entity<Product>().HasIndex(p => p.Name).IsUnique();
            modelBuilder.Entity<ProductCategory>().HasIndex("ProductId", "MedicineTypeId").IsUnique();
        }

    }
}
