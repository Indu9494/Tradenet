namespace TradeNetAPI.Models.DTOs
{
    public class BusinessRegistrationDto
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = "Trader";
        public string Address { get; set; } = string.Empty;
        public string ContactInfo { get; set; } = string.Empty;
    }
}
