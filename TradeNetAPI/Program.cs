using TradeNetAPI.Data;
using TradeNetAPI.Interfaces;
using TradeNetAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// 1. Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog(); // Set Serilog as the logging provider

// Add services to the container
builder.Services.AddControllers();

// Add Health Checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<TradeNetDbContext>();

// 2. Add Swagger/OpenAPI Services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowPortal", policy =>
    {
        policy.WithOrigins("https://localhost:7088")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add DbContext
builder.Services.AddDbContext<TradeNetDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});

// Register repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBusinessRepository, BusinessRepository>();
builder.Services.AddScoped<IBusinessDocumentRepository, BusinessDocumentRepository>();
builder.Services.AddScoped<ITradeLicenseRepository, TradeLicenseRepository>();
builder.Services.AddScoped<ILicenseDocumentRepository, LicenseDocumentRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ITradeProgramRepository, TradeProgramRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IComplianceRecordRepository, ComplianceRecordRepository>();
builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();
builder.Services.AddScoped<IMarketRecordRepository, MarketRecordRepository>();

var app = builder.Build();

// Database initialization
using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<TradeNetDbContext>();
        context.Database.EnsureCreated();
        Log.Information("Database initialized successfully");
    }
    catch (Exception ex)
    {
        Log.Error(ex, "An error occurred during database initialization");
        throw;
    }
}

// Configure HTTP Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c =>
    {
        c.RouteTemplate = "swagger/{documentName}/swagger.json";
    });
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TradeNet API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseCors("AllowPortal");
app.UseRouting();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();