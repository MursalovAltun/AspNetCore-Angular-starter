using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Auth.Identity;
using EF.Models.Models;
using MediatR;

namespace Application.Auth.Account.Commands.CreateAccount;

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, User>
{
    private readonly IUserManager _userManager;

    public CreateAccountCommandHandler(IUserManager userManager)
    {
        _userManager = userManager;
    }

    public async Task<User> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            UserName = request.Email,
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded) throw new Exception();

        return user;
    }
}