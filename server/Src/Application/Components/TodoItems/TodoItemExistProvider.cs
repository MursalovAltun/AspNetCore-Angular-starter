using System.Threading.Tasks;
using Application.Providers.CurrentUserProvider;
using Autofac.Extras.RegistrationAttributes.RegistrationAttributes;
using EF.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Components.TodoItems
{
    [As(typeof(ITodoItemExistProvider))]
    public class TodoItemExistProvider : ITodoItemExistProvider
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUserProvider _currentUserProvider;

        public TodoItemExistProvider(AppDbContext context,
            ICurrentUserProvider currentUserProvider)
        {
            _context = context;
            _currentUserProvider = currentUserProvider;
        }

        public async Task<bool> ExistAsync(string description)
        {
            var user = await _currentUserProvider.GetUserAsync();

            return await _context.TodoItems
                .AnyAsync(item => item.UserId == user.Id && item.Description == description);
        }
    }
}