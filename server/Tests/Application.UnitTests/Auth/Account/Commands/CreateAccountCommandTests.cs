using System;
using Application.Auth.Account.Commands.CreateAccount;
using Application.Auth.Identity;
using EF.Models.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.UnitTests.Auth.Account.Commands;

public class CreateAccountCommandTests
{
    [Fact]
    public async Task Should_ReturnUser_When_UserIsSuccessfullyCreated()
    {
        using var mock = AutoMock.GetLoose();

        var request = new CreateAccountCommand
        {
            Password = "password",
            Email = "email",
            FirstName = "first name",
            LastName = "last name"
        };

        mock.Mock<IUserManager>()
            .Setup(manager => manager.CreateAsync(It.IsAny<User>(), "password"))
            .ReturnsAsync(IdentityResult.Success);

        (await mock.Create<CreateAccountCommandHandler>().Handle(request, CancellationToken.None)).Email.Should().Be("email");

        mock.Mock<IUserManager>()
            .Verify(manager => manager.CreateAsync(It.Is<User>(user => user.Email == "email"), "password"));
    }

    [Fact]
    public void Should_ThrowException_When_UserIsNotCreated()
    {
        using var mock = AutoMock.GetLoose();

        var request = new CreateAccountCommand
        {
            Password = "password",
            Email = "email",
            FirstName = "first name",
            LastName = "last name"
        };

        mock.Mock<IUserManager>()
            .Setup(manager => manager.CreateAsync(It.IsAny<User>(), "password"))
            .ReturnsAsync(IdentityResult.Failed());

        var sut = mock.Create<CreateAccountCommandHandler>();

        Func<Task> act = async () => await sut.Handle(request, CancellationToken.None);

        act.Should().ThrowExactlyAsync<Exception>();
    }
}