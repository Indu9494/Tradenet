using Tradenet_ProgramManager_2.API.Data;
using Tradenet_ProgramManager_2.API.Models;

namespace Tradenet_ProgramManager_2.API.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context, bool forceReseed = false)
        {
            context.Database.EnsureCreated();

            // If force reseed, clear existing data
            if (forceReseed)
            {
                context.Transactions.RemoveRange(context.Transactions);
                context.TradePrograms.RemoveRange(context.TradePrograms);
                context.SaveChanges();
            }

            // Check if data already exists
            if (context.TradePrograms.Any() && !forceReseed)
            {
                return;   // DB has been seeded
            }

            // Add Trade Programs
            var programs = new TradeProgram[]
            {
                new TradeProgram
                {
                    Title = "Global Export Initiative",
                    Budget = 250000.00m,
                    Status = "Active"
                },
                new TradeProgram
                {
                    Title = "Digital Commerce Platform",
                    Budget = 180000.00m,
                    Status = "Active"
                },
                new TradeProgram
                {
                    Title = "Supply Chain Optimization",
                    Budget = 95000.00m,
                    Status = "Non-Compliant"
                },
                new TradeProgram
                {
                    Title = "Market Expansion Asia-Pacific",
                    Budget = 320000.00m,
                    Status = "Pending"
                },
                new TradeProgram
                {
                    Title = "Cross-Border Trade Agreement",
                    Budget = 150000.00m,
                    Status = "Completed"
                }
            };

            context.TradePrograms.AddRange(programs);
            context.SaveChanges();

            // Add Transactions
            var transactions = new Transaction[]
            {
                // Program 1 - Global Export Initiative
                new Transaction
                {
                    ProgramId = 1,
                    Type = "Sale",
                    Amount = 45000.00m,
                    Date = DateTime.Now.AddDays(-15),
                    Description = "Export of electronics to European markets"
                },
                new Transaction
                {
                    ProgramId = 1,
                    Type = "Purchase",
                    Amount = 28000.00m,
                    Date = DateTime.Now.AddDays(-12),
                    Description = "Raw materials procurement from suppliers"
                },
                new Transaction
                {
                    ProgramId = 1,
                    Type = "Sale",
                    Amount = 67000.00m,
                    Date = DateTime.Now.AddDays(-8),
                    Description = "Bulk order shipment to North America"
                },

                // Program 2 - Digital Commerce Platform
                new Transaction
                {
                    ProgramId = 2,
                    Type = "Purchase",
                    Amount = 35000.00m,
                    Date = DateTime.Now.AddDays(-20),
                    Description = "Software licensing and cloud infrastructure"
                },
                new Transaction
                {
                    ProgramId = 2,
                    Type = "Sale",
                    Amount = 89000.00m,
                    Date = DateTime.Now.AddDays(-10),
                    Description = "Digital product sales via online platform"
                },
                new Transaction
                {
                    ProgramId = 2,
                    Type = "Sale",
                    Amount = 52000.00m,
                    Date = DateTime.Now.AddDays(-5),
                    Description = "Subscription service revenue Q1"
                },

                // Program 3 - Supply Chain Optimization
                new Transaction
                {
                    ProgramId = 3,
                    Type = "Purchase",
                    Amount = 42000.00m,
                    Date = DateTime.Now.AddDays(-18),
                    Description = "Logistics and warehousing costs"
                },
                new Transaction
                {
                    ProgramId = 3,
                    Type = "Purchase",
                    Amount = 31000.00m,
                    Date = DateTime.Now.AddDays(-7),
                    Description = "Transportation and shipping expenses"
                },

                // Program 4 - Market Expansion Asia-Pacific
                new Transaction
                {
                    ProgramId = 4,
                    Type = "Sale",
                    Amount = 125000.00m,
                    Date = DateTime.Now.AddDays(-25),
                    Description = "Major contract with Singapore distributor"
                },
                new Transaction
                {
                    ProgramId = 4,
                    Type = "Purchase",
                    Amount = 58000.00m,
                    Date = DateTime.Now.AddDays(-14),
                    Description = "Market research and regulatory compliance"
                },
                new Transaction
                {
                    ProgramId = 4,
                    Type = "Sale",
                    Amount = 78000.00m,
                    Date = DateTime.Now.AddDays(-6),
                    Description = "Product sales in Japanese market"
                },

                // Program 5 - Cross-Border Trade Agreement
                new Transaction
                {
                    ProgramId = 5,
                    Type = "Sale",
                    Amount = 95000.00m,
                    Date = DateTime.Now.AddDays(-30),
                    Description = "Trade agreement facilitation fees"
                },
                new Transaction
                {
                    ProgramId = 5,
                    Type = "Purchase",
                    Amount = 44000.00m,
                    Date = DateTime.Now.AddDays(-22),
                    Description = "Legal and consulting services"
                },
                new Transaction
                {
                    ProgramId = 5,
                    Type = "Sale",
                    Amount = 61000.00m,
                    Date = DateTime.Now.AddDays(-3),
                    Description = "Cross-border transaction fees collected"
                }
            };

            context.Transactions.AddRange(transactions);
            context.SaveChanges();
        }
    }
}
