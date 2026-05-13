using TradeNetAPI.Models;

namespace TradeNetAPI.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetUserByIdWithBusinessesAsync(int userId);
}