using System.Collections.Generic;
using EF.Models.Models;

namespace Application.Components.Culture
{
    public interface IUserLocalizerProvider<T>
    {
        IDictionary<string, string> Get(User user);

        IDictionary<string, string> Get(string languageCode);
    }
}