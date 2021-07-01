using System.Collections.Generic;
using System.Threading.Tasks;
using JWTAuth.Dtos.User;
using JWTAuth.Models;

namespace JWTAuth.Services
{
    public interface IUserService
    {
        Task<ServiceResponse<List<GetUserDto>>> GetAllUsers();
        Task<ServiceResponse<GetUserDto>> GetUserById(int id);
        Task<ServiceResponse<List<GetUserDto>>> AddUser(AddUserDto newUser);
        Task<ServiceResponse<GetUserDto>> UpdateUser(UpdateUserDto updatedUser);
        Task<ServiceResponse<List<GetUserDto>>> DeleteUser(int id);
        Task<ServiceResponse<GetUserDto>> AuthenticateUser(AuthenticateUserDto authUser);
        Task<GetUserDto> GetById(int id);
    }

}