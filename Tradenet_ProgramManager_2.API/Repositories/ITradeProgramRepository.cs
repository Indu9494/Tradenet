using Tradenet_ProgramManager_2.API.Models;

namespace Tradenet_ProgramManager_2.API.Repositories
{
    public interface ITradeProgramRepository
    {
        Task<IEnumerable<TradeProgram>> GetAll();
        Task<TradeProgram?> GetById(int id);
        Task Add(TradeProgram tradeProgram);
        Task Update(TradeProgram tradeProgram);
        Task Delete(int id);
    }
}
