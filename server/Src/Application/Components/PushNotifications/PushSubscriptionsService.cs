using System.Threading.Tasks;
using Autofac.Extras.RegistrationAttributes.RegistrationAttributes;
using EF.Models;
using EF.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Components.PushNotifications
{
    [As(typeof(IPushSubscriptionsService))]
    public class PushSubscriptionsService : IPushSubscriptionsService
    {
        private readonly AppDbContext _context;

        public PushSubscriptionsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task UpsertAsync(PushSubscriptionDto pushSubscriptionDto, User user)
        {
            var pushSubscription = await _context.PushSubscriptions
                .SingleOrDefaultAsync(ps => ps.Endpoint == pushSubscriptionDto.Endpoint);

            if (pushSubscription == null)
            {
                pushSubscription = new PushSubscription
                {
                    Endpoint = pushSubscriptionDto.Endpoint,
                    Auth = pushSubscriptionDto.Auth,
                    P256DH = pushSubscriptionDto.P256DH,
                    User = user
                };

                _context.PushSubscriptions.Add(pushSubscription);
            }
            else
            {
                pushSubscription.User = user;

                _context.PushSubscriptions.Update(pushSubscription);
            }

            await _context.SaveChangesAsync();
        }
    }
}