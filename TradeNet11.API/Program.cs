using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerGen;
using TradeNet11.Data;
using TradeNet11.Interfaces;
using TradeNet11.Repositories;
using TradeNet11.Services;

namespace TradeNet11.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllers();

            // Swagger/OpenAPI
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new()
                {
                    Title = "TradeNet11 API",
                    Version = "v1",
                    Description = "RESTful API for Trade Compliance Management System",
                    Contact = new()
                    {
                        Name = "TradeNet Team",
                    }
                });
            });
            builder.Services.AddOpenApi();

            // EF Core
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Repositories
            builder.Services.AddScoped<IComplianceOfficerRepository, ComplianceOfficerRepository>();
            builder.Services.AddScoped<IComplianceNotificationRepository, ComplianceNotificationRepository>();
            builder.Services.AddScoped<IComplianceCaseRepository, ComplianceCaseRepository>();
            builder.Services.AddScoped<IAuditRepository, AuditRepository>();
            builder.Services.AddScoped<IProgramComplianceRepository, ProgramComplianceRepository>();
            builder.Services.AddScoped<IComplianceRecordRepository, ComplianceRecordRepository>();

            // Services
            builder.Services.AddScoped<IComplianceNotificationService, ComplianceNotificationService>();
            builder.Services.AddScoped<IComplianceCaseService, ComplianceCaseService>();
            builder.Services.AddScoped<IAuditService, AuditService>();
            builder.Services.AddScoped<IProgramComplianceService, ProgramComplianceService>();

            // CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            // Configure Kestrel server options
            builder.WebHost.ConfigureKestrel(serverOptions =>
            {
                serverOptions.ListenLocalhost(5212, listenOptions =>
                {
                    listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1AndHttp2;
                });
                serverOptions.ListenLocalhost(7101, listenOptions =>
                {
                    listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1AndHttp2;
                    listenOptions.UseHttps();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "TradeNet11 API v1");
                    options.RoutePrefix = string.Empty;
                });
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
