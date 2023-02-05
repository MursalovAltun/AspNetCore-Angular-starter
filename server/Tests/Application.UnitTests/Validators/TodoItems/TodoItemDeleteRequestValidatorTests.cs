using System;
using System.Threading.Tasks;
using Application.Components.TodoItems;
using Application.Validators.UserItem;
using Autofac.Extras.Moq;
using EF.Models.Models;
using FluentValidation.TestHelper;
using Moq;
using Xunit;

namespace Application.UnitTests.Validators.TodoItems
{
    public class TodoItemDeleteRequestValidatorTests
    {
        private readonly AutoMock _mock;
        private readonly TodoItemDeleteRequestValidator _validator;

        public TodoItemDeleteRequestValidatorTests()
        {
            _mock = AutoMock.GetLoose();

            _validator = _mock.Create<TodoItemDeleteRequestValidator>();
        }

        [Fact]
        public async Task Returns_Invalid_When_User_Has_No_Access()
        {
            var request = new TodoItemDeleteRequest
            {
                Id = Guid.NewGuid()
            };

            SetValidationServices(request, FailedValidationService.HasAccess);

            (await _validator.TestValidateAsync(request))
                .ShouldHaveValidationErrorFor(r => r.Id);
        }

        [Fact]
        public async Task Returns_Invalid_When_Item_No_Exists()
        {
            var request = new TodoItemDeleteRequest
            {
                Id = Guid.NewGuid()
            };

            SetValidationServices(request, FailedValidationService.IsExists);

            (await _validator.TestValidateAsync(request))
                .ShouldHaveValidationErrorFor(r => r.Id);
        }

        private enum FailedValidationService
        {
            HasAccess,
            IsExists
        }

        private void SetValidationServices(TodoItemDeleteRequest request, FailedValidationService failedService)
        {
            _mock.Mock<IUserItemPermissionValidator<TodoItem>>()
                .Setup(validator => validator.HasAccess(request.Id))
                .ReturnsAsync(failedService != FailedValidationService.HasAccess);
            _mock.Mock<ITodoItemValidatorService>()
                .Setup(service => service.IsExists(request.Id))
                .ReturnsAsync(failedService != FailedValidationService.IsExists);
        }
    }
}