using Microsoft.EntityFrameworkCore;
using TradeNetAPI.Models;

namespace TradeNetAPI.Data
{
    public class TradeNetDbContext : DbContext
    {
        public TradeNetDbContext(DbContextOptions<TradeNetDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<BusinessDocument> BusinessDocuments { get; set; }
        public DbSet<TradeLicense> TradeLicenses { get; set; }
        public DbSet<LicenseDocument> LicenseDocuments { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TradeProgram> TradePrograms { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ComplianceRecord> ComplianceRecords { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<MarketRecord> MarketRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure primary keys and identity columns
            modelBuilder.Entity<User>().HasKey(u => u.UserID);
            modelBuilder.Entity<User>().Property(u => u.UserID).ValueGeneratedOnAdd();

            modelBuilder.Entity<Business>().HasKey(b => b.BusinessID);
            modelBuilder.Entity<Business>().Property(b => b.BusinessID).ValueGeneratedOnAdd();

            modelBuilder.Entity<BusinessDocument>().HasKey(bd => bd.DocumentID);
            modelBuilder.Entity<BusinessDocument>().Property(bd => bd.DocumentID).ValueGeneratedOnAdd();

            modelBuilder.Entity<TradeLicense>().HasKey(tl => tl.LicenseID);
            modelBuilder.Entity<TradeLicense>().Property(tl => tl.LicenseID).ValueGeneratedOnAdd();

            modelBuilder.Entity<LicenseDocument>().HasKey(ld => ld.DocumentID);
            modelBuilder.Entity<LicenseDocument>().Property(ld => ld.DocumentID).ValueGeneratedOnAdd();

            modelBuilder.Entity<Transaction>().HasKey(t => t.TransactionID);
            modelBuilder.Entity<Transaction>().Property(t => t.TransactionID).ValueGeneratedOnAdd();

            modelBuilder.Entity<TradeProgram>().HasKey(tp => tp.ProgramID);
            modelBuilder.Entity<TradeProgram>().Property(tp => tp.ProgramID).ValueGeneratedOnAdd();

            modelBuilder.Entity<Notification>().HasKey(n => n.NotificationID);
            modelBuilder.Entity<Notification>().Property(n => n.NotificationID).ValueGeneratedOnAdd();

            modelBuilder.Entity<ComplianceRecord>().HasKey(cr => cr.ComplianceID);
            modelBuilder.Entity<ComplianceRecord>().Property(cr => cr.ComplianceID).ValueGeneratedOnAdd();

            modelBuilder.Entity<AuditLog>().HasKey(al => al.AuditID);
            modelBuilder.Entity<AuditLog>().Property(al => al.AuditID).ValueGeneratedOnAdd();

            modelBuilder.Entity<MarketRecord>().HasKey(mr => mr.RecordID);
            modelBuilder.Entity<MarketRecord>().Property(mr => mr.RecordID).ValueGeneratedOnAdd();

            // Seed initial data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Note: Do NOT seed Users or Businesses here - they have auto-increment IDs
            // Seed only reference data with explicit IDs

            // Seed Trade Licenses (these are reference data, only seeded once)
            modelBuilder.Entity<TradeLicense>().HasData(
                new TradeLicense 
                { 
                    LicenseID = 1, 
                    BusinessID = 0, 
                    Type = "Import", 
                    Title = "General Import License", 
                    Description = "Allows import of goods from international markets", 
                    Status = "Available", 
                    Fee = 5000 
                },
                new TradeLicense 
                { 
                    LicenseID = 2, 
                    BusinessID = 0, 
                    Type = "Export", 
                    Title = "General Export License", 
                    Description = "Allows export of goods to international markets", 
                    Status = "Available", 
                    Fee = 4500 
                },
                new TradeLicense 
                { 
                    LicenseID = 3, 
                    BusinessID = 0, 
                    Type = "Local", 
                    Title = "Local Trade License", 
                    Description = "Allows domestic trading activities", 
                    Status = "Available", 
                    Fee = 2000 
                }
            );

            // Seed Trade Programs (reference data only)
            modelBuilder.Entity<TradeProgram>().HasData(
                new TradeProgram 
                { 
                    ProgramID = 1, 
                    Title = "Export Promotion Scheme", 
                    Description = "Subsidies for exporters to promote international trade", 
                    StartDate = DateTime.Now, 
                    EndDate = DateTime.Now.AddYears(1), 
                    Budget = 1000000, 
                    Status = "Active", 
                    ProgramType = "ExportPromotion", 
                    EligibilityCriteria = "Must have active export license" 
                },
                new TradeProgram 
                { 
                    ProgramID = 2, 
                    Title = "SME Trade Support", 
                    Description = "Financial support for small and medium enterprises", 
                    StartDate = DateTime.Now, 
                    EndDate = DateTime.Now.AddYears(2), 
                    Budget = 500000, 
                    Status = "Active", 
                    ProgramType = "Subsidy", 
                    EligibilityCriteria = "SME with annual turnover below $1M" 
                }
            );
        }
    }
}
