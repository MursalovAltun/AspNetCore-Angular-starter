using EF.Models.Models;
using MediatR;

namespace Application.Auth.Account.Commands.CreateAccount;

public class CreateAccountCommand : IRequest<User>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string CaptchaToken { get; set; } = string.Empty;
}