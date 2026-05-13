using System.ComponentModel.DataAnnotations;

namespace TradeNetProject.Models
{
    public class ComplianceRecord
    {
        [Key]
      public int ComplianceID { get; set; }
        public string BusinessName { get; set; }
        public string InspectionType { get; set; }
        public string InspectedDate { get; set; }
        public string InspectedBy { get; set; }
        public string Remarks { get; set; }
        public string Result { get; set; }
    }
}
