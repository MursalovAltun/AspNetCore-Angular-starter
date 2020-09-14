using System;
using Autofac.Extras.RegistrationAttributes.RegistrationAttributes;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Application.Components.PushNotifications
{
    [As(typeof(INotificationOptionsToPayloadConverter))]
    public class NotificationOptionsToPayloadConverter : INotificationOptionsToPayloadConverter
    {
        private readonly CommonConfiguration _commonConfiguration;

        public NotificationOptionsToPayloadConverter(IOptions<CommonConfiguration> commonConfiguration)
        {
            _commonConfiguration = commonConfiguration.Value;
        }

        public string Convert(PushNotificationOptions notificationOptions)
        {
            var payload = new
            {
                Notification = new
                {
                    notificationOptions.Title,
                    Body = notificationOptions.Message,
                    Data = new
                    {
                        notificationOptions.Url
                    },
                    Icon = GetAbsoluteUri("/assets/icons/notification-icon-512x512.png"),
                    Badge = GetAbsoluteUri("/assets/icons/badge-72x72.png"),
                }
            };

            return JsonConvert.SerializeObject(payload, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }

        private string GetAbsoluteUri(string relativeUri)
        {
            return new Uri(new Uri(_commonConfiguration.ClientBaseUrl), relativeUri).AbsoluteUri;
        }
    }
}