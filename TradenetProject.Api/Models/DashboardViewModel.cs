namespace TradeNetProject.Models
{
    public class DashboardViewModel
    {
        public string OfficerName { get; set; }

        // Licenses
        public int TotalLicenses { get; set; }
        public int ApprovedLicenses { get; set; }

        // Transactions
        public int TotalTransactions { get; set; }
        public int CompletedTransactions { get; set; }

        // Documents
        public int TotalDocuments { get; set; }
        public int VerifiedDocuments { get; set; }

        // Markets
        public int TotalMarkets { get; set; }
        public int ActiveMarkets { get; set; }

        // Compliance
        public int TotalCompliance { get; set; }
        public int PassedCompliance { get; set; }

        public int LicensePercent => TotalLicenses == 0 ? 0 : (int)Math.Round(ApprovedLicenses * 100.0 / TotalLicenses);
        public int TransactionPercent => TotalTransactions == 0 ? 0 : (int)Math.Round(CompletedTransactions * 100.0 / TotalTransactions);
        public int DocumentPercent => TotalDocuments == 0 ? 0 : (int)Math.Round(VerifiedDocuments * 100.0 / TotalDocuments);
        public int MarketPercent => TotalMarkets == 0 ? 0 : (int)Math.Round(ActiveMarkets * 100.0 / TotalMarkets);
        public int CompliancePercent => TotalCompliance == 0 ? 0 : (int)Math.Round(PassedCompliance * 100.0 / TotalCompliance);
        public int OverallPercent
        {
            get
            {
                int total = TotalLicenses + TotalTransactions + TotalDocuments + TotalMarkets + TotalCompliance;
                int done = ApprovedLicenses + CompletedTransactions + VerifiedDocuments + ActiveMarkets + PassedCompliance;
                return total == 0 ? 0 : (int)Math.Round(done * 100.0 / total);
            }
        }
    }
}
