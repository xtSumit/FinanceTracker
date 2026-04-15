using Microsoft.AspNetCore.Identity;

namespace Finance_Tracker.Infrastructure.Services
{

    public class PasswordHasherService : IPasswordHasherService
    {
        private readonly IPasswordHasher<object> _hasher;

        public PasswordHasherService(IPasswordHasher<object> hasher)
        {
            _hasher = hasher;
        }

        public string Hash(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be null or empty.", nameof(password));

            return _hasher.HashPassword(null, password);
        }

        public bool Verify(string hashedPassword, string providedPassword)
        {
            if (string.IsNullOrWhiteSpace(hashedPassword) || string.IsNullOrWhiteSpace(providedPassword))
                throw new ArgumentException("Password cannot be null or empty.");

            var result = _hasher.VerifyHashedPassword(null, hashedPassword, providedPassword);
            return result == PasswordVerificationResult.Success;
        }
    }

}

