using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.TodoItems;

namespace Application.Components.TodoItems
{
    public interface ITodoItemsService
    {
        Task<IEnumerable<TodoItemDto>> GetListAsync(Guid userId);
        Task<TodoItemDto> AddAsync(TodoItemAddRequest request, Guid userId);
        Task<TodoItemDto> DoneAsync(TodoItemUpdateDoneRequest request);
        Task<TodoItemDto> UpdateAsync(TodoItemDto request);
        Task DeleteAsync(TodoItemDeleteRequest request);
    }
}