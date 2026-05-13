namespace TradeNetAPI.Models
{
    public class MarketRecord
    {
        public int RecordID { get; set; }
        public int TransactionID { get; set; }
        public int OfficerID { get; set; }
        public string Notes { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Status { get; set; } = "Active";
    }
}
