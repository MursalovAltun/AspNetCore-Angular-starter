using System;
using System.Threading.Tasks;

namespace Application.Components.TodoItems
{
    public interface ITodoItemValidatorService
    {
        Task<bool> IsExists(Guid id);
    }
}
