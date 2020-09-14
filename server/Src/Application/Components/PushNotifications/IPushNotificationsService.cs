using System.Threading.Tasks;
using EF.Models.Models;

namespace Application.Components.PushNotifications
{
    public interface IPushNotificationsService
    {
        string PublicKey { get; }

        Task SendPushToEmployee(User user, PushNotificationOptions notificationOptions);
    }
}
