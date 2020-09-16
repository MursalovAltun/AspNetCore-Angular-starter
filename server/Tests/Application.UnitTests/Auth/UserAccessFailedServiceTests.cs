using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Auth;
using Application.UnitTests.Fixtures;
using Autofac.Extras.Moq;
using Common.Time;
using EF.Models;
using EF.Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Xunit;

namespace Application.UnitTests.Auth
{
    public class UserAccessFailedServiceTests
    {
        private readonly TestFixture _fixture;
        private readonly AppDbContext _context;

        public UserAccessFailedServiceTests()
        {
            _fixture = new TestFixture();
            _context = _fixture.Context;
        }

        [Fact]
        public async Task Should_ReturnNull_When_OnlyOneAttemptInRange()
        {
            var now = new DateTime(2020, 1, 1, 15, 0, 0);

            var user = new User
            {
                UserAccessFailedAttempts = new List<UserAccessFailedAttempt>
                {
                    new UserAccessFailedAttempt {Date = now.AddMinutes(-40)},
                    new UserAccessFailedAttempt {Date = now.AddMinutes(-35)},
                    new UserAccessFailedAttempt {Date = now.AddMinutes(-25)},
                }
            };

            _context.Users.Add(user);

            using var mock = AutoMock.GetLoose(_fixture.BeforeBuild);

            mock.Mock<IOptions<SignInConfiguration>>()
                .SetupGet(configuration => configuration.Value)
                .Returns(new SignInConfiguration
                {
                    CaptchaLockoutMinutes = 30,
                    FailedAttemptsCount = 2,
                });

            mock.Mock<IDateTimeProvider>()
                .Setup(provider => provider.UtcNow)
                .Returns(now);

            await _context.SaveChangesAsync();

            var sut = mock.Create<UserAccessFailedService>();

            var actual = await sut.IsCaptchaRequired(user);

            Assert.Null(actual);
        }

        [Fact]
        public async Task Should_ReturnUntilDate_When_EnoughtAttemptInRange()
        {
            var now = new DateTime(2020, 1, 1, 15, 0, 0);

            var user = new User
            {
                UserAccessFailedAttempts = new List<UserAccessFailedAttempt>
                {
                    new UserAccessFailedAttempt {Date = now.AddMinutes(-20)},
                    new UserAccessFailedAttempt {Date = now.AddMinutes(-15)},
                    new UserAccessFailedAttempt {Date = now.AddMinutes(-5)},
                }
            };

            _context.Users.Add(user);

            using var mock = AutoMock.GetLoose(_fixture.BeforeBuild);

            mock.Mock<IOptions<SignInConfiguration>>()
                .SetupGet(configuration => configuration.Value)
                .Returns(new SignInConfiguration
                {
                    CaptchaLockoutMinutes = 30,
                    FailedAttemptsCount = 2,
                });

            mock.Mock<IDateTimeProvider>()
                .Setup(provider => provider.UtcNow)
                .Returns(now);

            await _context.SaveChangesAsync();

            var sut = mock.Create<UserAccessFailedService>();

            var actual = await sut.IsCaptchaRequired(user);

            Assert.Equal(actual, now.AddMinutes(15));
        }

        [Fact]
        public async Task Should_RegisterFailedAttempt_For_User()
        {
            var now = new DateTime(2020, 1, 1, 15, 0, 0);

            var user = new User();

            _context.Users.Add(user);

            using var mock = AutoMock.GetLoose(_fixture.BeforeBuild);

            mock.Mock<IOptions<SignInConfiguration>>()
                .SetupGet(configuration => configuration.Value)
                .Returns(new SignInConfiguration
                {
                    CaptchaLockoutMinutes = 30,
                    FailedAttemptsCount = 2,
                });

            mock.Mock<IDateTimeProvider>()
                .Setup(provider => provider.UtcNow)
                .Returns(now);

            await _context.SaveChangesAsync();

            var sut = mock.Create<UserAccessFailedService>();

            await sut.RegisterFailedAttempt(user);

            var added = await _context.UserAccessFailedAttempts.SingleAsync(uafa => uafa.UserId == user.Id);

            Assert.Equal(added.Date, now);
            Assert.Equal(added.User, user);
        }
    }
}