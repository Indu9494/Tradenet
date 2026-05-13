namespace TradeNetAPI.Models
{
    public class Transaction
    {
        public int TransactionID { get; set; }
        public int BusinessID { get; set; }
        public string Type { get; set; } = string.Empty; // Sale/Purchase/Export/Import
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } = "Pending"; // Pending/Completed/Failed/UnderReview
        public string? Description { get; set; }
        public string? InvoiceNumber { get; set; }
        public string? Counterparty { get; set; }
    }
}
