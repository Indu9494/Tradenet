using TradeNetAPI.Models;

namespace TradeNetAPI.Models.ViewModels
{
    public class DashboardViewModel
    {
        public List<TradeLicense> AppliedLicenses { get; set; } = new List<TradeLicense>();
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
        public List<TradeProgram> AvailableSubsidies { get; set; } = new List<TradeProgram>();
        public int PendingLicenses { get; set; }
        public int ApprovedLicenses { get; set; }
        public int PendingTransactions { get; set; }
        public decimal TotalTransactionAmount { get; set; }
    }
}
