using System.Threading.Tasks;
using Application.Components.PushNotifications;
using Application.Providers.CurrentUserProvider;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PushSubscriptionsController : ControllerBase
    {
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly IPushSubscriptionsService _pushSubscriptionsService;
        private readonly IPushNotificationsService _pushNotificationsService;

        public PushSubscriptionsController(
            ICurrentUserProvider currentUserProvider,
            IPushSubscriptionsService pushSubscriptionsService,
            IPushNotificationsService pushNotificationsService)
        {
            _currentUserProvider = currentUserProvider;
            _pushSubscriptionsService = pushSubscriptionsService;
            _pushNotificationsService = pushNotificationsService;
        }

        [HttpGet]
        public string GetPublicKey()
        {
            return _pushNotificationsService.PublicKey;
        }

        [HttpPost]
        public async Task UpsertSubscription([FromBody] PushSubscriptionDto subscription)
        {
            var user = await _currentUserProvider.GetUserAsync();

            await _pushSubscriptionsService.UpsertAsync(subscription, user);
        }
    }
}