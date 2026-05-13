namespace TradeNetAPI.Models.ViewModels
{
    public class TransactionCreateViewModel
    {
        public int BusinessID { get; set; }
        public string Type { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public string? InvoiceNumber { get; set; }
        public string? Counterparty { get; set; }
    }
}
