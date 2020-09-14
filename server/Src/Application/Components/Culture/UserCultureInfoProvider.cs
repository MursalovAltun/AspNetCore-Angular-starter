using System.Globalization;
using Autofac.Extras.RegistrationAttributes.RegistrationAttributes;
using EF.Models.Models;

namespace Application.Components.Culture
{
    [As(typeof(IUserCultureInfoProvider))]
    public class UserCultureInfoProvider : IUserCultureInfoProvider
    {
        private readonly ICultureInfoProvider _cultureInfoProvider;

        public UserCultureInfoProvider(ICultureInfoProvider cultureInfoProvider)
        {
            _cultureInfoProvider = cultureInfoProvider;
        }

        public CultureInfo Get(User user)
        {
            return _cultureInfoProvider.Get(user.LanguageCode);
        }
    }
}