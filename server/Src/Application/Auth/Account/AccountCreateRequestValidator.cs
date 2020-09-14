using Application.Auth.Identity;
using Application.Components.Captcha;
using Autofac.Extras.RegistrationAttributes.RegistrationAttributes;
using FluentValidation;

namespace Application.Auth.Account
{
    [As(typeof(IValidator<AccountCreateRequest>))]
    public class AccountCreateRequestValidator : AbstractValidator<AccountCreateRequest>
    {
        public AccountCreateRequestValidator(
            ICaptchaValidationService captchaValidationService,
            IUserManager userManager)
        {
            RuleFor(request => request.Email)
                .MustAsync(async (email, cancellation) => await userManager.FindByEmailAsync(email) == null);

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
}