using System.Threading.Tasks;
using EF.Models.Models;

namespace Application.Auth.Account
{
    public interface IAccountService
    {
        Task<User> Create(AccountCreateRequest request);
    }
}