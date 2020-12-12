using System;
using JWTAuth.Models;

namespace JWTAuth.Dtos.User
{
     public class AddUserDto
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public UserStatus Status { get; set; }
        public DateTime DateCreated { get; set; }
        public string ActivateKey { get; set; }
    }
}