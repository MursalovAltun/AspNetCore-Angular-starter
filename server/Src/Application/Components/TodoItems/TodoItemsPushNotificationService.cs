using System.Threading.Tasks;
using Application.Components.Culture;
using Application.Components.PushNotifications;
using Application.TodoItems;
using Autofac.Extras.RegistrationAttributes.RegistrationAttributes;
using EF.Models.Models;

namespace Application.Components.TodoItems
{
    [As(typeof(ITodoItemsPushNotificationService))]
    public class TodoItemsPushNotificationService : ITodoItemsPushNotificationService
    {
        private readonly IUserLocalizerProvider<TodoItemsPushNotificationService> _userLocalizerProvider;
        private readonly IPushNotificationsService _pushNotificationsService;

        public TodoItemsPushNotificationService(
            IUserLocalizerProvider<TodoItemsPushNotificationService> userLocalizerProvider,
            IPushNotificationsService pushNotificationsService)
        {
            _userLocalizerProvider = userLocalizerProvider;
            _pushNotificationsService = pushNotificationsService;
        }

        public async Task SendAsync(TodoItem todoItem)
        {
            var recipient = todoItem.User;
            var localizer = _userLocalizerProvider.Get(recipient);

            await _pushNotificationsService.SendPushToEmployee(recipient, new PushNotificationOptions
            {
                Title = localizer["TODO_ITEM_DONE_PUSH_TITLE"],
                Message = localizer["TODO_ITEM_DONE_PUSH_BODY"],
            });
        }
    }
}