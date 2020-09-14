using Application.TodoItems;
using Autofac.Extras.RegistrationAttributes.RegistrationAttributes;
using EF.Models.Models;
using FluentValidation;

namespace Application.Components.TodoItems
{
    [As(typeof(IValidator<TodoItemUpdateDoneRequest>))]
    public class TodoItemUpdateDoneRequestValidator : AbstractValidator<TodoItemUpdateDoneRequest>
    {
        public TodoItemUpdateDoneRequestValidator(
            IUserItemPermissionValidator<TodoItem> userItemPermissionValidator)
        {
            RuleFor(request => request.TodoItemId)
                .MustAsync(async (todoItemId, cancellation) =>
                    await userItemPermissionValidator.HasAccess(todoItemId));
        }
    }
}