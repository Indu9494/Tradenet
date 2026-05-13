using System.ComponentModel.DataAnnotations;

namespace TradeNetProject.Models
{
    public class Document
    {
        [Key]
        public int DocumentID { get; set; }
        public string BusinessName { get; set; }
        public string DocumentType { get; set; }
        public string SubmittedDate { get; set; }
        public string VerifiedBy { get; set; }
        public string Status { get; set; }
    }
}
