using Application.Validators.UserItem;
using Autofac.Extras.RegistrationAttributes.RegistrationAttributes;
using EF.Models.Models;
using FluentValidation;

namespace Application.Components.TodoItems
{
    [As(typeof(IValidator<TodoItemDeleteRequest>))]
    public class TodoItemDeleteRequestValidator : AbstractValidator<TodoItemDeleteRequest>
    {
        public TodoItemDeleteRequestValidator(
            IUserItemPermissionValidator<TodoItem> validator,
            ITodoItemValidatorService service)
        {
            RuleFor(request => request.Id)
                .MustAsync(async (id, cancellation) =>
                await service.IsExists(id) && await validator.HasAccess(id));
        }
    }
}
