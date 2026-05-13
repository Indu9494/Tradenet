using TradeNetProject.Models;

namespace TradeNetProject.Services
{
    public interface IMarketRecordService
    {
        List<MarketRecord> GetAllRecords();
        MarketRecord GetRecordById(int id);
        void AddRecord(MarketRecord record);
        void UpdateRecord(MarketRecord record);
        void DeleteRecord(int id);
    }
}
