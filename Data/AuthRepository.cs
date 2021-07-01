using System;
using System.Security.Claims;
using System.Threading.Tasks;
using JWTAuth.JWT;
using JWTAuth.Models;
using JWTAuth.Utils;
using Microsoft.EntityFrameworkCore;

namespace JWTAuth.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;

        private readonly CustomPasswordHasher customPasswordHasher;

        private readonly IJwtAuthManager _jwtAuthManager;

        public AuthRepository(DataContext context, IJwtAuthManager jwtAuthManager)
        {
            _context = context;
            customPasswordHasher = new CustomPasswordHasher();
            _jwtAuthManager = jwtAuthManager;
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            ServiceResponse<int> response = new ServiceResponse<int>();

            if (await UserExists(user.Name))
            {
                response.Success = false;
                response.Message = "User already exists.";
                return response;
            }

            user.PasswordHash = customPasswordHasher.HashPassword(password);
            user.DateCreated = DateTime.Now.ToUniversalTime().ToString();
            Guid g = Guid.NewGuid();
            user.ActivateKey = g.ToString().Replace("-", "");
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            response.Data = user.Id;
            return response;
        }

        public async Task<bool> UserExists(string username)
        {
            if (await _context.Users.AnyAsync(x => x.Name.ToLower() == username.ToLower()))
            {
                return true;
            }
            return false;
        }

        public async Task<ServiceResponse<string>> Login(string username, string password)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            User user = await _context.Users.FirstOrDefaultAsync(x => x.Name.ToLower().Equals(username.ToLower()));
            if (user == null)
            {
                response.Success = false;
                response.Message = "Wrong username or password.";
            }
            else if (!VerifyPasswordHash(password, user.PasswordHash))
            {
                response.Success = false;
                response.Message = "Wrong username or password.";
            }
            else
            {
                response.Data = CreateToken(user);
            }

            return response;
        }

        //!customPasswordHasher.VerifyPassword(user.PasswordHash, authUser.Password)
        private bool VerifyPasswordHash(string password, string hash)
        {
            if (!customPasswordHasher.VerifyPassword(hash, password))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private string CreateToken(User user)
        {
            _jwtAuthManager.RemoveRefreshTokenByUserName(user.Name);
            var claims = new[]
            {
                    new Claim("Email", user.Name),
                    new Claim("Role", "role"),
                    new Claim("Activated", user.Status == UserStatus.Enabled ? "Yes": "No"),
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Name", user.Name)
                };
            var jwtResult = _jwtAuthManager.GenerateTokens(user.Name, claims, DateTime.Now);
            return jwtResult.AccessToken;
        }
    }
}