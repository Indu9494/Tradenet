using TradeNetProject.Models;

namespace TradeNetProject.Services
{
    public interface ILicenseService
    {
        List<TradeLicense> GetAllLicenses();
        TradeLicense GetLicenseById(int id);
        void AddLicense(TradeLicense license);
        void UpdateLicense(TradeLicense license);
        void DeleteLicense(int id);
        void ApproveLicense(int id);
        void RejectLicense(int id);
    }
}
