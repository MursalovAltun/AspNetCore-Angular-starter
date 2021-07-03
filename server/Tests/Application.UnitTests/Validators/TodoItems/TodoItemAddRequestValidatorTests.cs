using Application.Components.TodoItems;
using Application.Validators.TodoItems;
using Autofac.Extras.Moq;
using FluentValidation.TestHelper;
using Moq;
using Xunit;

namespace Application.UnitTests.Validators.TodoItems
{
    public class TodoItemAddRequestValidatorTests
    {
        private readonly AutoMock _mock;
        private readonly TodoItemAddRequestValidator _validator;

        public TodoItemAddRequestValidatorTests()
        {
            _mock = AutoMock.GetLoose();

            _validator = _mock.Create<TodoItemAddRequestValidator>();
        }

        [Theory]
        [InlineData("")]
        [InlineData("           ")]
        [InlineData(null)]
        public void Returns_Invalid_When_Description_Is_Empty(string description)
        {
            var request = new TodoItemAddRequest
            {
                Description = description
            };

            SetValidationServices(request, FailedValidationService.None);

            _validator
                .ShouldHaveValidationErrorFor(r => r.Description, request);
        }

        [Fact]
        public void Returns_Invalid_When_Description_Is_Already_Exists()
        {
            var request = new TodoItemAddRequest
            {
                Description = "description"
            };

            SetValidationServices(request, FailedValidationService.TodoItemExists);

            _validator
                .ShouldHaveValidationErrorFor(r => r.Description, request);
        }

        private enum FailedValidationService
        {
            None,
            TodoItemExists
        }

        private void SetValidationServices(TodoItemAddRequest request, FailedValidationService failedService)
        {
            _mock.Mock<ITodoItemExistProvider>()
                .Setup(provider => provider.ExistAsync(request.Description))
                .ReturnsAsync(failedService == FailedValidationService.TodoItemExists);
        }
    }
}