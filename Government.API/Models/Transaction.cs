namespace Government.API.Models
{
    public class Transaction
    {
        public int TransactionID { get; set; }
        public int BusinessID { get; set; }
        public string Type { get; set; } = string.Empty; // Sale/Purchase
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } = string.Empty;

        public Business? Business { get; set; }
    }
}

