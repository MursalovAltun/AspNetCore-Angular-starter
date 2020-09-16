using System.Threading.Tasks;

namespace Application.Components.TodoItems
{
    public interface ITodoItemExistProvider
    {
        Task<bool> ExistAsync(string description);
    }
}