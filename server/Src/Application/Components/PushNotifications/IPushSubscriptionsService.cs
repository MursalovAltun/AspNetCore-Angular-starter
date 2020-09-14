using System.Threading.Tasks;
using EF.Models.Models;

namespace Application.Components.PushNotifications
{
    public interface IPushSubscriptionsService
    {
        Task UpsertAsync(PushSubscriptionDto pushSubscriptionDto, User user);
    }
}