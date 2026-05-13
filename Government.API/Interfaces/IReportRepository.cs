using Goverment.Models;

namespace Goverment.Interfaces
{
    public interface IReportRepository
    {
        Task<IEnumerable<Report>> GetAllReportsAsync();
        Task<Report?> GetReportByIdAsync(int reportId);
        Task<IEnumerable<Report>> GetReportsByScopeAsync(string scope);
        Task<IEnumerable<Report>> GetReportsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<Report> CreateReportAsync(Report report);
        Task<Report> UpdateReportAsync(Report report);
        Task<bool> DeleteReportAsync(int reportId);
    }
}
