using System;
using JWTAuth.Models;

namespace JWTAuth.Dtos.User
{
    public class AuthenticateUserDto
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
}