using TradeNet11.Interfaces;
using TradeNet11.Models;

namespace TradeNet11.Services
{
    public class AuditService : IAuditService
    {
        private readonly IAuditRepository _auditRepo;

        public AuditService(IAuditRepository auditRepo)
        {
            _auditRepo = auditRepo;
        }

        public async Task<IEnumerable<Audit>> GetAllAuditsAsync()
        {
            return await _auditRepo.GetAllAsync();
        }

        public async Task<Audit?> GetAuditDetailAsync(int id)
        {
            return await _auditRepo.GetByIdAsync(id);
        }

        public async Task StartAuditAsync(int id)
        {
            var audit = await _auditRepo.GetByIdAsync(id);
            if (audit is null)
                throw new InvalidOperationException("Audit not found.");

            if (audit.Status != "Scheduled")
                throw new InvalidOperationException("Only scheduled audits can be started.");

            audit.Status = "InProgress";
            await _auditRepo.UpdateAsync(audit);
        }

        public async Task CompleteAuditAsync(int id, string findings)
        {
            var audit = await _auditRepo.GetByIdAsync(id);
            if (audit is null)
                throw new InvalidOperationException("Audit not found.");

            audit.Status = "Completed";
            audit.Findings = findings;
            audit.CompletedDate = DateTime.UtcNow;
            await _auditRepo.UpdateAsync(audit);
        }

        public async Task CreateAuditAsync(Audit audit)
        {
            await _auditRepo.AddAsync(audit);
        }
    }
}
