using TradeNetAPI.Models;

namespace TradeNetAPI.Services
{
    public class InMemoryUserStore
    {
        private static List<User> _users = new List<User>();
        private static List<Business> _businesses = new List<Business>();
        private static int _nextUserId = 1;
        private static int _nextBusinessId = 1;

        public User? GetUserByEmail(string email)
        {
            return _users.FirstOrDefault(u => u.Email == email);
        }

        public User? GetUserById(int userId)
        {
            return _users.FirstOrDefault(u => u.UserID == userId);
        }

        public User AddUser(string name, string email)
        {
            var user = new User
            {
                UserID = _nextUserId++,
                Name = name,
                Email = email,
                Role = "Business",
                Status = "Active",
                Phone = ""
            };
            _users.Add(user);
            return user;
        }

        public Business? GetBusinessByUserId(int userId)
        {
            return _businesses.FirstOrDefault(b => b.UserID == userId);
        }

        public Business AddBusiness(int userId, string businessName)
        {
            var business = new Business
            {
                BusinessID = _nextBusinessId++,
                UserID = userId,
                Name = businessName,
                Type = "Trader",
                Address = "",
                ContactInfo = "",
                Status = "Pending",
                RegistrationDate = DateTime.Now,
                ComplianceStatus = "Compliant"
            };
            _businesses.Add(business);
            return business;
        }
    }
}
