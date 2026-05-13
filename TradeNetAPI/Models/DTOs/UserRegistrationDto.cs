namespace TradeNetAPI.Models.DTOs
{
    public class UserRegistrationDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Role { get; set; } = "Business";
        public string Status { get; set; } = "Active";
        public string Password { get; set; } = string.Empty;
    }
}
