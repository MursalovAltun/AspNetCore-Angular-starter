using Application.Components.TodoItems;
using Autofac.Extras.RegistrationAttributes.RegistrationAttributes;
using FluentValidation;

namespace Application.Validators.TodoItems
{
    [As(typeof(IValidator<TodoItemAddRequest>))]
    public class TodoItemAddRequestValidator : AbstractValidator<TodoItemAddRequest>
    {
        public TodoItemAddRequestValidator(ITodoItemExistProvider todoItemExistProvider)
        {
            RuleFor(request => request.Description)
                .NotEmpty()
                .MustAsync(async (description, cancellation) =>
                    !await todoItemExistProvider.ExistAsync(description));
        }
    }
}