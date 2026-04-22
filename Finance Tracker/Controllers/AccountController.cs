using Finance_Tracker.Application.DTOs;
using Finance_Tracker.Application.Services;
using Microsoft.AspNetCore.Mvc;

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
            await _authService.RegisterAsync(request);

            return Ok(new { message = "User registered successfully" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            string jwtToken =  await _authService.LoginAsync(request);
            

            return Ok(new
            {
                token = jwtToken
            });
        }
    }
}
