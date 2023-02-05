using System;
using System.Threading.Tasks;
using Application.Auth.Account;
using Application.Auth.Identity;
using Autofac.Extras.Moq;
using EF.Models.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using UnitTests.Common.Asserts;
using Xunit;

namespace Application.UnitTests.Auth.Account
{
    public class AccountServiceTests
    {
        [Fact]
        public async Task Should_ReturnUser_When_UserIsSuccessfullyCreated()
        {
            using var mock = AutoMock.GetLoose();

            var request = new AccountCreateRequest
            {
                Password = "password",
                Email = "email",
                FirstName = "first name",
                LastName = "last name"
            };

            mock.Mock<IUserManager>()
                .Setup(manager => manager.CreateAsync(It.IsAny<User>(), "password"))
                .ReturnsAsync(IdentityResult.Success);

            (await mock.Create<AccountService>().Create(request)).Email.Should().Be("email");

            mock.Mock<IUserManager>()
                .Verify(manager => manager.CreateAsync(It.Is<User>(user => user.Email == "email"), "password"));
        }

        [Fact]
        public void Should_ThrowException_When_UserIsNotCreated()
        {
            using var mock = AutoMock.GetLoose();

            var request = new AccountCreateRequest
            {
                Password = "password",
                Email = "email",
                FirstName = "first name",
                LastName = "last name"
            };

            mock.Mock<IUserManager>()
                .Setup(manager => manager.CreateAsync(It.IsAny<User>(), "password"))
                .ReturnsAsync(IdentityResult.Failed());

            var sut = mock.Create<AccountService>();

            var expected = new Exception();

            ExceptionAssert.ThrowsAsync(expected, () => sut.Create(request));
        }
    }
}