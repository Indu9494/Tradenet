namespace Tradenet_ProgramManager_2.API.Models.ViewModels
{
    public class TransactionViewModel
    {
        public decimal SalesVolume { get; set; }
        public decimal PurchaseVolume { get; set; }
        public IEnumerable<Transaction> RecentTransactions { get; set; } = new List<Transaction>();
    }
}
