using TradeNetProject.Models;

namespace TradeNetProject.Services
{
    public interface IComplianceService
    {
        List<ComplianceRecord> GetAllRecords();
        ComplianceRecord GetRecordById(int id);
        void AddRecord(ComplianceRecord record);
        void UpdateRecord(ComplianceRecord record);
        void DeleteRecord(int id);
        void MarkPassed(int id);
        void MarkFailed(int id);
    }
}
