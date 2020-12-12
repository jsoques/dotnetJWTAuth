
using AutoMapper;
using JWTAuth.Dtos.User;
using JWTAuth.Models;


namespace JWTAuth
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, GetUserDto>();
            CreateMap<AddUserDto, User>();
        }

    }
}