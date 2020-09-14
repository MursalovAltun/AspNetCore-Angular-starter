using System;
using System.Threading.Tasks;
using Application.Providers.CurrentUserProvider;
using Autofac.Extras.RegistrationAttributes.RegistrationAttributes;
using EF.Models;
using EF.Models.Interfaces;

namespace Application.Validators.UserItem
{
    [As(typeof(IUserItemPermissionValidator<>))]
    public class UserItemPermissionValidator<T> : IUserItemPermissionValidator<T> where T : class, IUserItem
    {
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly AppDbContext _context;

        public UserItemPermissionValidator(ICurrentUserProvider currentUserProvider,
            AppDbContext context)
        {
            _currentUserProvider = currentUserProvider;
            _context = context;
        }

        public async Task<bool> HasAccess(Guid entityId)
        {
            var entity = await _context.FindAsync<T>(entityId);

            var user = await _currentUserProvider.GetUserAsync();

            return entity.UserId == user.Id;
        }
    }
}