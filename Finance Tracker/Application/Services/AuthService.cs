using Finance_Tracker.Application.DTOs;
using Finance_Tracker.Infrastructure;
using Finance_Tracker.Infrastructure.Services.Contracts;
using Finance_Tracker.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Finance_Tracker.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly FinanceTrackerDbContext _context;
        private readonly IPasswordHasherService _passwordHasher;
        private readonly ITokenService _token;

        public AuthService(FinanceTrackerDbContext context, IPasswordHasherService passwordHasher, ITokenService token)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _token = token;
        }

        public async Task<string> LoginAsync(LoginRequest request)
        {
            var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Email == request.Email);

            if (user == null)
                throw new Exception("Invalid email or password");

            var isValid = _passwordHasher.Verify(user.PasswordHash, request.Password);

            if (!isValid)
                throw new Exception("Invalid email or password");

            return _token.GenerateToken(user);
        }

        public async Task RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);

            if (existingUser != null)
                throw new Exception("User already exists. Please use a different email.");

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = _passwordHasher.Hash(request.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
