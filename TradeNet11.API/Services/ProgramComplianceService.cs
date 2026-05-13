using TradeNet11.Interfaces;
using TradeNet11.Models;

namespace TradeNet11.Services
{
    public class ProgramComplianceService : IProgramComplianceService
    {
        private readonly IProgramComplianceRepository _programRepo;

        public ProgramComplianceService(IProgramComplianceRepository programRepo)
        {
            _programRepo = programRepo;
        }

        public async Task<IEnumerable<ProgramCompliance>> GetAllAsync()
        {
            return await _programRepo.GetAllAsync();
        }

        public async Task<ProgramCompliance?> GetByIdAsync(int id)
        {
            return await _programRepo.GetByIdAsync(id);
        }

        public async Task UpdateEligibilityAsync(int id, string status, bool misuseFlag, string? remarks)
        {
            var program = await _programRepo.GetByIdAsync(id);
            if (program is null)
                throw new InvalidOperationException("Program compliance record not found.");

            program.EligibilityStatus = status;
            program.IsMisuseFlag = misuseFlag;
            program.Remarks = remarks;
            program.ReviewedAt = DateTime.UtcNow;

            await _programRepo.UpdateAsync(program);
        }
    }
}
