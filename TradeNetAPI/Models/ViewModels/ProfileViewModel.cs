using TradeNetAPI.Models;

namespace TradeNetAPI.Models.ViewModels
{
    public class ProfileViewModel
    {
        public User User { get; set; } = new User();
        public Business? Business { get; set; }
        public List<BusinessDocument> Documents { get; set; } = new List<BusinessDocument>();
        public string ComplianceStatus { get; set; } = "Compliant";
        public string? ComplianceMessage { get; set; }
    }
}
