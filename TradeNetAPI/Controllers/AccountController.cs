using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TradeNetAPI.Data;
using TradeNetAPI.Models;
using TradeNetAPI.Models.DTOs;

namespace TradeNetAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly TradeNetDbContext _context;
        public AccountController(TradeNetDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest request)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(request.User.Name) || 
                string.IsNullOrWhiteSpace(request.User.Email) ||
                string.IsNullOrWhiteSpace(request.User.Password))
            {
                return BadRequest(new { error = "Name, email, and password are required." });
            }

            if (string.IsNullOrWhiteSpace(request.Business.Name))
            {
                return BadRequest(new { error = "Business name is required." });
            }

            // Check if email already exists
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.User.Email);
            if (existingUser != null)
            {
                return BadRequest(new { error = "User with this email already exists." });
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Create User
                var user = new User
                {
                    Name = request.User.Name,
                    Email = request.User.Email,
                    Phone = request.User.Phone,
                    Role = request.User.Role,
                    Status = request.User.Status,
                    // TODO: Hash password in production
                    // Password = BCrypt.Net.BCrypt.HashPassword(request.User.Password)
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // Create Business
                var business = new Business
                {
                    UserID = user.UserID,
                    Name = request.Business.Name,
                    Type = request.Business.Type,
                    Address = request.Business.Address,
                    ContactInfo = request.Business.ContactInfo,
                    Status = "Pending",
                    RegistrationDate = DateTime.Now,
                    ComplianceStatus = "Compliant"
                };
                _context.Businesses.Add(business);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return Ok(new { success = true, user, business });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(new { error = "Registration failed: " + ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { error = "Email and password are required." });
            }

            try
            {
                // Find user by email
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
                if (user == null)
                {
                    return Unauthorized(new { error = "User not registered. Please register first." });
                }

                // TODO: Verify password hash in production
                // if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                // {
                //     return Unauthorized(new { error = "Invalid password." });
                // }

                // Get associated business
                var business = await _context.Businesses.FirstOrDefaultAsync(b => b.UserID == user.UserID);

                return Ok(new { success = true, user, business });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Login failed: " + ex.Message });
            }
        }
    }
}
