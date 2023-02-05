using Application.Auth.Account.DTO;
using AutoMapper;
using EF.Models.Models;

namespace Application.Auth.Account.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();
    }
}