using EF.Models.Models;

namespace Application.Auth
{
    public interface IAuthenticationResultProvider
    {
        AuthenticateResult Get(User user);
    }
}