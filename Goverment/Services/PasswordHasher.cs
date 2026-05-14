using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace Goverment.Services
{
    public class PasswordHasher
    {
        public static (string hash, string salt) HashPassword(string password)
        {
            // Generate a 128-bit salt
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Hash password with PBKDF2
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            string saltString = Convert.ToBase64String(salt);

            return (hashed, saltString);
        }

        public static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            byte[] salt = Convert.FromBase64String(storedSalt);

            // Hash the provided password with the stored salt
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            // Use constant-time comparison to prevent timing attacks
            return CryptographicOperations.FixedTimeEquals(
                Convert.FromBase64String(hashed),
                Convert.FromBase64String(storedHash));
        }

        public static bool ValidatePasswordStrength(string password)
        {
            if (password.Length < 6) return false;
            if (!password.Any(char.IsUpper)) return false;
            if (!password.Any(char.IsLower)) return false;
            if (!password.Any(char.IsDigit)) return false;

            return true;
        }
    }
}
