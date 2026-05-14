using Microsoft.EntityFrameworkCore;
using TradeNetProject.Data;
using TradeNetProject.Repositories.Interfaces;
using TradeNetProject.Repositories;
using TradeNetProject.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null
        )
    ));

// Register repositories and services from the API project
builder.Services.AddScoped<IComplianceRepository, ComplianceRepository>();
builder.Services.AddScoped<ILicenseRepository, LicenseRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
builder.Services.AddScoped<IMarketRecordRepository, MarketRecordRepository>();
builder.Services.AddScoped<ITradeOfficerRepository, TradeOfficerRepository>();

builder.Services.AddScoped<IComplianceService, ComplianceService>();
builder.Services.AddScoped<ILicenseService, LicenseService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddScoped<IMarketRecordService, MarketRecordService>();
builder.Services.AddScoped<ITradeOfficerService, TradeOfficerService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
