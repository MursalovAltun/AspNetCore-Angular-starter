using System;

namespace Application.TodoItems
{
    public class TodoItemDto
    {
        public Guid Id { get; set; }
        public bool Done { get; set; }
        public Guid UserId { get; set; }
        public string Description { get; set; }
        public DateTime LastModified { get; set; }
    }
}