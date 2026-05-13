using Microsoft.AspNetCore.Http;

namespace TradeNetAPI.Models.ViewModels
{
    public class LicenseApplicationViewModel
    {
        public int LicenseID { get; set; }
        public string LicenseType { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int BusinessID { get; set; }
        public List<IFormFile>? Documents { get; set; }
        public string? AdditionalNotes { get; set; }
    }
}
