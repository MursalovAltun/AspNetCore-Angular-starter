using System;
using Application.Components.TodoItems;
using Application.Validators.TodoItems;
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
    public class TodoItemUpdateDoneRequestValidatorTests
    {
        private readonly AutoMock _mock;
        private readonly TodoItemUpdateDoneRequestValidator _validator;

        public TodoItemUpdateDoneRequestValidatorTests()
        {
            _mock = AutoMock.GetLoose();

            _validator = _mock.Create<TodoItemUpdateDoneRequestValidator>();
        }

        [Fact]
        public void HasAppropriateValidators()
        {
            _validator
                .HasPropertyRule(request => request.TodoItemId)
                .HasValidator<AsyncPredicateValidator>();
        }

        [Fact]
        public void Returns_Invalid_When_User_Has_No_Access()
        {
            var request = new TodoItemUpdateDoneRequest
            {
                Done = true,
                TodoItemId = Guid.NewGuid()
            };
            
            SetValidationServices(request, FailedValidationService.HasAccess);

            _validator
                .ShouldHaveValidationErrorFor(r => r.TodoItemId, request);
        }

        private enum FailedValidationService
        {
            HasAccess
        }

        private void SetValidationServices(TodoItemUpdateDoneRequest request, FailedValidationService failedService)
        {
            _mock.Mock<IUserItemPermissionValidator<TodoItem>>()
                .Setup(validator => validator.HasAccess(request.TodoItemId))
                .ReturnsAsync(failedService != FailedValidationService.HasAccess);
        }
    }
}