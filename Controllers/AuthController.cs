using System.Threading.Tasks;
using JWTAuth.Data;
using JWTAuth.Dtos.User;
using JWTAuth.Models;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuth.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly IAuthRepository _authRepo;
        public AuthController(IAuthRepository authRepository)
        {
            _authRepo = authRepository;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(AddUserDto request)
        {
            ServiceResponse<int> response = await _authRepo.Register(
                   new User { Name = request.Name }, request.Password);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(AuthenticateUserDto request)
        {
            ServiceResponse<string> response = await _authRepo.Login(
                request.Email, request.Password);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

    }
}