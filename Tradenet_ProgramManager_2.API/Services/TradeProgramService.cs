using Tradenet_ProgramManager_2.API.Models;
using Tradenet_ProgramManager_2.API.Models.ViewModels;
using Tradenet_ProgramManager_2.API.Repositories;

namespace Tradenet_ProgramManager_2.API.Services
{
    public class TradeProgramService : ITradeProgramService
    {
        private readonly ITradeProgramRepository _repository;
        private readonly ITransactionRepository _transactionRepository;

        public TradeProgramService(ITradeProgramRepository repository, ITransactionRepository transactionRepository)
        {
            _repository = repository;
            _transactionRepository = transactionRepository;
        }

        public async Task<IEnumerable<TradeProgram>> GetAllPrograms()
        {
            return await _repository.GetAll();
        }

        public async Task<TradeProgram?> GetProgramById(int id)
        {
            return await _repository.GetById(id);
        }

        public async Task<bool> AddProgram(TradeProgram tradeProgram)
        {
            if (tradeProgram.Budget < 0)
            {
                return false;
            }

            await _repository.Add(tradeProgram);
            return true;
        }

        public async Task<bool> UpdateProgram(TradeProgram tradeProgram)
        {
            if (tradeProgram.Budget < 0)
            {
                return false;
            }

            await _repository.Update(tradeProgram);
            return true;
        }

        public async Task DeleteProgram(int id)
        {
            await _repository.Delete(id);
        }

        public async Task<DashboardViewModel> GetDashboardData()
        {
            var programs = await _repository.GetAll();
            var totalSales = await _transactionRepository.GetTotalByType("Sale");
            var totalPurchases = await _transactionRepository.GetTotalByType("Purchase");
            var budgetUsed = programs.Sum(p => p.Budget);
            var netBalance = totalSales - totalPurchases;

            var marketHealth = netBalance > 0 ? "Excellent" : netBalance == 0 ? "Good" : "Needs Attention";

            return new DashboardViewModel
            {
                TotalPrograms = programs.Count(),
                BudgetUsed = budgetUsed,
                MarketHealth = marketHealth,
                TotalSales = totalSales,
                TotalPurchases = totalPurchases,
                NetBalance = netBalance
            };
        }

        public async Task<TransactionViewModel> GetTransactionData()
        {
            var salesVolume = await _transactionRepository.GetTotalByType("Sale");
            var purchaseVolume = await _transactionRepository.GetTotalByType("Purchase");
            var recentTransactions = await _transactionRepository.GetAll();

            return new TransactionViewModel
            {
                SalesVolume = salesVolume,
                PurchaseVolume = purchaseVolume,
                RecentTransactions = recentTransactions.Take(20)
            };
        }

        public async Task<bool> HasNonCompliantPrograms()
        {
            var programs = await _repository.GetAll();
            return programs.Any(p => p.Status.Equals("Non-Compliant", StringComparison.OrdinalIgnoreCase));
        }
    }
}
