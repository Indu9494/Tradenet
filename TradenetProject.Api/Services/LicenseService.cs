using TradeNetProject.Models;
using TradeNetProject.Repositories.Interfaces;

namespace TradeNetProject.Services
{
    public class LicenseService : ILicenseService
    {
        private readonly ILicenseRepository _repository;

        public LicenseService(ILicenseRepository repository)
        {
            _repository = repository;
        }

        public List<TradeLicense> GetAllLicenses() => _repository.GetAll();

        public TradeLicense GetLicenseById(int id) => _repository.GetById(id);

        public void AddLicense(TradeLicense license) => _repository.Add(license);

        public void UpdateLicense(TradeLicense license) => _repository.Update(license);

        public void DeleteLicense(int id) => _repository.Delete(id);

        public void ApproveLicense(int id)
        {
            var license = _repository.GetById(id);
            if (license != null)
            {
                license.Status = "Approved";
                _repository.Update(license);
            }
        }

        public void RejectLicense(int id)
        {
            var license = _repository.GetById(id);
            if (license != null)
            {
                license.Status = "Rejected";
                _repository.Update(license);
            }
        }
    }
}
