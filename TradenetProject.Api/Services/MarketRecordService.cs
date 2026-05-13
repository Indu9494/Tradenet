using TradeNetProject.Models;
using TradeNetProject.Repositories.Interfaces;

namespace TradeNetProject.Services
{
    public class MarketRecordService : IMarketRecordService
    {
        private readonly IMarketRecordRepository _repository;

        public MarketRecordService(IMarketRecordRepository repository)
        {
            _repository = repository;
        }

        public List<MarketRecord> GetAllRecords() => _repository.GetAll();

        public MarketRecord GetRecordById(int id) => _repository.GetById(id);

        public void AddRecord(MarketRecord record) => _repository.Add(record);

        public void UpdateRecord(MarketRecord record) => _repository.Update(record);

        public void DeleteRecord(int id) => _repository.Delete(id);
    }
}
