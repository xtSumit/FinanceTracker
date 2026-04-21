using Finance_Tracker.Models.Entities;

namespace Finance_Tracker.Infrastructure.Services.Contracts
{
    public interface ITokenService
    {
        public string GenerateToken(User user);
    }
}
