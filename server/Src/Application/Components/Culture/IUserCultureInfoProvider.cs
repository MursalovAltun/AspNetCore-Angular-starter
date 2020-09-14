using System.Globalization;
using EF.Models.Models;

namespace Application.Components.Culture
{
    public interface IUserCultureInfoProvider
    {
        CultureInfo Get(User securityUser);
    }
}