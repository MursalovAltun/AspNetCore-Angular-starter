using System.Threading.Tasks;
using Autofac.Extras.RegistrationAttributes.RegistrationAttributes;
using Microsoft.Extensions.Options;
using WebPush;
using PushSubscription = EF.Models.Models.PushSubscription;

namespace Application.Components.PushNotifications
{
    [As(typeof(IPushNotificationsClient))]
    public class PushNotificationsClient : IPushNotificationsClient
    {
        private readonly WebPushClient _webPushClient = new WebPushClient();

        public PushNotificationsClient(IOptions<PushNotificationsConfiguration> configuration,
            IOptions<CommonConfiguration> commonConfiguration)
        {
            _webPushClient.SetVapidDetails(commonConfiguration.Value.ClientBaseUrl,
                configuration.Value.PublicKey,
                configuration.Value.PrivateKey);
        }

        public async Task SendNotificationAsync(PushSubscription pushSubscription, string payload)
        {
            try
            {
                var webPushSubscription = new WebPush.PushSubscription
                {
                    Auth = pushSubscription.Auth,
                    Endpoint = pushSubscription.Endpoint,
                    P256DH = pushSubscription.P256DH,
                };

                await _webPushClient.SendNotificationAsync(webPushSubscription, payload);
            }
            catch (WebPushException)
            {
            }
        }
    }
}