using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Government.API.Models
{
    /// <summary>
    /// Business Document model (adding from TradeNetAPI)
    /// </summary>
    public class BusinessDocument
    {
        [Key]
        public int DocumentID { get; set; }

        [ForeignKey("Business")]
        public int BusinessID { get; set; }

        public string DocType { get; set; } = string.Empty;

        public string FileURI { get; set; } = string.Empty;

        public DateTime UploadedDate { get; set; } = DateTime.Now;

        public string VerificationStatus { get; set; } = "Pending"; // Pending/Verified/Rejected

        // Navigation property
        public virtual Business? Business { get; set; }
    }
}

