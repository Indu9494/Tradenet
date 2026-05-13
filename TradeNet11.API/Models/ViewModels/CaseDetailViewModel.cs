namespace TradeNet11.Models.ViewModels
{
    public class CaseDetailViewModel
    {
        public ComplianceCase Case { get; set; } = null!;
        public IEnumerable<ComplianceRecord> Records { get; set; } = [];
    }
}
