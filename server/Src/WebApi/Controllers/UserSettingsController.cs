using System.Threading.Tasks;
using Application.Components.UserSettings;
using Application.Providers.CurrentUserProvider;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    public class UserSettingsController : ControllerBase
    {
        private readonly IUserSettingsService _userSettingsService;
        private readonly ICurrentUserProvider _currentUserProvider;

        public UserSettingsController(
            IUserSettingsService userSettingsService,
            ICurrentUserProvider currentUserProvider)
        {
            _userSettingsService = userSettingsService;
            _currentUserProvider = currentUserProvider;
        }

        [HttpPost]
        public async Task UpdateLanguage([FromQuery] string languageCode)
        {
            var user = await _currentUserProvider.GetUserAsync();

            await _userSettingsService.UpdateLanguageAsync(languageCode, user);
        }
    }
}