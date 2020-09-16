using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.RegistrationAttributes.RegistrationAttributes;
using Common.Time;
using EF.Models;
using EF.Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Application.Auth
{
    [As(typeof(IUserAccessFailedService))]
    public class UserAccessFailedService : IUserAccessFailedService
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly AppDbContext _context;
        private readonly SignInConfiguration _signInConfiguration;

        public UserAccessFailedService(
            IOptions<SignInConfiguration> signInOptions,
            IDateTimeProvider dateTimeProvider,
            AppDbContext context)
        {
            _dateTimeProvider = dateTimeProvider;
            _context = context;
            _signInConfiguration = signInOptions.Value;
        }


        public async Task RegisterFailedAttempt(User user)
        {
            _context.UserAccessFailedAttempts.Add(new UserAccessFailedAttempt
            {
                User = user,
                Date = _dateTimeProvider.UtcNow
            });

            await _context.SaveChangesAsync();
        }

        public async Task<DateTime?> IsCaptchaRequired(User user)
        {
            var sinceDate = _dateTimeProvider.UtcNow.AddMinutes(-_signInConfiguration.CaptchaLockoutMinutes ?? 0);
            var recentAttempts = await _context.UserAccessFailedAttempts
                .Where(uafa => uafa.UserId == user.Id && uafa.Date > sinceDate)
                .ToListAsync();

            if (!(recentAttempts.Count >= _signInConfiguration.FailedAttemptsCount)) return null;

            var attemptDate = recentAttempts
                .OrderByDescending(uafa => uafa.Date)
                .Take(_signInConfiguration.FailedAttemptsCount ?? 0)
                .Last().Date;

            return attemptDate.AddMinutes(_signInConfiguration.CaptchaLockoutMinutes ?? 0);
        }
    }
}