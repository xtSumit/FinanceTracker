using Finance_Tracker.Models.Entities;

namespace Finance_Tracker.Infrastructure.Contracts
{
    public interface IUserRepository
    {
        Task<User> GetByEmailAsync(string email);
        Task<User> GetUserByRefreshTokenAsync(string token);
        Task AddUserAsync(User user);
        Task SaveChangesAsync();
    }
}
