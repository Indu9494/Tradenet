
using Government.API.Data;
using Government.API.Middleware;
using Government.API.Services;
using Goverment.Interfaces;
using Goverment.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

namespace Government.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure Serilog with Bootstrap Logger
            Log.Logger = new LoggerConfiguration()
                
                .WriteTo.Console()
                .WriteTo.File(
                    "logs/government.txt",
                    rollingInterval: RollingInterval.Day)
                .CreateBootstrapLogger();

  

            try
            {
                // Configure DbContext from appsettings (use local API data context)
                // Configure DbContext and point migrations to the Goverment project where migrations were created
                builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("ProductDbConnection"), sql =>
                        sql.MigrationsAssembly("Goverment")));

                // Configure JWT Authentication
                var jwtSettings = builder.Configuration.GetSection("JwtSettings");
                var secretKey = jwtSettings["SecretKey"];
                var issuer = jwtSettings["Issuer"];
                var audience = jwtSettings["Audience"];

                if (!string.IsNullOrEmpty(secretKey))
                {
                    var key = Encoding.UTF8.GetBytes(secretKey);
                    builder.Services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(key),
                            ValidateIssuer = !string.IsNullOrEmpty(issuer),
                            ValidIssuer = issuer,
                            ValidateAudience = !string.IsNullOrEmpty(audience),
                            ValidAudience = audience,
                            ValidateLifetime = true,
                            ClockSkew = TimeSpan.Zero
                        };
                    });
                }

                // Register JWT Token Service
                builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

                // Register repositories
                builder.Services.AddScoped<IBusinessRepository, BusinessRepository>();
                builder.Services.AddScoped<IAuditRepository, AuditRepository>();
                builder.Services.AddScoped<ITradeProgramRepository, TradeProgramRepository>();
                builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
                builder.Services.AddScoped<IResourceRepository, ResourceRepository>();
                builder.Services.AddScoped<IReportRepository, ReportRepository>();
                builder.Services.AddScoped<IComplianceRecordRepository, ComplianceRecordRepository>();
                builder.Services.AddScoped<ITradeLicenseRepository, TradeLicenseRepository>();
                builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
                builder.Services.AddScoped<ISubsidyRepository, SubsidyRepository>();

                // Add controllers and Swagger/OpenAPI
                // Restrict discovered controllers to this API assembly only so controllers from the main
                // `Goverment` project are not discovered (prevents duplicate endpoints in Swagger).
                builder.Services.AddControllers()
                    .ConfigureApplicationPartManager(apm =>
                    {
                        apm.ApplicationParts.Clear();
                        apm.ApplicationParts.Add(new Microsoft.AspNetCore.Mvc.ApplicationParts.AssemblyPart(typeof(Program).Assembly));
                    });
                builder.Services.AddOpenApi();
                builder.Services.AddSwaggerGen();
                // Enable CORS for testing (adjust origins for production)
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("AllowAll", policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
                });

                // Configure problem details for exception handler
                builder.Services.AddProblemDetails();

                var app = builder.Build();
                app.UseExceptionHandler();

                // Use global exception handler middleware
                app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

                // Initialize database migrations if necessary
                using (var scope = app.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    try
                    {
                        var context = services.GetRequiredService<AppDbContext>();
                        logger.LogInformation("Starting database migration...");
                        context.Database.Migrate();
                        logger.LogInformation("Database migration completed successfully.");
                    }
                    catch (Exception ex)
                    {
                        logger.LogCritical(ex, "Database migration failed during startup.");
                        throw;
                    }
                }

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    // OpenAPI (Microsoft) mapping
                    app.MapOpenApi();

                    // Swagger UI
                    app.UseSwagger();
                    app.UseSwaggerUI(c =>
                    {
                        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Government.API v1");
                        c.RoutePrefix = "swagger"; // serve at /swagger
                    });
                }

                app.UseHttpsRedirection();
                // Apply CORS policy
                app.UseCors("AllowAll");

                // Use authentication and authorization middleware
                app.UseAuthentication();
                app.UseAuthorization();

                app.MapControllers();

                Log.Information("Starting Government.API application");
                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
