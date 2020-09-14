using System;

namespace Application.Components.TodoItems
{
    public class TodoItemUpdateDoneRequest
    {
        public Guid TodoItemId { get; set; }
        public bool Done { get; set; }
    }
}