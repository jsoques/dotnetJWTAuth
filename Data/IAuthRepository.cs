using System.Collections.Generic;
using System.Threading.Tasks;
using JWTAuth.Models;

namespace JWTAuth.Data
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<int>> Register(User user, string password);
        Task<ServiceResponse<string>> Login(string username, string password);
        Task<bool> UserExists(string username);
    }
}