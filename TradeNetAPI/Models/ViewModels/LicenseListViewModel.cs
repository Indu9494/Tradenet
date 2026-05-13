using TradeNetAPI.Models;

namespace TradeNetAPI.Models.ViewModels
{
    public class LicenseListViewModel
    {
        public List<TradeLicense> AvailableLicenses { get; set; } = new List<TradeLicense>();
        public List<TradeProgram> ActivePrograms { get; set; } = new List<TradeProgram>();
    }
}
