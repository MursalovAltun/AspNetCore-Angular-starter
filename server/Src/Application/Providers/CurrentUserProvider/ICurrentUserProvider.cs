using EF.Models.Models;

namespace Application.Providers.CurrentUserProvider
{
    public interface ICurrentUserProvider
    {
        Task<User> GetUserAsync();
    }
}