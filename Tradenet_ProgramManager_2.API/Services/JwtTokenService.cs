using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Tradenet_ProgramManager_2.API.Services
{
    /// <summary>
    /// JWT Token Service for generating secure authentication tokens.
    /// Production-grade: All JWT secrets are loaded from secure configuration sources.
    /// No hardcoded fallback secrets - ensures security policy compliance.
    /// </summary>
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<JwtTokenService> _logger;

        public JwtTokenService(IConfiguration configuration, ILogger<JwtTokenService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Generate a JWT token for the specified user with their roles.
        /// </summary>
        /// <param name="userId">Unique identifier of the user</param>
        /// <param name="email">Email address of the user</param>
        /// <param name="roles">List of roles assigned to the user</param>
        /// <returns>Signed JWT token as string</returns>
        /// <exception cref="InvalidOperationException">Thrown when JWT configuration is missing or invalid</exception>
        public string GenerateToken(string userId, string email, List<string> roles)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            // Retrieve JWT configuration - no hardcoded fallbacks
            var secretKey = jwtSettings["SecretKey"];
            var issuer = jwtSettings["Issuer"] ?? "TradeNetAPI";
            var audience = jwtSettings["Audience"] ?? "TradeNetClient";

            // Parse expiration minutes with validation
            if (!int.TryParse(jwtSettings["ExpirationMinutes"], out var expirationMinutes))
            {
                expirationMinutes = 60; // Default if not configured
            }

            // Validate required JWT secret key is configured
            if (string.IsNullOrWhiteSpace(secretKey))
            {
                var errorMessage = "JWT Secret Key is not configured. This is a critical security configuration error. " +
                    "Please configure 'JwtSettings:SecretKey' in appsettings.json or via 'dotnet user-secrets'.";
                _logger.LogError(errorMessage);
                throw new InvalidOperationException(errorMessage);
            }

            try
            {
                // Create signing credentials using the configured secret key
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                // Build claims from user information and roles
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Email, email),
                    new Claim("Email", email)
                };

                // Add role claims for authorization
                if (roles != null)
                {
                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }
                }

                // Create JWT token with expiration
                var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                    signingCredentials: credentials
                );

                // Serialize token to string
                var tokenHandler = new JwtSecurityTokenHandler();
                return tokenHandler.WriteToken(token);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Invalid JWT configuration provided. Secret key may be too short or contain invalid characters.");
                throw new InvalidOperationException("JWT configuration is invalid. Please verify 'JwtSettings:SecretKey' in your configuration.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while generating JWT token");
                throw;
            }
        }
    }
}
