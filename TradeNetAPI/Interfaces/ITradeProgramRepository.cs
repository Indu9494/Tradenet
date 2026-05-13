using TradeNetAPI.Models;

namespace TradeNetAPI.Interfaces;

public interface ITradeProgramRepository : IRepository<TradeProgram>
{
    Task<IEnumerable<TradeProgram>> GetActiveProgramsAsync();
}