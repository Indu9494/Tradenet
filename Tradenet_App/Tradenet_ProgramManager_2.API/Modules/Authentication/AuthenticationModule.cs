using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Tradenet_ProgramManager_2.API.Core;
using Tradenet_ProgramManager_2.API.Data;
using Tradenet_ProgramManager_2.API.Models;
using Tradenet_ProgramManager_2.API.Services;

namespace Tradenet_ProgramManager_2.API.Modules.Authentication
{
    /// <summary>
    /// Authentication Module Registration
    /// Handles JWT, Identity, and Account services
    /// </summary>
    public class AuthenticationModule : IModuleRegistration
    {
        public string GetModuleName() => "Authentication";

        public void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            // Register Identity
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            services.AddScoped<UserManager<ApplicationUser>>();
            services.AddScoped<SignInManager<ApplicationUser>>();

            // Register JWT
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];

            if (string.IsNullOrWhiteSpace(secretKey))
            {
                throw new InvalidOperationException("JWT Secret Key is not configured.");
            }

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"] ?? "TradeNetAPI",
                    ValidAudience = jwtSettings["Audience"] ?? "TradeNetClient",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ClockSkew = TimeSpan.Zero
                };
            });

            // Register authentication services
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IAccountService, AccountService>();
        }
    }
}
