using System.Threading.Tasks;
using Autofac.Extras.RegistrationAttributes.RegistrationAttributes;
using EF.Models.Models;
using Microsoft.Extensions.Options;

namespace Application.Components.PushNotifications
{
    [As(typeof(IPushNotificationsService))]
    public class PushNotificationsService : IPushNotificationsService
    {
        private readonly INotificationOptionsToPayloadConverter _notificationOptionsToPayloadConverter;
        private readonly PushNotificationsConfiguration _configuration;
        private readonly IPushNotificationsClient _pushNotificationsClient;

        public PushNotificationsService(IOptions<PushNotificationsConfiguration> configuration,
            IPushNotificationsClient pushNotificationsClient,
            INotificationOptionsToPayloadConverter notificationOptionsToPayloadConverter)
        {
            _pushNotificationsClient = pushNotificationsClient;
            _notificationOptionsToPayloadConverter = notificationOptionsToPayloadConverter;
            _configuration = configuration.Value;
        }

        public string PublicKey => _configuration.PublicKey;

        public async Task SendPushToEmployee(User user, PushNotificationOptions notificationOptions)
        {
            foreach (var pushSubscription in user.PushSubscriptions)
            {
                var payload = _notificationOptionsToPayloadConverter.Convert(notificationOptions);

                await _pushNotificationsClient.SendNotificationAsync(pushSubscription, payload);
            }
        }
    }
}
