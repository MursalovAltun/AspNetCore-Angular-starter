using System;
using System.Security.Claims;
using System.Threading.Tasks;
using EF.Models.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Auth.Identity
{
    public interface IUserManager
    {
        Task<IdentityResult> CreateAsync(User user, string password);
        Task<User> FindByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(User user, string password);
        Guid? GetUserId(ClaimsPrincipal principal);
        Task<User> FindByIdAsync(Guid userId);
    }
}