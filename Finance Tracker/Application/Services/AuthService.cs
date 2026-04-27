using Finance_Tracker.Application.DTOs;
using Finance_Tracker.Infrastructure;
using Finance_Tracker.Infrastructure.Contracts;
using Finance_Tracker.Infrastructure.Services;
using Finance_Tracker.Infrastructure.Services.Contracts;
using Finance_Tracker.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Finance_Tracker.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasherService _passwordHasher;
        private readonly ITokenService _token;

        public AuthService(IUserRepository userRepository, IPasswordHasherService passwordHasher, ITokenService token)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _token = token;
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null)
                throw new Exception("Invalid email or password");

            var isValid = _passwordHasher.Verify(user.PasswordHash, request.Password);

            if (!isValid)
                throw new Exception("Invalid email or password");

            var accessToken = _token.GenerateToken(user);

            var refreshToken = _token.GenerateRefreshToken();
            user.RefreshTokens.Add(refreshToken);

            await _userRepository.SaveChangesAsync();

            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token
            };
        }

        public async Task RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);

            if (existingUser != null)
                throw new Exception("User already exists. Please use a different email.");

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = _passwordHasher.Hash(request.Password)
            };

            await _userRepository.AddUserAsync(user);
            await _userRepository.SaveChangesAsync();
        }

        public async Task<AuthResponse> RefreshAsync(string token)
        {
            var user = await _userRepository.GetUserByRefreshTokenAsync(token);
            var refreshToken = user?.RefreshTokens.FirstOrDefault(x => x.Token == token);

            if (refreshToken == null || refreshToken.IsRevoked || refreshToken.ExpiresAt < DateTime.UtcNow)
                throw new Exception("Invalid refresh token");

            refreshToken.IsRevoked = true;
            refreshToken.RevokedAt = DateTime.UtcNow;

            var newRefreshToken = _token.GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);

            var accessToken = _token.GenerateToken(user);

            await _userRepository.SaveChangesAsync();

            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken.Token
            };
        }

        public async Task LogoutAsync(string token)
        {
            var user = await _userRepository.GetUserByRefreshTokenAsync(token);
            var refreshToken = user?.RefreshTokens.FirstOrDefault(x => x.Token == token);

            if (refreshToken != null)
            {
                refreshToken.IsRevoked = true;
                refreshToken.RevokedAt = DateTime.UtcNow; //To track when the token was revoked
                await _userRepository.SaveChangesAsync();
            }
        }
    }
}
