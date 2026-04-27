using Finance_Tracker.Infrastructure.Services.Contracts;
using Finance_Tracker.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace Finance_Tracker.Infrastructure.Services
{

    public class PasswordHasherService : IPasswordHasherService
    {
        private readonly IPasswordHasher<User> _hasher;

        public PasswordHasherService(IPasswordHasher<User> hasher)
        {
            _hasher = hasher;
        }

        public string Hash(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be null or empty.", nameof(password));

            return _hasher.HashPassword(new User(), password);
        }

        public bool Verify(string hashedPassword, string providedPassword)
        {
            if (string.IsNullOrWhiteSpace(hashedPassword) || string.IsNullOrWhiteSpace(providedPassword))
                throw new ArgumentException("Password cannot be null or empty.");

            var result = _hasher.VerifyHashedPassword(new User(), hashedPassword, providedPassword);
            return result == PasswordVerificationResult.Success;
        }
    }

}

