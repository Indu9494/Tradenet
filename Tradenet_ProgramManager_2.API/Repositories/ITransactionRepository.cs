using Tradenet_ProgramManager_2.API.Models;

namespace Tradenet_ProgramManager_2.API.Repositories
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<Transaction>> GetAll();
        Task<IEnumerable<Transaction>> GetByProgramId(int programId);
        Task<IEnumerable<Transaction>> GetByType(string type);
        Task<Transaction?> GetById(int id);
        Task Add(Transaction transaction);
        Task<decimal> GetTotalByType(string type);
    }
}
