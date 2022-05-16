using Microsoft.EntityFrameworkCore;
using ProntoalivioPharmacy.Data.Entities;

namespace ProntoalivioPharmacy.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<City> Cities { get; set; }
        public DbSet<Laboratory> Laboratories { get; set; }
        public DbSet<MedicineType> MedicineTypes { get; set; }
        public DbSet<Neighborhood> Neighborhoods { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<City>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<MedicineType>().HasIndex(m => m.Name).IsUnique();
            modelBuilder.Entity<Neighborhood>().HasIndex("Name", "CityId").IsUnique();
            modelBuilder.Entity<Laboratory>().HasIndex("Name", "NeighborhoodId").IsUnique();
        }

    }
}
