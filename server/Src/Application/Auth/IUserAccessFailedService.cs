using System;
using System.Threading.Tasks;
using EF.Models.Models;

namespace Application.Auth
{
    public interface IUserAccessFailedService
    {
        Task RegisterFailedAttempt(User user);
        
        Task<DateTime?> IsCaptchaRequired(User user);
    }
}