using Goverment.Models;
using Microsoft.EntityFrameworkCore;

namespace Goverment.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<TradeLicense> TradeLicenses { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TradeProgram> TradePrograms { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<ComplianceRecord> ComplianceRecords { get; set; }
        public DbSet<Audit> Audits { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Subsidy> Subsidies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User entity configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserID);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Role).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Phone).HasMaxLength(50);
                entity.Property(e => e.Status).HasMaxLength(50);
                entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(500);
                entity.Property(e => e.PasswordSalt).IsRequired().HasMaxLength(500);
                entity.Property(e => e.BusinessName).HasMaxLength(300);
            });

            // Business entity configuration
            modelBuilder.Entity<Business>(entity =>
            {
                entity.HasKey(e => e.BusinessID);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(300);
                entity.Property(e => e.Type).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Address).HasMaxLength(500);
                entity.Property(e => e.ContactInfo).HasMaxLength(200);
                entity.Property(e => e.Status).HasMaxLength(50);
            });

            // TradeLicense entity configuration
            modelBuilder.Entity<TradeLicense>(entity =>
            {
                entity.HasKey(e => e.LicenseID);
                entity.Property(e => e.Type).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Status).HasMaxLength(50);

                entity.HasOne(e => e.Business)
                    .WithMany()
                    .HasForeignKey(e => e.BusinessID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Transaction entity configuration
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.TransactionID);
                entity.Property(e => e.Type).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Amount).HasPrecision(18, 2);
                entity.Property(e => e.Status).HasMaxLength(50);

                entity.HasOne(e => e.Business)
                    .WithMany()
                    .HasForeignKey(e => e.BusinessID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // TradeProgram entity configuration
            modelBuilder.Entity<TradeProgram>(entity =>
            {
                entity.HasKey(e => e.ProgramID);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(300);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Budget).HasPrecision(18, 2);
                entity.Property(e => e.Status).HasMaxLength(50);

                entity.HasMany(e => e.Resources)
                    .WithOne(r => r.TradeProgram)
                    .HasForeignKey(r => r.ProgramID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Resource entity configuration
            modelBuilder.Entity<Resource>(entity =>
            {
                entity.HasKey(e => e.ResourceID);
                entity.Property(e => e.Type).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Status).HasMaxLength(50);
            });

            // ComplianceRecord entity configuration
            modelBuilder.Entity<ComplianceRecord>(entity =>
            {
                entity.HasKey(e => e.ComplianceID);
                entity.Property(e => e.Type).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Result).HasMaxLength(100);
                entity.Property(e => e.Notes).HasMaxLength(1000);
            });

            // Audit entity configuration
            modelBuilder.Entity<Audit>(entity =>
            {
                entity.HasKey(e => e.AuditID);
                entity.Property(e => e.Scope).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Findings).HasMaxLength(2000);
                entity.Property(e => e.Status).HasMaxLength(50);

                entity.HasOne(e => e.Officer)
                    .WithMany()
                    .HasForeignKey(e => e.OfficerID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Report entity configuration
            modelBuilder.Entity<Report>(entity =>
            {
                entity.HasKey(e => e.ReportID);
                entity.Property(e => e.Scope).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Metrics).HasMaxLength(2000);
            });

            // Notification entity configuration
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.NotificationID);
                entity.Property(e => e.Message).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.Category).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Status).HasMaxLength(50);

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Subsidy entity configuration
            modelBuilder.Entity<Subsidy>(entity =>
            {
                entity.HasKey(e => e.SubsidyID);
                entity.Property(e => e.Type).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Amount).HasPrecision(18, 2);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
                entity.Property(e => e.RejectionReason).HasMaxLength(500);
                entity.Property(e => e.Notes).HasMaxLength(2000);

                entity.HasOne(e => e.Business)
                    .WithMany()
                    .HasForeignKey(e => e.BusinessID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.TradeProgram)
                    .WithMany()
                    .HasForeignKey(e => e.ProgramID)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
