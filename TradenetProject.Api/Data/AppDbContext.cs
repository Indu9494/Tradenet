using Microsoft.EntityFrameworkCore;
using TradeNetProject.Models;

namespace TradeNetProject.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<TradeOfficer> TradeOfficers { get; set; }
        public DbSet<TradeLicense> TradeLicenses { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<MarketRecord> MarketRecords { get; set; }
        public DbSet<ComplianceRecord> ComplianceRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TradeOfficer>().HasData(
                new TradeOfficer
                {
                    OfficerID = 1001,
                    FullName = "Trade Officer",
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

            modelBuilder.Entity<TradeLicense>().HasData(
                new TradeLicense { LicenseID = 1, BusinessName = "ABC Traders", Type = "Import", Status = "Pending" },
                new TradeLicense { LicenseID = 2, BusinessName = "XYZ Exports", Type = "Export", Status = "Pending" },
                new TradeLicense { LicenseID = 3, BusinessName = "Global Imports Ltd", Type = "Import", Status = "Approved" },
                new TradeLicense { LicenseID = 4, BusinessName = "Eastern Commerce", Type = "Export", Status = "Approved" },
                new TradeLicense { LicenseID = 5, BusinessName = "Pacific Wholesale", Type = "Import", Status = "Rejected" },
                new TradeLicense { LicenseID = 6, BusinessName = "Sunrise Trading Co", Type = "Export", Status = "Approved" },
                new TradeLicense { LicenseID = 7, BusinessName = "Delta Freight", Type = "Import", Status = "Pending" },
                new TradeLicense { LicenseID = 8, BusinessName = "Coastal Ventures", Type = "Export", Status = "Approved" },
                new TradeLicense { LicenseID = 9, BusinessName = "Metro Distributors", Type = "Import", Status = "Pending" },
                new TradeLicense { LicenseID = 10, BusinessName = "Summit Logistics", Type = "Export", Status = "Rejected" }
            );

            modelBuilder.Entity<Transaction>().HasData(
                new Transaction { TransactionID = 1, BusinessName = "ABC Traders", Type = "Sale", Amount = 5000, Status = "Completed" },
                new Transaction { TransactionID = 2, BusinessName = "XYZ Exports", Type = "Purchase", Amount = 8000, Status = "Pending" },
                new Transaction { TransactionID = 3, BusinessName = "Global Imports Ltd", Type = "Sale", Amount = 12500, Status = "Completed" },
                new Transaction { TransactionID = 4, BusinessName = "Eastern Commerce", Type = "Purchase", Amount = 3200, Status = "Completed" },
                new Transaction { TransactionID = 5, BusinessName = "Pacific Wholesale", Type = "Sale", Amount = 9750, Status = "Pending" },
                new Transaction { TransactionID = 6, BusinessName = "Sunrise Trading Co", Type = "Purchase", Amount = 15000, Status = "Completed" },
                new Transaction { TransactionID = 7, BusinessName = "Delta Freight", Type = "Sale", Amount = 6800, Status = "Pending" },
                new Transaction { TransactionID = 8, BusinessName = "Coastal Ventures", Type = "Purchase", Amount = 22000, Status = "Completed" },
                new Transaction { TransactionID = 9, BusinessName = "Metro Distributors", Type = "Sale", Amount = 4100, Status = "Completed" },
                new Transaction { TransactionID = 10, BusinessName = "Summit Logistics", Type = "Purchase", Amount = 18500, Status = "Pending" }
            );

            modelBuilder.Entity<Document>().HasData(
                new Document { DocumentID = 1, BusinessName = "ABC Traders", DocumentType = "Trade Certificate", SubmittedDate = "2026-01-10", VerifiedBy = "", Status = "Pending" },
                new Document { DocumentID = 2, BusinessName = "XYZ Exports", DocumentType = "Tax Clearance", SubmittedDate = "2026-01-12", VerifiedBy = "", Status = "Pending" },
                new Document { DocumentID = 3, BusinessName = "Global Imports Ltd", DocumentType = "Import Permit", SubmittedDate = "2026-01-08", VerifiedBy = "Trade Officer", Status = "Verified" },
                new Document { DocumentID = 4, BusinessName = "Eastern Commerce", DocumentType = "Business Registration", SubmittedDate = "2026-01-15", VerifiedBy = "", Status = "Pending" },
                new Document { DocumentID = 5, BusinessName = "Pacific Wholesale", DocumentType = "Export License", SubmittedDate = "2026-01-05", VerifiedBy = "Trade Officer", Status = "Rejected" },
                new Document { DocumentID = 6, BusinessName = "Sunrise Trading Co", DocumentType = "Trade Certificate", SubmittedDate = "2026-01-18", VerifiedBy = "Trade Officer", Status = "Verified" },
                new Document { DocumentID = 7, BusinessName = "Delta Freight", DocumentType = "Tax Clearance", SubmittedDate = "2026-01-20", VerifiedBy = "", Status = "Pending" },
                new Document { DocumentID = 8, BusinessName = "Coastal Ventures", DocumentType = "Import Permit", SubmittedDate = "2026-01-22", VerifiedBy = "Trade Officer", Status = "Verified" },
                new Document { DocumentID = 9, BusinessName = "Metro Distributors", DocumentType = "Business Registration", SubmittedDate = "2026-01-25", VerifiedBy = "Trade Officer", Status = "Rejected" },
                new Document { DocumentID = 10, BusinessName = "Summit Logistics", DocumentType = "Export License", SubmittedDate = "2026-01-28", VerifiedBy = "", Status = "Pending" }
            );

            modelBuilder.Entity<MarketRecord>().HasData(
                new MarketRecord { RecordID = 1, MarketName = "Central Trade Market", Location = "Downtown District", TotalVendors = 124, Category = "Wholesale", Revenue = 450000, Status = "Active" },
                new MarketRecord { RecordID = 2, MarketName = "Eastern Export Hub", Location = "Harbor Zone", TotalVendors = 86, Category = "Export", Revenue = 320000, Status = "Active" },
                new MarketRecord { RecordID = 3, MarketName = "Riverside Market", Location = "South Bank", TotalVendors = 45, Category = "Retail", Revenue = 125000, Status = "Under Review" },
                new MarketRecord { RecordID = 4, MarketName = "North Import Center", Location = "Industrial Park", TotalVendors = 67, Category = "Import", Revenue = 275000, Status = "Active" },
                new MarketRecord { RecordID = 5, MarketName = "Sunset Bazaar", Location = "West End", TotalVendors = 32, Category = "Retail", Revenue = 85000, Status = "Inactive" },
                new MarketRecord { RecordID = 6, MarketName = "Hilltop Trade Plaza", Location = "Uptown Heights", TotalVendors = 98, Category = "Wholesale", Revenue = 390000, Status = "Active" },
                new MarketRecord { RecordID = 7, MarketName = "Lakeside Commerce Park", Location = "Lake District", TotalVendors = 53, Category = "Import", Revenue = 210000, Status = "Active" },
                new MarketRecord { RecordID = 8, MarketName = "Old Town Market", Location = "Heritage Quarter", TotalVendors = 28, Category = "Retail", Revenue = 72000, Status = "Under Review" },
                new MarketRecord { RecordID = 9, MarketName = "Greenfield Export Zone", Location = "Airport Road", TotalVendors = 74, Category = "Export", Revenue = 415000, Status = "Active" },
                new MarketRecord { RecordID = 10, MarketName = "Bayview Trading Center", Location = "Coastal Strip", TotalVendors = 41, Category = "Wholesale", Revenue = 165000, Status = "Inactive" }
            );

            modelBuilder.Entity<ComplianceRecord>().HasData(
                new ComplianceRecord { ComplianceID = 1, BusinessName = "ABC Traders", InspectionType = "Annual Audit", InspectedDate = "2026-01-05", InspectedBy = "Trade Officer", Remarks = "All documents in order, no violations found.", Result = "Passed" },
                new ComplianceRecord { ComplianceID = 2, BusinessName = "XYZ Exports", InspectionType = "Safety Inspection", InspectedDate = "2026-01-10", InspectedBy = "Trade Officer", Remarks = "Minor safety signage missing, corrective notice issued.", Result = "Failed" },
                new ComplianceRecord { ComplianceID = 3, BusinessName = "Global Imports Ltd", InspectionType = "License Compliance", InspectedDate = "2026-01-12", InspectedBy = "Trade Officer", Remarks = "Operating license valid, import permits verified.", Result = "Passed" },
                new ComplianceRecord { ComplianceID = 4, BusinessName = "Eastern Commerce", InspectionType = "Tax Compliance", InspectedDate = "2026-01-18", InspectedBy = "", Remarks = "", Result = "Pending" },
                new ComplianceRecord { ComplianceID = 5, BusinessName = "Pacific Wholesale", InspectionType = "Environmental Check", InspectedDate = "2026-01-20", InspectedBy = "", Remarks = "", Result = "Pending" },
                new ComplianceRecord { ComplianceID = 6, BusinessName = "Sunrise Trading Co", InspectionType = "Annual Audit", InspectedDate = "2025-12-28", InspectedBy = "Trade Officer", Remarks = "Expired trade certificate, renewal required.", Result = "Failed" },
                new ComplianceRecord { ComplianceID = 7, BusinessName = "Delta Freight", InspectionType = "Safety Inspection", InspectedDate = "2026-02-01", InspectedBy = "Trade Officer", Remarks = "Fire extinguishers up to date, exits clearly marked.", Result = "Passed" },
                new ComplianceRecord { ComplianceID = 8, BusinessName = "Coastal Ventures", InspectionType = "License Compliance", InspectedDate = "2026-02-05", InspectedBy = "Trade Officer", Remarks = "Export license expired, renewal in progress.", Result = "Failed" },
                new ComplianceRecord { ComplianceID = 9, BusinessName = "Metro Distributors", InspectionType = "Tax Compliance", InspectedDate = "2026-02-08", InspectedBy = "", Remarks = "", Result = "Pending" },
                new ComplianceRecord { ComplianceID = 10, BusinessName = "Summit Logistics", InspectionType = "Annual Audit", InspectedDate = "2026-02-10", InspectedBy = "Trade Officer", Remarks = "All regulatory filings current, warehouse standards met.", Result = "Passed" }
            );
        }
    }
}
