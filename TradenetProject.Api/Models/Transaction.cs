using System.ComponentModel.DataAnnotations;

namespace TradeNetProject.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionID { get; set; }
        public string BusinessName { get; set; }
        public string Type { get; set; }
        public double Amount { get; set; }
        public string Status { get; set; }
    }
}