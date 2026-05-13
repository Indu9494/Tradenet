using TradeNetProject.Models;
using TradeNetProject.Repositories.Interfaces;

namespace TradeNetProject.Services
{
    public class TradeOfficerService : ITradeOfficerService
    {
        private readonly ITradeOfficerRepository _repository;

        public TradeOfficerService(ITradeOfficerRepository repository)
        {
            _repository = repository;
        }

        public TradeOfficer GetCurrentOfficer() => _repository.GetCurrentOfficer();

        public TradeOfficer GetOfficerById(int id) => _repository.GetById(id);
    }
}
