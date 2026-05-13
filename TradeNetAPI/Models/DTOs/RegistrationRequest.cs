namespace TradeNetAPI.Models.DTOs
{
    public class RegistrationRequest
    {
        public UserRegistrationDto User { get; set; } = new UserRegistrationDto();
        public BusinessRegistrationDto Business { get; set; } = new BusinessRegistrationDto();
    }
}
