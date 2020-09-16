using System.Threading.Tasks;
using Application.Components.EmailSender;
using Newtonsoft.Json.Linq;
using PushSubscription = EF.Models.Models.PushSubscription;

namespace Application.Components.PushNotifications
{
    public class MailHogPushNotificationsClient : IPushNotificationsClient
    {
        private readonly IEmailService _emailService;

        public MailHogPushNotificationsClient(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task SendNotificationAsync(PushSubscription pushSubscription, string payload)
        {
            var jObject = JObject.Parse(payload);

            var title = jObject["notification"]["title"].Value<string>();
            var body = jObject["notification"]["body"].Value<string>();
            var email = $"push+{pushSubscription.User.Email}";

            await _emailService.SendAsync(email, title, body);
        }
    }
}