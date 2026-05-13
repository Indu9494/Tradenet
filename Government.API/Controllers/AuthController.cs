using Goverment.Models;
using Goverment.Services;
using Government.API.Data;
using Government.API.Exceptions;
using Government.API.Models.ViewModels;
using Government.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Government.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(AppDbContext context, IJwtTokenService jwtTokenService, ILogger<AuthController> logger)
        {
            _context = context;
            _jwtTokenService = jwtTokenService;
            _logger = logger;
        }

        /// <summary>
        /// Login endpoint that returns a JWT token
        /// </summary>
        /// <param name="request">Login credentials (email and password)</param>
        /// <returns>JWT token and user information on successful authentication</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthTokenResponse>> Login([FromBody] ApiLoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                _logger.LogWarning("Login attempt with missing credentials");
                throw new ValidationException("Email and password are required.", new List<ValidationError>
                {
                    new ValidationError { Field = nameof(request.Email), Message = "Email is required" },
                    new ValidationError { Field = nameof(request.Password), Message = "Password is required" }
                });
            }

            try
            {
                _logger.LogInformation($"Login attempt for email: {request.Email}");
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

                if (user == null)
                {
                    _logger.LogWarning($"Login attempt with non-existent email: {request.Email}");
                    throw new InvalidCredentialsException();
                }

                // Verify password
                if (!PasswordHasher.VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt))
                {
                    _logger.LogWarning($"Failed login attempt for user: {request.Email}");
                    throw new InvalidCredentialsException();
                }

                // Update last login date
                user.LastLoginDate = DateTime.Now;
                await _context.SaveChangesAsync();

                // Generate JWT token
                var token = _jwtTokenService.GenerateToken(user);

                _logger.LogInformation($"User logged in successfully: {request.Email}");

                return Ok(new AuthTokenResponse
                {
                    Success = true,
                    Message = "Login successful.",
                    Token = token,
                    User = new UserInfoDto
                    {
                        UserId = user.UserID,
                        Name = user.Name,
                        Email = user.Email,
                        Role = user.Role,
                        BusinessName = user.BusinessName ?? ""
                    }
                });
            }
            catch (InvalidCredentialsException)
            {
                throw;
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during login");
                throw new DatabaseException("An error occurred during login", ex);
            }
        }

        /// <summary>
        /// Register endpoint for new users
        /// </summary>
        /// <param name="request">Registration details</param>
        /// <returns>JWT token and user information on successful registration</returns>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthTokenResponse>> Register([FromBody] ApiRegisterRequest request)
        {
            var validationErrors = new List<ValidationError>();

            if (string.IsNullOrWhiteSpace(request.FullName))
            {
                validationErrors.Add(new ValidationError { Field = nameof(request.FullName), Message = "Full name is required" });
            }

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                validationErrors.Add(new ValidationError { Field = nameof(request.Email), Message = "Email is required" });
            }

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                validationErrors.Add(new ValidationError { Field = nameof(request.Password), Message = "Password is required" });
            }

            if (validationErrors.Count > 0)
            {
                _logger.LogWarning("Registration validation failed with missing required fields");
                throw new ValidationException("Registration validation failed", validationErrors);
            }

            if (request.Password != request.ConfirmPassword)
            {
                _logger.LogWarning("Registration attempt with mismatched passwords");
                throw new ValidationException("Passwords do not match", new List<ValidationError>
                {
                    new ValidationError { Field = nameof(request.ConfirmPassword), Message = "Passwords do not match" }
                });
            }

            // Validate password strength
            if (!PasswordHasher.ValidatePasswordStrength(request.Password))
            {
                _logger.LogWarning("Registration attempt with weak password");
                throw new InvalidPasswordException("Password must be at least 6 characters and contain uppercase, lowercase, and digit");
            }

            try
            {
                _logger.LogInformation($"Registration attempt for email: {request.Email}");

                // Check if user already exists
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
                if (existingUser != null)
                {
                    _logger.LogWarning($"Registration attempt with existing email: {request.Email}");
                    throw new UserAlreadyExistsException($"User with email '{request.Email}' already exists", request.Email);
                }

                // Hash password
                var (hash, salt) = PasswordHasher.HashPassword(request.Password);

                // Create new user
                var user = new User
                {
                    Name = request.FullName,
                    Email = request.Email,
                    Role = request.Role,
                    BusinessName = request.BusinessName,
                    Phone = "",
                    Status = "Active",
                    PasswordHash = hash,
                    PasswordSalt = salt,
                    CreatedDate = DateTime.Now
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // Generate JWT token
                var token = _jwtTokenService.GenerateToken(user);

                _logger.LogInformation($"New user registered successfully: {request.Email}");

                return CreatedAtAction(nameof(Register), new AuthTokenResponse
                {
                    Success = true,
                    Message = "Registration successful.",
                    Token = token,
                    User = new UserInfoDto
                    {
                        UserId = user.UserID,
                        Name = user.Name,
                        Email = user.Email,
                        Role = user.Role,
                        BusinessName = user.BusinessName ?? ""
                    }
                });
            }
            catch (UserAlreadyExistsException)
            {
                throw;
            }
            catch (InvalidPasswordException)
            {
                throw;
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during registration");
                throw new DatabaseException("An error occurred during registration", ex);
            }
        }

        /// <summary>
        /// Validate token endpoint - checks if provided token is valid
        /// </summary>
        /// <returns>Validation result</returns>
        [HttpGet("validate-token")]
        [Authorize]
        public IActionResult ValidateToken()
        {
            try
            {
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                var emailClaim = User.FindFirst(System.Security.Claims.ClaimTypes.Email);
                var roleClaim = User.FindFirst(System.Security.Claims.ClaimTypes.Role);

                if (userIdClaim == null)
                {
                    _logger.LogWarning("Validate token attempt with missing user claims");
                    throw new InvalidTokenException("Invalid token: missing user information");
                }

                _logger.LogInformation($"Token validated for user: {emailClaim?.Value}");

                return Ok(new
                {
                    Success = true,
                    Message = "Token is valid.",
                    UserId = userIdClaim.Value,
                    Email = emailClaim?.Value,
                    Role = roleClaim?.Value
                });
            }
            catch (InvalidTokenException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating token");
                throw new InvalidTokenException("An error occurred while validating token", ex);
            }
        }
    }
}
