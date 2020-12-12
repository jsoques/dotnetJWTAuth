using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using JWTAuth.Models;
using JWTAuth.Services;
using System.Threading.Tasks;
using JWTAuth.Dtos.User;
using JWTAuth.JWT;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JWTAuth.Controllers
{

    [ApiController]
    [Authorize]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtAuthManager _jwtAuthManager;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger, IJwtAuthManager jwtAuthManager)
        {
            _userService = userService;
            _logger = logger;
            _jwtAuthManager = jwtAuthManager;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _userService.GetAllUsers());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingle(int id)
        {
            return Ok(await _userService.GetUserById(id));
        }

        [AllowAnonymous]
        [HttpPost("")]
        public async Task<IActionResult> AddUser(AddUserDto newUser)
        {
            ServiceResponse<List<GetUserDto>> response = await _userService.AddUser(newUser);
            if (response.Data == null)
            {
                return BadRequest(response);
            }
            else
            {
                return Ok(response);
            }
        }

        [HttpPut("")]
        public async Task<IActionResult> UpdateUser(UpdateUserDto updatedUser)
        {
            ServiceResponse<GetUserDto> response = await _userService.UpdateUser(updatedUser);

            if (response.Data == null)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResponse<List<GetUserDto>> response = await _userService.DeleteUser(id);
            if (response.Data == null)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> AuthenticateUser([FromBody] AuthenticateUserDto authUser)
        {
            ServiceResponse<GetUserDto> response = await _userService.AuthenticateUser(authUser);
            if (response.Data == null)
            {
                return Unauthorized(response);
            }
            else
            {
                ServiceResponse<JwtTokens> respjwtTokens = new ServiceResponse<JwtTokens>();

                _jwtAuthManager.RemoveRefreshTokenByUserName(authUser.Name);
                var claims = new[]
                {
                    new Claim("Email", authUser.Name),
                    new Claim("Role", "role"),
                    new Claim("Activated", response.Data.Status == UserStatus.Disabled ? "False": "True")
                };
                var jwtResult = _jwtAuthManager.GenerateTokens(authUser.Name, claims, DateTime.Now);
                _logger.LogInformation($"User [{authUser.Name}] logged in the system.");
                JwtTokens jwtTokens = new JwtTokens();
                jwtTokens.AccessToken = jwtResult.AccessToken.ToString();
                jwtTokens.RefreshToken = jwtResult.RefreshToken.TokenString;
                respjwtTokens.Data = jwtTokens;
                respjwtTokens.Message = "Validated";
                return Ok(jwtResult);
            }
        }

    }

    class JwtTokens

    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}