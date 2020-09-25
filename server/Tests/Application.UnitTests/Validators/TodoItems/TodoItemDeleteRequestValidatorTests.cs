using System;
using Application.Components.TodoItems;
using Application.Validators.UserItem;
using Autofac.Extras.Moq;
using EF.Models.Models;
using FluentValidation.TestHelper;
using FluentValidation.Validators;
using Moq;
using UnitTests.Common.Asserts;
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
        public void HasAppropriateValidators()
        {
            _validator
                .HasPropertyRule(request => request.Id)
                .HasValidator<AsyncPredicateValidator>();
        }

        [Fact]
        public void Returns_Invalid_When_User_Has_No_Access()
        {
            var request = new TodoItemDeleteRequest
            {
                Id = Guid.NewGuid()
            };

            SetValidationServices(request, FailedValidationService.HasAccess);

            _validator
                .ShouldHaveValidationErrorFor(r => r.Id, request);
        }
        
        [Fact]
        public void Returns_Invalid_When_Item_No_Exists()
        {
            var request = new TodoItemDeleteRequest
            {
                Id = Guid.NewGuid()
            };

            SetValidationServices(request, FailedValidationService.IsExists);

            _validator
                .ShouldHaveValidationErrorFor(r => r.Id, request);
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