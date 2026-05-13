using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tradenet_ProgramManager_2.API.Models;
using Tradenet_ProgramManager_2.API.Models.ViewModels;
using Tradenet_ProgramManager_2.API.Services;

namespace Tradenet_ProgramManager_2.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            IAccountService accountService,
            UserManager<ApplicationUser> userManager,
            ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _userManager = userManager;
            _logger = logger;
        }

        /// <summary>
        /// Register a new user with professional information and credentials
        /// </summary>
        /// <param name="model">Registration details including FullName, EmployeeID, Department, Designation, Email, Password</param>
        /// <returns>Registration result</returns>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(new { message = "Validation failed", errors });
            }

            var result = await _accountService.RegisterAsync(model);

            if (!result.Success)
            {
                return BadRequest(new
                {
                    message = result.Message,
                    errors = result.Errors
                });
            }

            return Ok(new
            {
                message = result.Message,
                userId = result.UserId,
                email = result.Email
            });
        }

        /// <summary>
        /// Login with email and password, returns JWT token
        /// </summary>
        /// <param name="model">Login credentials (Email and Password)</param>
        /// <returns>JWT token and user information if successful</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(new { message = "Validation failed", errors });
            }

            var result = await _accountService.LoginAsync(model);

            if (!result.Success)
            {
                return Unauthorized(new { message = result.Message });
            }

            return Ok(new LoginResponseViewModel
            {
                Success = true,
                Message = result.Message,
                Token = result.Token,
                UserId = result.UserId,
                Email = result.Email
            });
        }

        /// <summary>
        /// Test endpoint to verify JWT token is valid (requires authentication)
        /// </summary>
        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized(new { message = "User not found" });
            }

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new
            {
                userId = user.Id,
                email = user.Email,
                userName = user.UserName,
                roles
            });
        }
    }
}
