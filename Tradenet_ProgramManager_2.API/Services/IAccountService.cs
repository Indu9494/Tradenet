using Tradenet_ProgramManager_2.API.Models;
using Tradenet_ProgramManager_2.API.Models.ViewModels;

namespace Tradenet_ProgramManager_2.API.Services
{
    /// <summary>
    /// Interface for account-related operations including registration and login
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Register a new user with professional information
        /// </summary>
        Task<AccountResult> RegisterAsync(RegisterViewModel model);

        /// <summary>
        /// Authenticate user and generate JWT token
        /// </summary>
        Task<LoginResult> LoginAsync(LoginViewModel model);

        /// <summary>
        /// Get user profile information
        /// </summary>
        Task<ApplicationUser> GetUserProfileAsync(string userId);
    }

    /// <summary>
    /// Result object for registration
    /// </summary>
    public class AccountResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; } = new();
        public string UserId { get; set; }
        public string Email { get; set; }
    }

    /// <summary>
    /// Result object for login
    /// </summary>
    public class LoginResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
    }
}
