using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Autofac.Extras.RegistrationAttributes.RegistrationAttributes;
using EF.Models.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.Auth.Identity
{
    [As(typeof(IUserManager))]
    public class UserManager : UserManager<User>, IUserManager
    {
        public UserManager(IUserStore<User> store, IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<User> passwordHasher, IEnumerable<IUserValidator<User>> userValidators,
            IEnumerable<IPasswordValidator<User>> passwordValidators, ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<User>> logger) : base(store,
            optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services,
            logger)
        {
        }

        public new Guid? GetUserId(ClaimsPrincipal principal)
        {
            var userId = base.GetUserId(principal);

            return userId != null
                ? Guid.Parse(userId)
                : (Guid?) null;
        }

        public async Task<User> FindByIdAsync(Guid userId)
        {
            return await FindByIdAsync(userId.ToString());
        }
    }
}