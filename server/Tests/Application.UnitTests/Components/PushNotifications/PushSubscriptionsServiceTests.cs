using System.Linq;
using System.Threading.Tasks;
using Application.Components.PushNotifications;
using Application.UnitTests.Fixtures;
using Autofac.Extras.Moq;
using EF.Models;
using EF.Models.Models;
using UnitTests.Common.Asserts;
using Xunit;

namespace Application.UnitTests.Components.PushNotifications
{
    public class PushSubscriptionsServiceTests
    {
        private readonly TestFixture _fixture;
        private readonly AppDbContext _context;

        public PushSubscriptionsServiceTests()
        {
            _fixture = new TestFixture();
            _context = _fixture.Context;
        }

        [Fact]
        public async Task Should_AddPushSubscription_When_PushSubscriptionWithSuchEndpointDoesNotExist()
        {
            using var mock = AutoMock.GetLoose(_fixture.BeforeBuild);

            var user = new User();

            var pushSubscriptionDto = new PushSubscriptionDto
            {
                Auth = "auth",
                Endpoint = "endpoint",
                P256DH = "p256dh",
            };

            _context.PushSubscriptions.Add(new PushSubscription
            {
                Endpoint = "eee"
            });

            await _context.SaveChangesAsync();

            var sut = mock.Create<PushSubscriptionsService>();

            await sut.UpsertAsync(pushSubscriptionDto, user);

            var added = _context.PushSubscriptions.Single(ps => ps.Endpoint == "endpoint");

            var expected = new PushSubscription
            {
                Auth = "auth",
                Endpoint = "endpoint",
                P256DH = "p256dh",
                User = user,
                UserId = user.Id,
            };

            ContentAssert.AreEqual(added, expected);
        }

        [Fact]
        public async Task Should_AddPushSubscription_When_PushSubscriptionWithSuchEndpointExists()
        {
            using var mock = AutoMock.GetLoose(_fixture.BeforeBuild);

            var user = new User();
            var user1 = new User();

            var pushSubscriptionDto = new PushSubscriptionDto
            {
                Auth = "auth",
                Endpoint = "endpoint",
                P256DH = "p256dh",
            };

            _context.PushSubscriptions.Add(new PushSubscription
            {
                Auth = "auth",
                Endpoint = "endpoint",
                P256DH = "p256dh",
                User = user1,
            });

            await _context.SaveChangesAsync();

            var sut = mock.Create<PushSubscriptionsService>();

            await sut.UpsertAsync(pushSubscriptionDto, user);

            var updated = _context.PushSubscriptions.Single(ps => ps.Endpoint == "endpoint");

            var expected = new PushSubscription
            {
                Auth = "auth",
                Endpoint = "endpoint",
                P256DH = "p256dh",
                User = user,
                UserId = user.Id,
            };

            ContentAssert.AreEqual(updated, expected);
        }
    }
}