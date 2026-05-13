using TradeNet11.Interfaces;
using TradeNet11.Models;

namespace TradeNet11.Services
{
    public class ComplianceCaseService : IComplianceCaseService
    {
        private readonly IComplianceCaseRepository _caseRepo;
        private readonly IComplianceRecordRepository _recordRepo;

        public ComplianceCaseService(IComplianceCaseRepository caseRepo, IComplianceRecordRepository recordRepo)
        {
            _caseRepo = caseRepo;
            _recordRepo = recordRepo;
        }

        public async Task<IEnumerable<ComplianceCase>> GetAllCasesAsync()
        {
            return await _caseRepo.GetAllAsync();
        }

        public async Task<ComplianceCase?> GetCaseDetailAsync(int id)
        {
            return await _caseRepo.GetByIdAsync(id);
        }

        public async Task<IEnumerable<ComplianceRecord>> GetCaseRecordsAsync(int caseId)
        {
            return await _recordRepo.GetByCaseIdAsync(caseId);
        }

        public async Task UpdateCaseStatusAsync(int caseId, string newStatus, int officerId, string? remarks)
        {
            var complianceCase = await _caseRepo.GetByIdAsync(caseId);
            if (complianceCase is null)
                throw new InvalidOperationException("Case not found.");

            var oldStatus = complianceCase.Status;
            complianceCase.Status = newStatus;

            if (newStatus == "Compliant" || newStatus == "NonCompliant")
                complianceCase.ResolvedAt = DateTime.UtcNow;

            await _caseRepo.UpdateAsync(complianceCase);

            await _recordRepo.AddAsync(new ComplianceRecord
            {
                ComplianceCaseId = caseId,
                ActionTaken = "StatusChange",
                Details = $"Status changed from {oldStatus} to {newStatus}",
                OfficerRemarks = remarks,
                OfficerId = officerId,
                Timestamp = DateTime.UtcNow
            });
        }
    }
}
