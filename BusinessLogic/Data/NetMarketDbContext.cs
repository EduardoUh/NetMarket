using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BusinessLogic.Data
{
    // by adding this constructor and receiving the DbContextOptions the object can be initialized from
    // web api project and we avoid hard coding the connection string here
    public class NetMarketDbContext(DbContextOptions<NetMarketDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Product { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Brand> Brand { get; set; }

        // configuring ef to read the configurations and constraints we added to our entities(future tables)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // with this ef will read all classes that implements the IEntityTypeConfiguration interface
            // and so the configurations within it
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
