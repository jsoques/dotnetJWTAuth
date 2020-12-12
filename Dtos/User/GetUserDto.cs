using System;
using JWTAuth.Models;

namespace JWTAuth.Dtos.User
{
     public class GetUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public string Password { get; set; }
        public UserStatus Status { get; set; }
        public DateTime DateCreated { get; set; }
        public string ActivateKey { get; set; }
    }
}