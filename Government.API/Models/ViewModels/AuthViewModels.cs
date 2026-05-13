namespace Goverment.Models.ViewModels
{
    public class RegisterViewModel
    {
        public string FullName { get; set; } = string.Empty;
        public string BusinessName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public bool AgreeTerms { get; set; }
        public string Role { get; set; } = "Business"; // Default role
    }

    public class LoginViewModel
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; }
    }
}
