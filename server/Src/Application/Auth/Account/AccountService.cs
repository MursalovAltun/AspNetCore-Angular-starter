using System;
using System.Threading.Tasks;
using Application.Auth.Identity;
using Autofac.Extras.RegistrationAttributes.RegistrationAttributes;
using EF.Models.Models;

namespace Application.Auth.Account
{
    [As(typeof(IAccountService))]
    public class AccountService : IAccountService
    {
        private readonly IUserManager _userManager;

        public AccountService(IUserManager userManager)
        {
            _userManager = userManager;
        }

        public async Task<User> Create(AccountCreateRequest request)
        {
            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email,
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded) throw new Exception();

            return user;
        }
    }
}