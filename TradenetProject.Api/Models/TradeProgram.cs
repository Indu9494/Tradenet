using System.ComponentModel.DataAnnotations;

namespace TradeNetProject.Models
{
    /// <summary>
    /// Unified TradeProgram model combining all APIs
    /// </summary>
    public class TradeProgram
    {
        [Key]
        public int ProgramID { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string? ProgramType { get; set; }

        public decimal Budget { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string? EligibilityCriteria { get; set; }

        public string Status { get; set; } = "Active"; // Active/Inactive/Completed
    }
}
