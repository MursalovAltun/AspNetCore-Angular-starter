using AutoMapper;
using EF.Models.Models;

namespace Application.Auth.Account
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
        }
    }
}