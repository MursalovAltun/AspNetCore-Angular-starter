using System.Threading.Tasks;
using EF.Models.Models;

namespace Application.Components.TodoItems
{
    public interface ITodoItemsPushNotificationService
    {
        Task SendAsync(TodoItem todoItem);
    }
}