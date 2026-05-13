using Microsoft.EntityFrameworkCore;
using TradeNet11.Models;

namespace TradeNet11.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ComplianceOfficer> ComplianceOfficers => Set<ComplianceOfficer>();
        public DbSet<ComplianceNotification> ComplianceNotifications => Set<ComplianceNotification>();
        public DbSet<ComplianceCase> ComplianceCases => Set<ComplianceCase>();
        public DbSet<Audit> Audits => Set<Audit>();
        public DbSet<ProgramCompliance> ProgramCompliances => Set<ProgramCompliance>();
        public DbSet<ComplianceRecord> ComplianceRecords => Set<ComplianceRecord>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            // Seed officer
            modelBuilder.Entity<ComplianceOfficer>().HasData(new ComplianceOfficer
            {
                Id = 1,
                Username = "officer1",
                PasswordHash = "AQAAAAIAAYagAAAAEA==",
                FullName = "Default Compliance Officer",
                Email = "officer1@tradenet.com",
                CreatedAt = seedDate,
                IsActive = true
            });

            // Seed compliance cases
            modelBuilder.Entity<ComplianceCase>().HasData(
                new ComplianceCase { Id = 1, BusinessName = "Alpha Trading Co.", LicenseNumber = "LIC-2025-001", IssueType = "Violation", Description = "Operating without updated trade license since Jan 2025.", Severity = "High", Status = "Pending", ReportedAt = seedDate, AssignedOfficerId = 1 },
                new ComplianceCase { Id = 2, BusinessName = "Beta Imports Ltd.", LicenseNumber = "LIC-2025-042", IssueType = "DocumentFraud", Description = "Certificate of origin appears forged for shipment #8821.", Severity = "Critical", Status = "Investigating", ReportedAt = seedDate.AddDays(5), AssignedOfficerId = 1 },
                new ComplianceCase { Id = 3, BusinessName = "Gamma Exports", LicenseNumber = "LIC-2024-318", IssueType = "Expired", Description = "Trade license expired 3 months ago, still conducting business.", Severity = "Medium", Status = "Pending", ReportedAt = seedDate.AddDays(10), AssignedOfficerId = 1 },
                new ComplianceCase { Id = 4, BusinessName = "Delta Wholesale", LicenseNumber = "LIC-2025-099", IssueType = "SuspiciousActivity", Description = "Unusual volume of transactions flagged by monitoring system.", Severity = "High", Status = "Pending", ReportedAt = seedDate.AddDays(12), AssignedOfficerId = 1 },
                new ComplianceCase { Id = 5, BusinessName = "Epsilon Services", LicenseNumber = "LIC-2025-205", IssueType = "Mismatch", Description = "Declared goods do not match inspection findings.", Severity = "Low", Status = "Compliant", ReportedAt = seedDate.AddDays(2), ResolvedAt = seedDate.AddDays(8), AssignedOfficerId = 1 }
            );

            // Seed notifications
            modelBuilder.Entity<ComplianceNotification>().HasData(
                new ComplianceNotification { Id = 1, Title = "License Violation Detected", Description = "Alpha Trading Co. operating without updated license.", AlertType = "Violation", Severity = "High", IsRead = false, CreatedAt = seedDate, ComplianceCaseId = 1, AssignedOfficerId = 1 },
                new ComplianceNotification { Id = 2, Title = "Document Fraud Alert", Description = "Forged certificate of origin found for Beta Imports shipment.", AlertType = "DocumentIssue", Severity = "Critical", IsRead = false, CreatedAt = seedDate.AddDays(5), ComplianceCaseId = 2, AssignedOfficerId = 1 },
                new ComplianceNotification { Id = 3, Title = "Suspicious Transaction Flagged", Description = "Delta Wholesale shows unusual transaction volume.", AlertType = "SuspiciousTransaction", Severity = "High", IsRead = false, CreatedAt = seedDate.AddDays(12), ComplianceCaseId = 4, AssignedOfficerId = 1 },
                new ComplianceNotification { Id = 4, Title = "Audit Due Today", Description = "Scheduled audit for Gamma Exports is due.", AlertType = "AuditDue", Severity = "Medium", IsRead = true, CreatedAt = seedDate.AddDays(15), AuditId = 1, AssignedOfficerId = 1 },
                new ComplianceNotification { Id = 5, Title = "License Flagged – Expired", Description = "Gamma Exports license expired, still active in system.", AlertType = "LicenseFlag", Severity = "Medium", IsRead = false, CreatedAt = seedDate.AddDays(10), ComplianceCaseId = 3, AssignedOfficerId = 1 }
            );

            // Seed audits
            modelBuilder.Entity<Audit>().HasData(
                new Audit { Id = 1, AuditTitle = "Annual License Audit – Gamma Exports", BusinessName = "Gamma Exports", Status = "Scheduled", ScheduledDate = seedDate.AddDays(15), AssignedOfficerId = 1, ComplianceCaseId = 3 },
                new Audit { Id = 2, AuditTitle = "Document Verification – Beta Imports", BusinessName = "Beta Imports Ltd.", Status = "InProgress", ScheduledDate = seedDate.AddDays(7), AssignedOfficerId = 1, ComplianceCaseId = 2 },
                new Audit { Id = 3, AuditTitle = "Transaction Review – Delta Wholesale", BusinessName = "Delta Wholesale", Status = "Scheduled", ScheduledDate = seedDate.AddDays(20), AssignedOfficerId = 1, ComplianceCaseId = 4 }
            );

            // Seed program compliance
            modelBuilder.Entity<ProgramCompliance>().HasData(
                new ProgramCompliance { Id = 1, ProgramName = "Trade Subsidy Scheme 2025", BusinessName = "Alpha Trading Co.", EligibilityStatus = "Pending", FundUsageSummary = "Received $50,000 for export incentives.", IsMisuseFlag = false, Remarks = null, ReviewedAt = seedDate, ReviewedByOfficerId = 1 },
                new ProgramCompliance { Id = 2, ProgramName = "SME Support Program", BusinessName = "Gamma Exports", EligibilityStatus = "Eligible", FundUsageSummary = "Used $20,000 for packaging equipment.", IsMisuseFlag = false, Remarks = "Usage verified.", ReviewedAt = seedDate.AddDays(3), ReviewedByOfficerId = 1 },
                new ProgramCompliance { Id = 3, ProgramName = "Import Duty Waiver", BusinessName = "Beta Imports Ltd.", EligibilityStatus = "NotEligible", FundUsageSummary = "Claimed $15,000 waiver on restricted goods.", IsMisuseFlag = true, Remarks = "Goods not in approved list. Misuse suspected.", ReviewedAt = seedDate.AddDays(6), ReviewedByOfficerId = 1 }
            );

            // Seed compliance records (history)
            modelBuilder.Entity<ComplianceRecord>().HasData(
                new ComplianceRecord { Id = 1, ComplianceCaseId = 2, ActionTaken = "StatusChange", Details = "Status changed from Pending to Investigating", OfficerRemarks = "Reviewing submitted documents.", OfficerId = 1, Timestamp = seedDate.AddDays(6) },
                new ComplianceRecord { Id = 2, ComplianceCaseId = 5, ActionTaken = "StatusChange", Details = "Status changed from Pending to Compliant", OfficerRemarks = "Inspection resolved. No issues found.", OfficerId = 1, Timestamp = seedDate.AddDays(8) },
                new ComplianceRecord { Id = 3, ComplianceCaseId = 2, ActionTaken = "DocumentReview", Details = "Certificate of origin sent for verification.", OfficerRemarks = "Awaiting response from issuing authority.", OfficerId = 1, Timestamp = seedDate.AddDays(7) }
            );
        }
    }
}
