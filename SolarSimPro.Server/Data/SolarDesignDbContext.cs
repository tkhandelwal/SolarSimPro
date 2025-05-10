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
        public DbSet<PanelModel> PanelModels { get; set; }
        public DbSet<InverterModel> InverterModels { get; set; }
        public DbSet<SimulationResult> SimulationResults { get; set; }
        public DbSet<MonthlyResult> MonthlyResults { get; set; }
        public DbSet<LossBreakdown> LossBreakdowns { get; set; }
        public DbSet<FinancialAnalysis> FinancialAnalyses { get; set; }
        public DbSet<Inverter> Inverters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships
            modelBuilder.Entity<SolarSystem>()
                .HasOne(s => s.Project)
                .WithMany(p => p.Systems)
                .HasForeignKey(s => s.ProjectId);

            modelBuilder.Entity<SolarSystem>()
                .HasOne(s => s.PanelModel)
                .WithMany()
                .HasForeignKey(s => s.PanelModelId)
                .IsRequired(false);

            modelBuilder.Entity<SolarSystem>()
                .HasOne(s => s.InverterModel)
                .WithMany()
                .HasForeignKey(s => s.InverterModelId)
                .IsRequired(false);

            modelBuilder.Entity<SimulationResult>()
                .HasOne(s => s.SolarSystem)
                .WithMany(s => s.SimulationResults)
                .HasForeignKey(s => s.SolarSystemId);

            modelBuilder.Entity<MonthlyResult>()
                .HasOne(m => m.SimulationResult)
                .WithMany(s => s.MonthlyResults)
                .HasForeignKey(m => m.SimulationResultId);

            modelBuilder.Entity<LossBreakdown>()
                .HasOne(l => l.SimulationResult)
                .WithOne(s => s.Losses)
                .HasForeignKey<LossBreakdown>(l => l.SimulationResultId);

            modelBuilder.Entity<FinancialAnalysis>()
                .HasOne(f => f.SimulationResult)
                .WithOne(s => s.FinancialAnalysis)
                .HasForeignKey<FinancialAnalysis>(f => f.SimulationResultId);

            modelBuilder.Entity<Inverter>()
                .HasOne(i => i.SolarSystem)
                .WithMany(s => s.Inverters)
                .HasForeignKey(i => i.SolarSystemId);

            modelBuilder.Entity<Inverter>()
                .HasOne(i => i.InverterModel)
                .WithMany()
                .HasForeignKey(i => i.InverterModelId);
        }
    }
}