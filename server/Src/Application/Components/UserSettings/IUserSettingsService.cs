using System.Threading.Tasks;
using EF.Models.Models;

namespace Application.Components.UserSettings
{
    public interface IUserSettingsService
    {
        Task UpdateLanguageAsync(string languageCode, User user);
    }
}