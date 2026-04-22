using Finance_Tracker.Application.DTOs;

namespace Finance_Tracker.Application.Services
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterRequest request);
        Task<string> LoginAsync(LoginRequest request);
    }
}