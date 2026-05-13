using Microsoft.AspNetCore.Http;

namespace TradeNetAPI.Models.ViewModels
{
    public class BusinessUpdateViewModel
    {
        public int BusinessID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string ContactInfo { get; set; } = string.Empty;
        public string? RegistrationNumber { get; set; }
        public IFormFile? ProfilePicture { get; set; }
        public List<IFormFile>? Documents { get; set; }
    }
}
