using Autofac.Extras.RegistrationAttributes.RegistrationAttributes;
using EF.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Application.Components.TodoItems
{
    [As(typeof(ITodoItemValidatorService))]
    public class TodoItemValidatorService : ITodoItemValidatorService
    {
        private readonly AppDbContext _context;

        public TodoItemValidatorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsExists(Guid id)
        {
            return await _context.TodoItems.AnyAsync(toDoItem => toDoItem.Id == id);
        }
    }
}
