using Finance_Tracker.Application.DTOs;

namespace Finance_Tracker.Application.Services
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<AuthResponse> RefreshAsync(string refreshToken);
        Task LogoutAsync(string refreshToken);
    }
}