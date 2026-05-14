using Tradenet_ProgramManager_2.API.Core;
using Tradenet_ProgramManager_2.API.Repositories;
using Tradenet_ProgramManager_2.API.Services;

namespace Tradenet_ProgramManager_2.API.Modules.TradeProgram
{
    /// <summary>
    /// Trade Program Module Registration
    /// Handles all trade program related functionality
    /// </summary>
    public class TradeProgramModule : IModuleRegistration
    {
        public string GetModuleName() => "TradeProgram";

        public void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            // Register repositories
            services.AddScoped<ITradeProgramRepository, TradeProgramRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();

            // Register services
            services.AddScoped<ITradeProgramService, TradeProgramService>();
        }
    }
}
