using System.Threading.Tasks;
using EF.Models.Models;

namespace Application.Providers.CurrentUserProvider
{
    public interface ICurrentUserProvider
    {
        Task<User> GetUserAsync();
    }
}