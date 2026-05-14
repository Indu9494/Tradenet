using Microsoft.EntityFrameworkCore;
using TradeNetProject.Models;

namespace TradeNetProject.Data
{
    /// <summary>
    /// Unified DbContext for all TradeNet APIs
    /// Includes all entities from all team members' APIs
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Core entities (unified from all APIs)
        public DbSet<User> Users { get; set; }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<TradeLicense> TradeLicenses { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TradeProgram> TradePrograms { get; set; }
        public DbSet<ComplianceRecord> ComplianceRecords { get; set; }
        public DbSet<BusinessDocument> BusinessDocuments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        // Legacy entities (keeping for backward compatibility)
        public DbSet<TradeOfficer> TradeOfficers { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<MarketRecord> MarketRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Foreign key relationships
            modelBuilder.Entity<Business>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TradeLicense>()
                .HasOne(l => l.Business)
                .WithMany()
                .HasForeignKey(l => l.BusinessID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Business)
                .WithMany()
                .HasForeignKey(t => t.BusinessID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BusinessDocument>()
                .HasOne(d => d.Business)
                .WithMany()
                .HasForeignKey(d => d.BusinessID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AuditLog>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ComplianceRecord>()
                .HasOne(c => c.Business)
                .WithMany()
                .HasForeignKey(c => c.EntityID)
                .OnDelete(DeleteBehavior.SetNull);

            // Seed data - Users
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserID = 1,
                    Name = "Admin User",
                    Role = "Admin",
                    Email = "admin@tradenet.gov",
                    Phone = "+1 (800) 555-0100",
                    Status = "Active",
                    CreatedDate = new DateTime(2026, 1, 1)
                },
                new User
                {
                    UserID = 2,
                    Name = "Trade Officer",
                    Role = "Officer",
                    Email = "officer@tradenet.gov",
                    Phone = "+1 (800) 555-0142",
                    Status = "Active",
                    CreatedDate = new DateTime(2026, 1, 1)
                },
                new User
                {
                    UserID = 3,
                    Name = "Program Manager",
                    Role = "Manager",
                    Email = "manager@tradenet.gov",
                    Phone = "+1 (800) 555-0143",
                    Status = "Active",
                    CreatedDate = new DateTime(2026, 1, 1)
                },
                new User
                {
                    UserID = 4,
                    Name = "Test Trader",
                    Role = "Business",
                    Email = "trader@business.com",
                    Phone = "+1 (800) 555-0144",
                    Status = "Active",
                    BusinessName = "ABC Traders",
                    CreatedDate = new DateTime(2026, 1, 1)
                }
            );

            // Seed data - Businesses
            modelBuilder.Entity<Business>().HasData(
                new Business
                {
                    BusinessID = 1,
                    UserID = 4,
                    Name = "ABC Traders",
                    Type = "Trader",
                    Address = "123 Trade Street, Commerce City",
                    ContactInfo = "contact@abctraders.com",
                    Status = "Active",
                    RegistrationNumber = "REG-2024-001",
                    RegistrationDate = new DateTime(2024, 1, 15),
                    ComplianceStatus = "Compliant"
                }
            );

            // Seed data - Trade Licenses
            modelBuilder.Entity<TradeLicense>().HasData(
                new TradeLicense { LicenseID = 1, BusinessID = 1, Type = "Import", Title = "Import License", Description = "Import License for Goods", Fee = 5000, Status = "Available", BusinessName = "ABC Traders" },
                new TradeLicense { LicenseID = 2, BusinessID = 1, Type = "Export", Title = "Export License", Description = "Export License for Goods", Fee = 5500, Status = "Available", BusinessName = "ABC Traders" },
                new TradeLicense { LicenseID = 3, BusinessID = 1, Type = "Local", Title = "Local Trade License", Description = "Local Trade License", Fee = 2000, Status = "Available", BusinessName = "ABC Traders" }
            );

            // Seed data - Trade Officer (legacy)
            modelBuilder.Entity<TradeOfficer>().HasData(
                new TradeOfficer
                {
                    OfficerID = 1001,
                    FullName = "Senior Trade Officer",
                    Email = "officer@tradenet.gov",
                    Phone = "+1 (800) 555-0142",
                    Department = "Trade Licensing & Compliance",
                    Designation = "Senior Trade Officer",
                    EmployeeCode = "TO-2024-1001",
                    Region = "North America – Eastern Division",
                    DateOfJoining = new DateTime(2021, 3, 15),
                    Status = "Active"
                }
            );
        }
    }
}
