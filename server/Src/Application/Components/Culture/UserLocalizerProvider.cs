using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.RegistrationAttributes.RegistrationAttributes;
using EF.Models.Models;
using Microsoft.Extensions.Localization;

namespace Application.Components.Culture
{
    [As(typeof(IUserLocalizerProvider<>))]
    public class UserLocalizerProvider<T> : IUserLocalizerProvider<T>
    {
        private readonly IStringLocalizer<T> _localizer;
        private readonly ICultureInfoProvider _cultureInfoProvider;

        public UserLocalizerProvider(
            IStringLocalizer<T> localizer,
            ICultureInfoProvider cultureInfoProvider)
        {
            _localizer = localizer;
            _cultureInfoProvider = cultureInfoProvider;
        }

        public IDictionary<string, string> Get(User user)
        {
            return Get(user.LanguageCode);
        }

        public IDictionary<string, string> Get(string languageCode)
        {
            var culture = _cultureInfoProvider.Get(languageCode);
            using (new CultureSwitcher(culture))
            {
                return _localizer.GetAllStrings()
                    .ToDictionary(localizedString => localizedString.Name, localizedString => localizedString.Value);
            }
        }
    }
}