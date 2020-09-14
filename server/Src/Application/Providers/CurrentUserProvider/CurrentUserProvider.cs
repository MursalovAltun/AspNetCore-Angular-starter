using System.Threading.Tasks;
using Application.Auth.Identity;
using Autofac.Extras.RegistrationAttributes.RegistrationAttributes;
using EF.Models.Models;
using Microsoft.AspNetCore.Http;

namespace Application.Providers.CurrentUserProvider
{
    [As(typeof(ICurrentUserProvider))]
    public class CurrentUserProvider : ICurrentUserProvider
    {
        private readonly IUserManager _userManager;
        private readonly IHttpContextAccessor _contextAccessor;

        public CurrentUserProvider(IUserManager userManager,
            IHttpContextAccessor contextAccessor)
        {
            _userManager = userManager;
            _contextAccessor = contextAccessor;
        }

        public async Task<User> GetUserAsync()
        {
            var userId = _userManager.GetUserId(_contextAccessor.HttpContext.User);

            return userId.HasValue
                ? await _userManager.FindByIdAsync(userId.Value)
                : null;
        }
    }
}