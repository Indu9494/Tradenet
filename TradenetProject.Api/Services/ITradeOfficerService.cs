using TradeNetProject.Models;

namespace TradeNetProject.Services
{
    public interface ITradeOfficerService
    {
        TradeOfficer GetCurrentOfficer();
        TradeOfficer GetOfficerById(int id);
    }
}
