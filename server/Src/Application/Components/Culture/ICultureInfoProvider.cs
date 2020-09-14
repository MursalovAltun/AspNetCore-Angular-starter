using System.Globalization;

namespace Application.Components.Culture
{
    public interface ICultureInfoProvider
    {
        CultureInfo Get(string cultureName);
    }
}