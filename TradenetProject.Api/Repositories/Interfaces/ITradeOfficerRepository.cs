using TradeNetProject.Models;

namespace TradeNetProject.Repositories.Interfaces
{
    public interface ITradeOfficerRepository : IRepository<TradeOfficer>
    {
        TradeOfficer GetCurrentOfficer();
    }
}
