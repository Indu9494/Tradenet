using TradeNetProject.Models;
using TradeNetProject.Repositories.Interfaces;

namespace TradeNetProject.Services
{
    public class ComplianceService : IComplianceService
    {
        private readonly IComplianceRepository _repository;

        public ComplianceService(IComplianceRepository repository)
        {
            _repository = repository;
        }

        public List<ComplianceRecord> GetAllRecords() => _repository.GetAll();

        public ComplianceRecord GetRecordById(int id) => _repository.GetById(id);

        public void AddRecord(ComplianceRecord record) => _repository.Add(record);

        public void UpdateRecord(ComplianceRecord record) => _repository.Update(record);

        public void DeleteRecord(int id) => _repository.Delete(id);

        public void MarkPassed(int id)
        {
            var record = _repository.GetById(id);
            if (record != null)
            {
                record.Result = "Passed";
                record.InspectedBy = "Trade Officer";
                _repository.Update(record);
            }
        }

        public void MarkFailed(int id)
        {
            var record = _repository.GetById(id);
            if (record != null)
            {
                record.Result = "Failed";
                record.InspectedBy = "Trade Officer";
                _repository.Update(record);
            }
        }
    }
}
