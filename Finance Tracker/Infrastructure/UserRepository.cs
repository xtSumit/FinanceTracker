using Finance_Tracker.Infrastructure.Contracts;
using Finance_Tracker.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Finance_Tracker.Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly FinanceTrackerDbContext _context;

        public UserRepository(FinanceTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(x => x.RefreshTokens)
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User> GetUserByRefreshTokenAsync(string token)
        {
            return await _context.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
