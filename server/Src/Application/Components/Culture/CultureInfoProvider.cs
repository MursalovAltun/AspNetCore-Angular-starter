using System.Globalization;
using Autofac.Extras.RegistrationAttributes.RegistrationAttributes;

namespace Application.Components.Culture
{
    [As(typeof(ICultureInfoProvider))]
    public class CultureInfoProvider : ICultureInfoProvider
    {
        public CultureInfo Get(string cultureName)
        {
            return CultureInfo.CreateSpecificCulture(cultureName);
        }
    }
}