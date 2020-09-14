using System.Threading.Tasks;
using EF.Models.Models;

namespace Application.Components.PushNotifications
{
    public interface IPushNotificationsClient
    {
        Task SendNotificationAsync(PushSubscription pushSubscription, string payload);
    }
}