using Application.Auth.Account.Commands.CreateAccount;
using Application.Auth.Identity;
using Application.Components.Captcha;
using FluentValidation;

namespace Application.Auth.Account.Validators;

public class AccountCreateRequestValidator : AbstractValidator<CreateAccountCommand>
{
    public AccountCreateRequestValidator(
        ICaptchaValidationService captchaValidationService,
        IUserManager userManager)
    {
        RuleFor(request => request.Email)
            .MustAsync(async (email, cancellation) =>
            {
                return await userManager.FindByEmailAsync(email) == null;
            });

        RuleFor(request => request.Password)
            .Must(password => !string.IsNullOrEmpty(password));

        RuleFor(request => request.FirstName)
            .Must(password => !string.IsNullOrEmpty(password));

        RuleFor(request => request.LastName)
            .Must(password => !string.IsNullOrEmpty(password));

        RuleFor(request => request.CaptchaToken)
            .MustAsync(async (captchaToken, cancellationToken) => await captchaValidationService.IsValidAsync(captchaToken));
    }
}