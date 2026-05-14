using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradeNetProject.Models
{
    /// <summary>
    /// Unified Transaction model combining all APIs
    /// </summary>
    public class Transaction
    {
        [Key]
        public int TransactionID { get; set; }

        [ForeignKey("Business")]
        public int BusinessID { get; set; }

        [Required]
        public string Type { get; set; } = string.Empty; // Sale/Purchase/Export/Import

        public decimal Amount { get; set; }

        public DateTime? Date { get; set; } = DateTime.Now;

        public string Status { get; set; } = "Pending"; // Pending/Completed/Failed

        public string? Counterparty { get; set; }

        public string? InvoiceNumber { get; set; }

        public string? Description { get; set; }

        // Legacy compatibility - old MVC used BusinessName instead of BusinessID
        public string? BusinessName { get; set; }

        // Navigation property
        public virtual Business? Business { get; set; }
    }
}
