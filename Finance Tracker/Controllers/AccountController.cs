using Finance_Tracker.Application.DTOs;
using Finance_Tracker.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Finance_Tracker.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            try
            {
                if (request == null || string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                    return BadRequest(new { message = "Invalid input parameters!" });

                await _authService.RegisterAsync(request);
                return Ok(new { message = $"User '{request.Name}' registered successfully!" });
            }
            //TODO: User already exists exception -- custom
            catch (Exception ex)
            {
                // Log exception here
                return StatusCode(500, new { message = "An unexpected error occurred. Please try again later." });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                if (request == null || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                    return BadRequest(new { message = "Invalid input parameters!" });

                string jwtToken = await _authService.LoginAsync(request);

                if (string.IsNullOrEmpty(jwtToken))
                    return Unauthorized(new { message = "Invalid email or password." });

                return Ok(new { token = jwtToken });
            }
            // TODO: Add custom exception handling for invalid credentials if implemented in AuthService
            catch (Exception ex)
            {
                // Log exception here
                return StatusCode(500, new { message = "An unexpected error occurred. Please try again later." });
            }
        }
    }
}
