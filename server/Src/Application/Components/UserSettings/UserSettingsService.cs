using System.Threading.Tasks;
using Autofac.Extras.RegistrationAttributes.RegistrationAttributes;
using EF.Models;
using EF.Models.Models;

namespace Application.Components.UserSettings
{
    [As(typeof(IUserSettingsService))]
    public class UserSettingsService : IUserSettingsService
    {
        private readonly AppDbContext _context;

        public UserSettingsService(
            AppDbContext context)
        {
            _context = context;
        }

        public async Task UpdateLanguageAsync(string languageCode, User user)
        {
            user.LanguageCode = languageCode;

            _context.Update(user);

            await _context.SaveChangesAsync();
        }
    }
}