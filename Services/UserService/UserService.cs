
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AutoMapper;
using JWTAuth.Data;
using JWTAuth.Dtos.User;
using JWTAuth.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using BC = BCrypt.Net.BCrypt;
using JWTAuth.Utils;

namespace JWTAuth.Services
{
    public class UserService : IUserService
    {

        private readonly IMapper _mapper;
        private readonly DataContext _context;

        private readonly CustomPasswordHasher customPasswordHasher;

        public UserService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
            customPasswordHasher = new CustomPasswordHasher();
        }

        public async Task<ServiceResponse<List<GetUserDto>>> AddUser(AddUserDto newUser)
        {
            ServiceResponse<List<GetUserDto>> serviceResponse = new ServiceResponse<List<GetUserDto>>();

            var user = _mapper.Map<User>(newUser);
            user.DateCreated = DateTime.Now.ToUniversalTime().ToString();
            Guid g = Guid.NewGuid();
            user.ActivateKey = g.ToString().Replace("-", "");
            //user.Password = BC.HashPassword(newUser.Password);
            user.PasswordHash = customPasswordHasher.HashPassword(newUser.Password);
            user.Status = UserStatus.Disabled;

            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                serviceResponse.Data = (_context.Users.Select(u => _mapper.Map<GetUserDto>(u))).ToList();
            }
            catch (System.Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.ToString();
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetUserDto>>> GetAllUsers()
        {
            ServiceResponse<List<GetUserDto>> serviceResponse = new ServiceResponse<List<GetUserDto>>();
            List<User> dbCharacters = await _context.Users.ToListAsync();
            serviceResponse.Data = (dbCharacters.Select(u => _mapper.Map<GetUserDto>(u))).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetUserDto>> GetUserById(int id)
        {
            ServiceResponse<GetUserDto> serviceResponse = new ServiceResponse<GetUserDto>();
            serviceResponse.Data = _mapper.Map<GetUserDto>(await _context.Users.FirstOrDefaultAsync(u => u.Id == id));
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetUserDto>> UpdateUser(UpdateUserDto updatedUser)
        {
            ServiceResponse<GetUserDto> serviceResponse = new ServiceResponse<GetUserDto>();

            try
            {
                User user = _context.Users.FirstOrDefault(u => u.Id == updatedUser.Id);

                if (!String.IsNullOrEmpty(updatedUser.Name) && !String.IsNullOrWhiteSpace(updatedUser.Name))
                {
                    user.Name = updatedUser.Name;
                }

                if (!String.IsNullOrEmpty(updatedUser.Password) && !String.IsNullOrWhiteSpace(updatedUser.Password))
                {
                    //user.Password = BC.HashPassword(updatedUser.Password);
                    user.PasswordHash = customPasswordHasher.HashPassword(updatedUser.Password);
                }

                if (!String.IsNullOrEmpty(updatedUser.ActivateKey))
                {
                    user.ActivateKey = updatedUser.ActivateKey;
                }


                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<GetUserDto>(user);
            }
            catch (System.Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }


            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetUserDto>>> DeleteUser(int id)
        {
            ServiceResponse<List<GetUserDto>> serviceResponse = new ServiceResponse<List<GetUserDto>>();

            try
            {
                User user = _context.Users.First(u => u.Id == id);
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                serviceResponse.Data = (_context.Users.Select(u => _mapper.Map<GetUserDto>(u))).ToList();
            }
            catch (System.Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }


            return serviceResponse;
        }

        public async Task<ServiceResponse<GetUserDto>> AuthenticateUser(AuthenticateUserDto authUser)
        {
            ServiceResponse<GetUserDto> serviceResponse = new ServiceResponse<GetUserDto>();

            try
            {
                User user = await (from u in _context.Users
                                   where u.Name == authUser.Email
                                   select u).FirstOrDefaultAsync();

                //if (user == null || !BC.Verify(authUser.Password, user.Password))
                if (user == null || !customPasswordHasher.VerifyPassword(user.PasswordHash, authUser.Password))
                {
                    serviceResponse.Data = null;
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Wrong user and/or password";
                }
                else
                {
                    serviceResponse.Data = _mapper.Map<GetUserDto>(user);
                    serviceResponse.Message = "JWT Token goes here!";
                }
            }
            catch (System.Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        private string xHashPassword(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");

            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            Console.WriteLine($"Hashed: {hashed}");

            return Convert.ToBase64String(salt) + "|" + hashed;
        }
    }

}