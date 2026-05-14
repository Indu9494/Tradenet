using Goverment.Data;
using Goverment.Models;
using Goverment.Models.ViewModels;
using Goverment.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Goverment.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // =============================================
        // BROWSER ENDPOINTS (HTML Forms)
        // =============================================

        // GET: Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid email or password.");
                    return View(model);
                }

                // Verify password
                if (!PasswordHasher.VerifyPassword(model.Password, user.PasswordHash, user.PasswordSalt))
                {
                    ModelState.AddModelError(string.Empty, "Invalid email or password.");
                    return View(model);
                }

                // Update last login date
                user.LastLoginDate = DateTime.Now;
                await _context.SaveChangesAsync();

                // Create claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim("BusinessName", user.BusinessName ?? "")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe,
                    ExpiresUtc = model.RememberMe ? DateTimeOffset.UtcNow.AddDays(30) : DateTimeOffset.UtcNow.AddHours(24)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                // Redirect based on user role
                return user.Role switch
                {
                    "Business" => RedirectToAction("Dashboard", "BusinessOfficer"),
                    "Officer" => RedirectToAction("Dashboard", "TradeOfficer"),
                    "Compliance" => RedirectToAction("Dashboard", "ComplianceOfficer"),
                    "Manager" => RedirectToAction("Dashboard", "ProgramManager"),
                    "Auditor" => RedirectToAction("Index", "AuditorDashboard"),
                    "Admin" => RedirectToAction("Index", "AuditorDashboard"),
                    _ => RedirectToLocal(returnUrl)
                };
            }

            return View(model);
        }

        // GET: Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Password != model.ConfirmPassword)
                {
                    ModelState.AddModelError("", "Passwords do not match");
                    return View(model);
                }

                if (!model.AgreeTerms)
                {
                    ModelState.AddModelError("", "You must agree to terms and conditions");
                    return View(model);
                }

                // Validate password strength
                if (!PasswordHasher.ValidatePasswordStrength(model.Password))
                {
                    ModelState.AddModelError("", "Password must be at least 6 characters and contain uppercase, lowercase, and digit");
                    return View(model);
                }

                // Check if user already exists
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "User with this email already exists");
                    return View(model);
                }

                // Hash password
                var (hash, salt) = PasswordHasher.HashPassword(model.Password);

                // Create new user
                var user = new User
                {
                    Name = model.FullName,
                    Email = model.Email,
                    Role = model.Role,
                    BusinessName = model.BusinessName,
                    Phone = "",
                    Status = "Active",
                    PasswordHash = hash,
                    PasswordSalt = salt,
                    CreatedDate = DateTime.Now
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // Sign in the user
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim("BusinessName", user.BusinessName ?? "")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = false,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(24)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                // Redirect based on role
                if (user.Role == "Auditor")
                {
                    return RedirectToAction("Index", "AuditorDashboard");
                }

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        // POST: Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // GET: Account/AccessDenied
        public IActionResult AccessDenied()
        {
            return View();
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
