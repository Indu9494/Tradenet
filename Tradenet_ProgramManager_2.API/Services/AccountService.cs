using Microsoft.AspNetCore.Identity;
using Tradenet_ProgramManager_2.API.Models;
using Tradenet_ProgramManager_2.API.Models.ViewModels;

namespace Tradenet_ProgramManager_2.API.Services
{
    /// <summary>
    /// Implementation of account service for handling registration, login, and user profile operations
    /// </summary>
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly ILogger<AccountService> _logger;

        public AccountService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtTokenService jwtTokenService,
            ILogger<AccountService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
            _logger = logger;
        }

        /// <summary>
        /// Register a new user with professional information
        /// </summary>
        public async Task<AccountResult> RegisterAsync(RegisterViewModel model)
        {
            try
            {
                // Validate passwords match
                if (model.Password != model.ConfirmPassword)
                {
                    return new AccountResult
                    {
                        Success = false,
                        Message = "Passwords do not match",
                        Errors = new List<string> { "Passwords do not match" }
                    };
                }

                // Check if user already exists
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    return new AccountResult
                    {
                        Success = false,
                        Message = "Email address is already in use",
                        Errors = new List<string> { "Email address is already in use" }
                    };
                }

                // Create new ApplicationUser with professional information
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.FullName,
                    EmployeeID = model.EmployeeID,
                    Department = model.Department,
                    Designation = model.Designation
                };

                // Create user with password
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Assign default "User" role
                    await _userManager.AddToRoleAsync(user, "User");

                    _logger.LogInformation($"User {model.Email} registered successfully with professional info: FullName={model.FullName}, EmployeeID={model.EmployeeID}, Department={model.Department}, Designation={model.Designation}");

                    return new AccountResult
                    {
                        Success = true,
                        Message = "User registered successfully",
                        UserId = user.Id,
                        Email = user.Email
                    };
                }

                // Log validation errors
                var errorMessages = result.Errors.Select(e => e.Description).ToList();
                _logger.LogWarning($"User registration failed for {model.Email}: {string.Join(", ", errorMessages)}");

                return new AccountResult
                {
                    Success = false,
                    Message = "Registration failed",
                    Errors = errorMessages
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception during user registration for {model.Email}: {ex.Message}");
                return new AccountResult
                {
                    Success = false,
                    Message = "An unexpected error occurred during registration",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        /// <summary>
        /// Authenticate user and generate JWT token
        /// </summary>
        public async Task<LoginResult> LoginAsync(LoginViewModel model)
        {
            try
            {
                // Find user by email
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    _logger.LogWarning($"Login attempt for non-existent user: {model.Email}");
                    return new LoginResult
                    {
                        Success = false,
                        Message = "Invalid email or password"
                    };
                }

                // Check password
                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                if (!result.Succeeded)
                {
                    _logger.LogWarning($"Failed login attempt for user: {model.Email}");
                    return new LoginResult
                    {
                        Success = false,
                        Message = "Invalid email or password"
                    };
                }

                // Get user roles
                var roles = await _userManager.GetRolesAsync(user);

                // Generate JWT token
                var token = _jwtTokenService.GenerateToken(user.Id, user.Email, roles.ToList());

                _logger.LogInformation($"User {model.Email} logged in successfully");

                return new LoginResult
                {
                    Success = true,
                    Message = "Login successful",
                    Token = token,
                    UserId = user.Id,
                    Email = user.Email
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception during login for {model.Email}: {ex.Message}");
                return new LoginResult
                {
                    Success = false,
                    Message = "An unexpected error occurred during login"
                };
            }
        }

        /// <summary>
        /// Get user profile information
        /// </summary>
        public async Task<ApplicationUser> GetUserProfileAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }
    }
}
