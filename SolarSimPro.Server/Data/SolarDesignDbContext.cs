// Data/SolarDesignDbContext.cs
using Microsoft.EntityFrameworkCore;
using SolarSimPro.Server.Models;

namespace SolarSimPro.Server.Data
{
    public class SolarDesignDbContext : DbContext
    {
        public SolarDesignDbContext(DbContextOptions<SolarDesignDbContext> options)
            : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<SolarSystem> SolarSystems { get; set; }
        // Add other DbSet properties

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships here
        }
    }
}